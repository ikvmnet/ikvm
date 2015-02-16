/*
  Copyright (C) 2010-2013 Jeroen Frijters
  Copyright (C) 2011 Marek Safar

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
using System.Runtime.InteropServices;
using System.Text;

namespace IKVM.Reflection
{
	struct ParsedAssemblyName
	{
		internal string Name;
		internal string Version;
		internal string Culture;
		internal string PublicKeyToken;
		internal bool? Retargetable;
		internal ProcessorArchitecture ProcessorArchitecture;
		internal bool HasPublicKey;
		internal bool WindowsRuntime;
	}

	enum ParseAssemblyResult
	{
		OK,
		GenericError,
		DuplicateKey,
	}

	static class Fusion
	{
		static readonly Version FrameworkVersion = new Version(4, 0, 0, 0);
		static readonly Version FrameworkVersionNext = new Version(4, 1, 0, 0);
		static readonly Version SilverlightVersion = new Version(2, 0, 5, 0);
		static readonly Version SilverlightVersionMinimum = new Version(2, 0, 0, 0);
		static readonly Version SilverlightVersionMaximum = new Version(5, 9, 0, 0);
		const string PublicKeyTokenEcma = "b77a5c561934e089";
		const string PublicKeyTokenMicrosoft = "b03f5f7f11d50a3a";
		const string PublicKeyTokenSilverlight = "7cec85d7bea7798e";
		const string PublicKeyTokenWinFX = "31bf3856ad364e35";

#if !CORECLR
		internal static bool CompareAssemblyIdentityNative(string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
		{
			bool equivalent;
			Marshal.ThrowExceptionForHR(CompareAssemblyIdentity(assemblyIdentity1, unified1, assemblyIdentity2, unified2, out equivalent, out result));
			return equivalent;
		}

		[DllImport("fusion", CharSet = CharSet.Unicode)]
		private static extern int CompareAssemblyIdentity(string pwzAssemblyIdentity1, bool fUnified1, string pwzAssemblyIdentity2, bool fUnified2, out bool pfEquivalent, out AssemblyComparisonResult pResult);
#endif

		// internal for use by mcs
		internal static bool CompareAssemblyIdentityPure(string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
		{
			ParsedAssemblyName name1;
			ParsedAssemblyName name2;

			ParseAssemblyResult r1 = ParseAssemblyName(assemblyIdentity1, out name1);
			ParseAssemblyResult r2 = ParseAssemblyName(assemblyIdentity2, out name2);

			Version version1;
			if (unified1)
			{
				if (name1.Name == null || !ParseVersion(name1.Version, out version1) || version1 == null || version1.Revision == -1
					|| name1.Culture == null || name1.PublicKeyToken == null || name1.PublicKeyToken.Length < 2)
				{
					result = AssemblyComparisonResult.NonEquivalent;
					throw new ArgumentException();
				}
			}

			Version version2 = null;
			if (!ParseVersion(name2.Version, out version2) || version2 == null || version2.Revision == -1 
				|| name2.Culture == null || name2.PublicKeyToken == null || name2.PublicKeyToken.Length < 2)
			{
				result = AssemblyComparisonResult.NonEquivalent;
				throw new ArgumentException();
			}

			if (name2.Name != null && name2.Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase))
			{
				if (name1.Name != null && name1.Name.Equals(name2.Name, StringComparison.OrdinalIgnoreCase))
				{
					result = AssemblyComparisonResult.EquivalentFullMatch;
					return true;
				}
				else
				{
					result = AssemblyComparisonResult.NonEquivalent;
					return false;
				}
			}

			if (r1 != ParseAssemblyResult.OK)
			{
				result = AssemblyComparisonResult.NonEquivalent;
				switch (r1)
				{
					case ParseAssemblyResult.DuplicateKey:
						throw new System.IO.FileLoadException();
					case ParseAssemblyResult.GenericError:
					default:
						throw new ArgumentException();
				}
			}

			if (r2 != ParseAssemblyResult.OK)
			{
				result = AssemblyComparisonResult.NonEquivalent;
				switch (r2)
				{
					case ParseAssemblyResult.DuplicateKey:
						throw new System.IO.FileLoadException();
					case ParseAssemblyResult.GenericError:
					default:
						throw new ArgumentException();
				}
			}

			if (!ParseVersion(name1.Version, out version1))
			{
				result = AssemblyComparisonResult.NonEquivalent;
				throw new ArgumentException();
			}

			bool partial = IsPartial(name1, version1);

			if (partial && name1.Retargetable.HasValue)
			{
				result = AssemblyComparisonResult.NonEquivalent;
				throw new System.IO.FileLoadException();
			}
			if ((partial && unified1) || IsPartial(name2, version2))
			{
				result = AssemblyComparisonResult.NonEquivalent;
				throw new ArgumentException();
			}
			if (!name1.Name.Equals(name2.Name, StringComparison.OrdinalIgnoreCase))
			{
				result = AssemblyComparisonResult.NonEquivalent;
				return false;
			}
			if (partial && name1.Culture == null)
			{
			}
			else if (!name1.Culture.Equals(name2.Culture, StringComparison.OrdinalIgnoreCase))
			{
				result = AssemblyComparisonResult.NonEquivalent;
				return false;
			}

			if (!name1.Retargetable.GetValueOrDefault() && name2.Retargetable.GetValueOrDefault())
			{
				result = AssemblyComparisonResult.NonEquivalent;
				return false;
			}

			// HACK handle the case "System.Net, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e, Retargetable=Yes"
			// compared with "System.Net, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e, Retargetable=No"
			if (name1.PublicKeyToken == name2.PublicKeyToken
				&& version1 != null
				&& name1.Retargetable.GetValueOrDefault()
				&& !name2.Retargetable.GetValueOrDefault()
				&& GetRemappedPublicKeyToken(ref name1, version1) != null)
			{
				name1.Retargetable = false;
			}

			string remappedPublicKeyToken1 = null;
			string remappedPublicKeyToken2 = null;
			if (version1 != null && (remappedPublicKeyToken1 = GetRemappedPublicKeyToken(ref name1, version1)) != null)
			{
				name1.PublicKeyToken = remappedPublicKeyToken1;
				version1 = FrameworkVersion;
			}
			if ((remappedPublicKeyToken2 = GetRemappedPublicKeyToken(ref name2, version2)) != null)
			{
				name2.PublicKeyToken = remappedPublicKeyToken2;
				version2 = FrameworkVersion;
			}
			if (name1.Retargetable.GetValueOrDefault())
			{
				if (name2.Retargetable.GetValueOrDefault())
				{
					if (remappedPublicKeyToken1 != null ^ remappedPublicKeyToken2 != null)
					{
						result = AssemblyComparisonResult.NonEquivalent;
						return false;
					}
				}
				else if (remappedPublicKeyToken1 == null || remappedPublicKeyToken2 != null)
				{
					result = AssemblyComparisonResult.Unknown;
					return false;
				}
			}

			bool fxUnified = false;
			bool versionMatch = version1 == version2;
			if (IsFrameworkAssembly(name1))
			{
				fxUnified |= !versionMatch;
				version1 = FrameworkVersion;
			}
			if (IsFrameworkAssembly(name2) && version2 < FrameworkVersionNext)
			{
				fxUnified |= !versionMatch;
				version2 = FrameworkVersion;
			}

			if (IsStrongNamed(name2))
			{
				if (name1.PublicKeyToken != null && name1.PublicKeyToken != name2.PublicKeyToken)
				{
					result = AssemblyComparisonResult.NonEquivalent;
					return false;
				}
				else if (version1 == null)
				{
					result = AssemblyComparisonResult.EquivalentPartialMatch;
					return true;
				}
				else if (version1.Revision == -1 || version2.Revision == -1)
				{
					result = AssemblyComparisonResult.NonEquivalent;
					throw new ArgumentException();
				}
				else if (version1 < version2)
				{
					if (unified2)
					{
						result = partial ? AssemblyComparisonResult.EquivalentPartialUnified : AssemblyComparisonResult.EquivalentUnified;
						return true;
					}
					else
					{
						result = partial ? AssemblyComparisonResult.NonEquivalentPartialVersion : AssemblyComparisonResult.NonEquivalentVersion;
						return false;
					}
				}
				else if (version1 > version2)
				{
					if (unified1)
					{
						result = partial ? AssemblyComparisonResult.EquivalentPartialUnified : AssemblyComparisonResult.EquivalentUnified;
						return true;
					}
					else
					{
						result = partial ? AssemblyComparisonResult.NonEquivalentPartialVersion : AssemblyComparisonResult.NonEquivalentVersion;
						return false;
					}
				}
				else if (!versionMatch || fxUnified)
				{
					result = partial ? AssemblyComparisonResult.EquivalentPartialFXUnified : AssemblyComparisonResult.EquivalentFXUnified;
					return true;
				}
				else
				{
					result = partial ? AssemblyComparisonResult.EquivalentPartialMatch : AssemblyComparisonResult.EquivalentFullMatch;
					return true;
				}
			}
			else if (IsStrongNamed(name1))
			{
				result = AssemblyComparisonResult.NonEquivalent;
				return false;
			}
			else
			{
				result = partial ? AssemblyComparisonResult.EquivalentPartialWeakNamed : AssemblyComparisonResult.EquivalentWeakNamed;
				return true;
			}
		}

		static bool IsFrameworkAssembly(ParsedAssemblyName name)
		{
			// Framework assemblies use different unification rules, so when
			// a new framework is released the new assemblies need to be added.
			switch (name.Name)
			{
				case "System":
				case "System.Core":
				case "System.Data":
				case "System.Data.DataSetExtensions":
				case "System.Data.Linq":
				case "System.Data.OracleClient":
				case "System.Data.Services":
				case "System.Data.Services.Client":
				case "System.IdentityModel":
				case "System.IdentityModel.Selectors":
				case "System.IO.Compression":
				case "System.Numerics":
				case "System.Reflection.Context":
				case "System.Runtime.Remoting":
				case "System.Runtime.Serialization":
				case "System.Runtime.WindowsRuntime":
				case "System.Runtime.WindowsRuntime.UI.Xaml":
				case "System.ServiceModel":
				case "System.Transactions":
				case "System.Windows.Forms":
				case "System.Xml":
				case "System.Xml.Linq":
				case "System.Xml.Serialization":
					return name.PublicKeyToken == PublicKeyTokenEcma;

				case "Microsoft.CSharp":
				case "Microsoft.VisualBasic":
				case "System.Collections":
				case "System.Collections.Concurrent":
				case "System.ComponentModel":
				case "System.ComponentModel.Annotations":
				case "System.ComponentModel.EventBasedAsync":
				case "System.Configuration":
				case "System.Configuration.Install":
				case "System.Design":
				case "System.Diagnostics.Contracts":
				case "System.Diagnostics.Debug":
				case "System.Diagnostics.Tools":
				case "System.Diagnostics.Tracing":
				case "System.DirectoryServices":
				case "System.Drawing":
				case "System.Drawing.Design":
				case "System.Dynamic.Runtime":
				case "System.EnterpriseServices":
				case "System.Globalization":
				case "System.IO":
				case "System.Linq":
				case "System.Linq.Expressions":
				case "System.Linq.Parallel":
				case "System.Linq.Queryable":
				case "System.Management":
				case "System.Messaging":
				case "System.Net":
				case "System.Net.Http":
				case "System.Net.Http.Rtc":
				case "System.Net.NetworkInformation":
				case "System.Net.Primitives":
				case "System.Net.Requests":
				case "System.ObjectModel":
				case "System.Reflection":
				case "System.Reflection.Extensions":
				case "System.Reflection.Primitives":
				case "System.Resources.ResourceManager":
				case "System.Runtime":
				case "System.Runtime.Extensions":
				case "System.Runtime.InteropServices":
				case "System.Runtime.InteropServices.WindowsRuntime":
				case "System.Runtime.Numerics":
				case "System.Runtime.Serialization.Formatters.Soap":
				case "System.Runtime.Serialization.Json":
				case "System.Runtime.Serialization.Primitives":
				case "System.Runtime.Serialization.Xml":
				case "System.Security":
				case "System.Security.Principal":
				case "System.ServiceModel.Duplex":
				case "System.ServiceModel.Http":
				case "System.ServiceModel.NetTcp":
				case "System.ServiceModel.Primitives":
				case "System.ServiceModel.Security":
				case "System.ServiceProcess":
				case "System.Text.Encoding":
				case "System.Text.Encoding.Extensions":
				case "System.Text.RegularExpressions":
				case "System.Threading":
				case "System.Threading.Tasks":
				case "System.Threading.Tasks.Parallel":
				case "System.Web":
				case "System.Web.Mobile":
				case "System.Web.Services":
				case "System.Windows":
				case "System.Xml.ReaderWriter":
				case "System.Xml.XDocument":
				case "System.Xml.XmlSerializer":
					return name.PublicKeyToken == PublicKeyTokenMicrosoft;

				case "System.ComponentModel.DataAnnotations":
				case "System.ServiceModel.Web":
				case "System.Web.Abstractions":
				case "System.Web.Extensions":
				case "System.Web.Extensions.Design":
				case "System.Web.DynamicData":
				case "System.Web.Routing":
					return name.PublicKeyToken == PublicKeyTokenWinFX;
			}

			return false;
		}

		static string GetRemappedPublicKeyToken(ref ParsedAssemblyName name, Version version)
		{
			if (name.Retargetable.GetValueOrDefault() && version < SilverlightVersion)
			{
				return null;
			}
			if (name.PublicKeyToken == "ddd0da4d3e678217" && name.Name == "System.ComponentModel.DataAnnotations" && name.Retargetable.GetValueOrDefault())
			{
				return PublicKeyTokenWinFX;
			}
			if (SilverlightVersionMinimum <= version && version <= SilverlightVersionMaximum)
			{
				switch (name.PublicKeyToken)
				{
					case PublicKeyTokenSilverlight:
						switch (name.Name)
						{
							case "System":
							case "System.Core":
								return PublicKeyTokenEcma;
						}
						if (name.Retargetable.GetValueOrDefault())
						{
							switch (name.Name)
							{
								case "System.Runtime.Serialization":
								case "System.Xml":
									return PublicKeyTokenEcma;
								case "System.Net":
								case "System.Windows":
									return PublicKeyTokenMicrosoft;
								case "System.ServiceModel.Web":
									return PublicKeyTokenWinFX;
							}
						}
						break;
					case PublicKeyTokenWinFX:
						switch (name.Name)
						{
							case "System.ComponentModel.Composition":
								return PublicKeyTokenEcma;
						}
						if (name.Retargetable.GetValueOrDefault())
						{
							switch (name.Name)
							{
								case "Microsoft.CSharp":
									return PublicKeyTokenMicrosoft;
								case "System.Numerics":
								case "System.ServiceModel":
								case "System.Xml.Serialization":
								case "System.Xml.Linq":
									return PublicKeyTokenEcma;
							}
						}
						break;
				}
			}
			return null;
		}

		internal static ParseAssemblyResult ParseAssemblySimpleName(string fullName, out int pos, out string simpleName)
		{
			pos = 0;
			if (!TryParse(fullName, ref pos, out simpleName) || simpleName.Length == 0)
			{
				return ParseAssemblyResult.GenericError;
			}
			if (pos == fullName.Length && fullName[fullName.Length - 1] == ',')
			{
				return ParseAssemblyResult.GenericError;
			}
			return ParseAssemblyResult.OK;
		}

		private static bool TryParse(string fullName, ref int pos, out string value)
		{
			value = null;
			StringBuilder sb = new StringBuilder();
			while (pos < fullName.Length && char.IsWhiteSpace(fullName[pos]))
			{
				pos++;
			}
			int quote = -1;
			if (pos < fullName.Length && (fullName[pos] == '"' || fullName[pos] == '\''))
			{
				quote = fullName[pos++];
			}
			for (; pos < fullName.Length; pos++)
			{
				char ch = fullName[pos];
				if (ch == '\\')
				{
					if (++pos == fullName.Length)
					{
						return false;
					}
					ch = fullName[pos];
					if (ch == '\\')
					{
						return false;
					}
				}
				else if (ch == quote)
				{
					for (pos++; pos != fullName.Length; pos++)
					{
						ch = fullName[pos];
						if (ch == ',' || ch == '=')
						{
							break;
						}
						if (!char.IsWhiteSpace(ch))
						{
							return false;
						}
					}
					break;
				}
				else if (quote == -1 && (ch == '"' || ch == '\''))
				{
					return false;
				}
				else if (quote == -1 && (ch == ',' || ch == '='))
				{
					break;
				}
				sb.Append(ch);
			}
			value = sb.ToString().Trim();
			return value.Length != 0 || quote != -1;
		}

		private static bool TryConsume(string fullName, char ch, ref int pos)
		{
			if (pos < fullName.Length && fullName[pos] == ch)
			{
				pos++;
				return true;
			}
			return false;
		}

		private static bool TryParseAssemblyAttribute(string fullName, ref int pos, ref string key, ref string value)
		{
			return TryConsume(fullName, ',', ref pos)
				&& TryParse(fullName, ref pos, out key)
				&& TryConsume(fullName, '=', ref pos)
				&& TryParse(fullName, ref pos, out value);
		}

		internal static ParseAssemblyResult ParseAssemblyName(string fullName, out ParsedAssemblyName parsedName)
		{
			parsedName = new ParsedAssemblyName();
			int pos;
			ParseAssemblyResult res = ParseAssemblySimpleName(fullName, out pos, out parsedName.Name);
			if (res != ParseAssemblyResult.OK)
			{
				return res;
			}
			else
			{
				const int ERROR_SXS_IDENTITIES_DIFFERENT = unchecked((int)0x80073716);
				System.Collections.Generic.Dictionary<string, string> unknownAttributes = null;
				bool hasProcessorArchitecture = false;
				bool hasContentType = false;
				bool hasPublicKeyToken = false;
				string publicKeyToken;
				while (pos != fullName.Length)
				{
					string key = null;
					string value = null;
					if (!TryParseAssemblyAttribute(fullName, ref pos, ref key, ref value))
					{
						return ParseAssemblyResult.GenericError;
					}
					key = key.ToLowerInvariant();
					switch (key)
					{
						case "version":
							if (parsedName.Version != null)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							parsedName.Version = value;
							break;
						case "culture":
							if (parsedName.Culture != null)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							if (!ParseCulture(value, out parsedName.Culture))
							{
								return ParseAssemblyResult.GenericError;
							}
							break;
						case "publickeytoken":
							if (hasPublicKeyToken)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							if (!ParsePublicKeyToken(value, out publicKeyToken))
							{
								return ParseAssemblyResult.GenericError;
							}
							if (parsedName.HasPublicKey && parsedName.PublicKeyToken != publicKeyToken)
							{
								Marshal.ThrowExceptionForHR(ERROR_SXS_IDENTITIES_DIFFERENT);
							}
							parsedName.PublicKeyToken = publicKeyToken;
							hasPublicKeyToken = true;
							break;
						case "publickey":
							if (parsedName.HasPublicKey)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							if (!ParsePublicKey(value, out publicKeyToken))
							{
								return ParseAssemblyResult.GenericError;
							}
							if (hasPublicKeyToken && parsedName.PublicKeyToken != publicKeyToken)
							{
								Marshal.ThrowExceptionForHR(ERROR_SXS_IDENTITIES_DIFFERENT);
							}
							parsedName.PublicKeyToken = publicKeyToken;
							parsedName.HasPublicKey = true;
							break;
						case "retargetable":
							if (parsedName.Retargetable.HasValue)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							switch (value.ToLowerInvariant())
							{
								case "yes":
									parsedName.Retargetable = true;
									break;
								case "no":
									parsedName.Retargetable = false;
									break;
								default:
									return ParseAssemblyResult.GenericError;
							}
							break;
						case "processorarchitecture":
							if (hasProcessorArchitecture)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							hasProcessorArchitecture = true;
							switch (value.ToLowerInvariant())
							{
								case "none":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.None;
									break;
								case "msil":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.MSIL;
									break;
								case "x86":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.X86;
									break;
								case "ia64":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.IA64;
									break;
								case "amd64":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.Amd64;
									break;
								case "arm":
									parsedName.ProcessorArchitecture = ProcessorArchitecture.Arm;
									break;
								default:
									return ParseAssemblyResult.GenericError;
							}
							break;
						case "contenttype":
							if (hasContentType)
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							hasContentType = true;
							if (!value.Equals("windowsruntime", StringComparison.OrdinalIgnoreCase))
							{
								return ParseAssemblyResult.GenericError;
							}
							parsedName.WindowsRuntime = true;
							break;
						default:
							if (key.Length == 0)
							{
								return ParseAssemblyResult.GenericError;
							}
							if (unknownAttributes == null)
							{
								unknownAttributes = new System.Collections.Generic.Dictionary<string, string>();
							}
							if (unknownAttributes.ContainsKey(key))
							{
								return ParseAssemblyResult.DuplicateKey;
							}
							unknownAttributes.Add(key, null);
							break;
					}
				}
				return ParseAssemblyResult.OK;
			}
		}

		private static bool ParseVersion(string str, out Version version)
		{
			if (str == null)
			{
				version = null;
				return true;
			}
			string[] parts = str.Split('.');
			if (parts.Length < 2 || parts.Length > 4)
			{
				version = null;
				ushort dummy;
				// if the version consists of a single integer, it is invalid, but not invalid enough to fail the parse of the whole assembly name
				return parts.Length == 1 && ushort.TryParse(parts[0], System.Globalization.NumberStyles.Integer, null, out dummy);
			}
			if (parts[0] == "" || parts[1] == "")
			{
				// this is a strange scenario, the version is invalid, but not invalid enough to fail the parse of the whole assembly name
				version = null;
				return true;
			}
			ushort major, minor, build = 65535, revision = 65535;
			if (ushort.TryParse(parts[0], System.Globalization.NumberStyles.Integer, null, out major)
				&& ushort.TryParse(parts[1], System.Globalization.NumberStyles.Integer, null, out minor)
				&& (parts.Length <= 2 || parts[2] == "" || ushort.TryParse(parts[2], System.Globalization.NumberStyles.Integer, null, out build))
				&& (parts.Length <= 3 || parts[3] == "" || (parts[2] != "" && ushort.TryParse(parts[3], System.Globalization.NumberStyles.Integer, null, out revision))))
			{
				if (parts.Length == 4 && parts[3] != "" && parts[2] != "")
				{
					version = new Version(major, minor, build, revision);
				}
				else if (parts.Length == 3 && parts[2] != "")
				{
					version = new Version(major, minor, build);
				}
				else
				{
					version = new Version(major, minor);
				}
				return true;
			}
			version = null;
			return false;
		}

		private static bool ParseCulture(string str, out string culture)
		{
			if (str == null)
			{
				culture = null;
				return false;
			}
			culture = str;
			return true;
		}

		private static bool ParsePublicKeyToken(string str, out string publicKeyToken)
		{
			if (str == null)
			{
				publicKeyToken = null;
				return false;
			}
			publicKeyToken = str.ToLowerInvariant();
			return true;
		}

		private static bool ParsePublicKey(string str, out string publicKeyToken)
		{
			if (str == null)
			{
				publicKeyToken = null;
				return false;
			}
			publicKeyToken = AssemblyName.ComputePublicKeyToken(str);
			return true;
		}

		private static bool IsPartial(ParsedAssemblyName name, Version version)
		{
			return version == null || name.Culture == null || name.PublicKeyToken == null;
		}

		private static bool IsStrongNamed(ParsedAssemblyName name)
		{
			return name.PublicKeyToken != null && name.PublicKeyToken != "null";
		}
	}
}
