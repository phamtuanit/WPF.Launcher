using PropertyChanged;
using System;

namespace MEF.Launcher.Platform.Menu
{
    [ImplementPropertyChanged]
    public class MenuItemEx
    {
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
