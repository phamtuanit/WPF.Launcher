using MEF.Launcher.Contract;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using log4net;
using MEF.Launcher.Contract.IoC;
using System.Linq;

namespace MEF.Launcher.Manager
{
    [Export(typeof(IPluginManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class PluginManager : IPluginManager
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(PluginManager));

        /// <summary>
        /// Gets or sets the footer bar manager.
        /// </summary>
        /// <value>
        /// The footer bar manager.
        /// </value>
        private IFooterBarManager footerBarManager => SimpleIoC.Get<IFooterBarManager>();

        /// <summary>
        /// Gets or sets plugin.
        /// </summary>
        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<IPlugin> Plugins { get; set; }

        /// <summary>
        /// Loads the plugin.
        /// </summary>
        public void LoadPlugin()
        {
            Logger.Info($"Load {this.Plugins.Count()} plugins...");
            foreach (var plugin in this.Plugins)
            {
                try
                {
                    plugin.InitPlugin();
                }
                catch (System.Exception ex)
                {
                    this.footerBarManager.SetMessage($"Cannot load plugin {plugin.PluignName}");
                    Logger.Error($"Failed to load plugin {plugin.PluignName}", ex);
                }
            }
            Logger.Info($"Load {this.Plugins.Count()} plugins... DONE");
        }

        /// <summary>
        /// Unloads the plugin.
        /// </summary>
        public void UnloadPlugin()
        {
            Logger.Info($"Unload all plugins...");
            foreach (var plugin in this.Plugins)
            {
                try
                {
                    plugin.UninitPlugin();
                }
                catch (System.Exception ex)
                {
                    this.footerBarManager.SetMessage($"Cannot unload plugin {plugin.PluignName}");
                    Logger.Error($"Failed to load plugin {plugin.PluignName}", ex);
                }
            }
            Logger.Info($"Unload all plugins... DONE");
        }
    }
}
