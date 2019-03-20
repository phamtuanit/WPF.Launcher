using log4net;
using MEF.Launcher.Contract;
using MEF.Launcher.Contract.IoC;
using System.ComponentModel;

namespace MEF.Launcher.Platform.Screen
{
    public class ScreenBase : Caliburn.Micro.Screen, IScreen, INotifyPropertyChanged
    {
        /// <summary>
        /// Implement INotifyPropertyChanged
        /// </summary>
        public override event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILog Logger = LogManager.GetLogger(typeof(ScreenBase));

        /// <summary>
        /// The footer bar manager
        /// </summary>
        protected readonly IFooterBarManager FooterBarManager = SimpleIoC.Get<IFooterBarManager>();

        /// <summary>
        /// The dialog manager
        /// </summary>
        protected readonly IDialogManager DialogManager = SimpleIoC.Get<IDialogManager>();
    }
}
