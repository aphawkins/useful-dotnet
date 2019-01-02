// <copyright file="Program.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.WinForms
{
    using System;
    using System.Windows.Forms;
    using Controllers;
    using Security.Cryptography;
    using Views;

    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IRepository<ICipher> repository = CipherRepository.Create();
            using (IDisposableCipherView view = new CryptographyView())
            {
                IController controller = new CipherController(repository, view);
                controller.LoadView();
            }
        }
    }
}