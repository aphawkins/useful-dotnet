// <copyright file="CipherRepositoryTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography.Tests
{
    using System.Linq;
    using Moq;
    using Useful.Security.Cryptography;
    using Xunit;

    public class CipherRepositoryTests
    {
        private readonly Mock<ICipher> _moqCipher;

        public CipherRepositoryTests()
        {
            _moqCipher = new Mock<ICipher>();
            _moqCipher.Setup(x => x.CipherName).Returns("MoqCipherName");
        }

        [Fact]
        public void RepositoryCreate()
        {
            CipherRepository repository = new CipherRepository();
            int count = repository.Read().Count();
            repository.Create(_moqCipher.Object);
            Assert.Equal(count + 1, repository.Read().Count());
        }

        [Fact]
        public void RepositoryRead()
        {
            CipherRepository repository = new CipherRepository();
            Assert.Empty(repository.Read());
        }

        [Fact]
        public void RepositoryUpdate()
        {
            CipherRepository repository = new CipherRepository();
            int count = repository.Read().Count();
            repository.Update(_moqCipher.Object);
            Assert.Equal(count, repository.Read().Count());
        }

        [Fact]
        public void RepositoryDelete()
        {
            CipherRepository repository = new CipherRepository();
            repository.Create(_moqCipher.Object);
            int count = repository.Read().Count();
            repository.Delete(repository.Read().ToList()[0]);
            Assert.Equal(count - 1, repository.Read().Count());
        }

        [Fact]
        public void RepositorySetCurrentItem()
        {
            CipherRepository repository = new CipherRepository();
            repository.Create(_moqCipher.Object);
            repository.SetCurrentItem(x => x.CipherName == "MoqCipherName");
            Assert.Equal(repository.CurrentItem, _moqCipher.Object);
        }
    }
}