// <copyright file="Program.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Console
{
    using Useful;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.UI.Views;
    using UsefulConsole.UI.Views;

    internal class Program
    {
        private static void Main()
        {
            IRepository<ICipher> repository = new CipherRepository();
            ICipherView view = new ConsoleView();
            IController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}