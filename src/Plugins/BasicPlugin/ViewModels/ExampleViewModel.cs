using MEF.Launcher.Platform.Screen;
using System.ComponentModel.Composition;

namespace BasicPlugin.ViewModels
{
    [Export]
    public class ExampleViewModel : ScreenBase
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
            this.DialogManager.ShowBusyIndicator();
        }

        #endregion
    }
}
