using System;

using IKVM.Runtime.Extensions;

namespace IKVM.Runtime.Syntax
{

    /// <summary>
    /// Provides methods to parse a Java class name.
    /// </summary>
    readonly struct JavaTypeName
    {

        public static JavaTypeName Empty => new(ReadOnlyMemory<char>.Empty);

        public static implicit operator string(JavaTypeName typeName) => typeName.ToString();
        public static implicit operator JavaTypeName(string typeName) => new(typeName);

        public static implicit operator ReadOnlyMemory<char>(JavaTypeName typeName) => typeName.value;
        public static implicit operator JavaTypeName(ReadOnlyMemory<char> typeName) => new(typeName);

        public static bool operator ==(JavaTypeName a, JavaTypeName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal);
        public static bool operator !=(JavaTypeName a, JavaTypeName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal) == false;

        readonly ReadOnlyMemory<char> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaTypeName(string value) :
            this(value.AsMemory())
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaTypeName(ReadOnlyMemory<char> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        public JavaTypeName(JavaPackageName package, JavaUnqualifiedTypeName name) :
            this(package.IsEmpty ? name.Value : (package + '.' + name).AsMemory())
        {

        }

        /// <summary>
        /// Gets the underlying value of the type name.
        /// </summary>
        public ReadOnlyMemory<char> Value => value;

        /// <summary>
        /// Returns <c>true</c> if this represents the empty type name.
        /// </summary>
        public bool IsEmpty => value.IsEmpty;

        /// <summary>
        /// Gets the package name.
        /// </summary>
        public JavaPackageName PackageName => GetPackageName();

        /// <summary>
        /// Gets the package name.
        /// </summary>
        /// <returns></returns>
        JavaPackageName GetPackageName()
        {
            var i = value.Span.LastIndexOf('.');
            if (i != -1)
                return new JavaPackageName(value.Slice(0, i));
            else
                return new JavaPackageName("");
        }

        /// <summary>
        /// Gets the unqualified type name.
        /// </summary>
        public JavaUnqualifiedTypeName UnqualifiedName => GetUnqualifiedName();

        /// <summary>
        /// Gets the unqualified type name.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        JavaUnqualifiedTypeName GetUnqualifiedName()
        {
            var index = value.Span.LastIndexOf('.');
            return index != -1 ? new JavaUnqualifiedTypeName(value.Slice(index + 1)) : new JavaUnqualifiedTypeName(value);
        }

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        public JavaSimpleTypeName SimpleName => GetSimpleName();

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        /// <returns></returns>
        JavaSimpleTypeName GetSimpleName()
        {
            if (IsEmpty)
                return JavaSimpleTypeName.Empty;
            if (IsAnonymousClass)
                return JavaSimpleTypeName.Empty;

            var index = value.Span.LastIndexOfAny('.', '$');
            return index != -1 ? new JavaSimpleTypeName(value.Slice(index + 1)) : new JavaSimpleTypeName(value);
        }

        /// <summary>
        /// Returns <c>true</c> if this type is a member of the given package.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool IsMemberOf(JavaPackageName packageName) => PackageName == packageName;

        /// <summary>
        /// Returns <c>true</c> if this name represents an inner class of another class.
        /// </summary>
        public bool IsInnerClass => UnqualifiedName.IsInnerClass;

        /// <summary>
        /// Returns <c>true</c> if this name represents a nested class.
        /// </summary>
        public bool IsNestedClass => UnqualifiedName.IsNestedClass;

        /// <summary>
        /// Returns <c>true</c> if this name represents an anonymous class.
        /// </summary>
        public bool IsAnonymousClass => UnqualifiedName.IsAnonymousClass;

        /// <summary>
        /// Returns <c>true</c> if this name represents a local class.
        /// </summary>
        public bool IsLocalClass => UnqualifiedName.IsLocalClass;

        /// <summary>
        /// If this class name is a nested, inner or anonymous class, returns the parent class name, else returns empty.
        /// </summary>
        public JavaTypeName Parent => GetParent();

        JavaTypeName GetParent()
        {
            var b = value.Span.LastIndexOf('$');
            return b != -1 ? new JavaTypeName(value.Slice(0, b)) : Empty;
        }

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaTypeName other) => value.Span.Equals(other.value.Span, StringComparison.Ordinal);

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
