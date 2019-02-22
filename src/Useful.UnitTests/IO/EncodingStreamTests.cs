// <copyright file="EncodingStreamTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.IO.Tests
{
    using System;
    using System.IO;
    using System.Text;
    using Useful.IO;
    using Xunit;

    public class EncodingStreamTests : IDisposable
    {
        [Fact]
        public void EncodingStreamDispose()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.ASCII, Encoding.UTF8))
                {
                }
            }
        }

        [Fact]
        public void EncodingStreamDoubleDispose()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.ASCII, Encoding.UTF8))
                {
                    target.Dispose();
                }
            }
        }

        [Theory]
        [InlineData("Hello World")]
        public void EncodingStreamReadSameEncoding(string text)
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;
                Encoding to = Encoding.Unicode;

                byte[] fromBytes = from.GetBytes(text);
                byte[] toBytes = new byte[to.GetBytes(text).Length];

                using (EncodingStream target = new EncodingStream(input, from, to))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Flush();
                    target.Position = to.GetPreamble().Length;

                    Assert.Equal(target.Length, to.GetPreamble().Length + toBytes.Length);
                    Assert.Equal(toBytes.Length, target.Read(toBytes, 0, toBytes.Length));
                    Assert.Equal(text, to.GetString(toBytes));
                }
            }
        }

        [Theory]
        [InlineData("Hello World")]
        public void EncodingStreamReadChangeEncoding(string text)
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;
                Encoding to = Encoding.UTF8;

                byte[] fromBytes = from.GetBytes(text);
                byte[] toBytes = new byte[to.GetBytes(text).Length];

                using (EncodingStream target = new EncodingStream(input, from, to))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Position = to.GetPreamble().Length;

                    Assert.Equal(target.Length, to.GetPreamble().Length + toBytes.Length);
                    Assert.Equal(toBytes.Length, target.Read(toBytes, 0, toBytes.Length));
                    Assert.Equal(text, to.GetString(toBytes));
                }
            }
        }

        [Fact]
        public void EncodingStreamReadNullBuffer()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
                {
                    Assert.Throws<ArgumentNullException>(() => target.Read(null, 0, 1));
                }
            }
        }

        [Fact]
        public void EncodingStreamCanRead()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
                {
                    Assert.True(target.CanRead);
                }
            }
        }

        [Fact]
        public void EncodingStreamCanWrite()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
                {
                    Assert.True(target.CanWrite);
                }
            }
        }

        [Fact]
        public void EncodingStreamCanSeek()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
                {
                    Assert.True(target.CanSeek);
                }
            }
        }

        [Fact]
        public void EncodingStreamWriteNullBuffer()
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
                {
                    Assert.Throws<ArgumentNullException>(() => target.Write(null, 0, 1));
                }
            }
        }

        [Theory]
        [InlineData("Hello World")]
        public void EncodingStreamSeek(string text)
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;

                byte[] fromBytes = from.GetBytes(text);

                using (EncodingStream target = new EncodingStream(input, from, Encoding.UTF8))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Seek(2, SeekOrigin.Begin);
                    Assert.Equal(2, target.Position);
                }
            }
        }

        [Theory]
        [InlineData("Hello World")]
        public void EncodingStreamSetLength(string text)
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;

                byte[] fromBytes = from.GetBytes(text);

                using (EncodingStream target = new EncodingStream(input, from, Encoding.UTF8))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.SetLength(50);
                    Assert.Equal(50, target.Length);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }

            // free native resources if there are any.
        }
    }
}