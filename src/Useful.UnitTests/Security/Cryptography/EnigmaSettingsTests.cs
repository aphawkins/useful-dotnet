// <copyright file="EnigmaSettingsTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaSettingsTests
    {
        private string _propertiesChanged = string.Empty;

        [Fact]
        public void Ctor()
        {
            EnigmaSettings target = new EnigmaSettings();
            TestState(
                target,
                EnigmaReflectorNumber.B,
                string.Empty,
                EnigmaRotorNumber.I,
                EnigmaRotorNumber.II,
                EnigmaRotorNumber.III,
                1,
                1,
                1,
                'A',
                'A',
                'A',
                "B|III II I|01 01 01|",
                "A A A");
        }

        [Fact]
        public void KeyIvDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal("B|III II I|01 01 01|", Encoding.Unicode.GetString(target.Key.ToArray()));
            Assert.Equal("A A A", Encoding.Unicode.GetString(target.IV.ToArray()));
        }

        [Fact]
        public void PlugboardDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ||False", Encoding.Unicode.GetString(target.Plugboard.Key.ToArray()));
        }

        [Fact]
        public void ReflectorDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(EnigmaReflectorNumber.B, target.ReflectorNumber);
        }

        [Fact]
        public void ReflectorCtor()
        {
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.C, new EnigmaRotorSettings(), new MonoAlphabeticSettings());
            Assert.Equal(EnigmaReflectorNumber.C, target.ReflectorNumber);
        }

        [Fact]
        public void ReflectorKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("C|III II I|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
            Assert.Equal(EnigmaReflectorNumber.C, target.ReflectorNumber);
        }

        [Fact]
        public void RotorsDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(new EnigmaRotorSettings().RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Fact]
        public void RotorsKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|IV III II|01 01 01|"), Encoding.Unicode.GetBytes("A A A"));
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
            Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Fact]
        public void RotorsCtor()
        {
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.IV);
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.III);
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II);
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new MonoAlphabeticSettings());
            Assert.Equal(rotorSettings.RotorOrderKey(), target.Rotors.RotorOrderKey());
        }

        [Fact]
        public void RingDefault()
        {
            EnigmaSettings target = new EnigmaSettings();
            Assert.Equal(new EnigmaRotorSettings().RingKey(), target.Rotors.RingKey());
        }

        [Fact]
        public void RingKeyCtor()
        {
            EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|04 03 02|"), Encoding.Unicode.GetBytes("A A A"));
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
            rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
            rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
            Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        }

        [Fact]
        public void RingCtor()
        {
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Third].RingPosition = 4;
            rotorSettings[EnigmaRotorPosition.Second].RingPosition = 3;
            rotorSettings[EnigmaRotorPosition.Fastest].RingPosition = 2;
            EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new MonoAlphabeticSettings());
            Assert.Equal(rotorSettings.RingKey(), target.Rotors.RingKey());
        }

        [Fact]
        public void CtorModelMissing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"|B|III II I|A A A|AA BB");
            byte[] iv = Encoding.Unicode.GetBytes(string.Empty);

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorModelMilitary()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            EnigmaSettings target = new EnigmaSettings(key, iv);
            TestState(
                target,
                EnigmaReflectorNumber.B,
                string.Empty,
                EnigmaRotorNumber.III,
                EnigmaRotorNumber.II,
                EnigmaRotorNumber.I,
                'A',
                'A',
                'A',
                'A',
                'A',
                'A',
                Encoding.Unicode.GetString(key),
                Encoding.Unicode.GetString(iv));
        }

        [Fact]
        public void CtorModelInvalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"invalid|B|III II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorReflectorInvalid1()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|BThin|I II III|A A A|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorReflectorInvalid2()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"M4|B|Beta I II III|A A A A|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRotorsMissing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B||A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRotorsWrongNumber()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|IV III II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRotorsDuplicate()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I I I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRotorsInvalid()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|Beta II I|A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRingsMissing()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III||AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRingsWrongNumber()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A A A|AB");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void CtorRingsInvalidChar()
        {
            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|I II III|A A Å|");
            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

            Assert.Throws<ArgumentException>(() => new EnigmaSettings(key, iv));
        }

        [Fact]
        public void SetPlugboardEmpty()
        {
            string tempKey = @"Military|B|III II I|A A A|AB";
            byte[] key = Encoding.Unicode.GetBytes(tempKey);
            string tempIV = @"A A A";
            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
            _propertiesChanged = string.Empty;

            // Should not exception
            EnigmaSettings target = new EnigmaSettings(key, iv);
            target.PropertyChanged += TargetPropertyChanged;

            // Removes settings
            TestState(
                target,
                EnigmaReflectorNumber.B,
                string.Empty,
                EnigmaRotorNumber.I,
                EnigmaRotorNumber.II,
                EnigmaRotorNumber.III,
                'A',
                'A',
                'A',
                'A',
                'A',
                'A',
                @"Military|B|III II I|A A A|",
                tempIV);

            _propertiesChanged = string.Empty;

            // Shouldn't do anything (already empty)
            TestState(
                target,
                EnigmaReflectorNumber.B,
                string.Empty,
                EnigmaRotorNumber.I,
                EnigmaRotorNumber.II,
                EnigmaRotorNumber.III,
                'A',
                'A',
                'A',
                'A',
                'A',
                'A',
                @"Military|B|III II I|A A A|",
                tempIV);
        }

        // TODO: Check Each part notifies the Key/IV has changed

        // [TestMethod()]
        //        public void EnigmaSettings_SetPlugboard_Padding()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A| AB");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Not_Pairs()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|A A A");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"");

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Duplicate_Pair_0()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AA");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_ctor_Plugboard_Invalid_Char()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AÅ");
        //            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_SetIV_Incorrect_Format()
        //        {
        //            byte[] iv = Encoding.Unicode.GetBytes("AA A");
        //            EnigmaSettings target = new EnigmaSettings();

        // try
        //            {
        //                target.SetIV(iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_ctor_IV_Invalid_Chars()
        //        {
        //            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|");
        //            byte[] iv = Encoding.Unicode.GetBytes("A A Å");

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // sucess
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_SetIV_Invalid_Case()
        //        {
        //            string tempKey = @"Military|B|III II I|A A A|";
        //            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        //            string tempIV = "A A A";
        //            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        //            settingsChangedCount = 0;

        // EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        // try
        //            {
        //                target.SetIV(Encoding.Unicode.GetBytes(@"A a A"));
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_ctor_IV_Invalid_Case()
        //        {
        //            string tempKey = @"Military|B|III II I|A A A|";
        //            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        //            string tempIV = "A a A";
        //            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        //            settingsChangedCount = 0;

        // try
        //            {
        //                EnigmaSettings target = new EnigmaSettings(key, iv);
        //                Assert.Fail();
        //            }
        //            catch (ArgumentException)
        //            {
        //                // success
        //            }
        //        }

        //// [TestMethod()]
        ////        public void EnigmaSettings_SetIV_Valid()
        ////        {
        ////            string tempKey = @"Military|B|III II I|A A A|AB";
        ////            byte[] key = Encoding.Unicode.GetBytes(tempKey);
        ////            string tempIV = @"A A A";
        ////            byte[] iv = Encoding.Unicode.GetBytes(tempIV);
        ////            settingsChangedCount = 0;

        //// EnigmaSettings target = new EnigmaSettings(key, iv);
        ////            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //// tempIV = @"Q E V";
        ////            target.SetIV(Encoding.Unicode.GetBytes(tempIV));

        //// TestState(target, EnigmaModel.Military, EnigmaReflectorNumber.B, 1, 2,
        ////                EnigmaRotorNumber.One, EnigmaRotorNumber.Two, EnigmaRotorNumber.Three, null,
        ////                'A', 'A', 'A', null,
        ////                'V', 'E', 'Q', null,
        ////                tempKey, tempIV);
        ////        }

        private void TargetPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            _propertiesChanged += e.PropertyName;
            _propertiesChanged += ";";
        }

        // [TestMethod()]
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

        // [TestMethod()]
        //        public void EnigmaSettings_SetRotorOrder()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.Rotors[EnigmaRotorPosition.Fastest] = EnigmaRotor.None;
        //            target.Rotors[EnigmaRotorPosition.Second] = EnigmaRotor.None;
        //            target.Rotors[EnigmaRotorPosition.Third] = EnigmaRotor.None;
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        // List<EnigmaRotorNumber> availableRotors;
        //            this.settingsChangedCount = 0;

        // #region No rotors set
        //            availableRotors = target.Rotors.AvailableRotors.ToList();
        //            Assert.IsTrue(availableRotors.Count == 6);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.One);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Two);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[5] == EnigmaRotorNumber.Five);
        //            #endregion

        // #region One rotor set
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));

        // Assert.IsTrue(this.settingsChangedCount == 1);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 5);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Two);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[4] == EnigmaRotorNumber.Five);
        //            #endregion

        // #region Two rotors set
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, new EnigmaRotorSettings(EnigmaRotorNumber.Two));

        // Assert.IsTrue(this.settingsChangedCount == 2);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 4);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Three);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[3] == EnigmaRotorNumber.Five);
        //            #endregion

        // #region All rotors set
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, new EnigmaRotorSettings(EnigmaRotorNumber.Three));

        // Assert.IsTrue(this.settingsChangedCount == 3);
        //            availableRotors = target.AvailableRotors().ToList();
        //            Assert.IsTrue(availableRotors.Count == 3);
        //            Assert.IsTrue(availableRotors[0] == EnigmaRotorNumber.None);
        //            Assert.IsTrue(availableRotors[1] == EnigmaRotorNumber.Four);
        //            Assert.IsTrue(availableRotors[2] == EnigmaRotorNumber.Five);
        //            #endregion
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_SetRotorOrderToSelf()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorSettings.Empty);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        // this.settingsChangedCount = 0;

        // target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, new EnigmaRotorSettings(EnigmaRotorNumber.One));
        //            // Check the settings didn't change
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //        }

        // [TestMethod()]
        //        public void EnigmaSettings_SetRotorSetting()
        //        {
        //            EnigmaSettings target = new EnigmaSettings();
        //            target.SetRotorOrder(EnigmaRotorPosition.Fastest, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Second, EnigmaRotorSettings.Empty);
        //            target.SetRotorOrder(EnigmaRotorPosition.Third, EnigmaRotorSettings.Empty);

        // #region No rotors set
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

        // #region Rotors not yet set
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

        // #region Rotors set
        //            string tempKey;
        //            byte[] key;

        // string tempIV = null;
        //            byte[] iv = null;

        // tempKey = @"Military|B|III II I|A A A|";
        //            key = Encoding.Unicode.GetBytes(tempKey);
        //            tempIV = @"A A A";
        //            iv = Encoding.Unicode.GetBytes(tempIV);

        // target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        // target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 1);
        //            target.Rotors[EnigmaRotorPosition.Second].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 2);
        //            target.Rotors[EnigmaRotorPosition.Third].CurrentSetting = 'A';
        //            Assert.IsTrue(this.settingsChangedCount == 3);
        //            #endregion

        // #region GetCurrentRotorPosition
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Fastest) == 'A');
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Second) == 'A');
        //            Assert.IsTrue(target.GetRotorSetting(EnigmaRotorPosition.Third) == 'A');
        //            Assert.IsTrue(this.settingsChangedCount == 3);
        //            #endregion

        // #region Set Invalid position
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

        // #region Set Invalid setting
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

        // #region Get Invalid position
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

        // [TestMethod()]
        //        public void EnigmaSettings_SetRings()
        //        {
        //            string tempKey;
        //            byte[] key;

        // string tempIV = null;
        //            byte[] iv = null;

        // tempKey = @"Military|B|I II III|A A A|";
        //            key = Encoding.Unicode.GetBytes(tempKey);
        //            tempIV = @"A A A";
        //            iv = Encoding.Unicode.GetBytes(tempIV);

        // #region Invalid Char
        //            EnigmaSettings target = new EnigmaSettings(key, iv);
        //            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        // this.settingsChangedCount = 0;

        // Assert.IsTrue(target.Rotors[EnigmaRotorPosition.Fastest].RingPosition == 'A');
        //            #endregion

        // }

        //// [TestMethod()]
        ////        public void EnigmaSettings_SetPlugboard_DuplicatePairs()
        ////        {
        ////            byte[] key = Encoding.Unicode.GetBytes(@"Military|B|III II I|A A A|AB");
        ////            byte[] iv = Encoding.Unicode.GetBytes(@"A A A");

        //// #region Duplicate Pairs
        ////            EnigmaSettings target = new EnigmaSettings(key, iv);
        ////            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //// this.settingsChangedCount = 0;

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //// SubstitutionPair subs = new SubstitutionPair('C', 'C');

        //// target.SetPlugboardPair(subs);

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        ////            Assert.IsTrue(this.settingsChangedCount == 1);
        ////            #endregion
        ////        }

        //// [TestMethod()]
        ////        public void EnigmaSettings_SetPlugboard()
        ////        {
        ////            string tempKey;
        ////            byte[] key;

        //// string tempIV = null;
        ////            byte[] iv = null;

        //// tempKey = @"Military|B|III II I|A A A|AB";
        ////            key = Encoding.Unicode.GetBytes(tempKey);
        ////            tempIV = @"A A A";
        ////            iv = Encoding.Unicode.GetBytes(tempIV);

        //// #region Invalid Char
        ////            EnigmaSettings target = new EnigmaSettings(key, iv);
        ////            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //// this.settingsChangedCount = 0;

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //// SubstitutionPair subs = new SubstitutionPair('C', 'Å');
        ////            try
        ////            {
        ////                target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });
        ////                Assert.Fail();
        ////            }
        ////            catch (ArgumentException)
        ////            {
        ////                // success
        ////            }

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        ////            Assert.IsTrue(this.settingsChangedCount == 0);
        ////            #endregion

        //// #region Valid
        ////            target = new EnigmaSettings(key, iv);
        ////            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //// this.settingsChangedCount = 0;

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //// subs = new SubstitutionPair('C', 'D');
        ////            target.SetPlugboardNew(new Collection<SubstitutionPair>() { subs });

        //// tempKey = @"Military|B|III II I|A A A|CD";

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);
        ////            Assert.IsTrue(this.settingsChangedCount == 1);
        ////            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        ////            #endregion

        //// #region Valid
        ////            target = new EnigmaSettings(key, iv);
        ////            target.SettingsChanged += new EventHandler<EventArgs>(target_SettingsChanged);

        //// this.settingsChangedCount = 0;

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 2);

        //// target.SetPlugboardPair(new SubstitutionPair('C', 'D'));

        //// tempKey = @"Military|B|III II I|A A A|AB CD";

        //// Assert.IsTrue(target.PlugboardSubstitutionCount == 4);
        ////            Assert.IsTrue(this.settingsChangedCount == 1);
        ////            Assert.IsTrue(string.Compare(Encoding.Unicode.GetString(target.GetKey()), tempKey, false) == 0);
        ////            #endregion

        //// }

        private void TestState(
            EnigmaSettings target,
            EnigmaReflectorNumber reflector,
            string propertiesChanged,
            EnigmaRotorNumber rotorPositionFastest,
            EnigmaRotorNumber rotorPositionSecond,
            EnigmaRotorNumber rotorPositionThird,
            int ringSettingFastest,
            int ringSettingSecond,
            int ringSettingThird,
            char rotorSettingFastest,
            char rotorSettingSecond,
            char rotorSettingThird,
            string key,
            string iv)
        {
            // IList<EnigmaRotorPosition> positions = new List<EnigmaRotorPosition>() { EnigmaRotorPosition.Fastest, EnigmaRotorPosition.Second, EnigmaRotorPosition.Third };

            // Assert.True("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToList().TrueForAll(x => target.AllowedLetters.Contains(x)));
            // Assert.True(positions.TrueForAll(x => target.Rotors.AllowedRotorPositions.Contains(x)));
            Assert.Equal(rotorPositionFastest, target.Rotors[EnigmaRotorPosition.Fastest].RotorNumber);
            Assert.Equal(rotorPositionSecond, target.Rotors[EnigmaRotorPosition.Second].RotorNumber);
            Assert.Equal(rotorPositionThird, target.Rotors[EnigmaRotorPosition.Third].RotorNumber);
            Assert.Equal(rotorSettingFastest, target.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal(rotorSettingSecond, target.Rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal(rotorSettingThird, target.Rotors[EnigmaRotorPosition.Third].CurrentSetting);
            Assert.Equal(ringSettingFastest, target.Rotors[EnigmaRotorPosition.Fastest].RingPosition);
            Assert.Equal(ringSettingSecond, target.Rotors[EnigmaRotorPosition.Second].RingPosition);
            Assert.Equal(ringSettingThird, target.Rotors[EnigmaRotorPosition.Third].RingPosition);
            Assert.Equal(reflector, target.ReflectorNumber);
            Assert.Equal(propertiesChanged, _propertiesChanged);
            Assert.Equal(key, Encoding.Unicode.GetString(target.Key.ToArray()));
            Assert.Equal(iv, Encoding.Unicode.GetString(target.IV.ToArray()));
        }
    }
}