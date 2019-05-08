using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Useful.Security.Cryptography;

namespace UsefulQA
{
    /// <summary>
    ///This is a test class for EnigmaSettingsTest and is intended
    ///to contain all EnigmaSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnigmaRotorSettingsTest
    {
        #region Fields
        string propertiesChanged;
        #endregion

        public EnigmaRotorSettingsTest()
        {
        }

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextInstance { get; set; }
        #endregion

        #region Methods

        /// <summary>
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void EnigmaRotorSettings_ctor()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.Military);
        }

        [TestMethod()]
        public void EnigmaSettings_SetRotorOrder()
        {
            EnigmaRotorSettings target = EnigmaRotorSettings.Create(EnigmaModel.Military);
            List<EnigmaRotorNumber> availableRotors;

            #region No rotors set
            availableRotors = target.AvailableRotors.ToList();
            Assert.IsTrue(availableRotors.Count == 6);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region One rotor set
            target[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);

            availableRotors = target.AvailableRotors.ToList();
            Assert.IsTrue(availableRotors.Count == 5);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Five);
            #endregion

            #region Two rotors set
            this.propertiesChanged = string.Empty;
            target[EnigmaRotorPosition.Second] = EnigmaRotor.Create(EnigmaRotorNumber.Two);

            availableRotors = target.AvailableRotors.ToList();
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);
            #endregion

            #region All rotors set
            target[EnigmaRotorPosition.Third] = EnigmaRotor.Create(EnigmaRotorNumber.Three);

            availableRotors = target.AvailableRotors.ToList();
            Assert.IsTrue(availableRotors.Count == 3);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Five);
            #endregion
        }

        [TestMethod()]
        public void EnigmaSettings_SetRotorOrderPropertyChanged()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.Military);
            settings.PropertyChanged += target_PropertyChanged;
            this.propertiesChanged = "";

            settings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            Assert.IsTrue(this.propertiesChanged == "Item;AvailableRotors;");
        }

        [TestMethod()]
        public void EnigmaRotorSettings_SetRotor()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.Military);

            #region No rotors set
            Assert.IsTrue(settings[EnigmaRotorPosition.Fastest].RotorNumber == EnigmaRotorNumber.None);
            #endregion

            #region Set rotor
            settings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            Assert.IsTrue(settings[EnigmaRotorPosition.Fastest].RotorNumber == EnigmaRotorNumber.One);
            #endregion
        }

        [TestMethod()]
        public void EnigmaRotorSettings_GetAllowedRotorPositions()
        {
            Collection<EnigmaRotorPosition> positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.Military);
            Assert.IsTrue(positions.Count == 3);
            Assert.IsTrue(positions[0] == EnigmaRotorPosition.Fastest);
            Assert.IsTrue(positions[1] == EnigmaRotorPosition.Second);
            Assert.IsTrue(positions[2] == EnigmaRotorPosition.Third);

            positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.M3);
            Assert.IsTrue(positions.Count == 4);
            Assert.IsTrue(positions[0] == EnigmaRotorPosition.Fastest);
            Assert.IsTrue(positions[1] == EnigmaRotorPosition.Second);
            Assert.IsTrue(positions[2] == EnigmaRotorPosition.Third);
            Assert.IsTrue(positions[3] == EnigmaRotorPosition.Forth);

            positions = EnigmaRotorSettings.GetAllowedRotorPositions(EnigmaModel.M4);
            Assert.IsTrue(positions.Count == 4);
            Assert.IsTrue(positions[0] == EnigmaRotorPosition.Fastest);
            Assert.IsTrue(positions[1] == EnigmaRotorPosition.Second);
            Assert.IsTrue(positions[2] == EnigmaRotorPosition.Third);
            Assert.IsTrue(positions[3] == EnigmaRotorPosition.Forth);
        }

        [TestMethod()]
        public void EnigmaRotorSettings_GetAllowedRotorPositions1()
        {
            List<EnigmaRotorNumber> rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Fastest);
            Assert.IsTrue(rotors.Count == 6);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Second);
            Assert.IsTrue(rotors.Count == 6);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.Military, EnigmaRotorPosition.Third);
            Assert.IsTrue(rotors.Count == 6);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Fastest);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Second);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M3, EnigmaRotorPosition.Third);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Fastest);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Second);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Third);
            Assert.IsTrue(rotors.Count == 9);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(rotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(rotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(rotors[5] == EnigmaRotorNumber.Five);
            Assert.IsTrue(rotors[6] == EnigmaRotorNumber.Six);
            Assert.IsTrue(rotors[7] == EnigmaRotorNumber.Seven);
            Assert.IsTrue(rotors[8] == EnigmaRotorNumber.Eight);

            rotors = EnigmaRotorSettings.GetAllowedRotors(EnigmaModel.M4, EnigmaRotorPosition.Forth);
            Assert.IsTrue(rotors.Count == 3);
            Assert.IsTrue(rotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(rotors[1] == EnigmaRotorNumber.Beta);
            Assert.IsTrue(rotors[2] == EnigmaRotorNumber.Gamma);
        }

        [TestMethod()]
        public void EnigmaRotorSettings_GetOrderKeys()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.M4);
            settings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            settings[EnigmaRotorPosition.Second] = EnigmaRotor.Create(EnigmaRotorNumber.Three);
            settings[EnigmaRotorPosition.Third] = EnigmaRotor.Create(EnigmaRotorNumber.Five);
            settings[EnigmaRotorPosition.Forth] = EnigmaRotor.Create(EnigmaRotorNumber.Beta);
            Assert.AreEqual(settings.GetOrderKey(), "Beta V III I");
        }

        [TestMethod()]
        public void EnigmaRotorSettings_GetSettingKeys()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.M4);
            settings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            settings[EnigmaRotorPosition.Fastest].CurrentSetting = 'B';
            settings[EnigmaRotorPosition.Second] = EnigmaRotor.Create(EnigmaRotorNumber.Three);
            settings[EnigmaRotorPosition.Second].CurrentSetting = 'D';
            settings[EnigmaRotorPosition.Third] = EnigmaRotor.Create(EnigmaRotorNumber.Five);
            settings[EnigmaRotorPosition.Third].CurrentSetting = 'E';
            settings[EnigmaRotorPosition.Forth] = EnigmaRotor.Create(EnigmaRotorNumber.Beta);
            settings[EnigmaRotorPosition.Forth].CurrentSetting = 'G';
            Assert.AreEqual(settings.GetSettingKey(), "G E D B");
        }

        [TestMethod()]
        public void EnigmaRotorSettings_GetRingKeys()
        {
            EnigmaRotorSettings settings = EnigmaRotorSettings.Create(EnigmaModel.M4);
            settings[EnigmaRotorPosition.Fastest] = EnigmaRotor.Create(EnigmaRotorNumber.One);
            settings[EnigmaRotorPosition.Fastest].RingPosition = 'B';
            settings[EnigmaRotorPosition.Second] = EnigmaRotor.Create(EnigmaRotorNumber.Three);
            settings[EnigmaRotorPosition.Second].RingPosition = 'D';
            settings[EnigmaRotorPosition.Third] = EnigmaRotor.Create(EnigmaRotorNumber.Five);
            settings[EnigmaRotorPosition.Third].RingPosition = 'E';
            settings[EnigmaRotorPosition.Forth] = EnigmaRotor.Create(EnigmaRotorNumber.Beta);
            settings[EnigmaRotorPosition.Forth].RingPosition = 'G';
            Assert.AreEqual(settings.GetRingKey(), "G E D B");
        }

        private void target_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.propertiesChanged += e.PropertyName;
            this.propertiesChanged += ";";
        }
        #endregion
    }
}
