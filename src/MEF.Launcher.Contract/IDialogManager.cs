using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MEF.Launcher.Contract
{
    public interface IDialogManager
    {
        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        Task<object> ShowDialog(IScreen viewModel, Action<IScreen> dialogOpenedCallBack = null, Action<IScreen> dialogClosingCallBack = null);

        /// <summary>
        /// Shows the busy indicator.
        /// </summary>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="">The .</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        /// <returns></returns>
        Task ShowBusyIndicator(Action<object> dialogOpenedCallBack = null , Action<object> dialogClosingCallBack = null);

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="view">The view.</param>
        void CloseDialog(UserControl view);

        /// <summary>
        /// Closes the dialog.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void CloseDialog(IScreen viewModel);

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="view">The view.</param>
        /// <param name="dialogOpenedCallBack">The dialog opened call back.</param>
        /// <param name="dialogClosingCallBack">The dialog closing call back.</param>
        /// <returns></returns>
        Task<object> ShowDialog(UserControl view, Action<UserControl> dialogOpenedCallBack = null, Action<UserControl> dialogClosingCallBack = null);
    }
}
