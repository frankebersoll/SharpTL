// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLType.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SharpTL.Compiler.Utils;

namespace SharpTL.Compiler
{
    /// <summary>
    /// TL type.
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public class TLType : IEquatable<TLType>
    {
        private int _lastHashCode;
        private string _text;

        public string OriginalName { get; set; }

        public string Name { get; set; }

        public bool IsVoid
        {
            get { return this.Name == "void"; }
        }

        public uint? Number
        {
            get
            {
                return this.Constructors != null && this.Constructors.Count > 0
                           ? this.Constructors.Select(ctr => ctr.Number).Aggregate((u, u1) => unchecked(u + u1))
                           : (uint?) null;
            }
        }

        public List<TLCombinator> Constructors { get; set; }

        public TLSerializationMode? SerializationModeOverride { get; set; }

        public IEnumerable<TLCombinatorParameter> Parameters
        {
            get
            {
                return this.Constructors
                           .Aggregate(null, (IEnumerable<TLCombinatorParameter> a, TLCombinator c) =>
                                            a == null
                                                ? c.Parameters
                                                : a.Intersect(c.Parameters));
            }
        }

        public string Text
        {
            get { return this.ToString(); }
        }

        public TLType(string name, bool autoConvertToConventionalCase = true)
        {
            this.OriginalName = name;
            this.Name = autoConvertToConventionalCase ? "I" + name.ToConventionalCase(Case.PascalCase) : name;
            this.Constructors = new List<TLCombinator>();
        }

        public override string ToString()
        {
            int currentHashCode = this.GetHashCode();
            if (this._lastHashCode != currentHashCode)
            {
                this._lastHashCode = currentHashCode;
                this._text = string.Format("{0} 0x{1:X8} (0x{2})", this.Name, this.Number,
                                           (this.Constructors != null && this.Constructors.Count > 0)
                                               ? this.Constructors.Select(u => u.Number.ToString("X8"))
                                                     .Aggregate(
                                                                (paramsText, paramText) =>
                                                                paramsText + " + 0x" + paramText)
                                               : string.Empty);
            }
            return this._text;
        }

        #region Equality

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
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((TLType) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (this.OriginalName != null ? this.OriginalName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (this.Constructors != null ? this.Constructors.GetHashCode() : 0);
                return hashCode;
            }
        }

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
            return string.Equals(this.OriginalName, other.OriginalName) && string.Equals(this.Name, other.Name)
                   && this.Constructors.SequenceEqual(other.Constructors);
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