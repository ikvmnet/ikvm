/*
  Copyright (C) 2002, 2003, 2004 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.Attributes
{
	public sealed class SourceFileAttribute : Attribute
	{
		private string file;

		public SourceFileAttribute(string file)
		{
			this.file = file;
		}

		public string SourceFile
		{
			get
			{
				return file;
			}
		}
	}

	public sealed class LineNumberTableAttribute : Attribute
	{
		private byte[] table;

		public LineNumberTableAttribute(byte[] table)
		{
			this.table = table;
		}

		internal class LineNumberWriter
		{
			private System.IO.MemoryStream stream;
			private int prevILOffset;
			private int prevLineNum;

			internal LineNumberWriter(int estimatedCount)
			{
				stream = new System.IO.MemoryStream(estimatedCount * 2);
			}

			internal void AddMapping(int ilOffset, int linenumber)
			{
				WritePackedInteger(ilOffset - prevILOffset);
				WritePackedInteger(linenumber - prevLineNum);
				prevILOffset = ilOffset;
				prevLineNum = linenumber;
			}

			internal byte[] ToArray()
			{
				return stream.ToArray();
			}

			/*
			 * packed integer format:
			 * ----------------------
			 * 
			 * First byte:
			 * 00 - 7F      Single byte integer (-64 - 63)
			 * 80 - BF      Double byte integer (-8192 - 8191)
			 * C0 - DF      Triple byte integer (-2097152 - 2097151)
			 * E0 - FE      Reserved
			 * FF           Five byte integer
			 */
			private void WritePackedInteger(int val)
			{
				if(val >= -64 && val < 64)
				{
					val += 64;
					stream.WriteByte((byte)val);
				}
				else if(val >= -8192 && val < 8192)
				{
					val += 8192;
					stream.WriteByte((byte)(0x80 + (val >> 8)));
					stream.WriteByte((byte)val);
				}
				else if(val >= -2097152 && val < 2097152)
				{
					val += 2097152;
					stream.WriteByte((byte)(0xC0 + (val >> 16)));
					stream.WriteByte((byte)(val >> 8));
					stream.WriteByte((byte)val);
				}
				else
				{
					stream.WriteByte(0xFF);
					stream.WriteByte((byte)(val >> 24));
					stream.WriteByte((byte)(val >> 16));
					stream.WriteByte((byte)(val >>  8));
					stream.WriteByte((byte)(val >>  0));
				}
			}
		}

		private int ReadPackedInteger(ref int position)
		{
			byte b = table[position++];
			if(b < 128)
			{
				return b - 64;
			}
			else if((b & 0xC0) == 0x80)
			{
				return ((b & 0x7F) << 8) + table[position++] - 8192;
			}
			else if((b & 0xE0) == 0xC0)
			{
				int val = ((b & 0x3F) << 16);
				val += (table[position++] << 8);
				val += table[position++];
				return val - 2097152;
			}
			else if(b == 0xFF)
			{
				int val = table[position++] << 24;
				val += table[position++] << 16;
				val += table[position++] <<  8;
				val += table[position++] <<  0;
				return val;
			}
			else
			{
				throw new InvalidProgramException();
			}
		}

		public int GetLineNumber(int ilOffset)
		{
			int prevILOffset = 0;
			int prevLineNum = 0;
			int line = -1;
			for(int i = 0; i < table.Length;)
			{
				int currILOffset = ReadPackedInteger(ref i) + prevILOffset;
				if(currILOffset > ilOffset)
				{
					return line;
				}
				line = ReadPackedInteger(ref i) + prevLineNum;
				prevILOffset = currILOffset;
				prevLineNum = line;
			}
			return line;
		}
	}

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ExceptionIsUnsafeForMappingAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class RemappedInterfaceMethodAttribute : Attribute
	{
		private string name;
		private string mappedTo;

		public RemappedInterfaceMethodAttribute(string name, string mappedTo)
		{
			this.name = name;
			this.mappedTo = mappedTo;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string MappedTo
		{
			get
			{
				return mappedTo;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class RemappedClassAttribute : Attribute
	{
		private string name;
		private Type remappedType;

		public RemappedClassAttribute(string name, Type remappedType)
		{
			this.name = name;
			this.remappedType = remappedType;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public Type RemappedType
		{
			get
			{
				return remappedType;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class RemappedTypeAttribute : Attribute
	{
		private Type type;

		public RemappedTypeAttribute(Type type)
		{
			this.type = type;
		}

		public Type Type
		{
			get
			{
				return type;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Module)]
	public sealed class JavaModuleAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Assembly)]
	public sealed class NoPackagePrefixAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Struct)]
	public sealed class GhostInterfaceAttribute : Attribute
	{
	}

	// Whenever the VM or compiler generates a helper class/method/field, it should be marked
	// with this custom attribute, so that it can be hidden from Java.
	[AttributeUsage(AttributeTargets.All)]
	public sealed class HideFromJavaAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class MirandaMethodAttribute : Attribute
	{
	}

	[Flags]
	public enum Modifiers : ushort
	{
		Public			= 0x0001,
		Private			= 0x0002,
		Protected		= 0x0004,
		Static			= 0x0008,
		Final			= 0x0010,
		Super			= 0x0020,
		Synchronized	= 0x0020,
		Volatile		= 0x0040,
		Transient		= 0x0080,
		Native			= 0x0100,
		Interface		= 0x0200,
		Abstract		= 0x0400,
		Strictfp		= 0x0800,

		// Masks
		AccessMask		= Public | Private | Protected
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class ModifiersAttribute : Attribute
	{
		private Modifiers modifiers;

		public ModifiersAttribute(Modifiers modifiers)
		{
			this.modifiers = modifiers;
		}

		public Modifiers Modifiers
		{
			get
			{
				return modifiers;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field)]
	public sealed class NameSigAttribute : Attribute
	{
		private string name;
		private string sig;

		public NameSigAttribute(string name, string sig)
		{
			this.name = name;
			this.sig = sig;
		}

		public string Name
		{
			get
			{
				return name;
			}
		}

		public string Sig
		{
			get
			{
				return sig;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
	public sealed class ThrowsAttribute : Attribute
	{
		private string[] classes;

		// NOTE this is not CLS compliant, so maybe we should have a couple of overloads
		public ThrowsAttribute(string[] classes)
		{
			this.classes = classes;
		}

		// dotted Java class names (e.g. java.lang.Throwable)
		public string[] Classes
		{
			get
			{
				return classes;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ImplementsAttribute : Attribute
	{
		private string[] interfaces;

		// NOTE this is not CLS compliant, so maybe we should have a couple of overloads
		public ImplementsAttribute(string[] interfaces)
		{
			this.interfaces = interfaces;
		}

		public string[] Interfaces
		{
			get
			{
				return interfaces;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class InnerClassAttribute : Attribute
	{
		private string innerClassName;
		private string outerClassName;
		private string name;
		private Modifiers modifiers;

		public InnerClassAttribute(string innerClassName, string outerClassName, string name, Modifiers modifiers)
		{
			this.innerClassName = innerClassName;
			this.outerClassName = outerClassName;
			this.name = name;
			this.modifiers = modifiers;
		}

		public string InnerClassName
		{
			get
			{
				return innerClassName;
			}
		}

		public string OuterClassName
		{
			get
			{
				return outerClassName;
			}
		}

		// NOTE returns null for anonymous inner classes
		public string Name
		{
			get
			{
				return name;
			}
		}

		public Modifiers Modifiers
		{
			get
			{
				return modifiers;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Field)]
	public sealed class ConstantValueAttribute : Attribute
	{
		private object val;

		public ConstantValueAttribute(bool val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(byte val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(short val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(char val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(int val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(long val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(float val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(double val)
		{
			this.val = val;
		}

		public ConstantValueAttribute(string val)
		{
			this.val = val;
		}

		public object GetConstantValue()
		{
			return val;
		}
	}
}
