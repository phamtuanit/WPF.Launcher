namespace MEF.Launcher.ViewModels
{
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using Contract;
    using MEF.Launcher.Contract.IoC;
    using MEF.Launcher.Manager;
    using ILog = log4net.ILog;
    using LogManager = log4net.LogManager;
    using System.Threading.Tasks;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    [Export]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Constants

        /// <summary>
        /// The default application title
        /// </summary>
        private const string DefaultAppTitle = "MEF.Launcher";

        #endregion

        #region Variables

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(MainViewModel));

        #endregion

        #region Event

        /// <summary>
        /// Implement INotifyPropertyChanged
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

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
        /// Activates the item.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public virtual void Activate(Launcher.Contract.IScreen viewModel)
        {
            System.Action showingViewAct = () =>
            {
                var view = ViewLocator.LocateForModel(viewModel, null, null);
                ViewModelBinder.Bind(viewModel, view, null);
                this.ActivateItem = view;
            };

            showingViewAct.OnUIThread();
        }


        #region Window event

        /// <summary>
        /// Called when an attached view's Loaded event fires.
        /// </summary>
        public virtual void DoViewLoaded()
        {
            Logger.Info("---------------------------------------------------------------------");
            Logger.Info("------------------------ Main UI is loaded --------------------------");
            Task.Run(() => this.pluginManager.LoadPlugin());
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Closing()
        {
            this.pluginManager.UnloadPlugin();
        }

        #endregion
    }
}
