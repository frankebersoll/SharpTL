// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Int128.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Runtime.InteropServices;

namespace SharpTL.BaseTypes
{
    /// <summary>
    ///     Represents a 128-bit unsigned integer.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Int128
    {
        /// <summary>
        ///     High ulong part of 128-bit integer.
        /// </summary>
        [FieldOffset(0)] public ulong H;

        /// <summary>
        ///     Low ulong part of 128-bit integer.
        /// </summary>
        [FieldOffset(8)] public ulong L;
    }
}
