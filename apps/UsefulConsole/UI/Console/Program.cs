// <copyright file="Program.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Console
{
    using Useful;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;
    using UsefulConsole.UI.Views;

    internal class Program
    {
        private static void Main()
        {
            IRepository<ICipher> repository = new CipherRepository();

#pragma warning disable IDISP004 // Don't ignore return value of type IDisposable.
#pragma warning disable CA2000 // Dispose objects before losing scope
            repository.Create(new Atbash());
            repository.Create(new Caesar());
            repository.Create(new ROT13());
#pragma warning restore CA2000
#pragma warning restore IDISP004

            ICipherView view = new ConsoleView();
            IController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}