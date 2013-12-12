// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLBytesSerializer.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SharpTL.Serializers
{
    public class TLBytesSerializer : TLBareTypeSerializerBase
    {
        private static readonly Type _SupportedType = typeof (byte[]);

        public override Type SupportedType
        {
            get { return _SupportedType; }
        }

        public override uint ConstructorNumber
        {
            get { return 0xB5286E24; }
        }

        protected override object ReadBody(TLSerializationContext context)
        {
            return context.Streamer.ReadTLBytes();
        }

        protected override void WriteBody(object obj, TLSerializationContext context)
        {
            context.Streamer.WriteTLBytes((byte[]) obj);
        }
    }
}
