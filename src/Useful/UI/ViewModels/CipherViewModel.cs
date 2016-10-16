namespace Useful.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;
    using Security.Cryptography;

    public class CipherViewModel : ViewModelBase
    {
        private ICipher currentCipher;
        private ICipherRepository repository;
        private ICommand encryptCommand;
        private string plaintext = string.Empty;
        private string ciphertext = string.Empty;

        public CipherViewModel(ICipherRepository repository)
        {
            this.repository = repository;
            this.Ciphers = repository.GetCiphers();
            this.currentCipher = this.Ciphers[0];

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

        public IList<string> CipherNames
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
                EncryptCommand.IsEnabled = true;
            }
        }

        public string CurrentCipherName
        {
            get
            {
                return this.currentCipher.CipherName;
            }

            set
            {
                if (value == null)
                {
                    this.CurrentCipher = null;
                    return;
                }
                if (this.currentCipher?.CipherName == value)
                {
                    return;
                }

                // PropertyChanged is raised by CurrentCipher
                this.CurrentCipher = this.Ciphers.First(x => x.CipherName == value);
            }
        }

        public void Encrypt()
        {
            this.Ciphertext = this.CurrentCipher.Encrypt(this.Plaintext);
        }

        public string Plaintext
        {
            get
            {
                return this.plaintext;
            }
            set
            {
                if (value == plaintext)
                {
                    return;
                }

                this.plaintext = value;
                OnPropertyChanged(nameof(Plaintext));
            }
        }

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
    }
}