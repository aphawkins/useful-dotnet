namespace UsefulConsole.UI.Console
{
    using Useful;
    using Useful.Security.Cryptography;
    using Useful.UI.Controllers;
    using Useful.UI.Views;
    using Views;

    internal class Program
    {
        private static void Main()
        {
            IRepository<ICipher> repository = CipherRepository.Create();
            ICipherView view = new ConsoleView();
            IController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}