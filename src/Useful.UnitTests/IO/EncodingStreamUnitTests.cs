// <copyright file="EncodingStreamUnitTests.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.Security.Cryptography
{
    using System;
    using System.IO;
    using System.Text;
    using Useful.IO;
    using Xunit;

    /// <summary>
    /// This is a test class for EncodingStream and is intended to contain all the Unit Tests.
    /// </summary>
    public class EncodingStreamUnitTests : IDisposable
    {
        /// <summary>
        /// Test dispose.
        /// </summary>
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

        /// <summary>
        /// Test dispose after the object has been disposed.
        /// </summary>
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

        /// <summary>
        /// Test reading from the stream - same encoding.
        /// </summary>
        [Fact]
        public void EncodingStreamReadSameEncoding()
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;
                Encoding to = Encoding.Unicode;
                string test = "Hello World";

                byte[] fromBytes = from.GetBytes(test);
                byte[] toBytes = new byte[to.GetBytes(test).Length];

                using (EncodingStream target = new EncodingStream(input, from, to))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Flush();
                    target.Position = to.GetPreamble().Length;

                    Assert.Equal(target.Length, to.GetPreamble().Length + toBytes.Length);
                    Assert.Equal(toBytes.Length, target.Read(toBytes, 0, toBytes.Length));
                    Assert.Equal(test, to.GetString(toBytes));
                }
            }
        }

        /// <summary>
        /// Test reading from the stream - changing the encoding.
        /// </summary>
        [Fact]
        public void EncodingStreamReadChangeEncoding()
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;
                Encoding to = Encoding.UTF8;
                string test = "Hello World";

                byte[] fromBytes = from.GetBytes(test);
                byte[] toBytes = new byte[to.GetBytes(test).Length];

                using (EncodingStream target = new EncodingStream(input, from, to))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Position = to.GetPreamble().Length;

                    Assert.Equal(target.Length, to.GetPreamble().Length + toBytes.Length);
                    Assert.Equal(toBytes.Length, target.Read(toBytes, 0, toBytes.Length));
                    Assert.Equal(test, to.GetString(toBytes));
                }
            }
        }

        ///// <summary>
        ///// Test reading using a null buffer.
        ///// </summary>
        // [TestMethod]
        // [ExpectedException(typeof(ArgumentNullException))]
        // public void EncodingStreamReadNullBuffer()
        // {
        //    using (MemoryStream input = new MemoryStream())
        //    {
        //        using (EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8))
        //        {
        //            target.Read(null, 0, 1);
        //        }
        //    }
        // }

        /// <summary>
        /// Test ability to read.
        /// </summary>
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

        /// <summary>
        /// Test ability to write.
        /// </summary>
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

        /// <summary>
        /// Test ability to seek.
        /// </summary>
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

        ///// <summary>
        ///// Test writing using a null buffer.
        ///// </summary>
        // [TestMethod]
        // [ExpectedException(typeof(ArgumentNullException))]
        // public void EncodingStream_Write_Null_Buffer()
        // {
        //    using (MemoryStream input = new MemoryStream())
        //    {
        //        EncodingStream target = new EncodingStream(input, Encoding.Unicode, Encoding.UTF8);
        //        target.Write(null, 0, 1);
        //    }
        // }

        /// <summary>
        /// Test reading from the stream - changing the encoding.
        /// </summary>
        [Fact]
        public void EncodingStreamSeek()
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;

                byte[] fromBytes = from.GetBytes("Hello World");

                using (EncodingStream target = new EncodingStream(input, from, Encoding.UTF8))
                {
                    target.Write(fromBytes, 0, fromBytes.Length);
                    target.Seek(2, SeekOrigin.Begin);
                    Assert.Equal(2, target.Position);
                }
            }
        }

        /// <summary>
        /// Test reading from the stream - changing the encoding.
        /// </summary>
        [Fact]
        public void EncodingStreamSetLength()
        {
            using (MemoryStream input = new MemoryStream())
            {
                Encoding from = Encoding.Unicode;

                byte[] fromBytes = from.GetBytes("Hello World");

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