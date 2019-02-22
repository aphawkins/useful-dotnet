// <copyright file="CaesarTransform.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    internal sealed class CaesarTransform : ICryptoTransform
    {
        private const int _blockSize = 2;
        private readonly CipherTransformMode _transformMode;
        private readonly CaesarCipher _cipher;
        private readonly CaesarCipherSymmetricSettings _settings;

        internal CaesarTransform(byte[] rgbKey, CipherTransformMode transformMode)
        {
            _settings = new CaesarCipherSymmetricSettings(rgbKey, null);
            _transformMode = transformMode;
            _cipher = new CaesarCipher(_settings);
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

            byte[] inputBytes = new byte[_blockSize];
            Array.Copy(inputBuffer, inputOffset, inputBytes, 0, _blockSize);
            char[] inputChars = Encoding.Unicode.GetChars(inputBytes);
            char cipheredChar;

            switch (_transformMode)
            {
                case CipherTransformMode.Encrypt:
                    {
                        cipheredChar = _cipher.Encrypt($"{inputChars[0]}")[0];
                        break;
                    }

                case CipherTransformMode.Decrypt:
                    {
                        cipheredChar = _cipher.Decrypt($"{inputChars[0]}")[0];
                        break;
                    }

                default:
                    {
                        throw new CryptographicException($"Unsupported transform mode {_transformMode}.");
                    }
            }

            byte[] cipheredBytes = Encoding.Unicode.GetBytes(new char[] { cipheredChar });
            Array.Copy(cipheredBytes, 0, outputBuffer, 0, _blockSize);

            return inputCount;
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer[0] == 0)
            {
                return inputBuffer;
            }

            byte[] outputBuffer = new byte[inputBuffer.Length];
            int bytesWritten = TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            if (bytesWritten > 0)
            {
                return outputBuffer;
            }

            return Array.Empty<byte>();
        }
    }
}