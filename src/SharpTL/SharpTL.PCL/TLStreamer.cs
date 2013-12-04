// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLStreamer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;

namespace SharpTL
{
    /// <summary>
    ///     TL streamer.
    /// </summary>
    public class TLStreamer : IDisposable
    {
        private readonly bool _shouldDisposeTheStream;
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
        public TLStreamer(Stream stream)
        {
            _stream = stream;
            _shouldDisposeTheStream = false;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TLStreamer" /> class.
        /// </summary>
        /// <param name="bytes">Bytes.</param>
        public TLStreamer(byte[] bytes)
        {
            _stream = new MemoryStream(bytes);
            _shouldDisposeTheStream = true;
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
        ///     Dispose.
        /// </summary>
        public void Dispose()
        {
            if (_stream != null && _shouldDisposeTheStream)
            {
                _stream.Dispose();
            }
            _stream = null;
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
        ///     Reads byte.
        /// </summary>
        public int ReadByte()
        {
            return _stream.ReadByte();
        }

        /// <summary>
        ///     Reads 32-bit signed integer.
        /// </summary>
        public int ReadInt()
        {
            var bytes = new byte[4];
            _stream.Read(bytes, 0, 4);

            return _streamAsLittleEndianInternal ? bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24 : bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        /// <summary>
        ///     Reads 32-bit unsigned integer.
        /// </summary>
        public uint ReadUInt()
        {
            return (uint) ReadInt();
        }

        /// <summary>
        ///     Reads 64-bit signed integer.
        /// </summary>
        public long ReadLong()
        {
            var bytes = new byte[8];
            _stream.Read(bytes, 0, 8);

            return _streamAsLittleEndianInternal
                ? bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 | (long) bytes[3] << 24 | (long) bytes[4] << 32 | (long) bytes[5] << 40 | (long) bytes[6] << 48 |
                    (long) bytes[7] << 56
                : (long) bytes[0] << 56 | (long) bytes[1] << 48 | (long) bytes[2] << 40 | (long) bytes[3] << 32 | (long) bytes[4] << 24 | (long) bytes[5] << 16 |
                    (long) bytes[6] << 8 | bytes[7];
        }

        /// <summary>
        ///     Reads 64-bit unsigned integer.
        /// </summary>
        public ulong ReadULong()
        {
            return (ulong) ReadLong();
        }

        /// <summary>
        ///     Reads double.
        /// </summary>
        public double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
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
        ///     Writes byte.
        /// </summary>
        public void WriteByte(byte value)
        {
            _stream.WriteByte(value);
        }

        /// <summary>
        ///     Writes 32-bit signed integer.
        /// </summary>
        public void WriteInt(int value)
        {
            var bytes = new byte[4];

            if (_streamAsLittleEndianInternal)
            {
                bytes[0] = (byte) value;
                bytes[1] = (byte) (value >> 8);
                bytes[2] = (byte) (value >> 16);
                bytes[3] = (byte) (value >> 24);
            }
            else
            {
                bytes[0] = (byte) (value >> 24);
                bytes[1] = (byte) (value >> 16);
                bytes[2] = (byte) (value >> 8);
                bytes[3] = (byte) value;
            }

            _stream.Write(bytes, 0, 4);
        }

        /// <summary>
        ///     Writes 32-bit unsigned integer.
        /// </summary>
        public void WriteUInt(uint value)
        {
            WriteInt((int) value);
        }

        /// <summary>
        ///     Writes 64-bit signed integer.
        /// </summary>
        public void WriteLong(long value)
        {
            var bytes = new byte[8];

            if (_streamAsLittleEndianInternal)
            {
                bytes[0] = (byte) value;
                bytes[1] = (byte) (value >> 8);
                bytes[2] = (byte) (value >> 16);
                bytes[3] = (byte) (value >> 24);
                bytes[4] = (byte) (value >> 32);
                bytes[5] = (byte) (value >> 40);
                bytes[6] = (byte) (value >> 48);
                bytes[7] = (byte) (value >> 56);
            }
            else
            {
                bytes[0] = (byte) (value >> 56);
                bytes[1] = (byte) (value >> 48);
                bytes[2] = (byte) (value >> 40);
                bytes[3] = (byte) (value >> 32);
                bytes[4] = (byte) (value >> 24);
                bytes[5] = (byte) (value >> 16);
                bytes[6] = (byte) (value >> 8);
                bytes[7] = (byte) value;
            }

            _stream.Write(bytes, 0, 8);
        }

        /// <summary>
        ///     Writes 64-bit unsigned integer.
        /// </summary>
        public void WriteULong(ulong value)
        {
            WriteLong((long) value);
        }

        /// <summary>
        ///     Writes double.
        /// </summary>
        public void WriteDouble(double value)
        {
            WriteLong(BitConverter.DoubleToInt64Bits(value));
        }
    }
}
