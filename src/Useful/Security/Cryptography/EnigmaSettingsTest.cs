// <copyright file="EnigmaSettingsTest.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace TestProject1
{
    using System;
    using System.Collections.ObjectModel;
    using System.Text;
    using Useful.Security.Cryptography;

    /// <summary>
    /// This is a test class for EnigmaSettingsTest and is intended
    /// to contain all EnigmaSettingsTest Unit Tests.
    /// </summary>
    [TestClass]
    public class EnigmaSettingsTest
    {
        Enigma _enigma = new Enigma();
        EnigmaSettings _target;
        int _settingsChangedCount;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContextInstance {get; set; }

        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext)
        // {
        // }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup()
        // {
        // }
        //
        // Use TestInitialize to run code before running each test
        // [TestInitialize()]
        // public void MyTestInitialize()
        // {

        // }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup()
        // {
        // }
        //

        /// <summary>
        /// A test for EnigmaSettings Constructor.
        /// </summary>
        [TestMethod]
        public void EnigmaSettings_ctor()
        {
            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);

            Assert.IsTrue(string.Compare(_target.CipherName, "Enigma") == 0);
            Assert.IsTrue(_target.Counter == 0);
            Assert.IsTrue(_target.Model == EnigmaModel.Military);
            Assert.IsTrue(_target.PlugboardSubstitutionCount != 0);
            Assert.IsTrue(_target.ReflectorNumber == EnigmaReflectorNumber.ReflectorB);
            Assert.IsTrue(_target.RotorPositionCount == 3);
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Fastest) != EnigmaRotorNumber.None);
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Second) != EnigmaRotorNumber.None);
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Third) != EnigmaRotorNumber.None);
            try
            {
                _target.GetRotorOrder(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            catch
            {
                Assert.Fail();
            }

            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetKey()), Encoding.Unicode.GetString(_enigma.Key), false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetIV()), Encoding.Unicode.GetString(_enigma.IV), false) == 0);
        }

        /// <summary>
        /// A test for EnigmaSettings Constructor.
        /// </summary>
        [TestMethod]
        public void EnigmaSettings_ctor_Key_Null()
        {
            try
            {
                _target = new EnigmaSettings(null, _enigma.IV);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_IV_Null()
        {
            try
            {
                _target = new EnigmaSettings(_enigma.Key, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        /// <summary>
        /// A test for EnigmaSettings Constructor.
        /// </summary>
        [TestMethod]
        public void EnigmaSettings_SetKey_Null()
        {
            byte[] iv = Encoding.Unicode.GetBytes(@"".ToString());
            byte[] key = null;

            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);

            try
            {
                _target.SetKey(null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetKey_Valid()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            _settingsChangedCount = 0;

            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            _settingsChangedCount = 0;

            _target.SetKey(key);

            TestState(1, 2, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, tempKey, Encoding.Unicode.GetString(_enigma.IV));
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Model_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"|I II III|AA BB");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Model_Invalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"invalid|I II III|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Rotors_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military||AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Rotors_Wrong_Number()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III IV|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Rotors_Duplicate()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I I I|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Rotors_Invalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II Beta|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetPlugboard_Null()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);

            _target = new EnigmaSettings(key, iv);

            try
            {
                _target.SetPlugboardNew(null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
            catch (Exception ex)
            {
                int i = 0;
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetPlugboard_Empty()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            _settingsChangedCount = 0;

            // Should not exception
            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _target.SetPlugboardNew(new Collection<SubstitutionPair>());

            // Removes settings
            TestState(1, 0, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, @"Military|I II III|", tempIV);

            _target.SetPlugboardNew(new Collection<SubstitutionPair>());

            // Shouldn't do anything (already empty)
            TestState(2, 0, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, @"Military|I II III|", tempIV);
        }

        [TestMethod]
        public void EnigmaSettings_SetPlugboard_Padding()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III| AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Plugboard_Not_Pairs()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|A A A");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Plugboard_Duplicate_Pair_0()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AA");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_Plugboard_Invalid_Char()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AÅ");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetIV_Incorrect_Format()
        {
            byte[] iv = Encoding.Unicode.GetBytes("AA A");
            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);

            try
            {
                _target.SetIV(iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_IV_Invalid_Chars()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|");
            byte[] iv = Encoding.Unicode.GetBytes("A A Å");

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetIV_Invalid_Case()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = "A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            _settingsChangedCount = 0;

            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            try
            {
                _target.SetIV(Encoding.Unicode.GetBytes(@"A a A"));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
        }

        [TestMethod]
        public void EnigmaSettings_ctor_IV_Invalid_Case()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = "A a A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            _settingsChangedCount = 0;

            try
            {
                _target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
        }

        [TestMethod]
        public void EnigmaSettings_SetIV_Valid()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            _settingsChangedCount = 0;

            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            tempIV = @"Q E V";
            _target.SetIV(Encoding.Unicode.GetBytes(tempIV));

            TestState(1, 2, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, tempKey, tempIV);
        }

        void target_SettingsChanged(object sender, EventArgs e)
        {
            _settingsChangedCount++;
        }

        [TestMethod]
        public void EnigmaSettings_SetRotorOrder()
        {
            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);
            _target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.None);
            _target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.None);
            _target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.None);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            Collection<EnigmaRotorNumber> availableRotors;
            Collection<EnigmaRotorNumber> allowedRotors;

            #region No rotors set
            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(availableRotors.Count == 6);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[5] == EnigmaRotorNumber.Five);

            allowedRotors = _target.AllowedRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region Invalid Available position
            try
            {
                _target.AvailableRotors(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch
            {
                // Success
            }
            #endregion

            #region Invalid Allowed position
            try
            {
                _target.AllowedRotors(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch
            {
                // Success
            }
            #endregion

            #region One rotor set
            _target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);

            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Second);
            Assert.IsTrue(availableRotors.Count == 5);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Five);

            allowedRotors = _target.AllowedRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region Two rotors set
            _target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.Two);

            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            allowedRotors = _target.AllowedRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region All rotors set
            _settingsChangedCount = 0;
            _target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);
            Assert.IsTrue(_settingsChangedCount == 1);
            _target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.Two);
            Assert.IsTrue(_settingsChangedCount == 2);
            _target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.Three);
            Assert.IsTrue(_settingsChangedCount == 3);

            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Second);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            availableRotors = _target.AvailableRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            allowedRotors = _target.AllowedRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion
        }

        [TestMethod]
        public void EnigmaSettings_SetRotorSetting()
        {
            _target = new EnigmaSettings(_enigma.Key, _enigma.IV);
            _target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.None);
            _target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.None);
            _target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.None);

            #region No rotors set
            try
            {
                _target.GetRotorSetting(EnigmaRotorPosition.Fastest);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion

            #region Rotors not yet set
            try
            {
                _target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'A');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion

            #region Rotors set
            string tempKey;
            byte[] key;

            string tempIV = null;
            byte[] iv = null;

            tempKey = @"Military|I II III|";
            key = Encoding.Unicode.GetBytes(tempKey);
            tempIV = @"A A A";
            iv = Encoding.Unicode.GetBytes(tempIV);

            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'A');
            Assert.IsTrue(_settingsChangedCount == 1);
            _target.SetRotorSetting(EnigmaRotorPosition.Second, 'A');
            Assert.IsTrue(_settingsChangedCount == 2);
            _target.SetRotorSetting(EnigmaRotorPosition.Third, 'A');
            Assert.IsTrue(_settingsChangedCount == 3);
            #endregion

            #region GetCurrentRotorPosition
            Assert.IsTrue(_target.GetRotorSetting(EnigmaRotorPosition.Fastest) == 'A');
            Assert.IsTrue(_target.GetRotorSetting(EnigmaRotorPosition.Second) == 'A');
            Assert.IsTrue(_target.GetRotorSetting(EnigmaRotorPosition.Third) == 'A');
            Assert.IsTrue(_settingsChangedCount == 3);
            #endregion

            #region Set Invalid position
            try
            {
                _target.SetRotorSetting(EnigmaRotorPosition.Forth, 'A');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion

            #region Set Invalid setting
            try
            {
                _target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'Å');
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion

            #region Get Invalid position
            try
            {
                _target.GetRotorSetting(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion
        }

        [TestMethod]
        public void EnigmaSettings_SetPlugboard_DuplicatePairs()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            #region Duplicate Pairs
            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _settingsChangedCount = 0;

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);

            SubstitutionPair subs = new SubstitutionPair('C', 'C');

            _target.SetPlugboardPair(subs);

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(_settingsChangedCount == 1);
            #endregion
        }

        [TestMethod]
        public void EnigmaSettings_SetPlugboard()
        {
            string tempKey;
            byte[] key;

            string tempIV = null;
            byte[] iv = null;

            tempKey = @"Military|I II III|AB";
            key = Encoding.Unicode.GetBytes(tempKey);
            tempIV = @"A A A";
            iv = Encoding.Unicode.GetBytes(tempIV);

            #region Invalid Char
            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _settingsChangedCount = 0;

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);

            SubstitutionPair subs = new SubstitutionPair('C', 'Å');
            try
            {
                _target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(_settingsChangedCount == 0);
            #endregion

            #region Valid
            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _settingsChangedCount = 0;

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);

            subs = new SubstitutionPair('C', 'D');
            _target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });

            tempKey = @"Military|I II III|CD";

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(_settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetKey()), tempKey, false) == 0);
            #endregion

            #region Valid
            _target = new EnigmaSettings(key, iv);
            _target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            _settingsChangedCount = 0;

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 2);

            _target.SetPlugboardPair(new SubstitutionPair('C', 'D'));

            tempKey = @"Military|I II III|AB CD";

            Assert.IsTrue(_target.PlugboardSubstitutionCount == 4);
            Assert.IsTrue(_settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetKey()), tempKey, false) == 0);
            #endregion

        }

        private void TestState(
            int settingsChangedCount,
            int substitutionCount,
            EnigmaRotorNumber rotorPositionFastest,
            EnigmaRotorNumber rotorPositionSecond,
            EnigmaRotorNumber rotorPositionThird,
            string key,
            string iv
            )
        {
            #region Properties
            Assert.IsTrue(_settingsChangedCount == settingsChangedCount);
            Assert.IsTrue(string.Compare(_target.CipherName, "Enigma") == 0);
            Assert.IsTrue(_target.Counter == 0);
            Assert.IsTrue(_target.Model == EnigmaModel.Military);
            Assert.IsTrue(_target.PlugboardSubstitutionCount == substitutionCount);
            Assert.IsTrue(_target.ReflectorNumber == EnigmaReflectorNumber.ReflectorB);
            Assert.IsTrue(_target.RotorPositionCount == 3);
            #endregion
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Fastest) == rotorPositionFastest);
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Second) == rotorPositionSecond);
            Assert.IsTrue(_target.GetRotorOrder(EnigmaRotorPosition.Third) == rotorPositionThird);
            try
            {
                _target.GetRotorOrder(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            catch
            {
                Assert.Fail();
            }

            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetKey()), key, false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(_target.GetIV()), iv, false) == 0);
        }
    }
}