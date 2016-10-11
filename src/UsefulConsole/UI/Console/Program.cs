namespace Useful.UI.Console
{
    using Controllers;
    using Security.Cryptography;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Views;

    class Program
    {
        static void Main(string[] args)
        {
            // args[0] = model

            ICipher model;

            CipherRepository repo = new CipherRepository();
            IList<ICipher> ciphers = repo.GetCiphers();

            if (args.Length < 1)
            {
                model = ciphers[0];
            }
            else
            {
                model = ciphers.FirstOrDefault(x => string.Equals(args[0], x.CipherName, StringComparison.OrdinalIgnoreCase));

                if (model == null)
                {
                    model = ciphers[0];
                }
            }

            IView view = new ConsoleView();

            CipherController controller = new CipherController(model, view);

            controller.LoadView();
        }
    }
}