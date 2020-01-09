// <copyright file="MonoAlphabeticTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.Security.Cryptography;
    using Xunit;

    public class MonoAlphabeticTests
    {
        public static TheoryData<string, string, string> Data => new TheoryData<string, string, string>
        {
            { "ABC|BAC", "ABC", "BAC" },
            { "ABCD|BADC", "ABCD", "BADC" },
            { "ABC|BCA", "ABC", "BCA" },
            { "ABC|ABC", "ABC", "ABC" },
            { "ABCD|ABCD", "Å", "Å" },
            { "ABCD|BADC", "AB CD", "BA DC" },
            { "ABCDEFGHIJKLMNOPQRSTUVWXYZ|BADCFEHGIJKLMNOPQRSTUVWXYZ", "HeLlOwOrLd", "GeLlOwOrLd" },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptCipher(string key, string plaintext, string ciphertext)
        {
            using MonoAlphabetic cipher = new MonoAlphabetic
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void DecryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new MonoAlphabetic
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(plaintext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Decrypt, ciphertext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptCipher(string key, string plaintext, string ciphertext)
        {
            using MonoAlphabetic cipher = new MonoAlphabetic
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void EncryptSymmetric(string key, string plaintext, string ciphertext)
        {
            using SymmetricAlgorithm cipher = new MonoAlphabetic
            {
                Key = Encoding.Unicode.GetBytes(key),
            };
            Assert.Equal(ciphertext, CipherMethods.SymmetricTransform(cipher, CipherTransformMode.Encrypt, plaintext));
        }

        [Fact]
        public void IvGenerateCorrectness()
        {
            using MonoAlphabetic cipher = new MonoAlphabetic();
            cipher.GenerateIV();
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void IvSet()
        {
            using MonoAlphabetic cipher = new MonoAlphabetic
            {
                IV = Encoding.Unicode.GetBytes("A"),
            };
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void CtorSettings()
        {
            byte[] key = Encoding.Unicode.GetBytes("ABC|ABC");
            using MonoAlphabetic cipher = new MonoAlphabetic(new MonoAlphabeticSettings(key));
            Assert.Equal(key, cipher.Settings.Key.ToArray());
            Assert.Equal(key, cipher.Key);
            Assert.Equal(Array.Empty<byte>(), cipher.Settings.IV.ToArray());
            Assert.Equal(Array.Empty<byte>(), cipher.IV);
        }

        [Fact]
        public void KeyGenerateCorrectness()
        {
            using MonoAlphabetic cipher = new MonoAlphabetic();
            string keyString;
            for (int i = 0; i < 100; i++)
            {
                cipher.GenerateKey();
                keyString = Encoding.Unicode.GetString(cipher.Key);

                // How to test for correctness?
            }
        }

        [Fact]
        public void KeyGenerateRandomness()
        {
            bool diff = false;

            using (MonoAlphabetic cipher = new MonoAlphabetic())
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
            using MonoAlphabetic cipher = new MonoAlphabetic();
            byte[] key = Encoding.Unicode.GetBytes("ABC|ABC");
            cipher.Key = key;
            Assert.Equal(key, cipher.Settings.Key.ToArray());
            Assert.Equal(key, cipher.Key);
        }

        [Fact]
        public void Name()
        {
            using MonoAlphabetic cipher = new MonoAlphabetic();
            Assert.Equal("MonoAlphabetic", cipher.CipherName);
            Assert.Equal("MonoAlphabetic", cipher.ToString());
        }

        [Fact]
        public void SinghCodeBook()
        {
            string ciphertext =
@"BT JPX RMLX PCUV AMLX ICVJP IBTWXVR CI M LMT'R PMTN, MTN YVCJX CDXV MWMBTRJ JPX AMTNGXRJBAH UQCT JPX QGMRJXV CI JPX YMGG CI JPX HBTW'R QMGMAX; " +
"MTN JPX HBTW RMY JPX QMVJ CI JPX PMTN JPMJ YVCJX. " +
"JPXT JPX HBTW'R ACUTJXTMTAX YMR APMTWXN, MTN PBR JPCUWPJR JVCUFGXN PBL, RC JPMJ JPX SCBTJR CI PBR GCBTR YXVX GCCRXN, MTN PBR HTXXR RLCJX CTX MWMBTRJ MTCJPXV. " +
"JPX HBTW AVBXN MGCUN JC FVBTW BT JPX MRJVCGCWXVR, JPX APMGNXMTR, MTN JPX RCCJPRMEXVR. " +
"MTN JPX HBTW RQMHX, MTN RMBN JC JPX YBRX LXT CI FMFEGCT, YPCRCXDXV RPMGG VXMN JPBR YVBJBTW, MTN RPCY LX JPX BTJXVQVXJMJBCT JPXVXCI, RPMGG FX AGCJPXN YBJP RAMVGXJ, MTN PMDX M APMBT CI WCGN MFCUJ PBR TXAH, MTN RPMGG FX JPX JPBVN VUGXV BT JPX HBTWNCL. " +
"JPXT AMLX BT MGG JPX HBTW'R YBRX LXT; " +
"FUJ JPXE ACUGN TCJ VXMN JPX YVBJBTW, TCV LMHX HTCYT JC JPX HBTW JPX BTJXVQVXJMJBCT JPXVXCI. " +
"JPXT YMR HBTW FXGRPMOOMV WVXMJGE JVCUFGXN, MTN PBR ACUTJXTMTAX YMR APMTWXN BT PBL, MTN PBR GCVNR YXVX MRJCTBRPXN. " +
"TCY JPX KUXXT, FE VXMRCT CI JPX YCVNR CI JPX HBTW MTN PBR GCVNR, AMLX BTJC JPX FMTKUXJ PCURX; " +
"MTN JPX KUXXT RQMHX MTN RMBN, C HBTW, GBDX ICVXDXV; " +
"GXJ TCJ JPE JPCUWPJR JVCUFGX JPXX, TCV GXJ JPE ACUTJXTMTAX FX APMTWXN; " +
"JPXVX BR M LMT BT JPE HBTWNCL, BT YPCL BR JPX RQBVBJ CI JPX PCGE WCNR; " +
"MTN BT JPX NMER CI JPE IMJPXV GBWPJ MTN UTNXVRJMTNBTW MTN YBRNCL, GBHX JPX YBRNCL CI JPX WCNR, YMR ICUTN BT PBL; " +
"YPCL JPX HBTW TXFUAPMNTXOOMV JPE IMJPXV, JPX HBTW, B RME, JPE IMJPXV, LMNX LMRJXV CI JPX LMWBABMTR, MRJVCGCWXVR, APMGNXMTR, MTN RCCJPRMEXVR; " +
"ICVMRLUAP MR MT XZAXGGXTJ RQBVBJ, MTN HTCYGXNWX, MTN UTNXVRJMTNBTW, BTJXVQVXJBTW CI NVXMLR, MTN RPCYBTW CI PMVN RXTJXTAXR, MTN NBRRCGDBTW CI NCUFJR, YXVX ICUTN BT JPX RMLX NMTBXG, YPCL JPX HBTW TMLXN FXGJXRPMOOMV; " +
"TCY GXJ NMTBXG FX AMGGXN, MTN PX YBGG RPCY JPX BTJXVQVXJMJBCT. " +
"JPX IBVRJ ACNXYCVN BR CJPXGGC. ";

            string plaintext =
@"IN THE SAME HOUR CAME FORTH FINGERS OF A MAN'S HAND, AND WROTE OVER AGAINST THE CANDLESTICK UPON THE PLASTER OF THE WALL OF THE KING'S PALACE; " +
"AND THE KING SAW THE PART OF THE HAND THAT WROTE. " +
"THEN THE KING'S COUNTENANCE WAS CHANGED, AND HIS THOUGHTS TROUBLED HIM, SO THAT THE JOINTS OF HIS LOINS WERE LOOSED, AND HIS KNEES SMOTE ONE AGAINST ANOTHER. " +
"THE KING CRIED ALOUD TO BRING IN THE ASTROLOGERS, THE CHALDEANS, AND THE SOOTHSAYERS. " +
"AND THE KING SPAKE, AND SAID TO THE WISE MEN OF BABYLON, WHOSOEVER SHALL READ THIS WRITING, AND SHOW ME THE INTERPRETATION THEREOF, SHALL BE CLOTHED WITH SCARLET, AND HAVE A CHAIN OF GOLD ABOUT HIS NECK, AND SHALL BE THE THIRD RULER IN THE KINGDOM. " +
"THEN CAME IN ALL THE KING'S WISE MEN; " +
"BUT THEY COULD NOT READ THE WRITING, NOR MAKE KNOWN TO THE KING THE INTERPRETATION THEREOF. " +
"THEN WAS KING BELSHAZZAR GREATLY TROUBLED, AND HIS COUNTENANCE WAS CHANGED IN HIM, AND HIS LORDS WERE ASTONISHED. " +
"NOW THE QUEEN, BY REASON OF THE WORDS OF THE KING AND HIS LORDS, CAME INTO THE BANQUET HOUSE; " +
"AND THE QUEEN SPAKE AND SAID, O KING, LIVE FOREVER; " +
"LET NOT THY THOUGHTS TROUBLE THEE, NOR LET THY COUNTENANCE BE CHANGED; " +
"THERE IS A MAN IN THY KINGDOM, IN WHOM IS THE SPIRIT OF THE HOLY GODS; " +
"AND IN THE DAYS OF THY FATHER LIGHT AND UNDERSTANDING AND WISDOM, LIKE THE WISDOM OF THE GODS, WAS FOUND IN HIM; " +
"WHOM THE KING NEBUCHADNEZZAR THY FATHER, THE KING, I SAY, THY FATHER, MADE MASTER OF THE MAGICIANS, ASTROLOGERS, CHALDEANS, AND SOOTHSAYERS; " +
"FORASMUCH AS AN EXCELLENT SPIRIT, AND KNOWLEDGE, AND UNDERSTANDING, INTERPRETING OF DREAMS, AND SHOWING OF HARD SENTENCES, AND DISSOLVING OF DOUBTS, WERE FOUND IN THE SAME DANIEL, WHOM THE KING NAMED BELTESHAZZAR; " +
"NOW LET DANIEL BE CALLED, AND HE WILL SHOW THE INTERPRETATION. " +
"THE FIRST CODEWORD IS OTHELLO. ";

            byte[] key = Encoding.Unicode.GetBytes("ABCDEFGHIJKLMNOPQRSTUVWXYZ|MFANXIWPBSHGLTCQKVRJUDYZEO");

            using MonoAlphabetic cipher = new MonoAlphabetic(new MonoAlphabeticSettings(key));
            System.Diagnostics.Debug.WriteLine(cipher.Decrypt(ciphertext));
            Assert.Equal(plaintext, cipher.Decrypt(ciphertext));
            Assert.Equal(ciphertext, cipher.Encrypt(plaintext));
        }
    }
}