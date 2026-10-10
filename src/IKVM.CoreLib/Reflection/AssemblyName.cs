/*
  Copyright (C) 2009-2012 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Globalization;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;

using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{

    /// <summary>
    /// Describes an assembly's unique identity in full.
    /// </summary>
    sealed class AssemblyName :
        ICloneable
    {

        string _name;
        string _culture;
        Version _version;
        byte[] _publicKeyToken;
        byte[] _publicKey;
        StrongNameKeyPair _keyPair;
        AssemblyNameFlags _flags;
        AssemblyHashAlgorithm _hashAlgorithm;
        string _codeBase;

        /// <summary>
        /// The hash value of the referenced assembly, as stored in an AssemblyRef row, or <see langword="null"/> if there is none.
        /// </summary>
        internal byte[] Hash;

        /// <summary>
        /// Initializes a new instance with no name set.
        /// </summary>
        public AssemblyName()
        {
        }

        /// <summary>
        /// Initializes a new instance with the specified display name.
        /// </summary>
        /// <param name="assemblyName">The display name of the assembly, as returned by the <see cref="FullName"/> property.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assemblyName"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="assemblyName"/> is a zero length string.</exception>
        /// <exception cref="FileLoadException"><paramref name="assemblyName"/> is not a valid assembly display name.</exception>
        public AssemblyName(string assemblyName)
        {
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");
            if (assemblyName == "")
                throw new ArgumentException();

            switch (Fusion.ParseAssemblyName(assemblyName, out var parsed))
            {
                case ParseAssemblyResult.GenericError:
                case ParseAssemblyResult.DuplicateKey:
                    throw new FileLoadException();
            }

            if (!ParseVersion(parsed.Version, parsed.Retargetable.HasValue, out _version))
                throw new FileLoadException();

            _name = parsed.Name;
            if (parsed.Culture != null)
            {
                if (parsed.Culture.Equals("neutral", StringComparison.OrdinalIgnoreCase))
                    _culture = "";
                else if (parsed.Culture == "")
                    throw new FileLoadException();
                else
                    _culture = new CultureInfo(parsed.Culture).Name;
            }

            if (parsed.PublicKeyToken != null)
            {
                if (parsed.PublicKeyToken.Equals("null", StringComparison.OrdinalIgnoreCase))
                    _publicKeyToken = Array.Empty<byte>();
                else if (parsed.PublicKeyToken.Length != 16)
                    throw new FileLoadException();
                else
                    _publicKeyToken = ParseKey(parsed.PublicKeyToken);
            }

            if (parsed.Retargetable.HasValue)
            {
                if (parsed.Culture == null || parsed.PublicKeyToken == null || _version == null)
                    throw new FileLoadException();
                if (parsed.Retargetable.Value)
                    _flags |= AssemblyNameFlags.Retargetable;
            }

            ProcessorArchitecture = parsed.ProcessorArchitecture;
            if (parsed.WindowsRuntime)
                ContentType = AssemblyContentType.WindowsRuntime;
        }

        /// <summary>
        /// Parses a hexadecimal string into the bytes of a public key or public key token.
        /// </summary>
        /// <param name="key">The hexadecimal string.</param>
        /// <returns>The parsed bytes.</returns>
        /// <exception cref="FileLoadException"><paramref name="key"/> has an odd length or contains a character that is not a hexadecimal digit.</exception>
        static byte[] ParseKey(string key)
        {
            if ((key.Length & 1) != 0)
                throw new FileLoadException();

            var buf = new byte[key.Length / 2];
            for (var i = 0; i < buf.Length; i++)
                buf[i] = (byte)(ParseHexDigit(key[i * 2]) * 16 + ParseHexDigit(key[i * 2 + 1]));

            return buf;
        }

        /// <summary>
        /// Parses a single hexadecimal digit, ignoring case.
        /// </summary>
        /// <param name="digit">The character to parse.</param>
        /// <returns>The value of the digit, from 0 to 15.</returns>
        /// <exception cref="FileLoadException"><paramref name="digit"/> is not a hexadecimal digit.</exception>
        static int ParseHexDigit(char digit)
        {
            if (digit >= '0' && digit <= '9')
            {
                return digit - '0';
            }
            else
            {
                digit |= (char)0x20;
                if (digit >= 'a' && digit <= 'f')
                    return 10 + digit - 'a';
                else
                    throw new FileLoadException();
            }
        }

        /// <summary>
        /// Returns the full name of the assembly, also known as the display name.
        /// </summary>
        /// <returns>The full name of the assembly, or an empty string if the name has not been set.</returns>
        public override string ToString() => FullName;

        /// <summary>
        /// Gets or sets the simple name of the assembly. This is usually, but not necessarily, the file name of the manifest
        /// file of the assembly, minus its extension.
        /// </summary>
        public string Name
        {
            get => _name;
            set => _name = value;
        }

        /// <summary>
        /// Gets or sets the culture supported by the assembly.
        /// </summary>
        public CultureInfo CultureInfo
        {
            get => _culture == null ? null : new CultureInfo(_culture);
            set => _culture = value?.Name;
        }

        /// <summary>
        /// Gets or sets the name of the culture associated with the assembly; an empty string denotes the neutral culture.
        /// </summary>
        public string CultureName
        {
            get => _culture;
            set => _culture = value;
        }

        /// <summary>
        /// Gets or sets the major, minor, build, and revision numbers of the assembly.
        /// </summary>
        public Version Version
        {
            get => _version;
            set => _version = value;
        }

        /// <summary>
        /// Gets or sets the public and private cryptographic key pair that is used to create a strong name signature for the assembly.
        /// </summary>
        public StrongNameKeyPair KeyPair
        {
            get => _keyPair;
            set => _keyPair = value;
        }

        /// <summary>
        /// Gets or sets the location of the assembly as a URL.
        /// </summary>
        public string CodeBase
        {
            get => _codeBase;
            set => _codeBase = value;
        }

        /// <summary>
        /// Gets or sets a value that identifies the processor and bits-per-word of the platform targeted by an executable.
        /// Values outside the defined range are ignored.
        /// </summary>
        public ProcessorArchitecture ProcessorArchitecture
        {
            get => (ProcessorArchitecture)(((int)_flags & 0x70) >> 4);
            set
            {
                if (value >= ProcessorArchitecture.None && value <= ProcessorArchitecture.Arm64)
                    _flags = (_flags & ~(AssemblyNameFlags)0x70) | (AssemblyNameFlags)((int)value << 4);
            }
        }

        /// <summary>
        /// Gets or sets the attributes of the assembly, excluding the processor architecture and content type bits, which are
        /// exposed through <see cref="ProcessorArchitecture"/> and <see cref="ContentType"/>.
        /// </summary>
        public AssemblyNameFlags Flags
        {
            get => _flags & (AssemblyNameFlags)~0xEF0;
            set => _flags = (_flags & (AssemblyNameFlags)0xEF0) | (value & (AssemblyNameFlags)~0xEF0);
        }

        /// <summary>
        /// Gets or sets a value that indicates what type of content the assembly contains. Values outside the defined range are ignored.
        /// </summary>
        public AssemblyContentType ContentType
        {
            get => (AssemblyContentType)(((int)_flags & 0xE00) >> 9);
            set
            {
                if (value >= AssemblyContentType.Default && value <= AssemblyContentType.WindowsRuntime)
                    _flags = (_flags & ~(AssemblyNameFlags)0xE00) | (AssemblyNameFlags)((int)value << 9);
            }
        }

        /// <summary>
        /// Gets the public key of the assembly.
        /// </summary>
        /// <returns>A byte array that contains the public key of the assembly, or <see langword="null"/> if none has been set.</returns>
        public byte[] GetPublicKey() => _publicKey;

        /// <summary>
        /// Sets the public key identifying the assembly, and sets or clears <see cref="AssemblyNameFlags.PublicKey"/> accordingly.
        /// </summary>
        /// <param name="publicKey">A byte array containing the public key of the assembly, or <see langword="null"/>.</param>
        public void SetPublicKey(byte[] publicKey)
        {
            _publicKey = publicKey;
            _flags = (_flags & ~AssemblyNameFlags.PublicKey) | (publicKey == null ? 0 : AssemblyNameFlags.PublicKey);
        }

        /// <summary>
        /// Gets the public key token, which is the last 8 bytes of the SHA-1 hash of the public key under which the application
        /// or assembly is signed.
        /// </summary>
        /// <returns>A byte array that contains the public key token, an empty array if the assembly has no public key token, or
        /// <see langword="null"/> if neither a public key nor a public key token has been set.</returns>
        public byte[] GetPublicKeyToken()
        {
            if (_publicKeyToken == null && _publicKey != null)
            {
                // note that GetPublicKeyToken() has a side effect in this case, because we retain this token even after the public key subsequently gets changed
                _publicKeyToken = ComputePublicKeyToken(_publicKey);
            }

            return _publicKeyToken;
        }

        /// <summary>
        /// Sets the public key token, which is the last 8 bytes of the SHA-1 hash of the public key under which the application
        /// or assembly is signed.
        /// </summary>
        /// <param name="publicKeyToken">A byte array containing the public key token of the assembly.</param>
        public void SetPublicKeyToken(byte[] publicKeyToken) => _publicKeyToken = publicKeyToken;

        /// <summary>
        /// Gets or sets the hash algorithm used by the assembly manifest.
        /// </summary>
        public AssemblyHashAlgorithm HashAlgorithm
        {
            get => _hashAlgorithm;
            set => _hashAlgorithm = value;
        }

        /// <summary>
        /// Gets the full name of the assembly, also known as the display name.
        /// </summary>
        public string FullName
        {
            get
            {
                if (_name == null)
                    return "";

                ushort versionMajor = 0xFFFF;
                ushort versionMinor = 0xFFFF;
                ushort versionBuild = 0xFFFF;
                ushort versionRevision = 0xFFFF;

                if (_version != null)
                {
                    versionMajor = (ushort)_version.Major;
                    versionMinor = (ushort)_version.Minor;
                    versionBuild = (ushort)_version.Build;
                    versionRevision = (ushort)_version.Revision;
                }

                var publicKeyToken = _publicKeyToken;
                if ((publicKeyToken == null || publicKeyToken.Length == 0) && _publicKey != null)
                    publicKeyToken = ComputePublicKeyToken(_publicKey);

                return GetFullName(_name, versionMajor, versionMinor, versionBuild, versionRevision, _culture, publicKeyToken, (int)_flags);
            }
        }

        /// <summary>
        /// Formats an assembly display name from its components, quoting and escaping the simple name where required.
        /// </summary>
        /// <param name="name">The simple name of the assembly.</param>
        /// <param name="versionMajor">The major version, or 0xFFFF to omit the version.</param>
        /// <param name="versionMinor">The minor version, or 0xFFFF to omit it and the following components.</param>
        /// <param name="versionBuild">The build number, or 0xFFFF to omit it and the following component.</param>
        /// <param name="versionRevision">The revision number, or 0xFFFF to omit it.</param>
        /// <param name="culture">The culture name, an empty string for the neutral culture, or <see langword="null"/> to omit it.</param>
        /// <param name="publicKeyToken">The public key token, an empty array for <c>null</c>, or <see langword="null"/> to omit it.</param>
        /// <param name="flags">The raw assembly name flags, used for the Retargetable and ContentType components.</param>
        /// <returns>The display name.</returns>
        internal static string GetFullName(string name, ushort versionMajor, ushort versionMinor, ushort versionBuild, ushort versionRevision, string culture, byte[] publicKeyToken, int flags)
        {
            var sb = new StringBuilder();
            var doubleQuotes = name.StartsWith(" ") || name.EndsWith(" ") || name.IndexOf('\'') != -1;
            var singleQuotes = name.IndexOf('"') != -1;
            if (singleQuotes)
                sb.Append('\'');
            else if (doubleQuotes)
                sb.Append('"');

            if (name.IndexOf(',') != -1 || name.IndexOf('\\') != -1 || name.IndexOf('=') != -1 || (singleQuotes && name.IndexOf('\'') != -1))
            {
                for (var i = 0; i < name.Length; i++)
                {
                    var c = name[i];
                    if (c == ',' || c == '\\' || c == '=' || (singleQuotes && c == '\''))
                    {
                        sb.Append('\\');
                    }
                    sb.Append(c);
                }
            }
            else
            {
                sb.Append(name);
            }
            if (singleQuotes)
            {
                sb.Append('\'');
            }
            else if (doubleQuotes)
            {
                sb.Append('"');
            }
            if (versionMajor != 0xFFFF)
            {
                sb.Append(", Version=").Append(versionMajor);
                if (versionMinor != 0xFFFF)
                {
                    sb.Append('.').Append(versionMinor);
                    if (versionBuild != 0xFFFF)
                    {
                        sb.Append('.').Append(versionBuild);
                        if (versionRevision != 0xFFFF)
                        {
                            sb.Append('.').Append(versionRevision);
                        }
                    }
                }
            }
            if (culture != null)
            {
                sb.Append(", Culture=").Append(culture == "" ? "neutral" : culture);
            }
            if (publicKeyToken != null)
            {
                sb.Append(", PublicKeyToken=");
                if (publicKeyToken.Length == 0)
                {
                    sb.Append("null");
                }
                else
                {
                    AppendPublicKey(sb, publicKeyToken);
                }
            }
            if ((flags & (int)AssemblyNameFlags.Retargetable) != 0)
            {
                sb.Append(", Retargetable=Yes");
            }
            if ((AssemblyContentType)((flags & 0xE00) >> 9) == AssemblyContentType.WindowsRuntime)
            {
                sb.Append(", ContentType=WindowsRuntime");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Computes the public key token of a public key: the last 8 bytes of its SHA-1 hash, in reverse order.
        /// </summary>
        /// <param name="publicKey">The public key.</param>
        /// <returns>The public key token, or <paramref name="publicKey"/> itself if it is empty.</returns>
        internal static byte[] ComputePublicKeyToken(byte[] publicKey)
        {
            if (publicKey.Length == 0)
                return publicKey;

            byte[] hash;
            using (SHA1 sha1 = SHA1.Create())
                hash = sha1.ComputeHash(publicKey);

            var token = new byte[8];
            for (var i = 0; i < token.Length; i++)
                token[i] = hash[hash.Length - 1 - i];

            return token;
        }

        /// <summary>
        /// Computes the public key token of a public key given as a hexadecimal string.
        /// </summary>
        /// <param name="publicKey">The public key as a hexadecimal string.</param>
        /// <returns>The public key token as a lower-case hexadecimal string.</returns>
        internal static string ComputePublicKeyToken(string publicKey)
        {
            var sb = new StringBuilder(16);
            AppendPublicKey(sb, ComputePublicKeyToken(ParseKey(publicKey)));
            return sb.ToString();
        }

        /// <summary>
        /// Appends the bytes of a public key or public key token to <paramref name="sb"/> as lower-case hexadecimal digits.
        /// </summary>
        /// <param name="sb">The string builder to append to.</param>
        /// <param name="publicKey">The bytes to append.</param>
        static void AppendPublicKey(StringBuilder sb, byte[] publicKey)
        {
            for (var i = 0; i < publicKey.Length; i++)
            {
                sb.Append("0123456789abcdef"[publicKey[i] >> 4]);
                sb.Append("0123456789abcdef"[publicKey[i] & 0x0F]);
            }
        }

        /// <summary>
        /// Determines whether the specified object is an <see cref="AssemblyName"/> with the same full name.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="AssemblyName"/> with an equal <see cref="FullName"/>; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is AssemblyName other && other.FullName == FullName;

        /// <summary>
        /// Returns the hash code of the full name.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => FullName.GetHashCode();

        /// <summary>
        /// Makes a copy of this <see cref="AssemblyName"/> object.
        /// </summary>
        /// <returns>An object that is a copy of this <see cref="AssemblyName"/> object.</returns>
        public object Clone()
        {
            var copy = (AssemblyName)MemberwiseClone();
            copy._publicKey = Copy(_publicKey);
            copy._publicKeyToken = Copy(_publicKeyToken);
            return copy;
        }

        /// <summary>
        /// Copies a byte array, returning <see langword="null"/> and empty arrays as they are.
        /// </summary>
        /// <param name="b">The array to copy.</param>
        /// <returns>A copy of <paramref name="b"/>, or <paramref name="b"/> itself if it is <see langword="null"/> or empty.</returns>
        static byte[] Copy(byte[] b) => b == null || b.Length == 0 ? b : (byte[])b.Clone();

        /// <summary>
        /// Gets the <see cref="AssemblyName"/> for a given file.
        /// </summary>
        /// <param name="assemblyFile">The path for the assembly whose <see cref="AssemblyName"/> is to be returned.</param>
        /// <returns>An object that represents the given assembly file.</returns>
        /// <exception cref="BadImageFormatException"><paramref name="assemblyFile"/> is not a valid assembly.</exception>
        /// <exception cref="FileNotFoundException"><paramref name="assemblyFile"/> cannot be opened.</exception>
        public static AssemblyName GetAssemblyName(string assemblyFile)
        {
            try
            {
                using var st = new FileStream(Path.GetFullPath(assemblyFile), FileMode.Open, FileAccess.Read, FileShare.Read);
                using var pe = new System.Reflection.PortableExecutable.PEReader(st);
                var mr = pe.GetMetadataReader();

                if (mr.IsAssembly == false)
                    throw new BadImageFormatException("Module does not contain a manifest");

                var nm = mr.GetAssemblyDefinition().Name;
                if (nm.IsNil)
                    throw new BadImageFormatException("Module does not contain a manifest");

                return new AssemblyName(mr.GetString(nm));
            }
            catch (IOException e)
            {
                throw new FileNotFoundException(e.Message, e);
            }
            catch (UnauthorizedAccessException e)
            {
                throw new FileNotFoundException(e.Message, e);
            }
        }

        /// <summary>
        /// Gets or sets all assembly name flags, including the processor architecture and content type bits.
        /// </summary>
        internal AssemblyNameFlags RawFlags
        {
            get => _flags;
            set => _flags = value;
        }

        /// <summary>
        /// Parses the version component of an assembly display name.
        /// </summary>
        /// <param name="str">The version string, or <see langword="null"/> if the display name has no version.</param>
        /// <param name="mustBeComplete"><see langword="true"/> if the version must have all four components to be used, as is the case when the name is retargetable.</param>
        /// <param name="version">When this method returns, the parsed version, or <see langword="null"/> if there is no usable version.</param>
        /// <returns><see langword="false"/> if the version is invalid enough to fail the parse of the whole assembly name; otherwise, <see langword="true"/>.</returns>
        static bool ParseVersion(string str, bool mustBeComplete, out Version version)
        {
            version = null;

            if (str == null)
                return true;

            var parts = str.Split('.');
            if (parts.Length < 2 || parts.Length > 4)
            {
                // if the version consists of a single integer, it is invalid, but not invalid enough to fail the parse of the whole assembly name
                return parts.Length == 1 && ushort.TryParse(parts[0], NumberStyles.Integer, null, out _);
            }
            if (parts[0] == "" || parts[1] == "")
            {
                // this is a strange scenario, the version is invalid, but not invalid enough to fail the parse of the whole assembly name
                return true;
            }

            ushort build = 65535, revision = 65535;
            if (ushort.TryParse(parts[0], NumberStyles.Integer, null, out var major) &&
                ushort.TryParse(parts[1], NumberStyles.Integer, null, out var minor) &&
                (parts.Length <= 2 || parts[2] == "" || ushort.TryParse(parts[2], NumberStyles.Integer, null, out build)) &&
                (parts.Length <= 3 || parts[3] == "" || (parts[2] != "" && ushort.TryParse(parts[3], NumberStyles.Integer, null, out revision))))
            {
                if (mustBeComplete && (parts.Length < 4 || parts[2] == "" || parts[3] == ""))
                    version = null;
                else if (major == 65535 || minor == 65535)
                    version = null;
                else
                    version = new Version(major, minor, build, revision);

                return true;
            }

            version = null;
            return false;
        }

    }

}
