//  --------------------------------------------------------------------------------------------------------------------
//  <copyright file="AppManager.cs" company="SMEE">
//    Copyright (c) SMEE, 2016
//    All rights are reserved. Reproduction or transmission in whole or in part, in
//    any form or by any means, electronic, mechanical or otherwise, is prohibited
//    without the prior written consent of the copyright owner.
//  </copyright>
//  <summary>
//    Defines the AppManager.cs type.
//  </summary>
//  --------------------------------------------------------------------------------------------------------------------
namespace MEF.Launcher.Contract
{
    public interface IAppManager
    {
        /// <summary>
        /// Shows the main UI.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        void ShowMainUI(IScreen viewModel);

        /// <summary>
        /// Sets the application title.
        /// </summary>
        /// <param name="title">The title.</param>
        void SetAppTitle(string title);
    }
}