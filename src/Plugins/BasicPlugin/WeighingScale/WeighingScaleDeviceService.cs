using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;

namespace BasicPlugin.WeighingScale
{
    public class WeighingScaleDeviceService
    {
        // Thread signal.
        private ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// The logger
        /// </summary>
        private static ILog Logger = LogManager.GetLogger(typeof(WeighingScaleDeviceService));

        public Action<string> OnLog;

        public Action<string> OnError;

        public Action<string, string> OnConnected;

        public Action<string, string> OnDisconnected;

        private Socket server;

        private WeighingScaleDevice deviceInstance;

        private bool needStop = false;

        public WeighingScaleDeviceService(WeighingScaleDevice device)
        {
            this.deviceInstance = device;
            Logger = LogManager.GetLogger(this.deviceInstance.Id);
        }

        public void Start(string ip, int port)
        {
            this.needStop = false;
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".
            var ipAddress = IPAddress.Parse(ip);
            var localEndPoint = new IPEndPoint(ipAddress, port);
            Logger.Info($"Start TVCP server '{this.deviceInstance.Id}' at {ipAddress}/{port}...");

            // Create a TCP/IP socket.
            this.server = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                this.server.Bind(localEndPoint);
                this.server.Listen(100);

                Task.Run(() =>
                {
                    while (!this.needStop)
                    {
                        // Set the event to nonsignaled state.  
                        allDone.Reset();

                        // Start an asynchronous socket to listen for connections.
                        Logger.Info($"Server '{this.deviceInstance.Id}' is wsiting for client.");
                        this.server.BeginAccept(new AsyncCallback(this.AcceptCallback), this.server);

                        // Wait until a connection is made before continuing.  
                        allDone.WaitOne();
                    }
                });
            }
            catch (Exception e)
            {
                throw e;
            }
            Logger.Info($"Start TVCP server '{this.deviceInstance.Id}' at {ipAddress}/{port}... DONE");
        }

        public void Stop()
        {
            if (this.server == null)
            {
                return;
            }

            this.needStop = true;
            allDone.Set();
            try
            {
                this.server.Shutdown(SocketShutdown.Both);
                this.server.Close();
                this.server.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Warn("Got an error while closing socket.", ex);
                this.LogError(ex.Message);
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            Logger.Info($"Server '{this.deviceInstance.Id}' accepted a client '{handler.GetHashCode()}'.");

            this.OnConnected?.Invoke(this.deviceInstance.Id, $"[{this.deviceInstance.Id}] Connected to client '{handler.GetHashCode()}'");
            Task.Run(() => this.HandleClient(handler));
        }

        private void HandleClient(Socket handler)
        {
            var netStream = new NetworkStream(handler, true);
            Func<Socket, bool> checkConnection = (socket) =>
            {
                try
                {
                    return !(socket.Poll(1, SelectMode.SelectRead) && socket.Available == 0);
                }
                catch (SocketException) { return false; }
            };

            while (!this.needStop && checkConnection(handler))
            {
                if(netStream.CanRead)
                {
                    int totalReceivedBytes = 0;
                    IList<byte[]> recievedBytes = new List<byte[]>();

                    // Incoming message may be larger than the buffer size.
                    do
                    {
                        var readBuffer = new byte[4];
                        var numberOfBytesRead = 0;

                        try
                        {
                            numberOfBytesRead = netStream.Read(readBuffer, 0, readBuffer.Length);
                        }
                        catch (Exception ex)
                        {
                            Logger.Error("Got an error while reading data.", ex);
                            this.LogError($"Cannot read. {ex.Message}");
                            continue;
                        }

                        totalReceivedBytes += numberOfBytesRead;
                        var newBuffer = new byte[numberOfBytesRead];
                        Array.Copy(readBuffer, 0, newBuffer, 0, newBuffer.Length);
                        recievedBytes.Add(newBuffer);

                    }
                    while (netStream.DataAvailable);

                    if (totalReceivedBytes <= 0)
                    {
                        continue;
                    }

                    var totalReadBuffer = new byte[totalReceivedBytes];
                    int currIndex = 0;
                    foreach (var item in recievedBytes)
                    {
                        Array.Copy(item, 0, totalReadBuffer, currIndex, item.Length);
                        currIndex += item.Length;
                    }
                    var message = Encoding.ASCII.GetString(totalReadBuffer, 0, totalReadBuffer.Length);
                    Logger.Info($"Received a message from client [{handler.GetHashCode()}].{Environment.NewLine}{message}");
                    this.HandleMessage(netStream, message);
                }
                Thread.Sleep(1 * 1000);
            }

            try
            {
                netStream.Close();
                netStream.Dispose();
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                handler.Dispose();
            }
            catch
            {
                // Do nothing
            }
            this.OnDisconnected?.Invoke(this.deviceInstance.Id, $"[{this.deviceInstance.Id}] Disconnected to client '{handler.GetHashCode()}'");
        }

        private void HandleMessage(NetworkStream netStream, string message)
        {
            if (message.Equals(Environment.NewLine))
            {
                return;
            }

            this.LogMsg($"Rev: {message}");
            if (message.StartsWith("SI"))
            {
                var response = "S S ";
                var weightStr = string.Format("{0:N2}", this.deviceInstance.Weight.Value);
                response += weightStr.PadLeft(10);
                response += string.Format(" {0}", this.deviceInstance.Weight.Unit);
                this.respond(netStream, response);
            } else if (message.StartsWith("I4"))
            {
                this.respond(netStream, "I4 A B12345678");
            }
        }

        private void respond(NetworkStream netStream, string response)
        {
            response += Environment.NewLine;

            var responseBytes = Encoding.ASCII.GetBytes(response);
            Logger.Info($"Sending a message to client.{Environment.NewLine}{response}");

            try
            {
                netStream.Write(responseBytes, 0, responseBytes.Length);
                this.LogMsg($"Sent: {response}");
            }
            catch (Exception ex)
            {
                Logger.Error("Got an error while writing data.", ex);
                this.LogError($"Cannot write. {ex.Message}");
            }
        }

        private void LogError(string error)
        {
            this.OnError?.Invoke($"[{this.deviceInstance.Id}] {error}");

        }

        private void LogMsg(string msg)
        {
            this.OnLog?.Invoke($"[{this.deviceInstance.Id}] {msg}");
        }
    }
}
