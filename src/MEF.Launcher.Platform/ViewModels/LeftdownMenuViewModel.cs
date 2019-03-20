using Caliburn.Micro;
using MEF.Launcher.Platform.Menu;
using MEF.Launcher.Platform.Screen;
using PropertyChanged;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace MEF.Launcher.Platform.ViewModels
{
    [Export]
    [Export(typeof(ILeftSideMenuManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class LeftdownMenuViewModel : Conductor<ScreenBase>, Contract.IScreen, ILeftSideMenuManager, INotifyPropertyChanged
    {
        /// <summary>
        /// Implement INotifyPropertyChanged
        /// </summary>
        public override event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(LeftdownMenuViewModel));

        /// <summary>
        /// Gets or sets the menu items.
        /// </summary>
        /// <value>
        /// The menu items.
        /// </value>
        public BindableCollection<MenuItemEx> MenuItems { get; set; } = new BindableCollection<MenuItemEx>();

        /// <summary>
        /// Gets or sets the current menu item.
        /// </summary>
        /// <value>
        /// The current menu item.
        /// </value>
        public MenuItemEx CurrentMenuItem { get; set; }

        /// <summary>
        /// Registers the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public void RegisterMenu(MenuItemEx menu)
        {
            if (string.IsNullOrWhiteSpace(menu.Name))
            {
                throw new ArgumentException("name must not be NULL");
            }

            var existingMenu = this.MenuItems.FirstOrDefault(mn => mn.Name.Equals(menu.Name));
            if (existingMenu!= null)
            {
                throw new ArgumentException("Menu is already existing");
            }

            this.MenuItems.Add(menu);
        }

        /// <summary>
        /// Unregisters the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        public void UnregisterMenu(MenuItemEx menu)
        {
            this.MenuItems.Remove(menu);
        }

        /// <summary>
        /// Unregisters the menu by name.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        public void UnregisterMenu(string menuName)
        {
            var menu = this.MenuItems.FirstOrDefault(mn => mn.Name.Equals(menuName));
            if (menu != null)
            {
                this.MenuItems.Remove(menu);
            }
        }

        /// <summary>
        /// Selections the changed.
        /// </summary>
        public void SelectionChanged()
        {
            if (this.ActiveItem == null && this.CurrentMenuItem != null)
            {
                this.CurrentMenuItem.OnClick();
            }
        }
    }
}
