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

sealed class MethodDescriptor
{
	private ClassLoaderWrapper classLoader;
	private string name;
	private string sig;
	private Type[] args;
	private Type ret;

	internal MethodDescriptor(ClassLoaderWrapper classLoader, string name, string sig)
	{
		if(classLoader == null)
		{
			throw new ArgumentNullException();
		}
		this.classLoader = classLoader;
		this.name = name;
		this.sig = sig;
		// class name in the sig should be slashed instead of dotted
		Debug.Assert(sig.IndexOf('.') < 0);
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

	internal Type[] ArgTypes
	{
		get
		{
			if(args == null)
			{
				args = classLoader.ArgTypeListFromSig(sig);
			}
			return args;
		}
	}

	internal Type RetType
	{
		get
		{
			if(ret == null)
			{
				ret = classLoader.RetTypeFromSig(sig);
			}
			return ret;
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

	internal static string getSigName(Type type)
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
				return "L" + type.FullName.Replace('.', '/') + ";";
			}
		}
		else
		{
			string s = NativeCode.java.lang.Class.getName(type).Replace('.', '/');
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
			sb.Append(getSigName(param.ParameterType));
		}
		sb.Append(')');
		if(mb is ConstructorInfo)
		{
			sb.Append('V');
			return new MethodDescriptor(ClassLoaderWrapper.GetClassLoader(mb.DeclaringType), mb.IsStatic ? "<clinit>" : "<init>", sb.ToString());
		}
		else
		{
			sb.Append(getSigName(((MethodInfo)mb).ReturnType));
			return new MethodDescriptor(ClassLoaderWrapper.GetClassLoader(mb.DeclaringType), mb.Name, sb.ToString());
		}
	}
}

abstract class TypeWrapper
{
	private ClassLoaderWrapper classLoader;
	private string name;		// java name (e.g. java/lang/Object)
	private Modifiers modifiers;
	protected Hashtable methods = new Hashtable();
	private Hashtable fields = new Hashtable();
	private TypeWrapper baseWrapper;

	public TypeWrapper(Modifiers modifiers, string name, TypeWrapper baseWrapper, ClassLoaderWrapper classLoader)
	{
		this.modifiers = modifiers;
		this.name = name;
		this.baseWrapper = baseWrapper;
		this.classLoader = classLoader;
	}

	internal Modifiers Modifiers
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

	internal ClassLoaderWrapper GetClassLoader()
	{
		return classLoader;
	}

	protected abstract FieldWrapper GetFieldImpl(string fieldName);

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
		if(method == null)
		{
			throw new ArgumentNullException();
		}
		methods[method.Descriptor] = method;
	}

	public void AddField(FieldWrapper field)
	{
		if(field == null)
		{
			throw new ArgumentNullException();
		}
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
			int index = name.LastIndexOf('/');
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
			int index1 = name.LastIndexOf('/');
			int index2 = wrapper.name.LastIndexOf('/');
			if(index1 == -1 && index2 == -1)
			{
				return true;
			}
			if(index1 == -1 || index2 == -1)
			{
				return false;
			}
			string package1 = name.Substring(0, index1);
			string package2 = wrapper.name.Substring(0, index2);
			return package1 == package2;
		}
		return false;
	}

	public abstract Type Type
	{
		get;
	}

	public TypeWrapper BaseTypeWrapper
	{
		get
		{
			return baseWrapper;
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
		if(baseType.IsInterface)
		{
			if(baseType == this)
			{
				return true;
			}
			return ImplementsInterface(baseType);
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

	public abstract bool IsInterface
	{
		get;
	}

	public abstract TypeWrapper[] Interfaces
	{
		get;
	}

	public abstract void Finish();

	private void ImplementInterfaceMethodStubImpl(MethodDescriptor md, MethodBase ifmethod, TypeBuilder typeBuilder, TypeWrapper wrapper)
	{
		CustomAttributeBuilder methodFlags = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { Modifiers.Synthetic });
		// HACK we're mangling the name to prevent subclasses from overriding this method (I think it is a bug
		// the CLR that it tries to override privatescope methods, by name)
		string mangledName = ifmethod.Name + "$" + wrapper.Name;
		MethodWrapper mce = wrapper.GetMethodWrapper(md, true);
		if(mce != null)
		{
			if(!mce.IsPublic)
			{
				// NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
				// it makes sense, so I hope the spec is wrong
				// UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
				// methods. Sigh! So I have to use private methods and mangle the name
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, md.RetType, md.ArgTypes);
				mb.SetCustomAttribute(methodFlags);
				ILGenerator ilGenerator = mb.GetILGenerator();
				TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/IllegalAccessError");
				ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name + "." + md.Name + md.Signature);
				exception.GetMethodWrapper(new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), "<init>", "(Ljava/lang/String;)V"), false).EmitNewobj.Emit(ilGenerator);
				ilGenerator.Emit(OpCodes.Throw);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
			}
			else if(mce.GetMethod().Name != ifmethod.Name)
			{
				MethodBuilder mb = typeBuilder.DefineMethod(mangledName, MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final, md.RetType, md.ArgTypes);
				mb.SetCustomAttribute(methodFlags);
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
				mb.SetCustomAttribute(methodFlags);
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
				mb.SetCustomAttribute(methodFlags);
				ILGenerator ilGenerator = mb.GetILGenerator();
				TypeWrapper exception = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/AbstractMethodError");
				ilGenerator.Emit(OpCodes.Ldstr, wrapper.Name + "." + md.Name + md.Signature);
				exception.GetMethodWrapper(new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), "<init>", "(Ljava/lang/String;)V"), false).EmitNewobj.Emit(ilGenerator);
				ilGenerator.Emit(OpCodes.Throw);
				typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod);
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
				MethodBuilder mb = typeBuilder.DefineMethod(md.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Abstract, md.RetType, md.ArgTypes);
				mb.SetCustomAttribute(methodFlags);
				// NOTE because we are introducing a Miranda method, we must also update the corresponding wrapper.
				// If we don't do this, subclasses might think they are introducing a new method, instead of overriding
				// this one.
				wrapper.AddMethod(MethodWrapper.Create(wrapper, md, mb, mb, Modifiers.Public | Modifiers.Abstract));
			}
		}
	}

	internal void ImplementInterfaceMethodStubs(TypeBuilder typeBuilder, TypeWrapper wrapper, Hashtable doneSet)
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

	internal virtual void ImplementOverrideStubsAndVirtuals(TypeBuilder typeBuilder, TypeWrapper wrapper, Hashtable methodLookup)
	{
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

	private Type type;

	private PrimitiveTypeWrapper(Type type)
		: base(Modifiers.Public | Modifiers.Abstract | Modifiers.Final, null, null, ClassLoaderWrapper.GetBootstrapClassLoader())
	{
		this.type = type;
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

	public override bool IsInterface
	{
		get
		{
			return false;
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			// TODO does a primitive implement any interfaces?
			return new TypeWrapper[0];
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

	internal DynamicTypeWrapper(string name, ClassFile f, TypeWrapper baseType, ClassLoaderWrapper classLoader, Hashtable nativeMethods)
		: base(f.Modifiers, name, baseType, classLoader)
	{
		JavaTypeImpl impl = new JavaTypeImpl(f, this, baseType, nativeMethods);
		this.impl = impl;
		interfaces = impl.GetInterfaces();
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		return impl.GetFieldImpl(fieldName);
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		return impl.GetMethodImpl(md);
	}

	public override bool IsInterface
	{
		get
		{
			return impl.IsInterface;
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			return interfaces;
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
			impl = impl.Finish();
		}
	}

	private abstract class DynamicImpl
	{
		public abstract FieldWrapper GetFieldImpl(string fieldName);
		public abstract MethodWrapper GetMethodImpl(MethodDescriptor md);
		public abstract Type Type { get; }
		public abstract bool IsInterface { get; }
		public abstract DynamicImpl Finish();
	}

	private class JavaTypeImpl : DynamicImpl
	{
		private ClassFile classFile;
		private DynamicTypeWrapper wrapper;
		private TypeWrapper baseWrapper;
		private TypeBuilder typeBuilder;
		private TypeWrapper[] interfaces;
		private MethodWrapper[] methods;
		private FieldWrapper[] fields;
		private Hashtable methodLookup;
		private Hashtable fieldLookup;
		private bool finishing;
		private FinishedTypeImpl finishedType;
		private Hashtable nativeMethods;

		internal JavaTypeImpl(ClassFile f, DynamicTypeWrapper wrapper, TypeWrapper baseWrapper, Hashtable nativeMethods)
		{
			//		Console.WriteLine("constructing JavaTypeImpl for " + f.Name);
			this.classFile = f;
			this.wrapper = wrapper;
			this.baseWrapper = baseWrapper;
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
			if(f.IsPublic)
			{
				typeAttribs |= TypeAttributes.Public;
			}
			interfaces = new TypeWrapper[f.Interfaces.Length];
			for(int i = 0; i < f.Interfaces.Length; i++)
			{
				interfaces[i] = wrapper.GetClassLoader().LoadClassBySlashedName(f.Interfaces[i]);
				if(!interfaces[i].IsInterface)
				{
					throw JavaException.IncompatibleClassChangeError("Implementing class");
				}
				if(!interfaces[i].IsAccessibleFrom(wrapper))
				{
					throw JavaException.IllegalAccessError("Class {0} cannot access its superinterface {1}", wrapper.Name, interfaces[i].Name);
				}
			}
			// NOTE we call DefineType after all the interfaces have been resolved, because otherwise
			// we end up with a .NET type that cannot be completed
			if(f.IsInterface)
			{
				typeAttribs |= TypeAttributes.Interface | TypeAttributes.Abstract;
				typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(f.Name.Replace('/', '.'), typeAttribs);
			}
			else
			{
				typeAttribs |= TypeAttributes.Class;
				typeBuilder = wrapper.GetClassLoader().ModuleBuilder.DefineType(f.Name.Replace('/', '.'), typeAttribs, baseWrapper.Type);
			}
			for(int i = 0; i < interfaces.Length; i++)
			{
				typeBuilder.AddInterfaceImplementation(interfaces[i].Type);
			}
		}

		internal TypeWrapper[] GetInterfaces()
		{
			return interfaces;
		}

		public override DynamicImpl Finish()
		{
			if(baseWrapper != null)
			{
				// make sure that the base type is already finished (because we need any Miranda methods it
				// might introduce to be visible)
				baseWrapper.Finish();
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
			try
			{
				if(finishing)
				{
					throw new InvalidOperationException("Finishing already in progress, for type " + classFile.Name);
				}
				finishing = true;
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
					methodDescriptors[i] = new MethodDescriptor(wrapper.GetClassLoader(), classFile.Methods[i].Name, classFile.Methods[i].Signature);
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
				wrapper.BaseTypeWrapper.Finish();
				bool basehasclinit = wrapper.BaseTypeWrapper.Type.GetConstructor(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, null, CallingConventions.Any, Type.EmptyTypes, null) != null;
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
							ilGenerator.Emit(OpCodes.Ldtoken, Type.BaseType);
							ilGenerator.Emit(OpCodes.Call, typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetMethod("RunClassConstructor"));
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
							TypeWrapper exceptionType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName("java/lang/AbstractMethodError");
							MethodWrapper method = exceptionType.GetMethodWrapper(new MethodDescriptor(ClassLoaderWrapper.GetBootstrapClassLoader(), "<init>", "(Ljava/lang/String;)V"), false);
							ilGenerator.Emit(OpCodes.Ldstr, m.ClassFile.Name + "." + m.Name + m.Signature);
							method.EmitNewobj.Emit(ilGenerator);
							ilGenerator.Emit(OpCodes.Throw);
						}
					}
					else if(m.IsNative)
					{
						CustomAttributeBuilder methodFlags = new CustomAttributeBuilder(typeof(ModifiersAttribute).GetConstructor(new Type[] { typeof(Modifiers) }), new object[] { m.Modifiers });
						if(mb is ConstructorBuilder)
						{
							((ConstructorBuilder)mb).SetCustomAttribute(methodFlags);
						}
						else
						{
							((MethodBuilder)mb).SetCustomAttribute(methodFlags);
						}
						// do we have a native implementation in map.xml?
						if(nativeMethods != null)
						{
							string key = classFile.Name.Replace('/', '.') + "." + m.Name + m.Signature;
							CodeEmitter opcodes = (CodeEmitter)nativeMethods[key];
							if(opcodes != null)
							{
								opcodes.Emit(ilGenerator);
								continue;
							}
						}
						// see if there exists a NativeCode class for this type
						Type nativeCodeType = Type.GetType("NativeCode." + classFile.Name.Replace('/', '.'));
						MethodInfo nativeMethod = null;
						if(nativeCodeType != null)
						{
							// TODO use better resolution
							nativeMethod = nativeCodeType.GetMethod(m.Name);
						}
						Type[] args = wrapper.GetClassLoader().ArgTypeListFromSig(m.Signature);
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
							Type retType = wrapper.GetClassLoader().RetTypeFromSig(m.Signature);
							if(!retType.Equals(nativeMethod.ReturnType))
							{
								ilGenerator.Emit(OpCodes.Castclass, retType);
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
						else
						{
							if(JVM.NoJniStubs)
							{
								// TODO consider throwing a java.lang.UnsatisfiedLinkError here
								//Console.WriteLine("Native method not implemented: " + classFile.Name + "." + m.Name + m.Signature);
								ilGenerator.Emit(OpCodes.Ldstr, "Native method not implemented: " + classFile.Name + "." + m.Name + m.Signature);
								ilGenerator.Emit(OpCodes.Newobj, typeof(NotImplementedException).GetConstructor(new Type[] { typeof(string) }));
								ilGenerator.Emit(OpCodes.Throw);
								continue;
							}
							FieldBuilder methodPtr = typeBuilder.DefineField(m.Name + "$Ptr", typeof(IntPtr), FieldAttributes.Static | FieldAttributes.PrivateScope);
							Type localRefStructType = JVM.JniProvider.GetLocalRefStructType();
							LocalBuilder localRefStruct = ilGenerator.DeclareLocal(localRefStructType);
							ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							ilGenerator.Emit(OpCodes.Initobj, localRefStructType);
							ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
							Label oklabel = ilGenerator.DefineLabel();
							ilGenerator.Emit(OpCodes.Brtrue, oklabel);
							ilGenerator.Emit(OpCodes.Ldstr, m.Name);
							ilGenerator.Emit(OpCodes.Ldstr, m.Signature);
							ilGenerator.Emit(OpCodes.Ldstr, classFile.Name);
							ilGenerator.Emit(OpCodes.Call, JVM.JniProvider.GetJniFuncPtrMethod());
							ilGenerator.Emit(OpCodes.Stsfld, methodPtr);
							ilGenerator.MarkLabel(oklabel);
							ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("Enter"));
							LocalBuilder jnienv = ilGenerator.DeclareLocal(typeof(IntPtr));
							ilGenerator.Emit(OpCodes.Stloc, jnienv);
							Label tryBlock = ilGenerator.BeginExceptionBlock();
							Type retType = wrapper.GetClassLoader().RetTypeFromSig(m.Signature);
							if(!retType.IsValueType && retType != typeof(void))
							{
								// this one is for use after we return from "calli"
								ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							}
							ilGenerator.Emit(OpCodes.Ldloc, jnienv);
							Type[] modargs = new Type[args.Length + 2];
							modargs[0] = typeof(IntPtr);
							modargs[1] = typeof(IntPtr);
							args.CopyTo(modargs, 2);
							int add = 0;
							if(!m.IsStatic)
							{
								ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
								ilGenerator.Emit(OpCodes.Ldarg_0);
								ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("MakeLocalRef"));
								add = 1;
							}
							else
							{
								ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
								ilGenerator.Emit(OpCodes.Ldtoken, this.Type);
								ilGenerator.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"));
								ilGenerator.Emit(OpCodes.Call, typeof(NativeCode.java.lang.Class).GetMethod("getClassFromType"));
								ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("MakeLocalRef"));
							}
							for(int j = 0; j < args.Length; j++)
							{
								if(!args[j].IsValueType)
								{
									ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
									ilGenerator.Emit(OpCodes.Ldarg, j + add);
									ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("MakeLocalRef"));
									modargs[j + 2] = typeof(IntPtr);
								}
								else
								{
									ilGenerator.Emit(OpCodes.Ldarg, j + add);
								}
							}
							ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
							ilGenerator.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, (retType.IsValueType || retType == typeof(void)) ? retType : typeof(IntPtr), modargs);
							LocalBuilder retValue = null;
							if(retType != typeof(void))
							{
								if(!retType.IsValueType)
								{
									ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("UnwrapLocalRef"));
									ilGenerator.Emit(OpCodes.Castclass, retType);
								}
								retValue = ilGenerator.DeclareLocal(retType);
								ilGenerator.Emit(OpCodes.Stloc, retValue);
							}
							ilGenerator.BeginCatchBlock(typeof(object));
							ilGenerator.EmitWriteLine("*** exception in native code ***");
							ilGenerator.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }, null));
							ilGenerator.Emit(OpCodes.Rethrow);
							ilGenerator.BeginFinallyBlock();
							ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
							ilGenerator.Emit(OpCodes.Call, localRefStructType.GetMethod("Leave"));
							ilGenerator.EndExceptionBlock();
							if(retType != typeof(void))
							{
								ilGenerator.Emit(OpCodes.Ldloc, retValue);
							}
							ilGenerator.Emit(OpCodes.Ret);
						}
					}
					else
					{
						Compiler.Compile(wrapper, m, ilGenerator, wrapper.GetClassLoader());
					}
				}
				// if we don't have a <clinit> we need to inject one to call the base class <clinit>
				if(basehasclinit && !hasclinit && !classFile.IsInterface)
				{
					ILGenerator ilGenerator = typeBuilder.DefineConstructor(MethodAttributes.Private | MethodAttributes.Static, CallingConventions.Standard, Type.EmptyTypes).GetILGenerator();
					ilGenerator.Emit(OpCodes.Ldtoken, Type.BaseType);
					ilGenerator.Emit(OpCodes.Call, typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetMethod("RunClassConstructor"));
					ilGenerator.Emit(OpCodes.Ret);
				}

				if(!classFile.IsInterface)
				{
					// here we loop thru all the interfaces to explicitly implement any methods that we inherit from
					// base types that may have a different name from the name in the interface
					// (e.g. interface that has an equals() method that should override System.Object.Equals())
					// also deals with interface methods that aren't implemented (generate a stub that throws AbstractMethodError)
					// and with methods that aren't public (generate a stub that throws IllegalAccessError)
					Hashtable doneSet = new Hashtable();
					for(int i = 0; i < interfaces.Length; i++)
					{
						interfaces[i].ImplementInterfaceMethodStubs(typeBuilder, wrapper, doneSet);
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
				finishedType = new FinishedTypeImpl(type);
				return finishedType;
			}
			catch(Exception x)
			{
				Console.WriteLine("****** Exception during finishing ******");
				Console.WriteLine(x);
				// we bail out, because there is not much chance that we can continue to run after this
				Environment.Exit(1);
				return null;
			}
		}

		public override bool IsInterface
		{
			get
			{
				return typeBuilder.IsInterface;
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
			FieldBuilder field;
			ClassFile.Field fld = classFile.Fields[i];
			Type type = null;
			try
			{
				type = wrapper.GetClassLoader().ExpressionType(fld.Signature);
			}
			catch(Exception x)
			{
				if(x.GetType().FullName == "java.lang.ClassNotFoundException")
				{
					// TODO set fields[i] to a special FieldWrapper that does the appropriate thing (whatever that may be)
					fields[i] = new FieldWrapper(this.wrapper, fld.Name, fld.Signature, fld.Modifiers);
					Console.Error.WriteLine("Type " + fld.Signature + " of field " + fld.Name + " in class " + classFile.Name + " is unloadable");
					return;
				}
				throw;
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
				attribs |= FieldAttributes.Literal;
				field = typeBuilder.DefineField(fld.Name, type, attribs);
				field.SetConstant(constantValue);
				fields[i] = FieldWrapper.Create(wrapper, field, fld.Signature, fld.Modifiers);
				// NOTE even though you're not supposed to access a constant static final (the compiler is supposed
				// to inline them), we have to support it (because it does happen, e.g. if the field becomes final
				// after the referencing class was compiled)
				if(constantValue is int || constantValue is short || constantValue is sbyte || constantValue is char || constantValue is bool)
				{
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Ldc_I4, ((IConvertible)constantValue).ToInt32(null));
				}
				else if(constantValue is string)
				{
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Ldstr, (string)constantValue);
				}
				else if(constantValue is float)
				{
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Ldc_R4, (float)constantValue);
				}
				else if(constantValue is double)
				{
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Ldc_R8, (double)constantValue);
				}
				else if(constantValue is long)
				{
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Ldc_I8, (long)constantValue);
				}
				else
				{
					throw new NotImplementedException(constantValue.GetType().FullName);
				}
				// TODO this should probably emit a IllegalAccessError (or whatever)
				// TODO also check other field[x].EmitSet's for final fields
				// UPDATE for other final fields this shouldn't happend, because the compiler will check access before
				// calling EmitSet
				// UPDATE when non-blank final fields are updated, the JIT normally doesn't see that (because the
				// constant value is inlined), so we emulate that behavior by emitting a Nop
				fields[i].EmitSet = CodeEmitter.Create(OpCodes.Nop);
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
					ModifiersAttribute.SetModifiers(getter, Modifiers.Synthetic);
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
					fields[i] = FieldWrapper.Create(wrapper, field, fld.Signature, fld.Modifiers);
					fields[i].EmitGet = CodeEmitter.Create(OpCodes.Call, getter);
				}
				else
				{
					fields[i] = FieldWrapper.Create(wrapper, field, fld.Signature, fld.Modifiers);
				}
			}
			// if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
			// the Java modifiers
			if(setModifiers)
			{
				ModifiersAttribute.SetModifiers(field, fld.Modifiers);
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
					methodLookup[new MethodDescriptor(wrapper.GetClassLoader(), classFile.Methods[i].Name, classFile.Methods[i].Signature)] = i;
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
			if(methods[index] != null)
			{
				throw new InvalidOperationException();
			}
			MethodBase method;
			ClassFile.Method m = classFile.Methods[index];
			Type[] args = null;
			Type retType = null;
			try
			{
				args = wrapper.GetClassLoader().ArgTypeListFromSig(m.Signature);
				retType = wrapper.GetClassLoader().RetTypeFromSig(m.Signature);
			}
			catch(Exception x)
			{
				if(x.GetType().FullName == "java.lang.ClassNotFoundException")
				{
					// TODO set methods[i] to a special MethodWrapper that does the appropriate thing (whatever that may be)
					methods[index] = new MethodWrapper(this.wrapper, new MethodDescriptor(wrapper.GetClassLoader(), m.Name, m.Signature), null, m.Modifiers);
					Console.Error.WriteLine("Type " + x.Message + " of method " + m.Name + m.Signature + " in class " + classFile.Name + " is unloadable");
					return;
				}
				throw;
			}
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
			}
			if(m.IsFinal)
			{
				if(!m.IsStatic && !m.IsPrivate)
				{
					attribs |= MethodAttributes.Final;
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
				method = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, args);
			}
			else if(m.Name == "<clinit>" && m.Signature == "()V")
			{
				// I think this is a CLR bug, the verifier requires static inititializes of interfaces to be public
				// TODO report this bug
				if(classFile.IsInterface)
				{
					attribs &= ~MethodAttributes.MemberAccessMask;
					attribs |= MethodAttributes.Public;
				}
				method = typeBuilder.DefineConstructor(attribs, CallingConventions.Standard, args);
			}
			else
			{
				if(!m.IsPrivate && !m.IsStatic)
				{
					attribs |= MethodAttributes.Virtual;
				}
				string name = m.Name;
				MethodDescriptor md = new MethodDescriptor(wrapper.GetClassLoader(), name, m.Signature);
				// if a method is virtual, we need to find the method it overrides (if any), for several reasons:
				// - if we're overriding a method that has a different name (e.g. some of the virtual methods
				//   in System.Object [Equals <-> equals]) we need to add an explicit MethodOverride
				// - if one of the base classes has a similar method that is private (or package) that we aren't
				//   overriding, we need to specify an explicit MethodOverride
				MethodBase baseMethod = null;
				bool explicitOverride = false;
				if((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
				{
					TypeWrapper tw = baseWrapper;
					while(tw != null)
					{
						MethodWrapper baseMce = tw.GetMethodWrapper(md, true);
						if(baseMce == null)
						{
							break;
						}
						// here are the complex rules for determining wether this method overrides the method we found
						// RULE 1: final methods may not be overriden
						if(baseMce.IsFinal)
						{
							// NOTE we don't need to test for our method being private, because if it is
							// we'll never get here (because private methods aren't virtual)
							// TODO make sure the VerifyError is translated into a java.lang.VerifyError
							throw new VerifyError();
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
						attribs |= MethodAttributes.NewSlot;
					}
				}
				MethodBuilder mb = typeBuilder.DefineMethod(name, attribs, retType, args);
				method = mb;
				// since Java constructors (and static intializers?) aren't allowed to be synchronized, we only check this here
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
			methods[index] = MethodWrapper.Create(wrapper, new MethodDescriptor(wrapper.GetClassLoader(), m.Name, m.Signature), method, method, m.Modifiers);
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

		public FinishedTypeImpl(Type type)
		{
			this.type = type;
		}

		public override bool IsInterface
		{
			get
			{
				return type.IsInterface;
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
	private Type type;
	private TypeWrapper[] interfaces;
	private bool virtualsInterfaceBuilt;
	private Type virtualsInterface;
	private Type virtualsHelperHack;

	public RemappedTypeWrapper(Modifiers modifiers, string name, Type type, TypeWrapper[] interfaces, TypeWrapper baseType)
		: base(modifiers, name, baseType, ClassLoaderWrapper.GetBootstrapClassLoader())
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
					MethodWrapper mw = MethodWrapper.Create(this, md, mb, mb, modifiers);
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
						Type t = this.type;
						if(method.redirect.Class != null)
						{
							t = Type.GetType(method.redirect.Class, true);
						}
						redirect = t.GetMethod(name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
						if(redirect == null)
						{
							throw new InvalidOperationException("remapping method: " + name + sig + " not found");
						}
						string ret = md.Signature.Substring(md.Signature.IndexOf(')') + 1);
						// when constructors are remapped, we have to assume that the type is correct because the original
						// return type (of the constructor) is void.
						if(ret[0] != 'V' && ret != redir.Signature.Substring(redir.Signature.IndexOf(')') + 1))
						{
							retcast = new CastEmitter(md.Signature);
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
						// NOTE we abuse MethodWrapper.Create here to construct the emitters for us
						MethodWrapper temp = MethodWrapper.Create(this, md, overrideMethod, redirect, modifiers);
						newopc = temp.EmitNewobj;
						invokespecial = temp.EmitCall;
						invokevirtual = temp.EmitCallvirt;
					}
					else
					{
						if(method.@override != null)
						{
							MethodDescriptor redir = new MethodDescriptor(GetClassLoader(), method.@override.Name, sig);
							BindingFlags binding = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
							overrideMethod = type.GetMethod(redir.Name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
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
					MethodWrapper mw = new MethodWrapper(this, md, overrideMethod, modifiers);
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
					MethodWrapper mw = MethodWrapper.Create(this, md, method, method, modifiers);
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
						Type t = this.type;
						if(constructor.redirect.Class != null)
						{
							t = Type.GetType(constructor.redirect.Class, true);
						}
						MethodBase redirect = null;
						if(constructor.redirect.Name != null)
						{
							redirect = t.GetMethod(constructor.redirect.Name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
						}
						else
						{
							redirect = t.GetConstructor(binding, null, CallingConventions.Standard, redir.ArgTypes, null);
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
							retcast = new CastEmitter(md.Signature);
						}
						// NOTE we abuse MethodWrapper.Create here to construct the emitters for us
						MethodWrapper temp = MethodWrapper.Create(this, md, null, redirect, modifiers);
						newopc = temp.EmitNewobj;
					}
					else
					{
						newopc = constructor.newobj;
						invokespecial = constructor.invokespecial;
					}
					MethodWrapper mw = new MethodWrapper(this, md, null, modifiers);
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
					t = Type.GetType(field.redirect.Class, true);
				}
				MethodInfo method = t.GetMethod(name, binding, null, CallingConventions.Standard, redir.ArgTypes, null);
				if(method == null)
				{
					throw new InvalidOperationException("remapping method: " + name + sig + " not found");
				}
				// TODO ensure that return type for redirected method matches with field type, or emit a castclass
				FieldWrapper fw = new FieldWrapper(this, fieldName, fieldSig, modifiers);
				fw.EmitGet = CodeEmitter.Create(OpCodes.Call, method);
				fw.EmitSet = null;
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
			Type stubType = ClassLoaderWrapper.GetBootstrapClassLoader().GetBootstrapTypeRaw(Name.Replace('/', '.') + "$OverrideStub");
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
				TypeBuilder stub = GetClassLoader().ModuleBuilder.DefineType(Name.Replace('/', '.') + "$OverrideStub", type.Attributes, type);
				CustomAttributeBuilder overrideStubAttrib = new CustomAttributeBuilder(typeof(OverrideStubTypeAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
				stub.SetCustomAttribute(overrideStubAttrib);
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

	public override bool IsInterface
	{
		get
		{
			return type.IsInterface;
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			return interfaces;
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

	internal override void ImplementOverrideStubsAndVirtuals(TypeBuilder typeBuilder, TypeWrapper wrapper, Hashtable methodLookup)
	{
		MethodWrapper[] methods = new MethodWrapper[this.methods.Count];
		this.methods.Values.CopyTo(methods, 0);
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
		foreach(MethodWrapper mw in methods.Values)
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
						virtualsInterface = ClassLoaderWrapper.GetType(Name.Replace('/', '.') + "$VirtualMethods");
						virtualsHelperHack = ClassLoaderWrapper.GetType(Name.Replace('/', '.') + "$VirtualMethodsHelper");
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
					TypeBuilder typeBuilder = GetClassLoader().ModuleBuilder.DefineType(Name.Replace('/', '.') + "$VirtualMethods", TypeAttributes.Abstract | TypeAttributes.Interface | TypeAttributes.Public);
					TypeBuilder tbStaticHack = GetClassLoader().ModuleBuilder.DefineType(Name.Replace('/', '.') + "$VirtualMethodsHelper", TypeAttributes.Class | TypeAttributes.Public);
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
	private ClassFile classFile;
	private Type type;

	// TODO consider constructing modifiers from .NET type instead of the netexp class
	public NetExpTypeWrapper(ClassFile f, string dotnetType, TypeWrapper baseType)
		: base(f.Modifiers, f.Name, baseType, ClassLoaderWrapper.GetBootstrapClassLoader())
	{
		this.classFile = f;
		// TODO if the type isn't found, it should be handled differently
		type = Type.GetType(dotnetType, true);
	}

	public override bool IsInterface
	{
		get
		{
			return type.IsInterface;
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			// TODO resolve the interfaces!
			return new TypeWrapper[0];
		}
	}

	protected override FieldWrapper GetFieldImpl(string fieldName)
	{
		// HACK this is a totally broken quick & dirty implementation
		// TODO clean this up, add error checking and whatnot
		FieldInfo field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
		Modifiers modifiers = ModifiersAttribute.GetModifiers(field);
		if(modifiers != Modifiers.Synthetic)
		{
			return FieldWrapper.Create(this, field, MethodDescriptor.getSigName(field.FieldType), modifiers);
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

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
		// special case for delegate constructors!
		if(md.Name == "<init>" && type.IsSubclassOf(typeof(MulticastDelegate)))
		{
			// TODO set method flags
			MethodWrapper method = new MethodWrapper(this, md, null, Modifiers.Public);
			// TODO what class loader should we use?
			TypeWrapper iface = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassBySlashedName(classFile.Name + "$Method");
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
					return MethodWrapper.Create(this, md, method, method, ModifiersAttribute.GetModifiers(method));
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
	private Type type;
	private TypeWrapper[] interfaces;

	// TODO consider resolving the baseType lazily
	internal CompiledTypeWrapper(string name, Type type, TypeWrapper baseType)
		: base(ModifiersAttribute.GetModifiers(type), name, baseType, ClassLoaderWrapper.GetBootstrapClassLoader())
	{
		this.type = type;
	}

	public override bool IsInterface
	{
		get
		{
			return type.IsInterface;
		}
	}

	public override TypeWrapper[] Interfaces
	{
		get
		{
			if(interfaces == null)
			{
				// TODO instead of getting the interfaces list from Type, we should use a custom
				// attribute to list the implemented interfaces (alternatively, we could check the assembly
				// of each interface to make sure it is from an IKVM compiled assembly, but even if the
				// interfaces doesn't come from an IKVM assembly we still must call GetWrapperFromTypeFast
				// to handle remapped interfaces.
				Type[] ifaces = type.GetInterfaces();
				ArrayList wrappers = new ArrayList();
				for(int i = 0; i < ifaces.Length; i++)
				{
					// HACK if the interface wrapper isn't found, we'll just ignore it (this happens for
					// example for Throwable derived classes, which seem to implement ISerializable [because System.Exception does],
					// but we cannot find a wrapper for this type)
					try
					{
						wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(ifaces[i]));
					}
					catch(Exception)
					{
					}
				}
				interfaces = (TypeWrapper[])wrappers.ToArray(typeof(TypeWrapper));
			}
			return interfaces;
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
			Modifiers modifiers = ModifiersAttribute.GetModifiers(fields[i]);
			if(modifiers != Modifiers.Synthetic)
			{
				list.Add(CreateFieldWrapper(modifiers, fields[i].Name, fields[i].FieldType, fields[i], null));
			}
		}
		return (FieldWrapper[])list.ToArray(typeof(FieldWrapper));
	}

	private FieldWrapper CreateFieldWrapper(Modifiers modifiers, string name, Type fieldType, FieldInfo field, MethodInfo getter)
	{
		FieldWrapper fieldWrapper = new FieldWrapper(this, name, MethodDescriptor.getSigName(fieldType), modifiers);
		if((modifiers & Modifiers.Static) != 0)
		{
			if(getter != null)
			{
				fieldWrapper.EmitGet = CodeEmitter.Create(OpCodes.Call, getter);
			}
			else
			{
				// TODO if field is a literal, we should emit an ldc instead of a ldsfld
				fieldWrapper.EmitGet = CodeEmitter.Create(OpCodes.Ldsfld, field);
			}
			if(field != null)
			{
				fieldWrapper.EmitSet = CodeEmitter.Create(OpCodes.Stsfld, field);
			}
			else
			{
				// TODO what happens when you try to set a final field?
				// through reflection: java.lang.IllegalAccessException: Field is final
				// through code: java.lang.IllegalAccessError: Field <class>.<field> is final
				fieldWrapper.EmitSet = CodeEmitter.Create(OpCodes.Nop);
			}
		}
		else
		{
			if(getter != null)
			{
				fieldWrapper.EmitGet = CodeEmitter.Create(OpCodes.Callvirt, getter);
			}
			else
			{
				// TODO is it possible to have literal instance fields?
				fieldWrapper.EmitGet = CodeEmitter.Create(OpCodes.Ldfld, field);
			}
			if(field != null)
			{
				fieldWrapper.EmitSet = CodeEmitter.Create(OpCodes.Stfld, field);
			}
			else
			{
				// TODO what happens when you try to set a final field through reflection?
				// see above
				fieldWrapper.EmitSet = CodeEmitter.Create(OpCodes.Nop);
			}
		}
		return fieldWrapper;
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
			Modifiers modifiers = ModifiersAttribute.GetModifiers(field);
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
			return CreateFieldWrapper(ModifiersAttribute.GetModifiers(fi), fi.Name, fi.FieldType, fi, null);
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
		Modifiers modifiers = ModifiersAttribute.GetModifiers(mb);
		if(modifiers == Modifiers.Synthetic)
		{
			return null;
		}
		MethodWrapper method = new MethodWrapper(this, md, mb, ModifiersAttribute.GetModifiers(mb));
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
		return method;
	}

	protected override MethodWrapper GetMethodImpl(MethodDescriptor md)
	{
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

public abstract class CodeEmitter
{
	internal abstract void Emit(ILGenerator ilgen);

	private class ChainCodeEmitter : CodeEmitter
	{
		private CodeEmitter left;
		private CodeEmitter right;

		internal ChainCodeEmitter(CodeEmitter left, CodeEmitter right)
		{
			this.left = left;
			this.right = right;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			left.Emit(ilgen);
			right.Emit(ilgen);
		}
	}

	public static CodeEmitter operator+(CodeEmitter left, CodeEmitter right)
	{
		if(left == null)
		{
			return right;
		}
		return new ChainCodeEmitter(left, right);
	}

	private class MethodInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private MethodInfo mi;

		internal MethodInfoCodeEmitter(OpCode opcode, MethodInfo mi)
		{
			this.opcode = opcode;
			this.mi = mi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, mi);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, MethodInfo mi)
	{
		return new MethodInfoCodeEmitter(opcode, mi);
	}

	private class ConstructorInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private ConstructorInfo ci;

		internal ConstructorInfoCodeEmitter(OpCode opcode, ConstructorInfo ci)
		{
			this.opcode = opcode;
			this.ci = ci;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, ci);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, ConstructorInfo ci)
	{
		return new ConstructorInfoCodeEmitter(opcode, ci);
	}

	private class FieldInfoCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private FieldInfo fi;

		internal FieldInfoCodeEmitter(OpCode opcode, FieldInfo fi)
		{
			this.opcode = opcode;
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, fi);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, FieldInfo fi)
	{
		return new FieldInfoCodeEmitter(opcode, fi);
	}

	private class OpCodeEmitter : CodeEmitter
	{
		private OpCode opcode;

		internal OpCodeEmitter(OpCode opcode)
		{
			this.opcode = opcode;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode);
		}
	}

	internal static CodeEmitter Create(OpCode opcode)
	{
		return new OpCodeEmitter(opcode);
	}

	private class TypeCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private Type type;

		internal TypeCodeEmitter(OpCode opcode, Type type)
		{
			this.opcode = opcode;
			this.type = type;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, type);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, Type type)
	{
		return new TypeCodeEmitter(opcode, type);
	}

	private class IntCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private int i;

		internal IntCodeEmitter(OpCode opcode, int i)
		{
			this.opcode = opcode;
			this.i = i;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, i);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, int i)
	{
		return new IntCodeEmitter(opcode, i);
	}

	private class FloatCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private float f;

		internal FloatCodeEmitter(OpCode opcode, float f)
		{
			this.opcode = opcode;
			this.f = f;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, f);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, float f)
	{
		return new FloatCodeEmitter(opcode, f);
	}

	private class DoubleCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private double d;

		internal DoubleCodeEmitter(OpCode opcode, double d)
		{
			this.opcode = opcode;
			this.d = d;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, d);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, double d)
	{
		return new DoubleCodeEmitter(opcode, d);
	}

	private class StringCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private string s;

		internal StringCodeEmitter(OpCode opcode, string s)
		{
			this.opcode = opcode;
			this.s = s;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, s);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, string s)
	{
		return new StringCodeEmitter(opcode, s);
	}

	private class LongCodeEmitter : CodeEmitter
	{
		private OpCode opcode;
		private long l;

		internal LongCodeEmitter(OpCode opcode, long l)
		{
			this.opcode = opcode;
			this.l = l;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			ilgen.Emit(opcode, l);
		}
	}

	internal static CodeEmitter Create(OpCode opcode, long l)
	{
		return new LongCodeEmitter(opcode, l);
	}
}

sealed class MethodWrapper
{
	private TypeWrapper declaringType;
	private MethodDescriptor md;
	private MethodBase originalMethod;
	private Modifiers modifiers;
	private bool isRemappedVirtual;
	private bool isRemappedOverride;
	internal CodeEmitter EmitCall;
	internal CodeEmitter EmitCallvirt;
	internal CodeEmitter EmitNewobj;

	internal static MethodWrapper Create(TypeWrapper declaringType, MethodDescriptor md, MethodBase originalMethod, MethodBase method, Modifiers modifiers)
	{
		if(method == null)
		{
			throw new InvalidOperationException();
		}
		MethodWrapper wrapper = new MethodWrapper(declaringType, md, originalMethod, modifiers);
		if(method is ConstructorInfo)
		{
			wrapper.EmitCall = CodeEmitter.Create(OpCodes.Call, (ConstructorInfo)method);
			wrapper.EmitCallvirt = null;
			wrapper.EmitNewobj = CodeEmitter.Create(OpCodes.Newobj, (ConstructorInfo)method);
		}
		else
		{
			wrapper.EmitCall = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
			if(originalMethod != null && originalMethod != method)
			{
				// if we're calling a virtual method that is redirected, that overrides an already
				// existing method, we have to call it virtually, instead of redirecting
				wrapper.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)originalMethod);
			}
			else if(method.IsStatic)
			{
				// because of redirection, it can be legal to call a static method with invokevirtual
				wrapper.EmitCallvirt = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
			}
			else
			{
				wrapper.EmitCallvirt = CodeEmitter.Create(OpCodes.Callvirt, (MethodInfo)method);
			}
			wrapper.EmitNewobj = CodeEmitter.Create(OpCodes.Call, (MethodInfo)method);
		}
		return wrapper;
	}

	internal MethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodBase originalMethod, Modifiers modifiers)
	{
		this.declaringType = declaringType;
		this.md = md;
		// NOTE originalMethod may be null
		this.originalMethod = originalMethod;
		this.modifiers = modifiers;
	}

	internal TypeWrapper DeclaringType
	{
		get
		{
			return declaringType;
		}
	}

	internal MethodDescriptor Descriptor
	{
		get
		{
			return md;
		}
	}

	internal string Name
	{
		get
		{
			return md.Name;
		}
	}

	internal TypeWrapper ReturnType
	{
		get
		{
			return declaringType.GetClassLoader().RetTypeWrapperFromSig(md.Signature);
		}
	}

	internal TypeWrapper[] GetParameters()
	{
		return declaringType.GetClassLoader().ArgTypeWrapperListFromSig(md.Signature);
	}

	internal Modifiers Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	// IsRemappedOverride (only possible for remapped types) indicates whether the method in the
	// remapped type overrides (i.e. replaces) the method from the underlying type
	// Example: System.Object.ToString() is overriden by java.lang.Object.toString(),
	//          because java.lang.Object's toString() does something different from
	//          System.Object's ToString().
	internal bool IsRemappedOverride
	{
		get
		{
			return isRemappedOverride;
		}
		set
		{
			isRemappedOverride = value;
		}
	}

	// IsRemappedVirtual (only possible for remapped types) indicates that the method is virtual,
	// but doesn't really exist in the underlying type (there is no vtable slot to reuse)
	internal bool IsRemappedVirtual
	{
		get
		{
			return isRemappedVirtual;
		}
		set
		{
			isRemappedVirtual = value;
		}
	}

	// we expose the underlying MethodBase object,
	// for Java types, this is the method that contains the compiled Java bytecode
	// for remapped types, this is the method that underlies the remapped method
	internal MethodBase GetMethod()
	{
		return originalMethod;
	}

	// this returns the Java method's attributes in .NET terms (e.g. used to create stubs for this method)
	internal MethodAttributes GetMethodAttributes()
	{
		MethodAttributes attribs = 0;
		if(IsStatic)
		{
			attribs |= MethodAttributes.Static;
		}
		if(IsPublic)
		{
			attribs |= MethodAttributes.Public;
		}
		else if(IsPrivate)
		{
			attribs |= MethodAttributes.Private;
		}
		else if(IsProtected)
		{
			attribs |= MethodAttributes.FamORAssem;
		}
		else
		{
			attribs |= MethodAttributes.Family;
		}
		// constructors aren't virtual
		if(!IsStatic && !IsPrivate && md.Name != "<init>")
		{
			attribs |= MethodAttributes.Virtual;
		}
		if(IsFinal)
		{
			attribs |= MethodAttributes.Final;
		}
		if(IsAbstract)
		{
			attribs |= MethodAttributes.Abstract;
		}
		return attribs;
	}

	internal bool IsStatic
	{
		get
		{
			return (modifiers & Modifiers.Static) != 0;
		}
	}

	internal bool IsPublic
	{
		get
		{
			return (modifiers & Modifiers.Public) != 0;
		}
	}

	internal bool IsPrivate
	{
		get
		{
			return (modifiers & Modifiers.Private) != 0;
		}
	}

	internal bool IsProtected
	{
		get
		{
			return (modifiers & Modifiers.Protected) != 0;
		}
	}

	internal bool IsFinal
	{
		get
		{
			return (modifiers & Modifiers.Final) != 0;
		}
	}

	internal bool IsAbstract
	{
		get
		{
			return (modifiers & Modifiers.Abstract) != 0;
		}
	}
}

class CastEmitter : CodeEmitter
{
	private Type retType;
	private string sig;

	internal CastEmitter(string sig)
	{
		this.sig = sig;
	}

	internal override void Emit(ILGenerator ilgen)
	{
		if(retType == null)
		{
			retType = ClassLoaderWrapper.GetBootstrapClassLoader().RetTypeFromSig(sig);
		}
		ilgen.Emit(OpCodes.Castclass, retType);
	}
}

class VirtualEmitter : CodeEmitter
{
	private MethodDescriptor md;
	private RemappedTypeWrapper wrapper;
	private MethodInfo method;

	internal VirtualEmitter(MethodDescriptor md, RemappedTypeWrapper wrapper)
	{
		this.md = md;
		this.wrapper = wrapper;
	}

	internal override void Emit(ILGenerator ilgen)
	{
		if(method == null)
		{
			Type[] args = new Type[md.ArgTypes.Length + 1];
			md.ArgTypes.CopyTo(args, 1);
			args[0] = wrapper.Type;
			method = wrapper.VirtualsHelperHack.GetMethod(md.Name, BindingFlags.Public | BindingFlags.Static, null, CallingConventions.Standard, args, null);
		}
		ilgen.Emit(OpCodes.Call, method);
	}
}

sealed class FieldWrapper
{
	private TypeWrapper declaringType;
	private string name;
	private string sig;
	private Modifiers modifiers;
	internal CodeEmitter EmitGet;
	internal CodeEmitter EmitSet;

	internal FieldWrapper(TypeWrapper declaringType, string name, string sig, Modifiers modifiers)
	{
		if(modifiers == Modifiers.Synthetic)
		{
			throw new InvalidOperationException();
		}
		this.declaringType = declaringType;
		this.name = name;
		this.sig = sig;
		this.modifiers = modifiers;
	}

	internal TypeWrapper DeclaringType
	{
		get
		{
			return declaringType;
		}
	}

	internal string Name
	{
		get
		{
			return name;
		}
	}

	internal Type FieldType
	{
		get
		{
			return declaringType.GetClassLoader().ExpressionType(sig);
		}
	}

	internal Modifiers Modifiers
	{
		get
		{
			return modifiers;
		}
	}

	internal bool IsStatic
	{
		get
		{
			return (modifiers & Modifiers.Static) != 0;
		}
	}

	internal bool IsPublic
	{
		get
		{
			return (modifiers & Modifiers.Public) != 0;
		}
	}

	internal bool IsPrivate
	{
		get
		{
			return (modifiers & Modifiers.Private) != 0;
		}
	}

	internal bool IsProtected
	{
		get
		{
			return (modifiers & Modifiers.Protected) != 0;
		}
	}

	internal bool IsFinal
	{
		get
		{
			return (modifiers & Modifiers.Final) != 0;
		}
	}

	internal bool IsVolatile
	{
		get
		{
			return (modifiers & Modifiers.Volatile) != 0;
		}
	}

	private class VolatileLongDoubleGetter : CodeEmitter
	{
		private static MethodInfo getFieldFromHandle = typeof(FieldInfo).GetMethod("GetFieldFromHandle");
		private static MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter");
		private static MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit");
		private FieldInfo fi;

		internal VolatileLongDoubleGetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			if(!fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Dup);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue, label);
				ilgen.ThrowException(typeof(NullReferenceException));
				ilgen.MarkLabel(label);
			}
			// HACK we lock on the FieldInfo object
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorEnter);
			if(fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Ldsfld, fi);
			}
			else
			{
				ilgen.Emit(OpCodes.Ldfld, fi);
			}
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorExit);
		}
	}

	private class VolatileLongDoubleSetter : CodeEmitter
	{
		private static MethodInfo getFieldFromHandle = typeof(FieldInfo).GetMethod("GetFieldFromHandle");
		private static MethodInfo monitorEnter = typeof(System.Threading.Monitor).GetMethod("Enter");
		private static MethodInfo monitorExit = typeof(System.Threading.Monitor).GetMethod("Exit");
		private FieldInfo fi;

		internal VolatileLongDoubleSetter(FieldInfo fi)
		{
			this.fi = fi;
		}

		internal override void Emit(ILGenerator ilgen)
		{
			if(!fi.IsStatic)
			{
				LocalBuilder local = ilgen.DeclareLocal(fi.FieldType);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Dup);
				Label label = ilgen.DefineLabel();
				ilgen.Emit(OpCodes.Brtrue, label);
				ilgen.ThrowException(typeof(NullReferenceException));
				ilgen.MarkLabel(label);
				ilgen.Emit(OpCodes.Ldloc, local);
			}
			// HACK we lock on the FieldInfo object
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorEnter);
			if(fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Stsfld, fi);
			}
			else
			{
				ilgen.Emit(OpCodes.Stfld, fi);
			}
			ilgen.Emit(OpCodes.Ldtoken, fi);
			ilgen.Emit(OpCodes.Call, getFieldFromHandle);
			ilgen.Emit(OpCodes.Call, monitorExit);
		}
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, FieldInfo fi, string sig, Modifiers modifiers)
	{
		FieldWrapper field = new FieldWrapper(declaringType, fi.Name, sig, modifiers);
		if(field.IsVolatile)
		{
			// long & double field accesses must be made atomic
			if(fi.FieldType == typeof(long) || fi.FieldType == typeof(double))
			{
				field.EmitGet = new VolatileLongDoubleGetter(fi);
				field.EmitSet = new VolatileLongDoubleSetter(fi);
				return field;
			}
			field.EmitGet += CodeEmitter.Create(OpCodes.Volatile);
			field.EmitSet += CodeEmitter.Create(OpCodes.Volatile);
		}
		if(field.IsStatic)
		{
			field.EmitGet += CodeEmitter.Create(OpCodes.Ldsfld, fi);
			field.EmitSet += CodeEmitter.Create(OpCodes.Stsfld, fi);
		}
		else
		{
			field.EmitGet += CodeEmitter.Create(OpCodes.Ldfld, fi);
			field.EmitSet += CodeEmitter.Create(OpCodes.Stfld, fi);
		}
		return field;
	}
}
