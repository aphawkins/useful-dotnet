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
    public class EnigmaSettingsTest
    {
        #region Fields

        EnigmaSettings defaultSettings = EnigmaSettings.GetDefault();
        byte[] defaultKey;
        byte[] defaultIV;

        string propertiesChanged = string.Empty;
        #endregion

        public EnigmaSettingsTest()
        {
            defaultKey = defaultSettings.Key;
            defaultIV = defaultSettings.IV;
        }

        //        #region Properties
        //        /// <summary>
        //        ///Gets or sets the test context which provides
        //        ///information about and functionality for the current test run.
        //        ///</summary>
        //        public TestContext TestContextInstance {get; set; }
        //        #endregion

        //        #region Methods
        //        #region Additional test attributes
        //        // 
        //        //You can use the following additional attributes as you write your tests:
        //        //
        //        //Use ClassInitialize to run code before running the first test in the class
        //        //[ClassInitialize()]
        //        //public static void MyClassInitialize(TestContext testContext)
        //        //{
        //        //}
        //        //
        //        //Use ClassCleanup to run code after all tests in a class have run
        //        //[ClassCleanup()]
        //        //public static void MyClassCleanup()
        //        //{
        //        //}
        //        //
        //        // Use TestInitialize to run code before running each test
        //        //[TestInitialize()]
        //        //public void MyTestInitialize()
        //        //{

        //        //}
        //        //
        //        //Use TestCleanup to run code after each test has run
        //        //[TestCleanup()]
        //        //public void MyTestCleanup()
        //        //{
        //        //}
        //        //
        //        #endregion

        /// <summary>
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void EnigmaSettings_ctor()
        {
            EnigmaSettings target = EnigmaSettings.GetDefault();
            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, string.Empty, 0,
                EnigmaRotorNumber.Three, EnigmaRotorNumber.Two, EnigmaRotorNumber.One, null,
                'A', 'A', 'A', null,
                'A', 'A', 'A', null,
                Encoding.Unicode.GetString(defaultKey), Encoding.Unicode.GetString(defaultIV));
        }

        [TestMethod()]
        public void EnigmaSettings_SetKey_Valid()
        {
            string tempKey = @"Military|B|III II I|A A A|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            propertiesChanged = string.Empty;

            EnigmaSettings target = EnigmaSettings.GetDefault();
            target.PropertyChanged += target_PropertyChanged;
            this.propertiesChanged = string.Empty;

            target.Key = key;

            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, "Key;", 1,
                EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, null,
                'A', 'A', 'A', null,
                'A', 'A', 'A', null,
                tempKey, Encoding.Unicode.GetString(defaultIV));
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Model_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"|B|III II I|A A A|AA BB");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                EnigmaSettings target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Model_Military()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            EnigmaSettings target = new EnigmaSettings(key, iv);
            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, string.Empty, 1,
                EnigmaRotorNumber.Three, EnigmaRotorNumber.Two, EnigmaRotorNumber.One, null,
                'A', 'A', 'A', null,
                'A', 'A', 'A', null,
                Encoding.Unicode.GetString(key), Encoding.Unicode.GetString(iv));
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Model_M4()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"M4|BThin|Beta I II III|D C B A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"Z Y X W");

            EnigmaSettings target = new EnigmaSettings(key, iv);
            TestState(target, EnigmaModel.M4, EnigmaReflectorNumber.BThin, string.Empty, 1,
                EnigmaRotorNumber.Three, EnigmaRotorNumber.Two, EnigmaRotorNumber.One, EnigmaRotorNumber.Beta,
                'A', 'B', 'C', 'D',
                'W', 'X', 'Y', 'Z',
                Encoding.Unicode.GetString(key), Encoding.Unicode.GetString(iv));
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Model_Invalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"invalid|B|III II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Reflector_Invalid_1()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|BThin|I II III|A A A|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Reflector_Invalid_2()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"M4|B|Beta I II III|A A A A|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rotors_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B||A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rotors_Wrong_Number()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|IV III II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rotors_Duplicate()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I I I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rotors_Invalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|Beta II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rings_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III||AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rings_Wrong_Number()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Rings_Invalid_Char()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A Å|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                EnigmaSettings settings = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetPlugboard_Empty()
        {
            string tempKey = @"Military|B|III II I|A A A|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            this.propertiesChanged = string.Empty;

            // Should not exception
            EnigmaSettings target = new EnigmaSettings(key, iv);
            target.PropertyChanged += target_PropertyChanged;

            target.Plugboard.Clear();

            // Removes settings
            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, "", 0,
                EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, null,
                'A', 'A', 'A', null,
                'A', 'A', 'A', null,
                @"Military|B|III II I|A A A|", tempIV);

            this.propertiesChanged = string.Empty;
            target.Plugboard.Clear();

            // Shouldn't do anything (already empty)
            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, "", 0,
                EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, null,
                'A', 'A', 'A', null,
                'A', 'A', 'A', null,
                @"Military|B|III II I|A A A|", tempIV);
        }

        // TODO: Check Each part notifies the Key/IV has changed

        //        [TestMethod()]
        //        public void EnigmaSettings_SetPlugboard_Padding()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A| AB");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Not_Pairs()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|A A A");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"");

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Duplicate_Pair_0()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AA");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Invalid_Char()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AÅ");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetIV_Incorrect_Format()
        //        {
        //            byte[] iv = Encoding.Unicode.GetBytes("AA A");
        //            EnigmaSettings target = new EnigmaSettings();

        //            try
        //            {
        //                target.SetIV(iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_ctor_IV_Invalid_Chars()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|");
        //            byte[] iv = Encoding.Unicode.GetBytes("A A Å");

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetIV_Invalid_Case()
        //        {
        //            string tempKey = @"Military|B|III II I|A A A|";
        //            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        //            string tempIV = "A A A";
        //            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        //            settingsChangedCount = 0;

        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            try
        //            {
        //                target.SetIV(Encoding.Unicode.GetBytes(@"A a A"));
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_ctor_IV_Invalid_Case()
        //        {
        //            string tempKey = @"Military|B|III II I|A A A|";
        //            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        //            string tempIV = "A a A";
        //            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        //            settingsChangedCount = 0;

        //            try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetIV_Valid()
        //        {
        //            string tempKey = @"Military|B|III II I|A A A|AB";
        //            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        //            string tempIV = @"A A A";
        //            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        //            settingsChangedCount = 0;

        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            tempIV = @"Q E V";
        //            target.SetIV(Encoding.Unicode.GetBytes(tempIV));

        //            TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, 1, 2, 
        //                EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, null,
        //                'A', 'A', 'A', null,
        //                'V', 'E', 'Q', null,
        //                tempKey, tempIV);
        //        }

        private void target_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.propertiesChanged += e.PropertyName;
            this.propertiesChanged += ";";
        }

        //        [TestMethod()]
        //        public void EnigmaSettings_GetRotorOrder()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            try
        //            {
        //                char c = target.Rotors[EnigmaRotorPosition.Forth].CurrentSetting;
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            catch
        //            {
        //                Assert.Fail();
        //            }
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetRotorOrder()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.Rotors[EnigmaRotorPosition.Fastest] = EnigmaRotor.None;
        //            target.Rotors[EnigmaRotorPosition.Second] = EnigmaRotor.None;
        //            target.Rotors[EnigmaRotorPosition.Third] = EnigmaRotor.None;
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            List<EnigmaRotorNumber> availableRotors;
        //            this.settingsChangedCount = 0;

        //            #region No rotors set
        //            availableRotors = target.Rotors.AvailableRotors.ToList();
        //            Assert.IsTrue(availableRotors.Count == 6);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Two);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[5] == EnigmaRotorNumber.Five);
        //            #endregion

        //            #region One rotor set
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));

        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 5);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Five);
        //            #endregion

        //            #region Two rotors set
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, new EnigmaRotorSettings(EnigmaRotorNumber.Two));

        //            Assert.IsTrue(this.settingsChangedCount == 2);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 4);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);
        //            #endregion

        //            #region All rotors set
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, new EnigmaRotorSettings(EnigmaRotorNumber.Three));

        //            Assert.IsTrue(this.settingsChangedCount == 3);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 3);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Five);
        //            #endregion
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetRotorOrderToSelf()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorSettings.Empty);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));
        //            // Check the settings didn't change
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetRotorSetting()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorSettings.Empty);

        //            #region No rotors set
        //            try
        //            {
        //                target.GetRotorSetting(EnigmaRotorPosition.Fastest);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            #endregion

        //            #region Rotors not yet set
        //            try
        //            {
        //                target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        //                // target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'A');
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            #endregion

        //            #region Rotors set
        //            string tempKey;
        //            byte[] key;

        //            string tempIV = null;
        //            byte[] iv = null;

        //            tempKey = @"Military|B|III II I|A A A|";
        //            key = Encoding.Unicode.GetBytes(tempKey);
        //            tempIV = @"A A A";
        //            iv = Encoding.Unicode.GetBytes(tempIV);

        //            target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            target.Rotors[EnigmaRotorPosition.Second].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 2);
        //            target.Rotors[EnigmaRotorPosition.Third].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 3);
        //            #endregion

        //            #region GetCurrentRotorPosition
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Fastest) == 'A');
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Second) == 'A');
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Third) == 'A');
        //            Assert.IsTrue(this.settingsChangedCount == 3);
        //            #endregion

        //            #region Set Invalid position
        //            try
        //            {
        //                target.Rotors[EnigmaRotorPosition.Forth].CurrentSetting = 'A';
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            #endregion

        //            #region Set Invalid setting
        //            try
        //            {
        //                target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'Å';
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            #endregion

        //            #region Get Invalid position
        //            try
        //            {
        //                target.GetRotorSetting(EnigmaRotorPosition.Forth);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //            #endregion
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetRings()
        //        {
        //            string tempKey;
        //            byte[] key;

        //            string tempIV = null;
        //            byte[] iv = null;

        //            tempKey = @"Military|B|I II III|A A A|";
        //            key = Encoding.Unicode.GetBytes(tempKey);
        //            tempIV = @"A A A";
        //            iv = Encoding.Unicode.GetBytes(tempIV);


        //            #region Invalid Char
        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Fastest].RingPosition == 'A');
        //            #endregion



        //        }


        //        [TestMethod()]
        //        public void EnigmaSettings_SetPlugboard_DuplicatePairs()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AB");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        //            #region Duplicate Pairs
        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //            SubstitutionPair subs = new SubstitutionPair('C', 'C');

        //            target.SetPlugboardPair(subs);

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            #endregion
        //        }

        //        [TestMethod()]
        //        public void EnigmaSettings_SetPlugboard()
        //        {
        //            string tempKey;
        //            byte[] key;

        //            string tempIV = null;
        //            byte[] iv = null;

        //            tempKey = @"Military|B|III II I|A A A|AB";
        //            key = Encoding.Unicode.GetBytes(tempKey);
        //            tempIV = @"A A A";
        //            iv = Encoding.Unicode.GetBytes(tempIV);


        //            #region Invalid Char
        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //            SubstitutionPair subs = new SubstitutionPair('C', 'Å');
        //            try
        //            {
        //                target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        //            Assert.IsTrue(this.settingsChangedCount == 0);
        //            #endregion

        //            #region Valid
        //            target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //            subs = new SubstitutionPair('C', 'D');
        //            target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });

        //            tempKey = @"Military|B|III II I|A A A|CD";

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        //            #endregion

        //            #region Valid
        //            target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //            this.settingsChangedCount = 0;

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //            target.SetPlugboardPair(new SubstitutionPair('C', 'D'));

        //            tempKey = @"Military|B|III II I|A A A|AB CD";

        //            Assert.IsTrue(target.PlugboardSubstitutionCount == 4);
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        //            #endregion

        //        }

        private void TestState(
            EnigmaSettings target,
            EnigmaModel model,
            EnigmaReflectorNumber reflector,
            string propertiesChanged,
            int plugboardSubstitutionCount,
            EnigmaRotorNumber rotorPositionFastest,
            EnigmaRotorNumber rotorPositionSecond,
            EnigmaRotorNumber rotorPositionThird,
            EnigmaRotorNumber? rotorPositionForth,
            char ringSettingFastest,
            char ringSettingSecond,
            char ringSettingThird,
            char? ringSettingForth,
            char rotorSettingFastest,
            char rotorSettingSecond,
            char rotorSettingThird,
            char? rotorSettingForth,
            string key,
            string iv
            )
        {
            List<EnigmaRotorPosition> positions = new List<EnigmaRotorPosition>() { EnigmaRotorPosition.Fastest, EnigmaRotorPosition.Second, EnigmaRotorPosition.Third };

            Assert.IsTrue("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList().TrueForAll(x => target.AllowedLetters.Contains(x)));
            Assert.IsTrue(positions.TrueForAll(x => target.Rotors.AllowedRotorPositions.Contains(x)));
            Assert.IsTrue(string.Compare(target.CipherName, "Enigma") == 0);
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Fastest].RotorNumber == rotorPositionFastest);
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Second].RotorNumber == rotorPositionSecond);
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Third].RotorNumber == rotorPositionThird);
            if (rotorPositionForth != null)
            {
                Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Forth].RotorNumber == rotorPositionForth);
            }
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting == rotorSettingFastest);
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Second].CurrentSetting == rotorSettingSecond);
            Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Third].CurrentSetting == rotorSettingThird);
            if (rotorSettingForth != null)
            {
                Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Forth].CurrentSetting == rotorSettingForth);
            }
            Assert.IsTrue(target.Model == model);
            Assert.IsTrue(target.PlugboardSubstitutionCount == plugboardSubstitutionCount);
            Assert.IsTrue(target.ReflectorNumber == reflector);
            Assert.IsTrue(target.Rotors.Count == (rotorSettingForth == null ? 3 : 4));
            Assert.IsTrue(this.propertiesChanged == propertiesChanged);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.Key), key, false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.IV), iv, false) == 0);
        }
        //#endregion
    }
}
