using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Useful.Security.Cryptography
{
	internal sealed class EnigmaTransform : ICryptoTransform
	{
        //private const int letterNum = 26;
        private const int blockSize = 2;

        private readonly CipherTransformMode transformMode;
        private readonly byte[] key;
		private readonly byte[] iv;

        private EnigmaSettings settings;

        private CultureInfo culture = CultureInfo.InvariantCulture;

		#region Constructor
		internal EnigmaTransform(byte[] rgbKey, byte[] rgbIV, CipherTransformMode transformMode)
		{
            Debug.Assert(rgbKey != null);
            Debug.Assert(rgbIV != null);

			this.key = rgbKey;
			this.iv = rgbIV;
            this.settings = new EnigmaSettings(rgbKey, rgbIV);
			this.transformMode = transformMode;
		}
		#endregion

		#region ICryptoTransform Members

		public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
		{
			if (inputBuffer[0] == 0)
			{
				return inputBuffer;
			}
			else
			{
				byte[] outputBuffer = new byte[0];
				TransformBlock(inputBuffer, inputOffset, inputCount, outputBuffer, 0);
				return outputBuffer;
			}
		}

		public bool CanReuseTransform
		{
			get
			{
				return true;
			}
		}

		public int InputBlockSize
		{
			get
			{
				return blockSize;
			}
		}

		public int OutputBlockSize
		{
			get
			{
				return blockSize;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="inputBuffer">The input for which to compute the Enigma code.</param>
		/// <param name="inputOffset"></param>
		/// <param name="inputCount"></param>
		/// <param name="outputBuffer"></param>
		/// <param name="outputOffset"></param>
		/// <returns></returns>
		public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer, int outputOffset)
		{
			if (inputBuffer == null)
			{
				throw new ArgumentNullException("inputBuffer");
			}
			if (outputBuffer == null)
			{
				throw new ArgumentNullException("outputBuffer");
			}
			if (inputBuffer.Length < (inputOffset + inputCount))
			{
				throw new ArgumentException(Resource.InputBufferLength, "inputBuffer");
			}

            int blocksTransformed;

			try
			{
				switch (this.transformMode)
				{
					case (CipherTransformMode.Encrypt):
					case (CipherTransformMode.Decrypt):
						{

							byte[] plaintextBytes = new byte[blockSize];
							Array.Copy(inputBuffer, inputOffset, plaintextBytes, 0, blockSize);
							char[] plaintextChars = Encoding.Unicode.GetChars(plaintextBytes);

                            if (!this.settings.KeyboardLetters.Contains(plaintextChars[0]))
                            {
                                blocksTransformed = 0;
                                break;
                            }
                            this.settings.AdvanceRotorsToPosition(this.settings.Counter);
							char encipheredChar = Encipher(plaintextChars[0]);

							byte[] encipheredBytes = Encoding.Unicode.GetBytes(new char[] { encipheredChar });
							Array.Copy(encipheredBytes, 0, outputBuffer, 0, blockSize);

                            blocksTransformed = inputCount;
							break;
						}
					default:
						{
							throw new CryptographicException(string.Format(this.culture, Resource.UnsupportedTransformMode, this.transformMode.ToString()));
						}
				}
                return blocksTransformed;
			}
			catch
			{
				throw;
			}
		}

		public bool CanTransformMultipleBlocks
		{
			get
			{
				return false;
			}
		}

		#endregion



		#region Encipher
		/// <summary>
		/// Encipher a plaintext letter into an enciphered letter.  Decipher works in the same way as encipher.
		/// </summary>
		/// <param name="letter">The plaintext letter to encipher.</param>
		/// <returns>The enciphered letter.</returns>
		private char Encipher(char letter)
		{
			try
			{
                Debug.Assert(this.settings.KeyboardLetters.Contains(letter));

				// Advance the rotors one position
				this.settings.AdvanceRotors(1);

				// Plugboard
				char newLetter = this.settings.Plugboard.Encipher(letter);

				//Go thru the rotors forwards
				foreach (EnigmaRotorNumber rotorNumber in this.settings.RotorOrder.Values)
				{
                    newLetter = this.settings.GetRotor(rotorNumber).Forward(newLetter);
				}

				// Go thru the relector
				if (this.settings.HasReflector)
				{
					newLetter = this.settings.Reflector.Reflect(newLetter);
				}

				//Go thru the rotors backwards
				foreach (EnigmaRotorNumber rotorNumber in this.settings.ReverseRotorOrder.Values)
				{
					newLetter = this.settings.GetRotor(rotorNumber).Backward(newLetter);
				}

				newLetter = this.settings.Plugboard.Decipher(newLetter);

				// Letter cannot encrypt to itself.
                Debug.Assert(char.ToUpper(letter, this.culture) != char.ToUpper(newLetter, this.culture), "Letter cannot encrypt to itself.");

				return newLetter;
			}
			catch
			{
				throw;
			}
		}
		#endregion

		#region IDisposable Members

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		public void Dispose(bool disposing)
		{
			// A call to Dispose(false) should only clean up native resources. 
			// A call to Dispose(true) should clean up both managed and native resources.
			if (disposing)
			{
				// Dispose managed resources
			}
			// Free native resources
		}

		/// <summary>
		/// Releases all resources used by this object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
