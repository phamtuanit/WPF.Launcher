# WPF.Launcher
Allow you make new WPF application faster. This also supports Plug and Play.
To make new application, you just develop your pluign based on MVVM and MEF framework.

In this project, we used [Caliburn.Micro](http://materialdesigninxaml.net/) as MVVM framework.

We also used some 3rd libraries for User Interface.
* [Material Design In XAML](http://materialdesigninxaml.net/)
* [MPropertyChanged.Fody](https://github.com/Fody/PropertyChanged)
* [ControlzEx](https://github.com/ControlzEx/ControlzEx)
* [log4net](http://logging.apache.org/log4net/)
* [PropertyChanged.Fody and Fody](https://github.com/Fody/PropertyChanged)
* [CommonServiceLocator](https://github.com/unitycontainer/commonservicelocator)


# Making a plugin
```powershell
PM> Install-Package MEF.Launcher
```

After installing the nuget package, you need to create entry point class calling BasicPlugin like this.

```csharp
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

        // Initializes the plugin.
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
                    // YourViewModel will be displayed when user click onto this button <Basic Menu>
                    mainViewModel.ActivateItem(IoC.Get<YourViewModel>());
                }
            });


            // Writing message at status-bar
            this.FooterBarManager.SetMessage($"Plugin {this.PluignName} is initialized");
        }

        // Uninits the plugin.
        public override void UninitPlugin()
        {
            // Do something here when application terminating
        }
    }
}

```

![phamtuantech](/doc/images/example_ui.png)
![phamtuantech](/doc/images/example_ui_menu.png)

Finding me on [phamtuantech.com](https://phamtuantech.com/)
# License
[MIT](https://choosealicense.com/licenses/mit/)
