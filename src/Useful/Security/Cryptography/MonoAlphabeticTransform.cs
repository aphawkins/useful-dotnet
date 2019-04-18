//-----------------------------------------------------------------------
// <copyright file="MonoAlphabeticTransform.cs" company="APH Software">
//     Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>
// <summary>The MonoAlphabetic algorithm.</summary>
//-----------------------------------------------------------------------

namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The MonoAlphabetic algorithm.
    /// </summary>
    internal sealed class MonoAlphabeticTransform : IUsefulCryptoTransform, ICryptoTransform
    {
        /// <summary>
        /// How many bytes this cipher can process in one go. 2 bytes == unicode.
        /// </summary>
        private const int BlockSize = 2;

        /// <summary>
        /// Whether to encipher or decipher.
        /// </summary>
        private readonly CipherTransformMode transformMode;

        /// <summary>
        /// The current culture.
        /// </summary>
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// States if this object has been disposed.
        /// </summary>
        private bool isDisposed;

        /// <summary>
        /// The current settings.
        /// </summary>
        private MonoAlphabeticSettingsObservableCollection settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MonoAlphabeticTransform"/> class.
        /// </summary>
        /// <param name="rgbKey">The key.</param>
        /// <param name="rgbIV">The initialization vector.</param>
        /// <param name="transformMode">the direction of transform.</param>
        internal MonoAlphabeticTransform(byte[] rgbKey, byte[] rgbIV, CipherTransformMode transformMode)
        {
            this.CanReuseTransform = false;
            this.CanTransformMultipleBlocks = false;
            this.InputBlockSize = BlockSize;
            this.OutputBlockSize = BlockSize;

            this.settings = MonoAlphabeticSettingsObservableCollection.Create(new List<byte>(rgbKey), new List<byte>(rgbIV));

            this.transformMode = transformMode;
        }

        /// <summary>
        /// Gets a value indicating whether the current transform can be reused.
        /// </summary>
        /// <returns>true if the current transform can be reused; otherwise, false.</returns>
        public bool CanReuseTransform
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether multiple blocks can be transformed.
        /// </summary>
        /// <returns>true if multiple blocks can be transformed; otherwise, false.</returns>
        public bool CanTransformMultipleBlocks
        {
            get;
        }

        /// <summary>
        /// Gets the input block size.
        /// </summary>
        /// <returns>The size of the input data blocks in bytes.</returns>
        public int InputBlockSize
        {
            get;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <returns>The Initialization Vector.</returns>
        public ICollection<byte> IV
        {
            get
            {
                return this.settings.IV;
            }
        }

        /// <summary>
        /// Gets the Key.
        /// </summary>
        /// <returns>The Key value.</returns>
        public ICollection<byte> Key
        {
            get
            {
                return this.settings.Key;
            }
        }

        /// <summary>
        /// Gets the output block size.
        /// </summary>
        /// <returns>The size of the output data blocks in bytes.</returns>
        public int OutputBlockSize
        {
            get;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public void Dispose(bool disposing)
        {
            if (this.isDisposed)
            {
                return;
            }

            // A call to Dispose(false) should only clean up native resources.
            // A call to Dispose(true) should clean up both managed and native resources.
            if (disposing)
            {
            }

            // Free native resources
            this.isDisposed = true;
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// Transforms the specified region of the input byte array and copies the resulting transform to the specified region of the output byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform (unicode).</param>
        /// <param name="inputOffset">The offset into the input byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the input byte array to use as data.</param>
        /// <param name="outputBuffer">The output to which to write the transform.</param>
        /// <param name="outputOffset">The offset into the output byte array from which to begin writing data.</param>
        /// <returns>The number of bytes written.</returns>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            byte[] bytesBlock = new byte[BlockSize];

            Array.Copy(inputBuffer, inputOffset, bytesBlock, 0, BlockSize);

            char[] charsBlock = Encoding.Unicode.GetChars(bytesBlock);
            char processedChar;

            ////// TODO: loop through chars?
            ////if (charsBlock.Length == 0)
            ////{
            ////    return 0;
            ////}

            char c = charsBlock[0];

            switch (this.transformMode)
            {
                default:
                case CipherTransformMode.Encrypt:
                    {
                        processedChar = this.Encipher(c);
                        break;
                    }

                case CipherTransformMode.Decrypt:
                    {
                        processedChar = this.Decipher(c);
                        break;
                    }
            }

            byte[] processedBytes = Encoding.Unicode.GetBytes(new char[] { processedChar });

            ////destinationIndex = 0;
            ////sourceIndex = 0;

            ////if (sourceIndex + BlockSize > processedBytes.GetLowerBound(0) + processedBytes.Length)
            ////{
            ////    throw new InvalidOperationException();
            ////}

            ////if (destinationIndex + BlockSize > outputBuffer.GetLowerBound(0) + outputBuffer.Length)
            ////{
            ////    throw new InvalidOperationException();
            ////}

            ////Array.Copy(processedBytes, sourceIndex, outputBuffer, destinationIndex, BlockSize);

            processedBytes.CopyTo(outputBuffer, outputOffset);

            return inputCount;
        }

        /// <summary>
        /// Transforms the specified region of the specified byte array.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the transform.</param>
        /// <param name="inputOffset">The offset into the byte array from which to begin using data.</param>
        /// <param name="inputCount">The number of bytes in the byte array to use as data.</param>
        /// <returns>The computed transform.</returns>
        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            return new byte[0];

            ////if (inputCount == 0)
            ////{
            ////    return new byte[0];
            ////}

            ////if (inputBuffer[0] == 0)
            ////{
            ////    return inputBuffer;
            ////}

            ////byte[] outputBuffer = new byte[0];
            ////this.TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            ////return outputBuffer;
        }

        /// <summary>
        /// Transforms an entire string.
        /// </summary>
        /// <param name="input">The input string to transform.</param>
        /// <returns>The transformed string.</returns>
        public string TransformString(string input)
        {
            StringBuilder output = new StringBuilder();
            foreach (char inputChar in input.ToCharArray())
            {
                switch (this.transformMode)
                {
                    default:
                    case CipherTransformMode.Encrypt:
                        {
                            output.Append(this.Encipher(inputChar));
                            break;
                        }

                    case CipherTransformMode.Decrypt:
                        {
                            output.Append(this.Decipher(inputChar));
                            break;
                        }
                }
            }

            return output.ToString();
        }

        ////public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        ////{
        ////    ////if (inputBuffer == null)
        ////    ////{
        ////    ////    throw new ArgumentNullException("inputBuffer");
        ////    ////}

        ////    ////if (inputOffset < 0)
        ////    ////{
        ////    ////    throw new ArgumentException("inputOffset cannot be negative.");
        ////    ////}

        ////    ////if (inputCount < 0)
        ////    ////{
        ////    ////    throw new ArgumentException("inputCount cannot be negative.");
        ////    ////}

        ////    ////if (inputOffset + inputCount > inputBuffer.GetLowerBound(0) + inputBuffer.Length)
        ////    ////{
        ////    ////    throw new ArgumentException("input buffer length exceeded.");
        ////    ////}

        ////    return ((ICryptoTransform)this).TransformFinalBlock(inputBuffer, inputOffset, inputCount);
        ////}
        /////// <summary>
        ///////
        /////// </summary>
        /////// <param name="inputBuffer">The input for which to compute the Caesar code.</param>
        /////// <param name="inputOffset"></param>
        /////// <param name="inputCount"></param>
        /////// <param name="outputBuffer"></param>
        /////// <param name="outputOffset"></param>
        /////// <returns></returns>
        ////int ICryptoTransform.TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        ////{
        ////    ////if (inputBuffer == null)
        ////    ////{
        ////    ////    throw new ArgumentNullException("inputBuffer");
        ////    ////}

        ////    ////if (inputOffset < 0)
        ////    ////{
        ////    ////    throw new ArgumentException("Value cannot be negative.", "inputOffset");
        ////    ////}

        ////    ////if (inputCount < 0)
        ////    ////{
        ////    ////    throw new ArgumentException("Value cannot be negative.", "inputCount");
        ////    ////}

        ////    ////if (outputBuffer == null)
        ////    ////{
        ////    ////    throw new ArgumentNullException("outputBuffer");
        ////    ////}

        ////    ////if (outputOffset < 0)
        ////    ////{
        ////    ////    throw new ArgumentException("Value cannot be negative.", "outputOffset");
        ////    ////}

        ////    ////if (inputOffset + inputCount > inputBuffer.GetLowerBound(0) + inputBuffer.Length)
        ////    ////{
        ////    ////    throw new ArgumentException("input buffer length exceeed.");
        ////    ////}

        ////    return ((IUsefulCryptoTransform)this).TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
        ////}

        /// <summary>
        /// Encipher a plaintext letter into an enciphered letter.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        internal char Encipher(char letter)
        {
            return this.settings[letter];
        }

        /// <summary>
        /// Deciphers a plaintext letter into an plaintext letter.
        /// </summary>
        /// <param name="letter">The enciphered letter to decipher.</param>
        /// <returns>The deciphered letter.</returns>
        internal char Decipher(char letter)
        {
            return this.settings.Reverse(letter);
        }
    }
}