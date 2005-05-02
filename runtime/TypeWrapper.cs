/*
  Copyright (C) 2002, 2003, 2004, 2005 Jeroen Frijters

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
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using IKVM.Runtime;
using IKVM.Attributes;
using IKVM.Internal;

using ILGenerator = CountingILGenerator;

class EmitHelper
{
	private static MethodInfo objectToString = typeof(object).GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
	private static MethodInfo verboseCastFailure = Environment.GetEnvironmentVariable("IKVM_VERBOSE_CAST") == null ? null : typeof(ByteCodeHelper).GetMethod("VerboseCastFailure");

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

	internal static void RunClassConstructor(ILGenerator ilgen, Type type)
	{
		// NOTE we're *not* running the .cctor if the class is not a Java class
		// NOTE this is a potential versioning problem, if the base class lives in another assembly and doesn't
		// have a <clinit> now, a newer version that does have a <clinit> will not have it's <clinit> called by us.
		// A possible solution would be to use RuntimeHelpers.RunClassConstructor when "type" is a Java type and
		// lives in another assembly as the caller (which we don't know at the moment).
		FieldInfo field = type.GetField("__<clinit>", BindingFlags.Static | BindingFlags.NonPublic);
		if(field != null)
		{
			ilgen.Emit(OpCodes.Ldsfld, field);
			ilgen.Emit(OpCodes.Pop);
		}
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

class AttributeHelper
{
	private static CustomAttributeBuilder ghostInterfaceAttribute;
	private static CustomAttributeBuilder hideFromJavaAttribute;
	private static CustomAttributeBuilder deprecatedAttribute;
	private static CustomAttributeBuilder editorBrowsableNever;
	private static ConstructorInfo implementsAttribute;
	private static ConstructorInfo throwsAttribute;
	private static ConstructorInfo sourceFileAttribute;
	private static ConstructorInfo lineNumberTableAttribute;

	private static object ParseValue(TypeWrapper tw, string val)
	{
		if(tw == CoreClasses.java.lang.String.Wrapper)
		{
			return val;
		}
		else if(tw.TypeAsTBD.IsEnum)
		{
			return Enum.Parse(tw.TypeAsTBD, val);
		}
		else if(tw.TypeAsTBD == typeof(Type))
		{
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

	internal static CustomAttributeBuilder CreateCustomAttribute(IKVM.Internal.MapXml.Attribute attr)
	{
		// TODO add error handling
		TypeWrapper[] twargs = ClassFile.ArgTypeWrapperListFromSig(ClassLoaderWrapper.GetBootstrapClassLoader(), new Hashtable(), attr.Sig);
		Type[] argTypes = new Type[twargs.Length];
		object[] args = new object[argTypes.Length];
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
		if(attr.Type != null)
		{
			Type t = Type.GetType(attr.Type, true);
			ConstructorInfo ci = t.GetConstructor(argTypes);
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

	internal static void SetEditorBrowsableNever(MethodBuilder mb)
	{
		if(editorBrowsableNever == null)
		{
			editorBrowsableNever = new CustomAttributeBuilder(typeof(System.ComponentModel.EditorBrowsableAttribute).GetConstructor(new Type[] { typeof(System.ComponentModel.EditorBrowsableState) }), new object[] { System.ComponentModel.EditorBrowsableState.Never });
		}
		mb.SetCustomAttribute(editorBrowsableNever);
	}

	internal static void SetEditorBrowsableNever(PropertyBuilder pb)
	{
		if(editorBrowsableNever == null)
		{
			editorBrowsableNever = new CustomAttributeBuilder(typeof(System.ComponentModel.EditorBrowsableAttribute).GetConstructor(new Type[] { typeof(System.ComponentModel.EditorBrowsableState) }), new object[] { System.ComponentModel.EditorBrowsableState.Never });
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

	internal static void SetThrowsAttribute(MethodBase mb, string[] exceptions)
	{
		if(exceptions != null && exceptions.Length != 0)
		{
			if(throwsAttribute == null)
			{
				throwsAttribute = typeof(ThrowsAttribute).GetConstructor(new Type[] { typeof(string[]) });
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
			ghostInterfaceAttribute = new CustomAttributeBuilder(typeof(GhostInterfaceAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		typeBuilder.SetCustomAttribute(ghostInterfaceAttribute);
	}

	internal static void MirandaMethod(MethodBuilder mb)
	{
		CustomAttributeBuilder cab = new CustomAttributeBuilder(typeof(MirandaMethodAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		mb.SetCustomAttribute(cab);
	}

	internal static bool IsMirandaMethod(MethodInfo mi)
	{
		return mi.IsDefined(typeof(MirandaMethodAttribute), false);
	}

	internal static void HideFromJava(TypeBuilder typeBuilder)
	{
		if(hideFromJavaAttribute == null)
		{
			hideFromJavaAttribute = new CustomAttributeBuilder(typeof(HideFromJavaAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		typeBuilder.SetCustomAttribute(hideFromJavaAttribute);
	}

	internal static void HideFromJava(ConstructorBuilder cb)
	{
		if(hideFromJavaAttribute == null)
		{
			hideFromJavaAttribute = new CustomAttributeBuilder(typeof(HideFromJavaAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		cb.SetCustomAttribute(hideFromJavaAttribute);
	}

	internal static void HideFromJava(MethodBuilder mb)
	{
		if(hideFromJavaAttribute == null)
		{
			hideFromJavaAttribute = new CustomAttributeBuilder(typeof(HideFromJavaAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		mb.SetCustomAttribute(hideFromJavaAttribute);
	}

	internal static void HideFromJava(FieldBuilder fb)
	{
		if(hideFromJavaAttribute == null)
		{
			hideFromJavaAttribute = new CustomAttributeBuilder(typeof(HideFromJavaAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		fb.SetCustomAttribute(hideFromJavaAttribute);
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
		return mi.IsDefined(typeof(HideFromJavaAttribute), false);
	}

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
				implementsAttribute = typeof(ImplementsAttribute).GetConstructor(new Type[] { typeof(string[]) });
			}
			typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { interfaces }));
		}
	}

	internal static Modifiers GetModifiers(MethodBase mb, bool assemblyIsPrivate)
	{
		object[] customAttribute = mb.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
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
		return modifiers;
	}

	internal static Modifiers GetModifiers(FieldInfo fi, bool assemblyIsPrivate)
	{
		object[] customAttribute = fi.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
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
		// TODO reflection doesn't support volatile
		return modifiers;
	}

	internal static void SetModifiers(MethodBuilder mb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		mb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetModifiers(ConstructorBuilder cb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		cb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetModifiers(FieldBuilder fb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		fb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetModifiers(TypeBuilder tb, Modifiers modifiers)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { modifiers });
		tb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetNameSig(MethodBase mb, string name, string sig)
	{
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(NameSigAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { name, sig });
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
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(typeof(NameSigAttribute).GetConstructor(new Type[] { typeof(string), typeof(string) }), new object[] { name, sig });
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

	internal static void SetInnerClass(TypeBuilder typeBuilder, string innerClass, string outerClass, string name, Modifiers modifiers)
	{
		Type[] argTypes = new Type[] { typeof(string), typeof(string), typeof(string), typeof(Modifiers) };
		object[] args = new object[] { innerClass, outerClass, name, modifiers };
		ConstructorInfo ci = typeof(InnerClassAttribute).GetConstructor(argTypes);
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ci, args);
		typeBuilder.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetSourceFile(TypeBuilder typeBuilder, string filename)
	{
		if(sourceFileAttribute == null)
		{
			sourceFileAttribute = typeof(SourceFileAttribute).GetConstructor(new Type[] { typeof(string) });
		}
		typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(sourceFileAttribute, new object[] { filename }));
	}

	internal static void SetLineNumberTable(MethodBase mb, byte[] table)
	{
		if(lineNumberTableAttribute == null)
		{
			lineNumberTableAttribute = typeof(LineNumberTableAttribute).GetConstructor(new Type[] { typeof(byte[]) });
		}
		if(mb is ConstructorBuilder)
		{
			((ConstructorBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(lineNumberTableAttribute, new object[] { table }));
		}
		else
		{
			((MethodBuilder)mb).SetCustomAttribute(new CustomAttributeBuilder(lineNumberTableAttribute, new object[] { table }));
		}
	}
}

abstract class TypeWrapper
{
	private readonly ClassLoaderWrapper classLoader;
	private readonly string name;		// java name (e.g. java.lang.Object)
	private readonly Modifiers modifiers;
	private MethodWrapper[] methods;
	private FieldWrapper[] fields;
	private readonly TypeWrapper baseWrapper;
	private readonly object classObject;
	private bool hasIncompleteInterfaceImplementation;
	internal static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
	internal const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
	internal const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

	internal TypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper, ClassLoaderWrapper classLoader, object protectionDomain)
	{
		Profiler.Count("TypeWrapper");
		// class name should be dotted or null for primitives
		Debug.Assert(name == null || name.IndexOf('/') < 0);

		this.modifiers = modifiers;
		this.name = name;
		this.baseWrapper = baseWrapper;
		this.classLoader = classLoader;
		if(IsUnloadable || IsVerifierType || JVM.IsStaticCompiler)
		{
			this.classObject = null;
		}
		else
		{
			this.classObject = JVM.Library.newClass(this, protectionDomain);
		}
	}

	internal object ClassObject
	{
		get
		{
			Debug.Assert(!IsUnloadable && !IsVerifierType);
			return classObject;
		}
	}

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
			return hasIncompleteInterfaceImplementation || (baseWrapper != null && baseWrapper.HasIncompleteInterfaceImplementation);
		}
		set
		{
			hasIncompleteInterfaceImplementation = value;
		}
	}

	internal virtual bool HasStaticInitializer
	{
		get
		{
			return false;
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

	internal virtual ClassLoaderWrapper GetClassLoader()
	{
		return classLoader;
	}

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
		foreach(MethodWrapper mw in methods)
		{
			if(mw.Name == name && mw.Signature == sig)
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

	internal abstract Assembly Assembly
	{
		get;
	}

	// returns true iff wrapper is allowed to access us
	internal bool IsAccessibleFrom(TypeWrapper wrapper)
	{
		return IsPublic || IsInSamePackageAs(wrapper);
	}

	internal bool IsInSamePackageAs(TypeWrapper wrapper)
	{
		if(GetClassLoader() == wrapper.GetClassLoader() &&
			// Both types must also be in the same assembly, otherwise
			// the packages are not accessible.
			wrapper.Assembly == this.Assembly)
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

	internal virtual TypeBuilder TypeAsBuilder
	{
		get
		{
			TypeBuilder typeBuilder = TypeAsTBD as TypeBuilder;
			Debug.Assert(typeBuilder != null);
			return typeBuilder;
		}
	}

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
				int rank = ArrayRank;
				string type = "System.Object";
				for(int i = 0; i < rank; i++)
				{
					type += "[]";
				}
				return Type.GetType(type, true);
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
				int rank = ArrayRank;
				string type = "System.Object";
				for(int i = 0; i < rank; i++)
				{
					type += "[]";
				}
				return Type.GetType(type, true);
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
				int rank = ArrayRank;
				string type = "System.Object";
				for(int i = 0; i < rank; i++)
				{
					type += "[]";
				}
				return Type.GetType(type, true);
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
					return classLoader.LoadClassByDottedNameFast(name.Substring(1));
				case 'L':
					// NOTE this call to LoadClassByDottedNameFast can never fail and will not trigger a class load
					// (because the ultimate element type was already loaded when this type was created)
					return classLoader.LoadClassByDottedNameFast(name.Substring(2, name.Length - 3));
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
			return !elem1.IsNonPrimitiveValueType && elem1.IsSubTypeOf(elem2);
		}
		return this.IsSubTypeOf(wrapper);
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

	internal void Finish()
	{
		Finish(false);
	}

	internal abstract void Finish(bool forDebugSave);

	private void ImplementInterfaceMethodStubImpl(MethodWrapper ifmethod, TypeBuilder typeBuilder, DynamicTypeWrapper wrapper)
	{
		// we're mangling the name to prevent subclasses from accidentally overriding this method and to
		// prevent clashes with overloaded method stubs that are erased to the same signature (e.g. unloadable types and ghost arrays)
		string mangledName = this.Name + "/" + ifmethod.Name + ifmethod.Signature;
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
			if(!mce.IsPublic)
			{
				// NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
				// it makes sense, so I hope the spec is wrong
				// UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
				// methods. Sigh! So I have to use private methods and mangle the name
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
				AttributeHelper.HideFromJava(mb);
				EmitHelper.Throw(mb.GetILGenerator(), "java.lang.IllegalAccessError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
				wrapper.HasIncompleteInterfaceImplementation = true;
			}
			else if(mce.GetMethod() == null || mce.RealName != ifmethod.RealName)
			{
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
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
			else if(mce.DeclaringType.TypeAsTBD.Assembly != typeBuilder.Assembly)
			{
				// NOTE methods inherited from base classes in a different assembly do *not* automatically implement
				// interface methods, so we have to generate a stub here that doesn't do anything but call the base
				// implementation
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
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
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, ifmethod.ReturnTypeForDefineMethod, ifmethod.GetParametersForDefineMethod());
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

		// make sure we don't do the same method twice
		if(doneSet.ContainsKey(this))
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

	[Conditional("DEBUG")]
	internal static void AssertFinished(Type type)
	{
		if(type != null)
		{
			while(type.IsArray)
			{
				type = type.GetElementType();
			}
			Debug.Assert(!(type is TypeBuilder));
		}
	}

	internal void RunClassInit()
	{
		System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(TypeAsTBD.TypeHandle);
	}

	internal void EmitUnbox(ILGenerator ilgen)
	{
		Debug.Assert(this.IsNonPrimitiveValueType);

		Type type = this.TypeAsTBD;
		// NOTE if the reference is null, we treat it as a default instance of the value type.
		ilgen.Emit(OpCodes.Dup);
		Label label1 = ilgen.DefineLabel();
		ilgen.Emit(OpCodes.Brtrue_S, label1);
		ilgen.Emit(OpCodes.Pop);
		ilgen.Emit(OpCodes.Ldloc, ilgen.DeclareLocal(type));
		Label label2 = ilgen.DefineLabel();
		ilgen.Emit(OpCodes.Br_S, label2);
		ilgen.MarkLabel(label1);
		ilgen.Emit(OpCodes.Unbox, type);
		ilgen.Emit(OpCodes.Ldobj, type);
		ilgen.MarkLabel(label2);
	}

	internal void EmitBox(ILGenerator ilgen)
	{
		Debug.Assert(this.IsNonPrimitiveValueType);

		ilgen.Emit(OpCodes.Box, this.TypeAsTBD);
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
		}
	}

	internal virtual void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
	{
		if(IsGhost)
		{
			ilgen.Emit(OpCodes.Dup);
			// TODO make sure we get the right "Cast" method and cache it
			// NOTE for dynamic ghosts we don't end up here because DynamicTypeWrapper overrides this method,
			// so we're safe to call GetMethod on TypeAsTBD (because it has to be a compiled type, if we're here)
			ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("Cast"));
			ilgen.Emit(OpCodes.Pop);
		}
		else if(IsGhostArray)
		{
			ilgen.Emit(OpCodes.Dup);
			// TODO make sure we get the right "CastArray" method and cache it
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
			ilgen.Emit(OpCodes.Call, tw.TypeAsTBD.GetMethod("CastArray"));
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
			ilgen.Emit(OpCodes.Call, TypeAsTBD.GetMethod("IsInstanceArray"));
		}
		else
		{
			ilgen.Emit(OpCodes.Isinst, TypeAsTBD);
			ilgen.Emit(OpCodes.Ldnull);
			ilgen.Emit(OpCodes.Cgt_Un);
		}
	}

	internal static string GetSigNameFromType(Type type)
	{
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);

		if(wrapper != null)
		{
			return wrapper.SigName;
		}

		if(type.IsArray)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			while(type.IsArray)
			{
				sb.Append('[');
				type = type.GetElementType();
			}
			return sb.Append(GetSigNameFromType(type)).ToString();
		}

		string s = TypeWrapper.GetNameFromType(type);
		if(s[0] != '[')
		{
			s = "L" + s + ";";
		}
		return s;
	}

	// NOTE returns null for primitive types
	internal static string GetNameFromType(Type type)
	{
		TypeWrapper.AssertFinished(type);

		if(type.IsArray)
		{
			return GetSigNameFromType(type);
		}

		// first we check if a wrapper exists, because if it does we must use the name from the wrapper to
		// make sure that remapped types return the proper name
		TypeWrapper wrapper = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
		if(wrapper != null)
		{
			return wrapper.Name;
		}

		if(type.Module.IsDefined(typeof(JavaModuleAttribute), false))
		{
			return CompiledTypeWrapper.GetName(type);
		}
		else
		{
			return DotNetTypeWrapper.GetName(type);
		}
	}

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
}

class UnloadableTypeWrapper : TypeWrapper
{
	private static Hashtable warningHashtable;

	internal UnloadableTypeWrapper(string name)
		: base(TypeWrapper.UnloadableModifiersHack, name, null, null, null)
	{
		if(JVM.IsStaticCompiler && name != "<verifier>")
		{
			if(warningHashtable == null)
			{
				warningHashtable = new Hashtable();
			}
			if(!warningHashtable.ContainsKey(name))
			{
				warningHashtable.Add(name, name);
				Console.Error.WriteLine("Warning: class \"{0}\" not found", name);
			}
		}
	}

	internal override TypeWrapper EnsureLoadable(ClassLoaderWrapper loader)
	{
		TypeWrapper tw = null;
		try
		{
			tw = loader.LoadClassByDottedNameFast(this.Name);
		}
		catch
		{
		}
		if(tw == null)
		{
			throw new NoClassDefFoundError(this.Name);
		}
		return tw;
	}

	internal override Assembly Assembly
	{
		get
		{
			return null;
		}
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

	internal override void Finish(bool forDebugSave)
	{
		throw new InvalidOperationException("Finish called on UnloadableTypeWrapper: " + Name);
	}

	internal override void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
	{
		ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
		ilgen.Emit(OpCodes.Ldstr, Name);
		ilgen.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicCast"));
	}

	internal override void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
	{
		ilgen.Emit(OpCodes.Ldtoken, context.TypeAsTBD);
		ilgen.Emit(OpCodes.Ldstr, Name);
		ilgen.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("DynamicInstanceOf"));
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
		: base(Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null, null, null, null)
	{
		this.type = type;
		this.sigName = sigName;
	}

	internal override Assembly Assembly
	{
		get
		{
			return null;
		}
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

	internal override void Finish(bool forDebugSave)
	{
	}

	public override string ToString()
	{
		return "PrimitiveTypeWrapper[" + sigName + "]";
	}
}

class BakedTypeCleanupHack
{
	private static readonly FieldInfo m_methodBuilder = typeof(ConstructorBuilder).GetField("m_methodBuilder", BindingFlags.Instance | BindingFlags.NonPublic);
	private static readonly FieldInfo[] methodBuilderFields = GetFieldList(typeof(MethodBuilder), new string[]
			{
				"m_ilGenerator",
				"m_ubBody",
				"m_RVAFixups",
				"mm_mdMethodFixups",
				"m_localSignature",
				"m_localSymInfo",
				"m_exceptions",
				"m_parameterTypes",
				"m_retParam",
				"m_returnType",
				"m_signature"
			});
	private static readonly FieldInfo[] fieldBuilderFields = GetFieldList(typeof(FieldBuilder), new string[]
			{
				"m_data",
				"m_fieldType",
			});

	private static bool IsSupportedVersion
	{
		get
		{
			return Environment.Version.Major == 1 && Environment.Version.Minor == 1 && Environment.Version.Build == 4322;
		}
	}

	private static FieldInfo[] GetFieldList(Type type, string[] list)
	{
		if(JVM.IsStaticCompiler || ClassLoaderWrapper.IsSaveDebugImage || !IsSupportedVersion)
		{
			// if we're going to be saving the image, we cannot bash the MethodBuilder fields,
			// because the data is required to generated the PE file.
			return null;
		}
		FieldInfo[] fields = new FieldInfo[list.Length];
		for(int i = 0; i < list.Length; i++)
		{
			fields[i] = type.GetField(list[i], BindingFlags.Instance | BindingFlags.NonPublic);
			if(fields[i] == null)
			{
				return null;
			}
		}
		return fields;
	}

	internal static void Process(DynamicTypeWrapper wrapper)
	{
		if(m_methodBuilder != null && methodBuilderFields != null && fieldBuilderFields != null)
		{
			foreach(MethodWrapper mw in wrapper.GetMethods())
			{
				MethodBuilder mb = mw.GetMethod() as MethodBuilder;
				if(mb == null)
				{
					ConstructorBuilder cb = mw.GetMethod() as ConstructorBuilder;
					if(cb != null)
					{
						mb = (MethodBuilder)m_methodBuilder.GetValue(cb);
					}
				}
				if(mb != null)
				{
					foreach(FieldInfo fi in methodBuilderFields)
					{
						fi.SetValue(mb, null);
					}
				}
			}
			foreach(FieldWrapper fw in wrapper.GetFields())
			{
				FieldBuilder fb = fw.GetField() as FieldBuilder;
				if(fb != null)
				{
					foreach(FieldInfo fi in fieldBuilderFields)
					{
						fi.SetValue(fb, null);
					}
				}
			}
		}
	}
}

sealed class DynamicTypeWrapper : TypeWrapper
{
	private volatile DynamicImpl impl;
	private TypeWrapper[] interfaces;
	private FieldInfo ghostRefField;
	private MethodBuilder ghostIsInstanceMethod;
	private MethodBuilder ghostIsInstanceArrayMethod;
	private MethodBuilder ghostCastMethod;
	private MethodBuilder ghostCastArrayMethod;
	private bool hasStaticInitializer;
	private static TypeWrapper[] mappedExceptions;
	private static bool[] mappedExceptionsAllSubClasses;
	private static Hashtable ghosts;
	private static Hashtable mapxml;

	private static TypeWrapper LoadTypeWrapper(ClassLoaderWrapper classLoader, string name)
	{
		TypeWrapper tw = classLoader.LoadClassByDottedNameFast(name);
		if(tw == null)
		{
			throw new NoClassDefFoundError(name);
		}
		return tw;
	}

	internal DynamicTypeWrapper(ClassFile f, ClassLoaderWrapper classLoader, object protectionDomain)
		: base(f.Modifiers, f.Name, f.IsInterface ? null : LoadTypeWrapper(classLoader, f.SuperClass), classLoader, protectionDomain)
	{
		Profiler.Count("DynamicTypeWrapper");
		if(BaseTypeWrapper != null)
		{
			if(!BaseTypeWrapper.IsAccessibleFrom(this))
			{
				throw new IllegalAccessError("Class " + f.Name + " cannot access its superclass " + BaseTypeWrapper.Name);
			}
			if(BaseTypeWrapper.IsFinal)
			{
				throw new VerifyError("Cannot inherit from final class");
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
			if(!iface.IsInterface)
			{
				throw new IncompatibleClassChangeError("Implementing class");
			}
			this.interfaces[i] = iface;
		}

		impl = new JavaTypeImpl(f, this);
	}

	internal override bool HasStaticInitializer
	{
		get
		{
			return hasStaticInitializer;
		}
	}

	internal override Assembly Assembly
	{
		get
		{
			return GetClassLoader().ModuleBuilder.Assembly;
		}
	}

	internal override FieldInfo GhostRefField
	{
		get
		{
			return ghostRefField;
		}
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

	internal override Type TypeAsBaseType
	{
		get
		{
			return impl.TypeAsBaseType;
		}
	}

	internal override void Finish(bool forDebugSave)
	{
		lock(this)
		{
			Profiler.Enter("DynamicTypeWrapper.Finish");
			try
			{
				impl = impl.Finish(forDebugSave);
				// call Finish again to get the verify error for doomed types
				impl.Finish(forDebugSave);
			}
			finally
			{
				Profiler.Leave("DynamicTypeWrapper.Finish");
			}
		}
	}

	internal static void SetupGhosts(IKVM.Internal.MapXml.Root map)
	{
		ghosts = new Hashtable();

		// find the ghost interfaces
		foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
		{
			if(c.Shadows != null && c.Interfaces != null)
			{
				// NOTE we don't support interfaces that inherit from other interfaces
				// (actually, if they are explicitly listed it would probably work)
				TypeWrapper typeWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().GetLoadedClass(c.Name);
				foreach(IKVM.Internal.MapXml.Interface iface in c.Interfaces)
				{
					TypeWrapper ifaceWrapper = ClassLoaderWrapper.GetBootstrapClassLoader().GetLoadedClass(iface.Name);
					if(ifaceWrapper == null || !ifaceWrapper.TypeAsTBD.IsAssignableFrom(typeWrapper.TypeAsTBD))
					{
						AddGhost(iface.Name, typeWrapper);
					}
				}
			}
		}
		// we manually add the array ghost interfaces
		TypeWrapper array = ClassLoaderWrapper.GetWrapperFromType(typeof(Array));
		AddGhost("java.io.Serializable", array);
		AddGhost("java.lang.Cloneable", array);
	}

	private static void AddGhost(string interfaceName, TypeWrapper implementer)
	{
		ArrayList list = (ArrayList)ghosts[interfaceName];
		if(list == null)
		{
			list = new ArrayList();
			ghosts[interfaceName] = list;
		}
		list.Add(implementer);
	}

	internal override bool IsGhost
	{
		get
		{
			return ghosts != null && IsInterface && ghosts.ContainsKey(Name);
		}
	}

	private static TypeWrapper[] GetGhostImplementers(TypeWrapper wrapper)
	{
		ArrayList list = (ArrayList)ghosts[wrapper.Name];
		if(list == null)
		{
			return TypeWrapper.EmptyArray;
		}
		return (TypeWrapper[])list.ToArray(typeof(TypeWrapper));
	}

	private class ExceptionMapEmitter : CodeEmitter
	{
		private IKVM.Internal.MapXml.ExceptionMapping[] map;

		internal ExceptionMapEmitter(IKVM.Internal.MapXml.ExceptionMapping[] map)
		{
			this.map = map;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Callvirt, typeof(Object).GetMethod("GetType"));
			MethodInfo GetTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");
			for(int i = 0; i < map.Length; i++)
			{
				ilgen.Emit(OpCodes.Dup);
				ilgen.Emit(OpCodes.Ldtoken, Type.GetType(map[i].src));
				ilgen.Emit(OpCodes.Call, GetTypeFromHandle);
				ilgen.Emit(OpCodes.Ceq);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brfalse_S, label);
				ilgen.Emit(OpCodes.Pop);
				if(map[i].code != null)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
					map[i].code.Emit(ilgen);
					ilgen.Emit(OpCodes.Ret);
				}
				else
				{
					TypeWrapper tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(map[i].dst);
					MethodWrapper mw = tw.GetMethodWrapper("<init>", "()V", false);
					mw.Link();
					mw.EmitNewobj(ilgen);
					ilgen.Emit(OpCodes.Ret);
				}
				ilgen.MarkLabel(label);
			}
			ilgen.Emit(OpCodes.Pop);
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ret);
		}
	}

	internal static void LoadMapXml(IKVM.Internal.MapXml.Root map)
	{
		mapxml = new Hashtable();
		// HACK we've got a hardcoded location for the exception mapping method that is generated from the xml mapping
		mapxml["java.lang.ExceptionHelper.MapExceptionImpl(Ljava.lang.Throwable;)Ljava.lang.Throwable;"] = new ExceptionMapEmitter(map.exceptionMappings);
		foreach(IKVM.Internal.MapXml.Class c in map.assembly.Classes)
		{
			// HACK if it is not a remapped type, we assume it is a container for native methods
			if(c.Shadows == null)
			{
				string className = c.Name;
				mapxml.Add(className, c);
				if(c.Methods != null)
				{
					foreach(IKVM.Internal.MapXml.Method method in c.Methods)
					{
						if(method.body != null)
						{
							string methodName = method.Name;
							string methodSig = method.Sig;
							mapxml.Add(className + "." + methodName + methodSig, method.body);
						}
					}
				}
			}
		}
	}

	internal override bool IsMapUnsafeException
	{
		get
		{
			if(mappedExceptions != null)
			{
				for(int i = 0; i < mappedExceptions.Length; i++)
				{
					if(mappedExceptions[i].IsSubTypeOf(this) ||
						(mappedExceptionsAllSubClasses[i] && this.IsSubTypeOf(mappedExceptions[i])))
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	internal static void LoadMappedExceptions(IKVM.Internal.MapXml.Root map)
	{
		if(map.exceptionMappings != null)
		{
			mappedExceptionsAllSubClasses = new bool[map.exceptionMappings.Length];
			mappedExceptions = new TypeWrapper[map.exceptionMappings.Length];
			for(int i = 0; i < mappedExceptions.Length; i++)
			{
				string dst = map.exceptionMappings[i].dst;
				if(dst[0] == '*')
				{
					mappedExceptionsAllSubClasses[i] = true;
					dst = dst.Substring(1);
				}
				mappedExceptions[i] = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dst);
			}
		}
	}

	// NOTE can only be used if the type hasn't been finished yet!
	internal FieldInfo ClassObjectField
	{
		get
		{
			return ((JavaTypeImpl)impl).ClassObjectField;
		}
	}

	private abstract class DynamicImpl
	{
		internal abstract Type Type { get; }
		internal abstract Type TypeAsBaseType { get; }
		internal abstract TypeWrapper[] InnerClasses { get; }
		internal abstract TypeWrapper DeclaringTypeWrapper { get; }
		internal abstract Modifiers ReflectiveModifiers { get; }
		internal abstract DynamicImpl Finish(bool forDebugSave);
		internal abstract MethodBase LinkMethod(MethodWrapper mw);
		internal abstract FieldInfo LinkField(FieldWrapper fw);
	}

	private class JavaTypeImpl : DynamicImpl
	{
		private readonly ClassFile classFile;
		private readonly DynamicTypeWrapper wrapper;
		private readonly TypeBuilder typeBuilder;
		private MethodWrapper[] methods;
		private MethodWrapper[] baseMethods;
		private FieldWrapper[] fields;
		private bool finishing;
		private bool finishingForDebugSave;
		private FinishedTypeImpl finishedType;
		private readonly TypeWrapper outerClassWrapper;
		private readonly TypeBuilder typeBuilderGhostInterface;
		private Hashtable memberclashtable;
		private Hashtable classCache = new Hashtable();
		private FieldInfo classObjectField;

		internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper)
		{
			Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
			this.classFile = f;
			this.wrapper = wrapper;

			// process all methods
			bool hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
			methods = new MethodWrapper[classFile.Methods.Length];
			baseMethods = new MethodWrapper[classFile.Methods.Length];
			for(int i = 0; i < methods.Length; i++)
			{
				ClassFile.Method m = classFile.Methods[i];
				if(m.IsClassInitializer)
				{
					if(JVM.IsStaticCompiler)
					{
						if(!IsSideEffectFreeStaticInitializer(m))
						{
							hasclinit = true;
						}
					}
					else
					{
						hasclinit = true;
					}
				}
				if(wrapper.IsGhost)
				{
					methods[i] = new MethodWrapper.GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, MemberFlags.None);
				}
				else if(m.Name == "<init>")
				{
					methods[i] = new SmartConstructorMethodWrapper(wrapper, m.Name, m.Signature, null, null, m.Modifiers, MemberFlags.None);
				}
				else
				{
					bool explicitOverride = false;
					if(!classFile.IsInterface && !m.IsStatic && !m.IsPrivate)
					{
						baseMethods[i] = FindBaseMethod(m.Name, m.Signature, out explicitOverride);
					}
					methods[i] = new SmartCallMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, explicitOverride ? MemberFlags.ExplicitOverride : MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
				}
			}
			wrapper.hasStaticInitializer = hasclinit;
			if(wrapper.IsAbstract && !wrapper.IsInterface)
			{
				ArrayList methodsArray = new ArrayList(methods);
				ArrayList baseMethodsArray = new ArrayList(baseMethods);
				AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
				this.methods = (MethodWrapper[])methodsArray.ToArray(typeof(MethodWrapper));
				this.baseMethods = (MethodWrapper[])baseMethodsArray.ToArray(typeof(MethodWrapper));
			}
			wrapper.SetMethods(methods);

			fields = new FieldWrapper[classFile.Fields.Length];
			for(int i = 0; i < fields.Length; i++)
			{
				ClassFile.Field fld = classFile.Fields[i];
				if(fld.IsStatic && fld.IsFinal && fld.ConstantValue != null)
				{
					fields[i] = new ConstantFieldWrapper(wrapper, null, fld.Name, fld.Signature, fld.Modifiers, null, fld.ConstantValue);
				}
				else if(fld.IsFinal && (fld.IsPublic || fld.IsProtected) && !wrapper.IsInterface)
				{
					fields[i] = new GetterFieldWrapper(wrapper, null, null, fld.Name, fld.Signature, fld.Modifiers, null);
				}
				else
				{
					fields[i] = FieldWrapper.Create(wrapper, null, null, fld.Name, fld.Signature, fld.Modifiers);
				}
			}
			wrapper.SetFields(fields);

			// from now on we shouldn't be throwing any exceptions (to be precise, after we've
			// called ModuleBuilder.DefineType)
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
				TypeBuilder outer = null;
				// only if requested, we compile inner classes as nested types, because it has a higher cost
				// and doesn't buy us anything, unless we're compiling a library that could be used from C# (e.g.)
				if(JVM.CompileInnerClassesAsNestedTypes)
				{
					if(f.OuterClass != null)
					{
						if(!CheckInnerOuterNames(f.Name, f.OuterClass.Name))
						{
							Tracer.Warning(Tracer.Compiler, "Incorrect InnerClasses attribute on {0}", f.Name);
						}
						else
						{
							outerClassWrapper = wrapper.GetClassLoader().LoadClassByDottedName(f.OuterClass.Name);
							if(outerClassWrapper is DynamicTypeWrapper)
							{
								outer = outerClassWrapper.TypeAsBuilder;
							}
						}
					}
				}
				if(f.IsPublic)
				{
					if(outer != null)
					{
						typeAttribs |= TypeAttributes.NestedPublic;
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
				if(f.IsInterface)
				{
					typeAttribs |= TypeAttributes.Interface | TypeAttributes.Abstract;
					if(outer != null)
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
							typeAttribs &= ~(TypeAttributes.Interface | TypeAttributes.Abstract);
							typeAttribs |= TypeAttributes.Class | TypeAttributes.Sealed;
							typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(wrapper.GetClassLoader().MangleTypeName(f.Name), typeAttribs, typeof(ValueType));
							AttributeHelper.SetGhostInterface(typeBuilder);
							AttributeHelper.SetModifiers(typeBuilder, wrapper.Modifiers);
							wrapper.ghostRefField = typeBuilder.DefineField("__<ref>", typeof(object), FieldAttributes.Public | FieldAttributes.SpecialName);
							AttributeHelper.HideFromJava((FieldBuilder)wrapper.ghostRefField);
							typeBuilderGhostInterface = typeBuilder.DefineNestedType("__Interface", TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.NestedPublic);
							AttributeHelper.HideFromJava(typeBuilderGhostInterface);
							wrapper.ghostIsInstanceMethod = typeBuilder.DefineMethod("IsInstance", MethodAttributes.Public | MethodAttributes.Static, typeof(bool), new Type[] { typeof(object) });
							wrapper.ghostIsInstanceArrayMethod = typeBuilder.DefineMethod("IsInstanceArray", MethodAttributes.Public | MethodAttributes.Static, typeof(bool), new Type[] { typeof(object), typeof(int) });
							wrapper.ghostCastMethod = typeBuilder.DefineMethod("Cast", MethodAttributes.Public | MethodAttributes.Static, typeBuilder, new Type[] { typeof(object) });
							wrapper.ghostCastArrayMethod = typeBuilder.DefineMethod("CastArray", MethodAttributes.Public | MethodAttributes.Static, typeof(void), new Type[] { typeof(object), typeof(int) });
						}
						else
						{
							typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(wrapper.GetClassLoader().MangleTypeName(f.Name), typeAttribs);
						}
					}
				}
				else
				{
					typeAttribs |= TypeAttributes.Class;
					if(outer != null)
					{
						// LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
						// (and broken by J# as well), so we'll just ignore it too.
						typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs, wrapper.BaseTypeWrapper.TypeAsBaseType);
					}
					else
					{
						typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(wrapper.GetClassLoader().MangleTypeName(f.Name), typeAttribs, wrapper.BaseTypeWrapper.TypeAsBaseType);
					}
				}
				TypeWrapper[] interfaces = wrapper.Interfaces;
				for(int i = 0; i < interfaces.Length; i++)
				{
					// NOTE we're using TypeAsBaseType for the interfaces!
					typeBuilder.AddInterfaceImplementation(interfaces[i].TypeAsBaseType);
				}
				AttributeHelper.SetImplementsAttribute(typeBuilder, interfaces);
				if(JVM.IsStaticCompiler && classFile.DeprecatedAttribute)
				{
					AttributeHelper.SetDeprecatedAttribute(typeBuilder);
				}
				if(f.SourceFileAttribute != null && !JVM.NoStackTraceInfo)
				{
					AttributeHelper.SetSourceFile(typeBuilder, f.SourceFileAttribute);
				}
			}
			catch(Exception x)
			{
				if(typeBuilder != null)
				{
					JVM.CriticalFailure("Exception during critical part of JavaTypeImpl construction", x);
				}
				throw;
			}
		}

		private bool IsSideEffectFreeStaticInitializer(ClassFile.Method m)
		{
			if(m.ExceptionTable.Length != 0)
			{
				return false;
			}
			for(int i = 0; i < m.Instructions.Length; i++)
			{
				ByteCode bc = m.Instructions[i].OpCode;
				if(bc == ByteCode.__getstatic || bc == ByteCode.__putstatic)
				{
					ClassFile.ConstantPoolItemFieldref fld = classFile.GetFieldref(m.Instructions[i].Arg1);
					if(fld.Class != classFile.Name)
					{
						return false;
					}
					// don't allow getstatic to load non-primitive fields, because that would
					// cause the verifier to try to load the type
					if(bc == ByteCode.__getstatic && "L[".IndexOf(fld.Signature[0]) != -1)
					{
						return false;
					}
				}
				else if(bc == ByteCode.__areturn ||
					bc == ByteCode.__ireturn ||
					bc == ByteCode.__lreturn ||
					bc == ByteCode.__freturn ||
					bc == ByteCode.__dreturn)
				{
					return false;
				}
				else if(ByteCodeMetaData.CanThrowException(bc))
				{
					return false;
				}
			}
			// the method needs to be verifiable to be side effect free, since we already analysed it,
			// we know that the verifier won't try to load any types (which isn't allowed at this time)
			try
			{
				new MethodAnalyzer(wrapper, null, classFile, m, null);
				return true;
			}
			catch(VerifyError)
			{
				return false;
			}
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
								mw = new SmartCallMethodWrapper(wrapper, ifmethod.Name, ifmethod.Signature, null, null, null, Modifiers.Public | Modifiers.Abstract, MemberFlags.HideFromReflection, SimpleOpCode.Call, SimpleOpCode.Callvirt);
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

		private static bool CheckInnerOuterNames(string inner, string outer)
		{
			// do some sanity checks on the inner/outer class names
			return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.IndexOf('$', outer.Length + 1) == -1;
		}

		private static string GetInnerClassName(string outer, string inner)
		{
			Debug.Assert(CheckInnerOuterNames(inner, outer));
			return inner.Substring(outer.Length + 1);
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
						classObjectField = typeBuilder.DefineField("__<classObject>", typeof(object), FieldAttributes.Private | FieldAttributes.Static);
						AttributeHelper.HideFromJava((FieldBuilder)classObjectField);
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
					if(baseMethod.ReturnType.IsUnloadable || finishingForDebugSave)
					{
						if(!mw.ReturnType.IsUnloadable || (!baseMethod.ReturnType.IsUnloadable && finishingForDebugSave))
						{
							unloadableOverrideStub = true;
						}
					}
					else
					{
						throw new LinkageError("Loader constraints violated");
					}
				}
				TypeWrapper[] here = mw.GetParameters();
				TypeWrapper[] there = baseMethod.GetParameters();
				for(int i = 0; i < here.Length; i++)
				{
					if(here[i] != there[i])
					{
						if(there[i].IsUnloadable || finishingForDebugSave)
						{
							if(!here[i].IsUnloadable || (!there[i].IsUnloadable && finishingForDebugSave))
							{
								unloadableOverrideStub = true;
							}
						}
						else
						{
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
			Debug.Assert(fw != null);
			FieldBuilder field;
			ClassFile.Field fld = classFile.Fields[GetFieldIndex(fw)];
			string fieldName = fld.Name;
			TypeWrapper typeWrapper = fw.FieldTypeWrapper;
			Type type = typeWrapper.TypeAsSignatureType;
			bool setNameSig = typeWrapper.IsUnloadable || typeWrapper.IsGhostArray;
			if(setNameSig)
			{
				// TODO use clashtable
				// the field name is mangled here, because otherwise it can (theoretically)
				// conflict with another unloadable or object or ghost array field
				// (fields can be overloaded on type)
				fieldName += "/" + typeWrapper.Name;
			}
			FieldAttributes attribs = 0;
			MethodAttributes methodAttribs = 0;
			bool setModifiers = false;
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
				bool isWrappedFinal = false;
				if(fld.IsFinal && (fld.IsPublic || fld.IsProtected))
				{
					if(wrapper.IsInterface)
					{
						attribs |= FieldAttributes.InitOnly;
					}
					else
					{
						// NOTE public/protected blank final fields get converted into a read-only property with a private field
						// backing store
						// we used to make the field privatescope, but that really serves no purpose (and it hinders
						// serialization, which uses .NET reflection to get at the field)
						attribs &= ~FieldAttributes.FieldAccessMask;
						attribs |= FieldAttributes.Private;
						setModifiers = true;
						isWrappedFinal = true;
					}
				}
				else if(fld.IsFinal)
				{
					setModifiers = true;
				}
				field = typeBuilder.DefineField(fieldName, type, attribs);
				if(fld.IsTransient)
				{
					CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(typeof(NonSerializedAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
					field.SetCustomAttribute(transientAttrib);
				}
				if(fld.IsVolatile)
				{
					// TODO the field should be marked as modreq(IsVolatile), but Reflection.Emit doesn't have a way of doing this
					setModifiers = true;
				}
				// Instance fields can also have a ConstantValue attribute (and are inlined by the compiler),
				// and ikvmstub has to export them, so we have to add a custom attribute.
				if(constantValue != null)
				{
					CustomAttributeBuilder constantValueAttrib = new CustomAttributeBuilder(typeof(ConstantValueAttribute).GetConstructor(new Type[] { constantValue.GetType() }), new object[] { constantValue });
					field.SetCustomAttribute(constantValueAttrib);
				}
				if(isWrappedFinal)
				{
					methodAttribs |= MethodAttributes.SpecialName;
					// TODO we should ensure that the getter method name doesn't clash with an existing method
					MethodBuilder getter = typeBuilder.DefineMethod("get_" + fld.Name, methodAttribs, CallingConventions.Standard, type, Type.EmptyTypes);
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
					PropertyBuilder pb = typeBuilder.DefineProperty(fld.Name, PropertyAttributes.None, type, Type.EmptyTypes);
					pb.SetGetMethod(getter);
					((GetterFieldWrapper)fw).SetGetter(getter);
				}
			}
			// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
			// the Java modifiers
			if(setModifiers)
			{
				AttributeHelper.SetModifiers(field, fld.Modifiers);
			}
			if(setNameSig)
			{
				AttributeHelper.SetNameSig(field, fld.Name, fld.Signature);
			}
			if(JVM.IsStaticCompiler && fld.DeprecatedAttribute)
			{
				AttributeHelper.SetDeprecatedAttribute(field);
			}
			return field;
		}

		internal override DynamicImpl Finish(bool forDebugSave)
		{
			this.finishingForDebugSave = forDebugSave;
			// NOTE if a finish is triggered during static compilation phase 1, it cannot be handled properly,
			// so we bail out.
			// (this should only happen during compilation of classpath.dll and is most likely caused by a bug somewhere)
			if(JVM.IsStaticCompilerPhase1)
			{
				JVM.CriticalFailure("Finish triggered during phase 1 of compilation.", null);
				return null;
			}
			if(wrapper.BaseTypeWrapper != null)
			{
				wrapper.BaseTypeWrapper.Finish(forDebugSave);
			}
			if(outerClassWrapper != null)
			{
				outerClassWrapper.Finish(forDebugSave);
			}
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
				wrapper.Interfaces[i].Finish(forDebugSave);
			}
			// make sure all classes are loaded, before we start finishing the type. During finishing, we
			// may not run any Java code, because that might result in a request to finish the type that we
			// are in the process of finishing, and this would be a problem.
			classFile.Link(wrapper, classCache);
			for(int i = 0; i < fields.Length; i++)
			{
				fields[i].Link();
			}
			for(int i = 0; i < methods.Length; i++)
			{
				methods[i].Link();
			}
			// it is possible that the loading of the referenced classes triggered a finish of us,
			// if that happens, we just return
			if(finishedType != null)
			{
				return finishedType;
			}
			Profiler.Enter("JavaTypeImpl.Finish.Core");
			try
			{
				Debug.Assert(!finishing);
				finishing = true;
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
								AttributeHelper.SetInnerClass(typeBuilder,
									classFile.GetConstantPoolClass(innerclasses[i].innerClass),
									classFile.GetConstantPoolClass(innerclasses[i].outerClass),
									innerclasses[i].name == 0 ? null : classFile.GetConstantPoolUtf8String(innerclasses[i].name),
									innerclasses[i].accessFlags);
							}
						}
					}
					innerClassesTypeWrappers = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
				}
				if(typeBuilderGhostInterface != null)
				{
					// TODO consider adding methods from base interface and java.lang.Object as well
					for(int i = 0; i < methods.Length; i++)
					{
						// skip <clinit>
						if(!methods[i].IsStatic)
						{
							TypeWrapper[] args = methods[i].GetParameters();
							MethodBuilder stub = typeBuilder.DefineMethod(methods[i].Name, MethodAttributes.Public, methods[i].ReturnTypeForDefineMethod, methods[i].GetParametersForDefineMethod());
							AttributeHelper.SetModifiers(stub, methods[i].Modifiers);
							ILGenerator ilgen = stub.GetILGenerator();
							Label end = ilgen.DefineLabel();
							TypeWrapper[] implementers = GetGhostImplementers(wrapper);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
							ilgen.Emit(OpCodes.Dup);
							ilgen.Emit(OpCodes.Isinst, wrapper.TypeAsBaseType);
							Label label = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Brfalse_S, label);
							ilgen.Emit(OpCodes.Castclass, wrapper.TypeAsBaseType);
							for(int k = 0; k < args.Length; k++)
							{
								ilgen.Emit(OpCodes.Ldarg_S, (byte)(k + 1));
							}
							ilgen.Emit(OpCodes.Callvirt, (MethodInfo)methods[i].GetMethod());
							ilgen.Emit(OpCodes.Br, end);
							ilgen.MarkLabel(label);
							for(int j = 0; j < implementers.Length; j++)
							{
								ilgen.Emit(OpCodes.Dup);
								ilgen.Emit(OpCodes.Isinst, implementers[j].TypeAsTBD);
								label = ilgen.DefineLabel();
								ilgen.Emit(OpCodes.Brfalse_S, label);
								ilgen.Emit(OpCodes.Castclass, implementers[j].TypeAsTBD);
								for(int k = 0; k < args.Length; k++)
								{
									ilgen.Emit(OpCodes.Ldarg_S, (byte)(k + 1));
								}
								MethodWrapper mw = implementers[j].GetMethodWrapper(methods[i].Name, methods[i].Signature, true);
								mw.EmitCallvirt(ilgen);
								ilgen.Emit(OpCodes.Br, end);
								ilgen.MarkLabel(label);
							}
							// we need to do a null check (null fails all the isinst checks)
							EmitHelper.NullCheck(ilgen);
							EmitHelper.Throw(ilgen, "java.lang.IncompatibleClassChangeError", wrapper.Name);
							ilgen.MarkLabel(end);
							ilgen.Emit(OpCodes.Ret);
						}
					}
					// HACK create a scope to enable reuse of "implementers" name
					if(true)
					{
						MethodBuilder mb;
						ILGenerator ilgen;
						LocalBuilder local;
						// add implicit conversions for all the ghost implementers
						TypeWrapper[] implementers = GetGhostImplementers(wrapper);
						for(int i = 0; i < implementers.Length; i++)
						{
							mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, wrapper.TypeAsSignatureType, new Type[] { implementers[i].TypeAsSignatureType });
							AttributeHelper.HideFromJava(mb);
							ilgen = mb.GetILGenerator();
							local = ilgen.DeclareLocal(wrapper.TypeAsSignatureType);
							ilgen.Emit(OpCodes.Ldloca, local);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Stfld, wrapper.GhostRefField);
							ilgen.Emit(OpCodes.Ldloca, local);
							ilgen.Emit(OpCodes.Ldobj, wrapper.TypeAsSignatureType);			
							ilgen.Emit(OpCodes.Ret);
						}
						// Implement the "IsInstance" method
						mb = wrapper.ghostIsInstanceMethod;
						AttributeHelper.HideFromJava(mb);
						ilgen = mb.GetILGenerator();
						Label end = ilgen.DefineLabel();
						for(int i = 0; i < implementers.Length; i++)
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
							Label label = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Brfalse_S, label);
							ilgen.Emit(OpCodes.Ldc_I4_1);
							ilgen.Emit(OpCodes.Br, end);
							ilgen.MarkLabel(label);
						}
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, wrapper.TypeAsBaseType);
						ilgen.Emit(OpCodes.Ldnull);
						ilgen.Emit(OpCodes.Cgt_Un);
						ilgen.MarkLabel(end);
						ilgen.Emit(OpCodes.Ret);
						// Implement the "IsInstanceArray" method
						mb = wrapper.ghostIsInstanceArrayMethod;
						AttributeHelper.HideFromJava(mb);
						ilgen = mb.GetILGenerator();
						LocalBuilder localType = ilgen.DeclareLocal(typeof(Type));
						ilgen.Emit(OpCodes.Ldarg_0);
						Label skip = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brtrue_S, skip);
						ilgen.Emit(OpCodes.Ldc_I4_0);
						ilgen.Emit(OpCodes.Ret);
						ilgen.MarkLabel(skip);
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Call, typeof(object).GetMethod("GetType"));
						ilgen.Emit(OpCodes.Stloc, localType);
						skip = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Br_S, skip);
						Label iter = ilgen.DefineLabel();
						ilgen.MarkLabel(iter);
						ilgen.Emit(OpCodes.Ldarg_1);
						ilgen.Emit(OpCodes.Ldc_I4_1);
						ilgen.Emit(OpCodes.Sub);
						ilgen.Emit(OpCodes.Starg_S, (byte)1);
						ilgen.Emit(OpCodes.Ldloc, localType);
						ilgen.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("GetElementType"));
						ilgen.Emit(OpCodes.Stloc, localType);
						ilgen.MarkLabel(skip);
						ilgen.Emit(OpCodes.Ldloc, localType);
						ilgen.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("get_IsArray"));
						ilgen.Emit(OpCodes.Brtrue_S, iter);
						ilgen.Emit(OpCodes.Ldarg_1);
						skip = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brfalse_S, skip);
						ilgen.Emit(OpCodes.Ldc_I4_0);
						ilgen.Emit(OpCodes.Ret);
						ilgen.MarkLabel(skip);
						for(int i = 0; i < implementers.Length; i++)
						{
							ilgen.Emit(OpCodes.Ldtoken, implementers[i].TypeAsTBD);
							ilgen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
							ilgen.Emit(OpCodes.Ldloc, localType);
							ilgen.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("IsAssignableFrom"));
							Label label = ilgen.DefineLabel();
							ilgen.Emit(OpCodes.Brfalse_S, label);
							ilgen.Emit(OpCodes.Ldc_I4_1);
							ilgen.Emit(OpCodes.Ret);
							ilgen.MarkLabel(label);
						}
						ilgen.Emit(OpCodes.Ldtoken, wrapper.TypeAsBaseType);
						ilgen.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
						ilgen.Emit(OpCodes.Ldloc, localType);
						ilgen.Emit(OpCodes.Callvirt, typeof(Type).GetMethod("IsAssignableFrom"));
						ilgen.Emit(OpCodes.Ret);
						
						// Implement the "Cast" method
						mb = wrapper.ghostCastMethod;
						AttributeHelper.HideFromJava(mb);
						ilgen = mb.GetILGenerator();
						end = ilgen.DefineLabel();
						for(int i = 0; i < implementers.Length; i++)
						{
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
							ilgen.Emit(OpCodes.Brtrue, end);
						}
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Castclass, wrapper.TypeAsBaseType);
						ilgen.Emit(OpCodes.Pop);
						ilgen.MarkLabel(end);
						local = ilgen.DeclareLocal(wrapper.TypeAsSignatureType);
						ilgen.Emit(OpCodes.Ldloca, local);
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Stfld, wrapper.ghostRefField);
						ilgen.Emit(OpCodes.Ldloca, local);
						ilgen.Emit(OpCodes.Ldobj, wrapper.TypeAsSignatureType);	
						ilgen.Emit(OpCodes.Ret);
						// Add "ToObject" methods
						mb = typeBuilder.DefineMethod("ToObject", MethodAttributes.Public, typeof(object), Type.EmptyTypes);
						AttributeHelper.HideFromJava(mb);
						ilgen = mb.GetILGenerator();
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
						ilgen.Emit(OpCodes.Ret);

						// Implement the "CastArray" method
						// NOTE unlike "Cast" this doesn't return anything, it just throws a ClassCastException if the
						// cast is unsuccessful. Also, because of the complexity of this test, we call IsInstanceArray
						// instead of reimplementing the check here.
						mb = wrapper.ghostCastArrayMethod;
						AttributeHelper.HideFromJava(mb);
						ilgen = mb.GetILGenerator();
						end = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Brfalse_S, end);
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Ldarg_1);
						ilgen.Emit(OpCodes.Call, wrapper.ghostIsInstanceArrayMethod);
						ilgen.Emit(OpCodes.Brtrue_S, end);
						EmitHelper.Throw(ilgen, "java.lang.ClassCastException");
						ilgen.MarkLabel(end);
						ilgen.Emit(OpCodes.Ret);
					}
				}
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
							if(mi != null && mi.IsAbstract && wrapper.GetMethodWrapper(mw.Name, mw.Signature, true).IsAbstract)
							{
								// NOTE in Sun's JRE 1.4.1 this method cannot be overridden by subclasses,
								// but I think this is a bug, so we'll support it anyway.
								MethodBuilder mb = typeBuilder.DefineMethod(mi.Name, mi.Attributes & ~(MethodAttributes.Abstract|MethodAttributes.NewSlot), CallingConventions.Standard, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
								AttributeHelper.HideFromJava(mb);
								EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", wrapper.Name + "." + mw.Name + mw.Signature);
							}
						}
						parent = parent.BaseTypeWrapper;
					}
				}
				string verifyError = null;
				bool basehasclinit = wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasStaticInitializer;
				bool hasclinit = false;
				for(int i = 0; i < classFile.Methods.Length; i++)
				{
					ClassFile.Method m = classFile.Methods[i];
					MethodBase mb = methods[i].GetMethod();
					if(mb is ConstructorBuilder)
					{
						ILGenerator ilGenerator = ((ConstructorBuilder)mb).GetILGenerator();
						Tracer.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
						if(basehasclinit && m.IsClassInitializer && !classFile.IsInterface)
						{
							hasclinit = true;
							// before we call the base class initializer, we need to set the non-final static ConstantValue fields
							EmitConstantValueInitialization(ilGenerator);
							EmitHelper.RunClassConstructor(ilGenerator, Type.BaseType);
						}
						Compiler.Compile(wrapper, methods[i], classFile, m, ilGenerator, ref verifyError);
					}
					else
					{
						if(m.IsAbstract)
						{
							// NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
							// the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
							if(!classFile.IsAbstract)
							{
								ILGenerator ilGenerator = ((MethodBuilder)mb).GetILGenerator();
								Tracer.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
								EmitHelper.Throw(ilGenerator, "java.lang.AbstractMethodError", classFile.Name + "." + m.Name + m.Signature);
							}
						}
						else if(m.IsNative)
						{
							Profiler.Enter("JavaTypeImpl.Finish.Native");
							try
							{
								ILGenerator ilGenerator = ((MethodBuilder)mb).GetILGenerator();
								Tracer.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
								// do we have a native implementation in map.xml?
								if(mapxml != null)
								{
									CodeEmitter opcodes = (CodeEmitter)mapxml[classFile.Name + "." + m.Name + m.Signature];
									if(opcodes != null)
									{
										opcodes.Emit(ilGenerator);
										continue;
									}
								}
								// see if there exists a IKVM.NativeCode class for this type
								Type nativeCodeType = Type.GetType("IKVM.NativeCode." + classFile.Name.Replace('$', '+'));
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
									if(JVM.NoJniStubs)
									{
										// since NoJniStubs can only be set when we're statically compiling, it is safe to use the "compiler" trace switch
										Tracer.Warning(Tracer.Compiler, "Native method not implemented: {0}.{1}.{2}", classFile.Name, m.Name, m.Signature);
										EmitHelper.Throw(ilGenerator, "java.lang.UnsatisfiedLinkError", "Native method not implemented (compiled with -nojni): " + classFile.Name + "." + m.Name + m.Signature);
									}
									else
									{
										if(ClassLoaderWrapper.IsSaveDebugImage)
										{
											JniProxyBuilder.Generate(ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args);
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
							Tracer.EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
							if(mapxml != null)
							{
								CodeEmitter opcodes = (CodeEmitter)mapxml[classFile.Name + "." + m.Name + m.Signature];
								if(opcodes != null)
								{
									opcodes.Emit(ilGenerator);
									continue;
								}
							}
							bool nonleaf = false;
							Compiler.Compile(wrapper, methods[i], classFile, m, ilGenerator, ref verifyError, ref nonleaf);
							if(nonleaf)
							{
								mbld.SetImplementationFlags(mbld.GetMethodImplementationFlags() | MethodImplAttributes.NoInlining);
							}
						}
					}
				}
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
								EmitHelper.RunClassConstructor(ilGenerator, Type.BaseType);
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
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
						// if we implement a ghost interface, add an implicit conversion to the ghost reference value type
						// TODO do this for indirectly implemented interfaces (interfaces implemented by interfaces) as well
						if(interfaces[i].IsGhost)
						{
							MethodBuilder mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, interfaces[i].TypeAsSignatureType, new Type[] { wrapper.TypeAsSignatureType });
							AttributeHelper.HideFromJava(mb);
							ILGenerator ilgen = mb.GetILGenerator();
							LocalBuilder local = ilgen.DeclareLocal(interfaces[i].TypeAsSignatureType);
							ilgen.Emit(OpCodes.Ldloca, local);
							ilgen.Emit(OpCodes.Ldarg_0);
							ilgen.Emit(OpCodes.Stfld, interfaces[i].GhostRefField);
							ilgen.Emit(OpCodes.Ldloca, local);
							ilgen.Emit(OpCodes.Ldobj, interfaces[i].TypeAsSignatureType);
							ilgen.Emit(OpCodes.Ret);
						}
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
					foreach(MethodWrapper mw in methods)
					{
						if(mw.Name != "<init>" && !mw.IsStatic && !mw.IsPrivate)
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

				// If we're an interface that has public/protected fields, we create an inner class
				// to expose these fields to C# (which stubbornly refuses to see fields in interfaces).
				TypeBuilder tbFields = null;
				if(JVM.IsStaticCompiler && classFile.IsInterface && !wrapper.IsGhost && classFile.Fields.Length > 0)
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
				if(mapxml != null)
				{
					IKVM.Internal.MapXml.Class clazz = (IKVM.Internal.MapXml.Class)mapxml[classFile.Name];
					if(clazz != null)
					{
						if(clazz.Attributes != null)
						{
							PublishAttributes(clazz);
						}
						if(clazz.Properties != null)
						{
							PublishProperties(clazz);
						}
						if(clazz.Fields != null)
						{
							foreach(IKVM.Internal.MapXml.Field field in clazz.Fields)
							{
								if(field.Attributes != null)
								{
									foreach(FieldWrapper fw in fields)
									{
										if(fw.Name == field.Name && fw.Signature == field.Sig)
										{
											FieldBuilder fb = fw.GetField() as FieldBuilder;
											if(fb != null)
											{
												foreach(IKVM.Internal.MapXml.Attribute attr in field.Attributes)
												{
													fb.SetCustomAttribute(AttributeHelper.CreateCustomAttribute(attr));
												}
											}
										}
									}
								}
							}
						}
						if(clazz.Constructors != null)
						{
							foreach(IKVM.Internal.MapXml.Constructor constructor in clazz.Constructors)
							{
								if(constructor.Attributes != null)
								{
									foreach(MethodWrapper mw in methods)
									{
										if(mw.Name == "<init>" && mw.Signature == constructor.Sig)
										{
											ConstructorBuilder mb = mw.GetMethod() as ConstructorBuilder;
											if(mb != null)
											{
												foreach(IKVM.Internal.MapXml.Attribute attr in constructor.Attributes)
												{
													mb.SetCustomAttribute(AttributeHelper.CreateCustomAttribute(attr));
												}
											}
										}
									}
								}
							}
						}
						if(clazz.Methods != null)
						{
							foreach(IKVM.Internal.MapXml.Method method in clazz.Methods)
							{
								if(method.Attributes != null)
								{
									foreach(MethodWrapper mw in methods)
									{
										if(mw.Name == method.Name && mw.Signature == method.Sig)
										{
											MethodBuilder mb = mw.GetMethod() as MethodBuilder;
											if(mb != null)
											{
												foreach(IKVM.Internal.MapXml.Attribute attr in method.Attributes)
												{
													mb.SetCustomAttribute(AttributeHelper.CreateCustomAttribute(attr));
												}
											}
										}
									}
								}
							}
						}
					}
				}

				Type type;
				Profiler.Enter("TypeBuilder.CreateType");
				try
				{
					type = typeBuilder.CreateType();
					if(tbFields != null)
					{
						tbFields.CreateType();
					}
					BakedTypeCleanupHack.Process(wrapper);
				}
				finally
				{
					Profiler.Leave("TypeBuilder.CreateType");
				}
				ClassLoaderWrapper.SetWrapperForType(type, wrapper);
				if(verifyError != null)
				{
					return new DoomedTypeImpl(verifyError);
				}
				else
				{
					finishedType = new FinishedTypeImpl(type, typeBuilderGhostInterface != null ? typeBuilderGhostInterface.CreateType() : null, innerClassesTypeWrappers, declaringTypeWrapper, this.ReflectiveModifiers);
					return finishedType;
				}
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

		private static MethodAttributes GetPropertyMethodAttributes(MethodWrapper mw, bool final)
		{
			MethodAttributes attribs = (MethodAttributes)0;
			if(mw.IsStatic)
			{
				attribs |= MethodAttributes.Static;
			}
			else
			{
				// NOTE in order for IntelliSense to consider the property a "real" property,
				// the getter and setter methods need to have substantially the same method attributes,
				// so we may need to look at our peer to determine whether we should be final
				// or not (and vice versa).
				attribs |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
				if(final)
				{
					attribs |= MethodAttributes.Final;
				}
			}
			// TODO what happens if accessibility doesn't match our peer?
			if(mw.IsPublic)
			{
				attribs |= MethodAttributes.Public;
			}
			else if(mw.IsProtected)
			{
				attribs |= MethodAttributes.FamORAssem;
			}
			else if(mw.IsPrivate)
			{
				attribs |= MethodAttributes.Private;
			}
			else
			{
				attribs |= MethodAttributes.Assembly;
			}
			return attribs;
		}

		private static bool CheckPropertyArgs(Type[] args1, Type[] args2)
		{
			if(args1.Length == args2.Length)
			{
				for(int i = 0; i < args1.Length; i++)
				{
					if(args1[i] != args2[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		private void PublishAttributes(IKVM.Internal.MapXml.Class clazz)
		{
			foreach(IKVM.Internal.MapXml.Attribute attr in clazz.Attributes)
			{
				typeBuilder.SetCustomAttribute(AttributeHelper.CreateCustomAttribute(attr));
			}
		}

		private void PublishProperties(IKVM.Internal.MapXml.Class clazz)
		{
			foreach(IKVM.Internal.MapXml.Property prop in clazz.Properties)
			{
				TypeWrapper typeWrapper = ClassFile.RetTypeWrapperFromSig(wrapper.GetClassLoader(), classCache, prop.Sig);
				TypeWrapper[] propargs = ClassFile.ArgTypeWrapperListFromSig(wrapper.GetClassLoader(), classCache, prop.Sig);
				Type[] indexer = new Type[propargs.Length];
				for(int i = 0; i < propargs.Length; i++)
				{
					indexer[i] = propargs[i].TypeAsSignatureType;
				}
				PropertyBuilder propbuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.None, typeWrapper.TypeAsSignatureType, indexer);
				if(prop.Attributes != null)
				{
					foreach(IKVM.Internal.MapXml.Attribute attr in prop.Attributes)
					{
						propbuilder.SetCustomAttribute(AttributeHelper.CreateCustomAttribute(attr));
					}
				}
				MethodWrapper getter = null;
				MethodWrapper setter = null;
				if(prop.getter != null)
				{
					getter = wrapper.GetMethodWrapper(prop.getter.Name, prop.getter.Sig, true);
					if(getter == null)
					{
						Console.Error.WriteLine("Warning: getter not found for {0}::{1}", clazz.Name, prop.Name);
					}
				}
				if(prop.setter != null)
				{
					setter = wrapper.GetMethodWrapper(prop.setter.Name, prop.setter.Sig, true);
					if(setter == null)
					{
						Console.Error.WriteLine("Warning: setter not found for {0}::{1}", clazz.Name, prop.Name);
					}
				}
				bool final = (getter != null && getter.IsFinal) || (setter != null && setter.IsFinal);
				if(getter != null)
				{
					MethodWrapper mw = getter;
					if(!CheckPropertyArgs(mw.GetParametersForDefineMethod(), indexer) || mw.ReturnType != typeWrapper)
					{
						Console.Error.WriteLine("Warning: ignoring invalid property getter for {0}::{1}", clazz.Name, prop.Name);
					}
					else
					{
						MethodBuilder mb = mw.GetMethod() as MethodBuilder;
						if(mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
						{
							mb = typeBuilder.DefineMethod(GenerateUniqueMethodName("get_" + prop.Name, mw), GetPropertyMethodAttributes(mw, final), typeWrapper.TypeAsSignatureType, indexer);
							AttributeHelper.HideFromJava(mb);
							ILGenerator ilgen = new CountingILGenerator(mb.GetILGenerator());
							if(mw.IsStatic)
							{
								for(int i = 0; i < indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, i);
								}
								mw.EmitCall(ilgen);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i < indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, i + 1);
								}
								mw.EmitCallvirt(ilgen);
							}
							ilgen.Emit(OpCodes.Ret);
						}
						propbuilder.SetGetMethod(mb);
					}
				}
				if(setter != null)
				{
					MethodWrapper mw = setter;
					Type[] args = new Type[indexer.Length + 1];
					indexer.CopyTo(args, 0);
					args[args.Length - 1] = typeWrapper.TypeAsSignatureType;
					if(!CheckPropertyArgs(args, mw.GetParametersForDefineMethod()))
					{
						Console.Error.WriteLine("Warning: ignoring invalid property setter for {0}::{1}", clazz.Name, prop.Name);
					}
					else
					{
						MethodBuilder mb = mw.GetMethod() as MethodBuilder;
						if(mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
						{
							mb = typeBuilder.DefineMethod(GenerateUniqueMethodName("set_" + prop.Name, mw), GetPropertyMethodAttributes(mw, final), mw.ReturnTypeForDefineMethod, args);
							AttributeHelper.HideFromJava(mb);
							ILGenerator ilgen = new CountingILGenerator(mb.GetILGenerator());
							if(mw.IsStatic)
							{
								for(int i = 0; i <= indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, i);
								}
								mw.EmitCall(ilgen);
							}
							else
							{
								ilgen.Emit(OpCodes.Ldarg_0);
								for(int i = 0; i <= indexer.Length; i++)
								{
									ilgen.Emit(OpCodes.Ldarg, i + 1);
								}
								mw.EmitCallvirt(ilgen);
							}
							ilgen.Emit(OpCodes.Ret);
						}
						propbuilder.SetSetMethod(mb);
					}
				}
			}
		}

		internal class JniProxyBuilder
		{
			private static ModuleBuilder mod;
			private static int count;

			static JniProxyBuilder()
			{
				mod = ((AssemblyBuilder)ClassLoaderWrapper.GetBootstrapClassLoader().ModuleBuilder.Assembly).DefineDynamicModule("jniproxy.dll", "jniproxy.dll");
			}

			private static string Cleanup(string n)
			{
				n = n.Replace('\\', '_');
				n = n.Replace('[', '_');
				n = n.Replace(']', '_');
				n = n.Replace('+', '_');
				n = n.Replace(',', '_');
				return n;
			}

			internal static void Generate(ILGenerator ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args)
			{
				TypeBuilder tb = mod.DefineType("class" + (count++), TypeAttributes.Public | TypeAttributes.Class);
				int instance = m.IsStatic ? 0 : 1;
				Type[] argTypes = new Type[args.Length + instance + 1];
				if(instance != 0)
				{
					argTypes[0] = wrapper.TypeAsSignatureType;
				}
				for(int i = 0; i < args.Length; i++)
				{
					argTypes[i + instance] = args[i].TypeAsSignatureType;
				}
				argTypes[argTypes.Length - 1] = typeof(RuntimeMethodHandle);
				MethodBuilder mb = tb.DefineMethod("method", MethodAttributes.Public | MethodAttributes.Static, mw.ReturnType.TypeAsSignatureType, argTypes);
				JniBuilder.Generate(mb.GetILGenerator(), wrapper, mw, tb, classFile, m, args, true);
				for(int i = 0; i < argTypes.Length - 1; i++)
				{
					ilGenerator.Emit(OpCodes.Ldarg, (short)i);
				}
				ilGenerator.Emit(OpCodes.Ldtoken, (MethodInfo)mw.GetMethod());
				ilGenerator.Emit(OpCodes.Call, mb);
				ilGenerator.Emit(OpCodes.Ret);
				Type type = tb.CreateType();
				// HACK since we're creating a type in the dynamic assembly, we must also register its wrapper,
				// because the ClassLoaderWrapper assumes that all dynamic types are in its hashtable,
				// but note that this only registers it for reverse lookup (from Type -> TypeWrapper), this
				// is necessary to make stack walking work.
				ClassLoaderWrapper.SetWrapperForType(type, CompiledTypeWrapper.newInstance(type.FullName, type));
			}
		}

		private class JniBuilder
		{
			private static readonly Type localRefStructType = typeof(IKVM.Runtime.JNI.Frame);
			private static readonly MethodInfo jniFuncPtrMethod = localRefStructType.GetMethod("GetFuncPtr");
			private static readonly MethodInfo enterLocalRefStruct = localRefStructType.GetMethod("Enter");
			private static readonly MethodInfo leaveLocalRefStruct = localRefStructType.GetMethod("Leave");
			private static readonly MethodInfo makeLocalRef = localRefStructType.GetMethod("MakeLocalRef");
			private static readonly MethodInfo unwrapLocalRef = localRefStructType.GetMethod("UnwrapLocalRef");
			private static readonly MethodInfo getClassFromTypeHandle = typeof(ByteCodeHelper).GetMethod("GetClassFromTypeHandle");
			private static readonly MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }, null);
			private static readonly MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter", new Type[] { typeof(object) });
			private static readonly MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit", new Type[] { typeof(object) });

			internal static void Generate(ILGenerator ilGenerator, DynamicTypeWrapper wrapper, MethodWrapper mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, TypeWrapper[] args, bool thruProxy)
			{
				LocalBuilder syncObject = null;
				FieldInfo classObjectField;
				if(thruProxy)
				{
					classObjectField = typeBuilder.DefineField("__<classObject>", typeof(object), FieldAttributes.Static | FieldAttributes.Private);
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
					ilGenerator.Emit(OpCodes.Call, getClassFromTypeHandle);
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
				FieldBuilder methodPtr = typeBuilder.DefineField(JNI.METHOD_PTR_FIELD_PREFIX + m.Name + sig, typeof(IntPtr), FieldAttributes.Static | FieldAttributes.PrivateScope);
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
				Label tryBlock = ilGenerator.BeginExceptionBlock();
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
					ilGenerator.Emit(OpCodes.Call, getClassFromTypeHandle);
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
						if(methods[i].IsLinked && methods[i].Name != "<init>")
						{
							string key = GenerateClashKey("method", methods[i].RealName, methods[i].ReturnTypeForDefineMethod, methods[i].GetParametersForDefineMethod());
							memberclashtable.Add(key, key);
						}
					}
				}
			}
		}

		private string GenerateClashKey(string type, string name, Type retOrFieldType, Type[] args)
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
			if(!classFile.IsFinal && !classFile.IsInterface && wrapper.HasStaticInitializer)
			{
				// We create a field that the derived classes can access in their .cctor to trigger our .cctor
				// (previously we used RuntimeHelpers.RunClassConstructor, but that is slow and requires additional privileges)
				FieldBuilder field = typeBuilder.DefineField("__<clinit>", typeof(int), FieldAttributes.SpecialName | FieldAttributes.Family | FieldAttributes.Static);
				AttributeHelper.HideFromJava(field);
			}
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
				// TODO we need to handle static methods (duh!)
				MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
				if(baseMethod == null)
				{
					return null;
				}
				// here are the complex rules for determining whether this method overrides the method we found
				// RULE 1: final methods may not be overridden
				if(baseMethod.IsFinal && !baseMethod.IsPrivate)
				{
					throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + tw.Name + " is overriden in " + wrapper.Name);
				}
				// RULE 2: public & protected methods can be overridden (package methods are handled by RULE 4)
				// (by public, protected & *package* methods [even if they are in a different package])
				if(baseMethod.IsPublic || baseMethod.IsProtected)
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
				// RULE 3: private methods are ignored
				if(!baseMethod.IsPrivate)
				{
					// RULE 4: package methods can only be overridden in the same package
					if(baseMethod.DeclaringType.IsInSamePackageAs(wrapper))
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

		private string GenerateUniqueMethodName(string basename, MethodWrapper mw)
		{
			string name = basename;
			string key = GenerateClashKey("method", name, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
			UpdateClashTable();
			lock(memberclashtable.SyncRoot)
			{
				for(int clashcount = 0; memberclashtable.ContainsKey(key); clashcount++)
				{
					name = basename + "_" + clashcount;
					key = GenerateClashKey("method", name, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod());
				}
				memberclashtable.Add(key, key);
			}
			return name;
		}

		private MethodBase GenerateMethod(int index, bool unloadableOverrideStub)
		{
			methods[index].AssertLinked();
			Profiler.Enter("JavaTypeImpl.GenerateMethod");
			try
			{
				if(index >= classFile.Methods.Length)
				{
					// We're a Miranda method
					Debug.Assert(baseMethods[index].DeclaringType.IsInterface);
					string name = GenerateUniqueMethodName(methods[index].Name, baseMethods[index]);
					// TODO if the interface is not public, we probably shouldn't make the Miranda method public
					MethodBuilder mb = typeBuilder.DefineMethod(methods[index].Name, MethodAttributes.NewSlot | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract | MethodAttributes.CheckAccessOnOverride, baseMethods[index].ReturnTypeForDefineMethod, baseMethods[index].GetParametersForDefineMethod());
					AttributeHelper.MirandaMethod(mb);
					if(unloadableOverrideStub || name != methods[index].Name)
					{
						// instead of creating an override stub, we created the Miranda method with the proper signature and
						// decorate it with a NameSigAttribute that contains the real signature
						AttributeHelper.SetNameSig(mb, methods[index].Name, methods[index].Signature);
					}
					// if we changed the name or if the interface method name is remapped, we need to add an explicit methodoverride.
					if(name != baseMethods[index].RealName)
					{
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethods[index].GetMethod());
					}
					return mb;
				}
				ClassFile.Method m = classFile.Methods[index];
				MethodBase method;
				bool setNameSig = methods[index].ReturnType.IsUnloadable || methods[index].ReturnType.IsGhostArray;
				foreach(TypeWrapper tw in methods[index].GetParameters())
				{
					setNameSig |= tw.IsUnloadable || tw.IsGhostArray;
				}
				bool setModifiers = m.IsNative;
				MethodAttributes attribs = 0;
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
				if(m.Name == "<init>")
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
					if(JVM.IsStaticCompiler || ClassLoaderWrapper.IsSaveDebugImage)
					{
						AddParameterNames(method, m);
					}
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
							attribs |= MethodAttributes.Abstract;
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
					}
					else if(!m.IsPrivate)
					{
						attribs |= MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
					}
					string name = m.Name;
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
								(baseMethod.IsFamily && !m.IsPublic && !m.IsProtected) ||
								(!m.IsPublic && !m.IsProtected && !baseMce.DeclaringType.IsInSamePackageAs(wrapper)))
							{
								attribs &= ~MethodAttributes.MemberAccessMask;
								attribs |= baseMethod.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
								setModifiers = true;
							}
						}
					}
					MethodBuilder mb;
					if(typeBuilderGhostInterface != null)
					{
						mb = typeBuilderGhostInterface.DefineMethod(name, attribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
					}
					else
					{
						bool needFinalize = false;
						bool needDispatch = false;
						if(baseMce != null && m.Name == "finalize" && m.Signature == "()V")
						{
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
								Type baseFinalizeType = typeBuilder.BaseType.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null).DeclaringType;
								if(baseFinalizeType == typeof(object))
								{
									needDispatch = true;
								}
							}
							else
							{
								// One of our base classes already has a finalize method, but it may have been an empty
								// method so that the hookup to the real Finalize was optimized away, we need to check
								// for that.
								Type baseFinalizeType = typeBuilder.BaseType.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null).DeclaringType;
								if(baseFinalizeType == typeof(object))
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
						if(unloadableOverrideStub)
						{
							attribs |= MethodAttributes.NewSlot;
						}
						if(setNameSig || memberclashtable != null)
						{
							name = GenerateUniqueMethodName(name, methods[index]);
						}
						mb = typeBuilder.DefineMethod(name, attribs, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
						if(unloadableOverrideStub)
						{
							GenerateUnloadableOverrideStub(baseMce, mb, methods[index].ReturnTypeForDefineMethod, methods[index].GetParametersForDefineMethod());
						}
						else if(baseMce != null && (explicitOverride || baseMce.RealName != name) && !needFinalize)
						{
							// assert that the method we're overriding is in fact virtual and not final!
							Debug.Assert(baseMce.GetMethod().IsVirtual && !baseMce.GetMethod().IsFinal);
							typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMce.GetMethod());
						}
						// if we're overriding java.lang.Object.finalize we need to emit a stub to override System.Object.Finalize,
						// or if we're subclassing a non-Java class that has a Finalize method, we need a new Finalize override
						if(needFinalize)
						{
							MethodInfo baseFinalize = typeBuilder.BaseType.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
							MethodAttributes attr = MethodAttributes.Virtual;
							// make sure we don't reduce accessibility
							attr |= baseFinalize.IsPublic ? MethodAttributes.Public : MethodAttributes.Family;
							if(m.IsFinal)
							{
								attr |= MethodAttributes.Final;
							}
							// TODO if the Java class also defines a Finalize() method, we need to name the stub differently
							// (and make it effectively appear hidden by the class's Finalize method)
							MethodBuilder finalize = typeBuilder.DefineMethod("Finalize", attr, CallingConventions.Standard, typeof(void), Type.EmptyTypes);
							AttributeHelper.HideFromJava(finalize);
							ILGenerator ilgen = finalize.GetILGenerator();
							ilgen.Emit(OpCodes.Call, typeof(ByteCodeHelper).GetMethod("SkipFinalizer"));
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
					}
					if(JVM.IsStaticCompiler || ClassLoaderWrapper.IsSaveDebugImage)
					{
						AddParameterNames(mb, m);
						if(setModifiers)
						{
							AttributeHelper.SetModifiers(mb, m.Modifiers);
						}
					}
					method = mb;
				}
				string[] exceptions = m.ExceptionsAttribute;
				methods[index].SetDeclaredExceptions(exceptions);
				if(JVM.IsStaticCompiler || ClassLoaderWrapper.IsSaveDebugImage)
				{
					AttributeHelper.SetThrowsAttribute(method, exceptions);
					if(m.DeprecatedAttribute)
					{
						AttributeHelper.SetDeprecatedAttribute(method);
					}
					if(setNameSig)
					{
						AttributeHelper.SetNameSig(method, m.Name, m.Signature);
					}
				}
				return method;
			}
			finally
			{
				Profiler.Leave("JavaTypeImpl.GenerateMethod");
			}
		}

		private static ParameterBuilder[] AddParameterNames(MethodBase mb, ClassFile.Method m)
		{
			ClassFile.Method.LocalVariableTableEntry[] localVars = m.LocalVariableTableAttribute;
			if(localVars != null)
			{
				int bias = 1;
				if(m.IsStatic)
				{
					bias = 0;
				}
				ParameterBuilder[] parameterBuilders = new ParameterBuilder[m.ArgMap.Length - bias];
				for(int i = bias; i < m.ArgMap.Length; i++)
				{
					if(m.ArgMap[i] != -1)
					{
						for(int j = 0; j < localVars.Length; j++)
						{
							if(localVars[j].index == i && parameterBuilders[i - bias] == null)
							{
								if(mb is MethodBuilder)
								{
									parameterBuilders[i - bias] = ((MethodBuilder)mb).DefineParameter(m.ArgMap[i] + 1 - bias, ParameterAttributes.None, localVars[j].name);
								}
								else if(mb is ConstructorBuilder)
								{
									parameterBuilders[i - bias] = ((ConstructorBuilder)mb).DefineParameter(m.ArgMap[i], ParameterAttributes.None, localVars[j].name);
								}
								break;
							}
						}
					}
				}
				return parameterBuilders;
			}
			return null;
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

		internal override Type TypeAsBaseType
		{
			get
			{
				return typeBuilderGhostInterface != null ? typeBuilderGhostInterface : typeBuilder;
			}
		}
	}
	
	private class DoomedTypeImpl : DynamicImpl
	{
		private string message;

		internal DoomedTypeImpl(string message)
		{
			this.message = message;
		}

		internal override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				return null;
			}
		}

		internal override DynamicImpl Finish(bool forDebugSave)
		{
			if(!forDebugSave)
			{
				throw new VerifyError(message);
			}
			return this;
		}

		internal override TypeWrapper[] InnerClasses
		{
			get
			{
				return null;
			}
		}

		internal override FieldInfo LinkField(FieldWrapper fw)
		{
			return null;
		}

		internal override MethodBase LinkMethod(MethodWrapper mw)
		{
			return null;
		}

		internal override Modifiers ReflectiveModifiers
		{
			get
			{
				return (Modifiers)0;
			}
		}

		internal override Type Type
		{
			get
			{
				return null;
			}
		}

		internal override Type TypeAsBaseType
		{
			get
			{
				return null;
			}
		}
	}

	private class FinishedTypeImpl : DynamicImpl
	{
		private Type type;
		private Type typeGhostInterface;
		private TypeWrapper[] innerclasses;
		private TypeWrapper declaringTypeWrapper;
		private Modifiers reflectiveModifiers;

		internal FinishedTypeImpl(Type type, Type typeGhostInterface, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers)
		{
			this.type = type;
			this.typeGhostInterface = typeGhostInterface;
			this.innerclasses = innerclasses;
			this.declaringTypeWrapper = declaringTypeWrapper;
			this.reflectiveModifiers = reflectiveModifiers;
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

		internal override Type TypeAsBaseType
		{
			get
			{
				return typeGhostInterface != null ? typeGhostInterface : type;
			}
		}

		internal override DynamicImpl Finish(bool forDebugSave)
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
	}

	internal override void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
	{
		if(IsGhost)
		{
			ilgen.Emit(OpCodes.Dup);
			ilgen.Emit(OpCodes.Call, ghostCastMethod);
			ilgen.Emit(OpCodes.Pop);
		}
		else if(IsGhostArray)
		{
			ilgen.Emit(OpCodes.Dup);
			ilgen.Emit(OpCodes.Call, ghostCastArrayMethod);
		}
		else
		{
			base.EmitCheckcast(context, ilgen);
		}
	}

	internal override void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
	{
		if(IsGhost)
		{
			ilgen.Emit(OpCodes.Call, ghostIsInstanceMethod);
		}
		else if(IsGhostArray)
		{
			ilgen.Emit(OpCodes.Call, ghostIsInstanceArrayMethod);
		}
		else
		{
			base.EmitInstanceOf(context, ilgen);
		}
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
}

class CompiledTypeWrapper : TypeWrapper
{
	private readonly Type type;
	private TypeWrapper[] interfaces;
	private TypeWrapper[] innerclasses;

	internal static CompiledTypeWrapper newInstance(string name, Type type)
	{
		// TODO since ghost and remapped types can only exist in the core library assembly, we probably
		// should be able to remove the Type.IsDefined() tests in most cases
		if(type.IsValueType && type.IsDefined(typeof(GhostInterfaceAttribute), false))
		{
			return new CompiledGhostTypeWrapper(name, type);
		}
		else if(type.IsDefined(typeof(RemappedTypeAttribute), false))
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
			object[] attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
			if(attribs.Length != 1)
			{
				throw new InvalidOperationException();
			}
			remappedType = ((RemappedTypeAttribute)attribs[0]).Type;
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
				foreach(RemappedInterfaceMethodAttribute m in type.GetCustomAttributes(typeof(RemappedInterfaceMethodAttribute), false))
				{
					MethodInfo method = remappedType.GetMethod(m.MappedTo);
					MethodInfo mbHelper = method;
					Modifiers modifiers = AttributeHelper.GetModifiers(method, false);
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
			Modifiers modifiers = AttributeHelper.GetModifiers(mb, false);
			string name;
			string sig;
			TypeWrapper retType;
			TypeWrapper[] paramTypes;
			GetNameSigFromMethodBase(mb, out name, out sig, out retType, out paramTypes);
			MethodInfo mbHelper = mb as MethodInfo;
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
			return new CompiledRemappedMethodWrapper(this, name, sig, mb, retType, paramTypes, modifiers, false, mbHelper, mbNonvirtualHelper);
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
		Debug.Assert(type.Module.IsDefined(typeof(JavaModuleAttribute), false));

		// look for our custom attribute, that contains the real name of the type (for inner classes)
		Object[] attribs = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
		if(attribs.Length == 1)
		{
			return ((InnerClassAttribute)attribs[0]).InnerClassName;
		}
		return type.FullName;
	}

	// TODO consider resolving the baseType lazily
	private static TypeWrapper GetBaseTypeWrapper(Type type)
	{
		if(type.IsInterface || type.IsDefined(typeof(GhostInterfaceAttribute), false))
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
			object[] attribs = type.GetCustomAttributes(typeof(RemappedTypeAttribute), false);
			if(attribs.Length == 1)
			{
				if(((RemappedTypeAttribute)attribs[0]).Type == typeof(object))
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

	private static ClassLoaderWrapper GetClassLoader(Type type)
	{
		return ClassLoaderWrapper.IsCoreAssemblyType(type) ? ClassLoaderWrapper.GetBootstrapClassLoader() : ClassLoaderWrapper.GetSystemClassLoader();
	}

	private CompiledTypeWrapper(string name, Type type)
		: base(GetModifiers(type), name, GetBaseTypeWrapper(type), GetClassLoader(type), null)
	{
		Debug.Assert(!(type is TypeBuilder));
		Debug.Assert(!type.IsArray);

		this.type = type;
	}

	private static Modifiers GetModifiers(Type type)
	{
		object[] customAttribute = type.GetCustomAttributes(typeof(ModifiersAttribute), false);
		if(customAttribute.Length == 1)
		{
			return ((ModifiersAttribute)customAttribute[0]).Modifiers;
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
		return modifiers;
	}

	internal override bool HasStaticInitializer
	{
		get
		{
			return type.GetField("__<clinit>", BindingFlags.NonPublic | BindingFlags.Static) != null;
		}
	}

	internal override Assembly Assembly
	{
		get
		{
			return type.Assembly;
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
				object[] attribs = type.GetCustomAttributes(typeof(ImplementsAttribute), false);
				if(attribs.Length == 1)
				{
					string[] interfaceNames = ((ImplementsAttribute)attribs[0]).Interfaces;
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
			return null;
		}
	}

	internal override Modifiers ReflectiveModifiers
	{
		get
		{
			object[] customAttribute = type.GetCustomAttributes(typeof(InnerClassAttribute), false);
			if(customAttribute.Length == 1)
			{
				return ((InnerClassAttribute)customAttribute[0]).Modifiers;
			}
			return Modifiers;
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
			else
			{
				if(sigtype[0] == 'L')
				{
					sigtype = sigtype.Substring(1, sigtype.Length - 2);
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
		if(method.IsDefined(typeof(NameSigAttribute), false))
		{
			NameSigAttribute attr = (NameSigAttribute)method.GetCustomAttributes(typeof(NameSigAttribute), false)[0];
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
					string name;
					string sig;
					TypeWrapper retType;
					TypeWrapper[] paramTypes;
					GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes);
					MethodInfo mi = method as MethodInfo;
					bool miranda = mi != null ? AttributeHelper.IsMirandaMethod(mi) : false;
					methods.Add(MethodWrapper.Create(this, name, sig, method, retType, paramTypes, AttributeHelper.GetModifiers(method, false), miranda));
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
		SetMethods((MethodWrapper[])methods.ToArray(typeof(MethodWrapper)));
		SetFields((FieldWrapper[])fields.ToArray(typeof(FieldWrapper)));
	}

	private class CompiledRemappedMethodWrapper : SmartMethodWrapper
	{
		private MethodInfo mbHelper;
		private MethodInfo mbNonvirtualHelper;

		internal CompiledRemappedMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection, MethodInfo mbHelper, MethodInfo mbNonvirtualHelper)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
		{
			this.mbHelper = mbHelper;
			this.mbNonvirtualHelper = mbNonvirtualHelper;
		}

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
			ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
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
	}

	private FieldWrapper CreateFieldWrapper(FieldInfo field)
	{
		Modifiers modifiers = AttributeHelper.GetModifiers(field, false);
		string name = field.Name;
		TypeWrapper type = ClassLoaderWrapper.GetWrapperFromType(field.FieldType);
		if(field.IsDefined(typeof(NameSigAttribute), false))
		{
			NameSigAttribute attr = (NameSigAttribute)field.GetCustomAttributes(typeof(NameSigAttribute), false)[0];
			name = attr.Name;
			SigTypePatchUp(attr.Sig, ref type);
		}

		// If the backing field is private, but the modifiers aren't, we've got a final field that
		// has a property accessor method.
		if(field.IsPrivate && ((modifiers & Modifiers.Private) == 0))
		{
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public;
			bindingFlags |= field.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
			PropertyInfo prop = field.DeclaringType.GetProperty(field.Name, bindingFlags, null, field.FieldType, Type.EmptyTypes, null);
			MethodInfo getter = prop.GetGetMethod(true);
			return new GetterFieldWrapper(this, type, field, name, type.SigName, modifiers, getter);
		}
		else if(field.IsLiteral)
		{
			return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers, field, null);
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
			return type.IsDefined(typeof(ExceptionIsUnsafeForMappingAttribute), false);
		}
	}

	internal override void Finish(bool forDebugSave)
	{
	}
}

sealed class Whidbey
{
	private static readonly object[] noargs = new object[0];
	private static readonly MethodInfo get_IsGenericTypeDefinition = typeof(Type).GetMethod("get_IsGenericTypeDefinition");
	private static readonly MethodInfo get_IsGenericMethodDefinition = typeof(MethodBase).GetMethod("get_IsGenericMethodDefinition");

	internal static bool IsGenericTypeDefinition(Type type)
	{
		return get_IsGenericTypeDefinition != null && (bool)get_IsGenericTypeDefinition.Invoke(type, noargs);
	}

	internal static bool IsGenericMethodDefinition(MethodBase mb)
	{
		return get_IsGenericMethodDefinition != null && (bool)get_IsGenericMethodDefinition.Invoke(mb, noargs);
	}
}

sealed class DotNetTypeWrapper : TypeWrapper
{
	private const string NamePrefix = "cli.";
	private const string DelegateInterfaceSuffix = "$Method";
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
		Debug.Assert(!type.IsArray && !type.Module.IsDefined(typeof(JavaModuleAttribute), false));

		if(type.IsDefined(typeof(NoPackagePrefixAttribute), false) || type.Assembly.IsDefined(typeof(NoPackagePrefixAttribute), false))
		{
			// TODO figure out if this is even required
			return type.FullName.Replace('+', '$');
		}

		return MangleTypeName(type.FullName);
	}

	private static string MangleTypeName(string name)
	{
		// TODO a fully reversible name mangling should be used (all characters not supported by Java should be escaped)
		return NamePrefix + name.Replace('+', '$');
	}

	private static string DemangleTypeName(string name)
	{
		Debug.Assert(name.StartsWith(NamePrefix));
		// TODO a fully reversible name mangling should be used (all characters not supported by Java should be escaped)
		name = name.Substring(NamePrefix.Length);
		if(name.EndsWith(DelegateInterfaceSuffix))
		{
			// HACK if we're a delegate nested type, don't replace the $ sign
			return name;
		}
		return name.Replace('$', '+');
	}

	// this method returns a new TypeWrapper instance for each invocation (doesn't prevent duplicates)
	// the caller is responsible for making sure that only one TypeWrapper with the specified name escapes
	// out into the world
	internal static TypeWrapper CreateDotNetTypeWrapper(string name)
	{
		bool prefixed = name.StartsWith(NamePrefix);
		if(prefixed)
		{
			name = DemangleTypeName(name);
		}
		Type type = LoadTypeFromLoadedAssemblies(name);
		if(type != null)
		{
			if(Whidbey.IsGenericTypeDefinition(type))
			{
				return null;
			}
			if(prefixed || type.IsDefined(typeof(NoPackagePrefixAttribute), false) || type.Assembly.IsDefined(typeof(NoPackagePrefixAttribute), false))
			{
				return new DotNetTypeWrapper(type);
			}
		}
		if(name.EndsWith(DelegateInterfaceSuffix))
		{
			Type delegateType = LoadTypeFromLoadedAssemblies(name.Substring(0, name.Length - DelegateInterfaceSuffix.Length));
			if(delegateType != null && IsDelegate(delegateType))
			{
				MethodInfo invoke = delegateType.GetMethod("Invoke");
				ParameterInfo[] parameters = invoke.GetParameters();
				Type[] args = new Type[parameters.Length];
				for(int i = 0; i < args.Length; i++)
				{
					// we know there aren't any unsupported parameter types, because IsDelegate() returned true
					args[i] = parameters[i].ParameterType;
				}
				ModuleBuilder moduleBuilder = ClassLoaderWrapper.GetBootstrapClassLoader().ModuleBuilder;
				TypeBuilder typeBuilder = moduleBuilder.DefineType(NamePrefix + name, TypeAttributes.Public | TypeAttributes.Interface | TypeAttributes.Abstract);
				AttributeHelper.SetInnerClass(typeBuilder, NamePrefix + name, NamePrefix + delegateType.FullName, "Method", Modifiers.Public | Modifiers.Interface | Modifiers.Static | Modifiers.Abstract);
				typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Abstract | MethodAttributes.Virtual, CallingConventions.Standard, invoke.ReturnType, args);
				return CompiledTypeWrapper.newInstance(NamePrefix + name, typeBuilder.CreateType());
			}
		}
		return null;
	}

	private static Type LoadTypeFromLoadedAssemblies(string name)
	{
		foreach(Assembly a in AppDomain.CurrentDomain.GetAssemblies())
		{
			// HACK we also look inside Java assemblies, because precompiled delegate interfaces might have ended up there
			if(!(a is AssemblyBuilder))
			{
				Type t = a.GetType(name);
				if(t != null)
				{
					return t;
				}
				// HACK we might be looking for an inner classes
				// (if we remove the mangling of NoPackagePrefix types from GetName, we don't need this anymore)
				t = a.GetType(name.Replace('$', '+'));
				if(t != null)
				{
					return t;
				}
			}
		}
		return null;
	}

	internal static TypeWrapper GetWrapperFromDotNetType(Type type)
	{
		// TODO there should be a better way
		return ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(DotNetTypeWrapper.GetName(type));
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
		: base(GetModifiers(type), GetName(type), GetBaseTypeWrapper(type), null, null)
	{
		Debug.Assert(!(type.IsByRef), type.FullName);
		Debug.Assert(!(type.IsPointer), type.FullName);
		Debug.Assert(!(type.IsArray), type.FullName);
		Debug.Assert(!(type is TypeBuilder), type.FullName);
		Debug.Assert(!(type.Module.IsDefined(typeof(JavaModuleAttribute), false)));

		this.type = type;
	}

	internal override ClassLoaderWrapper GetClassLoader()
	{
		return ClassLoaderWrapper.GetSystemClassLoader();
	}

	private class DelegateMethodWrapper : MethodWrapper
	{
		private ConstructorInfo delegateConstructor;
		private MethodInfo method;

		internal DelegateMethodWrapper(TypeWrapper declaringType, Type delegateType, TypeWrapper iface)
			: base(declaringType, "<init>", "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, Modifiers.Public, MemberFlags.None)
		{
			this.delegateConstructor = delegateType.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) });
			this.method = iface.TypeAsTBD.GetMethod("Invoke");
		}

		internal override void EmitNewobj(ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Dup);
			ilgen.Emit(OpCodes.Ldvirtftn, method);
			ilgen.Emit(OpCodes.Newobj, delegateConstructor);
		}

		internal override object Invoke(object obj, object[] args, bool nonVirtual)
		{
			// TODO map exceptions
			return Delegate.CreateDelegate(DeclaringType.TypeAsTBD, args[0], "Invoke");
		}
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
					type = type.Assembly.GetType(type.GetElementType().FullName + "[]", true);
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

		internal override void EmitCall(ILGenerator ilgen)
		{
			// We don't actually need to do anything here!
			// The compiler will insert a boxing operation after calling us and that will
			// result in our argument being boxed (since that's still sitting on the stack).
		}

		internal override object Invoke(object obj, object[] args, bool nonVirtual)
		{
			return Enum.ToObject(DeclaringType.TypeAsTBD, ((IConvertible)args[0]).ToInt64(null));
		}
	}

	internal class EnumValueFieldWrapper : FieldWrapper
	{
		// NOTE if the reference on the stack is null, we *want* the NullReferenceException, so we don't use TypeWrapper.EmitUnbox
		internal EnumValueFieldWrapper(DotNetTypeWrapper tw, TypeWrapper fieldType)
			: base(tw, fieldType, "Value", fieldType.SigName, Modifiers.Public | Modifiers.Final, null)
		{
		}

		protected override void EmitGetImpl(ILGenerator ilgen)
		{
			DotNetTypeWrapper tw = (DotNetTypeWrapper)this.DeclaringType;
			ilgen.Emit(OpCodes.Unbox, tw.type);
			// FXBUG the .NET 1.1 verifier doesn't understand that ldobj on an enum that has an underlying type
			// of byte or short that the resulting type on the stack is an int32, so we have to
			// to it the hard way. Note that this is fixed in Whidbey.
			Type underlyingType = Enum.GetUnderlyingType(tw.type);
			if(underlyingType == typeof(sbyte) || underlyingType == typeof(byte))
			{
				ilgen.Emit(OpCodes.Ldind_I1);
			}
			else if(underlyingType == typeof(short) || underlyingType == typeof(ushort))
			{
				ilgen.Emit(OpCodes.Ldind_I2);
			}
			else if(underlyingType == typeof(int) || underlyingType == typeof(uint))
			{
				ilgen.Emit(OpCodes.Ldind_I4);
			}
			else if(underlyingType == typeof(long) || underlyingType == typeof(ulong))
			{
				ilgen.Emit(OpCodes.Ldind_I8);
			}
		}

		protected override void EmitSetImpl(ILGenerator ilgen)
		{
			throw new InvalidOperationException();
		}

		internal override void SetValue(object obj, object val)
		{
			// NOTE even though the field is final, JNI reflection can still be used to set its value!
			// NOTE the CLI spec says that an enum has exactly one instance field, so we take advantage of that fact.
			FieldInfo f = DeclaringType.TypeAsTBD.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)[0];
			f.SetValue(obj, val);
		}

		// this method takes a boxed Enum and returns its value as a boxed primitive
		// of the subset of Java primitives (i.e. byte, short, int, long)
		internal static object GetEnumPrimitiveValue(object obj)
		{
			Type underlyingType = Enum.GetUnderlyingType(obj.GetType());
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

		internal override object GetValue(object obj)
		{
			return GetEnumPrimitiveValue(obj);
		}
	}

	internal override Assembly Assembly
	{
		get
		{
			return type.Assembly;
		}
	}

	private class ValueTypeDefaultCtor : MethodWrapper
	{
		internal ValueTypeDefaultCtor(DotNetTypeWrapper tw)
			: base(tw, "<init>", "()V", null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Public, MemberFlags.None)
		{
		}

		internal override void EmitNewobj(ILGenerator ilgen)
		{
			LocalBuilder local = ilgen.DeclareLocal(DeclaringType.TypeAsTBD);
			ilgen.Emit(OpCodes.Ldloc, local);
			ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
		}

		internal override object Invoke(object obj, object[] args, bool nonVirtual)
		{
			if(obj == null)
			{
				obj = Activator.CreateInstance(DeclaringType.TypeAsTBD);
			}
			return obj;
		}
	}

	protected override void LazyPublishMembers()
	{
		ArrayList fieldsList = new ArrayList();
		ArrayList methodsList = new ArrayList();
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
					object val = EnumValueFieldWrapper.GetEnumPrimitiveValue(fields[i].GetValue(null));
					fieldsList.Add(new ConstantFieldWrapper(this, fieldType, name, fieldType.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final, fields[i], val));
				}
			}
			fieldsList.Add(new EnumValueFieldWrapper(this, fieldType));
			methodsList.Add(new EnumWrapMethodWrapper(this, fieldType));
		}
		else
		{
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
					fieldsList.Add(CreateFieldWrapperDotNet(AttributeHelper.GetModifiers(fields[i], true), fields[i].Name, fields[i].FieldType, fields[i]));
				}
			}

			// special case for delegate constructors!
			if(IsDelegate(type))
			{
				TypeWrapper iface = InnerClasses[0];
				Debug.Assert(iface is CompiledTypeWrapper);
				iface.Finish();
				methodsList.Add(new DelegateMethodWrapper(this, type, iface));
			}

			bool fabricateDefaultCtor = type.IsValueType;

			ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
			for(int i = 0; i < constructors.Length; i++)
			{
				string name;
				string sig;
				if(MakeMethodDescriptor(constructors[i], out name, out sig))
				{
					if(fabricateDefaultCtor && !constructors[i].IsStatic && sig == "()V")
					{
						fabricateDefaultCtor = false;
					}
					// TODO handle name/signature clash
					methodsList.Add(CreateMethodWrapper(name, sig, constructors[i], false));
				}
			}

			if(fabricateDefaultCtor)
			{
				// Value types have an implicit default ctor
				methodsList.Add(new ValueTypeDefaultCtor(this));
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
					if(MakeMethodDescriptor(methods[i], out name, out sig))
					{
						if(!methods[i].IsStatic && !methods[i].IsPrivate && BaseTypeWrapper != null)
						{
							MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
							if(baseMethod != null && baseMethod.IsFinal && !baseMethod.IsStatic && !baseMethod.IsPrivate)
							{
								continue;
							}
						}
						// TODO handle name/signature clash
						methodsList.Add(CreateMethodWrapper(name, sig, methods[i], false));
					}
				}
			}

			// HACK private interface implementations need to be published as well
			// (otherwise the type appears abstract while it isn't)
			if(!type.IsInterface)
			{
				Hashtable clash = null;
				Type[] interfaces = type.GetInterfaces();
				for(int i = 0; i < interfaces.Length; i++)
				{
					if(interfaces[i].IsPublic)
					{
						InterfaceMapping map = type.GetInterfaceMap(interfaces[i]);
						for(int j = 0; j < map.InterfaceMethods.Length; j++)
						{
							if(!map.TargetMethods[j].IsPublic && map.TargetMethods[j].DeclaringType == type)
							{
								string name;
								string sig;
								if(MakeMethodDescriptor(map.InterfaceMethods[j], out name, out sig))
								{
									if(BaseTypeWrapper != null)
									{
										MethodWrapper baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
										if(baseMethod != null && !baseMethod.IsStatic && baseMethod.IsPublic)
										{
											continue;
										}
									}
									if(clash == null)
									{
										clash = new Hashtable();
										foreach(MethodWrapper mw in methodsList)
										{
											clash.Add(mw.Name + mw.Signature, null);
										}										
									}
									if(!clash.ContainsKey(name + sig))
									{
										clash.Add(name + sig, null);
										methodsList.Add(CreateMethodWrapper(name, sig, map.InterfaceMethods[j], true));
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
				Hashtable h = new Hashtable();
				TypeWrapper baseTypeWrapper = this.BaseTypeWrapper;
				while(baseTypeWrapper != null)
				{
					foreach(MethodWrapper m in baseTypeWrapper.GetMethods())
					{
						if(!m.IsStatic && !m.IsFinal && (m.IsPublic || m.IsProtected) && m.Name != "<init>")
						{
							if(!h.ContainsKey(m.Name + m.Signature))
							{
								h.Add(m.Name + m.Signature, "");
								// TODO handle name/sig clash (what should we do?)
								methodsList.Add(new BaseFinalMethodWrapper(this, m));
							}
						}
					}
					baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
				}
			}
		}
		SetMethods((MethodWrapper[])methodsList.ToArray(typeof(MethodWrapper)));
		SetFields((FieldWrapper[])fieldsList.ToArray(typeof(FieldWrapper)));
	}

	private class BaseFinalMethodWrapper : MethodWrapper
	{
		private MethodWrapper m;

		internal BaseFinalMethodWrapper(DotNetTypeWrapper tw, MethodWrapper m)
			: base(tw, m.Name, m.Signature, m.GetMethod(), m.ReturnType, m.GetParameters(), m.Modifiers | Modifiers.Final, MemberFlags.None)
		{
			this.m = m;
		}

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

		internal override object Invoke(object obj, object[] args, bool nonVirtual)
		{
			return m.Invoke(obj, args, nonVirtual);
		}
	}

	private bool MakeMethodDescriptor(MethodBase mb, out string name, out string sig)
	{
		if(Whidbey.IsGenericMethodDefinition(mb))
		{
			name = null;
			sig = null;
			return false;
		}
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append('(');
		ParameterInfo[] parameters = mb.GetParameters();
		TypeWrapper[] args = new TypeWrapper[parameters.Length];
		for(int i = 0; i < parameters.Length; i++)
		{
			Type type = parameters[i].ParameterType;
			if(type.IsPointer)
			{
				name = null;
				sig = null;
				return false;
			}
			if(type.IsByRef)
			{
				if(type.GetElementType().IsPointer)
				{
					name = null;
					sig = null;
					return false;
				}
				type = type.Assembly.GetType(type.GetElementType().FullName + "[]", true);
				if(mb.IsAbstract)
				{
					// Since we cannot override methods with byref arguments, we don't report abstract
					// methods with byref args.
					name = null;
					sig = null;
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
			TypeWrapper ret = PrimitiveTypeWrapper.VOID;
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
				return false;
			}
			TypeWrapper ret = ClassLoaderWrapper.GetWrapperFromType(type);
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
					for(int i = 0; i < interfaces.Length; i++)
					{
						if(interfaceTypes[i].DeclaringType != null &&
							interfaceTypes[i].IsDefined(typeof(HideFromJavaAttribute), false) &&
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
					if(IsDelegate(type))
					{
						innerClasses = new TypeWrapper[] { ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(Name + DelegateInterfaceSuffix) };
					}
					else
					{
						Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
						ArrayList list = new ArrayList(nestedTypes.Length);
						for(int i = 0; i < nestedTypes.Length; i++)
						{
							if(!Whidbey.IsGenericTypeDefinition(nestedTypes[i]))
							{
								list.Add(ClassLoaderWrapper.GetWrapperFromType(nestedTypes[i]));
							}
						}
						innerClasses = (TypeWrapper[])list.ToArray(typeof(TypeWrapper));
					}
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
			return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers, field, null);
		}
		else
		{
			return FieldWrapper.Create(this, type, field, name, type.SigName, modifiers);
		}
	}

	private MethodWrapper CreateMethodWrapper(string name, string sig, MethodBase mb, bool privateInterfaceImplHack)
	{
		Modifiers mods = AttributeHelper.GetModifiers(mb, true);
		if(name == "Finalize" && sig == "()V" && !mb.IsStatic &&
			TypeAsBaseType.IsSubclassOf(CoreClasses.java.lang.Object.Wrapper.TypeAsBaseType))
		{
			// TODO if the .NET also has a "finalize" method, we need to hide that one (or rename it, or whatever)
			MethodWrapper mw = new SimpleCallMethodWrapper(this, "finalize", "()V", (MethodInfo)mb, null, null, mods, false, SimpleOpCode.Call, SimpleOpCode.Callvirt);
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
			// TODO pass in the argument and return types
			return new ByRefMethodWrapper(args, byrefs, this, name, sig, mb, null, null, mods, false);
		}
		else
		{
			if(mb is ConstructorInfo)
			{
				// TODO pass in the argument and return types
				return new SmartConstructorMethodWrapper(this, name, sig, (ConstructorInfo)mb, null, mods, false);
			}
			else
			{
				// TODO pass in the argument and return types
				return new SmartCallMethodWrapper(this, name, sig, (MethodInfo)mb, null, null, mods, false, SimpleOpCode.Call, SimpleOpCode.Callvirt);
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

	internal override void EmitInstanceOf(TypeWrapper context, ILGenerator ilgen)
	{
		if(IsRemapped)
		{
			TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
			MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<instanceof>");
			if(method != null)
			{
				ilgen.Emit(OpCodes.Call, method);
				return;
			}
		}
		ilgen.Emit(OpCodes.Isinst, type);
		ilgen.Emit(OpCodes.Ldnull);
		ilgen.Emit(OpCodes.Cgt_Un);
	}

	internal override void EmitCheckcast(TypeWrapper context, ILGenerator ilgen)
	{
		if(IsRemapped)
		{
			TypeWrapper shadow = ClassLoaderWrapper.GetWrapperFromTypeFast(type);
			MethodInfo method = shadow.TypeAsBaseType.GetMethod("__<checkcast>");
			if(method != null)
			{
				ilgen.Emit(OpCodes.Call, method);
				return;
			}
		}
		EmitHelper.Castclass(ilgen, type);
	}

	internal override void Finish(bool forDebugSave)
	{
		// TODO instead of linking here, we should just pre-link in LazyPublishMembers
		foreach(MethodWrapper mw in GetMethods())
		{
			mw.Link();
		}
		foreach(FieldWrapper fw in GetFields())
		{
			fw.Link();
		}
	}
}

sealed class ArrayTypeWrapper : TypeWrapper
{
	private static TypeWrapper[] interfaces;
	private static MethodInfo clone;
	private Type type;
	private Modifiers reflectiveModifiers;

	internal ArrayTypeWrapper(Type type, Modifiers modifiers, Modifiers reflectiveModifiers, string name, ClassLoaderWrapper classLoader)
		: base(modifiers, name, CoreClasses.java.lang.Object.Wrapper, classLoader, null)
	{
		this.type = type;
		this.reflectiveModifiers = reflectiveModifiers;
	}

	protected override void LazyPublishMembers()
	{
		if(clone == null)
		{
			clone = typeof(Array).GetMethod("Clone", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
		}
		MethodWrapper mw = new SimpleCallMethodWrapper(this, "clone", "()Ljava.lang.Object;", clone, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray, Modifiers.Public, true, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt);
		mw.Link();
		SetMethods(new MethodWrapper[] { mw });
		SetFields(FieldWrapper.EmptyArray);
	}

	internal override Modifiers ReflectiveModifiers
	{
		get
		{
			return reflectiveModifiers;
		}
	}

	internal override Assembly Assembly
	{
		get
		{
			return type.Assembly;
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
			return type;
		}
	}

	private bool IsFinished
	{
		get
		{
			Type elem = type.GetElementType();
			while(elem.IsArray)
			{
				elem = elem.GetElementType();
			}
			return !(elem is TypeBuilder);
		}
	}

	internal override void Finish(bool forDebugSave)
	{
		lock(this)
		{
			// TODO optimize this
			if(!IsFinished)
			{
				TypeWrapper elementTypeWrapper = ElementTypeWrapper;
				Type elementType = elementTypeWrapper.TypeAsArrayType;
				elementTypeWrapper.Finish();
				type = elementType.Assembly.GetType(elementType.FullName + "[]", true);
				ClassLoaderWrapper.SetWrapperForType(type, this);
			}
		}
	}
}
