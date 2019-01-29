// <copyright file="CipherControllerTests.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Useful.UI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using Moq;
    using Useful.Security.Cryptography;
    using Useful.UI.Controllers;
    using Useful.UI.Views;
    using Xunit;

    public class CipherControllerTests
    {
        private Mock<ICipher> _moqCipher;
        private Mock<IRepository<ICipher>> _moqRepository;
        private Mock<ICipherView> _moqView;
        private Mock<ICipherSettingsView> _moqSettingsView;
        private CipherController _controller;

        public CipherControllerTests()
        {
            _moqCipher = new Mock<ICipher>();
            _moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
            _moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
            _moqRepository = new Mock<IRepository<ICipher>>();
            _moqRepository.Setup(x => x.Read()).Returns(() => new List<ICipher>() { _moqCipher.Object });
            _moqRepository.Setup(x => x.CurrentItem).Returns(() => _moqCipher.Object);
            _moqView = new Mock<ICipherView>();
            _moqSettingsView = new Mock<ICipherSettingsView>();
            _controller = new CipherController(_moqRepository.Object, _moqView.Object);
        }

        [Fact]
        public void Initialized()
        {
            _controller.LoadView();
            _moqView.Verify(x => x.Initialize());
        }

        [Fact]
        public void SelectCipher()
        {
            _controller.SelectCipher(_moqCipher.Object, _moqSettingsView.Object);
            _moqRepository.Verify(x => x.SetCurrentItem(It.IsAny<Func<ICipher, bool>>()));
        }

        [Theory]
        [InlineData("Hello World!")]
        public void Encrypt(string plaintext)
        {
            _controller.Encrypt(plaintext);
            _moqView.Verify(x => x.ShowPlaintext(It.IsAny<string>()));
            _moqView.Verify(x => x.ShowCiphertext(It.IsAny<string>()));
        }

        [Theory]
        [InlineData("Hello World!")]
        public void Decrypt(string ciphertext)
        {
            _controller.Encrypt(ciphertext);
            _moqView.Verify(x => x.ShowPlaintext(It.IsAny<string>()));
            _moqView.Verify(x => x.ShowCiphertext(It.IsAny<string>()));
        }
    }
}