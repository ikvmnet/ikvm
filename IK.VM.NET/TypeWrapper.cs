/*
  Copyright (C) 2002 Jeroen Frijters

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
using OpenSystem.Java;

sealed class MethodDescriptor
{
	private ClassLoaderWrapper classLoader;
	private string name;
	private string sig;
	private Type[] args;
	private TypeWrapper[] argTypeWrappers;
	private TypeWrapper retTypeWrapper;

	internal MethodDescriptor(ClassLoaderWrapper classLoader, ClassFile.ConstantPoolItemFMI cpi)
		: this(classLoader, cpi.Name, cpi.Signature)
	{
		argTypeWrappers = cpi.GetArgTypes(classLoader);
		retTypeWrapper = cpi.GetRetType(classLoader);
	}

	internal MethodDescriptor(ClassLoaderWrapper classLoader, ClassFile.Method method)
		: this(classLoader, method.Name, method.Signature)
	{
		argTypeWrappers = method.GetArgTypes(classLoader);
		retTypeWrapper = method.GetRetType(classLoader);
	}

	internal MethodDescriptor(ClassLoaderWrapper classLoader, string name, string sig)
	{
		Debug.Assert(classLoader != null);
		// class name in the sig should be dotted
		Debug.Assert(sig.IndexOf('/') < 0);

		this.classLoader = classLoader;
		this.name = name;
		this.sig = sig;
	}

	internal string Name
	{
		get
		{
			return name;
		}
	}

	internal string Signature
	{
		get
		{
			return sig;
		}
	}

	// NOTE this exposes potentially unfinished types!
	internal Type[] ArgTypes
	{
		get
		{
			if(args == null)
			{
				TypeWrapper[] wrappers = ArgTypeWrappers;
				Type[] temp = new Type[wrappers.Length];
				for(int i = 0; i < wrappers.Length; i++)
				{
					temp[i] = wrappers[i].Type;
				}
				args = temp;
			}
			return args;
		}
	}

	internal TypeWrapper[] ArgTypeWrappers
	{
		get
		{
			if(argTypeWrappers == null)
			{
				argTypeWrappers = classLoader.ArgTypeWrapperListFromSig(sig);
			}
			return argTypeWrappers;
		}
	}

	// NOTE this exposes a potentially unfinished type!
	internal Type RetType
	{
		get
		{
			return RetTypeWrapper.Type;
		}
	}

	internal TypeWrapper RetTypeWrapper
	{
		get
		{
			if(retTypeWrapper == null)
			{
				retTypeWrapper = classLoader.RetTypeWrapperFromSig(sig);
			}
			return retTypeWrapper;
		}
	}

	public override bool Equals(object o)
	{
		// TODO instead of comparing the signature strings, we should compare the actual types
		// (because, in the face of multiple class loaders, there can be multiple classes with the same name)
		MethodDescriptor md = o as MethodDescriptor;
		return md != null && md.name == name && md.sig == sig;
	}

	public override int GetHashCode()
	{
		return name.GetHashCode() ^ sig.GetHashCode();
	}

	internal static string GetSigName(ParameterInfo param)
	{
		Type type = param.ParameterType;
		if(type == typeof(object))
		{
			return GetSigNameFromCustomAttribute(param);
		}
		return GetSigNameFromType(type);
	}

	internal static string GetSigName(FieldInfo field)
	{
		Type type = field.FieldType;
		if(type == typeof(object))
		{
			return GetSigNameFromCustomAttribute(field);
		}
		return GetSigNameFromType(type);
	}

	internal static string GetSigName(MethodInfo method)
	{
		Type type = method.ReturnType;
		if(type == typeof(object))
		{
			return GetSigNameFromCustomAttribute(method);
		}
		return GetSigNameFromType(type);
	}

	private static string GetSigNameFromCustomAttribute(ICustomAttributeProvider provider)
	{
		object[] attribs = provider.GetCustomAttributes(typeof(GhostTypeAttribute), false);
		if(attribs.Length == 1)
		{
			return GetSigNameFromType(((GhostTypeAttribute)attribs[0]).Type);
		}
		attribs = provider.GetCustomAttributes(typeof(UnloadableTypeAttribute), false);
		if(attribs.Length == 1)
		{
			return "L" + ((UnloadableTypeAttribute)attribs[0]).Name + ";";
		}
		return "Ljava.lang.Object;";
	}

	private static string GetSigNameFromType(Type type)
	{
		if(type.IsValueType)
		{
			if(type == typeof(void))
			{
				return "V";
			}
			else if(type == typeof(bool))
			{
				return "Z";
			}
			else if(type == typeof(sbyte))
			{
				return "B";
			}
			else if(type == typeof(char))
			{
				return "C";
			}
			else if(type == typeof(short))
			{
				return "S";
			}
			else if(type == typeof(int))
			{
				return "I";
			}
			else if(type == typeof(long))
			{
				return "J";
			}
			else if(type == typeof(float))
			{
				return "F";
			}
			else if(type == typeof(double))
			{
				return "D";
			}
			else
			{
				return "L" + type.FullName + ";";
			}
		}
		else
		{
			string s = NativeCode.java.lang.VMClass.getName(type);
			if(s[0] != '[')
			{
				s = "L" + s + ";";
			}
			return s;
		}
	}

	internal static MethodDescriptor FromMethodBase(MethodBase mb)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append('(');
		foreach(ParameterInfo param in mb.GetParameters())
		{
			sb.Append(GetSigName(param));
		}
		sb.Append(')');
		if(mb is ConstructorInfo)
		{
			sb.Append('V');
			return new MethodDescriptor(ClassLoaderWrapper.GetClassLoader(mb.DeclaringType), mb.IsStatic ? "<clinit>" : "<init>", sb.ToString());
		}
		else
		{
			sb.Append(GetSigName((MethodInfo)mb));
			return new MethodDescriptor(ClassLoaderWrapper.GetClassLoader(mb.DeclaringType), mb.Name, sb.ToString());
		}
	}
}

class EmitHelper
{
	internal static void Throw(ILGenerator ilgen, string dottedClassName, string message)
	{
		TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(dottedClassName);
		ilgen.Emit(OpCodes.Ldstr, message);
		MethodDescriptor md = new MethodDescriptor(exception.GetClassLoader(), "<init>", "(Ljava.lang.String;)V");
		exception.GetMethodWrapper(md, false).EmitNewobj.Emit(ilgen);
		ilgen.Emit(OpCodes.Throw);
	}

	internal static void RunClassConstructor(ILGenerator ilgen, Type type)
	{
		ilgen.Emit(OpCodes.Ldtoken, type);
		ilgen.Emit(OpCodes.Call, typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetMethod("RunClassConstructor"));
	}
}

class AttributeHelper
{
	private static CustomAttributeBuilder hideFromReflectionAttribute;
	private static ConstructorInfo implementsAttribute;
	private static ConstructorInfo ghostTypeAttribute;

	internal static void HideFromReflection(ConstructorBuilder cb)
	{
		if(hideFromReflectionAttribute == null)
		{
			hideFromReflectionAttribute = new CustomAttributeBuilder(typeof(HideFromReflectionAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		cb.SetCustomAttribute(hideFromReflectionAttribute);
	}

	internal static void HideFromReflection(MethodBuilder mb)
	{
		if(hideFromReflectionAttribute == null)
		{
			hideFromReflectionAttribute = new CustomAttributeBuilder(typeof(HideFromReflectionAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		}
		mb.SetCustomAttribute(hideFromReflectionAttribute);
	}

	internal static bool IsHideFromReflection(MemberInfo mi)
	{
		// NOTE all privatescope fields and methods are "hideFromReflection"
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
		return mi.IsDefined(typeof(HideFromReflectionAttribute), false);
	}

	internal static void ImplementsAttribute(TypeBuilder typeBuilder, Type iface)
	{
		if(implementsAttribute == null)
		{
			implementsAttribute = typeof(ImplementsAttribute).GetConstructor(new Type[] { typeof(Type) });
		}
		typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(implementsAttribute, new object[] { iface }));
	}

	internal static Modifiers GetModifiers(MethodBase mb)
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
		if(mb.IsPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(mb.IsFamily || mb.IsFamilyOrAssembly)
		{
			modifiers |= Modifiers.Protected;
		}
		// NOTE Java doesn't support non-virtual methods, but we set the Final modifier for
		// non-virtual methods to approximate the semantics
		if(mb.IsFinal || (!mb.IsStatic && !mb.IsVirtual && !mb.IsConstructor))
		{
			modifiers |= Modifiers.Final;
		}
		if(mb.IsAbstract)
		{
			modifiers |= Modifiers.Abstract;
		}
		if(mb.IsStatic)
		{
			modifiers |= Modifiers.Static;
		}
		if((mb.GetMethodImplementationFlags() & MethodImplAttributes.Synchronized) != 0)
		{
			modifiers |= Modifiers.Synchronized;
		}
		if((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
		{
			modifiers |= Modifiers.Native;
		}
		return modifiers;
	}

	internal static Modifiers GetModifiers(FieldInfo fi)
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
		if(fi.IsPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(fi.IsFamily || fi.IsFamilyOrAssembly)
		{
			modifiers |= Modifiers.Protected;
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

	internal static void SetGhostType(ParameterBuilder pb, Type type)
	{
		if(ghostTypeAttribute == null)
		{
			ghostTypeAttribute = typeof(GhostTypeAttribute).GetConstructor(new Type[] { typeof(Type) });
		}
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ghostTypeAttribute, new object[] { type });
		pb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetGhostType(MethodBuilder mb, Type type)
	{
		if(ghostTypeAttribute == null)
		{
			ghostTypeAttribute = typeof(GhostTypeAttribute).GetConstructor(new Type[] { typeof(Type) });
		}
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ghostTypeAttribute, new object[] { type });
		mb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetGhostType(FieldBuilder fb, Type type)
	{
		if(ghostTypeAttribute == null)
		{
			ghostTypeAttribute = typeof(GhostTypeAttribute).GetConstructor(new Type[] { typeof(Type) });
		}
		CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ghostTypeAttribute, new object[] { type });
		fb.SetCustomAttribute(customAttributeBuilder);
	}

	internal static void SetUnloadableType(FieldBuilder field, string name)
	{
		CustomAttributeBuilder attrib = new CustomAttributeBuilder(typeof(UnloadableTypeAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { name });
		field.SetCustomAttribute(attrib);
	}
}

abstract class TypeWrapper
{
	private static TypeWrapper java_lang_Object;
	private readonly ClassLoaderWrapper classLoader;
	private readonly string name;		// java name (e.g. java.lang.Object)
	private readonly Modifiers modifiers;
	private readonly Hashtable methods = new Hashtable();
	private readonly Hashtable fields = new Hashtable();
	private readonly TypeWrapper baseWrapper;
	private bool hasIncompleteInterfaceImplementation;
	public static readonly TypeWrapper[] EmptyArray = new TypeWrapper[0];
	public const Modifiers UnloadableModifiersHack = Modifiers.Final | Modifiers.Interface | Modifiers.Private;
	public const Modifiers VerifierTypeModifiersHack = Modifiers.Final | Modifiers.Interface;

	public TypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper, ClassLoaderWrapper classLoader)
	{
		Profiler.Count("TypeWrapper");
		// class name should be dotted or null for primitives
		Debug.Assert(name == null || name.IndexOf('/') < 0);

		this.modifiers = modifiers;
		this.name = name;
		this.baseWrapper = baseWrapper;
		this.classLoader = classLoader;
	}

	public override string ToString()
	{
		return GetType().Name + "[" + name + "]";
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

	// a ghost is an interface that appears to be implemented by a .NET type
	// (e.g. System.String (aka java.lang.String) appears to implement java.lang.CharSequence,
	// so java.lang.CharSequence is a ghost)
	internal bool IsGhost
	{
		get
		{
			return ClassLoaderWrapper.IsGhost(this);
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
			return this != VerifierTypeWrapper.Null && !IsPrimitive && Type.IsValueType;
		}
	}

	internal bool IsPrimitive
	{
		get
		{
			return name == null;
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

	// TODO since for inner classes, the modifiers returned by Class.getModifiers are different from the actual
	// modifiers (as used by the VM access control mechanism), we need an additional property (e.g. InnerClassModifiers)
	internal Modifiers Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	// since for inner classes, the modifiers returned by Class.getModifiers are different from the actual
	// modifiers (as used by the VM access control mechanism), we have this additional property
	// NOTE this property can only be called for finished types!
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
			return (modifiers & Modifiers.Abstract) != 0;
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

	internal virtual ClassLoaderWrapper GetClassLoader()
	{
		return classLoader;
	}

	protected abstract FieldWrapper GetFieldImpl(string fieldName);

	// TODO this shouldn't just be based on the name, fields can be overloaded on type
	public FieldWrapper GetFieldWrapper(string fieldName)
	{
		FieldWrapper fae = (FieldWrapper)fields[fieldName];
		if(fae == null)
		{
			fae = GetFieldImpl(fieldName);
			if(fae == null)
			{
				if(baseWrapper != null)
				{
					return baseWrapper.GetFieldWrapper(fieldName);
				}
				return null;
			}
			fields[fieldName] = fae;
		}
		return fae;
	}

	// TODO figure out when it is safe to call this
	// HACK for now we assume that the method hashtable has always been filled when this method is called (by java.lang.Class)
	internal virtual MethodWrapper[] GetMethods()
	{
		MethodWrapper[] wrappers = new MethodWrapper[methods.Count];
		methods.Values.CopyTo(wrappers, 0);
		return wrappers;
	}

	// TODO figure out when it is safe to call this
	// HACK for now we assume that the fields hashtable has always been filled when this method is called (by java.lang.Class)
	internal virtual FieldWrapper[] GetFields()
	{
		FieldWrapper[] wrappers = new FieldWrapper[fields.Count];
		fields.Values.CopyTo(wrappers, 0);
		return wrappers;
	}

	protected abstract MethodWrapper GetMethodImpl(MethodDescriptor md);

	public MethodWrapper GetMethodWrapper(MethodDescriptor md, bool inherit)
	{
		MethodWrapper mce = (MethodWrapper)methods[md];
		if(mce == null)
		{
			mce = GetMethodImpl(md);
			if(mce == null)
			{
				if(inherit && baseWrapper != null)
				{
					return baseWrapper.GetMethodWrapper(md, inherit);
				}
				return null;
			}
			methods[md] = mce;
		}
		return mce;
	}

	public void AddMethod(MethodWrapper method)
	{
		Debug.Assert(method != null);
		methods[method.Descriptor] = method;
	}

	public void AddField(FieldWrapper field)
	{
		Debug.Assert(field != null);
		fields[field.Name] = field;
	}

	public string Name
	{
		get
		{
			return name;
		}
	}

	internal string PackageName
	{
		get
		{
			int index = name.LastIndexOf('.');
			if(index == -1)
			{
				return "";
			}
			return name.Substring(0, index);
		}
	}

	// returns true iff wrapper is allowed to access us
	internal bool IsAccessibleFrom(TypeWrapper wrapper)
	{
		return IsPublic || IsInSamePackageAs(wrapper);
	}

	public bool IsInSamePackageAs(TypeWrapper wrapper)
	{
		if(GetClassLoader() == wrapper.GetClassLoader())
		{
			int index1 = name.LastIndexOf('.');
			int index2 = wrapper.name.LastIndexOf('.');
			if(index1 == -1 && index2 == -1)
			{
				return true;
			}
			if(index1 != index2)
			{
				return false;
			}
			return String.CompareOrdinal(name, 0, wrapper.name, 0, index1) == 0;
		}
		return false;
	}

	public abstract Type Type
	{
		get;
	}

	// TODO this should really be named TypeAsLocalArgOrStackType (or something like that)
	internal Type TypeOrUnloadableAsObject
	{
		get
		{
			if(IsUnloadable || IsGhost)
			{
				return typeof(object);
			}
			// HACK as a convenience to the compiler, we replace return address types with typeof(int)
			if(VerifierTypeWrapper.IsRet(this))
			{
				return typeof(int);
			}
			return Type;
		}
	}

	public TypeWrapper BaseTypeWrapper
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
			if(this == VerifierTypeWrapper.Null)
			{
				return VerifierTypeWrapper.Null;
			}
			if(name[0] != '[')
			{
				throw new InvalidOperationException(name);
			}
			// TODO consider caching the element type
			switch(name[1])
			{
				case '[':
					return classLoader.LoadClassByDottedName(name.Substring(1));
				case 'L':
					return classLoader.LoadClassByDottedName(name.Substring(2, name.Length - 3));
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

	public bool ImplementsInterface(TypeWrapper interfaceWrapper)
	{
		TypeWrapper typeWrapper = this;
		while(typeWrapper != null)
		{
			for(int i = 0; i < typeWrapper.Interfaces.Length; i++)
			{
				if(typeWrapper.Interfaces[i] == interfaceWrapper)
				{
					return true;
				}
				if(typeWrapper.Interfaces[i].ImplementsInterface(interfaceWrapper))
				{
					return true;
				}
			}
			typeWrapper = typeWrapper.BaseTypeWrapper;
		}
		return false;
	}

	public bool IsSubTypeOf(TypeWrapper baseType)
	{
		// make sure IsSubTypeOf isn't used on primitives
		Debug.Assert(!this.IsPrimitive);
		Debug.Assert(!baseType.IsPrimitive);

		if(baseType.IsInterface)
		{
			if(baseType == this)
			{
				return true;
			}
			return ImplementsInterface(baseType);
		}
		if(java_lang_Object == null)
		{
			// TODO cache java.lang.Object somewhere else
			java_lang_Object = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Object");
		}
		// NOTE this isn't just an optimization, it is also required when this is an interface
		if(baseType == java_lang_Object)
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

	public abstract TypeWrapper[] Interfaces
	{
		get;
	}

	// NOTE this property can only be called for finished types!
	public abstract TypeWrapper[] InnerClasses
	{
		get;
	}

	// NOTE this property can only be called for finished types!
	public abstract TypeWrapper DeclaringTypeWrapper
	{
		get;
	}

	public abstract void Finish();

	private void ImplementInterfaceMethodStubImpl(MethodDescriptor md, MethodBase ifmethod, TypeBuilder typeBuilder, DynamicTypeWrapper wrapper)
	{
		// HACK we're mangling the name to prevent subclasses from overriding this method
		string mangledName = this.Name + "$" + ifmethod.Name + "$" + wrapper.Name;
		MethodWrapper mce = wrapper.GetMethodWrapper(md, true);
		if(mce != null && mce.HasUnloadableArgsOrRet)
		{
			// HACK for now we make it seem as if the method isn't there, we should be emitting
			// a stub that throws a NoClassDefFoundError
			// NOTE AFAICT this can only happen when code explicitly messes around with the custom class loaders
			// that violate the class loader rules.
			mce = null;
		}
		if(mce != null)
		{
			if(!mce.IsPublic)
			{
				// NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
				// it makes sense, so I hope the spec is wrong
				// UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
				// methods. Sigh! So I have to use private methods and mangle the name
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, md.RetType, md.ArgTypes);
				AttributeHelper.HideFromReflection(mb);
				EmitHelper.Throw(mb.GetILGenerator(), "java.lang.IllegalAccessError", wrapper.Name + "." + md.Name + md.Signature);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
				wrapper.HasIncompleteInterfaceImplementation = true;
			}
			else if(mce.GetMethod().Name != ifmethod.Name)
			{
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, md.RetType, md.ArgTypes);
				AttributeHelper.HideFromReflection(mb);
				ILGenerator ilGenerator = mb.GetILGenerator();
				ilGenerator.Emit(OpCodes.Ldarg_0);
				int argc = md.ArgTypes.Length;
				for(int n = 0; n < argc; n++)
				{
					ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
				}
				mce.EmitCallvirt.Emit(ilGenerator);
				ilGenerator.Emit(OpCodes.Ret);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
			}
			else if(mce.GetMethod().DeclaringType.Assembly != typeBuilder.Assembly)
			{
				// NOTE methods inherited from base classes in a different assembly do *not* automatically implement
				// interface methods, so we have to generate a stub here that doesn't do anything but call the base
				// implementation
				if(mce.IsAbstract)
				{
					// TODO figure out what to do here
					throw new NotImplementedException();
				}
				MethodBuilder mb = typeBuilder.DefineMethod(md.Name, MethodAttributes.Public | MethodAttributes.Virtual, md.RetType, md.ArgTypes);
				AttributeHelper.HideFromReflection(mb);
				ILGenerator ilGenerator = mb.GetILGenerator();
				ilGenerator.Emit(OpCodes.Ldarg_0);
				int argc = md.ArgTypes.Length;
				for(int n = 0; n < argc; n++)
				{
					ilGenerator.Emit(OpCodes.Ldarg_S, (byte)(n + 1));
				}
				mce.EmitCall.Emit(ilGenerator);
				ilGenerator.Emit(OpCodes.Ret);
			}
		}
		else
		{
			if(!wrapper.IsAbstract)
			{
				// the type doesn't implement the interface method and isn't abstract either. The JVM allows this, but the CLR doesn't,
				// so we have to create a stub method that throws an AbstractMethodError
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, md.RetType, md.ArgTypes);
				AttributeHelper.HideFromReflection(mb);
				EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", wrapper.Name + "." + md.Name + md.Signature);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
				wrapper.HasIncompleteInterfaceImplementation = true;
			}
			else
			{
				// because of a bug in the .NET 1.0 CLR, we have emit an abstract Miranda method, otherwise
				// the class will not be loadable under some circumstances
				// Example (compile with Jikes 1.18):
				//interface __Shape
				//{
				//    public abstract __Rectangle getBounds();
				//    public abstract __Rectangle2D getBounds2D();
				//}
				//
				//abstract class __RectangularShape implements __Shape
				//{
				//    public __Rectangle getBounds()
				//    {
				//	     return null;
				//    }
				//}
				//
				//abstract class __Rectangle2D extends __RectangularShape
				//{
				//    public __Rectangle2D getBounds2D()
				//    {
				//        return null;
				//    }
				//}
				//
				//class __Rectangle extends __Rectangle2D implements __Shape
				//{
				//    public __Rectangle getBounds()
				//    {
				//        return null;
				//    }
				//
				//    public __Rectangle2D getBounds2D()
				//    {
				//        return null;
				//    }
				//}
				MethodBuilder mb = typeBuilder.DefineMethod(md.Name, MethodAttributes.NewSlot | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract, md.RetType, md.ArgTypes);
				AttributeHelper.HideFromReflection(mb);
				// NOTE because we are introducing a Miranda method, we must also update the corresponding wrapper.
				// If we don't do this, subclasses might think they are introducing a new method, instead of overriding
				// this one.
				wrapper.AddMethod(MethodWrapper.Create(wrapper, md, mb, mb, Modifiers.Public | Modifiers.Abstract, true));
				// NOTE if the interface method name is remapped, we need to add an explicit methodoverride. Note that when this
				// is required we always need to emit this stub, even if the above mentioned bug is fixed in the CLR
				if(md.Name != ifmethod.Name)
				{
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
				}
			}
		}
	}

	internal void ImplementInterfaceMethodStubs(TypeBuilder typeBuilder, DynamicTypeWrapper wrapper, Hashtable doneSet)
	{
		// TODO interfaces that implement other interfaces need to be handled as well...
		if(!IsInterface)
		{
			throw new InvalidOperationException();
		}
		// make sure we don't do the same method twice
		if(doneSet.ContainsKey(this))
		{
			return;
		}
		doneSet.Add(this, this);
		Finish();
		// NOTE for dynamic types it isn't legal to call Type.GetMethods() (because
		// that might trigger finishing of types that are already in the process of
		// being finished) and for RemappedTypeWrappers it makes no sense, so both
		// of these (ab)use the methods hashtable to obtain a list of methods
		// NOTE since the types have been finished, we know for sure that all methods
		// are in fact in the methods cache
		if(Type.Assembly is AssemblyBuilder || this is RemappedTypeWrapper)
		{
			foreach(MethodWrapper method in methods.Values)
			{
				MethodBase ifmethod = method.GetMethod();
				if(!ifmethod.IsStatic)
				{
					ImplementInterfaceMethodStubImpl(method.Descriptor, ifmethod, typeBuilder, wrapper);
				}
			}
		}
		else
		{
			MethodInfo[] methods = Type.GetMethods();
			for(int i = 0; i < methods.Length; i++)
			{
				MethodInfo ifmethod = methods[i];
				if(!ifmethod.IsStatic)
				{
					ImplementInterfaceMethodStubImpl(MethodDescriptor.FromMethodBase(ifmethod), ifmethod, typeBuilder, wrapper);
				}
			}
		}
		TypeWrapper[] interfaces = Interfaces;
		for(int i = 0; i < interfaces.Length; i++)
		{
			interfaces[i].ImplementInterfaceMethodStubs(typeBuilder, wrapper, doneSet);
		}
	}

	internal virtual void ImplementOverrideStubsAndVirtuals(TypeBuilder typeBuilder, DynamicTypeWrapper wrapper, Hashtable methodLookup)
	{
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
}

class UnloadableTypeWrapper : TypeWrapper
{
	internal UnloadableTypeWrapper(string name)
		: base(TypeWrapper.UnloadableModifiersHack, name, null, null)
	{
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		throw new InvalidOperationException("GetFieldImpl called on UnloadableTypeWrapper: " + Name);
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		throw new InvalidOperationException("GetMethodImpl called on UnloadableTypeWrapper: " + Name);
	}

	public override Type Type
	{
		get
		{
			throw new InvalidOperationException("get_Type called on UnloadableTypeWrapper: " + Name);
		} 
	} 

	public override TypeWrapper[] Interfaces
	{
		get
		{
			throw new InvalidOperationException("get_Interfaces called on UnloadableTypeWrapper: " + Name);
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			throw new InvalidOperationException("get_InnerClasses called on UnloadableTypeWrapper: " + Name);
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			throw new InvalidOperationException("get_DeclaringTypeWrapper called on UnloadableTypeWrapper: " + Name);
		}
	}

	public override void Finish()
	{
		throw new InvalidOperationException("Finish called on UnloadableTypeWrapper: " + Name);
	}
}

class PrimitiveTypeWrapper : TypeWrapper
{
	internal static readonly PrimitiveTypeWrapper BYTE = new PrimitiveTypeWrapper(typeof(sbyte));
	internal static readonly PrimitiveTypeWrapper CHAR = new PrimitiveTypeWrapper(typeof(char));
	internal static readonly PrimitiveTypeWrapper DOUBLE = new PrimitiveTypeWrapper(typeof(double));
	internal static readonly PrimitiveTypeWrapper FLOAT = new PrimitiveTypeWrapper(typeof(float));
	internal static readonly PrimitiveTypeWrapper INT = new PrimitiveTypeWrapper(typeof(int));
	internal static readonly PrimitiveTypeWrapper LONG = new PrimitiveTypeWrapper(typeof(long));
	internal static readonly PrimitiveTypeWrapper SHORT = new PrimitiveTypeWrapper(typeof(short));
	internal static readonly PrimitiveTypeWrapper BOOLEAN = new PrimitiveTypeWrapper(typeof(bool));
	internal static readonly PrimitiveTypeWrapper VOID = new PrimitiveTypeWrapper(typeof(void));

	private readonly Type type;

	private PrimitiveTypeWrapper(Type type)
		: base(Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null, null, null)
	{
		this.type = type;
	}

	internal override ClassLoaderWrapper GetClassLoader()
	{
		return ClassLoaderWrapper.GetBootstrapClassLoader();
	}

	public override Type Type
	{
		get
		{
			return type;
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		return null;
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		return null;
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			// TODO does a primitive implement any interfaces?
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			return null;
		}
	}

	public override void Finish()
	{
	}
}

class DynamicTypeWrapper : TypeWrapper
{
	private DynamicImpl impl;
	private TypeWrapper[] interfaces;

	internal DynamicTypeWrapper(ClassFile f, ClassLoaderWrapper classLoader, Hashtable nativeMethods)
		: base(f.Modifiers, f.Name, f.IsInterface ? null : f.GetSuperTypeWrapper(classLoader), classLoader)
	{
		Profiler.Count("DynamicTypeWrapper");
		if(BaseTypeWrapper != null)
		{
			if(BaseTypeWrapper.IsUnloadable)
			{
				throw JavaException.NoClassDefFoundError(BaseTypeWrapper.Name);
			}
			if(!BaseTypeWrapper.IsAccessibleFrom(this))
			{
				throw JavaException.IllegalAccessError("Class {0} cannot access its superclass {1}", f.Name, BaseTypeWrapper.Name);
			}
			if(BaseTypeWrapper.IsFinal)
			{
				throw JavaException.VerifyError("Cannot inherit from final class");
			}
			if(BaseTypeWrapper.IsInterface)
			{
				throw JavaException.IncompatibleClassChangeError("Class {0} has interface {1} as superclass", f.Name, BaseTypeWrapper.Name);
			}
		}
		interfaces = f.GetInterfaceTypeWrappers(classLoader);
		for(int i = 0; i < interfaces.Length; i++)
		{
			if(interfaces[i].IsUnloadable)
			{
				throw JavaException.NoClassDefFoundError(interfaces[i].Name);
			}
			if(!interfaces[i].IsInterface)
			{
				throw JavaException.IncompatibleClassChangeError("Implementing class");
			}
			if(!interfaces[i].IsAccessibleFrom(this))
			{
				throw JavaException.IllegalAccessError("Class {0} cannot access its superinterface {1}", f.Name, interfaces[i].Name);
			}
		}

		impl = new JavaTypeImpl(f, this, nativeMethods);
	}

	internal override Modifiers ReflectiveModifiers
	{
		get
		{
			return impl.ReflectiveModifiers;
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		return impl.GetFieldImpl(fieldName);
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		return impl.GetMethodImpl(md);
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			return interfaces;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			return impl.InnerClasses;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			return impl.DeclaringTypeWrapper;
		}
	}

	public override Type Type
	{
		get
		{
			return impl.Type;
		}
	}

	public override void Finish()
	{
		lock(GetType())
		{
			Profiler.Enter("DynamicTypeWrapper.Finish");
			try
			{
				impl = impl.Finish();
			}
			finally
			{
				Profiler.Leave("DynamicTypeWrapper.Finish");
			}
		}
	}

	private abstract class DynamicImpl
	{
		public abstract FieldWrapper GetFieldImpl(string fieldName);
		public abstract MethodWrapper GetMethodImpl(MethodDescriptor md);
		public abstract Type Type { get; }
		public abstract TypeWrapper[] InnerClasses { get; }
		public abstract TypeWrapper DeclaringTypeWrapper { get; }
		public abstract Modifiers ReflectiveModifiers { get; }
		public abstract DynamicImpl Finish();
	}

	private class JavaTypeImpl : DynamicImpl
	{
		private readonly ClassFile classFile;
		private readonly DynamicTypeWrapper wrapper;
		private readonly TypeBuilder typeBuilder;
		private MethodWrapper[] methods;
		private FieldWrapper[] fields;
		private Hashtable methodLookup;
		private Hashtable fieldLookup;
		private bool finishing;
		private FinishedTypeImpl finishedType;
		private readonly Hashtable nativeMethods;
		private readonly TypeWrapper outerClassWrapper;

		internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper, Hashtable nativeMethods)
		{
			//Console.WriteLine("constructing JavaTypeImpl for " + f.Name);
			this.classFile = f;
			this.wrapper = wrapper;
			this.nativeMethods = nativeMethods;

			TypeAttributes typeAttribs = 0;
			if(f.IsAbstract)
			{
				typeAttribs |= TypeAttributes.Abstract;
			}
			if(f.IsFinal)
			{
				typeAttribs |= TypeAttributes.Sealed;
			}
			TypeBuilder outer = null;
			// only if requested, we compile inner classes as nested types, because it has a higher cost
			// and doesn't buy us anything, unless we're compiling a library that could be used from C# (e.g.)
			if(JVM.CompileInnerClassesAsNestedTypes)
			{
				if(f.OuterClass != null)
				{
					outerClassWrapper = wrapper.GetClassLoader().LoadClassByDottedName(f.OuterClass.Name);
					if(outerClassWrapper is DynamicTypeWrapper)
					{
						outer = (TypeBuilder)outerClassWrapper.Type;
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
					// TODO in the CLR interfaces cannot contain nested types!
					typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs);
				}
				else
				{
					typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(wrapper.GetClassLoader().MangleTypeName(f.Name), typeAttribs);
				}
			}
			else
			{
				typeAttribs |= TypeAttributes.Class;
				if(outer != null)
				{
					// TODO in the CLR interfaces cannot contain nested types!
					typeBuilder = outer.DefineNestedType(GetInnerClassName(outerClassWrapper.Name, f.Name), typeAttribs, wrapper.BaseTypeWrapper.Type);
				}
				else
				{
					typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(wrapper.GetClassLoader().MangleTypeName(f.Name), typeAttribs, wrapper.BaseTypeWrapper.Type);
				}
			}
			TypeWrapper[] interfaces = wrapper.Interfaces;
			for(int i = 0; i < interfaces.Length; i++)
			{
				typeBuilder.AddInterfaceImplementation(interfaces[i].Type);
				AttributeHelper.ImplementsAttribute(typeBuilder, interfaces[i].Type);
			}
		}

		private static string GetInnerClassName(string outer, string inner)
		{
			if(inner.Length <= (outer.Length + 1) || inner[outer.Length] != '$' || inner.IndexOf('$', outer.Length + 1) >= 0)
			{
				throw new InvalidOperationException(string.Format("Inner class name {0} is not well formed wrt outer class {1}", inner, outer));
			}
			return inner.Substring(outer.Length + 1);
		}

		public override DynamicImpl Finish()
		{
			if(wrapper.BaseTypeWrapper != null)
			{
				// make sure that the base type is already finished (because we need any Miranda methods it
				// might introduce to be visible)
				wrapper.BaseTypeWrapper.Finish();
			}
			if(outerClassWrapper != null)
			{
				outerClassWrapper.Finish();
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
				wrapper.Interfaces[i].Finish();
			}
			// make sure all classes are loaded, before we start finishing the type. During finishing, we
			// may not run any Java code, because that might result in a request to finish the type that we
			// are in the process of finishing, and this would be a problem.
			classFile.LoadAllReferencedTypes(wrapper.GetClassLoader());
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
				Modifiers reflectiveModifiers = wrapper.Modifiers;
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
							if(classFile.GetConstantPoolClassType(innerclasses[i].outerClass, wrapper.GetClassLoader()) == wrapper)
							{
								wrappers.Add(classFile.GetConstantPoolClassType(innerclasses[i].innerClass, wrapper.GetClassLoader()));
							}
							if(classFile.GetConstantPoolClassType(innerclasses[i].innerClass, wrapper.GetClassLoader()) == wrapper)
							{
								declaringTypeWrapper = classFile.GetConstantPoolClassType(innerclasses[i].outerClass, wrapper.GetClassLoader());
								reflectiveModifiers = innerclasses[i].accessFlags;
								Type[] argTypes = new Type[] { typeof(string), typeof(string), typeof(string), typeof(Modifiers) };
								object[] args = new object[] {
									classFile.GetConstantPoolClass(innerclasses[i].innerClass),
									classFile.GetConstantPoolClass(innerclasses[i].outerClass),
									innerclasses[i].name == 0 ? null : classFile.GetConstantPoolUtf8String(innerclasses[i].name),
									reflectiveModifiers
								};
								ConstructorInfo ci = typeof(InnerClassAttribute).GetConstructor(argTypes);
								CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(ci, args);
								typeBuilder.SetCustomAttribute(customAttributeBuilder);
							}
						}
					}
					innerClassesTypeWrappers = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
				}
				//Console.WriteLine("finishing TypeFactory for " + classFile.Name);
				if(fieldLookup == null)
				{
					fields = new FieldWrapper[classFile.Fields.Length];
					fieldLookup = new Hashtable();
					for(int i = 0; i < classFile.Fields.Length; i++)
					{
						fieldLookup[classFile.Fields[i].Name] = i;
					}
				}
				for(int i = 0; i < fields.Length; i++)
				{
					if(fields[i] == null)
					{
						GenerateField(i);
						wrapper.AddField(fields[i]);
					}
				}
				MethodDescriptor[] methodDescriptors = new MethodDescriptor[classFile.Methods.Length];
				for(int i = 0; i < classFile.Methods.Length; i++)
				{
					methodDescriptors[i] = new MethodDescriptor(wrapper.GetClassLoader(), classFile.Methods[i]);
				}
				if(methodLookup == null)
				{
					methods = new MethodWrapper[classFile.Methods.Length];
					methodLookup = new Hashtable();
					for(int i = 0; i < classFile.Methods.Length; i++)
					{
						methodLookup[methodDescriptors[i]] = i;
					}
				}
				for(int i = 0; i < methods.Length; i++)
				{
					if(methods[i] == null)
					{
						GenerateMethod(i);
						wrapper.AddMethod(methods[i]);
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
						MethodWrapper[] methods = parent.GetMethods();
						for(int i = 0; i < methods.Length; i++)
						{
							MethodInfo mi = methods[i].GetMethod() as MethodInfo;
							MethodDescriptor md = methods[i].Descriptor;
							if(mi != null && mi.IsAbstract && wrapper.GetMethodWrapper(md, true).IsAbstract)
							{
								// NOTE in Sun's JRE 1.4.1 this method cannot be overridden by subclasses,
								// but I think this is a bug, so we'll support it anyway.
								MethodBuilder mb = typeBuilder.DefineMethod(mi.Name, mi.Attributes & ~(MethodAttributes.Abstract|MethodAttributes.NewSlot), CallingConventions.Standard, md.RetType, md.ArgTypes);
								AttributeHelper.SetModifiers(mb, methods[i].Modifiers);
								EmitHelper.Throw(mb.GetILGenerator(), "java.lang.AbstractMethodError", wrapper.Name + "." + md.Name + md.Signature);
							}
						}
						parent = parent.BaseTypeWrapper;
					}
				}
				bool basehasclinit = (wrapper.BaseTypeWrapper == null) ? false : wrapper.BaseTypeWrapper.Type.TypeInitializer != null;
				bool hasclinit = false;
				for(int i = 0; i < methods.Length; i++)
				{
					ILGenerator ilGenerator;
					MethodBase mb = methods[i].GetMethod();
					if(mb is ConstructorBuilder)
					{
						ilGenerator = ((ConstructorBuilder)mb).GetILGenerator();
						if(basehasclinit && classFile.Methods[i].Name == "<clinit>" && classFile.Methods[i].Signature == "()V" && !classFile.IsInterface)
						{
							hasclinit = true;
							EmitHelper.RunClassConstructor(ilGenerator, Type.BaseType);
						}
					}
					else if(mb != null)
					{
						ilGenerator = ((MethodBuilder)mb).GetILGenerator();
					}
					else
					{
						// HACK methods that have unloadable types in the signature do not have an underlying method, so we end
						// up here
						continue;
					}
					ClassFile.Method m = classFile.Methods[i];
					if(m.IsAbstract)
					{
						// NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
						// the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
						if(!m.ClassFile.IsAbstract && !m.ClassFile.IsInterface)
						{
							EmitHelper.Throw(ilGenerator, "java.lang.AbstractMethodError", m.ClassFile.Name + "." + m.Name + m.Signature);
						}
					}
					else if(m.IsNative)
					{
						Profiler.Enter("JavaTypeImpl.Finish.Native");
						try
						{
							if(mb is ConstructorBuilder)
							{
								AttributeHelper.SetModifiers((ConstructorBuilder)mb, m.Modifiers);
							}
							else
							{
								AttributeHelper.SetModifiers((MethodBuilder)mb, m.Modifiers);
							}
							// do we have a native implementation in map.xml?
							if(nativeMethods != null)
							{
								string key = classFile.Name + "." + m.Name + m.Signature;
								CodeEmitter opcodes = (CodeEmitter)nativeMethods[key];
								if(opcodes != null)
								{
									opcodes.Emit(ilGenerator);
									continue;
								}
							}
							// see if there exists a NativeCode class for this type
							Type nativeCodeType = Type.GetType("NativeCode." + classFile.Name);
							MethodInfo nativeMethod = null;
							if(nativeCodeType != null)
							{
								// TODO use better resolution
								nativeMethod = nativeCodeType.GetMethod(m.Name);
							}
							TypeWrapper[] args = m.GetArgTypes(wrapper.GetClassLoader());
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
									ilGenerator.Emit(OpCodes.Ldarg, j + add);
								}
								ilGenerator.Emit(OpCodes.Call, nativeMethod);
								TypeWrapper retTypeWrapper = m.GetRetType(wrapper.GetClassLoader());
								if(!retTypeWrapper.Type.Equals(nativeMethod.ReturnType) && !retTypeWrapper.IsGhost)
								{
									ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.Type);
								}
								ilGenerator.Emit(OpCodes.Ret);
							}
							else
							{
								if(JVM.NoJniStubs)
								{
									//Console.WriteLine("Native method not implemented: " + classFile.Name + "." + m.Name + m.Signature);
									EmitHelper.Throw(ilGenerator, "java.lang.UnsatisfiedLinkError", "Native method not implemented: " + classFile.Name + "." + m.Name + m.Signature);
								}
								else
								{
									JniBuilder.Generate(ilGenerator, wrapper, typeBuilder, m, args);
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
						Compiler.Compile(wrapper, m, ilGenerator, wrapper.GetClassLoader());
					}
				}
				if(!classFile.IsInterface)
				{
					// if we don't have a <clinit> we need to inject one, to call the base class <clinit>
					if(basehasclinit && !hasclinit)
					{
						ConstructorBuilder cb = typeBuilder.DefineTypeInitializer();
						AttributeHelper.HideFromReflection(cb);
						ILGenerator ilGenerator = cb.GetILGenerator();
						EmitHelper.RunClassConstructor(ilGenerator, Type.BaseType);
						ilGenerator.Emit(OpCodes.Ret);
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
					wrapper.BaseTypeWrapper.ImplementOverrideStubsAndVirtuals(typeBuilder, wrapper, methodLookup);
				}

				Type type;
				Profiler.Enter("TypeBuilder.CreateType");
				try
				{
					type = typeBuilder.CreateType();
				}
				finally
				{
					Profiler.Leave("TypeBuilder.CreateType");
				}
				ClassLoaderWrapper.SetWrapperForType(type, wrapper);
				finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, reflectiveModifiers);
				return finishedType;
			}
			catch(Exception x)
			{
				Console.WriteLine("****** Exception during finishing of {0} ******", wrapper.Name);
				Console.WriteLine(x);
				Console.WriteLine(new StackTrace(x));
				Console.WriteLine(new StackTrace(true));
				// we bail out, because there is not much chance that we can continue to run after this
				Environment.Exit(1);
				return null;
			}
			finally
			{
				Profiler.Leave("JavaTypeImpl.Finish.Core");
			}
		}

		private class JniBuilder
		{
			private static readonly Type localRefStructType = JVM.JniProvider.GetLocalRefStructType();
			private static readonly MethodInfo jniFuncPtrMethod = JVM.JniProvider.GetJniFuncPtrMethod();
			private static readonly MethodInfo enterLocalRefStruct = localRefStructType.GetMethod("Enter");
			private static readonly MethodInfo leaveLocalRefStruct = localRefStructType.GetMethod("Leave");
			private static readonly MethodInfo makeLocalRef = localRefStructType.GetMethod("MakeLocalRef");
			private static readonly MethodInfo unwrapLocalRef = localRefStructType.GetMethod("UnwrapLocalRef");
			private static readonly MethodInfo getTypeFromHandle = typeof(Type).GetMethod("GetTypeFromHandle");
			private static readonly MethodInfo getClassFromType = typeof(NativeCode.java.lang.VMClass).GetMethod("getClassFromType");
			private static readonly MethodInfo writeLine = typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }, null);

			internal static void Generate(ILGenerator ilGenerator, TypeWrapper wrapper, TypeBuilder typeBuilder, ClassFile.Method m, TypeWrapper[] args)
			{
				FieldBuilder methodPtr = typeBuilder.DefineField(m.Name + "$Ptr", typeof(IntPtr), FieldAttributes.Static | FieldAttributes.PrivateScope);
				LocalBuilder localRefStruct = ilGenerator.DeclareLocal(localRefStructType);
				ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
				ilGenerator.Emit(OpCodes.Initobj, localRefStructType);
				ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
				Label oklabel = ilGenerator.DefineLabel();
				ilGenerator.Emit(OpCodes.Brtrue, oklabel);
				ilGenerator.Emit(OpCodes.Ldstr, m.Name);
				ilGenerator.Emit(OpCodes.Ldstr, m.Signature.Replace('.', '/'));
				ilGenerator.Emit(OpCodes.Ldstr, m.ClassFile.Name.Replace('.', '/'));
				ilGenerator.Emit(OpCodes.Call, jniFuncPtrMethod);
				ilGenerator.Emit(OpCodes.Stsfld, methodPtr);
				ilGenerator.MarkLabel(oklabel);
				ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
				ilGenerator.Emit(OpCodes.Call, enterLocalRefStruct);
				LocalBuilder jnienv = ilGenerator.DeclareLocal(typeof(IntPtr));
				ilGenerator.Emit(OpCodes.Stloc, jnienv);
				Label tryBlock = ilGenerator.BeginExceptionBlock();
				Type retType = m.GetRetType(wrapper.GetClassLoader()).Type;
				if(!retType.IsValueType && retType != typeof(void))
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
					modargs[i + 2] = args[i].Type;
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
					ilGenerator.Emit(OpCodes.Ldtoken, wrapper.Type);
					ilGenerator.Emit(OpCodes.Call, getTypeFromHandle);
					ilGenerator.Emit(OpCodes.Call, getClassFromType);
					ilGenerator.Emit(OpCodes.Call, makeLocalRef);
				}
				for(int j = 0; j < args.Length; j++)
				{
					if(!args[j].Type.IsValueType)
					{
						ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
						ilGenerator.Emit(OpCodes.Ldarg, j + add);
						ilGenerator.Emit(OpCodes.Call, makeLocalRef);
						modargs[j + 2] = typeof(IntPtr);
					}
					else
					{
						ilGenerator.Emit(OpCodes.Ldarg, j + add);
					}
				}
				ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
				Type returnType =(retType.IsValueType || retType == typeof(void)) ? retType : typeof(IntPtr);
				ilGenerator.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, returnType, modargs);
				LocalBuilder retValue = null;
				if(retType != typeof(void))
				{
					if(!retType.IsValueType)
					{
						ilGenerator.Emit(OpCodes.Call, unwrapLocalRef);
						ilGenerator.Emit(OpCodes.Castclass, retType);
					}
					retValue = ilGenerator.DeclareLocal(retType);
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
				if(retType != typeof(void))
				{
					ilGenerator.Emit(OpCodes.Ldloc, retValue);
				}
				ilGenerator.Emit(OpCodes.Ret);
			}
		}

		public override TypeWrapper[] InnerClasses
		{
			get
			{
				throw new InvalidOperationException("InnerClasses is only available for finished types");
			}
		}

		public override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				throw new InvalidOperationException("DeclaringTypeWrapper is only available for finished types");
			}
		}

		public override Modifiers ReflectiveModifiers
		{
			get
			{
				throw new InvalidOperationException("ReflectiveModifiers is only available for finished types");
			}
		}

		public override FieldWrapper GetFieldImpl(string fieldName)
		{
			if(fieldLookup == null)
			{
				fields = new FieldWrapper[classFile.Fields.Length];
				fieldLookup = new Hashtable();
				for(int i = 0; i < classFile.Fields.Length; i++)
				{
					fieldLookup[classFile.Fields[i].Name] = i;
				}
			}
			object index = fieldLookup[fieldName];
			if(index != null)
			{
				int i = (int)index;
				if(fields[i] == null)
				{
					GenerateField(i);
				}
				return fields[i];
			}
			return null;
		}

		private void GenerateField(int i)
		{
			Profiler.Enter("JavaTypeImpl.GenerateField");
			try
			{
				FieldBuilder field;
				ClassFile.Field fld = classFile.Fields[i];
				TypeWrapper typeWrapper = fld.GetFieldType(wrapper.GetClassLoader());
				Type type = typeWrapper.TypeOrUnloadableAsObject;
				if(typeWrapper.IsUnloadable)
				{
					// TODO the field name should be mangled here, because otherwise it might conflict with another field
					// with the same name and a different unloadable type (or java.lang.Object as its type)
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
					field = typeBuilder.DefineField(fld.Name, type, attribs);
					field.SetConstant(constantValue);
					// NOTE even though you're not supposed to access a constant static final (the compiler is supposed
					// to inline them), we have to support it (because it does happen, e.g. if the field becomes final
					// after the referencing class was compiled)
					CodeEmitter emitGet = CodeEmitter.CreateLoadConstant(constantValue);
					// when non-blank final fields are updated, the JIT normally doesn't see that (because the
					// constant value is inlined), so we emulate that behavior by emitting a Pop
					CodeEmitter emitSet = CodeEmitter.Pop;
					fields[i] = FieldWrapper.Create(wrapper, fld.GetFieldType(wrapper.GetClassLoader()), fld.Name, fld.Signature, fld.Modifiers, emitGet, emitSet);
				}
				else
				{
					if(fld.IsFinal)
					{
						// final doesn't make sense for private fields, so if the field is private we ignore final
						if(!fld.IsPrivate && !wrapper.IsInterface)
						{
							// NOTE blank final fields get converted into a read-only property with a private field backing store
							// we used to make the field privatescope, but that really serves no purpose (and it hinders serialization,
							// which uses .NET reflection to get at the field)
							attribs &= ~FieldAttributes.FieldAccessMask;
							attribs |= FieldAttributes.Private;
							setModifiers = true;
						}
					}
					field = typeBuilder.DefineField(fld.Name, type, attribs);
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
					if(fld.IsFinal && !fld.IsPrivate && !wrapper.IsInterface)
					{
						methodAttribs |= MethodAttributes.SpecialName;
						// TODO we should ensure that the getter method name doesn't clash with an existing method
						MethodBuilder getter = typeBuilder.DefineMethod("get_" + fld.Name, methodAttribs, CallingConventions.Standard, type, Type.EmptyTypes);
						AttributeHelper.HideFromReflection(getter);
						ILGenerator ilgen = getter.GetILGenerator();
						if(fld.IsVolatile)
						{
							ilgen.Emit(OpCodes.Volatile);
						}
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
						CodeEmitter emitGet = CodeEmitter.Create(OpCodes.Call, getter);
						CodeEmitter emitSet = null;
						if(fld.IsVolatile)
						{
							emitSet += CodeEmitter.Volatile;
						}
						if(fld.IsStatic)
						{
							emitSet += CodeEmitter.Create(OpCodes.Stsfld, field);
						}
						else
						{
							emitSet += CodeEmitter.Create(OpCodes.Stfld, field);
						}
						fields[i] = FieldWrapper.Create(wrapper, fld.GetFieldType(wrapper.GetClassLoader()), fld.Name, fld.Signature, fld.Modifiers, emitGet, emitSet);
					}
					else
					{
						fields[i] = FieldWrapper.Create(wrapper, fld.GetFieldType(wrapper.GetClassLoader()), field, fld.Signature, fld.Modifiers);
					}
				}
				if(typeWrapper.IsUnloadable)
				{
					AttributeHelper.SetUnloadableType(field, typeWrapper.Name);
				}
				if(typeWrapper.IsGhost)
				{
					AttributeHelper.SetGhostType(field, typeWrapper.Type);
				}
				// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
				// the Java modifiers
				if(setModifiers)
				{
					AttributeHelper.SetModifiers(field, fld.Modifiers);
				}
			}
			finally
			{
				Profiler.Leave("JavaTypeImpl.GenerateField");
			}
		}

		public override MethodWrapper GetMethodImpl(MethodDescriptor md)
		{
			if(methodLookup == null)
			{
				methods = new MethodWrapper[classFile.Methods.Length];
				methodLookup = new Hashtable();
				for(int i = 0; i < classFile.Methods.Length; i++)
				{
					Profiler.Count("DynamicMethod");
					methodLookup[new MethodDescriptor(wrapper.GetClassLoader(), classFile.Methods[i])] = i;
				}
			}
			object index = methodLookup[md];
			if(index != null)
			{
				int i = (int)index;
				if(methods[i] == null)
				{
					GenerateMethod(i);
				}
				return methods[i];
			}
			return null;
		}

		private void GenerateMethod(int index)
		{
			Profiler.Enter("JavaTypeImpl.GenerateMethod");
			try
			{
				Debug.Assert(methods[index] == null);
				// TODO things to consider when we support unloadable types on the argument list on return type:
				// - later on, the method can be overriden by a class that does have access to the type, so
				//   this should be detected and an appropriate override stub should be generated
				// - overloading might conflict with the generalised argument list (unloadable types appear
				//   as System.Object). The nicest way to solve this would be to emit a modreq attribute on the parameter,
				//   but Reflection.Emit doesn't support this, so we'll probably have to use a name mangling scheme
				MethodBase method;
				ClassFile.Method m = classFile.Methods[index];
				TypeWrapper[] argTypeWrappers = m.GetArgTypes(wrapper.GetClassLoader());
				TypeWrapper retTypeWrapper = m.GetRetType(wrapper.GetClassLoader());
				Type[] args = new Type[argTypeWrappers.Length];
				Type retType = retTypeWrapper.TypeOrUnloadableAsObject;
				for(int i = 0; i < args.Length; i++)
				{
					args[i] = argTypeWrappers[i].TypeOrUnloadableAsObject;
				}
				bool setModifiers = false;
				MethodAttributes attribs = 0;
				if(m.IsAbstract)
				{
					// only if the classfile is abstract, we make the CLR method abstract, otherwise,
					// we have to generate a method that throws an AbstractMethodError (because the JVM
					// allows abstract methods in non-abstract classes)
					if(m.ClassFile.IsAbstract || m.ClassFile.IsInterface)
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
				if(m.IsStatic)
				{
					attribs |= MethodAttributes.Static;
				}
				if(m.Name == "<init>")
				{
					// NOTE we don't need to record the modifiers here, because only access modifiers are valid for
					// constructors and we have a well defined (reversible) mapping from them
					method = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, args);
					if(JVM.IsStaticCompiler)
					{
						AddParameterNames(method, m);
					}
					ParameterBuilder[] parameterBuilders = null;
					if(JVM.IsStaticCompiler)
					{
						parameterBuilders = AddParameterNames(method, m);
					}
					for(int i = 0; i < argTypeWrappers.Length; i++)
					{
						if(argTypeWrappers[i].IsGhost)
						{
							ParameterBuilder pb = (parameterBuilders != null) ? parameterBuilders[i] : null;
							if(pb == null)
							{
								pb = ((ConstructorBuilder)method).DefineParameter(i + 1, ParameterAttributes.None, null);
							}
							AttributeHelper.SetGhostType(pb, argTypeWrappers[i].Type);
						}
					}
				}
				else if(m.Name == "<clinit>" && m.Signature == "()V")
				{
					// NOTE we don't need to record the modifiers here, because they aren't visible from Java reflection
					// (well they might be visible from JNI reflection, but that isn't important enough to justify the custom attribute)
					// HACK because Peverify is complaining about private methods in interfaces, I'm making them public for the time being
					method = typeBuilder.DefineConstructor(MethodAttributes.Static | MethodAttributes.Public, CallingConventions.Standard, Type.EmptyTypes);
					//method = typeBuilder.DefineTypeInitializer();
				}
				else
				{
					if(!m.IsPrivate && !m.IsStatic)
					{
						attribs |= MethodAttributes.Virtual;
					}
					string name = m.Name;
					MethodDescriptor md = new MethodDescriptor(wrapper.GetClassLoader(), m);
					// if a method is virtual, we need to find the method it overrides (if any), for several reasons:
					// - if we're overriding a method that has a different name (e.g. some of the virtual methods
					//   in System.Object [Equals <-> equals]) we need to add an explicit MethodOverride
					// - if one of the base classes has a similar method that is private (or package) that we aren't
					//   overriding, we need to specify an explicit MethodOverride
					MethodBase baseMethod = null;
					MethodWrapper baseMce = null;
					bool explicitOverride = false;
					if((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
					{
						TypeWrapper tw = wrapper.BaseTypeWrapper;
						while(tw != null)
						{
							baseMce = tw.GetMethodWrapper(md, true);
							if(baseMce == null)
							{
								break;
							}
							// here are the complex rules for determining whether this method overrides the method we found
							// RULE 1: final methods may not be overriden
							if(baseMce.IsFinal)
							{
								// NOTE we don't need to test for our method being private, because if it is
								// we'll never get here (because private methods aren't virtual)
								// TODO make sure the VerifyError is translated into a java.lang.VerifyError
								throw new VerifyError("final method " + baseMce.Name + baseMce.Descriptor.Signature + " in " + tw.Name + " is overriden in " + wrapper.Name);
							}
							// RULE 2: public & protected methods can be overridden (package methods are handled by RULE 4)
							// (by public, protected & *package* methods [even if they are in a different package])
							if(baseMce.IsPublic || baseMce.IsProtected)
							{
								// if we already encountered a package method, we cannot override the base method of
								// that package method
								if(explicitOverride)
								{
									explicitOverride = false;
									break;
								}
								// if our method's accessibility is less than the method it overrides, we
								// need to make our method more accessible, because the CLR doesn't allow reducing access
								if((attribs & MethodAttributes.Public) == 0)
								{
									attribs &= ~MethodAttributes.MemberAccessMask;
									if(baseMce.IsPublic)
									{
										attribs |= MethodAttributes.Public;
									}
									else
									{
										attribs |= MethodAttributes.FamORAssem;
									}
								}
								baseMethod = baseMce.GetMethod();
								break;
							}
							// RULE 3: private methods are ignored
							if(!baseMce.IsPrivate)
							{
								// RULE 4: package methods can only be overridden in the same package
								if(baseMce.DeclaringType.IsInSamePackageAs(wrapper))
								{
									baseMethod = baseMce.GetMethod();
									break;
								}
								// since we encountered a method with the same name/signature that we aren't overriding,
								// we need to specify an explicit override
								// NOTE we only do this if baseMce isn't private, because if it is, Reflection.Emit
								// will complain about the explicit MethodOverride (possibly a bug)
								explicitOverride = true;
							}
							tw = baseMce.DeclaringType.BaseTypeWrapper;
						}
						if(baseMethod == null)
						{
							// we need set NewSlot here, to prevent accidentally overriding methods
							// (for example, if a Java class has a method "boolean Equals(object)", we don't want that method
							// to override System.Object.Equals)
							// Unless, of course, we're implementing an inherited interface method (the miranda method might not
							// have been created at this point, because that happens during the finishing of our base class)
							// TODO a better way to fix this would be to move Miranda method creation from Finish to GetMethodImpl
							if(wrapper.BaseTypeWrapper == null || !IsInterfaceMethod(wrapper.BaseTypeWrapper, md))
							{
								attribs |= MethodAttributes.NewSlot;
							}
						}
						else
						{
							// if we have a method overriding a more accessible method (yes, this does work), we need to make the
							// method more accessible, because otherwise the CLR will complain that we're reducing access)
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
					MethodBuilder mb = typeBuilder.DefineMethod(name, attribs, retType, args);
					ParameterBuilder[] parameterBuilders = null;
					if(JVM.IsStaticCompiler)
					{
						parameterBuilders = AddParameterNames(mb, m);
					}
					for(int i = 0; i < argTypeWrappers.Length; i++)
					{
						if(argTypeWrappers[i].IsGhost)
						{
							ParameterBuilder pb = (parameterBuilders != null) ? parameterBuilders[i] : null;
							if(pb == null)
							{
								pb = mb.DefineParameter(i + 1, ParameterAttributes.None, null);
							}
							AttributeHelper.SetGhostType(pb, argTypeWrappers[i].Type);
						}
					}
					if(retTypeWrapper.IsGhost)
					{
						AttributeHelper.SetGhostType(mb, retTypeWrapper.Type);
					}
					if(setModifiers)
					{
						AttributeHelper.SetModifiers(mb, m.Modifiers);
					}
					// if we're public and we're overriding a method that is not public, then we might be also
					// be implementing an interface method that has an IllegalAccessError stub
					// Example:
					//   class Base {
					//     protected void Foo() {}
					//   }
					//   interface IFoo {
					//     public void Foo();
					//   }
					//   class Derived extends Base implements IFoo {
					//   }
					//   class MostDerived extends Derived {
					//     public void Foo() {} 
					//   }
					// TODO this implementation isn't correct. I need to find out what happens for the following:
					//   class Base {
					//     public void Foo() {}
					//   }
					//   interface IFoo {
					//     public void Foo() {}
					//   }
					//   class Derived extends Base implements IFoo {
					//   }
					//   class MostDerived extends Derived {
					//     protected void Foo() {}
					//   }
					if(wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasIncompleteInterfaceImplementation)
					{
						Hashtable hashtable = null;
						TypeWrapper tw = wrapper.BaseTypeWrapper;
						while(tw.HasIncompleteInterfaceImplementation)
						{
							foreach(TypeWrapper iface in tw.Interfaces)
							{
								AddMethodOverride(typeBuilder, mb, iface, md, ref hashtable);
							}
							tw = tw.BaseTypeWrapper;
						}
					}
					method = mb;
					// since Java constructors (and static intializers) aren't allowed to be synchronized, we only check this here
					if(m.IsSynchronized)
					{
						mb.SetImplementationFlags(method.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);
					}
					if(baseMethod != null && (explicitOverride || baseMethod.Name != name))
					{
						// assert that the method we're overriding is in fact virtual and not final!
						Debug.Assert(baseMethod.IsVirtual && !baseMethod.IsFinal);
						typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethod);
					}
				}
				if(retTypeWrapper.IsUnloadable)
				{
					CustomAttributeBuilder attrib = new CustomAttributeBuilder(typeof(UnloadableTypeAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { retTypeWrapper.Name });
					// NOTE since DefineParameter(0, ...) throws an exception (bug in .NET, I believe),
					// we attach the attribute to the method instead of the return value
					((MethodBuilder)method).SetCustomAttribute(attrib);
				}
				for(int i = 0; i < argTypeWrappers.Length; i++)
				{
					if(argTypeWrappers[i].IsUnloadable)
					{
						CustomAttributeBuilder attrib = new CustomAttributeBuilder(typeof(UnloadableTypeAttribute).GetConstructor(new Type[] { typeof(string) }), new object[] { argTypeWrappers[i].Name });
						if(method is MethodBuilder)
						{
							((MethodBuilder)method).DefineParameter(i + 1, ParameterAttributes.None, null).SetCustomAttribute(attrib);
						}
						else
						{
							((ConstructorBuilder)method).DefineParameter(i + 1, ParameterAttributes.None, null).SetCustomAttribute(attrib);
						}
					}
				}
				methods[index] = MethodWrapper.Create(wrapper, new MethodDescriptor(wrapper.GetClassLoader(), m), method, method, m.Modifiers, false);
			}
			finally
			{
				Profiler.Leave("JavaTypeImpl.GenerateMethod");
			}
		}

		private static ParameterBuilder[] AddParameterNames(MethodBase mb, ClassFile.Method m)
		{
			if(m.CodeAttribute != null)
			{
				ClassFile.Method.LocalVariableTableEntry[] localVars = m.CodeAttribute.LocalVariableTableAttribute;
				if(localVars != null)
				{
					int bias = 1;
					if(m.IsStatic)
					{
						bias = 0;
					}
					ParameterBuilder[] parameterBuilders = new ParameterBuilder[m.CodeAttribute.ArgMap.Length - bias];
					for(int i = bias; i < m.CodeAttribute.ArgMap.Length; i++)
					{
						if(m.CodeAttribute.ArgMap[i] != -1)
						{
							for(int j = 0; j < localVars.Length; j++)
							{
								if(localVars[j].index == i)
								{
									if(mb is MethodBuilder)
									{
										parameterBuilders[i - bias] = ((MethodBuilder)mb).DefineParameter(m.CodeAttribute.ArgMap[i] + 1 - bias, ParameterAttributes.None, localVars[j].name);
									}
									else if(mb is ConstructorBuilder)
									{
										parameterBuilders[i - bias] = ((ConstructorBuilder)mb).DefineParameter(m.CodeAttribute.ArgMap[i], ParameterAttributes.None, localVars[j].name);
									}
									break;
								}
							}
						}
					}
					return parameterBuilders;
				}
			}
			return null;
		}

		private static bool IsInterfaceMethod(TypeWrapper wrapper, MethodDescriptor md)
		{
			foreach(TypeWrapper iface in wrapper.Interfaces)
			{
				if(iface.GetMethodWrapper(md, false) != null || IsInterfaceMethod(iface, md))
				{
					return true;
				}
			}
			return false;
		}

		private static void AddMethodOverride(TypeBuilder typeBuilder, MethodBuilder mb, TypeWrapper iface, MethodDescriptor md, ref Hashtable hashtable)
		{
			if(hashtable != null && hashtable.ContainsKey(iface))
			{
				return;
			}
			MethodWrapper mw = iface.GetMethodWrapper(md, false);
			if(mw != null)
			{
				if(hashtable == null)
				{
					hashtable = new Hashtable();
				}
				hashtable.Add(iface, iface);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
			}
			foreach(TypeWrapper iface2 in iface.Interfaces)
			{
				AddMethodOverride(typeBuilder, mb, iface2, md, ref hashtable);
			}
		}

		public override Type Type
		{
			get
			{
				return typeBuilder;
			}
		}
	}

	private class FinishedTypeImpl : DynamicImpl
	{
		private Type type;
		private TypeWrapper[] innerclasses;
		private TypeWrapper declaringTypeWrapper;
		private Modifiers reflectiveModifiers;

		public FinishedTypeImpl(Type type, TypeWrapper[] innerclasses, TypeWrapper declaringTypeWrapper, Modifiers reflectiveModifiers)
		{
			this.type = type;
			this.innerclasses = innerclasses;
			this.declaringTypeWrapper = declaringTypeWrapper;
			this.reflectiveModifiers = reflectiveModifiers;
		}

		public override TypeWrapper[] InnerClasses
		{
			get
			{
				// TODO compute the innerclasses lazily (and fix JavaTypeImpl to not always compute them)
				return innerclasses;
			}
		}

		public override TypeWrapper DeclaringTypeWrapper
		{
			get
			{
				// TODO compute lazily (and fix JavaTypeImpl to not always compute it)
				return declaringTypeWrapper;
			}
		}

		public override Modifiers ReflectiveModifiers
		{
			get
			{
				return reflectiveModifiers;
			}
		}

		public override FieldWrapper GetFieldImpl(string fieldName)
		{
			return null;
		}
	
		public override MethodWrapper GetMethodImpl(MethodDescriptor md)
		{
			return null;
		}

		public override Type Type
		{
			get
			{
				return type;
			}
		}

		public override DynamicImpl Finish()
		{
			return this;
		}
	}
}

class RemappedTypeWrapper : TypeWrapper
{
	private readonly Type type;
	private TypeWrapper[] interfaces;
	private bool virtualsInterfaceBuilt;
	private Type virtualsInterface;
	private Type virtualsHelperHack;

	public RemappedTypeWrapper(ClassLoaderWrapper classLoader, Modifiers modifiers, string name, Type type, TypeWrapper[] interfaces, TypeWrapper baseType)
		: base(modifiers, name, baseType, classLoader)
	{
		this.type = type;
		this.interfaces = interfaces;
	}

	public void LoadRemappings(MapXml.Class classMap)
	{
		bool hasOverrides = false;
		ArrayList methods = new ArrayList();
		if(classMap.Methods != null)
		{
			foreach(MapXml.Method method in classMap.Methods)
			{
				string name = method.Name;
				string sig = method.Sig;
				Modifiers modifiers = (Modifiers)method.Modifiers;
				bool isStatic = (modifiers & Modifiers.Static) != 0;
				MethodDescriptor md = new MethodDescriptor(GetClassLoader(), name, sig);
				if(method.invokevirtual == null &&
					method.invokespecial == null &&
					method.invokestatic == null &&
					method.redirect == null &&
					method.@override == null)
				{
					// TODO use a better way to get the method
					BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic;
					if(isStatic)
					{
						binding |= BindingFlags.Static;
					}
					else
					{
						binding |= BindingFlags.Instance;
					}
					MethodBase mb = type.GetMethod(name, binding, null, CallingConventions.Standard, md.ArgTypes, null);
					if(mb == null)
					{
						throw new InvalidOperationException("declared method: " + Name + "." + name + sig + " not found");
					}
					MethodWrapper mw = MethodWrapper.Create(this, md, mb, mb, modifiers, false);
					AddMethod(mw);
					methods.Add(mw);
				}
				else
				{
					CodeEmitter newopc = null;
					CodeEmitter invokespecial = null;
					CodeEmitter invokevirtual = null;
					CodeEmitter retcast = null;
					MethodBase redirect = null;
					MethodBase overrideMethod = null;
					if(method.redirect != null)
					{
						if(method.invokevirtual != null ||
							method.invokespecial != null ||
							method.invokestatic != null ||
							method.@override != null)
						{
							throw new InvalidOperationException();
						}
						if(method.redirect.Name != null)
						{
							name = method.redirect.Name;
						}
						if(method.redirect.Sig != null)
						{
							sig = method.redirect.Sig;
						}
						string stype = isStatic ? "static" : "instance";
						if(method.redirect.Type != null)
						{
							stype = method.redirect.Type;
						}
						MethodDescriptor redir = new MethodDescriptor(GetClassLoader(), name, sig);
						BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic;
						if(stype == "static")
						{
							binding |= BindingFlags.Static;
						}
						else
						{
							binding |= BindingFlags.Instance;
						}
						if(method.redirect.Class != null)
						{
							TypeWrapper tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(method.redirect.Class);
							if(tw is DynamicTypeWrapper)
							{
								MethodWrapper mw1 = tw.GetMethodWrapper(redir, false);
								if(mw1 == null)
								{
									Console.WriteLine("method not found: " + tw.Name + "." + redir.Name + redir.Signature);
								}
								redirect = mw1.GetMethod();
							}
							else
							{
								redirect = tw.Type.GetMethod(name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
							}
						}
						else
						{
							redirect = this.type.GetMethod(name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
						}
						if(redirect == null)
						{
							throw new InvalidOperationException("remapping method: " + name + sig + " not found");
						}
						string ret = md.Signature.Substring(md.Signature.IndexOf(')') + 1);
						// when constructors are remapped, we have to assume that the type is correct because the original
						// return type (of the constructor) is void.
						if(ret[0] != 'V' && ret != redir.Signature.Substring(redir.Signature.IndexOf(')') + 1))
						{
							if(ret[0] == 'L')
							{
								ret = ret.Substring(1, ret.Length - 2);
							}
							retcast = new CastEmitter(ret);
						}
						if(BaseTypeWrapper != null && !Type.IsSealed)
						{
							MethodWrapper mce1 = BaseTypeWrapper.GetMethodWrapper(md, true);
							if(mce1 != null)
							{
								MethodBase org = mce1.GetMethod();
								if(org != null)
								{
									ParameterInfo[] paramInfo = org.GetParameters();
									Type[] argTypes = new Type[paramInfo.Length];
									for(int i = 0; i < argTypes.Length; i++)
									{
										argTypes[i] = paramInfo[i].ParameterType;
									}
									BindingFlags binding1 = BindingFlags.Public | BindingFlags.NonPublic;
									if(isStatic)
									{
										// TODO this looks like total nonsense, a static method cannot override a method,
										// so we shouldn't ever get here
										binding1 |= BindingFlags.Static;
									}
									else
									{
										binding1 |= BindingFlags.Instance;
									}
									overrideMethod = type.GetMethod(org.Name, binding1, null, CallingConventions.Standard, argTypes, null);
								}
							}
						}
						MethodWrapper.CreateEmitters(overrideMethod, redirect, ref invokespecial, ref invokevirtual, ref newopc);
					}
					else
					{
						if(method.@override != null)
						{
							BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
							overrideMethod = type.GetMethod(method.@override.Name, binding, null, CallingConventions.Standard, GetClassLoader().ArgTypeListFromSig(sig), null);
							if(overrideMethod == null)
							{
								throw new InvalidOperationException("Override method not found: " + Name + "." + name + sig);
							}
						}
						invokespecial = method.invokespecial;
						invokevirtual = method.invokevirtual;
						if(method.invokestatic != null)
						{
							invokespecial = method.invokestatic;
						}
					}
					// if invokespecial isn't redefined, it means that the base class' implementation is correct,
					// so we don't need to generate an override stub for this method
					bool trivialOverride = (invokespecial == null);
					if(overrideMethod != null)
					{
						if(invokespecial == null)
						{
							invokespecial = CodeEmitter.Create(OpCodes.Call, (MethodInfo)overrideMethod);
						}
						if(invokevirtual == null)
						{
							invokevirtual = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)overrideMethod);
						}
					}
					// HACK MethodWrapper doesn't accept a MethodBuilder, so we have to blank it out, note
					// that this means that reflection won't work on this method, so we have to add support
					// for that
					// TODO support reflection
					if(redirect is MethodBuilder)
					{
						redirect = null;
					}
					MethodWrapper mw = new MethodWrapper(this, md, overrideMethod, redirect, modifiers, false);
					mw.EmitNewobj = newopc;
					mw.EmitCall = invokespecial;
					mw.EmitCallvirt = invokevirtual;
					if(retcast != null)
					{
						mw.EmitNewobj += retcast;
						mw.EmitCall += retcast;
						mw.EmitCallvirt += retcast;
					}
					// don't generate override stubs for trivial methods (i.e. methods that are only renamed)
					if(overrideMethod != null && !trivialOverride)
					{
						hasOverrides = true;
						mw.IsRemappedOverride = true;
					}
					if(method.Type == "virtual")
					{
						// TODO we're overwriting the retcast (if there is any). We shouldn't do this.
						mw.EmitCallvirt = new VirtualEmitter(md, this);
						mw.IsRemappedVirtual = true;
					}
					AddMethod(mw);
					methods.Add(mw);
				}
			}
		}
		if(classMap.Constructors != null)
		{
			foreach(MapXml.Constructor constructor in classMap.Constructors)
			{
				Modifiers modifiers = (Modifiers)constructor.Modifiers;
				MethodDescriptor md = new MethodDescriptor(GetClassLoader(), "<init>", constructor.Sig);
				if(constructor.invokespecial == null && constructor.newobj == null && constructor.redirect == null)
				{
					// TODO use a better way to get the method
					BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
					MethodBase method = type.GetConstructor(binding, null, CallingConventions.Standard, md.ArgTypes, null);
					if(method == null)
					{
						throw new InvalidOperationException("declared constructor: " + classMap.Name + constructor.Sig + " not found");
					}
					MethodWrapper mw = MethodWrapper.Create(this, md, method, method, modifiers, false);
					AddMethod(mw);
					methods.Add(mw);
				}
				else
				{
					CodeEmitter newopc = null;
					CodeEmitter invokespecial = null;
					CodeEmitter retcast = null;
					if(constructor.redirect != null)
					{
						if(constructor.invokespecial != null || constructor.newobj != null)
						{
							throw new InvalidOperationException();
						}
						string sig = constructor.Sig;
						if(constructor.redirect.Sig != null)
						{
							sig = constructor.redirect.Sig;
						}
						MethodDescriptor redir = new MethodDescriptor(GetClassLoader(), "<init>", sig);
						BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic;
						if(constructor.redirect.Type == "static")
						{
							binding |= BindingFlags.Static;
							if((classMap.Modifiers & MapXml.MapModifiers.Final) == 0)
							{
								// NOTE only final classes can have constructors redirected to static methods,
								// because we don't have support for making the distinction between new and invokespecial
								throw new InvalidOperationException();
							}
						}
						else
						{
							binding |= BindingFlags.Instance;
						}
						MethodBase redirect = null;
						if(constructor.redirect.Class != null)
						{
							TypeWrapper tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(constructor.redirect.Class);
							if(tw is DynamicTypeWrapper)
							{
								MethodDescriptor md1 = new MethodDescriptor(GetClassLoader(), constructor.redirect.Name != null ? constructor.redirect.Name : "<init>", sig);
								MethodWrapper mw1 = tw.GetMethodWrapper(md1, false);
								if(mw1 == null)
								{
									Console.WriteLine("constructor not found: " + tw.Name + "." + redir.Name + redir.Signature);
								}
								redirect = mw1.GetMethod();
							}
							else
							{
								if(constructor.redirect.Name != null)
								{
									redirect = tw.Type.GetMethod(constructor.redirect.Name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
								}
								else
								{
									redirect = tw.Type.GetConstructor(binding, null, CallingConventions.Standard, redir.ArgTypes, null);
								}
							}
						}
						else
						{
							if(constructor.redirect.Name != null)
							{
								redirect = this.type.GetMethod(constructor.redirect.Name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
							}
							else
							{
								redirect = this.type.GetConstructor(binding, null, CallingConventions.Standard, redir.ArgTypes, null);
							}
						}
						if(redirect == null)
						{
							throw new InvalidOperationException("remapping constructor: " + classMap.Name + constructor.Sig + " not found");
						}
						string ret = md.Signature.Substring(md.Signature.IndexOf(')') + 1);
						// when constructors are remapped, we have to assume that the type is correct because the original
						// return type (of the constructor) is void.
						// TODO we could look at return type of the redirected method and see if that matches the type of the
						// object we're supposed to be constructing
						if(ret[0] != 'V' && ret != redir.Signature.Substring(redir.Signature.IndexOf(')') + 1))
						{
							if(ret[0] == 'L')
							{
								ret = ret.Substring(1, ret.Length - 2);
							}
							retcast = new CastEmitter(ret);
						}
						CodeEmitter ignore = null;
						MethodWrapper.CreateEmitters(null, redirect, ref ignore, ref ignore, ref newopc);
					}
					else
					{
						newopc = constructor.newobj;
						invokespecial = constructor.invokespecial;
					}
					MethodWrapper mw = new MethodWrapper(this, md, null, null, modifiers, false);
					mw.EmitNewobj = newopc;
					mw.EmitCall = invokespecial;
					if(retcast != null)
					{
						mw.EmitNewobj += retcast;
						mw.EmitCall += retcast;
					}
					AddMethod(mw);
					methods.Add(mw);
				}
			}
		}
		if(classMap.Fields != null)
		{
			foreach(MapXml.Field field in classMap.Fields)
			{
				string name = field.Name;
				string sig = field.Sig;
				string fieldName = name;
				string fieldSig = sig;
				Modifiers modifiers = (Modifiers)field.Modifiers;
				bool isStatic = (modifiers & Modifiers.Static) != 0;
				if(field.redirect == null)
				{
					throw new InvalidOperationException();
				}
				// NOTE when fields are redirected it's always to a method!
				// NOTE only reading a field can be redirected!
				if(field.redirect.Name != null)
				{
					name = field.redirect.Name;
				}
				if(field.redirect.Sig != null)
				{
					sig = field.redirect.Sig;
				}
				string stype = isStatic ? "static" : "instance";
				if(field.redirect.Type != null)
				{
					stype = field.redirect.Type;
				}
				MethodDescriptor redir = new MethodDescriptor(GetClassLoader(), name, sig);
				BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic;
				if(stype == "static")
				{
					binding |= BindingFlags.Static;
				}
				else
				{
					binding |= BindingFlags.Instance;
				}
				Type t = this.type;
				if(field.redirect.Class != null)
				{
					t = ClassLoaderWrapper.GetType(field.redirect.Class);
				}
				MethodInfo method = t.GetMethod(name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
				if(method == null)
				{
					throw new InvalidOperationException("remapping method: " + name + sig + " not found");
				}
				CodeEmitter getter = CodeEmitter.Create(OpCodes.Call, method);
				// ensure that return type for redirected method matches with field type, or emit a castclass
				if(!field.redirect.Sig.EndsWith(fieldSig))
				{
					if(fieldSig[0] == 'L')
					{
						getter += new CastEmitter(fieldSig.Substring(1, fieldSig.Length - 2));
					}
					else if(fieldSig[0] == '[')
					{
						getter += new CastEmitter(fieldSig);
					}
					else
					{
						throw new InvalidOperationException("invalid field sig: " + fieldSig);
					}
				}
				CodeEmitter setter = CodeEmitter.Throw("java.lang.IllegalAccessError", "Redirected field " + this.Name + "." + fieldName + " is read-only");
				// HACK we abuse RetTypeWrapperFromSig
				FieldWrapper fw = FieldWrapper.Create(this, GetClassLoader().RetTypeWrapperFromSig("()" + fieldSig), fieldName, fieldSig, modifiers, getter, setter);
				AddField(fw);
			}
		}
		if(classMap.Interfaces != null)
		{
			ArrayList ar = new ArrayList();
			if(interfaces != null)
			{
				for(int i = 0; i < interfaces.Length; i++)
				{
					ar.Add(interfaces[i]);
				}
			}
			foreach(MapXml.Interface iface in classMap.Interfaces)
			{
				ar.Add(GetClassLoader().LoadClassByDottedName(iface.Name));
			}
			interfaces = (TypeWrapper[])ar.ToArray(typeof(TypeWrapper));
		}
		// if the type has "overrides" we need to construct a stub class that actually overrides the methods
		// (for when the type itself is instantiated, instead of a subtype [e.g. java.lang.Throwable])
		if(hasOverrides)
		{
			//Console.WriteLine("constructing override stub for " + Name);
			// HACK because we don't want to end up with System.Exception (which is the type that corresponds to the
			// TypeWrapper that corresponds to the type of Throwable$OverrideStub) we have to use GetBootstrapTypeRaw,
			// which was introduced specifically to deal with this problem
			Type stubType = ClassLoaderWrapper.GetBootstrapClassLoader().GetBootstrapTypeRaw(Name + "$OverrideStub");
			if(stubType != null)
			{
				foreach(MethodWrapper mw in methods)
				{
					MethodDescriptor md = mw.Descriptor;
					if(md.Name == "<init>")
					{
						//Console.WriteLine("replacing newobj " + stubType.FullName + " to " + stubType.GetConstructor(md.ArgTypes));
						// NOTE we only support public constructors here, as that correct?
						mw.EmitNewobj = CodeEmitter.Create(OpCodes.Newobj, stubType.GetConstructor(md.ArgTypes));
					}
				}
			}
			else
			{
				// TODO we can ignore the normal ClassNotFoundException, what should we do with other exceptions?
				TypeBuilder stub = GetClassLoader().ModuleBuilder.DefineType(Name + "$OverrideStub", type.Attributes, type);
				CustomAttributeBuilder hideFromReflectionAttrib = new CustomAttributeBuilder(typeof(HideFromReflectionAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
				stub.SetCustomAttribute(hideFromReflectionAttrib);
				foreach(MethodWrapper mw in methods)
				{
					MethodDescriptor md = mw.Descriptor;
					if(mw.IsRemappedOverride)
					{
						MethodBuilder mb = stub.DefineMethod(md.Name, mw.GetMethodAttributes(), CallingConventions.Standard, md.RetType, md.ArgTypes);
						ILGenerator ilgen = mb.GetILGenerator();
						ilgen.Emit(OpCodes.Ldarg_0);
						int argc = md.ArgTypes.Length;
						for(int n = 0; n < argc; n++)
						{
							ilgen.Emit(OpCodes.Ldarg, n + 1);
						}
						mw.EmitCall.Emit(ilgen);
						ilgen.Emit(OpCodes.Ret);
						// TODO only explicitly override if it is needed
						stub.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
					}
					else if(md.Name == "<init>")
					{
						ConstructorBuilder cb = stub.DefineConstructor(mw.GetMethodAttributes(), CallingConventions.Standard, md.ArgTypes);
						ILGenerator ilgen = cb.GetILGenerator();
						ilgen.Emit(OpCodes.Ldarg_0);
						int argc = md.ArgTypes.Length;
						for(int n = 0; n < argc; n++)
						{
							ilgen.Emit(OpCodes.Ldarg, n + 1);
						}
						mw.EmitCall.Emit(ilgen);
						ilgen.Emit(OpCodes.Ret);
						mw.EmitNewobj = CodeEmitter.Create(OpCodes.Newobj, cb);
					}
				}
				// TODO consider post-poning this until the type is really needed
				stub.CreateType();
			}
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			return interfaces;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			// remapped types do not support inner classes at the moment
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			// remapped types cannot be inside inner classes at the moment
			return null;
		}
	}

	public override Type Type
	{
		get
		{
			return type;
		}
	}

	public override void Finish()
	{
	}

	internal override void ImplementOverrideStubsAndVirtuals(TypeBuilder typeBuilder, DynamicTypeWrapper wrapper, Hashtable methodLookup)
	{
		MethodWrapper[] methods = GetMethods();
		Type virtualsInterface = VirtualsInterface;
		if(virtualsInterface != null)
		{
			typeBuilder.AddInterfaceImplementation(virtualsInterface);
		}
		for(int i = 0; i < methods.Length; i++)
		{
			MethodWrapper mce = methods[i];
			if(mce.IsRemappedOverride)
			{
				MethodDescriptor md = mce.Descriptor;
				if(!methodLookup.ContainsKey(md))
				{
					MethodBuilder mb = typeBuilder.DefineMethod(md.Name, mce.GetMethodAttributes(), CallingConventions.Standard, md.RetType, md.ArgTypes);
					ILGenerator ilgen = mb.GetILGenerator();
					ilgen.Emit(OpCodes.Ldarg_0);
					int argc = md.ArgTypes.Length;
					for(int n = 0; n < argc; n++)
					{
						ilgen.Emit(OpCodes.Ldarg, n + 1);
					}
					mce.EmitCall.Emit(ilgen);
					ilgen.Emit(OpCodes.Ret);
					// TODO only explicitly override if it is needed
					typeBuilder.DefineMethodOverride(mb, (MethodInfo)mce.GetMethod());
					// now add the method to methodLookup, to prevent the virtuals loop below from adding it again
					methodLookup[md] = md;
				}
			}
			if(mce.IsRemappedVirtual)
			{
				MethodDescriptor md = mce.Descriptor;
				if(!methodLookup.ContainsKey(md))
				{
					// TODO the attributes aren't correct, but we cannot make the method non-public, because
					// that would violate the interface contract. In other words, we need to find a different
					// mechanism for implementing non-public virtuals.
					MethodBuilder mb = typeBuilder.DefineMethod(md.Name, MethodAttributes.Virtual | MethodAttributes.Public, CallingConventions.Standard, md.RetType, md.ArgTypes);
					ILGenerator ilgen = mb.GetILGenerator();
					ilgen.Emit(OpCodes.Ldarg_0);
					int argc = md.ArgTypes.Length;
					for(int n = 0; n < argc; n++)
					{
						ilgen.Emit(OpCodes.Ldarg, n + 1);
					}
					mce.EmitCall.Emit(ilgen);
					ilgen.Emit(OpCodes.Ret);
				}
			}
		}
	}

	private MethodWrapper[] GetVirtuals()
	{
		ArrayList array = new ArrayList();
		foreach(MethodWrapper mw in GetMethods())
		{
			if(mw.IsRemappedVirtual)
			{
				array.Add(mw);
			}
		}
		return (MethodWrapper[])array.ToArray(typeof(MethodWrapper));
	}

	private Type VirtualsInterface
	{
		get
		{
			if(!virtualsInterfaceBuilt)
			{
				virtualsInterfaceBuilt = true;
				MethodWrapper[] virtuals = GetVirtuals();
				if(virtuals.Length > 0)
				{
					// if the virtualsinterface already exists in one of the bootstrap DLLs, we need to reference that one
					// instead of creating another one, because creating a new type breaks compatibility with pre-compiled code
					try
					{
						virtualsInterface = ClassLoaderWrapper.GetType(Name + "$VirtualMethods");
						virtualsHelperHack = ClassLoaderWrapper.GetType(Name + "$VirtualMethodsHelper");
					}
					catch(Exception)
					{
					}
					if(virtualsInterface != null && virtualsHelperHack != null)
					{
						return virtualsInterface;
					}
					// TODO since this construct makes all virtual methods public, we need to find another way of doing this,
					// or split the methods in two interfaces, one public and one friendly (but how about protected?).
					TypeBuilder typeBuilder = GetClassLoader().ModuleBuilder.DefineType(Name + "$VirtualMethods", TypeAttributes.Abstract | TypeAttributes.Interface | TypeAttributes.Public);
					TypeBuilder tbStaticHack = GetClassLoader().ModuleBuilder.DefineType(Name + "$VirtualMethodsHelper", TypeAttributes.Class | TypeAttributes.Public);
					foreach(MethodWrapper mw in virtuals)
					{
						MethodDescriptor md = mw.Descriptor;
						MethodBuilder ifmethod = typeBuilder.DefineMethod(md.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract, md.RetType, md.ArgTypes);
						Type[] args = new Type[md.ArgTypes.Length + 1];
						md.ArgTypes.CopyTo(args, 1);
						args[0] = this.Type;
						MethodBuilder mb = tbStaticHack.DefineMethod(md.Name, MethodAttributes.Public | MethodAttributes.Static, md.RetType, args);
						ILGenerator ilgen = mb.GetILGenerator();
						ilgen.Emit(OpCodes.Ldarg_0);
						ilgen.Emit(OpCodes.Isinst, typeBuilder);
						ilgen.Emit(OpCodes.Dup);
						Label label1 = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Brtrue_S, label1);
						ilgen.Emit(OpCodes.Pop);
						for(int i = 0; i < args.Length; i++)
						{
							ilgen.Emit(OpCodes.Ldarg, i);
						}
						GetMethodWrapper(md, true).EmitCall.Emit(ilgen);
						Label label2 = ilgen.DefineLabel();
						ilgen.Emit(OpCodes.Br_S, label2);
						ilgen.MarkLabel(label1);
						for(int i = 1; i < args.Length; i++)
						{
							ilgen.Emit(OpCodes.Ldarg, i);
						}
						ilgen.Emit(OpCodes.Callvirt, ifmethod);
						ilgen.MarkLabel(label2);
						ilgen.Emit(OpCodes.Ret);
					}
					virtualsInterface = typeBuilder.CreateType();
					virtualsHelperHack = tbStaticHack.CreateType();
				}
			}
			return virtualsInterface;
		}
	}

	// HACK since Reflection.Emit won't allow static methods on an interface (which is a bug), we create
	// a separate type to contain the static helper methods
	public Type VirtualsHelperHack
	{
		get
		{
			// make sure that the type has been created
			object o = this.VirtualsInterface;
			return virtualsHelperHack;
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		return null;
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		return null;
	}
}

class NetExpTypeWrapper : TypeWrapper
{
	private readonly ClassFile classFile;
	private readonly Type type;

	// TODO consider constructing modifiers from .NET type instead of the netexp class
	public NetExpTypeWrapper(ClassFile f, string dotnetType, TypeWrapper baseType)
		: base(f.Modifiers, f.Name, baseType, ClassLoaderWrapper.GetBootstrapClassLoader())
	{
		this.classFile = f;
		// TODO if the type isn't found, it should be handled differently
		type = Type.GetType(dotnetType, true);
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			// TODO resolve the interfaces!
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			// TODO resolve the inner classes!
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			// TODO resolve the outer class!
			return null;
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		// HACK this is a totally broken quick & dirty implementation
		// TODO clean this up, add error checking and whatnot
		FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		if(!AttributeHelper.IsHideFromReflection(field))
		{
			return FieldWrapper.Create(this, ClassLoaderWrapper.GetWrapperFromType(field.FieldType), field, MethodDescriptor.GetSigName(field), AttributeHelper.GetModifiers(field));
		}
		return null;
	}

	private class DelegateConstructorEmitter : CodeEmitter
	{
		private ConstructorInfo delegateConstructor;
		private MethodInfo method;

		internal DelegateConstructorEmitter(ConstructorInfo delegateConstructor, MethodInfo method)
		{
			this.delegateConstructor = delegateConstructor;
			this.method = method;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Dup);
			ilgen.Emit(OpCodes.Ldvirtftn, method);
			ilgen.Emit(OpCodes.Newobj, delegateConstructor);
		}
	}

	private class RefArgConverter : CodeEmitter
	{
		private Type[] args;

		internal RefArgConverter(Type[] args)
		{
			this.args = args;
		}

		internal override void Emit(ILGenerator ilgen)
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
		}
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		// special case for delegate constructors!
		if(md.Name == "<init>" && type.IsSubclassOf(typeof(MulticastDelegate)))
		{
			// TODO set method flags
			MethodWrapper method = new MethodWrapper(this, md, null, null, Modifiers.Public, false);
			// TODO what class loader should we use?
			TypeWrapper iface = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName(classFile.Name + "$Method");
			iface.Finish();
			method.EmitNewobj = new DelegateConstructorEmitter(type.GetConstructor(new Type[] { typeof(object), typeof(IntPtr) }), iface.Type.GetMethod("Invoke"));
			return method;
		}
		// HACK this is a totally broken quick & dirty implementation
		// TODO clean this up, add error checking and whatnot
		ClassFile.Method[] methods = classFile.Methods;
		for(int i = 0; i < methods.Length; i++)
		{
			if(methods[i].Name == md.Name && methods[i].Signature == md.Signature)
			{
				bool hasByRefArgs = false;
				Type[] args;
				string[] sig = methods[i].NetExpSigAttribute;
				if(sig == null)
				{
					args = md.ArgTypes;
				}
				else
				{
					args = new Type[sig.Length];
					for(int j = 0; j < sig.Length; j++)
					{
						args[j] = Type.GetType(sig[j], true);
						if(args[j].IsByRef)
						{
							hasByRefArgs = true;
						}
					}
				}
				MethodBase method;
				if(md.Name == "<init>")
				{
					method = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, args, null);
				}
				else
				{
					method = type.GetMethod(md.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance, null, CallingConventions.Standard, args, null);
				}
				if(method != null)
				{
					// TODO we can decode the actual method attributes, or we can use them from the NetExp class, what is
					// preferred?
					MethodWrapper mw = MethodWrapper.Create(this, md, method, method, AttributeHelper.GetModifiers(method), AttributeHelper.IsHideFromReflection(method));
					if(hasByRefArgs)
					{
						mw.EmitCall = new RefArgConverter(args) + mw.EmitCall;
						mw.EmitCallvirt = new RefArgConverter(args) + mw.EmitCallvirt;
						mw.EmitNewobj = new RefArgConverter(args) + mw.EmitNewobj;
					}
					return mw;
				}
			}
		}
		return null;
	}

	public override Type Type
	{
		get
		{
			return type;
		}
	}

	public override void Finish()
	{
	}
}

class CompiledTypeWrapper : TypeWrapper
{
	private readonly Type type;
	private TypeWrapper[] interfaces;
	private TypeWrapper[] innerclasses;

	// TODO consider resolving the baseType lazily
	internal CompiledTypeWrapper(string name, Type type, TypeWrapper baseType)
		: base(GetModifiers(type), name, baseType, ClassLoaderWrapper.GetBootstrapClassLoader())
	{
		Debug.Assert(!(type is TypeBuilder));
		Debug.Assert(!type.IsArray);
		this.type = type;
	}

	private static Modifiers GetModifiers(Type type)
	{
		try
		{
			object[] customAttribute = type.GetCustomAttributes(typeof(ModifiersAttribute), false);
			if(customAttribute.Length == 1)
			{
				return ((ModifiersAttribute)customAttribute[0]).Modifiers;
			}
		}
		catch(Exception x)
		{
			// HACK this is just to find other types that don't support GetCustomAttributes
			Console.WriteLine(x);
		}
		// only returns public, protected, private, final, static, abstract and interface (as per
		// the documentation of Class.getModifiers())
		Modifiers modifiers = 0;
		if(type.IsPublic || type.IsNestedPublic)
		{
			modifiers |= Modifiers.Public;
		}
		if(type.IsNestedPrivate)
		{
			modifiers |= Modifiers.Private;
		}
		if(type.IsNestedFamily || type.IsNestedFamORAssem)
		{
			modifiers |= Modifiers.Protected;
		}
		if(type.IsSealed)
		{
			modifiers |= Modifiers.Final;
		}
		if(type.DeclaringType != null)
		{
			modifiers |= Modifiers.Static;
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

	public override TypeWrapper[] Interfaces
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
				ArrayList wrappers = new ArrayList();
				for(int i = 0; i < attribs.Length; i++)
				{
					ImplementsAttribute impl = (ImplementsAttribute)attribs[i];
					wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(impl.Type));
				}
				interfaces = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
			}
			return interfaces;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			if(innerclasses == null)
			{
				Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
				TypeWrapper[] wrappers = new TypeWrapper[nestedTypes.Length];
				for(int i = 0; i < nestedTypes.Length; i++)
				{
					wrappers[i] = ClassLoaderWrapper.GetWrapperFromType(nestedTypes[i]);
				}
				innerclasses = wrappers;
			}
			return innerclasses;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
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

	// TODO there is an inconsistency here, this method returns regular FieldWrappers for final fields, while
	// GetFieldImpl returns a FieldWrapper that is aware of the getter method. Currently this isn't a problem,
	// because this method is used for reflection (which doesn't care about accessibility) and GetFieldImpl is used for
	// compiled code (which does care about accessibility).
	internal override FieldWrapper[] GetFields()
	{
		ArrayList list = new ArrayList();
		FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		for(int i = 0; i < fields.Length; i++)
		{
			if(!AttributeHelper.IsHideFromReflection(fields[i]))
			{
				list.Add(CreateFieldWrapper(AttributeHelper.GetModifiers(fields[i]), fields[i].Name, fields[i].FieldType, fields[i], null));
			}
		}
		return (FieldWrapper[])list.ToArray(typeof(FieldWrapper));
	}

	private FieldWrapper CreateFieldWrapper(Modifiers modifiers, string name, Type fieldType, FieldInfo field, MethodInfo getter)
	{
		CodeEmitter emitGet;
		CodeEmitter emitSet;
		if((modifiers & Modifiers.Static) != 0)
		{
			if(getter != null)
			{
				emitGet = CodeEmitter.Create(OpCodes.Call, getter);
			}
			else
			{
				// if field is a literal, we emit an ldc instead of a ldsfld
				if(field.IsLiteral)
				{
					emitGet = CodeEmitter.CreateLoadConstant(field.GetValue(null));
				}
				else
				{
					emitGet = CodeEmitter.Create(OpCodes.Ldsfld, field);
				}
			}
			if(field != null && !field.IsLiteral)
			{
				emitSet = CodeEmitter.Create(OpCodes.Stsfld, field);
			}
			else
			{
				// TODO what happens when you try to set a final field?
				// through reflection: java.lang.IllegalAccessException: Field is final
				// through code: java.lang.IllegalAccessError: Field <class>.<field> is final
				emitSet = CodeEmitter.Nop;
			}
		}
		else
		{
			if(getter != null)
			{
				emitGet = CodeEmitter.Create(OpCodes.Callvirt, getter);
			}
			else
			{
				// TODO is it possible to have literal instance fields?
				emitGet = CodeEmitter.Create(OpCodes.Ldfld, field);
			}
			if(field != null)
			{
				emitSet = CodeEmitter.Create(OpCodes.Stfld, field);
			}
			else
			{
				// TODO what happens when you try to set a final field through reflection?
				// see above
				emitSet = CodeEmitter.Nop;
			}
		}
		return FieldWrapper.Create(this, ClassLoaderWrapper.GetWrapperFromType(fieldType), name, MethodDescriptor.GetSigName(field), modifiers, emitGet, emitSet);
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		// TODO this is a crappy implementation, just to get going, but it needs to be revisited
		MemberInfo[] members = type.GetMember(fieldName, MemberTypes.Field | MemberTypes.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
		if(members.Length > 2)
		{
			throw new NotImplementedException();
		}
		if(members.Length == 0)
		{
			return null;
		}
		if(members.Length == 2)
		{
			PropertyInfo prop;
			FieldInfo field;
			if(members[0] is PropertyInfo && !(members[1] is PropertyInfo))
			{
				prop = (PropertyInfo)members[0];
				field = (FieldInfo)members[1];
			}
			else if(members[1] is PropertyInfo && !(members[0] is PropertyInfo))
			{
				prop = (PropertyInfo)members[1];
				field = (FieldInfo)members[0];
			}
			else
			{
				throw new InvalidOperationException();
			}
			Modifiers modifiers = AttributeHelper.GetModifiers(field);
			MethodInfo getter = prop.GetGetMethod(true);
			MethodInfo setter = prop.GetSetMethod(true);
			if(getter == null || setter != null)
			{
				throw new InvalidOperationException();
			}
			return CreateFieldWrapper(modifiers, field.Name, field.FieldType, field, getter);
		}
		else
		{
			FieldInfo fi = (FieldInfo)members[0];
			return CreateFieldWrapper(AttributeHelper.GetModifiers(fi), fi.Name, fi.FieldType, fi, null);
		}
	}
	
	// TODO this is broken
	internal override MethodWrapper[] GetMethods()
	{
		ArrayList list = new ArrayList();
		MethodInfo[] methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		for(int i = 0; i < methods.Length; i++)
		{
			MethodWrapper mw = CreateMethodWrapper(MethodDescriptor.FromMethodBase(methods[i]), methods[i]);
			if(mw != null)
			{
				list.Add(mw);
			}
		}
		ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		for(int i = 0; i < constructors.Length; i++)
		{
			MethodWrapper mw = CreateMethodWrapper(MethodDescriptor.FromMethodBase(constructors[i]), constructors[i]);
			if(mw != null)
			{
				list.Add(mw);
			}
		}
		return (MethodWrapper[])list.ToArray(typeof(MethodWrapper));
	}

	private MethodWrapper CreateMethodWrapper(MethodDescriptor md, MethodBase mb)
	{
		if(AttributeHelper.IsHideFromReflection(mb))
		{
			return null;
		}
		MethodWrapper method = new MethodWrapper(this, md, mb, null, AttributeHelper.GetModifiers(mb), false);
		if(IsGhost)
		{
			method.EmitCallvirt = new MethodWrapper.GhostCallEmitter(this, md, mb);
		}
		else
		{
			if(mb is ConstructorInfo)
			{
				method.EmitCall = CodeEmitter.Create(OpCodes.Call, (ConstructorInfo)mb);
				method.EmitNewobj = CodeEmitter.Create(OpCodes.Newobj, (ConstructorInfo)mb);
			}
			else
			{
				method.EmitCall = CodeEmitter.Create(OpCodes.Call, (MethodInfo)mb);
				if(!mb.IsStatic)
				{
					method.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)mb);
				}
			}
		}
		return method;
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		// If the MethodDescriptor contains types that aren't compiled types, we can never have that method
		// This check is important because Type.GetMethod throws an ArgumentException if one of the argument types
		// is a TypeBuilder
		for(int i = 0; i < md.ArgTypeWrappers.Length; i++)
		{
			if(md.ArgTypeWrappers[i].IsUnloadable || md.ArgTypeWrappers[i].Type is TypeBuilder)
			{
				return null;
			}
		}
		// TODO this is a crappy implementation, just to get going, but it needs to be revisited
		if(md.Name == "<init>")
		{
			ConstructorInfo ci = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, md.ArgTypes, null);
			if(ci != null)
			{
				return CreateMethodWrapper(md, ci);
			}
		}
		else
		{
			MethodInfo mi = type.GetMethod(md.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, CallingConventions.Standard, md.ArgTypes, null);
			if(mi != null)
			{
				return CreateMethodWrapper(md, mi);
			}
		}
		return null;
	}

	public override Type Type
	{
		get
		{
			return type;
		}
	}

	public override void Finish()
	{
	}
}

class ArrayTypeWrapper : TypeWrapper
{
	private static TypeWrapper[] interfaces;
	private static MethodDescriptor mdClone;
	private static MethodInfo clone;
	private static CodeEmitter callclone;
	private Type type;

	internal ArrayTypeWrapper(Type type, Modifiers modifiers, string name, ClassLoaderWrapper classLoader)
		: base(modifiers, name, ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Object"), classLoader)
	{
		this.type = type;
		if(mdClone == null)
		{
			mdClone = new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), "clone", "()Ljava.lang.Object;");
		}
		if(clone == null)
		{
			clone = typeof(Array).GetMethod("Clone");
		}
		MethodWrapper mw = new MethodWrapper(this, mdClone, clone, null, Modifiers.Public, true);
		if(callclone == null)
		{
			callclone = CodeEmitter.Create(OpCodes.Callvirt, clone);
		}
		mw.EmitCall = callclone;
		mw.EmitCallvirt = callclone;
		AddMethod(mw);
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			if(interfaces == null)
			{
				TypeWrapper[] tw = new TypeWrapper[2];
				tw[0] = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.Cloneable");
				tw[1] = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedName("java.io.Serializable");
				interfaces = tw;
			}
			return interfaces;
		}
	}

	public override TypeWrapper[] InnerClasses
	{
		get
		{
			return TypeWrapper.EmptyArray;
		}
	}

	public override TypeWrapper DeclaringTypeWrapper
	{
		get
		{
			return null;
		}
	}

	public override Type Type
	{
		get
		{
			return type;
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		return null;
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		return null;
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

	public override void Finish()
	{
		// TODO once we have upward notification (when element TypeWrappers have a reference to their containing arrays)
		// we can optimize this
		lock(this)
		{
			if(!IsFinished)
			{
				TypeWrapper elementTypeWrapper = ElementTypeWrapper;
				Type elementType = elementTypeWrapper.Type;
				elementTypeWrapper.Finish();
				type = elementType.Assembly.GetType(elementType.FullName + "[]", true);
				ClassLoaderWrapper.SetWrapperForType(type, this);
			}
		}
	}
}
