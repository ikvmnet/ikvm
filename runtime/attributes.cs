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
		private ushort[] table;
		private byte[] wideTable;

		public LineNumberTableAttribute(ushort[] table)
		{
			this.table = table;
		}

		public LineNumberTableAttribute(byte[] table)
		{
			this.wideTable = table;
		}

		public int GetLineNumber(int ilOffset)
		{
			int line = -1;
			if(table != null)
			{
				for(int i = 0; i < table.Length; i += 2)
				{
					if(table[i + 0] > ilOffset)
					{
						return line;
					}
					line = table[i + 1];
				}
			}
			else
			{
				for(int i = 0; i < wideTable.Length; i += 6)
				{
					int offset =
						(wideTable[i + 0] << 0) +
						(wideTable[i + 1] << 8) +
						(wideTable[i + 2] << 16) +
						(wideTable[i + 3] << 24);
					if(offset > ilOffset)
					{
						return line;
					}
					line = wideTable[i + 4] + (wideTable[i + 5] << 8);
				}
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
		Strictfp		= 0x0800
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
				return OuterClassName;
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
}
