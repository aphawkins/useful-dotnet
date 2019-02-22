// <copyright file="EncodingStream.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace Useful.IO
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Changes the encoding of a byte stream.
    /// </summary>
    [DebuggerDisplay("Position={this.Position}")]
    public sealed class EncodingStream : Stream
    {
        private Encoding _from;
        private Stream _source;
        private Encoding _to;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncodingStream"/> class.
        /// </summary>
        /// <param name="stream">The underlying stream to perform the encoding change on.</param>
        /// <param name="from">Encoding of the source.</param>
        /// <param name="to">Encoding of the destination.</param>
        public EncodingStream(Stream stream, Encoding from, Encoding to)
        {
            _source = stream;
            _from = from;
            _to = to;
            byte[] preamble = _to.GetPreamble();
            if (preamble.Length > 0)
            {
                _source.Write(preamble, 0, preamble.Length);
            }
        }

        /// <inheritdoc/>
        public override bool CanRead => _source.CanRead;

        /// <inheritdoc/>
        public override bool CanSeek => _source.CanSeek;

        /// <inheritdoc/>
        public override bool CanWrite => _source.CanWrite;

        /// <inheritdoc/>
        public override long Length => _source.Length;

        /// <inheritdoc/>
        public override long Position
        {
            get => _source.Position;
            set => _source.Position = value;
        }

        /// <inheritdoc/>
        public override void Flush() => _source.Flush();

        /// <inheritdoc/>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            int result = _source.Read(buffer, offset, count);

            return result;
        }

        /// <inheritdoc/>
        public override long Seek(long offset, SeekOrigin origin) => _source.Seek(offset, origin);

        /// <inheritdoc/>
        public override void SetLength(long value) => _source.SetLength(value);

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies count bytes from buffer to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="ArgumentNullException"> Thrown if <paramref name="buffer" /> is null.</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            char[] c = _from.GetChars(buffer, offset, count);
            byte[] b = _to.GetBytes(c);

            _source.Write(b, 0, b.Length);
        }
    }
}