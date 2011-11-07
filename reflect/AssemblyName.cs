/*
  Copyright (C) 2009-2011 Jeroen Frijters

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
using System.Configuration.Assemblies;
using System.IO;
using System.Text;
using IKVM.Reflection.Reader;

namespace IKVM.Reflection
{
	public sealed class AssemblyName : ICloneable
	{
		private string name;
		private string culture;
		private Version version;
		private byte[] publicKeyToken;
		private byte[] publicKey;
		private StrongNameKeyPair keyPair;
		private AssemblyNameFlags flags;
		private AssemblyHashAlgorithm hashAlgorithm;
		private AssemblyVersionCompatibility versionCompatibility = AssemblyVersionCompatibility.SameMachine;
		private string codeBase;
		internal byte[] hash;

		public AssemblyName()
		{
		}

		public AssemblyName(string assemblyName)
		{
			// HACK use the real AssemblyName to parse the string
			System.Reflection.AssemblyName impl = new System.Reflection.AssemblyName(assemblyName);
			name = impl.Name;
			culture = impl.CultureInfo == null ? null : impl.CultureInfo.Name;
			version = impl.Version;
			publicKeyToken = impl.GetPublicKeyToken();
			flags = (AssemblyNameFlags)(int)impl.Flags;
			ProcessorArchitecture = (ProcessorArchitecture)impl.ProcessorArchitecture;
		}

		public override string ToString()
		{
			return FullName;
		}

		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		public CultureInfo CultureInfo
		{
			get { return culture == null ? null : new CultureInfo(culture); }
			set { culture = value == null ? null : value.Name; }
		}

		internal string Culture
		{
			get { return culture; }
			set { culture = value; }
		}

		public Version Version
		{
			get { return version; }
			set { version = value; }
		}

		public StrongNameKeyPair KeyPair
		{
			get { return keyPair; }
			set { keyPair = value; }
		}

		public string CodeBase
		{
			get { return codeBase; }
			set { codeBase = value; }
		}

		public string EscapedCodeBase
		{
			get
			{
				// HACK use the real AssemblyName to escape the codebase
				System.Reflection.AssemblyName tmp = new System.Reflection.AssemblyName();
				tmp.CodeBase = codeBase;
				return tmp.EscapedCodeBase;
			}
		}

		public ProcessorArchitecture ProcessorArchitecture
		{
			get { return (ProcessorArchitecture)(((int)flags & 0x70) >> 4); }
			set
			{
				if (value >= ProcessorArchitecture.None && value <= ProcessorArchitecture.Arm)
				{
					flags = (flags & ~(AssemblyNameFlags)0x70) | (AssemblyNameFlags)((int)value << 4);
				}
			}
		}

		public AssemblyNameFlags Flags
		{
			get { return flags & (AssemblyNameFlags)~0xF0; }
			set { flags = (flags & (AssemblyNameFlags)0xF0) | (value & (AssemblyNameFlags)~0xF0); }
		}

		public AssemblyVersionCompatibility VersionCompatibility
		{
			get { return versionCompatibility; }
			set { versionCompatibility = value; }
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
			get { return hashAlgorithm; }
			set { hashAlgorithm = value; }
		}

		public string FullName
		{
			get
			{
				if (name == null)
				{
					return "";
				}
				StringBuilder sb = new StringBuilder();
				bool doubleQuotes = name.StartsWith(" ") || name.EndsWith(" ") || name.IndexOf('\'') != -1;
				bool singleQuotes = name.IndexOf('"') != -1;
				if (singleQuotes)
				{
					sb.Append('\'');
				}
				else if (doubleQuotes)
				{
					sb.Append('"');
				}
				if (name.IndexOf(',') != -1 || name.IndexOf('\\') != -1 || (singleQuotes && name.IndexOf('\'') != -1))
				{
					for (int i = 0; i < name.Length; i++)
					{
						char c = name[i];
						if (c == ',' || c == '\\' || (singleQuotes && c == '\''))
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
				if (version != null)
				{
					sb.AppendFormat(", Version={0}.{1}", version.Major, version.Minor);
					// TODO what's this all about?
					if (version.Build != 65535 && version.Build != -1)
					{
						sb.AppendFormat(".{0}", version.Build);
						if (version.Revision != 65535 && version.Revision != -1)
						{
							sb.AppendFormat(".{0}", version.Revision);
						}
					}
				}
				if (culture != null)
				{
					sb.Append(", Culture=").Append(culture == "" ? "neutral" : culture);
				}
				byte[] publicKeyToken = this.publicKeyToken;
				if ((publicKeyToken == null || publicKeyToken.Length == 0) && publicKey != null)
				{
					publicKeyToken = ComputePublicKeyToken(publicKey);
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
						for (int i = 0; i < publicKeyToken.Length; i++)
						{
							sb.AppendFormat("{0:x2}", publicKeyToken[i]);
						}
					}
				}
				if ((Flags & AssemblyNameFlags.Retargetable) != 0)
				{
					sb.Append(", Retargetable=Yes");
				}
				return sb.ToString();
			}
		}

		private static byte[] ComputePublicKeyToken(byte[] publicKey)
		{
			if (publicKey.Length == 0)
			{
				return publicKey;
			}
			// HACK use the real AssemblyName to convert PublicKey to PublicKeyToken
			StringBuilder sb = new StringBuilder("Foo, PublicKey=", 20 + publicKey.Length * 2);
			for (int i = 0; i < publicKey.Length; i++)
			{
				sb.AppendFormat("{0:x2}", publicKey[i]);
			}
			return new System.Reflection.AssemblyName(sb.ToString()).GetPublicKeyToken();
		}

		public override bool Equals(object obj)
		{
			AssemblyName other = obj as AssemblyName;
			return other != null && other.FullName == this.FullName;
		}

		public override int GetHashCode()
		{
			return FullName.GetHashCode();
		}

		public object Clone()
		{
			AssemblyName copy = (AssemblyName)MemberwiseClone();
			copy.publicKey = Copy(publicKey);
			copy.publicKeyToken = Copy(publicKeyToken);
			return copy;
		}

		private static byte[] Copy(byte[] b)
		{
			return b == null || b.Length == 0 ? b : (byte[])b.Clone();
		}

		public static bool ReferenceMatchesDefinition(AssemblyName reference, AssemblyName definition)
		{
			// HACK use the real AssemblyName to implement the (broken) ReferenceMatchesDefinition method
			return System.Reflection.AssemblyName.ReferenceMatchesDefinition(new System.Reflection.AssemblyName(reference.FullName), new System.Reflection.AssemblyName(definition.FullName));
		}

		public static AssemblyName GetAssemblyName(string path)
		{
			try
			{
				path = Path.GetFullPath(path);
				using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					ModuleReader module = new ModuleReader(null, null, fs, path);
					if (module.Assembly == null)
					{
						throw new BadImageFormatException("Module does not contain a manifest");
					}
					return module.Assembly.GetName();
				}
			}
			catch (IOException x)
			{
				throw new FileNotFoundException(x.Message, x);
			}
			catch (UnauthorizedAccessException x)
			{
				throw new FileNotFoundException(x.Message, x);
			}
		}

		internal AssemblyNameFlags RawFlags
		{
			get { return flags; }
			set { flags = value; }
		}
	}
}
