using MEF.Launcher.Contract;
using log4net;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MEF.Launcher.Control;

namespace MEF.Launcher.Manager
{
    [Export(typeof(IDialogManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class DialogManager : IDialogManager
    {
        #region Constant and Variables

        /// <summary>
        /// The root dialog name
        /// </summary>
        private const string RootDialogName = "RootDialog";

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DialogManager));

        /// <summary>
        /// The view model view caching
        /// </summary>
        private readonly IDictionary<IScreen, UIElement> viewModelViewCaching = new Dictionary<IScreen, UIElement>(); 

        #endregion

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        public Task<object> ShowDialog(IScreen viewModel, Action<IScreen> dialogOpenedCallBack, Action<IScreen> dialogClosingCallBack)
        {
            if (viewModel == null)
            {
                Logger.Warn("[ShowDialog] viewModel must not be NULL.");
                return null;
            }

            var view = Caliburn.Micro.ViewLocator.LocateForModel(viewModel, null, null);
            Caliburn.Micro.ViewModelBinder.Bind(viewModel, view, null);

            return DialogHost.Show(view, RootDialogName,
                (sender, args) =>
                {
                    // Caching view
                    this.viewModelViewCaching[viewModel] = view;

                    dialogOpenedCallBack?.Invoke(viewModel);
                },
                (sender, args) =>
                {
                    if (this.viewModelViewCaching.ContainsKey(viewModel))
                    {
                        // Remove caching
                        this.viewModelViewCaching.Remove(viewModel);
                    }

                    dialogClosingCallBack?.Invoke(viewModel);
                });
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        /// <returns></returns>
        public Task<object> ShowDialog(UserControl view, Action<UserControl> dialogOpenedCallBack, Action<UserControl> dialogClosingCallBack)
        {
            if (view == null)
            {
                Logger.Warn("[ShowDialog] view must not be NULL.");
                return null;
            }

            return DialogHost.Show(view, RootDialogName,
                (sender, args) =>
                {
                    dialogOpenedCallBack?.Invoke(view);
                },
                (sender, args) =>
                {
                    dialogClosingCallBack?.Invoke(view);
                });
        }

        /// <summary>
        /// Shows the busy indicator.
        /// </summary>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="">The .</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        /// <returns></returns>
        public Task ShowBusyIndicator(Action<object> dialogOpenedCallBack, Action<object> dialogClosingCallBack)
        {
            var view = new BusyIndicator();
            return DialogHost.Show(view, RootDialogName,
                (sender, args) =>
                {
                    dialogOpenedCallBack?.Invoke(view);
                },
                (sender, args) =>
                {
                    dialogClosingCallBack?.Invoke(view);
                });
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="view">The view.</param>
        public void CloseDialog(UserControl view)
        {
            DialogHost.CloseDialogCommand.Execute(true, view);
        }

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void CloseDialog(IScreen viewModel)
        {
            if (this.viewModelViewCaching.ContainsKey(viewModel))
            {
                var view = this.viewModelViewCaching[viewModel];
                DialogHost.CloseDialogCommand.Execute(true, view);
            }
            else
            {
                Logger.Warn($"[CloseDialog] The dialog [{nameof(viewModel)}] is not existed.");
            }
        }
    }
}
