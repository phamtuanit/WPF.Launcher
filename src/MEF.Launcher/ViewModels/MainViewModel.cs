namespace MEF.Launcher.ViewModels
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using Contract;
    using MEF.Launcher.Contract.IoC;
    using MEF.Launcher.Manager;
    using log4net;
    using ILog = log4net.ILog;
    using LogManager = log4net.LogManager;
    using PropertyChanged;
    using Launcher.Platform.Screen;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    [Export]
    [ImplementPropertyChanged]
    public class MainViewModel : PropertyChangedBase
    {
        #region Constants

        /// <summary>
        /// The default application title
        /// </summary>
        private const string DefaultAppTitle = "MEF.Launcher";

        #endregion

        #region Properties

        #region Variables

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainViewModel));

        #endregion

        /// <summary>
        /// Gets or sets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        public string AppTitle { get; set; }

        /// <summary>
        /// Gets or sets the activate item.
        /// </summary>
        /// <value>
        /// The activate item.
        /// </value>
        public object ActivateItem { get; set; }

        /// <summary>
        /// Gets or sets the footer bar manager.
        /// </summary>
        /// <value>
        /// The footer bar manager.
        /// </value>
        public IFooterBarManager FooterBarManager { get; set; } = SimpleIoC.Get<IFooterBarManager>();

        /// <summary>
        /// The plugin manager
        /// </summary>
        private IPluginManager pluginManager = SimpleIoC.Get<IPluginManager>();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel()
        {
            this.AppTitle = DefaultAppTitle;
        }

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        /// <param name="view">The view</param>
        public void DoViewLoaded()
        {
            Logger.Info("---------------------------------------------------------------------");
            Logger.Info("------------------------ Main UI is loaded --------------------------");
            Task.Run(() => this.pluginManager.LoadPlugin());
        }

        public void Closing()
        {
            this.pluginManager.UnloadPlugin();
        }

        /// <summary>
        /// Activates the item.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void Activate(Launcher.Contract.IScreen viewModel)
        {
            this.OnUIThread(() =>
            {
                var view = ViewLocator.LocateForModel(viewModel, null, null);
                ViewModelBinder.Bind(viewModel, view, null);
                this.ActivateItem = view;
            });
        }
    }
}
