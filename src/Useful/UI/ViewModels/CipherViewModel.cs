namespace Useful.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Security.Cryptography;

    public class CipherViewModel : ViewModelBase
    {
        private ICipher _currentCipher;
        private CipherRepository _repository;

        private ICommand encryptCommand;

        private string plaintext = string.Empty;
        private string ciphertext = string.Empty;

        public CipherViewModel()
        {
            this._repository = new CipherRepository();
            this.Ciphers = _repository.GetCiphers();
            this._currentCipher = this.Ciphers[0];

            WireCommands();
        }

        private void WireCommands()
        {
            EncryptCommand = new RelayCommand(Encrypt);
        }

        public RelayCommand EncryptCommand
        {
            get;
            private set;
        }

        public List<ICipher> Ciphers
        {
            get;
            set;
        }

        public List<string> CipherNames
        {
            get
            {
                List<string> names = new List<string>();
                this.Ciphers.ForEach(x => names.Add(x.CipherName));

                return names;
            }
        }

        public ICipher CurrentCipher
        {
            get
            {
                return _currentCipher;
            }

            set
            {
                if (_currentCipher != value)
                {
                    _currentCipher = value;
                    OnPropertyChanged(nameof(CurrentCipher));
                    OnPropertyChanged(nameof(CurrentCipherName));
                    EncryptCommand.IsEnabled = true;
                }
            }
        }

        public string CurrentCipherName
        {
            get
            {
                return this._currentCipher.CipherName;
            }

            set
            {
                if (this._currentCipher.CipherName != value)
                {
                    this.CurrentCipher = this.Ciphers.First(x => x.CipherName == value);
                }
            }
        }

        public void Encrypt()
        {
            this.Ciphertext = _repository.Encrypt(CurrentCipher, this.Plaintext);
        }

        public string Plaintext
        {
            get { return plaintext; }
            set
            {
                if (value != plaintext)
                {
                    plaintext = value;
                    OnPropertyChanged(nameof(Plaintext));
                }
            }
        }

        public string Ciphertext
        {
            get { return ciphertext; }
            set
            {
                if (value != ciphertext)
                {
                    ciphertext = value;
                    OnPropertyChanged(nameof(Ciphertext));
                }
            }
        }
    }
}