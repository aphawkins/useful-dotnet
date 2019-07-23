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

#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
            repository.Create(new Atbash());
            repository.Create(new Caesar());
            repository.Create(new MonoAlphabetic());
            repository.Create(new Reflector());
            repository.Create(new ROT13());
#pragma warning restore CA2000 // Dispose objects before losing scope

            using (IDisposableCipherView view = new WinFormsView())
            {
                IController controller = new CipherController(repository, view);
                controller.LoadView();
            }
        }
    }
}