using System;

using IKVM.ByteCode.Extensions;

namespace IKVM.ByteCode.Syntax
{

    /// <summary>
    /// Provides methods to parse a Java class name.
    /// </summary>
    public readonly struct JavaClassName
    {

        public static JavaClassName Empty => new(ReadOnlyMemory<char>.Empty);

        public static implicit operator string(JavaClassName typeName) => typeName.ToString();
        public static implicit operator JavaClassName(string typeName) => Parse(typeName.AsMemory());

        public static implicit operator ReadOnlyMemory<char>(JavaClassName typeName) => typeName.value;
        public static implicit operator JavaClassName(ReadOnlyMemory<char> typeName) => Parse(typeName);

        public static bool operator ==(JavaClassName a, JavaClassName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal);
        public static bool operator !=(JavaClassName a, JavaClassName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal) == false;

        /// <summary>
        /// Parses the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static JavaClassName Parse(ReadOnlyMemory<char> buffer) => new JavaClassName(buffer);

        /// <summary>
        /// Parses the given binary name buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static JavaClassName ParseBinaryName(ReadOnlyMemory<char> buffer) => Parse(buffer.ToString().Replace('/', '.').AsMemory());

        readonly ReadOnlyMemory<char> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public JavaClassName(string value) :
            this(value.AsMemory())
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        JavaClassName(ReadOnlyMemory<char> value)
        {
            this.value = value;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="package"></param>
        /// <param name="name"></param>
        public JavaClassName(JavaPackageName package, JavaUnqualifiedClassName name) :
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
        public JavaUnqualifiedClassName UnqualifiedName => GetUnqualifiedName();

        /// <summary>
        /// Gets the unqualified type name.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        JavaUnqualifiedClassName GetUnqualifiedName()
        {
            var index = value.Span.LastIndexOf('.');
            return index != -1 ? new JavaUnqualifiedClassName(value.Slice(index + 1)) : new JavaUnqualifiedClassName(value);
        }

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        public JavaSimpleClassName SimpleName => GetSimpleName();

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        /// <returns></returns>
        JavaSimpleClassName GetSimpleName()
        {
            if (IsEmpty)
                return JavaSimpleClassName.Empty;
            if (IsAnonymousClass)
                return JavaSimpleClassName.Empty;

            var index = value.Span.LastIndexOfAny('.', '$');
            return index != -1 ? new JavaSimpleClassName(value.Slice(index + 1)) : new JavaSimpleClassName(value);
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
        public JavaClassName? Parent => GetParent();

        JavaClassName? GetParent()
        {
            var b = value.Span.LastIndexOf('$');
            return b != -1 ? new JavaClassName(value.Slice(0, b)) : null;
        }

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaClassName other) => value.Span.Equals(other.value.Span, StringComparison.Ordinal);

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
