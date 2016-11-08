namespace Useful.UI.Console
{
    using Controllers;
    using Security.Cryptography;
    using Views;

    class Program
    {
        static void Main()
        {
            IRepository<ICipher> repository = CipherRepository.Create();
            ICipherView view = new ConsoleView();
            CipherController controller = new CipherController(repository, view);
            controller.LoadView();
        }
    }
}