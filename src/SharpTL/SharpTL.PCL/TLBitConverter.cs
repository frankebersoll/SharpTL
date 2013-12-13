// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLBitConverter.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Runtime.CompilerServices;

namespace SharpTL
{
    /// <summary>
    ///     Bit converter methods which support explicit endian.
    /// </summary>
    public static class TLBitConverter
    {
        /// <summary>
        ///     Converts <see cref="int" /> to array of bytes.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="asLittleEndian">Convert to little endian.</param>
        /// <returns>Array of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(this int value, bool asLittleEndian)
        {
            var bytes = new byte[4];
            if (asLittleEndian)
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
            return bytes;
        }

        /// <summary>
        ///     Converts array of bytes to <see cref="int" />.
        /// </summary>
        /// <param name="bytes">Bytes array.</param>
        /// <param name="asLittleEndian">Convert from little endian.</param>
        /// <returns><see cref="int" /> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(this byte[] bytes, bool asLittleEndian)
        {
            return asLittleEndian ? bytes[0] | bytes[1] << 8 | bytes[2] << 16 | bytes[3] << 24 : bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
        }

        /// <summary>
        ///     Converts <see cref="long" /> to array of bytes.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="asLittleEndian">Convert to little endian.</param>
        /// <returns>Array of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(this long value, bool asLittleEndian)
        {
            var bytes = new byte[8];
            if (asLittleEndian)
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
            return bytes;
        }

        /// <summary>
        ///     Converts array of bytes to <see cref="long" />.
        /// </summary>
        /// <param name="bytes">Bytes array.</param>
        /// <param name="asLittleEndian">Convert from little endian.</param>
        /// <returns><see cref="long" /> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(this byte[] bytes, bool asLittleEndian)
        {
            if (bytes.Length != 8)
            {
                throw new ArgumentOutOfRangeException("bytes", string.Format("Length of bytes array must be 8, actual is {0}.", bytes.Length));
            }

            return asLittleEndian
                ? bytes[0] | (long) bytes[1] << 8 | (long) bytes[2] << 16 | (long) bytes[3] << 24 | (long) bytes[4] << 32 | (long) bytes[5] << 40 | (long) bytes[6] << 48 |
                    (long) bytes[7] << 56
                : (long) bytes[0] << 56 | (long) bytes[1] << 48 | (long) bytes[2] << 40 | (long) bytes[3] << 32 | (long) bytes[4] << 24 | (long) bytes[5] << 16 |
                    (long) bytes[6] << 8 | bytes[7];
        }

        /// <summary>
        ///     Converts <see cref="ulong" /> to array of bytes.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="asLittleEndian">Convert to little endian.</param>
        /// <returns>Array of bytes.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ToBytes(this ulong value, bool asLittleEndian)
        {
            return ((long) value).ToBytes(asLittleEndian);
        }

        /// <summary>
        ///     Converts array of bytes to <see cref="ulong" />.
        /// </summary>
        /// <param name="bytes">Bytes array.</param>
        /// <param name="asLittleEndian">Convert from little endian.</param>
        /// <returns><see cref="ulong" /> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(this byte[] bytes, bool asLittleEndian)
        {
            return (ulong) bytes.ToInt64(asLittleEndian);
        }
    }
}
