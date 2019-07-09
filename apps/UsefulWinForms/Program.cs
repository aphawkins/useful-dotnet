// <copyright file="Program.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWinForms
{
    using System;
    using System.Windows.Forms;
    using Useful;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

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

            IRepository<ICipher> repository = new CipherRepository();
            using (IDisposableCipherView view = new CryptographyView())
            {
                IController controller = new CipherController(repository, view);
                controller.LoadView();
            }
        }
    }
}