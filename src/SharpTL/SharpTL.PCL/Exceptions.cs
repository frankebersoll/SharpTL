// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Exceptions.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace SharpTL
{
    public class InvalidTLConstructorNumber : Exception
    {
        public InvalidTLConstructorNumber()
        {
        }

        public InvalidTLConstructorNumber(string message) : base(message)
        {
        }
    }
}
