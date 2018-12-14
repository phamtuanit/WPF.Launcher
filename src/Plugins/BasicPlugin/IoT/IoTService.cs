using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;
using BasicPlugin.Models.IoT;

namespace BasicPlugin.IoT
{
    public class DI_ValueController : ApiController
    {
        [ActionName("slot_0")]
        public IEnumerable<Channel> GetAllChannels()
        {
            return IoT.IoTDevice.Instance.InputChannels;
        }

        [ActionName("ch_")]
        public Channel GetChannelById(int id = 0)
        {
            if (id >= IoT.IoTDevice.Instance.InputChannels.Count)
            {
                return null;
            }
            return IoT.IoTDevice.Instance.InputChannels[id];
        }

        [ActionName("ch_")]
        [HttpPut]
        public string UpdateChannelById(int id, [FromBody] Channel data)
        {
            if (data == null || data.Ch < 0 || data.Ch > IoT.IoTDevice.Instance.InputChannels.Count)
            {
                throw new Exception("Data is invalid. Maybe the Ch No. is out of range.");
            }
            if (data.Ch != id)
            {
                throw new Exception("The Ch No. in request body is not mactching with API.");
            }
            IoT.IoTDevice.Instance.InputChannels[id].Update(data);
            return "Updated";
        }
    }

    public class DO_ValueController : ApiController
    {
        [ActionName("slot_0")]
        public IEnumerable<Channel> GetAllChannels()
        {
            return IoT.IoTDevice.Instance.OutputChannels;
        }

        [ActionName("ch_")]
        public Channel GetChannelById(int id = 0)
        {
            if (id >= IoT.IoTDevice.Instance.OutputChannels.Count)
            {
                return null;
            }
            return IoT.IoTDevice.Instance.OutputChannels[id];
        }

        [ActionName("ch_")]
        [HttpPut]
        public string UpdateChannelById(int id, [FromBody] Channel data)
        {
            if (data == null || data.Ch < 0 || data.Ch > IoT.IoTDevice.Instance.OutputChannels.Count) {
                throw new Exception("Data is invalid. Maybe the Ch No. is out of range.");
            }
            if (data.Ch != id)
            {
                throw new Exception("The Ch No. in request body is not mactching with API.");
            }
            IoT.IoTDevice.Instance.OutputChannels[id].Update(data);
            return "Updated";
        }
    }

    public class IoTServer : MarshalByRefObject
    {
        /// <summary>
        /// The server
        /// </summary>
        private HttpSelfHostServer server;

        private HttpSelfHostConfiguration config;

        public void Initialize(string name, IoTDevice device)
        {
            IoTDevice.Instance = device;
        }

        public void Start(int port)
        {
            this.Stop();

            this.config = new HttpSelfHostConfiguration($"http://0.0.0.0:{port}");
            config.Routes.MapHttpRoute("API Default", "{controller}/", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("AllChannels", "{controller}/slot_0", new { action = "slot_0", id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("ChannelByNo", "{controller}/slot_0/ch_{id}", new { action = "ch_", id = RouteParameter.Optional });
            this.server = new HttpSelfHostServer(config);
            server.OpenAsync().Wait();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            if (this.server != null)
            {
                try
                {
                    this.server.CloseAsync().Wait();
                    this.config.Dispose();
                    this.server.Dispose();
                    this.server = null;
                }
                catch (Exception ex)
                {
                    // Do nothing
                }
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
