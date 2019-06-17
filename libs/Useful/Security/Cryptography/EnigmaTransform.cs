namespace Useful.Security.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// The Enigma algorithm.
    /// </summary>
    internal sealed class EnigmaTransform : IUsefulCryptoTransform
    {
        /// <summary>
        /// The block size.
        /// </summary>
        private const int BlockSize = 2;

        /// <summary>
        /// The transform mode.
        /// </summary>
        private readonly CipherTransformMode _TransformMode;

        /// <summary>
        /// The settings for this cipher.
        /// </summary>
        private EnigmaSettings _settings;

        /// <summary>
        /// The plugboard settings.
        /// </summary>
        private MonoAlphabeticTransform _plugboard;

        /// <summary>
        /// Defines which rotor is in which position.
        /// </summary>
        private Dictionary<EnigmaRotorPosition, EnigmaRotor> _rotors;

        /// <summary>
        /// Defines the reverse order for which rotor is in which position.
        /// </summary>
        private SortedDictionary<EnigmaRotorPosition, EnigmaRotor> _reverseRotorOrder;

        /// <summary>
        /// The reflector for this transform.
        /// </summary>
        private EnigmaReflector _reflector;

        /// <summary>
        /// Has this object been disposed?.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnigmaTransform"/> class.
        /// </summary>
        /// <param name="rgbKey">The transform's Key.</param>
        /// <param name="rgbIV">The transform's Initialization Vector.</param>
        /// <param name="transformMode">The direction of encryption.</param>
        internal EnigmaTransform(byte[] rgbKey, byte[] rgbIV, CipherTransformMode transformMode)
        {
            Contract.Requires(rgbKey != null);
            Contract.Requires(rgbIV != null);

            this.settings = new EnigmaSettings(rgbKey, rgbIV);
            _TransformMode = transformMode;

            this.CanReuseTransform = true;
            this.CanTransformMultipleBlocks = false;
            this.InputBlockSize = BlockSize;
            this.OutputBlockSize = BlockSize;

            // Plugboard
            _plugboard = new MonoAlphabeticTransform(this.settings.Plugboard.Key, this.settings.Plugboard.IV, _TransformMode);

            // Rotors
            _rotors = new Dictionary<EnigmaRotorPosition, EnigmaRotor>();
            EnigmaRotor previousRotor = null;
            foreach (EnigmaRotorPosition rotorPosition in this.settings.Rotors.AllowedRotorPositions)
            {
                EnigmaRotor rotor = this.settings.Rotors[rotorPosition];
                _rotors.Add(rotorPosition, rotor);
                rotor.RotorAdvanced += EnigmaTransform_RotorAdvanced;

                if (previousRotor != null)
                {
                    previousRotor.RotorAdvanced += rotor.PreviousRotorAdvanced;
                }

                previousRotor = rotor;
            }

            _reverseRotorOrder = new SortedDictionary<EnigmaRotorPosition, EnigmaRotor>(_rotors, new EnigmaRotorPositionSorter(SortOrder.Descending));

            // Initial rotor position
            if (this.settings.Rotors.Count > 0)
            {
                foreach (EnigmaRotorPosition rotorPosition in this.settings.Rotors.AllowedRotorPositions)
                {
                    _rotors[rotorPosition].RingPosition = this.settings.Rotors[rotorPosition].RingPosition;
                    _rotors[rotorPosition].CurrentSetting = this.settings.Rotors[rotorPosition].CurrentSetting;
                }
            }

            // Reflector
            _reflector = EnigmaReflector.Create(this.settings.ReflectorNumber);
        }

        /// <summary>
        /// Gets a value indicating whether this transform be reused.
        /// </summary>
        public bool CanReuseTransform { get; private set; }

        /// <summary>
        /// Gets the Input block size.
        /// </summary>
        public int InputBlockSize { get; private set; }

        /// <summary>
        /// Gets the output block size.
        /// </summary>
        public int OutputBlockSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this can transform multiple blocks.
        /// </summary>
        public bool CanTransformMultipleBlocks { get; private set; }

        /// <summary>
        /// Gets the Key.
        /// </summary>
        /// <returns>The Key value.</returns>
        public byte[] GetKey()
        {
            // Debug.Assert(this.m_settings.GetKey() != null);
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Assert(this.settings.Key != null);

            return this.settings.Key;
        }

        /// <summary>
        /// Gets the Initialization Vector.
        /// </summary>
        /// <returns>The Initialization Vector.</returns>
        public byte[] GetIV()
        {
            Contract.Ensures(Contract.Result<byte[]>() != null);
            Contract.Assert(this.settings.IV != null);

            return this.settings.IV;
        }

        /// <summary>
        /// Transforms the final block.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The offset to write from.</param>
        /// <param name="inputCount">The number of bytes to transform.</param>
        /// <returns>The transformed final block.</returns>
        byte[] IUsefulCryptoTransform.TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputCount == 0)
            {
                return new byte[0];
            }

            if (inputBuffer[0] == 0)
            {
                return inputBuffer;
            }

            byte[] outputBuffer = new byte[0];
            ((IUsefulCryptoTransform)this).TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
            return outputBuffer;
        }

        /// <summary>
        /// Transforms the final block.
        /// </summary>
        /// <param name="inputBuffer">The input buffer.</param>
        /// <param name="inputOffset">The offset to write from.</param>
        /// <param name="inputCount">The number of bytes to transform.</param>
        /// <returns>The transformed final block.</returns>
        byte[] ICryptoTransform.TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException(nameof(inputBuffer));
            }

            if (inputOffset < 0)
            {
                throw new ArgumentException("inputOffset cannot be negative.");
            }

            if (inputCount < 0)
            {
                throw new ArgumentException("inputCount cannot be negative.");
            }

            if (inputOffset + inputCount > inputBuffer.GetLowerBound(0) + inputBuffer.Length)
            {
                throw new ArgumentException("input buffer length exceeded.");
            }

            return ((IUsefulCryptoTransform)this).TransformFinalBlock(inputBuffer, inputOffset, inputCount);
        }

        /// <summary>
        /// Transforms a block of input.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the Enigma code.</param>
        /// <param name="inputOffset">The position to start writing in the input buffer.</param>
        /// <param name="inputCount">The number of bytes to write.</param>
        /// <param name="outputBuffer">The output for which to compute the Enigma code into.</param>
        /// <param name="outputOffset">The position to start writing in the input buffer.</param>
        /// <returns>The number of transformed blocks.</returns>
        int IUsefulCryptoTransform.TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            int blocksTransformed;

            // Encrypt and Decrypt work the same way
            byte[] plaintextBytes = new byte[BlockSize];
            Array.Copy(inputBuffer, inputOffset, plaintextBytes, 0, BlockSize);
            char[] plaintextChars = Encoding.Unicode.GetChars(plaintextBytes);

            if (Letters.IsCleanable(this.settings.AllowedLetters, plaintextChars[0]))
            {
                char encipheredChar = this.Encipher(plaintextChars[0]);

                byte[] encipheredBytes = Encoding.Unicode.GetBytes(new char[] { encipheredChar });
                Array.Copy(encipheredBytes, 0, outputBuffer, 0, BlockSize);
                blocksTransformed = inputCount;
            }
            else
            {
                blocksTransformed = 0;
            }

            return blocksTransformed;
        }

        /// <summary>
        /// Transforms a block of input.
        /// </summary>
        /// <param name="inputBuffer">The input for which to compute the Enigma code.</param>
        /// <param name="inputOffset">The position to start writing in the input buffer.</param>
        /// <param name="inputCount">The number of bytes to write.</param>
        /// <param name="outputBuffer">The output for which to compute the Enigma code into.</param>
        /// <param name="outputOffset">The position to start writing in the input buffer.</param>
        /// <returns>The number of transformed blocks.</returns>
        int ICryptoTransform.TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
        {
            if (inputBuffer == null)
            {
                throw new ArgumentNullException(nameof(inputBuffer));
            }

            if (inputOffset < 0)
            {
                throw new ArgumentException("Value cannot be negative.", nameof(inputOffset));
            }

            if (inputCount < 0)
            {
                throw new ArgumentException("Value cannot be negative.", nameof(inputCount));
            }

            if (outputBuffer == null)
            {
                throw new ArgumentNullException(nameof(outputBuffer));
            }

            if (outputOffset < 0)
            {
                throw new ArgumentException("Value cannot be negative.", nameof(outputOffset));
            }

            if (inputOffset + inputCount > inputBuffer.GetLowerBound(0) + inputBuffer.Length)
            {
                throw new ArgumentException("input buffer length exceeed.");
            }

            return ((IUsefulCryptoTransform)this).TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, outputOffset);
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
                if (Letters.IsCleanable(this.settings.AllowedLetters, inputChar))
                {
                    // Encrypt and Decrypt work the same way
                    output.Append(this.Encipher(inputChar));
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        public void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // A call to Dispose(false) should only clean up native resources.
            // A call to Dispose(true) should clean up both managed and native resources.
            if (disposing)
            {
                // Dispose managed resources
                if (_plugboard != null)
                {
                    _plugboard.Dispose();
                }

                if (_reflector != null)
                {
                    _reflector.Dispose();
                }
            }

            // Free native resources
            _disposed = true;
        }

        /// <summary>
        /// Releases all resources used by this object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnigmaTransform_RotorAdvanced(object sender, EnigmaRotorAdvanceEventArgs e)
        {
            Contract.Requires(this.settings != null);
            Contract.Requires(_rotors != null);

            // Find which rotor has advanced
            EnigmaRotorPosition rotorPosition = this.settings.Rotors.AllowedRotorPositions.FirstOrDefault(x => this.settings.Rotors[x].RotorNumber == e.RotorNumber);
            this.settings.AdvanceRotor(rotorPosition, _rotors[rotorPosition].CurrentSetting);
        }

        /// <summary>
        /// Advances the rotors by a specified number of positions.
        /// </summary>
        /// <param name="numberOfPositions">The number of positions to move the rotors.</param>
        private void AdvanceRotors(int numberOfPositions)
        {
            Contract.Requires(_rotors != null);

            // Advance the fastest rotor
            EnigmaRotor rotor = _rotors[EnigmaRotorPosition.Fastest];

            for (int i = 0; i < numberOfPositions; i++)
            {
                rotor.AdvanceRotor();
            }
        }

        /////// <summary>
        /////// Advances the rotors by a specified number of positions in a reverse direction.
        /////// </summary>
        /////// <param name="numberOfPositions">The number of positions to move the rotors.</param>
        ////private void ReverseAdvanceRotors(int numberOfPositions)
        ////{
        ////    // Reverse the fastest rotor
        ////    EnigmaRotor rotor = this.rotors[EnigmaRotorPosition.Fastest];

        ////    if (!rotor.CanTurn)
        ////    {
        ////        return;
        ////    }

        ////    for (int i = 0; i < numberOfPositions; i++)
        ////    {
        ////        rotor.ReverseRotor();
        ////    }
        ////}

        /// <summary>
        /// Encipher a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
        /// </summary>
        /// <param name="letter">The plaintext letter to encipher.</param>
        /// <returns>The enciphered letter.</returns>
        private char Encipher(char letter)
        {
            Contract.Requires(this.settings != null);
            Contract.Requires(_rotors != null);

            char newLetter;

            if (!Letters.IsCleanable(this.settings.AllowedLetters, letter))
            {
                return letter;
            }

            newLetter = Letters.Clean(this.settings.AllowedLetters, letter);

            Contract.Assert(this.settings.AllowedLetters.Contains(newLetter));

            // Ensure all the rotors are set correctly
            ////this.AdvanceRotorsToPosition(this.settings.Counter);

            // Advance the rotors one position
            AdvanceRotors(1);

            // Plugboard
            newLetter = _plugboard.Encipher(newLetter);

            // Go thru the rotors forwards
            foreach (EnigmaRotorPosition rotorPosition in this.settings.Rotors.AllowedRotorPositions)
            {
                newLetter = _rotors[rotorPosition].Forward(newLetter);
            }

            // Go thru the relector
            newLetter = _reflector.Reflect(newLetter);

            // Go thru the rotors backwards
            foreach (EnigmaRotorPosition rotorPosition in _reverseRotorOrder.Keys)
            {
                newLetter = _reverseRotorOrder[rotorPosition].Backward(newLetter);
            }

            newLetter = _plugboard.Decipher(newLetter);

            // Letter cannot encrypt to itself.
            Debug.Assert(Letters.Clean(this.settings.AllowedLetters, letter) != Letters.Clean(this.settings.AllowedLetters, newLetter), "Letter cannot encrypt to itself.");

            return newLetter;
        }
    }
}