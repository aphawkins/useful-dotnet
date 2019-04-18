// <copyright file="CipherMethods.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Methods to aid cryptography.
    /// </summary>
    public static class CipherMethods
    {
        /// <summary>
        /// Enciphers or deciphers a stream.
        /// </summary>
        /// <param name="cipher">The cipher to use.</param>
        /// <param name="transformMode">Defines the way in which the cipher will work.</param>
        /// <param name="input">The input stream.</param>
        /// <param name="output">The output stream.</param>
        public static void SymmetricTransform(SymmetricAlgorithm cipher, CipherTransformMode transformMode, Stream input, Stream output)
        {
            using (ICryptoTransform transformer = GetTransformer(cipher, transformMode))
            {
                using (StreamReader reader = new StreamReader(input, true))
                {
                    reader.Peek();

                    // Create a CryptoStream using the FileStream and the passed key and initialization vector (IV).
                    using (CryptoStream crypto = new CryptoStream(output, transformer, CryptoStreamMode.Write))
                    {
                        byte[] bytes;

                        // The cipher is expecting Unicode
                        while (!reader.EndOfStream)
                        {
                            bytes = reader.CurrentEncoding.GetBytes(new char[] { (char)reader.Read() });
                            crypto.Write(bytes, 0, bytes.Length);
                        }
                    }
                }

                // IUsefulCryptoTransform usefulCryptoTransform = transformer as IUsefulCryptoTransform;
                // if (usefulCryptoTransform != null)
                // {
                //    usefulCryptoTransform.Key.CopyTo(cipher.Key, 0);
                //    usefulCryptoTransform.IV.CopyTo(cipher.IV, 0);
                // }
            }
        }

        /// <summary>
        /// Enciphers or deciphers a string.
        /// </summary>
        /// <param name="cipher">The cipher to use.</param>
        /// <param name="transformMode">Defines the way in which the cipher will work.</param>
        /// <param name="input">The input text.</param>
        /// <returns>The output text.</returns>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="cipher" /> is null.</exception>
        /// <exception cref="NotImplementedException"> Thrown sometimes.</exception>
        public static string SymmetricTransform(SymmetricAlgorithm cipher, CipherTransformMode transformMode, string input)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException(nameof(cipher));
            }

            Encoding encoding = new UnicodeEncoding();

            using (ICryptoTransform cryptoTransform = GetTransformer(cipher, transformMode))
            {
                // if (cryptoTransform is IUsefulCryptoTransform usefulCryptoTransform)
                // {
                //    string output = usefulCryptoTransform.TransformString(input);

                // usefulCryptoTransform.Key.CopyTo(cipher.Key, 0);
                //    usefulCryptoTransform.IV.CopyTo(cipher.IV, 0);

                // return output;
                // }
                // else
                // {
                //    throw new NotImplementedException();

                // Encrypt the data.
                using (MemoryStream inputStream = new MemoryStream(encoding.GetBytes(input)))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        SymmetricTransform(cipher, transformMode, inputStream, outputStream);

                        // Remove padding on odd length bytes
                        byte[] encrypted;
                        if (cryptoTransform.OutputBlockSize % 2 == 1)
                        {
                            encrypted = TrimArray(outputStream.ToArray());
                        }
                        else
                        {
                            encrypted = outputStream.ToArray();
                        }

                        char[] encryptedChars = encoding.GetChars(encrypted);

                        return new string(encryptedChars);
                    }
                }

                // }
            }
        }

        /// <summary>
        /// Gets a SymmetricAlgorithm's transformer.
        /// </summary>
        /// <param name="cipher">The SymmetricAlgorithm.</param>
        /// <param name="transformMode">The get the encrypt/decrypt transformer.</param>
        /// <returns>The SymmetricAlgorithm's transformer.</returns>
        private static ICryptoTransform GetTransformer(SymmetricAlgorithm cipher, CipherTransformMode transformMode)
        {
            if (transformMode == CipherTransformMode.Encrypt)
            {
                // Get an encryptor.
                return cipher.CreateEncryptor();
            }

            // Get an decryptor.
            return cipher.CreateDecryptor();
        }

        // Resize the dimensions of the array to a size that contains only valid bytes.
        private static byte[] TrimArray(byte[] targetArray)
        {
            int i = 0;

            foreach (byte b in targetArray)
            {
                if ($"{b}".Equals("0", StringComparison.InvariantCultureIgnoreCase))
                {
                    break;
                }

                i++;
            }

            // Create a new array with the number of valid bytes.
            byte[] returnedArray = new byte[i];
            Array.Copy(targetArray, returnedArray, i);
            return returnedArray;
        }
    }
}