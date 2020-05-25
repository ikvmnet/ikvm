/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
#if STATIC_COMPILER || STUB_GENERATOR
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;

namespace IKVM.Internal
{
	static class StringConstants
	{
		internal static readonly string CLINIT = string.Intern("<clinit>");
		internal static readonly string INIT = string.Intern("<init>");
		internal static readonly string SIG_VOID = string.Intern("()V");
		internal static readonly string FINALIZE = string.Intern("finalize");
		internal static readonly string CLONE = string.Intern("clone");
	}

	struct ExModifiers
	{
		internal readonly Modifiers Modifiers;
		internal readonly bool IsInternal;

		internal ExModifiers(Modifiers modifiers, bool isInternal)
		{
			this.Modifiers = modifiers;
			this.IsInternal = isInternal;
		}
	}

	struct MethodParametersEntry
	{
		internal static readonly MethodParametersEntry[] Malformed = new MethodParametersEntry[0];
		internal string name;
		internal ushort flags;
	}

	static class AttributeHelper
	{
#if STATIC_COMPILER
		private static CustomAttributeBuilder ghostInterfaceAttribute;
		private static CustomAttributeBuilder deprecatedAttribute;
		private static CustomAttributeBuilder editorBrowsableNever;
		private static ConstructorInfo implementsAttribute;
		private static ConstructorInfo throwsAttribute;
		private static ConstructorInfo sourceFileAttribute;
		private static ConstructorInfo lineNumberTableAttribute1;
		private static ConstructorInfo lineNumberTableAttribute2;
		private static ConstructorInfo enclosingMethodAttribute;
		private static ConstructorInfo signatureAttribute;
		private static ConstructorInfo methodParametersAttribute;
		private static ConstructorInfo runtimeVisibleTypeAnnotationsAttribute;
		private static ConstructorInfo constantPoolAttribute;
		private static CustomAttributeBuilder paramArrayAttribute;
		private static ConstructorInfo nonNestedInnerClassAttribute;
		private static ConstructorInfo nonNestedOuterClassAttribute;
		private static readonly Type typeofModifiers = JVM.LoadType(typeof(Modifiers));
		private static readonly Type typeofSourceFileAttribute = JVM.LoadType(typeof(SourceFileAttribute));
		private static readonly Type typeofLineNumberTableAttribute = JVM.LoadType(typeof(LineNumberTableAttribute));
#endif // STATIC_COMPILER
		private static readonly Type typeofRemappedClassAttribute = JVM.LoadType(typeof(RemappedClassAttribute));
		private static readonly Type typeofRemappedTypeAttribute = JVM.LoadType(typeof(RemappedTypeAttribute));
		private static readonly Type typeofModifiersAttribute = JVM.LoadType(typeof(ModifiersAttribute));
		private static readonly Type typeofRemappedInterfaceMethodAttribute = JVM.LoadType(typeof(RemappedInterfaceMethodAttribute));
		private static readonly Type typeofNameSigAttribute = JVM.LoadType(typeof(NameSigAttribute));
		private static readonly Type typeofJavaModuleAttribute = JVM.LoadType(typeof(JavaModuleAttribute));
		private static readonly Type typeofSignatureAttribute = JVM.LoadType(typeof(SignatureAttribute));
		private static readonly Type typeofInnerClassAttribute = JVM.LoadType(typeof(InnerClassAttribute));
		private static readonly Type typeofImplementsAttribute = JVM.LoadType(typeof(ImplementsAttribute));
		private static readonly Type typeofGhostInterfaceAttribute = JVM.LoadType(typeof(GhostInterfaceAttribute));
		private static readonly Type typeofExceptionIsUnsafeForMappingAttribute = JVM.LoadType(typeof(ExceptionIsUnsafeForMappingAttribute));
		private static readonly Type typeofThrowsAttribute = JVM.LoadType(typeof(ThrowsAttribute));
		private static readonly Type typeofHideFromJavaAttribute = JVM.LoadType(typeof(HideFromJavaAttribute));
		private static readonly Type typeofHideFromJavaFlags = JVM.LoadType(typeof(HideFromJavaFlags));
		private static readonly Type typeofNoPackagePrefixAttribute = JVM.LoadType(typeof(NoPackagePrefixAttribute));
		private static readonly Type typeofAnnotationAttributeAttribute = JVM.LoadType(typeof(AnnotationAttributeAttribute));
		private static readonly Type typeofNonNestedInnerClassAttribute = JVM.LoadType(typeof(NonNestedInnerClassAttribute));
		private static readonly Type typeofNonNestedOuterClassAttribute = JVM.LoadType(typeof(NonNestedOuterClassAttribute));
		private static readonly Type typeofEnclosingMethodAttribute = JVM.LoadType(typeof(EnclosingMethodAttribute));
		private static readonly Type typeofMethodParametersAttribute = JVM.LoadType(typeof(MethodParametersAttribute));
		private static readonly Type typeofRuntimeVisibleTypeAnnotationsAttribute = JVM.LoadType(typeof(RuntimeVisibleTypeAnnotationsAttribute));
		private static readonly Type typeofConstantPoolAttribute = JVM.LoadType(typeof(ConstantPoolAttribute));
		private static readonly CustomAttributeBuilder hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
		private static readonly CustomAttributeBuilder hideFromReflection = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(new Type[] { typeofHideFromJavaFlags }), new object[] { HideFromJavaFlags.Reflection | HideFromJavaFlags.StackTrace | HideFromJavaFlags.StackWalk });

		// we don't want beforefieldinit
		static AttributeHelper() { }

#if STATIC_COMPILER
		private static object ParseValue(ClassLoaderWrapper loader, TypeWrapper tw, string val)
		{
			if(tw == CoreClasses.java.lang.String.Wrapper)
			{
				return val;
			}
			else if(tw.IsUnloadable)
			{
				throw new FatalCompilerErrorException(Message.MapFileTypeNotFound, tw.Name);
			}
			else if(tw.TypeAsTBD.IsEnum)
			{
				return EnumHelper.Parse(tw.TypeAsTBD, val);
			}
			else if(tw.TypeAsTBD == Types.Type)
			{
				TypeWrapper valtw = loader.LoadClassByDottedNameFast(val);
				if(valtw != null)
				{
					return valtw.TypeAsBaseType;
				}
				return StaticCompiler.Universe.GetType(val, true);
			}
			else if(tw == PrimitiveTypeWrapper.BOOLEAN)
			{
				return bool.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.BYTE)
			{
				return (byte)sbyte.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.CHAR)
			{
				return char.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.SHORT)
			{
				return short.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.INT)
			{
				return int.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.FLOAT)
			{
				return float.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.LONG)
			{
				return long.Parse(val);
			}
			else if(tw == PrimitiveTypeWrapper.DOUBLE)
			{
				return double.Parse(val);
			}
			else
			{
				throw new NotImplementedException();
			}
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, TypeBuilder tb, IKVM.Internal.MapXml.Attribute attr)
		{
			bool declarativeSecurity;
			CustomAttributeBuilder cab = CreateCustomAttribute(loader, attr, out declarativeSecurity);
			if (declarativeSecurity)
			{
				tb.__AddDeclarativeSecurity(cab);
			}
			else
			{
				tb.SetCustomAttribute(cab);
			}
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, FieldBuilder fb, IKVM.Internal.MapXml.Attribute attr)
		{
			fb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, ParameterBuilder pb, IKVM.Internal.MapXml.Attribute attr)
		{
			pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, MethodBuilder mb, IKVM.Internal.MapXml.Attribute attr)
		{
			bool declarativeSecurity;
			CustomAttributeBuilder cab = CreateCustomAttribute(loader, attr, out declarativeSecurity);
			if (declarativeSecurity)
			{
				mb.__AddDeclarativeSecurity(cab);
			}
			else
			{
				mb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
			}
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, PropertyBuilder pb, IKVM.Internal.MapXml.Attribute attr)
		{
			pb.SetCustomAttribute(CreateCustomAttribute(loader, attr));
		}

		internal static void SetCustomAttribute(ClassLoaderWrapper loader, AssemblyBuilder ab, IKVM.Internal.MapXml.Attribute attr)
		{
			ab.SetCustomAttribute(CreateCustomAttribute(loader, attr));
		}

		private static void GetAttributeArgsAndTypes(ClassLoaderWrapper loader, IKVM.Internal.MapXml.Attribute attr, out Type[] argTypes, out object[] args)
		{
			// TODO add error handling
			TypeWrapper[] twargs = loader.ArgTypeWrapperListFromSig(attr.Sig, LoadMode.Link);
			argTypes = new Type[twargs.Length];
			args = new object[argTypes.Length];
			for(int i = 0; i < twargs.Length; i++)
			{
				argTypes[i] = twargs[i].TypeAsSignatureType;
				TypeWrapper tw = twargs[i];
				if(tw == CoreClasses.java.lang.Object.Wrapper)
				{
					tw = loader.FieldTypeWrapperFromSig(attr.Params[i].Sig, LoadMode.Link);
				}
				if(tw.IsArray)
				{
					Array arr = Array.CreateInstance(Type.__GetSystemType(Type.GetTypeCode(tw.ElementTypeWrapper.TypeAsArrayType)), attr.Params[i].Elements.Length);
					for(int j = 0; j < arr.Length; j++)
					{
						arr.SetValue(ParseValue(loader, tw.ElementTypeWrapper, attr.Params[i].Elements[j].Value), j);
					}
					args[i] = arr;
				}
				else
				{
					args[i] = ParseValue(loader, tw, attr.Params[i].Value);
				}
			}
		}

		private static CustomAttributeBuilder CreateCustomAttribute(ClassLoaderWrapper loader, IKVM.Internal.MapXml.Attribute attr)
		{
			bool ignore;
			return CreateCustomAttribute(loader, attr, out ignore);
		}

		private static CustomAttributeBuilder CreateCustomAttribute(ClassLoaderWrapper loader, IKVM.Internal.MapXml.Attribute attr, out bool isDeclarativeSecurity)
		{
			// TODO add error handling
			Type[] argTypes;
			object[] args;
			GetAttributeArgsAndTypes(loader, attr, out argTypes, out args);
			if(attr.Type != null)
			{
				Type t = StaticCompiler.GetTypeForMapXml(loader, attr.Type);
				isDeclarativeSecurity = t.IsSubclassOf(Types.SecurityAttribute);
				ConstructorInfo ci = t.GetConstructor(argTypes);
				if(ci == null)
				{
					throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Type, attr.Sig));
				}
				PropertyInfo[] namedProperties;
				object[] propertyValues;
				if(attr.Properties != null)
				{
					namedProperties = new PropertyInfo[attr.Properties.Length];
					propertyValues = new object[attr.Properties.Length];
					for(int i = 0; i < namedProperties.Length; i++)
					{
						namedProperties[i] = t.GetProperty(attr.Properties[i].Name);
						propertyValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Properties[i].Sig, LoadMode.Link), attr.Properties[i].Value);
					}
				}
				else
				{
					namedProperties = new PropertyInfo[0];
					propertyValues = new object[0];
				}
				FieldInfo[] namedFields;
				object[] fieldValues;
				if(attr.Fields != null)
				{
					namedFields = new FieldInfo[attr.Fields.Length];
					fieldValues = new object[attr.Fields.Length];
					for(int i = 0; i < namedFields.Length; i++)
					{
						namedFields[i] = t.GetField(attr.Fields[i].Name);
						fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
					}
				}
				else
				{
					namedFields = new FieldInfo[0];
					fieldValues = new object[0];
				}
				return new CustomAttributeBuilder(ci, args, namedProperties, propertyValues, namedFields, fieldValues);
			}
			else
			{
				if(attr.Properties != null)
				{
					throw new NotImplementedException("Setting property values on Java attributes is not implemented");
				}
				TypeWrapper t = loader.LoadClassByDottedName(attr.Class);
				isDeclarativeSecurity = t.TypeAsBaseType.IsSubclassOf(Types.SecurityAttribute);
				FieldInfo[] namedFields;
				object[] fieldValues;
				if(attr.Fields != null)
				{
					namedFields = new FieldInfo[attr.Fields.Length];
					fieldValues = new object[attr.Fields.Length];
					for(int i = 0; i < namedFields.Length; i++)
					{
						FieldWrapper fw = t.GetFieldWrapper(attr.Fields[i].Name, attr.Fields[i].Sig);
						fw.Link();
						namedFields[i] = fw.GetField();
						fieldValues[i] = ParseValue(loader, loader.FieldTypeWrapperFromSig(attr.Fields[i].Sig, LoadMode.Link), attr.Fields[i].Value);
					}
				}
				else
				{
					namedFields = new FieldInfo[0];
					fieldValues = new object[0];
				}
				MethodWrapper mw = t.GetMethodWrapper("<init>", attr.Sig, false);
				if (mw == null)
				{
					throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));
				}
				mw.Link();
				ConstructorInfo ci = (mw.GetMethod() as ConstructorInfo) ?? ((MethodInfo)mw.GetMethod()).__AsConstructorInfo();
				return new CustomAttributeBuilder(ci, args, namedFields, fieldValues);
			}
		}

		private static CustomAttributeBuilder GetEditorBrowsableNever()
		{
			if (editorBrowsableNever == null)
			{
				// to avoid having to load (and find) System.dll, we construct a symbolic CustomAttributeBuilder
				AssemblyName name = Types.Object.Assembly.GetName();
				name.Name = "System";
				Universe u = StaticCompiler.Universe;
				Type typeofEditorBrowsableAttribute = u.ResolveType(Types.Object.Assembly, "System.ComponentModel.EditorBrowsableAttribute, " + name.FullName);
				Type typeofEditorBrowsableState = u.ResolveType(Types.Object.Assembly, "System.ComponentModel.EditorBrowsableState, " + name.FullName);
				u.MissingTypeIsValueType += delegate(Type type) { return type == typeofEditorBrowsableState; };
				ConstructorInfo ctor = (ConstructorInfo)typeofEditorBrowsableAttribute.__CreateMissingMethod(ConstructorInfo.ConstructorName,
					CallingConventions.Standard | CallingConventions.HasThis, null, default(CustomModifiers), new Type[] { typeofEditorBrowsableState }, null);
				editorBrowsableNever = CustomAttributeBuilder.__FromBlob(ctor, new byte[] { 01, 00, 01, 00, 00, 00, 00, 00 });
			}
			return editorBrowsableNever;
		}

		internal static void SetEditorBrowsableNever(TypeBuilder tb)
		{
			tb.SetCustomAttribute(GetEditorBrowsableNever());
		}

		internal static void SetEditorBrowsableNever(MethodBuilder mb)
		{
			mb.SetCustomAttribute(GetEditorBrowsableNever());
		}

		internal static void SetEditorBrowsableNever(PropertyBuilder pb)
		{
			pb.SetCustomAttribute(GetEditorBrowsableNever());
		}

		internal static void SetDeprecatedAttribute(MethodBuilder mb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			mb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetDeprecatedAttribute(TypeBuilder tb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			tb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetDeprecatedAttribute(FieldBuilder fb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			fb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetDeprecatedAttribute(PropertyBuilder pb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ObsoleteAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			pb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetThrowsAttribute(MethodBuilder mb, string[] exceptions)
		{
			if(exceptions != null && exceptions.Length != 0)
			{
				if(throwsAttribute == null)
				{
					throwsAttribute = typeofThrowsAttribute.GetConstructor(new Type[] { JVM.Import(typeof(string[])) });
				}
				exceptions = UnicodeUtil.EscapeInvalidSurrogates(exceptions);
				mb.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute, new object[] { exceptions }));
			}
		}

		internal static void SetGhostInterface(TypeBuilder typeBuilder)
		{
			if(ghostInterfaceAttribute == null)
			{
				ghostInterfaceAttribute = new CustomAttributeBuilder(typeofGhostInterfaceAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
		}

		internal static void SetNonNestedInnerClass(TypeBuilder typeBuilder, string className)
		{
			if(nonNestedInnerClassAttribute == null)
			{
				nonNestedInnerClassAttribute = typeofNonNestedInnerClassAttribute.GetConstructor(new Type[] { Types.String });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedInnerClassAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
		}

		internal static void SetNonNestedOuterClass(TypeBuilder typeBuilder, string className)
		{
			if(nonNestedOuterClassAttribute == null)
			{
				nonNestedOuterClassAttribute = typeofNonNestedOuterClassAttribute.GetConstructor(new Type[] { Types.String });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedOuterClassAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(className) }));
		}
#endif // STATIC_COMPILER

		internal static void HideFromReflection(MethodBuilder mb)
		{
			mb.SetCustomAttribute(hideFromReflection);
		}

		internal static void HideFromReflection(FieldBuilder fb)
		{
			fb.SetCustomAttribute(hideFromReflection);
		}

		internal static void HideFromReflection(PropertyBuilder pb)
		{
			pb.SetCustomAttribute(hideFromReflection);
		}

		internal static void HideFromJava(TypeBuilder typeBuilder)
		{
			typeBuilder.SetCustomAttribute(hideFromJavaAttribute);
		}

		internal static void HideFromJava(MethodBuilder mb)
		{
			mb.SetCustomAttribute(hideFromJavaAttribute);
		}

		internal static void HideFromJava(MethodBuilder mb, HideFromJavaFlags flags)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(new Type[] { typeofHideFromJavaFlags }), new object[] { flags });
			mb.SetCustomAttribute(cab);
		}

		internal static void HideFromJava(FieldBuilder fb)
		{
			fb.SetCustomAttribute(hideFromJavaAttribute);
		}

#if STATIC_COMPILER
		internal static void HideFromJava(PropertyBuilder pb)
		{
			pb.SetCustomAttribute(hideFromJavaAttribute);
		}
#endif // STATIC_COMPILER

		internal static bool IsHideFromJava(Type type)
		{
			return type.IsDefined(typeofHideFromJavaAttribute, false)
				|| (type.IsNested && (type.DeclaringType.IsDefined(typeofHideFromJavaAttribute, false) || type.Name.StartsWith("__<", StringComparison.Ordinal)));
		}

		internal static bool IsHideFromJava(MemberInfo mi)
		{
			return (GetHideFromJavaFlags(mi) & HideFromJavaFlags.Code) != 0;
		}

		internal static HideFromJavaFlags GetHideFromJavaFlags(MemberInfo mi)
		{
			// NOTE all privatescope fields and methods are "hideFromJava"
			// because Java cannot deal with the potential name clashes
			FieldInfo fi = mi as FieldInfo;
			if(fi != null && (fi.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
			{
				return HideFromJavaFlags.All;
			}
			MethodBase mb = mi as MethodBase;
			if(mb != null && (mb.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope)
			{
				return HideFromJavaFlags.All;
			}
			if (mi.Name.StartsWith("__<", StringComparison.Ordinal))
			{
				return HideFromJavaFlags.All;
			}
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = mi.GetCustomAttributes(typeofHideFromJavaAttribute, false);
			if (attr.Length == 1)
			{
				return ((HideFromJavaAttribute)attr[0]).Flags;
			}
#else
			IList<CustomAttributeData> attr = CustomAttributeData.__GetCustomAttributes(mi, typeofHideFromJavaAttribute, false);
			if(attr.Count == 1)
			{
				IList<CustomAttributeTypedArgument> args = attr[0].ConstructorArguments;
				if(args.Count == 1)
				{
					return (HideFromJavaFlags)args[0].Value;
				}
				return HideFromJavaFlags.All;
			}
#endif
			return HideFromJavaFlags.None;
		}

#if STATIC_COMPILER
		internal static void SetImplementsAttribute(TypeBuilder typeBuilder, TypeWrapper[] ifaceWrappers)
		{
			string[] interfaces = new string[ifaceWrappers.Length];
			for(int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i] = UnicodeUtil.EscapeInvalidSurrogates(ifaceWrappers[i].Name);
			}
			if(implementsAttribute == null)
			{
				implementsAttribute = typeofImplementsAttribute.GetConstructor(new Type[] { JVM.Import(typeof(string[])) });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { interfaces }));
		}
#endif

		internal static bool IsGhostInterface(Type type)
		{
			return type.IsDefined(typeofGhostInterfaceAttribute, false);
		}

		internal static bool IsRemappedType(Type type)
		{
			return type.IsDefined(typeofRemappedTypeAttribute, false);
		}

		internal static bool IsExceptionIsUnsafeForMapping(Type type)
		{
			return type.IsDefined(typeofExceptionIsUnsafeForMappingAttribute, false);
		}

		internal static ModifiersAttribute GetModifiersAttribute(MemberInfo member)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = member.GetCustomAttributes(typeof(ModifiersAttribute), false);
			return attr.Length == 1 ? (ModifiersAttribute)attr[0] : null;
#else
			IList<CustomAttributeData> attr = CustomAttributeData.__GetCustomAttributes(member, typeofModifiersAttribute, false);
			if(attr.Count == 1)
			{
				IList<CustomAttributeTypedArgument> args = attr[0].ConstructorArguments;
				if(args.Count == 2)
				{
					return new ModifiersAttribute((Modifiers)args[0].Value, (bool)args[1].Value);
				}
				return new ModifiersAttribute((Modifiers)args[0].Value);
			}
			return null;
#endif
		}

		internal static ExModifiers GetModifiers(MethodBase mb, bool assemblyIsPrivate)
		{
			ModifiersAttribute attr = GetModifiersAttribute(mb);
			if(attr != null)
			{
				return new ExModifiers(attr.Modifiers, attr.IsInternal);
			}
			Modifiers modifiers = 0;
			if(mb.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
			else if(mb.IsPrivate)
			{
				modifiers |= Modifiers.Private;
			}
			else if(mb.IsFamily || mb.IsFamilyOrAssembly)
			{
				modifiers |= Modifiers.Protected;
			}
			else if(assemblyIsPrivate)
			{
				modifiers |= Modifiers.Private;
			}
			// NOTE Java doesn't support non-virtual methods, but we set the Final modifier for
			// non-virtual methods to approximate the semantics
			if((mb.IsFinal || (!mb.IsVirtual && ((modifiers & Modifiers.Private) == 0))) && !mb.IsStatic && !mb.IsConstructor)
			{
				modifiers |= Modifiers.Final;
			}
			if(mb.IsAbstract)
			{
				modifiers |= Modifiers.Abstract;
			}
			else
			{
				// Some .NET interfaces (like System._AppDomain) have synchronized methods,
				// Java doesn't allow synchronized on an abstract methods, so we ignore it for
				// abstract methods.
				if((mb.GetMethodImplementationFlags() & MethodImplAttributes.Synchronized) != 0)
				{
					modifiers |= Modifiers.Synchronized;
				}
			}
			if(mb.IsStatic)
			{
				modifiers |= Modifiers.Static;
			}
			if((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
			{
				modifiers |= Modifiers.Native;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			if(parameters.Length > 0 && parameters[parameters.Length - 1].IsDefined(JVM.Import(typeof(ParamArrayAttribute)), false))
			{
				modifiers |= Modifiers.VarArgs;
			}
			return new ExModifiers(modifiers, false);
		}

		internal static ExModifiers GetModifiers(FieldInfo fi, bool assemblyIsPrivate)
		{
			ModifiersAttribute attr = GetModifiersAttribute(fi);
			if(attr != null)
			{
				return new ExModifiers(attr.Modifiers, attr.IsInternal);
			}
			Modifiers modifiers = 0;
			if(fi.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
			else if(fi.IsPrivate)
			{
				modifiers |= Modifiers.Private;
			}
			else if(fi.IsFamily || fi.IsFamilyOrAssembly)
			{
				modifiers |= Modifiers.Protected;
			}
			else if(assemblyIsPrivate)
			{
				modifiers |= Modifiers.Private;
			}
			if(fi.IsInitOnly || fi.IsLiteral)
			{
				modifiers |= Modifiers.Final;
			}
			if(fi.IsNotSerialized)
			{
				modifiers |= Modifiers.Transient;
			}
			if(fi.IsStatic)
			{
				modifiers |= Modifiers.Static;
			}
			if(Array.IndexOf(fi.GetRequiredCustomModifiers(), Types.IsVolatile) != -1)
			{
				modifiers |= Modifiers.Volatile;
			}
			return new ExModifiers(modifiers, false);
		}

#if STATIC_COMPILER
		internal static void SetModifiers(MethodBuilder mb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			mb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(FieldBuilder fb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			fb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(PropertyBuilder pb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			pb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(TypeBuilder tb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, Types.Boolean }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			tb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetNameSig(MethodBuilder mb, string name, string sig)
		{
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeofNameSigAttribute.GetConstructor(new Type[] { Types.String, Types.String }),
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(name), UnicodeUtil.EscapeInvalidSurrogates(sig) });
			mb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetInnerClass(TypeBuilder typeBuilder, string innerClass, Modifiers modifiers)
		{
			Type[] argTypes = new Type[] { Types.String, typeofModifiers };
			object[] args = new object[] { UnicodeUtil.EscapeInvalidSurrogates(innerClass), modifiers };
			ConstructorInfo ci = typeofInnerClassAttribute.GetConstructor(argTypes);
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ci, args);
			typeBuilder.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetSourceFile(TypeBuilder typeBuilder, string filename)
		{
			if(sourceFileAttribute == null)
			{
				sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { Types.String });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal static void SetSourceFile(ModuleBuilder moduleBuilder, string filename)
		{
			if(sourceFileAttribute == null)
			{
				sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { Types.String });
			}
			moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal static void SetLineNumberTable(MethodBuilder mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
		{
			object arg;
			ConstructorInfo con;
			if(writer.Count == 1)
			{
				if(lineNumberTableAttribute2 == null)
				{
					lineNumberTableAttribute2 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { Types.UInt16 });
				}
				con = lineNumberTableAttribute2;
				arg = (ushort)writer.LineNo;
			}
			else
			{
				if(lineNumberTableAttribute1 == null)
				{
					lineNumberTableAttribute1 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { JVM.Import(typeof(byte[])) });
				}
				con = lineNumberTableAttribute1;
				arg = writer.ToArray();
			}
			mb.SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { arg }));
		}

		internal static void SetEnclosingMethodAttribute(TypeBuilder tb, string className, string methodName, string methodSig)
		{
			if(enclosingMethodAttribute == null)
			{
				enclosingMethodAttribute = typeofEnclosingMethodAttribute.GetConstructor(new Type[] { Types.String, Types.String, Types.String });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(enclosingMethodAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(className), UnicodeUtil.EscapeInvalidSurrogates(methodName), UnicodeUtil.EscapeInvalidSurrogates(methodSig) }));
		}

		internal static void SetSignatureAttribute(TypeBuilder tb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal static void SetSignatureAttribute(FieldBuilder fb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
			}
			fb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal static void SetSignatureAttribute(MethodBuilder mb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { Types.String });
			}
			mb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute,
				new object[] { UnicodeUtil.EscapeInvalidSurrogates(signature) }));
		}

		internal static void SetMethodParametersAttribute(MethodBuilder mb, Modifiers[] modifiers)
		{
			if(methodParametersAttribute == null)
			{
				methodParametersAttribute = typeofMethodParametersAttribute.GetConstructor(new Type[] { typeofModifiers.MakeArrayType() });
			}
			mb.SetCustomAttribute(new CustomAttributeBuilder(methodParametersAttribute, new object[] { modifiers }));
		}

		internal static void SetRuntimeVisibleTypeAnnotationsAttribute(TypeBuilder tb, byte[] data)
		{
			if(runtimeVisibleTypeAnnotationsAttribute == null)
			{
				runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
		}

		internal static void SetRuntimeVisibleTypeAnnotationsAttribute(FieldBuilder fb, byte[] data)
		{
			if(runtimeVisibleTypeAnnotationsAttribute == null)
			{
				runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
			}
			fb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
		}

		internal static void SetRuntimeVisibleTypeAnnotationsAttribute(MethodBuilder mb, byte[] data)
		{
			if(runtimeVisibleTypeAnnotationsAttribute == null)
			{
				runtimeVisibleTypeAnnotationsAttribute = typeofRuntimeVisibleTypeAnnotationsAttribute.GetConstructor(new Type[] { Types.Byte.MakeArrayType() });
			}
			mb.SetCustomAttribute(new CustomAttributeBuilder(runtimeVisibleTypeAnnotationsAttribute, new object[] { data }));
		}

		internal static void SetConstantPoolAttribute(TypeBuilder tb, object[] constantPool)
		{
			if(constantPoolAttribute == null)
			{
				constantPoolAttribute = typeofConstantPoolAttribute.GetConstructor(new Type[] { Types.Object.MakeArrayType() });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(constantPoolAttribute, new object[] { constantPool }));
		}

		internal static void SetParamArrayAttribute(ParameterBuilder pb)
		{
			if(paramArrayAttribute == null)
			{
				paramArrayAttribute = new CustomAttributeBuilder(JVM.Import(typeof(ParamArrayAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			pb.SetCustomAttribute(paramArrayAttribute);
		}
#endif  // STATIC_COMPILER

		internal static NameSigAttribute GetNameSig(MemberInfo member)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = member.GetCustomAttributes(typeof(NameSigAttribute), false);
			return attr.Length == 1 ? (NameSigAttribute)attr[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofNameSigAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
			}
			return null;
#endif
		}

		internal static T[] DecodeArray<T>(CustomAttributeTypedArgument arg)
		{
			IList<CustomAttributeTypedArgument> elems = (IList<CustomAttributeTypedArgument>)arg.Value;
			T[] arr = new T[elems.Count];
			for(int i = 0; i < arr.Length; i++)
			{
				arr[i] = (T)elems[i].Value;
			}
			return arr;
		}

		internal static ImplementsAttribute GetImplements(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = type.GetCustomAttributes(typeof(ImplementsAttribute), false);
			return attribs.Length == 1 ? (ImplementsAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofImplementsAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new ImplementsAttribute(DecodeArray<string>(args[0]));
			}
			return null;
#endif
		}

		internal static ThrowsAttribute GetThrows(MethodBase mb)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = mb.GetCustomAttributes(typeof(ThrowsAttribute), false);
			return attribs.Length == 1 ? (ThrowsAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(mb, typeofThrowsAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				if (args[0].ArgumentType == Types.String.MakeArrayType())
				{
					return new ThrowsAttribute(DecodeArray<string>(args[0]));
				}
				else if (args[0].ArgumentType == Types.Type.MakeArrayType())
				{
					return new ThrowsAttribute(DecodeArray<Type>(args[0]));
				}
				else
				{
					return new ThrowsAttribute((Type)args[0].Value);
				}
			}
			return null;
#endif
		}

		internal static string[] GetNonNestedInnerClasses(Type t)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = t.GetCustomAttributes(typeof(NonNestedInnerClassAttribute), false);
			string[] classes = new string[attribs.Length];
			for (int i = 0; i < attribs.Length; i++)
			{
				classes[i] = ((NonNestedInnerClassAttribute)attribs[i]).InnerClassName;
			}
			return classes;
#else
			List<string> list = new List<string>();
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(t, typeofNonNestedInnerClassAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				list.Add(UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value));
			}
			return list.ToArray();
#endif
		}

		internal static string GetNonNestedOuterClasses(Type t)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = t.GetCustomAttributes(typeof(NonNestedOuterClassAttribute), false);
			return attribs.Length == 1 ? ((NonNestedOuterClassAttribute)attribs[0]).OuterClassName : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(t, typeofNonNestedOuterClassAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return UnicodeUtil.UnescapeInvalidSurrogates((string)args[0].Value);
			}
			return null;
#endif
		}

		internal static SignatureAttribute GetSignature(MemberInfo member)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = member.GetCustomAttributes(typeof(SignatureAttribute), false);
			return attribs.Length == 1 ? (SignatureAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofSignatureAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new SignatureAttribute((string)args[0].Value);
			}
			return null;
#endif
		}

		internal static MethodParametersAttribute GetMethodParameters(MethodBase method)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = method.GetCustomAttributes(typeof(MethodParametersAttribute), false);
			return attribs.Length == 1 ? (MethodParametersAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(method, typeofMethodParametersAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new MethodParametersAttribute(DecodeArray<Modifiers>(args[0]));
			}
			return null;
#endif
		}

		internal static object[] GetConstantPool(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = type.GetCustomAttributes(typeof(ConstantPoolAttribute), false);
			return attribs.Length == 1 ? ((ConstantPoolAttribute)attribs[0]).constantPool : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofConstantPoolAttribute, false))
			{
				return ConstantPoolAttribute.Decompress(DecodeArray<object>(cad.ConstructorArguments[0]));
			}
			return null;
#endif
		}

		internal static byte[] GetRuntimeVisibleTypeAnnotations(MemberInfo member)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = member.GetCustomAttributes(typeof(RuntimeVisibleTypeAnnotationsAttribute), false);
			return attribs.Length == 1 ? ((RuntimeVisibleTypeAnnotationsAttribute)attribs[0]).data : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(member, typeofRuntimeVisibleTypeAnnotationsAttribute, false))
			{
				return DecodeArray<byte>(cad.ConstructorArguments[0]);
			}
			return null;
#endif
		}

		internal static InnerClassAttribute GetInnerClass(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
			return attribs.Length == 1 ? (InnerClassAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofInnerClassAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
			}
			return null;
#endif
		}

		internal static RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = type.GetCustomAttributes(typeof(RemappedInterfaceMethodAttribute), false);
			RemappedInterfaceMethodAttribute[] attr1 = new RemappedInterfaceMethodAttribute[attr.Length];
			Array.Copy(attr, attr1, attr.Length);
			return attr1;
#else
			List<RemappedInterfaceMethodAttribute> attrs = new List<RemappedInterfaceMethodAttribute>();
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofRemappedInterfaceMethodAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value, DecodeArray<string>(args[2])));
			}
			return attrs.ToArray();
#endif
		}

		internal static RemappedTypeAttribute GetRemappedType(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
			return attribs.Length == 1 ? (RemappedTypeAttribute)attribs[0] : null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofRemappedTypeAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				return new RemappedTypeAttribute((Type)args[0].Value);
			}
			return null;
#endif
		}

		internal static RemappedClassAttribute[] GetRemappedClasses(Assembly coreAssembly)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = coreAssembly.GetCustomAttributes(typeof(RemappedClassAttribute), false);
			RemappedClassAttribute[] attr1 = new RemappedClassAttribute[attr.Length];
			Array.Copy(attr, attr1, attr.Length);
			return attr1;
#else
			List<RemappedClassAttribute> attrs = new List<RemappedClassAttribute>();
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(coreAssembly, typeofRemappedClassAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				attrs.Add(new RemappedClassAttribute((string)args[0].Value, (Type)args[1].Value));
			}
			return attrs.ToArray();
#endif
		}

		internal static string GetAnnotationAttributeType(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = type.GetCustomAttributes(typeof(AnnotationAttributeAttribute), false);
			if(attr.Length == 1)
			{
				return ((AnnotationAttributeAttribute)attr[0]).AttributeType;
			}
			return null;
#else
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofAnnotationAttributeAttribute, false))
			{
				return UnicodeUtil.UnescapeInvalidSurrogates((string)cad.ConstructorArguments[0].Value);
			}
			return null;
#endif
		}

		internal static AssemblyName[] GetInternalsVisibleToAttributes(Assembly assembly)
		{
			List<AssemblyName> list = new List<AssemblyName>();
			foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(assembly))
			{
				if(cad.Constructor.DeclaringType == JVM.Import(typeof(System.Runtime.CompilerServices.InternalsVisibleToAttribute)))
				{
					try
					{
						list.Add(new AssemblyName((string)cad.ConstructorArguments[0].Value));
					}
					catch
					{
						// HACK since there is no list of exception that the AssemblyName constructor can throw, we simply catch all
					}
				}
			}
			return list.ToArray();
		}

		internal static bool IsJavaModule(Module mod)
		{
			return mod.IsDefined(typeofJavaModuleAttribute, false);
		}

		internal static object[] GetJavaModuleAttributes(Module mod)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			return mod.GetCustomAttributes(typeofJavaModuleAttribute, false);
#else
			List<JavaModuleAttribute> attrs = new List<JavaModuleAttribute>();
			foreach(CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(mod, typeofJavaModuleAttribute, false))
			{
				IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
				if(args.Count == 0)
				{
					attrs.Add(new JavaModuleAttribute());
				}
				else
				{
					attrs.Add(new JavaModuleAttribute(DecodeArray<string>(args[0])));
				}
			}
			return attrs.ToArray();
#endif
		}

		internal static bool IsNoPackagePrefix(Type type)
		{
			return type.IsDefined(typeofNoPackagePrefixAttribute, false) || type.Assembly.IsDefined(typeofNoPackagePrefixAttribute, false);
		}

		internal static bool HasEnclosingMethodAttribute(Type type)
		{
			return type.IsDefined(typeofEnclosingMethodAttribute, false);
		}

		internal static EnclosingMethodAttribute GetEnclosingMethodAttribute(Type type)
		{
#if !STATIC_COMPILER && !STUB_GENERATOR
			object[] attr = type.GetCustomAttributes(typeof(EnclosingMethodAttribute), false);
			if (attr.Length == 1)
			{
				return ((EnclosingMethodAttribute)attr[0]).SetClassName(type);
			}
			return null;
#else
			foreach (CustomAttributeData cad in CustomAttributeData.__GetCustomAttributes(type, typeofEnclosingMethodAttribute, false))
			{
				return new EnclosingMethodAttribute((string)cad.ConstructorArguments[0].Value, (string)cad.ConstructorArguments[1].Value, (string)cad.ConstructorArguments[2].Value).SetClassName(type);
			}
			return null;
#endif
		}

#if STATIC_COMPILER
		internal static void SetRemappedClass(AssemblyBuilder assemblyBuilder, string name, Type shadowType)
		{
			ConstructorInfo remappedClassAttribute = typeofRemappedClassAttribute.GetConstructor(new Type[] { Types.String, Types.Type });
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, shadowType }));
		}

		internal static void SetRemappedType(TypeBuilder typeBuilder, Type shadowType)
		{
			ConstructorInfo remappedTypeAttribute = typeofRemappedTypeAttribute.GetConstructor(new Type[] { Types.Type });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { shadowType }));
		}

		internal static void SetRemappedInterfaceMethod(TypeBuilder typeBuilder, string name, string mappedTo, string[] throws)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofRemappedInterfaceMethodAttribute.GetConstructor(new Type[] { Types.String, Types.String, Types.String.MakeArrayType() }), new object[] { name, mappedTo, throws });
			typeBuilder.SetCustomAttribute(cab);
		}

		internal static void SetExceptionIsUnsafeForMapping(TypeBuilder typeBuilder)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofExceptionIsUnsafeForMappingAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			typeBuilder.SetCustomAttribute(cab);
		}
#endif // STATIC_COMPILER

		internal static void SetRuntimeCompatibilityAttribute(AssemblyBuilder assemblyBuilder)
		{
			Type runtimeCompatibilityAttribute = JVM.Import(typeof(System.Runtime.CompilerServices.RuntimeCompatibilityAttribute));
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(
				runtimeCompatibilityAttribute.GetConstructor(Type.EmptyTypes), new object[0],
				new PropertyInfo[] { runtimeCompatibilityAttribute.GetProperty("WrapNonExceptionThrows") }, new object[] { true },
				new FieldInfo[0], new object[0]));
		}

		internal static void SetInternalsVisibleToAttribute(AssemblyBuilder assemblyBuilder, string assemblyName)
		{
			Type internalsVisibleToAttribute = JVM.Import(typeof(System.Runtime.CompilerServices.InternalsVisibleToAttribute));
			CustomAttributeBuilder cab = new CustomAttributeBuilder(
				internalsVisibleToAttribute.GetConstructor(new Type[] { Types.String }), new object[] { assemblyName });
			assemblyBuilder.SetCustomAttribute(cab);
		}
	}

	static class EnumHelper
	{
		internal static Type GetUnderlyingType(Type enumType)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			return enumType.GetEnumUnderlyingType();
#else
			return Enum.GetUnderlyingType(enumType);
#endif
		}

#if STATIC_COMPILER
		internal static object Parse(Type type, string value)
		{
			object retval = null;
			foreach (string str in value.Split(','))
			{
				FieldInfo field = type.GetField(str.Trim(), BindingFlags.Public | BindingFlags.Static);
				if (field == null)
				{
					throw new InvalidOperationException("Enum value '" + str + "' not found in " + type.FullName);
				}
				if (retval == null)
				{
					retval = field.GetRawConstantValue();
				}
				else
				{
					retval = OrBoxedIntegrals(retval, field.GetRawConstantValue());
				}
			}
			return retval;
		}
#endif

		// note that we only support the integer types that C# supports
		// (the CLI also supports bool, char, IntPtr & UIntPtr)
		internal static object OrBoxedIntegrals(object v1, object v2)
		{
			Debug.Assert(v1.GetType() == v2.GetType());
			if (v1 is ulong)
			{
				ulong l1 = (ulong)v1;
				ulong l2 = (ulong)v2;
				return l1 | l2;
			}
			else
			{
				long v = ((IConvertible)v1).ToInt64(null) | ((IConvertible)v2).ToInt64(null);
				switch (Type.GetTypeCode(JVM.Import(v1.GetType())))
				{
					case TypeCode.SByte:
						return (sbyte)v;
					case TypeCode.Byte:
						return (byte)v;
					case TypeCode.Int16:
						return (short)v;
					case TypeCode.UInt16:
						return (ushort)v;
					case TypeCode.Int32:
						return (int)v;
					case TypeCode.UInt32:
						return (uint)v;
					case TypeCode.Int64:
						return (long)v;
					default:
						throw new InvalidOperationException();
				}
			}
		}

		// this method can be used to convert an enum value or its underlying value to a Java primitive
		internal static object GetPrimitiveValue(Type underlyingType, object obj)
		{
			// Note that this method doesn't trust that obj is of the correct type,
			// because it turns out there exist assemblies (e.g. gtk-sharp.dll) that
			// have incorrectly typed enum constant values (e.g. int32 instead of uint32).
			long value;
			if (obj is ulong || (obj is Enum && underlyingType == Types.UInt64))
			{
				value = unchecked((long)((IConvertible)obj).ToUInt64(null));
			}
			else
			{
				value = ((IConvertible)obj).ToInt64(null);
			}
			if (underlyingType == Types.SByte || underlyingType == Types.Byte)
			{
				return unchecked((byte)value);
			}
			else if (underlyingType == Types.Int16 || underlyingType == Types.UInt16)
			{
				return unchecked((short)value);
			}
			else if (underlyingType == Types.Int32 || underlyingType == Types.UInt32)
			{
				return unchecked((int)value);
			}
			else if (underlyingType == Types.Int64 || underlyingType == Types.UInt64)
			{
				return value;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
	}

	static class TypeNameUtil
	{
		// note that MangleNestedTypeName() assumes that there are less than 16 special characters
		private const string specialCharactersString = "\\+,[]*&\u0000";
		internal const string ProxiesContainer = "__<Proxies>";

		internal static string ReplaceIllegalCharacters(string name)
		{
			name = UnicodeUtil.EscapeInvalidSurrogates(name);
			// only the NUL character is illegal in CLR type names, so we replace it with a space
			return name.Replace('\u0000', ' ');
		}

		internal static string Unescape(string name)
		{
			int pos = name.IndexOf('\\');
			if (pos == -1)
			{
				return name;
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder(name.Length);
			sb.Append(name, 0, pos);
			for (int i = pos; i < name.Length; i++)
			{
				char c = name[i];
				if (c == '\\')
				{
					c = name[++i];
				}
				sb.Append(c);
			}
			return sb.ToString();
		}

		internal static string MangleNestedTypeName(string name)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (char c in name)
			{
				int index = specialCharactersString.IndexOf(c);
				if (c == '.')
				{
					sb.Append("_");
				}
				else if (c == '_')
				{
					sb.Append("^-");
				}
				else if (index == -1)
				{
					sb.Append(c);
					if (c == '^')
					{
						sb.Append(c);
					}
				}
				else
				{
					sb.Append('^').AppendFormat("{0:X1}", index);
				}
			}
			return sb.ToString();
		}

		internal static string UnmangleNestedTypeName(string name)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			for (int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				int index = specialCharactersString.IndexOf(c);
				if (c == '_')
				{
					sb.Append('.');
				}
				else if (c == '^')
				{
					c = name[++i];
					if (c == '-')
					{
						sb.Append('_');
					}
					else if (c == '^')
					{
						sb.Append('^');
					}
					else
					{
						sb.Append(specialCharactersString[c - '0']);
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		internal static string GetProxyNestedName(TypeWrapper[] interfaces)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			foreach (TypeWrapper tw in interfaces)
			{
				sb.Append(tw.Name.Length).Append('|').Append(tw.Name);
			}
			return TypeNameUtil.MangleNestedTypeName(sb.ToString());
		}

		internal static string GetProxyName(TypeWrapper[] interfaces)
		{
			return ProxiesContainer + "+" + GetProxyNestedName(interfaces);
		}
	}

	static class ArrayUtil
	{
		internal static T[] Concat<T, X>(X obj, T[] arr)
			where X : T
		{
			T[] narr = new T[arr.Length + 1];
			narr[0] = obj;
			Array.Copy(arr, 0, narr, 1, arr.Length);
			return narr;
		}

		internal static T[] Concat<T, X>(T[] arr, X obj)
			where X : T
		{
			Array.Resize(ref arr, arr.Length + 1);
			arr[arr.Length - 1] = obj;
			return arr;
		}

		internal static T[] DropFirst<T>(T[] arr)
		{
			T[] narr = new T[arr.Length - 1];
			Array.Copy(arr, 1, narr, 0, narr.Length);
			return narr;
		}
	}

	static class UnicodeUtil
	{
		// We use part of the Supplementary Private Use Area-B to encode
		// invalid surrogates. If we encounter either of these two
		// markers, we always encode the surrogate (single or pair)
		private const char HighSurrogatePrefix = '\uDBFF';
		private const char LowSurrogatePrefix = '\uDBFE';

		// Identifiers in ECMA CLI metadata and strings in custom attribute blobs are encoded
		// using UTF-8 and don't allow partial surrogates, so we have to "complete" them to
		// produce valid Unicode and reverse the process when we read back the names.
		internal static string EscapeInvalidSurrogates(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					char c = str[i];
					if (Char.IsLowSurrogate(c))
					{
						str = str.Substring(0, i) + LowSurrogatePrefix + c + str.Substring(i + 1);
						i++;
					}
					else if (Char.IsHighSurrogate(c))
					{
						i++;
						// always escape the markers
						if (c == HighSurrogatePrefix || c == LowSurrogatePrefix || i == str.Length || !Char.IsLowSurrogate(str[i]))
						{
							str = str.Substring(0, i - 1) + HighSurrogatePrefix + (char)(c + 0x400) + str.Substring(i);
						}
					}
				}
			}
			return str;
		}

		internal static string UnescapeInvalidSurrogates(string str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					switch (str[i])
					{
						case HighSurrogatePrefix:
							str = str.Substring(0, i) + (char)(str[i + 1] - 0x400) + str.Substring(i + 2);
							break;
						case LowSurrogatePrefix:
							str = str.Substring(0, i) + str[i + 1] + str.Substring(i + 2);
							break;
					}
				}
			}
			return str;
		}

		internal static string[] EscapeInvalidSurrogates(string[] str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					str[i] = EscapeInvalidSurrogates(str[i]);
				}
			}
			return str;
		}

		internal static string[] UnescapeInvalidSurrogates(string[] str)
		{
			if (str != null)
			{
				for (int i = 0; i < str.Length; i++)
				{
					str[i] = UnescapeInvalidSurrogates(str[i]);
				}
			}
			return str;
		}
	}

	abstract class Annotation
	{
#if STATIC_COMPILER
		internal static Annotation LoadAssemblyCustomAttribute(ClassLoaderWrapper loader, object[] def)
		{
			Debug.Assert(def[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION));
			string annotationClass = (string)def[1];
			if (ClassFile.IsValidFieldSig(annotationClass))
			{
				try
				{
					return loader.RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.LoadOrThrow).Annotation;
				}
				catch (RetargetableJavaException)
				{
				}
			}
			return null;
		}
#endif

#if !STUB_GENERATOR
		// NOTE this method returns null if the type could not be found
		// or if the type is not a Custom Attribute and we're not in the static compiler
		internal static Annotation Load(TypeWrapper owner, object[] def)
		{
			Debug.Assert(def[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION));
			string annotationClass = (string)def[1];
#if !STATIC_COMPILER
			if(!annotationClass.EndsWith("$Annotation;")
				&& !annotationClass.EndsWith("$Annotation$__ReturnValue;")
				&& !annotationClass.EndsWith("$Annotation$__Multiple;"))
			{
				// we don't want to try to load an annotation in dynamic mode,
				// unless it is a .NET custom attribute (which can affect runtime behavior)
				return null;
			}
#endif
			if (ClassFile.IsValidFieldSig(annotationClass))
			{
				TypeWrapper tw = owner.GetClassLoader().RetTypeWrapperFromSig(annotationClass.Replace('/', '.'), LoadMode.Link);
				// Java allows inaccessible annotations to be used, so when the annotation isn't visible
				// we fall back to using the DynamicAnnotationAttribute.
				if (!tw.IsUnloadable && tw.IsAccessibleFrom(owner))
				{
					return tw.Annotation;
				}
			}
			Tracer.Warning(Tracer.Compiler, "Unable to load annotation class {0}", annotationClass);
#if STATIC_COMPILER
			return new CompiledTypeWrapper.CompiledAnnotation(StaticCompiler.GetRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute"));
#else
			return null;
#endif
		}
#endif

		private static object LookupEnumValue(Type enumType, string value)
		{
			FieldInfo field = enumType.GetField(value, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if(field != null)
			{
				return field.GetRawConstantValue();
			}
			// both __unspecified and missing values end up here
			return EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(enumType), 0);
		}

		protected static object ConvertValue(ClassLoaderWrapper loader, Type targetType, object obj)
		{
			if(targetType.IsEnum)
			{
				// TODO check the obj descriptor matches the type we expect
				if(((object[])obj)[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
				{
					object[] arr = (object[])obj;
					object value = null;
					for(int i = 1; i < arr.Length; i++)
					{
						// TODO check the obj descriptor matches the type we expect
						string s = ((object[])arr[i])[2].ToString();
						object newval = LookupEnumValue(targetType, s);
						if (value == null)
						{
							value = newval;
						}
						else
						{
							value = EnumHelper.OrBoxedIntegrals(value, newval);
						}
					}
					return value;
				}
				else
				{
					string s = ((object[])obj)[2].ToString();
					if(s == "__unspecified")
					{
						// TODO we should probably return null and handle that
					}
					return LookupEnumValue(targetType, s);
				}
			}
			else if(targetType == Types.Type)
			{
				// TODO check the obj descriptor matches the type we expect
				return loader.FieldTypeWrapperFromSig(((string)((object[])obj)[1]).Replace('/', '.'), LoadMode.LoadOrThrow).TypeAsTBD;
			}
			else if(targetType.IsArray)
			{
				// TODO check the obj descriptor matches the type we expect
				object[] arr = (object[])obj;
				Type elementType = targetType.GetElementType();
				object[] targetArray = new object[arr.Length - 1];
				for(int i = 1; i < arr.Length; i++)
				{
					targetArray[i - 1] = ConvertValue(loader, elementType, arr[i]);
				}
				return targetArray;
			}
			else
			{
				return obj;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal static bool MakeDeclSecurity(Type type, object annotation, out SecurityAction action, out PermissionSet permSet)
		{
			ConstructorInfo ci = type.GetConstructor(new Type[] { typeof(SecurityAction) });
			if (ci == null)
			{
				// TODO should we support HostProtectionAttribute? (which has a no-arg constructor)
				// TODO issue message?
				action = 0;
				permSet = null;
				return false;
			}
			SecurityAttribute attr = null;
			object[] arr = (object[])annotation;
			for (int i = 2; i < arr.Length; i += 2)
			{
				string name = (string)arr[i];
				if (name == "value")
				{
					attr = (SecurityAttribute)ci.Invoke(new object[] { ConvertValue(null, typeof(SecurityAction), arr[i + 1]) });
				}
			}
			if (attr == null)
			{
				// TODO issue message?
				action = 0;
				permSet = null;
				return false;
			}
			for (int i = 2; i < arr.Length; i += 2)
			{
				string name = (string)arr[i];
				if (name != "value")
				{
					PropertyInfo pi = type.GetProperty(name);
					pi.SetValue(attr, ConvertValue(null, pi.PropertyType, arr[i + 1]), null);
				}
			}
			action = attr.Action;
			permSet = new PermissionSet(PermissionState.None);
			permSet.AddPermission(attr.CreatePermission());
			return true;
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		internal static bool HasRetentionPolicyRuntime(object[] annotations)
		{
			if(annotations != null)
			{
				foreach(object[] def in annotations)
				{
					if(def[1].Equals("Ljava/lang/annotation/Retention;"))
					{
						for(int i = 2; i < def.Length; i += 2)
						{
							if(def[i].Equals("value"))
							{
								object[] val = def[i + 1] as object[];
								if(val != null
									&& val.Length == 3
									&& val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
									&& val[1].Equals("Ljava/lang/annotation/RetentionPolicy;")
									&& val[2].Equals("RUNTIME"))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		internal static bool HasObsoleteAttribute(object[] annotations)
		{
			if (annotations != null)
			{
				foreach (object[] def in annotations)
				{
					if (def[1].Equals("Lcli/System/ObsoleteAttribute$Annotation;"))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected static object QualifyClassNames(ClassLoaderWrapper loader, object annotation)
		{
			bool copy = false;
			object[] def = (object[])annotation;
			for(int i = 3; i < def.Length; i += 2)
			{
				object[] val = def[i] as object[];
				if(val != null)
				{
					object[] newval = ValueQualifyClassNames(loader, val);
					if(newval != val)
					{
						if(!copy)
						{
							copy = true;
							object[] newdef = new object[def.Length];
							Array.Copy(def, newdef, def.Length);
							def = newdef;
						}
						def[i] = newval;
					}
				}
			}
			return def;
		}

		private static object[] ValueQualifyClassNames(ClassLoaderWrapper loader, object[] val)
		{
			if(val[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
			{
				return (object[])QualifyClassNames(loader, val);
			}
			else if(val[0].Equals(AnnotationDefaultAttribute.TAG_CLASS))
			{
				string sig = (string)val[1];
				if(sig.StartsWith("L"))
				{
					TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
					if(tw != null)
					{
						return new object[] { AnnotationDefaultAttribute.TAG_CLASS, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";" };
					}
				}
				return val;
			}
			else if(val[0].Equals(AnnotationDefaultAttribute.TAG_ENUM))
			{
				string sig = (string)val[1];
				TypeWrapper tw = loader.LoadClassByDottedNameFast(sig.Substring(1, sig.Length - 2).Replace('/', '.'));
				if(tw != null)
				{
					return new object[] { AnnotationDefaultAttribute.TAG_ENUM, "L" + tw.TypeAsBaseType.AssemblyQualifiedName.Replace('.', '/') + ";", val[2] };
				}
				return val;
			}
			else if(val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
			{
				bool copy = false;
				for(int i = 1; i < val.Length; i++)
				{
					object[] nval = val[i] as object[];
					if(nval != null)
					{
						object newnval = ValueQualifyClassNames(loader, nval);
						if(newnval != nval)
						{
							if(!copy)
							{
								copy = true;
								object[] newval = new object[val.Length];
								Array.Copy(val, newval, val.Length);
								val = newval;
							}
							val[i] = newnval;
						}
					}
				}
				return val;
			}
			else if(val[0].Equals(AnnotationDefaultAttribute.TAG_ERROR))
			{
				return val;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		internal abstract void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation);

		internal virtual void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
		{
		}

		internal abstract bool IsCustomAttribute { get; }
	}

	[Flags]
	enum TypeFlags : ushort
	{
		None = 0,
		HasIncompleteInterfaceImplementation = 1,
		InternalAccess = 2,
		HasStaticInitializer = 4,
		VerifyError = 8,
		ClassFormatError = 16,
		HasUnsupportedAbstractMethods = 32,
		Anonymous = 64,
		Linked = 128,
	}

	static class NamePrefix
	{
		internal const string Type2AccessStubBackingField = "__<>";
		internal const string AccessStub = "<accessstub>";
		internal const string NonVirtual = "<nonvirtual>";
		internal const string Bridge = "<bridge>";
		internal const string Incomplete = "<incomplete>";
		internal const string DefaultMethod = "<default>";
		internal const string PrivateInterfaceInstanceMethod = "<piim>";
	}

	static class NestedTypeName
	{
		internal const string CallerID = "__<CallerID>";
		internal const string InterfaceHelperMethods = "__<>IHM";
		internal const string PrivateInterfaceMethods = "__<>PIM";

		// interop types (mangled if necessary)
		internal const string Fields = "__Fields";
		internal const string Methods = "__Methods";
		internal const string DefaultMethods = "__DefaultMethods";

		// prefixes
		internal const string ThreadLocal = "__<tls>_";
		internal const string AtomicReferenceFieldUpdater = "__<ARFU>_";
		internal const string IndyCallSite = "__<>IndyCS";
		internal const string MethodHandleConstant = "__<>MHC";
		internal const string MethodTypeConstant = "__<>MTC";
		internal const string IntrinsifiedAnonymousClass = "__<>Anon";
	}

	internal abstract class TypeWrapper
	{
		private static readonly object flagsLock = new object();
		private readonly string name;		// java name (e.g. java.lang.Object)
		private readonly Modifiers modifiers;
		private TypeFlags flags;
		private MethodWrapper[] methods;
		private FieldWrapper[] fields;
#if !STATIC_COMPILER && !STUB_GENERATOR
		private java.lang.Class classObject;
#endif
		internal static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
		internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
		internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

		internal TypeWrapper(TypeFlags flags, Modifiers modifiers, string name)
		{
			Profiler.Count("TypeWrapper");
			// class name should be dotted or null for primitives
			Debug.Assert(name == null || name.IndexOf('/') < 0);

			this.flags = flags;
			this.modifiers = modifiers;
			this.name = name == null ? null : String.Intern(name);
		}

#if EMITTERS
		internal void EmitClassLiteral(CodeEmitter ilgen)
		{
			Debug.Assert(!this.IsPrimitive);

			Type type = GetClassLiteralType();

			// note that this has to be the same check as in LazyInitClass
			if (!this.IsFastClassLiteralSafe || IsForbiddenTypeParameterType(type))
			{
				int rank = 0;
				while (ReflectUtil.IsVector(type))
				{
					rank++;
					type = type.GetElementType();
				}
				if (rank == 0)
				{
					ilgen.Emit(OpCodes.Ldtoken, type);
					Compiler.getClassFromTypeHandle.EmitCall(ilgen);
				}
				else
				{
					ilgen.Emit(OpCodes.Ldtoken, type);
					ilgen.EmitLdc_I4(rank);
					Compiler.getClassFromTypeHandle2.EmitCall(ilgen);
				}
			}
			else
			{
				ilgen.Emit(OpCodes.Ldsfld, RuntimeHelperTypes.GetClassLiteralField(type));
			}
		}
#endif // EMITTERS

		private Type GetClassLiteralType()
		{
			Debug.Assert(!this.IsPrimitive);

			TypeWrapper tw = this;
			if (tw.IsGhostArray)
			{
				int rank = tw.ArrayRank;
				while (tw.IsArray)
				{
					tw = tw.ElementTypeWrapper;
				}
				return ArrayTypeWrapper.MakeArrayType(tw.TypeAsTBD, rank);
			}
			else
			{
				return tw.IsRemapped ? tw.TypeAsBaseType : tw.TypeAsTBD;
			}
		}

		private static bool IsForbiddenTypeParameterType(Type type)
		{
			// these are the types that may not be used as a type argument when instantiating a generic type
			return type == Types.Void
				|| type == JVM.Import(typeof(ArgIterator))
				|| type == JVM.Import(typeof(RuntimeArgumentHandle))
				|| type == JVM.Import(typeof(TypedReference))
				|| type.ContainsGenericParameters
				|| type.IsByRef;
		}

		internal virtual bool IsFastClassLiteralSafe
		{
			get { return false; }
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal void SetClassObject(java.lang.Class classObject)
		{
			this.classObject = classObject;
		}

		internal java.lang.Class ClassObject
		{
			get
			{
				Debug.Assert(!IsUnloadable && !IsVerifierType);
				if (classObject == null)
				{
					LazyInitClass();
				}
				return classObject;
			}
		}

#if !FIRST_PASS
		private java.lang.Class GetPrimitiveClass()
		{
			if (this == PrimitiveTypeWrapper.BYTE)
			{
				return java.lang.Byte.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.CHAR)
			{
				return java.lang.Character.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.DOUBLE)
			{
				return java.lang.Double.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.FLOAT)
			{
				return java.lang.Float.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.INT)
			{
				return java.lang.Integer.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.LONG)
			{
				return java.lang.Long.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.SHORT)
			{
				return java.lang.Short.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.BOOLEAN)
			{
				return java.lang.Boolean.TYPE;
			}
			else if (this == PrimitiveTypeWrapper.VOID)
			{
				return java.lang.Void.TYPE;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}
#endif

		private void LazyInitClass()
		{
			lock (this)
			{
				if (classObject == null)
				{
					// DynamicTypeWrapper should haved already had SetClassObject explicitly
					Debug.Assert(!IsDynamic);
#if !FIRST_PASS
					java.lang.Class clazz;
					// note that this has to be the same check as in EmitClassLiteral
					if (!this.IsFastClassLiteralSafe)
					{
						if (this.IsPrimitive)
						{
							clazz = GetPrimitiveClass();
						}
						else
						{
							clazz = new java.lang.Class(null);
						}
					}
					else
					{
						Type type = GetClassLiteralType();
						if (IsForbiddenTypeParameterType(type))
						{
							clazz = new java.lang.Class(type);
						}
						else
						{
							clazz = (java.lang.Class)typeof(ikvm.@internal.ClassLiteral<>).MakeGenericType(type).GetField("Value").GetValue(null);
						}
					}
#if __MonoCS__
					SetTypeWrapperHack(clazz, this);
#else
					clazz.typeWrapper = this;
#endif
					// MONOBUG Interlocked.Exchange is broken on Mono, so we use CompareExchange
					System.Threading.Interlocked.CompareExchange(ref classObject, clazz, null);
#endif
				}
			}
		}

#if __MonoCS__
		// MONOBUG this method is to work around an mcs bug
		internal static void SetTypeWrapperHack(object clazz, TypeWrapper type)
		{
#if !FIRST_PASS
			typeof(java.lang.Class).GetField("typeWrapper", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(clazz, type);
#endif
		}
#endif

#if !FIRST_PASS
		private static void ResolvePrimitiveTypeWrapperClasses()
		{
			// note that we're evaluating all ClassObject properties for the side effect
			// (to initialize and associate the ClassObject with the TypeWrapper)
			if (PrimitiveTypeWrapper.BYTE.ClassObject == null
				|| PrimitiveTypeWrapper.CHAR.ClassObject == null
				|| PrimitiveTypeWrapper.DOUBLE.ClassObject == null
				|| PrimitiveTypeWrapper.FLOAT.ClassObject == null
				|| PrimitiveTypeWrapper.INT.ClassObject == null
				|| PrimitiveTypeWrapper.LONG.ClassObject == null
				|| PrimitiveTypeWrapper.SHORT.ClassObject == null
				|| PrimitiveTypeWrapper.BOOLEAN.ClassObject == null
				|| PrimitiveTypeWrapper.VOID.ClassObject == null)
			{
				throw new InvalidOperationException();
			}
		}
#endif

		internal static TypeWrapper FromClass(java.lang.Class clazz)
		{
#if FIRST_PASS
			return null;
#else
			// MONOBUG redundant cast to workaround mcs bug
			TypeWrapper tw = (TypeWrapper)(object)clazz.typeWrapper;
			if(tw == null)
			{
				Type type = clazz.type;
				if (type == null)
				{
					ResolvePrimitiveTypeWrapperClasses();
					return FromClass(clazz);
				}
				if (type == typeof(void) || type.IsPrimitive || ClassLoaderWrapper.IsRemappedType(type))
				{
					tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
				}
				else
				{
					tw = ClassLoaderWrapper.GetWrapperFromType(type);
				}
#if __MonoCS__
				SetTypeWrapperHack(clazz, tw);
#else
				clazz.typeWrapper = tw;
#endif
			}
			return tw;
#endif
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		public override string ToString()
		{
			return GetType().Name + "[" + name + "]";
		}

		// For UnloadableTypeWrapper it tries to load the type through the specified loader
		// and if that fails it throw a NoClassDefFoundError (not a java.lang.NoClassDefFoundError),
		// for all other types this is a no-op.
		internal virtual TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
		{
			return this;
		}

		private void SetTypeFlag(TypeFlags flag)
		{
			// we use a global lock object, since the chance of contention is very small
			lock (flagsLock)
			{
				flags |= flag;
			}
		}

		internal bool HasIncompleteInterfaceImplementation
		{
			get
			{
				TypeWrapper baseWrapper = this.BaseTypeWrapper;
				return (flags & TypeFlags.HasIncompleteInterfaceImplementation) != 0 || (baseWrapper != null && baseWrapper.HasIncompleteInterfaceImplementation);
			}
		}

		internal void SetHasIncompleteInterfaceImplementation()
		{
			SetTypeFlag(TypeFlags.HasIncompleteInterfaceImplementation);
		}

		internal bool HasUnsupportedAbstractMethods
		{
			get
			{
				foreach(TypeWrapper iface in this.Interfaces)
				{
					if(iface.HasUnsupportedAbstractMethods)
					{
						return true;
					}
				}
				TypeWrapper baseWrapper = this.BaseTypeWrapper;
				return (flags & TypeFlags.HasUnsupportedAbstractMethods) != 0 || (baseWrapper != null && baseWrapper.HasUnsupportedAbstractMethods);
			}
		}

		internal void SetHasUnsupportedAbstractMethods()
		{
			SetTypeFlag(TypeFlags.HasUnsupportedAbstractMethods);
		}

		internal virtual bool HasStaticInitializer
		{
			get
			{
				return (flags & TypeFlags.HasStaticInitializer) != 0;
			}
		}

		internal void SetHasStaticInitializer()
		{
			SetTypeFlag(TypeFlags.HasStaticInitializer);
		}

		internal bool HasVerifyError
		{
			get
			{
				return (flags & TypeFlags.VerifyError) != 0;
			}
		}

		internal void SetHasVerifyError()
		{
			SetTypeFlag(TypeFlags.VerifyError);
		}

		internal bool HasClassFormatError
		{
			get
			{
				return (flags & TypeFlags.ClassFormatError) != 0;
			}
		}

		internal void SetHasClassFormatError()
		{
			SetTypeFlag(TypeFlags.ClassFormatError);
		}

		internal virtual bool IsFakeTypeContainer
		{
			get
			{
				return false;
			}
		}

		internal virtual bool IsFakeNestedType
		{
			get
			{
				return false;
			}
		}

		// is this an anonymous class (in the sense of Unsafe.defineAnonymousClass(), not the JLS)
		internal bool IsUnsafeAnonymous
		{
			get { return (flags & TypeFlags.Anonymous) != 0; }
		}

		// a ghost is an interface that appears to be implemented by a .NET type
		// (e.g. System.String (aka java.lang.String) appears to implement java.lang.CharSequence,
		// so java.lang.CharSequence is a ghost)
		internal virtual bool IsGhost
		{
			get
			{
				return false;
			}
		}

		// is this an array type of which the ultimate element type is a ghost?
		internal bool IsGhostArray
		{
			get
			{
				return !IsUnloadable && IsArray && (ElementTypeWrapper.IsGhost || ElementTypeWrapper.IsGhostArray);
			}
		}

		internal virtual FieldInfo GhostRefField
		{
			get
			{
				throw new InvalidOperationException();
			}
		}

		internal virtual bool IsRemapped
		{
			get
			{
				return false;
			}
		}

		internal bool IsArray
		{
			get
			{
				return name != null && name[0] == '[';
			}
		}

		// NOTE for non-array types this returns 0
		internal int ArrayRank
		{
			get
			{
				int i = 0;
				if(name != null)
				{
					while(name[i] == '[')
					{
						i++;
					}
				}
				return i;
			}
		}

		internal virtual TypeWrapper GetUltimateElementTypeWrapper()
		{
			throw new InvalidOperationException();
		}

		internal bool IsNonPrimitiveValueType
		{
			get
			{
				return this != VerifierTypeWrapper.Null && !IsPrimitive && !IsGhost && TypeAsTBD.IsValueType;
			}
		}

		internal bool IsPrimitive
		{
			get
			{
				return name == null;
			}
		}

		internal bool IsWidePrimitive
		{
			get
			{
				return this == PrimitiveTypeWrapper.LONG || this == PrimitiveTypeWrapper.DOUBLE;
			}
		}

		internal bool IsIntOnStackPrimitive
		{
			get
			{
				return name == null &&
					(this == PrimitiveTypeWrapper.BOOLEAN ||
					this == PrimitiveTypeWrapper.BYTE ||
					this == PrimitiveTypeWrapper.CHAR ||
					this == PrimitiveTypeWrapper.SHORT ||
					this == PrimitiveTypeWrapper.INT);
			}
		}

		private static bool IsJavaPrimitive(Type type)
		{
			return type == PrimitiveTypeWrapper.BOOLEAN.TypeAsTBD
				|| type == PrimitiveTypeWrapper.BYTE.TypeAsTBD
				|| type == PrimitiveTypeWrapper.CHAR.TypeAsTBD
				|| type == PrimitiveTypeWrapper.DOUBLE.TypeAsTBD
				|| type == PrimitiveTypeWrapper.FLOAT.TypeAsTBD
				|| type == PrimitiveTypeWrapper.INT.TypeAsTBD
				|| type == PrimitiveTypeWrapper.LONG.TypeAsTBD
				|| type == PrimitiveTypeWrapper.SHORT.TypeAsTBD
				|| type == PrimitiveTypeWrapper.VOID.TypeAsTBD;
		}

		internal bool IsBoxedPrimitive
		{
			get
			{
				return !IsPrimitive && IsJavaPrimitive(TypeAsSignatureType);
			}
		}

		internal bool IsErasedOrBoxedPrimitiveOrRemapped
		{
			get
			{
				bool erased = IsUnloadable || IsGhostArray;
				return erased || IsBoxedPrimitive || (IsRemapped && this is DotNetTypeWrapper);
			}
		}

		internal bool IsUnloadable
		{
			get
			{
				// NOTE we abuse modifiers to note unloadable classes
				return modifiers == UnloadableModifiersHack;
			}
		}

		internal bool IsVerifierType
		{
			get
			{
				// NOTE we abuse modifiers to note verifier types
				return modifiers == VerifierTypeModifiersHack;
			}
		}

		internal virtual bool IsMapUnsafeException
		{
			get
			{
				return false;
			}
		}

		internal Modifiers Modifiers
		{
			get
			{
				return modifiers;
			}
		}

		// since for inner classes, the modifiers returned by Class.getModifiers are different from the actual
		// modifiers (as used by the VM access control mechanism), we have this additional property
		internal virtual Modifiers ReflectiveModifiers
		{
			get
			{
				return modifiers;
			}
		}

		internal bool IsInternal
		{
			get
			{
				return (flags & TypeFlags.InternalAccess) != 0;
			}
		}

		internal bool IsPublic
		{
			get
			{
				return (modifiers & Modifiers.Public) != 0;
			}
		}

		internal bool IsAbstract
		{
			get
			{
				// interfaces don't need to marked abstract explicitly (and javac 1.1 didn't do it)
				return (modifiers & (Modifiers.Abstract | Modifiers.Interface)) != 0;
			}
		}

		internal bool IsFinal
		{
			get
			{
				return (modifiers & Modifiers.Final) != 0;
			}
		}

		internal bool IsInterface
		{
			get
			{
				Debug.Assert(!IsUnloadable && !IsVerifierType);
				return (modifiers & Modifiers.Interface) != 0;
			}
		}

		// this exists because interfaces and arrays of interfaces are treated specially
		// by the verifier, interfaces don't have a common base (other than java.lang.Object)
		// so any object reference or object array reference can be used where an interface
		// or interface array reference is expected (the compiler will insert the required casts).
		internal bool IsInterfaceOrInterfaceArray
		{
			get
			{
				TypeWrapper tw = this;
				while(tw.IsArray)
				{
					tw = tw.ElementTypeWrapper;
				}
				return tw.IsInterface;
			}
		}

		internal abstract ClassLoaderWrapper GetClassLoader();

		internal FieldWrapper GetFieldWrapper(string fieldName, string fieldSig)
		{
			foreach(FieldWrapper fw in GetFields())
			{
				if(fw.Name == fieldName && fw.Signature == fieldSig)
				{
					return fw;
				}	
			}
			foreach(TypeWrapper iface in this.Interfaces)
			{
				FieldWrapper fw = iface.GetFieldWrapper(fieldName, fieldSig);
				if(fw != null)
				{
					return fw;
				}
			}
			TypeWrapper baseWrapper = this.BaseTypeWrapper;
			if(baseWrapper != null)
			{
				return baseWrapper.GetFieldWrapper(fieldName, fieldSig);
			}
			return null;
		}

		protected virtual void LazyPublishMembers()
		{
			if(methods == null)
			{
				methods = MethodWrapper.EmptyArray;
			}
			if(fields == null)
			{
				fields = FieldWrapper.EmptyArray;
			}
		}

		protected virtual void LazyPublishMethods()
		{
			LazyPublishMembers();
		}

		protected virtual void LazyPublishFields()
		{
			LazyPublishMembers();
		}

		internal MethodWrapper[] GetMethods()
		{
			if(methods == null)
			{
				lock(this)
				{
					if(methods == null)
					{
#if STATIC_COMPILER
						if(IsUnloadable || !CheckMissingBaseTypes(TypeAsBaseType))
						{
							return methods = MethodWrapper.EmptyArray;
						}
#endif
						LazyPublishMethods();
					}
				}
			}
			return methods;
		}

		internal FieldWrapper[] GetFields()
		{
			if(fields == null)
			{
				lock(this)
				{
					if(fields == null)
					{
#if STATIC_COMPILER
						if(IsUnloadable || !CheckMissingBaseTypes(TypeAsBaseType))
						{
							return fields = FieldWrapper.EmptyArray;
						}
#endif
						LazyPublishFields();
					}
				}
			}
			return fields;
		}

#if STATIC_COMPILER
		private static bool CheckMissingBaseTypes(Type type)
		{
			while (type != null)
			{
				if (type.__ContainsMissingType)
				{
					StaticCompiler.IssueMissingTypeMessage(type);
					return false;
				}
				bool ok = true;
				foreach (Type iface in type.__GetDeclaredInterfaces())
				{
					ok &= CheckMissingBaseTypes(iface);
				}
				if (!ok)
				{
					return false;
				}
				type = type.BaseType;
			}
			return true;
		}
#endif

		internal MethodWrapper GetMethodWrapper(string name, string sig, bool inherit)
		{
			// We need to get the methods before calling String.IsInterned, because getting them might cause the strings to be interned
			MethodWrapper[] methods = GetMethods();
			// MemberWrapper interns the name and sig so we can use ref equality
			// profiling has shown this to be more efficient
			string _name = String.IsInterned(name);
			string _sig = String.IsInterned(sig);
			foreach(MethodWrapper mw in methods)
			{
				// NOTE we can use ref equality, because names and signatures are
				// always interned by MemberWrapper
				if(ReferenceEquals(mw.Name, _name) && ReferenceEquals(mw.Signature, _sig))
				{
					return mw;
				}
			}
			TypeWrapper baseWrapper = this.BaseTypeWrapper;
			if(inherit && baseWrapper != null)
			{
				return baseWrapper.GetMethodWrapper(name, sig, inherit);
			}
			return null;
		}

		internal MethodWrapper GetInterfaceMethod(string name, string sig)
		{
			MethodWrapper method = GetMethodWrapper(name, sig, false);
			if (method != null)
			{
				return method;
			}
			TypeWrapper[] interfaces = Interfaces;
			for (int i = 0; i < interfaces.Length; i++)
			{
				method = interfaces[i].GetInterfaceMethod(name, sig);
				if (method != null)
				{
					return method;
				}
			}
			return null;
		}

		internal void SetMethods(MethodWrapper[] methods)
		{
			Debug.Assert(methods != null);
			System.Threading.Thread.MemoryBarrier();
			this.methods = methods;
		}

		internal void SetFields(FieldWrapper[] fields)
		{
			Debug.Assert(fields != null);
			System.Threading.Thread.MemoryBarrier();
			this.fields = fields;
		}

		internal string Name
		{
			get
			{
				return name;
			}
		}

		// the name of the type as it appears in a Java signature string (e.g. "Ljava.lang.Object;" or "I")
		internal virtual string SigName
		{
			get
			{
				return "L" + this.Name + ";";
			}
		}

		// returns true iff wrapper is allowed to access us
		internal bool IsAccessibleFrom(TypeWrapper wrapper)
		{
			return IsPublic
				|| (IsInternal && InternalsVisibleTo(wrapper))
				|| IsPackageAccessibleFrom(wrapper);
		}

		internal bool InternalsVisibleTo(TypeWrapper wrapper)
		{
			return GetClassLoader().InternalsVisibleToImpl(this, wrapper);
		}

		internal virtual bool IsPackageAccessibleFrom(TypeWrapper wrapper)
		{
			if (MatchingPackageNames(name, wrapper.name))
			{
#if STATIC_COMPILER
				CompilerClassLoader ccl = GetClassLoader() as CompilerClassLoader;
				if (ccl != null)
				{
					// this is a hack for multi target -sharedclassloader compilation
					// (during compilation we have multiple CompilerClassLoader instances to represent the single shared runtime class loader)
					return ccl.IsEquivalentTo(wrapper.GetClassLoader());
				}
#endif
				return GetClassLoader() == wrapper.GetClassLoader();
			}
			else
			{
				return false;
			}
		}

		private static bool MatchingPackageNames(string name1, string name2)
		{
			int index1 = name1.LastIndexOf('.');
			int index2 = name2.LastIndexOf('.');
			if (index1 == -1 && index2 == -1)
			{
				return true;
			}
			// for array types we need to skip the brackets
			int skip1 = 0;
			int skip2 = 0;
			while (name1[skip1] == '[')
			{
				skip1++;
			}
			while (name2[skip2] == '[')
			{
				skip2++;
			}
			if (skip1 > 0)
			{
				// skip over the L that follows the brackets
				skip1++;
			}
			if (skip2 > 0)
			{
				// skip over the L that follows the brackets
				skip2++;
			}
			if ((index1 - skip1) != (index2 - skip2))
			{
				return false;
			}
			return String.CompareOrdinal(name1, skip1, name2, skip2, index1 - skip1) == 0;
		}

		internal abstract Type TypeAsTBD
		{
			get;
		}

		internal Type TypeAsSignatureType
		{
			get
			{
				if(IsUnloadable)
				{
					return ((UnloadableTypeWrapper)this).MissingType ?? Types.Object;
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
				}
				return TypeAsTBD;
			}
		}

		internal Type TypeAsPublicSignatureType
		{
			get
			{
				return (IsPublic ? this : GetPublicBaseTypeWrapper()).TypeAsSignatureType;
			}
		}

		internal virtual Type TypeAsBaseType
		{
			get
			{
				return TypeAsTBD;
			}
		}

		internal Type TypeAsLocalOrStackType
		{
			get
			{
				if(IsUnloadable || IsGhost)
				{
					return Types.Object;
				}
				if(IsNonPrimitiveValueType)
				{
					// return either System.ValueType or System.Enum
					return TypeAsTBD.BaseType;
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
				}
				return TypeAsTBD;
			}
		}

		/** <summary>Use this if the type is used as an array or array element</summary> */
		internal Type TypeAsArrayType
		{
			get
			{
				if(IsUnloadable || IsGhost)
				{
					return Types.Object;
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(Types.Object, ArrayRank);
				}
				return TypeAsTBD;
			}
		}

		internal Type TypeAsExceptionType
		{
			get
			{
				if(IsUnloadable)
				{
					return Types.Exception;
				}
				return TypeAsTBD;
			}
		}

		internal abstract TypeWrapper BaseTypeWrapper
		{
			get;
		}

		internal TypeWrapper ElementTypeWrapper
		{
			get
			{
				Debug.Assert(!this.IsUnloadable);
				Debug.Assert(this == VerifierTypeWrapper.Null || this.IsArray);

				if(this == VerifierTypeWrapper.Null)
				{
					return VerifierTypeWrapper.Null;
				}

				// TODO consider caching the element type
				switch(name[1])
				{
					case '[':
						// NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
						// (because the ultimate element type was already loaded when this type was created)
						return GetClassLoader().LoadClassByDottedNameFast(name.Substring(1));
					case 'L':
						// NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
						// (because the ultimate element type was already loaded when this type was created)
						return GetClassLoader().LoadClassByDottedNameFast(name.Substring(2, name.Length - 3));
					case 'Z':
						return PrimitiveTypeWrapper.BOOLEAN;
					case 'B':
						return PrimitiveTypeWrapper.BYTE;
					case 'S':
						return PrimitiveTypeWrapper.SHORT;
					case 'C':
						return PrimitiveTypeWrapper.CHAR;
					case 'I':
						return PrimitiveTypeWrapper.INT;
					case 'J':
						return PrimitiveTypeWrapper.LONG;
					case 'F':
						return PrimitiveTypeWrapper.FLOAT;
					case 'D':
						return PrimitiveTypeWrapper.DOUBLE;
					default:
						throw new InvalidOperationException(name);
				}
			}
		}

		internal TypeWrapper MakeArrayType(int rank)
		{
			Debug.Assert(rank != 0);
			// NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
			return GetClassLoader().LoadClassByDottedNameFast(new String('[', rank) + this.SigName);
		}

		internal bool ImplementsInterface(TypeWrapper interfaceWrapper)
		{
			TypeWrapper typeWrapper = this;
			while(typeWrapper != null)
			{
				TypeWrapper[] interfaces = typeWrapper.Interfaces;
				for(int i = 0; i < interfaces.Length; i++)
				{
					if(interfaces[i] == interfaceWrapper)
					{
						return true;
					}
					if(interfaces[i].ImplementsInterface(interfaceWrapper))
					{
						return true;
					}
				}
				typeWrapper = typeWrapper.BaseTypeWrapper;
			}
			return false;
		}

		internal bool IsSubTypeOf(TypeWrapper baseType)
		{
			// make sure IsSubTypeOf isn't used on primitives
			Debug.Assert(!this.IsPrimitive);
			Debug.Assert(!baseType.IsPrimitive);
			// can't be used on Unloadable
			Debug.Assert(!this.IsUnloadable);
			Debug.Assert(!baseType.IsUnloadable);

			if(baseType.IsInterface)
			{
				if(baseType == this)
				{
					return true;
				}
				return ImplementsInterface(baseType);
			}
			// NOTE this isn't just an optimization, it is also required when this is an interface
			if(baseType == CoreClasses.java.lang.Object.Wrapper)
			{
				return true;
			}
			TypeWrapper subType = this;
			while(subType != baseType)
			{
				subType = subType.BaseTypeWrapper;
				if(subType == null)
				{
					return false;
				}
			}
			return true;
		}

		internal bool IsAssignableTo(TypeWrapper wrapper)
		{
			if(this == wrapper)
			{
				return true;
			}
			if(this.IsPrimitive || wrapper.IsPrimitive)
			{
				return false;
			}
			if(this == VerifierTypeWrapper.Null)
			{
				return true;
			}
			if(wrapper.IsInterface)
			{
				return ImplementsInterface(wrapper);
			}
			int rank1 = this.ArrayRank;
			int rank2 = wrapper.ArrayRank;
			if(rank1 > 0 && rank2 > 0)
			{
				rank1--;
				rank2--;
				TypeWrapper elem1 = this.ElementTypeWrapper;
				TypeWrapper elem2 = wrapper.ElementTypeWrapper;
				while(rank1 != 0 && rank2 != 0)
				{
					elem1 = elem1.ElementTypeWrapper;
					elem2 = elem2.ElementTypeWrapper;
					rank1--;
					rank2--;
				}
				if(elem1.IsPrimitive || elem2.IsPrimitive)
				{
					return false;
				}
				return (!elem1.IsNonPrimitiveValueType && elem1.IsSubTypeOf(elem2));
			}
			return this.IsSubTypeOf(wrapper);
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal bool IsInstance(object obj)
		{
			if(obj != null)
			{
				TypeWrapper thisWrapper = this;
				TypeWrapper objWrapper = IKVM.NativeCode.ikvm.runtime.Util.GetTypeWrapperFromObject(obj);
				return objWrapper.IsAssignableTo(thisWrapper);
			}
			return false;
		}
#endif

		internal virtual TypeWrapper[] Interfaces
		{
			get { return EmptyArray; }
		}

		// NOTE this property can only be called for finished types!
		internal virtual TypeWrapper[] InnerClasses
		{
			get { return EmptyArray; }
		}

		// NOTE this property can only be called for finished types!
		internal virtual TypeWrapper DeclaringTypeWrapper
		{
			get { return null; }
		}

		internal virtual void Finish()
		{
		}

		internal void LinkAll()
		{
			if ((flags & TypeFlags.Linked) == 0)
			{
				TypeWrapper tw = BaseTypeWrapper;
				if (tw != null)
				{
					tw.LinkAll();
				}
				foreach (TypeWrapper iface in Interfaces)
				{
					iface.LinkAll();
				}
				foreach (MethodWrapper mw in GetMethods())
				{
					mw.Link();
				}
				foreach (FieldWrapper fw in GetFields())
				{
					fw.Link();
				}
				SetTypeFlag(TypeFlags.Linked);
			}
		}

#if !STATIC_COMPILER
		[Conditional("DEBUG")]
		internal static void AssertFinished(Type type)
		{
			if(type != null)
			{
				while(type.HasElementType)
				{
					type = type.GetElementType();
				}
				Debug.Assert(!(type is TypeBuilder));
			}
		}
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal void RunClassInit()
		{
			Type t = IsRemapped ? TypeAsBaseType : TypeAsTBD;
			if(t != null)
			{
				System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.TypeHandle);
			}
		}
#endif

#if EMITTERS
		internal void EmitUnbox(CodeEmitter ilgen)
		{
			Debug.Assert(this.IsNonPrimitiveValueType);

			ilgen.EmitUnboxSpecial(this.TypeAsTBD);
		}

		internal void EmitBox(CodeEmitter ilgen)
		{
			Debug.Assert(this.IsNonPrimitiveValueType);

			ilgen.Emit(OpCodes.Box, this.TypeAsTBD);
		}

		internal void EmitConvSignatureTypeToStackType(CodeEmitter ilgen)
		{
			if(IsUnloadable)
			{
			}
			else if(this == PrimitiveTypeWrapper.BYTE)
			{
				ilgen.Emit(OpCodes.Conv_I1);
			}
			else if(IsNonPrimitiveValueType)
			{
				EmitBox(ilgen);
			}
			else if(IsGhost)
			{
				CodeEmitterLocal local = ilgen.DeclareLocal(TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloca, local);
				ilgen.Emit(OpCodes.Ldfld, GhostRefField);
			}
		}

		// NOTE sourceType is optional and only used for interfaces,
		// it is *not* used to automatically downcast
		internal void EmitConvStackTypeToSignatureType(CodeEmitter ilgen, TypeWrapper sourceType)
		{
			if(!IsUnloadable)
			{
				if(IsGhost)
				{
					CodeEmitterLocal local1 = ilgen.DeclareLocal(TypeAsLocalOrStackType);
					ilgen.Emit(OpCodes.Stloc, local1);
					CodeEmitterLocal local2 = ilgen.DeclareLocal(TypeAsSignatureType);
					ilgen.Emit(OpCodes.Ldloca, local2);
					ilgen.Emit(OpCodes.Ldloc, local1);
					ilgen.Emit(OpCodes.Stfld, GhostRefField);
					ilgen.Emit(OpCodes.Ldloca, local2);
					ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);
				}
					// because of the way interface merging works, any reference is valid
					// for any interface reference
				else if(IsInterfaceOrInterfaceArray && (sourceType == null || sourceType.IsUnloadable || !sourceType.IsAssignableTo(this)))
				{
					ilgen.EmitAssertType(TypeAsTBD);
					Profiler.Count("InterfaceDownCast");
				}
				else if(IsNonPrimitiveValueType)
				{
					EmitUnbox(ilgen);
				}
				else if(sourceType != null && sourceType.IsUnloadable)
				{
					ilgen.Emit(OpCodes.Castclass, TypeAsSignatureType);
				}
			}
		}

		internal virtual void EmitCheckcast(CodeEmitter ilgen)
		{
			if(IsGhost)
			{
				ilgen.Emit(OpCodes.Dup);
				// TODO make sure we get the right "Cast" method and cache it
				// NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
				// so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
				ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("Cast"));
				ilgen.Emit(OpCodes.Pop);
			}
			else if(IsGhostArray)
			{
				ilgen.Emit(OpCodes.Dup);
				// TODO make sure we get the right "CastArray" method and cache it
				// NOTE for dynamic ghosts we don't end up here because AotTypeWrapper overrides this method,
				// so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
				TypeWrapper tw = this;
				int rank = 0;
				while(tw.IsArray)
				{
					rank++;
					tw = tw.ElementTypeWrapper;
				}
				ilgen.EmitLdc_I4(rank);
				ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("CastArray"));
				ilgen.Emit(OpCodes.Castclass, ArrayTypeWrapper.MakeArrayType(Types.Object, rank));
			}
			else
			{
				ilgen.EmitCastclass(TypeAsTBD);
			}
		}

		internal virtual void EmitInstanceOf(CodeEmitter ilgen)
		{
			if(IsGhost)
			{
				// TODO make sure we get the right "IsInstance" method and cache it
				// NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
				// so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
				ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("IsInstance"));
			}
			else if(IsGhostArray)
			{
				// TODO make sure we get the right "IsInstanceArray" method and cache it
				// NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
				// so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
				TypeWrapper tw = this;
				int rank = 0;
				while(tw.IsArray)
				{
					rank++;
					tw = tw.ElementTypeWrapper;
				}
				ilgen.EmitLdc_I4(rank);
				ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("IsInstanceArray"));
			}
			else
			{
				ilgen.Emit_instanceof(TypeAsTBD);
			}
		}
#endif // EMITTERS

		// NOTE don't call this method, call MethodWrapper.Link instead
		internal virtual MethodBase LinkMethod(MethodWrapper mw)
		{
			return mw.GetMethod();
		}

		// NOTE don't call this method, call FieldWrapper.Link instead
		internal virtual FieldInfo LinkField(FieldWrapper fw)
		{
			return fw.GetField();
		}

#if EMITTERS
		internal virtual void EmitRunClassConstructor(CodeEmitter ilgen)
		{
		}
#endif // EMITTERS

		internal virtual string GetGenericSignature()
		{
			return null;
		}

		internal virtual string GetGenericMethodSignature(MethodWrapper mw)
		{
			return null;
		}

		internal virtual string GetGenericFieldSignature(FieldWrapper fw)
		{
			return null;
		}

		internal virtual MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
		{
			return null;
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal virtual string[] GetEnclosingMethod()
		{
			return null;
		}

		internal virtual object[] GetDeclaredAnnotations()
		{
			return null;
		}

		internal virtual object[] GetMethodAnnotations(MethodWrapper mw)
		{
			return null;
		}

		internal virtual object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			return null;
		}

		internal virtual object[] GetFieldAnnotations(FieldWrapper fw)
		{
			return null;
		}

		internal virtual string GetSourceFileName()
		{
			return null;
		}

		internal virtual int GetSourceLineNumber(MethodBase mb, int ilOffset)
		{
			return -1;
		}

		internal virtual object GetAnnotationDefault(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb != null)
			{
				object[] attr = mb.GetCustomAttributes(typeof(AnnotationDefaultAttribute), false);
				if(attr.Length == 1)
				{
					return JVM.NewAnnotationElementValue(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), mw.ReturnType.ClassObject, ((AnnotationDefaultAttribute)attr[0]).Value);
				}
			}
			return null;
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		internal virtual Annotation Annotation
		{
			get
			{
				return null;
			}
		}

		internal virtual Type EnumType
		{
			get
			{
				return null;
			}
		}

		private static Type[] GetInterfaces(Type type)
		{
#if STATIC_COMPILER || STUB_GENERATOR
			List<Type> list = new List<Type>();
			for (; type != null && !type.__IsMissing; type = type.BaseType)
			{
				AddInterfaces(list, type);
			}
			return list.ToArray();
#else
			return type.GetInterfaces();
#endif
		}

#if STATIC_COMPILER || STUB_GENERATOR
		private static void AddInterfaces(List<Type> list, Type type)
		{
			foreach (Type iface in type.__GetDeclaredInterfaces())
			{
				if (!list.Contains(iface))
				{
					list.Add(iface);
					if (!iface.__IsMissing)
					{
						AddInterfaces(list, iface);
					}
				}
			}
		}
#endif

		protected static TypeWrapper[] GetImplementedInterfacesAsTypeWrappers(Type type)
		{
			Type[] interfaceTypes = GetInterfaces(type);
			TypeWrapper[] interfaces = new TypeWrapper[interfaceTypes.Length];
			for (int i = 0; i < interfaceTypes.Length; i++)
			{
				Type decl = interfaceTypes[i].DeclaringType;
				if (decl != null && AttributeHelper.IsGhostInterface(decl))
				{
					// we have to return the declaring type for ghost interfaces
					interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(decl);
				}
				else
				{
					interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(interfaceTypes[i]);
				}
			}
			for (int i = 0; i < interfaceTypes.Length; i++)
			{
				if (interfaces[i].IsRemapped)
				{
					// for remapped interfaces, we also return the original interface (Java types will ignore it, if it isn't listed in the ImplementsAttribute)
					TypeWrapper twRemapped = interfaces[i];
					TypeWrapper tw = DotNetTypeWrapper.GetWrapperFromDotNetType(interfaceTypes[i]);
					interfaces[i] = tw;
					if (Array.IndexOf(interfaces, twRemapped) == -1)
					{
						interfaces = ArrayUtil.Concat(interfaces, twRemapped);
					}
				}
			}
			return interfaces;
		}

		internal TypeWrapper GetPublicBaseTypeWrapper()
		{
			Debug.Assert(!this.IsPublic);
			if (this.IsUnloadable || this.IsInterface)
			{
				return CoreClasses.java.lang.Object.Wrapper;
			}
			for (TypeWrapper tw = this; ; tw = tw.BaseTypeWrapper)
			{
				if (tw.IsPublic)
				{
					return tw;
				}
			}
		}

#if !STUB_GENERATOR
		// return the constructor used for automagic .NET serialization
		internal virtual MethodBase GetSerializationConstructor()
		{
			return this.TypeAsBaseType.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] {
						JVM.Import(typeof(System.Runtime.Serialization.SerializationInfo)), JVM.Import(typeof(System.Runtime.Serialization.StreamingContext)) }, null);
		}

		internal virtual MethodBase GetBaseSerializationConstructor()
		{
			return BaseTypeWrapper.GetSerializationConstructor();
		}
#endif

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal virtual object GhostWrap(object obj)
		{
			return obj;
		}

		internal virtual object GhostUnwrap(object obj)
		{
			return obj;
		}
#endif

		internal bool IsDynamic
		{
#if STUB_GENERATOR
			get { return false; }
#else
			get { return this is DynamicTypeWrapper; }
#endif
		}

		internal virtual object[] GetConstantPool()
		{
			return null;
		}

		internal virtual byte[] GetRawTypeAnnotations()
		{
			return null;
		}

		internal virtual byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
		{
			return null;
		}

		internal virtual byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
		{
			return null;
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal virtual TypeWrapper Host
		{
			get { return null; }
		}
#endif
	}

	sealed class UnloadableTypeWrapper : TypeWrapper
	{
		internal const string ContainerTypeName = "__<Unloadable>";
		private readonly Type missingType;
		private Type customModifier;

		internal UnloadableTypeWrapper(string name)
			: base(TypeFlags.None, TypeWrapper.UnloadableModifiersHack, name)
		{
		}

		internal UnloadableTypeWrapper(Type missingType)
			: this(missingType.FullName)	// TODO demangle and re-mangle appropriately
		{
			this.missingType = missingType;
		}

		internal UnloadableTypeWrapper(string name, Type customModifier)
			: this(name)
		{
			this.customModifier = customModifier;
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get { return null; }
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return null;
		}

		internal override TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
		{
			TypeWrapper tw = loader.LoadClassByDottedNameFast(this.Name);
			if(tw == null)
			{
				throw new NoClassDefFoundError(this.Name);
			}
			return tw;
		}

		internal override string SigName
		{
			get
			{
				string name = Name;
				if(name.StartsWith("["))
				{
					return name;
				}
				return "L" + name + ";";
			}
		}

		protected override void LazyPublishMembers()
		{
			throw new InvalidOperationException("LazyPublishMembers called on UnloadableTypeWrapper: " + Name);
		}

		internal override Type TypeAsTBD
		{
			get
			{
				throw new InvalidOperationException("get_Type called on UnloadableTypeWrapper: " + Name);
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
#if STATIC_COMPILER
				if (missingType != null)
				{
					StaticCompiler.IssueMissingTypeMessage(missingType);
					return TypeWrapper.EmptyArray;
				}
#endif
				throw new InvalidOperationException("get_Interfaces called on UnloadableTypeWrapper: " + Name);
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				throw new InvalidOperationException("get_InnerClasses called on UnloadableTypeWrapper: " + Name);
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				throw new InvalidOperationException("get_DeclaringTypeWrapper called on UnloadableTypeWrapper: " + Name);
			}
		}

		internal override void Finish()
		{
			throw new InvalidOperationException("Finish called on UnloadableTypeWrapper: " + Name);
		}

		internal Type MissingType
		{
			get { return missingType; }
		}

		internal Type CustomModifier
		{
			get { return customModifier; }
		}

		internal void SetCustomModifier(Type type)
		{
			this.customModifier = type;
		}

#if EMITTERS
		internal Type GetCustomModifier(TypeWrapperFactory context)
		{
			// we don't need to lock, because we're only supposed to be called while holding the finish lock
			return customModifier ?? (customModifier = context.DefineUnloadable(this.Name));
		}

		internal override void EmitCheckcast(CodeEmitter ilgen)
		{
			throw new InvalidOperationException("EmitCheckcast called on UnloadableTypeWrapper: " + Name);
		}

		internal override void EmitInstanceOf(CodeEmitter ilgen)
		{
			throw new InvalidOperationException("EmitInstanceOf called on UnloadableTypeWrapper: " + Name);
		}
#endif // EMITTERS
	}

	sealed class PrimitiveTypeWrapper : TypeWrapper
	{
		internal static readonly PrimitiveTypeWrapper BYTE = new PrimitiveTypeWrapper(Types.Byte, "B");
		internal static readonly PrimitiveTypeWrapper CHAR = new PrimitiveTypeWrapper(Types.Char, "C");
		internal static readonly PrimitiveTypeWrapper DOUBLE = new PrimitiveTypeWrapper(Types.Double, "D");
		internal static readonly PrimitiveTypeWrapper FLOAT = new PrimitiveTypeWrapper(Types.Single, "F");
		internal static readonly PrimitiveTypeWrapper INT = new PrimitiveTypeWrapper(Types.Int32, "I");
		internal static readonly PrimitiveTypeWrapper LONG = new PrimitiveTypeWrapper(Types.Int64, "J");
		internal static readonly PrimitiveTypeWrapper SHORT = new PrimitiveTypeWrapper(Types.Int16, "S");
		internal static readonly PrimitiveTypeWrapper BOOLEAN = new PrimitiveTypeWrapper(Types.Boolean, "Z");
		internal static readonly PrimitiveTypeWrapper VOID = new PrimitiveTypeWrapper(Types.Void, "V");

		private readonly Type type;
		private readonly string sigName;

		private PrimitiveTypeWrapper(Type type, string sigName)
			: base(TypeFlags.None, Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null)
		{
			this.type = type;
			this.sigName = sigName;
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get { return null; }
		}

		internal static bool IsPrimitiveType(Type type)
		{
			return type == BYTE.type
				|| type == CHAR.type
				|| type == DOUBLE.type
				|| type == FLOAT.type
				|| type == INT.type
				|| type == LONG.type
				|| type == SHORT.type
				|| type == BOOLEAN.type
				|| type == VOID.type;
		}

		internal override string SigName
		{
			get
			{
				return sigName;
			}
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return ClassLoaderWrapper.GetBootstrapClassLoader();
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return type;
			}
		}

		public override string ToString()
		{
			return "PrimitiveTypeWrapper[" + sigName + "]";
		}
	}

	class CompiledTypeWrapper : TypeWrapper
	{
		private readonly Type type;
		private TypeWrapper baseTypeWrapper = VerifierTypeWrapper.Null;
		private volatile TypeWrapper[] interfaces;
		private MethodInfo clinitMethod;
		private volatile bool clinitMethodSet;
		private Modifiers reflectiveModifiers;

		internal static CompiledTypeWrapper newInstance(string name, Type type)
		{
			// TODO since ghost and remapped types can only exist in the core library assembly, we probably
			// should be able to remove the Type.IsDefined() tests in most cases
			if(type.IsValueType && AttributeHelper.IsGhostInterface(type))
			{
				return new CompiledGhostTypeWrapper(name, type);
			}
			else if(AttributeHelper.IsRemappedType(type))
			{
				return new CompiledRemappedTypeWrapper(name, type);
			}
			else
			{
				return new CompiledTypeWrapper(name, type);
			}
		}

		private sealed class CompiledRemappedTypeWrapper : CompiledTypeWrapper
		{
			private readonly Type remappedType;

			internal CompiledRemappedTypeWrapper(string name, Type type)
				: base(name, type)
			{
				RemappedTypeAttribute attr = AttributeHelper.GetRemappedType(type);
				if(attr == null)
				{
					throw new InvalidOperationException();
				}
				remappedType = attr.Type;
			}

			internal override Type TypeAsTBD
			{
				get
				{
					return remappedType;
				}
			}

			internal override bool IsRemapped
			{
				get
				{
					return true;
				}
			}

			protected override void LazyPublishMethods()
			{
				List<MethodWrapper> list = new List<MethodWrapper>();
				const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
				foreach(ConstructorInfo ctor in type.GetConstructors(bindingFlags))
				{
					AddMethod(list, ctor);
				}
				foreach(MethodInfo method in type.GetMethods(bindingFlags))
				{
					AddMethod(list, method);
				}
				// if we're a remapped interface, we need to get the methods from the real interface
				if(remappedType.IsInterface)
				{
					Type nestedHelper = type.GetNestedType("__Helper", BindingFlags.Public | BindingFlags.Static);
					foreach(RemappedInterfaceMethodAttribute m in AttributeHelper.GetRemappedInterfaceMethods(type))
					{
						MethodInfo method = remappedType.GetMethod(m.MappedTo);
						MethodInfo mbHelper = method;
						ExModifiers modifiers = AttributeHelper.GetModifiers(method, false);
						string name;
						string sig;
						TypeWrapper retType;
						TypeWrapper[] paramTypes;
						MemberFlags flags = MemberFlags.None;
						GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
						if(nestedHelper != null)
						{
							mbHelper = nestedHelper.GetMethod(m.Name);
							if(mbHelper == null)
							{
								mbHelper = method;
							}
						}
						MethodWrapper mw = new CompiledRemappedMethodWrapper(this, m.Name, sig, method, retType, paramTypes, modifiers, false, mbHelper, null);
						mw.SetDeclaredExceptions(m.Throws);
						list.Add(mw);
					}
				}
				SetMethods(list.ToArray());
			}

			private void AddMethod(List<MethodWrapper> list, MethodBase method)
			{
				HideFromJavaFlags flags = AttributeHelper.GetHideFromJavaFlags(method);
				if((flags & HideFromJavaFlags.Code) == 0
					&& (remappedType.IsSealed || !method.Name.StartsWith("instancehelper_"))
					&& (!remappedType.IsSealed || method.IsStatic))
				{
					list.Add(CreateRemappedMethodWrapper(method, flags));
				}
			}

			protected override void LazyPublishFields()
			{
				List<FieldWrapper> list = new List<FieldWrapper>();
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				foreach(FieldInfo field in fields)
				{
					HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
					if((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
					{
						list.Add(CreateFieldWrapper(field, hideFromJavaFlags));
					}
				}
				SetFields(list.ToArray());
			}

			private MethodWrapper CreateRemappedMethodWrapper(MethodBase mb, HideFromJavaFlags hideFromJavaflags)
			{
				ExModifiers modifiers = AttributeHelper.GetModifiers(mb, false);
				string name;
				string sig;
				TypeWrapper retType;
				TypeWrapper[] paramTypes;
				MemberFlags flags = MemberFlags.None;
				GetNameSigFromMethodBase(mb, out name, out sig, out retType, out paramTypes, ref flags);
				MethodInfo mbHelper = mb as MethodInfo;
				bool hideFromReflection = mbHelper != null && (hideFromJavaflags & HideFromJavaFlags.Reflection) != 0;
				MethodInfo mbNonvirtualHelper = null;
				if(!mb.IsStatic && !mb.IsConstructor)
				{
					ParameterInfo[] parameters = mb.GetParameters();
					Type[] argTypes = new Type[parameters.Length + 1];
					argTypes[0] = remappedType;
					for(int i = 0; i < parameters.Length; i++)
					{
						argTypes[i + 1] = parameters[i].ParameterType;
					}
					MethodInfo helper = type.GetMethod("instancehelper_" + mb.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, argTypes, null);
					if(helper != null)
					{
						mbHelper = helper;
					}
					mbNonvirtualHelper = type.GetMethod("nonvirtualhelper/" + mb.Name, BindingFlags.NonPublic | BindingFlags.Static, null, argTypes, null);
				}
				return new CompiledRemappedMethodWrapper(this, name, sig, mb, retType, paramTypes, modifiers, hideFromReflection, mbHelper, mbNonvirtualHelper);
			}
		}

		private sealed class CompiledGhostTypeWrapper : CompiledTypeWrapper
		{
			private volatile FieldInfo ghostRefField;
			private volatile Type typeAsBaseType;

			internal CompiledGhostTypeWrapper(string name, Type type)
				: base(name, type)
			{
			}

			internal override Type TypeAsBaseType
			{
				get
				{
					if(typeAsBaseType == null)
					{
						typeAsBaseType = type.GetNestedType("__Interface");
					}
					return typeAsBaseType;
				}
			}

			internal override FieldInfo GhostRefField
			{
				get
				{
					if(ghostRefField == null)
					{
						ghostRefField = type.GetField("__<ref>");
					}
					return ghostRefField;
				}
			}

			internal override bool IsGhost
			{
				get
				{
					return true;
				}
			}

#if !STATIC_COMPILER && !STUB_GENERATOR && !FIRST_PASS
			internal override object GhostWrap(object obj)
			{
				return type.GetMethod("Cast").Invoke(null, new object[] { obj });
			}

			internal override object GhostUnwrap(object obj)
			{
				return type.GetMethod("ToObject").Invoke(obj, new object[0]);
			}
#endif
		}

		internal static string GetName(Type type)
		{
			Debug.Assert(!type.HasElementType);
			Debug.Assert(!type.IsGenericType);
			Debug.Assert(AttributeHelper.IsJavaModule(type.Module));

			// look for our custom attribute, that contains the real name of the type (for inner classes)
			InnerClassAttribute attr = AttributeHelper.GetInnerClass(type);
			if(attr != null)
			{
				string name = attr.InnerClassName;
				if(name != null)
				{
					return name;
				}
			}
			if(type.DeclaringType != null)
			{
				return GetName(type.DeclaringType) + "$" + TypeNameUtil.Unescape(type.Name);
			}
			return TypeNameUtil.Unescape(type.FullName);
		}

		private static TypeWrapper GetBaseTypeWrapper(Type type)
		{
			if(type.IsInterface || AttributeHelper.IsGhostInterface(type))
			{
				return null;
			}
			else if(type.BaseType == null)
			{
				// System.Object must appear to be derived from java.lang.Object
				return CoreClasses.java.lang.Object.Wrapper;
			}
			else
			{
				RemappedTypeAttribute attr = AttributeHelper.GetRemappedType(type);
				if(attr != null)
				{
					if(attr.Type == Types.Object)
					{
						return null;
					}
					else
					{
						return CoreClasses.java.lang.Object.Wrapper;
					}
				}
				else if(ClassLoaderWrapper.IsRemappedType(type.BaseType))
				{
					// if we directly extend System.Object or System.Exception, the base class must be cli.System.Object or cli.System.Exception
					return DotNetTypeWrapper.GetWrapperFromDotNetType(type.BaseType);
				}
				TypeWrapper tw = null;
				while(tw == null)
				{
					type = type.BaseType;
					tw = ClassLoaderWrapper.GetWrapperFromType(type);
				}
				return tw;
			}
		}

		private CompiledTypeWrapper(ExModifiers exmod, string name)
			: base(exmod.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, exmod.Modifiers, name)
		{
		}

		private CompiledTypeWrapper(string name, Type type)
			: this(GetModifiers(type), name)
		{
			Debug.Assert(!(type is TypeBuilder));
			Debug.Assert(!type.Name.EndsWith("[]"));

			this.type = type;
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get
			{
				if (baseTypeWrapper != VerifierTypeWrapper.Null)
				{
					return baseTypeWrapper;
				}
				return baseTypeWrapper = GetBaseTypeWrapper(type);
			}
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return AssemblyClassLoader.FromAssembly(type.Assembly);
		}

		private static ExModifiers GetModifiers(Type type)
		{
			ModifiersAttribute attr = AttributeHelper.GetModifiersAttribute(type);
			if(attr != null)
			{
				return new ExModifiers(attr.Modifiers, attr.IsInternal);
			}
			// only returns public, protected, private, final, static, abstract and interface (as per
			// the documentation of Class.getModifiers())
			Modifiers modifiers = 0;
			if(type.IsPublic || type.IsNestedPublic)
			{
				modifiers |= Modifiers.Public;
			}
			if(type.IsSealed)
			{
				modifiers |= Modifiers.Final;
			}
			if(type.IsAbstract)
			{
				modifiers |= Modifiers.Abstract;
			}
			if(type.IsInterface)
			{
				modifiers |= Modifiers.Interface;
			}
			else
			{
				modifiers |= Modifiers.Super;
			}
			return new ExModifiers(modifiers, false);
		}

		internal override bool HasStaticInitializer
		{
			get
			{
				if(!clinitMethodSet)
				{
					try
					{
						clinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					}
#if STATIC_COMPILER
					catch (IKVM.Reflection.MissingMemberException) { }
#endif
					finally { }
					clinitMethodSet = true;
				}
				return clinitMethod != null;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				if(interfaces == null)
				{
					interfaces = GetInterfaces();
				}
				return interfaces;
			}
		}

		private TypeWrapper[] GetInterfaces()
		{
			// NOTE instead of getting the interfaces list from Type, we use a custom
			// attribute to list the implemented interfaces, because Java reflection only
			// reports the interfaces *directly* implemented by the type, not the inherited
			// interfaces. This is significant for serialVersionUID calculation (for example).
			ImplementsAttribute attr = AttributeHelper.GetImplements(type);
			if (attr == null)
			{
				if (BaseTypeWrapper == CoreClasses.java.lang.Object.Wrapper)
				{
					return GetImplementedInterfacesAsTypeWrappers(type);
				}
				return TypeWrapper.EmptyArray;
			}
			string[] interfaceNames = attr.Interfaces;
			TypeWrapper[] interfaceWrappers = new TypeWrapper[interfaceNames.Length];
			if (this.IsRemapped)
			{
				for (int i = 0; i < interfaceWrappers.Length; i++)
				{
					interfaceWrappers[i] = ClassLoaderWrapper.LoadClassCritical(interfaceNames[i]);
				}
			}
			else
			{
				TypeWrapper[] typeWrappers = GetImplementedInterfacesAsTypeWrappers(type);
				for (int i = 0; i < interfaceWrappers.Length; i++)
				{
					for (int j = 0; j < typeWrappers.Length; j++)
					{
						if (typeWrappers[j].Name == interfaceNames[i])
						{
							interfaceWrappers[i] = typeWrappers[j];
							break;
						}
					}
					if (interfaceWrappers[i] == null)
					{
#if STATIC_COMPILER
						throw new FatalCompilerErrorException(Message.UnableToResolveInterface, interfaceNames[i], this);
#else
						JVM.CriticalFailure("Unable to resolve interface " + interfaceNames[i] + " on type " + this, null);
#endif
					}
				}
			}
			return interfaceWrappers;
		}

		private static bool IsNestedTypeAnonymousOrLocalClass(Type type)
		{
			switch (type.Attributes & (TypeAttributes.SpecialName | TypeAttributes.VisibilityMask))
			{
				case TypeAttributes.SpecialName | TypeAttributes.NestedPublic:
				case TypeAttributes.SpecialName | TypeAttributes.NestedAssembly:
					return AttributeHelper.HasEnclosingMethodAttribute(type);
				default:
					return false;
			}
		}

		private static bool IsAnnotationAttribute(Type type)
		{
			return type.Name.EndsWith("Attribute", StringComparison.Ordinal)
				&& type.IsClass
				&& type.BaseType.FullName == "ikvm.internal.AnnotationAttributeBase";
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				List<TypeWrapper> wrappers = new List<TypeWrapper>();
				foreach(Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
				{
					if(IsAnnotationAttribute(nested))
					{
						// HACK it's the custom attribute we generated for a corresponding annotation, so we shouldn't surface it as an inner classes
						// (we can't put a HideFromJavaAttribute on it, because we do want the class to be visible as a $Proxy)
					}
					else if(IsNestedTypeAnonymousOrLocalClass(nested))
					{
						// anonymous and local classes are not reported as inner classes
					}
					else if(AttributeHelper.IsHideFromJava(nested))
					{
						// ignore
					}
					else
					{
						wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(nested));
					}
				}
				foreach(string s in AttributeHelper.GetNonNestedInnerClasses(type))
				{
					wrappers.Add(GetClassLoader().LoadClassByDottedName(s));
				}
				return wrappers.ToArray();
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				if(IsNestedTypeAnonymousOrLocalClass(type))
				{
					return null;
				}
				Type declaringType = type.DeclaringType;
				if(declaringType != null)
				{
					return ClassLoaderWrapper.GetWrapperFromType(declaringType);
				}
				string decl = AttributeHelper.GetNonNestedOuterClasses(type);
				if(decl != null)
				{
					return GetClassLoader().LoadClassByDottedName(decl);
				}
				return null;
			}
		}

		// returns true iff name is of the form "...$<n>"
		private static bool IsAnonymousClassName(string name)
		{
			int index = name.LastIndexOf('$') + 1;
			if (index > 1 && index < name.Length)
			{
				while (index < name.Length)
				{
					if ("0123456789".IndexOf(name[index++]) == -1)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// This method uses some heuristics to predict the reflective modifiers and if the prediction matches
		// we can avoid storing the InnerClassesAttribute to record the modifiers.
		// The heuristics are based on javac from Java 7.
		internal static Modifiers PredictReflectiveModifiers(TypeWrapper tw)
		{
			Modifiers modifiers = Modifiers.Static | (tw.Modifiers & (Modifiers.Public | Modifiers.Abstract | Modifiers.Interface));
			// javac marks anonymous classes as final, but the InnerClasses attribute access_flags does not have the ACC_FINAL flag set
			if (tw.IsFinal && !IsAnonymousClassName(tw.Name))
			{
				modifiers |= Modifiers.Final;
			}
			// javac uses the this$0 field to store the outer instance reference for non-static inner classes
			foreach (FieldWrapper fw in tw.GetFields())
			{
				if (fw.Name == "this$0")
				{
					modifiers &= ~Modifiers.Static;
					break;
				}
			}
			return modifiers;
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				if (reflectiveModifiers == 0)
				{
					Modifiers mods;
					InnerClassAttribute attr = AttributeHelper.GetInnerClass(type);
					if (attr != null)
					{
						// the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
						// (minus ACC_SUPER)
						mods = attr.Modifiers & (Modifiers)0x761F;
					}
					else if (type.DeclaringType != null)
					{
						mods = PredictReflectiveModifiers(this);
					}
					else
					{
						// the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
						// (minus ACC_SUPER)
						mods = Modifiers & (Modifiers)0x7611;
					}
					if (IsInterface)
					{
						mods |= Modifiers.Abstract;
					}
					reflectiveModifiers = mods;
				}
				return reflectiveModifiers;
			}
		}

		internal override Type TypeAsBaseType
		{
			get
			{
				return type;
			}
		}

		private void SigTypePatchUp(string sigtype, ref TypeWrapper type)
		{
			if(sigtype != type.SigName)
			{
				// if type is an array, we know that it is a ghost array, because arrays of unloadable are compiled
				// as object (not as arrays of object)
				if(type.IsArray)
				{
					type = GetClassLoader().FieldTypeWrapperFromSig(sigtype, LoadMode.LoadOrThrow);
				}
				else if(type.IsPrimitive)
				{
					type = DotNetTypeWrapper.GetWrapperFromDotNetType(type.TypeAsTBD);
					if(sigtype != type.SigName)
					{
						throw new InvalidOperationException();
					}
				}
				else if(type.IsNonPrimitiveValueType)
				{
					// this can't happen and even if it does happen we cannot return
					// UnloadableTypeWrapper because that would result in incorrect code
					// being generated
					throw new InvalidOperationException();
				}
				else
				{
					if(sigtype[0] == 'L')
					{
						sigtype = sigtype.Substring(1, sigtype.Length - 2);
					}
					try
					{
						TypeWrapper tw = GetClassLoader().LoadClassByDottedNameFast(sigtype);
						if(tw != null && tw.IsRemapped)
						{
							type = tw;
							return;
						}
					}
					catch(RetargetableJavaException)
					{
					}
					type = new UnloadableTypeWrapper(sigtype);
				}
			}
		}

		private static void ParseSig(string sig, out string[] sigparam, out string sigret)
		{
			List<string> list = new List<string>();
			int pos = 1;
			for(;;)
			{
				switch(sig[pos])
				{
					case 'L':
					{
						int end = sig.IndexOf(';', pos) + 1;
						list.Add(sig.Substring(pos, end - pos));
						pos = end;
						break;
					}
					case '[':
					{
						int skip = 1;
						while(sig[pos + skip] == '[') skip++;
						if(sig[pos + skip] == 'L')
						{
							int end = sig.IndexOf(';', pos) + 1;
							list.Add(sig.Substring(pos, end - pos));
							pos = end;
						}
						else
						{
							skip++;
							list.Add(sig.Substring(pos, skip));
							pos += skip;
						}
						break;
					}
					case ')':
						sigparam = list.ToArray();
						sigret = sig.Substring(pos + 1);
						return;
					default:
						list.Add(sig.Substring(pos, 1));
						pos++;
						break;
				}
			}
		}

		private static bool IsCallerID(Type type)
		{
#if STUB_GENERATOR
			return type.FullName == "ikvm.internal.CallerID";
#else
			return type == CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
#endif
		}

		private static bool IsCallerSensitive(MethodBase mb)
		{
#if FIRST_PASS
			return false;
#elif STATIC_COMPILER || STUB_GENERATOR
			foreach (CustomAttributeData cad in mb.GetCustomAttributesData())
			{
				if (cad.AttributeType.FullName == "sun.reflect.CallerSensitiveAttribute")
				{
					return true;
				}
			}
			return false;
#else
			return mb.IsDefined(typeof(sun.reflect.CallerSensitiveAttribute), false);
#endif
		}

		private void GetNameSigFromMethodBase(MethodBase method, out string name, out string sig, out TypeWrapper retType, out TypeWrapper[] paramTypes, ref MemberFlags flags)
		{
			retType = method is ConstructorInfo ? PrimitiveTypeWrapper.VOID : GetParameterTypeWrapper(((MethodInfo)method).ReturnParameter);
			ParameterInfo[] parameters = method.GetParameters();
			int len = parameters.Length;
			if(len > 0
				&& IsCallerID(parameters[len - 1].ParameterType)
				&& GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader()				
				&& IsCallerSensitive(method))
			{
				len--;
				flags |= MemberFlags.CallerID;
			}
			paramTypes = new TypeWrapper[len];
			for(int i = 0; i < len; i++)
			{
				paramTypes[i] = GetParameterTypeWrapper(parameters[i]);
			}
			NameSigAttribute attr = AttributeHelper.GetNameSig(method);
			if(attr != null)
			{
				name = attr.Name;
				sig = attr.Sig;
				string[] sigparams;
				string sigret;
				ParseSig(sig, out sigparams, out sigret);
				// HACK newhelper methods have a return type, but it should be void
				if(name == "<init>")
				{
					retType = PrimitiveTypeWrapper.VOID;
				}
				SigTypePatchUp(sigret, ref retType);
				// if we have a remapped method, the paramTypes array contains an additional entry for "this" so we have
				// to remove that
				if(paramTypes.Length == sigparams.Length + 1)
				{
					paramTypes = ArrayUtil.DropFirst(paramTypes);
				}
				Debug.Assert(sigparams.Length == paramTypes.Length);
				for(int i = 0; i < sigparams.Length; i++)
				{
					SigTypePatchUp(sigparams[i], ref paramTypes[i]);
				}
			}
			else
			{
				if(method is ConstructorInfo)
				{
					name = method.IsStatic ? "<clinit>" : "<init>";
				}
				else
				{
					name = method.Name;
					if(name.StartsWith(NamePrefix.Bridge, StringComparison.Ordinal))
					{
						name = name.Substring(NamePrefix.Bridge.Length);
					}
					if(method.IsSpecialName)
					{
						name = UnicodeUtil.UnescapeInvalidSurrogates(name);
					}
				}
				if(method.IsSpecialName && method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
				{
					paramTypes = ArrayUtil.DropFirst(paramTypes);
				}
				System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
				foreach(TypeWrapper tw in paramTypes)
				{
					sb.Append(tw.SigName);
				}
				sb.Append(")");
				sb.Append(retType.SigName);
				sig = sb.ToString();
			}
		}

		private sealed class DelegateConstructorMethodWrapper : MethodWrapper
		{
			private readonly ConstructorInfo constructor;
			private MethodInfo invoke;

			private DelegateConstructorMethodWrapper(TypeWrapper tw, TypeWrapper iface, ExModifiers mods)
				: base(tw, StringConstants.INIT, "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, mods.Modifiers, mods.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None)
			{
			}

			internal DelegateConstructorMethodWrapper(TypeWrapper tw, MethodBase method)
				: this(tw, tw.GetClassLoader().LoadClassByDottedName(tw.Name + DotNetTypeWrapper.DelegateInterfaceSuffix), AttributeHelper.GetModifiers(method, false))
			{
				constructor = (ConstructorInfo)method;
			}

			protected override void DoLinkMethod()
			{
				MethodWrapper mw = GetParameters()[0].GetMethods()[0];
				mw.Link();
				invoke = (MethodInfo)mw.GetMethod();
			}

#if EMITTERS
			internal override void EmitNewobj(CodeEmitter ilgen)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Ldvirtftn, invoke);
				ilgen.Emit(OpCodes.Newobj, constructor);
			}
#endif // EMITTERS
		}

		protected override void LazyPublishMethods()
		{
			bool isDelegate = type.BaseType == Types.MulticastDelegate;
			List<MethodWrapper> methods = new List<MethodWrapper>();
			const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
			foreach(ConstructorInfo ctor in type.GetConstructors(flags))
			{
				HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(ctor);
				if (isDelegate && !ctor.IsStatic && (hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
				{
					methods.Add(new DelegateConstructorMethodWrapper(this, ctor));
				}
				else
				{
					AddMethodOrConstructor(ctor, hideFromJavaFlags, methods);
				}
			}
			AddMethods(type.GetMethods(flags), methods);
			if (type.IsInterface && (type.IsPublic || type.IsNestedPublic))
			{
				Type privateInterfaceMethods = type.GetNestedType(NestedTypeName.PrivateInterfaceMethods, BindingFlags.NonPublic);
				if (privateInterfaceMethods != null)
				{
					AddMethods(privateInterfaceMethods.GetMethods(flags), methods);
				}
			}
			SetMethods(methods.ToArray());
		}

		private void AddMethods(MethodInfo[] add, List<MethodWrapper> methods)
		{
			foreach (MethodInfo method in add)
			{
				AddMethodOrConstructor(method, AttributeHelper.GetHideFromJavaFlags(method), methods);
			}
		}

		private void AddMethodOrConstructor(MethodBase method, HideFromJavaFlags hideFromJavaFlags, List<MethodWrapper> methods)
		{
			if((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
			{
				if(method.Name.StartsWith(NamePrefix.Incomplete, StringComparison.Ordinal))
				{
					SetHasIncompleteInterfaceImplementation();
				}
			}
			else
			{
				if(method.IsSpecialName && (method.Name.StartsWith("__<", StringComparison.Ordinal) || method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)))
				{
					// skip
				}
				else
				{
					string name;
					string sig;
					TypeWrapper retType;
					TypeWrapper[] paramTypes;
					MethodInfo mi = method as MethodInfo;
					bool hideFromReflection = mi != null && (hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0;
					MemberFlags flags = hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None;
					GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
					ExModifiers mods = AttributeHelper.GetModifiers(method, false);
					if(mods.IsInternal)
					{
						flags |= MemberFlags.InternalAccess;
					}
					if(hideFromReflection && name.StartsWith(NamePrefix.AccessStub, StringComparison.Ordinal))
					{
						int id = Int32.Parse(name.Substring(NamePrefix.AccessStub.Length, name.IndexOf('|', NamePrefix.AccessStub.Length) - NamePrefix.AccessStub.Length));
						name = name.Substring(name.IndexOf('|', NamePrefix.AccessStub.Length) + 1);
						flags |= MemberFlags.AccessStub;
						MethodInfo nonvirt = type.GetMethod(NamePrefix.NonVirtual + id, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
						methods.Add(new AccessStubMethodWrapper(this, name, sig, mi, mi, nonvirt ?? mi, retType, paramTypes, mods.Modifiers & ~Modifiers.Final, flags));
						return;
					}
					MethodInfo impl;
					MethodWrapper mw;
					if (IsGhost && (mods.Modifiers & (Modifiers.Static | Modifiers.Private)) == 0)
					{
						Type[] types = new Type[paramTypes.Length];
						for (int i = 0; i < types.Length; i++)
						{
							types[i] = paramTypes[i].TypeAsSignatureType;
						}
						MethodInfo ifmethod = TypeAsBaseType.GetMethod(method.Name, types);
						mw = new GhostMethodWrapper(this, name, sig, ifmethod, (MethodInfo)method, retType, paramTypes, mods.Modifiers, flags);
						if (!mw.IsAbstract)
						{
							((GhostMethodWrapper)mw).SetDefaultImpl(TypeAsSignatureType.GetMethod(NamePrefix.DefaultMethod + method.Name, types));
						}
					}
					else if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.PrivateInterfaceInstanceMethod, StringComparison.Ordinal))
					{
						mw = new PrivateInterfaceMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
					}
					else if (IsInterface && method.IsAbstract && (mods.Modifiers & Modifiers.Abstract) == 0 && (impl = GetDefaultInterfaceMethodImpl(mi, sig)) != null)
					{
						mw = new DefaultInterfaceMethodWrapper(this, name, sig, mi, impl, retType, paramTypes, mods.Modifiers, flags);
					}
					else
					{
						mw = new TypicalMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
					}
					if (mw.HasNonPublicTypeInSignature)
					{
						if (mi != null)
						{
							MethodInfo stubVirt;
							MethodInfo stubNonVirt;
							if (GetType2AccessStubs(name, sig, out stubVirt, out stubNonVirt))
							{
								mw = new AccessStubMethodWrapper(this, name, sig, mi, stubVirt, stubNonVirt ?? stubVirt, retType, paramTypes, mw.Modifiers, flags);
							}
						}
						else
						{
							ConstructorInfo stub;
							if (GetType2AccessStub(sig, out stub))
							{
								mw = new AccessStubConstructorMethodWrapper(this, sig, (ConstructorInfo)method, stub, paramTypes, mw.Modifiers, flags);
							}
						}
					}
					methods.Add(mw);
				}
			}
		}

		private MethodInfo GetDefaultInterfaceMethodImpl(MethodInfo method, string expectedSig)
		{
			foreach (MethodInfo candidate in method.DeclaringType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
			{
				if (candidate.IsSpecialName
					&& candidate.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)
					&& candidate.Name.Length == method.Name.Length + NamePrefix.DefaultMethod.Length
					&& candidate.Name.EndsWith(method.Name, StringComparison.Ordinal))
				{
					string name;
					string sig;
					TypeWrapper retType;
					TypeWrapper[] paramTypes;
					MemberFlags flags = MemberFlags.None;
					GetNameSigFromMethodBase(candidate, out name, out sig, out retType, out paramTypes, ref flags);
					if (sig == expectedSig)
					{
						return candidate;
					}
				}
			}
			return null;
		}

		private bool GetType2AccessStubs(string name, string sig, out MethodInfo stubVirt, out MethodInfo stubNonVirt)
		{
			stubVirt = null;
			stubNonVirt = null;
			const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
			foreach (MethodInfo method in type.GetMethods(flags))
			{
				if (AttributeHelper.IsHideFromJava(method))
				{
					NameSigAttribute attr = AttributeHelper.GetNameSig(method);
					if (attr != null && attr.Name == name && attr.Sig == sig)
					{
						if (method.Name.StartsWith(NamePrefix.NonVirtual, StringComparison.Ordinal))
						{
							stubNonVirt = method;
						}
						else
						{
							stubVirt = method;
						}
					}
				}
			}
			return stubVirt != null;
		}

		private bool GetType2AccessStub(string sig, out ConstructorInfo stub)
		{
			stub = null;
			const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
			foreach (ConstructorInfo ctor in type.GetConstructors(flags))
			{
				if (AttributeHelper.IsHideFromJava(ctor))
				{
					NameSigAttribute attr = AttributeHelper.GetNameSig(ctor);
					if (attr != null && attr.Sig == sig)
					{
						stub = ctor;
					}
				}
			}
			return stub != null;
		}

		private static int SortFieldByToken(FieldInfo field1, FieldInfo field2)
		{
			return field1.MetadataToken.CompareTo(field2.MetadataToken);
		}

		protected override void LazyPublishFields()
		{
			List<FieldWrapper> fields = new List<FieldWrapper>();
			const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
			FieldInfo[] rawfields = type.GetFields(flags);
			Array.Sort(rawfields, SortFieldByToken);
			// FXBUG on .NET 3.5 and Mono Type.GetProperties() will not return "duplicate" properties (i.e. that have the same name and type, but differ in custom modifiers).
			// .NET 4.0 works as expected. We don't have a workaround, because that would require name mangling again and this situation is very unlikely anyway.
			PropertyInfo[] properties = type.GetProperties(flags);
			foreach(FieldInfo field in rawfields)
			{
				HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
				if((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
				{
					if(field.Name.StartsWith(NamePrefix.Type2AccessStubBackingField, StringComparison.Ordinal))
					{
						TypeWrapper tw = GetFieldTypeWrapper(field);
						string name = field.Name.Substring(NamePrefix.Type2AccessStubBackingField.Length);
						for(int i = 0; i < properties.Length; i++)
						{
							if(properties[i] != null
								&& name == properties[i].Name
								&& MatchTypes(tw, GetPropertyTypeWrapper(properties[i])))
							{
								fields.Add(new CompiledAccessStubFieldWrapper(this, properties[i], field, tw));
								properties[i] = null;
								break;
							}
						}
					}
				}
				else
				{
					if(field.IsSpecialName && field.Name.StartsWith("__<", StringComparison.Ordinal))
					{
						// skip
					}
					else
					{
						fields.Add(CreateFieldWrapper(field, hideFromJavaFlags));
					}
				}
			}
			foreach(PropertyInfo property in properties)
			{
				if(property != null)
				{
					AddPropertyFieldWrapper(fields, property, null);
				}
			}
			SetFields(fields.ToArray());
		}

		private static bool MatchTypes(TypeWrapper tw1, TypeWrapper tw2)
		{
			return tw1 == tw2 || (tw1.IsUnloadable && tw2.IsUnloadable && tw1.Name == tw2.Name);
		}

		private void AddPropertyFieldWrapper(List<FieldWrapper> fields, PropertyInfo property, FieldInfo field)
		{
			// NOTE explictly defined properties (in map.xml) are decorated with HideFromJava,
			// so we don't need to worry about them here
			HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(property);
			if((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
			{
				// is it a type 1 access stub?
				if((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
				{
					fields.Add(new CompiledAccessStubFieldWrapper(this, property, GetPropertyTypeWrapper(property)));
				}
				else
				{
					// It must be an explicit property
					// (defined in Java source by an @ikvm.lang.Property annotation)
					ModifiersAttribute mods = AttributeHelper.GetModifiersAttribute(property);
					fields.Add(new CompiledPropertyFieldWrapper(this, property, new ExModifiers(mods.Modifiers, mods.IsInternal)));
				}
			}
		}

		private sealed class CompiledRemappedMethodWrapper : SmartMethodWrapper
		{
			private readonly MethodInfo mbHelper;
#if !STATIC_COMPILER
			private readonly MethodInfo mbNonvirtualHelper;
#endif

			internal CompiledRemappedMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, ExModifiers modifiers, bool hideFromReflection, MethodInfo mbHelper, MethodInfo mbNonvirtualHelper)
				: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers.Modifiers,
						(modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None))
			{
				this.mbHelper = mbHelper;
#if !STATIC_COMPILER
				this.mbNonvirtualHelper = mbNonvirtualHelper;
#endif
			}

#if EMITTERS
			protected override void CallImpl(CodeEmitter ilgen)
			{
				MethodBase mb = GetMethod();
				MethodInfo mi = mb as MethodInfo;
				if(mi != null)
				{
					if(!IsStatic && IsFinal)
					{
						// When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
						// then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
						// is final, this won't affect the semantics.
						CallvirtImpl(ilgen);
					}
					else
					{
						ilgen.Emit(OpCodes.Call, mi);
					}
				}
				else
				{
					ilgen.Emit(OpCodes.Call, mb);
				}
			}

			protected override void CallvirtImpl(CodeEmitter ilgen)
			{
				Debug.Assert(!mbHelper.IsStatic || mbHelper.Name.StartsWith("instancehelper_") || mbHelper.DeclaringType.Name == "__Helper");
				if(mbHelper.IsPublic)
				{
					ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
				}
				else
				{
					// HACK the helper is not public, this means that we're dealing with finalize or clone
					ilgen.Emit(OpCodes.Callvirt, GetMethod());
				}
			}

			protected override void NewobjImpl(CodeEmitter ilgen)
			{
				MethodBase mb = GetMethod();
				MethodInfo mi = mb as MethodInfo;
				if(mi != null)
				{
					Debug.Assert(mi.Name == "newhelper");
					ilgen.Emit(OpCodes.Call, mi);
				}
				else
				{
					ilgen.Emit(OpCodes.Newobj, mb);
				}
			}
#endif // EMITTERS

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
			[HideFromJava]
			internal override object Invoke(object obj, object[] args)
			{
				MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
				if (mb.IsStatic && !IsStatic)
				{
					args = ArrayUtil.Concat(obj, args);
					obj = null;
				}
				return InvokeAndUnwrapException(mb, obj, args);
			}

			[HideFromJava]
			internal override object CreateInstance(object[] args)
			{
				MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
				if (mb.IsStatic)
				{
					return InvokeAndUnwrapException(mb, null, args);
				}
				return base.CreateInstance(args);
			}

			[HideFromJava]
			internal override object InvokeNonvirtualRemapped(object obj, object[] args)
			{
				MethodInfo mi = mbNonvirtualHelper;
				if (mi == null)
				{
					mi = mbHelper;
				}
				return mi.Invoke(null, ArrayUtil.Concat(obj, args));
			}
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR

#if EMITTERS
			internal override void EmitCallvirtReflect(CodeEmitter ilgen)
			{
				MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
				ilgen.Emit(mb.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mb);
			}
#endif // EMITTERS

			internal string GetGenericSignature()
			{
				SignatureAttribute attr = AttributeHelper.GetSignature(mbHelper != null ? mbHelper : GetMethod());
				if(attr != null)
				{
					return attr.Signature;
				}
				return null;
			}
		}

		private static TypeWrapper TypeWrapperFromModOpt(Type[] modopt)
		{
			int rank = 0;
			TypeWrapper tw = null;
			foreach (Type type in modopt)
			{
				if (type == JVM.LoadType(typeof(IKVM.Attributes.AccessStub)))
				{
					// ignore
				}
				else if (type == Types.Array)
				{
					rank++;
				}
				else if (type == Types.Void || type.IsPrimitive || ClassLoaderWrapper.IsRemappedType(type))
				{
					tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
				}
				else if (type.DeclaringType != null && type.DeclaringType.FullName == UnloadableTypeWrapper.ContainerTypeName)
				{
					tw = new UnloadableTypeWrapper(TypeNameUtil.UnmangleNestedTypeName(type.Name), type);
				}
				else
				{
					tw = ClassLoaderWrapper.GetWrapperFromType(type);
				}
			}
			if (rank != 0)
			{
				tw = tw.MakeArrayType(rank);
			}
			return tw;
		}

		private static TypeWrapper GetPropertyTypeWrapper(PropertyInfo property)
		{
			return TypeWrapperFromModOpt(property.GetOptionalCustomModifiers())
				?? ClassLoaderWrapper.GetWrapperFromType(property.PropertyType);
		}

		internal static TypeWrapper GetFieldTypeWrapper(FieldInfo field)
		{
			return TypeWrapperFromModOpt(field.GetOptionalCustomModifiers())
				?? ClassLoaderWrapper.GetWrapperFromType(field.FieldType);
		}

		internal static TypeWrapper GetParameterTypeWrapper(ParameterInfo param)
		{
			TypeWrapper tw = TypeWrapperFromModOpt(param.GetOptionalCustomModifiers());
			if (tw != null)
			{
				return tw;
			}
			Type parameterType = param.ParameterType;
			if (parameterType.IsByRef)
			{
				// we only support ByRef parameters for automatically generated delegate invoke stubs
				parameterType = parameterType.GetElementType().MakeArrayType();
			}
			return ClassLoaderWrapper.GetWrapperFromType(parameterType);
		}

		private FieldWrapper CreateFieldWrapper(FieldInfo field, HideFromJavaFlags hideFromJavaFlags)
		{
			ExModifiers modifiers = AttributeHelper.GetModifiers(field, false);
			TypeWrapper type = GetFieldTypeWrapper(field);
			string name = field.Name;

			if(field.IsSpecialName)
			{
				name = UnicodeUtil.UnescapeInvalidSurrogates(name);
			}

			if(field.IsLiteral)
			{
				MemberFlags flags = MemberFlags.None;
				if((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
				{
					flags |= MemberFlags.HideFromReflection;
				}
				if(modifiers.IsInternal)
				{
					flags |= MemberFlags.InternalAccess;
				}
				return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers.Modifiers, field, null, flags);
			}
			else
			{
				return FieldWrapper.Create(this, type, field, name, type.SigName, modifiers);
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return type;
			}
		}

		internal override bool IsMapUnsafeException
		{
			get
			{
				return AttributeHelper.IsExceptionIsUnsafeForMapping(type);
			}
		}

#if EMITTERS
		internal override void EmitRunClassConstructor(CodeEmitter ilgen)
		{
			if(HasStaticInitializer)
			{
				ilgen.Emit(OpCodes.Call, clinitMethod);
			}
		}
#endif // EMITTERS

		internal override string GetGenericSignature()
		{
			SignatureAttribute attr = AttributeHelper.GetSignature(type);
			if(attr != null)
			{
				return attr.Signature;
			}
			return null;
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			if(mw is CompiledRemappedMethodWrapper)
			{
				return ((CompiledRemappedMethodWrapper)mw).GetGenericSignature();
			}
			MethodBase mb = mw.GetMethod();
			if(mb != null)
			{
				SignatureAttribute attr = AttributeHelper.GetSignature(mb);
				if(attr != null)
				{
					return attr.Signature;
				}
			}
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			FieldInfo fi = fw.GetField();
			if(fi != null)
			{
				SignatureAttribute attr = AttributeHelper.GetSignature(fi);
				if(attr != null)
				{
					return attr.Signature;
				}
			}
			return null;
		}

		internal override MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if (mb == null)
			{
				// delegate constructor
				return null;
			}
			MethodParametersAttribute attr = AttributeHelper.GetMethodParameters(mb);
			if (attr == null)
			{
				return null;
			}
			if (attr.IsMalformed)
			{
				return MethodParametersEntry.Malformed;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			MethodParametersEntry[] mp = new MethodParametersEntry[attr.Modifiers.Length];
			for (int i = 0; i < mp.Length; i++)
			{
				mp[i].name = i < parameters.Length ? parameters[i].Name : null;
				mp[i].flags = (ushort)attr.Modifiers[i];
			}
			return mp;
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal override string[] GetEnclosingMethod()
		{
			EnclosingMethodAttribute enc = AttributeHelper.GetEnclosingMethodAttribute(type);
			if (enc != null)
			{
				return new string[] { enc.ClassName, enc.MethodName, enc.MethodSignature };
			}
			return null;
		}

		internal override object[] GetDeclaredAnnotations()
		{
			return type.GetCustomAttributes(false);
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb == null)
			{
				// delegate constructor
				return null;
			}
			return mb.GetCustomAttributes(false);
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb == null)
			{
				// delegate constructor
				return null;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			int skip = 0;
			if(mb.IsStatic && !mw.IsStatic && mw.Name != "<init>")
			{
				skip = 1;
			}
			int skipEnd = 0;
			if(mw.HasCallerID)
			{
				skipEnd = 1;
			}
			object[][] attribs = new object[parameters.Length - skip - skipEnd][];
			for(int i = skip; i < parameters.Length - skipEnd; i++)
			{
				attribs[i - skip] = parameters[i].GetCustomAttributes(false);
			}
			return attribs;
		}

		internal override object[] GetFieldAnnotations(FieldWrapper fw)
		{
			FieldInfo field = fw.GetField();
			if(field != null)
			{
				return field.GetCustomAttributes(false);
			}
			CompiledPropertyFieldWrapper prop = fw as CompiledPropertyFieldWrapper;
			if(prop != null)
			{
				return prop.GetProperty().GetCustomAttributes(false);
			}
			return new object[0];
		}
#endif

		internal sealed class CompiledAnnotation : Annotation
		{
			private readonly ConstructorInfo constructor;

			internal CompiledAnnotation(Type type)
			{
				constructor = type.GetConstructor(new Type[] { JVM.Import(typeof(object[])) });
			}

			private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
			{
				return new CustomAttributeBuilder(constructor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
			}

			internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
			{
				tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
			{
				mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
			{
				fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
			{
				pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
			{
				ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
			{
				pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
			}

			internal override bool IsCustomAttribute
			{
				get { return false; }
			}
		}

		internal override Annotation Annotation
		{
			get
			{
				string annotationAttribute = AttributeHelper.GetAnnotationAttributeType(type);
				if(annotationAttribute != null)
				{
					return new CompiledAnnotation(type.Assembly.GetType(annotationAttribute, true));
				}
				return null;
			}
		}

		internal override Type EnumType
		{
			get
			{
				if((this.Modifiers & Modifiers.Enum) != 0)
				{
					return type.GetNestedType("__Enum");
				}
				return null;
			}
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal override string GetSourceFileName()
		{
			object[] attr = type.GetCustomAttributes(typeof(SourceFileAttribute), false);
			if(attr.Length == 1)
			{
				return ((SourceFileAttribute)attr[0]).SourceFile;
			}
			if(DeclaringTypeWrapper != null)
			{
				return DeclaringTypeWrapper.GetSourceFileName();
			}
			if(IsNestedTypeAnonymousOrLocalClass(type))
			{
				return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetSourceFileName();
			}
			if(type.Module.IsDefined(typeof(SourceFileAttribute), false))
			{
				return type.Name + ".java";
			}
			return null;
		}

		internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
		{
			object[] attr = mb.GetCustomAttributes(typeof(LineNumberTableAttribute), false);
			if(attr.Length == 1)
			{
				return ((LineNumberTableAttribute)attr[0]).GetLineNumber(ilOffset);
			}
			return -1;
		}
#endif

		internal override bool IsFastClassLiteralSafe
		{
			get { return true; }
		}

		internal override object[] GetConstantPool()
		{
			return AttributeHelper.GetConstantPool(type);
		}

		internal override byte[] GetRawTypeAnnotations()
		{
			return AttributeHelper.GetRuntimeVisibleTypeAnnotations(type);
		}

		internal override byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			return mb == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(mb);
		}

		internal override byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
		{
			FieldInfo fi = fw.GetField();
			return fi == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(fi);
		}
	}

	sealed class ArrayTypeWrapper : TypeWrapper
	{
		private static volatile TypeWrapper[] interfaces;
		private static volatile MethodInfo clone;
		private readonly TypeWrapper ultimateElementTypeWrapper;
		private Type arrayType;
		private bool finished;

		internal ArrayTypeWrapper(TypeWrapper ultimateElementTypeWrapper, string name)
			: base(ultimateElementTypeWrapper.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None,
				Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.Modifiers & Modifiers.Public), name)
		{
			Debug.Assert(!ultimateElementTypeWrapper.IsArray);
			this.ultimateElementTypeWrapper = ultimateElementTypeWrapper;
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get { return CoreClasses.java.lang.Object.Wrapper; }
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return ultimateElementTypeWrapper.GetClassLoader();
		}

		internal static MethodInfo CloneMethod
		{
			get
			{
				if(clone == null)
				{
					clone = Types.Array.GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
				}
				return clone;
			}
		}

		protected override void LazyPublishMembers()
		{
			MethodWrapper mw = new SimpleCallMethodWrapper(this, "clone", "()Ljava.lang.Object;", CloneMethod, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray, Modifiers.Public, MemberFlags.HideFromReflection, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt);
			mw.Link();
			SetMethods(new MethodWrapper[] { mw });
			SetFields(FieldWrapper.EmptyArray);
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				return Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.ReflectiveModifiers & Modifiers.AccessMask);
			}
		}

		internal override string SigName
		{
			get
			{
				// for arrays the signature name is the same as the normal name
				return Name;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				if(interfaces == null)
				{
					TypeWrapper[] tw = new TypeWrapper[2];
					tw[0] = CoreClasses.java.lang.Cloneable.Wrapper;
					tw[1] = CoreClasses.java.io.Serializable.Wrapper;
					interfaces = tw;
				}
				return interfaces;
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				while (arrayType == null)
				{
					bool prevFinished = finished;
					Type type = MakeArrayType(ultimateElementTypeWrapper.TypeAsArrayType, this.ArrayRank);
					if (prevFinished)
					{
						// We were already finished prior to the call to MakeArrayType, so we can safely
						// set arrayType to the finished type.
						// Note that this takes advantage of the fact that once we've been finished,
						// we can never become unfinished.
						arrayType = type;
					}
					else
					{
						lock (this)
						{
							// To prevent a race with Finish, we can only set arrayType in this case
							// (inside the locked region) if we've not already finished. If we have
							// finished, we need to rerun MakeArrayType on the now finished element type.
							// Note that there is a benign race left, because it is possible that another
							// thread finishes right after we've set arrayType and exited the locked
							// region. This is not problem, because TypeAsTBD is only guaranteed to
							// return a finished type *after* Finish has been called.
							if (!finished)
							{
								arrayType = type;
							}
						}
					}
				}
				return arrayType;
			}
		}

		internal override void Finish()
		{
			if (!finished)
			{
				ultimateElementTypeWrapper.Finish();
				lock (this)
				{
					// Now that we've finished the element type, we must clear arrayType,
					// because it may still refer to a TypeBuilder. Note that we have to
					// do this atomically with setting "finished", to prevent a race
					// with TypeAsTBD.
					finished = true;
					arrayType = null;
				}
			}
		}

		internal override bool IsFastClassLiteralSafe
		{
			// here we have to deal with the somewhat strange fact that in Java you cannot represent primitive type class literals,
			// but you can represent arrays of primitive types as a class literal
			get { return ultimateElementTypeWrapper.IsFastClassLiteralSafe || ultimateElementTypeWrapper.IsPrimitive; }
		}

		internal override TypeWrapper GetUltimateElementTypeWrapper()
		{
			return ultimateElementTypeWrapper;
		}

		internal static Type MakeArrayType(Type type, int dims)
		{
			// NOTE this is not just an optimization, but it is also required to
			// make sure that ReflectionOnly types stay ReflectionOnly types
			// (in particular instantiations of generic types from mscorlib that
			// have ReflectionOnly type parameters).
			for(int i = 0; i < dims; i++)
			{
				type = type.MakeArrayType();
			}
			return type;
		}
	}

	// this is a container for the special verifier TypeWrappers
	sealed class VerifierTypeWrapper : TypeWrapper
	{
		// the TypeWrapper constructor interns the name, so we have to pre-intern here to make sure we have the same string object
		// (if it has only been interned previously)
		private static readonly string This = string.Intern("this");
		private static readonly string New = string.Intern("new");
		private static readonly string Fault = string.Intern("<fault>");
		internal static readonly TypeWrapper Invalid = null;
		internal static readonly TypeWrapper Null = new VerifierTypeWrapper("null", 0, null, null);
		internal static readonly TypeWrapper UninitializedThis = new VerifierTypeWrapper("uninitialized-this", 0, null, null);
		internal static readonly TypeWrapper Unloadable = new UnloadableTypeWrapper("<verifier>");
		internal static readonly TypeWrapper ExtendedFloat = new VerifierTypeWrapper("<extfloat>", 0, null, null);
		internal static readonly TypeWrapper ExtendedDouble = new VerifierTypeWrapper("<extdouble>", 0, null, null);

		private readonly int index;
		private readonly TypeWrapper underlyingType;
		private readonly MethodAnalyzer methodAnalyzer;

#if STUB_GENERATOR
		internal class MethodAnalyzer
		{
			internal void ClearFaultBlockException(int dummy) { }
		}
#endif

		public override string ToString()
		{
			return GetType().Name + "[" + Name + "," + index + "," + underlyingType + "]";
		}

		internal static TypeWrapper MakeNew(TypeWrapper type, int bytecodeIndex)
		{
			return new VerifierTypeWrapper(New, bytecodeIndex, type, null);
		}

		internal static TypeWrapper MakeFaultBlockException(MethodAnalyzer ma, int handlerIndex)
		{
			return new VerifierTypeWrapper(Fault, handlerIndex, null, ma);
		}

		// NOTE the "this" type is special, it can only exist in local[0] and on the stack
		// as soon as the type on the stack is merged or popped it turns into its underlying type.
		// It exists to capture the verification rules for non-virtual base class method invocation in .NET 2.0,
		// which requires that the invocation is done on a "this" reference that was directly loaded onto the
		// stack (using ldarg_0).
		internal static TypeWrapper MakeThis(TypeWrapper type)
		{
			return new VerifierTypeWrapper(This, 0, type, null);
		}

		internal static bool IsNotPresentOnStack(TypeWrapper w)
		{
			return IsNew(w) || IsFaultBlockException(w);
		}

		internal static bool IsNew(TypeWrapper w)
		{
			return w != null && w.IsVerifierType && ReferenceEquals(w.Name, New);
		}

		internal static bool IsFaultBlockException(TypeWrapper w)
		{
			return w != null && w.IsVerifierType && ReferenceEquals(w.Name, Fault);
		}

		internal static bool IsNullOrUnloadable(TypeWrapper w)
		{
			return w == Null || w.IsUnloadable;
		}

		internal static bool IsThis(TypeWrapper w)
		{
			return w != null && w.IsVerifierType && ReferenceEquals(w.Name, This);
		}

		internal static void ClearFaultBlockException(TypeWrapper w)
		{
			VerifierTypeWrapper vtw = (VerifierTypeWrapper)w;
			vtw.methodAnalyzer.ClearFaultBlockException(vtw.Index);
		}

		internal int Index
		{
			get
			{
				return index;
			}
		}

		internal TypeWrapper UnderlyingType
		{
			get
			{
				return underlyingType;
			}
		}

		private VerifierTypeWrapper(string name, int index, TypeWrapper underlyingType, MethodAnalyzer methodAnalyzer)
			: base(TypeFlags.None, TypeWrapper.VerifierTypeModifiersHack, name)
		{
			this.index = index;
			this.underlyingType = underlyingType;
			this.methodAnalyzer = methodAnalyzer;
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get { return null; }
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return null;
		}

		protected override void LazyPublishMembers()
		{
			throw new InvalidOperationException("LazyPublishMembers called on " + this);
		}

		internal override Type TypeAsTBD
		{
			get
			{
				throw new InvalidOperationException("get_Type called on " + this);
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				throw new InvalidOperationException("get_Interfaces called on " + this);
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				throw new InvalidOperationException("get_InnerClasses called on " + this);
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				throw new InvalidOperationException("get_DeclaringTypeWrapper called on " + this);
			}
		}

		internal override void Finish()
		{
			throw new InvalidOperationException("Finish called on " + this);
		}
	}

#if !STATIC_COMPILER && !STUB_GENERATOR
	// this represents an intrinsified anonymous class (currently used only by LambdaMetafactory)
	sealed class AnonymousTypeWrapper : TypeWrapper
	{
		private readonly Type type;

		internal AnonymousTypeWrapper(Type type)
			: base(TypeFlags.Anonymous, Modifiers.Final | Modifiers.Synthetic, GetName(type))
		{
			this.type = type;
		}

		internal static bool IsAnonymous(Type type)
		{
			return type.IsSpecialName
				&& type.Name.StartsWith(NestedTypeName.IntrinsifiedAnonymousClass, StringComparison.Ordinal)
				&& AttributeHelper.IsJavaModule(type.Module);
		}

		private static string GetName(Type type)
		{
			return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).Name
				+ type.Name.Replace(NestedTypeName.IntrinsifiedAnonymousClass, "$$Lambda$");
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetClassLoader();
		}

		internal override Type TypeAsTBD
		{
			get { return type; }
		}

		internal override TypeWrapper BaseTypeWrapper
		{
			get { return CoreClasses.java.lang.Object.Wrapper; }
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				TypeWrapper[] interfaces = GetImplementedInterfacesAsTypeWrappers(type);
				if (type.IsSerializable)
				{
					// we have to remove the System.Runtime.Serialization.ISerializable interface
					List<TypeWrapper> list = new List<TypeWrapper>(interfaces);
					list.RemoveAll(Serialization.IsISerializable);
					return list.ToArray();
				}
				return interfaces;
			}
		}

		protected override void LazyPublishMembers()
		{
			List<MethodWrapper> methods = new List<MethodWrapper>();
			foreach (MethodInfo mi in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				if (mi.IsSpecialName)
				{
					// we use special name to hide default methods
				}
				else if (mi.IsPublic)
				{
					TypeWrapper returnType;
					TypeWrapper[] parameterTypes;
					string signature;
					GetSig(mi, out returnType, out parameterTypes, out signature);
					methods.Add(new TypicalMethodWrapper(this, mi.Name, signature, mi, returnType, parameterTypes, Modifiers.Public, MemberFlags.None));
				}
				else if (mi.Name == "writeReplace")
				{
					methods.Add(new TypicalMethodWrapper(this, "writeReplace", "()Ljava.lang.Object;", mi, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray,
						Modifiers.Private | Modifiers.Final, MemberFlags.None));
				}
			}
			SetMethods(methods.ToArray());
			List<FieldWrapper> fields = new List<FieldWrapper>();
			foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
			{
				TypeWrapper fieldType = CompiledTypeWrapper.GetFieldTypeWrapper(fi);
				fields.Add(new SimpleFieldWrapper(this, fieldType, fi, fi.Name, fieldType.SigName, new ExModifiers(Modifiers.Private | Modifiers.Final, false)));
			}
			SetFields(fields.ToArray());
		}

		private void GetSig(MethodInfo mi, out TypeWrapper returnType, out TypeWrapper[] parameterTypes, out string signature)
		{
			returnType = CompiledTypeWrapper.GetParameterTypeWrapper(mi.ReturnParameter);
			ParameterInfo[] parameters = mi.GetParameters();
			parameterTypes = new TypeWrapper[parameters.Length];
			System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
			for (int i = 0; i < parameters.Length; i++)
			{
				parameterTypes[i] = CompiledTypeWrapper.GetParameterTypeWrapper(parameters[i]);
				sb.Append(parameterTypes[i].SigName);
			}
			sb.Append(')');
			sb.Append(returnType.SigName);
			signature = sb.ToString();
		}
	}
#endif // !STATIC_COMPILER && !STUB_GENERATOR
}
