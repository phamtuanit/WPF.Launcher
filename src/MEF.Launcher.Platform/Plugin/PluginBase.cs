using MEF.Launcher.Contract;
using MEF.Launcher.Contract.IoC;
using log4net;

namespace MEF.Launcher.Platform.Plugin
{
    public abstract class PluginBase<T> : IPlugin
    {
        /// <summary>
        /// The logger
        /// </summary>
        protected readonly ILog Logger = LogManager.GetLogger(typeof(T));

        /// <summary>
        /// Gets or sets the name of the pluign.
        /// </summary>
        /// <value>
        /// The name of the pluign.
        /// </value>
        public virtual string PluignName { get; set; }

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
        /// <value>
        /// The plugin version.
        /// </value>
        public virtual string PluginVersion { get; set; }

        /// <summary>
        /// The footer bar manager
        /// </summary>
        protected readonly IFooterBarManager FooterBarManager = SimpleIoC.Get<IFooterBarManager>();

        /// <summary>
        /// The application manager
        /// </summary>
        protected readonly IAppManager AppManager = SimpleIoC.Get<IAppManager>();

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public abstract void InitPlugin();

        /// <summary>
        /// Uninits the plugin.
        /// </summary>
        public abstract void UninitPlugin();
    }
}
