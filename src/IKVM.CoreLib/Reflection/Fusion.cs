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

    /// <summary>
    /// Parses and compares assembly display names the way the .NET Framework assembly loader (fusion) does.
    /// </summary>
    static class Fusion
    {

        /// <summary>
        /// The version that .NET Framework assemblies are unified to.
        /// </summary>
        static readonly Version s_frameworkVersion = new(4, 0, 0, 0);

        /// <summary>
        /// The first version of a framework assembly that is no longer unified to <see cref="s_frameworkVersion"/>.
        /// </summary>
        static readonly Version s_frameworkVersionNext = new(5, 0, 0, 0);

        /// <summary>
        /// The minimum version of a retargetable assembly for its public key token to be remapped.
        /// </summary>
        static readonly Version s_silverlightVersion = new(2, 0, 5, 0);

        /// <summary>
        /// The lowest Silverlight assembly version whose public key token is remapped to a .NET Framework one.
        /// </summary>
        static readonly Version s_silverlightVersionMinimum = new(2, 0, 0, 0);

        /// <summary>
        /// The highest Silverlight assembly version whose public key token is remapped to a .NET Framework one.
        /// </summary>
        static readonly Version s_silverlightVersionMaximum = new(5, 9, 0, 0);

        /// <summary>
        /// The public key token of the ECMA key, used by the core .NET Framework assemblies.
        /// </summary>
        const string PublicKeyTokenEcma = "b77a5c561934e089";

        /// <summary>
        /// The public key token of the Microsoft key, used by most other .NET Framework assemblies.
        /// </summary>
        const string PublicKeyTokenMicrosoft = "b03f5f7f11d50a3a";

        /// <summary>
        /// The public key token of the Silverlight platform assemblies.
        /// </summary>
        const string PublicKeyTokenSilverlight = "7cec85d7bea7798e";

        /// <summary>
        /// The public key token of the WinFX key, used by the assemblies introduced with .NET Framework 3.0 and 3.5.
        /// </summary>
        const string PublicKeyTokenWinFX = "31bf3856ad364e35";

        /// <summary>
        /// Compares two assembly identities using the native CompareAssemblyIdentity function of fusion.dll, which is
        /// only available on the .NET Framework on Windows.
        /// </summary>
        /// <param name="coreLibName">The simple name of the core library. Not used by the native implementation.</param>
        /// <param name="assemblyIdentity1">The display name of the reference assembly.</param>
        /// <param name="unified1"><see langword="true"/> if the version of <paramref name="assemblyIdentity1"/> has been unified by the application.</param>
        /// <param name="assemblyIdentity2">The display name of the definition assembly.</param>
        /// <param name="unified2"><see langword="true"/> if the version of <paramref name="assemblyIdentity2"/> has been unified by the application.</param>
        /// <param name="result">When this method returns, describes how the identities compare.</param>
        /// <returns><see langword="true"/> if the identities are equivalent; otherwise, <see langword="false"/>.</returns>
        internal static bool CompareAssemblyIdentityNative(string coreLibName, string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            Marshal.ThrowExceptionForHR(CompareAssemblyIdentity(assemblyIdentity1, unified1, assemblyIdentity2, unified2, out var equivalent, out result));
            return equivalent;
        }

        /// <summary>
        /// Compares two assembly identities to determine whether they are equivalent.
        /// </summary>
        /// <param name="pwzAssemblyIdentity1">The display name of the reference assembly.</param>
        /// <param name="fUnified1">Whether the version of the first assembly has been unified by the application.</param>
        /// <param name="pwzAssemblyIdentity2">The display name of the definition assembly.</param>
        /// <param name="fUnified2">Whether the version of the second assembly has been unified by the application.</param>
        /// <param name="pfEquivalent">When this method returns, whether the identities are equivalent.</param>
        /// <param name="pResult">When this method returns, describes how the identities compare.</param>
        /// <returns>An HRESULT.</returns>
        [DllImport("fusion", CharSet = CharSet.Unicode)]
        static extern int CompareAssemblyIdentity(string pwzAssemblyIdentity1, bool fUnified1, string pwzAssemblyIdentity2, bool fUnified2, out bool pfEquivalent, out AssemblyComparisonResult pResult);

        /// <summary>
        /// Compares two assembly identities, following the rules of the native CompareAssemblyIdentity function of
        /// fusion.dll, including .NET Framework and Silverlight unification and retargeting.
        /// </summary>
        /// <param name="coreLibName">The simple name of the core library; a definition with this name matches any reference with the same name.</param>
        /// <param name="assemblyIdentity1">The display name of the reference assembly.</param>
        /// <param name="unified1"><see langword="true"/> if the version of <paramref name="assemblyIdentity1"/> has been unified by the application.</param>
        /// <param name="assemblyIdentity2">The display name of the definition assembly.</param>
        /// <param name="unified2"><see langword="true"/> if the version of <paramref name="assemblyIdentity2"/> has been unified by the application.</param>
        /// <param name="result">When this method returns, describes how the identities compare.</param>
        /// <returns><see langword="true"/> if the identities are equivalent; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">One of the display names is invalid, or is partial where a full name is required.</exception>
        /// <exception cref="System.IO.FileLoadException">The reference has a duplicate attribute, or is partial and retargetable.</exception>
        internal static bool CompareAssemblyIdentityPure(string coreLibName, string assemblyIdentity1, bool unified1, string assemblyIdentity2, bool unified2, out AssemblyComparisonResult result)
        {
            var r1 = ParseAssemblyName(assemblyIdentity1, out var name1);
            var r2 = ParseAssemblyName(assemblyIdentity2, out var name2);

            Version version1;
            if (unified1)
            {
                if (name1.Name == null || !ParseVersion(name1.Version, out version1) || version1 == null || version1.Revision == -1 || name1.Culture == null || name1.PublicKeyToken == null || name1.PublicKeyToken.Length < 2)
                {
                    result = AssemblyComparisonResult.NonEquivalent;
                    throw new ArgumentException();
                }
            }

            if (!ParseVersion(name2.Version, out var version2) || version2 == null || version2.Revision == -1 || name2.Culture == null || name2.PublicKeyToken == null || name2.PublicKeyToken.Length < 2)
            {
                result = AssemblyComparisonResult.NonEquivalent;
                throw new ArgumentException();
            }

            if (name2.Name != null && name2.Name.Equals(coreLibName, StringComparison.OrdinalIgnoreCase))
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
                throw r1 switch
                {
                    ParseAssemblyResult.DuplicateKey => new System.IO.FileLoadException(),
                    _ => new ArgumentException(),
                };
            }

            if (r2 != ParseAssemblyResult.OK)
            {
                result = AssemblyComparisonResult.NonEquivalent;
                throw r2 switch
                {
                    ParseAssemblyResult.DuplicateKey => new System.IO.FileLoadException(),
                    _ => new ArgumentException(),
                };
            }

            if (!ParseVersion(name1.Version, out version1))
            {
                result = AssemblyComparisonResult.NonEquivalent;
                throw new ArgumentException();
            }

            var partial = IsPartial(name1, version1);

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

            if (version1 != null && (remappedPublicKeyToken1 = GetRemappedPublicKeyToken(ref name1, version1)) != null)
            {
                name1.PublicKeyToken = remappedPublicKeyToken1;
                version1 = s_frameworkVersion;
            }

            string remappedPublicKeyToken2;
            if ((remappedPublicKeyToken2 = GetRemappedPublicKeyToken(ref name2, version2)) != null)
            {
                name2.PublicKeyToken = remappedPublicKeyToken2;
                version2 = s_frameworkVersion;
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

            var fxUnified = false;

            if (version1 == null)
            {
                result = AssemblyComparisonResult.Unknown;
                return false;
            }

            if (version2 == null)
            {
                result = AssemblyComparisonResult.Unknown;
                return false;
            }

            // build and revision numbers are ignored
            var fxVersionMatch = version1.Major == version2.Major && version1.Minor == version2.Minor;
            if (IsFrameworkAssembly(name1))
            {
                fxUnified |= !fxVersionMatch;
                version1 = s_frameworkVersion;
            }
            if (IsFrameworkAssembly(name2) && version2 < s_frameworkVersionNext)
            {
                fxUnified |= !fxVersionMatch;
                version2 = s_frameworkVersion;
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
                else if (fxUnified || version1 != version2)
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

        /// <summary>
        /// Determines whether the assembly is a .NET Framework assembly whose version is unified to the framework version.
        /// </summary>
        /// <param name="name">The parsed assembly name.</param>
        /// <returns><see langword="true"/> if the name and public key token identify a framework assembly; otherwise, <see langword="false"/>.</returns>
        static bool IsFrameworkAssembly(ParsedAssemblyName name)
        {
            // framework assemblies use different unification rules, so when
            // a new framework is released the new assemblies need to be added
            return name.Name switch
            {
                "System" or "System.Core" or "System.Data" or "System.Data.DataSetExtensions" or "System.Data.Linq" or "System.Data.OracleClient" or "System.Data.Services" or "System.Data.Services.Client" or "System.IdentityModel" or "System.IdentityModel.Selectors" or "System.IO.Compression" or "System.Numerics" or "System.Reflection.Context" or "System.Runtime.Remoting" or "System.Runtime.Serialization" or "System.Runtime.WindowsRuntime" or "System.Runtime.WindowsRuntime.UI.Xaml" or "System.ServiceModel" or "System.Transactions" or "System.Windows.Forms" or "System.Xml" or "System.Xml.Linq" or "System.Xml.Serialization" => name.PublicKeyToken == PublicKeyTokenEcma,
                "Microsoft.CSharp" or "Microsoft.VisualBasic" or "System.Collections" or "System.Collections.Concurrent" or "System.ComponentModel" or "System.ComponentModel.Annotations" or "System.ComponentModel.EventBasedAsync" or "System.Configuration" or "System.Configuration.Install" or "System.Design" or "System.Diagnostics.Contracts" or "System.Diagnostics.Debug" or "System.Diagnostics.Tools" or "System.Diagnostics.Tracing" or "System.DirectoryServices" or "System.Drawing" or "System.Drawing.Design" or "System.Dynamic.Runtime" or "System.EnterpriseServices" or "System.Globalization" or "System.IO" or "System.Linq" or "System.Linq.Expressions" or "System.Linq.Parallel" or "System.Linq.Queryable" or "System.Management" or "System.Messaging" or "System.Net" or "System.Net.Http" or "System.Net.Http.Rtc" or "System.Net.NetworkInformation" or "System.Net.Primitives" or "System.Net.Requests" or "System.ObjectModel" or "System.Reflection" or "System.Reflection.Extensions" or "System.Reflection.Primitives" or "System.Resources.ResourceManager" or "System.Runtime" or "System.Runtime.Extensions" or "System.Runtime.InteropServices" or "System.Runtime.InteropServices.WindowsRuntime" or "System.Runtime.Numerics" or "System.Runtime.Serialization.Formatters.Soap" or "System.Runtime.Serialization.Json" or "System.Runtime.Serialization.Primitives" or "System.Runtime.Serialization.Xml" or "System.Security" or "System.Security.Principal" or "System.ServiceModel.Duplex" or "System.ServiceModel.Http" or "System.ServiceModel.NetTcp" or "System.ServiceModel.Primitives" or "System.ServiceModel.Security" or "System.ServiceProcess" or "System.Text.Encoding" or "System.Text.Encoding.Extensions" or "System.Text.RegularExpressions" or "System.Threading" or "System.Threading.Tasks" or "System.Threading.Tasks.Parallel" or "System.Web" or "System.Web.Mobile" or "System.Web.Services" or "System.Windows" or "System.Xml.ReaderWriter" or "System.Xml.XDocument" or "System.Xml.XmlSerializer" => name.PublicKeyToken == PublicKeyTokenMicrosoft,
                "System.ComponentModel.DataAnnotations" or "System.ServiceModel.Web" or "System.Web.Abstractions" or "System.Web.Extensions" or "System.Web.Extensions.Design" or "System.Web.DynamicData" or "System.Web.Routing" => name.PublicKeyToken == PublicKeyTokenWinFX,
                _ => false,
            };
        }

        /// <summary>
        /// Gets the .NET Framework public key token that a Silverlight or retargetable assembly reference is remapped to.
        /// </summary>
        /// <param name="name">The parsed assembly name.</param>
        /// <param name="version">The version of the assembly.</param>
        /// <returns>The remapped public key token, or <see langword="null"/> if the assembly is not remapped.</returns>
        static string GetRemappedPublicKeyToken(ref ParsedAssemblyName name, Version version)
        {
            if (name.Retargetable.GetValueOrDefault() && version < s_silverlightVersion)
                return null;

            if (name.PublicKeyToken == "ddd0da4d3e678217" && name.Name == "System.ComponentModel.DataAnnotations" && name.Retargetable.GetValueOrDefault())
                return PublicKeyTokenWinFX;

            if (s_silverlightVersionMinimum <= version && version <= s_silverlightVersionMaximum)
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

        /// <summary>
        /// Parses the simple name at the start of an assembly display name.
        /// </summary>
        /// <param name="fullName">The assembly display name.</param>
        /// <param name="pos">When this method returns, the position following the simple name.</param>
        /// <param name="simpleName">When this method returns, the simple name.</param>
        /// <returns><see cref="ParseAssemblyResult.OK"/> if the simple name was parsed; otherwise, <see cref="ParseAssemblyResult.GenericError"/>.</returns>
        internal static ParseAssemblyResult ParseAssemblySimpleName(string fullName, out int pos, out string simpleName)
        {
            pos = 0;
            if (!TryParse(fullName, ref pos, out simpleName) || simpleName.Length == 0)
                return ParseAssemblyResult.GenericError;

            if (pos == fullName.Length && fullName[^1] == ',')
                return ParseAssemblyResult.GenericError;

            return ParseAssemblyResult.OK;
        }

        /// <summary>
        /// Parses a possibly quoted and escaped token of an assembly display name, up to the next unquoted comma or equals sign.
        /// </summary>
        /// <param name="fullName">The assembly display name.</param>
        /// <param name="pos">The position to start parsing at; advanced past the token.</param>
        /// <param name="value">When this method returns, the token with surrounding white space removed.</param>
        /// <returns><see langword="true"/> if a token was parsed; <see langword="false"/> if the token is empty or malformed.</returns>
        static bool TryParse(string fullName, ref int pos, out string value)
        {
            value = null;

            var sb = new StringBuilder();
            while (pos < fullName.Length && char.IsWhiteSpace(fullName[pos]))
                pos++;

            var quote = -1;
            if (pos < fullName.Length && (fullName[pos] == '"' || fullName[pos] == '\''))
                quote = fullName[pos++];

            for (; pos < fullName.Length; pos++)
            {
                var ch = fullName[pos];
                if (ch == '\\')
                {
                    if (++pos == fullName.Length)
                        return false;

                    ch = fullName[pos];
                    if (ch == '\\')
                        return false;
                }
                else if (ch == quote)
                {
                    for (pos++; pos != fullName.Length; pos++)
                    {
                        ch = fullName[pos];
                        if (ch == ',' || ch == '=')
                            break;
                        if (!char.IsWhiteSpace(ch))
                            return false;
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

        /// <summary>
        /// Consumes the specified character if it is at the current position.
        /// </summary>
        /// <param name="fullName">The assembly display name.</param>
        /// <param name="ch">The character to consume.</param>
        /// <param name="pos">The current position; advanced if the character was consumed.</param>
        /// <returns><see langword="true"/> if the character was consumed; otherwise, <see langword="false"/>.</returns>
        static bool TryConsume(string fullName, char ch, ref int pos)
        {
            if (pos < fullName.Length && fullName[pos] == ch)
            {
                pos++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses a <c>, key=value</c> attribute of an assembly display name.
        /// </summary>
        /// <param name="fullName">The assembly display name.</param>
        /// <param name="pos">The current position; advanced past the attribute.</param>
        /// <param name="key">When this method returns, the attribute key.</param>
        /// <param name="value">When this method returns, the attribute value.</param>
        /// <returns><see langword="true"/> if an attribute was parsed; otherwise, <see langword="false"/>.</returns>
        static bool TryParseAssemblyAttribute(string fullName, ref int pos, ref string key, ref string value)
        {
            return TryConsume(fullName, ',', ref pos)
                && TryParse(fullName, ref pos, out key)
                && TryConsume(fullName, '=', ref pos)
                && TryParse(fullName, ref pos, out value);
        }

        /// <summary>
        /// Parses an assembly display name into its components.
        /// </summary>
        /// <param name="fullName">The assembly display name.</param>
        /// <param name="parsedName">When this method returns, the parsed components.</param>
        /// <returns><see cref="ParseAssemblyResult.OK"/> if the name was parsed, <see cref="ParseAssemblyResult.DuplicateKey"/> if an attribute occurs more than once, or <see cref="ParseAssemblyResult.GenericError"/> if the name is malformed.</returns>
        /// <exception cref="COMException">The public key and public key token attributes do not match.</exception>
        internal static ParseAssemblyResult ParseAssemblyName(string fullName, out ParsedAssemblyName parsedName)
        {
            const int ERROR_SXS_IDENTITIES_DIFFERENT = unchecked((int)0x80073716);

            parsedName = new ParsedAssemblyName();
            var res = ParseAssemblySimpleName(fullName, out var pos, out parsedName.Name);
            if (res != ParseAssemblyResult.OK)
                return res;

            System.Collections.Generic.Dictionary<string, string> unknownAttributes = null;
            var hasProcessorArchitecture = false;
            var hasContentType = false;
            var hasPublicKeyToken = false;
            string publicKeyToken;

            while (pos != fullName.Length)
            {
                string key = null;
                string value = null;
                if (!TryParseAssemblyAttribute(fullName, ref pos, ref key, ref value))
                    return ParseAssemblyResult.GenericError;

                key = key.ToLowerInvariant();
                switch (key)
                {
                    case "version":
                        if (parsedName.Version != null)
                            return ParseAssemblyResult.DuplicateKey;

                        parsedName.Version = value;
                        break;
                    case "culture":
                        if (parsedName.Culture != null)
                            return ParseAssemblyResult.DuplicateKey;
                        if (!ParseCulture(value, out parsedName.Culture))
                            return ParseAssemblyResult.GenericError;

                        break;
                    case "publickeytoken":
                        if (hasPublicKeyToken)
                            return ParseAssemblyResult.DuplicateKey;
                        if (!ParsePublicKeyToken(value, out publicKeyToken))
                            return ParseAssemblyResult.GenericError;
                        if (parsedName.HasPublicKey && parsedName.PublicKeyToken != publicKeyToken)
                            Marshal.ThrowExceptionForHR(ERROR_SXS_IDENTITIES_DIFFERENT);

                        parsedName.PublicKeyToken = publicKeyToken;
                        hasPublicKeyToken = true;
                        break;
                    case "publickey":
                        if (parsedName.HasPublicKey)
                            return ParseAssemblyResult.DuplicateKey;
                        if (!ParsePublicKey(value, out publicKeyToken))
                            return ParseAssemblyResult.GenericError;
                        if (hasPublicKeyToken && parsedName.PublicKeyToken != publicKeyToken)
                            Marshal.ThrowExceptionForHR(ERROR_SXS_IDENTITIES_DIFFERENT);

                        parsedName.PublicKeyToken = publicKeyToken;
                        parsedName.HasPublicKey = true;
                        break;
                    case "retargetable":
                        if (parsedName.Retargetable.HasValue)
                            return ParseAssemblyResult.DuplicateKey;

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
                            return ParseAssemblyResult.DuplicateKey;

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
                            return ParseAssemblyResult.DuplicateKey;

                        hasContentType = true;
                        if (!value.Equals("windowsruntime", StringComparison.OrdinalIgnoreCase))
                            return ParseAssemblyResult.GenericError;

                        parsedName.WindowsRuntime = true;
                        break;
                    default:
                        if (key.Length == 0)
                            return ParseAssemblyResult.GenericError;

                        unknownAttributes ??= [];
                        if (unknownAttributes.ContainsKey(key))
                            return ParseAssemblyResult.DuplicateKey;

                        unknownAttributes.Add(key, null);
                        break;
                }
            }

            return ParseAssemblyResult.OK;
        }

        /// <summary>
        /// Parses the version attribute of an assembly display name.
        /// </summary>
        /// <param name="str">The version string, or <see langword="null"/> if the display name has no version.</param>
        /// <param name="version">When this method returns, the parsed version, or <see langword="null"/> if there is no usable version.</param>
        /// <returns><see langword="false"/> if the version is invalid enough to fail the parse of the whole assembly name; otherwise, <see langword="true"/>.</returns>
        static bool ParseVersion(string str, out Version version)
        {
            if (str == null)
            {
                version = null;
                return true;
            }

            var parts = str.Split('.');
            if (parts.Length < 2 || parts.Length > 4)
            {
                version = null;

                // if the version consists of a single integer, it is invalid, but not invalid enough to fail the parse of the whole assembly name
                return parts.Length == 1 && ushort.TryParse(parts[0], System.Globalization.NumberStyles.Integer, null, out _);
            }

            if (parts[0] == "" || parts[1] == "")
            {
                // this is a strange scenario, the version is invalid, but not invalid enough to fail the parse of the whole assembly name
                version = null;
                return true;
            }

            ushort build = 65535, revision = 65535;
            if (ushort.TryParse(parts[0], System.Globalization.NumberStyles.Integer, null, out var major)
                && ushort.TryParse(parts[1], System.Globalization.NumberStyles.Integer, null, out var minor)
                && (parts.Length <= 2 || parts[2] == "" || ushort.TryParse(parts[2], System.Globalization.NumberStyles.Integer, null, out build))
                && (parts.Length <= 3 || parts[3] == "" || (parts[2] != "" && ushort.TryParse(parts[3], System.Globalization.NumberStyles.Integer, null, out revision))))
            {
                if (parts.Length == 4 && parts[3] != "" && parts[2] != "")
                    version = new Version(major, minor, build, revision);
                else if (parts.Length == 3 && parts[2] != "")
                    version = new Version(major, minor, build);
                else
                    version = new Version(major, minor);

                return true;
            }

            version = null;
            return false;
        }

        /// <summary>
        /// Parses the culture attribute of an assembly display name.
        /// </summary>
        /// <param name="str">The attribute value.</param>
        /// <param name="culture">When this method returns, the culture name.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        static bool ParseCulture(string str, out string culture)
        {
            if (str == null)
            {
                culture = null;
                return false;
            }

            culture = str;
            return true;
        }

        /// <summary>
        /// Parses the public key token attribute of an assembly display name.
        /// </summary>
        /// <param name="str">The attribute value.</param>
        /// <param name="publicKeyToken">When this method returns, the public key token in lower case.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        static bool ParsePublicKeyToken(string str, out string publicKeyToken)
        {
            if (str == null)
            {
                publicKeyToken = null;
                return false;
            }

            publicKeyToken = str.ToLowerInvariant();
            return true;
        }

        /// <summary>
        /// Parses the public key attribute of an assembly display name and computes its public key token.
        /// </summary>
        /// <param name="str">The attribute value, a hexadecimal public key.</param>
        /// <param name="publicKeyToken">When this method returns, the public key token computed from the public key.</param>
        /// <returns><see langword="true"/> if <paramref name="str"/> is not <see langword="null"/>; otherwise, <see langword="false"/>.</returns>
        static bool ParsePublicKey(string str, out string publicKeyToken)
        {
            if (str == null)
            {
                publicKeyToken = null;
                return false;
            }

            publicKeyToken = AssemblyName.ComputePublicKeyToken(str);
            return true;
        }

        /// <summary>
        /// Determines whether an assembly name is partial, that is, lacks a version, culture or public key token.
        /// </summary>
        /// <param name="name">The parsed assembly name.</param>
        /// <param name="version">The parsed version of the assembly.</param>
        /// <returns><see langword="true"/> if the name is partial; otherwise, <see langword="false"/>.</returns>
        static bool IsPartial(ParsedAssemblyName name, Version version) => version == null || name.Culture == null || name.PublicKeyToken == null;

        /// <summary>
        /// Determines whether an assembly name has a public key token other than <c>null</c>.
        /// </summary>
        /// <param name="name">The parsed assembly name.</param>
        /// <returns><see langword="true"/> if the name is strong named; otherwise, <see langword="false"/>.</returns>
        static bool IsStrongNamed(ParsedAssemblyName name) => name.PublicKeyToken != null && name.PublicKeyToken != "null";

    }

}
