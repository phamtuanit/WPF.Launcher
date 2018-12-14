namespace MEF.Launcher
{
    using System.Windows;
    using log4net.Config;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            XmlConfigurator.Configure();
        }
    }
}
