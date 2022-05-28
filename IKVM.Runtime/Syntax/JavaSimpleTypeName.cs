using System;

using IKVM.Runtime.Extensions;

namespace IKVM.Runtime.Syntax
{

    /// <summary>
    /// Provides methods to parse a Java class simple name.
    /// </summary>
    readonly struct JavaSimpleTypeName
    {

        public static JavaSimpleTypeName Empty => new(ReadOnlyMemory<char>.Empty);

        public static implicit operator string(JavaSimpleTypeName typeName) => typeName.ToString();
        public static implicit operator JavaSimpleTypeName(string typeName) => new(typeName);

        public static implicit operator ReadOnlyMemory<char>(JavaSimpleTypeName typeName) => typeName.value;
        public static implicit operator JavaSimpleTypeName(ReadOnlyMemory<char> typeName) => new(typeName);

        public static bool operator ==(JavaSimpleTypeName a, JavaSimpleTypeName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal);
        public static bool operator !=(JavaSimpleTypeName a, JavaSimpleTypeName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal) == false;


        readonly ReadOnlyMemory<char> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaSimpleTypeName(string value) :
            this(value.AsMemory())
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaSimpleTypeName(ReadOnlyMemory<char> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the underlying value of this simple name.
        /// </summary>
        public ReadOnlyMemory<char> Value => value;

        /// <summary>
        /// Returns <c>true</c> if this type name is empty.
        /// </summary>
        public bool IsEmpty => value.IsEmpty;

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaSimpleTypeName other) => value.Span.Equals(other.value.Span, StringComparison.Ordinal);

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => false;

        /// <summary>
        /// Gets the hash code of this type name.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => value.Span.GetHashCodeExtension();

        /// <summary>
        /// Returns a string instance of the type name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => value.Span.ToString();

    }

}
