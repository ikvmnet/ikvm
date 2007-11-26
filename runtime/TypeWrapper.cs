/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006, 2007 Jeroen Frijters

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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if !COMPACT_FRAMEWORK
using System.Reflection.Emit;
using ILGenerator = IKVM.Internal.CountingILGenerator;
using Label = IKVM.Internal.CountingLabel;
#endif
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using IKVM.Attributes;


namespace IKVM.Internal
{
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

#if !COMPACT_FRAMEWORK
	class EmitHelper
	{
		private static MethodInfo objectToString = typeof(object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
		private static MethodInfo verboseCastFailure = JVM.SafeGetEnvironmentVariable("IKVM_VERBOSE_CAST") == null ? null : ByteCodeHelperMethods.VerboseCastFailure;

		static EmitHelper() {}

		internal static void Throw(ILGenerator ilgen, string dottedClassName)
		{
			TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dottedClassName);
			MethodWrapper mw = exception.GetMethodWrapper("<init>", "()V", false);
			mw.Link();
			mw.EmitNewobj(ilgen);
			ilgen.Emit(OpCodes.Throw);
		}

		internal static void Throw(ILGenerator ilgen, string dottedClassName, string message)
		{
			TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dottedClassName);
			ilgen.Emit(OpCodes.Ldstr, message);
			MethodWrapper mw = exception.GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
			mw.Link();
			mw.EmitNewobj(ilgen);
			ilgen.Emit(OpCodes.Throw);
		}

		internal static void NullCheck(ILGenerator ilgen)
		{
			// I think this is the most efficient way to generate a NullReferenceException if the
			// reference is null
			ilgen.Emit(OpCodes.Ldvirtftn, objectToString);
			ilgen.Emit(OpCodes.Pop);
		}

		internal static void Castclass(ILGenerator ilgen, Type type)
		{
			if(verboseCastFailure != null)
			{
				LocalBuilder lb = ilgen.DeclareLocal(typeof(object));
				ilgen.Emit(OpCodes.Stloc, lb);
				ilgen.Emit(OpCodes.Ldloc, lb);
				ilgen.Emit(OpCodes.Isinst, type);
				ilgen.Emit(OpCodes.Dup);
				Label ok = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue_S, ok);
				ilgen.Emit(OpCodes.Ldloc, lb);
				ilgen.Emit(OpCodes.Brfalse_S, ok);	// handle null
				ilgen.Emit(OpCodes.Ldtoken, type);
				ilgen.Emit(OpCodes.Ldloc, lb);
				ilgen.Emit(OpCodes.Call, verboseCastFailure);
				ilgen.MarkLabel(ok);
			}
			else
			{
				ilgen.Emit(OpCodes.Castclass, type);
			}
		}

		// This is basically the same as Castclass, except that it
		// throws an IncompatibleClassChangeError on failure.
		internal static void EmitAssertType(ILGenerator ilgen, Type type)
		{
			LocalBuilder lb = ilgen.DeclareLocal(typeof(object));
			ilgen.Emit(OpCodes.Stloc, lb);
			ilgen.Emit(OpCodes.Ldloc, lb);
			ilgen.Emit(OpCodes.Isinst, type);
			ilgen.Emit(OpCodes.Dup);
			Label ok = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Brtrue_S, ok);
			ilgen.Emit(OpCodes.Ldloc, lb);
			ilgen.Emit(OpCodes.Brfalse_S, ok);	// handle null
			EmitHelper.Throw(ilgen, "java.lang.IncompatibleClassChangeError");
			ilgen.MarkLabel(ok);
		}
	}
#endif

	class AttributeHelper
	{
#if !COMPACT_FRAMEWORK
		private static CustomAttributeBuilder hideFromJavaAttribute;
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
		private static CustomAttributeBuilder paramArrayAttribute;
		private static ConstructorInfo nonNestedInnerClassAttribute;
		private static ConstructorInfo nonNestedOuterClassAttribute;
#endif // STATIC_COMPILER
#endif // !COMPACT_FRAMEWORK
		private static Type typeofRemappedClassAttribute = JVM.LoadType(typeof(RemappedClassAttribute));
		private static Type typeofRemappedTypeAttribute = JVM.LoadType(typeof(RemappedTypeAttribute));
		private static Type typeofModifiersAttribute = JVM.LoadType(typeof(ModifiersAttribute));
		private static Type typeofModifiers = JVM.LoadType(typeof(Modifiers));
		private static Type typeofRemappedInterfaceMethodAttribute = JVM.LoadType(typeof(RemappedInterfaceMethodAttribute));
		private static Type typeofNameSigAttribute = JVM.LoadType(typeof(NameSigAttribute));
		private static Type typeofJavaModuleAttribute = JVM.LoadType(typeof(JavaModuleAttribute));
		private static Type typeofSourceFileAttribute = JVM.LoadType(typeof(SourceFileAttribute));
		private static Type typeofLineNumberTableAttribute = JVM.LoadType(typeof(LineNumberTableAttribute));
		private static Type typeofEnclosingMethodAttribute = JVM.LoadType(typeof(EnclosingMethodAttribute));
		private static Type typeofSignatureAttribute = JVM.LoadType(typeof(SignatureAttribute));
		private static Type typeofInnerClassAttribute = JVM.LoadType(typeof(InnerClassAttribute));
		private static Type typeofImplementsAttribute = JVM.LoadType(typeof(ImplementsAttribute));
		private static Type typeofGhostInterfaceAttribute = JVM.LoadType(typeof(GhostInterfaceAttribute));
		private static Type typeofExceptionIsUnsafeForMappingAttribute = JVM.LoadType(typeof(ExceptionIsUnsafeForMappingAttribute));
		private static Type typeofThrowsAttribute = JVM.LoadType(typeof(ThrowsAttribute));
		private static Type typeofHideFromReflectionAttribute = JVM.LoadType(typeof(HideFromReflectionAttribute));
		private static Type typeofHideFromJavaAttribute = JVM.LoadType(typeof(HideFromJavaAttribute));
		private static Type typeofNoPackagePrefixAttribute = JVM.LoadType(typeof(NoPackagePrefixAttribute));
		private static Type typeofConstantValueAttribute = JVM.LoadType(typeof(ConstantValueAttribute));
		private static Type typeofAnnotationAttributeAttribute = JVM.LoadType(typeof(AnnotationAttributeAttribute));
		private static Type typeofNonNestedInnerClassAttribute = JVM.LoadType(typeof(NonNestedInnerClassAttribute));
		private static Type typeofNonNestedOuterClassAttribute = JVM.LoadType(typeof(NonNestedOuterClassAttribute));

#if STATIC_COMPILER && !COMPACT_FRAMEWORK
		private static object ParseValue(TypeWrapper tw, string val)
		{
			if(tw == CoreClasses.java.lang.String.Wrapper)
			{
				return val;
			}
			else if(tw.TypeAsTBD.IsEnum)
			{
				if(tw.TypeAsTBD.Assembly.ReflectionOnly)
				{
					// TODO implement full parsing semantics
					FieldInfo field = tw.TypeAsTBD.GetField(val);
					if(field == null)
					{
						throw new NotImplementedException("Parsing enum value: " + val);
					}
					return field.GetRawConstantValue();
				}
				return Enum.Parse(tw.TypeAsTBD, val);
			}
			else if(tw.TypeAsTBD == typeof(Type))
			{
				TypeWrapper valtw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(val);
				if(valtw != null)
				{
					return valtw.TypeAsBaseType;
				}
				return Type.GetType(val, true);
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

		private static void SetPropertiesAndFields(Attribute attrib, IKVM.Internal.MapXml.Attribute attr)
		{
			Type t = attrib.GetType();
			if(attr.Properties != null)
			{
				foreach(IKVM.Internal.MapXml.Param prop in attr.Properties)
				{
					PropertyInfo pi = t.GetProperty(prop.Name);
					pi.SetValue(attrib, ParseValue(ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), prop.Sig), prop.Value), null);
				}
			}
			if(attr.Fields != null)
			{
				foreach(IKVM.Internal.MapXml.Param field in attr.Fields)
				{
					FieldInfo fi = t.GetField(field.Name);
					fi.SetValue(attrib, ParseValue(ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), field.Sig), field.Value));
				}
			}
		}

		internal static Attribute InstantiatePseudoCustomAttribute(IKVM.Internal.MapXml.Attribute attr)
		{
			Type t = StaticCompiler.GetType(attr.Type);
			Type[] argTypes;
			object[] args;
			GetAttributeArgsAndTypes(attr, out argTypes, out args);
			ConstructorInfo ci = t.GetConstructor(argTypes);
			Attribute attrib = ci.Invoke(args) as Attribute;
			SetPropertiesAndFields(attrib, attr);
			return attrib;
		}

		private static bool IsCodeAccessSecurityAttribute(IKVM.Internal.MapXml.Attribute attr, out SecurityAction action, out PermissionSet pset)
		{
			action = SecurityAction.Deny;
			pset = null;
			if(attr.Type != null)
			{
				Type t = StaticCompiler.GetType(attr.Type);
				if(typeof(CodeAccessSecurityAttribute).IsAssignableFrom(t))
				{
					Type[] argTypes;
					object[] args;
					GetAttributeArgsAndTypes(attr, out argTypes, out args);
					ConstructorInfo ci = t.GetConstructor(argTypes);
					CodeAccessSecurityAttribute attrib = ci.Invoke(args) as CodeAccessSecurityAttribute;
					SetPropertiesAndFields(attrib, attr);
					action = attrib.Action;
					pset = new PermissionSet(PermissionState.None);
					pset.AddPermission(attrib.CreatePermission());
					return true;
				}
			}
			return false;
		}

		internal static void SetCustomAttribute(TypeBuilder tb, IKVM.Internal.MapXml.Attribute attr)
		{
			SecurityAction action;
			PermissionSet pset;
			if(IsCodeAccessSecurityAttribute(attr, out action, out pset))
			{
				tb.AddDeclarativeSecurity(action, pset);
			}
			else
			{
				tb.SetCustomAttribute(CreateCustomAttribute(attr));
			}
		}

		internal static void SetCustomAttribute(FieldBuilder fb, IKVM.Internal.MapXml.Attribute attr)
		{
			fb.SetCustomAttribute(CreateCustomAttribute(attr));
		}

		internal static void SetCustomAttribute(ParameterBuilder pb, IKVM.Internal.MapXml.Attribute attr)
		{
			pb.SetCustomAttribute(CreateCustomAttribute(attr));
		}

		internal static void SetCustomAttribute(MethodBuilder mb, IKVM.Internal.MapXml.Attribute attr)
		{
			SecurityAction action;
			PermissionSet pset;
			if(IsCodeAccessSecurityAttribute(attr, out action, out pset))
			{
				mb.AddDeclarativeSecurity(action, pset);
			}
			else
			{
				mb.SetCustomAttribute(CreateCustomAttribute(attr));
			}
		}

		internal static void SetCustomAttribute(ConstructorBuilder cb, IKVM.Internal.MapXml.Attribute attr)
		{
			SecurityAction action;
			PermissionSet pset;
			if(IsCodeAccessSecurityAttribute(attr, out action, out pset))
			{
				cb.AddDeclarativeSecurity(action, pset);
			}
			else
			{
				cb.SetCustomAttribute(CreateCustomAttribute(attr));
			}
		}

		internal static void SetCustomAttribute(PropertyBuilder pb, IKVM.Internal.MapXml.Attribute attr)
		{
			pb.SetCustomAttribute(CreateCustomAttribute(attr));
		}

		internal static void SetCustomAttribute(AssemblyBuilder ab, IKVM.Internal.MapXml.Attribute attr)
		{
			ab.SetCustomAttribute(CreateCustomAttribute(attr));
		}

		private static void GetAttributeArgsAndTypes(IKVM.Internal.MapXml.Attribute attr, out Type[] argTypes, out object[] args)
		{
			// TODO add error handling
			TypeWrapper[] twargs = ClassFile.ArgTypeWrapperListFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Sig);
			argTypes = new Type[twargs.Length];
			args = new object[argTypes.Length];
			for(int i = 0; i < twargs.Length; i++)
			{
				argTypes[i] = twargs[i].TypeAsSignatureType;
				TypeWrapper tw = twargs[i];
				if(tw == CoreClasses.java.lang.Object.Wrapper)
				{
					tw = ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Params[i].Sig);
				}
				if(tw.IsArray)
				{
					Array arr = Array.CreateInstance(tw.ElementTypeWrapper.TypeAsArrayType, attr.Params[i].Elements.Length);
					for(int j = 0; j < arr.Length; j++)
					{
						arr.SetValue(ParseValue(tw.ElementTypeWrapper, attr.Params[i].Elements[j].Value), j);
					}
					args[i] = arr;
				}
				else
				{
					args[i] = ParseValue(tw, attr.Params[i].Value);
				}
			}
		}

		private static CustomAttributeBuilder CreateCustomAttribute(IKVM.Internal.MapXml.Attribute attr)
		{
			// TODO add error handling
			Type[] argTypes;
			object[] args;
			GetAttributeArgsAndTypes(attr, out argTypes, out args);
			if(attr.Type != null)
			{
				Type t = StaticCompiler.GetType(attr.Type);
				if(typeof(CodeAccessSecurityAttribute).IsAssignableFrom(t))
				{
					throw new NotImplementedException("CodeAccessSecurityAttribute support not implemented");
				}
				ConstructorInfo ci = t.GetConstructor(argTypes);
				if(ci == null)
				{
					throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));
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
						propertyValues[i] = ParseValue(ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Properties[i].Sig), attr.Properties[i].Value);
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
						fieldValues[i] = ParseValue(ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Fields[i].Sig), attr.Fields[i].Value);
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
				TypeWrapper t = ClassLoaderWrapper.LoadClassCritical(attr.Class);
				MethodWrapper mw = t.GetMethodWrapper("<init>", attr.Sig, false);
				mw.Link();
				ConstructorInfo ci = (ConstructorInfo)mw.GetMethod();
				if(ci == null)
				{
					throw new InvalidOperationException(string.Format("Constructor missing: {0}::<init>{1}", attr.Class, attr.Sig));
				}
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
						fieldValues[i] = ParseValue(ClassFile.FieldTypeWrapperFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Fields[i].Sig), attr.Fields[i].Value);
					}
				}
				else
				{
					namedFields = new FieldInfo[0];
					fieldValues = new object[0];
				}
				return new CustomAttributeBuilder(ci, args, namedFields, fieldValues);
			}
		}
#endif

#if !COMPACT_FRAMEWORK
#if STATIC_COMPILER
		internal static void SetEditorBrowsableNever(TypeBuilder tb)
		{
			if(editorBrowsableNever == null)
			{
				editorBrowsableNever = new CustomAttributeBuilder(StaticCompiler.GetType("System.ComponentModel.EditorBrowsableAttribute").GetConstructor(new Type[] { StaticCompiler.GetType("System.ComponentModel.EditorBrowsableState") }), new object[] { (int)System.ComponentModel.EditorBrowsableState.Never });
			}
			tb.SetCustomAttribute(editorBrowsableNever);
		}

		internal static void SetEditorBrowsableNever(MethodBuilder mb)
		{
			if(editorBrowsableNever == null)
			{
				editorBrowsableNever = new CustomAttributeBuilder(StaticCompiler.GetType("System.ComponentModel.EditorBrowsableAttribute").GetConstructor(new Type[] { StaticCompiler.GetType("System.ComponentModel.EditorBrowsableState") }), new object[] { (int)System.ComponentModel.EditorBrowsableState.Never });
			}
			mb.SetCustomAttribute(editorBrowsableNever);
		}

		internal static void SetEditorBrowsableNever(ConstructorBuilder cb)
		{
			if(editorBrowsableNever == null)
			{
				editorBrowsableNever = new CustomAttributeBuilder(StaticCompiler.GetType("System.ComponentModel.EditorBrowsableAttribute").GetConstructor(new Type[] { StaticCompiler.GetType("System.ComponentModel.EditorBrowsableState") }), new object[] { (int)System.ComponentModel.EditorBrowsableState.Never });
			}
			cb.SetCustomAttribute(editorBrowsableNever);
		}

		internal static void SetEditorBrowsableNever(PropertyBuilder pb)
		{
			if(editorBrowsableNever == null)
			{
				editorBrowsableNever = new CustomAttributeBuilder(StaticCompiler.GetType("System.ComponentModel.EditorBrowsableAttribute").GetConstructor(new Type[] { StaticCompiler.GetType("System.ComponentModel.EditorBrowsableState") }), new object[] { (int)System.ComponentModel.EditorBrowsableState.Never });
			}
			pb.SetCustomAttribute(editorBrowsableNever);
		}

		internal static void SetDeprecatedAttribute(MethodBase mb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(typeof(ObsoleteAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			MethodBuilder method = mb as MethodBuilder;
			if(method != null)
			{
				method.SetCustomAttribute(deprecatedAttribute);
			}
			else
			{
				((ConstructorBuilder)mb).SetCustomAttribute(deprecatedAttribute);
			}
		}

		internal static void SetDeprecatedAttribute(TypeBuilder tb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(typeof(ObsoleteAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			tb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetDeprecatedAttribute(FieldBuilder fb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(typeof(ObsoleteAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			fb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetDeprecatedAttribute(PropertyBuilder pb)
		{
			if(deprecatedAttribute == null)
			{
				deprecatedAttribute = new CustomAttributeBuilder(typeof(ObsoleteAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			pb.SetCustomAttribute(deprecatedAttribute);
		}

		internal static void SetThrowsAttribute(MethodBase mb, string[] exceptions)
		{
			if(exceptions != null && exceptions.Length != 0)
			{
				if(throwsAttribute == null)
				{
					throwsAttribute = typeofThrowsAttribute.GetConstructor(new Type[] { typeof(string[]) });
				}
				if(mb is MethodBuilder)
				{
					MethodBuilder method = (MethodBuilder)mb;
					method.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute, new object[] { exceptions }));
				}
				else
				{
					ConstructorBuilder constructor = (ConstructorBuilder)mb;
					constructor.SetCustomAttribute(new CustomAttributeBuilder(throwsAttribute, new object[] { exceptions }));
				}
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
				nonNestedInnerClassAttribute = typeofNonNestedInnerClassAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedInnerClassAttribute, new object[] { className }));
		}

		internal static void SetNonNestedOuterClass(TypeBuilder typeBuilder, string className)
		{
			if(nonNestedOuterClassAttribute == null)
			{
				nonNestedOuterClassAttribute = typeofNonNestedOuterClassAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(nonNestedOuterClassAttribute, new object[] { className }));
		}
#endif // STATIC_COMPILER

		internal static void HideFromReflection(MethodBuilder mb)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofHideFromReflectionAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			mb.SetCustomAttribute(cab);
		}

		internal static void HideFromReflection(FieldBuilder fb)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofHideFromReflectionAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			fb.SetCustomAttribute(cab);
		}

		internal static void HideFromReflection(PropertyBuilder pb)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofHideFromReflectionAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			pb.SetCustomAttribute(cab);
		}
#endif // !COMPACT_FRAMEWORK

		internal static bool IsHideFromReflection(MethodInfo mi)
		{
			return IsDefined(mi, typeofHideFromReflectionAttribute);
		}

		internal static bool IsHideFromReflection(FieldInfo fi)
		{
			return IsDefined(fi, typeofHideFromReflectionAttribute);
		}

		internal static bool IsHideFromReflection(PropertyInfo pi)
		{
			return IsDefined(pi, typeofHideFromReflectionAttribute);
		}

#if !COMPACT_FRAMEWORK
		internal static void HideFromJava(TypeBuilder typeBuilder)
		{
			if(hideFromJavaAttribute == null)
			{
				hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			typeBuilder.SetCustomAttribute(hideFromJavaAttribute);
		}

		internal static void HideFromJava(ConstructorBuilder cb)
		{
			if(hideFromJavaAttribute == null)
			{
				hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			cb.SetCustomAttribute(hideFromJavaAttribute);
		}

		internal static void HideFromJava(MethodBuilder mb)
		{
			if(hideFromJavaAttribute == null)
			{
				hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			mb.SetCustomAttribute(hideFromJavaAttribute);
		}

		internal static void HideFromJava(FieldBuilder fb)
		{
			if(hideFromJavaAttribute == null)
			{
				hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			fb.SetCustomAttribute(hideFromJavaAttribute);
		}

#if STATIC_COMPILER
		internal static void HideFromJava(PropertyBuilder pb)
		{
			if(hideFromJavaAttribute == null)
			{
				hideFromJavaAttribute = new CustomAttributeBuilder(typeofHideFromJavaAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			}
			pb.SetCustomAttribute(hideFromJavaAttribute);
		}
#endif // STATIC_COMPILER
#endif // !COMPACT_FRAMEWORK

		internal static bool IsHideFromJava(Type type)
		{
			return IsDefined(type, typeofHideFromJavaAttribute);
		}

		internal static bool IsHideFromJava(MemberInfo mi)
		{
			// NOTE all privatescope fields and methods are "hideFromJava"
			// because Java cannot deal with the potential name clashes
			FieldInfo fi = mi as FieldInfo;
			if(fi != null && (fi.Attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
			{
				return true;
			}
			MethodBase mb = mi as MethodBase;
			if(mb != null && (mb.Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.PrivateScope)
			{
				return true;
			}
			return IsDefined(mi, typeofHideFromJavaAttribute);
		}

#if STATIC_COMPILER && !COMPACT_FRAMEWORK
		internal static void SetImplementsAttribute(TypeBuilder typeBuilder, TypeWrapper[] ifaceWrappers)
		{
			if(ifaceWrappers != null && ifaceWrappers.Length != 0)
			{
				string[] interfaces = new string[ifaceWrappers.Length];
				for(int i = 0; i < interfaces.Length; i++)
				{
					interfaces[i] = ifaceWrappers[i].Name;
				}
				if(implementsAttribute == null)
				{
					implementsAttribute = typeofImplementsAttribute.GetConstructor(new Type[] { typeof(string[]) });
				}
				typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { interfaces }));
			}
		}
#endif

		internal static bool IsGhostInterface(Type type)
		{
			return IsDefined(type, typeofGhostInterfaceAttribute);
		}

		internal static bool IsRemappedType(Type type)
		{
			return IsDefined(type, typeofRemappedTypeAttribute);
		}

		internal static bool IsExceptionIsUnsafeForMapping(Type type)
		{
			return IsDefined(type, typeofExceptionIsUnsafeForMappingAttribute);
		}

#if !COMPACT_FRAMEWORK
		// this method compares t1 and t2 by name
		// if the type name and assembly name (ignoring the version and strong name) match
		// the type are considered the same
		private static bool MatchTypes(Type t1, Type t2)
		{
			return t1.FullName == t2.FullName
				&& t1.Assembly.GetName().Name == t2.Assembly.GetName().Name;
		}
#endif

		internal static object GetConstantValue(FieldInfo field)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || field.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(field))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofConstantValueAttribute))
					{
						return cad.ConstructorArguments[0].Value;
					}
				}
				return null;
			}
#endif
			// In Java, instance fields can also have a ConstantValue attribute so we emulate that
			// with ConstantValueAttribute (for consumption by ikvmstub only)
			object[] attrib = field.GetCustomAttributes(typeof(ConstantValueAttribute), false);
			if(attrib.Length == 1)
			{
				return ((ConstantValueAttribute)attrib[0]).GetConstantValue();
			}
			return null;
		}

		internal static ModifiersAttribute GetModifiersAttribute(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofModifiersAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						if(args.Count == 2)
						{
							return new ModifiersAttribute((Modifiers)args[0].Value, (bool)args[1].Value);
						}
						return new ModifiersAttribute((Modifiers)args[0].Value);
					}
				}
				return null;
			}
#endif
			object[] attr = type.GetCustomAttributes(typeof(ModifiersAttribute), false);
			return attr.Length == 1 ? (ModifiersAttribute)attr[0] : null;
		}

		internal static ExModifiers GetModifiers(MethodBase mb, bool assemblyIsPrivate)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || mb.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mb))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofModifiersAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						if(args.Count == 2)
						{
							return new ExModifiers((Modifiers)args[0].Value, (bool)args[1].Value);
						}
						return new ExModifiers((Modifiers)args[0].Value, false);
					}
				}
			}
			else
#endif
			{
				object[] customAttribute = mb.GetCustomAttributes(typeof(ModifiersAttribute), false);
				if(customAttribute.Length == 1)
				{
					ModifiersAttribute mod = (ModifiersAttribute)customAttribute[0];
					return new ExModifiers(mod.Modifiers, mod.IsInternal);
				}
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
			if(parameters.Length > 0 && IsDefined(parameters[parameters.Length - 1], typeof(ParamArrayAttribute)))
			{
				modifiers |= Modifiers.VarArgs;
			}
			return new ExModifiers(modifiers, false);
		}

		internal static ExModifiers GetModifiers(FieldInfo fi, bool assemblyIsPrivate)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || fi.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(fi))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofModifiersAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						if(args.Count == 2)
						{
							return new ExModifiers((Modifiers)args[0].Value, (bool)args[1].Value);
						}
						return new ExModifiers((Modifiers)args[0].Value, false);
					}
				}
			}
			else
#endif
			{
				object[] customAttribute = fi.GetCustomAttributes(typeof(ModifiersAttribute), false);
				if(customAttribute.Length == 1)
				{
					ModifiersAttribute mod = (ModifiersAttribute)customAttribute[0];
					return new ExModifiers(mod.Modifiers, mod.IsInternal);
				}
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
			if(Array.IndexOf(fi.GetRequiredCustomModifiers(), typeof(System.Runtime.CompilerServices.IsVolatile)) != -1)
			{
				modifiers |= Modifiers.Volatile;
			}
			return new ExModifiers(modifiers, false);
		}

#if STATIC_COMPILER && !COMPACT_FRAMEWORK
		internal static void SetModifiers(MethodBuilder mb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, typeof(bool) }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			mb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(ConstructorBuilder cb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, typeof(bool) }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			cb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(FieldBuilder fb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, typeof(bool) }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			fb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetModifiers(TypeBuilder tb, Modifiers modifiers, bool isInternal)
		{
			CustomAttributeBuilder customAttributeBuilder;
			if (isInternal)
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers, typeof(bool) }), new object[] { modifiers, isInternal });
			}
			else
			{
				customAttributeBuilder = new CustomAttributeBuilder(typeofModifiersAttribute.GetConstructor(new Type[] { typeofModifiers }), new object[] { modifiers });
			}
			tb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetNameSig(MethodBase mb, string name, string sig)
		{
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeofNameSigAttribute.GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { name, sig });
			MethodBuilder method = mb as MethodBuilder;
			if(method != null)
			{
				method.SetCustomAttribute(customAttributeBuilder);
			}
			else
			{
				((ConstructorBuilder)mb).SetCustomAttribute(customAttributeBuilder);
			}
		}

		internal static void SetNameSig(FieldBuilder fb, string name, string sig)
		{
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeofNameSigAttribute.GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { name, sig });
			fb.SetCustomAttribute(customAttributeBuilder);
		}

		internal static byte[] FreezeDryType(Type type)
		{
			System.IO.MemoryStream mem = new System.IO.MemoryStream();
			System.IO.BinaryWriter bw = new System.IO.BinaryWriter(mem, System.Text.UTF8Encoding.UTF8);
			bw.Write((short)1);
			bw.Write(type.FullName);
			bw.Write((short)0);
			return mem.ToArray();
		}

		internal static void SetInnerClass(TypeBuilder typeBuilder, string innerClass, Modifiers modifiers)
		{
			Type[] argTypes = new Type[] { typeof(string), typeofModifiers };
			object[] args = new object[] { innerClass, modifiers };
			ConstructorInfo ci = typeofInnerClassAttribute.GetConstructor(argTypes);
			CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ci, args);
			typeBuilder.SetCustomAttribute(customAttributeBuilder);
		}

		internal static void SetSourceFile(TypeBuilder typeBuilder, string filename)
		{
			if(sourceFileAttribute == null)
			{
				sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal static void SetSourceFile(ModuleBuilder moduleBuilder, string filename)
		{
			if(sourceFileAttribute == null)
			{
				sourceFileAttribute = typeofSourceFileAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			moduleBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
		}

		internal static void SetLineNumberTable(MethodBase mb, IKVM.Attributes.LineNumberTableAttribute.LineNumberWriter writer)
		{
			object arg;
			ConstructorInfo con;
			if(writer.Count == 1)
			{
				if(lineNumberTableAttribute2 == null)
				{
					lineNumberTableAttribute2 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { typeof(ushort) });
				}
				con = lineNumberTableAttribute2;
				arg = (ushort)writer.LineNo;
			}
			else
			{
				if(lineNumberTableAttribute1 == null)
				{
					lineNumberTableAttribute1 = typeofLineNumberTableAttribute.GetConstructor(new Type[] { typeof(byte[]) });
				}
				con = lineNumberTableAttribute1;
				arg = writer.ToArray();
			}
			if(mb is ConstructorBuilder)
			{
				((ConstructorBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { arg }));
			}
			else
			{
				((MethodBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(con, new object[] { arg }));
			}
		}

		internal static void SetEnclosingMethodAttribute(TypeBuilder tb, string className, string methodName, string methodSig)
		{
			if(enclosingMethodAttribute == null)
			{
				enclosingMethodAttribute = typeofEnclosingMethodAttribute.GetConstructor(new Type[] { typeof(string), typeof(string), typeof(string) });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(enclosingMethodAttribute, new object[] { className, methodName, methodSig }));
		}

		internal static void SetSignatureAttribute(TypeBuilder tb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			tb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { signature }));
		}

		internal static void SetSignatureAttribute(FieldBuilder fb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			fb.SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { signature }));
		}

		internal static void SetSignatureAttribute(MethodBase mb, string signature)
		{
			if(signatureAttribute == null)
			{
				signatureAttribute = typeofSignatureAttribute.GetConstructor(new Type[] { typeof(string) });
			}
			if(mb is ConstructorBuilder)
			{
				((ConstructorBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { signature }));
			}
			else
			{
				((MethodBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(signatureAttribute, new object[] { signature }));
			}
		}

		internal static void SetParamArrayAttribute(ParameterBuilder pb)
		{
			if(paramArrayAttribute == null)
			{
				paramArrayAttribute = new CustomAttributeBuilder(typeof(ParamArrayAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
			}
			pb.SetCustomAttribute(paramArrayAttribute);
		}
#endif  // STATIC_COMPILER && !COMPACT_FRAMEWORK

		internal static NameSigAttribute GetNameSig(FieldInfo field)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || field.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(field))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofNameSigAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
					}
				}
				return null;
			}
#endif
			object[] attr = field.GetCustomAttributes(typeof(NameSigAttribute), false);
			return attr.Length == 1 ? (NameSigAttribute)attr[0] : null;
		}

		internal static NameSigAttribute GetNameSig(MethodBase method)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || method.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(method))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofNameSigAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new NameSigAttribute((string)args[0].Value, (string)args[1].Value);
					}
				}
				return null;
			}
#endif
			object[] attr = method.GetCustomAttributes(typeof(NameSigAttribute), false);
			return attr.Length == 1 ? (NameSigAttribute)attr[0] : null;
		}

#if !COMPACT_FRAMEWORK
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
#endif

		internal static ImplementsAttribute GetImplements(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofImplementsAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new ImplementsAttribute(DecodeArray<string>(args[0]));
					}
				}
				return null;
			}
#endif
			object[] attribs = type.GetCustomAttributes(typeof(ImplementsAttribute), false);
			return attribs.Length == 1 ? (ImplementsAttribute)attribs[0] : null;
		}

		internal static ThrowsAttribute GetThrows(MethodBase mb)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || mb.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mb))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofThrowsAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new ThrowsAttribute(DecodeArray<string>(args[0]));
					}
				}
				return null;
			}
#endif
			object[] attribs = mb.GetCustomAttributes(typeof(ThrowsAttribute), false);
			return attribs.Length == 1 ? (ThrowsAttribute)attribs[0] : null;
		}

		internal static string[] GetNonNestedInnerClasses(Type t)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || t.Assembly.ReflectionOnly)
			{
				List<string> list = new List<string>();
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(t))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofNonNestedInnerClassAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						list.Add((string)args[0].Value);
					}
				}
				return list.ToArray();
			}
#endif
			object[] attribs = t.GetCustomAttributes(typeof(NonNestedInnerClassAttribute), false);
			string[] classes = new string[attribs.Length];
			for(int i = 0; i < attribs.Length; i++)
			{
				classes[i] = ((NonNestedInnerClassAttribute)attribs[i]).InnerClassName;
			}
			return classes;
		}

		internal static string GetNonNestedOuterClasses(Type t)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || t.Assembly.ReflectionOnly)
			{
				List<string> list = new List<string>();
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(t))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofNonNestedOuterClassAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return (string)args[0].Value;
					}
				}
				return null;
			}
#endif
			object[] attribs = t.GetCustomAttributes(typeof(NonNestedOuterClassAttribute), false);
			return attribs.Length == 1 ? ((NonNestedOuterClassAttribute)attribs[0]).OuterClassName : null;
		}

		internal static SignatureAttribute GetSignature(MethodBase mb)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || mb.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mb))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofSignatureAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new SignatureAttribute((string)args[0].Value);
					}
				}
				return null;
			}
#endif
			object[] attribs = mb.GetCustomAttributes(typeof(SignatureAttribute), false);
			return attribs.Length == 1 ? (SignatureAttribute)attribs[0] : null;
		}

		internal static SignatureAttribute GetSignature(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofSignatureAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new SignatureAttribute((string)args[0].Value);
					}
				}
				return null;
			}
#endif
			object[] attribs = type.GetCustomAttributes(typeof(SignatureAttribute), false);
			// HACK for the time being we have to support having two signature attributes
			// (because of the hack in map.xml to make japi happy on the non-generics branch)
			return attribs.Length >= 1 ? (SignatureAttribute)attribs[0] : null;
		}

		internal static SignatureAttribute GetSignature(FieldInfo fi)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || fi.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(fi))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofSignatureAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new SignatureAttribute((string)args[0].Value);
					}
				}
				return null;
			}
#endif
			object[] attribs = fi.GetCustomAttributes(typeof(SignatureAttribute), false);
			return attribs.Length == 1 ? (SignatureAttribute)attribs[0] : null;
		}

		internal static InnerClassAttribute GetInnerClass(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofInnerClassAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new InnerClassAttribute((string)args[0].Value, (Modifiers)args[1].Value);
					}
				}
				return null;
			}
#endif
			object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
			return attribs.Length == 1 ? (InnerClassAttribute)attribs[0] : null;
		}

		internal static RemappedInterfaceMethodAttribute[] GetRemappedInterfaceMethods(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				List<RemappedInterfaceMethodAttribute> attrs = new List<RemappedInterfaceMethodAttribute>();
					foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
					{
						if(MatchTypes(cad.Constructor.DeclaringType, typeofRemappedInterfaceMethodAttribute))
						{
							IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
							attrs.Add(new RemappedInterfaceMethodAttribute((string)args[0].Value, (string)args[1].Value));
						}
					}
				return attrs.ToArray();
			}
#endif
			object[] attr = type.GetCustomAttributes(typeof(RemappedInterfaceMethodAttribute), false);
			RemappedInterfaceMethodAttribute[] attr1 = new RemappedInterfaceMethodAttribute[attr.Length];
			Array.Copy(attr, attr1, attr.Length);
			return attr1;
		}

		internal static RemappedTypeAttribute GetRemappedType(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofRemappedTypeAttribute))
					{
						IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
						return new RemappedTypeAttribute((Type)args[0].Value);
					}
				}
				return null;
			}
#endif
			object[] attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
			return attribs.Length == 1 ? (RemappedTypeAttribute)attribs[0] : null;
		}

		internal static RemappedClassAttribute[] GetRemappedClasses(Assembly coreAssembly)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || coreAssembly.ReflectionOnly)
			{
				List<RemappedClassAttribute> attrs = new List<RemappedClassAttribute>();
					foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(coreAssembly))
					{
						if(MatchTypes(cad.Constructor.DeclaringType, typeofRemappedClassAttribute))
						{
							IList<CustomAttributeTypedArgument> args = cad.ConstructorArguments;
							attrs.Add(new RemappedClassAttribute((string)args[0].Value, (Type)args[1].Value));
						}
					}
				return attrs.ToArray();
			}
#endif
			object[] attr = coreAssembly.GetCustomAttributes(typeof(RemappedClassAttribute), false);
			RemappedClassAttribute[] attr1 = new RemappedClassAttribute[attr.Length];
			Array.Copy(attr, attr1, attr.Length);
			return attr1;
		}

		internal static string GetAnnotationAttributeType(Type type)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofAnnotationAttributeAttribute))
					{
						return (string)cad.ConstructorArguments[0].Value;
					}
				}
				return null;
			}
#endif
			object[] attr = type.GetCustomAttributes(typeof(AnnotationAttributeAttribute), false);
			if(attr.Length == 1)
			{
				return ((AnnotationAttributeAttribute)attr[0]).AttributeType;
			}
			return null;
		}

		internal static bool IsDefined(Module mod, Type attribute)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || mod.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mod))
				{
					// NOTE we don't support subtyping relations!
					if(MatchTypes(cad.Constructor.DeclaringType, attribute))
					{
						return true;
					}
				}
				return false;
			}
#endif
			return mod.IsDefined(attribute, false);
		}

		internal static bool IsDefined(Assembly asm, Type attribute)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || asm.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(asm))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, attribute))
					{
						return true;
					}
				}
				return false;
			}
#endif
			return asm.IsDefined(attribute, false);
		}

		internal static bool IsDefined(Type type, Type attribute)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || type.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(type))
				{
					// NOTE we don't support subtyping relations!
					if(MatchTypes(cad.Constructor.DeclaringType, attribute))
					{
						return true;
					}
				}
				return false;
			}
#endif
			return type.IsDefined(attribute, false);
		}

		internal static bool IsDefined(ParameterInfo pi, Type attribute)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || pi.Member.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(pi))
				{
					// NOTE we don't support subtyping relations!
					if(MatchTypes(cad.Constructor.DeclaringType, attribute))
					{
						return true;
					}
				}
				return false;
			}
#endif
			return pi.IsDefined(attribute, false);
		}

		internal static bool IsDefined(MemberInfo member, Type attribute)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || member.DeclaringType.Assembly.ReflectionOnly)
			{
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(member))
				{
					// NOTE we don't support subtyping relations!
					if(MatchTypes(cad.Constructor.DeclaringType, attribute))
					{
						return true;
					}
				}
				return false;
			}
#endif
			return member.IsDefined(attribute, false);
		}

		internal static bool IsJavaModule(Module mod)
		{
			return IsDefined(mod, typeofJavaModuleAttribute);
		}

		internal static object[] GetJavaModuleAttributes(Module mod)
		{
#if !COMPACT_FRAMEWORK
			if(JVM.IsStaticCompiler || mod.Assembly.ReflectionOnly)
			{
				ArrayList attrs = new ArrayList();
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(mod))
				{
					if(MatchTypes(cad.Constructor.DeclaringType, typeofJavaModuleAttribute))
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
				}
				return attrs.ToArray();
			}
#endif
			return mod.GetCustomAttributes(typeofJavaModuleAttribute, false);
		}

		internal static bool IsNoPackagePrefix(Type type)
		{
			return IsDefined(type, typeofNoPackagePrefixAttribute) || IsDefined(type.Assembly, typeofNoPackagePrefixAttribute);
		}

#if STATIC_COMPILER && !COMPACT_FRAMEWORK
		internal static void SetRemappedClass(AssemblyBuilder assemblyBuilder, string name, Type shadowType)
		{
			ConstructorInfo remappedClassAttribute = typeofRemappedClassAttribute.GetConstructor(new Type[] { typeof(string), typeof(Type) });
			assemblyBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedClassAttribute, new object[] { name, shadowType }));
		}

		internal static void SetRemappedType(TypeBuilder typeBuilder, Type shadowType)
		{
			ConstructorInfo remappedTypeAttribute = typeofRemappedTypeAttribute.GetConstructor(new Type[] { typeof(Type) });
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(remappedTypeAttribute, new object[] { shadowType }));
		}

		internal static void SetRemappedInterfaceMethod(TypeBuilder typeBuilder, string name, string mappedTo)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofRemappedInterfaceMethodAttribute.GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { name, mappedTo } );
			typeBuilder.SetCustomAttribute(cab);
		}

		internal static void SetExceptionIsUnsafeForMapping(TypeBuilder typeBuilder)
		{
			CustomAttributeBuilder cab = new CustomAttributeBuilder(typeofExceptionIsUnsafeForMappingAttribute.GetConstructor(Type.EmptyTypes), new object[0]);
			typeBuilder.SetCustomAttribute(cab);
		}

		internal static void SetConstantValue(FieldBuilder field, object constantValue)
		{
			CustomAttributeBuilder constantValueAttrib;
			try
			{
				constantValueAttrib = new CustomAttributeBuilder(typeofConstantValueAttribute.GetConstructor(new Type[] { constantValue.GetType() }), new object[] { constantValue });
			}
			catch (OverflowException)
			{
				// FXBUG for char values > 32K .NET (1.1 and 2.0) throws an exception (because it tries to convert to Int16)
				if (constantValue is char)
				{
					// we use the int constant value instead, the stub generator can handle that
					constantValueAttrib = new CustomAttributeBuilder(typeofConstantValueAttribute.GetConstructor(new Type[] { typeof(int) }), new object[] { (int)(char)constantValue });
				}
				else
				{
					throw;
				}
			}
			field.SetCustomAttribute(constantValueAttrib);
		}
#endif // STATIC_COMPILER && !COMPACT_FRAMEWORK
	}

#if !COMPACT_FRAMEWORK
	abstract class Annotation
	{
		// NOTE this method returns null if the type could not be found
		// or if the type is not a Custom Attribute and we're not in the static compiler
		internal static Annotation Load(ClassLoaderWrapper loader, object[] def)
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
			try
			{
				TypeWrapper annot = loader.RetTypeWrapperFromSig(annotationClass.Replace('/', '.'));
				return annot.Annotation;
			}
#if STATIC_COMPILER
			catch(ClassNotFoundException x)
			{
				StaticCompiler.IssueMessage(Message.ClassNotFound, x.Message);
				return null;
			}
#endif
			catch (RetargetableJavaException)
			{
				Tracer.Warning(Tracer.Compiler, "Unable to load annotation class {0}", annotationClass);
				return null;
			}
		}

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

		internal abstract void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation);
		internal abstract void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation);

		internal virtual void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
		{
		}
	}
#endif

	[Flags]
	enum TypeFlags : ushort
	{
		HasIncompleteInterfaceImplementation = 1,
		InternalAccess = 2,
		HasStaticInitializer = 4,
		VerifyError = 8,
		ClassFormatError = 16,
		HasUnsupportedAbstractMethods = 32,
	}

	internal abstract class TypeWrapper
	{
		private readonly string name;		// java name (e.g. java.lang.Object)
		private readonly Modifiers modifiers;
		private TypeFlags flags;
		private MethodWrapper[] methods;
		private FieldWrapper[] fields;
		private readonly TypeWrapper baseWrapper;
#if !STATIC_COMPILER
		private object classObject;
#endif
		internal static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
		internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
		internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

		internal TypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper)
		{
			Profiler.Count("TypeWrapper");
			// class name should be dotted or null for primitives
			Debug.Assert(name == null || name.IndexOf('/') < 0);

			this.modifiers = modifiers;
			this.name = name == null ? null : String.Intern(name);
			this.baseWrapper = baseWrapper;
		}

#if !STATIC_COMPILER
		internal void SetClassObject(object classObject)
		{
			this.classObject = classObject;
		}

		internal object ClassObject
		{
			get
			{
				Debug.Assert(!IsUnloadable && !IsVerifierType);
				lock(this)
				{
					if(classObject == null)
					{
#if !COMPACT_FRAMEWORK
						// DynamicTypeWrapper should haved already had SetClassObject explicitly
						Debug.Assert(!(this is DynamicTypeWrapper));
#endif // !COMPACT_FRAMEWORK
						classObject = JVM.Library.newClass(this, null, GetClassLoader().GetJavaClassLoader());
					}
				}
				return classObject;
			}
		}

		internal static TypeWrapper FromClass(object classObject)
		{
			return (TypeWrapper)JVM.Library.getWrapperFromClass(classObject);
		}
#endif // !STATIC_COMPILER

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

		internal bool HasIncompleteInterfaceImplementation
		{
			get
			{
				return (flags & TypeFlags.HasIncompleteInterfaceImplementation) != 0 || (baseWrapper != null && baseWrapper.HasIncompleteInterfaceImplementation);
			}
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.HasIncompleteInterfaceImplementation;
				}
				else
				{
					flags &= ~TypeFlags.HasIncompleteInterfaceImplementation;
				}
			}
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
				return (flags & TypeFlags.HasUnsupportedAbstractMethods) != 0 || (baseWrapper != null && baseWrapper.HasUnsupportedAbstractMethods);
			}
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.HasUnsupportedAbstractMethods;
				}
				else
				{
					flags &= ~TypeFlags.HasUnsupportedAbstractMethods;
				}
			}
		}

		internal virtual bool HasStaticInitializer
		{
			get
			{
				return (flags & TypeFlags.HasStaticInitializer) != 0;
			}
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.HasStaticInitializer;
				}
				else
				{
					flags &= ~TypeFlags.HasStaticInitializer;
				}
			}
		}

		internal bool HasVerifyError
		{
			get
			{
				return (flags & TypeFlags.VerifyError) != 0;
			}
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.VerifyError;
				}
				else
				{
					flags &= ~TypeFlags.VerifyError;
				}
			}
		}

		internal bool HasClassFormatError
		{
			get
			{
				return (flags & TypeFlags.ClassFormatError) != 0;
			}
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.ClassFormatError;
				}
				else
				{
					flags &= ~TypeFlags.ClassFormatError;
				}
			}
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
				return IsArray && (ElementTypeWrapper.IsGhost || ElementTypeWrapper.IsGhostArray);
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

		internal bool IsErasedOrBoxedPrimitiveOrRemapped
		{
			get
			{
				bool erased = IsUnloadable || IsGhostArray || IsDynamicOnly;
				return erased || (!IsPrimitive && IsJavaPrimitive(TypeAsSignatureType)) || (IsRemapped && this is DotNetTypeWrapper);
			}
		}

		internal virtual bool IsDynamicOnly
		{
			get
			{
				return false;
			}
		}

		// is this an array type of which the ultimate element type is dynamic-only?
		internal bool IsDynamicOnlyArray
		{
			get
			{
				return IsArray && (ElementTypeWrapper.IsDynamicOnly || ElementTypeWrapper.IsDynamicOnlyArray);
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
			set
			{
				// TODO do we need locking here?
				if(value)
				{
					flags |= TypeFlags.InternalAccess;
				}
				else
				{
					flags &= ~TypeFlags.InternalAccess;
				}
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
			lock(this)
			{
				if(fields == null)
				{
					LazyPublishMembers();
				}
			}
			foreach(FieldWrapper fw in fields)
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

		internal MethodWrapper[] GetMethods()
		{
			lock(this)
			{
				if(methods == null)
				{
					LazyPublishMembers();
				}
			}
			return methods;
		}

		internal FieldWrapper[] GetFields()
		{
			lock(this)
			{
				if(fields == null)
				{
					LazyPublishMembers();
				}
			}
			return fields;
		}

		internal MethodWrapper GetMethodWrapper(string name, string sig, bool inherit)
		{
			lock(this)
			{
				if(methods == null)
				{
					LazyPublishMembers();
				}
			}
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
			if(inherit && baseWrapper != null)
			{
				return baseWrapper.GetMethodWrapper(name, sig, inherit);
			}
			return null;
		}

		internal void SetMethods(MethodWrapper[] methods)
		{
			Debug.Assert(methods != null);
			this.methods = methods;
		}

		internal void SetFields(FieldWrapper[] fields)
		{
			Debug.Assert(fields != null);
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
				|| (IsInternal && GetClassLoader() == wrapper.GetClassLoader())
				|| IsInSamePackageAs(wrapper);
		}

		internal bool IsInSamePackageAs(TypeWrapper wrapper)
		{
			if(GetClassLoader() == wrapper.GetClassLoader())
			{
				int index1 = name.LastIndexOf('.');
				int index2 = wrapper.name.LastIndexOf('.');
				if(index1 == -1 && index2 == -1)
				{
					return true;
				}
				// for array types we need to skip the brackets
				int skip1 = 0;
				int skip2 = 0;
				while(name[skip1] == '[')
				{
					skip1++;
				}
				while(wrapper.name[skip2] == '[')
				{
					skip2++;
				}
				if(skip1 > 0)
				{
					// skip over the L that follows the brackets
					skip1++;
				}
				if(skip2 > 0)
				{
					// skip over the L that follows the brackets
					skip2++;
				}
				if((index1 - skip1) != (index2 - skip2))
				{
					return false;
				}
				return String.CompareOrdinal(name, skip1, wrapper.name, skip2, index1 - skip1) == 0;
			}
			return false;
		}

		internal abstract Type TypeAsTBD
		{
			get;
		}

#if !COMPACT_FRAMEWORK
		internal virtual TypeBuilder TypeAsBuilder
		{
			get
			{
				TypeBuilder typeBuilder = TypeAsTBD as TypeBuilder;
				Debug.Assert(typeBuilder != null);
				return typeBuilder;
			}
		}
#endif

		internal Type TypeAsSignatureType
		{
			get
			{
				if(IsUnloadable)
				{
					return typeof(object);
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(typeof(object), ArrayRank);
				}
				return TypeAsTBD;
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
				// NOTE as a convenience to the compiler, we replace return address types with typeof(int)
				if(VerifierTypeWrapper.IsRet(this))
				{
					return typeof(int);
				}
				if(IsUnloadable || IsGhost)
				{
					return typeof(object);
				}
				if(IsNonPrimitiveValueType)
				{
					// return either System.ValueType or System.Enum
					return TypeAsTBD.BaseType;
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(typeof(object), ArrayRank);
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
					return typeof(object);
				}
				if(IsGhostArray)
				{
					return ArrayTypeWrapper.MakeArrayType(typeof(object), ArrayRank);
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
					return typeof(Exception);
				}
				return TypeAsTBD;
			}
		}

		internal TypeWrapper BaseTypeWrapper
		{
			get
			{
				return baseWrapper;
			}
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
				return (!elem1.IsNonPrimitiveValueType && elem1.IsSubTypeOf(elem2)) || (rank1 == rank2 && elem2.IsGhost && elem1 == CoreClasses.java.lang.Object.Wrapper);
			}
			return this.IsSubTypeOf(wrapper);
		}

		internal bool IsInstance(object obj)
		{
#if !FIRST_PASS && !STATIC_COMPILER
			if(obj != null)
			{
				TypeWrapper thisWrapper = this;
				TypeWrapper objWrapper = IKVM.NativeCode.ikvm.runtime.Util.GetTypeWrapperFromObject(obj);
				if(thisWrapper.IsGhostArray)
				{
					TypeWrapper elementType = objWrapper;
					while(elementType.IsArray)
					{
						elementType = elementType.ElementTypeWrapper;
					}
					return thisWrapper.ArrayRank == objWrapper.ArrayRank && elementType == CoreClasses.java.lang.Object.Wrapper;
				}
				if(thisWrapper.IsDynamicOnlyArray)
				{
					TypeWrapper elementType = thisWrapper;
					while(elementType.IsArray)
					{
						elementType = elementType.ElementTypeWrapper;
					}
					elementType = elementType.BaseTypeWrapper;
					if(elementType == null)
					{
						elementType = CoreClasses.java.lang.Object.Wrapper;
					}
					thisWrapper = elementType.MakeArrayType(thisWrapper.ArrayRank);
				}
				return objWrapper.IsAssignableTo(thisWrapper);
			}
#endif
			return false;
		}

		internal abstract TypeWrapper[] Interfaces
		{
			get;
		}

		// NOTE this property can only be called for finished types!
		internal abstract TypeWrapper[] InnerClasses
		{
			get;
		}

		// NOTE this property can only be called for finished types!
		internal abstract TypeWrapper DeclaringTypeWrapper
		{
			get;
		}

		internal abstract void Finish();

#if !COMPACT_FRAMEWORK
		private void ImplementInterfaceMethodStubImpl(MethodWrapper ifmethod, TypeBuilder typeBuilder, DynamicTypeWrapper wrapper)
		{
			// we're mangling the name to prevent subclasses from accidentally overriding this method and to
			// prevent clashes with overloaded method stubs that are erased to the same signature (e.g. unloadable types and ghost arrays)
			// HACK the signature and name are the wrong way around to work around a C++/CLI bug (apparantely it looks looks at the last n
			// characters of the method name, or something bizarre like that)
			// https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=234167
			string mangledName = this.Name + "/" + ifmethod.Signature + ifmethod.Name;
			MethodWrapper mce = null;
			TypeWrapper lookup = wrapper;
			while(lookup != null)
			{
				mce = lookup.GetMethodWrapper(ifmethod.Name, ifmethod.Signature, true);
				if(mce == null || !mce.IsStatic)
				{
					break;
				}
				lookup = mce.DeclaringType.BaseTypeWrapper;
			}
			if(mce != null)
			{
				if(mce.DeclaringType != wrapper)
				{
					// check the loader constraints
					bool error = false;
					if(mce.ReturnType != ifmethod.ReturnType)
					{
						// TODO handle unloadable
						error = true;
					}
					TypeWrapper[] mceparams = mce.GetParameters();
					TypeWrapper[] ifparams = ifmethod.GetParameters();
					for(int i = 0; i < mceparams.Length; i++)
					{
						if(mceparams[i] != ifparams[i])
						{
							// TODO handle unloadable
							error = true;
							break;
						}
					}
					if(error)
					{
						MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
						AttributeHelper.HideFromJava(mb);
						EmitHelper.Throw(mb.GetILGenerator(), "java.lang.LinkageError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
						return;
					}
				}
				if(mce.IsMirandaMethod && mce.DeclaringType == wrapper)
				{
					// Miranda methods already have a methodimpl (if needed) to implement the correct interface method
				}
				else if(!mce.IsPublic)
				{
					// NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
					// it makes sense, so I hope the spec is wrong
					// UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
					// methods. Sigh! So I have to use private methods and mangle the name
					MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
					AttributeHelper.HideFromJava(mb);
					EmitHelper.Throw(mb.GetILGenerator(), "java.lang.IllegalAccessError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
					wrapper.HasIncompleteInterfaceImplementation = true;
				}
				else if(mce.GetMethod() == null || mce.RealName != ifmethod.RealName)
				{
					MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
					AttributeHelper.HideFromJava(mb);
					ILGenerator ilGenerator = mb.GetILGenerator();
					ilGenerator.Emit(OpCodes.Ldarg_0);
					int argc = mce.GetParameters().Length;
					for(int n = 0; n < argc; n++)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
					}
					mce.EmitCallvirt(ilGenerator);
					ilGenerator.Emit(OpCodes.Ret);
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
				}
				else if(!mce.DeclaringType.TypeAsTBD.Assembly.Equals(typeBuilder.Assembly))
				{
					// NOTE methods inherited from base classes in a different assembly do *not* automatically implement
					// interface methods, so we have to generate a stub here that doesn't do anything but call the base
					// implementation
					MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
					AttributeHelper.HideFromJava(mb);
					ILGenerator ilGenerator = mb.GetILGenerator();
					ilGenerator.Emit(OpCodes.Ldarg_0);
					int argc = mce.GetParameters().Length;
					for(int n = 0; n < argc; n++)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
					}
					mce.EmitCallvirt(ilGenerator);
					ilGenerator.Emit(OpCodes.Ret);
				}
			}
			else
			{
				if(!wrapper.IsAbstract)
				{
					// the type doesn't implement the interface method and isn't abstract either. The JVM allows this, but the CLR doesn't,
					// so we have to create a stub method that throws an AbstractMethodError
					MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
					AttributeHelper.HideFromJava(mb);
					EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
					wrapper.HasIncompleteInterfaceImplementation = true;
				}
			}
		}

		internal void ImplementInterfaceMethodStubs(TypeBuilder typeBuilder, DynamicTypeWrapper wrapper, Hashtable doneSet)
		{
			Debug.Assert(this.IsInterface);

			// make sure we don't do the same method twice and dynamic only interfaces
			// don't really exist, so there is no point in generating stub methods for
			// them (nor can we).
			if(doneSet.ContainsKey(this) || this.IsDynamicOnly)
			{
				return;
			}
			doneSet.Add(this, this);
			foreach(MethodWrapper method in GetMethods())
			{
				if(!method.IsStatic)
				{
					ImplementInterfaceMethodStubImpl(method, typeBuilder, wrapper);
				}
			}
			TypeWrapper[] interfaces = Interfaces;
			for(int i = 0; i < interfaces.Length; i++)
			{
				interfaces[i].ImplementInterfaceMethodStubs(typeBuilder, wrapper, doneSet);
			}
		}
#endif

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

		internal void RunClassInit()
		{
			Type t = IsRemapped ? TypeAsBaseType : TypeAsTBD;
			if(t != null)
			{
				System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(t.TypeHandle);
			}
		}

#if !COMPACT_FRAMEWORK
		internal void EmitUnbox(CountingILGenerator ilgen)
		{
			Debug.Assert(this.IsNonPrimitiveValueType);

			ilgen.LazyEmitUnboxSpecial(this.TypeAsTBD);
		}

		internal void EmitBox(CountingILGenerator ilgen)
		{
			Debug.Assert(this.IsNonPrimitiveValueType);

			ilgen.LazyEmitBox(this.TypeAsTBD);
		}

		internal void EmitConvSignatureTypeToStackType(ILGenerator ilgen)
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
				LocalBuilder local = ilgen.DeclareLocal(TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloca, local);
				ilgen.Emit(OpCodes.Ldfld, GhostRefField);
			}
		}

		// NOTE sourceType is optional and only used for interfaces,
		// it is *not* used to automatically downcast
		internal void EmitConvStackTypeToSignatureType(ILGenerator ilgen, TypeWrapper sourceType)
		{
			if(!IsUnloadable)
			{
				if(IsGhost)
				{
					LocalBuilder local1 = ilgen.DeclareLocal(TypeAsLocalOrStackType);
					ilgen.Emit(OpCodes.Stloc, local1);
					LocalBuilder local2 = ilgen.DeclareLocal(TypeAsSignatureType);
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
					EmitHelper.EmitAssertType(ilgen, TypeAsTBD);
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

		internal virtual void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
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
				ilgen.Emit(OpCodes.Ldc_I4, rank);
				ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("CastArray"));
				ilgen.Emit(OpCodes.Castclass, ArrayTypeWrapper.MakeArrayType(typeof(object), rank));
			}
			else if(IsDynamicOnly)
			{
				ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldstr, this.Name);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCast);
			}
			else
			{
				EmitHelper.Castclass(ilgen, TypeAsTBD);
			}
		}

		internal virtual void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
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
				ilgen.Emit(OpCodes.Ldc_I4, rank);
				ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("IsInstanceArray"));
			}
			else if(IsDynamicOnly)
			{
				ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldstr, this.Name);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicInstanceOf);
			}
			else
			{
				ilgen.LazyEmit_instanceof(TypeAsTBD);
			}
		}
#endif

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

#if !COMPACT_FRAMEWORK
		internal virtual void EmitRunClassConstructor(ILGenerator ilgen)
		{
		}
#endif

		internal abstract string GetGenericSignature();

		internal abstract string GetGenericMethodSignature(MethodWrapper mw);

		internal abstract string GetGenericFieldSignature(FieldWrapper fw);

		internal abstract string[] GetEnclosingMethod();

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

#if !STATIC_COMPILER
		internal virtual object GetAnnotationDefault(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb != null)
			{
				if(mb.DeclaringType.Assembly.ReflectionOnly)
				{
					// TODO
					return null;
				}
				object[] attr = mb.GetCustomAttributes(typeof(AnnotationDefaultAttribute), false);
				if(attr.Length == 1)
				{
					return JVM.Library.newAnnotationElementValue(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), mw.ReturnType.ClassObject, ((AnnotationDefaultAttribute)attr[0]).Value);
				}
			}
			return null;
		}
#endif // !STATIC_COMPILER

#if !COMPACT_FRAMEWORK
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
#endif
	}

	class UnloadableTypeWrapper : TypeWrapper
	{
		internal UnloadableTypeWrapper(string name)
			: base(TypeWrapper.UnloadableModifiersHack, name, null)
		{
#if STATIC_COMPILER
			if(name != "<verifier>")
			{
				if(name.StartsWith("["))
				{
					int skip = 1;
					while(name[skip++] == '[');
					name = name.Substring(skip, name.Length - skip - 1);
				}
				StaticCompiler.IssueMessage(Message.ClassNotFound, name);
			}
#endif
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

#if !COMPACT_FRAMEWORK
		internal override void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
			ilgen.Emit(OpCodes.Ldstr, Name);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCast);
		}

		internal override void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
			ilgen.Emit(OpCodes.Ldstr, Name);
			ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicInstanceOf);
		}
#endif

		internal override string GetGenericSignature()
		{
			throw new InvalidOperationException("GetGenericSignature called on UnloadableTypeWrapper: " + Name);
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			throw new InvalidOperationException("GetGenericMethodSignature called on UnloadableTypeWrapper: " + Name);
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			throw new InvalidOperationException("GetGenericFieldSignature called on UnloadableTypeWrapper: " + Name);
		}

		internal override string[] GetEnclosingMethod()
		{
			throw new InvalidOperationException("GetEnclosingMethod called on UnloadableTypeWrapper: " + Name);
		}
	}

	class PrimitiveTypeWrapper : TypeWrapper
	{
		internal static readonly PrimitiveTypeWrapper BYTE = new PrimitiveTypeWrapper(typeof(byte), "B");
		internal static readonly PrimitiveTypeWrapper CHAR = new PrimitiveTypeWrapper(typeof(char), "C");
		internal static readonly PrimitiveTypeWrapper DOUBLE = new PrimitiveTypeWrapper(typeof(double), "D");
		internal static readonly PrimitiveTypeWrapper FLOAT = new PrimitiveTypeWrapper(typeof(float), "F");
		internal static readonly PrimitiveTypeWrapper INT = new PrimitiveTypeWrapper(typeof(int), "I");
		internal static readonly PrimitiveTypeWrapper LONG = new PrimitiveTypeWrapper(typeof(long), "J");
		internal static readonly PrimitiveTypeWrapper SHORT = new PrimitiveTypeWrapper(typeof(short), "S");
		internal static readonly PrimitiveTypeWrapper BOOLEAN = new PrimitiveTypeWrapper(typeof(bool), "Z");
		internal static readonly PrimitiveTypeWrapper VOID = new PrimitiveTypeWrapper(typeof(void), "V");

		private readonly Type type;
		private readonly string sigName;

		private PrimitiveTypeWrapper(Type type, string sigName)
			: base(Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null, null)
		{
			this.type = type;
			this.sigName = sigName;
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

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				return TypeWrapper.EmptyArray;
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				return TypeWrapper.EmptyArray;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				return null;
			}
		}

		internal override void Finish()
		{
		}

		public override string ToString()
		{
			return "PrimitiveTypeWrapper[" + sigName + "]";
		}

		internal override string GetGenericSignature()
		{
			return null;
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			return null;
		}

		internal override string[] GetEnclosingMethod()
		{
			return null;
		}
	}

#if !COMPACT_FRAMEWORK
#if STATIC_COMPILER
	abstract class DynamicTypeWrapper : TypeWrapper
#else
	class DynamicTypeWrapper : TypeWrapper
#endif
	{
		protected readonly ClassLoaderWrapper classLoader;
		private volatile DynamicImpl impl;
		private TypeWrapper[] interfaces;
		private readonly string sourceFileName;
#if !STATIC_COMPILER
		private byte[][] lineNumberTables;
#endif

		private static TypeWrapper LoadTypeWrapper(ClassLoaderWrapper classLoader, string name)
		{
			TypeWrapper tw = classLoader.LoadClassByDottedNameFast(name);
			if(tw == null)
			{
				throw new NoClassDefFoundError(name);
			}
			return tw;
		}

		internal DynamicTypeWrapper(ClassFile f, ClassLoaderWrapper classLoader)
			: base(f.Modifiers, f.Name, f.IsInterface ? null : LoadTypeWrapper(classLoader, f.SuperClass))
		{
			Profiler.Count("DynamicTypeWrapper");
			this.classLoader = classLoader;
			this.IsInternal = f.IsInternal;
			this.sourceFileName = f.SourceFileAttribute;
			if(BaseTypeWrapper != null)
			{
				if(!BaseTypeWrapper.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superclass " + BaseTypeWrapper.Name);
				}
				if(!BaseTypeWrapper.IsPublic && !BaseTypeWrapper.TypeAsBaseType.Assembly.Equals(classLoader.GetTypeWrapperFactory().ModuleBuilder.Assembly))
				{
					// NOTE this can only happen if evil code calls ClassLoader.defineClass() on an assembly class loader (which we allow for compatibility with other slightly less evil code)
					throw new IllegalAccessError("Class " + f.Name + " cannot access its non-public superclass " + BaseTypeWrapper.Name + " from another assembly");
				}
				if(BaseTypeWrapper.IsFinal)
				{
					throw new VerifyError("Class " + f.Name + " extends final class " + BaseTypeWrapper.Name);
				}
				if(BaseTypeWrapper.IsInterface)
				{
					throw new IncompatibleClassChangeError("Class " + f.Name + " has interface " + BaseTypeWrapper.Name + " as superclass");
				}
				if(!f.IsFinal)
				{
					if(BaseTypeWrapper.TypeAsTBD == typeof(ValueType) || BaseTypeWrapper.TypeAsTBD == typeof(Enum))
					{
						throw new VerifyError("Value types must be final");
					}
					if(BaseTypeWrapper.TypeAsTBD == typeof(MulticastDelegate))
					{
						throw new VerifyError("Delegates must be final");
					}
				}
				if(BaseTypeWrapper.TypeAsTBD == typeof(Delegate))
				{
					throw new VerifyError(BaseTypeWrapper.Name + " cannot be used as a base class");
				}
				// NOTE defining value types, enums and delegates is not supported in IKVM v1
				if(BaseTypeWrapper.TypeAsTBD == typeof(ValueType) || BaseTypeWrapper.TypeAsTBD == typeof(Enum))
				{
					throw new VerifyError("Defining value types in Java is not implemented in IKVM v1");
				}
				if(BaseTypeWrapper.TypeAsTBD == typeof(MulticastDelegate))
				{
					throw new VerifyError("Defining delegates in Java is not implemented in IKVM v1");
				}
			}

			ClassFile.ConstantPoolItemClass[] interfaces = f.Interfaces;
			this.interfaces = new TypeWrapper[interfaces.Length];
			for(int i = 0; i < interfaces.Length; i++)
			{
				TypeWrapper iface = LoadTypeWrapper(classLoader, interfaces[i].Name);
				if(!iface.IsAccessibleFrom(this))
				{
					throw new IllegalAccessError("Class " + f.Name + " cannot access its superinterface " + iface.Name);
				}
				if(!iface.IsPublic
					&& !iface.TypeAsBaseType.Assembly.Equals(classLoader.GetTypeWrapperFactory().ModuleBuilder.Assembly)
					&& iface.TypeAsBaseType.Assembly.GetType(DynamicClassLoader.GetProxyHelperName(iface.TypeAsTBD)) == null)
				{
					// NOTE this happens when you call Proxy.newProxyInstance() on a non-public .NET interface
					// (for ikvmc compiled Java types, ikvmc generates public proxy stubs).
					// NOTE we don't currently check interfaces inherited from other interfaces because mainstream .NET languages
					// don't allow public interfaces extending non-public interfaces.
					throw new IllegalAccessError("Class " + f.Name + " cannot access its non-public superinterface " + iface.Name + " from another assembly");
				}
				if(!iface.IsInterface)
				{
					throw new IncompatibleClassChangeError("Implementing class");
				}
				this.interfaces[i] = iface;
			}

			impl = new JavaTypeImpl(f, this);
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return classLoader;
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				return impl.ReflectiveModifiers;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				return interfaces;
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				return impl.InnerClasses;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				return impl.DeclaringTypeWrapper;
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return impl.Type;
			}
		}

#if STATIC_COMPILER
		internal override Annotation Annotation
		{
			get
			{
				return impl.Annotation;
			}
		}

		internal override Type EnumType
		{
			get
			{
				return impl.EnumType;
			}
		}
#endif // STATIC_COMPILER

		internal override void Finish()
		{
			// we don't need locking, because Finish is Thread safe
			impl = impl.Finish();
		}

		// NOTE can only be used if the type hasn't been finished yet!
		internal FieldInfo ClassObjectField
		{
			get
			{
				return ((JavaTypeImpl)impl).ClassObjectField;
			}
		}

		// NOTE can only be used if the type hasn't been finished yet!
		protected string GenerateUniqueMethodName(string basename, MethodWrapper mw)
		{
			return ((JavaTypeImpl)impl).GenerateUniqueMethodName(basename, mw);
		}

		// NOTE can only be used if the type hasn't been finished yet!
		internal string GenerateUniqueMethodName(string basename, Type returnType, Type[] parameterTypes)
		{
			return ((JavaTypeImpl)impl).GenerateUniqueMethodName(basename, returnType, parameterTypes);
		}

		internal void CreateStep1(out bool hasclinit)
		{
			((JavaTypeImpl)impl).CreateStep1(out hasclinit);
		}

		internal void CreateStep2NoFail(bool hasclinit, string mangledTypeName)
		{
			((JavaTypeImpl)impl).CreateStep2NoFail(hasclinit, mangledTypeName);
		}

		private abstract class DynamicImpl
		{
			internal abstract Type Type { get; }
			internal abstract TypeWrapper[] InnerClasses { get; }
			internal abstract TypeWrapper DeclaringTypeWrapper { get; }
			internal abstract Modifiers ReflectiveModifiers { get; }
#if STATIC_COMPILER
			internal abstract Annotation Annotation { get; }
			internal abstract Type EnumType { get; }
#endif
			internal abstract DynamicImpl Finish();
			internal abstract MethodBase LinkMethod(MethodWrapper mw);
			internal abstract FieldInfo LinkField(FieldWrapper fw);
			internal abstract void EmitRunClassConstructor(ILGenerator ilgen);
			internal abstract string GetGenericSignature();
			internal abstract string[] GetEnclosingMethod();
			internal abstract string GetGenericMethodSignature(int index);
			internal abstract string GetGenericFieldSignature(int index);
			internal abstract object[] GetDeclaredAnnotations();
			internal abstract object GetMethodDefaultValue(int index);
			internal abstract object[] GetMethodAnnotations(int index);
			internal abstract object[][] GetParameterAnnotations(int index);
			internal abstract object[] GetFieldAnnotations(int index);
		}

		private sealed class JavaTypeImpl : DynamicImpl
		{
			private readonly ClassFile classFile;
			private readonly DynamicTypeWrapper wrapper;
			private TypeBuilder typeBuilder;
			private MethodWrapper[] methods;
			private MethodWrapper[] baseMethods;
			private FieldWrapper[] fields;
			private FinishedTypeImpl finishedType;
			private Hashtable memberclashtable;
			private Hashtable classCache = Hashtable.Synchronized(new Hashtable());
			private FieldInfo classObjectField;
			private MethodBuilder clinitMethod;
			private MethodBuilder finalizeMethod;
#if STATIC_COMPILER
			private DynamicTypeWrapper outerClassWrapper;
			private AnnotationBuilder annotationBuilder;
			private TypeBuilder enumBuilder;
#endif

			internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper)
			{
				Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
				this.classFile = f;
				this.wrapper = wrapper;
			}

			internal void CreateStep1(out bool hasclinit)
			{
				// process all methods
				hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
				methods = new MethodWrapper[classFile.Methods.Length];
				baseMethods = new MethodWrapper[classFile.Methods.Length];
				for(int i = 0; i < methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					if(m.IsClassInitializer)
					{
#if STATIC_COMPILER
						if(!IsSideEffectFreeStaticInitializer(m))
						{
							hasclinit = true;
						}
#else
						hasclinit = true;
#endif
					}
					MemberFlags flags = MemberFlags.None;
					if(m.IsInternal)
					{
						flags |= MemberFlags.InternalAccess;
					}
					if(wrapper.IsGhost)
					{
						methods[i] = new MethodWrapper.GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
					}
					else if(ReferenceEquals(m.Name, StringConstants.INIT))
					{
						methods[i] = new SmartConstructorMethodWrapper(wrapper, m.Name, m.Signature, null, null, m.Modifiers, flags);
					}
					else
					{
						if(!classFile.IsInterface && !m.IsStatic && !m.IsPrivate)
						{
							bool explicitOverride = false;
							baseMethods[i] = FindBaseMethod(m.Name, m.Signature, out explicitOverride);
							if(explicitOverride)
							{
								flags |= MemberFlags.ExplicitOverride;
							}
						}
						methods[i] = new SmartCallMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags, SimpleOpCode.Call, SimpleOpCode.Callvirt);
					}
				}
				wrapper.HasStaticInitializer = hasclinit;
				if(!wrapper.IsInterface || wrapper.IsPublic)
				{
					ArrayList methodsArray = null;
					ArrayList baseMethodsArray = null;
					if(wrapper.IsAbstract)
					{
						methodsArray = new ArrayList(methods);
						baseMethodsArray = new ArrayList(baseMethods);
						AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
					}
#if STATIC_COMPILER
					if(!wrapper.IsInterface && wrapper.IsPublic)
					{
						TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
						while(baseTypeWrapper != null && !baseTypeWrapper.IsPublic)
						{
							if(methodsArray == null)
							{
								methodsArray = new ArrayList(methods);
								baseMethodsArray = new ArrayList(baseMethods);
							}
							AddAccessStubMethods(methodsArray, baseMethodsArray, baseTypeWrapper);
							baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
						}
					}
#endif
					if(methodsArray != null)
					{
						this.methods = (MethodWrapper[])methodsArray.ToArray(typeof(MethodWrapper));
						this.baseMethods = (MethodWrapper[])baseMethodsArray.ToArray(typeof(MethodWrapper));
					}
				}
				wrapper.SetMethods(methods);

				fields = new FieldWrapper[classFile.Fields.Length];
				for(int i = 0; i < fields.Length; i++)
				{
					ClassFile.Field fld = classFile.Fields[i];
					if(fld.IsStatic && fld.IsFinal && fld.ConstantValue != null)
					{
						TypeWrapper fieldType = null;
#if !STATIC_COMPILER
						fieldType = ClassLoaderWrapper.GetBootstrapClassLoader().FieldTypeWrapperFromSig(fld.Signature);
#endif
						fields[i] = new ConstantFieldWrapper(wrapper, fieldType, fld.Name, fld.Signature, fld.Modifiers, null, fld.ConstantValue, MemberFlags.None);
					}
#if STATIC_COMPILER
					else if(fld.IsFinal
						&& (fld.IsPublic || fld.IsProtected)
						&& wrapper.IsPublic
						&& !wrapper.IsInterface
						&& (!wrapper.classLoader.StrictFinalFieldSemantics || ReferenceEquals(wrapper.Name, StringConstants.JAVA_LANG_SYSTEM)))
					{
						fields[i] = new GetterFieldWrapper(wrapper, null, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal), null, null);
					}
#endif
					else
					{
						fields[i] = FieldWrapper.Create(wrapper, null, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal));
					}
				}
#if STATIC_COMPILER
				if(wrapper.IsPublic)
				{
					ArrayList fieldsArray = new ArrayList(fields);
					AddAccessStubFields(fieldsArray, wrapper);
					fields = (FieldWrapper[])fieldsArray.ToArray(typeof(FieldWrapper));
				}
				((AotTypeWrapper)wrapper).AddMapXmlFields(ref fields);
#endif
				wrapper.SetFields(fields);
			}

			internal void CreateStep2NoFail(bool hasclinit, string mangledTypeName)
			{
				// this method is not allowed to throw exceptions (if it does, the runtime will abort)
				ClassFile f = classFile;
				try
				{
					TypeAttributes typeAttribs = 0;
					if(f.IsAbstract)
					{
						typeAttribs |= TypeAttributes.Abstract;
					}
					if(f.IsFinal)
					{
						typeAttribs |= TypeAttributes.Sealed;
					}
					if(!hasclinit)
					{
						typeAttribs |= TypeAttributes.BeforeFieldInit;
					}
#if STATIC_COMPILER
					bool cantNest = false;
					bool setModifiers = false;
					TypeBuilder outer = null;
					// we only compile inner classes as nested types in the static compiler, because it has a higher cost
					// and doesn't buy us anything in dynamic mode (and if fact, due to an FXBUG it would make handling
					// the TypeResolve event very hard)
					ClassFile.InnerClass outerClass = getOuterClass();
					if(outerClass.outerClass != 0)
					{
						string outerClassName = classFile.GetConstantPoolClass(outerClass.outerClass);
						if(!CheckInnerOuterNames(f.Name, outerClassName))
						{
							Tracer.Warning(Tracer.Compiler, "Incorrect InnerClasses attribute on {0}", f.Name);
						}
						else
						{
							try
							{
								outerClassWrapper = wrapper.GetClassLoader().LoadClassByDottedNameFast(outerClassName) as DynamicTypeWrapper;
							}
							catch(RetargetableJavaException x)
							{
								Tracer.Warning(Tracer.Compiler, "Unable to load outer class {0} for inner class {1} ({2}: {3})", outerClassName, f.Name, x.GetType().Name, x.Message);
							}
							if(outerClassWrapper != null)
							{
								// make sure the relationship is reciprocal (otherwise we run the risk of
								// baking the outer type before the inner type)
								if(outerClassWrapper.impl is JavaTypeImpl)
								{
									ClassFile outerClassFile = ((JavaTypeImpl)outerClassWrapper.impl).classFile;
									ClassFile.InnerClass[] outerInnerClasses = outerClassFile.InnerClasses;
									if(outerInnerClasses == null)
									{
										outerClassWrapper = null;
									}
									else
									{
										bool ok = false;
										for(int i = 0; i < outerInnerClasses.Length; i++)
										{
											if(outerInnerClasses[i].outerClass != 0
												&& outerClassFile.GetConstantPoolClass(outerInnerClasses[i].outerClass) == outerClassFile.Name
												&& outerInnerClasses[i].innerClass != 0
												&& outerClassFile.GetConstantPoolClass(outerInnerClasses[i].innerClass) == f.Name)
											{
												ok = true;
												break;
											}
										}
										if(!ok)
										{
											outerClassWrapper = null;
										}
									}
								}
								else
								{
									outerClassWrapper = null;
								}
								if(outerClassWrapper != null)
								{
									outer = outerClassWrapper.TypeAsBuilder;
								}
								else
								{
									Tracer.Warning(Tracer.Compiler, "Non-reciprocal inner class {0}", f.Name);
								}
							}
						}
					}
					if(f.IsPublic)
					{
						if(outer != null)
						{
							if(outerClassWrapper.IsPublic)
							{
								typeAttribs |= TypeAttributes.NestedPublic;
							}
							else
							{
								// We're a public type nested inside a non-public type, this means that we can't compile this type as a nested type,
								// because that would mean it wouldn't be visible outside the assembly.
								cantNest = true;
								typeAttribs |= TypeAttributes.Public;
							}
						}
						else
						{
							typeAttribs |= TypeAttributes.Public;
						}
					}
					else if(outer != null)
					{
						typeAttribs |= TypeAttributes.NestedAssembly;
					}
#else // STATIC_COMPILER
					if(f.IsPublic)
					{
						typeAttribs |= TypeAttributes.Public;
					}
#endif // STATIC_COMPILER
					if(f.IsInterface)
					{
						typeAttribs |= TypeAttributes.Interface | TypeAttributes.Abstract;
#if STATIC_COMPILER
						if(outer != null && !cantNest)
						{
							if(wrapper.IsGhost)
							{
								// TODO this is low priority, since the current Java class library doesn't define any ghost interfaces
								// as inner classes
								throw new NotImplementedException();
							}
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs);
						}
						else
						{
							if(wrapper.IsGhost)
							{
								typeBuilder = wrapper.DefineGhostType(mangledTypeName, typeAttribs);
							}
							else
							{
								typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
							}
						}
#else // STATIC_COMPILER
						typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
#endif // STATIC_COMPILER
					}
					else
					{
						typeAttribs |= TypeAttributes.Class;
#if STATIC_COMPILER
						if(f.IsEffectivelyFinal)
						{
							if(outer == null)
							{
								setModifiers = true;
							}
							else
							{
								// we don't need a ModifiersAttribute, because the InnerClassAttribute already records
								// the modifiers
							}
							typeAttribs |= TypeAttributes.Sealed;
							Tracer.Info(Tracer.Compiler, "Sealing type {0}", f.Name);
						}
						if(outer != null && !cantNest)
						{
							// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
							// (and broken by J# as well), so we'll just ignore it too.
							typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs, wrapper.BaseTypeWrapper.TypeAsBaseType);
						}
						else
#endif // STATIC_COMPILER
						{
							typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs, wrapper.BaseTypeWrapper.TypeAsBaseType);
						}
					}
#if STATIC_COMPILER
					if(outer != null && cantNest)
					{
						AttributeHelper.SetNonNestedOuterClass(typeBuilder, outerClassWrapper.Name);
						AttributeHelper.SetNonNestedInnerClass(outer, f.Name);
					}
					if(outer == null && mangledTypeName != wrapper.Name)
					{
						// HACK we abuse the InnerClassAttribute to record to real name
						AttributeHelper.SetInnerClass(typeBuilder, wrapper.Name, wrapper.Modifiers);
					}
					if(typeBuilder.FullName != wrapper.Name
						&& wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
					{
						((CompilerClassLoader)wrapper.GetClassLoader()).AddNameMapping(wrapper.Name, typeBuilder.FullName);
					}
					string annotationAttributeType = null;
					if(f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
					{
						annotationBuilder = new AnnotationBuilder(this);
						((AotTypeWrapper)wrapper).SetAnnotation(annotationBuilder);
						annotationAttributeType = annotationBuilder.AttributeTypeName;
					}
					// For Java 5 Enum types, we generate a nested .NET enum.
					// This is primarily to support annotations that take enum parameters.
					if(f.IsEnum && f.IsPublic)
					{
						// TODO make sure there isn't already a nested type with the __Enum name
						enumBuilder = wrapper.TypeAsBuilder.DefineNestedType("__Enum", TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, typeof(Enum));
						AttributeHelper.HideFromJava(enumBuilder);
						enumBuilder.DefineField("value__", typeof(int), FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
						for(int i = 0; i < f.Fields.Length; i++)
						{
							if(f.Fields[i].IsEnum)
							{
								FieldBuilder fieldBuilder = enumBuilder.DefineField(f.Fields[i].Name, enumBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
								fieldBuilder.SetConstant(i);
							}
						}
					}
					string sourceFile = null;
					if(wrapper.classLoader.EmitStackTraceInfo)
					{
						if(f.SourceFileAttribute != null)
						{
							if(f.SourceFileAttribute != typeBuilder.Name + ".java")
							{
								sourceFile = f.SourceFileAttribute;
							}
						}
					}
					TypeWrapper[] interfaces = wrapper.Interfaces;
					string[] implements = new string[interfaces.Length];
					for(int i = 0; i < implements.Length; i++)
					{
						implements[i] = interfaces[i].Name;
					}
					if(outer != null)
					{
						Modifiers innerClassModifiers = outerClass.accessFlags;
						string innerClassName = classFile.GetConstantPoolClass(outerClass.innerClass);
						if(innerClassName == classFile.Name && innerClassName == outerClassWrapper.Name + "$" + typeBuilder.Name)
						{
							innerClassName = null;
						}
						AttributeHelper.SetInnerClass(typeBuilder, innerClassName, innerClassModifiers);
					}
					else if(outerClass.innerClass != 0)
					{
						AttributeHelper.SetInnerClass(typeBuilder, null, outerClass.accessFlags);
					}
					string enclosingMethodClass = null;
					string enclosingMethodName = null;
					string enclosingMethodSig = null;
					if(classFile.EnclosingMethod != null)
					{
						enclosingMethodClass = classFile.EnclosingMethod[0];
						enclosingMethodName = classFile.EnclosingMethod[1];
						enclosingMethodSig = classFile.EnclosingMethod[2];
					}
					AttributeHelper.SetImplementsAttribute(typeBuilder, interfaces);
					if(classFile.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(typeBuilder);
					}
					if(classFile.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(typeBuilder, classFile.GenericSignature);
					}
					if(classFile.EnclosingMethod != null)
					{
						AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, classFile.EnclosingMethod[0], classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
					}
					if(annotationAttributeType != null)
					{
						CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(AnnotationAttributeAttribute)).GetConstructor(new Type[] { typeof(string) }), new object[] { annotationAttributeType });
						typeBuilder.SetCustomAttribute(cab);
					}
					if(wrapper.classLoader.EmitStackTraceInfo)
					{
						if(f.SourceFileAttribute != null)
						{
							if(f.SourceFileAttribute != typeBuilder.Name + ".java")
							{
								AttributeHelper.SetSourceFile(typeBuilder, f.SourceFileAttribute);
							}
						}
						else
						{
							AttributeHelper.SetSourceFile(typeBuilder, null);
						}
					}
					// NOTE in Whidbey we can (and should) use CompilerGeneratedAttribute to mark Synthetic types
					if(setModifiers || classFile.IsInternal || (classFile.Modifiers & (Modifiers.Synthetic | Modifiers.Annotation | Modifiers.Enum)) != 0)
					{
						AttributeHelper.SetModifiers(typeBuilder, classFile.Modifiers, classFile.IsInternal);
					}
#endif // STATIC_COMPILER
					if(hasclinit)
					{
						// We create a empty method that we can use to trigger our .cctor
						// (previously we used RuntimeHelpers.RunClassConstructor, but that is slow and requires additional privileges)
						MethodAttributes attribs = MethodAttributes.Static | MethodAttributes.SpecialName;
						if(classFile.IsAbstract)
						{
							bool hasfields = false;
							// If we have any public static fields, the cctor trigger must (and may) be public as well
							foreach(ClassFile.Field fld in classFile.Fields)
							{
								if(fld.IsPublic && fld.IsStatic)
								{
									hasfields = true;
									break;
								}
							}
							attribs |= hasfields ? MethodAttributes.Public : MethodAttributes.FamORAssem;
						}
						else
						{
							attribs |= MethodAttributes.Public;
						}
						clinitMethod = typeBuilder.DefineMethod("__<clinit>", attribs, null, null);
						clinitMethod.GetILGenerator().Emit(OpCodes.Ret);
						// FXBUG on .NET 2.0 RTM x64 the JIT sometimes throws an InvalidProgramException while trying to inline this method,
						// so we prevent inlining for now (it also turns out that on x86 not inlining this method actually has a positive perf impact in some cases...)
						// http://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=285772
						clinitMethod.SetImplementationFlags(clinitMethod.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
					}
					if(HasStructLayoutAttributeAnnotation(classFile))
					{
						// when we have a StructLayoutAttribute, field order is significant,
						// so we link all fields here to make sure they are created in class file order.
						foreach(FieldWrapper fw in fields)
						{
							fw.Link();
						}
					}
				}
				catch(Exception x)
				{
					JVM.CriticalFailure("Exception during JavaTypeImpl.CreateStep2NoFail", x);
				}
			}

			private static bool HasStructLayoutAttributeAnnotation(ClassFile c)
			{
				if(c.Annotations != null)
				{
					foreach(object[] annot in c.Annotations)
					{
						if("Lcli/System/Runtime/InteropServices/StructLayoutAttribute$Annotation;".Equals(annot[1]))
						{
							return true;
						}
					}
				}
				return false;
			}

			private ClassFile.InnerClass getOuterClass()
			{
				ClassFile.InnerClass[] innerClasses = classFile.InnerClasses;
				if(innerClasses != null)
				{
					for(int j = 0; j < innerClasses.Length; j++)
					{
						if(innerClasses[j].innerClass != 0
							&& classFile.GetConstantPoolClass(innerClasses[j].innerClass) == classFile.Name)
						{
							return innerClasses[j];
						}
					}
				}
				return new ClassFile.InnerClass();
			}

#if STATIC_COMPILER
			private bool IsSideEffectFreeStaticInitializer(ClassFile.Method m)
			{
				if(m.ExceptionTable.Length != 0)
				{
					return false;
				}
				for(int i = 0; i < m.Instructions.Length; i++)
				{
					NormalizedByteCode bc = m.Instructions[i].NormalizedOpCode;
					if(bc == NormalizedByteCode.__getstatic || bc == NormalizedByteCode.__putstatic)
					{
						ClassFile.ConstantPoolItemFieldref fld = classFile.SafeGetFieldref(m.Instructions[i].Arg1);
						if(fld == null || fld.Class != classFile.Name)
						{
							return false;
						}
						// don't allow getstatic to load non-primitive fields, because that would
						// cause the verifier to try to load the type
						if(bc == NormalizedByteCode.__getstatic && "L[".IndexOf(fld.Signature[0]) != -1)
						{
							return false;
						}
					}
					else if(bc == NormalizedByteCode.__areturn ||
						bc == NormalizedByteCode.__ireturn ||
						bc == NormalizedByteCode.__lreturn ||
						bc == NormalizedByteCode.__freturn ||
						bc == NormalizedByteCode.__dreturn)
					{
						return false;
					}
					else if(ByteCodeMetaData.CanThrowException(bc))
					{
						return false;
					}
					else if(bc == NormalizedByteCode.__ldc
						&& classFile.SafeIsConstantPoolClass(m.Instructions[i].Arg1))
					{
						return false;
					}
				}
				// the method needs to be verifiable to be side effect free, since we already analysed it,
				// we know that the verifier won't try to load any types (which isn't allowed at this time)
				try
				{
					new MethodAnalyzer(wrapper, null, classFile, m, wrapper.classLoader);
					return true;
				}
				catch(VerifyError)
				{
					return false;
				}
			}
#endif // STATIC_COMPILER

			private static bool ContainsMemberWrapper(ArrayList members, string name, string sig)
			{
				foreach(MemberWrapper mw in members)
				{
					if(mw.Name == name && mw.Signature == sig)
					{
						return true;
					}
				}
				return false;
			}

			private MethodWrapper GetMethodWrapperDuringCtor(TypeWrapper lookup, ArrayList methods, string name, string sig)
			{
				if(lookup == wrapper)
				{
					foreach(MethodWrapper mw in methods)
					{
						if(mw.Name == name && mw.Signature == sig)
						{
							return mw;
						}
					}
					if(lookup.BaseTypeWrapper == null)
					{
						return null;
					}
					else
					{
						return lookup.BaseTypeWrapper.GetMethodWrapper(name, sig, true);
					}
				}
				else
				{
					return lookup.GetMethodWrapper(name, sig, true);
				}
			}

			private void AddMirandaMethods(ArrayList methods, ArrayList baseMethods, TypeWrapper tw)
			{
				foreach(TypeWrapper iface in tw.Interfaces)
				{
					if(iface.IsPublic && this.wrapper.IsInterface)
					{
						// for interfaces, we only need miranda methods for non-public interfaces that we extend
						continue;
					}
					AddMirandaMethods(methods, baseMethods, iface);
					foreach(MethodWrapper ifmethod in iface.GetMethods())
					{
						// skip <clinit>
						if(!ifmethod.IsStatic)
						{
							TypeWrapper lookup = wrapper;
							while(lookup != null)
							{
								MethodWrapper mw = GetMethodWrapperDuringCtor(lookup, methods, ifmethod.Name, ifmethod.Signature);
								if(mw == null)
								{
									mw = new SmartCallMethodWrapper(wrapper, ifmethod.Name, ifmethod.Signature, null, null, null, Modifiers.Public | Modifiers.Abstract, MemberFlags.HideFromReflection | MemberFlags.MirandaMethod, SimpleOpCode.Call, SimpleOpCode.Callvirt);
									methods.Add(mw);
									baseMethods.Add(ifmethod);
									break;
								}
								if(!mw.IsStatic)
								{
									break;
								}
								lookup = mw.DeclaringType.BaseTypeWrapper;
							}
						}
					}
				}
			}

			private void AddAccessStubMethods(ArrayList methods, ArrayList baseMethods, TypeWrapper tw)
			{
				foreach(MethodWrapper mw in tw.GetMethods())
				{
					if((mw.IsPublic || mw.IsProtected)
						&& mw.Name != "<init>"
						&& !ContainsMemberWrapper(methods, mw.Name, mw.Signature))
					{
						MethodWrapper stub = new SmartCallMethodWrapper(wrapper, mw.Name, mw.Signature, null, null, null, mw.Modifiers, MemberFlags.HideFromReflection | MemberFlags.AccessStub, SimpleOpCode.Call, SimpleOpCode.Callvirt);
						methods.Add(stub);
						baseMethods.Add(mw);
					}
				}
			}

			private void AddAccessStubFields(ArrayList fields, TypeWrapper tw)
			{
				do
				{
					if(!tw.IsPublic)
					{
						foreach(FieldWrapper fw in tw.GetFields())
						{
							if((fw.IsPublic || fw.IsProtected)
								&& !ContainsMemberWrapper(fields, fw.Name, fw.Signature))
							{
								fields.Add(new AotAccessStubFieldWrapper(wrapper, fw));
							}
						}
					}
					foreach(TypeWrapper iface in tw.Interfaces)
					{
						AddAccessStubFields(fields, iface);
					}
					tw = tw.BaseTypeWrapper;
				} while(tw != null && !tw.IsPublic);
			}

			private static bool CheckInnerOuterNames(string inner, string outer)
			{
				// do some sanity checks on the inner/outer class names
				return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.StartsWith(outer);
			}

			private static string GetInnerClassName(string outer, string inner)
			{
				Debug.Assert(CheckInnerOuterNames(inner, outer));
				return DynamicClassLoader.EscapeName(inner.Substring(outer.Length + 1));
			}

			private static bool IsCompatibleArgList(TypeWrapper[] caller, TypeWrapper[] callee)
			{
				if(caller.Length == callee.Length)
				{
					for(int i = 0; i < caller.Length; i++)
					{
						if(!caller[i].IsAssignableTo(callee[i]))
						{
							return false;
						}
					}
					return true;
				}
				return false;
			}

			private void EmitConstantValueInitialization(ILGenerator ilGenerator)
			{
				ClassFile.Field[] fields = classFile.Fields;
				for(int i = 0; i < fields.Length; i++)
				{
					ClassFile.Field f = fields[i];
					if(f.IsStatic && !f.IsFinal)
					{
						object constant = f.ConstantValue;
						if(constant != null)
						{
							if(constant is int)
							{
								ilGenerator.Emit(OpCodes.Ldc_I4, (int)constant);
							}
							else if(constant is long)
							{
								ilGenerator.Emit(OpCodes.Ldc_I8, (long)constant);
							}
							else if(constant is double)
							{
								ilGenerator.Emit(OpCodes.Ldc_R8, (double)constant);
							}
							else if(constant is float)
							{
								ilGenerator.Emit(OpCodes.Ldc_R4, (float)constant);
							}
							else if(constant is string)
							{
								ilGenerator.Emit(OpCodes.Ldstr, (string)constant);
							}
							else
							{
								throw new InvalidOperationException();
							}
							this.fields[i].EmitSet(ilGenerator);
						}
					}
				}
			}

			internal FieldInfo ClassObjectField
			{
				get
				{
					lock(this)
					{
						if(classObjectField == null)
						{
							classObjectField = typeBuilder.DefineField("__<classObject>", typeof(object), FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.SpecialName);
						}
						return classObjectField;
					}
				}
			}

			private int GetMethodIndex(MethodWrapper mw)
			{
				for(int i = 0; i < methods.Length; i++)
				{
					if(methods[i] == mw)
					{
						return i;
					}
				}
				throw new InvalidOperationException();
			}

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				Debug.Assert(mw != null);
				bool unloadableOverrideStub = false;
				int index = GetMethodIndex(mw);
				MethodWrapper baseMethod = baseMethods[index];
				if(baseMethod != null)
				{
					baseMethod.Link();
					// check the loader constraints
					if(mw.ReturnType != baseMethod.ReturnType)
					{
						if(baseMethod.ReturnType.IsUnloadable || JVM.FinishingForDebugSave)
						{
							if(!mw.ReturnType.IsUnloadable || (!baseMethod.ReturnType.IsUnloadable && JVM.FinishingForDebugSave))
							{
								unloadableOverrideStub = true;
							}
						}
						else
						{
#if STATIC_COMPILER
							StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has a return type \"{1}\"", mw.ReturnType, baseMethod.ReturnType, mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#endif
							throw new LinkageError("Loader constraints violated");
						}
					}
					TypeWrapper[] here = mw.GetParameters();
					TypeWrapper[] there = baseMethod.GetParameters();
					for(int i = 0; i < here.Length; i++)
					{
						if(here[i] != there[i])
						{
							if(there[i].IsUnloadable || JVM.FinishingForDebugSave)
							{
								if(!here[i].IsUnloadable || (!there[i].IsUnloadable && JVM.FinishingForDebugSave))
								{
									unloadableOverrideStub = true;
								}
							}
							else
							{
#if STATIC_COMPILER
								StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has an argument type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has an argument type \"{1}\"", here[i], there[i], mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#endif
								throw new LinkageError("Loader constraints violated");
							}
						}
					}
				}
				Debug.Assert(mw.GetMethod() == null);
				MethodBase mb = GenerateMethod(index, unloadableOverrideStub);
				if((mw.Modifiers & (Modifiers.Synchronized | Modifiers.Static)) == Modifiers.Synchronized)
				{
					// note that constructors cannot be synchronized in Java
					MethodBuilder mbld = (MethodBuilder)mb;
					mbld.SetImplementationFlags(mbld.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);
				}
				return mb;
			}

			private int GetFieldIndex(FieldWrapper fw)
			{
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i] == fw)
					{
						return i;
					}
				}
				throw new InvalidOperationException();
			}

			internal override FieldInfo LinkField(FieldWrapper fw)
			{
				if(fw.IsAccessStub)
				{
					((AotAccessStubFieldWrapper)fw).DoLink(typeBuilder);
					return null;
				}
				int fieldIndex = GetFieldIndex(fw);
#if STATIC_COMPILER
				if(fieldIndex >= classFile.Fields.Length)
				{
					// this must be a field defined in map.xml
					FieldAttributes fieldAttribs = 0;
					if(fw.IsPublic)
					{
						fieldAttribs |= FieldAttributes.Public;
					}
					else if(fw.IsProtected)
					{
						fieldAttribs |= FieldAttributes.FamORAssem;
					}
					else if(fw.IsPrivate)
					{
						fieldAttribs |= FieldAttributes.Private;
					}
					else
					{
						fieldAttribs |= FieldAttributes.Assembly;
					}
					if(fw.IsStatic)
					{
						fieldAttribs |= FieldAttributes.Static;
					}
					if(fw.IsFinal)
					{
						fieldAttribs |= FieldAttributes.InitOnly;
					}
					return typeBuilder.DefineField(fw.Name, fw.FieldTypeWrapper.TypeAsSignatureType, fieldAttribs);
				}
#endif // STATIC_COMPILER
				FieldBuilder field;
				ClassFile.Field fld = classFile.Fields[fieldIndex];
				string fieldName = fld.Name;
				TypeWrapper typeWrapper = fw.FieldTypeWrapper;
				Type type = typeWrapper.TypeAsSignatureType;
				bool setNameSig = typeWrapper.IsErasedOrBoxedPrimitiveOrRemapped;
				if(setNameSig)
				{
					// TODO use clashtable
					// the field name is mangled here, because otherwise it can (theoretically)
					// conflict with another unloadable or object or ghost array field
					// (fields can be overloaded on type)
					fieldName += "/" + typeWrapper.Name;
				}
				FieldAttributes attribs = 0;
				MethodAttributes methodAttribs = MethodAttributes.HideBySig;
#if STATIC_COMPILER
				bool setModifiers = fld.IsInternal || (fld.Modifiers & (Modifiers.Synthetic | Modifiers.Enum)) != 0;
#endif
				bool isWrappedFinal = false;
				if(fld.IsPrivate)
				{
					attribs |= FieldAttributes.Private;
				}
				else if(fld.IsProtected)
				{
					attribs |= FieldAttributes.FamORAssem;
					methodAttribs |= MethodAttributes.FamORAssem;
				}
				else if(fld.IsPublic)
				{
					attribs |= FieldAttributes.Public;
					methodAttribs |= MethodAttributes.Public;
				}
				else
				{
					attribs |= FieldAttributes.Assembly;
					methodAttribs |= MethodAttributes.Assembly;
				}
				if(fld.IsStatic)
				{
					attribs |= FieldAttributes.Static;
					methodAttribs |= MethodAttributes.Static;
				}
				// NOTE "constant" static finals are converted into literals
				// TODO it would be possible for Java code to change the value of a non-blank static final, but I don't
				// know if we want to support this (since the Java JITs don't really support it either)
				object constantValue = fld.ConstantValue;
				if(fld.IsStatic && fld.IsFinal && constantValue != null)
				{
					Profiler.Count("Static Final Constant");
					attribs |= FieldAttributes.Literal;
					field = typeBuilder.DefineField(fieldName, type, attribs);
					field.SetConstant(constantValue);
				}
				else
				{
					if(fld.IsFinal)
					{
						isWrappedFinal = fw is GetterFieldWrapper;
						if(isWrappedFinal)
						{
							// NOTE public/protected blank final fields get converted into a read-only property with a private field
							// backing store
							attribs &= ~FieldAttributes.FieldAccessMask;
							attribs |= FieldAttributes.PrivateScope;
						}
						else if(wrapper.IsInterface || wrapper.classLoader.StrictFinalFieldSemantics)
						{
							attribs |= FieldAttributes.InitOnly;
						}
						else
						{
#if STATIC_COMPILER
							setModifiers = true;
#endif
						}
					}
					Type[] modreq = Type.EmptyTypes;
					if(fld.IsVolatile)
					{
						modreq = new Type[] { typeof(System.Runtime.CompilerServices.IsVolatile) };
					}
					// MONOBUG the __<> prefix for wrapped final fields is to work around a bug in mcs 1.1.17
					// it crashes when it tries to lookup the property with the same name as the privatescope field
					// http://bugzilla.ximian.com/show_bug.cgi?id=79451
					field = typeBuilder.DefineField(isWrappedFinal ? "__<>" + fieldName : fieldName, type, modreq, Type.EmptyTypes, attribs);
					if(fld.IsTransient)
					{
						CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
						field.SetCustomAttribute(transientAttrib);
					}
#if STATIC_COMPILER
					// Instance fields can also have a ConstantValue attribute (and are inlined by the compiler),
					// and ikvmstub has to export them, so we have to add a custom attribute.
					if(constantValue != null)
					{
						AttributeHelper.SetConstantValue(field, constantValue);
					}
#endif // STATIC_COMPILER
					if(isWrappedFinal)
					{
						methodAttribs |= MethodAttributes.SpecialName;
						MethodBuilder getter = typeBuilder.DefineMethod(GenerateUniqueMethodName("get_" + fieldName, type, Type.EmptyTypes), methodAttribs, CallingConventions.Standard, type, Type.EmptyTypes);
						AttributeHelper.HideFromJava(getter);
						ILGenerator ilgen = getter.GetILGenerator();
						if(fld.IsStatic)
						{
							ilgen.Emit(OpCodes.Ldsfld, field);
						}
						else
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldfld, field);
						}
						ilgen.Emit(OpCodes.Ret);

						PropertyBuilder pb = typeBuilder.DefineProperty(fieldName, PropertyAttributes.None, type, Type.EmptyTypes);
						pb.SetGetMethod(getter);
						if(!fld.IsStatic)
						{
							// this method exist for use by reflection only
							// (that's why it only exists for instance fields, final static fields are not settable by reflection)
							MethodBuilder setter = typeBuilder.DefineMethod("__<set>", MethodAttributes.PrivateScope, CallingConventions.Standard, typeof(void), new Type[] { type });
							ilgen = setter.GetILGenerator();
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldarg_1);
							ilgen.Emit(OpCodes.Stfld, field);
							ilgen.Emit(OpCodes.Ret);
							pb.SetSetMethod(setter);
						}
						((GetterFieldWrapper)fw).SetGetter(getter);
#if STATIC_COMPILER
						if(setNameSig)
						{
							AttributeHelper.SetNameSig(getter, fld.Name, fld.Signature);
						}
						if(setModifiers || fld.IsTransient)
						{
							AttributeHelper.SetModifiers(getter, fld.Modifiers, fld.IsInternal);
						}
						if(fld.DeprecatedAttribute)
						{
							// NOTE for better interop with other languages, we set the ObsoleteAttribute on the property itself
							AttributeHelper.SetDeprecatedAttribute(pb);
						}
						if(fld.GenericSignature != null)
						{
							AttributeHelper.SetSignatureAttribute(getter, fld.GenericSignature);
						}
#endif // STATIC_COMPILER
					}
				}
#if STATIC_COMPILER
				if(!isWrappedFinal)
				{
					// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
					// the Java modifiers
					if(setModifiers)
					{
						AttributeHelper.SetModifiers(field, fld.Modifiers, fld.IsInternal);
					}
					if(setNameSig)
					{
						AttributeHelper.SetNameSig(field, fld.Name, fld.Signature);
					}
					if(fld.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(field);
					}
					if(fld.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(field, fld.GenericSignature);
					}
				}
#endif // STATIC_COMPILER
				return field;
			}

			internal override void EmitRunClassConstructor(ILGenerator ilgen)
			{
				if(clinitMethod != null)
				{
					ilgen.Emit(OpCodes.Call, clinitMethod);
				}
			}

			internal override DynamicImpl Finish()
			{
				if(wrapper.BaseTypeWrapper != null)
				{
					wrapper.BaseTypeWrapper.Finish();
				}
#if STATIC_COMPILER
				if(outerClassWrapper != null)
				{
					outerClassWrapper.Finish();
				}
#endif // STATIC_COMPILER
				// NOTE there is a bug in the CLR (.NET 1.0 & 1.1 [1.2 is not yet available]) that
				// causes the AppDomain.TypeResolve event to receive the incorrect type name for nested types.
				// The Name in the ResolveEventArgs contains only the nested type name, not the full type name,
				// for example, if the type being resolved is "MyOuterType+MyInnerType", then the event only
				// receives "MyInnerType" as the name. Since we only compile inner classes as nested types
				// when we're statically compiling, we can only run into this bug when we're statically compiling.
				// NOTE To work around this bug, we have to make sure that all types that are going to be
				// required in finished form, are finished explicitly here. It isn't clear what other types are
				// required to be finished. I instrumented a static compilation of classpath.dll and this
				// turned up no other cases of the TypeResolve event firing.
				for(int i = 0; i < wrapper.Interfaces.Length; i++)
				{
					wrapper.Interfaces[i].Finish();
				}
				// make sure all classes are loaded, before we start finishing the type. During finishing, we
				// may not run any Java code, because that might result in a request to finish the type that we
				// are in the process of finishing, and this would be a problem.
				classFile.Link(wrapper, classCache);
				for(int i = 0; i < fields.Length; i++)
				{
#if STATIC_COMPILER
					if(fields[i] is AotAccessStubFieldWrapper)
					{
						// HACK we skip access stubs, because we want to do the methods first
						// (to prevent the stub method from taking the name of a real method)
						continue;
					}
#endif
					fields[i].Link();
				}
				for(int i = 0; i < methods.Length; i++)
				{
					methods[i].Link();
				}
#if STATIC_COMPILER
				// HACK second pass for the access stubs (see above)
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i] is AotAccessStubFieldWrapper)
					{
						fields[i].Link();
					}
				}
#endif
				// this is the correct lock, FinishCore doesn't call any user code and mutates global state,
				// so it needs to be protected by a lock.
				lock(this)
				{
					return FinishCore();
				}
			}

			private FinishedTypeImpl FinishCore()
			{
				// it is possible that the loading of the referenced classes triggered a finish of us,
				// if that happens, we just return
				if(finishedType != null)
				{
					return finishedType;
				}
				Tracer.Info(Tracer.Compiler, "Finishing: {0}", wrapper.Name);
				Profiler.Enter("JavaTypeImpl.Finish.Core");
				try
				{
					TypeWrapper declaringTypeWrapper = null;
					TypeWrapper[] innerClassesTypeWrappers = TypeWrapper.EmptyArray;
					// if we're an inner class, we need to attach an InnerClass attribute
					ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
					if(innerclasses != null)
					{
						// TODO consider not pre-computing innerClassesTypeWrappers and declaringTypeWrapper here
						ArrayList wrappers = new ArrayList();
						for(int i = 0; i < innerclasses.Length; i++)
						{
							if(innerclasses[i].innerClass != 0 && innerclasses[i].outerClass != 0)
							{
								if(classFile.GetConstantPoolClassType(innerclasses[i].outerClass) == wrapper)
								{
									wrappers.Add(classFile.GetConstantPoolClassType(innerclasses[i].innerClass));
								}
								if(classFile.GetConstantPoolClassType(innerclasses[i].innerClass) == wrapper)
								{
									declaringTypeWrapper = classFile.GetConstantPoolClassType(innerclasses[i].outerClass);
								}
							}
						}
						innerClassesTypeWrappers = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
					}
#if STATIC_COMPILER
					wrapper.FinishGhost(typeBuilder, methods);
#endif // STATIC_COMPILER
					// if we're not abstract make sure we don't inherit any abstract methods
					if(!wrapper.IsAbstract)
					{
						TypeWrapper parent = wrapper.BaseTypeWrapper;
						// if parent is not abstract, the .NET implementation will never have abstract methods (only
						// stubs that throw AbstractMethodError)
						// NOTE interfaces are supposed to be abstract, but the VM doesn't enforce this, so
						// we have to check for a null parent (interfaces have no parent).
						while(parent != null && parent.IsAbstract)
						{
							foreach(MethodWrapper mw in parent.GetMethods())
							{
								MethodInfo mi = mw.GetMethod() as MethodInfo;
								if(mi != null && mi.IsAbstract && !mi.DeclaringType.IsInterface)
								{
									bool needStub = false;
									bool needRename = false;
									if(mw.IsPublic || mw.IsProtected)
									{
										MethodWrapper fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
										while(fmw != mw && (fmw.IsStatic || fmw.IsPrivate))
										{
											needRename = true;
											fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
										}
										if(fmw == mw && fmw.DeclaringType != wrapper)
										{
											needStub = true;
										}
									}
									else
									{
										MethodWrapper fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
										while(fmw != mw && (fmw.IsStatic || fmw.IsPrivate || !fmw.DeclaringType.IsInSamePackageAs(mw.DeclaringType)))
										{
											needRename = true;
											fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
										}
										if(fmw == mw && fmw.DeclaringType != wrapper)
										{
											needStub = true;
										}
									}
									if(needStub)
									{
										// NOTE in Sun's JRE 1.4.1 this method cannot be overridden by subclasses,
										// but I think this is a bug, so we'll support it anyway.
										string name = mi.Name;
										MethodAttributes attr = mi.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.NewSlot);
										if(needRename)
										{
											name = "__<>" + name + "/" + mi.DeclaringType.FullName;
											attr = MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot;
										}
										MethodBuilder mb = typeBuilder.DefineMethod(name, attr, CallingConventions.Standard, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
										if(needRename)
										{
											typeBuilder.DefineMethodOverride(mb, mi);
										}
										AttributeHelper.HideFromJava(mb);
										EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", mw.DeclaringType.Name + "." + mw.Name + mw.Signature);
									}
								}
							}
							parent = parent.BaseTypeWrapper;
						}
					}
					Hashtable invokespecialstubcache = new Hashtable();
					bool basehasclinit = wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasStaticInitializer;
					bool hasclinit = false;
					bool hasConstructor = false;
					for(int i = 0; i < classFile.Methods.Length; i++)
					{
						ClassFile.Method m = classFile.Methods[i];
						MethodBase mb = methods[i].GetMethod();
						if(mb is ConstructorBuilder)
						{
							ILGenerator ilGenerator = ((ConstructorBuilder)mb).GetILGenerator();
							TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
							if(m.IsClassInitializer)
							{
								if(basehasclinit && !classFile.IsInterface)
								{
									hasclinit = true;
									// before we call the base class initializer, we need to set the non-final static ConstantValue fields
									EmitConstantValueInitialization(ilGenerator);
									wrapper.BaseTypeWrapper.EmitRunClassConstructor(ilGenerator);
								}
							}
							else
							{
								hasConstructor = true;
							}
#if STATIC_COMPILER
							// do we have a native implementation in map.xml?
							if(wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
							{
								continue;
							}
#endif
							LineNumberTableAttribute.LineNumberWriter lineNumberTable = null;
							bool nonLeaf = false;
							Compiler.Compile(wrapper, methods[i], classFile, m, ilGenerator, ref nonLeaf, invokespecialstubcache, ref lineNumberTable);
							if(lineNumberTable != null)
							{
#if STATIC_COMPILER
								AttributeHelper.SetLineNumberTable(methods[i].GetMethod(), lineNumberTable);
#else // STATIC_COMPILER
								if(wrapper.lineNumberTables == null)
								{
									wrapper.lineNumberTables = new byte[methods.Length][];
								}
								wrapper.lineNumberTables[i] = lineNumberTable.ToArray();
#endif // STATIC_COMPILER
							}
						}
						else
						{
							if(m.IsAbstract)
							{
								bool stub = false;
								if(!classFile.IsAbstract)
								{
									// NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
									// the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
									stub = true;
								}
								else if(classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
								{
									// We have an abstract package accessible method in our public class. To allow a class in another
									// assembly to subclass this class, we must fake the abstractness of this method.
									stub = true;
								}
								if(stub)
								{
									ILGenerator ilGenerator = ((MethodBuilder)mb).GetILGenerator();
									TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
									EmitHelper.Throw(ilGenerator, "java.lang.AbstractMethodError", classFile.Name + "." + m.Name + m.Signature);
								}
							}
							else if(m.IsNative)
							{
								if((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
								{
									continue;
								}
								Profiler.Enter("JavaTypeImpl.Finish.Native");
								try
								{
									ILGenerator ilGenerator = ((MethodBuilder)mb).GetILGenerator();
									TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
									// do we have a native implementation in map.xml?
									if(wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
									{
										continue;
									}
#endif
									// see if there exists a IKVM.NativeCode class for this type
									Type nativeCodeType = null;
#if STATIC_COMPILER
									nativeCodeType = StaticCompiler.GetType("IKVM.NativeCode." + classFile.Name.Replace('$', '+'), false);
#endif
									MethodInfo nativeMethod = null;
									TypeWrapper[] args = methods[i].GetParameters();
									if(nativeCodeType != null)
									{
										TypeWrapper[] nargs = args;
										if(!m.IsStatic)
										{
											nargs = new TypeWrapper[args.Length + 1];
											args.CopyTo(nargs, 1);
											nargs[0] = this.wrapper;
										}
										MethodInfo[] nativeCodeTypeMethods = nativeCodeType.GetMethods(BindingFlags.Static | BindingFlags.Public);
										foreach(MethodInfo method in nativeCodeTypeMethods)
										{
											ParameterInfo[] param = method.GetParameters();
											TypeWrapper[] match = new TypeWrapper[param.Length];
											for(int j = 0; j < param.Length; j++)
											{
												match[j] = ClassLoaderWrapper.GetWrapperFromType(param[j].ParameterType);
											}
											if(m.Name == method.Name && IsCompatibleArgList(nargs, match))
											{
												// TODO instead of taking the first matching method, we should find the best one
												nativeMethod = method;
												break;
											}
										}
									}
									if(nativeMethod != null)
									{
										int add = 0;
										if(!m.IsStatic)
										{
											ilGenerator.Emit(OpCodes.Ldarg_0);
											add = 1;
										}
										for(int j = 0; j < args.Length; j++)
										{
											ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
										}
										ilGenerator.Emit(OpCodes.Call, nativeMethod);
										TypeWrapper retTypeWrapper = methods[i].ReturnType;
										if(!retTypeWrapper.TypeAsTBD.Equals(nativeMethod.ReturnType) && !retTypeWrapper.IsGhost)
										{
											ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
										}
										ilGenerator.Emit(OpCodes.Ret);
									}
									else
									{
										if(wrapper.classLoader.NoJNI)
										{
											// since NoJniStubs can only be set when we're statically compiling, it is safe to use the "compiler" trace switch
											Tracer.Warning(Tracer.Compiler, "Native method not implemented: {0}.{1}.{2}", classFile.Name, m.Name, m.Signature);
											EmitHelper.Throw(ilGenerator, "java.lang.UnsatisfiedLinkError", "Native method not implemented (compiled with -nojni): " + classFile.Name + "." + m.Name + m.Signature);
										}
										else
										{
											if(JVM.IsSaveDebugImage)
											{
#if !STATIC_COMPILER
												JniProxyBuilder.Generate(ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args);
#endif // !STATIC_COMPILER
											}
											else
											{
												JniBuilder.Generate(ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args, false);
											}
										}
									}
								}
								finally
								{
									Profiler.Leave("JavaTypeImpl.Finish.Native");
								}
							}
							else
							{
								MethodBuilder mbld = (MethodBuilder)mb;
								ILGenerator ilGenerator = mbld.GetILGenerator();
								TraceHelper.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if STATIC_COMPILER
								if(wrapper.EmitMapXmlMethodBody(ilGenerator, classFile, m))
								{
									continue;
								}
#endif // STATIC_COMPILER
								LineNumberTableAttribute.LineNumberWriter lineNumberTable = null;
								bool nonleaf = false;
								Compiler.Compile(wrapper, methods[i], classFile, m, ilGenerator, ref nonleaf, invokespecialstubcache, ref lineNumberTable);
								if(nonleaf)
								{
									mbld.SetImplementationFlags(mbld.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
								}
								if(lineNumberTable != null)
								{
#if STATIC_COMPILER
									AttributeHelper.SetLineNumberTable(methods[i].GetMethod(), lineNumberTable);
#else // STATIC_COMPILER
									if(wrapper.lineNumberTables == null)
									{
										wrapper.lineNumberTables = new byte[methods.Length][];
									}
									wrapper.lineNumberTables[i] = lineNumberTable.ToArray();
#endif // STATIC_COMPILER
								}
							}
						}
					}

					// add all interfaces that we implement (including the magic ones) and handle ghost conversions
					ImplementInterfaces(wrapper.Interfaces, new ArrayList());

					// NOTE non-final fields aren't allowed in interfaces so we don't have to initialize constant fields
					if(!classFile.IsInterface)
					{
						// if we don't have a <clinit> we may need to inject one
						if(!hasclinit)
						{
							bool hasconstantfields = false;
							if(!basehasclinit)
							{
								foreach(ClassFile.Field f in classFile.Fields)
								{
									if(f.IsStatic && !f.IsFinal && f.ConstantValue != null)
									{
										hasconstantfields = true;
										break;
									}
								}
							}
							if(basehasclinit || hasconstantfields)
							{
								ConstructorBuilder cb = DefineClassInitializer();
								AttributeHelper.HideFromJava(cb);
								ILGenerator ilGenerator = cb.GetILGenerator();
								EmitConstantValueInitialization(ilGenerator);
								if(basehasclinit)
								{
									wrapper.BaseTypeWrapper.EmitRunClassConstructor(ilGenerator);
								}
								ilGenerator.Emit(OpCodes.Ret);
							}
						}
						// if a class has no constructor, we generate one otherwise Ref.Emit will create a default ctor
						// and that has several problems:
						// - base type may not have an accessible default constructor
						// - Ref.Emit uses BaseType.GetConstructors() which may trigger a TypeResolve event
						// - we don't want the synthesized constructor to show up in Java
						if(!hasConstructor)
						{
							ConstructorBuilder cb = typeBuilder.DefineConstructor(MethodAttributes.PrivateScope, CallingConventions.Standard, Type.EmptyTypes);
							ILGenerator ilgen = cb.GetILGenerator();
							ilgen.Emit(OpCodes.Ldnull);
							ilgen.Emit(OpCodes.Throw);
						}

						// here we loop thru all the interfaces to explicitly implement any methods that we inherit from
						// base types that may have a different name from the name in the interface
						// (e.g. interface that has an equals() method that should override System.Object.Equals())
						// also deals with interface methods that aren't implemented (generate a stub that throws AbstractMethodError)
						// and with methods that aren't public (generate a stub that throws IllegalAccessError)
						Hashtable doneSet = new Hashtable();
						TypeWrapper[] interfaces = wrapper.Interfaces;
						for(int i = 0; i < interfaces.Length; i++)
						{
							interfaces[i].ImplementInterfaceMethodStubs(typeBuilder, wrapper, doneSet);
						}
						// if any of our base classes has an incomplete interface implementation we need to look through all
						// the base class interfaces to see if we've got an implementation now
						TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
						while(baseTypeWrapper.HasIncompleteInterfaceImplementation)
						{
							for(int i = 0; i < baseTypeWrapper.Interfaces.Length; i++)
							{
								baseTypeWrapper.Interfaces[i].ImplementInterfaceMethodStubs(typeBuilder, wrapper, doneSet);
							}
							baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
						}
						if(!wrapper.IsAbstract && wrapper.HasUnsupportedAbstractMethods)
						{
							AddUnsupportedAbstractMethods();
						}
						foreach(MethodWrapper mw in methods)
						{
							if(mw.Name != "<init>" && !mw.IsStatic && mw.IsPublic)
							{
								if(wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasIncompleteInterfaceImplementation)
								{
									Hashtable hashtable = null;
									TypeWrapper tw = wrapper.BaseTypeWrapper;
									while(tw.HasIncompleteInterfaceImplementation)
									{
										foreach(TypeWrapper iface in tw.Interfaces)
										{
											AddMethodOverride(mw, (MethodBuilder)mw.GetMethod(), iface, mw.Name, mw.Signature, ref hashtable, false);
										}
										tw = tw.BaseTypeWrapper;
									}
								}
								if(true)
								{
									Hashtable hashtable = null;
									foreach(TypeWrapper iface in wrapper.Interfaces)
									{
										AddMethodOverride(mw, (MethodBuilder)mw.GetMethod(), iface, mw.Name, mw.Signature, ref hashtable, true);
									}
								}
							}
						}
					}

#if STATIC_COMPILER
					// If we're an interface that has public/protected fields, we create an inner class
					// to expose these fields to C# (which stubbornly refuses to see fields in interfaces).
					TypeBuilder tbFields = null;
					if(classFile.IsInterface && classFile.IsPublic && !wrapper.IsGhost && classFile.Fields.Length > 0)
					{
						// TODO handle name clash
						tbFields = typeBuilder.DefineNestedType("__Fields", TypeAttributes.Class | TypeAttributes.NestedPublic | TypeAttributes.Sealed);
						tbFields.DefineDefaultConstructor(MethodAttributes.Private);
						AttributeHelper.HideFromJava(tbFields);
						ILGenerator ilgenClinit = null;
						foreach(ClassFile.Field f in classFile.Fields)
						{
							TypeWrapper typeWrapper = ClassFile.FieldTypeWrapperFromSig(wrapper.GetClassLoader(), classCache, f.Signature);
							if(f.ConstantValue != null)
							{
								FieldAttributes attribs = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;
								FieldBuilder fb = tbFields.DefineField(f.Name, typeWrapper.TypeAsSignatureType, attribs);
								fb.SetConstant(f.ConstantValue);
							}
							else
							{
								FieldAttributes attribs = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.InitOnly;
								FieldBuilder fb = tbFields.DefineField(f.Name, typeWrapper.TypeAsSignatureType, attribs);
								if(ilgenClinit == null)
								{
									ilgenClinit = tbFields.DefineTypeInitializer().GetILGenerator();
								}
								wrapper.GetFieldWrapper(f.Name, f.Signature).EmitGet(ilgenClinit);
								ilgenClinit.Emit(OpCodes.Stsfld, fb);
							}
						}
						if(ilgenClinit != null)
						{
							ilgenClinit.Emit(OpCodes.Ret);
						}
					}

					// See if there is any additional metadata
					wrapper.EmitMapXmlMetadata(typeBuilder, classFile, fields, methods);
#endif // STATIC_COMPILER

					for(int i = 0; i < classFile.Methods.Length; i++)
					{
						ClassFile.Method m = classFile.Methods[i];
						MethodBase mb = methods[i].GetMethod();
						ParameterBuilder returnParameter = null;
						ParameterBuilder[] parameterBuilders = null;
						if(wrapper.GetClassLoader().EmitDebugInfo
#if STATIC_COMPILER
							|| (classFile.IsPublic && (m.IsPublic || m.IsProtected))
#endif
							)
						{
							string[] parameterNames = new string[methods[i].GetParameters().Length];
							GetParameterNamesFromLVT(m, parameterNames);
							GetParameterNamesFromSig(m.Signature, parameterNames);
#if STATIC_COMPILER
							((AotTypeWrapper)wrapper).GetParameterNamesFromXml(m.Name, m.Signature, parameterNames);
#endif
							parameterBuilders = GetParameterBuilders(mb, parameterNames.Length, parameterNames);
						}
#if STATIC_COMPILER
						if((m.Modifiers & Modifiers.VarArgs) != 0)
						{
							if(parameterBuilders == null)
							{
								parameterBuilders = GetParameterBuilders(mb, methods[i].GetParameters().Length, null);
							}
							if(parameterBuilders.Length > 0)
							{
								AttributeHelper.SetParamArrayAttribute(parameterBuilders[parameterBuilders.Length - 1]);
							}
						}
						((AotTypeWrapper)wrapper).AddXmlMapParameterAttributes(mb, classFile.Name, m.Name, m.Signature, ref parameterBuilders);
#endif
						ConstructorBuilder cb = mb as ConstructorBuilder;
						MethodBuilder mBuilder = mb as MethodBuilder;
						if(m.Annotations != null)
						{
							foreach(object[] def in m.Annotations)
							{
								Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
								if(annotation != null)
								{
									if(cb != null)
									{
										annotation.Apply(wrapper.GetClassLoader(), cb, def);
									}
									if(mBuilder != null)
									{
										annotation.Apply(wrapper.GetClassLoader(), mBuilder, def);
										annotation.ApplyReturnValue(wrapper.GetClassLoader(), mBuilder, ref returnParameter, def);
									}
								}
							}
						}
						if(m.ParameterAnnotations != null)
						{
							if(parameterBuilders == null)
							{
								parameterBuilders = GetParameterBuilders(mb, methods[i].GetParameters().Length, null);
							}
							object[][] defs = m.ParameterAnnotations;
							for(int j = 0; j < defs.Length; j++)
							{
								foreach(object[] def in defs[j])
								{
									Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
									if(annotation != null)
									{
										annotation.Apply(wrapper.GetClassLoader(), parameterBuilders[j], def);
									}
								}
							}
						}
					}

					for(int i = 0; i < classFile.Fields.Length; i++)
					{
						if(classFile.Fields[i].Annotations != null)
						{
							foreach(object[] def in classFile.Fields[i].Annotations)
							{
								Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
								if(annotation != null)
								{
									GetterFieldWrapper getter = fields[i] as GetterFieldWrapper;
									if(getter != null)
									{
										annotation.Apply(wrapper.GetClassLoader(), (MethodBuilder)getter.GetGetter(), def);
									}
									else
									{
										annotation.Apply(wrapper.GetClassLoader(), (FieldBuilder)fields[i].GetField(), def);
									}
								}
							}
						}
					}

					if(classFile.Annotations != null)
					{
						foreach(object[] def in classFile.Annotations)
						{
							Annotation annotation = Annotation.Load(wrapper.GetClassLoader(), def);
							if(annotation != null)
							{
								annotation.Apply(wrapper.GetClassLoader(), typeBuilder, def);
							}
						}
					}

					Type type;
					Profiler.Enter("TypeBuilder.CreateType");
					try
					{
						type = typeBuilder.CreateType();
#if STATIC_COMPILER
						if(tbFields != null)
						{
							tbFields.CreateType();
						}
						if(enumBuilder != null)
						{
							enumBuilder.CreateType();
						}
						if(annotationBuilder != null)
						{
							annotationBuilder.Finish(this);
						}
						if(classFile.IsInterface && !classFile.IsPublic)
						{
							((DynamicClassLoader)wrapper.classLoader.GetTypeWrapperFactory()).DefineProxyHelper(type);
						}
#endif
					}
					finally
					{
						Profiler.Leave("TypeBuilder.CreateType");
					}
					ClassLoaderWrapper.SetWrapperForType(type, wrapper);
#if STATIC_COMPILER
					wrapper.FinishGhostStep2();
#endif
					finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, this.ReflectiveModifiers, Metadata.Create(classFile)
#if STATIC_COMPILER
						, annotationBuilder, enumBuilder
#endif
						);
					return finishedType;
				}
				catch(Exception x)
				{
					JVM.CriticalFailure("Exception during finishing of: " + wrapper.Name, x);
					return null;
				}
				finally
				{
					Profiler.Leave("JavaTypeImpl.Finish.Core");
				}
			}

			private void ImplementInterfaces(TypeWrapper[] interfaces, ArrayList interfaceList)
			{
				foreach (TypeWrapper iface in interfaces)
				{
					if (!interfaceList.Contains(iface))
					{
						interfaceList.Add(iface);
						// skip interfaces that don't really exist
						// (e.g. delegate "Method" and attribute "Annotation" inner interfaces)
						if (!iface.IsDynamicOnly)
						{
							// NOTE we're using TypeAsBaseType for the interfaces!
							Type ifaceType = iface.TypeAsBaseType;
							if(!iface.IsPublic && !ifaceType.Assembly.Equals(typeBuilder.Assembly))
							{
								ifaceType = ifaceType.Assembly.GetType(DynamicClassLoader.GetProxyHelperName(ifaceType));
							}
							typeBuilder.AddInterfaceImplementation(ifaceType);
						}
#if STATIC_COMPILER
						if (!wrapper.IsInterface)
						{
							// look for "magic" interfaces that imply a .NET interface
							if (iface.GetClassLoader() == CoreClasses.java.lang.Object.Wrapper.GetClassLoader())
							{
								if (iface.Name == "java.lang.Iterable"
									&& !wrapper.ImplementsInterface(ClassLoaderWrapper.GetWrapperFromType(typeof(IEnumerable))))
								{
									TypeWrapper enumeratorType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("ikvm.lang.IterableEnumerator");
									if (enumeratorType != null)
									{
										typeBuilder.AddInterfaceImplementation(typeof(IEnumerable));
										MethodBuilder mb = typeBuilder.DefineMethod("__<>GetEnumerator", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName, typeof(IEnumerator), Type.EmptyTypes);
										typeBuilder.DefineMethodOverride(mb, typeof(IEnumerable).GetMethod("GetEnumerator"));
										ILGenerator ilgen = mb.GetILGenerator();
										ilgen.Emit(OpCodes.Ldarg_0);
										MethodWrapper mw = enumeratorType.GetMethodWrapper("<init>", "(Ljava.lang.Iterable;)V", false);
										mw.Link();
										mw.EmitNewobj(ilgen);
										ilgen.Emit(OpCodes.Ret);
									}
								}
								else if (iface.Name == "java.io.Closeable"
									&& !wrapper.ImplementsInterface(ClassLoaderWrapper.GetWrapperFromType(typeof(IDisposable))))
								{
									typeBuilder.AddInterfaceImplementation(typeof(IDisposable));
									MethodBuilder mb = typeBuilder.DefineMethod("__<>Dispose", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName, typeof(void), Type.EmptyTypes);
									typeBuilder.DefineMethodOverride(mb, typeof(IDisposable).GetMethod("Dispose"));
									ILGenerator ilgen = mb.GetILGenerator();
									ilgen.Emit(OpCodes.Ldarg_0);
									MethodWrapper mw = iface.GetMethodWrapper("close", "()V", false);
									mw.Link();
									mw.EmitCallvirt(ilgen);
									ilgen.Emit(OpCodes.Ret);
								}
							}
							// if we implement a ghost interface, add an implicit conversion to the ghost reference value type
							if(iface.IsGhost && wrapper.IsPublic)
							{
								MethodBuilder mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, iface.TypeAsSignatureType, new Type[] { wrapper.TypeAsSignatureType });
								ILGenerator ilgen = mb.GetILGenerator();
								LocalBuilder local = ilgen.DeclareLocal(iface.TypeAsSignatureType);
								ilgen.Emit(OpCodes.Ldloca, local);
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Stfld, iface.GhostRefField);
								ilgen.Emit(OpCodes.Ldloca, local);
								ilgen.Emit(OpCodes.Ldobj, iface.TypeAsSignatureType);
								ilgen.Emit(OpCodes.Ret);
							}
						}
#endif // STATIC_COMPILER
						// NOTE we're recursively "implementing" all interfaces that we inherit from the interfaces we implement.
						// The C# compiler also does this and the Compact Framework requires it.
						ImplementInterfaces(iface.Interfaces, interfaceList);
					}
				}
			}

			private void AddUnsupportedAbstractMethods()
			{
				foreach(MethodBase mb in wrapper.BaseTypeWrapper.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
				{
					if(DotNetTypeWrapper.IsUnsupportedAbstractMethod(mb))
					{
						GenerateUnsupportedAbstractMethodStub(mb);
					}
				}
				Hashtable h = new Hashtable();
				TypeWrapper tw = wrapper;
				while(tw != null)
				{
					foreach(TypeWrapper iface in tw.Interfaces)
					{
						foreach(MethodBase mb in iface.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
						{
							if(!h.ContainsKey(mb))
							{
								h.Add(mb, mb);
								if(DotNetTypeWrapper.IsUnsupportedAbstractMethod(mb))
								{
									GenerateUnsupportedAbstractMethodStub(mb);
								}
							}
						}
					}
					tw = tw.BaseTypeWrapper;
				}
			}

			private void GenerateUnsupportedAbstractMethodStub(MethodBase mb)
			{
				ParameterInfo[] parameters = mb.GetParameters();
				Type[] parameterTypes = new Type[parameters.Length];
				for(int i = 0; i < parameters.Length; i++)
				{
					parameterTypes[i] = parameters[i].ParameterType;
				}
				MethodAttributes attr = MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Private;
				MethodBuilder m = typeBuilder.DefineMethod("__<unsupported>" + mb.DeclaringType.FullName + "/" + mb.Name, attr, ((MethodInfo)mb).ReturnType, parameterTypes);
				EmitHelper.Throw(m.GetILGenerator(), "java.lang.AbstractMethodError", "Method " + mb.DeclaringType.FullName + "." + mb.Name + " is unsupported by IKVM.");
				typeBuilder.DefineMethodOverride(m, (MethodInfo)mb);
			}

			class TraceHelper
			{
				private readonly static MethodInfo methodIsTracedMethod = typeof(Tracer).GetMethod("IsTracedMethod");
				private readonly static MethodInfo methodMethodInfo = typeof(Tracer).GetMethod("MethodInfo");

				internal static void EmitMethodTrace(ILGenerator ilgen, string tracemessage)
				{
					if(Tracer.IsTracedMethod(tracemessage))
					{
						Label label = ilgen.DefineLabel();
#if STATIC_COMPILER
						// TODO this should be a boolean field test instead of a call to Tracer.IsTracedMessage
						ilgen.Emit(OpCodes.Ldstr, tracemessage);
						ilgen.Emit(OpCodes.Call, methodIsTracedMethod);
						ilgen.Emit(OpCodes.Brfalse_S, label);
#endif
						ilgen.Emit(OpCodes.Ldstr, tracemessage);
						ilgen.Emit(OpCodes.Call, methodMethodInfo);
						ilgen.MarkLabel(label);
					}
				}
			}

			private bool IsValidAnnotationElementType(string type)
			{
				if(type[0] == '[')
				{
					type = type.Substring(1);
				}
				switch(type)
				{
					case "Z":
					case "B":
					case "S":
					case "C":
					case "I":
					case "J":
					case "F":
					case "D":
					case "Ljava.lang.String;":
					case "Ljava.lang.Class;":
						return true;
				}
				if(type.StartsWith("L") && type.EndsWith(";"))
				{
					try
					{
						TypeWrapper tw = wrapper.GetClassLoader().LoadClassByDottedNameFast(type.Substring(1, type.Length - 2));
						if(tw != null)
						{
							if((tw.Modifiers & Modifiers.Annotation) != 0)
							{
								return true;
							}
							if((tw.Modifiers & Modifiers.Enum) != 0)
							{
								TypeWrapper enumType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("java.lang.Enum");
								if(enumType != null && tw.IsSubTypeOf(enumType))
								{
									return true;
								}
							}
						}
					}
					catch
					{
					}
				}
				return false;
			}

#if STATIC_COMPILER
			sealed class AnnotationBuilder : Annotation
			{
				private TypeBuilder annotationTypeBuilder;
				private TypeBuilder attributeTypeBuilder;
				private ConstructorBuilder defineConstructor;

				internal AnnotationBuilder(JavaTypeImpl o)
				{
					// Make sure the annotation type only has valid methods
					for(int i = 0; i < o.methods.Length; i++)
					{
						if(!o.methods[i].IsStatic)
						{
							if(!o.methods[i].Signature.StartsWith("()"))
							{
								return;
							}
							if(!o.IsValidAnnotationElementType(o.methods[i].Signature.Substring(2)))
							{
								return;
							}
						}
					}

					// we only set annotationTypeBuilder if we're valid
					annotationTypeBuilder = o.typeBuilder;

					TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");

					// TODO attribute should be .NET serializable
					TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Sealed;
					if(o.outerClassWrapper != null)
					{
						if(o.wrapper.IsPublic)
						{
							typeAttributes |= TypeAttributes.NestedPublic;
						}
						else
						{
							typeAttributes |= TypeAttributes.NestedAssembly;
						}
						attributeTypeBuilder = o.outerClassWrapper.TypeAsBuilder.DefineNestedType(GetInnerClassName(o.outerClassWrapper.Name, o.classFile.Name) + "Attribute", typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
					}
					else
					{
						if(o.wrapper.IsPublic)
						{
							typeAttributes |= TypeAttributes.Public;
						}
						else
						{
							typeAttributes |= TypeAttributes.NotPublic;
						}
						attributeTypeBuilder = o.wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(o.classFile.Name + "Attribute", typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
					}
					if(o.wrapper.IsPublic)
					{
						// In the Java world, the class appears as a non-public proxy class
						AttributeHelper.SetModifiers(attributeTypeBuilder, Modifiers.Final, false);
					}
					// NOTE we "abuse" the InnerClassAttribute to add a custom attribute to name the class "$Proxy[Annotation]" in the Java world
					int dotindex = o.classFile.Name.LastIndexOf('.') + 1;
					AttributeHelper.SetInnerClass(attributeTypeBuilder, o.classFile.Name.Substring(0, dotindex) + "$Proxy" + o.classFile.Name.Substring(dotindex), Modifiers.Final);
					attributeTypeBuilder.AddInterfaceImplementation(o.typeBuilder);
					AttributeHelper.SetImplementsAttribute(attributeTypeBuilder, new TypeWrapper[] { o.wrapper });

					if(o.classFile.Annotations != null)
					{
						foreach(object[] def in o.classFile.Annotations)
						{
							if(def[1].Equals("Ljava/lang/annotation/Target;"))
							{
								for(int i = 2; i < def.Length; i += 2)
								{
									if(def[i].Equals("value"))
									{
										object[] val = def[i + 1] as object[];
										if(val != null
											&& val.Length > 0
											&& val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
										{
											AttributeTargets targets = 0;
											for(int j = 1; j < val.Length; j++)
											{
												object[] eval = val[j] as object[];
												if(eval != null
													&& eval.Length == 3
													&& eval[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
													&& eval[1].Equals("Ljava/lang/annotation/ElementType;"))
												{
													switch((string)eval[2])
													{
														case "ANNOTATION_TYPE":
															targets |= AttributeTargets.Interface;
															break;
														case "CONSTRUCTOR":
															targets |= AttributeTargets.Constructor;
															break;
														case "FIELD":
															targets |= AttributeTargets.Field;
															break;
														case "LOCAL_VARIABLE":
															break;
														case "METHOD":
															targets |= AttributeTargets.Method;
															break;
														case "PACKAGE":
															targets |= AttributeTargets.Interface;
															break;
														case "PARAMETER":
															targets |= AttributeTargets.Parameter;
															break;
														case "TYPE":
															targets |= AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Delegate | AttributeTargets.Enum;
															break;
													}
												}
											}
											CustomAttributeBuilder cab2 = new CustomAttributeBuilder(typeof(AttributeUsageAttribute).GetConstructor(new Type[] { typeof(AttributeTargets) }), new object[] { targets });
											attributeTypeBuilder.SetCustomAttribute(cab2);
										}
									}
								}
							}
						}
					}

					defineConstructor = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(object[]) });
					AttributeHelper.SetEditorBrowsableNever(defineConstructor);
				}

				private static Type TypeWrapperToAnnotationParameterType(TypeWrapper tw)
				{
					bool isArray = false;
					if(tw.IsArray)
					{
						isArray = true;
						tw = tw.ElementTypeWrapper;
					}
					if(tw.Annotation != null)
					{
						// we don't support Annotation args
						return null;
					}
					else
					{
						Type argType;
						if(tw == CoreClasses.java.lang.Class.Wrapper)
						{
							argType = typeof(Type);
						}
						else if(tw.EnumType != null)
						{
							argType = tw.EnumType;
						}
						else
						{
							argType = tw.TypeAsSignatureType;
						}
						if(isArray)
						{
							argType = ArrayTypeWrapper.MakeArrayType(argType, 1);
						}
						return argType;
					}
				}

				internal string AttributeTypeName
				{
					get
					{
						if(attributeTypeBuilder != null)
						{
							return attributeTypeBuilder.FullName;
						}
						return null;
					}
				}

				private static void EmitSetValueCall(TypeWrapper annotationAttributeBaseType, ILGenerator ilgen, string name, TypeWrapper tw, int argIndex)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldstr, name);
					ilgen.Emit(OpCodes.Ldarg_S, (byte)argIndex);
					if(tw.TypeAsSignatureType.IsValueType)
					{
						ilgen.Emit(OpCodes.Box, tw.TypeAsSignatureType);
					}
					else if(tw.EnumType != null)
					{
						ilgen.Emit(OpCodes.Box, tw.EnumType);
					}
					MethodWrapper setValueMethod = annotationAttributeBaseType.GetMethodWrapper("setValue", "(Ljava.lang.String;Ljava.lang.Object;)V", false);
					setValueMethod.Link();
					setValueMethod.EmitCall(ilgen);
				}

				internal void Finish(JavaTypeImpl o)
				{
					if(annotationTypeBuilder == null)
					{
						// not a valid annotation type
						return;
					}
					TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");
					annotationAttributeBaseType.Finish();

					int requiredArgCount = 0;
					int valueArg = -1;
					bool unsupported = false;
					for(int i = 0; i < o.methods.Length; i++)
					{
						if(!o.methods[i].IsStatic)
						{
							if(valueArg == -1 && o.methods[i].Name == "value")
							{
								valueArg = i;
							}
							if(o.classFile.Methods[i].AnnotationDefault == null)
							{
								if(TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType) == null)
								{
									unsupported = true;
									break;
								}
								requiredArgCount++;
							}
						}
					}

					ConstructorBuilder defaultConstructor = attributeTypeBuilder.DefineConstructor(unsupported || requiredArgCount > 0 ? MethodAttributes.Private : MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
					ILGenerator ilgen;

					if(!unsupported)
					{
						if(requiredArgCount > 0)
						{
							Type[] args = new Type[requiredArgCount];
							for(int i = 0, j = 0; i < o.methods.Length; i++)
							{
								if(!o.methods[i].IsStatic)
								{
									if(o.classFile.Methods[i].AnnotationDefault == null)
									{
										args[j++] = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
									}
								}
							}
							ConstructorBuilder reqArgConstructor = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, args);
							AttributeHelper.HideFromJava(reqArgConstructor);
							ilgen = reqArgConstructor.GetILGenerator();
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Call, defaultConstructor);
							for(int i = 0, j = 0; i < o.methods.Length; i++)
							{
								if(!o.methods[i].IsStatic)
								{
									if(o.classFile.Methods[i].AnnotationDefault == null)
									{
										reqArgConstructor.DefineParameter(++j, ParameterAttributes.None, o.methods[i].Name);
										EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, j);
									}
								}
							}
							ilgen.Emit(OpCodes.Ret);
						}
						else if(valueArg != -1)
						{
							// We don't have any required parameters, but we do have an optional "value" parameter,
							// so we create an additional constructor (the default constructor will be public in this case)
							// that accepts the value parameter.
							Type argType = TypeWrapperToAnnotationParameterType(o.methods[valueArg].ReturnType);
							if(argType != null)
							{
								ConstructorBuilder cb = attributeTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { argType });
								AttributeHelper.HideFromJava(cb);
								cb.DefineParameter(1, ParameterAttributes.None, "value");
								ilgen = cb.GetILGenerator();
								ilgen.Emit(OpCodes.Ldarg_0);
								ilgen.Emit(OpCodes.Call, defaultConstructor);
								EmitSetValueCall(annotationAttributeBaseType, ilgen, "value", o.methods[valueArg].ReturnType, 1);
								ilgen.Emit(OpCodes.Ret);
							}
						}
					}

					ilgen = defaultConstructor.GetILGenerator();
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldtoken, annotationTypeBuilder);
					ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.GetClassFromTypeHandle);
					CoreClasses.java.lang.Class.Wrapper.EmitCheckcast(null, ilgen);
					annotationAttributeBaseType.GetMethodWrapper("<init>", "(Ljava.lang.Class;)V", false).EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ret);

					ilgen = defineConstructor.GetILGenerator();
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Call, defaultConstructor);
					ilgen.Emit(OpCodes.Ldarg_0);
					ilgen.Emit(OpCodes.Ldarg_1);
					annotationAttributeBaseType.GetMethodWrapper("setDefinition", "([Ljava.lang.Object;)V", false).EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ret);

					MethodWrapper getValueMethod = annotationAttributeBaseType.GetMethodWrapper("getValue", "(Ljava.lang.String;)Ljava.lang.Object;", false);
					MethodWrapper getByteValueMethod = annotationAttributeBaseType.GetMethodWrapper("getByteValue", "(Ljava.lang.String;)B", false);
					MethodWrapper getBooleanValueMethod = annotationAttributeBaseType.GetMethodWrapper("getBooleanValue", "(Ljava.lang.String;)Z", false);
					MethodWrapper getCharValueMethod = annotationAttributeBaseType.GetMethodWrapper("getCharValue", "(Ljava.lang.String;)C", false);
					MethodWrapper getShortValueMethod = annotationAttributeBaseType.GetMethodWrapper("getShortValue", "(Ljava.lang.String;)S", false);
					MethodWrapper getIntValueMethod = annotationAttributeBaseType.GetMethodWrapper("getIntValue", "(Ljava.lang.String;)I", false);
					MethodWrapper getFloatValueMethod = annotationAttributeBaseType.GetMethodWrapper("getFloatValue", "(Ljava.lang.String;)F", false);
					MethodWrapper getLongValueMethod = annotationAttributeBaseType.GetMethodWrapper("getLongValue", "(Ljava.lang.String;)J", false);
					MethodWrapper getDoubleValueMethod = annotationAttributeBaseType.GetMethodWrapper("getDoubleValue", "(Ljava.lang.String;)D", false);
					for(int i = 0; i < o.methods.Length; i++)
					{
						// skip <clinit>
						if(!o.methods[i].IsStatic)
						{
							MethodBuilder mb = attributeTypeBuilder.DefineMethod(o.methods[i].Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot, o.methods[i].ReturnTypeForDefineMethod, o.methods[i].GetParametersForDefineMethod());
							attributeTypeBuilder.DefineMethodOverride(mb, (MethodInfo)o.methods[i].GetMethod());
							ilgen = mb.GetILGenerator();
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldstr, o.methods[i].Name);
							if(o.methods[i].ReturnType.IsPrimitive)
							{
								if(o.methods[i].ReturnType == PrimitiveTypeWrapper.BYTE)
								{
									getByteValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.BOOLEAN)
								{
									getBooleanValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.CHAR)
								{
									getCharValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.SHORT)
								{
									getShortValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.INT)
								{
									getIntValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.FLOAT)
								{
									getFloatValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.LONG)
								{
									getLongValueMethod.EmitCall(ilgen);
								}
								else if(o.methods[i].ReturnType == PrimitiveTypeWrapper.DOUBLE)
								{
									getDoubleValueMethod.EmitCall(ilgen);
								}
								else
								{
									throw new InvalidOperationException();
								}
							}
							else
							{
								getValueMethod.EmitCall(ilgen);
								o.methods[i].ReturnType.EmitCheckcast(null, ilgen);
							}
							ilgen.Emit(OpCodes.Ret);

							if(o.classFile.Methods[i].AnnotationDefault != null
								&& !(o.methods[i].Name == "value" && requiredArgCount == 0))
							{
								// now add a .NET property for this annotation optional parameter
								Type argType = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
								if(argType != null)
								{
									PropertyBuilder pb = attributeTypeBuilder.DefineProperty(o.methods[i].Name, PropertyAttributes.None, argType, Type.EmptyTypes);
									AttributeHelper.HideFromJava(pb);
									MethodBuilder setter = attributeTypeBuilder.DefineMethod("set_" + o.methods[i].Name, MethodAttributes.Public, typeof(void), new Type[] { argType });
									AttributeHelper.HideFromJava(setter);
									pb.SetSetMethod(setter);
									ilgen = setter.GetILGenerator();
									EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, 1);
									ilgen.Emit(OpCodes.Ret);
									MethodBuilder getter = attributeTypeBuilder.DefineMethod("get_" + o.methods[i].Name, MethodAttributes.Public, argType, Type.EmptyTypes);
									AttributeHelper.HideFromJava(getter);
									pb.SetGetMethod(getter);
									// TODO implement the getter method
									getter.GetILGenerator().ThrowException(typeof(NotImplementedException));
								}
							}
						}
					}
					attributeTypeBuilder.CreateType();
				}

				internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						tb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						mb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						cb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						fb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						pb.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}

				internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
				{
					if(annotationTypeBuilder != null)
					{
						ab.SetCustomAttribute(new CustomAttributeBuilder(defineConstructor, new object[] { annotation }));
					}
				}
			}
#endif // STATIC_COMPILER

#if !STATIC_COMPILER
			internal class JniProxyBuilder
			{
				private static ModuleBuilder mod;
				private static int count;

				static JniProxyBuilder()
				{
					AssemblyName name = new AssemblyName();
					name.Name = "jniproxy";
					AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(name, JVM.IsSaveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
					DynamicClassLoader.RegisterForSaveDebug(ab);
					mod = ab.DefineDynamicModule("jniproxy.dll", "jniproxy.dll");
					CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(JavaModuleAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
					mod.SetCustomAttribute(cab);
				}

				internal static void Generate(ILGenerator ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args)
				{
					TypeBuilder tb = mod.DefineType("__<jni>" + (count++), TypeAttributes.Public | TypeAttributes.Class);
					int instance = m.IsStatic ? 0 : 1;
					Type[] argTypes = new Type[args.Length + instance + 1];
					if(instance != 0)
					{
						argTypes[0] = typeof(object);
					}
					for(int i = 0; i < args.Length; i++)
					{
						// NOTE we take a shortcut here by assuming that all "special" types (i.e. ghost or value types)
						// are public and so we can get away with replacing all other types with object.
						argTypes[i + instance] = (args[i].IsPrimitive || args[i].IsGhost || args[i].IsNonPrimitiveValueType) ? args[i].TypeAsSignatureType : typeof(object);
					}
					argTypes[argTypes.Length - 1] = typeof(RuntimeMethodHandle);
					Type retType = (mw.ReturnType.IsPrimitive || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType) ? mw.ReturnType.TypeAsSignatureType : typeof(object);
					MethodBuilder mb = tb.DefineMethod("method", MethodAttributes.Public | MethodAttributes.Static, retType, argTypes);
					AttributeHelper.HideFromJava(mb);
					JniBuilder.Generate(mb.GetILGenerator(), wrapper, mw, tb, classFile, m, args, true);
					tb.CreateType();
					for(int i = 0; i < argTypes.Length - 1; i++)
					{
						ilGenerator.Emit(OpCodes.Ldarg, (short)i);
					}
					ilGenerator.Emit(OpCodes.Ldtoken, (MethodInfo)mw.GetMethod());
					ilGenerator.Emit(OpCodes.Call, mb);
					if(!mw.ReturnType.IsPrimitive && !mw.ReturnType.IsGhost && !mw.ReturnType.IsNonPrimitiveValueType)
					{
						ilGenerator.Emit(OpCodes.Castclass, mw.ReturnType.TypeAsSignatureType);
					}
					ilGenerator.Emit(OpCodes.Ret);
				}
			}
#endif // !STATIC_COMPILER

			private class JniBuilder
			{
#if STATIC_COMPILER
				private static readonly Type localRefStructType = StaticCompiler.GetType("IKVM.Runtime.JNI.Frame");
#else
				private static readonly Type localRefStructType = JVM.LoadType(typeof(IKVM.Runtime.JNI.Frame));
#endif
				private static readonly MethodInfo jniFuncPtrMethod = localRefStructType.GetMethod("GetFuncPtr");
				private static readonly MethodInfo enterLocalRefStruct = localRefStructType.GetMethod("Enter");
				private static readonly MethodInfo leaveLocalRefStruct = localRefStructType.GetMethod("Leave");
				private static readonly MethodInfo makeLocalRef = localRefStructType.GetMethod("MakeLocalRef");
				private static readonly MethodInfo unwrapLocalRef = localRefStructType.GetMethod("UnwrapLocalRef");
				private static readonly MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }, null);
				private static readonly MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter", new Type[] { typeof(object) });
				private static readonly MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit", new Type[] { typeof(object) });

				internal static void Generate(ILGenerator ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args, bool thruProxy)
				{
					LocalBuilder syncObject = null;
					FieldInfo classObjectField;
					if(thruProxy)
					{
						classObjectField = typeBuilder.DefineField("__<classObject>", typeof(object), FieldAttributes.Static | FieldAttributes.Private | FieldAttributes.SpecialName);
					}
					else
					{
						classObjectField = wrapper.ClassObjectField;
					}
					if(m.IsSynchronized && m.IsStatic)
					{
						ilGenerator.Emit(OpCodes.Ldsfld, classObjectField);
						Label label = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Brtrue_S, label);
						ilGenerator.Emit(OpCodes.Ldtoken, wrapper.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.GetClassFromTypeHandle);
						ilGenerator.Emit(OpCodes.Stsfld, classObjectField);
						ilGenerator.MarkLabel(label);
						ilGenerator.Emit(OpCodes.Ldsfld, classObjectField);
						ilGenerator.Emit(OpCodes.Dup);
						syncObject = ilGenerator.DeclareLocal(typeof(object));
						ilGenerator.Emit(OpCodes.Stloc, syncObject);
						ilGenerator.Emit(OpCodes.Call, monitorEnter);
						ilGenerator.BeginExceptionBlock();
					}
					string sig = m.Signature.Replace('.', '/');
					// TODO use/unify JNI.METHOD_PTR_FIELD_PREFIX
					FieldBuilder methodPtr = typeBuilder.DefineField("__<jniptr>" + m.Name + sig, typeof(IntPtr), FieldAttributes.Static | FieldAttributes.PrivateScope);
					LocalBuilder localRefStruct = ilGenerator.DeclareLocal(localRefStructType);
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					ilGenerator.Emit(OpCodes.Initobj, localRefStructType);
					ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
					Label oklabel = ilGenerator.DefineLabel();
					ilGenerator.Emit(OpCodes.Brtrue, oklabel);
					if(thruProxy)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(args.Length + (mw.IsStatic ? 0 : 1)));
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldtoken, (MethodInfo)mw.GetMethod());
					}
					ilGenerator.Emit(OpCodes.Ldstr, classFile.Name.Replace('.', '/'));
					ilGenerator.Emit(OpCodes.Ldstr, m.Name);
					ilGenerator.Emit(OpCodes.Ldstr, sig);
					ilGenerator.Emit(OpCodes.Call, jniFuncPtrMethod);
					ilGenerator.Emit(OpCodes.Stsfld, methodPtr);
					ilGenerator.MarkLabel(oklabel);
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					if(thruProxy)
					{
						ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(args.Length + (mw.IsStatic ? 0 : 1)));
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldtoken, (MethodInfo)mw.GetMethod());
					}
					ilGenerator.Emit(OpCodes.Call, enterLocalRefStruct);
					LocalBuilder jnienv = ilGenerator.DeclareLocal(typeof(IntPtr));
					ilGenerator.Emit(OpCodes.Stloc, jnienv);
					ilGenerator.BeginExceptionBlock();
					TypeWrapper retTypeWrapper = mw.ReturnType;
					if(!retTypeWrapper.IsUnloadable && !retTypeWrapper.IsPrimitive)
					{
						// this one is for use after we return from "calli"
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					}
					ilGenerator.Emit(OpCodes.Ldloc, jnienv);
					Type[] modargs = new Type[args.Length + 2];
					modargs[0] = typeof(IntPtr);
					modargs[1] = typeof(IntPtr);
					for(int i = 0; i < args.Length; i++)
					{
						modargs[i + 2] = args[i].TypeAsSignatureType;
					}
					int add = 0;
					if(!m.IsStatic)
					{
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
						ilGenerator.Emit(OpCodes.Ldarg_0);
						ilGenerator.Emit(OpCodes.Call, makeLocalRef);
						add = 1;
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);

						ilGenerator.Emit(OpCodes.Ldsfld, classObjectField);
						Label label = ilGenerator.DefineLabel();
						ilGenerator.Emit(OpCodes.Brtrue_S, label);
						ilGenerator.Emit(OpCodes.Ldtoken, wrapper.TypeAsTBD);
						ilGenerator.Emit(OpCodes.Call, ByteCodeHelperMethods.GetClassFromTypeHandle);
						ilGenerator.Emit(OpCodes.Stsfld, classObjectField);
						ilGenerator.MarkLabel(label);
						ilGenerator.Emit(OpCodes.Ldsfld, classObjectField);

						ilGenerator.Emit(OpCodes.Call, makeLocalRef);
					}
					for(int j = 0; j < args.Length; j++)
					{
						if(args[j].IsUnloadable || !args[j].IsPrimitive)
						{
							ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							if(!args[j].IsUnloadable && args[j].IsNonPrimitiveValueType)
							{
								ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
								args[j].EmitBox(ilGenerator);
							}
							else if(!args[j].IsUnloadable && args[j].IsGhost)
							{
								ilGenerator.Emit(OpCodes.Ldarga_S, (byte)(j + add));
								ilGenerator.Emit(OpCodes.Ldfld, args[j].GhostRefField);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
							}
							ilGenerator.Emit(OpCodes.Call, makeLocalRef);
							modargs[j + 2] = typeof(IntPtr);
						}
						else
						{
							ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(j + add));
						}
					}
					ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
					Type realRetType;
					if(retTypeWrapper == PrimitiveTypeWrapper.BOOLEAN)
					{
						realRetType = typeof(byte);
					}
					else if(retTypeWrapper.IsPrimitive)
					{
						realRetType = retTypeWrapper.TypeAsSignatureType;
					}
					else
					{
						realRetType = typeof(IntPtr);
					}
					ilGenerator.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, realRetType, modargs);
					LocalBuilder retValue = null;
					if(retTypeWrapper != PrimitiveTypeWrapper.VOID)
					{
						if(!retTypeWrapper.IsUnloadable && !retTypeWrapper.IsPrimitive)
						{
							ilGenerator.Emit(OpCodes.Call, unwrapLocalRef);
							if(retTypeWrapper.IsNonPrimitiveValueType)
							{
								retTypeWrapper.EmitUnbox(ilGenerator);
							}
							else if(retTypeWrapper.IsGhost)
							{
								LocalBuilder ghost = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
								LocalBuilder obj = ilGenerator.DeclareLocal(typeof(object));
								ilGenerator.Emit(OpCodes.Stloc, obj);
								ilGenerator.Emit(OpCodes.Ldloca, ghost);
								ilGenerator.Emit(OpCodes.Ldloc, obj);
								ilGenerator.Emit(OpCodes.Stfld, retTypeWrapper.GhostRefField);
								ilGenerator.Emit(OpCodes.Ldloc, ghost);
							}
							else
							{
								ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
							}
						}
						retValue = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
						ilGenerator.Emit(OpCodes.Stloc, retValue);
					}
					ilGenerator.BeginCatchBlock(typeof(object));
					ilGenerator.EmitWriteLine("*** exception in native code ***");
					ilGenerator.Emit(OpCodes.Call, writeLine);
					ilGenerator.Emit(OpCodes.Rethrow);
					ilGenerator.BeginFinallyBlock();
					ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
					ilGenerator.Emit(OpCodes.Call, leaveLocalRefStruct);
					ilGenerator.EndExceptionBlock();
					if(m.IsSynchronized && m.IsStatic)
					{
						ilGenerator.BeginFinallyBlock();
						ilGenerator.Emit(OpCodes.Ldloc, syncObject);
						ilGenerator.Emit(OpCodes.Call, monitorExit);
						ilGenerator.EndExceptionBlock();
					}
					if(retTypeWrapper != PrimitiveTypeWrapper.VOID)
					{
						ilGenerator.Emit(OpCodes.Ldloc, retValue);
					}
					ilGenerator.Emit(OpCodes.Ret);
				}
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					throw new InvalidOperationException("InnerClasses is only available for finished types");
				}
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					throw new InvalidOperationException("DeclaringTypeWrapper is only available for finished types");
				}
			}

			internal override Modifiers ReflectiveModifiers
			{
				get
				{
					ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
					if(innerclasses != null)
					{
						for(int i = 0; i < innerclasses.Length; i++)
						{
							if(innerclasses[i].innerClass != 0)
							{
								if(classFile.GetConstantPoolClass(innerclasses[i].innerClass) == wrapper.Name)
								{
									return innerclasses[i].accessFlags;
								}
							}
						}
					}
					return classFile.Modifiers;
				}
			}

			private void UpdateClashTable()
			{
				lock(this)
				{
					if(memberclashtable == null)
					{
						memberclashtable = new Hashtable();
						for(int i = 0; i < methods.Length; i++)
						{
							// TODO at the moment we don't support constructor signature clash resolving, so we better
							// not put them in the clash table
							if(methods[i].IsLinked && methods[i].GetMethod() != null && methods[i].Name != "<init>")
							{
								string key = GenerateClashKey("method", methods[i].RealName, methods[i].ReturnTypeForDefineMethod, methods[i].GetParametersForDefineMethod());
								memberclashtable.Add(key, key);
							}
						}
					}
				}
			}

			private static string GenerateClashKey(string type, string name, Type retOrFieldType, Type[] args)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder(type);
				sb.Append(':').Append(name).Append(':').Append(retOrFieldType.FullName);
				if(args != null)
				{
					foreach(Type t in args)
					{
						sb.Append(':').Append(t.FullName);
					}
				}
				return sb.ToString();
			}

			private ConstructorBuilder DefineClassInitializer()
			{
				if(typeBuilder.IsInterface)
				{
					// LAMESPEC the ECMA spec says (part. I, sect. 8.5.3.2) that all interface members must be public, so we make
					// the class constructor public
					return typeBuilder.DefineConstructor(MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
				}
				// NOTE we don't need to record the modifiers here, because they aren't visible from Java reflection
				return typeBuilder.DefineTypeInitializer();
			}

			// this finds the method that md is going to be overriding
			private MethodWrapper FindBaseMethod(string name, string sig, out bool explicitOverride)
			{
				Debug.Assert(!classFile.IsInterface);
				Debug.Assert(name != "<init>");

				explicitOverride = false;
				TypeWrapper tw = wrapper.BaseTypeWrapper;
				while(tw != null)
				{
					MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
					if(baseMethod == null)
					{
						return null;
					}
					// here are the complex rules for determining whether this method overrides the method we found
					// RULE 1: final methods may not be overridden
					// (note that we intentionally not check IsStatic here!)
					if(baseMethod.IsFinal
						&& !baseMethod.IsPrivate
						&& (baseMethod.IsPublic || baseMethod.IsProtected || baseMethod.DeclaringType.IsInSamePackageAs(wrapper)))
					{
						throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overriden in " + wrapper.Name);
					}
					// RULE 1a: static methods are ignored (other than the RULE 1 check)
					if(baseMethod.IsStatic)
					{
					}
					// RULE 2: public & protected methods can be overridden (package methods are handled by RULE 4)
					// (by public, protected & *package* methods [even if they are in a different package])
					else if(baseMethod.IsPublic || baseMethod.IsProtected)
					{
						// if we already encountered a package method, we cannot override the base method of
						// that package method
						if(explicitOverride)
						{
							explicitOverride = false;
							return null;
						}
						return baseMethod;
					}
					// RULE 3: private and static methods are ignored
					else if(!baseMethod.IsPrivate)
					{
						// RULE 4: package methods can only be overridden in the same package
						if(baseMethod.DeclaringType.IsInSamePackageAs(wrapper)
							|| (baseMethod.IsInternal && baseMethod.DeclaringType.GetClassLoader() == wrapper.GetClassLoader()))
						{
							return baseMethod;
						}
						// since we encountered a method with the same name/signature that we aren't overriding,
						// we need to specify an explicit override
						// NOTE we only do this if baseMethod isn't private, because if it is, Reflection.Emit
						// will complain about the explicit MethodOverride (possibly a bug)
						explicitOverride = true;
					}
					tw = baseMethod.DeclaringType.BaseTypeWrapper;
				}
				return null;
			}

			internal string GenerateUniqueMethodName(string basename, MethodWrapper mw)
			{
				return GenerateUniqueMethodName(basename, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
			}

			internal string GenerateUniqueMethodName(string basename, Type returnType, Type[] parameterTypes)
			{
				string name = basename;
				string key = GenerateClashKey("method", name, returnType, parameterTypes);
				UpdateClashTable();
				lock(memberclashtable.SyncRoot)
				{
					for(int clashcount = 0; memberclashtable.ContainsKey(key); clashcount++)
					{
						name = basename + "_" + clashcount;
						key = GenerateClashKey("method", name, returnType, parameterTypes);
					}
					memberclashtable.Add(key, key);
				}
				return name;
			}

			private static MethodInfo GetBaseFinalizeMethod(TypeWrapper wrapper, out bool clash)
			{
				clash = false;
				for(;;)
				{
					// HACK we get called during method linking (which is probably a bad idea) and
					// it is possible for the base type not to be finished yet, so we look at the
					// private state of the unfinished base types to find the finalize method.
					DynamicTypeWrapper dtw = wrapper as DynamicTypeWrapper;
					if(dtw == null)
					{
						break;
					}
					JavaTypeImpl impl = dtw.impl as JavaTypeImpl;
					if(impl == null)
					{
						break;
					}
					MethodWrapper mw = dtw.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, false);
					if(mw != null)
					{
						mw.Link();
					}
					if(impl.finalizeMethod != null)
					{
						return impl.finalizeMethod;
					}
					wrapper = wrapper.BaseTypeWrapper;
				}
				Type type = wrapper.TypeAsBaseType;
				MethodInfo baseFinalize = type.GetMethod("__<Finalize>", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
				if(baseFinalize != null)
				{
					return baseFinalize;
				}
				while(type != null)
				{
					foreach(MethodInfo m in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
					{
						if(m.Name == "Finalize"
							&& m.ReturnType == typeof(void)
							&& m.GetParameters().Length == 0)
						{
							if(m.GetBaseDefinition().DeclaringType == typeof(object))
							{
								return m;
							}
							clash = true;
						}
					}
					type = type.BaseType;
				}
				return null;
			}

			private MethodBase GenerateMethod(int index, bool unloadableOverrideStub)
			{
				methods[index].AssertLinked();
				Profiler.Enter("JavaTypeImpl.GenerateMethod");
				try
				{
					if(index >= classFile.Methods.Length)
					{
						if(methods[index].IsMirandaMethod)
						{
							// We're a Miranda method
							Debug.Assert(baseMethods[index].DeclaringType.IsInterface);
							string name = GenerateUniqueMethodName(methods[index].Name, baseMethods[index]);
							MethodBuilder mb = typeBuilder.DefineMethod(methods[index].Name, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.CheckAccessOnOverride, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							AttributeHelper.HideFromReflection(mb);
#if STATIC_COMPILER
							if(unloadableOverrideStub || name != methods[index].Name)
							{
								// instead of creating an override stub, we created the Miranda method with the proper signature and
								// decorate it with a NameSigAttribute that contains the real signature
								AttributeHelper.SetNameSig(mb, methods[index].Name, methods[index].Signature);
							}
#endif // STATIC_COMPILER
							// if we changed the name or if the interface method name is remapped, we need to add an explicit methodoverride.
							if(!baseMethods[index].DeclaringType.IsDynamicOnly && name != baseMethods[index].RealName)
							{
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethods[index].GetMethod());
							}
							return mb;
						}
						else if(methods[index].IsAccessStub)
						{
							MethodAttributes stubattribs = baseMethods[index].IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
							stubattribs |= MethodAttributes.HideBySig;
							if(baseMethods[index].IsStatic)
							{
								stubattribs |= MethodAttributes.Static;
							}
							else
							{
								stubattribs |= MethodAttributes.CheckAccessOnOverride | MethodAttributes.Virtual;
								if(baseMethods[index].IsAbstract && wrapper.IsAbstract)
								{
									stubattribs |= MethodAttributes.Abstract;
								}
								if(baseMethods[index].IsFinal)
								{
									// NOTE final methods still need to be virtual, because a subclass may need this method to
									// implement an interface method
									stubattribs |= MethodAttributes.Final | MethodAttributes.NewSlot;
								}
							}
							MethodBuilder mb = typeBuilder.DefineMethod(methods[index].Name, stubattribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							AttributeHelper.HideFromReflection(mb);
							if(!baseMethods[index].IsAbstract)
							{
								ILGenerator ilgen = mb.GetILGenerator();
								int argc = methods[index].GetParametersForDefineMethod().Length + (methods[index].IsStatic ? 0 : 1);
								for(int i = 0; i < argc; i++)
								{
									ilgen.Emit(OpCodes.Ldarg_S, (byte)i);
								}
								baseMethods[index].EmitCall(ilgen);
								ilgen.Emit(OpCodes.Ret);
							}
							else if(!wrapper.IsAbstract)
							{
								EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", wrapper.Name + "." + methods[index].Name + methods[index].Signature);
							}
							return mb;
						}
						else
						{
							throw new InvalidOperationException();
						}
					}
					ClassFile.Method m = classFile.Methods[index];
					MethodBase method;
					bool setNameSig = methods[index].ReturnType.IsErasedOrBoxedPrimitiveOrRemapped;
					foreach(TypeWrapper tw in methods[index].GetParameters())
					{
						setNameSig |= tw.IsErasedOrBoxedPrimitiveOrRemapped;
					}
					bool setModifiers = false;
					MethodAttributes attribs = MethodAttributes.HideBySig;
					if(m.IsNative)
					{
						if(wrapper.IsPInvokeMethod(m))
						{
							// this doesn't appear to be necessary, but we use the flag in Finish to know
							// that we shouldn't emit a method body
							attribs |= MethodAttributes.PinvokeImpl;
						}
						else
						{
							setModifiers = true;
						}
					}
					if(m.IsPrivate)
					{
						attribs |= MethodAttributes.Private;
					}
					else if(m.IsProtected)
					{
						attribs |= MethodAttributes.FamORAssem;
					}
					else if(m.IsPublic)
					{
						attribs |= MethodAttributes.Public;
					}
					else
					{
						attribs |= MethodAttributes.Assembly;
					}
					if(ReferenceEquals(m.Name, StringConstants.INIT))
					{
						if(setNameSig)
						{
							// TODO we might have to mangle the signature to make it unique
						}
						// strictfp is the only modifier that a constructor can have
						if(m.IsStrictfp)
						{
							setModifiers = true;
						}
						method = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, methods[index].GetParametersForDefineMethod());
						((ConstructorBuilder)method).SetImplementationFlags(MethodImplAttributes.NoInlining);
					}
					else if(m.IsClassInitializer)
					{
						method = DefineClassInitializer();
					}
					else
					{
						if(m.IsAbstract)
						{
							// only if the classfile is abstract, we make the CLR method abstract, otherwise,
							// we have to generate a method that throws an AbstractMethodError (because the JVM
							// allows abstract methods in non-abstract classes)
							if(classFile.IsAbstract)
							{
								if(classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
								{
									setModifiers = true;
								}
								else
								{
									attribs |= MethodAttributes.Abstract;
								}
							}
							else
							{
								setModifiers = true;
							}
						}
						if(m.IsFinal)
						{
							if(!m.IsStatic && !m.IsPrivate)
							{
								attribs |= MethodAttributes.Final;
							}
							else
							{
								setModifiers = true;
							}
						}
						if(m.IsStatic)
						{
							attribs |= MethodAttributes.Static;
							if(m.IsSynchronized)
							{
								setModifiers = true;
							}
						}
						else if(!m.IsPrivate)
						{
							attribs |= MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
						}
						string name = m.Name;
#if STATIC_COMPILER
						if((m.Modifiers & Modifiers.Bridge) != 0 && (m.IsPublic || m.IsProtected) && wrapper.IsPublic)
						{
							string sigbase = m.Signature.Substring(0, m.Signature.LastIndexOf(')') + 1);
							foreach(MethodWrapper mw in methods)
							{
								if(mw.Name == m.Name && mw.Signature.StartsWith(sigbase) && mw.Signature != m.Signature)
								{
									// To prevent bridge methods with covariant return types from confusing
									// other .NET compilers (like C#), we rename the bridge method.
									name = "<bridge>" + name;
									setNameSig = true;
									break;
								}
							}
						}
#endif
						// if a method is virtual, we need to find the method it overrides (if any), for several reasons:
						// - if we're overriding a method that has a different name (e.g. some of the virtual methods
						//   in System.Object [Equals <-> equals]) we need to add an explicit MethodOverride
						// - if one of the base classes has a similar method that is private (or package) that we aren't
						//   overriding, we need to specify an explicit MethodOverride
						MethodWrapper baseMce = baseMethods[index];
						bool explicitOverride = methods[index].IsExplicitOverride;
						if((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
						{
							// make sure the base method is already defined
							Debug.Assert(baseMce == null || baseMce.GetMethod() != null);
							if(baseMce == null || baseMce.DeclaringType.IsInterface)
							{
								// we need to set NewSlot here, to prevent accidentally overriding methods
								// (for example, if a Java class has a method "boolean Equals(object)", we don't want that method
								// to override System.Object.Equals)
								attribs |= MethodAttributes.NewSlot;
							}
							else
							{
								// if we have a method overriding a more accessible method (the JVM allows this), we need to make the
								// method more accessible, because otherwise the CLR will complain that we're reducing access
								MethodBase baseMethod = baseMce.GetMethod();
								if((baseMethod.IsPublic && !m.IsPublic) ||
									((baseMethod.IsFamily || baseMethod.IsFamilyOrAssembly) && !m.IsPublic && !m.IsProtected) ||
									(!m.IsPublic && !m.IsProtected && !baseMce.DeclaringType.IsInSamePackageAs(wrapper)))
								{
									attribs &= ~MethodAttributes.MemberAccessMask;
									attribs |= baseMethod.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
									setModifiers = true;
								}
							}
						}
						MethodBuilder mb = null;
#if STATIC_COMPILER
						mb = wrapper.DefineGhostMethod(name, attribs, methods[index]);
#endif
						if(mb == null)
						{
							bool needFinalize = false;
							bool needDispatch = false;
							bool finalizeClash = false;
							MethodInfo baseFinalize = null;
							if(baseMce != null && ReferenceEquals(m.Name, StringConstants.FINALIZE) && ReferenceEquals(m.Signature, StringConstants.SIG_VOID))
							{
								baseFinalize = GetBaseFinalizeMethod(wrapper.BaseTypeWrapper, out finalizeClash);
								if(baseMce.RealName == "Finalize")
								{
									// We're overriding Finalize (that was renamed to finalize by DotNetTypeWrapper)
									// in a non-Java base class.
									attribs |= MethodAttributes.NewSlot;
									needFinalize = true;
									needDispatch = true;
								}
								else if(baseMce.DeclaringType == CoreClasses.java.lang.Object.Wrapper)
								{
									// This type is the first type in the hierarchy to introduce a finalize method
									// (other than the one in java.lang.Object obviously), so we need to override
									// the real Finalize method and emit a dispatch call to our finalize method.
									needFinalize = true;
									needDispatch = true;
								}
								else if(m.IsFinal)
								{
									// One of our base classes already has a  finalize method, so we already are
									// hooked into the real Finalize, but we need to override it again, to make it
									// final (so that non-Java types cannot override it either).
									needFinalize = true;
									needDispatch = false;
									// If the base class finalize was optimized away, we need a dispatch call after all.
									if(baseFinalize.DeclaringType == typeof(object))
									{
										needDispatch = true;
									}
								}
								else
								{
									// One of our base classes already has a finalize method, but it may have been an empty
									// method so that the hookup to the real Finalize was optimized away, we need to check
									// for that.
									if(baseFinalize.DeclaringType == typeof(object))
									{
										needFinalize = true;
										needDispatch = true;
									}
								}
								if(needFinalize &&
									!m.IsAbstract && !m.IsNative &&
									(!m.IsFinal || classFile.IsFinal) &&
									m.Instructions.Length > 0 &&
									m.Instructions[0].NormalizedOpCode == NormalizedByteCode.__return)
								{
									// we've got an empty finalize method, so we don't need to override the real finalizer
									// (not having a finalizer makes a huge perf difference)
									needFinalize = false;
								}
							}
							if(setNameSig || memberclashtable != null)
							{
								// TODO we really should make sure that the name we generate doesn't already exist in a
								// base class (not in the Java method namespace, but in the CLR method namespace)
								name = GenerateUniqueMethodName(name, methods[index]);
								if(name != m.Name)
								{
									setNameSig = true;
								}
							}
							bool needMethodImpl = baseMce != null && (explicitOverride || baseMce.RealName != name) && !needFinalize;
							if(unloadableOverrideStub || needMethodImpl)
							{
								attribs |= MethodAttributes.NewSlot;
							}
							mb = typeBuilder.DefineMethod(name, attribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							if(unloadableOverrideStub)
							{
								GenerateUnloadableOverrideStub(baseMce, mb, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
							}
							else if(needMethodImpl)
							{
								// assert that the method we're overriding is in fact virtual and not final!
								Debug.Assert(baseMce.GetMethod().IsVirtual && !baseMce.GetMethod().IsFinal);
								typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMce.GetMethod());
							}
							if(!m.IsStatic && !m.IsAbstract && !m.IsPrivate && baseMce != null && !baseMce.DeclaringType.IsInSamePackageAs(wrapper))
							{
								// we may have to explicitly override another package accessible abstract method
								TypeWrapper btw = baseMce.DeclaringType.BaseTypeWrapper;
								while(btw != null)
								{
									MethodWrapper bmw = btw.GetMethodWrapper(m.Name, m.Signature, true);
									if(bmw == null)
									{
										break;
									}
									if(bmw.DeclaringType.IsInSamePackageAs(wrapper) && bmw.IsAbstract && !(bmw.IsPublic || bmw.IsProtected))
									{
										if(bmw != baseMce)
										{
											typeBuilder.DefineMethodOverride(mb, (MethodInfo)bmw.GetMethod());
										}
										break;
									}
									btw = bmw.DeclaringType.BaseTypeWrapper;
								}
							}
							// if we're overriding java.lang.Object.finalize we need to emit a stub to override System.Object.Finalize,
							// or if we're subclassing a non-Java class that has a Finalize method, we need a new Finalize override
							if(needFinalize)
							{
								string finalizeName = finalizeClash ? "__<Finalize>" : baseFinalize.Name;
								// if the Java class also defines a Finalize() method, we need to name the stub differently
								foreach(ClassFile.Method mi in classFile.Methods)
								{
									if(mi.Name == "Finalize" && mi.Signature == "()V")
									{
										finalizeName = "__<Finalize>";
										break;
									}
								}
								MethodAttributes attr = MethodAttributes.HideBySig | MethodAttributes.Virtual;
								// make sure we don't reduce accessibility
								attr |= baseFinalize.IsPublic ? MethodAttributes.Public : MethodAttributes.Family;
								if(m.IsFinal)
								{
									attr |= MethodAttributes.Final;
								}
								finalizeMethod = typeBuilder.DefineMethod(finalizeName, attr, CallingConventions.Standard, typeof(void), Type.EmptyTypes);
								if(finalizeName != baseFinalize.Name)
								{
									typeBuilder.DefineMethodOverride(finalizeMethod, baseFinalize);
								}
								AttributeHelper.HideFromJava(finalizeMethod);
								ILGenerator ilgen = finalizeMethod.GetILGenerator();
								ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.SkipFinalizer);
								Label skip = ilgen.DefineLabel();
								ilgen.Emit(OpCodes.Brtrue_S, skip);
								if(needDispatch)
								{
									ilgen.BeginExceptionBlock();
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Callvirt, mb);
									ilgen.BeginCatchBlock(typeof(object));
									ilgen.EndExceptionBlock();
								}
								else
								{
									ilgen.Emit(OpCodes.Ldarg_0);
									ilgen.Emit(OpCodes.Call, baseFinalize);
								}
								ilgen.MarkLabel(skip);
								ilgen.Emit(OpCodes.Ret);
							}
#if STATIC_COMPILER
							if(classFile.Methods[index].AnnotationDefault != null)
							{
								CustomAttributeBuilder cab = new CustomAttributeBuilder(StaticCompiler.GetType("IKVM.Attributes.AnnotationDefaultAttribute").GetConstructor(new Type[] { typeof(object) }), new object[] { classFile.Methods[index].AnnotationDefault });
								mb.SetCustomAttribute(cab);
							}
#endif // STATIC_COMPILER
						}
						method = mb;
					}
					string[] exceptions = m.ExceptionsAttribute;
					methods[index].SetDeclaredExceptions(exceptions);
#if STATIC_COMPILER
					AttributeHelper.SetThrowsAttribute(method, exceptions);
					if(setModifiers || m.IsInternal || (m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0)
					{
						if(method is ConstructorBuilder)
						{
							AttributeHelper.SetModifiers((ConstructorBuilder)method, m.Modifiers, m.IsInternal);
						}
						else
						{
							AttributeHelper.SetModifiers((MethodBuilder)method, m.Modifiers, m.IsInternal);
						}
					}
					if((m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0
						&& (m.IsPublic || m.IsProtected) && wrapper.IsPublic)
					{
						if(method is ConstructorBuilder)
						{
							AttributeHelper.SetEditorBrowsableNever((ConstructorBuilder)method);
						}
						else
						{
							AttributeHelper.SetEditorBrowsableNever((MethodBuilder)method);
						}
						// TODO on WHIDBEY apply CompilerGeneratedAttribute
					}
					if(m.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(method);
					}
					if(setNameSig)
					{
						AttributeHelper.SetNameSig(method, m.Name, m.Signature);
					}
					if(m.GenericSignature != null)
					{
						AttributeHelper.SetSignatureAttribute(method, m.GenericSignature);
					}
#else // STATIC_COMPILER
					if(setModifiers)
					{
						// shut up the compiler
					}
#endif // STATIC_COMPILER
					return method;
				}
				finally
				{
					Profiler.Leave("JavaTypeImpl.GenerateMethod");
				}
			}

			private void GenerateUnloadableOverrideStub(MethodWrapper baseMethod, MethodInfo target, Type targetRet, Type[] targetArgs)
			{
				Type stubret = baseMethod.ReturnTypeForDefineMethod;
				Type[] stubargs = baseMethod.GetParametersForDefineMethod();
				string name = GenerateUniqueMethodName(baseMethod.RealName + "/unloadablestub", baseMethod);
				MethodBuilder overrideStub = typeBuilder.DefineMethod(name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final, stubret, stubargs);
				AttributeHelper.HideFromJava(overrideStub);
				typeBuilder.DefineMethodOverride(overrideStub, (MethodInfo)baseMethod.GetMethod());
				ILGenerator ilgen = overrideStub.GetILGenerator();
				ilgen.Emit(OpCodes.Ldarg_0);
				for(int i = 0; i < targetArgs.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldarg_S, (byte)(i + 1));
					if(targetArgs[i] != stubargs[i])
					{
						ilgen.Emit(OpCodes.Castclass, targetArgs[i]);
					}
				}
				ilgen.Emit(OpCodes.Callvirt, target);
				if(targetRet != stubret)
				{
					ilgen.Emit(OpCodes.Castclass, stubret);
				}
				ilgen.Emit(OpCodes.Ret);
			}

			private static bool CheckRequireOverrideStub(MethodWrapper mw1, MethodWrapper mw2)
			{
				// TODO this is too late to generate LinkageErrors so we need to figure this out earlier
				if(mw1.ReturnType != mw2.ReturnType  && !(mw1.ReturnType.IsUnloadable && mw2.ReturnType.IsUnloadable))
				{
					return true;
				}
				TypeWrapper[] args1 = mw1.GetParameters();
				TypeWrapper[] args2 = mw2.GetParameters();
				for(int i = 0; i < args1.Length; i++)
				{
					if(args1[i] != args2[i] && !(args1[i].IsUnloadable && args2[i].IsUnloadable))
					{
						return true;
					}
				}
				return false;
			}

			private void AddMethodOverride(MethodWrapper method, MethodBuilder mb, TypeWrapper iface, string name, string sig, ref Hashtable hashtable, bool unloadableOnly)
			{
				if(hashtable != null && hashtable.ContainsKey(iface))
				{
					return;
				}
				MethodWrapper mw = iface.GetMethodWrapper(name, sig, false);
				if(mw != null)
				{
					if(hashtable == null)
					{
						hashtable = new Hashtable();
					}
					hashtable.Add(iface, iface);
					if(CheckRequireOverrideStub(method, mw))
					{
						GenerateUnloadableOverrideStub(mw, mb, method.ReturnTypeForDefineMethod, method.GetParametersForDefineMethod());
					}
					else if(!unloadableOnly)
					{
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
					}
				}
				foreach(TypeWrapper iface2 in iface.Interfaces)
				{
					AddMethodOverride(method, mb, iface2, name, sig, ref hashtable, unloadableOnly);
				}
			}

			internal override Type Type
			{
				get
				{
					return typeBuilder;
				}
			}

			internal override string GetGenericSignature()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string[] GetEnclosingMethod()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string GetGenericMethodSignature(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override string GetGenericFieldSignature(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetDeclaredAnnotations()
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object GetMethodDefaultValue(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetMethodAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[][] GetParameterAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

			internal override object[] GetFieldAnnotations(int index)
			{
				Debug.Fail("Unreachable code");
				return null;
			}

#if STATIC_COMPILER
			internal override Annotation Annotation
			{
				get
				{
					return annotationBuilder;
				}
			}

			internal override Type EnumType
			{
				get
				{
					return enumBuilder;
				}
			}
#endif // STATIC_COMPILER
		}

		private sealed class Metadata
		{
			private string[] genericMetaData;
			private object[][] annotations;

			private Metadata(string[] genericMetaData, object[][] annotations)
			{
				this.genericMetaData = genericMetaData;
				this.annotations = annotations;
			}

			internal static Metadata Create(ClassFile classFile)
			{
				if(classFile.MajorVersion < 49)
				{
					return null;
				}
				string[] genericMetaData = null;
				object[][] annotations = null;
				for(int i = 0; i < classFile.Methods.Length; i++)
				{
					if(classFile.Methods[i].GenericSignature != null)
					{
						if(genericMetaData == null)
						{
							genericMetaData = new string[classFile.Methods.Length + classFile.Fields.Length + 4];
						}
						genericMetaData[i + 4] = classFile.Methods[i].GenericSignature;
					}
					if(classFile.Methods[i].Annotations != null)
					{
						if(annotations == null)
						{
							annotations = new object[5][];
						}
						if(annotations[1] == null)
						{
							annotations[1] = new object[classFile.Methods.Length];
						}
						annotations[1][i] = classFile.Methods[i].Annotations;
					}
					if(classFile.Methods[i].ParameterAnnotations != null)
					{
						if(annotations == null)
						{
							annotations = new object[5][];
						}
						if(annotations[2] == null)
						{
							annotations[2] = new object[classFile.Methods.Length];
						}
						annotations[2][i] = classFile.Methods[i].ParameterAnnotations;
					}
					if(classFile.Methods[i].AnnotationDefault != null)
					{
						if(annotations == null)
						{
							annotations = new object[5][];
						}
						if(annotations[3] == null)
						{
							annotations[3] = new object[classFile.Methods.Length];
						}
						annotations[3][i] = classFile.Methods[i].AnnotationDefault;
					}
				}
				for(int i = 0; i < classFile.Fields.Length; i++)
				{
					if(classFile.Fields[i].GenericSignature != null)
					{
						if(genericMetaData == null)
						{
							genericMetaData = new string[classFile.Methods.Length + classFile.Fields.Length + 4];
						}
						genericMetaData[i + 4 + classFile.Methods.Length] = classFile.Fields[i].GenericSignature;
					}
					if(classFile.Fields[i].Annotations != null)
					{
						if(annotations == null)
						{
							annotations = new object[5][];
						}
						if(annotations[4] == null)
						{
							annotations[4] = new object[classFile.Fields.Length][];
						}
						annotations[4][i] = classFile.Fields[i].Annotations;
					}
				}
				if(classFile.EnclosingMethod != null)
				{
					if(genericMetaData == null)
					{
						genericMetaData = new string[classFile.Methods.Length + classFile.Fields.Length + 4];
					}
					genericMetaData[0] = classFile.EnclosingMethod[0];
					genericMetaData[1] = classFile.EnclosingMethod[1];
					genericMetaData[2] = classFile.EnclosingMethod[2];
				}
				if(classFile.GenericSignature != null)
				{
					if(genericMetaData == null)
					{
						genericMetaData = new string[classFile.Methods.Length + classFile.Fields.Length + 4];
					}
					genericMetaData[3] = classFile.GenericSignature;
				}
				if(classFile.Annotations != null)
				{
					if(annotations == null)
					{
						annotations = new object[5][];
					}
					annotations[0] = classFile.Annotations;
				}
				if(genericMetaData != null || annotations != null)
				{
					return new Metadata(genericMetaData, annotations);
				}
				return null;
			}

			internal static string GetGenericSignature(Metadata m)
			{
				if(m != null && m.genericMetaData != null)
				{
					return m.genericMetaData[3];
				}
				return null;
			}

			internal static string[] GetEnclosingMethod(Metadata m)
			{
				if(m != null && m.genericMetaData != null && m.genericMetaData[0] != null)
				{
					return new string[] { m.genericMetaData[0], m.genericMetaData[1], m.genericMetaData[2] };
				}
				return null;
			}

			internal static string GetGenericMethodSignature(Metadata m, int index)
			{
				if(m != null && m.genericMetaData != null)
				{
					return m.genericMetaData[index + 4];
				}
				return null;
			}

			// note that the caller is responsible for computing the correct index (field index + method count)
			internal static string GetGenericFieldSignature(Metadata m, int index)
			{
				if(m != null && m.genericMetaData != null)
				{
					return m.genericMetaData[index + 4];
				}
				return null;
			}

			internal static object[] GetAnnotations(Metadata m)
			{
				if(m != null && m.annotations != null)
				{
					return m.annotations[0];
				}
				return null;
			}

			internal static object[] GetMethodAnnotations(Metadata m, int index)
			{
				if(m != null && m.annotations != null && m.annotations[1] != null)
				{
					return (object[])m.annotations[1][index];
				}
				return null;
			}

			internal static object[][] GetMethodParameterAnnotations(Metadata m, int index)
			{
				if(m != null && m.annotations != null && m.annotations[2] != null)
				{
					return (object[][])m.annotations[2][index];
				}
				return null;
			}

			internal static object GetMethodDefaultValue(Metadata m, int index)
			{
				if(m != null && m.annotations != null && m.annotations[3] != null)
				{
					return m.annotations[3][index];
				}
				return null;
			}

			// note that unlike GetGenericFieldSignature, the index is simply the field index 
			internal static object[] GetFieldAnnotations(Metadata m, int index)
			{
				if(m != null && m.annotations != null && m.annotations[4] != null)
				{
					return (object[])m.annotations[4][index];
				}
				return null;
			}
		}
	
		private sealed class FinishedTypeImpl : DynamicImpl
		{
			private Type type;
			private TypeWrapper[] innerclasses;
			private TypeWrapper declaringTypeWrapper;
			private Modifiers reflectiveModifiers;
			private MethodInfo clinitMethod;
			private Metadata metadata;
#if STATIC_COMPILER
			private Annotation annotationBuilder;
			private TypeBuilder enumBuilder;
#endif

			internal FinishedTypeImpl(Type type, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers, Metadata metadata
#if STATIC_COMPILER
				, Annotation annotationBuilder
				, TypeBuilder enumBuilder
#endif
				)
			{
				this.type = type;
				this.innerclasses = innerclasses;
				this.declaringTypeWrapper = declaringTypeWrapper;
				this.reflectiveModifiers = reflectiveModifiers;
				this.clinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				this.metadata = metadata;
#if STATIC_COMPILER
				this.annotationBuilder = annotationBuilder;
				this.enumBuilder = enumBuilder;
#endif
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					// TODO compute the innerclasses lazily (and fix JavaTypeImpl to not always compute them)
					return innerclasses;
				}
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					// TODO compute lazily (and fix JavaTypeImpl to not always compute it)
					return declaringTypeWrapper;
				}
			}

			internal override Modifiers ReflectiveModifiers
			{
				get
				{
					return reflectiveModifiers;
				}
			}

			internal override Type Type
			{
				get
				{
					return type;
				}
			}

			internal override void EmitRunClassConstructor(CountingILGenerator ilgen)
			{
				if(clinitMethod != null)
				{
					ilgen.Emit(OpCodes.Call, clinitMethod);
				}
			}

			internal override DynamicImpl Finish()
			{
				return this;
			}

			internal override MethodBase LinkMethod(MethodWrapper mw)
			{
				// we should never be called, because all methods on a finished type are already linked
				Debug.Assert(false);
				return mw.GetMethod();
			}

			internal override FieldInfo LinkField(FieldWrapper fw)
			{
				// we should never be called, because all fields on a finished type are already linked
				Debug.Assert(false);
				return fw.GetField();
			}

			internal override string GetGenericSignature()
			{
				return Metadata.GetGenericSignature(metadata);
			}

			internal override string[] GetEnclosingMethod()
			{
				return Metadata.GetEnclosingMethod(metadata);
			}

			internal override string GetGenericMethodSignature(int index)
			{
				return Metadata.GetGenericMethodSignature(metadata, index);
			}

			// note that the caller is responsible for computing the correct index (field index + method count)
			internal override string GetGenericFieldSignature(int index)
			{
				return Metadata.GetGenericFieldSignature(metadata, index);
			}

			internal override object[] GetDeclaredAnnotations()
			{
				return Metadata.GetAnnotations(metadata);
			}

			internal override object GetMethodDefaultValue(int index)
			{
				return Metadata.GetMethodDefaultValue(metadata, index);
			}

			internal override object[] GetMethodAnnotations(int index)
			{
				return Metadata.GetMethodAnnotations(metadata, index);
			}

			internal override object[][] GetParameterAnnotations(int index)
			{
				return Metadata.GetMethodParameterAnnotations(metadata, index);
			}

			internal override object[] GetFieldAnnotations(int index)
			{
				return Metadata.GetFieldAnnotations(metadata, index);
			}

#if STATIC_COMPILER
			internal override Annotation Annotation
			{
				get
				{
					return annotationBuilder;
				}
			}

			internal override Type EnumType
			{
				get
				{
					return enumBuilder;
				}
			}
#endif // STATIC_COMPILER
		}

		protected static void GetParameterNamesFromLVT(ClassFile.Method m, string[] parameterNames)
		{
			ClassFile.Method.LocalVariableTableEntry[] localVars = m.LocalVariableTableAttribute;
			if(localVars != null)
			{
				for(int i = m.IsStatic ? 0 : 1, pos = 0; i < m.ArgMap.Length; i++)
				{
					// skip double & long fillers
					if(m.ArgMap[i] != -1)
					{
						if(parameterNames[pos] == null)
						{
							for(int j = 0; j < localVars.Length; j++)
							{
								if(localVars[j].index == i)
								{
									parameterNames[pos] = localVars[j].name;
									break;
								}
							}
						}
						pos++;
					}
				}
			}
		}

		protected static void GetParameterNamesFromSig(string sig, string[] parameterNames)
		{
			ArrayList names = new ArrayList();
			for(int i = 1; sig[i] != ')'; i++)
			{
				if(sig[i] == 'L')
				{
					i++;
					int end = sig.IndexOf(';', i);
					names.Add(GetParameterName(sig.Substring(i, end - i)));
					i = end;
				}
				else if(sig[i] == '[')
				{
					while(sig[++i] == '[');
					if(sig[i] == 'L')
					{
						i++;
						int end = sig.IndexOf(';', i);
						names.Add(GetParameterName(sig.Substring(i, end - i)) + "arr");
						i = end;
					}
					else
					{
						switch(sig[i])
						{
							case 'B':
							case 'Z':
								names.Add("barr");
								break;
							case 'C':
								names.Add("charr");
								break;
							case 'S':
								names.Add("sarr");
								break;
							case 'I':
								names.Add("iarr");
								break;
							case 'J':
								names.Add("larr");
								break;
							case 'F':
								names.Add("farr");
								break;
							case 'D':
								names.Add("darr");
								break;
						}
					}
				}
				else
				{
					switch(sig[i])
					{
						case 'B':
						case 'Z':
							names.Add("b");
							break;
						case 'C':
							names.Add("ch");
							break;
						case 'S':
							names.Add("s");
							break;
						case 'I':
							names.Add("i");
							break;
						case 'J':
							names.Add("l");
							break;
						case 'F':
							names.Add("f");
							break;
						case 'D':
							names.Add("d");
							break;
					}
				}
			}
			for(int i = 0; i < parameterNames.Length; i++)
			{
				if(parameterNames[i] == null)
				{
					parameterNames[i] = (string)names[i];
				}
			}
		}

		protected static ParameterBuilder[] GetParameterBuilders(MethodBase mb, int parameterCount, string[] parameterNames)
		{
			ParameterBuilder[] parameterBuilders = new ParameterBuilder[parameterCount];
			Hashtable clashes = null;
			for(int i = 0; i < parameterBuilders.Length; i++)
			{
				string name = null;
				if(parameterNames != null)
				{
					name = parameterNames[i];
					if(Array.IndexOf(parameterNames, name, i + 1) >= 0 || (clashes != null && clashes.ContainsKey(name)))
					{
						if(clashes == null)
						{
							clashes = new Hashtable();
						}
						int clash = 1;
						if(clashes.ContainsKey(name))
						{
							clash = (int)clashes[name] + 1;
						}
						clashes[name] = clash;
						name += clash;
					}
				}
				MethodBuilder mBuilder = mb as MethodBuilder;
				if(mBuilder != null)
				{
					parameterBuilders[i] = mBuilder.DefineParameter(i + 1, ParameterAttributes.None, name);
				}
				else
				{
					parameterBuilders[i] = ((ConstructorBuilder)mb).DefineParameter(i + 1, ParameterAttributes.None, name);
				}
			}
			return parameterBuilders;
		}

		private static string GetParameterName(string type)
		{
			if(type == "java.lang.String")
			{
				return "str";
			}
			else if(type == "java.lang.Object")
			{
				return "obj";
			}
			else
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				for(int i = type.LastIndexOf('.') + 1; i < type.Length; i++)
				{
					if(char.IsUpper(type, i))
					{
						sb.Append(char.ToLower(type[i]));
					}
				}
				return sb.ToString();
			}
		}

#if STATIC_COMPILER
		protected abstract void AddMapXmlFields(ref FieldWrapper[] fields);
		protected abstract bool EmitMapXmlMethodBody(ILGenerator ilgen, ClassFile f, ClassFile.Method m);
		protected abstract void EmitMapXmlMetadata(TypeBuilder typeBuilder, ClassFile classFile, FieldWrapper[] fields, MethodWrapper[] methods);
		protected abstract MethodBuilder DefineGhostMethod(string name, MethodAttributes attribs, MethodWrapper mw);
		protected abstract void FinishGhost(TypeBuilder typeBuilder, MethodWrapper[] methods);
		protected abstract void FinishGhostStep2();
		protected abstract TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs);
#endif // STATIC_COMPILER

		protected virtual bool IsPInvokeMethod(ClassFile.Method m)
		{
			if(m.Annotations != null)
			{
				foreach(object[] annot in m.Annotations)
				{
					if("Lcli/System/Runtime/InteropServices/DllImportAttribute$Annotation;".Equals(annot[1]))
					{
						return true;
					}
				}
			}
			return false;
		}

		internal override MethodBase LinkMethod(MethodWrapper mw)
		{
			mw.AssertLinked();
			return impl.LinkMethod(mw);
		}

		internal override FieldInfo LinkField(FieldWrapper fw)
		{
			fw.AssertLinked();
			return impl.LinkField(fw);
		}

		internal override void EmitRunClassConstructor(CountingILGenerator ilgen)
		{
			impl.EmitRunClassConstructor(ilgen);
		}

		internal override string GetGenericSignature()
		{
			return impl.GetGenericSignature();
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for(int i = 0; i < methods.Length; i++)
			{
				if(methods[i] == mw)
				{
					return impl.GetGenericMethodSignature(i);
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			FieldWrapper[] fields = GetFields();
			for(int i = 0; i < fields.Length; i++)
			{
				if(fields[i] == fw)
				{
					return impl.GetGenericFieldSignature(i + GetMethods().Length);
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override string[] GetEnclosingMethod()
		{
			return impl.GetEnclosingMethod();
		}

		internal override string GetSourceFileName()
		{
			return sourceFileName;
		}

		private int GetMethodBaseToken(MethodBase mb)
		{
			ConstructorInfo ci = mb as ConstructorInfo;
			if(ci != null)
			{
				return classLoader.GetTypeWrapperFactory().ModuleBuilder.GetConstructorToken(ci).Token;
			}
			else
			{
				return classLoader.GetTypeWrapperFactory().ModuleBuilder.GetMethodToken((MethodInfo)mb).Token;
			}
		}

#if !STATIC_COMPILER
		internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
		{
			if(lineNumberTables != null)
			{
				int token = GetMethodBaseToken(mb);
				MethodWrapper[] methods = GetMethods();
				for(int i = 0; i < methods.Length; i++)
				{
					if(GetMethodBaseToken(methods[i].GetMethod()) == token)
					{
						if(lineNumberTables[i] != null)
						{
							return new LineNumberTableAttribute(lineNumberTables[i]).GetLineNumber(ilOffset);
						}
						break;
					}
				}
			}
			return -1;
		}

		internal override object[] GetDeclaredAnnotations()
		{
			object[] annotations = impl.GetDeclaredAnnotations();
			if(annotations != null)
			{
				object[] objs = new object[annotations.Length];
				for(int i = 0; i < annotations.Length; i++)
				{
					objs[i] = JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[i]);
				}
				return objs;
			}
			return null;
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for(int i = 0; i < methods.Length; i++)
			{
				if(methods[i] == mw)
				{
					object[] annotations = impl.GetMethodAnnotations(i);
					if(annotations != null)
					{
						object[] objs = new object[annotations.Length];
						for(int j = 0; j < annotations.Length; j++)
						{
							objs[j] = JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j]);
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for(int i = 0; i < methods.Length; i++)
			{
				if(methods[i] == mw)
				{
					object[][] annotations = impl.GetParameterAnnotations(i);
					if(annotations != null)
					{
						object[][] objs = new object[annotations.Length][];
						for(int j = 0; j < annotations.Length; j++)
						{
							objs[j] = new object[annotations[j].Length];
							for(int k = 0; k < annotations[j].Length; k++)
							{
								objs[j][k] = JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j][k]);
							}
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object[] GetFieldAnnotations(FieldWrapper fw)
		{
			FieldWrapper[] fields = GetFields();
			for(int i = 0; i < fields.Length; i++)
			{
				if(fields[i] == fw)
				{
					object[] annotations = impl.GetFieldAnnotations(i);
					if(annotations != null)
					{
						object[] objs = new object[annotations.Length];
						for(int j = 0; j < annotations.Length; j++)
						{
							objs[j] = JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), annotations[j]);
						}
						return objs;
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}

		internal override object GetAnnotationDefault(MethodWrapper mw)
		{
			MethodWrapper[] methods = GetMethods();
			for(int i = 0; i < methods.Length; i++)
			{
				if(methods[i] == mw)
				{
					object defVal = impl.GetMethodDefaultValue(i);
					if(defVal != null)
					{
						return JVM.Library.newAnnotationElementValue(mw.DeclaringType.GetClassLoader().GetJavaClassLoader(), mw.ReturnType.ClassObject, defVal);
					}
					return null;
				}
			}
			Debug.Fail("Unreachable code");
			return null;
		}
#endif
	}
#endif // !COMPACT_FRAMEWORK

	class CompiledTypeWrapper : TypeWrapper
	{
		private readonly Type type;
		private TypeWrapper[] interfaces;
		private TypeWrapper[] innerclasses;
		private MethodInfo clinitMethod;
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

			protected override void LazyPublishMembers()
			{
				ArrayList methods = new ArrayList();
				ArrayList fields = new ArrayList();
				MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				foreach(MemberInfo m in members)
				{
					if(!AttributeHelper.IsHideFromJava(m))
					{
						MethodBase method = m as MethodBase;
						if(method != null &&
							(remappedType.IsSealed || !m.Name.StartsWith("instancehelper_")) &&
							(!remappedType.IsSealed || method.IsStatic))
						{
							methods.Add(CreateRemappedMethodWrapper(method));
						}
						else
						{
							FieldInfo field = m as FieldInfo;
							if(field != null)
							{
								fields.Add(CreateFieldWrapper(field));
							}
						}
					}
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
						GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes);
						if(nestedHelper != null)
						{
							mbHelper = nestedHelper.GetMethod(m.Name);
							if(mbHelper == null)
							{
								mbHelper = method;
							}
						}
						methods.Add(new CompiledRemappedMethodWrapper(this, m.Name, sig, method, retType, paramTypes, modifiers, false, mbHelper, null));
					}
				}
				SetMethods((MethodWrapper[])methods.ToArray(typeof(MethodWrapper)));
				SetFields((FieldWrapper[])fields.ToArray(typeof(FieldWrapper)));
			}

			private MethodWrapper CreateRemappedMethodWrapper(MethodBase mb)
			{
				ExModifiers modifiers = AttributeHelper.GetModifiers(mb, false);
				string name;
				string sig;
				TypeWrapper retType;
				TypeWrapper[] paramTypes;
				GetNameSigFromMethodBase(mb, out name, out sig, out retType, out paramTypes);
				MethodInfo mbHelper = mb as MethodInfo;
				bool hideFromReflection = mbHelper != null && AttributeHelper.IsHideFromReflection(mbHelper);
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
			private FieldInfo ghostRefField;
			private Type typeAsBaseType;

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
		}

		internal static string GetName(Type type)
		{
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
				if(type.DeclaringType != null)
				{
					return GetName(type.DeclaringType) + "$" + type.Name;
				}
			}
			return type.FullName;
		}

		// TODO consider resolving the baseType lazily
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
					if(attr.Type == typeof(object))
					{
						return null;
					}
					else
					{
						return CoreClasses.java.lang.Object.Wrapper;
					}
				}
				return ClassLoaderWrapper.GetWrapperFromType(type.BaseType);
			}
		}

		private CompiledTypeWrapper(ExModifiers exmod, string name, TypeWrapper baseTypeWrapper)
			: base(exmod.Modifiers, name, baseTypeWrapper)
		{
			this.IsInternal = exmod.IsInternal;
		}

		private CompiledTypeWrapper(string name, Type type)
			: this(GetModifiers(type), name, GetBaseTypeWrapper(type))
		{
			Debug.Assert(!(type is TypeBuilder));
			Debug.Assert(!type.Name.EndsWith("[]"));

			this.type = type;
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			return ClassLoaderWrapper.GetAssemblyClassLoader(type.Assembly);
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
			if(type.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
				// TODO do we really need to look for nested attributes? I think all inner classes will have the ModifiersAttribute.
			else if(type.IsNestedPublic)
			{
				modifiers |= Modifiers.Public | Modifiers.Static;
			}
			else if(type.IsNestedPrivate)
			{
				modifiers |= Modifiers.Private | Modifiers.Static;
			}
			else if(type.IsNestedFamily || type.IsNestedFamORAssem)
			{
				modifiers |= Modifiers.Protected | Modifiers.Static;
			}
			else if(type.IsNestedAssembly || type.IsNestedFamANDAssem)
			{
				modifiers |= Modifiers.Static;
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
			return new ExModifiers(modifiers, false);
		}

		internal override bool HasStaticInitializer
		{
			get
			{
				// trigger LazyPublishMembers
				GetMethods();
				return clinitMethod != null;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				if(interfaces == null)
				{
					// NOTE instead of getting the interfaces list from Type, we use a custom
					// attribute to list the implemented interfaces, because Java reflection only
					// reports the interfaces *directly* implemented by the type, not the inherited
					// interfaces. This is significant for serialVersionUID calculation (for example).
					ImplementsAttribute attr = AttributeHelper.GetImplements(type);
					if(attr != null)
					{
						string[] interfaceNames = attr.Interfaces;
						TypeWrapper[] interfaceWrappers = new TypeWrapper[interfaceNames.Length];
						for(int i = 0; i < interfaceWrappers.Length; i++)
						{
							interfaceWrappers[i] = GetClassLoader().LoadClassByDottedName(interfaceNames[i]);
						}
						this.interfaces = interfaceWrappers;
					}
					else
					{
						interfaces = TypeWrapper.EmptyArray;
					}
				}
				return interfaces;
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				// TODO why are we caching this?
				if(innerclasses == null)
				{
					Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
					ArrayList wrappers = new ArrayList();
					for(int i = 0; i < nestedTypes.Length; i++)
					{
						if(!AttributeHelper.IsHideFromJava(nestedTypes[i]))
						{
							wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(nestedTypes[i]));
						}
					}
					foreach(string s in AttributeHelper.GetNonNestedInnerClasses(type))
					{
						wrappers.Add(GetClassLoader().LoadClassByDottedName(s));
					}
					innerclasses = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
				}
				return innerclasses;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
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

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				if (reflectiveModifiers == 0)
				{
					InnerClassAttribute attr = AttributeHelper.GetInnerClass(type);
					if (attr != null)
					{
						reflectiveModifiers = attr.Modifiers;
					}
					else
					{
						reflectiveModifiers = Modifiers;
					}
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
					type = GetClassLoader().FieldTypeWrapperFromSig(sigtype);
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
			ArrayList list = new ArrayList();
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
						sigparam = (string[])list.ToArray(typeof(string));
						sigret = sig.Substring(pos + 1);
						return;
					default:
						list.Add(sig.Substring(pos, 1));
						pos++;
						break;
				}
			}
		}

		private void GetNameSigFromMethodBase(MethodBase method, out string name, out string sig, out TypeWrapper retType, out TypeWrapper[] paramTypes)
		{
			retType = method is ConstructorInfo ? PrimitiveTypeWrapper.VOID : ClassLoaderWrapper.GetWrapperFromType(((MethodInfo)method).ReturnType);
			ParameterInfo[] parameters = method.GetParameters();
			paramTypes = new TypeWrapper[parameters.Length];
			for(int i = 0; i < parameters.Length; i++)
			{
				paramTypes[i] = ClassLoaderWrapper.GetWrapperFromType(parameters[i].ParameterType);
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
					TypeWrapper[] temp = paramTypes;
					paramTypes = new TypeWrapper[sigparams.Length];
					Array.Copy(temp, 1, paramTypes, 0, paramTypes.Length);
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

		protected override void LazyPublishMembers()
		{
			clinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			ArrayList methods = new ArrayList();
			ArrayList fields = new ArrayList();
			MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			foreach(MemberInfo m in members)
			{
				if(!AttributeHelper.IsHideFromJava(m))
				{
					MethodBase method = m as MethodBase;
					if(method != null)
					{
						if(method.IsSpecialName && 
							(method.Name == "op_Implicit" || method.Name.StartsWith("__<")))
						{
							// skip
						}
						else
						{
							string name;
							string sig;
							TypeWrapper retType;
							TypeWrapper[] paramTypes;
							GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes);
							MethodInfo mi = method as MethodInfo;
							bool hideFromReflection = mi != null ? AttributeHelper.IsHideFromReflection(mi) : false;
							MemberFlags flags = hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None;
							ExModifiers mods = AttributeHelper.GetModifiers(method, false);
							if(mods.IsInternal)
							{
								flags |= MemberFlags.InternalAccess;
							}
							methods.Add(MethodWrapper.Create(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags));
						}
					}
					else
					{
						FieldInfo field = m as FieldInfo;
						if(field != null)
						{
							if(field.IsSpecialName && field.Name.StartsWith("__<"))
							{
								// skip
							}
							else
							{
								fields.Add(CreateFieldWrapper(field));
							}
						}
						else
						{
							// NOTE explictly defined properties (in map.xml) are decorated with HideFromJava,
							// so we don't need to worry about them here
							PropertyInfo property = m as PropertyInfo;
							if(property != null)
							{
								// Only AccessStub properties (marked by HideFromReflectionAttribute)
								// are considered here
								if(AttributeHelper.IsHideFromReflection(property))
								{
									fields.Add(new CompiledAccessStubFieldWrapper(this, property));
								}
								else
								{
									fields.Add(CreateFieldWrapper(property));
								}
							}
						}
					}
				}
			}
			SetMethods((MethodWrapper[])methods.ToArray(typeof(MethodWrapper)));
			SetFields((FieldWrapper[])fields.ToArray(typeof(FieldWrapper)));
		}

		private class CompiledRemappedMethodWrapper : SmartMethodWrapper
		{
			private MethodInfo mbHelper;
			private MethodInfo mbNonvirtualHelper;

			internal CompiledRemappedMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, ExModifiers modifiers, bool hideFromReflection, MethodInfo mbHelper, MethodInfo mbNonvirtualHelper)
				: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers.Modifiers,
						(modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None))
			{
				this.mbHelper = mbHelper;
				this.mbNonvirtualHelper = mbNonvirtualHelper;
			}

#if !COMPACT_FRAMEWORK
			protected override void CallImpl(ILGenerator ilgen)
			{
				MethodBase mb = GetMethod();
				MethodInfo mi = mb as MethodInfo;
				if(mi != null)
				{
					ilgen.Emit(OpCodes.Call, mi);
				}
				else
				{
					ilgen.Emit(OpCodes.Call, (ConstructorInfo)mb);
				}
			}

			protected override void CallvirtImpl(ILGenerator ilgen)
			{
				Debug.Assert(!mbHelper.IsStatic || mbHelper.Name.StartsWith("instancehelper_") || mbHelper.DeclaringType.Name == "__Helper");
				if(mbHelper.IsPublic)
				{
					ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
				}
				else
				{
					// HACK the helper is not public, this means that we're dealing with finalize or clone
					ilgen.Emit(OpCodes.Callvirt, (MethodInfo)GetMethod());
				}
			}

			protected override void NewobjImpl(ILGenerator ilgen)
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
					ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)mb);
				}
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				MethodBase mb;
				if(nonVirtual)
				{
					if(DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
					{
						mb = GetMethod();
					}
					else if(mbNonvirtualHelper != null)
					{
						mb = mbNonvirtualHelper;
					}
					else if(mbHelper != null)
					{
						mb = mbHelper;
					}
					else
					{
						// we can end up here if someone calls a constructor with nonVirtual set (which is pointless, but legal)
						mb = GetMethod();
					}
				}
				else
				{
					mb = mbHelper != null ? mbHelper : GetMethod();
				}
				return InvokeImpl(mb, obj, args, nonVirtual);
			}
#endif // !STATIC_COMPILER

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

		private FieldWrapper CreateFieldWrapper(PropertyInfo prop)
		{
			MethodInfo getter = prop.GetGetMethod(true);
			ExModifiers modifiers = AttributeHelper.GetModifiers(getter, false);
			// for static methods AttributeHelper.GetModifiers won't set the Final flag
			modifiers = new ExModifiers(modifiers.Modifiers | Modifiers.Final, modifiers.IsInternal);
			string name = prop.Name;
			TypeWrapper type = ClassLoaderWrapper.GetWrapperFromType(prop.PropertyType);
			NameSigAttribute attr = AttributeHelper.GetNameSig(getter);
			if(attr != null)
			{
				name = attr.Name;
				SigTypePatchUp(attr.Sig, ref type);
			}
			return new GetterFieldWrapper(this, type, null, name, type.SigName, modifiers, getter, prop);
		}

		private FieldWrapper CreateFieldWrapper(FieldInfo field)
		{
			ExModifiers modifiers = AttributeHelper.GetModifiers(field, false);
			string name = field.Name;
			TypeWrapper type = ClassLoaderWrapper.GetWrapperFromType(field.FieldType);
			NameSigAttribute attr = AttributeHelper.GetNameSig(field);
			if(attr != null)
			{
				name = attr.Name;
				SigTypePatchUp(attr.Sig, ref type);
			}

			if(field.IsLiteral)
			{
				MemberFlags flags = MemberFlags.None;
				if(AttributeHelper.IsHideFromReflection(field))
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

		internal override void Finish()
		{
			if(BaseTypeWrapper != null)
			{
				BaseTypeWrapper.Finish();
			}
			foreach(TypeWrapper tw in this.Interfaces)
			{
				tw.Finish();
			}
		}

#if !COMPACT_FRAMEWORK
		internal override void EmitRunClassConstructor(ILGenerator ilgen)
		{
			// trigger LazyPublishMembers
			GetMethods();
			if(clinitMethod != null)
			{
				ilgen.Emit(OpCodes.Call, clinitMethod);
			}
		}
#endif

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
			else
			{
				GetterFieldWrapper getter = fw as GetterFieldWrapper;
				if(getter != null)
				{
					SignatureAttribute attr = AttributeHelper.GetSignature(getter.GetGetter());
					if(attr != null)
					{
						return attr.Signature;
					}
				}
			}
			return null;
		}

		internal override string[] GetEnclosingMethod()
		{
			object[] attr = type.GetCustomAttributes(typeof(EnclosingMethodAttribute), false);
			if(attr.Length == 1)
			{
				EnclosingMethodAttribute enc = (EnclosingMethodAttribute)attr[0];
				return new string[] { enc.ClassName, enc.MethodName, enc.MethodSignature };
			}
			return null;
		}

		internal override object[] GetDeclaredAnnotations()
		{
			if(type.Assembly.ReflectionOnly)
			{
				// TODO on Whidbey this must be implemented
				return null;
			}
			return type.GetCustomAttributes(false);
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb.DeclaringType.Assembly.ReflectionOnly)
			{
				// TODO on Whidbey this must be implemented
				return null;
			}
			return mb.GetCustomAttributes(false);
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			MethodBase mb = mw.GetMethod();
			if(mb.DeclaringType.Assembly.ReflectionOnly)
			{
				// TODO on Whidbey this must be implemented
				return null;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			int skip = 0;
			if(mb.IsStatic && !mw.IsStatic && mw.Name != "<init>")
			{
				skip = 1;
			}
			object[][] attribs = new object[parameters.Length - skip][];
			for(int i = skip; i < parameters.Length; i++)
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
				if (field.DeclaringType.Assembly.ReflectionOnly)
				{
					// TODO on Whidbey this must be implemented
					return null;
				}
				return field.GetCustomAttributes(false);
			}
			GetterFieldWrapper getter = fw as GetterFieldWrapper;
			if(getter != null)
			{
				if (getter.GetGetter().DeclaringType.Assembly.ReflectionOnly)
				{
					// TODO on Whidbey this must be implemented
					return null;
				}
				return getter.GetGetter().GetCustomAttributes(false);
			}
			return new object[0];
		}

#if !COMPACT_FRAMEWORK
		private class CompiledAnnotation : Annotation
		{
			private Type type;

			internal CompiledAnnotation(Type type)
			{
				this.type = type;
			}

			private CustomAttributeBuilder MakeCustomAttributeBuilder(object annotation)
			{
				return new CustomAttributeBuilder(type.GetConstructor(new Type[] { typeof(object[]) }), new object[] { annotation });
			}

			internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
			{
				tb.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
			{
				cb.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
			{
				mb.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
			{
				fb.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
			{
				pb.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
			}

			internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
			{
				ab.SetCustomAttribute(MakeCustomAttributeBuilder(annotation));
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

		internal override string GetSourceFileName()
		{
			object[] attr = type.GetCustomAttributes(typeof(SourceFileAttribute), false);
			if(attr.Length == 1)
			{
				return ((SourceFileAttribute)attr[0]).SourceFile;
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
	}

	sealed class DotNetTypeWrapper : TypeWrapper
	{
		private const string NamePrefix = "cli.";
		internal const string DelegateInterfaceSuffix = "$Method";
		internal const string AttributeAnnotationSuffix = "$Annotation";
		internal const string AttributeAnnotationReturnValueSuffix = "$__ReturnValue";
		internal const string AttributeAnnotationMultipleSuffix = "$__Multiple";
		internal const string EnumEnumSuffix = "$__Enum";
		private readonly Type type;
		private TypeWrapper[] innerClasses;
		private TypeWrapper outerClass;
		private TypeWrapper[] interfaces;

		private static Modifiers GetModifiers(Type type)
		{
			Modifiers modifiers = 0;
			if(type.IsPublic)
			{
				modifiers |= Modifiers.Public;
			}
			else if(type.IsNestedPublic)
			{
				modifiers |= Modifiers.Static;
				if(IsVisible(type))
				{
					modifiers |= Modifiers.Public;
				}
			}
			else if(type.IsNestedPrivate)
			{
				modifiers |= Modifiers.Private | Modifiers.Static;
			}
			else if(type.IsNestedFamily || type.IsNestedFamORAssem)
			{
				modifiers |= Modifiers.Protected | Modifiers.Static;
			}
			else if(type.IsNestedAssembly || type.IsNestedFamANDAssem)
			{
				modifiers |= Modifiers.Static;
			}

			if(type.IsSealed)
			{
				modifiers |= Modifiers.Final;
			}
			else if(type.IsAbstract) // we can't be abstract if we're final
			{
				modifiers |= Modifiers.Abstract;
			}
			if(type.IsInterface)
			{
				modifiers |= Modifiers.Interface;
			}
			return modifiers;
		}

		// NOTE when this is called on a remapped type, the "warped" underlying type name is returned.
		// E.g. GetName(typeof(object)) returns "cli.System.Object".
		internal static string GetName(Type type)
		{
			Debug.Assert(!type.Name.EndsWith("[]") && !AttributeHelper.IsJavaModule(type.Module));

			string name = type.FullName;

			if(name == null)
			{
				// generic type parameters don't have a full name
				return null;
			}

			if(type.ContainsGenericParameters)
			{
				// open generic types are not visible
				return null;
			}
			if(type.IsGenericType)
			{
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				sb.Append(MangleTypeName(type.GetGenericTypeDefinition().FullName));
				sb.Append("_$$$_");
				string sep = "";
				foreach(Type t1 in type.GetGenericArguments())
				{
					Type t = t1;
					sb.Append(sep);
					// NOTE we can't use ClassLoaderWrapper.GetWrapperFromType() here to get t's name,
					// because we might be resolving a generic type that refers to a type that is in
					// the process of being constructed.
					//
					// For example:
					//   class Base<T> { } 
					//   class Derived : Base<Derived> { }
					//
					while(ClassLoaderWrapper.IsVector(t))
					{
						t = t.GetElementType();
						sb.Append('A');
					}
					if(PrimitiveTypeWrapper.IsPrimitiveType(t))
					{
						sb.Append(ClassLoaderWrapper.GetWrapperFromType(t).SigName);
					}
					else
					{
						string s;
						if(ClassLoaderWrapper.IsRemappedType(t))
						{
							s = ClassLoaderWrapper.GetWrapperFromType(t).Name;
						}
						else if(ClassLoaderWrapper.IsDynamicType(t) || AttributeHelper.IsJavaModule(t.Module))
						{
							s = CompiledTypeWrapper.GetName(t);
						}
						else
						{
							s = DotNetTypeWrapper.GetName(t);
						}
						// only do the mangling for non-generic types (because we don't want to convert
						// the double underscores in two adjacent _$$$_ or _$$$$_ markers)
						if (s.IndexOf("_$$$_") == -1)
						{
							s = s.Replace("__", "$$005F$$005F");
							s = s.Replace(".", "__");
						}
						sb.Append('L').Append(s);
					}
					sep = "_$$_";
				}
				sb.Append("_$$$$_");
				return sb.ToString();
			}

			if(AttributeHelper.IsNoPackagePrefix(type)
				&& name.IndexOf('$') == -1)
			{
				return name.Replace('+', '$');
			}

			return MangleTypeName(name);
		}

		private static string MangleTypeName(string name)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder(NamePrefix, NamePrefix.Length + name.Length);
			bool escape = false;
			for(int i = 0; i < name.Length; i++)
			{
				char c = name[i];
				if(c == '+' && !escape && (sb.Length == 0 || sb[sb.Length - 1] != '$'))
				{
					sb.Append('$');
				}
				else if("_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c) != -1
					|| (c == '.' && !escape))
				{
					sb.Append(c);
				}
				else
				{
					sb.Append("$$");
					sb.Append(string.Format("{0:X4}", (int)c));
				}
				if(c == '\\')
				{
					escape = !escape;
				}
				else
				{
					escape = false;
				}
			}
			return sb.ToString();
		}

		// NOTE if the name is not a valid mangled type name, no demangling is done and the
		// original string is returned
		// NOTE we don't enforce canonical form, this is not required, because we cannot
		// guarantee it for unprefixed names anyway, so the caller is responsible for
		// ensuring that the original name was in fact the canonical name.
		internal static string DemangleTypeName(string name)
		{
			if(!name.StartsWith(NamePrefix))
			{
				return name.Replace('$', '+');
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder(name.Length - NamePrefix.Length);
			for(int i = NamePrefix.Length; i < name.Length; i++)
			{
				char c = name[i];
				if(c == '$')
				{
					if(i + 1 < name.Length && name[i + 1] != '$')
					{
						sb.Append('+');
					}
					else
					{
						i++;
						if(i + 5 > name.Length)
						{
							return name;
						}
						int digit0 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit1 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit2 = "0123456789ABCDEF".IndexOf(name[++i]);
						int digit3 = "0123456789ABCDEF".IndexOf(name[++i]);
						if(digit0 == -1 || digit1 == -1 || digit2 == -1 || digit3 == -1)
						{
							return name;
						}
						sb.Append((char)((digit0 << 12) + (digit1 << 8) + (digit2 << 4) + digit3));
					}
				}
				else
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}

		// TODO from a perf pov it may be better to allow creation of TypeWrappers,
		// but to simply make sure they don't have ClassObject
		internal static bool IsAllowedOutside(Type type)
		{
			// SECURITY we never expose types from IKVM.Runtime, because doing so would lead to a security hole,
			// since the reflection implementation lives inside this assembly, all internal members would
			// be accessible through Java reflection.
			if(type.Assembly == typeof(DotNetTypeWrapper).Assembly)
			{
				return false;
			}
			if(type.ContainsGenericParameters)
			{
				return false;
			}
			return true;
		}

		private class DelegateInnerClassTypeWrapper : TypeWrapper
		{
			private Type delegateType;

			internal DelegateInnerClassTypeWrapper(string name, Type delegateType, ClassLoaderWrapper classLoader)
				: base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract, name, null)
			{
				this.delegateType = delegateType;
				MethodInfo invoke = delegateType.GetMethod("Invoke");
				ParameterInfo[] parameters = invoke.GetParameters();
				TypeWrapper[] argTypeWrappers = new TypeWrapper[parameters.Length];
				System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
				for(int i = 0; i < parameters.Length; i++)
				{
					argTypeWrappers[i] = ClassLoaderWrapper.GetWrapperFromType(parameters[i].ParameterType);
					sb.Append(argTypeWrappers[i].SigName);
				}
				TypeWrapper returnType = ClassLoaderWrapper.GetWrapperFromType(invoke.ReturnType);
				sb.Append(")").Append(returnType.SigName);
				MethodWrapper invokeMethod = new DynamicOnlyMethodWrapper(this, "Invoke", sb.ToString(), returnType, argTypeWrappers);
				SetMethods(new MethodWrapper[] { invokeMethod });
				SetFields(FieldWrapper.EmptyArray);
			}

			internal override bool IsDynamicOnly
			{
				get
				{
					return true;
				}
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(delegateType);
				}
			}

			internal override void Finish()
			{
			}

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal override string[] GetEnclosingMethod()
			{
				return null;
			}

			internal override string GetGenericFieldSignature(FieldWrapper fw)
			{
				return null;
			}

			internal override string GetGenericMethodSignature(MethodWrapper mw)
			{
				return null;
			}

			internal override string GetGenericSignature()
			{
				return null;
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override TypeWrapper[] Interfaces
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override Type TypeAsTBD
			{
				get
				{
					return typeof(object);
				}
			}

			internal override Type TypeAsBaseType
			{
				get
				{
					throw new InvalidOperationException();
				}
			}
		}

		private class DynamicOnlyMethodWrapper : MethodWrapper
		{
			internal DynamicOnlyMethodWrapper(TypeWrapper declaringType, string name, string sig, TypeWrapper returnType, TypeWrapper[] parameterTypes)
				: base(declaringType, name, sig, null, returnType, parameterTypes, Modifiers.Public | Modifiers.Abstract, MemberFlags.None)
			{
			}

#if !STATIC_COMPILER
				internal override object Invoke(object obj, object[] args, bool nonVirtual)
				{
					return TypeWrapper.FromClass(NativeCode.ikvm.runtime.Util.getClassFromObject(obj))
						.GetMethodWrapper(this.Name, this.Signature, true)
						.Invoke(obj, args, false);
				}
#endif // !STATIC_COMPILER
		}

		private class EnumEnumTypeWrapper : TypeWrapper
		{
			private Type enumType;

			internal EnumEnumTypeWrapper(string name, Type enumType)
				: base(Modifiers.Public | Modifiers.Enum | Modifiers.Final, name, ClassLoaderWrapper.LoadClassCritical("java.lang.Enum"))
			{
				this.enumType = enumType;
			}

			private class EnumFieldWrapper : FieldWrapper
			{
				private readonly int ordinal;
				private object val;
				private static ConstructorInfo enumEnumConstructor;
				private static FieldInfo enumEnumTypeField;

				internal EnumFieldWrapper(TypeWrapper tw, string name, int ordinal)
					: base(tw, tw, name, tw.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final | Modifiers.Enum, null, MemberFlags.None)
				{
					this.ordinal = ordinal;
				}

				internal override object GetValue(object unused)
				{
					lock(this)
					{
						if(val == null)
						{
							if(enumEnumConstructor == null)
							{
								enumEnumConstructor = JVM.CoreAssembly.GetType("ikvm.internal.EnumEnum").GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(string), typeof(int) }, null);
							}
							object obj = enumEnumConstructor.Invoke(new object[] { this.Name, ordinal });
							if(enumEnumTypeField == null)
							{
								enumEnumTypeField = enumEnumConstructor.DeclaringType.GetField("typeWrapper", BindingFlags.NonPublic | BindingFlags.Instance);
							}
							enumEnumTypeField.SetValue(obj, this.DeclaringType);
							val = obj;
						}
					}
					return val;
				}

				protected override void EmitGetImpl(ILGenerator ilgen)
				{
					ilgen.Emit(OpCodes.Ldstr, this.DeclaringType.Name);
					CoreClasses.java.lang.Class.Wrapper.GetMethodWrapper("forName", "(Ljava.lang.String;)Ljava.lang.Class;", false).EmitCall(ilgen);
					ilgen.Emit(OpCodes.Ldstr, this.Name);
					this.DeclaringType.BaseTypeWrapper.GetMethodWrapper("valueOf", "(Ljava.lang.Class;Ljava.lang.String;)Ljava.lang.Enum;", false).EmitCall(ilgen);
				}

				protected override void EmitSetImpl(ILGenerator ilgen)
				{
				}
			}

			private class EnumValuesMethodWrapper : MethodWrapper
			{
				internal EnumValuesMethodWrapper(TypeWrapper declaringType)
					: base(declaringType, "values", "()[" + declaringType.SigName, null, declaringType.MakeArrayType(1), TypeWrapper.EmptyArray, Modifiers.Public | Modifiers.Static, MemberFlags.None)
				{
				}

#if !STATIC_COMPILER
				internal override object Invoke(object obj, object[] args, bool nonVirtual)
				{
					FieldWrapper[] values = this.DeclaringType.GetFields();
					object[] array = (object[])Array.CreateInstance(this.DeclaringType.TypeAsArrayType, values.Length);
					for(int i = 0; i < values.Length; i++)
					{
						array[i] = values[i].GetValue(null);
					}
					return array;
				}
#endif // !STATIC_COMPILER
			}

			private class EnumValueOfMethodWrapper : MethodWrapper
			{
				internal EnumValueOfMethodWrapper(TypeWrapper declaringType)
					: base(declaringType, "valueOf", "(Ljava.lang.String;)" + declaringType.SigName, null, declaringType, new TypeWrapper[] { CoreClasses.java.lang.String.Wrapper }, Modifiers.Public | Modifiers.Static, MemberFlags.None)
				{
				}

#if !STATIC_COMPILER && !FIRST_PASS
				internal override object Invoke(object obj, object[] args, bool nonVirtual)
				{
					FieldWrapper[] values = this.DeclaringType.GetFields();
					for(int i = 0; i < values.Length; i++)
					{
						if(values[i].Name.Equals(args[0]))
						{
							return values[i].GetValue(null);
						}
					}
					throw new java.lang.IllegalArgumentException("" + args[0]);
				}
#endif // !STATIC_COMPILER && !FIRST_PASS
			}

			protected override void LazyPublishMembers()
			{
				ArrayList fields = new ArrayList();
				int ordinal = 0;
				foreach(FieldInfo field in enumType.GetFields(BindingFlags.Static | BindingFlags.Public))
				{
					if(field.IsLiteral)
					{
						fields.Add(new EnumFieldWrapper(this, field.Name, ordinal++));
					}
				}
				// TODO if the enum already has an __unspecified value, rename this one
				fields.Add(new EnumFieldWrapper(this, "__unspecified", ordinal++));
				SetFields((FieldWrapper[])fields.ToArray(typeof(FieldWrapper)));
				SetMethods(new MethodWrapper[] { new EnumValuesMethodWrapper(this), new EnumValueOfMethodWrapper(this) });
				base.LazyPublishMembers();
			}

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(enumType);
				}
			}

			internal override void Finish()
			{
			}

			internal override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal override string[] GetEnclosingMethod()
			{
				return null;
			}

			internal override string GetGenericFieldSignature(FieldWrapper fw)
			{
				return null;
			}

			internal override string GetGenericMethodSignature(MethodWrapper mw)
			{
				return null;
			}

			internal override string GetGenericSignature()
			{
				return null;
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override TypeWrapper[] Interfaces
			{
				get
				{
					return TypeWrapper.EmptyArray;
				}
			}

			internal override bool IsDynamicOnly
			{
				get
				{
					return true;
				}
			}

			internal override Type TypeAsTBD
			{
				get
				{
					// return java.lang.Enum instead
					return BaseTypeWrapper.TypeAsTBD;
				}
			}

			internal override Type TypeAsBaseType
			{
				get
				{
					throw new InvalidOperationException();
				}
			}
		}

		private abstract class AttributeAnnotationTypeWrapperBase : TypeWrapper
		{
			internal AttributeAnnotationTypeWrapperBase(string name)
				: base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract | Modifiers.Annotation, name, null)
			{
			}

			internal sealed override void Finish()
			{
			}

			internal sealed override ClassLoaderWrapper GetClassLoader()
			{
				return DeclaringTypeWrapper.GetClassLoader();
			}

			internal sealed override string[] GetEnclosingMethod()
			{
				return null;
			}

			internal sealed override string GetGenericFieldSignature(FieldWrapper fw)
			{
				return null;
			}

			internal sealed override string GetGenericMethodSignature(MethodWrapper mw)
			{
				return null;
			}

			internal sealed override string GetGenericSignature()
			{
				return null;
			}

			internal sealed override TypeWrapper[] Interfaces
			{
				get
				{
					return new TypeWrapper[] { ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.annotation.Annotation") };
				}
			}

			internal sealed override bool IsDynamicOnly
			{
				get
				{
					return true;
				}
			}

			internal sealed override Type TypeAsTBD
			{
				get
				{
					return typeof(object);
				}
			}

			internal sealed override Type TypeAsBaseType
			{
				get
				{
					throw new InvalidOperationException();
				}
			}
		}

		private sealed class AttributeAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
		{
			private Type attributeType;
			private TypeWrapper[] innerClasses;

			internal AttributeAnnotationTypeWrapper(string name, Type attributeType)
				: base(name)
			{
				this.attributeType = attributeType;
			}

			private static bool IsSupportedType(Type type)
			{
				// Java annotations only support one-dimensional arrays
				if(type.IsArray)
				{
					type = type.GetElementType();
				}
				return type == typeof(string)
					|| type == typeof(bool)
					|| type == typeof(byte)
					|| type == typeof(char)
					|| type == typeof(short)
					|| type == typeof(int)
					|| type == typeof(float)
					|| type == typeof(long)
					|| type == typeof(double)
					|| type == typeof(Type)
					|| type.IsEnum;
			}

			internal static void GetConstructors(Type type, out ConstructorInfo defCtor, out ConstructorInfo singleOneArgCtor)
			{
				defCtor = null;
				int oneArgCtorCount = 0;
				ConstructorInfo oneArgCtor = null;
				foreach(ConstructorInfo ci in type.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
				{
					ParameterInfo[] args = ci.GetParameters();
					if(args.Length == 0)
					{
						defCtor = ci;
					}
					else if(args.Length == 1)
					{
						// HACK special case for p/invoke StructLayout attribute
						if(type == typeof(System.Runtime.InteropServices.StructLayoutAttribute) && args[0].ParameterType == typeof(short))
						{
							// we skip this constructor, so that the other one will be visible
							continue;
						}
						if(IsSupportedType(args[0].ParameterType))
						{
							oneArgCtor = ci;
							oneArgCtorCount++;
						}
						else
						{
							// set to two to make sure we don't see the oneArgCtor as viable
							oneArgCtorCount = 2;
						}
					}
				}
				singleOneArgCtor = oneArgCtorCount == 1 ? oneArgCtor : null;
			}

			private class AttributeAnnotationMethodWrapper : DynamicOnlyMethodWrapper
			{
				private bool optional;

				internal AttributeAnnotationMethodWrapper(AttributeAnnotationTypeWrapper tw, string name, Type type, bool optional)
					: this(tw, name, MapType(type, false), optional)
				{
				}

				private static TypeWrapper MapType(Type type, bool isArray)
				{
					if(type == typeof(string))
					{
						return CoreClasses.java.lang.String.Wrapper;
					}
					else if(type == typeof(bool))
					{
						return PrimitiveTypeWrapper.BOOLEAN;
					}
					else if(type == typeof(byte))
					{
						return PrimitiveTypeWrapper.BYTE;
					}
					else if(type == typeof(char))
					{
						return PrimitiveTypeWrapper.CHAR;
					}
					else if(type == typeof(short))
					{
						return PrimitiveTypeWrapper.SHORT;
					}
					else if(type == typeof(int))
					{
						return PrimitiveTypeWrapper.INT;
					}
					else if(type == typeof(float))
					{
						return PrimitiveTypeWrapper.FLOAT;
					}
					else if(type == typeof(long))
					{
						return PrimitiveTypeWrapper.LONG;
					}
					else if(type == typeof(double))
					{
						return PrimitiveTypeWrapper.DOUBLE;
					}
					else if(type == typeof(Type))
					{
						return CoreClasses.java.lang.Class.Wrapper;
					}
					else if (type.IsEnum)
					{
						foreach (TypeWrapper tw in ClassLoaderWrapper.GetWrapperFromType(type).InnerClasses)
						{
							if (tw is EnumEnumTypeWrapper)
							{
								if (!isArray && AttributeHelper.IsDefined(type, typeof(FlagsAttribute)))
								{
									return tw.MakeArrayType(1);
								}
								return tw;
							}
						}
						throw new InvalidOperationException();
					}
					else if(!isArray && type.IsArray)
					{
						return MapType(type.GetElementType(), true).MakeArrayType(1);
					}
					else
					{
						throw new NotImplementedException();
					}
				}

				private AttributeAnnotationMethodWrapper(AttributeAnnotationTypeWrapper tw, string name, TypeWrapper returnType, bool optional)
					: base(tw, name, "()" + returnType.SigName, returnType, TypeWrapper.EmptyArray)
				{
					this.optional = optional;
				}

				internal bool IsOptional
				{
					get
					{
						return optional;
					}
				}
			}

			protected override void LazyPublishMembers()
			{
				ArrayList methods = new ArrayList();
				ConstructorInfo defCtor;
				ConstructorInfo singleOneArgCtor;
				GetConstructors(attributeType, out defCtor, out singleOneArgCtor);
				if(singleOneArgCtor != null)
				{
					methods.Add(new AttributeAnnotationMethodWrapper(this, "value", singleOneArgCtor.GetParameters()[0].ParameterType, defCtor != null));
				}
				foreach(PropertyInfo pi in attributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
				{
					if(pi.CanRead && pi.CanWrite && IsSupportedType(pi.PropertyType))
					{
						methods.Add(new AttributeAnnotationMethodWrapper(this, pi.Name, pi.PropertyType, true));
					}
				}
				foreach(FieldInfo fi in attributeType.GetFields(BindingFlags.Public | BindingFlags.Instance))
				{
					// TODO add other field validations to make sure it is appropriate
					if(!fi.IsInitOnly && IsSupportedType(fi.FieldType))
					{
						methods.Add(new AttributeAnnotationMethodWrapper(this, fi.Name, fi.FieldType, true));
					}
				}
				SetMethods((MethodWrapper[])methods.ToArray(typeof(MethodWrapper)));
				base.LazyPublishMembers();
			}

#if !STATIC_COMPILER && !FIRST_PASS
			internal override object GetAnnotationDefault(MethodWrapper mw)
			{
				if(((AttributeAnnotationMethodWrapper)mw).IsOptional)
				{
					if (mw.ReturnType == PrimitiveTypeWrapper.BOOLEAN)
					{
						return java.lang.Boolean.FALSE;
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.BYTE)
					{
						return java.lang.Byte.valueOf((byte)0);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.CHAR)
					{
						return java.lang.Character.valueOf((char)0);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.SHORT)
					{
						return java.lang.Short.valueOf((short)0);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.INT)
					{
						return java.lang.Integer.valueOf(0);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.FLOAT)
					{
						return java.lang.Float.valueOf(0F);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.LONG)
					{
						return java.lang.Long.valueOf(0L);
					}
					else if(mw.ReturnType == PrimitiveTypeWrapper.DOUBLE)
					{
						return java.lang.Double.valueOf(0D);
					}
					else if(mw.ReturnType == CoreClasses.java.lang.String.Wrapper)
					{
						return "";
					}
					else if(mw.ReturnType == CoreClasses.java.lang.Class.Wrapper)
					{
						return (java.lang.Class)typeof(ikvm.@internal.__unspecified);
					}
					else if(mw.ReturnType is EnumEnumTypeWrapper)
					{
						return mw.ReturnType.GetFieldWrapper("__unspecified", mw.ReturnType.SigName).GetValue(null);
					}
					else if(mw.ReturnType.IsArray)
					{
						return Array.CreateInstance(mw.ReturnType.TypeAsArrayType, 0);
					}
				}
				return null;
			}
#endif // !STATIC_COMPILER && !FIRST_PASS

			internal override TypeWrapper DeclaringTypeWrapper
			{
				get
				{
					return ClassLoaderWrapper.GetWrapperFromType(attributeType);
				}
			}

			private sealed class ReturnValueAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
			{
				private AttributeAnnotationTypeWrapper declaringType;

				internal ReturnValueAnnotationTypeWrapper(AttributeAnnotationTypeWrapper declaringType)
					: base(declaringType.Name + AttributeAnnotationReturnValueSuffix)
				{
					this.declaringType = declaringType;
				}

				protected override void LazyPublishMembers()
				{
					TypeWrapper tw = declaringType;
					if(declaringType.GetAttributeUsage().AllowMultiple)
					{
						tw = tw.MakeArrayType(1);
					}
					SetMethods(new MethodWrapper[] { new DynamicOnlyMethodWrapper(this, "value", "()" + tw.SigName, tw, TypeWrapper.EmptyArray) });
					SetFields(FieldWrapper.EmptyArray);
				}

				internal override TypeWrapper DeclaringTypeWrapper
				{
					get
					{
						return declaringType;
					}
				}

				internal override TypeWrapper[] InnerClasses
				{
					get
					{
						return TypeWrapper.EmptyArray;
					}
				}

#if !STATIC_COMPILER
				internal override object[] GetDeclaredAnnotations()
				{
					return new object[] {
										JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), new object[] { AnnotationDefaultAttribute.TAG_ANNOTATION, "java.lang.annotation.Target", "value", 
											new object[] { AnnotationDefaultAttribute.TAG_ARRAY, new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "METHOD" } }
										}),
										JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), new object[] { AnnotationDefaultAttribute.TAG_ANNOTATION, "java.lang.annotation.Retention", "value", new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/RetentionPolicy;", "RUNTIME" } })
									};
				}
#endif

#if !COMPACT_FRAMEWORK
				private class ReturnValueAnnotation : Annotation
				{
					private AttributeAnnotationTypeWrapper type;

					internal ReturnValueAnnotation(AttributeAnnotationTypeWrapper type)
					{
						this.type = type;
					}

					internal override void ApplyReturnValue(ClassLoaderWrapper loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
					{
						// TODO make sure the descriptor is correct
						Annotation ann = type.Annotation;
						object[] arr = (object[])annotation;
						for(int i = 2; i < arr.Length; i += 2)
						{
							if("value".Equals(arr[i]))
							{
								if(pb == null)
								{
									pb = mb.DefineParameter(0, ParameterAttributes.None, null);
								}
								object[] value = (object[])arr[i + 1];
								if(value[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
								{
									ann.Apply(loader, pb, value);
								}
								else
								{
									for(int j = 1; j < value.Length; j++)
									{
										ann.Apply(loader, pb, value[j]);
									}
								}
								break;
							}
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
					{
					}

					internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
					{
					}
				}

				internal override Annotation Annotation
				{
					get
					{
						return new ReturnValueAnnotation(declaringType);
					}
				}
#endif
			}

			private sealed class MultipleAnnotationTypeWrapper : AttributeAnnotationTypeWrapperBase
			{
				private AttributeAnnotationTypeWrapper declaringType;

				internal MultipleAnnotationTypeWrapper(AttributeAnnotationTypeWrapper declaringType)
					: base(declaringType.Name + AttributeAnnotationMultipleSuffix)
				{
					this.declaringType = declaringType;
				}

				protected override void LazyPublishMembers()
				{
					TypeWrapper tw = declaringType.MakeArrayType(1);
					SetMethods(new MethodWrapper[] { new DynamicOnlyMethodWrapper(this, "value", "()" + tw.SigName, tw, TypeWrapper.EmptyArray) });
					SetFields(FieldWrapper.EmptyArray);
				}

				internal override TypeWrapper DeclaringTypeWrapper
				{
					get
					{
						return declaringType;
					}
				}

				internal override TypeWrapper[] InnerClasses
				{
					get
					{
						return TypeWrapper.EmptyArray;
					}
				}

#if !STATIC_COMPILER
				internal override object[] GetDeclaredAnnotations()
				{
					return declaringType.GetDeclaredAnnotations();
				}
#endif

#if !COMPACT_FRAMEWORK
				private class MultipleAnnotation : Annotation
				{
					private AttributeAnnotationTypeWrapper type;

					internal MultipleAnnotation(AttributeAnnotationTypeWrapper type)
					{
						this.type = type;
					}

					private static object[] UnwrapArray(object annotation)
					{
						// TODO make sure the descriptor is correct
						object[] arr = (object[])annotation;
						for (int i = 2; i < arr.Length; i += 2)
						{
							if ("value".Equals(arr[i]))
							{
								object[] value = (object[])arr[i + 1];
								object[] rc = new object[value.Length - 1];
								Array.Copy(value, 1, rc, 0, rc.Length);
								return rc;
							}
						}
						return new object[0];
					}

					internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach(object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, mb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, ab, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, cb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, fb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, pb, ann);
						}
					}

					internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
					{
						Annotation annot = type.Annotation;
						foreach (object ann in UnwrapArray(annotation))
						{
							annot.Apply(loader, tb, ann);
						}
					}
				}

				internal override Annotation Annotation
				{
					get
					{
						return new MultipleAnnotation(declaringType);
					}
				}
#endif
			}

			internal override TypeWrapper[] InnerClasses
			{
				get
				{
					lock(this)
					{
						if(innerClasses == null)
						{
							ArrayList list = new ArrayList();
							AttributeUsageAttribute attr = GetAttributeUsage();
							if((attr.ValidOn & AttributeTargets.ReturnValue) != 0)
							{
								list.Add(GetClassLoader().RegisterInitiatingLoader(new ReturnValueAnnotationTypeWrapper(this)));
							}
							if(attr.AllowMultiple)
							{
								list.Add(GetClassLoader().RegisterInitiatingLoader(new MultipleAnnotationTypeWrapper(this)));
							}
							innerClasses = (TypeWrapper[])list.ToArray(typeof(TypeWrapper));
						}
					}
					return innerClasses;
				}
			}

			private AttributeUsageAttribute GetAttributeUsage()
			{
				AttributeTargets validOn = AttributeTargets.All;
				bool allowMultiple = false;
				bool inherited = true;
				foreach(CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(attributeType))
				{
					if(cad.Constructor.DeclaringType == typeof(AttributeUsageAttribute))
					{
						if(cad.ConstructorArguments.Count == 1 && cad.ConstructorArguments[0].ArgumentType == typeof(AttributeTargets))
						{
							validOn = (AttributeTargets)cad.ConstructorArguments[0].Value;
						}
						foreach(CustomAttributeNamedArgument cana in cad.NamedArguments)
						{
							if (cana.MemberInfo.Name == "AllowMultiple")
							{
								allowMultiple = (bool)cana.TypedValue.Value;
							}
							else if(cana.MemberInfo.Name == "Inherited")
							{
								inherited = (bool)cana.TypedValue.Value;
							}
						}
					}
				}
				AttributeUsageAttribute attr = new AttributeUsageAttribute(validOn);
				attr.AllowMultiple = allowMultiple;
				attr.Inherited = inherited;
				return attr;
			}

#if !STATIC_COMPILER
			internal override object[] GetDeclaredAnnotations()
			{
				// note that AttributeUsageAttribute.Inherited does not map to java.lang.annotation.Inherited
				AttributeTargets validOn = GetAttributeUsage().ValidOn;
				ArrayList targets = new ArrayList();
				targets.Add(AnnotationDefaultAttribute.TAG_ARRAY);
				if((validOn & (AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "TYPE" });
				}
				if((validOn & AttributeTargets.Constructor) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "CONSTRUCTOR" });
				}
				if((validOn & AttributeTargets.Field) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "FIELD" });
				}
				if((validOn & AttributeTargets.Method) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "METHOD" });
				}
				if((validOn & AttributeTargets.Parameter) != 0)
				{
					targets.Add(new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/ElementType;", "PARAMETER" });
				}
				return new object[] {
										JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), new object[] { AnnotationDefaultAttribute.TAG_ANNOTATION, "java.lang.annotation.Target", "value", (object[])targets.ToArray() }),
										JVM.Library.newAnnotation(GetClassLoader().GetJavaClassLoader(), new object[] { AnnotationDefaultAttribute.TAG_ANNOTATION, "java.lang.annotation.Retention", "value", new object[] { AnnotationDefaultAttribute.TAG_ENUM, "Ljava/lang/annotation/RetentionPolicy;", "RUNTIME" } })
									};
			}
#endif

#if !COMPACT_FRAMEWORK
			private class AttributeAnnotation : Annotation
			{
				private Type type;

				internal AttributeAnnotation(Type type)
				{
					this.type = type;
				}

				private static object ConvertValue(ClassLoaderWrapper loader, Type targetType, object obj)
				{
					if(targetType.IsEnum)
					{
						// TODO check the obj descriptor matches the type we expect
						if(((object[])obj)[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
						{
							object[] arr = (object[])obj;
							string s = "";
							string sep = "";
							for(int i = 1; i < arr.Length; i++)
							{
								// TODO check the obj descriptor matches the type we expect
								string val = ((object[])arr[i])[2].ToString();
								if(val != "__unspecified")
								{
									s += sep + val;
									sep = ", ";
								}
							}
							if(s == "")
							{
								return Activator.CreateInstance(targetType);
							}
							return Enum.Parse(targetType, s);
						}
						else
						{
							string s = ((object[])obj)[2].ToString();
							if(s == "__unspecified")
							{
								// TODO instead of this, we should probably return null and handle that
								return Activator.CreateInstance(targetType);
							}
							return Enum.Parse(targetType, s);
						}
					}
					else if(targetType == typeof(Type))
					{
						// TODO check the obj descriptor matches the type we expect
						return loader.FieldTypeWrapperFromSig(((string)((object[])obj)[1]).Replace('/', '.')).TypeAsTBD;
					}
					else if(targetType.IsArray)
					{
						// TODO check the obj descriptor matches the type we expect
						object[] arr = (object[])obj;
						Type elementType = targetType.GetElementType();
						Array targetArray = Array.CreateInstance(elementType, arr.Length - 1);
						for(int i = 1; i < arr.Length; i++)
						{
							targetArray.SetValue(ConvertValue(loader, elementType, arr[i]), i - 1);
						}
						return targetArray;
					}
					else
					{
						return obj;
					}
				}

				private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
				{
					object[] arr = (object[])annotation;
					ConstructorInfo defCtor;
					ConstructorInfo singleOneArgCtor;
					object ctorArg = null;
					GetConstructors(type, out defCtor, out singleOneArgCtor);
					ArrayList properties = new ArrayList();
					ArrayList propertyValues = new ArrayList();
					ArrayList fields = new ArrayList();
					ArrayList fieldValues = new ArrayList();
					for(int i = 2; i < arr.Length; i += 2)
					{
						string name = (string)arr[i];
						if(name == "value" && singleOneArgCtor != null)
						{
							ctorArg = ConvertValue(loader, singleOneArgCtor.GetParameters()[0].ParameterType, arr[i + 1]);
						}
						else
						{
							PropertyInfo pi = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
							if(pi != null)
							{
								properties.Add(pi);
								propertyValues.Add(ConvertValue(loader, pi.PropertyType, arr[i + 1]));
							}
							else
							{
								FieldInfo fi = type.GetField(name, BindingFlags.Public | BindingFlags.Instance);
								if(fi != null)
								{
									fields.Add(fi);
									fieldValues.Add(ConvertValue(loader, fi.FieldType, arr[i + 1]));
								}
							}
						}
					}
					if(ctorArg == null && defCtor == null)
					{
						// TODO required argument is missing
					}
					return new CustomAttributeBuilder(ctorArg == null ? defCtor : singleOneArgCtor,
						ctorArg == null ? new object[0] : new object[] { ctorArg },
						(PropertyInfo[])properties.ToArray(typeof(PropertyInfo)),
						(object[])propertyValues.ToArray(),
						(FieldInfo[])fields.ToArray(typeof(FieldInfo)),
						(object[])fieldValues.ToArray());
				}

				internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
				{
					if(type == typeof(System.Runtime.InteropServices.StructLayoutAttribute) && tb.BaseType != typeof(object))
					{
						// we have to handle this explicitly, because if we apply an illegal StructLayoutAttribute,
						// TypeBuilder.CreateType() will later on throw an exception.
						Tracer.Error(Tracer.Runtime, "StructLayoutAttribute cannot be applied to {0}, because it does not directly extend cli.System.Object", tb.FullName);
						return;
					}
					tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
				}

				internal override void Apply(ClassLoaderWrapper loader, ConstructorBuilder cb, object annotation)
				{
					cb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
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
			}

			internal override Annotation Annotation
			{
				get
				{
					return new AttributeAnnotation(attributeType);
				}
			}
#endif //!COMPACT_FRAMEWORK
		}

		internal static TypeWrapper GetWrapperFromDotNetType(Type type)
		{
			return ClassLoaderWrapper.GetAssemblyClassLoader(type.Assembly).GetWrapperFromAssemblyType(type);
		}

		private static TypeWrapper GetBaseTypeWrapper(Type type)
		{
			if(type.IsInterface)
			{
				return null;
			}
			else if(ClassLoaderWrapper.IsRemappedType(type))
			{
				// Remapped types extend their alter ego
				// (e.g. cli.System.Object must appear to be derived from java.lang.Object)
				// except when they're sealed, of course.
				if(type.IsSealed)
				{
					return CoreClasses.java.lang.Object.Wrapper;
				}
				return ClassLoaderWrapper.GetWrapperFromType(type);
			}
			else if(ClassLoaderWrapper.IsRemappedType(type.BaseType))
			{
				return GetWrapperFromDotNetType(type.BaseType);
			}
			else
			{
				return ClassLoaderWrapper.GetWrapperFromType(type.BaseType);
			}
		}

		internal DotNetTypeWrapper(Type type)
			: base(GetModifiers(type), GetName(type), GetBaseTypeWrapper(type))
		{
			Debug.Assert(!(type.IsByRef), type.FullName);
			Debug.Assert(!(type.IsPointer), type.FullName);
			Debug.Assert(!(type.Name.EndsWith("[]")), type.FullName);
			Debug.Assert(!(type is TypeBuilder), type.FullName);
			Debug.Assert(!(AttributeHelper.IsJavaModule(type.Module)));

			this.type = type;
		}

		internal override ClassLoaderWrapper GetClassLoader()
		{
			if(type.IsGenericType)
			{
				return ClassLoaderWrapper.GetGenericClassLoader(this);
			}
			return ClassLoaderWrapper.GetAssemblyClassLoader(type.Assembly);
		}

		private class DelegateMethodWrapper : MethodWrapper
		{
			private ConstructorInfo delegateConstructor;
			private DelegateInnerClassTypeWrapper iface;

			internal DelegateMethodWrapper(TypeWrapper declaringType, DelegateInnerClassTypeWrapper iface)
				: base(declaringType, "<init>", "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, Modifiers.Public, MemberFlags.None)
			{
				this.delegateConstructor = declaringType.TypeAsTBD.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
				this.iface = iface;
			}

#if !COMPACT_FRAMEWORK
			internal override void EmitNewobj(ILGenerator ilgen, MethodAnalyzer ma, int opcodeIndex)
			{
				TypeWrapper targetType = ma == null ? null : ma.GetStackTypeWrapper(opcodeIndex, 0);
				if(targetType == null || targetType.IsInterface)
				{
					MethodInfo createDelegate = typeof(Delegate).GetMethod("CreateDelegate", new Type[] { typeof(Type), typeof(object), typeof(string) });
					LocalBuilder targetObj = ilgen.DeclareLocal(typeof(object));
					ilgen.Emit(OpCodes.Stloc, targetObj);
					ilgen.Emit(OpCodes.Ldtoken, delegateConstructor.DeclaringType);
					ilgen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
					ilgen.Emit(OpCodes.Ldloc, targetObj);
					ilgen.Emit(OpCodes.Ldstr, "Invoke");
					ilgen.Emit(OpCodes.Call, createDelegate);
					ilgen.Emit(OpCodes.Castclass, delegateConstructor.DeclaringType);
				}
				else
				{
					ilgen.Emit(OpCodes.Dup);
					// we know that a DelegateInnerClassTypeWrapper has only one method
					Debug.Assert(iface.GetMethods().Length == 1);
					MethodWrapper mw = targetType.GetMethodWrapper("Invoke", iface.GetMethods()[0].Signature, true);
					// TODO linking here is not safe
					mw.Link();
					ilgen.Emit(OpCodes.Ldvirtftn, (MethodInfo)mw.GetMethod());
					ilgen.Emit(OpCodes.Newobj, delegateConstructor);
				}
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				// TODO map exceptions
				return Delegate.CreateDelegate(DeclaringType.TypeAsTBD, args[0], "Invoke");
			}
#endif // !STATIC_COMPILER
		}

		private class ByRefMethodWrapper : SmartMethodWrapper
		{
			private bool[] byrefs;
			private Type[] args;

			internal ByRefMethodWrapper(Type[] args, bool[] byrefs, TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection)
				: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
			{
				this.args = args;
				this.byrefs = byrefs;
			}

#if !COMPACT_FRAMEWORK
			protected override void CallImpl(ILGenerator ilgen)
			{
				MethodBase mb = GetMethod();
				MethodInfo mi = mb as MethodInfo;
				if(mi != null)
				{
					ilgen.Emit(OpCodes.Call, mi);
				}
				else
				{
					ilgen.Emit(OpCodes.Call, (ConstructorInfo)mb);
				}
			}

			protected override void CallvirtImpl(ILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Callvirt, (MethodInfo)GetMethod());
			}

			protected override void NewobjImpl(ILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
			}

			protected override void PreEmit(ILGenerator ilgen)
			{
				LocalBuilder[] locals = new LocalBuilder[args.Length];
				for(int i = args.Length - 1; i >= 0; i--)
				{
					Type type = args[i];
					if(type.IsByRef)
					{
						type = ArrayTypeWrapper.MakeArrayType(type.GetElementType(), 1);
					}
					locals[i] = ilgen.DeclareLocal(type);
					ilgen.Emit(OpCodes.Stloc, locals[i]);
				}
				for(int i = 0; i < args.Length; i++)
				{
					ilgen.Emit(OpCodes.Ldloc, locals[i]);
					if(args[i].IsByRef)
					{
						ilgen.Emit(OpCodes.Ldc_I4_0);
						ilgen.Emit(OpCodes.Ldelema, args[i].GetElementType());
					}
				}
				base.PreEmit(ilgen);
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				object[] newargs = (object[])args.Clone();
				for(int i = 0; i < newargs.Length; i++)
				{
					if(byrefs[i])
					{
						newargs[i] = ((Array)args[i]).GetValue(0);
					}
				}
				try
				{
					return base.Invoke(obj, newargs, nonVirtual);
				}
				finally
				{
					for(int i = 0; i < newargs.Length; i++)
					{
						if(byrefs[i])
						{
							((Array)args[i]).SetValue(newargs[i], 0);
						}
					}
				}
			}
#endif // !STATIC_COMPILER
		}

		internal static bool IsVisible(Type type)
		{
			return type.IsPublic || (type.IsNestedPublic && IsVisible(type.DeclaringType));
		}

		private class EnumWrapMethodWrapper : MethodWrapper
		{
			internal EnumWrapMethodWrapper(DotNetTypeWrapper tw, TypeWrapper fieldType)
				: base(tw, "wrap", "(" + fieldType.SigName + ")" + tw.SigName, null, tw, new TypeWrapper[] { fieldType }, Modifiers.Static | Modifiers.Public, MemberFlags.None)
			{
			}

#if !COMPACT_FRAMEWORK
			internal override void EmitCall(ILGenerator ilgen)
			{
				// We don't actually need to do anything here!
				// The compiler will insert a boxing operation after calling us and that will
				// result in our argument being boxed (since that's still sitting on the stack).
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				return Enum.ToObject(DeclaringType.TypeAsTBD, ((IConvertible)args[0]).ToInt64(null));
			}
#endif // !STATIC_COMPILER
		}

		internal class EnumValueFieldWrapper : FieldWrapper
		{
			private Type underlyingType;

			internal EnumValueFieldWrapper(DotNetTypeWrapper tw, TypeWrapper fieldType)
				: base(tw, fieldType, "Value", fieldType.SigName, new ExModifiers(Modifiers.Public | Modifiers.Final, false), null)
			{
				underlyingType = Enum.GetUnderlyingType(tw.type);
			}

#if !COMPACT_FRAMEWORK
			protected override void EmitGetImpl(ILGenerator ilgen)
			{
				// NOTE if the reference on the stack is null, we *want* the NullReferenceException, so we don't use TypeWrapper.EmitUnbox
				ilgen.LazyEmitUnbox(underlyingType);
				ilgen.LazyEmitLdobj(underlyingType);
			}

			protected override void EmitSetImpl(ILGenerator ilgen)
			{
				throw new InvalidOperationException();
			}
#endif

#if !STATIC_COMPILER
			internal override void SetValue(object obj, object val)
			{
				// NOTE even though the field is final, JNI reflection can still be used to set its value!
				// NOTE the CLI spec says that an enum has exactly one instance field, so we take advantage of that fact.
				FieldInfo f = DeclaringType.TypeAsTBD.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)[0];
				f.SetValue(obj, val);
			}
#endif // !STATIC_COMPILER

			// this method takes a boxed Enum and returns its value as a boxed primitive
			// of the subset of Java primitives (i.e. byte, short, int, long)
			internal static object GetEnumPrimitiveValue(object obj)
			{
				return GetEnumPrimitiveValue(Enum.GetUnderlyingType(obj.GetType()), obj);
			}

			// this method can be used to convert an enum value or its underlying value to a Java primitive
			internal static object GetEnumPrimitiveValue(Type underlyingType, object obj)
			{
				if(underlyingType == typeof(sbyte) || underlyingType == typeof(byte))
				{
					return unchecked((byte)((IConvertible)obj).ToInt32(null));
				}
				else if(underlyingType == typeof(short) || underlyingType == typeof(ushort))
				{
					return unchecked((short)((IConvertible)obj).ToInt32(null));
				}
				else if(underlyingType == typeof(int))
				{
					return ((IConvertible)obj).ToInt32(null);
				}
				else if(underlyingType == typeof(uint))
				{
					return unchecked((int)((IConvertible)obj).ToUInt32(null));
				}
				else if(underlyingType == typeof(long))
				{
					return ((IConvertible)obj).ToInt64(null);
				}
				else if(underlyingType == typeof(ulong))
				{
					return unchecked((long)((IConvertible)obj).ToUInt64(null));
				}
				else
				{
					throw new InvalidOperationException();
				}
			}

#if !STATIC_COMPILER
			internal override object GetValue(object obj)
			{
				return GetEnumPrimitiveValue(obj);
			}
#endif // !STATIC_COMPILER
		}

		private class ValueTypeDefaultCtor : MethodWrapper
		{
			internal ValueTypeDefaultCtor(DotNetTypeWrapper tw)
				: base(tw, "<init>", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Public, MemberFlags.None)
			{
			}

#if !COMPACT_FRAMEWORK
			internal override void EmitNewobj(ILGenerator ilgen, MethodAnalyzer ma, int opcodeIndex)
			{
				LocalBuilder local = ilgen.DeclareLocal(DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, local);
				ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				if(obj == null)
				{
					obj = Activator.CreateInstance(DeclaringType.TypeAsTBD);
				}
				return obj;
			}
#endif // !STATIC_COMPILER
		}

		private class FinalizeMethodWrapper : MethodWrapper
		{
			internal FinalizeMethodWrapper(DotNetTypeWrapper tw)
				: base(tw, "finalize", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Protected, MemberFlags.None)
			{
			}

			internal override void EmitCall(ILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Pop);
			}

			internal override void EmitCallvirt(ILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Pop);
			}

#if !STATIC_COMPILER
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				return null;
			}
#endif // !STATIC_COMPILER
		}

		private class CloneMethodWrapper : MethodWrapper
		{
			internal CloneMethodWrapper(DotNetTypeWrapper tw)
				: base(tw, "clone", "()Ljava.lang.Object;", null, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray, Modifiers.Protected, MemberFlags.None)
			{
			}

			internal override void EmitCall(ILGenerator ilgen)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Isinst, ClassLoaderWrapper.LoadClassCritical("java.lang.Cloneable").TypeAsBaseType);
				Label label1 = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue_S, label1);
				Label label2 = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brfalse_S, label2);
				EmitHelper.Throw(ilgen, "java.lang.CloneNotSupportedException");
				ilgen.MarkLabel(label2);
				EmitHelper.Throw(ilgen, "java.lang.NullPointerException");
				ilgen.MarkLabel(label1);
				ilgen.Emit(OpCodes.Call, typeof(object).GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
			}

			internal override void EmitCallvirt(ILGenerator ilgen)
			{
				EmitCall(ilgen);
			}

#if !STATIC_COMPILER
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				return CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(Name, Signature, false).Invoke(obj, args, nonVirtual);
			}
#endif // !STATIC_COMPILER
		}

		protected override void LazyPublishMembers()
		{
			// special support for enums
			if(type.IsEnum)
			{
				Type underlyingType = Enum.GetUnderlyingType(type);
				if(underlyingType == typeof(sbyte))
				{
					underlyingType = typeof(byte);
				}
				else if(underlyingType == typeof(ushort))
				{
					underlyingType = typeof(short);
				}
				else if(underlyingType == typeof(uint))
				{
					underlyingType = typeof(int);
				}
				else if(underlyingType == typeof(ulong))
				{
					underlyingType = typeof(long);
				}
				TypeWrapper fieldType = ClassLoaderWrapper.GetWrapperFromType(underlyingType);
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
				ArrayList fieldsList = new ArrayList();
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i].FieldType == type)
					{
						string name = fields[i].Name;
						if(name == "Value")
						{
							name = "_Value";
						}
						else if(name.StartsWith("_") && name.EndsWith("Value"))
						{
							name = "_" + name;
						}
						object val = EnumValueFieldWrapper.GetEnumPrimitiveValue(underlyingType, fields[i].GetRawConstantValue());
						fieldsList.Add(new ConstantFieldWrapper(this, fieldType, name, fieldType.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final, fields[i], val, MemberFlags.None));
					}
				}
				fieldsList.Add(new EnumValueFieldWrapper(this, fieldType));
				SetFields((FieldWrapper[])fieldsList.ToArray(typeof(FieldWrapper)));
				SetMethods(new MethodWrapper[] { new EnumWrapMethodWrapper(this, fieldType) });
			}
			else
			{
				ArrayList fieldsList = new ArrayList();
				FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for(int i = 0; i < fields.Length; i++)
				{
					// TODO for remapped types, instance fields need to be converted to static getter/setter methods
					if(fields[i].FieldType.IsPointer)
					{
						// skip, pointer fields are not supported
					}
					else
					{
						// TODO handle name/signature clash
						fieldsList.Add(CreateFieldWrapperDotNet(AttributeHelper.GetModifiers(fields[i], true).Modifiers, fields[i].Name, fields[i].FieldType, fields[i]));
					}
				}
				SetFields((FieldWrapper[])fieldsList.ToArray(typeof(FieldWrapper)));

				Hashtable methodsList = new Hashtable();

				// special case for delegate constructors!
				if(IsDelegate(type))
				{
					TypeWrapper iface = InnerClasses[0];
					DelegateMethodWrapper mw = new DelegateMethodWrapper(this, (DelegateInnerClassTypeWrapper)iface);
					methodsList.Add(mw.Name + mw.Signature, mw);
				}

				ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for(int i = 0; i < constructors.Length; i++)
				{
					string name;
					string sig;
					TypeWrapper[] args;
					TypeWrapper ret;
					if(MakeMethodDescriptor(constructors[i], out name, out sig, out args, out ret))
					{
						MethodWrapper mw = CreateMethodWrapper(name, sig, args, ret, constructors[i], false);
						string key = mw.Name + mw.Signature;
						if(!methodsList.ContainsKey(key))
						{
							methodsList.Add(key, mw);
						}
					}
				}

				if(type.IsValueType && !methodsList.ContainsKey("<init>()V"))
				{
					// Value types have an implicit default ctor
					methodsList.Add("<init>()V", new ValueTypeDefaultCtor(this));
				}

				MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
				for(int i = 0; i < methods.Length; i++)
				{
					if(methods[i].IsStatic && type.IsInterface)
					{
						// skip, Java cannot deal with static methods on interfaces
					}
					else
					{
						string name;
						string sig;
						TypeWrapper[] args;
						TypeWrapper ret;
						if(MakeMethodDescriptor(methods[i], out name, out sig, out args, out ret))
						{
							if(!methods[i].IsStatic && !methods[i].IsPrivate && BaseTypeWrapper != null)
							{
								MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
								if(baseMethod != null && baseMethod.IsFinal && !baseMethod.IsStatic && !baseMethod.IsPrivate)
								{
									continue;
								}
							}
							MethodWrapper mw = CreateMethodWrapper(name, sig, args, ret, methods[i], false);
							string key = mw.Name + mw.Signature;
							MethodWrapper existing = (MethodWrapper)methodsList[key];
							if(existing == null || existing is ByRefMethodWrapper)
							{
								methodsList[key] = mw;
							}
						}
						else if(methods[i].IsAbstract)
						{
							this.HasUnsupportedAbstractMethods = true;
						}
					}
				}

				// make sure that all the interface methods that we implement are available as public methods,
				// otherwise javac won't like the class.
				if(!type.IsInterface)
				{
					Type[] interfaces = type.GetInterfaces();
					for(int i = 0; i < interfaces.Length; i++)
					{
						// we only handle public (or nested public) types, because we're potentially adding a
						// method that should be callable by anyone through the interface
						if(IsVisible(interfaces[i]))
						{
							InterfaceMapping map = type.GetInterfaceMap(interfaces[i]);
							for(int j = 0; j < map.InterfaceMethods.Length; j++)
							{
								if((!map.TargetMethods[j].IsPublic || map.TargetMethods[j].Name != map.InterfaceMethods[j].Name)
									&& map.TargetMethods[j].DeclaringType == type)
								{
									string name;
									string sig;
									TypeWrapper[] args;
									TypeWrapper ret;
									if(MakeMethodDescriptor(map.InterfaceMethods[j], out name, out sig, out args, out ret))
									{
										string key = name + sig;
										MethodWrapper existing = (MethodWrapper)methodsList[key];
										if(existing == null && BaseTypeWrapper != null)
										{
											MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
											if(baseMethod != null && !baseMethod.IsStatic && baseMethod.IsPublic)
											{
												continue;
											}
										}
										if(existing == null || existing is ByRefMethodWrapper || existing.IsStatic || !existing.IsPublic)
										{
											// TODO if existing != null, we need to rename the existing method (but this is complicated because
											// it also affects subclasses). This is especially required is the existing method is abstract,
											// because otherwise we won't be able to create any subclasses in Java.
											methodsList[key] = CreateMethodWrapper(name, sig, args, ret, map.InterfaceMethods[j], true);
										}
									}
								}
							}
						}
					}
				}

				// for non-final remapped types, we need to add all the virtual methods in our alter ego (which
				// appears as our base class) and make them final (to prevent Java code from overriding these
				// methods, which don't really exist).
				if(ClassLoaderWrapper.IsRemappedType(type) && !type.IsSealed && !type.IsInterface)
				{
					// Finish the type, to make sure the methods are populated
					this.BaseTypeWrapper.Finish();
					TypeWrapper baseTypeWrapper = this.BaseTypeWrapper;
					while(baseTypeWrapper != null)
					{
						foreach(MethodWrapper m in baseTypeWrapper.GetMethods())
						{
							if(!m.IsStatic && !m.IsFinal && (m.IsPublic || m.IsProtected) && m.Name != "<init>")
							{
								string key = m.Name + m.Signature;
								if(!methodsList.ContainsKey(key))
								{
									if(m.IsProtected)
									{
										if(m.Name == "finalize" && m.Signature == "()V")
										{
											methodsList.Add(key, new FinalizeMethodWrapper(this));
										}
										else if(m.Name == "clone" && m.Signature == "()Ljava.lang.Object;")
										{
											methodsList.Add(key, new CloneMethodWrapper(this));
										}
										else
										{
											// there should be a special MethodWrapper for this method
											throw new InvalidOperationException("Missing protected method support for " + baseTypeWrapper.Name + "::" + m.Name + m.Signature);
										}
									}
									else
									{
										methodsList.Add(key, new BaseFinalMethodWrapper(this, m));
									}
								}
							}
						}
						baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
					}
				}
				MethodWrapper[] methodArray = new MethodWrapper[methodsList.Count];
				methodsList.Values.CopyTo(methodArray, 0);
				SetMethods(methodArray);
			}
		}

		private class BaseFinalMethodWrapper : MethodWrapper
		{
			private MethodWrapper m;

			internal BaseFinalMethodWrapper(DotNetTypeWrapper tw, MethodWrapper m)
				: base(tw, m.Name, m.Signature, m.GetMethod(), m.ReturnType, m.GetParameters(), m.Modifiers | Modifiers.Final, MemberFlags.None)
			{
				this.m = m;
			}

#if !COMPACT_FRAMEWORK
			internal override void EmitCall(ILGenerator ilgen)
			{
				// we direct EmitCall to EmitCallvirt, because we always want to end up at the instancehelper method
				// (EmitCall would go to our alter ego .NET type and that wouldn't be legal)
				m.EmitCallvirt(ilgen);
			}

			internal override void EmitCallvirt(ILGenerator ilgen)
			{
				m.EmitCallvirt(ilgen);
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				return m.Invoke(obj, args, nonVirtual);
			}
#endif // !STATIC_COMPILER
		}

		internal static bool IsUnsupportedAbstractMethod(MethodBase mb)
		{
			if(mb.IsAbstract)
			{
				MethodInfo mi = (MethodInfo)mb;
				if(mi.ReturnType.IsPointer || mi.ReturnType.IsByRef)
				{
					return true;
				}
				foreach(ParameterInfo p in mi.GetParameters())
				{
					if(p.ParameterType.IsByRef || p.ParameterType.IsPointer)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool MakeMethodDescriptor(MethodBase mb, out string name, out string sig, out TypeWrapper[] args, out TypeWrapper ret)
		{
			if(mb.IsGenericMethodDefinition)
			{
				name = null;
				sig = null;
				args = null;
				ret = null;
				return false;
			}
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append('(');
			ParameterInfo[] parameters = mb.GetParameters();
			args = new TypeWrapper[parameters.Length];
			for(int i = 0; i < parameters.Length; i++)
			{
				Type type = parameters[i].ParameterType;
				if(type.IsPointer)
				{
					name = null;
					sig = null;
					args = null;
					ret = null;
					return false;
				}
				if(type.IsByRef)
				{
					if(type.GetElementType().IsPointer)
					{
						name = null;
						sig = null;
						args = null;
						ret = null;
						return false;
					}
					type = ArrayTypeWrapper.MakeArrayType(type.GetElementType(), 1);
					if(mb.IsAbstract)
					{
						// Since we cannot override methods with byref arguments, we don't report abstract
						// methods with byref args.
						name = null;
						sig = null;
						args = null;
						ret = null;
						return false;
					}
				}
				TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
				args[i] = tw;
				sb.Append(tw.SigName);
			}
			sb.Append(')');
			if(mb is ConstructorInfo)
			{
				ret = PrimitiveTypeWrapper.VOID;
				if(mb.IsStatic)
				{
					name = "<clinit>";
				}
				else
				{
					name = "<init>";
				}
				sb.Append(ret.SigName);
				sig = sb.ToString();
				return true;
			}
			else
			{
				Type type = ((MethodInfo)mb).ReturnType;
				if(type.IsPointer || type.IsByRef)
				{
					name = null;
					sig = null;
					ret = null;
					return false;
				}
				ret = ClassLoaderWrapper.GetWrapperFromType(type);
				sb.Append(ret.SigName);
				name = mb.Name;
				sig = sb.ToString();
				return true;
			}
		}

		internal override TypeWrapper[] Interfaces
		{
			get
			{
				lock(this)
				{
					if(interfaces == null)
					{
						Type[] interfaceTypes = type.GetInterfaces();
						interfaces = new TypeWrapper[interfaceTypes.Length];
						for(int i = 0; i < interfaceTypes.Length; i++)
						{
							if(interfaceTypes[i].DeclaringType != null &&
								AttributeHelper.IsHideFromJava(interfaceTypes[i]) &&
								interfaceTypes[i].Name == "__Interface")
							{
								// we have to return the declaring type for ghost interfaces
								interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(interfaceTypes[i].DeclaringType);
							}
							else
							{
								interfaces[i] = ClassLoaderWrapper.GetWrapperFromType(interfaceTypes[i]);
							}
						}
					}
					return interfaces;
				}
			}
		}

		private static bool IsAttribute(Type type)
		{
			if(!type.IsAbstract && type.IsSubclassOf(typeof(Attribute)) && IsVisible(type))
			{
				//
				// Based on the number of constructors and their arguments, we distinguish several types
				// of attributes:
				//                                   | def ctor | single 1-arg ctor
				// -----------------------------------------------------------------
				// complex only (i.e. Annotation{N}) |          |
				// all optional fields/properties    |    X     |
				// required "value"                  |          |   X
				// optional "value"                  |    X     |   X
				// -----------------------------------------------------------------
				// 
				// TODO currently we don't support "complex only" attributes.
				//
				ConstructorInfo defCtor;
				ConstructorInfo singleOneArgCtor;
				AttributeAnnotationTypeWrapper.GetConstructors(type, out defCtor, out singleOneArgCtor);
				return defCtor != null || singleOneArgCtor != null;
			}
			return false;
		}

		private static bool IsDelegate(Type type)
		{
			// HACK non-public delegates do not get the special treatment (because they are likely to refer to
			// non-public types in the arg list and they're not really useful anyway)
			// NOTE we don't have to check in what assembly the type lives, because this is a DotNetTypeWrapper,
			// we know that it is a different assembly.
			if(!type.IsAbstract && type.IsSubclassOf(typeof(MulticastDelegate)) && IsVisible(type))
			{
				MethodInfo invoke = type.GetMethod("Invoke");
				if(invoke != null)
				{
					foreach(ParameterInfo p in invoke.GetParameters())
					{
						// TODO at the moment we don't support delegates with pointer or byref parameters
						if(p.ParameterType.IsPointer || p.ParameterType.IsByRef)
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				lock(this)
				{
					if(innerClasses == null)
					{
						Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
						ArrayList list = new ArrayList(nestedTypes.Length);
						for(int i = 0; i < nestedTypes.Length; i++)
						{
							if (!nestedTypes[i].IsGenericTypeDefinition)
							{
								list.Add(ClassLoaderWrapper.GetWrapperFromType(nestedTypes[i]));
							}
						}
						if(IsDelegate(type))
						{
							ClassLoaderWrapper classLoader = GetClassLoader();
							list.Add(classLoader.RegisterInitiatingLoader(new DelegateInnerClassTypeWrapper(Name + DelegateInterfaceSuffix, type, classLoader)));
						}
						if(IsAttribute(type))
						{
							list.Add(GetClassLoader().RegisterInitiatingLoader(new AttributeAnnotationTypeWrapper(Name + AttributeAnnotationSuffix, type)));
						}
						if(type.IsEnum && IsVisible(type))
						{
							list.Add(GetClassLoader().RegisterInitiatingLoader(new EnumEnumTypeWrapper(Name + EnumEnumSuffix, type)));
						}
						innerClasses = (TypeWrapper[])list.ToArray(typeof(TypeWrapper));
					}
				}
				return innerClasses;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				if(outerClass == null)
				{
					Type outer = type.DeclaringType;
					if(outer != null)
					{
						outerClass = ClassLoaderWrapper.GetWrapperFromType(outer);
					}
				}
				return outerClass;
			}
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				if(DeclaringTypeWrapper != null)
				{
					return Modifiers | Modifiers.Static;
				}
				return Modifiers;
			}
		}

		private FieldWrapper CreateFieldWrapperDotNet(Modifiers modifiers, string name, Type fieldType, FieldInfo field)
		{
			TypeWrapper type = ClassLoaderWrapper.GetWrapperFromType(fieldType);
			if(field.IsLiteral)
			{
				return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers, field, null, MemberFlags.None);
			}
			else
			{
				return FieldWrapper.Create(this, type, field, name, type.SigName, new ExModifiers(modifiers, false));
			}
		}

		private MethodWrapper CreateMethodWrapper(string name, string sig, TypeWrapper[] argTypeWrappers, TypeWrapper retTypeWrapper, MethodBase mb, bool privateInterfaceImplHack)
		{
			ExModifiers exmods = AttributeHelper.GetModifiers(mb, true);
			Modifiers mods = exmods.Modifiers;
			if(name == "Finalize" && sig == "()V" && !mb.IsStatic &&
				TypeAsBaseType.IsSubclassOf(CoreClasses.java.lang.Object.Wrapper.TypeAsBaseType))
			{
				// TODO if the .NET also has a "finalize" method, we need to hide that one (or rename it, or whatever)
				MethodWrapper mw = new SimpleCallMethodWrapper(this, "finalize", "()V", (MethodInfo)mb, null, null, mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
				mw.SetDeclaredExceptions(new string[] { "java.lang.Throwable" });
				return mw;
			}
			ParameterInfo[] parameters = mb.GetParameters();
			Type[] args = new Type[parameters.Length];
			bool hasByRefArgs = false;
			bool[] byrefs = null;
			for(int i = 0; i < parameters.Length; i++)
			{
				args[i] = parameters[i].ParameterType;
				if(parameters[i].ParameterType.IsByRef)
				{
					if(byrefs == null)
					{
						byrefs = new bool[args.Length];
					}
					byrefs[i] = true;
					hasByRefArgs = true;
				}
			}
			if(privateInterfaceImplHack)
			{
				mods &= ~Modifiers.Abstract;
				mods |= Modifiers.Final;
			}
			if(hasByRefArgs)
			{
				if(!(mb is ConstructorInfo) && !mb.IsStatic)
				{
					mods |= Modifiers.Final;
				}
				return new ByRefMethodWrapper(args, byrefs, this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, false);
			}
			else
			{
				if(mb is ConstructorInfo)
				{
					return new SmartConstructorMethodWrapper(this, name, sig, (ConstructorInfo)mb, argTypeWrappers, mods, MemberFlags.None);
				}
				else
				{
					return new SmartCallMethodWrapper(this, name, sig, (MethodInfo)mb, retTypeWrapper, argTypeWrappers, mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
				}
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				return type;
			}
		}

		internal override bool IsRemapped
		{
			get
			{
				return ClassLoaderWrapper.IsRemappedType(type);
			}
		}

#if !COMPACT_FRAMEWORK
		internal override void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
		{
			if(IsRemapped)
			{
				TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromType(type);
				MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<instanceof>");
				if(method != null)
				{
					ilgen.Emit(OpCodes.Call, method);
					return;
				}
			}
			ilgen.LazyEmit_instanceof(type);
		}

		internal override void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
		{
			if(IsRemapped)
			{
				TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromType(type);
				MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<checkcast>");
				if(method != null)
				{
					ilgen.Emit(OpCodes.Call, method);
					return;
				}
			}
			EmitHelper.Castclass(ilgen, type);
		}
#endif

		internal override void Finish()
		{
			if(BaseTypeWrapper != null)
			{
				BaseTypeWrapper.Finish();
			}
			foreach(TypeWrapper tw in this.Interfaces)
			{
				tw.Finish();
			}
		}

		internal override string GetGenericSignature()
		{
			return null;
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			return null;
		}

		internal override string[] GetEnclosingMethod()
		{
			return null;
		}

		internal override object[] GetDeclaredAnnotations()
		{
			if(type.Assembly.ReflectionOnly)
			{
				// TODO on Whidbey this must be implemented
				return null;
			}
			return type.GetCustomAttributes(false);
		}

		internal override object[] GetFieldAnnotations(FieldWrapper fw)
		{
			// TODO on Whidbey this must be implemented
			return null;
		}

		internal override object[] GetMethodAnnotations(MethodWrapper mw)
		{
			// TODO on Whidbey this must be implemented
			return null;
		}

		internal override object[][] GetParameterAnnotations(MethodWrapper mw)
		{
			// TODO on Whidbey this must be implemented
			return null;
		}
	}

	sealed class ArrayTypeWrapper : TypeWrapper
	{
		private static TypeWrapper[] interfaces;
		private static MethodInfo clone;
		private readonly TypeWrapper ultimateElementTypeWrapper;
		private Type arrayType;
		private bool finished;

		internal ArrayTypeWrapper(TypeWrapper ultimateElementTypeWrapper, string name)
			: base(Modifiers.Final | Modifiers.Abstract | (ultimateElementTypeWrapper.Modifiers & Modifiers.Public), name, CoreClasses.java.lang.Object.Wrapper)
		{
			this.ultimateElementTypeWrapper = ultimateElementTypeWrapper;
			this.IsInternal = ultimateElementTypeWrapper.IsInternal;
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
					clone = typeof(Array).GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
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
					tw[0] = ClassLoaderWrapper.LoadClassCritical("java.lang.Cloneable");
					tw[1] = ClassLoaderWrapper.LoadClassCritical("java.io.Serializable");
					interfaces = tw;
				}
				return interfaces;
			}
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				return TypeWrapper.EmptyArray;
			}
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				return null;
			}
		}

		internal override Type TypeAsTBD
		{
			get
			{
				if(arrayType == null)
				{
					arrayType = MakeArrayType(ultimateElementTypeWrapper.TypeAsArrayType, this.ArrayRank);
				}
				return arrayType;
			}
		}

		internal override void Finish()
		{
			lock(this)
			{
				if(!finished)
				{
					finished = true;
					ultimateElementTypeWrapper.Finish();
					arrayType = MakeArrayType(ultimateElementTypeWrapper.TypeAsArrayType, this.ArrayRank);
					ClassLoaderWrapper.ResetWrapperForType(arrayType, this);
				}
			}
		}

		internal override string GetGenericSignature()
		{
			return null;
		}

		internal override string GetGenericMethodSignature(MethodWrapper mw)
		{
			return null;
		}

		internal override string GetGenericFieldSignature(FieldWrapper fw)
		{
			return null;
		}

		internal override string[] GetEnclosingMethod()
		{
			return null;
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
}
