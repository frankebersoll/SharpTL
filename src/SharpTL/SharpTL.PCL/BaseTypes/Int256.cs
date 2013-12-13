// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Int256.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SharpTL.BaseTypes
{
    /// <summary>
    ///     Represents a 256-bit unsigned integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct Int256
    {
        /// <summary>
        ///     High ulong part of 128-bit integer.
        /// </summary>
        [FieldOffset(0)] public Int128 H;

        /// <summary>
        ///     Low ulong part of 128-bit integer.
        /// </summary>
        [FieldOffset(16)] public Int128 L;
    }
}
