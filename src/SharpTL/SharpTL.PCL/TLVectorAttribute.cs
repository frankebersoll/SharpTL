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
        /// <param name="itemsSerializationModeOverride">Vector items serialization mode override.</param>
        public TLVectorAttribute(TLSerializationMode itemsSerializationModeOverride)
        {
            ItemsSerializationModeOverride = itemsSerializationModeOverride;
        }

        /// <summary>
        ///     Vector items serialization mode override.
        /// </summary>
        public TLSerializationMode ItemsSerializationModeOverride { get; private set; }
    }
}
