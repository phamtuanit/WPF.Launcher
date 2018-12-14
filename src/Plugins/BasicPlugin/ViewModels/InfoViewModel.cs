using MEF.Launcher.Platform.Screen;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BasicPlugin.ViewModels
{
    [Export]
    public class InfoViewModel : ScreenBase
    {
        /// <summary>
        /// Gets or Sets the Display Name
        /// </summary>
        public override string DisplayName { get; set; }

        #region Command

        /// <summary>
        /// Shows the form command.
        /// </summary>
        public void ShowFormCmd()
        {
            this.DialogManager.ShowBusyIndicator((ctrl) =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    this.DialogManager.CloseDialog(ctrl as UserControl);
                });
            });
        }

        #endregion
    }
}
