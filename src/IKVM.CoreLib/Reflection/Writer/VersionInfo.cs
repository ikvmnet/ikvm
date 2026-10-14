/*
  Copyright (C) 2008 Jeroen Frijters

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

using IKVM.Reflection.Emit;

namespace IKVM.Reflection.Writer
{

    /// <summary>
    /// Collects the assembly name, file name and the standard assembly attributes (copyright, product, file version
    /// and so on) and writes them as a Win32 VS_VERSIONINFO resource.
    /// </summary>
    sealed class VersionInfo
    {

        AssemblyName _name;
        string _fileName;
        string _copyright;
        string _trademark;
        string _product;
        string _company;
        string _description;
        string _title;
        string _informationalVersion;
        string _fileVersion;

        /// <summary>
        /// Sets the assembly name, which supplies the internal name, the culture and the default file version.
        /// </summary>
        /// <param name="name">The assembly name.</param>
        internal void SetName(AssemblyName name) => _name = name;

        /// <summary>
        /// Sets the original file name, taken from the file name part of the specified path.
        /// </summary>
        /// <param name="assemblyFileName">The path of the assembly file.</param>
        internal void SetFileName(string assemblyFileName) => _fileName = System.IO.Path.GetFileName(assemblyFileName);

        /// <summary>
        /// Records the value of a custom attribute if it is one of the assembly attributes that feed the version
        /// resource. Only the first occurrence of each attribute is used; other attributes are ignored.
        /// </summary>
        /// <param name="asm">The assembly the attribute is applied to.</param>
        /// <param name="cab">The custom attribute.</param>
        internal void SetAttribute(AssemblyBuilder asm, CustomAttributeBuilder cab)
        {
            var u = cab.Constructor.Module.Universe;
            var type = cab.Constructor.DeclaringType;
            if (_copyright == null && type == u.System_Reflection_AssemblyCopyrightAttribute)
            {
                _copyright = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_trademark == null && type == u.System_Reflection_AssemblyTrademarkAttribute)
            {
                _trademark = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_product == null && type == u.System_Reflection_AssemblyProductAttribute)
            {
                _product = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_company == null && type == u.System_Reflection_AssemblyCompanyAttribute)
            {
                _company = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_description == null && type == u.System_Reflection_AssemblyDescriptionAttribute)
            {
                _description = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_title == null && type == u.System_Reflection_AssemblyTitleAttribute)
            {
                _title = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_informationalVersion == null && type == u.System_Reflection_AssemblyInformationalVersionAttribute)
            {
                _informationalVersion = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
            else if (_fileVersion == null && type == u.System_Reflection_AssemblyFileVersionAttribute)
            {
                _fileVersion = (string)cab.DecodeBlob(asm).GetConstructorArgument(0);
            }
        }

        /// <summary>
        /// Writes the VS_VERSIONINFO resource. When no file version attribute was set, the assembly version, or
        /// 0.0.0.0, is used; the product version is parsed from the informational version when one was set.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        internal void Write(ByteBuffer bb)
        {
            if (_fileVersion == null)
            {
                if (_name.Version != null)
                {
                    _fileVersion = _name.Version.ToString();
                }
                else
                {
                    _fileVersion = "0.0.0.0";
                }
            }

            // Unicode codepage
            var codepage = 1200;

            var lcid = 0x7f;
            try
            {
                if (_name.CultureInfo != null)
                    lcid = _name.CultureInfo.LCID;
            }
            catch (ArgumentException)
            {
                // AssemblyName.CultureInfo throws an ArgumentException if AssemblyBuilder.__SetAssemblyCulture() was used to specify a non-existing culture
            }

            var filever = ParseVersionRobust(_fileVersion);
            var fileVersionMajor = filever.Major;
            var fileVersionMinor = filever.Minor;
            var fileVersionBuild = filever.Build;
            var fileVersionRevision = filever.Revision;

            var productVersionMajor = fileVersionMajor;
            var productVersionMinor = fileVersionMinor;
            var productVersionBuild = fileVersionBuild;
            var productVersionRevision = fileVersionRevision;
            if (_informationalVersion != null)
            {
                Version productver = ParseVersionRobust(_informationalVersion);
                productVersionMajor = productver.Major;
                productVersionMinor = productver.Minor;
                productVersionBuild = productver.Build;
                productVersionRevision = productver.Revision;
            }

            var stringTable = new ByteBuffer(512);
            stringTable.WriteInt16((short)0);    // wLength (placeholder)
            stringTable.WriteInt16((short)0);    // wValueLength
            stringTable.WriteInt16((short)1);    // wType
            WriteUTF16Z(stringTable, string.Format("{0:x4}{1:x4}", lcid, codepage));
            stringTable.Align(4);

            WriteString(stringTable, "Comments", _description);
            WriteString(stringTable, "CompanyName", _company);
            WriteString(stringTable, "FileDescription", _title);
            WriteString(stringTable, "FileVersion", _fileVersion);
            WriteString(stringTable, "InternalName", _name.Name);
            WriteString(stringTable, "LegalCopyright", _copyright);
            WriteString(stringTable, "LegalTrademarks", _trademark);
            WriteString(stringTable, "OriginalFilename", _fileName);
            WriteString(stringTable, "ProductName", _product);
            WriteString(stringTable, "ProductVersion", _informationalVersion);

            stringTable.Position = 0;
            stringTable.WriteInt16((short)stringTable.Length);

            var stringFileInfo = new ByteBuffer(512);
            stringFileInfo.WriteInt16((short)0); // wLength (placeholder)
            stringFileInfo.WriteInt16((short)0); // wValueLength
            stringFileInfo.WriteInt16((short)1); // wType
            WriteUTF16Z(stringFileInfo, "StringFileInfo");
            stringFileInfo.Align(4);
            stringFileInfo.WriteBuffer(stringTable);
            stringFileInfo.Position = 0;
            stringFileInfo.WriteInt16((short)stringFileInfo.Length);

            var preamble1 = new byte[] {
			  // VS_VERSIONINFO (platform SDK)
			  0x34, 0x00,				// wValueLength
			  0x00, 0x00,				// wType
			  0x56, 0x00, 0x53, 0x00, 0x5F, 0x00, 0x56, 0x00, 0x45, 0x00, 0x52, 0x00, 0x53, 0x00, 0x49, 0x00, 0x4F, 0x00, 0x4E, 0x00, 0x5F, 0x00, 0x49, 0x00, 0x4E, 0x00, 0x46, 0x00, 0x4F, 0x00, 0x00, 0x00,  // "VS_VERSION_INFO\0"
			  0x00, 0x00,				// Padding1 (32 bit alignment)
			  // VS_FIXEDFILEINFO starts
			  0xBD, 0x04, 0xEF, 0xFE,	// dwSignature (0xFEEF04BD)
			  0x00, 0x00, 0x01, 0x00,	// dwStrucVersion
			};

            var preamble2 = new byte[] {
              0x3F, 0x00, 0x00, 0x00,	// dwFileFlagsMask (??)
			  0x00, 0x00, 0x00, 0x00,	// dwFileFlags (??)
			  0x04, 0x00, 0x00, 0x00,	// dwFileOS
			  0x02, 0x00, 0x00, 0x00,	// dwFileType
			  0x00, 0x00, 0x00, 0x00,	// dwFileSubtype
			  0x00, 0x00, 0x00, 0x00,	// dwFileDateMS
			  0x00, 0x00, 0x00, 0x00,	// dwFileDateLS
										// Padding2 (32 bit alignment)
			  // VarFileInfo
			  0x44, 0x00,				// wLength
			  0x00, 0x00,				// wValueLength
			  0x01, 0x00,				// wType
			  0x56, 0x00, 0x61, 0x00, 0x72, 0x00, 0x46, 0x00, 0x69, 0x00, 0x6C, 0x00, 0x65, 0x00, 0x49, 0x00, 0x6E, 0x00, 0x66, 0x00, 0x6F, 0x00, 0x00, 0x00,	// "VarFileInfo\0"
			  0x00, 0x00,				// Padding
			  // Var
			  0x24, 0x00,				// wLength
			  0x04, 0x00,				// wValueLength
			  0x00, 0x00,				// wType
			  0x54, 0x00, 0x72, 0x00, 0x61, 0x00, 0x6E, 0x00, 0x73, 0x00, 0x6C, 0x00, 0x61, 0x00, 0x74, 0x00, 0x69, 0x00, 0x6F, 0x00, 0x6E, 0x00, 0x00, 0x00,	// "Translation\0"
			  0x00, 0x00,				// Padding (32 bit alignment)
			};

            bb.WriteInt16((short)(2 + preamble1.Length + 8 + 8 + preamble2.Length + 4 + stringFileInfo.Length));
            bb.WriteBytes(preamble1);
            bb.WriteInt16((short)fileVersionMinor);
            bb.WriteInt16((short)fileVersionMajor);
            bb.WriteInt16((short)fileVersionRevision);
            bb.WriteInt16((short)fileVersionBuild);
            bb.WriteInt16((short)productVersionMinor);
            bb.WriteInt16((short)productVersionMajor);
            bb.WriteInt16((short)productVersionRevision);
            bb.WriteInt16((short)productVersionBuild);
            bb.WriteBytes(preamble2);
            bb.WriteInt16((short)lcid);
            bb.WriteInt16((short)codepage);
            bb.WriteBuffer(stringFileInfo);
        }

        /// <summary>
        /// Writes a string as null-terminated UTF-16.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="str">The string to write.</param>
        static void WriteUTF16Z(ByteBuffer bb, string str)
        {
            foreach (var c in str)
                bb.WriteInt16((short)c);

            bb.WriteInt16((short)0);
        }

        /// <summary>
        /// Writes a String structure (a name and value pair) of a StringTable. A missing value is written as a single
        /// space.
        /// </summary>
        /// <param name="bb">The buffer to write to.</param>
        /// <param name="name">The name of the entry, for example <c>CompanyName</c>.</param>
        /// <param name="value">The value of the entry, or <see langword="null"/>.</param>
        static void WriteString(ByteBuffer bb, string name, string value)
        {
            value ??= " ";
            var pos = bb.Position;
            bb.WriteInt16((short)0);                 // wLength (placeholder)
            bb.WriteInt16((short)(value.Length + 1));// wValueLength
            bb.WriteInt16((short)1);                 // wType
            WriteUTF16Z(bb, name);
            bb.Align(4);
            WriteUTF16Z(bb, value);
            bb.Align(4);
            var savedPos = bb.Position;
            bb.Position = pos;
            bb.WriteInt16((short)(savedPos - pos));
            bb.Position = savedPos;
        }

        /// <summary>
        /// Parses up to four dot-separated numeric parts from a version string, tolerating malformed input. Parsing
        /// of the parts stops at the first character that is neither a digit nor a dot, and missing parts are 0.
        /// </summary>
        /// <param name="ver">The version string.</param>
        /// <returns>The parsed version.</returns>
        static Version ParseVersionRobust(string ver)
        {
            var index = 0;
            var major = ParseVersionPart(ver, ref index);
            var minor = ParseVersionPart(ver, ref index);
            var build = ParseVersionPart(ver, ref index);
            var revision = ParseVersionPart(ver, ref index);
            return new Version(major, minor, build, revision);
        }

        /// <summary>
        /// Parses the digits of one version part starting at <paramref name="pos"/>, consuming a following dot.
        /// </summary>
        /// <param name="str">The version string.</param>
        /// <param name="pos">The position to start at. It is advanced past the digits and the dot, if any.</param>
        /// <returns>The numeric value of the part, wrapping on overflow.</returns>
        static ushort ParseVersionPart(string str, ref int pos)
        {
            ushort value = 0;
            while (pos < str.Length)
            {
                var c = str[pos];
                if (c == '.')
                {
                    pos++;
                    break;
                }
                else if (c >= '0' && c <= '9')
                {
                    value *= 10;
                    value += (ushort)(c - '0');
                    pos++;
                }
                else
                {
                    break;
                }
            }
            return value;
        }

    }

}
