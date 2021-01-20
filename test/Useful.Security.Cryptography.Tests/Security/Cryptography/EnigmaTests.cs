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
        [Fact]
        public void Name()
        {
            IEnigmaSettings settings = new EnigmaSettings();
            ICipher cipher = new Enigma(settings);
            Assert.Equal("Enigma M3", cipher.CipherName);
            Assert.Equal("Enigma M3", cipher.ToString());
        }

        [Theory]
        [InlineData("", "", 'A')]
        [InlineData("HELLOWORLD", "MFNCZBBFZM", 'K')]
        [InlineData("HELLO WORLD", "MFNCZ BBFZM", 'K')]
        [InlineData("HeLlOwOrLd", "MFNCZBBFZM", 'K')]
        [InlineData("Å", "", 'A')]
        public void Encrypt(string plaintext, string ciphertext, char newFastestRotorPosition)
        {
            IEnigmaSettings settings = new EnigmaSettings();
            ICipher cipher = new Enigma(settings);
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
            Assert.Equal(newFastestRotorPosition, settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
        }

        [Theory]
        [InlineData("", "", 'A')]
        [InlineData("HELLOWORLD", "MFNCZBBFZM", 'K')]
        [InlineData("HELLO WORLD", "MFNCZ BBFZM", 'K')]
        [InlineData("HELLOWORLD", "MfNcZbBfZm", 'K')]
        [InlineData("", "Å", 'A')]
        public void Decrypt(string plaintext, string ciphertext, char newFastestRotorPosition)
        {
            IEnigmaSettings settings = new EnigmaSettings();
            ICipher cipher = new Enigma(settings);
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            Assert.Equal(newFastestRotorPosition, settings.Rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
        }

        [Fact]
        public void Enigma_1941_07_07_19_25()
        {
            StringBuilder sb = new();
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
            IEnigmaReflector reflector = new EnigmaReflector() { ReflectorNumber = EnigmaReflectorNumber.B };

            IEnigmaRotors rotors = new EnigmaRotors()
            {
                Rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>()
                {
                    {
                        EnigmaRotorPosition.Fastest,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.V,
                            RingPosition = 12,
                            CurrentSetting = 'A',
                        }
                    },
                    {
                        EnigmaRotorPosition.Second,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.IV,
                            RingPosition = 21,
                            CurrentSetting = 'L',
                        }
                    },
                    {
                        EnigmaRotorPosition.Third,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.II,
                            RingPosition = 2,
                            CurrentSetting = 'B',
                        }
                    },
                },
            };

            IList<EnigmaPlugboardPair> plugs = new List<EnigmaPlugboardPair>
            {
                { new EnigmaPlugboardPair() { From = 'A', To = 'V' } },
                { new EnigmaPlugboardPair() { From = 'B', To = 'S' } },
                { new EnigmaPlugboardPair() { From = 'C', To = 'G' } },
                { new EnigmaPlugboardPair() { From = 'D', To = 'L' } },
                { new EnigmaPlugboardPair() { From = 'F', To = 'U' } },
                { new EnigmaPlugboardPair() { From = 'H', To = 'Z' } },
                { new EnigmaPlugboardPair() { From = 'I', To = 'N' } },
                { new EnigmaPlugboardPair() { From = 'K', To = 'M' } },
                { new EnigmaPlugboardPair() { From = 'O', To = 'W' } },
                { new EnigmaPlugboardPair() { From = 'R', To = 'X' } },
            };
            IEnigmaPlugboard plugboard = new EnigmaPlugboard(plugs);

            sb = new StringBuilder();
            sb.Append("AUFKL XABTE ILUNG XVONX KURTI ");
            sb.Append("NOWAX KURTI NOWAX NORDW ESTLX ");
            sb.Append("SEBEZ XSEBE ZXUAF FLIEG ERSTR ");
            sb.Append("ASZER IQTUN GXDUB ROWKI XDUBR ");
            sb.Append("OWKIX OPOTS CHKAX OPOTS CHKAX ");
            sb.Append("UMXEI NSAQT DREIN ULLXU HRANG ");
            sb.Append("ETRET ENXAN GRIFF XINFX RGTX");

            string plaintext = sb.ToString();

            IEnigmaSettings settings = new EnigmaSettings() { Reflector = reflector, Rotors = rotors, Plugboard = plugboard };
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('S', rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('R', rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('B', rotors[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact]
        public void PracticalCryptography()
        {
            StringBuilder ciphertext = new();
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
            IEnigmaReflector reflector = new EnigmaReflector() { ReflectorNumber = EnigmaReflectorNumber.B };

            IEnigmaRotors rotors = new EnigmaRotors()
            {
                Rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>()
                {
                    {
                        EnigmaRotorPosition.Fastest,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.I,
                            RingPosition = 2,
                            CurrentSetting = 'S',
                        }
                    },
                    {
                        EnigmaRotorPosition.Second,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.V,
                            RingPosition = 15,
                            CurrentSetting = 'J',
                        }
                    },
                    {
                        EnigmaRotorPosition.Third,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.II,
                            RingPosition = 23,
                            CurrentSetting = 'K',
                        }
                    },
                },
            };

            IList<EnigmaPlugboardPair> plugs = new List<EnigmaPlugboardPair>()
            {
                { new EnigmaPlugboardPair() { From = 'P', To = 'O' } },
                { new EnigmaPlugboardPair() { From = 'M', To = 'L' } },
                { new EnigmaPlugboardPair() { From = 'I', To = 'U' } },
                { new EnigmaPlugboardPair() { From = 'K', To = 'J' } },
                { new EnigmaPlugboardPair() { From = 'N', To = 'H' } },
                { new EnigmaPlugboardPair() { From = 'Y', To = 'T' } },
            };

            IEnigmaPlugboard plugboard = new EnigmaPlugboard(plugs);

            StringBuilder plaintext = new();
            plaintext.Append("THEENIGMACIPHERWASAFIELDCIPHER");
            plaintext.Append("USEDBYTHEGERMANSDURINGWORLDWAR");
            plaintext.Append("IITHEENIGMAISONEOFTHEBETTERKNO");
            plaintext.Append("WNHISTORICALENCRYPTIONMACHINES");
            plaintext.Append("ANDITACTUALLYREFERSTOARANGEOFS");
            plaintext.Append("IMILARCIPHERMACHINES");

            IEnigmaSettings settings = new EnigmaSettings() { Reflector = reflector, Rotors = rotors, Plugboard = plugboard };
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('G', rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('P', rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('K', rotors[EnigmaRotorPosition.Third].CurrentSetting);
        }

        [Fact(Skip = "Settings uncertain.")]
        public void SinghCodeBook()
        {
            StringBuilder sb = new();
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
            IEnigmaReflector reflector = new EnigmaReflector() { ReflectorNumber = EnigmaReflectorNumber.B };

            IEnigmaRotors rotors = new EnigmaRotors()
            {
                Rotors = new Dictionary<EnigmaRotorPosition, IEnigmaRotor>()
                {
                    {
                        EnigmaRotorPosition.Fastest,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.II,
                            RingPosition = 1,
                            CurrentSetting = 'A',
                        }
                    },
                    {
                        EnigmaRotorPosition.Second,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.I,
                            RingPosition = 1,
                            CurrentSetting = 'U',
                        }
                    },
                    {
                        EnigmaRotorPosition.Third,
                        new EnigmaRotor()
                        {
                            RotorNumber = EnigmaRotorNumber.III,
                            RingPosition = 1,
                            CurrentSetting = 'O',
                        }
                    },
                },
            };

            IList<EnigmaPlugboardPair> plugs = new List<EnigmaPlugboardPair>()
            {
                { new EnigmaPlugboardPair() { From = 'E', To = 'I' } },
                { new EnigmaPlugboardPair() { From = 'A', To = 'S' } },
                { new EnigmaPlugboardPair() { From = 'J', To = 'N' } },
                { new EnigmaPlugboardPair() { From = 'K', To = 'L' } },
                { new EnigmaPlugboardPair() { From = 'M', To = 'U' } },
                { new EnigmaPlugboardPair() { From = 'O', To = 'T' } },
            };

            IEnigmaPlugboard plugboard = new EnigmaPlugboard(plugs);

            sb = new();
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

            IEnigmaSettings settings = new EnigmaSettings() { Reflector = reflector, Rotors = rotors, Plugboard = plugboard };
            ICipher cipher = new Enigma(settings);
            string newPlaintext = cipher.Decrypt(ciphertext.ToString());
            Assert.Equal(plaintext.ToString(), newPlaintext);
            Assert.Equal('S', rotors[EnigmaRotorPosition.Fastest].CurrentSetting);
            Assert.Equal('R', rotors[EnigmaRotorPosition.Second].CurrentSetting);
            Assert.Equal('B', rotors[EnigmaRotorPosition.Third].CurrentSetting);
        }
    }
}