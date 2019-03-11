// <copyright file="SymmetricTransform{T,TU}.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class SymmetricTransform<T, TU> : ICryptoTransform
        where T : ICipher, new()
        where TU : ISymmetricCipherSettings, new()
    {
        private const int _blockSize = 2;  // 2 for Unicode, 1 for UTF8
        private readonly CipherTransformMode _transformMode;
        private readonly ICipher _cipher;
        private readonly ISymmetricCipherSettings _settings;
        private Encoding _encoding = new UnicodeEncoding();

        internal SymmetricTransform(byte[] rgbKey, byte[] iv, CipherTransformMode transformMode)
        {
            _settings = new TU(rgbKey, iv);
            _transformMode = transformMode;
            _cipher = new T();
        }

        public bool CanReuseTransform => true;

        public bool CanTransformMultipleBlocks => false;

        public int InputBlockSize => _blockSize;

        public int OutputBlockSize => _blockSize;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public static void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }

            // free native resources if there are any.
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (inputCount <= 0)
            {
                // Nothing to do
                return 0;
            }

            if (inputBuffer == null)
            {
                throw new ArgumentNullException(nameof(inputBuffer));
            }

            if (outputBuffer == null)
            {
                throw new ArgumentNullException(nameof(outputBuffer));
            }

            if (inputBuffer.Length < (inputOffset + inputCount))
            {
                throw new ArgumentException("Input buffer not long enough.", nameof(inputBuffer));
            }

            string inputString = new string(_encoding.GetChars(inputBuffer));
            string outputString;

            switch (_transformMode)
            {
                case CipherTransformMode.Encrypt:
                    {
                        outputString = _cipher.Encrypt(inputString);
                        break;
                    }

                case CipherTransformMode.Decrypt:
                    {
                        outputString = _cipher.Decrypt(inputString);
                        break;
                    }

                default:
                    {
                        throw new CryptographicException($"Unsupported transform mode {_transformMode}.");
                    }
            }

            byte[] outputBytes = _encoding.GetBytes(outputString);
            Array.Copy(outputBytes, 0, outputBuffer, 0, OutputBlockSize);
            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer[0] == 0)
            {
                return inputBuffer;
            }

            byte[] outputBuffer = new byte[inputBuffer.Length];
            if (TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0) > 0)
            {
                return outputBuffer;
            }

            return Array.Empty<byte>();
        }
    }
}