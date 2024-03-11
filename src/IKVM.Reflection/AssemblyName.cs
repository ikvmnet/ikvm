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

    public sealed class AssemblyName :
        ICloneable
    {

        string name;
        string culture;
        Version version;
        byte[] publicKeyToken;
        byte[] publicKey;
        StrongNameKeyPair keyPair;
        AssemblyNameFlags flags;
        AssemblyHashAlgorithm hashAlgorithm;
        AssemblyVersionCompatibility versionCompatibility = AssemblyVersionCompatibility.SameMachine;
        string codeBase;
        internal byte[] hash;

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

            if (!ParseVersion(parsed.Version, parsed.Retargetable.HasValue, out version))
                throw new FileLoadException();

            name = parsed.Name;
            if (parsed.Culture != null)
            {
                if (parsed.Culture.Equals("neutral", StringComparison.OrdinalIgnoreCase))
                    culture = "";
                else if (parsed.Culture == "")
                    throw new FileLoadException();
                else
                    culture = new CultureInfo(parsed.Culture).Name;
            }

            if (parsed.PublicKeyToken != null)
            {
                if (parsed.PublicKeyToken.Equals("null", StringComparison.OrdinalIgnoreCase))
                    publicKeyToken = Array.Empty<byte>();
                else if (parsed.PublicKeyToken.Length != 16)
                    throw new FileLoadException();
                else
                    publicKeyToken = ParseKey(parsed.PublicKeyToken);
            }

            if (parsed.Retargetable.HasValue)
            {
                if (parsed.Culture == null || parsed.PublicKeyToken == null || version == null)
                    throw new FileLoadException();
                if (parsed.Retargetable.Value)
                    flags |= AssemblyNameFlags.Retargetable;
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
            for (int i = 0; i < buf.Length; i++)
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

        public override string ToString()
        {
            return FullName;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public CultureInfo CultureInfo
        {
            get => culture == null ? null : new CultureInfo(culture);
            set => culture = value?.Name;
        }

        public string CultureName
        {
            get => culture;
            set => culture = value;
        }

        public Version Version
        {
            get => version;
            set => version = value;
        }

        public StrongNameKeyPair KeyPair
        {
            get => keyPair;
            set => keyPair = value;
        }

        public string CodeBase
        {
            get => codeBase;
            set => codeBase = value;
        }

        public string EscapedCodeBase
        {
            get
            {
                // HACK use the real AssemblyName to escape the codebase
                var tmp = new System.Reflection.AssemblyName();
                tmp.CodeBase = codeBase;
                return tmp.EscapedCodeBase;
            }
        }

        public ProcessorArchitecture ProcessorArchitecture
        {
            get => (ProcessorArchitecture)(((int)flags & 0x70) >> 4);
            set
            {
                if (value >= ProcessorArchitecture.None && value <= ProcessorArchitecture.Arm64)
                    flags = (flags & ~(AssemblyNameFlags)0x70) | (AssemblyNameFlags)((int)value << 4);
            }
        }

        public AssemblyNameFlags Flags
        {
            get => flags & (AssemblyNameFlags)~0xEF0;
            set => flags = (flags & (AssemblyNameFlags)0xEF0) | (value & (AssemblyNameFlags)~0xEF0);
        }

        public AssemblyVersionCompatibility VersionCompatibility
        {
            get => versionCompatibility;
            set => versionCompatibility = value;
        }

        public AssemblyContentType ContentType
        {
            get => (AssemblyContentType)(((int)flags & 0xE00) >> 9);
            set
            {
                if (value >= AssemblyContentType.Default && value <= AssemblyContentType.WindowsRuntime)
                    flags = (flags & ~(AssemblyNameFlags)0xE00) | (AssemblyNameFlags)((int)value << 9);
            }
        }

        public byte[] GetPublicKey()
        {
            return publicKey;
        }

        public void SetPublicKey(byte[] publicKey)
        {
            this.publicKey = publicKey;
            flags = (flags & ~AssemblyNameFlags.PublicKey) | (publicKey == null ? 0 : AssemblyNameFlags.PublicKey);
        }

        public byte[] GetPublicKeyToken()
        {
            if (publicKeyToken == null && publicKey != null)
            {
                // note that GetPublicKeyToken() has a side effect in this case, because we retain this token even after the public key subsequently gets changed
                publicKeyToken = ComputePublicKeyToken(publicKey);
            }

            return publicKeyToken;
        }

        public void SetPublicKeyToken(byte[] publicKeyToken)
        {
            this.publicKeyToken = publicKeyToken;
        }

        public AssemblyHashAlgorithm HashAlgorithm
        {
            get => hashAlgorithm;
            set => hashAlgorithm = value;
        }

        public byte[] __Hash => hash;

        public string FullName
        {
            get
            {
                if (name == null)
                    return "";

                ushort versionMajor = 0xFFFF;
                ushort versionMinor = 0xFFFF;
                ushort versionBuild = 0xFFFF;
                ushort versionRevision = 0xFFFF;

                if (version != null)
                {
                    versionMajor = (ushort)version.Major;
                    versionMinor = (ushort)version.Minor;
                    versionBuild = (ushort)version.Build;
                    versionRevision = (ushort)version.Revision;
                }

                var publicKeyToken = this.publicKeyToken;
                if ((publicKeyToken == null || publicKeyToken.Length == 0) && publicKey != null)
                    publicKeyToken = ComputePublicKeyToken(publicKey);

                return GetFullName(name, versionMajor, versionMinor, versionBuild, versionRevision, culture, publicKeyToken, (int)flags);
            }
        }

        internal static string GetFullName(string name, ushort versionMajor, ushort versionMinor, ushort versionBuild, ushort versionRevision, string culture, byte[] publicKeyToken, int flags)
        {
            var sb = new StringBuilder();
            bool doubleQuotes = name.StartsWith(" ") || name.EndsWith(" ") || name.IndexOf('\'') != -1;
            bool singleQuotes = name.IndexOf('"') != -1;
            if (singleQuotes)
                sb.Append('\'');
            else if (doubleQuotes)
                sb.Append('"');

            if (name.IndexOf(',') != -1 || name.IndexOf('\\') != -1 || name.IndexOf('=') != -1 || (singleQuotes && name.IndexOf('\'') != -1))
            {
                for (int i = 0; i < name.Length; i++)
                {
                    char c = name[i];
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
            for (int i = 0; i < token.Length; i++)
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
            for (int i = 0; i < publicKey.Length; i++)
            {
                sb.Append("0123456789abcdef"[publicKey[i] >> 4]);
                sb.Append("0123456789abcdef"[publicKey[i] & 0x0F]);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is AssemblyName other && other.FullName == FullName;
        }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }

        public object Clone()
        {
            var copy = (AssemblyName)MemberwiseClone();
            copy.publicKey = Copy(publicKey);
            copy.publicKeyToken = Copy(publicKeyToken);
            return copy;
        }

        static byte[] Copy(byte[] b)
        {
            return b == null || b.Length == 0 ? b : (byte[])b.Clone();
        }

        public static bool ReferenceMatchesDefinition(AssemblyName reference, AssemblyName definition)
        {
            // HACK use the real AssemblyName to implement the (broken) ReferenceMatchesDefinition method
            return System.Reflection.AssemblyName.ReferenceMatchesDefinition(new System.Reflection.AssemblyName(reference.FullName), new System.Reflection.AssemblyName(definition.FullName));
        }

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
            get => flags;
            set => flags = value;
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
            if (ushort.TryParse(parts[0], NumberStyles.Integer, null, out ushort major) &&
                ushort.TryParse(parts[1], NumberStyles.Integer, null, out ushort minor) &&
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
