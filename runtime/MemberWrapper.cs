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
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Diagnostics;
using IKVM.Attributes;
using IKVM.Internal;
using IKVM.Runtime;

using ILGenerator = CountingILGenerator;

[Flags]
enum MemberFlags : short
{
	None = 0,
	HideFromReflection = 1,
	ExplicitOverride = 2
}

class MemberWrapper
{
	private System.Runtime.InteropServices.GCHandle handle;
	private TypeWrapper declaringType;
	private Modifiers modifiers;
	private MemberFlags flags;

	protected MemberWrapper(TypeWrapper declaringType, Modifiers modifiers, bool hideFromReflection)
		: this(declaringType, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
	{
	}

	protected MemberWrapper(TypeWrapper declaringType, Modifiers modifiers, MemberFlags flags)
	{
		Debug.Assert(declaringType != null);
		this.declaringType = declaringType;
		this.modifiers = modifiers;
		this.flags = flags;
	}

	~MemberWrapper()
	{
		if(handle.IsAllocated)
		{
			handle.Free();
		}
	}

	internal IntPtr Cookie
	{
		get
		{
			lock(this)
			{
				if(!handle.IsAllocated)
				{
					handle = System.Runtime.InteropServices.GCHandle.Alloc(this, System.Runtime.InteropServices.GCHandleType.Weak);
				}
			}
			return (IntPtr)handle;
		}
	}

	internal static MemberWrapper FromCookieImpl(IntPtr cookie)
	{
		return (MemberWrapper)((System.Runtime.InteropServices.GCHandle)cookie).Target;
	}

	internal TypeWrapper DeclaringType
	{
		get
		{
			return declaringType;
		}
	}

	internal bool IsAccessibleFrom(TypeWrapper caller, TypeWrapper instance)
	{
		return IsPublic ||
			caller == DeclaringType ||
			(IsProtected && (IsStatic ? caller.IsSubTypeOf(DeclaringType) : instance.IsSubTypeOf(caller))) ||
			(!IsPrivate && caller.IsInSamePackageAs(DeclaringType));
	}

	internal bool IsHideFromReflection
	{
		get
		{
			return (flags & MemberFlags.HideFromReflection) != 0;
		}
	}

	internal bool IsExplicitOverride
	{
		get
		{
			return (flags & MemberFlags.ExplicitOverride) != 0;
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
}

abstract class MethodWrapper : MemberWrapper
{
	private MethodDescriptor md;
	private MethodBase method;
	private string[] declaredExceptions;
	private TypeWrapper returnTypeWrapper;
	private TypeWrapper[] parameterTypeWrappers;

	internal virtual void EmitCall(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}

	internal virtual void EmitCallvirt(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}

	internal virtual void EmitNewobj(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}

	internal class GhostMethodWrapper : SmartMethodWrapper
	{
		private MethodInfo ghostMethod;

		internal GhostMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, md, method, returnType, parameterTypes, modifiers, flags)
		{
			// make sure we weren't handed the ghostMethod in the wrapper value type
			Debug.Assert(method == null || method.DeclaringType.IsInterface);
		}

		private void ResolveGhostMethod()
		{
			if(ghostMethod == null)
			{
				ghostMethod = DeclaringType.TypeAsParameterType.GetMethod(this.Name, this.GetParametersForDefineMethod());
				if(ghostMethod == null)
				{
					throw new InvalidOperationException("Unable to resolve ghost method");
				}
			}
		}

		protected override void CallvirtImpl(ILGenerator ilgen)
		{
			ResolveGhostMethod();
			ilgen.Emit(OpCodes.Call, ghostMethod);
		}

		internal override object Invoke(object obj, object[] args, bool nonVirtual)
		{
			object wrapper = Activator.CreateInstance(DeclaringType.TypeAsParameterType);
			DeclaringType.GhostRefField.SetValue(wrapper, obj);

			ResolveGhostMethod();
			return InvokeImpl(ghostMethod, wrapper, args, nonVirtual);
		}
	}

	internal static MethodWrapper Create(TypeWrapper declaringType, MethodDescriptor md, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection)
	{
		Debug.Assert(declaringType != null && md != null && method != null);

		if(declaringType.IsGhost)
		{
			// HACK since our caller isn't aware of the ghost issues, we'll handle the method swapping
			if(method.DeclaringType.IsValueType)
			{
				Type[] types = new Type[parameterTypes.Length];
				for(int i = 0; i < types.Length; i++)
				{
					types[i] = parameterTypes[i].TypeAsParameterType;
				}
				method = declaringType.TypeAsBaseType.GetMethod(method.Name, types);
			}
			return new GhostMethodWrapper(declaringType, md, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None);
		}
		else if(method is ConstructorInfo)
		{
			return new SmartConstructorMethodWrapper(declaringType, md, (ConstructorInfo)method, parameterTypes, modifiers, hideFromReflection);
		}
		else
		{
			return new SmartCallMethodWrapper(declaringType, md, (MethodInfo)method, returnType, parameterTypes, modifiers, hideFromReflection, OpCodes.Call, method.IsStatic ? OpCodes.Call : OpCodes.Callvirt);
		}
	}

	internal MethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
		: base(declaringType, modifiers, flags)
	{
		Profiler.Count("MethodWrapper");
		this.md = md;
		this.method = method;
		Debug.Assert(((returnType == null) == (parameterTypes == null)) || (returnType == PrimitiveTypeWrapper.VOID));
		this.returnTypeWrapper = returnType;
		this.parameterTypeWrappers = parameterTypes;
	}

	internal void SetDeclaredExceptions(string[] exceptions)
	{
		if(exceptions == null)
		{
			exceptions = new string[0];
		}
		this.declaredExceptions = (string[])exceptions.Clone();
	}

	internal void SetDeclaredExceptions(IKVM.Internal.MapXml.Throws[] throws)
	{
		if(throws != null)
		{
			declaredExceptions = new string[throws.Length];
			for(int i = 0; i < throws.Length; i++)
			{
				declaredExceptions[i] = throws[i].Class;
			}
		}
	}

	internal static MethodWrapper FromCookie(IntPtr cookie)
	{
		return (MethodWrapper)FromCookieImpl(cookie);
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

	internal string Signature
	{
		get
		{
			return md.Signature;
		}
	}

	internal bool IsLinked
	{
		get
		{
			return method != null;
		}
	}

	internal void Link()
	{
		lock(this)
		{
			if(parameterTypeWrappers == null)
			{
				Debug.Assert(returnTypeWrapper == null || returnTypeWrapper == PrimitiveTypeWrapper.VOID);
				ClassLoaderWrapper loader = this.DeclaringType.GetClassLoader();
				string sig = md.Signature;
				// TODO we need to use the actual classCache here
				System.Collections.Hashtable classCache = new System.Collections.Hashtable();
				returnTypeWrapper = ClassFile.RetTypeWrapperFromSig(loader, classCache, sig);
				parameterTypeWrappers = ClassFile.ArgTypeWrapperListFromSig(loader, classCache, sig);
				if(method == null)
				{
					try
					{
						method = this.DeclaringType.LinkMethod(this);
					}
					catch
					{
						// HACK if linking fails, we unlink to make sure
						// that the next link attempt will fail again
						returnTypeWrapper = null;
						parameterTypeWrappers = null;
						throw;
					}
				}
			}
		}
	}

	[Conditional("DEBUG")]
	internal void AssertLinked()
	{
		if(!(parameterTypeWrappers != null && returnTypeWrapper != null))
		{
			Tracer.Error(Tracer.Runtime, "AssertLinked failed: " + this.DeclaringType.Name + "::" + this.Name + this.Signature);
		}
		Debug.Assert(parameterTypeWrappers != null && returnTypeWrapper != null, this.DeclaringType.Name + "::" + this.Name + this.Signature);
	}

	internal TypeWrapper ReturnType
	{
		get
		{
			AssertLinked();
			return returnTypeWrapper;
		}
	}

	internal TypeWrapper[] GetParameters()
	{
		AssertLinked();
		return parameterTypeWrappers;
	}

	internal Type ReturnTypeForDefineMethod
	{
		get
		{
			return ReturnType.TypeAsParameterType;
		}
	}

	internal Type[] GetParametersForDefineMethod()
	{
		TypeWrapper[] wrappers = GetParameters();
		Type[] temp = new Type[wrappers.Length];
		for(int i = 0; i < wrappers.Length; i++)
		{
			temp[i] = wrappers[i].TypeAsParameterType;
		}
		return temp;
	}

	internal string[] GetExceptions()
	{
		// remapped types and dynamically compiled types have declaredExceptions set
		if(declaredExceptions != null)
		{
			return (string[])declaredExceptions.Clone();
		}
		// NOTE if method is a MethodBuilder, GetCustomAttributes doesn't work (and if
		// the method had any declared exceptions, the declaredExceptions field would have
		// been set)
		if(method != null && !(method is MethodBuilder))
		{
			object[] attributes = method.GetCustomAttributes(typeof(ThrowsAttribute), false);
			if(attributes.Length == 1)
			{
				return ((ThrowsAttribute)attributes[0]).Classes;
			}
		}
		return new string[0];
	}

	// we expose the underlying MethodBase object,
	// for Java types, this is the method that contains the compiled Java bytecode
	// for remapped types, this is the mbCore method (not the helper)
	// Note that for some artificial methods (notably wrap() in enums), method is null
	internal MethodBase GetMethod()
	{
		AssertLinked();
		return method;
	}

	internal string RealName
	{
		get
		{
			AssertLinked();
			return method.Name;
		}
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

	internal bool IsAbstract
	{
		get
		{
			return (Modifiers & Modifiers.Abstract) != 0;
		}
	}

	internal virtual object Invoke(object obj, object[] args, bool nonVirtual)
	{
		AssertLinked();
		// if we've still got the builder object, we need to replace it with the real thing before we can call it
		if(method is MethodBuilder)
		{
			bool found = false;
			int token = ((MethodBuilder)method).GetToken().Token;
			ModuleBuilder module = (ModuleBuilder)((MethodBuilder)method).GetModule();
			foreach(MethodInfo mi in this.DeclaringType.TypeAsTBD.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance))
			{
				if(module.GetMethodToken(mi).Token == token)
				{
					found = true;
					method = mi;
					break;
				}
			}
			if(!found)
			{
				throw new InvalidOperationException("Failed to fixate method: " + this.DeclaringType.Name + "." + this.Name + this.Signature);
			}
		}
		if(method is ConstructorBuilder)
		{
			bool found = false;
			int token = ((ConstructorBuilder)method).GetToken().Token;
			ModuleBuilder module = (ModuleBuilder)((ConstructorBuilder)method).GetModule();
			foreach(ConstructorInfo ci in this.DeclaringType.TypeAsTBD.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
			{
				if(module.GetConstructorToken(ci).Token == token)
				{
					found = true;
					method = ci;
					break;
				}
			}
			if(!found)
			{
				throw new InvalidOperationException("Failed to fixate constructor: " + this.DeclaringType.Name + "." + this.Name + this.Signature);
			}
		}
		return InvokeImpl(method, obj, args, nonVirtual);
	}

	private delegate object Invoker(IntPtr pFunc, object obj, object[] args);

	internal object InvokeImpl(MethodBase method, object obj, object[] args, bool nonVirtual)
	{
		Debug.Assert(!(method is MethodBuilder || method is ConstructorBuilder));

		if(IsStatic)
		{
			// Java allows bogus 'obj' to be specified for static methods
			obj = null;
		}
		else
		{
			if(md.Name == "<init>")
			{
				if(method is MethodInfo)
				{
					Debug.Assert(method.IsStatic);
					// we're dealing with a constructor on a remapped type, if obj is supplied, it means
					// that we should call the constructor on an already existing instance, but that isn't
					// possible with remapped types
					if(obj != null)
					{
						// the type of this exception is a bit random (note that this can only happen through JNI reflection or
						// if there is a bug in serialization [which uses the ObjectInputStream.callConstructor() in classpath.cs)
						throw JavaException.IncompatibleClassChangeError("Remapped type {0} doesn't support constructor invocation on an existing instance", DeclaringType.Name);
					}
				}
				else if(obj == null)
				{
					// calling <init> without an instance means that we're constructing a new instance
					// NOTE this means that we cannot detect a NullPointerException when calling <init> (does JNI require this?)
					try
					{
						InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, null, args);
						object o = ((ConstructorInfo)method).Invoke(proc.GetArgs());
						// since we just constructed an instance, it can't possibly be a ghost
						return o;
					}
					catch(ArgumentException x1)
					{
						throw JavaException.IllegalArgumentException(x1.Message);
					}
					catch(TargetInvocationException x)
					{
						throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
					}
				}
				else if(!method.DeclaringType.IsInstanceOfType(obj))
				{
					// we're trying to initialize an existing instance of a remapped type
					if(obj is System.Exception && (args == null || args.Length == 0))
					{
						// HACK special case for deserialization of java.lang.Throwable subclasses
						// we don't call the constructor here, it will be called by Throwable.readObject()
						return null;
					}
					else
					{
						// NOTE this will also happen if you try to deserialize a .NET object
						// (i.e. a type that doesn't derive from our java.lang.Object).
						// We might want to support that in the future (it's fairly easy, because
						// the call to Object.<init> can just be ignored)
						throw new NotSupportedException("Unable to partially construct object of type " + obj.GetType().FullName + " to type " + method.DeclaringType.FullName);
					}
				}
			}
			else if(nonVirtual && !method.IsStatic)
			{
				Invoker invoker = NonvirtualInvokeHelper.GetInvoker(this);
				try
				{
					InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, args);
					return invoker(method.MethodHandle.GetFunctionPointer(), proc.GetObj(), proc.GetArgs());
				}
				catch(ArgumentException x1)
				{
					throw JavaException.IllegalArgumentException(x1.Message);
				}
				catch(TargetInvocationException x)
				{
					throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
				}
			}
		}
		try
		{
			InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, args);
			object o = method.Invoke(proc.GetObj(), proc.GetArgs());
			TypeWrapper retType = this.ReturnType;
			if(!retType.IsUnloadable && retType.IsGhost)
			{
				o = retType.GhostRefField.GetValue(o);
			}
			return o;
		}
		catch(ArgumentException x1)
		{
			throw JavaException.IllegalArgumentException(x1.Message);
		}
		catch(TargetInvocationException x)
		{
			throw JavaException.InvocationTargetException(ExceptionHelper.MapExceptionFast(x.InnerException));
		}
	}

	private class NonvirtualInvokeHelper
	{
		private static Hashtable cache;
		private static ModuleBuilder module;

		private class KeyGen : IHashCodeProvider, IComparer
		{
			public int GetHashCode(object o)
			{
				MethodWrapper mw = (MethodWrapper)o;
				return mw.Signature.GetHashCode();
			}

			public int Compare(object x, object y)
			{
				MethodWrapper mw1 = (MethodWrapper)x;
				MethodWrapper mw2 = (MethodWrapper)y;
				if(mw1.ReturnType == mw2.ReturnType)
				{
					TypeWrapper[] p1 = mw1.GetParameters();
					TypeWrapper[] p2 = mw2.GetParameters();
					if(p1.Length == p2.Length)
					{
						for(int i = 0; i < p1.Length; i++)
						{
							if(p1[i] != p2[i])
							{
								return 1;
							}
						}
						return 0;
					}
				}
				return 1;
			}
		}

		static NonvirtualInvokeHelper()
		{
			KeyGen keygen = new KeyGen();
			cache = new Hashtable(keygen, keygen);
			AssemblyName name = new AssemblyName();
			name.Name = "NonvirtualInvoker";
			AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(name, ClassLoaderWrapper.IsSaveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
			if(ClassLoaderWrapper.IsSaveDebugImage)
			{
				module = ab.DefineDynamicModule("NonvirtualInvoker", "NonvirtualInvoker.dll");
				ClassLoaderWrapper.RegisterForSaveDebug(ab);
			}
			else
			{
				module = ab.DefineDynamicModule("NonvirtualInvoker");
			}
		}

		internal static Invoker GetInvoker(MethodWrapper mw)
		{
			lock(cache.SyncRoot)
			{
				Invoker inv = (Invoker)cache[mw];
				if(inv == null)
				{
					inv = CreateInvoker(mw);
					cache[mw] = inv;
				}
				return inv;
			}
		}

		private static Invoker CreateInvoker(MethodWrapper mw)
		{
			// TODO we need to support byref arguments...
			TypeBuilder typeBuilder = module.DefineType("class" + cache.Count);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Static, typeof(object), new Type[] { typeof(IntPtr), typeof(object), typeof(object[]) });
			ILGenerator ilgen = methodBuilder.GetILGenerator();
			ilgen.Emit(OpCodes.Ldarg_1);
			TypeWrapper[] paramTypes = mw.GetParameters();
			for(int i = 0; i < paramTypes.Length; i++)
			{
				ilgen.Emit(OpCodes.Ldarg_2);
				ilgen.Emit(OpCodes.Ldc_I4, i);
				ilgen.Emit(OpCodes.Ldelem_Ref);
				if(paramTypes[i].IsUnloadable)
				{
					// no need to do anything
				}
				else if(paramTypes[i].IsPrimitive)
				{
					ilgen.Emit(OpCodes.Unbox, paramTypes[i].TypeAsTBD);
					ilgen.Emit(OpCodes.Ldobj, paramTypes[i].TypeAsTBD);
				}
				else if(paramTypes[i].IsNonPrimitiveValueType)
				{
					paramTypes[i].EmitUnbox(ilgen);
				}
			}
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.EmitCalli(OpCodes.Calli, CallingConventions.HasThis, mw.ReturnTypeForDefineMethod, mw.GetParametersForDefineMethod(), null);
			if(mw.ReturnType.IsUnloadable)
			{
				// no need to do anything
			}
			else if(mw.ReturnType == PrimitiveTypeWrapper.VOID)
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
			else if(mw.ReturnType.IsGhost)
			{
				mw.ReturnType.EmitConvParameterToStackType(ilgen);
			}
			else if(mw.ReturnType.IsPrimitive)
			{
				ilgen.Emit(OpCodes.Box, mw.ReturnType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ret);
			Type type = typeBuilder.CreateType();
			return (Invoker)Delegate.CreateDelegate(typeof(Invoker), type.GetMethod("Invoke"));
		}
	}

	private struct InvokeArgsProcessor
	{
		private object obj;
		private object[] args;

		internal InvokeArgsProcessor(MethodWrapper mw, MethodBase method, object original_obj, object[] original_args)
		{
			TypeWrapper[] argTypes = mw.GetParameters();

			if(!mw.IsStatic && method.IsStatic && mw.md.Name != "<init>")
			{
				// we've been redirected to a static method, so we have to copy the 'obj' into the args
				args = new object[original_args.Length + 1];
				args[0] = original_obj;
				original_args.CopyTo(args, 1);
				this.obj = null;
				this.args = args;
				for(int i = 0; i < argTypes.Length; i++)
				{
					if(!argTypes[i].IsUnloadable && argTypes[i].IsGhost)
					{
						object v = Activator.CreateInstance(argTypes[i].TypeAsParameterType);
						argTypes[i].GhostRefField.SetValue(v, args[i + 1]);
						args[i + 1] = v;
					}
				}
			}
			else
			{
				this.obj = original_obj;
				this.args = original_args;
				for(int i = 0; i < argTypes.Length; i++)
				{
					if(!argTypes[i].IsUnloadable && argTypes[i].IsGhost)
					{
						if(this.args == original_args)
						{
							this.args = (object[])args.Clone();
						}
						object v = Activator.CreateInstance(argTypes[i].TypeAsParameterType);
						argTypes[i].GhostRefField.SetValue(v, args[i]);
						this.args[i] = v;
					}
				}
			}
		}

		internal object GetObj()
		{
			return obj;
		}

		internal object[] GetArgs()
		{
			return args;
		}
	}
}

class SmartMethodWrapper : MethodWrapper
{
	internal SmartMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
		: base(declaringType, md, method, returnType, parameterTypes, modifiers, flags)
	{
	}

	protected virtual void PreEmit(ILGenerator ilgen)
	{
	}
	
	protected void PostEmit(ILGenerator ilgen)
	{
		TypeWrapper retType = this.ReturnType;
		if(!retType.IsUnloadable)
		{
			if(retType.IsNonPrimitiveValueType)
			{
				retType.EmitBox(ilgen);
			}
			else if(retType.IsGhost)
			{
				LocalBuilder local = ilgen.DeclareLocal(retType.TypeAsParameterType);
				ilgen.Emit(OpCodes.Stloc, local);
				ilgen.Emit(OpCodes.Ldloca, local);
				ilgen.Emit(OpCodes.Ldfld, retType.GhostRefField);
			}
		}
	}

	internal sealed override void EmitCall(ILGenerator ilgen)
	{
		AssertLinked();
		PreEmit(ilgen);
		CallImpl(ilgen);
		PostEmit(ilgen);
	}

	protected virtual void CallImpl(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}

	internal sealed override void EmitCallvirt(ILGenerator ilgen)
	{
		AssertLinked();
		PreEmit(ilgen);
		if(DeclaringType.IsNonPrimitiveValueType)
		{
			// callvirt isn't allowed on a value type
			CallImpl(ilgen);
		}
		else
		{
			CallvirtImpl(ilgen);
		}
		PostEmit(ilgen);
	}

	protected virtual void CallvirtImpl(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}

	internal sealed override void EmitNewobj(ILGenerator ilgen)
	{
		AssertLinked();
		PreEmit(ilgen);
		NewobjImpl(ilgen);
		if(DeclaringType.IsNonPrimitiveValueType)
		{
			// HACK after constructing a new object, we don't want the custom boxing rule to run
			// (because that would turn "new IntPtr" into a null reference)
			ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
		}
	}

	protected virtual void NewobjImpl(ILGenerator ilgen)
	{
		throw new InvalidOperationException();
	}
}

sealed class SimpleCallMethodWrapper : MethodWrapper
{
	private OpCode call;
	private OpCode callvirt;

	internal SimpleCallMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection, OpCode call, OpCode callvirt)
		: base(declaringType, md, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
	{
		this.call = call;
		this.callvirt = callvirt;
	}

	internal override void EmitCall(ILGenerator ilgen)
	{
		ilgen.Emit(call, (MethodInfo)GetMethod());
	}

	internal override void EmitCallvirt(ILGenerator ilgen)
	{
		ilgen.Emit(callvirt, (MethodInfo)GetMethod());
	}
}

sealed class SmartCallMethodWrapper : SmartMethodWrapper
{
	private OpCode call;
	private OpCode callvirt;

	internal SmartCallMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection, OpCode call, OpCode callvirt)
		: this(declaringType, md, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None, call, callvirt)
	{
	}

	internal SmartCallMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags, OpCode call, OpCode callvirt)
		: base(declaringType, md, method, returnType, parameterTypes, modifiers, flags)
	{
		this.call = call;
		this.callvirt = callvirt;
	}

	protected override void CallImpl(ILGenerator ilgen)
	{
		ilgen.Emit(call, (MethodInfo)GetMethod());
	}

	protected override void CallvirtImpl(ILGenerator ilgen)
	{
		ilgen.Emit(callvirt, (MethodInfo)GetMethod());
	}
}

sealed class SmartConstructorMethodWrapper : SmartMethodWrapper
{
	internal SmartConstructorMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, ConstructorInfo method, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
		: base(declaringType, md, method, PrimitiveTypeWrapper.VOID, parameterTypes, modifiers, flags)
	{
	}

	internal SmartConstructorMethodWrapper(TypeWrapper declaringType, MethodDescriptor md, ConstructorInfo method, TypeWrapper[] parameterTypes, Modifiers modifiers, bool hideFromReflection)
		: base(declaringType, md, method, PrimitiveTypeWrapper.VOID, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
	{
	}

	protected override void CallImpl(ILGenerator ilgen)
	{
		ilgen.Emit(OpCodes.Call, (ConstructorInfo)GetMethod());
	}

	protected override void NewobjImpl(ILGenerator ilgen)
	{
		ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
	}
}

// This class tests if reflection on a constant field triggers the class constructor to run
// (it shouldn't run, but on .NET 1.0 & 1.1 it does)
sealed class ReflectionOnConstant
{
	private static bool isBroken;
	private static System.Collections.Hashtable warnOnce;

	static ReflectionOnConstant()
	{
		typeof(Helper).GetField("foo", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null);
	}

	internal static bool IsBroken
	{
		get
		{
			return isBroken;
		}
	}

	internal static void IssueWarning(FieldInfo field)
	{
		// FXBUG .NET (1.0 & 1.1)
		// FieldInfo.GetValue() on a literal causes the type initializer to run and
		// we don't want that.
		// TODO may need to find a workaround, for now we just spit out a warning
		if(ReflectionOnConstant.IsBroken && field.DeclaringType.TypeInitializer != null)
		{
			if(Tracer.FxBug.TraceWarning)
			{
				if(warnOnce == null)
				{
					warnOnce = new System.Collections.Hashtable();
				}
				if(!warnOnce.ContainsKey(field.DeclaringType.FullName))
				{
					warnOnce.Add(field.DeclaringType.FullName, null);
					Tracer.Warning(Tracer.FxBug, "Running type initializer for {0} due to CLR bug", field.DeclaringType.FullName);
				}
			}
		}
	}

	private class Helper
	{
		internal const int foo = 1;

		static Helper()
		{
			isBroken = true;
		}
	}
}

abstract class FieldWrapper : MemberWrapper
{
	private string name;
	private string sig;
	private FieldInfo field;
	private TypeWrapper fieldType;

	internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field)
		: base(declaringType, modifiers, false)
	{
		Debug.Assert(fieldType != null);
		Debug.Assert(name != null);
		Debug.Assert(sig != null);
		this.name = name;
		this.sig = sig;
		this.fieldType = fieldType;
		this.field = field;
	}

	internal FieldInfo GetField()
	{
		AssertLinked();
		return field;
	}

	[Conditional("DEBUG")]
	internal void AssertLinked()
	{
		if(fieldType == null)
		{
			Tracer.Error(Tracer.Runtime, "AssertLinked failed: " + this.DeclaringType.Name + "::" + this.name + " (" + this.sig + ")");
		}
		Debug.Assert(fieldType != null, this.DeclaringType.Name + "::" + this.name + " (" + this.sig+ ")");
	}

	// HACK used (indirectly thru IKVM.NativeCode.java.lang.Field.getConstant) by netexp to find out if the
	// field is a constant (and if it is, to get its value)
	internal object GetConstant()
	{
		AssertLinked();
		// NOTE only pritimives and string can be literals in Java (because the other "primitives" (like uint),
		// are treated as NonPrimitiveValueTypes)
		if(field != null && (fieldType.IsPrimitive || fieldType == CoreClasses.java.lang.String.Wrapper) && field.IsLiteral)
		{
			ReflectionOnConstant.IssueWarning(field);
			object val = field.GetValue(null);
			if(val != null && !(val is string))
			{
				return IKVM.NativeCode.java.lang.reflect.JavaWrapper.Box(val);
			}
			return val;
		}
		return null;
	}

	internal static FieldWrapper FromCookie(IntPtr cookie)
	{
		return (FieldWrapper)FromCookieImpl(cookie);
	}

	internal string Name
	{
		get
		{
			return name;
		}
	}

	internal TypeWrapper FieldTypeWrapper
	{
		get
		{
			AssertLinked();
			return fieldType;
		}
	}

	internal void EmitGet(ILGenerator ilgen)
	{
		AssertLinked();
		EmitGetImpl(ilgen);
	}

	protected abstract void EmitGetImpl(ILGenerator ilgen);

	internal void EmitSet(ILGenerator ilgen)
	{
		AssertLinked();
		EmitSetImpl(ilgen);
	}

	protected abstract void EmitSetImpl(ILGenerator ilgen);

	internal void Link()
	{
		lock(this)
		{
			if(fieldType == null)
			{
				// TODO we need to use the actual classCache here
				System.Collections.Hashtable classCache = new System.Collections.Hashtable();
				fieldType = ClassFile.FieldTypeWrapperFromSig(this.DeclaringType.GetClassLoader(), classCache, sig);
				try
				{
					this.DeclaringType.LinkField(this);
				}
				catch
				{
					// HACK if linking fails, we unlink to make sure
					// that the next link attempt will fail again
					fieldType = null;
					throw;
				}
			}
		}
	}

	internal bool IsVolatile
	{
		get
		{
			return (Modifiers & Modifiers.Volatile) != 0;
		}
	}

	internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers)
	{
		if(fieldType.IsUnloadable)
		{
			// TODO we might need to emit code to check the type dynamically
			// TODO the fact that the type is unloadable now, doesn't mean it will be unloadable when a method
			// that accesses this field is compiled, that means that that method may need to emit a cast
		}
		else
		{
			if(fieldType.IsGhost)
			{
				return new GhostFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
			}
			if((modifiers & Modifiers.Volatile) != 0)
			{
				// long & double field accesses must be made atomic
				if(fi.FieldType == typeof(long) || fi.FieldType == typeof(double))
				{
					return new VolatileLongDoubleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
				}
			}
		}
		if(declaringType.IsNonPrimitiveValueType)
		{
			return new NonPrimitiveValueTypeFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
		}
		else
		{
			return new SimpleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
		}
	}

	private void LookupField()
	{
		BindingFlags bindings = BindingFlags.Public | BindingFlags.NonPublic;
		if(IsStatic)
		{
			bindings |= BindingFlags.Static;
		}
		else
		{
			bindings |= BindingFlags.Instance;
		}
		// TODO instead of looking up the field by name, we should use the Token to find it.
		field = DeclaringType.TypeAsTBD.GetField(name, bindings);
		Debug.Assert(field != null);
	}

	internal virtual void SetValue(object obj, object val)
	{
		AssertLinked();
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null || field is FieldBuilder)
		{
			LookupField();
		}
		if(fieldType.IsGhost)
		{
			object temp = field.GetValue(obj);
			fieldType.GhostRefField.SetValue(temp, val);
			val = temp;
		}
		try
		{
			field.SetValue(obj, val);
		}
		catch(FieldAccessException x)
		{
			throw JavaException.IllegalAccessException(x.Message);
		}
	}

	internal virtual object GetValue(object obj)
	{
		AssertLinked();
		// TODO this is a broken implementation (for one thing, it needs to support redirection)
		if(field == null || field is FieldBuilder)
		{
			LookupField();
		}
		if(field.IsLiteral)
		{
			// on a non-broken CLR GetValue on a literal will not trigger type initialization, but on Java it should
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(field.DeclaringType.TypeHandle);
		}
		object val = field.GetValue(obj);
		if(fieldType.IsGhost)
		{
			val = fieldType.GhostRefField.GetValue(val);
		}
		return val;
	}
}

sealed class SimpleFieldWrapper : FieldWrapper
{
	internal SimpleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers)
		: base(declaringType, fieldType, name, sig, modifiers, fi)
	{
		Debug.Assert(!declaringType.IsNonPrimitiveValueType);
		Debug.Assert(!fieldType.IsGhost);
		Debug.Assert(fieldType != PrimitiveTypeWrapper.DOUBLE || fieldType != PrimitiveTypeWrapper.LONG || !IsVolatile);
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, GetField());
		if(!FieldTypeWrapper.IsUnloadable && FieldTypeWrapper.IsNonPrimitiveValueType)
		{
			FieldTypeWrapper.EmitBox(ilgen);
		}
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		if(!FieldTypeWrapper.IsUnloadable && FieldTypeWrapper.IsNonPrimitiveValueType)
		{
			FieldTypeWrapper.EmitUnbox(ilgen);
		}
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, GetField());
	}
}

sealed class VolatileLongDoubleFieldWrapper : FieldWrapper
{
	private static MethodInfo volatileReadDouble = typeof(System.Threading.Thread).GetMethod("VolatileRead", new Type[] { Type.GetType("System.Double&") });
	private static MethodInfo volatileReadLong = typeof(System.Threading.Thread).GetMethod("VolatileRead", new Type[] { Type.GetType("System.Int64&") });
	private static MethodInfo volatileWriteDouble = typeof(System.Threading.Thread).GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Double&"), typeof(double) });
	private static MethodInfo volatileWriteLong = typeof(System.Threading.Thread).GetMethod("VolatileWrite", new Type[] { Type.GetType("System.Int64&"), typeof(long) });

	internal VolatileLongDoubleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers)
		: base(declaringType, fieldType, name, sig, modifiers, fi)
	{
		Debug.Assert(IsVolatile);
		Debug.Assert(fieldType == PrimitiveTypeWrapper.DOUBLE || fieldType == PrimitiveTypeWrapper.LONG);
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		FieldInfo fi = GetField();
		if(fi.IsStatic)
		{
			ilgen.Emit(OpCodes.Ldsflda, fi);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ldflda, fi);
		}
		if(fi.FieldType == typeof(double))
		{
			ilgen.Emit(OpCodes.Call, volatileReadDouble);
		}
		else
		{
			Debug.Assert(fi.FieldType == typeof(long));
			ilgen.Emit(OpCodes.Call, volatileReadLong);
		}
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		FieldInfo fi = GetField();
		LocalBuilder temp = ilgen.DeclareLocal(fi.FieldType);
		ilgen.Emit(OpCodes.Stloc, temp);
		if(fi.IsStatic)
		{
			ilgen.Emit(OpCodes.Ldsflda, fi);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ldflda, fi);
		}
		ilgen.Emit(OpCodes.Ldloc, temp);
		if(fi.FieldType == typeof(double))
		{
			ilgen.Emit(OpCodes.Call, volatileWriteDouble);
		}
		else
		{
			Debug.Assert(fi.FieldType == typeof(long));
			ilgen.Emit(OpCodes.Call, volatileWriteLong);
		}
	}
}

sealed class GhostFieldWrapper : FieldWrapper
{
	internal GhostFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers)
		: base(declaringType, fieldType, name, sig, modifiers, fi)
	{
		Debug.Assert(fieldType.IsGhost);
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		FieldInfo fi = GetField();
		if(fi.IsStatic)
		{
			ilgen.Emit(OpCodes.Ldsflda, fi);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ldflda, fi);
		}
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(OpCodes.Ldfld, FieldTypeWrapper.GhostRefField);
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		FieldInfo fi = GetField();
		LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
		ilgen.Emit(OpCodes.Stloc, temp);
		if(fi.IsStatic)
		{
			ilgen.Emit(OpCodes.Ldsflda, fi);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ldflda, fi);
		}
		ilgen.Emit(OpCodes.Ldloc, temp);
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(OpCodes.Stfld, FieldTypeWrapper.GhostRefField);
	}
}

sealed class NonPrimitiveValueTypeFieldWrapper : FieldWrapper
{
	internal NonPrimitiveValueTypeFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers)
		: base(declaringType, fieldType, name, sig, modifiers, fi)
	{
		Debug.Assert(declaringType.IsNonPrimitiveValueType);
		Debug.Assert(!fieldType.IsGhost);
		Debug.Assert(fieldType != PrimitiveTypeWrapper.DOUBLE || fieldType != PrimitiveTypeWrapper.LONG || !IsVolatile);
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		if(!IsStatic)
		{
			ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
		}
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, GetField());
		if(!FieldTypeWrapper.IsUnloadable && FieldTypeWrapper.IsNonPrimitiveValueType)
		{
			FieldTypeWrapper.EmitBox(ilgen);
		}
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		if(!IsStatic)
		{
			FieldInfo fi = GetField();
			LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
			ilgen.Emit(OpCodes.Stloc, temp);
			ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			ilgen.Emit(OpCodes.Ldloc, temp);
		}
		if(!FieldTypeWrapper.IsUnloadable && FieldTypeWrapper.IsNonPrimitiveValueType)
		{
			FieldTypeWrapper.EmitUnbox(ilgen);
		}
		if(IsVolatile)
		{
			ilgen.Emit(OpCodes.Volatile);
		}
		ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, GetField());
	}
}

sealed class GetterFieldWrapper : FieldWrapper
{
	private MethodInfo getter;

	// NOTE fi may be null!
	internal GetterFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, Modifiers modifiers, MethodInfo getter)
		: base(declaringType, fieldType, name, sig, modifiers, fi)
	{
		Debug.Assert(!IsVolatile);

		this.getter = getter;
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		if(IsStatic)
		{
			ilgen.Emit(OpCodes.Call, getter);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Call, getter);
			}
			else
			{
				ilgen.Emit(getter.IsStatic ? OpCodes.Call : OpCodes.Callvirt, getter);
			}
		}
		if(FieldTypeWrapper.IsGhost)
		{
			LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsFieldType);
			ilgen.Emit(OpCodes.Stloc, temp);
			ilgen.Emit(OpCodes.Ldloca, temp);
			ilgen.Emit(OpCodes.Ldfld, FieldTypeWrapper.GhostRefField);
		}
		else if(FieldTypeWrapper.IsNonPrimitiveValueType)
		{
			FieldTypeWrapper.EmitBox(ilgen);
		}
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		FieldInfo fi = GetField();
		LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsLocalOrStackType);
		ilgen.Emit(OpCodes.Stloc, temp);
		if(FieldTypeWrapper.IsGhost)
		{
			if(fi.IsStatic)
			{
				ilgen.Emit(OpCodes.Ldsflda, fi);
			}
			else
			{
				if(DeclaringType.IsNonPrimitiveValueType)
				{
					ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				}
				ilgen.Emit(OpCodes.Ldflda, fi);
			}
			ilgen.Emit(OpCodes.Ldloc, temp);
			ilgen.Emit(OpCodes.Stfld, FieldTypeWrapper.GhostRefField);
		}
		else
		{
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			ilgen.Emit(OpCodes.Ldloc, temp);
			if(FieldTypeWrapper.IsNonPrimitiveValueType)
			{
				FieldTypeWrapper.EmitUnbox(ilgen);
			}
			ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
		}
	}
}

// NOTE this type is only used for remapped fields, dynamically compiled classes are always finished before we
// allow reflection (so we can look at the underlying field in that case)
sealed class ConstantFieldWrapper : FieldWrapper
{
	private object constant;

	internal ConstantFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, object constant)
		: base(declaringType, fieldType, name, sig, modifiers, field)
	{
		this.constant = constant;
	}

	protected override void EmitGetImpl(ILGenerator ilgen)
	{
		// NOTE even though you're not supposed to access a constant static final (the compiler is supposed
		// to inline them), we have to support it (because it does happen, e.g. if the field becomes final
		// after the referencing class was compiled)
		object v = GetValue(null);
		if(v == null)
		{
			ilgen.Emit(OpCodes.Ldnull);
		}
		else if(constant is int || 
			constant is short || constant is ushort ||
			constant is byte || constant is sbyte ||
			constant is char ||
			constant is bool)
		{
			ilgen.Emit(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
		}
		else if(constant is uint)
		{
			ilgen.Emit(OpCodes.Ldc_I4, unchecked((int)((IConvertible)constant).ToUInt32(null)));
		}
		else if(constant is string)
		{
			ilgen.Emit(OpCodes.Ldstr, (string)constant);
		}
		else if(constant is float)
		{
			ilgen.Emit(OpCodes.Ldc_R4, (float)constant);
		}
		else if(constant is double)
		{
			ilgen.Emit(OpCodes.Ldc_R8, (double)constant);
		}
		else if(constant is long)
		{
			ilgen.Emit(OpCodes.Ldc_I8, (long)constant);
		}
		else if(constant is ulong)
		{
			ilgen.Emit(OpCodes.Ldc_I8, unchecked((long)(ulong)constant));
		}
		else if(constant is Enum)
		{
			Type underlying = Enum.GetUnderlyingType(constant.GetType());
			if(underlying == typeof(long))
			{
				ilgen.Emit(OpCodes.Ldc_I8, ((IConvertible)constant).ToInt64(null));
			}
			if(underlying == typeof(ulong))
			{
				ilgen.Emit(OpCodes.Ldc_I8, unchecked((long)((IConvertible)constant).ToUInt64(null)));
			}
			else if(underlying == typeof(uint))
			{
				ilgen.Emit(OpCodes.Ldc_I4, unchecked((int)((IConvertible)constant).ToUInt32(null)));
			}
			else
			{
				ilgen.Emit(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
			}
		}
		else
		{
			throw new NotImplementedException(constant.GetType().FullName);
		}
	}

	protected override void EmitSetImpl(ILGenerator ilgen)
	{
		// when constant static final fields are updated, the JIT normally doesn't see that (because the
		// constant value is inlined), so we emulate that behavior by emitting a Pop
		ilgen.Emit(OpCodes.Pop);
	}

	internal override object GetValue(object obj)
	{
		if(constant == null)
		{
			constant = GetField().GetValue(null);
		}
		return constant;
	}
}
