// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuiltIn.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace SharpTL.Serializers
{
    /// <summary>
    ///     Built-in stuff.
    /// </summary>
    public static class BuiltIn
    {
        private static readonly List<ITLSerializer> BaseTypeSerializersInternal;

        static BuiltIn()
        {
            // Add base type serializers.
            BaseTypeSerializersInternal = new List<ITLSerializer>
            {
                new IntSerializer(),
                new UIntSerializer(),
                new LongSerializer(),
                new ULongSerializer(),
                new DoubleSerializer(),
                new StringSerializer(),
                new BooleanSerializer(),
                new TLVectorSerializer<object>(),
                new TLBytesSerializer()
            };
        }

        /// <summary>
        ///     Built-in base type serializers.
        /// </summary>
        public static IEnumerable<ITLSerializer> BaseTypeSerializers
        {
            get { return BaseTypeSerializersInternal; }
        }
    }
}
