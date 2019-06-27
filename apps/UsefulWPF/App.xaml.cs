// <copyright file="App.xaml.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.WPF
{
    using System.Windows;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.Interfaces;
    using Useful.UI.Services;
    using Useful.UI.ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CryptographyWindow app = new CryptographyWindow();
            ICipherRepository repository = new CipherRepository();
            CipherService service = new CipherService(repository);
            CipherViewModel context = new CipherViewModel(service);
            app.DataContext = context;
            app.Show();
        }
    }
}