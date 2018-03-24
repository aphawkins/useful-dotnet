// <copyright file="CipherViewModel.cs" company="APH Company">
// Copyright (c) APH Company. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Useful.UI.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Security.Cryptography;

    /// <summary>
    /// A viewmodel for ciphers.
    /// </summary>
    public class CipherViewModel : ViewModelBase
    {
        private string ciphertext = string.Empty;
        private ICipher currentCipher;
        private string plaintext = string.Empty;
        private IRepository<ICipher> repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherViewModel"/> class.
        /// </summary>
        /// <param name="repository">The repository holding the ciphers.</param>
        public CipherViewModel(IRepository<ICipher> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException();
            Ciphers = this.repository.Read();
            currentCipher = Ciphers[0];
            WireCommands();
        }

        private CipherViewModel()
                    : this(CipherRepository.Create())
        {
            // HACK: Default constructor required for the web project.
            // Construct the repository until dependency injection is working.
        }

        /// <summary>
        /// Gets a list of supported cipher names
        /// </summary>
        public IList<string> CipherNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (ICipher cipher in Ciphers)
                {
                    names.Add(cipher.CipherName);
                }

                return names;
            }
        }

        /// <summary>
        /// Gets a list of supported ciphers.
        /// </summary>
        public IList<ICipher> Ciphers
        {
            get;
        }

        /// <summary>
        /// Gets or sets the encrypted ciphertext.
        /// </summary>
        public string Ciphertext
        {
            get
            {
                return ciphertext;
            }

            set
            {
                if (value == ciphertext)
                {
                    return;
                }

                ciphertext = value;
                OnPropertyChanged(nameof(Ciphertext));
            }
        }

        /// <summary>
        /// Gets or sets the cipher currently in scope.
        /// </summary>
        public ICipher CurrentCipher
        {
            get
            {
                return currentCipher;
            }

            set
            {
                if (currentCipher == value)
                {
                    return;
                }

                currentCipher = value;
                OnPropertyChanged(nameof(CurrentCipher));
                OnPropertyChanged(nameof(CurrentCipherName));
            }
        }

        /// <summary>
        /// Gets or sets the cipher currently in scope's name.
        /// </summary>
        public string CurrentCipherName
        {
            get
            {
                return currentCipher.CipherName;
            }

            set
            {
                if (value == null)
                {
                    CurrentCipher = null;
                    return;
                }

                if (currentCipher?.CipherName == value)
                {
                    return;
                }

                // PropertyChanged is raised by CurrentCipher
                CurrentCipher = Ciphers.First(x => x.CipherName == value);
            }
        }

        /// <summary>
        /// Gets a property that can be used to link to a UI encrypt command.
        /// </summary>
        public ICommand EncryptCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a property that can be used to link to a UI decrypt command.
        /// </summary>
        public ICommand DecryptCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the unencrypted plaintext.
        /// </summary>
        public string Plaintext
        {
            get
            {
                return plaintext;
            }

            set
            {
                if (value == plaintext)
                {
                    return;
                }

                plaintext = value;
                OnPropertyChanged(nameof(Plaintext));
            }
        }

        /// <summary>
        /// Used to encrypt the <see cref="Plaintext"/> into <see cref="Ciphertext"/>.
        /// </summary>
        public void Encrypt()
        {
            if (CurrentCipher != null)
            {
                Ciphertext = CurrentCipher.Encrypt(Plaintext);
            }
        }

        /// <summary>
        /// Used to decrypt the <see cref="Ciphertext"/> into <see cref="Plaintext"/>.
        /// </summary>
        public void Decrypt()
        {
            if (CurrentCipher != null)
            {
                Plaintext = CurrentCipher.Decrypt(Ciphertext);
            }
        }

        private void WireCommands()
        {
            EncryptCommand = new RelayCommand<object>(a => Encrypt(), p => !string.IsNullOrEmpty(plaintext));
            DecryptCommand = new RelayCommand<object>(a => Decrypt(), p => !string.IsNullOrEmpty(ciphertext));
        }
    }
}