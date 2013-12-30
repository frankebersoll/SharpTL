// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Exceptions.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SharpTL
{
    /// <summary>
    /// Invalid TL constructor number exception.
    /// </summary>
    public class InvalidTLConstructorNumberException : Exception
    {
        public InvalidTLConstructorNumberException()
        {
        }

        public InvalidTLConstructorNumberException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Invalid TL-schema exception.
    /// </summary>
    public class InvalidTLSchemaException : Exception
    {
        public InvalidTLSchemaException()
        {
        }

        public InvalidTLSchemaException(string message) : base(message)
        {
        }

        public InvalidTLSchemaException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class TLSerializerNotFoundException : Exception
    {
        public TLSerializerNotFoundException()
        {
        }

        public TLSerializerNotFoundException(string message) : base(message)
        {
        }

        public TLSerializerNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
