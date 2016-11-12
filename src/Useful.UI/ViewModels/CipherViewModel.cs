namespace Useful.UI.ViewModels
{
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
            this.repository = repository;
            this.Ciphers = this.repository.Read();
            this.currentCipher = this.Ciphers[0];
            this.WireCommands();
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
                this.Ciphers.ForEach(x => names.Add(x.CipherName));

                return names;
            }
        }

        /// <summary>
        /// Gets a list of supported ciphers.
        /// </summary>
        public List<ICipher> Ciphers
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
                return this.ciphertext;
            }

            set
            {
                if (value == this.ciphertext)
                {
                    return;
                }

                this.ciphertext = value;
                this.OnPropertyChanged(nameof(this.Ciphertext));
            }
        }

        /// <summary>
        /// Gets or sets the cipher currently in scope.
        /// </summary>
        public ICipher CurrentCipher
        {
            get
            {
                return this.currentCipher;
            }

            set
            {
                if (this.currentCipher == value)
                {
                    return;
                }

                this.currentCipher = value;
                this.OnPropertyChanged(nameof(this.CurrentCipher));
                this.OnPropertyChanged(nameof(this.CurrentCipherName));
            }
        }

        /// <summary>
        /// Gets or sets the cipher currently in scope's name.
        /// </summary>
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

        /// <summary>
        /// Gets a property that can be used to link to a UI encrypt command.
        /// </summary>
        public ICommand EncryptCommand
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
                return this.plaintext;
            }

            set
            {
                if (value == this.plaintext)
                {
                    return;
                }

                this.plaintext = value;
                this.OnPropertyChanged(nameof(this.Plaintext));
            }
        }

        /// <summary>
        /// Used to encrypt the <see cref="Plaintext"/> into <see cref="Ciphertext"/>.
        /// </summary>
        public void Encrypt()
        {
            if (this.CurrentCipher != null)
            {
                this.Ciphertext = this.CurrentCipher.Encrypt(this.Plaintext);
            }
        }

        private void WireCommands()
        {
            this.EncryptCommand = new RelayCommand<object>(a => this.Encrypt(), p => !string.IsNullOrEmpty(this.plaintext));
        }
    }
}