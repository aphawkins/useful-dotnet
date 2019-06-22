// <copyright file="EnigmaTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaTests
    {
        [Theory]
        [InlineData("", "", "B|III II I|01 01 01|", "A A A")]
        [InlineData("HELLOWORLD", "MFNCZBBFZM", "B|III II I|01 01 01|", "A A K")]
        [InlineData("HELLO WORLD", "MFNCZ BBFZM", "B|III II I|01 01 01|", "A A K")]
        [InlineData("HeLlOwOrLd", "MOQZT", "B|III II I|01 01 01|", "A A F")]
        [InlineData("Å", "", "B|III II I|01 01 01|", "A A A")]
        public void EncryptCtor(string plaintext, string ciphertext, string newKey, string newIV)
        {
            using (Enigma target = new Enigma())
            {
                string s = CipherMethods.SymmetricTransform(target, CipherTransformMode.Encrypt, plaintext);
                Assert.Equal(ciphertext, s);
                Assert.Equal(newKey, Encoding.Unicode.GetString(target.Key));
                Assert.Equal(newIV, Encoding.Unicode.GetString(target.IV));
            }
        }

        [Theory]
        [InlineData("A", "F", "B|III II I|01 01 01|", "A A A", "A A B")] // Default
        [InlineData("A", "G", "B|III II I|01 01 01|", "A A E", "A A F")] // Change Setting
        [InlineData("A", "J", "B|III II I|01 01 12|", "A A A", "A A B")] // Change Ring
        [InlineData("A", "C", "B|III I II|01 01 01|", "A A A", "A A B")] // Change Rotor
        [InlineData("A", "H", "B|II I III|01 01 01|", "A A A", "A A B")] // Change Rotor
        [InlineData("A", "P", "B|III II IV|01 01 01|", "A A A", "A A B")] // Change Rotor
        [InlineData("A", "U", "B|III II V|01 01 01|", "A A A", "A A B")] // Change Rotor
        [InlineData("A", "W", "B|III II VI|01 01 01|", "A A A", "A A B")] // Change Rotor
        [InlineData("A", "B", "B|III II V|01 01 12|", "A A A", "A A B")] // Change Rotor + Ring
        [InlineData("A", "N", "B|III II V|01 01 12|", "A A E", "A A F")] // Change Rotor + Ring + Setting
        [InlineData("HELLOWORLD", "VKHWQLADBN", "B|III II I|02 02 02|", "A A A", "A A K")]
        [InlineData("A", "M", "B|III II I|01 01 01|", "K D Q", "K E R")] // Notch - single step
        [InlineData("A", "H", "B|III II I|01 01 01|", "K E R", "L F S")] // Doublestep the middle rotor here
        [InlineData("A", "J", "B|III II I|01 01 01|", "L F S", "L F T")] // Notch - single step
        [InlineData("HELLOWORLD", "ZFZEFSQZDU", "B|III II I|01 01 01|AB CD EF GH IJ KL MN OP QR ST UV WX YZ", "A A A", "A A K")] // Complex
        [InlineData("B", "I", "B|II V I|23 15 02|HN IU JK LM OP TY", "K K R", "K K S")] // Bugfix
        public void EncryptSettings(string plaintext, string ciphertext, string keyString, string ivString, string newIV)
        {
            EnigmaSettings settings = new EnigmaSettings(Encoding.Unicode.GetBytes(keyString), Encoding.Unicode.GetBytes(ivString));
            using (Enigma target = new Enigma(settings))
            {
                string s = CipherMethods.SymmetricTransform(target, CipherTransformMode.Encrypt, plaintext);
                Assert.Equal(keyString, Encoding.Unicode.GetString(target.Key));
                Assert.Equal(newIV, Encoding.Unicode.GetString(target.IV));
                Assert.Equal(ciphertext, s);
            }
        }

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
            string keyString = "B|II IV V|02 21 12|AV BS CG DL FU HZ IN KM OW RX";
            string ivString = "B L A";
            string newIv = "B R S";

            sb = new StringBuilder();
            sb.Append("AUFKL XABTE ILUNG XVONX KURTI ");
            sb.Append("NOWAX KURTI NOWAX NORDW ESTLX ");
            sb.Append("SEBEZ XSEBE ZXUAF FLIEG ERSTR ");
            sb.Append("ASZER IQTUN GXDUB ROWKI XDUBR ");
            sb.Append("OWKIX OPOTS CHKAX OPOTS CHKAX ");
            sb.Append("UMXEI NSAQT DREIN ULLXU HRANG ");
            sb.Append("ETRET ENXAN GRIFF XINFX RGTX");

            string plaintext = sb.ToString();

            EnigmaSettings settings = new EnigmaSettings(Encoding.Unicode.GetBytes(keyString), Encoding.Unicode.GetBytes(ivString));
            using (Enigma target = new Enigma(settings))
            {
                string s = CipherMethods.SymmetricTransform(target, CipherTransformMode.Decrypt, ciphertext);
                Assert.Equal(plaintext, s);
                Assert.Equal(keyString, Encoding.Unicode.GetString(target.Key));
                Assert.Equal(newIv, Encoding.Unicode.GetString(target.IV));
            }
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using (Enigma cipher = new Enigma())
            {
                string ivString;
                for (int i = 0; i < 100; i++)
                {
                    cipher.GenerateIV();
                    ivString = Encoding.Unicode.GetString(cipher.IV);

                    // Test IV correctness here
                }
            }
        }

        [Fact]
        public void IvGenerateRandomness()
        {
            bool diff = false;

            using (Enigma cipher = new Enigma())
            {
                byte[] iv = Array.Empty<byte>();
                byte[] newIv;

                cipher.GenerateIV();
                newIv = cipher.IV;
                iv = newIv;

                for (int i = 0; i < 10; i++)
                {
                    if (!newIv.SequenceEqual(iv))
                    {
                        diff = true;
                        break;
                    }

                    iv = newIv;
                    cipher.GenerateIV();
                    newIv = cipher.IV;
                }
            }

            Assert.True(diff);
        }

        [Fact]
        public void IvSet()
        {
            using (Enigma cipher = new Enigma())
            {
                byte[] iv = Encoding.Unicode.GetBytes("A B C");
                cipher.IV = iv;
                Assert.Equal(iv, cipher.Settings.IV.ToArray());
                Assert.Equal(iv, cipher.IV);
            }
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using (Enigma cipher = new Enigma())
            {
                string keyString;
                for (int i = 0; i < 100; i++)
                {
                    cipher.GenerateKey();
                    keyString = Encoding.Unicode.GetString(cipher.Key);

                    // Test key correctness here
                }
            }
        }

        [Fact]
        public void KeyGenerateRandomness()
        {
            bool diff = false;

            using (Enigma cipher = new Enigma())
            {
                byte[] key = Array.Empty<byte>();
                byte[] newKey;

                cipher.GenerateKey();
                newKey = cipher.Key;
                key = newKey;

                for (int i = 0; i < 10; i++)
                {
                    if (!newKey.SequenceEqual(key))
                    {
                        diff = true;
                        break;
                    }

                    key = newKey;
                    cipher.GenerateKey();
                    newKey = cipher.Key;
                }
            }

            Assert.True(diff);
        }
        [Fact]
        public void KeySet()
        {
            using (Enigma cipher = new Enigma())
            {
                byte[] key = Encoding.Unicode.GetBytes("B|I II III|01 02 03|AB CD");
                cipher.Key = key;
                Assert.Equal(key, cipher.Settings.Key.ToArray());
                Assert.Equal(key, cipher.Key);
            }
        }

        [Fact]
        public void Name()
        {
            using (Enigma cipher = new Enigma())
            {
                Assert.Equal("Enigma M3", cipher.CipherName);
                Assert.Equal("Enigma M3", cipher.ToString());
            }
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
            string keyString = "B|II V I|23 15 02|HN IU JK LM OP TY";
            string ivString = "K J S";
            string newIv = "K P G";

            StringBuilder plaintext = new StringBuilder();
            plaintext.Append("THEENIGMACIPHERWASAFIELDCIPHER");
            plaintext.Append("USEDBYTHEGERMANSDURINGWORLDWAR");
            plaintext.Append("IITHEENIGMAISONEOFTHEBETTERKNO");
            plaintext.Append("WNHISTORICALENCRYPTIONMACHINES");
            plaintext.Append("ANDITACTUALLYREFERSTOARANGEOFS");
            plaintext.Append("IMILARCIPHERMACHINES");

            EnigmaSettings settings = new EnigmaSettings(Encoding.Unicode.GetBytes(keyString), Encoding.Unicode.GetBytes(ivString));
            using (Enigma target = new Enigma(settings))
            {
                string s = CipherMethods.SymmetricTransform(target, CipherTransformMode.Decrypt, ciphertext.ToString());
                Assert.Equal(plaintext.ToString(), s);
                Assert.Equal(keyString, Encoding.Unicode.GetString(target.Key));
                Assert.Equal(newIv, Encoding.Unicode.GetString(target.IV));
            }
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
            // Message key:
            string keyString = "B|III II I|01 01 01|EI AS JN KL MU OT";
            string ivString = "O U A";
            string newIv = "B R S";

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

            EnigmaSettings settings = new EnigmaSettings(Encoding.Unicode.GetBytes(keyString), Encoding.Unicode.GetBytes(ivString));
            using (Enigma target = new Enigma(settings))
            {
                string s = CipherMethods.SymmetricTransform(target, CipherTransformMode.Decrypt, ciphertext);
                Assert.Equal(plaintext, s);
                Assert.Equal(keyString, Encoding.Unicode.GetString(target.Key));
                Assert.Equal(newIv, Encoding.Unicode.GetString(target.IV));
            }
        }
    }
}