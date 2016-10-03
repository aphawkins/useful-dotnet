namespace Useful.UI.WinForms
{
    using System;
    using System.Windows.Forms;
    using Security.Cryptography;
    using Controllers;
    using Views;
    using Winforms;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new Form1());

            // args[0] = model

            ICipher model;

            if (args.Length < 1)
            {
                model = new ReverseCipher();
            }
            else if (args[0] == "pad")
            {
                model = new ROT13Cipher();
            }
            else if (args[0] == "reverse")
            {
                model = new ReverseCipher();
            }
            else
            {
                model = new ReverseCipher();
            }

            IView view = new WinFormsView();

            CipherController controller = new CipherController(model, view);

            controller.LoadView();
        }
    }
}