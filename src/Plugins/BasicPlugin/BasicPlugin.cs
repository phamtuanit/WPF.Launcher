using BasicPlugin.ViewModels;
using Caliburn.Micro;
using MEF.Launcher.Contract;
using MEF.Launcher.Contract.IoC;
using MEF.Launcher.Platform.Plugin;
using MEF.Launcher.Platform.ViewModels;
using System.ComponentModel.Composition;

namespace BasicPlugin
{
    [Export(typeof(IPlugin))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class BasicPlugin : PluginBase<BasicPlugin>
    {
        /// <summary>
        /// Gets or sets the name of the plugin.
        /// </summary>
        /// <value>
        /// The name of the plugin.
        /// </value>
        public override string PluignName { get; set; } = "Hardware Simulator";

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
        /// <value>
        /// The plugin version.
        /// </value>
        public override string PluginVersion { get; set; } = "1.0.0.0";

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void InitPlugin()
        {
            var mainViewModel = SimpleIoC.Get<LeftdownMenuViewModel>();
            this.AppManager.ShowMainUI(mainViewModel);
            this.AppManager.SetAppTitle(this.PluignName);

            // Register menu
            mainViewModel.RegisterMenu(new MEF.Launcher.Platform.Menu.MenuItemEx
            {
                Name = "IoT Device",
                ClickAction = () =>
                {
                    mainViewModel.ActivateItem(IoC.Get<IoTViewModel>());
                }
            });

            mainViewModel.RegisterMenu(new MEF.Launcher.Platform.Menu.MenuItemEx
            {
                Name = "Weighing Scale",
                ClickAction = () =>
                {
                    mainViewModel.ActivateItem(IoC.Get<WeighingScaleSystemViewModel>());
                }
            });

            this.FooterBarManager.SetMessage($"Plugin {this.PluignName} is initialized");
        }

        /// <summary>
        /// Uninits the plugin.
        /// </summary>
        public override void UninitPlugin()
        {
            IoC.Get<IoTViewModel>()?.Stop();
            IoC.Get<WeighingScaleSystemViewModel>()?.Stop();
        }
    }
}
