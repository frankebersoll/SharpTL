// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLVectorAttribute.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SharpTL
{
    /// <summary>
    ///     TL vector attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TLVectorAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TLVectorAttribute" /> class.
        /// </summary>
        /// <param name="itemsSerializationMode">Vector items serialization mode.</param>
        public TLVectorAttribute(TLSerializationMode itemsSerializationMode)
        {
            ItemsSerializationMode = itemsSerializationMode;
        }

        /// <summary>
        ///     Vector items serialization mode.
        /// </summary>
        public TLSerializationMode ItemsSerializationMode { get; private set; }
    }
}
