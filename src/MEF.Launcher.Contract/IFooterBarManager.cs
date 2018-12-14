namespace MEF.Launcher.Contract
{
    public interface IFooterBarManager
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is display footer bar.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is display footer bar; otherwise, <c>false</c>.
        /// </value>
        bool IsDisplayFooterBar { get; set; }

        /// <summary>
        /// Sets the message.
        /// </summary>
        /// <param name="message">The message.</param>
        void SetMessage(string message);
    }
}
