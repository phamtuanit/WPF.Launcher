using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicPlugin.WeighingScale;
using Caliburn.Micro;
using log4net;
using MEF.Launcher.Platform.Screen;
using PropertyChanged;

namespace BasicPlugin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ImplementPropertyChanged]
    public class WeighingScaleSystemViewModel : ScreenBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(typeof(WeighingScaleSystemViewModel));

        public WeighingScaleDevice Scale01 { get; set; }

        public WeighingScaleDevice Scale02 { get; set; }

        [AlsoNotifyFor("CanStart", "CanStop")]
        public bool IsRunning { get; set; }
        public bool CanStart
        {
            get
            {
                return !this.IsRunning;
            }
        }

        public bool CanStop
        {
            get
            {
                return this.IsRunning;
            }
        }

        public int Port01 { get; set; } = 5555;

        public int Port02 { get; set; } = 6666;

        public string Ip { get; set; } = "127.0.0.1";

        public WeighingScaleDeviceService server01;

        public WeighingScaleDeviceService server02;

        public BindableCollection<string> LogItems { get; set; } = new BindableCollection<string>();

        public WeighingScaleSystemViewModel()
        {
            this.Scale01 = new WeighingScaleDevice("Scale - 01");
            this.Scale02 = new WeighingScaleDevice("Scale - 02");

            this.server01 = new WeighingScaleDeviceService(this.Scale01);
            this.server02 = new WeighingScaleDeviceService(this.Scale02);

            this.RegisterEvents();
        }

        private void RegisterEvents()
        {
            this.server01.OnError = this.server02.OnError = (err) =>
            {
                this.FooterBarManager.SetMessage(err);
                this.AddLog(err);
            };
            this.server01.OnLog = this.server02.OnLog = (err) =>
            {
                this.AddLog(err);
            };
            this.server01.OnConnected = (id, msg) =>
            {
                this.FooterBarManager.SetMessage($"{id} connected to client");
                this.AddLog(msg);
                this.Scale01.IsConnected = true;
            };
            this.server02.OnConnected = (id, msg) =>
            {
                this.FooterBarManager.SetMessage($"{id} connected to client");
                this.AddLog(msg);
                this.Scale02.IsConnected = true;
            };

            this.server01.OnDisconnected = (id, msg) =>
            {
                this.FooterBarManager.SetMessage($"{id} disconnected to client");
                this.AddLog(msg);
                this.Scale01.IsConnected = false;
            };

            this.server02.OnDisconnected = (id, msg) =>
            {
                this.FooterBarManager.SetMessage($"{id} disconnected to client");
                this.AddLog(msg);
                this.Scale02.IsConnected = false;
            };
        }

        private void AddLog(string err)
        {
            while (this.LogItems.Count > 150)
            {
                this.LogItems.RemoveAt(0);
            }

            this.LogItems.Add(err);
        }

        #region Cmds

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            try
            {
                Logger.Info("Start server...");
                this.server01.Start(this.Ip, this.Port01);
                this.server02.Start(this.Ip, this.Port02);
                this.IsRunning = true;
                this.FooterBarManager.SetMessage("All servers are running");
                Logger.Info("Start server... DONE");
            }
            catch (System.Exception ex)
            {
                Logger.Error("Cannot start server", ex);
                this.FooterBarManager.SetMessage($"Cannot start servers. {ex.Message}");
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            try
            {
                Logger.Info("Stop server...");
                this.server01.Stop();
                this.server02.Stop();
                this.IsRunning = false;
                this.FooterBarManager.SetMessage("All servers are stopped");
                Logger.Info("Stop server... DONE");
            }
            catch (System.Exception ex)
            {
                Logger.Error("Cannot stop servers", ex);
                this.FooterBarManager.SetMessage($"Cannot stop servers. {ex.Message}");
            }
        }

        #endregion
    }
}
