using System;

using IKVM.ByteCode.Extensions;

namespace IKVM.ByteCode.Syntax
{

    /// <summary>
    /// Provides methods to parse a Java package name.
    /// </summary>
    public readonly struct JavaPackageName
    {

        public static JavaPackageName Empty => new(ReadOnlyMemory<char>.Empty);

        public static implicit operator string(JavaPackageName packageName) => packageName.ToString();
        public static implicit operator JavaPackageName(string packageName) => new(packageName);

        public static implicit operator ReadOnlyMemory<char>(JavaPackageName packageName) => packageName.value;
        public static implicit operator JavaPackageName(ReadOnlyMemory<char> packageName) => new(packageName);

        public static bool operator ==(JavaPackageName a, JavaPackageName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal);
        public static bool operator !=(JavaPackageName a, JavaPackageName b) => a.value.Span.Equals(b.value.Span, StringComparison.Ordinal) == false;

        readonly ReadOnlyMemory<char> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="qualifiedName"></param>
        public JavaPackageName(string qualifiedName) :
            this(qualifiedName.AsMemory())
        {
            if (qualifiedName is null)
                throw new ArgumentNullException(nameof(qualifiedName));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="qualifiedName"></param>
        public JavaPackageName(ReadOnlyMemory<char> qualifiedName)
        {
            this.value = qualifiedName;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public JavaPackageName(JavaPackageName parent, ReadOnlyMemory<char> name) :
            this(parent.IsEmpty ? name : $"{parent}.{name}".AsMemory())
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        public JavaPackageName(JavaPackageName parent, string name) :
            this(parent, name.AsMemory())
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the underlying value of this package name.
        /// </summary>
        public ReadOnlyMemory<char> Value => value;

        /// <summary>
        /// Returns <c>true</c> if this package name is empty.
        /// </summary>
        public bool IsEmpty => value.IsEmpty;

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        public JavaSimplePackageName SimpleName => GetSimpleName();

        /// <summary>
        /// Gets the simple name.
        /// </summary>
        /// <returns></returns>
        JavaSimplePackageName GetSimpleName()
        {
            if (IsEmpty)
                return JavaSimplePackageName.Empty;

            var index = value.Span.LastIndexOf('.');
            return index != -1 ? new JavaSimplePackageName(value.Slice(index + 1)) : JavaSimplePackageName.Empty;
        }

        /// <summary>
        /// Gets the name of the parent package, or empty.
        /// </summary>
        public JavaPackageName Parent => GetParent();

        /// <summary>
        /// Gets the package name.
        /// </summary>
        /// <returns></returns>
        JavaPackageName GetParent()
        {
            var i = value.Span.LastIndexOf('.');
            return i != -1 ? new JavaPackageName(value.Slice(0, i)) : Empty;
        }

        /// <summary>
        /// Returns <c>true</c> if this package name is a member of the given package name.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        public bool IsMemberOf(JavaPackageName packageName) => Parent == packageName;

        /// <summary>
        /// Returns <c>true</c> if the given package name is a child of the <paramref name="other"/> package name.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool IsChildOf(JavaPackageName other)
        {
            // the empty package can never be a child of another package
            if (value.IsEmpty)
                return false;

            // but everything is a child of it
            if (other.IsEmpty)
                return true;

            // we're going to enumerate each package element in the other package and test we can advance through that in the target
            var o = new ReadOnlySpanSeperatorEnumerator(other.value.Span, '.');
            var e = new ReadOnlySpanSeperatorEnumerator(value.Span, '.');
            while (o.MoveNext())
            {
                // for each item in the other, we should be able to advance
                if (e.MoveNext() == false)
                    return false;

                // each item must match
                if (e.Current.Equals(o.Current, StringComparison.Ordinal) == false)
                    return false;
            }

            // there must still be more elements
            return e.MoveNext();
        }

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaPackageName other) => value.Span.Equals(other.value.Span, StringComparison.Ordinal);

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => false;

        /// <summary>
        /// Gets the hash code of this package name.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => value.Span.GetHashCodeExtension();

        /// <summary>
        /// Returns a string instance of the package name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => value.Span.ToString();

    }

}
