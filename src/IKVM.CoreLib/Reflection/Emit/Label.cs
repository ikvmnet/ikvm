// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Represents a label in the instruction stream. Used in conjunction with the <see cref="ILGenerator"/> class.
    /// </summary>
    /// <remarks>
    /// The Label class is an opaque representation of a label used by the
    /// <see cref="ILGenerator"/> class.  The token is used to mark where labels occur in the IL
    /// stream. Labels are created by using <see cref="ILGenerator.DefineLabel"/> and their position is set
    /// by using <see cref="ILGenerator.MarkLabel"/>.
    /// </remarks>
    readonly struct Label : IEquatable<Label>
    {

        readonly int _label;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="label">The unique id of the label within its <see cref="ILGenerator"/>.</param>
        internal Label(int label)
        {
            _label = label;
        }

        /// <summary>
        /// Gets the label unique id assigned by the ILGenerator.
        /// </summary>
        public int Id => _label;

        /// <summary>
        /// Generates a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance.</returns>
        public override int GetHashCode() => _label;

        /// <summary>
        /// Checks if the given object is an instance of <see cref="Label"/> and is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with this <see cref="Label"/> instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="Label"/> and is equal to this object; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj) =>
            obj is Label other && Equals(other);

        /// <summary>
        /// Indicates whether the current instance is equal to the specified <see cref="Label"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Label"/> to compare to the current instance.</param>
        /// <returns><see langword="true"/> if the value of <paramref name="obj"/> is equal to the value of the current instance; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Label obj) =>
            obj._label == _label;

    }

}

#nullable restore
