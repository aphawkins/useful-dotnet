namespace Useful.UI.WinForms
{
    using Controllers;
    using Security.Cryptography;
    using System;
    using System.Windows.Forms;
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
            ICipherView view = new CryptographyView();
            IController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}