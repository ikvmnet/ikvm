/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
#if STUB_GENERATOR
using IKVM.Reflection;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif
using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.StubGen
{
	static class StubGenerator
	{
		internal static void WriteClass(Stream stream, TypeWrapper tw, bool includeNonPublicInterfaces, bool includeNonPublicMembers, bool includeSerialVersionUID)
		{
			string name = tw.Name.Replace('.', '/');
			string super = null;
			if (tw.IsInterface)
			{
				super = "java/lang/Object";
			}
			else if (tw.BaseTypeWrapper != null)
			{
				super = tw.BaseTypeWrapper.Name.Replace('.', '/');
			}
			ClassFileWriter writer = new ClassFileWriter(tw.Modifiers, name, super, 0, 49);
			foreach (TypeWrapper iface in tw.Interfaces)
			{
				if (iface.IsPublic || includeNonPublicInterfaces)
				{
					writer.AddInterface(iface.Name.Replace('.', '/'));
				}
			}
			InnerClassesAttribute innerClassesAttribute = null;
			if (tw.DeclaringTypeWrapper != null)
			{
				TypeWrapper outer = tw.DeclaringTypeWrapper;
				string innername = name;
				int idx = name.LastIndexOf('$');
				if (idx >= 0)
				{
					innername = innername.Substring(idx + 1);
				}
				innerClassesAttribute = new InnerClassesAttribute(writer);
				innerClassesAttribute.Add(name, outer.Name.Replace('.', '/'), innername, (ushort)tw.ReflectiveModifiers);
			}
			foreach (TypeWrapper inner in tw.InnerClasses)
			{
				if (inner.IsPublic)
				{
					if (innerClassesAttribute == null)
					{
						innerClassesAttribute = new InnerClassesAttribute(writer);
					}
					string namePart = inner.Name;
					namePart = namePart.Substring(namePart.LastIndexOf('$') + 1);
					innerClassesAttribute.Add(inner.Name.Replace('.', '/'), name, namePart, (ushort)inner.ReflectiveModifiers);
				}
			}
			if (innerClassesAttribute != null)
			{
				writer.AddAttribute(innerClassesAttribute);
			}
			string genericTypeSignature = tw.GetGenericSignature();
			if (genericTypeSignature != null)
			{
				writer.AddStringAttribute("Signature", genericTypeSignature);
			}
			AddAnnotations(writer, writer, tw.TypeAsBaseType);
			writer.AddStringAttribute("IKVM.NET.Assembly", GetAssemblyName(tw));
			if (tw.TypeAsBaseType.IsDefined(JVM.Import(typeof(ObsoleteAttribute)), false))
			{
				writer.AddAttribute(new DeprecatedAttribute(writer));
			}
			foreach (MethodWrapper mw in tw.GetMethods())
			{
				if (!mw.IsHideFromReflection && (mw.IsPublic || mw.IsProtected || includeNonPublicMembers))
				{
					FieldOrMethod m;
					if (mw.Name == "<init>")
					{
						m = writer.AddMethod(mw.Modifiers, mw.Name, mw.Signature.Replace('.', '/'));
						CodeAttribute code = new CodeAttribute(writer);
						code.MaxLocals = (ushort)(mw.GetParameters().Length * 2 + 1);
						code.MaxStack = 3;
						ushort index1 = writer.AddClass("java/lang/UnsatisfiedLinkError");
						ushort index2 = writer.AddString("ikvmstub generated stubs can only be used on IKVM.NET");
						ushort index3 = writer.AddMethodRef("java/lang/UnsatisfiedLinkError", "<init>", "(Ljava/lang/String;)V");
						code.ByteCode = new byte[] {
						187, (byte)(index1 >> 8), (byte)index1,	// new java/lang/UnsatisfiedLinkError
						89,										// dup
						19,	 (byte)(index2 >> 8), (byte)index2,	// ldc_w "..."
						183, (byte)(index3 >> 8), (byte)index3, // invokespecial java/lang/UnsatisfiedLinkError/init()V
						191										// athrow
					};
						m.AddAttribute(code);
					}
					else
					{
						Modifiers mods = mw.Modifiers;
						if ((mods & Modifiers.Abstract) == 0)
						{
							mods |= Modifiers.Native;
						}
						m = writer.AddMethod(mods, mw.Name, mw.Signature.Replace('.', '/'));
						if (mw.IsOptionalAttributeAnnotationValue)
						{
							m.AddAttribute(new AnnotationDefaultClassFileAttribute(writer, GetAnnotationDefault(writer, mw.ReturnType)));
						}
					}
					MethodBase mb = mw.GetMethod();
					if (mb != null)
					{
						ThrowsAttribute throws = AttributeHelper.GetThrows(mb);
						if (throws == null)
						{
							string[] throwsArray = mw.GetDeclaredExceptions();
							if (throwsArray != null && throwsArray.Length > 0)
							{
								ExceptionsAttribute attrib = new ExceptionsAttribute(writer);
								foreach (string ex in throwsArray)
								{
									attrib.Add(ex.Replace('.', '/'));
								}
								m.AddAttribute(attrib);
							}
						}
						else
						{
							ExceptionsAttribute attrib = new ExceptionsAttribute(writer);
							if (throws.classes != null)
							{
								foreach (string ex in throws.classes)
								{
									attrib.Add(ex.Replace('.', '/'));
								}
							}
							if (throws.types != null)
							{
								foreach (Type ex in throws.types)
								{
									attrib.Add(ClassLoaderWrapper.GetWrapperFromType(ex).Name.Replace('.', '/'));
								}
							}
							m.AddAttribute(attrib);
						}
						if (mb.IsDefined(JVM.Import(typeof(ObsoleteAttribute)), false)
							// HACK the instancehelper methods are marked as Obsolete (to direct people toward the ikvm.extensions methods instead)
							// but in the Java world most of them are not deprecated (and to keep the Japi results clean we need to reflect this)
							&& (!mb.Name.StartsWith("instancehelper_")
								|| mb.DeclaringType.FullName != "java.lang.String"
							// the Java deprecated methods actually have two Obsolete attributes
								|| GetObsoleteCount(mb) == 2))
						{
							m.AddAttribute(new DeprecatedAttribute(writer));
						}
						CustomAttributeData attr = GetAnnotationDefault(mb);
						if (attr != null)
						{
							m.AddAttribute(new AnnotationDefaultClassFileAttribute(writer, GetAnnotationDefault(writer, attr.ConstructorArguments[0])));
						}
					}
					string sig = tw.GetGenericMethodSignature(mw);
					if (sig != null)
					{
						m.AddAttribute(writer.MakeStringAttribute("Signature", sig));
					}
					AddAnnotations(writer, m, mw.GetMethod());
					AddParameterAnnotations(writer, m, mw.GetMethod());
				}
			}
			bool hasSerialVersionUID = false;
			foreach (FieldWrapper fw in tw.GetFields())
			{
				if (!fw.IsHideFromReflection)
				{
					bool isSerialVersionUID = includeSerialVersionUID && fw.Name == "serialVersionUID" && fw.FieldTypeWrapper == PrimitiveTypeWrapper.LONG;
					hasSerialVersionUID |= isSerialVersionUID;
					if (fw.IsPublic || fw.IsProtected || isSerialVersionUID || includeNonPublicMembers)
					{
						object constant = null;
						if (fw.GetField() != null && fw.GetField().IsLiteral && (fw.FieldTypeWrapper.IsPrimitive || fw.FieldTypeWrapper == CoreClasses.java.lang.String.Wrapper))
						{
							constant = fw.GetField().GetRawConstantValue();
							if (fw.GetField().FieldType.IsEnum)
							{
								constant = EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(fw.GetField().FieldType), constant);
							}
						}
						FieldOrMethod f = writer.AddField(fw.Modifiers, fw.Name, fw.Signature.Replace('.', '/'), constant);
						string sig = tw.GetGenericFieldSignature(fw);
						if (sig != null)
						{
							f.AddAttribute(writer.MakeStringAttribute("Signature", sig));
						}
						if (fw.GetField() != null && fw.GetField().IsDefined(JVM.Import(typeof(ObsoleteAttribute)), false))
						{
							f.AddAttribute(new DeprecatedAttribute(writer));
						}
						AddAnnotations(writer, f, fw.GetField());
					}
				}
			}
			if (includeSerialVersionUID && !hasSerialVersionUID && IsSerializable(tw))
			{
				// class is serializable but doesn't have an explicit serialVersionUID, so we add the field to record
				// the serialVersionUID as we see it (mainly to make the Japi reports more realistic)
				writer.AddField(Modifiers.Private | Modifiers.Static | Modifiers.Final, "serialVersionUID", "J", SerialVersionUID.Compute(tw));
			}
			AddMetaAnnotations(writer, tw);
			writer.Write(stream);
		}

		private static void AddAnnotations(ClassFileWriter writer, IAttributeOwner target, MemberInfo source)
		{
#if !FIRST_PASS && !STUB_GENERATOR
			if (source != null)
			{
				RuntimeVisibleAnnotationsAttribute attr = null;
				foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(source))
				{
					object[] ann = GetAnnotation(cad);
					if (ann != null)
					{
						if (attr == null)
						{
							attr = new RuntimeVisibleAnnotationsAttribute(writer);
						}
						attr.Add(UnpackArray((IList<CustomAttributeTypedArgument>)cad.ConstructorArguments[0].Value));
					}
				}
				if (attr != null)
				{
					target.AddAttribute(attr);
				}
			}
#endif
		}

		private static void AddParameterAnnotations(ClassFileWriter writer, FieldOrMethod target, MethodBase source)
		{
#if !FIRST_PASS && !STUB_GENERATOR
			if (source != null)
			{
				RuntimeVisibleParameterAnnotationsAttribute attr = null;
				ParameterInfo[] parameters = source.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					RuntimeVisibleAnnotationsAttribute param = null;
					foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(parameters[i]))
					{
						object[] ann = GetAnnotation(cad);
						if (ann != null)
						{
							if (param == null)
							{
								if (attr == null)
								{
									attr = new RuntimeVisibleParameterAnnotationsAttribute(writer);
									for (int j = 0; j < i; j++)
									{
										attr.Add(new RuntimeVisibleAnnotationsAttribute(writer));
									}
								}
								param = new RuntimeVisibleAnnotationsAttribute(writer);
							}
							param.Add(UnpackArray((IList<CustomAttributeTypedArgument>)cad.ConstructorArguments[0].Value));
						}
					}
					if (attr != null)
					{
						attr.Add(param ?? new RuntimeVisibleAnnotationsAttribute(writer));
					}
				}
				if (attr != null)
				{
					target.AddAttribute(attr);
				}
			}
#endif
		}

#if !FIRST_PASS && !STUB_GENERATOR
		private static object[] GetAnnotation(CustomAttributeData cad)
		{
			if (cad.ConstructorArguments.Count == 1 && cad.ConstructorArguments[0].ArgumentType == typeof(object[]) &&
				(cad.Constructor.DeclaringType.IsSubclassOf(JVM.Import(typeof(ikvm.@internal.AnnotationAttributeBase)))
				|| cad.Constructor.DeclaringType == JVM.Import(typeof(DynamicAnnotationAttribute))))
			{
				return UnpackArray((IList<CustomAttributeTypedArgument>)cad.ConstructorArguments[0].Value);
			}
			return null;
		}
#endif

		private static object[] UnpackArray(IList<CustomAttributeTypedArgument> list)
		{
			object[] arr = new object[list.Count];
			for (int i = 0; i < arr.Length; i++)
			{
				if (list[i].Value is IList<CustomAttributeTypedArgument>)
				{
					arr[i] = UnpackArray((IList<CustomAttributeTypedArgument>)list[i].Value);
				}
				else
				{
					arr[i] = list[i].Value;
				}
			}
			return arr;
		}

		private static int GetObsoleteCount(MethodBase mb)
		{
#if STUB_GENERATOR
			return mb.__GetCustomAttributes(JVM.Import(typeof(ObsoleteAttribute)), false).Count;
#else
			return mb.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length;
#endif
		}

		private static CustomAttributeData GetAnnotationDefault(MethodBase mb)
		{
#if STUB_GENERATOR
			IList<CustomAttributeData> attr = CustomAttributeData.__GetCustomAttributes(mb, JVM.LoadType(typeof(AnnotationDefaultAttribute)), false);
			return attr.Count == 1 ? attr[0] : null;
#else
			foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mb))
			{
				if (cad.Constructor.DeclaringType == typeof(AnnotationDefaultAttribute))
				{
					return cad;
				}
			}
			return null;
#endif
		}

		private static string GetAssemblyName(TypeWrapper tw)
		{
			ClassLoaderWrapper loader = tw.GetClassLoader();
			AssemblyClassLoader acl = loader as AssemblyClassLoader;
			if (acl != null)
			{
				return acl.GetAssembly(tw).FullName;
			}
			else
			{
				return ((GenericClassLoaderWrapper)loader).GetName();
			}
		}

		private static bool IsSerializable(TypeWrapper tw)
		{
			if (tw.Name == "java.io.Serializable")
			{
				return true;
			}
			while (tw != null)
			{
				foreach (TypeWrapper iface in tw.Interfaces)
				{
					if (IsSerializable(iface))
					{
						return true;
					}
				}
				tw = tw.BaseTypeWrapper;
			}
			return false;
		}

		private static void AddMetaAnnotations(ClassFileWriter writer, TypeWrapper tw)
		{
			DotNetTypeWrapper.AttributeAnnotationTypeWrapperBase attributeAnnotation = tw as DotNetTypeWrapper.AttributeAnnotationTypeWrapperBase;
			if (attributeAnnotation != null)
			{
				// TODO write the annotation directly, instead of going thru the object[] encoding
				RuntimeVisibleAnnotationsAttribute annot = new RuntimeVisibleAnnotationsAttribute(writer);
				annot.Add(new object[] {
					AnnotationDefaultAttribute.TAG_ANNOTATION,
					"Ljava/lang/annotation/Retention;",
					"value",
					new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/RetentionPolicy;", "RUNTIME" }
				});
				AttributeTargets validOn = attributeAnnotation.AttributeTargets;
				List<object> targets = new List<object>();
				targets.Add(AnnotationDefaultAttribute.TAG_ARRAY);
				if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "TYPE" });
				}
				if ((validOn & AttributeTargets.Constructor) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "CONSTRUCTOR" });
				}
				if ((validOn & AttributeTargets.Field) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "FIELD" });
				}
				if ((validOn & (AttributeTargets.Method | AttributeTargets.ReturnValue)) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "METHOD" });
				}
				if ((validOn & AttributeTargets.Parameter) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "PARAMETER" });
				}
				annot.Add(new object[] {
					AnnotationDefaultAttribute.TAG_ANNOTATION,
					"Ljava/lang/annotation/Target;",
					"value",
					targets.ToArray()
				});
				writer.AddAttribute(annot);
			}
		}

		private static byte[] GetAnnotationDefault(ClassFileWriter classFile, TypeWrapper type)
		{
			MemoryStream mem = new MemoryStream();
			BigEndianStream bes = new BigEndianStream(mem);
			if (type == PrimitiveTypeWrapper.BOOLEAN)
			{
				bes.WriteByte((byte)'Z');
				bes.WriteUInt16(classFile.AddInt(0));
			}
			else if (type == PrimitiveTypeWrapper.BYTE)
			{
				bes.WriteByte((byte)'B');
				bes.WriteUInt16(classFile.AddInt(0));
			}
			else if (type == PrimitiveTypeWrapper.CHAR)
			{
				bes.WriteByte((byte)'C');
				bes.WriteUInt16(classFile.AddInt(0));
			}
			else if (type == PrimitiveTypeWrapper.SHORT)
			{
				bes.WriteByte((byte)'S');
				bes.WriteUInt16(classFile.AddInt(0));
			}
			else if (type == PrimitiveTypeWrapper.INT)
			{
				bes.WriteByte((byte)'I');
				bes.WriteUInt16(classFile.AddInt(0));
			}
			else if (type == PrimitiveTypeWrapper.FLOAT)
			{
				bes.WriteByte((byte)'F');
				bes.WriteUInt16(classFile.AddFloat(0));
			}
			else if (type == PrimitiveTypeWrapper.LONG)
			{
				bes.WriteByte((byte)'J');
				bes.WriteUInt16(classFile.AddLong(0));
			}
			else if (type == PrimitiveTypeWrapper.DOUBLE)
			{
				bes.WriteByte((byte)'D');
				bes.WriteUInt16(classFile.AddDouble(0));
			}
			else if (type == CoreClasses.java.lang.String.Wrapper)
			{
				bes.WriteByte((byte)'s');
				bes.WriteUInt16(classFile.AddUtf8(""));
			}
			else if ((type.Modifiers & Modifiers.Enum) != 0)
			{
				bes.WriteByte((byte)'e');
				bes.WriteUInt16(classFile.AddUtf8("L" + type.Name.Replace('.', '/') + ";"));
				bes.WriteUInt16(classFile.AddUtf8("__unspecified"));
			}
			else if (type == CoreClasses.java.lang.Class.Wrapper)
			{
				bes.WriteByte((byte)'c');
				bes.WriteUInt16(classFile.AddUtf8("Likvm/internal/__unspecified;"));
			}
			else if (type.IsArray)
			{
				bes.WriteByte((byte)'[');
				bes.WriteUInt16(0);
			}
			else
			{
				throw new InvalidOperationException();
			}
			return mem.ToArray();
		}

		private static byte[] GetAnnotationDefault(ClassFileWriter classFile, CustomAttributeTypedArgument value)
		{
			MemoryStream mem = new MemoryStream();
			BigEndianStream bes = new BigEndianStream(mem);
			try
			{
				WriteAnnotationElementValue(classFile, bes, value);
			}
			catch (InvalidCastException)
			{
				Warning("Warning: incorrect annotation default value");
			}
			catch (IndexOutOfRangeException)
			{
				Warning("Warning: incorrect annotation default value");
			}
			return mem.ToArray();
		}

		private static void WriteAnnotationElementValue(ClassFileWriter classFile, BigEndianStream bes, CustomAttributeTypedArgument value)
		{
			if (value.ArgumentType == Types.Boolean)
			{
				bes.WriteByte((byte)'Z');
				bes.WriteUInt16(classFile.AddInt((bool)value.Value ? 1 : 0));
			}
			else if (value.ArgumentType == Types.Byte)
			{
				bes.WriteByte((byte)'B');
				bes.WriteUInt16(classFile.AddInt((byte)value.Value));
			}
			else if (value.ArgumentType == Types.Char)
			{
				bes.WriteByte((byte)'C');
				bes.WriteUInt16(classFile.AddInt((char)value.Value));
			}
			else if (value.ArgumentType == Types.Int16)
			{
				bes.WriteByte((byte)'S');
				bes.WriteUInt16(classFile.AddInt((short)value.Value));
			}
			else if (value.ArgumentType == Types.Int32)
			{
				bes.WriteByte((byte)'I');
				bes.WriteUInt16(classFile.AddInt((int)value.Value));
			}
			else if (value.ArgumentType == Types.Single)
			{
				bes.WriteByte((byte)'F');
				bes.WriteUInt16(classFile.AddFloat((float)value.Value));
			}
			else if (value.ArgumentType == Types.Int64)
			{
				bes.WriteByte((byte)'J');
				bes.WriteUInt16(classFile.AddLong((long)value.Value));
			}
			else if (value.ArgumentType == Types.Double)
			{
				bes.WriteByte((byte)'D');
				bes.WriteUInt16(classFile.AddDouble((double)value.Value));
			}
			else if (value.ArgumentType == Types.String)
			{
				bes.WriteByte((byte)'s');
				bes.WriteUInt16(classFile.AddUtf8((string)value.Value));
			}
			else if (value.ArgumentType == Types.Object.MakeArrayType())
			{
				CustomAttributeTypedArgument[] array = (CustomAttributeTypedArgument[])value.Value;
				byte type = (byte)array[0].Value;
				if (type == AnnotationDefaultAttribute.TAG_ARRAY)
				{
					bes.WriteByte((byte)'[');
					bes.WriteUInt16((ushort)(array.Length - 1));
					for (int i = 1; i < array.Length; i++)
					{
						WriteAnnotationElementValue(classFile, bes, array[i]);
					}
				}
				else if (type == AnnotationDefaultAttribute.TAG_CLASS)
				{
					bes.WriteByte((byte)'c');
					bes.WriteUInt16(classFile.AddUtf8((string)array[1].Value));
				}
				else if (type == AnnotationDefaultAttribute.TAG_ENUM)
				{
					bes.WriteByte((byte)'e');
					bes.WriteUInt16(classFile.AddUtf8((string)array[1].Value));
					bes.WriteUInt16(classFile.AddUtf8((string)array[2].Value));
				}
				else if (type == AnnotationDefaultAttribute.TAG_ANNOTATION)
				{
					bes.WriteByte((byte)'@');
					bes.WriteUInt16(classFile.AddUtf8((string)array[1].Value));
					bes.WriteUInt16((ushort)((array.Length - 2) / 2));
					for (int i = 2; i < array.Length; i += 2)
					{
						bes.WriteUInt16(classFile.AddUtf8((string)array[i].Value));
						WriteAnnotationElementValue(classFile, bes, array[i + 1]);
					}
				}
				else
				{
					Warning("Warning: incorrect annotation default element tag: " + type);
				}
			}
			else
			{
				Warning("Warning: incorrect annotation default element type: " + value.ArgumentType);
			}
		}

		private static void Warning(string message)
		{
#if STUB_GENERATOR
			Console.Error.WriteLine(message);
#endif
		}
	}
}
