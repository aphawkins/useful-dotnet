// <copyright file="CipherViewModelTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.UI.UnitTests.ViewModels
{
    using System.Collections.Generic;
    using Moq;
    using Useful.Security.Cryptography;
    using Useful.UI.Services;
    using Useful.UI.ViewModels;
    using Xunit;

    public class CipherViewModelTests
    {
        private readonly CipherService _cipherService;
        private CipherViewModel _viewModel;
        private Mock<ICipherRepository> _moqRepository;
        private Mock<ICipher> _moqCipher;

        public CipherViewModelTests()
        {
            _moqCipher = new Mock<ICipher>();
            _moqCipher.Setup(x => x.CipherName).Returns("MoqCipher");
            _moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            _moqCipher.Setup(x => x.Decrypt(It.IsAny<string>())).Returns("MoqPlaintext");
            _moqRepository = new Mock<ICipherRepository>();
            _moqRepository.Setup(x => x.Read()).Returns(() => new List<ICipher>() { _moqCipher.Object });
            _moqRepository.Setup(x => x.CurrentItem).Returns(() => _moqCipher.Object);
            _cipherService = new CipherService(_moqRepository.Object);
            _viewModel = new CipherViewModel(_cipherService);
        }

        [Fact]
        public void Initialization()
        {
            // Assert.NotNull(viewModel.CurrentCipher);
            // Assert.Equal("MoqCipher", viewModel.CurrentCipher.CipherName);
            // Assert.NotNull(viewModel.EncryptCommand);
            // viewModel.EncryptCommand.Execute(null);
        }

        [Fact]
        public void SubscribedPlaintextChanged()
        {
            string propertyChanged;
            _viewModel.PropertyChanged += (sender, e) => { propertyChanged = e.PropertyName; };
            _viewModel.Plaintext = string.Empty;
            propertyChanged = string.Empty;
            _viewModel.Plaintext = "MoqPlaintext";
            Assert.Equal(nameof(_viewModel.Plaintext), propertyChanged);
        }

        [Fact]
        public void CipherViewModelPlaintext()
        {
            string propertyChanged;
            _viewModel.PropertyChanged += (sender, e) => { propertyChanged = e.PropertyName; };
            _viewModel.Plaintext = string.Empty;
            propertyChanged = string.Empty;
            _viewModel.Plaintext = "MoqPlaintext";
            Assert.Equal(nameof(_viewModel.Plaintext), propertyChanged);
        }

        [Fact]
        public void CipherViewModelPlaintextNoEventSubscription()
        {
            string propertyChanged;
            _viewModel.Plaintext = string.Empty;
            propertyChanged = string.Empty;
            _viewModel.Plaintext = "MoqPlaintext";
            Assert.Equal(string.Empty, propertyChanged);
        }

        [Fact]
        public void CipherViewModelCiphertext()
        {
            string propertyChanged;
            _viewModel.PropertyChanged += (sender, e) => { propertyChanged = e.PropertyName; };
            _viewModel.Ciphertext = string.Empty;
            propertyChanged = string.Empty;
            _viewModel.Ciphertext = "MoqCiphertext";
            Assert.Equal(nameof(_viewModel.Ciphertext), propertyChanged);
        }

        [Fact]
        public void CipherViewModelCurrentCipher()
        {
            string propertyChanged;
            _viewModel.PropertyChanged += (sender, e) => { propertyChanged = e.PropertyName; };
            _viewModel.CurrentCipher = _moqCipher.Object;
            _viewModel.CurrentCipher = null;
            propertyChanged = string.Empty;
            _viewModel.CurrentCipher = _moqCipher.Object;
            Assert.Equal(nameof(_viewModel.CurrentCipher), propertyChanged);
        }

        [Fact]
        public void CipherViewModelEncrypt()
        {
            _viewModel.Plaintext = "MoqPlaintext";
            _viewModel.Encrypt();
            Assert.Equal("MoqCiphertext", _viewModel.Ciphertext);
        }

        [Fact]
        public void CipherViewModelEncryptCommand()
        {
            _viewModel.Plaintext = "MoqPlaintext";
            Assert.True(_viewModel.EncryptCommand.CanExecute(null));
            _viewModel.EncryptCommand.Execute(null);
            Assert.Equal("MoqCiphertext", _viewModel.Ciphertext);
        }

        [Fact]
        public void CipherViewModelEncryptCommandNotExecutable()
        {
            Assert.Equal(string.Empty, _viewModel.Plaintext);
            Assert.False(_viewModel.EncryptCommand.CanExecute(null));
        }

        [Fact]
        public void CipherViewModelDecrypt()
        {
            _viewModel.Ciphertext = "MoqCiphertext";
            _viewModel.Decrypt();
            Assert.Equal("MoqPlaintext", _viewModel.Plaintext);
        }

        [Fact]
        public void CipherViewModelDecryptCommand()
        {
            _viewModel.Ciphertext = "MoqCiphertext";
            Assert.True(_viewModel.DecryptCommand.CanExecute(null));
            _viewModel.DecryptCommand.Execute(null);
            Assert.Equal("MoqPlaintext", _viewModel.Plaintext);
        }

        [Fact]
        public void CipherViewModelDecryptCommandNotExecutable()
        {
            _viewModel.Ciphertext = string.Empty;
            Assert.False(_viewModel.DecryptCommand.CanExecute(null));
        }
    }
}