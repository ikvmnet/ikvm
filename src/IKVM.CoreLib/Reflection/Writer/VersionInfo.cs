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

        internal void SetName(AssemblyName name)
        {
            this._name = name;
        }

        internal void SetFileName(string assemblyFileName)
        {
            this._fileName = System.IO.Path.GetFileName(assemblyFileName);
        }

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

            int codepage = 1200;    // Unicode codepage

            int lcid = 0x7f;
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

            byte[] preamble2 = new byte[] {
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

        static void WriteUTF16Z(ByteBuffer bb, string str)
        {
            foreach (char c in str)
                bb.WriteInt16((short)c);

            bb.WriteInt16((short)0);
        }

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

        static Version ParseVersionRobust(string ver)
        {
            int index = 0;
            var major = ParseVersionPart(ver, ref index);
            var minor = ParseVersionPart(ver, ref index);
            var build = ParseVersionPart(ver, ref index);
            var revision = ParseVersionPart(ver, ref index);
            return new Version(major, minor, build, revision);
        }

        static ushort ParseVersionPart(string str, ref int pos)
        {
            ushort value = 0;
            while (pos < str.Length)
            {
                char c = str[pos];
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
