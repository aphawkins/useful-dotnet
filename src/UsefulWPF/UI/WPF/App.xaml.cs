namespace Useful.UI.WPF
{
    using System.Windows;
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
            CipherViewModel context = new CipherViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}