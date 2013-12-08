// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLObjectAttribute.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SharpTL
{
    /// <summary>
    ///     TL object attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class TLObjectAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TLObjectAttribute" /> class.
        /// </summary>
        /// <param name="constructorNumber">Constructor number.</param>
        public TLObjectAttribute(uint constructorNumber)
        {
            ConstructorNumber = constructorNumber;
            SerializationMode = TLSerializationMode.Boxed;
        }

        /// <summary>
        ///     Constructor number.
        /// </summary>
        public uint ConstructorNumber { get; private set; }

        /// <summary>
        ///     Serialization mode.
        /// </summary>
        public TLSerializationMode SerializationMode { get; set; }
    }
}
