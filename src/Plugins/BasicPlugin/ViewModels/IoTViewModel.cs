using BasicPlugin.IoT;
using BasicPlugin.Models.IoT;
using MEF.Launcher.Platform.Screen;
using PropertyChanged;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System;
using System.Reflection;
using System.IO;
using log4net;

namespace BasicPlugin.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    [ImplementPropertyChanged]
    public class IoTViewModel : ScreenBase
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IoTViewModel));

        /// <summary>
        /// Gets or sets a value indicating whether this instance is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is running; otherwise, <c>false</c>.
        /// </value>
        [AlsoNotifyFor("CanStart", "CanStop")]
        public bool IsRunning { get; set; }

        public int Port01 { get; set; } = 7777;

        public int Port02 { get; set; } = 8888;

        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public override string DisplayName { get; set; }

        public IoTDevice IoTDev01 { get; set; }

        public IoTServer server01;

        public IoTDevice IoTDev02 { get; set; } = new IoTDevice("IoT - 02");

        public IoTServer server02;

        public IoTViewModel()
        {
            this.PrepareServer();
        }

        private void PrepareServer()
        {
            this.IoTDev01 = new IoTDevice("IoT - 01");
            this.IoTDev02 = new IoTDevice("IoT - 02");

            this.IoTDev01.ChannelChangedEvent += this.OnChannelDataChanged;

            this.server01 = CreateServer(this.IoTDev01);
            this.server02 = CreateServer(this.IoTDev02);
        }

        private void OnChannelDataChanged(Channel oldCh, Channel newCh)
        {
            Logger.Info($"Channel '{newCh.Ch}' has been changed. {newCh}");
            this.FooterBarManager.SetMessage($"{newCh.Type} channel 'Ch_{newCh.Ch}' has been changed. [{oldCh.Val}] -> [{newCh.Val}]");
        }

        private static IoTServer CreateServer(IoTDevice device)
        {
            Logger.Debug($"Create server'{device.Id}'...");
            AppDomain appDomain = CreateAppDomain(device.Id);
            var assemblyPath = new Uri(typeof(IoTServer).Assembly.CodeBase).LocalPath;
            var serverInstance = (IoTServer) appDomain.CreateInstanceFromAndUnwrap(assemblyPath, typeof(IoTServer).FullName);
            serverInstance.Initialize(device.Id, device);
            return serverInstance;
            Logger.Debug($"Create server'{device.Id}'... DONE");
        }

        private static AppDomain CreateAppDomain(string name)
        {
            Logger.Debug($"Create app domain '{name}'...");
            AppDomain crrAppDomain = AppDomain.CurrentDomain;
            AppDomainSetup domainSetup = new AppDomainSetup();
            domainSetup.ApplicationBase = crrAppDomain.SetupInformation.ApplicationBase;
            domainSetup.ApplicationName = crrAppDomain.SetupInformation.ApplicationName;
            domainSetup.PrivateBinPath = crrAppDomain.SetupInformation.PrivateBinPath;
            domainSetup.ConfigurationFile = crrAppDomain.SetupInformation.ConfigurationFile;

            AppDomain domain = AppDomain.CreateDomain(name);
            domain.Load(typeof(IoTViewModel).Assembly.GetName());
            Logger.Debug($"Create app domain '{name}'...");
            return domain;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can start.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can start; otherwise, <c>false</c>.
        /// </value>
        public bool CanStart
        {
            get
            {
                return !this.IsRunning;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance can stop.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        /// </value>
        public bool CanStop
        {
            get
            {
                return this.IsRunning;
            }
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
                this.server01.Start(this.Port01);
                this.server02.Start(this.Port02);
                this.IsRunning = true;
                this.FooterBarManager.SetMessage("All servers are running");
                Logger.Info("Start server... DONE");
            }
            catch (System.Exception ex)
            {
                Logger.Error("Cannot start server", ex);
                this.FooterBarManager.SetMessage($"Cannot start server. {ex.Message}");
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
                Logger.Error("Cannot stop server", ex);
                this.FooterBarManager.SetMessage($"Cannot stop server. {ex.Message}");
            }
        }

        #endregion
    }
}
