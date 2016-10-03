namespace Useful.UI.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Input;
    using Security.Cryptography;

    public class CipherViewModel : ViewModelBase
    {
        private List<ICipher> _ciphers;
        private ICipher _currentCipher;
        private CipherRepository _repository;

        private ICommand encryptCommand;

        private string plaintext;


        public CipherViewModel()
        {
            _repository = new CipherRepository();
            _ciphers = _repository.GetCiphers();

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
            get { return _ciphers; }
            set { _ciphers = value; }
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
                    EncryptCommand.IsEnabled = true;
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

        private string ciphertext;

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