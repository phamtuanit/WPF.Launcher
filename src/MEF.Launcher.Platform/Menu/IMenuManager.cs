namespace MEF.Launcher.Platform.Menu
{
    public interface ILeftSideMenuManager
    {
        /// <summary>
        /// Registers the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void RegisterMenu(MenuItemEx menu);

        /// <summary>
        /// Unregisters the menu.
        /// </summary>
        /// <param name="menu">The menu.</param>
        void UnregisterMenu(MenuItemEx menu);

        /// <summary>
        /// Unregisters the menu by name.
        /// </summary>
        /// <param name="menuName">Name of the menu.</param>
        void UnregisterMenu(string menuName);
    }
}
