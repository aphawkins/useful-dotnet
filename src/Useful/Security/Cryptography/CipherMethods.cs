// <copyright file="CipherMethods.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using Useful.IO;

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
        /// <param name="outputEncodingOverride">The encoding to use.</param>
        public static void SymmetricTransform(SymmetricAlgorithm cipher, CipherTransformMode transformMode, Stream input, Stream output, Encoding outputEncodingOverride)
        {
            Encoding outputEncoding;

            try
            {
                using (ICryptoTransform transformer = GetTransformer(cipher, transformMode))
                {
                    using (StreamReader reader = new StreamReader(input, true))
                    {
                        reader.Peek();

                        outputEncoding = outputEncodingOverride ?? reader.CurrentEncoding;

                        // Transform the stream's encoding
                        // using (EncodingStream enc = new EncodingStream(output, reader.CurrentEncoding, outputEncoding))
                        // {
                        // Create a CryptoStream using the FileStream
                        // and the passed key and initialization vector (IV).
                        // using (CryptoStream crypto = new CryptoStream(enc, transformer, CryptoStreamMode.Write))
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

                        // }
                    }

                    // IUsefulCryptoTransform usefulCryptoTransform = transformer as IUsefulCryptoTransform;
                    // if (usefulCryptoTransform != null)
                    // {
                    //    usefulCryptoTransform.Key.CopyTo(cipher.Key, 0);
                    //    usefulCryptoTransform.IV.CopyTo(cipher.IV, 0);
                    // }
                }
            }
            catch (CryptographicException ex)
            {
                Console.WriteLine($"A Cryptographic error occurred: {ex.Message}");
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"A file error occurred: {ex.Message}");
                throw;
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
                using (MemoryStream inputStream = new MemoryStream(Encoding.Unicode.GetBytes(input)))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        CipherMethods.SymmetricTransform(cipher, transformMode, inputStream, outputStream, Encoding.Unicode);

                        outputStream.Flush();

                        // Get encrypted array of bytes.
                        byte[] encrypted = outputStream.ToArray();

                        char[] encryptedChars = Encoding.Unicode.GetChars(encrypted);

                        return new string(encryptedChars);
                    }
                }

                // }
            }
        }

        /////// <summary>
        /////// Enciphers or deciphers a file.
        /////// </summary>
        /////// <param name="cipher">The cipher to use.</param>
        /////// <param name="transformMode">Defines the way in which the cipher will work.</param>
        /////// <param name="inputFilePath">The input file.</param>
        /////// <param name="outputFilePath">The output file.</param>
        /////// <param name="outputEncodingOverride">The encoding of the output file, else the encoding detected from the preamble is used.</param>
        /////// <returns>An indication of the success of the call.</returns>
        ////internal static ErrorCode DoCipher(SymmetricAlgorithm cipher, CipherTransformMode transformMode, string inputFilePath, string outputFilePath, Encoding outputEncodingOverride)
        ////{
        ////    ErrorCode error = FileManager.CheckFiles(inputFilePath, outputFilePath);
        ////    if (error != ErrorCode.None)
        ////    {
        ////        return error;
        ////    }

        ////    try
        ////    {
        ////        using (FileStream outputStream = new FileStream(outputFilePath, FileMode.OpenOrCreate, FileAccess.Write))
        ////        {
        ////            using (FileStream inputStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read))
        ////            {
        ////                CipherMethods.DoCipher(cipher, transformMode, inputStream, outputStream, outputEncodingOverride);
        ////            }
        ////        }
        ////    }
        ////    catch (UnauthorizedAccessException)
        ////    {
        ////        return ErrorCode.FileSecurity;
        ////    }

        ////    return ErrorCode.None;
        ////}

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
            else
            {
                // Get an decryptor.
                return cipher.CreateDecryptor();
            }
        }
    }
}