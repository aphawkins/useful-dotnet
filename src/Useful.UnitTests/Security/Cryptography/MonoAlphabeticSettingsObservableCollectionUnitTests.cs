//-----------------------------------------------------------------------
// <copyright file="MonoAlphabeticSettingsObservableCollectionUnitTests.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>Unit tests for the associated class.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Unit tests for the MonoAlphabeticSettings class.
    /// </summary>
    [TestClass]
    [DebuggerDisplay("{ToString()}")]
    public sealed class MonoAlphabeticSettingsObservableCollectionUnitTests
    {
        /// <summary>
        /// Test category name.
        /// </summary>
        private const string TestCategory = "MonoAlphabeticSettings";

        /// <summary>
        /// The delimiter for the changed properties.
        /// </summary>
        private const char PropertiesChangedDelimiter = ';';

        /// <summary>
        /// An empty initialization vector.
        /// </summary>
        private static byte[] emptyIv = new byte[0];

        /// <summary>
        /// The arguments of the raised events.
        /// </summary>
        private readonly List<NotifyCollectionChangedEventArgs> collectionChanged = new List<NotifyCollectionChangedEventArgs>();

        /// <summary>
        /// A delimited list of changed properties.
        /// </summary>
        private string propertiesChanged;

        /////// <summary>
        /////// Test the settings using the default Key.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void DefaultKey()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ||True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test the settings using the default IV.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void DefaultIV()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    Assert.AreEqual(string.Empty, Encoding.Unicode.GetString(settings.IV.ToArray()));
        ////}

        /////// <summary>
        /////// Test the settings using the default allowed letters.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void DefaultAllowedLetters()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    Assert.AreEqual(26, settings.AllowedLetters.Count);
        ////}

        /////// <summary>
        /////// Test the cipher name is correct.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CipherName()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    Assert.AreEqual("MonoAlphabetic", settings.CipherName);
        ////}

        /////// <summary>
        /////// Test getting a substitution.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void GetSubstitution()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual('B', settings['A']);
        ////}

        /////// <summary>
        /////// Test INotifyPropertyChanged on a substitution.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyPropertyChangedSubstitutions()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    settings.PropertyChanged += this.TargetPropertyChanged;
        ////    settings['E'] = 'F';

        ////    Assert.AreEqual("Item;Key;", this.propertiesChanged);
        ////}

        /////// <summary>
        /////// Test setting a symmetric substitution using an unallowed char.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////public void SetSubstitutionUnallowedA()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['a'] = 'A';
        ////}

        /////// <summary>
        /////// Test setting a symmetric substitution using an unallowed char.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////public void SetSubstitutionUnallowedB()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'a';
        ////}

        /////// <summary>
        /////// Test getting a symmetric substitution using an unallowed char.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void GetSubstitutionUnallowedA()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual(settings['a'], 'a');
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type A.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionSymmetricTypeA()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'B';
        ////    Assert.AreEqual('B', settings['A']);
        ////    Assert.AreEqual('A', settings['B']);
        ////    Assert.AreEqual('C', settings['C']);
        ////    Assert.AreEqual("ABC|AB|True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type A NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsSymmetricTypeA()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['A'] = 'B';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type B.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionSymmetricTypeB()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['C'] = 'A';
        ////    Assert.AreEqual('C', settings['A']);
        ////    Assert.AreEqual('B', settings['B']);
        ////    Assert.AreEqual('A', settings['C']);
        ////    Assert.AreEqual("ABC|AC|True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type B NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsSymmetricTypeB()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['C'] = 'A';

        ////    Assert.AreEqual(3, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[2];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type C.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionSymmetricTypeC()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AC|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'B';
        ////    Assert.AreEqual('B', settings['A']);
        ////    Assert.AreEqual('A', settings['B']);
        ////    Assert.AreEqual('C', settings['C']);
        ////    Assert.AreEqual("ABC|AB|True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type C NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsSymmetricTypeC()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AC|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['A'] = 'B';

        ////    Assert.AreEqual(3, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[2];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type D NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionSymmetricTypeD()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'A';
        ////    Assert.AreEqual('A', settings['A']);
        ////    Assert.AreEqual('B', settings['B']);
        ////    Assert.AreEqual('C', settings['C']);
        ////    Assert.AreEqual("ABC||True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test a symmetric substitution of Type D NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsSymmetricTypeD()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['A'] = 'A';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type A.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionAsymmetricTypeA()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'B';
        ////    Assert.AreEqual('B', settings['A']);
        ////    Assert.AreEqual('A', settings['B']);
        ////    Assert.AreEqual('C', settings['C']);
        ////    Assert.AreEqual("ABC|AB|False", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type A NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsAsymmetricTypeA()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC||False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['A'] = 'B';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type B.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionAsymmetricTypeB()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['B'] = 'C';
        ////    Assert.AreEqual('B', settings['A']);
        ////    Assert.AreEqual('C', settings['B']);
        ////    Assert.AreEqual('A', settings['C']);
        ////    Assert.AreEqual("ABC|AB BC CA|False", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type B NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsAsymmetricTypeB()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['B'] = 'C';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type C.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionAsymmetricTypeC()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB BC|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['C'] = 'A';
        ////    Assert.AreEqual('B', settings['A']);
        ////    Assert.AreEqual('C', settings['B']);
        ////    Assert.AreEqual('A', settings['C']);
        ////    Assert.AreEqual("ABC|AB BC CA|False", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type C NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsAsymmetricTypeC()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB BC|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['C'] = 'A';

        ////    Assert.AreEqual(0, this.collectionChanged.Count);
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type D.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionAsymmetricTypeD()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB BC CA|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['B'] = 'B';
        ////    Assert.AreEqual('C', settings['A']);
        ////    Assert.AreEqual('B', settings['B']);
        ////    Assert.AreEqual('A', settings['C']);
        ////    Assert.AreEqual("ABC|AC|False", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type D NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsAsymmetricTypeD()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AB BC CA|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['B'] = 'B';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(1, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('B', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type E.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SetSubstitutionAsymmetricTypeE()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AC|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings['A'] = 'A';
        ////    Assert.AreEqual('A', settings['A']);
        ////    Assert.AreEqual('B', settings['B']);
        ////    Assert.AreEqual('C', settings['C']);
        ////    Assert.AreEqual("ABC||False", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test an asymmetric substitution of Type E NotifyCollectionChanged.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void NotifyCollectionChangedSubstitutionsAsymmetricTypeE()
        ////{
        ////    this.propertiesChanged = string.Empty;
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABC|AC|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////    this.collectionChanged.Clear();
        ////    settings.CollectionChanged += this.TargetCollectionChanged;
        ////    settings['A'] = 'A';

        ////    Assert.AreEqual(2, this.collectionChanged.Count);

        ////    NotifyCollectionChangedEventArgs changedArgs = this.collectionChanged[0];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(0, changedArgs.OldStartingIndex);

        ////    changedArgs = this.collectionChanged[1];
        ////    Assert.AreEqual(NotifyCollectionChangedAction.Replace, changedArgs.Action);
        ////    Assert.AreEqual(1, changedArgs.NewItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Key);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.NewItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.NewStartingIndex);
        ////    Assert.AreEqual(1, changedArgs.OldItems.Count);
        ////    Assert.AreEqual('C', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Key);
        ////    Assert.AreEqual('A', ((KeyValuePair<char, char>)changedArgs.OldItems[0]).Value);
        ////    Assert.AreEqual(2, changedArgs.OldStartingIndex);
        ////}

        /////// <summary>
        /////// Test resetting the class.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void Reset()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    settings['A'] = 'B';
        ////    this.propertiesChanged = string.Empty;
        ////    settings.PropertyChanged += this.TargetPropertyChanged;
        ////    settings.Reset();
        ////    Assert.AreEqual(0, settings.SubstitutionCount);
        ////    Assert.AreEqual("Item;Key;", this.propertiesChanged);
        ////}

        /////// <summary>
        /////// Test a reverse substitution.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void Reverse()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    settings['A'] = 'B';
        ////    Assert.AreEqual('A', settings.Reverse('B'));
        ////}

        /////// <summary>
        /////// Test a reverse substitution for an unallowed letter.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void Reverse_Unallowed()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetDefault();
        ////    Assert.AreEqual('a', settings.Reverse('a'));
        ////}

        /////// <summary>
        /////// Tests creating a new symmetric instance with a valid symmetric key.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateSymmetricSymmetric()
        ////{
        ////    string tempKey = @"ABCD|AB CD|True";
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(tempKey), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual(tempKey, Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Tests creating a new symmetric instance with an invalid asymmetric key.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSymmetricAsymmetric()
        ////{
        ////    string tempKey = "ABCD|AB BC CA|True";
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(tempKey), emptyIv);
        ////}

        /////// <summary>
        /////// Tests creating a new asymmetric instance with a valid symmetric key.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateAsymmetricSymmetric()
        ////{
        ////    string tempKey = "ABCD|AB CD|False";
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(tempKey), emptyIv);
        ////    Assert.AreEqual(tempKey, Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Tests creating a new asymmetric instance with a valid asymmetric key.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateAsymmetricAsymmetric()
        ////{
        ////    string tempKey = "ABCD|AB BC CA|False";
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes(tempKey), emptyIv);
        ////    Assert.AreEqual(tempKey, Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Test INotifyPropertyChanged on a created class.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreatePropertyChanged()
        ////{
        ////    this.propertiesChanged = string.Empty;

        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    settings.PropertyChanged += this.TargetPropertyChanged;

        ////    Assert.AreEqual(string.Empty, this.propertiesChanged);
        ////}

        /////// <summary>
        /////// Tests creating a new instance using an empty initialisation vector.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateIV()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual(string.Empty, Encoding.Unicode.GetString(settings.IV.ToArray()));
        ////}

        /////// <summary>
        /////// Tests the allowed letters are correct.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void AllowedLetters()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual(4, settings.AllowedLetters.Count);
        ////}

        /////// <summary>
        /////// Tests the substitution count is correct.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SubstitutionCount()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual(2, settings.SubstitutionCount);
        ////}

        /////// <summary>
        /////// Tests the key is correct.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void SettingKey()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual("AB CD", settings.SettingKey());
        ////}

        /////// <summary>
        /////// Tests creating an instance with an incorrect number of key parts.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateKeyPartsMissing()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with an incorrectly formatted key alphabet.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateAlphabetPadded()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD ||False"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with an incorrectly formatted key substitutions.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSubstitutionsPadded()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD| AB CD |False"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid substitutions - disallowed letters.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSubstitutionsIllegal()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|DE|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid substitutions - incorrect case.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSubstitutionsCaseSensitive()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|aB CD|False"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid substitutions - substituting to self.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSubstitutionToSelf()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AA|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid substitutions - effective duplicate substitution.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSubstitutionsDuplicate()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB BA|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with valid substitutions - Unicode.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateUnicode()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ØABC|ØB|True"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual("ØABC|ØB|True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid allowed letters - duplicate letters.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateAllowedLettersDuplicate()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCC||True"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid symmetry - missing.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSymmetryMissing()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD||"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid symmetry - case sensitivity.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////public void CreateSymmetryCaseSensitive()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD||tRuE"), Encoding.Unicode.GetBytes(string.Empty));
        ////    Assert.AreEqual("ABCD||True", Encoding.Unicode.GetString(settings.Key.ToArray()));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid symmetry - disallowed value.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSymmetryWrong()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD||null"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid symmetry - incorrectly formatted.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSymmetryPaddedStart()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|| True"), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /////// <summary>
        /////// Tests creating an instance with invalid symmetry - incorrectly formatted.
        /////// </summary>
        ////[TestMethod]
        ////[TestCategory(TestCategory)]
        ////[ExpectedException(typeof(ArgumentException))]
        ////[ExcludeFromCodeCoverage]
        ////public void CreateSymmetryPaddedEnd()
        ////{
        ////    MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD||True "), Encoding.Unicode.GetBytes(string.Empty));
        ////}

        /// <summary>
        /// Tests creating an instance with random settings.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetRandom()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetRandom();
            Assert.IsNotNull(settings);
        }

        /// <summary>
        /// Tests creating an instance with random symmetric settings.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetRandomSymmetric()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetRandom(new Collection<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()), true);
            Assert.IsNotNull(settings);
        }

        /// <summary>
        /// Tests creating an instance with random asymmetric settings.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void GetRandomAsymmetric()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.GetRandom(new Collection<char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray()), false);
            Assert.IsNotNull(settings);
        }

        /// <summary>
        /// Tests generic IEnumerable conformance.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void TestEnumerableGenericForEach()
        {
            IEnumerable<KeyValuePair<char, char>> settings = (IEnumerable<KeyValuePair<char, char>>)MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
            foreach (KeyValuePair<char, char> sub in settings)
            {
                Assert.AreEqual(sub.Value, ((MonoAlphabeticSettingsObservableCollection)settings)[sub.Key]);
            }
        }

        /// <summary>
        /// Tests non-generic IEnumerable conformance.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void TestEnumerableNonGenericForEach()
        {
            IEnumerable settings = (IEnumerable)MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
            foreach (KeyValuePair<char, char> sub in settings)
            {
                Assert.AreEqual(sub.Value, ((MonoAlphabeticSettingsObservableCollection)settings)[sub.Key]);
            }
        }

        /// <summary>
        /// Tests non-generic IEnumerable count conformance.
        /// </summary>
        [TestMethod]
        [TestCategory(TestCategory)]
        public void TestEnumerableGenericCount()
        {
            MonoAlphabeticSettingsObservableCollection settings = MonoAlphabeticSettingsObservableCollection.Create(Encoding.Unicode.GetBytes("ABCD|AB CD|True"), Encoding.Unicode.GetBytes(string.Empty));
            Assert.AreEqual(4, settings.Count());
        }

        /// <summary>
        /// Called when a property has changed.
        /// </summary>
        /// <param name="sender">The object that sent the change.</param>
        /// <param name="e">The event arguments.</param>
        private void TargetPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.propertiesChanged += e.PropertyName;
            this.propertiesChanged += PropertiesChangedDelimiter;
        }

        /// <summary>
        /// Called when the collection has changed.
        /// </summary>
        /// <param name="sender">Object sender of the event.</param>
        /// <param name="e">Arguments of the event being raised.</param>
        private void TargetCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            this.collectionChanged.Add(e);
        }
    }
}