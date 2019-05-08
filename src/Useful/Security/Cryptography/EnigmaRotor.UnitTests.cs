using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Useful.Security.Cryptography;
using System.IO;
using System;

namespace UsefulQA
{
    /// <summary>
    ///This is a test class for EnigmaSettingsTest and is intended
    ///to contain all EnigmaSettingsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnigmaRotorUnitTests
    {
        #region Fields
        #endregion

        #region Properties
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContextInstance { get; set; }
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
        //Use TestInitialize to run code before running each test
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
        public void EnigmaRotor_ctor()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
        }

        [TestMethod()]
        public void EnigmaRotor_One()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);

            Assert.AreEqual(target.Forward('A'), 'E');
            Assert.AreEqual(target.Forward('B'), 'K');
            Assert.AreEqual(target.Forward('C'), 'M');
            Assert.AreEqual(target.Forward('D'), 'F');
            Assert.AreEqual(target.Forward('E'), 'L');
            Assert.AreEqual(target.Forward('F'), 'G');
            Assert.AreEqual(target.Forward('G'), 'D');
            Assert.AreEqual(target.Forward('H'), 'Q');
            Assert.AreEqual(target.Forward('I'), 'V');
            Assert.AreEqual(target.Forward('J'), 'Z');
            Assert.AreEqual(target.Forward('K'), 'N');
            Assert.AreEqual(target.Forward('L'), 'T');
            Assert.AreEqual(target.Forward('M'), 'O');
            Assert.AreEqual(target.Forward('N'), 'W');
            Assert.AreEqual(target.Forward('O'), 'Y');
            Assert.AreEqual(target.Forward('P'), 'H');
            Assert.AreEqual(target.Forward('Q'), 'X');
            Assert.AreEqual(target.Forward('R'), 'U');
            Assert.AreEqual(target.Forward('S'), 'S');
            Assert.AreEqual(target.Forward('T'), 'P');
            Assert.AreEqual(target.Forward('U'), 'A');
            Assert.AreEqual(target.Forward('V'), 'I');
            Assert.AreEqual(target.Forward('W'), 'B');
            Assert.AreEqual(target.Forward('X'), 'R');
            Assert.AreEqual(target.Forward('Y'), 'C');
            Assert.AreEqual(target.Forward('Z'), 'J');
        }

        [TestMethod()]
        public void EnigmaRotor_RotorNumber0()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.None);
            Assert.AreEqual(target.RotorNumber, EnigmaRotorNumber.None);
        }

        [TestMethod()]
        public void EnigmaRotor_RotorNumber1()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
            Assert.AreEqual(target.RotorNumber, EnigmaRotorNumber.One);
        }

        [TestMethod()]
        public void EnigmaRotor_CurrentSetting()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
            // Default
            Assert.AreEqual(target.CurrentSetting, 'A');
            target.CurrentSetting = 'W';
            Assert.AreEqual(target.CurrentSetting, 'W');
        }

        // Covered by contract
        //[TestMethod()]
        //public void EnigmaRotor_CurrentSetting_Invalid()
        //{
        //    EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
        //    target.CurrentSetting = 'W';
        //    target.CurrentSetting = 'Å';
        //    Assert.AreEqual(target.CurrentSetting, 'W');
        //}

        [TestMethod()]
        public void EnigmaRotor_RingPosition()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
            // Default
            Assert.AreEqual(target.RingPosition, 'A');
            target.RingPosition = 'W';
            Assert.AreEqual(target.RingPosition, 'W');
        }

        // Covered by contract
        //[TestMethod()]
        //public void EnigmaRotor_RingPosition_Invalid()
        //{
        //    EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
        //    target.RingPosition = 'W';
        //    target.RingPosition = 'Å';
        //    Assert.AreEqual(target.RingPosition, 'W');
        //}

        [TestMethod()]
        public void EnigmaRotor_AdvanceRotor()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
            target.RingPosition = 'A';
            target.CurrentSetting = 'A';
            Assert.AreEqual(target.Forward('A'), 'E');
            target.RingPosition = 'A';
            target.AdvanceRotor();
            Assert.AreEqual(target.CurrentSetting, 'B');
            Assert.AreEqual(target.Forward('A'), 'J');
        }

        [TestMethod()]
        public void EnigmaRotor_Ring()
        {
            EnigmaRotor target = EnigmaRotor.Create(EnigmaRotorNumber.One);
            target.RingPosition = 'B';
            target.CurrentSetting = 'A';
            Assert.AreEqual(target.Forward('A'), 'K');

            target.RingPosition = 'F';
            target.CurrentSetting = 'Y';
            Assert.AreEqual(target.Forward('A'), 'W');
        }

        #endregion
    }
}
