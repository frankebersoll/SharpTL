// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLStreamer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using SharpTL.BaseTypes;

namespace SharpTL
{
    /// <summary>
    ///     TL streamer.
    /// </summary>
    public class TLStreamer : IDisposable
    {
        private readonly bool _leaveOpen;
        private bool _disposed;
        private Stream _stream;
        private bool _streamAsLittleEndianInternal = true;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class.
        /// </summary>
        public TLStreamer()
        {
            _stream = new MemoryStream();
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
        public TLStreamer(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
            _leaveOpen = false;
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
        public long Position
        {
            get { return _stream.Position; }
            set { _stream.Position = value; }
        }

        /// <summary>
        ///     Length.
        /// </summary>
        public long Length
        {
            get { return _stream.Length; }
        }

        /// <summary>
        ///     Reads bytes to a buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public void Read(byte[] buffer, int offset, int count)
        {
            _stream.Read(buffer, offset, count);
        }

        /// <summary>
        ///     Writes bytes from a buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="count">Count.</param>
        public void Write(byte[] buffer, int offset, int count)
        {
            _stream.Write(buffer, offset, count);
        }

        /// <summary>
        ///     Reads byte.
        /// </summary>
        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        /// <summary>
        ///     Writes byte.
        /// </summary>
        public void WriteByte(byte value)
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

            return bytes.ToInt32(_streamAsLittleEndianInternal);
        }

        /// <summary>
        ///     Writes 32-bit signed integer.
        /// </summary>
        public void WriteInt(int value)
        {
            var bytes = value.ToBytes(_streamAsLittleEndianInternal);
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

            return bytes.ToInt64(_streamAsLittleEndianInternal);
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
            var buffer = new byte[16];
            Read(buffer, 0, buffer.Length);
            return new Int128(buffer);
        }

        /// <summary>
        ///     Writes a 128-bit signed integer.
        /// </summary>
        public void WriteInt128(Int128 value)
        {
            var buffer = value.ToByteArray(StreamAsLittleEndian);
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Reads a 256-bit signed integer.
        /// </summary>
        public Int256 ReadInt256()
        {
            return new Int256 {H = ReadInt128(), L = ReadInt128()};
        }

        /// <summary>
        ///     Writes a 128-bit signed integer.
        /// </summary>
        public void WriteInt256(Int256 value)
        {
            WriteInt128(value.L);
            WriteInt128(value.H);
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

        #region Disposing
        /// <summary>
        ///     Dispose.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose.
        /// </summary>
        /// <param name="disposing">
        ///     A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both
        ///     managed and native resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
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
