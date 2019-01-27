// <copyright file="CipherViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Useful.Security.Cryptography;

    /// <summary>
    /// A viewmodel for ciphers.
    /// </summary>
    public class CipherViewModel : ViewModelBase
    {
        private readonly CipherService service;
        private string ciphertext = string.Empty;
        private string plaintext = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherViewModel"/> class.
        /// </summary>
        /// <param name="service">The cipher service.</param>
        public CipherViewModel(CipherService service)
        {
            this.service = service;
            WireCommands();
        }

        /// <summary>
        /// Gets a list of supported cipher names
        /// </summary>
        public IList<string> CipherNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (ICipher cipher in service.Repository.Read())
                {
                    names.Add(cipher.CipherName);
                }

                return names;
            }
        }

        /// <summary>
        /// Gets a list of supported ciphers.
        /// </summary>
        public IList<ICipher> Ciphers => service.Repository.Read();

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
                return service.Repository.CurrentItem;
            }

            set
            {
                if (service.Repository.CurrentItem == null)
                {
                    return;
                }

                service.Repository.SetCurrentItem(x => x == value);

                OnPropertyChanged(nameof(CurrentCipher));
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
            ciphertext = CurrentCipher.Encrypt(plaintext);
        }

        /// <summary>
        /// Used to decrypt the <see cref="Ciphertext"/> into <see cref="Plaintext"/>.
        /// </summary>
        public void Decrypt()
        {
            plaintext = CurrentCipher.Decrypt(ciphertext);
        }

        private void WireCommands()
        {
            EncryptCommand = new RelayCommand<object>(a => Encrypt(), p => !string.IsNullOrEmpty(plaintext));
            DecryptCommand = new RelayCommand<object>(a => Decrypt(), p => !string.IsNullOrEmpty(ciphertext));
        }
    }
}