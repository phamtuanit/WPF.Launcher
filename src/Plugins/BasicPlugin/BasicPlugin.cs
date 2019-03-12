using BasicPlugin.ViewModels;
using Caliburn.Micro;
using MEF.Launcher.Contract;
using MEF.Launcher.Contract.IoC;
using MEF.Launcher.Platform.Plugin;
using MEF.Launcher.Platform.ViewModels;
using System.ComponentModel.Composition;

namespace BasicPlugin
{
    // Don't forget below attributes
    [Export(typeof(IPlugin))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class BasicPlugin : PluginBase<BasicPlugin>
    {
        // Plugin name.
        public override string PluignName { get; set; } = "Basic plugin";

        // Plugin version.
        public override string PluginVersion { get; set; } = "1.0.0.0";

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        public override void InitPlugin()
        {
            // Updating application title
            this.AppManager.SetAppTitle("My Application");

            // Show footer bar
            this.FooterBarManager.IsDisplayFooterBar = true;
            // Writing message at footer bar
            this.FooterBarManager.SetMessage($"Plugin {this.PluignName} is initializing");


            // LeftdownMenuViewModel is main view which hosts other views and supports left-menu as well
            // You can replace LeftdownMenuViewModel by your view by calling this.AppManager.ShowMainUI(<your view model>);
            var mainViewModel = SimpleIoC.Get<LeftdownMenuViewModel>();
            this.AppManager.ShowMainUI(mainViewModel);

            // Register menu using for LeftdownMenuViewModel only
            mainViewModel.RegisterMenu(new MEF.Launcher.Platform.Menu.MenuItemEx
            {
                Name = "Basic Menu",
                ClickAction = () =>
                {
                    // ExampleView will be displayed when user click onto this button <Basic Menu>
                    mainViewModel.ActivateItem(IoC.Get<ExampleViewModel>());
                }
            });


            // Writing message at status-bar
            this.FooterBarManager.SetMessage($"Plugin {this.PluignName} is initialized");
        }

        /// <summary>
        /// Uninits the plugin.
        /// </summary>
        public override void UninitPlugin()
        {
            // Do something here when application terminating
        }
    }
}
