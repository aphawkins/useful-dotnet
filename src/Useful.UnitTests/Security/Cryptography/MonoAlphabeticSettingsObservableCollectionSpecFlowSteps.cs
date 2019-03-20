namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Text;
    using TechTalk.SpecFlow;

    [Binding]
    [CLSCompliant(false)]
    public class MonoAlphabeticSettingsObservableCollectionSpecFlowSteps
    {
        /// <summary>
        /// The arguments of the raised events.
        /// </summary>
        private readonly List<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();
        private MonoAlphabeticSettingsObservableCollection settings;

        /// <summary>
        /// The delimiter for the changed properties.
        /// </summary>
        private const char PropertiesChangedDelimiter = ';';

        /// <summary>
        /// A delimited list of changed properties.
        /// </summary>
        private string propertiesChanged;

        [Given(@"I have the default settings")]
        public void GivenIHaveTheDefaultSettings()
        {
            this.settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        }

        [Then(@"The key will equal ""(.*)""")]
        public void ThenTheKeyWillEqual(string key)
        {
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ||True", Encoding.Unicode.GetString(this.settings.Key.ToArray()));
        }

        [Then(@"The IV will equal ""(.*)""")]
        public void ThenTheIVWillEqual(string iv)
        {
            Assert.AreEqual(iv, Encoding.Unicode.GetString(this.settings.IV.ToArray()));
        }

        [Then(@"The allowed letters will equal ""(.*)""")]
        public void ThenTheAllowedLettersWillEqual(string letters)
        {
            Assert.IsTrue("ABCDEFGHIJKLMNOPQRSTUVWXYZ".SequenceEqual(this.settings.AllowedLetters));
        }

        [Then(@"The cipher name will equal ""(.*)""")]
        public void ThenTheCipherNameWillEqual(string cipherName)
        {
            Assert.AreEqual(cipherName, this.settings.CipherName);
        }

        [When(@"I make the substitution ""(.*)"" to ""(.*)""")]
        public void WhenIMakeTheSubstitution(string from, string to)
        {
            try
            {
                this.settings.PropertyChanged += SettingsPropertyChanged;
                this.settings[from[0]] = to[0];
            }
            catch (ArgumentException)
            {
                // An unallowed letter was used?
            }
        }

        [Given(@"I have the substitution ""(.*)"" to ""(.*)""")]
        public void GivenIHaveTheSubstitution(string from, string to)
        {
            this.settings[from[0]] = to[0];
        }

        [Then(@"The letter ""(.*)"" will encrypt to ""(.*)""")]
        public void ThenTheLetterWillEncryptTo(string plaintext, string ciphertext)
        {
            Assert.AreEqual(ciphertext[0], this.settings[plaintext[0]]);
        }

        [Then(@"The letter ""(.*)"" will decrypt to ""(.*)""")]
        public void ThenTheLetterWillDecryptTo(string ciphertext, string plaintext)
        {
            Assert.AreEqual(plaintext[0], this.settings.Reverse(ciphertext[0]));
        }


        [Then(@"The properties changed are ""(.*)""")]
        public void ThenThePropertiesChangedAre(string changed)
        {
            Assert.AreEqual(changed, this.propertiesChanged);
        }

        [When(@"I Reset the settings")]
        public void WhenIResetTheSettings()
        {
            this.settings.PropertyChanged += this.SettingsPropertyChanged;
            this.settings.Reset();
        }

        [Then(@"The substitution count will be (.*)")]
        public void ThenTheSubstitutionCountWillBe(int count)
        {
            Assert.AreEqual(count, this.settings.SubstitutionCount);
        }

        [When(@"I create a new instance with the key ""(.*)"" and IV ""(.*)"" values")]
        public void WhenICreateANewInstanceWithTheKeyAndIVValues(string key, string iv)
        {
            this.settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(key), Encoding.Unicode.GetBytes(iv));
        }

        [Then(@"The key will be ""(.*)""")]
        public void ThenTheKeyWillBe(string key)
        {
            Assert.AreEqual(key, Encoding.Unicode.GetString(this.settings.Key.ToArray()));
        }

        [Then(@"The IV will be ""(.*)""")]
        public void ThenTheIVWillBe(string iv)
        {
            Assert.AreEqual(iv, Encoding.Unicode.GetString(this.settings.IV.ToArray()));
        }

        [Given(@"I am testing invalid keys:")]
        public void GivenIAmTestingInvalidKeys(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                try
                {
                    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(row["Key"]), Encoding.Unicode.GetBytes(row["IV"]));
                }
                catch (Exception)
                {
                    continue;
                }

                Assert.Fail();
            }
        }

        [Then(@"The SettingKey will equal ""(.*)""")]
        public void ThenTheSettingKeyWillEqual(string settingKey)
        {
            Assert.AreEqual(settingKey, this.settings.SettingKey());
        }


        [Given(@"I am testing MonoAlphabeticSettingsObservableCollection:")]
        public void GivenIAmTestingMonoAlphabeticSettingsObservableCollection(Table table)
        {
            foreach (TableRow row in table.Rows)
            {
                MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(row["Key"]), Encoding.Unicode.GetBytes(row["IV"]));
                this.collectionChanged.Clear();
                settings.CollectionChanged += this.TestCollectionChanged;
                string subs = row["Substitutions"];
                if (!string.IsNullOrEmpty(subs))
                {
                    settings[subs[0]] = subs[1];
                    string[] changes = row["Changed"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    Assert.AreEqual(changes.Length, this.collectionChanged.Count);
                    for (int i = 0; i < this.collectionChanged.Count; i++)
                    {
                        string[] change = changes[i].Split(',');
                        string newSubs = change[0];
                        string oldSubs = change[1];

                        Assert.AreEqual(row["Key"].IndexOf(newSubs[0]), this.collectionChanged[i].NewStartingIndex);
                        Assert.AreEqual(newSubs[0], ((KeyValuePair<char, char>)this.collectionChanged[i].NewItems[0]).Key);
                        Assert.AreEqual(newSubs[1], ((KeyValuePair<char, char>)this.collectionChanged[i].NewItems[0]).Value);
                        Assert.AreEqual(row["Key"].IndexOf(oldSubs[0]), this.collectionChanged[i].OldStartingIndex);
                        Assert.AreEqual(oldSubs[0], ((KeyValuePair<char, char>)this.collectionChanged[i].OldItems[0]).Key);
                        Assert.AreEqual(oldSubs[1], ((KeyValuePair<char, char>)this.collectionChanged[i].OldItems[0]).Value);
                    }
                }

                Assert.AreEqual(row["NewKey"], Encoding.Unicode.GetString(settings.Key.ToArray()));
            }
        }

        private void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.propertiesChanged += e.PropertyName;
            this.propertiesChanged += PropertiesChangedDelimiter;
        }

        private void TestCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.collectionChanged.Add(e);
        }
    }
}
