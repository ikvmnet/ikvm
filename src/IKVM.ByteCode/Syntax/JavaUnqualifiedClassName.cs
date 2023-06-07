using System;

using IKVM.ByteCode.Extensions;

namespace IKVM.ByteCode.Syntax
{

    /// <summary>
    /// Provides methods to parse a Java class name without package.
    /// </summary>
    public readonly struct JavaUnqualifiedClassName
    {

        public static JavaUnqualifiedClassName Empty => new(ReadOnlyMemory<char>.Empty);

        public static implicit operator string(JavaUnqualifiedClassName typeName) => typeName.ToString();
        public static implicit operator JavaUnqualifiedClassName(string typeName) => new(typeName);

        public static implicit operator ReadOnlyMemory<char>(JavaUnqualifiedClassName typeName) => typeName.value;
        public static implicit operator JavaUnqualifiedClassName(ReadOnlyMemory<char> typeName) => new(typeName);

        public static bool operator ==(JavaUnqualifiedClassName a, JavaUnqualifiedClassName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal);
        public static bool operator !=(JavaUnqualifiedClassName a, JavaUnqualifiedClassName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal) == false;

        readonly ReadOnlyMemory<char> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaUnqualifiedClassName(string value) :
            this(value.AsMemory())
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaUnqualifiedClassName(ReadOnlyMemory<char> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Gets the underlying value of the type name.
        /// </summary>
        public ReadOnlyMemory<char> Value => value;

        /// <summary>
        /// Returns <c>true</c> if this type name is empty.
        /// </summary>
        public bool IsEmpty => value.IsEmpty;

        /// <summary>
        /// Returns <c>true</c> if this name represents an inner class of another class.
        /// </summary>
        public bool IsInnerClass => value.Span.LastIndexOf('$') != -1;

        /// <summary>
        /// Returns <c>true</c> if this name represents a nested class.
        /// </summary>
        public bool IsNestedClass => value.Span.LastIndexOf('$') is int i && i != -1 && i + 1 < value.Length && char.IsNumber(value.Span[i + 1]) == false;

        /// <summary>
        /// Returns <c>true</c> if this name represents an anonymous class.
        /// </summary>
        public bool IsAnonymousClass => value.Span.LastIndexOf('$') is int i && i != -1 && i + 1 < value.Length && value.Slice(i + 1).Span.OnlyNumbers();

        /// <summary>
        /// Returns <c>true</c> if this name represents a local class.
        /// </summary>
        public bool IsLocalClass => value.Span.LastIndexOf('$') is int i && i != -1 && i + 2 < value.Length && char.IsNumber(value.Span[i + 1]) && value.Slice(i + 1).Span.OnlyNumbers() == false;

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaUnqualifiedClassName other) => value.Span.Equals(other.value.Span, StringComparison.Ordinal);

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