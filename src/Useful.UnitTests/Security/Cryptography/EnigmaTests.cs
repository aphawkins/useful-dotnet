// <copyright file="EnigmaTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class EnigmaTests
    {
        [Fact]
        public void Enigma_ctor()
        {
            using (Enigma target = new Enigma())
            {
            }
        }

        [Fact]
        public void Enigma_Default()
        {
            // Default is Military
            using (Enigma target = new Enigma())
            {
                Assert.Equal("Military|B|I II III|A A A|", Encoding.Unicode.GetString(target.Key));
                Assert.Equal("A A A", Encoding.Unicode.GetString(target.IV));
            }
        }

        [Fact]
        public void Enigma_Military()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"A A A", @"HELLOWORLD", @"MFNCZBBFZM", @"A A K");
            }
        }

        [Fact]
        public void Enigma_M4()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"M4|BThin|Beta I II III|A A A A|", @"A A A A", @"HELLOWORLD", @"ILBDAAMTAZ", @"A A A K");
            }
        }

        [Fact]
        public void Enigma_Rings_1()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|I II III|A A B|", @"A A B", @"A", @"B", @"A A C");
            }
        }

        [Fact]
        public void Enigma_Rings_2()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|I II III|B B B|", @"A A A", @"HELLOWORLD", @"LOFUHHMJJX", @"A A K");
            }
        }

        [Fact]
        public void Enigma_Notches_Singlestep()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"V E Q", @"A", @"Z", @"W F R");
                TestTarget(target, @"Military|B|III II I|A A A|", @"W F R", @"A", @"U", @"W F S");
            }
        }

        [Fact]
        public void Enigma_Notches_Doublestep()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"K D O", @"A", @"U", @"K D P");
                TestTarget(target, @"Military|B|III II I|A A A|", @"K D P", @"A", @"L", @"K D Q");
                TestTarget(target, @"Military|B|III II I|A A A|", @"K D Q", @"A", @"M", @"K E R");

                // Should doublestep the middle rotor here
                TestTarget(target, @"Military|B|III II I|A A A|", @"K E R", @"A", @"H", @"L F S");
                TestTarget(target, @"Military|B|III II I|A A A|", @"L F S", @"A", @"J", @"L F T");
                TestTarget(target, @"Military|B|III II I|A A A|", @"L F T", @"A", @"C", @"L F U");
            }
        }

        [Fact]
        public void Enigma_Spaces()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"A A A", @"HELLO WORLD", @"MFNCZBBFZM", @"A A K");
            }
        }

        [Fact]
        public void Enigma_Disallowed_Chars()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"A A A", @"HELLOÅÅÅÅÅWORLD", @"MFNCZBBFZM", @"A A K");
            }
        }

        [Fact]
        public void Enigma_Mixed_Case()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"A A A", @"HeLlOwOrLd", @"MFNCZBBFZM", @"A A K");
            }
        }

        [Fact]
        public void Enigma_Backspace()
        {
            using (Enigma target = new Enigma())
            {
                TestTarget(target, @"Military|B|III II I|A A A|", @"A A A", @"HELLOWORLD", @"MFNCZBBFZM", @"A A K");
            }
        }

        // [TestMethod()]
        // public void Enigma_Singh()
        // {
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("KJQPW CAISR XWQMA SEUPF OCZOQ");
        //    sb.Append("ZVGZG WWKYE ZVTEM TPZHV NOTKZ");
        //    sb.Append("HRCCF QLVRP CCWLW PUYON FHOGD");
        //    sb.Append("DMOJX GGBHW WUXNJ EZAXF UMEYS");
        //    sb.Append("ECSMA ZFXNN ASSZG WRBDD MAPGM");
        //    sb.Append("RWTGX XZAXL BXCPH ZBOUY VRRVF");
        //    sb.Append("DKHXM QOGYL YYCUW QBTAD RLBOZ");
        //    sb.Append("KYXQP WUUAF MIZTC EAXBC REDHZ");
        //    sb.Append("JDOPS QTNLI HIQHN MJZUH SMVAH");
        //    sb.Append("HQJLI JRRXQ ZNFKH UIINZ PMPAF");
        //    sb.Append("LHYON MRMDA DFOXT YOPEW EJGEC");
        //    sb.Append("AHPYF VMCIX AQDYI AGZXL DTFJW");
        //    sb.Append("JQZMG BSNER MIPCK POVLT HZOTU");
        //    sb.Append("XQLRS RZNQL DHXHL GHYDN ZKVBF");
        //    sb.Append("DMXRZ BROMD PRUXH MFSHJ");

        // string ciphertext = sb.ToString();

        // // Reflector: B
        //    // Wheel order: III I II (Possibly III II I)
        //    // Ring positions: A A A (?)
        //    // Plug pairs: EI AS JN KL MU OT
        //    // Message key:

        // string keyString = "Military|B|III II I|A F P|EI AS JN KL MU OT";
        //    byte[] key = Encoding.Unicode.GetBytes(keyString);
        //    string ivString = "O U A";
        //    byte[] iv = Encoding.Unicode.GetBytes(ivString);
        //    string newIv = "B R S";

        // sb = new StringBuilder();
        //    sb.Append("DASXL OESUN GSWOR TXIST XPLUT");
        //    sb.Append("OXXST UFEXN EUNXE NTHAE LTXEI");
        //    sb.Append("NEXMI TTEIL UNGXD IEXMI TXDES");
        //    sb.Append("XENTK ODIER TXIST XXICH XHABE");
        //    sb.Append("XDASX LINKS STEHE NDEXB YTEXD");
        //    sb.Append("ESXSC HLUES SELSX ENTDE CKTXX");
        //    sb.Append("ESXIS TXEIN SXEIN SXZER OXEIN");
        //    sb.Append("SXZER OXZER OXEIN SXEIN SXEIN");
        //    sb.Append("SXXIC HXPRO GRAMM IERTE XDESX");
        //    sb.Append("UNDXE NTDEC KTEXD ASSXD ASXWO");
        //    sb.Append("RTXDE BUGGE RXWEN NXESX MITXD");
        //    sb.Append("EMXUN TENST EHEND ENXSC HLUES");
        //    sb.Append("SELXE NTKOD IERTX WIRDX ALSXR");
        //    sb.Append("ESULT ATXDI EXUNT ENSTE HENDE");
        //    sb.Append("NXSCH RIFTZ EICHE NXHAT");

        // string plaintext = sb.ToString();

        // EnigmaSettings settings = new EnigmaSettings(key, iv);

        // Enigma enigma = new Enigma();

        // TestTarget(enigma, keyString, ivString, ciphertext, plaintext, newIv);
        // }

        [Fact]
        public void Enigma_1941_07_07_1925()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("EDPUD NRGYS ZRCXN UYTPO MRMBO ");
            sb.Append("FKTBZ REZKM LXLVE FGUEY SIOZV ");
            sb.Append("EQMIK UBPMM YLKLT TDEIS MDICA ");
            sb.Append("GYKUA CTCDO MOHWX MUUIA UBSTS ");
            sb.Append("LRNBZ SZWNR FXWFY SSXJZ VIJHI ");
            sb.Append("DISHP RKLKA YUPAD TXQSP INQMA ");
            sb.Append("TLPIF SVKDA SCTAC DPBOP VHJK ");

            string ciphertext = sb.ToString();

            // Reflector: B
            // Wheel order: II IV V
            // Ring positions:  02 21 12  (B U L)
            // Plug pairs: AV BS CG DL FU HZ IN KM OW RX
            // Message key: BLA
            string keyString = "Military|B|II IV V|B U L|AV BS CG DL FU HZ IN KM OW RX";
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            string ivString = "B L A";
            byte[] iv = Encoding.Unicode.GetBytes(ivString);
            string newIv = "B R S";

            sb = new StringBuilder();
            sb.Append("AUFKLXABTEILUNGXVONXKURTI");
            sb.Append("NOWAXKURTINOWAXNORDWESTLX");
            sb.Append("SEBEZXSEBEZXUAFFLIEGERSTR");
            sb.Append("ASZERIQTUNGXDUBROWKIXDUBR");
            sb.Append("OWKIXOPOTSCHKAXOPOTSCHKAX");
            sb.Append("UMXEINSAQTDREINULLXUHRANG");
            sb.Append("ETRETENXANGRIFFXINFXRGTX");

            string plaintext = sb.ToString();

            EnigmaSettings settings = new EnigmaSettings(key, iv);

            using (Enigma enigma = new Enigma())
            {
                TestTarget(enigma, keyString, ivString, ciphertext, plaintext, newIv);
            }
        }

        /// <summary>
        /// http://users.telenet.be/d.rijmenants/en/m4project.htm.
        /// </summary>
        [Fact]
        public void Enigma_M4_Example()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("NCZW VUSX PNYM INHZ XMQX ");
            sb.Append("SFWX WLKJ AHSH NMCO CCAK ");
            sb.Append("UQPM KCSM HKSE INJU SBLK ");
            sb.Append("IOSX CKUB HMLL XCSJ USRR ");
            sb.Append("DVKO HULX WCCB GVLI YXEO ");
            sb.Append("AHXR HKKF VDRE WEZL XOBA ");
            sb.Append("FGYU JQUK GRTV UKAM EURB ");
            sb.Append("VEKS UHHV OYHA BCJW MAKL ");
            sb.Append("FKLM YFVN RIZR VVRT KOFD ");
            sb.Append("ANJM OLBG FFLE OPRG TFLV ");
            sb.Append("RHOW OPBE KVWM UQFM PWPA ");
            sb.Append("RMFH AGKX IIBG ");

            string ciphertext = sb.ToString();

            // Enigma model: Kriegsmarine M4
            // Reflector: B
            // Rotors: Beta - II - IV - I
            // Stecker: AT BL DF GJ HM NW OP QY RZ VX
            // Ringsettings: A-A-A-V
            // Rotor startposition: V-J-N-A
            string keyString = "M4|BThin|Beta II IV I|A A A V|AT BL DF GJ HM NW OP QY RZ VX";
            byte[] key = Encoding.Unicode.GetBytes(keyString);
            string ivString = "V J N A";
            byte[] iv = Encoding.Unicode.GetBytes(ivString);
            string newIv = "V J W Y";

            sb = new StringBuilder();
            sb.Append("VONVONJLOOKSJHFFTTTE");
            sb.Append("INSEINSDREIZWOYYQNNS");
            sb.Append("NEUNINHALTXXBEIANGRI");
            sb.Append("FFUNTERWASSERGEDRUEC");
            sb.Append("KTYWABOSXLETZTERGEGN");
            sb.Append("ERSTANDNULACHTDREINU");
            sb.Append("LUHRMARQUANTONJOTANE");
            sb.Append("UNACHTSEYHSDREIYZWOZ");
            sb.Append("WONULGRADYACHTSMYSTO");
            sb.Append("SSENACHXEKNSVIERMBFA");
            sb.Append("ELLTYNNNNNNOOOVIERYS");
            sb.Append("ICHTEINSNULL");

            string plaintext = sb.ToString();

            EnigmaSettings settings = new EnigmaSettings(key, iv);

            using (Enigma enigma = new Enigma())
            {
                TestTarget(enigma, keyString, ivString, ciphertext, plaintext, newIv);
            }
        }

        private void TestTarget(SymmetricAlgorithm target, string key, string iv, string input, string output, string newIv)
        {
            CipherTestUtils.TestTarget(target, key, iv, input, output, newIv);
            Assert.Equal(key, Encoding.Unicode.GetString(target.Key));
            Assert.Equal(newIv, Encoding.Unicode.GetString(target.IV));
        }
    }
}