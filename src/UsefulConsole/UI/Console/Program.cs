namespace Useful.UI.Console
{
    using System;
    using Security.Cryptography;
    using Controllers;
    using Views;

    class Program
    {
        static void Main(string[] args)
        {
            // args[0] = model

            ICipher model;

            if (args.Length < 1)
            {
                model = new ReverseCipher();
            }
            else if (string.Equals(args[0], "ROT13", StringComparison.OrdinalIgnoreCase))
            {
                model = new ROT13Cipher();
            }
            else if (string.Equals(args[0], "REVERSE", StringComparison.OrdinalIgnoreCase))
            {
                model = new ReverseCipher();
            }
            else
            {
                model = new ReverseCipher();
            }

            IView view = new ConsoleView();

            CipherController controller = new CipherController(model, view);

            controller.LoadView();
        }
    }
}