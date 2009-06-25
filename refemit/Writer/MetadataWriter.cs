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
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace IKVM.Reflection.Emit.Writer
{
	sealed class MetadataWriter
	{
		private readonly ModuleBuilder moduleBuilder;
		private readonly Stream stream;
		private readonly byte[] buffer = new byte[8];

		internal MetadataWriter(ModuleBuilder moduleBuilder, Stream stream)
		{
			this.moduleBuilder = moduleBuilder;
			this.stream = stream;
		}

		internal ModuleBuilder ModuleBuilder
		{
			get { return moduleBuilder; }
		}

		internal int Position
		{
			get { return (int)stream.Position; }
		}

		internal void Write(ByteBuffer bb)
		{
			bb.WriteTo(stream);
		}

		internal void Write(byte[] value)
		{
			stream.Write(value, 0, value.Length);
		}

		internal void Write(byte value)
		{
			stream.WriteByte(value);
		}

		internal void Write(ushort value)
		{
			Write((short)value);
		}

		internal void Write(short value)
		{
			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			stream.Write(buffer, 0, 2);
		}

		internal void Write(uint value)
		{
			Write((int)value);
		}

		internal void Write(int value)
		{
			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			buffer[2] = (byte)(value >> 16);
			buffer[3] = (byte)(value >> 24);
			stream.Write(buffer, 0, 4);
		}

		internal void Write(ulong value)
		{
			Write((long)value);
		}

		internal void Write(long value)
		{
			buffer[0] = (byte)value;
			buffer[1] = (byte)(value >> 8);
			buffer[2] = (byte)(value >> 16);
			buffer[3] = (byte)(value >> 24);
			buffer[4] = (byte)(value >> 32);
			buffer[5] = (byte)(value >> 40);
			buffer[6] = (byte)(value >> 48);
			buffer[7] = (byte)(value >> 56);
			stream.Write(buffer, 0, 8);
		}

		internal void WriteCompressedInt(int value)
		{
			if (value <= 0x7F)
			{
				Write((byte)value);
			}
			else if (value <= 0x3FFF)
			{
				Write((byte)(0x80 | (value >> 8)));
				Write((byte)value);
			}
			else
			{
				Write((byte)(0xC0 | (value >> 24)));
				Write((byte)(value >> 16));
				Write((byte)(value >> 8));
				Write((byte)value);
			}
		}

		internal static int GetCompressedIntLength(int value)
		{
			if (value <= 0x7F)
			{
				return 1;
			}
			else if (value <= 0x3FFF)
			{
				return 2;
			}
			else
			{
				return 4;
			}
		}

		internal void WriteStringIndex(int index)
		{
			if (moduleBuilder.bigStrings)
			{
				Write(index);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteGuidIndex(int index)
		{
			if (moduleBuilder.bigGuids)
			{
				Write(index);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteBlobIndex(int index)
		{
			if (moduleBuilder.bigBlobs)
			{
				Write(index);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteTypeDefOrRef(int token)
		{
			switch (token >> 24)
			{
				case 0:
					break;
				case TableHeap.TypeDefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 0;
					break;
				case TableHeap.TypeRefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 1;
					break;
				case TableHeap.TypeSpecTable.Index:
					token = (token & 0xFFFFFF) << 2 | 2;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigTypeDefOrRef)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteEncodedTypeDefOrRef(int encodedToken)
		{
			if (moduleBuilder.bigTypeDefOrRef)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteHasCustomAttribute(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigHasCustomAttribute)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteCustomAttributeType(int token)
		{
			switch (token >> 24)
			{
				case TableHeap.MethodDefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 2;
					break;
				case TableHeap.MemberRefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 3;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigCustomAttributeType)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteField(int index)
		{
			if (moduleBuilder.bigField)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteMethodDef(int index)
		{
			if (moduleBuilder.bigMethodDef)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteParam(int index)
		{
			if (moduleBuilder.bigParam)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteTypeDef(int index)
		{
			if (moduleBuilder.bigTypeDef)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteEvent(int index)
		{
			if (moduleBuilder.bigEvent)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteProperty(int index)
		{
			if (moduleBuilder.bigProperty)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteGenericParam(int index)
		{
			if (moduleBuilder.bigGenericParam)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteModuleRef(int index)
		{
			if (moduleBuilder.bigModuleRef)
			{
				Write(index & 0xFFFFFF);
			}
			else
			{
				Write((short)index);
			}
		}

		internal void WriteResolutionScope(int token)
		{
			switch (token >> 24)
			{
				case TableHeap.ModuleTable.Index:
					token = (token & 0xFFFFFF) << 2 | 0;
					break;
				case TableHeap.ModuleRefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 1;
					break;
				case TableHeap.AssemblyRefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 2;
					break;
				case TableHeap.TypeRefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 3;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigResolutionScope)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteMemberRefParent(int token)
		{
			switch (token >> 24)
			{
				case TableHeap.TypeDefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 0;
					break;
				case TableHeap.TypeRefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 1;
					break;
				case TableHeap.ModuleRefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 2;
					break;
				case TableHeap.MethodDefTable.Index:
					token = (token & 0xFFFFFF) << 3 | 3;
					break;
				case TableHeap.TypeSpecTable.Index:
					token = (token & 0xFFFFFF) << 3 | 4;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigMemberRefParent)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteMethodDefOrRef(int token)
		{
			switch (token >> 24)
			{
				case TableHeap.MethodDefTable.Index:
					token = (token & 0xFFFFFF) << 1 | 0;
					break;
				case TableHeap.MemberRefTable.Index:
					token = (token & 0xFFFFFF) << 1 | 1;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigMethodDefOrRef)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteHasConstant(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigHasConstant)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteHasSemantics(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigHasSemantics)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteImplementation(int token)
		{
			switch (token >> 24)
			{
				case 0:
					break;
				case TableHeap.FileTable.Index:
					token = (token & 0xFFFFFF) << 2 | 0;
					break;
				case TableHeap.AssemblyRefTable.Index:
					token = (token & 0xFFFFFF) << 2 | 1;
					break;
				case TableHeap.ExportedTypeTable.Index:
					token = (token & 0xFFFFFF) << 2 | 2;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigImplementation)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteTypeOrMethodDef(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigTypeOrMethodDef)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteHasDeclSecurity(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigHasDeclSecurity)
			{
				Write(encodedToken);
			}
			else
			{
				Write((short)encodedToken);
			}
		}

		internal void WriteMemberForwarded(int token)
		{
			switch (token >> 24)
			{
				case TableHeap.FieldTable.Index:
					token = (token & 0xFFFFFF) << 1 | 0;
				    break;
				case TableHeap.MethodDefTable.Index:
					token = (token & 0xFFFFFF) << 1 | 1;
					break;
				default:
					throw new InvalidOperationException();
			}
			if (moduleBuilder.bigMemberForwarded)
			{
				Write(token);
			}
			else
			{
				Write((short)token);
			}
		}

		internal void WriteHasFieldMarshal(int encodedToken)
		{
			// NOTE because we've already had to do the encoding (to be able to sort the table)
			// here we simple write the value
			if (moduleBuilder.bigHasFieldMarshal)
			{
				Write(encodedToken & 0xFFFFFF);
			}
			else
			{
				Write((short)encodedToken);
			}
		}
	}
}
