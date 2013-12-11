using System;
using SharpTL.BaseTypes;

namespace SharpTL.Serializers
{
    /// <summary>
    /// Serializer for 256-bit integer.
    /// </summary>
    public class Int256Serializer : TLBareTypeSerializerBase
    {
        private static readonly Type _SupportedType = typeof(Int128);

        public override uint ConstructorNumber
        {
            get { return 0x7BEDEB5B; }
        }

        public override Type SupportedType
        {
            get { return _SupportedType; }
        }

        protected override object ReadBody(TLSerializationContext context)
        {
            return new Int128 { H = context.Streamer.ReadULong(), L = context.Streamer.ReadULong() };
        }

        protected override void WriteBody(object obj, TLSerializationContext context)
        {
            var int128 = (Int128)obj;
            context.Streamer.WriteULong(int128.L);
        }
    }
}