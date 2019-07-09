// <copyright file="App.xaml.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulWPF
{
    using System.Windows;
    using Useful;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Services;
    using Useful.Security.Cryptography.UI.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CryptographyWindow app = new CryptographyWindow();
            IRepository<ICipher> repository = new CipherRepository();
            CipherService service = new CipherService(repository);
            CipherViewModel context = new CipherViewModel(service);
            app.DataContext = context;
            app.Show();
        }
    }
}