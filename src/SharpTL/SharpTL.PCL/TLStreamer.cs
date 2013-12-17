// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLStreamer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using BigMath;
using BigMath.Utils;

namespace SharpTL
{
    /// <summary>
    ///     TL streamer.
    /// </summary>
    public class TLStreamer : Stream
    {
        private readonly bool _leaveOpen;
        private bool _disposed;
        private Stream _stream;
        private bool _streamAsLittleEndianInternal = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class with underlying <see cref="MemoryStream" />.
        /// </summary>
        public TLStreamer() : this(new MemoryStream())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class with underlying <see cref="MemoryStream" /> with
        ///     an expandable capacity initialized as specified.
        /// </summary>
        /// <param name="capacity">The initial size of the internal <see cref="MemoryStream" /> array in bytes.</param>
        public TLStreamer(int capacity) : this(new MemoryStream(capacity))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class.
        /// </summary>
        /// <param name="stream">Stream.</param>
        public TLStreamer(Stream stream) : this(stream, false)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class.
        /// </summary>
        /// <param name="stream">Stream.</param>
        /// <param name="leaveOpen">Leave underlying stream open.</param>
        public TLStreamer(Stream stream, bool leaveOpen)
        {
            _stream = stream;
            _leaveOpen = leaveOpen;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        public TLStreamer(byte[] bytes) : this(new MemoryStream(bytes))
        {
        }

        /// <summary>
        ///     Stream as little-endian.
        /// </summary>
        public bool StreamAsLittleEndian
        {
            get { return _streamAsLittleEndianInternal; }
            set { _streamAsLittleEndianInternal = value; }
        }

        /// <summary>
        ///     Current position.
        /// </summary>
        public override long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        /// <summary>
        ///     Sets a value indicating whether the underlying stream supports writing.
        /// </summary>
        public override bool CanWrite
        {
            get { return _stream.CanWrite; }
        }

        /// <summary>
        ///     Length.
        /// </summary>
        public override long Length
        {
            get { return _stream.Length; }
        }

        /// <summary>
        ///     Gets a value indicating whether the underlying stream supports reading.
        /// </summary>
        public override bool CanRead
        {
            get { return _stream.CanRead; }
        }

        /// <summary>
        ///     Gets a value indicating whether the underlying stream supports seeking.
        /// </summary>
        public override bool CanSeek
        {
            get { return _stream.CanSeek; }
        }

        /// <summary>
        ///     Reads bytes to a buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _stream.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Sets the length of the underlying stream.
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            _stream.SetLength(value);
        }

        /// <summary>
        ///     Writes bytes from a buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        ///     Reads an array of bytes.
        /// </summary>
        /// <param name="count">Count to read.</param>
        /// <returns>Array of bytes.</returns>
        public byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            Read(buffer, 0, count);
            return buffer;
        }

        /// <summary>
        ///     Reads byte.
        /// </summary>
        public override int ReadByte()
        {
            return _stream.ReadByte();
        }

        /// <summary>
        ///     Sets the position within the underlying stream.
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _stream.Seek(offset, origin);
        }

        /// <summary>
        ///     Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        public override void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        /// <summary>
        ///     Reads 32-bit signed integer.
        /// </summary>
        public int ReadInt()
        {
            var bytes = new byte[4];
            _stream.Read(bytes, 0, 4);

            return bytes.ToInt32(0, _streamAsLittleEndianInternal);
        }

        /// <summary>
        ///     Writes 32-bit signed integer.
        /// </summary>
        public void WriteInt(int value)
        {
            byte[] bytes = value.ToBytes(_streamAsLittleEndianInternal);
            _stream.Write(bytes, 0, 4);
        }

        /// <summary>
        ///     Reads 32-bit unsigned integer.
        /// </summary>
        public uint ReadUInt()
        {
            return (uint) ReadInt();
        }

        /// <summary>
        ///     Writes 32-bit unsigned integer.
        /// </summary>
        public void WriteUInt(uint value)
        {
            WriteInt((int) value);
        }

        /// <summary>
        ///     Reads 64-bit signed integer.
        /// </summary>
        public long ReadLong()
        {
            var bytes = new byte[8];
            _stream.Read(bytes, 0, 8);

            return bytes.ToInt64(0, _streamAsLittleEndianInternal);
        }

        /// <summary>
        ///     Writes 64-bit signed integer.
        /// </summary>
        public void WriteLong(long value)
        {
            _stream.Write(value.ToBytes(_streamAsLittleEndianInternal), 0, 8);
        }

        /// <summary>
        ///     Reads 64-bit unsigned integer.
        /// </summary>
        public ulong ReadULong()
        {
            return (ulong) ReadLong();
        }

        /// <summary>
        ///     Writes 64-bit unsigned integer.
        /// </summary>
        public void WriteULong(ulong value)
        {
            WriteLong((long) value);
        }

        /// <summary>
        ///     Reads double.
        /// </summary>
        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
        }

        /// <summary>
        ///     Writes double.
        /// </summary>
        public void WriteDouble(double value)
        {
            WriteLong(BitConverter.DoubleToInt64Bits(value));
        }

        /// <summary>
        ///     Reads a 128-bit signed integer.
        /// </summary>
        public Int128 ReadInt128()
        {
            return new Int128(ReadBytes(16), 0, _streamAsLittleEndianInternal);
        }

        /// <summary>
        ///     Writes a 128-bit signed integer.
        /// </summary>
        public void WriteInt128(Int128 value)
        {
            byte[] buffer = value.ToByteArray(StreamAsLittleEndian);
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Reads a 256-bit signed integer.
        /// </summary>
        public Int256 ReadInt256()
        {
            return new Int256(ReadBytes(32), 0, _streamAsLittleEndianInternal);
        }

        /// <summary>
        ///     Writes a 256-bit signed integer.
        /// </summary>
        public void WriteInt256(Int256 value)
        {
            Write(value.ToByteArray(_streamAsLittleEndianInternal), 0, 32);
        }

        /// <summary>
        ///     Reads a bunch of bytes formated as described in TL.
        /// </summary>
        public byte[] ReadTLBytes()
        {
            int offset = 1;
            int length = ReadByte();
            if (length == 254)
            {
                offset = 4;
                length = ReadByte() | ReadByte() << 8 | ReadByte() << 16;
            }
            var bytes = new byte[length];
            Read(bytes, 0, length);

            offset = 4 - (offset + length)%4;
            if (offset < 4)
            {
                Position += offset;
            }
            return bytes;
        }

        /// <summary>
        ///     Writes a bunch of bytes formated as described in TL.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException">When array size exceeds </exception>
        public void WriteTLBytes(byte[] bytes)
        {
            int length = bytes.Length;
            int offset = 1;
            if (length <= 253)
            {
                WriteByte((byte) length);
            }
            else if (length >= 254 && length <= 0xFFFFFF)
            {
                offset = 4;
                var lBytes = new byte[4];
                lBytes[0] = 254;
                lBytes[1] = (byte) length;
                lBytes[2] = (byte) (length >> 8);
                lBytes[3] = (byte) (length >> 16);
                Write(lBytes, 0, 4);
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("Array length {0} exceeds the maximum allowed {1}.", length, 0xFFFFFF));
            }

            Write(bytes, 0, length);

            offset = 4 - (offset + length)%4;
            if (offset < 4)
            {
                Write(new byte[offset], 0, offset);
            }
        }

        /// <summary>
        ///     Clears all buffers for the underlying stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            _stream.Flush();
        }

        #region Disposing
        /// <summary>
        ///     Dispose.
        /// </summary>
        /// <param name="disposing">
        ///     A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both
        ///     managed and native resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                if (_stream != null)
                {
                    if (_leaveOpen)
                    {
                        _stream.Flush();
                    }
                    else
                    {
                        _stream.Dispose();
                    }
                    _stream = null;
                }
            }

            _disposed = true;
        }
        #endregion
    }
}
