namespace Useful.UI.WinForms
{
    using System;
    using System.Windows.Forms;
    using Security.Cryptography;
    using Controllers;
    using Views;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            IRepository<ICipher> repository = CipherRepository.Create();
            ICipherView view = new WinFormsView();
            CipherController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}