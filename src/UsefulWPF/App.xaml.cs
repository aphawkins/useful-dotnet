// <copyright file="App.xaml.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.WPF
{
    using System.Windows;
    using Security.Cryptography;
    using ViewModels;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CryptographyWindow app = new CryptographyWindow();
            IRepository<ICipher> repository = CipherRepository.Create();
            CipherViewModel context = new CipherViewModel(repository);
            app.DataContext = context;
            app.Show();
        }
    }
}