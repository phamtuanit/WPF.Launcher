using MEF.Launcher.Contract;

namespace MEF.Launcher.Manager
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Launcher.Contract.IoC;
    using Launcher.Platform.Screen;
    using MaterialDesignThemes.Wpf;
    using ViewModels;
    using ILog = log4net.ILog;
    using LogManager = log4net.LogManager;

    [Export(typeof(IAppManager))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class AppManager : IAppManager
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(AppManager));

        /// <summary>
        /// The main view model
        /// </summary>
        private readonly MainViewModel mainViewModel = SimpleIoC.Get<MainViewModel>();

        #region Implementation of IAppManager

        /// <summary>
        /// Shows the main UI.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void ShowMainUI(IScreen viewModel)
        {
            if (viewModel == null)
            {
                Logger.Warn("[ShowMainUI] viewModel must not be NULL.");
                return;
            }

            Logger.Debug($"[ShowMainUI] Show [{nameof(viewModel)}]/{viewModel.DisplayName}...");
            this.mainViewModel.Activate(viewModel);
            Logger.Debug($"[ShowMainUI] Show [{nameof(viewModel)}]/{viewModel.DisplayName}... DONE");
        }

        /// <summary>
        /// Sets the application title.
        /// </summary>
        /// <param name="title">The title.</param>
        public void SetAppTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                Logger.Warn("[SetAppTitle] title must not be NULL.");
                return;
            }
            this.mainViewModel.AppTitle = title;
        }

        #endregion
    }
}
