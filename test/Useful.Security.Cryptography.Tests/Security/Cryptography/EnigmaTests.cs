// <copyright file="EnigmaTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Collections.Generic;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaTests
    {
        [Theory]
        [InlineData("", "", 'A')]
        [InlineData("HELLOWORLD", "MFNCZBBFZM", 'K')]
        [InlineData("HELLO WORLD", "MFNCZ BBFZM", 'K')]
        [InlineData("HeLlOwOrLd", "MOQZT", 'F')]
        [InlineData("Å", "", 'A')]
        public void EncryptCtor(string plaintext, string ciphertext, char newFastestRotorPosition)
        {
            EnigmaSettings settings = new EnigmaSettings();
            Enigma cipher = new Enigma(settings);
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            Assert.Equal(newFastestRotorPosition, settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
        }

        ////[Fact]
        ////public void SettingCtor()
        ////{
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
        ////    rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
        ////    rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        ////    EnigmaSettings target = new EnigmaSettings(EnigmaReflectorNumber.B, rotorSettings, new ReflectorSettings());
        ////    Assert.Equal("ABCDEFGHIJKLMNOPQRSTUVWXYZ", target.CharacterSet);
        ////    Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        ////}

        ////[Fact]
        ////public void SettingDefault()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings();
        ////    Assert.Equal(new EnigmaRotorSettings().SettingKey(), target.Rotors.SettingKey());
        ////}

        ////[Fact]
        ////public void SettingKeyCtor()
        ////{
        ////    EnigmaSettings target = new EnigmaSettings(Encoding.Unicode.GetBytes("B|III II I|01 01 01|"), Encoding.Unicode.GetBytes("C B A"));
        ////    EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
        ////    rotorSettings[EnigmaRotorPosition.Third].CurrentSetting = 'C';
        ////    rotorSettings[EnigmaRotorPosition.Second].CurrentSetting = 'B';
        ////    rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting = 'A';
        ////    Assert.Equal(rotorSettings.SettingKey(), target.Rotors.SettingKey());
        ////}

        [Fact]
        public void Enigma_1941_07_07_19_25()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("EDPUD NRGYS ZRCXN UYTPO MRMBO ");
            sb.Append("FKTBZ REZKM LXLVE FGUEY SIOZV ");
            sb.Append("EQMIK UBPMM YLKLT TDEIS MDICA ");
            sb.Append("GYKUA CTCDO MOHWX MUUIA UBSTS ");
            sb.Append("LRNBZ SZWNR FXWFY SSXJZ VIJHI ");
            sb.Append("DISHP RKLKA YUPAD TXQSP INQMA ");
            sb.Append("TLPIF SVKDA SCTAC DPBOP VHJK");

            string ciphertext = sb.ToString();

            // Reflector: B
            // Wheel order: II IV V
            // Ring positions:  02 21 12  (B U L)
            // Plug pairs: AV BS CG DL FU HZ IN KM OW RX
            // Message key: BLA
            // Final key: BRS
            EnigmaReflectorNumber reflector = EnigmaReflectorNumber.B;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.V, 12, 'A');
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.IV, 21, 'L');
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.II, 2, 'B');
            IDictionary<char, char> plugs = new Dictionary<char, char>
            {
                { 'A', 'V' },
                { 'B', 'S' },
                { 'C', 'G' },
                { 'D', 'L' },
                { 'F', 'U' },
                { 'H', 'Z' },
                { 'I', 'N' },
                { 'K', 'M' },
                { 'O', 'W' },
                { 'R', 'X' },
            };
            IEnigmaPlugboardSettings plugboardSettings = new EnigmaPlugboardSettings(plugs);

            sb = new StringBuilder();
            sb.Append("AUFKL XABTE ILUNG XVONX KURTI ");
            sb.Append("NOWAX KURTI NOWAX NORDW ESTLX ");
            sb.Append("SEBEZ XSEBE ZXUAF FLIEG ERSTR ");
            sb.Append("ASZER IQTUN GXDUB ROWKI XDUBR ");
            sb.Append("OWKIX OPOTS CHKAX OPOTS CHKAX ");
            sb.Append("UMXEI NSAQT DREIN ULLXU HRANG ");
            sb.Append("ETRET ENXAN GRIFF XINFX RGTX");

            string plaintext = sb.ToString();

            IEnigmaSettings settings = new EnigmaSettings(reflector, rotorSettings, plugboardSettings);
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('S', rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('R', rotorSettings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('B', rotorSettings[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void Name()
        {
            IEnigmaSettings settings = new EnigmaSettings();
            ICipher cipher = new Enigma(settings);
            Assert.Equal("Enigma M3", cipher.CipherName);
            Assert.Equal("Enigma M3", cipher.ToString());
        }

        [Fact]
        public void PracticalCryptography()
        {
            StringBuilder ciphertext = new StringBuilder();
            ciphertext.Append("YXBMXADQBDBAAYIMKDODAYIXNBDQZF");
            ciphertext.Append("JKOLFVEEQBCLUUXDFVQYGKEYBVRHON");
            ciphertext.Append("JKPJMKUNLYLZUKBKJOAJTWVWMOMDPG");
            ciphertext.Append("VXEPUKXBVSGHROFOSBCNKEHEHAKWKO");
            ciphertext.Append("GWTBZFXSYCGSUUPPIZTRTFVCXZVCXT");
            ciphertext.Append("FLMTPTAQVMREGWSBFZBM");

            // Reflector: B
            // Wheel order: II V I
            // Ring positions:  23 15 02  (W O B)
            // Plug pairs: PO ML IU KJ NH YT
            // Message key: KJS
            // Final key: KPG
            EnigmaReflectorNumber reflector = EnigmaReflectorNumber.B;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.I, 2, 'S');
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.V, 15, 'J');
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.II, 23, 'K');
            IDictionary<char, char> plugs = new Dictionary<char, char>
            {
                { 'P', 'O' },
                { 'M', 'L' },
                { 'I', 'U' },
                { 'K', 'J' },
                { 'N', 'H' },
                { 'Y', 'T' },
            };
            IEnigmaPlugboardSettings plugboardSettings = new EnigmaPlugboardSettings(plugs);

            StringBuilder plaintext = new StringBuilder();
            plaintext.Append("THEENIGMACIPHERWASAFIELDCIPHER");
            plaintext.Append("USEDBYTHEGERMANSDURINGWORLDWAR");
            plaintext.Append("IITHEENIGMAISONEOFTHEBETTERKNO");
            plaintext.Append("WNHISTORICALENCRYPTIONMACHINES");
            plaintext.Append("ANDITACTUALLYREFERSTOARANGEOFS");
            plaintext.Append("IMILARCIPHERMACHINES");

            IEnigmaSettings settings = new EnigmaSettings(reflector, rotorSettings, plugboardSettings);
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('G', rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('P', rotorSettings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('K', rotorSettings[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact(Skip = "Settings uncertain.")]
        public void SinghCodeBook()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("KJQPW CAISR XWQMA SEUPF OCZOQ");
            sb.Append("ZVGZG WWKYE ZVTEM TPZHV NOTKZ");
            sb.Append("HRCCF QLVRP CCWLW PUYON FHOGD");
            sb.Append("DMOJX GGBHW WUXNJ EZAXF UMEYS");
            sb.Append("ECSMA ZFXNN ASSZG WRBDD MAPGM");
            sb.Append("RWTGX XZAXL BXCPH ZBOUY VRRVF");
            sb.Append("DKHXM QOGYL YYCUW QBTAD RLBOZ");
            sb.Append("KYXQP WUUAF MIZTC EAXBC REDHZ");
            sb.Append("JDOPS QTNLI HIQHN MJZUH SMVAH");
            sb.Append("HQJLI JRRXQ ZNFKH UIINZ PMPAF");
            sb.Append("LHYON MRMDA DFOXT YOPEW EJGEC");
            sb.Append("AHPYF VMCIX AQDYI AGZXL DTFJW");
            sb.Append("JQZMG BSNER MIPCK POVLT HZOTU");
            sb.Append("XQLRS RZNQL DHXHL GHYDN ZKVBF");
            sb.Append("DMXRZ BROMD PRUXH MFSHJ");

            string ciphertext = sb.ToString();

            // Reflector: B
            // Wheel order: III I II (Possibly III II I)
            // Ring positions: 01 01 01 (A A A) (?)
            // Plug pairs: EI AS JN KL MU OT
            // Message key: OUA (?)
            // Final key: BRS (?)
            EnigmaReflectorNumber reflector = EnigmaReflectorNumber.B;
            EnigmaRotorSettings rotorSettings = new EnigmaRotorSettings();
            rotorSettings[EnigmaRotorPosition.Fastest] = new EnigmaRotor(EnigmaRotorNumber.II, 1, 'A');
            rotorSettings[EnigmaRotorPosition.Second] = new EnigmaRotor(EnigmaRotorNumber.I, 1, 'U');
            rotorSettings[EnigmaRotorPosition.Third] = new EnigmaRotor(EnigmaRotorNumber.III, 1, 'O');
            IDictionary<char, char> plugs = new Dictionary<char, char>
            {
                { 'E', 'I' },
                { 'A', 'S' },
                { 'J', 'N' },
                { 'K', 'L' },
                { 'M', 'U' },
                { 'O', 'T' },
            };
            IEnigmaPlugboardSettings plugboard = new EnigmaPlugboardSettings(plugs);

            sb = new StringBuilder();
            sb.Append("DASXL OESUN GSWOR TXIST XPLUT");
            sb.Append("OXXST UFEXN EUNXE NTHAE LTXEI");
            sb.Append("NEXMI TTEIL UNGXD IEXMI TXDES");
            sb.Append("XENTK ODIER TXIST XXICH XHABE");
            sb.Append("XDASX LINKS STEHE NDEXB YTEXD");
            sb.Append("ESXSC HLUES SELSX ENTDE CKTXX");
            sb.Append("ESXIS TXEIN SXEIN SXZER OXEIN");
            sb.Append("SXZER OXZER OXEIN SXEIN SXEIN");
            sb.Append("SXXIC HXPRO GRAMM IERTE XDESX");
            sb.Append("UNDXE NTDEC KTEXD ASSXD ASXWO");
            sb.Append("RTXDE BUGGE RXWEN NXESX MITXD");
            sb.Append("EMXUN TENST EHEND ENXSC HLUES");
            sb.Append("SELXE NTKOD IERTX WIRDX ALSXR");
            sb.Append("ESULT ATXDI EXUNT ENSTE HENDE");
            sb.Append("NXSCH RIFTZ EICHE NXHAT");

            string plaintext = sb.ToString();

            IEnigmaSettings settings = new EnigmaSettings(reflector, rotorSettings, plugboard);
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('S', rotorSettings[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('R', rotorSettings[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('B', rotorSettings[EnigmaRotorPosition.Third].CurrentSetting);
        }
    }
}