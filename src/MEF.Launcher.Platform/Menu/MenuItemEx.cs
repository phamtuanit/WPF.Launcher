using System;
using System.ComponentModel;

namespace MEF.Launcher.Platform.Menu
{
    public class MenuItemEx : INotifyPropertyChanged
    {
        /// <summary>
        /// Implement INotifyPropertyChanged
        /// </summary>
        public virtual event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// The click action
        /// </summary>
        public Action ClickAction;

        /// <summary>
        /// Called when [click].
        /// </summary>
        public void OnClick()
        {
            this.ClickAction?.Invoke();
        }
    }
}
