// <copyright file="CaesarSettingsView.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulConsole.UI.Views
{
    using System;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Controllers;
    using Useful.Security.Cryptography.UI.Views;

    internal class CaesarSettingsView : ICipherSettingsView
    {
        private SettingsController? _controller = null;

        public void Initialize()
        {
            bool isGood = false;
            int result = 0;

            while (!isGood)
            {
                Console.WriteLine("Select right shift (0 to 25):");

                string input = Console.ReadLine();
                isGood = int.TryParse(input, out result) && result >= 0 && result < 26;
            }

            ((CaesarSettings)_controller!.Settings).RightShift = result;

            Console.WriteLine($"Right shift selected: {result}");
        }

        public void SetController(IController controller)
        {
            _controller = (SettingsController)controller;
        }
    }
}