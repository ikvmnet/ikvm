/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Collections.Generic;
using IKVM.Internal;
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Attributes
{
	[AttributeUsage(AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Delegate)]
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

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
	public sealed class LineNumberTableAttribute : Attribute
	{
		private byte[] table;

		public LineNumberTableAttribute(ushort lineno)
		{
			LineNumberWriter w = new LineNumberWriter(1);
			w.AddMapping(0, lineno);
			table = w.ToArray();
		}

		public LineNumberTableAttribute(byte[] table)
		{
			this.table = table;
		}

		public sealed class LineNumberWriter
		{
			private System.IO.MemoryStream stream;
			private int prevILOffset;
			private int prevLineNum;
			private int count;

			public LineNumberWriter(int estimatedCount)
			{
				stream = new System.IO.MemoryStream(estimatedCount * 2);
			}

			public void AddMapping(int ilOffset, int linenumber)
			{
				if(count == 0)
				{
					if(ilOffset == 0 && linenumber != 0)
					{
						prevLineNum = linenumber;
						count++;
						WritePackedInteger(linenumber - (64 + 50));
						return;
					}
					else
					{
						prevLineNum = linenumber & ~3;
						WritePackedInteger(((-prevLineNum / 4) - (64 + 50)));
					}
				}
				bool pc_overflow;
				bool lineno_overflow;
				byte lead;
				int deltaPC = ilOffset - prevILOffset;
				if(deltaPC >= 0 && deltaPC < 31)
				{
					lead = (byte)deltaPC;
					pc_overflow = false;
				}
				else
				{
					lead = (byte)31;
					pc_overflow = true;
				}
				int deltaLineNo = linenumber - prevLineNum;
				const int bias = 2;
				if(deltaLineNo >= -bias && deltaLineNo < 7 - bias)
				{
					lead |= (byte)((deltaLineNo + bias) << 5);
					lineno_overflow = false;
				}
				else
				{
					lead |= (byte)(7 << 5);
					lineno_overflow = true;
				}
				stream.WriteByte(lead);
				if(pc_overflow)
				{
					WritePackedInteger(deltaPC - (64 + 31));
				}
				if(lineno_overflow)
				{
					WritePackedInteger(deltaLineNo);
				}
				prevILOffset = ilOffset;
				prevLineNum = linenumber;
				count++;
			}

			public int Count
			{
				get
				{
					return count;
				}
			}

			public int LineNo
			{
				get
				{
					return prevLineNum;
				}
			}

			public byte[] ToArray()
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
			 * C0 - DF      Triple byte integer (-1048576 - 1048576)
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
				else if(val >= -1048576 && val < 1048576)
				{
					val += 1048576;
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
				return val - 1048576;
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
			int i = 0;
			int prevILOffset = 0;
			int prevLineNum = ReadPackedInteger(ref i) + (64 + 50);
			int line;
			if(prevLineNum > 0)
			{
				line = prevLineNum;
			}
			else
			{
				prevLineNum = 4 * -prevLineNum;
				line = -1;
			}
			while(i < table.Length)
			{
				byte lead = table[i++];
				int deltaPC = lead & 31;
				int deltaLineNo = (lead >> 5) - 2;
				if(deltaPC == 31)
				{
					deltaPC = ReadPackedInteger(ref i) + (64 + 31);
				}
				if(deltaLineNo == 5)
				{
					deltaLineNo = ReadPackedInteger(ref i);
				}
				int currILOffset = prevILOffset + deltaPC;
				if(currILOffset > ilOffset)
				{
					return line;
				}
				line = prevLineNum + deltaLineNo;
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

	[AttributeUsage(AttributeTargets.Interface)]
	public sealed class RemappedInterfaceMethodAttribute : Attribute
	{
		private string name;
		private string mappedTo;
		private string[] throws;

		public RemappedInterfaceMethodAttribute(string name, string mappedTo, string[] throws)
		{
			this.name = name;
			this.mappedTo = mappedTo;
			this.throws = throws;
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

		public string[] Throws
		{
			get
			{
				return throws;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class RemappedClassAttribute : Attribute
	{
		private string name;
		private Type remappedType;

#if STUB_GENERATOR
		public RemappedClassAttribute(string name, System.Type remappedType)
		{
		}
#endif

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

#if STUB_GENERATOR
		public RemappedTypeAttribute(System.Type type)
		{
		}
#endif

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
		private string[] classMap;
		private string[] jars;

		public JavaModuleAttribute()
		{
		}

		public JavaModuleAttribute(string[] classMap)
		{
			this.classMap = UnicodeUtil.UnescapeInvalidSurrogates(classMap);
		}

		public string[] GetClassMap()
		{
			return classMap;
		}

		public string[] Jars
		{
			get { return jars; }
			set { jars = value; }
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Assembly)]
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
		private readonly HideFromJavaFlags flags;

		public HideFromJavaAttribute()
		{
			flags = HideFromJavaFlags.All;
		}

		public HideFromJavaAttribute(HideFromJavaFlags flags)
		{
			this.flags = flags;
		}

		public HideFromJavaFlags Flags
		{
			get { return flags; }
		}
	}

	[Flags]
	public enum HideFromJavaFlags : byte
	{
		All = Code | Reflection | StackWalk | StackTrace,
		None = 0,
		Code = 1,
		Reflection = 2,
		StackWalk = 4,		// used for LambdaForm$Compiled
		StackTrace = 8,		// used for LambdaForm$Hidden
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
		Bridge			= 0x0040,
		Transient		= 0x0080,
		VarArgs			= 0x0080,
		Native			= 0x0100,
		Interface		= 0x0200,
		Abstract		= 0x0400,
		Strictfp		= 0x0800,
		Synthetic		= 0x1000,
		Annotation		= 0x2000,
		Enum			= 0x4000,

		// Masks
		AccessMask		= Public | Private | Protected
	}

	[AttributeUsage(AttributeTargets.All)]
	public sealed class ModifiersAttribute : Attribute
	{
		private Modifiers modifiers;
		private bool isInternal;

		public ModifiersAttribute(Modifiers modifiers)
		{
			this.modifiers = modifiers;
		}

		public ModifiersAttribute(Modifiers modifiers, bool isInternal)
		{
			this.modifiers = modifiers;
			this.isInternal = isInternal;
		}

		public bool IsInternal
		{
			get
			{
				return isInternal;
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

	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Field)]
	public sealed class NameSigAttribute : Attribute
	{
		private string name;
		private string sig;

		public NameSigAttribute(string name, string sig)
		{
			this.name = UnicodeUtil.UnescapeInvalidSurrogates(name);
			this.sig = UnicodeUtil.UnescapeInvalidSurrogates(sig);
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
		internal string[] classes;
		internal Type[] types;

		// this constructor is used by ikvmc, the other constructors are for use in other .NET languages
		public ThrowsAttribute(string[] classes)
		{
			this.classes = UnicodeUtil.UnescapeInvalidSurrogates(classes);
		}

		public ThrowsAttribute(Type type)
			: this(new Type[] { type })
		{
		}

		public ThrowsAttribute(params Type[] types)
		{
			this.types = types;
		}

		// dotted Java class names (e.g. java.lang.Throwable)
		[Obsolete]
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
			this.interfaces = UnicodeUtil.UnescapeInvalidSurrogates(interfaces);
		}

		public string[] Interfaces
		{
			get
			{
				return interfaces;
			}
		}
	}

	// NOTE this attribute is also used by annotation attribute classes,
	// to give them a different name in the Java world ($Proxy[Annotation]).
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class InnerClassAttribute : Attribute
	{
		private string innerClassName;
		private Modifiers modifiers;

		public InnerClassAttribute(string innerClassName, Modifiers modifiers)
		{
			this.innerClassName = UnicodeUtil.UnescapeInvalidSurrogates(innerClassName);
			this.modifiers = modifiers;
		}

		public string InnerClassName
		{
			get
			{
				return innerClassName;
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

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
	public sealed class NonNestedInnerClassAttribute : Attribute
	{
		private string innerClassName;

		public NonNestedInnerClassAttribute(string innerClassName)
		{
			this.innerClassName = UnicodeUtil.UnescapeInvalidSurrogates(innerClassName);
		}

		public string InnerClassName
		{
			get
			{
				return innerClassName;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class NonNestedOuterClassAttribute : Attribute
	{
		private string outerClassName;

		public NonNestedOuterClassAttribute(string outerClassName)
		{
			this.outerClassName = UnicodeUtil.UnescapeInvalidSurrogates(outerClassName);
		}

		public string OuterClassName
		{
			get
			{
				return outerClassName;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Assembly)]
	public sealed class CustomAssemblyClassLoaderAttribute : Attribute
	{
		private Type type;

		public CustomAssemblyClassLoaderAttribute(Type type)
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

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method | AttributeTargets.Field)]
	public sealed class SignatureAttribute : Attribute
	{
		private string signature;

		public SignatureAttribute(string signature)
		{
			this.signature = UnicodeUtil.UnescapeInvalidSurrogates(signature);
		}

		public string Signature
		{
			get
			{
				return signature;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class EnclosingMethodAttribute : Attribute
	{
		private string className;
		private string methodName;
		private string methodSig;

		public EnclosingMethodAttribute(string className, string methodName, string methodSig)
		{
			this.className = UnicodeUtil.UnescapeInvalidSurrogates(className);
			this.methodName = UnicodeUtil.UnescapeInvalidSurrogates(methodName);
			this.methodSig = UnicodeUtil.UnescapeInvalidSurrogates(methodSig);
		}

		internal EnclosingMethodAttribute SetClassName(Type type)
		{
			if (className == null)
			{
				className = IKVM.Internal.ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).Name;
			}
			return this;
		}

		public string ClassName
		{
			get
			{
				return className;
			}
		}

		public string MethodName
		{
			get
			{
				return methodName;
			}
		}

		public string MethodSignature
		{
			get
			{
				return methodSig;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class AnnotationDefaultAttribute : Attribute
	{
		public const byte TAG_ENUM = (byte)'e';
		public const byte TAG_CLASS = (byte)'c';
		public const byte TAG_ANNOTATION = (byte)'@';
		public const byte TAG_ARRAY = (byte)'[';
		public const byte TAG_ERROR = (byte)'?';
		private object defaultValue;

		// element_value encoding:
		// primitives:
		//   boxed values
		// string:
		//   string
		// enum:
		//   new object[] { (byte)'e', "<EnumType>", "<enumvalue>" }
		// class:
		//   new object[] { (byte)'c', "<Type>" }
		// annotation:
		//   new object[] { (byte)'@', "<AnnotationType>", ("name", (element_value))* }
		// array:
		//   new object[] { (byte)'[', (element_value)* }
		// error:
		//   new object[] { (byte)'?', "<exceptionClass>", "<exceptionMessage>" }
		public AnnotationDefaultAttribute(object defaultValue)
		{
			this.defaultValue = Unescape(defaultValue);
		}

		public object Value
		{
			get
			{
				return defaultValue;
			}
		}

		internal static object Escape(object obj)
		{
			return EscapeOrUnescape(obj, true);
		}

		internal static object Unescape(object obj)
		{
			return EscapeOrUnescape(obj, false);
		}

		private static object EscapeOrUnescape(object obj, bool escape)
		{
			string str = obj as string;
			if (str != null)
			{
				return escape
					? UnicodeUtil.EscapeInvalidSurrogates(str)
					: UnicodeUtil.UnescapeInvalidSurrogates(str);
			}
			object[] arr = obj as object[];
			if (arr != null)
			{
				for (int i = 0; i < arr.Length; i++)
				{
					arr[i] = EscapeOrUnescape(arr[i], escape);
				}
			}
			return obj;
		}
	}

	[AttributeUsage(AttributeTargets.Interface)]
	public sealed class AnnotationAttributeAttribute : Attribute
	{
		private string attributeType;

		public AnnotationAttributeAttribute(string attributeType)
		{
			this.attributeType = UnicodeUtil.UnescapeInvalidSurrogates(attributeType);
		}

		public string AttributeType
		{
			get
			{
				return attributeType;
			}
		}
	}

	[AttributeUsage(AttributeTargets.Module, AllowMultiple = true)]
	public sealed class PackageListAttribute : Attribute
	{
		internal string jar;
		internal string[] packages;

		public PackageListAttribute(string jar, string[] packages)
		{
			this.jar = jar;
			this.packages = UnicodeUtil.UnescapeInvalidSurrogates(packages);
		}

		public string[] GetPackages()
		{
			return packages;
		}
	}

	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class JavaResourceAttribute : Attribute
	{
		private readonly string javaName;
		private readonly string resourceName;

		public JavaResourceAttribute(string javaName, string resourceName)
		{
			this.javaName = javaName;
			this.resourceName = resourceName;
		}

		public string JavaName
		{
			get { return javaName; }
		}

		public string ResourceName
		{
			get { return resourceName; }
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Delegate | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Parameter, AllowMultiple = false)]
	public sealed class DynamicAnnotationAttribute : Attribute
	{
		private readonly object[] definition;

		public DynamicAnnotationAttribute(object[] definition)
		{
			this.definition = (object[])AnnotationDefaultAttribute.Unescape(definition);
		}

		public object[] Definition
		{
			get { return definition; }
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor)]
	public sealed class MethodParametersAttribute : Attribute
	{
		private readonly Modifiers[] modifiers;

		public MethodParametersAttribute(Modifiers[] modifiers)
		{
			this.modifiers = modifiers;
		}

		public Modifiers[] Modifiers
		{
			get { return modifiers; }
		}

		public bool IsMalformed
		{
			get { return modifiers == null; }
		}
	}

	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class ConstantPoolAttribute : Attribute
	{
		internal readonly object[] constantPool;

		public ConstantPoolAttribute(object[] constantPool)
		{
			this.constantPool = Decompress(constantPool);
		}

		internal static object[] Decompress(object[] constantPool)
		{
			List<object> list = new List<object>();
			foreach (object obj in constantPool)
			{
				int emptySlots = obj as byte? ?? obj as ushort? ?? 0;
				if (emptySlots == 0)
				{
					list.Add(Unescape(obj));
				}
				else
				{
					for (int i = 0; i < emptySlots; i++)
					{
						list.Add(null);
					}
				}
			}
			return list.ToArray();
		}

		private static object Unescape(object obj)
		{
			string str = obj as string;
			if (str != null)
			{
				obj = UnicodeUtil.UnescapeInvalidSurrogates(str);
			}
			return obj;
		}

		internal static object[] Compress(object[] constantPool, bool[] inUse)
		{
			int length = constantPool.Length;
			while (!inUse[length - 1])
			{
				length--;
			}
			int write = 0;
			for (int read = 0; read < length; read++)
			{
				int start = read;
				while (!inUse[read])
				{
					read++;
				}
				int emptySlots = read - start;
				if (emptySlots > 255)
				{
					constantPool[write++] = (ushort)emptySlots;
				}
				else if (emptySlots > 0)
				{
					constantPool[write++] = (byte)emptySlots;
				}
				constantPool[write++] = Escape(constantPool[read]);
			}
			Array.Resize(ref constantPool, write);
			return constantPool;
		}

		private static object Escape(object obj)
		{
			string str = obj as string;
			if (str != null)
			{
				obj = UnicodeUtil.EscapeInvalidSurrogates(str);
			}
			return obj;
		}
	}

	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Field)]
	public sealed class RuntimeVisibleTypeAnnotationsAttribute : Attribute
	{
		internal readonly byte[] data;

		public RuntimeVisibleTypeAnnotationsAttribute(byte[] data)
		{
			this.data = data;
		}
	}

	// used in custom modifier for access stubs
	public static class AccessStub { }
}
