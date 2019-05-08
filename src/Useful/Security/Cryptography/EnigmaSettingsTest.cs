using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Useful.Security.Cryptography;

namespace TestProject1
{
    /// <summary>
    ///This is a test class for EnigmaSettingsTest and is intended
    ///to contain all EnigmaSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnigmaSettingsTest
    {
        #region Fields
        Enigma enigma = new Enigma();
        EnigmaSettings target;
        int settingsChangedCount;
        #endregion

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextInstance {get; set; }
        #endregion

        #region Methods
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        // Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{

        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void EnigmaSettings_ctor()
        {
            target = new EnigmaSettings(enigma.Key, enigma.IV);

            Assert.IsTrue(string.Compare(target.CipherName, "Enigma") == 0);
            Assert.IsTrue(target.Counter == 0);
            Assert.IsTrue(target.Model == EnigmaModel.Military);
            Assert.IsTrue(target.PlugboardSubstitutionCount != 0);
            Assert.IsTrue(target.ReflectorNumber == EnigmaReflectorNumber.ReflectorB);
            Assert.IsTrue(target.RotorPositionCount == 3);
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Fastest) != EnigmaRotorNumber.None);
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Second) != EnigmaRotorNumber.None);
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Third) != EnigmaRotorNumber.None);
            try
            {
                target.GetRotorOrder(EnigmaRotorPosition.Forth);
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
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), Encoding.Unicode.GetString(enigma.Key), false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetIV()), Encoding.Unicode.GetString(enigma.IV), false) == 0);
        }

        /// <summary>
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void EnigmaSettings_ctor_Key_Null()
        {
            try
            {
                target = new EnigmaSettings(null, enigma.IV);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_IV_Null()
        {
            try
            {
                target = new EnigmaSettings(enigma.Key, null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        /// <summary>
        ///A test for EnigmaSettings Constructor
        ///</summary>
        [TestMethod()]
        public void EnigmaSettings_SetKey_Null()
        {
            byte[] iv = Encoding.Unicode.GetBytes(@"".ToString());
            byte[] key = null;

            target = new EnigmaSettings(enigma.Key, enigma.IV);

            try
            {
                target.SetKey(null);
                Assert.Fail();
            }
            catch (ArgumentNullException)
            {
                // success
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetKey_Valid()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            settingsChangedCount = 0;

            target = new EnigmaSettings(enigma.Key, enigma.IV);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);
            this.settingsChangedCount = 0;

            target.SetKey(key);

            TestState(1, 2, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, tempKey, Encoding.Unicode.GetString(enigma.IV));
        }
            
        [TestMethod()]
        public void EnigmaSettings_ctor_Model_Missing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"|I II III|AA BB");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Model_Invalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"invalid|I II III|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                target = new EnigmaSettings(key, iv);
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
            byte[] key = Encoding.Unicode.GetBytes(@"Military||AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
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
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III IV|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
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
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I I I|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
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
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II Beta|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetPlugboard_Null()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
    
            target = new EnigmaSettings(key, iv);

            try
            {
                target.SetPlugboardNew(null);
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

        [TestMethod()]
        public void EnigmaSettings_SetPlugboard_Empty()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            this.settingsChangedCount = 0;

            // Should not exception
            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            target.SetPlugboardNew(new Collection<SubstitutionPair>());

            // Removes settings
            TestState(1, 0, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, @"Military|I II III|", tempIV);

            target.SetPlugboardNew(new Collection<SubstitutionPair>());

            // Shouldn't do anything (already empty)
            TestState(2, 0, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, @"Military|I II III|", tempIV);
        }

        [TestMethod()]
        public void EnigmaSettings_SetPlugboard_Padding()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III| AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");
    
            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Plugboard_Not_Pairs()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|A A A");
            byte[] iv = Encoding.Unicode.GetBytes(@"");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Plugboard_Duplicate_Pair_0()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AA");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_Plugboard_Invalid_Char()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AÅ");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetIV_Incorrect_Format()
        {
            byte[] iv = Encoding.Unicode.GetBytes("AA A");
            target = new EnigmaSettings(enigma.Key, enigma.IV);

            try
            {
                target.SetIV(iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_IV_Invalid_Chars()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|");
            byte[] iv = Encoding.Unicode.GetBytes("A A Å");

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // sucess
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetIV_Invalid_Case()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = "A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            settingsChangedCount = 0;

            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            try
            {
                target.SetIV(Encoding.Unicode.GetBytes(@"A a A"));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
        }

        [TestMethod()]
        public void EnigmaSettings_ctor_IV_Invalid_Case()
        {
            string tempKey = @"Military|I II III|";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = "A a A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            settingsChangedCount = 0;

            try
            {
                target = new EnigmaSettings(key, iv);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
        }

        [TestMethod()]
        public void EnigmaSettings_SetIV_Valid()
        {
            string tempKey = @"Military|I II III|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            settingsChangedCount = 0;

            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            tempIV = @"Q E V";
            target.SetIV(Encoding.Unicode.GetBytes(tempIV));

            TestState(1, 2, EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, tempKey, tempIV);
        }

        void target_SettingsChanged(object sender, EventArgs e)
        {
            this.settingsChangedCount++;
        }
            
        [TestMethod()]
        public void EnigmaSettings_SetRotorOrder()
        {
            target = new EnigmaSettings(enigma.Key, enigma.IV);
            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.None);
            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.None);
            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.None);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            Collection<EnigmaRotorNumber> availableRotors;
            Collection<EnigmaRotorNumber> allowedRotors;

            #region No rotors set
            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(availableRotors.Count == 6);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[5] == EnigmaRotorNumber.Five);

            allowedRotors = target.AllowedRotors(EnigmaRotorPosition.Fastest);
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
                target.AvailableRotors(EnigmaRotorPosition.Forth);
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
                target.AllowedRotors(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch
            {
                // Success
            }
            #endregion

            #region One rotor set
            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);

            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Second);
            Assert.IsTrue(availableRotors.Count == 5);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Five);

            allowedRotors = target.AllowedRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region Two rotors set
            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.Two);

            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            allowedRotors = target.AllowedRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion

            #region All rotors set
            this.settingsChangedCount = 0;
            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.One);
            Assert.IsTrue(this.settingsChangedCount == 1);
            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.Two);
            Assert.IsTrue(this.settingsChangedCount == 2);
            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.Three);
            Assert.IsTrue(this.settingsChangedCount == 3);

            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Second);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            availableRotors = target.AvailableRotors(EnigmaRotorPosition.Third);
            Assert.IsTrue(availableRotors.Count == 4);
            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);

            allowedRotors = target.AllowedRotors(EnigmaRotorPosition.Fastest);
            Assert.IsTrue(allowedRotors.Count == 6);
            Assert.IsTrue(allowedRotors[0] == EnigmaRotorNumber.None);
            Assert.IsTrue(allowedRotors[1] == EnigmaRotorNumber.One);
            Assert.IsTrue(allowedRotors[2] == EnigmaRotorNumber.Two);
            Assert.IsTrue(allowedRotors[3] == EnigmaRotorNumber.Three);
            Assert.IsTrue(allowedRotors[4] == EnigmaRotorNumber.Four);
            Assert.IsTrue(allowedRotors[5] == EnigmaRotorNumber.Five);
            #endregion
        }

        [TestMethod()]
        public void EnigmaSettings_SetRotorSetting()
        {
            target = new EnigmaSettings(enigma.Key, enigma.IV);
            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorNumber.None);
            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorNumber.None);
            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorNumber.None);

            #region No rotors set
            try
            {
                target.GetRotorSetting(EnigmaRotorPosition.Fastest);
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
                target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'A');
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

            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'A');
            Assert.IsTrue(this.settingsChangedCount == 1);
            target.SetRotorSetting(EnigmaRotorPosition.Second, 'A');
            Assert.IsTrue(this.settingsChangedCount == 2);
            target.SetRotorSetting(EnigmaRotorPosition.Third, 'A');
            Assert.IsTrue(this.settingsChangedCount == 3);
            #endregion

            #region GetCurrentRotorPosition
            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Fastest) == 'A');
            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Second) == 'A');
            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Third) == 'A');
            Assert.IsTrue(this.settingsChangedCount == 3);
            #endregion

            #region Set Invalid position
            try
            {
                target.SetRotorSetting(EnigmaRotorPosition.Forth, 'A');
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
                target.SetRotorSetting(EnigmaRotorPosition.Fastest, 'Å');
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
                target.GetRotorSetting(EnigmaRotorPosition.Forth);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }
            #endregion
        }


        [TestMethod()]
        public void EnigmaSettings_SetPlugboard_DuplicatePairs()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|I II III|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            #region Duplicate Pairs
            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            this.settingsChangedCount = 0;

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

            SubstitutionPair subs = new SubstitutionPair('C', 'C');

            target.SetPlugboardPair(subs);

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(this.settingsChangedCount == 1);
            #endregion
        }

        [TestMethod()]
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
            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            this.settingsChangedCount = 0;

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

            SubstitutionPair subs = new SubstitutionPair('C', 'Å');
            try
            {
                target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });
                Assert.Fail();
            }
            catch (ArgumentException)
            {
                // success
            }

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(this.settingsChangedCount == 0);
            #endregion

            #region Valid
            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            this.settingsChangedCount = 0;

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

            subs = new SubstitutionPair('C', 'D');
            target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });

            tempKey = @"Military|I II III|CD";

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
            #endregion

            #region Valid
            target = new EnigmaSettings(key, iv);
            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

            this.settingsChangedCount = 0;

            Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

            target.SetPlugboardPair(new SubstitutionPair('C', 'D'));

            tempKey = @"Military|I II III|AB CD";

            Assert.IsTrue(target.PlugboardSubstitutionCount == 4);
            Assert.IsTrue(this.settingsChangedCount == 1);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
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
            Assert.IsTrue(this.settingsChangedCount == settingsChangedCount);
            Assert.IsTrue(string.Compare(target.CipherName, "Enigma") == 0);
            Assert.IsTrue(target.Counter == 0);
            Assert.IsTrue(target.Model == EnigmaModel.Military);
            Assert.IsTrue(target.PlugboardSubstitutionCount == substitutionCount);
            Assert.IsTrue(target.ReflectorNumber == EnigmaReflectorNumber.ReflectorB);
            Assert.IsTrue(target.RotorPositionCount == 3);
            #endregion
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Fastest) == rotorPositionFastest);
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Second) == rotorPositionSecond);
            Assert.IsTrue(target.GetRotorOrder(EnigmaRotorPosition.Third) == rotorPositionThird);
            try
            {
                target.GetRotorOrder(EnigmaRotorPosition.Forth);
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
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), key, false) == 0);
            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetIV()), iv, false) == 0);
        }
        #endregion
    }
}
