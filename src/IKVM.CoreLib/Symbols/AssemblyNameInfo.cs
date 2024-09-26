using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class AssemblyNameInfo
    {

        readonly AssemblyNameFlags _flags;
        string? _fullName;

        /// <summary>
        /// Initializes a new instance of the AssemblyNameInfo class.
        /// </summary>
        /// <param name="name">The simple name of the assembly.</param>
        /// <param name="version">The version of the assembly.</param>
        /// <param name="cultureName">The name of the culture associated with the assembly.</param>
        /// <param name="flags">The attributes of the assembly.</param>
        /// <param name="publicKeyOrToken">The public key or its token. Set <paramref name="flags"/> to <seealso cref="AssemblyNameFlags.PublicKey"/> when it's public key.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is null.</exception>
        public AssemblyNameInfo(string name, Version? version = null, string? cultureName = null, AssemblyNameFlags flags = AssemblyNameFlags.None, ImmutableArray<byte> publicKeyOrToken = default)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Version = version;
            CultureName = cultureName;
            _flags = flags;
            PublicKeyOrToken = publicKeyOrToken;
        }

        /// <summary>
        /// Gets the simple name of the assembly.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the version of the assembly.
        /// </summary>
        public Version? Version { get; }

        /// <summary>
        /// Gets the name of the culture associated with the assembly.
        /// </summary>
        /// <remarks>
        /// Do not create a <see cref="System.Globalization.CultureInfo"/> instance from this string unless
        /// you know the string has originated from a trustworthy source.
        /// </remarks>
        public string? CultureName { get; }

        /// <summary>
        /// Gets the attributes of the assembly.
        /// </summary>
        public AssemblyNameFlags Flags => _flags;

        /// <summary>
        /// Gets the public key or the public key token of the assembly.
        /// </summary>
        /// <remarks>Check <seealso cref="Flags"/> for <seealso cref="AssemblyNameFlags.PublicKey"/> flag to see whether it's public key or its token.</remarks>
#if SYSTEM_PRIVATE_CORELIB
        public byte[]? PublicKeyOrToken { get; }
#else
        public ImmutableArray<byte> PublicKeyOrToken { get; }
#endif


        /// <summary>
        /// Gets the full name of the assembly, also known as the display name.
        /// </summary>
        /// <remarks>In contrary to <seealso cref="AssemblyName.FullName"/> it does not validate public key token neither computes it based on the provided public key.</remarks>
        public string FullName
        {
            get
            {
                if (_fullName is null)
                {
                    bool isPublicKey = (Flags & AssemblyNameFlags.PublicKey) != 0;

                    byte[]? publicKeyOrToken =
#if SYSTEM_PRIVATE_CORELIB
                    PublicKeyOrToken;
#elif NET8_0_OR_GREATER
                    !PublicKeyOrToken.IsDefault ? System.Runtime.InteropServices.ImmutableCollectionsMarshal.AsArray(PublicKeyOrToken) : null;
#else
                    !PublicKeyOrToken.IsDefault ? PublicKeyOrToken.ToArray() : null;
#endif
                    _fullName = AssemblyNameFormatter.ComputeDisplayName(Name, Version, CultureName,
                        pkt: isPublicKey ? null : publicKeyOrToken,
                        ExtractAssemblyNameFlags(_flags), ExtractAssemblyContentType(_flags),
                        pk: isPublicKey ? publicKeyOrToken : null);
                }

                return _fullName;
            }
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
            assemblyName.ContentType = ExtractAssemblyContentType(_flags);
#pragma warning disable SYSLIB0037 // Type or member is obsolete
            assemblyName.ProcessorArchitecture = ExtractProcessorArchitecture(_flags);
#pragma warning restore SYSLIB0037 // Type or member is obsolete

#if SYSTEM_PRIVATE_CORELIB
            if (PublicKeyOrToken is not null)
            {
                if ((Flags & AssemblyNameFlags.PublicKey) != 0)
                {
                    assemblyName.SetPublicKey(PublicKeyOrToken);
                }
                else
                {
                    assemblyName.SetPublicKeyToken(PublicKeyOrToken);
                }
            }
#else
            if (!PublicKeyOrToken.IsDefault)
            {
                // A copy of the array needs to be created, as AssemblyName allows for the mutation of provided array.
                if ((Flags & AssemblyNameFlags.PublicKey) != 0)
                {
                    assemblyName.SetPublicKey(PublicKeyOrToken.ToArray());
                }
                else
                {
                    assemblyName.SetPublicKeyToken(PublicKeyOrToken.ToArray());
                }
            }
#endif

            return assemblyName;
        }

        internal static AssemblyNameFlags ExtractAssemblyNameFlags(AssemblyNameFlags combinedFlags)
            => combinedFlags & unchecked((AssemblyNameFlags)0xFFFFF10F);

        internal static AssemblyContentType ExtractAssemblyContentType(AssemblyNameFlags flags)
            => (AssemblyContentType)((((int)flags) >> 9) & 0x7);

        internal static ProcessorArchitecture ExtractProcessorArchitecture(AssemblyNameFlags flags)
            => (ProcessorArchitecture)((((int)flags) >> 4) & 0x7);

    }

}
