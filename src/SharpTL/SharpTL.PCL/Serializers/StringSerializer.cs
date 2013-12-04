// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringSerializer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Text;

namespace SharpTL.Serializers
{
    public class StringSerializer : TLBareTypeSerializerBase
    {
        private static readonly Type _SupportedType = typeof (string);

        public override Type SupportedType
        {
            get { return _SupportedType; }
        }

        public override uint ConstructorNumber
        {
            get { return 0xB5286E24; }
        }

        private static Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        protected override object ReadBody(TLSerializationContext context)
        {
            TLStreamer streamer = context.Streamer;
            int offset = 1;
            int length = streamer.ReadByte();
            if (length == 254)
            {
                offset = 4;
                length = streamer.ReadByte() | streamer.ReadByte() << 8 | streamer.ReadByte() << 16;
            }
            var bytes = new byte[length];
            streamer.Read(bytes, 0, length);

            offset = 4 - (offset + length)%4;
            if (offset < 4)
            {
                streamer.Position += offset;
            }
            return Encoding.GetString(bytes, 0, length);
        }

        protected override void WriteBody(object obj, TLSerializationContext context)
        {
            var str = (string) obj;
            TLStreamer streamer = context.Streamer;

            // TODO: ensure encoding.
            byte[] bytes = Encoding.GetBytes(str);
            int length = bytes.Length;
            int offset = 1;
            if (length <= 253)
            {
                streamer.WriteByte((byte) length);
            }
            else if (length >= 254 && length <= 0xFFFFFF)
            {
                offset = 4;
                var lBytes = new byte[4];
                lBytes[0] = 254;
                lBytes[1] = (byte) length;
                lBytes[2] = (byte) (length >> 8);
                lBytes[3] = (byte) (length >> 16);
                streamer.Write(lBytes, 0, 4);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            streamer.Write(bytes, 0, length);

            offset = 4 - (offset + length)%4;
            if (offset < 4)
            {
                streamer.Write(new byte[offset], 0, offset);
            }
        }
    }
}
