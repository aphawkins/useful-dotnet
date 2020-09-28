// <copyright file="CipherViewModel.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Useful.Security.Cryptography;
    using Useful.Security.Cryptography.UI.Services;

    /// <summary>
    /// A viewmodel for ciphers.
    /// </summary>
    internal class CipherViewModel : ViewModelBase
    {
        private readonly CipherService _service;
        private string _ciphertext = string.Empty;
        private string _plaintext = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="CipherViewModel"/> class.
        /// </summary>
        /// <param name="service">The cipher service.</param>
        public CipherViewModel(CipherService service)
        {
            _service = service;
            _service.Repository.SetCurrentItem(x => x == _service.Repository.Read().ToList()[0]);

            WireCommands();
        }

        /// <summary>
        /// Gets a list of supported cipher names.
        /// </summary>
        public IEnumerable<string> CipherNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (ICipher cipher in _service.Repository.Read())
                {
                    names.Add(cipher.CipherName);
                }

                return names;
            }
        }

        /// <summary>
        /// Gets a list of supported ciphers.
        /// </summary>
        public IEnumerable<ICipher> Ciphers => _service.Repository.Read();

        /// <summary>
        /// Gets or sets the encrypted ciphertext.
        /// </summary>
        public string Ciphertext
        {
            get => _ciphertext;

            set
            {
                if (value == _ciphertext)
                {
                    return;
                }

                _ciphertext = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the cipher currently in scope.
        /// </summary>
        public ICipher CurrentCipher
        {
            get => _service.Repository.CurrentItem!;

            set
            {
                if (_service.Repository.CurrentItem == null)
                {
                    return;
                }

                _service.Repository.SetCurrentItem(x => x == value);

                NotifyPropertyChanged(nameof(CurrentCipher));
            }
        }

        /// <summary>
        /// Gets a property that can be used to link to a UI encrypt command.
        /// </summary>
        public ICommand? EncryptCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a property that can be used to link to a UI decrypt command.
        /// </summary>
        public ICommand? DecryptCommand
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the unencrypted plaintext.
        /// </summary>
        public string Plaintext
        {
            get => _plaintext;

            set
            {
                if (value == _plaintext)
                {
                    return;
                }

                _plaintext = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Used to encrypt the <see cref="Plaintext"/> into <see cref="Ciphertext"/>.
        /// </summary>
        public void Encrypt() => Ciphertext = CurrentCipher.Encrypt(_plaintext);

        /// <summary>
        /// Used to decrypt the <see cref="Ciphertext"/> into <see cref="Plaintext"/>.
        /// </summary>
        public void Decrypt() => Plaintext = CurrentCipher.Decrypt(_ciphertext);

        private void WireCommands()
        {
            EncryptCommand = new RelayCommand<object>(a => Encrypt(), p => !string.IsNullOrEmpty(_plaintext));
            DecryptCommand = new RelayCommand<object>(a => Decrypt(), p => !string.IsNullOrEmpty(_ciphertext));
        }
    }
}