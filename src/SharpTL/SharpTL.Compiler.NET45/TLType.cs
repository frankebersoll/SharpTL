// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLType.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SharpTL.Compiler
{
    /// <summary>
    ///     TL type.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class TLType : IEquatable<TLType>
    {
        private int _lastHashCode;
        private string _text;

        public string Name { get; set; }

        public string ClrName { get; set; }

        public uint Number { get { return ConstructorNumbers.Aggregate((u, u1) => unchecked (u + u1)); } }

        public List<uint> ConstructorNumbers { get; set; }

        public string Text
        {
            get { return ToString(); }
        }

        public override string ToString()
        {
            int currentHashCode = GetHashCode();
            if (_lastHashCode != currentHashCode)
            {
                _lastHashCode = currentHashCode;
                _text = string.Format("{0} #{1:X8} ({2})", Name, Number,
                    (ConstructorNumbers != null && ConstructorNumbers.Count > 0)
                        ? ConstructorNumbers.Select(u => u.ToString("X8")).Aggregate((paramsText, paramText) => paramsText + " " + paramText)
                        : string.Empty);
            }
            return _text;
        }

        #region Equality
        public bool Equals(TLType other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return Equals((TLType) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public static bool operator ==(TLType left, TLType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TLType left, TLType right)
        {
            return !Equals(left, right);
        }
        #endregion
    }
}
