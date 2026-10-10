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
        internal byte[] Hash;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public AssemblyName()
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileLoadException"></exception>
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

        static byte[] ParseKey(string key)
        {
            if ((key.Length & 1) != 0)
                throw new FileLoadException();

            var buf = new byte[key.Length / 2];
            for (var i = 0; i < buf.Length; i++)
                buf[i] = (byte)(ParseHexDigit(key[i * 2]) * 16 + ParseHexDigit(key[i * 2 + 1]));

            return buf;
        }

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

        public override string ToString() => FullName;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public CultureInfo CultureInfo
        {
            get => _culture == null ? null : new CultureInfo(_culture);
            set => _culture = value?.Name;
        }

        public string CultureName
        {
            get => _culture;
            set => _culture = value;
        }

        public Version Version
        {
            get => _version;
            set => _version = value;
        }

        public StrongNameKeyPair KeyPair
        {
            get => _keyPair;
            set => _keyPair = value;
        }

        public string CodeBase
        {
            get => _codeBase;
            set => _codeBase = value;
        }

        public ProcessorArchitecture ProcessorArchitecture
        {
            get => (ProcessorArchitecture)(((int)_flags & 0x70) >> 4);
            set
            {
                if (value >= ProcessorArchitecture.None && value <= ProcessorArchitecture.Arm64)
                    _flags = (_flags & ~(AssemblyNameFlags)0x70) | (AssemblyNameFlags)((int)value << 4);
            }
        }

        public AssemblyNameFlags Flags
        {
            get => _flags & (AssemblyNameFlags)~0xEF0;
            set => _flags = (_flags & (AssemblyNameFlags)0xEF0) | (value & (AssemblyNameFlags)~0xEF0);
        }

        public AssemblyContentType ContentType
        {
            get => (AssemblyContentType)(((int)_flags & 0xE00) >> 9);
            set
            {
                if (value >= AssemblyContentType.Default && value <= AssemblyContentType.WindowsRuntime)
                    _flags = (_flags & ~(AssemblyNameFlags)0xE00) | (AssemblyNameFlags)((int)value << 9);
            }
        }

        public byte[] GetPublicKey() => _publicKey;

        public void SetPublicKey(byte[] publicKey)
        {
            _publicKey = publicKey;
            _flags = (_flags & ~AssemblyNameFlags.PublicKey) | (publicKey == null ? 0 : AssemblyNameFlags.PublicKey);
        }

        public byte[] GetPublicKeyToken()
        {
            if (_publicKeyToken == null && _publicKey != null)
            {
                // note that GetPublicKeyToken() has a side effect in this case, because we retain this token even after the public key subsequently gets changed
                _publicKeyToken = ComputePublicKeyToken(_publicKey);
            }

            return _publicKeyToken;
        }

        public void SetPublicKeyToken(byte[] publicKeyToken) => _publicKeyToken = publicKeyToken;

        public AssemblyHashAlgorithm HashAlgorithm
        {
            get => _hashAlgorithm;
            set => _hashAlgorithm = value;
        }

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

        internal static string ComputePublicKeyToken(string publicKey)
        {
            var sb = new StringBuilder(16);
            AppendPublicKey(sb, ComputePublicKeyToken(ParseKey(publicKey)));
            return sb.ToString();
        }

        static void AppendPublicKey(StringBuilder sb, byte[] publicKey)
        {
            for (var i = 0; i < publicKey.Length; i++)
            {
                sb.Append("0123456789abcdef"[publicKey[i] >> 4]);
                sb.Append("0123456789abcdef"[publicKey[i] & 0x0F]);
            }
        }

        public override bool Equals(object obj) => obj is AssemblyName other && other.FullName == FullName;

        public override int GetHashCode() => FullName.GetHashCode();

        public object Clone()
        {
            var copy = (AssemblyName)MemberwiseClone();
            copy._publicKey = Copy(_publicKey);
            copy._publicKeyToken = Copy(_publicKeyToken);
            return copy;
        }

        static byte[] Copy(byte[] b) => b == null || b.Length == 0 ? b : (byte[])b.Clone();

        /// <summary>
        /// Gets the <see cref="AssemblyName"/> for a given file.
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        /// <exception cref="BadImageFormatException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
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

        internal AssemblyNameFlags RawFlags
        {
            get => _flags;
            set => _flags = value;
        }

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
