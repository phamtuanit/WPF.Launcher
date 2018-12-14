namespace MEF.Launcher.Manager
{
    public interface IPluginManager
    {
        /// <summary>
        /// Loads the plugin.
        /// </summary>
        void LoadPlugin();

        /// <summary>
        /// Unloads the plugin.
        /// </summary>
        void UnloadPlugin();
    }
}
