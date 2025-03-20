// Copyright (c) Andrew Hawkins. All rights reserved.

using Moq;
using Useful.Security.Cryptography.UI.Controllers;
using Useful.Security.Cryptography.UI.ViewModels;
using Useful.Security.Cryptography.UI.Views;
using Xunit;

namespace Useful.Security.Cryptography.UI.Tests.Controllers;

public class CipherControllerTests
{
    private readonly Mock<ICipher> _moqCipher;
    private readonly Mock<IRepository<ICipher>> _moqRepository;
    private readonly Mock<ICipherView> _moqView;
    private readonly Mock<ICipherSettingsViewModel> _moqSettings;
    private readonly Mock<ICipherSettingsView> _moqSettingsView;
    private readonly CipherController _controller;

    public CipherControllerTests()
    {
        _moqSettings = new Mock<ICipherSettingsViewModel>();
        _moqSettingsView = new Mock<ICipherSettingsView>();
        _moqCipher = new Mock<ICipher>();
        _moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
        _moqCipher.Setup(x => x.Encrypt(It.IsAny<string>())).Returns("MoqCiphertext");
        _moqRepository = new Mock<IRepository<ICipher>>();
        _moqRepository.Setup(x => x.Read()).Returns(() => [_moqCipher.Object]);
        _moqRepository.Setup(x => x.CurrentItem).Returns(() => _moqCipher.Object);
        _moqView = new Mock<ICipherView>();
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
        _controller.SelectCipher(_moqCipher.Object, _moqSettings.Object, _moqSettingsView.Object);
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
