// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLStreamerFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SharpTL.BaseTypes;

namespace SharpTL.Tests
{
    [TestFixture]
    public class TLStreamerFacts
    {
        private static readonly byte[] TestIntBytesInBigEndian = {0x1, 0x2, 0x3, 0x4};
        private static readonly byte[] TestLongBytesInBigEndian = {0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8};
        private static readonly byte[] TestDoubleBytesInBigEndian = TestLongBytesInBigEndian;
        private static readonly byte[] TestInt128BytesInBigEndian = { 0x1, 0x2, 0x3, 0x4, 0x5, 0x6, 0x7, 0x8, 0x9, 0xA, 0xB, 0xC, 0xD, 0xE, 0xF, 0x10 };

        private static readonly byte[] TestIntBytesInLittleEndian = TestIntBytesInBigEndian.Reverse().ToArray();
        private static readonly byte[] TestLongBytesInLittleEndian = TestLongBytesInBigEndian.Reverse().ToArray();
        private static readonly byte[] TestDoubleBytesInLittleEndian = TestLongBytesInLittleEndian;
        private static readonly byte[] TestInt128BytesInLittleEndian = TestInt128BytesInBigEndian.Reverse().ToArray();

        private const int TestInt = 0x01020304;
        private const long TestLong = 0x0102030405060708;
        private static readonly double TestDouble = BitConverter.Int64BitsToDouble(TestLong);
        private static readonly Int128 TestInt128 = Int128.Parse("0x0102030405060708090A0B0C0D0E0F10");
        
        [TestCase(true)]
        [TestCase(false)]
        public void Should_write_int(bool streamAsLittleEndian)
        {
            CheckWriteToStream(stream => stream.WriteInt(TestInt), TestIntBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_write_long(bool streamAsLittleEndian)
        {
            CheckWriteToStream(stream => stream.WriteLong(TestLong), TestLongBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_write_double(bool streamAsLittleEndian)
        {
            CheckWriteToStream(stream => stream.WriteDouble(TestDouble), TestLongBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_write_int128(bool streamAsLittleEndian)
        {
            CheckWriteToStream(stream => stream.WriteInt128(TestInt128), TestInt128BytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_read_int(bool streamAsLittleEndian)
        {
            CheckReadFromStream(stream => stream.ReadInt(), TestInt, streamAsLittleEndian ? TestIntBytesInLittleEndian : TestIntBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_read_long(bool streamAsLittleEndian)
        {
            CheckReadFromStream(stream => stream.ReadLong(), TestLong, streamAsLittleEndian ? TestLongBytesInLittleEndian : TestLongBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_read_double(bool streamAsLittleEndian)
        {
            CheckReadFromStream(stream => stream.ReadDouble(), TestDouble, streamAsLittleEndian ? TestDoubleBytesInLittleEndian : TestDoubleBytesInBigEndian, streamAsLittleEndian);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Should_read_int128(bool streamAsLittleEndian)
        {
            CheckReadFromStream(stream => stream.ReadInt128(), TestInt128, streamAsLittleEndian ? TestInt128BytesInLittleEndian : TestInt128BytesInBigEndian, streamAsLittleEndian);
        }

        private static void CheckWriteToStream(Action<TLStreamer> write, byte[] testBytesInBigEndian, bool streamAsLittleEndian)
        {
            int bytesCount = testBytesInBigEndian.Length;

            using (var streamer = new TLStreamer {StreamAsLittleEndian = streamAsLittleEndian})
            {
                write(streamer);
                streamer.Length.Should().Be(bytesCount);

                streamer.Position = 0;

                var bytes = new byte[bytesCount];
                streamer.Read(bytes, 0, bytesCount);

                if (streamAsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                for (byte i = 0; i < bytesCount; i++)
                {
                    bytes[i].Should().Be(testBytesInBigEndian[i]);
                }
            }
        }

        private static void CheckReadFromStream<T>(Func<TLStreamer, T> read, T testValue, byte[] streamBuffer, bool streamAsLittleEndian) where T : struct
        {
            using (var stream = new TLStreamer(streamBuffer) {StreamAsLittleEndian = streamAsLittleEndian})
            {
                T value = read(stream);
                value.ShouldBeEquivalentTo(testValue);
            }
        }
    }
}
