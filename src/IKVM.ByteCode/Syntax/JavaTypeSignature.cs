using System;

using IKVM.ByteCode.Extensions;

namespace IKVM.ByteCode.Syntax
{

    /// <summary>
    /// Describes a Java type signature.
    /// </summary>
    public readonly struct JavaTypeSignature
    {

        const char ObjectTypeSpec = 'L';
        const char ObjectTypeTerm = ';';

        public static JavaTypeSignature Boolean = Parse("Z");
        public static JavaTypeSignature Byte = Parse("B");
        public static JavaTypeSignature Char = Parse("C");
        public static JavaTypeSignature Short = Parse("S");
        public static JavaTypeSignature Int = Parse("I");
        public static JavaTypeSignature Long = Parse("J");
        public static JavaTypeSignature Float = Parse("F");
        public static JavaTypeSignature Double = Parse("D");
        public static JavaTypeSignature Void = Parse("V");

        public static implicit operator JavaTypeSignature(JavaClassName name) => Parse(name.Value);

        public static implicit operator string(JavaTypeSignature signature) => signature.ToString();
        public static implicit operator JavaTypeSignature(string signature) => Parse(signature.AsMemory());

        public static implicit operator ReadOnlyMemory<char>(JavaTypeSignature signature) => signature.memory;
        public static implicit operator JavaTypeSignature(ReadOnlyMemory<char> signature) => Parse(signature);

        public static implicit operator ReadOnlySpan<char>(JavaTypeSignature signature) => signature.memory.Span;
        public static implicit operator JavaTypeSignature(ReadOnlySpan<char> signature) => Parse(signature.ToArray());

        public static bool operator ==(JavaTypeSignature a, JavaTypeSignature b) => a.memory.Equals(b.memory);
        public static bool operator !=(JavaTypeSignature a, JavaTypeSignature b) => !(a == b);

        /// <summary>
        /// Parses the string into a <see cref="JavaTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JavaTypeSignature Parse(JavaClassName value)
        {
            return new JavaTypeSignature(value.Value);
        }

        /// <summary>
        /// Parses the buffer into a <see cref="JavaTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static JavaTypeSignature Parse(ReadOnlyMemory<char> value)
        {
            return new JavaTypeSignature(value);
        }

        /// <summary>
        /// Parses the string into a <see cref="JavaTypeSignature"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JavaTypeSignature Parse(string value)
        {
            return Parse(string.Intern(value).AsMemory());
        }

        readonly ReadOnlyMemory<char> memory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="arrayRank"></param>
        JavaTypeSignature(ReadOnlyMemory<char> memory)
        {
            this.memory = memory;
        }

        /// <summary>
        /// Gets the underlying memory of the type signature.
        /// </summary>
        public ReadOnlyMemory<char> Memory => memory;

        /// <summary>
        /// Gets the base type of the type signature, or null if the signature represents an object or void.
        /// </summary>
        public JavaPrimitiveTypeName? BaseTypeName => GetBaseTypeName();

        JavaPrimitiveTypeName? GetBaseTypeName()
        {
            var r = GetArrayRank();
            return memory.Span[r] != ObjectTypeSpec ? (JavaPrimitiveTypeName)memory.Span[r] : null;
        }

        /// <summary>
        /// Returns <c>true</c> if the signature represents an object.
        /// </summary>
        public bool IsObject => memory.Span[GetArrayRank()] == 'L';

        /// <summary>
        /// Returns <c>true</c> if the signature repreesnts an array.
        /// </summary>
        public bool IsArray => GetArrayRank() > 0;

        /// <summary>
        /// Gets the name of the type described by the signature.
        /// </summary>
        public JavaClassName? ClassName => GetClassName();

        /// <summary>
        /// Gets the name of the type described by the signature.
        /// </summary>
        JavaClassName? GetClassName()
        {
            var r = GetArrayRank();
            if (memory.Span[r] != ObjectTypeSpec)
                throw new NotSupportedException($"JavaTypeSignature does not begin with {ObjectTypeSpec}.");

            // slice starting at end of array spec
            var s = memory.Slice(r + 1);
            var z = MemoryExtensions.IndexOf(s.Span, ObjectTypeTerm);

            // check that we terminate with the right character
            if (s.Span[z] != ObjectTypeTerm)
                throw new FormatException($"Class signature does not terminate with '{ObjectTypeTerm}'.");

            return JavaClassName.Parse(s.Slice(0, z));
        }

        /// <summary>
        /// Gets the array rank of the type.
        /// </summary>
        public int ArrayRank => GetArrayRank();

        /// <summary>
        /// Gets the array rank of the type.
        /// </summary>
        /// <returns></returns>
        int GetArrayRank()
        {
            var i = 0;
            while (memory.Span[i] == '[') i++;
            return i;
        }

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(JavaTypeSignature other) => MemoryExtensions.Equals(memory.Span, other.memory.Span, StringComparison.Ordinal);

        /// <summary>
        /// Returns <c>true</c> if the two objects are equal.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) => false;

        /// <summary>
        /// Gets the hash code of this signature.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => memory.Span.GetHashCodeExtension();

        /// <summary>
        /// Returns a string instance of the package name.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => memory.Span.ToString();

    }

}
