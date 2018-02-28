namespace UsefulConsole.UI.Views
{
    using System;
    using Useful.Security.Cryptography;
    using Useful.UI.Controllers;
    using Useful.UI.Views;

    internal class CaesarSettingsView : ICipherSettingsView
    {
        private CaesarSettingsController controller;

        public void Initialize()
        {
            bool isGood = false;
            int result = 0;

            while (!isGood)
            {
                Console.WriteLine("Select right shift (0 to 25):");

                string input = Console.ReadLine();
                isGood = (int.TryParse(input, out result) && result >= 0 && result < 26);

                Console.WriteLine();
            }

            ((CaesarCipherSettings)controller.Settings).RightShift = result;

            Console.WriteLine($"Right shift selected: {result}");
        }

        public void SetController(IController controller)
        {
            this.controller = (CaesarSettingsController)controller;
        }
    }
}