using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents an identity of an assembly as defined by CLI metadata specification.
    /// </summary>
    class AssemblyIdentity
    {

        /// <summary>
        /// Determines whether two <see cref="AssemblyIdentity"/> instances are equal.
        /// </summary>
        /// <param name="left">The operand appearing on the left side of the operator.</param>
        /// <param name="right">The operand appearing on the right side of the operator.</param>
        public static bool operator ==(AssemblyIdentity? left, AssemblyIdentity? right)
        {
            return EqualityComparer<AssemblyIdentity>.Default.Equals(left!, right!);
        }

        /// <summary>
        /// Determines whether two <see cref="AssemblyIdentity"/> instances are not equal.
        /// </summary>
        /// <param name="left">The operand appearing on the left side of the operator.</param>
        /// <param name="right">The operand appearing on the right side of the operator.</param>
        public static bool operator !=(AssemblyIdentity? left, AssemblyIdentity? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns true (false) if specified assembly identities are (not) equal 
        /// regardless of unification, retargeting or other assembly binding policies. 
        /// Returns null if these policies must be consulted to determine name equivalence.
        /// </summary>
        public static bool? MemberwiseEqual(AssemblyIdentity x, AssemblyIdentity y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (!AssemblyIdentityComparer.SimpleNameComparer.Equals(x._name, y._name))
                return false;

            if (x._version.Equals(y._version) && EqualIgnoringNameAndVersion(x, y))
                return true;

            return null;
        }

        /// <summary>
        /// Returns <c>true</c> if the components of the assembly names, other than name and version, are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool EqualIgnoringNameAndVersion(AssemblyIdentity x, AssemblyIdentity y)
        {
            return x.ContentType == y.ContentType && AssemblyIdentityComparer.CultureComparer.Equals(x.CultureName, y.CultureName) && KeysEqual(x, y);
        }

        /// <summary>
        /// Returns <c>true</c> if the public keys of both identities are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool KeysEqual(AssemblyIdentity x, AssemblyIdentity y)
        {
            var xToken = x._publicKeyToken;
            var yToken = y._publicKeyToken;

            // weak names or both strong names with initialized PKT - compare tokens:
            if (!xToken.IsDefault && !yToken.IsDefault)
                return xToken.SequenceEqual(yToken);

            // both are strong names with uninitialized PKT - compare full keys:
            if (xToken.IsDefault && yToken.IsDefault)
                return x._publicKey.SequenceEqual(y._publicKey);

            // one of the strong names doesn't have PK, other doesn't have PTK initialized.
            if (xToken.IsDefault)
                return x.PublicKeyToken.SequenceEqual(yToken);
            else
                return xToken.SequenceEqual(y.PublicKeyToken);
        }

        /// <summary>
        /// Initializes the publickey and publickeytoken values.
        /// </summary>
        /// <param name="publicKeyOrToken"></param>
        /// <param name="hasPublicKey"></param>
        /// <param name="publicKey"></param>
        /// <param name="publicKeyToken"></param>
        static void InitializeKey(ImmutableArray<byte> publicKeyOrToken, bool hasPublicKey, out ImmutableArray<byte> publicKey, out ImmutableArray<byte> publicKeyToken)
        {
            if (hasPublicKey)
            {
                publicKey = publicKeyOrToken;
                publicKeyToken = default;
            }
            else
            {
                publicKey = ImmutableArray<byte>.Empty;
                publicKeyToken = publicKeyOrToken.IsDefault ? ImmutableArray<byte>.Empty : publicKeyOrToken;
            }
        }

        /// <summary>
        /// Calculates the public key token from the given public key.
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        static ImmutableArray<byte> CalculatePublicKeyToken(ImmutableArray<byte> publicKey)
        {
            var hash = SHA1.Create().ComputeHash(publicKey.ToArray());

            // SHA1 hash is always 160 bits:
            Debug.Assert(hash.Length == 20);

            // PublicKeyToken is the low 64 bits of the SHA-1 hash of the public key.
            int l = hash.Length - 1;
            var result = ImmutableArray.CreateBuilder<byte>(8);
            for (int i = 0; i < 8; i++)
                result.Add(hash[l - i]);

            return result.ToImmutable();
        }

        /// <summary>
        /// Parses a span of characters into a assembly name.
        /// </summary>
        /// <param name="assemblyName">A span containing the characters representing the assembly name to parse.</param>
        /// <returns>Parsed type name.</returns>
        /// <exception cref="ArgumentException">Provided assembly name was invalid.</exception>
        public static AssemblyIdentity Parse(ReadOnlySpan<char> assemblyName) => TryParse(assemblyName, out AssemblyIdentity? result) ? result! : throw new ArgumentException("Invalid assembly name.", nameof(assemblyName));

        /// <summary>
        /// Tries to parse a span of characters into an assembly name.
        /// </summary>
        /// <param name="assemblyName">A span containing the characters representing the assembly name to parse.</param>
        /// <param name="result">Contains the result when parsing succeeds.</param>
        /// <returns>true if assembly name was converted successfully, otherwise, false.</returns>
        public static bool TryParse(ReadOnlySpan<char> assemblyName, [NotNullWhen(true)] out AssemblyIdentity? result)
        {
            AssemblyIdentityParser.AssemblyIdentityParts parts = default;
            if (!assemblyName.IsEmpty && AssemblyIdentityParser.TryParse(assemblyName, ref parts))
            {
                result = new(parts._name, parts._version, parts._cultureName, parts._publicKeyOrToken?.ToImmutableArray() ?? [], parts._publicKeyOrToken != null);
                return true;
            }

            result = null;
            return false;
        }

        readonly string _name;
        readonly Version _version;
        readonly string? _cultureName;
        readonly AssemblyContentType _contentType;
        readonly ProcessorArchitecture _processorArchitecture;

        string? _fullName;
        ImmutableArray<byte> _publicKey;
        ImmutableArray<byte> _publicKeyToken;
        int _hashCode = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyIdentity"/> class.
        /// </summary>
        /// <param name="name">The simple name of the assembly.</param>
        /// <param name="version">The version of the assembly.</param>
        /// <param name="cultureName">The name of the culture associated with the assembly.</param>
        /// <param name="publicKeyOrToken">The public key or its token.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public AssemblyIdentity(string name, Version? version = null, string? cultureName = null, ImmutableArray<byte> publicKeyOrToken = default, bool hasPublicKey = false, AssemblyContentType contentType = default, ProcessorArchitecture processorArchitecture = ProcessorArchitecture.None)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _version = version ?? new Version(0, 0, 0, 0);
            _cultureName = cultureName;
            _contentType = contentType;
            _processorArchitecture = processorArchitecture;
            InitializeKey(publicKeyOrToken, hasPublicKey, out _publicKey, out _publicKeyToken);
        }

        /// <summary>
        /// Gets the simple name of the assembly.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        public Version? Version => _version;

        /// <summary>
        /// Gets the name of the culture associated with the assembly.
        /// </summary>
        /// <remarks>
        /// Do not create a <see cref="System.Globalization.CultureInfo"/> instance from this string unless
        /// you know the string has originated from a trustworthy source.
        /// </remarks>
        public string? CultureName => _cultureName;

        /// <summary>
        /// Returns <c>true</c> if a public key is available.
        /// </summary>
        public bool HasPublicKey => _publicKey.Length > 0;

        /// <summary>
        /// Gets the public key or the public key token of the assembly.
        /// </summary>
        public ImmutableArray<byte> PublicKey => _publicKey;

        /// <summary>
        /// Low 8 bytes of SHA1 hash of the public key, or empty.
        /// </summary>
        public ImmutableArray<byte> PublicKeyToken
        {
            get
            {
                if (_publicKeyToken.IsDefault)
                    ImmutableInterlocked.InterlockedCompareExchange(ref _publicKeyToken, CalculatePublicKeyToken(_publicKey), default);

                return _publicKeyToken;
            }
        }

        /// <summary>
        /// Gets the full name of the assembly, also known as the display name.
        /// </summary>
        /// <remarks>In contrary to <seealso cref="AssemblyName.FullName"/> it does not validate public key token neither computes it based on the provided public key.</remarks>
        public string FullName => _fullName ??= AssemblyNameFormatter.ComputeDisplayName(Name, Version, CultureName, PublicKeyToken, Flags, ContentType, PublicKey);

        /// <summary>
        /// Gets the <see cref="AssemblyNameFlags"/>.
        /// </summary>
        public AssemblyNameFlags Flags => HasPublicKey ? AssemblyNameFlags.PublicKey : AssemblyNameFlags.None;

        /// <summary>
        /// Specifies the binding model for how this object will be treated in comparisons.
        /// </summary>
        public AssemblyContentType ContentType => _contentType;

        /// <summary>
        /// Gets the processor architecture of the assembly name.
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture => _processorArchitecture;

        /// <summary>
        /// True if the assembly identity has a strong name, ie. either a full public key or a token.
        /// </summary>
        public bool IsStrongName => HasPublicKey || _publicKeyToken.Length > 0;

        /// <summary>
        /// Determines whether the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to be compared with the current instance.</param>
        public bool Equals(AssemblyIdentity? obj)
        {
            return !ReferenceEquals(obj, null) && (_hashCode == 0 || obj._hashCode == 0 || _hashCode == obj._hashCode) && MemberwiseEqual(this, obj) == true;
        }

        /// <summary>
        /// Determines whether the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="obj">The object to be compared with the current instance.</param>
        public override bool Equals(object? obj)
        {
            return Equals(obj as AssemblyIdentity);
        }

        /// <summary>
        /// Returns the hash code for the current instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (_hashCode == 0)
            {
                // Do not include PK/PKT in the hash - collisions on PK/PKT are rare (assembly identities differ only in PKT/PK)
                // and we can't calculate hash of PKT if only PK is available
                _hashCode =
                    HashCode.Combine(AssemblyIdentityComparer.SimpleNameComparer.GetHashCode(_name),
                    HashCode.Combine(_version?.GetHashCode(), GetHashCodeIgnoringNameAndVersion()));
            }

            return _hashCode;
        }

        /// <summary>
        /// Gets the hashcode for this instance, without considering the name and version.
        /// </summary>
        /// <returns></returns>
        int GetHashCodeIgnoringNameAndVersion()
        {
            return HashCode.Combine((int)_contentType, AssemblyIdentityComparer.CultureComparer.GetHashCode(_cultureName ?? ""));
        }

        /// <summary>
        /// Initializes a new instance of the <seealso cref="AssemblyName"/> class based on the stored information.
        /// </summary>
        /// <remarks>
        /// Do not create an <see cref="AssemblyName"/> instance with <see cref="CultureName"/> string unless
        /// you know the string has originated from a trustworthy source.
        /// </remarks>
        public AssemblyName ToAssemblyName()
        {
            AssemblyName assemblyName = new();
            assemblyName.Name = Name;
            assemblyName.CultureName = CultureName;
            assemblyName.Version = Version;
            assemblyName.Flags = Flags;
            assemblyName.ContentType = ContentType;
#pragma warning disable SYSLIB0037 // Type or member is obsolete
            assemblyName.ProcessorArchitecture = ProcessorArchitecture;
#pragma warning restore SYSLIB0037 // Type or member is obsolete

            if (HasPublicKey)
                assemblyName.SetPublicKey(PublicKey.ToArray());
            else if (PublicKeyToken.IsDefaultOrEmpty == false)
                assemblyName.SetPublicKeyToken(PublicKeyToken.ToArray());

            return assemblyName;
        }

    }

}
