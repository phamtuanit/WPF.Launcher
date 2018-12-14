namespace MEF.Launcher.Contract
{
    public interface IPlugin
    {
        /// <summary>
        /// Gets or sets the name of the pluign.
        /// </summary>
        /// <value>
        /// The name of the pluign.
        /// </value>
        string PluignName { get; set; }

        /// <summary>
        /// Gets or sets the plugin version.
        /// </summary>
        /// <value>
        /// The plugin version.
        /// </value>
        string PluginVersion { get; set; }

        /// <summary>
        /// Initializes the plugin.
        /// </summary>
        void InitPlugin();

        /// <summary>
        /// Uninits the plugin.
        /// </summary>
        void UninitPlugin();
    }
}
