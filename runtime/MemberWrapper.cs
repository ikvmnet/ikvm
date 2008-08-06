/*
  Copyright (C) 2002-2008 Jeroen Frijters

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
#if !COMPACT_FRAMEWORK
using System.Reflection.Emit;
#endif
using System.Diagnostics;
using IKVM.Attributes;

namespace IKVM.Internal
{
	[Flags]
	enum MemberFlags : short
	{
		None = 0,
		HideFromReflection = 1,
		ExplicitOverride = 2,
		MirandaMethod = 8,
		AccessStub = 16,
		InternalAccess = 32,  // member has "internal" access (@ikvm.lang.Internal)
		PropertyAccessor = 64,
		Intrinsic = 128,
		CallerID = 256,
	}

	class MemberWrapper
	{
#if !STATIC_COMPILER && !FIRST_PASS
		protected static readonly sun.reflect.ReflectionFactory reflectionFactory = (sun.reflect.ReflectionFactory)java.security.AccessController.doPrivileged(new sun.reflect.ReflectionFactory.GetReflectionFactoryAction());
#endif
		private System.Runtime.InteropServices.GCHandle handle;
		private TypeWrapper declaringType;
		private Modifiers modifiers;
		private MemberFlags flags;
		private string name;
		private string sig;

		protected MemberWrapper(TypeWrapper declaringType, string name, string sig, Modifiers modifiers, MemberFlags flags)
		{
			Debug.Assert(declaringType != null);
			this.declaringType = declaringType;
			this.name = String.Intern(name);
			this.sig = String.Intern(sig);
			this.modifiers = modifiers;
			this.flags = flags;
		}

		// NOTE since we don't support unloading code, there is no need to have a finalizer
#if CLASS_GC
	~MemberWrapper()
	{
		// NOTE when the AppDomain is being unloaded, we shouldn't clean up the handle, because
		// JNI code running in a finalize can use this handle later on (since finalization is
		// unordered). Note that this isn't a leak since the AppDomain is going away anyway.
		if(!Environment.HasShutdownStarted && handle.IsAllocated)
		{
			FreeHandle();
		}
	}

	private void FreeHandle()
	{
		// this has a LinkDemand, so it has to be in a separate method
		handle.Free();
	}
#endif

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

		internal bool IsAccessibleFrom(TypeWrapper referencedType, TypeWrapper caller, TypeWrapper instance)
		{
			if(referencedType.IsAccessibleFrom(caller))
			{
				return IsPublic ||
					caller == DeclaringType ||
					(IsProtected && caller.IsSubTypeOf(DeclaringType) && (IsStatic || instance.IsSubTypeOf(caller))) ||
					(IsInternal && DeclaringType.GetClassLoader().InternalsVisibleTo(caller.GetClassLoader())) ||
					(!IsPrivate && DeclaringType.IsPackageAccessibleFrom(caller));
			}
			return false;
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

		internal bool IsMirandaMethod
		{
			get
			{
				return (flags & MemberFlags.MirandaMethod) != 0;
			}
		}

		internal bool IsAccessStub
		{
			get
			{
				return (flags & MemberFlags.AccessStub) != 0;
			}
		}

		internal bool IsPropertyAccessor
		{
			get
			{
				return (flags & MemberFlags.PropertyAccessor) != 0;
			}
			set
			{
				// this is unsynchronized, so it may only be called during the JavaTypeImpl constructor
				if(value)
				{
					flags |= MemberFlags.PropertyAccessor;
				}
				else
				{
					flags &= ~MemberFlags.PropertyAccessor;
				}
			}
		}

		internal bool IsIntrinsic
		{
			get
			{
				return (flags & MemberFlags.Intrinsic) != 0;
			}
		}

		protected void SetIntrinsicFlag()
		{
			flags |= MemberFlags.Intrinsic;
		}

		internal bool HasCallerID
		{
			get
			{
				return (flags & MemberFlags.CallerID) != 0;
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

		internal bool IsInternal
		{
			get
			{
				return (flags & MemberFlags.InternalAccess) != 0;
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
#if !STATIC_COMPILER && !FIRST_PASS
		private volatile object reflectionMethod;
#endif
		internal static readonly MethodWrapper[] EmptyArray  = new MethodWrapper[0];
		private MethodBase method;
		private string[] declaredExceptions;
		private TypeWrapper returnTypeWrapper;
		private TypeWrapper[] parameterTypeWrappers;

#if !COMPACT_FRAMEWORK
		internal virtual void EmitCall(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal virtual void EmitCallvirt(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal void EmitNewobj(CodeEmitter ilgen)
		{
			EmitNewobj(ilgen, null, 0);
		}

		internal virtual void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
		{
			throw new InvalidOperationException();
		}
#endif
		internal virtual bool IsDynamicOnly
		{
			get
			{
				return false;
			}
		}

		internal class GhostMethodWrapper : SmartMethodWrapper
		{
			private MethodInfo ghostMethod;

			internal GhostMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
				: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
			{
				// make sure we weren't handed the ghostMethod in the wrapper value type
				Debug.Assert(method == null || method.DeclaringType.IsInterface);
			}

			private void ResolveGhostMethod()
			{
				if(ghostMethod == null)
				{
					ghostMethod = DeclaringType.TypeAsSignatureType.GetMethod(this.Name, this.GetParametersForDefineMethod());
					if(ghostMethod == null)
					{
						throw new InvalidOperationException("Unable to resolve ghost method");
					}
				}
			}

#if !COMPACT_FRAMEWORK
			protected override void CallvirtImpl(CodeEmitter ilgen)
			{
				ResolveGhostMethod();
				ilgen.Emit(OpCodes.Call, ghostMethod);
			}
#endif

#if !STATIC_COMPILER && !FIRST_PASS
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual, ikvm.@internal.CallerID callerID)
			{
				object wrapper = Activator.CreateInstance(DeclaringType.TypeAsSignatureType);
				DeclaringType.GhostRefField.SetValue(wrapper, obj);

				ResolveGhostMethod();
				return InvokeImpl(ghostMethod, wrapper, args, nonVirtual, callerID);
			}
#endif // !STATIC_COMPILER
		}

		internal static MethodWrapper Create(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
		{
			Debug.Assert(declaringType != null && name!= null && sig != null && method != null);

			if(declaringType.IsGhost)
			{
				// HACK since our caller isn't aware of the ghost issues, we'll handle the method swapping
				if(method.DeclaringType.IsValueType)
				{
					Type[] types = new Type[parameterTypes.Length];
					for(int i = 0; i < types.Length; i++)
					{
						types[i] = parameterTypes[i].TypeAsSignatureType;
					}
					method = declaringType.TypeAsBaseType.GetMethod(method.Name, types);
				}
				return new GhostMethodWrapper(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags);
			}
			else if(method is ConstructorInfo)
			{
				return new SmartConstructorMethodWrapper(declaringType, name, sig, (ConstructorInfo)method, parameterTypes, modifiers, flags);
			}
			else
			{
				return new SmartCallMethodWrapper(declaringType, name, sig, (MethodInfo)method, returnType, parameterTypes, modifiers, flags, SimpleOpCode.Call, method.IsStatic ? SimpleOpCode.Call : SimpleOpCode.Callvirt);
			}
		}

		internal MethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, modifiers, flags)
		{
			Profiler.Count("MethodWrapper");
			this.method = method;
			Debug.Assert(((returnType == null) == (parameterTypes == null)) || (returnType == PrimitiveTypeWrapper.VOID));
			this.returnTypeWrapper = returnType;
			this.parameterTypeWrappers = parameterTypes;
			if (Intrinsics.IsIntrinsic(this))
			{
				SetIntrinsicFlag();
			}
		}

		internal void SetDeclaredExceptions(string[] exceptions)
		{
			if(exceptions == null)
			{
				exceptions = new string[0];
			}
			this.declaredExceptions = (string[])exceptions.Clone();
		}

#if !STATIC_COMPILER
		internal object ToMethodOrConstructor(bool copy)
		{
#if FIRST_PASS
			return null;
#else
			object method = reflectionMethod;
			if (method == null)
			{
				TypeWrapper[] argTypes = GetParameters();
				java.lang.Class[] parameterTypes = new java.lang.Class[argTypes.Length];
				for (int i = 0; i < argTypes.Length; i++)
				{
					parameterTypes[i] = (java.lang.Class)argTypes[i].ClassObject;
				}
				string[] exceptions = GetExceptions();
				java.lang.Class[] checkedExceptions = new java.lang.Class[exceptions.Length];
				for (int i = 0; i < exceptions.Length; i++)
				{
					checkedExceptions[i] = (java.lang.Class)this.DeclaringType.GetClassLoader().LoadClassByDottedName(exceptions[i]).ClassObject;
				}
				if (this.Name == StringConstants.INIT)
				{
					method = reflectionFactory.newConstructor(
						(java.lang.Class)this.DeclaringType.ClassObject,
						parameterTypes,
						checkedExceptions,
						(int)this.Modifiers | (this.IsInternal ? 0x40000000 : 0),
						Array.IndexOf(this.DeclaringType.GetMethods(), this),
						this.DeclaringType.GetGenericMethodSignature(this),
						null,
						null
					);
				}
				else
				{
					method = reflectionFactory.newMethod(
						(java.lang.Class)this.DeclaringType.ClassObject,
						this.Name,
						parameterTypes,
						(java.lang.Class)this.ReturnType.ClassObject,
						checkedExceptions,
						(int)this.Modifiers | (this.IsInternal ? 0x40000000 : 0),
						Array.IndexOf(this.DeclaringType.GetMethods(), this),
						this.DeclaringType.GetGenericMethodSignature(this),
						null,
						null,
						null
					);
				}
				lock (this)
				{
					if (reflectionMethod == null)
					{
						reflectionMethod = method;
					}
					else
					{
						method = reflectionMethod;
					}
				}
			}
			if (copy)
			{
				java.lang.reflect.Constructor ctor = method as java.lang.reflect.Constructor;
				if (ctor != null)
				{
					return reflectionFactory.copyConstructor(ctor);
				}
				return reflectionFactory.copyMethod((java.lang.reflect.Method)method);
			}
			return method;
#endif
		}

		internal static MethodWrapper FromMethodOrConstructor(object methodOrConstructor)
		{
#if FIRST_PASS
			return null;
#else
			java.lang.reflect.Method method = methodOrConstructor as java.lang.reflect.Method;
			if (method != null)
			{
				return TypeWrapper.FromClass(method.getDeclaringClass()).GetMethods()[method._slot()];
			}
			java.lang.reflect.Constructor constructor = (java.lang.reflect.Constructor)methodOrConstructor;
			return TypeWrapper.FromClass(constructor.getDeclaringClass()).GetMethods()[constructor._slot()];
#endif
		}
#endif // !STATIC_COMPILER

		internal static MethodWrapper FromCookie(IntPtr cookie)
		{
			return (MethodWrapper)FromCookieImpl(cookie);
		}

		internal bool IsLinked
		{
			get
			{
				return parameterTypeWrappers != null;
			}
		}

		internal void Link()
		{
			lock(this)
			{
				if(parameterTypeWrappers != null)
				{
					return;
				}
			}
			ClassLoaderWrapper loader = this.DeclaringType.GetClassLoader();
			// TODO we need to use the actual classCache here
			System.Collections.Hashtable classCache = new System.Collections.Hashtable();
			TypeWrapper ret = ClassFile.RetTypeWrapperFromSig(loader, classCache, Signature);
			TypeWrapper[] parameters = ClassFile.ArgTypeWrapperListFromSig(loader, classCache, Signature);
			lock(this)
			{
				if(parameterTypeWrappers == null)
				{
					Debug.Assert(returnTypeWrapper == null || returnTypeWrapper == PrimitiveTypeWrapper.VOID);
					returnTypeWrapper = ret;
					parameterTypeWrappers = parameters;
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
				return ReturnType.TypeAsSignatureType;
			}
		}

		internal Type[] GetParametersForDefineMethod()
		{
			TypeWrapper[] wrappers = GetParameters();
			int len = wrappers.Length;
			if(HasCallerID)
			{
				len++;
			}
			Type[] temp = new Type[len];
			for(int i = 0; i < wrappers.Length; i++)
			{
				temp[i] = wrappers[i].TypeAsSignatureType;
			}
			if(HasCallerID)
			{
				temp[len - 1] = CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
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
#if COMPACT_FRAMEWORK
			if(method != null)
#else
			if(method != null && !(method is MethodBuilder))
#endif
			{
				ThrowsAttribute attr = AttributeHelper.GetThrows(method);
				if(attr != null)
				{
					return attr.Classes;
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
			MethodAttributes attribs = MethodAttributes.HideBySig;
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
			if(!IsStatic && !IsPrivate && Name != "<init>")
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

#if !STATIC_COMPILER
		[HideFromJava]
		internal object InvokeJNI(object obj, object[] args, bool nonVirtual, MethodBase callerID)
		{
#if FIRST_PASS
			return null;
#else
			return Invoke(obj, args, nonVirtual, ikvm.@internal.CallerID.create(callerID));
#endif
		}
#endif // !STATIC_COMPILER

#if !STATIC_COMPILER && !FIRST_PASS
		[HideFromJava]
		internal virtual object Invoke(object obj, object[] args, bool nonVirtual, ikvm.@internal.CallerID callerID)
		{
			AssertLinked();
			ResolveMethod();
			return InvokeImpl(method, obj, args, nonVirtual, callerID);
		}

		internal void ResolveMethod()
		{
#if !COMPACT_FRAMEWORK
			// if we've still got the builder object, we need to replace it with the real thing before we can call it
			if(method is MethodBuilder)
			{
				method = method.Module.ResolveMethod(((MethodBuilder)method).GetToken().Token);
			}
			if(method is ConstructorBuilder)
			{
				method = method.Module.ResolveMethod(((ConstructorBuilder)method).GetToken().Token);
			}
#endif // !COMPACT_FRAMEWORK
		}

		private delegate object Invoker(IntPtr pFunc, object obj, object[] args, ikvm.@internal.CallerID callerID);

		[HideFromJava]
		internal object InvokeImpl(MethodBase method, object obj, object[] args, bool nonVirtual, ikvm.@internal.CallerID callerID)
		{
#if !FIRST_PASS
#if !COMPACT_FRAMEWORK
			Debug.Assert(!(method is MethodBuilder || method is ConstructorBuilder));
#endif // !COMPACT_FRAMEWORK

			if(IsStatic)
			{
				// Java allows bogus 'obj' to be specified for static methods
				obj = null;
			}
			else
			{
				if(ReferenceEquals(Name, StringConstants.INIT))
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
							throw new java.lang.IncompatibleClassChangeError(string.Format("Remapped type {0} doesn't support constructor invocation on an existing instance", DeclaringType.Name));
						}
					}
					else if(obj == null)
					{
						// calling <init> without an instance means that we're constructing a new instance
						// NOTE this means that we cannot detect a NullPointerException when calling <init> (does JNI require this?)
						try
						{
							InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, null, args, callerID);
							object o = ((ConstructorInfo)method).Invoke(proc.GetArgs());
							// since we just constructed an instance, it can't possibly be a ghost
							return o;
						}
						catch(ArgumentException x1)
						{
							throw new java.lang.IllegalArgumentException(x1.Message);
						}
						catch(TargetInvocationException x)
						{
							throw new java.lang.reflect.InvocationTargetException(ikvm.runtime.Util.mapException(x.InnerException));
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
				else if(nonVirtual &&
					method.IsVirtual &&	// if the method isn't virtual, normal reflection will work
					!method.IsFinal &&	// if the method is final, normal reflection will work
					!method.DeclaringType.IsSealed && // if the type is sealed, normal reflection will work
					!method.IsAbstract)	// if the method is abstract, it doesn't make sense, so we'll do a virtual call
					// (Sun does a virtual call for interface methods and crashes for abstract methods)
				{
#if COMPACT_FRAMEWORK
					throw new NotSupportedException("Reflective non-virtual method invocation is not supported on the Compact Framework");
#else
					Invoker invoker = NonvirtualInvokeHelper.GetInvoker(this);
					try
					{
						InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, args, null);
						return invoker(method.MethodHandle.GetFunctionPointer(), proc.GetObj(), proc.GetArgs(), callerID);
					}
					catch(ArgumentException x1)
					{
						throw new java.lang.IllegalArgumentException(x1.Message);
					}
					catch(TargetInvocationException x)
					{
						throw new java.lang.reflect.InvocationTargetException(ikvm.runtime.Util.mapException(x.InnerException));
					}
#endif
				}
			}
			try
			{
				InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, args, callerID);
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
				throw new java.lang.IllegalArgumentException(x1.Message);
			}
			catch(TargetInvocationException x)
			{
				throw new java.lang.reflect.InvocationTargetException(ikvm.runtime.Util.mapException(x.InnerException));
			}
#else // !FIRST_PASS
			return null;
#endif
		}

#if !COMPACT_FRAMEWORK
		private static class NonvirtualInvokeHelper
		{
			private static Hashtable cache;
			private static ModuleBuilder module;

			private class KeyGen : IEqualityComparer
			{
				public int GetHashCode(object o)
				{
					MethodWrapper mw = (MethodWrapper)o;
					return mw.Signature.GetHashCode();
				}

				public new bool Equals(object x, object y)
				{
					return Compare(x, y) == 0;
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
				cache = new Hashtable(keygen);
				AssemblyName name = new AssemblyName();
				name.Name = "NonvirtualInvoker";
				AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(name, JVM.IsSaveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
				if(JVM.IsSaveDebugImage)
				{
					module = ab.DefineDynamicModule("NonvirtualInvoker", "NonvirtualInvoker.dll");
					DynamicClassLoader.RegisterForSaveDebug(ab);
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
				MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Static, typeof(object), new Type[] { typeof(IntPtr), typeof(object), typeof(object[]), typeof(ikvm.@internal.CallerID) });
				AttributeHelper.HideFromJava(methodBuilder);
				CodeEmitter ilgen = CodeEmitter.Create(methodBuilder);
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
				if(mw.HasCallerID)
				{
					ilgen.Emit(OpCodes.Ldarg_3);
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
					LocalBuilder local = ilgen.DeclareLocal(mw.ReturnType.TypeAsSignatureType);
					ilgen.Emit(OpCodes.Stloc, local);
					ilgen.Emit(OpCodes.Ldloca, local);
					ilgen.Emit(OpCodes.Ldfld, mw.ReturnType.GhostRefField);
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
#endif

		private struct InvokeArgsProcessor
		{
			private object obj;
			private object[] args;

			internal InvokeArgsProcessor(MethodWrapper mw, MethodBase method, object original_obj, object[] original_args, ikvm.@internal.CallerID callerID)
			{
				TypeWrapper[] argTypes = mw.GetParameters();

				if(!mw.IsStatic && method.IsStatic && mw.Name != "<init>")
				{
					// we've been redirected to a static method, so we have to copy the 'obj' into the args
					object[] nargs = new object[original_args.Length + 1];
					nargs[0] = original_obj;
					original_args.CopyTo(nargs, 1);
					this.obj = null;
					this.args = nargs;
					for(int i = 0; i < argTypes.Length; i++)
					{
						if(!argTypes[i].IsUnloadable && argTypes[i].IsGhost)
						{
							object v = Activator.CreateInstance(argTypes[i].TypeAsSignatureType);
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
							object v = Activator.CreateInstance(argTypes[i].TypeAsSignatureType);
							argTypes[i].GhostRefField.SetValue(v, args[i]);
							this.args[i] = v;
						}
					}
				}

				if(mw.HasCallerID)
				{
					object[] nargs = new object[args.Length + 1];
					Array.Copy(args, nargs, args.Length);
					nargs[args.Length] = callerID;
					args = nargs;
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
#endif // !STATIC_COMPILER && !FIRST_PASS

#if !COMPACT_FRAMEWORK
		internal static OpCode SimpleOpCodeToOpCode(SimpleOpCode opc)
		{
			switch(opc)
			{
				case SimpleOpCode.Call:
					return OpCodes.Call;
				case SimpleOpCode.Callvirt:
					return OpCodes.Callvirt;
				case SimpleOpCode.Newobj:
					return OpCodes.Newobj;
				default:
					throw new InvalidOperationException();
			}
		}
#endif
	}

	class SmartMethodWrapper : MethodWrapper
	{
		internal SmartMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
		}

#if !COMPACT_FRAMEWORK
		protected virtual void PreEmit(CodeEmitter ilgen)
		{
		}

		internal sealed override void EmitCall(CodeEmitter ilgen)
		{
			AssertLinked();
			PreEmit(ilgen);
			CallImpl(ilgen);
		}

		protected virtual void CallImpl(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal sealed override void EmitCallvirt(CodeEmitter ilgen)
		{
			AssertLinked();
			PreEmit(ilgen);
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				// callvirt isn't allowed on a value type
				// TODO we need to check for a null reference
				CallImpl(ilgen);
			}
			else
			{
				CallvirtImpl(ilgen);
			}
		}

		protected virtual void CallvirtImpl(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal sealed override void EmitNewobj(CodeEmitter ilgen, MethodAnalyzer ma, int opcodeIndex)
		{
			AssertLinked();
			PreEmit(ilgen);
			NewobjImpl(ilgen);
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				DeclaringType.EmitBox(ilgen);
			}
		}

		protected virtual void NewobjImpl(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}
#endif
	}

	enum SimpleOpCode : byte
	{
		Call,
		Callvirt,
		Newobj
	}

	sealed class SimpleCallMethodWrapper : MethodWrapper
	{
		private SimpleOpCode call;
		private SimpleOpCode callvirt;

		internal SimpleCallMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags, SimpleOpCode call, SimpleOpCode callvirt)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
			this.call = call;
			this.callvirt = callvirt;
		}

#if !COMPACT_FRAMEWORK
		internal override void EmitCall(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(call), (MethodInfo)GetMethod());
		}

		internal override void EmitCallvirt(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(callvirt), (MethodInfo)GetMethod());
		}
#endif
	}

	sealed class SmartCallMethodWrapper : SmartMethodWrapper
	{
		private SimpleOpCode call;
		private SimpleOpCode callvirt;

		internal SmartCallMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags, SimpleOpCode call, SimpleOpCode callvirt)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
			this.call = call;
			this.callvirt = callvirt;
		}

#if !COMPACT_FRAMEWORK
		protected override void CallImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(call), (MethodInfo)GetMethod());
		}

		protected override void CallvirtImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(callvirt), (MethodInfo)GetMethod());
		}
#endif
	}

	sealed class SmartConstructorMethodWrapper : SmartMethodWrapper
	{
		internal SmartConstructorMethodWrapper(TypeWrapper declaringType, string name, string sig, ConstructorInfo method, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, method, PrimitiveTypeWrapper.VOID, parameterTypes, modifiers, flags)
		{
		}

#if !COMPACT_FRAMEWORK
		protected override void CallImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, (ConstructorInfo)GetMethod());
		}

		protected override void NewobjImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
		}
#endif
	}

	abstract class FieldWrapper : MemberWrapper
	{
#if !STATIC_COMPILER && !FIRST_PASS
		private static readonly FieldInfo slotField = typeof(java.lang.reflect.Field).GetField("slot", BindingFlags.Instance | BindingFlags.NonPublic);
		private volatile object reflectionField;
#endif
		internal static readonly FieldWrapper[] EmptyArray  = new FieldWrapper[0];
		private FieldInfo field;
		private TypeWrapper fieldType;

		internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, MemberFlags flags)
			: base(declaringType, name, sig, modifiers, flags)
		{
			Debug.Assert(name != null);
			Debug.Assert(sig != null);
			this.fieldType = fieldType;
			this.field = field;
		}

		internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, ExModifiers modifiers, FieldInfo field)
			: this(declaringType, fieldType, name, sig, modifiers.Modifiers, field,
					(modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None))
		{
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
				Tracer.Error(Tracer.Runtime, "AssertLinked failed: " + this.DeclaringType.Name + "::" + this.Name + " (" + this.Signature + ")");
			}
			Debug.Assert(fieldType != null, this.DeclaringType.Name + "::" + this.Name + " (" + this.Signature+ ")");
		}

#if !STATIC_COMPILER
		// NOTE used (thru IKVM.Runtime.Util.GetFieldConstantValue) by ikvmstub to find out if the
		// field is a constant (and if it is, to get its value)
		internal object GetConstant()
		{
			AssertLinked();
			// only pritimives and string can be literals in Java (because the other "primitives" (like uint),
			// are treated as NonPrimitiveValueTypes)
			if(field != null && (fieldType.IsPrimitive || fieldType == CoreClasses.java.lang.String.Wrapper))
			{
				object val = null;
				if(field.IsLiteral)
				{
					val = field.GetRawConstantValue();
					if(field.FieldType.IsEnum)
					{
						val = DotNetTypeWrapper.EnumValueFieldWrapper.GetEnumPrimitiveValue(Enum.GetUnderlyingType(field.FieldType), val);
					}
				}
				else
				{
					// NOTE instance fields can also be "constant" and we round trip this information to make the Japi results look
					// nice (but otherwise this has no practical value), but note that this only works when the code is compiled
					// with -strictfieldfieldsemantics (because the ConstantValueAttribute is on the field and when we're a GetterFieldWrapper
					// we don't have access to the corresponding field).
					val = AttributeHelper.GetConstantValue(field);
				}
				if(val != null && !(val is string))
				{
					return JVM.Box(val);
				}
				return val;
			}
			return null;
		}

		internal static FieldWrapper FromField(object field)
		{
#if FIRST_PASS
			return null;
#else
			java.lang.reflect.Field f = (java.lang.reflect.Field)field;
			return TypeWrapper.FromClass(f.getDeclaringClass()).GetFields()[(int)slotField.GetValue(f)];
#endif
		}

		internal object ToField(bool copy)
		{
#if FIRST_PASS
			return null;
#else
			object field = reflectionField;
			if (field == null)
			{
				field = reflectionFactory.newField(
					(java.lang.Class)this.DeclaringType.ClassObject,
					this.Name,
					(java.lang.Class)this.FieldTypeWrapper.ClassObject,
					(int)this.Modifiers | (this.IsInternal ? 0x40000000 : 0),
					Array.IndexOf(this.DeclaringType.GetFields(), this),
					this.DeclaringType.GetGenericFieldSignature(this),
					null
				);
			}
			lock (this)
			{
				if (reflectionField == null)
				{
					reflectionField = field;
				}
				else
				{
					field = reflectionField;
				}
			}
			if (copy)
			{
				field = reflectionFactory.copyField((java.lang.reflect.Field)field);
			}
			return field;
#endif // FIRST_PASS
		}
#endif // !STATIC_COMPILER

		internal static FieldWrapper FromCookie(IntPtr cookie)
		{
			return (FieldWrapper)FromCookieImpl(cookie);
		}

		internal TypeWrapper FieldTypeWrapper
		{
			get
			{
				AssertLinked();
				return fieldType;
			}
		}

#if !COMPACT_FRAMEWORK
		internal void EmitGet(CodeEmitter ilgen)
		{
			AssertLinked();
			EmitGetImpl(ilgen);
		}

		protected abstract void EmitGetImpl(CodeEmitter ilgen);

		internal void EmitSet(CodeEmitter ilgen)
		{
			AssertLinked();
			EmitSetImpl(ilgen);
		}

		protected abstract void EmitSetImpl(CodeEmitter ilgen);
#endif

		internal void Link()
		{
			lock(this)
			{
				if(fieldType != null)
				{
					return;
				}
			}
			// TODO we need to use the actual classCache here
			System.Collections.Hashtable classCache = new System.Collections.Hashtable();
			TypeWrapper fld = ClassFile.FieldTypeWrapperFromSig(this.DeclaringType.GetClassLoader(), classCache, Signature);
			lock(this)
			{
				if(fieldType == null)
				{
					fieldType = fld;
					try
					{
						field = this.DeclaringType.LinkField(this);
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

		internal static FieldWrapper Create(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
		{
			// volatile long & double field accesses must be made atomic
			if((modifiers.Modifiers & Modifiers.Volatile) != 0 && (sig == "J" || sig == "D"))
			{
				return new VolatileLongDoubleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
			}
			return new SimpleFieldWrapper(declaringType, fieldType, fi, name, sig, modifiers);
		}

		internal virtual void ResolveField()
		{
			FieldBuilder fb = field as FieldBuilder;
			if(fb != null)
			{
				field = field.Module.ResolveField(fb.GetToken().Token);
			}
		}

#if !STATIC_COMPILER
		internal virtual void SetValue(object obj, object val)
		{
			AssertLinked();
			ResolveField();
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
#if !FIRST_PASS
				throw new java.lang.IllegalAccessException(x.Message);
#endif
			}
		}
#endif // !STATIC_COMPILER

		internal virtual object GetValue(object obj)
		{
			AssertLinked();
			ResolveField();
#if STATIC_COMPILER
			return field.GetValue(null);
#else
			object val = field.GetValue(obj);
			if(fieldType.IsGhost)
			{
				val = fieldType.GhostRefField.GetValue(val);
			}
			return val;
#endif // STATIC_COMPILER
		}
	}

	sealed class SimpleFieldWrapper : FieldWrapper
	{
		internal SimpleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(!(fieldType == PrimitiveTypeWrapper.DOUBLE || fieldType == PrimitiveTypeWrapper.LONG) || !IsVolatile);
		}

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			if(!IsStatic && DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
			}
			if(IsVolatile)
			{
				ilgen.Emit(OpCodes.Volatile);
			}
			ilgen.Emit(IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, GetField());
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			FieldInfo fi = GetField();
			if(!IsStatic && DeclaringType.IsNonPrimitiveValueType)
			{
				LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, temp);
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, temp);
			}
			if(IsVolatile)
			{
				ilgen.Emit(OpCodes.Volatile);
			}
			ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
		}
#endif
	}

	sealed class VolatileLongDoubleFieldWrapper : FieldWrapper
	{
		internal VolatileLongDoubleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(IsVolatile);
			Debug.Assert(sig == "J" || sig == "D");
		}

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(CodeEmitter ilgen)
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
			if(FieldTypeWrapper == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileReadDouble);
			}
			else
			{
				Debug.Assert(FieldTypeWrapper == PrimitiveTypeWrapper.LONG);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileReadLong);
			}
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			FieldInfo fi = GetField();
			LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
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
			if(FieldTypeWrapper == PrimitiveTypeWrapper.DOUBLE)
			{
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileWriteDouble);
			}
			else
			{
				Debug.Assert(FieldTypeWrapper == PrimitiveTypeWrapper.LONG);
				ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.volatileWriteLong);
			}
		}
#endif
	}

	sealed class GetterFieldWrapper : FieldWrapper
	{
		private MethodInfo getter;
		private PropertyInfo prop;

		// NOTE fi may be null!
		internal GetterFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers, MethodInfo getter, PropertyInfo prop)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(!IsVolatile);

			this.getter = getter;
			this.prop = prop;
		}

		internal void SetGetter(MethodInfo getter)
		{
			this.getter = getter;
		}

		internal MethodInfo GetGetter()
		{
			return getter;
		}

		internal PropertyInfo GetProperty()
		{
			return prop;
		}

#if !STATIC_COMPILER
		internal override object GetValue(object obj)
		{
			return prop.GetValue(obj, null);
		}

		internal override void SetValue(object obj, object val)
		{
			if(FieldTypeWrapper.IsGhost)
			{
				object temp = GetValue(obj);
				FieldTypeWrapper.GhostRefField.SetValue(temp, val);
				val = temp;
			}
			prop.SetValue(obj, val, null);
		}
#endif

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			if(!IsStatic && DeclaringType.IsNonPrimitiveValueType)
			{
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Call, getter);
			}
			else
			{
				// NOTE we look at the static-ness of the getter method and not our own,
				// because for instance fields we can still have a static getter method
				ilgen.Emit(getter.IsStatic ? OpCodes.Call : OpCodes.Callvirt, getter);
			}
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			if(!IsStatic && DeclaringType.IsNonPrimitiveValueType)
			{
				LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, temp);
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, temp);
			}
			FieldInfo fi = GetField();
			if(fi != null)
			{
				// common case (we're in a DynamicTypeWrapper and the caller is too)
				ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
			}
			else
			{
				// this means that we are an instance on a CompiledTypeWrapper and we're being called
				// from DynamicMethod based reflection, so we can safely emit a call to the private
				// setter, because the DynamicMethod is allowed to access our private members.
				ilgen.Emit(OpCodes.Call, prop.GetSetMethod(true));
			}
		}
#endif
	}

	// this class represents a .NET property defined in Java with the ikvm.lang.Property annotation
	sealed class DynamicPropertyFieldWrapper : FieldWrapper
	{
		private readonly MethodWrapper getter;
		private readonly MethodWrapper setter;
		private PropertyBuilder pb;

		private MethodWrapper GetMethod(string name, string sig, bool isstatic)
		{
			if(name != null)
			{
				MethodWrapper mw = this.DeclaringType.GetMethodWrapper(name, sig, false);
				if(mw != null && mw.IsStatic == isstatic)
				{
					mw.IsPropertyAccessor = true;
					return mw;
				}
				Tracer.Error(Tracer.Compiler, "Property '{0}' accessor '{1}' not found in class '{2}'", this.Name, name, this.DeclaringType.Name);
			}
			return null;
		}

		internal DynamicPropertyFieldWrapper(TypeWrapper declaringType, ClassFile.Field fld)
			: base(declaringType, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal), null)
		{
			getter = GetMethod(fld.PropertyGetter, "()" + fld.Signature, fld.IsStatic);
			setter = GetMethod(fld.PropertySetter, "(" + fld.Signature + ")V", fld.IsStatic);
		}

#if !STATIC_COMPILER && !FIRST_PASS
		internal override void ResolveField()
		{
			if (getter != null)
			{
				getter.ResolveMethod();
			}
			if (setter != null)
			{
				setter.ResolveMethod();
			}
		}
#endif

		internal PropertyBuilder GetPropertyBuilder()
		{
			AssertLinked();
			return pb;
		}

		internal void DoLink(TypeBuilder tb)
		{
			if(getter != null)
			{
				getter.Link();
			}
			if(setter != null)
			{
				setter.Link();
			}
			pb = tb.DefineProperty(this.Name, PropertyAttributes.None, this.FieldTypeWrapper.TypeAsSignatureType, Type.EmptyTypes);
			if(getter != null)
			{
				pb.SetGetMethod((MethodBuilder)getter.GetMethod());
			}
			if(setter != null)
			{
				pb.SetSetMethod((MethodBuilder)setter.GetMethod());
			}
#if STATIC_COMPILER
			AttributeHelper.SetModifiers(pb, this.Modifiers, this.IsInternal);
#endif
		}

#if !STATIC_COMPILER && !FIRST_PASS
		internal override object GetValue(object obj)
		{
			if(getter == null)
			{
				throw new global::java.lang.NoSuchMethodError();
			}
			return getter.Invoke(obj, new object[0], false, null);
		}

		internal override void SetValue(object obj, object val)
		{
			if(setter == null)
			{
				throw new global::java.lang.NoSuchMethodError();
			}
			setter.Invoke(obj, new object[] { val }, false, null);
		}
#endif

		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			if(getter == null)
			{
				EmitThrowNoSuchMethodErrorForGetter(ilgen, this.FieldTypeWrapper, this.IsStatic);
			}
			else if(getter.IsStatic)
			{
				getter.EmitCall(ilgen);
			}
			else
			{
				getter.EmitCallvirt(ilgen);
			}
		}

		internal static void EmitThrowNoSuchMethodErrorForGetter(CodeEmitter ilgen, TypeWrapper type, bool isStatic)
		{
			// HACK the branch around the throw is to keep the verifier happy
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Ldc_I4_0);
			ilgen.Emit(OpCodes.Brtrue_S, label);
			EmitHelper.Throw(ilgen, "java.lang.NoSuchMethodError");
			ilgen.MarkLabel(label);
			if (!isStatic)
			{
				ilgen.Emit(OpCodes.Pop);
			}
			ilgen.Emit(OpCodes.Ldloc, ilgen.DeclareLocal(type.TypeAsLocalOrStackType));
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			if(setter == null)
			{
				if(this.IsFinal)
				{
					ilgen.Emit(OpCodes.Pop);
					if(!this.IsStatic)
					{
						ilgen.Emit(OpCodes.Pop);
					}
				}
				else
				{
					EmitThrowNoSuchMethodErrorForSetter(ilgen, this.IsStatic);
				}
			}
			else if(setter.IsStatic)
			{
				setter.EmitCall(ilgen);
			}
			else
			{
				setter.EmitCallvirt(ilgen);
			}
		}

		internal static void EmitThrowNoSuchMethodErrorForSetter(CodeEmitter ilgen, bool isStatic)
		{
			// HACK the branch around the throw is to keep the verifier happy
			CodeEmitterLabel label = ilgen.DefineLabel();
			ilgen.Emit(OpCodes.Ldc_I4_0);
			ilgen.Emit(OpCodes.Brtrue_S, label);
			EmitHelper.Throw(ilgen, "java.lang.NoSuchMethodError");
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Pop);
			if (!isStatic)
			{
				ilgen.Emit(OpCodes.Pop);
			}
		}
	}

	// this class represents a .NET property defined in Java with the ikvm.lang.Property annotation
	sealed class CompiledPropertyFieldWrapper : FieldWrapper
	{
		private readonly PropertyInfo property;

		internal CompiledPropertyFieldWrapper(TypeWrapper declaringType, PropertyInfo property, ExModifiers modifiers)
			: base(declaringType, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType), property.Name, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType).SigName, modifiers, null)
		{
			this.property = property;
		}

#if !STATIC_COMPILER && !FIRST_PASS
		internal override object GetValue(object obj)
		{
			if (!property.CanRead)
			{
				throw new global::java.lang.NoSuchMethodError();
			}
			return property.GetValue(obj, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty, null, null, null);
		}

		internal override void SetValue(object obj, object val)
		{
			if(!property.CanWrite)
			{
				throw new global::java.lang.NoSuchMethodError();
			}
			property.SetValue(obj, val, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty, null, null, null);
		}
#endif

		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			MethodInfo getter = property.GetGetMethod(true);
			if(getter == null)
			{
				DynamicPropertyFieldWrapper.EmitThrowNoSuchMethodErrorForGetter(ilgen, this.FieldTypeWrapper, this.IsStatic);
			}
			else if(getter.IsStatic)
			{
				ilgen.Emit(OpCodes.Call, getter);
			}
			else
			{
				ilgen.Emit(OpCodes.Callvirt, getter);
			}
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			MethodInfo setter = property.GetSetMethod(true);
			if (setter == null)
			{
				if(this.IsFinal)
				{
					ilgen.Emit(OpCodes.Pop);
					if(!this.IsStatic)
					{
						ilgen.Emit(OpCodes.Pop);
					}
				}
				else
				{
					DynamicPropertyFieldWrapper.EmitThrowNoSuchMethodErrorForSetter(ilgen, this.IsStatic);
				}
			}
			else if(setter.IsStatic)
			{
				ilgen.Emit(OpCodes.Call, setter);
			}
			else
			{
				ilgen.Emit(OpCodes.Callvirt, setter);
			}
		}

		internal PropertyInfo GetProperty()
		{
			return property;
		}
	}

	sealed class ConstantFieldWrapper : FieldWrapper
	{
		private object constant;

		internal ConstantFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, object constant, MemberFlags flags)
			: base(declaringType, fieldType, name, sig, modifiers, field, flags)
		{
			Debug.Assert(IsStatic);
			this.constant = constant;
		}

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			// Reading a field should trigger the cctor, but since we're inlining the value
			// we have to trigger it explicitly
			DeclaringType.EmitRunClassConstructor(ilgen);

			// NOTE even though you're not supposed to access a constant static final (the compiler is supposed
			// to inline them), we have to support it (because it does happen, e.g. if the field becomes final
			// after the referencing class was compiled, or when we're accessing an unsigned primitive .NET field)
			object v = GetConstantValue();
			if(v == null)
			{
				ilgen.Emit(OpCodes.Ldnull);
			}
			else if(constant is int || 
				constant is short ||
				constant is ushort ||
				constant is byte ||
				constant is sbyte ||
				constant is char ||
				constant is bool)
			{
				ilgen.Emit(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
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
			else if(constant is uint)
			{
				ilgen.Emit(OpCodes.Ldc_I4, unchecked((int)((IConvertible)constant).ToUInt32(null)));
			}
			else if(constant is ulong)
			{
				ilgen.Emit(OpCodes.Ldc_I8, unchecked((long)(ulong)constant));
			}
			else if(constant is Enum)
			{
				object val = DotNetTypeWrapper.EnumValueFieldWrapper.GetEnumPrimitiveValue(constant);
				if(val is long)
				{
					ilgen.Emit(OpCodes.Ldc_I8, (long)constant);
				}
				else
				{
					ilgen.Emit(OpCodes.Ldc_I4, ((IConvertible)constant).ToInt32(null));
				}
			}
			else
			{
				throw new InvalidOperationException(constant.GetType().FullName);
			}
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			// when constant static final fields are updated, the JIT normally doesn't see that (because the
			// constant value is inlined), so we emulate that behavior by emitting a Pop
			ilgen.Emit(OpCodes.Pop);
		}
#endif
		internal object GetConstantValue()
		{
			if(constant == null)
			{
				FieldInfo field = GetField();
#if !STATIC_COMPILER
				if(field.FieldType.IsEnum && !field.DeclaringType.IsEnum)
				{
					if(field.DeclaringType.Assembly.ReflectionOnly)
					{
						return null;
					}
					constant = field.GetValue(null);
				}
				else
#endif // !STATIC_COMPILER
				{
					constant = field.GetRawConstantValue();
				}
			}
			return constant;
		}

		internal override object GetValue(object obj)
		{
			// on a non-broken CLR GetValue on a literal will not trigger type initialization, but on Java it should
			DeclaringType.RunClassInit();
			return GetConstantValue();
		}
	}

#if !COMPACT_FRAMEWORK
	// This type is used during AOT compilation only!
	sealed class AotAccessStubFieldWrapper : FieldWrapper
	{
		private FieldWrapper basefield;
		private MethodBuilder getter;
		private MethodBuilder setter;

		internal AotAccessStubFieldWrapper(TypeWrapper wrapper, FieldWrapper basefield)
			: base(wrapper, null, basefield.Name, basefield.Signature, basefield.Modifiers, null, MemberFlags.AccessStub | MemberFlags.HideFromReflection)
		{
			this.basefield = basefield;
		}

		private string GenerateUniqueMethodName(string basename, Type returnType, Type[] parameterTypes)
		{
			return ((DynamicTypeWrapper)this.DeclaringType).GenerateUniqueMethodName(basename, returnType, parameterTypes);
		}

		internal void DoLink(TypeBuilder typeBuilder)
		{
			basefield.Link();
			if(basefield is ConstantFieldWrapper)
			{
				FieldAttributes attribs = basefield.IsPublic ? FieldAttributes.Public : FieldAttributes.FamORAssem;
				attribs |= FieldAttributes.Static | FieldAttributes.Literal;
				FieldBuilder fb = typeBuilder.DefineField(Name, basefield.FieldTypeWrapper.TypeAsSignatureType, attribs);
				AttributeHelper.HideFromReflection(fb);
				fb.SetConstant(((ConstantFieldWrapper)basefield).GetConstantValue());
			}
			else
			{
				Type propType = basefield.FieldTypeWrapper.TypeAsSignatureType;
				PropertyBuilder pb = typeBuilder.DefineProperty(Name, PropertyAttributes.None, propType, Type.EmptyTypes);
				AttributeHelper.HideFromReflection(pb);
				MethodAttributes attribs = basefield.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
				attribs |= MethodAttributes.HideBySig;
				if(basefield.IsStatic)
				{
					attribs |= MethodAttributes.Static;
				}
				getter = typeBuilder.DefineMethod(GenerateUniqueMethodName("get_" + Name, propType, Type.EmptyTypes), attribs, propType, Type.EmptyTypes);
				AttributeHelper.HideFromJava(getter);
				pb.SetGetMethod(getter);
				CodeEmitter ilgen = CodeEmitter.Create(getter);
				if(!basefield.IsStatic)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
				}
				basefield.EmitGet(ilgen);
				ilgen.Emit(OpCodes.Ret);
				if(!basefield.IsFinal)
				{
					setter = typeBuilder.DefineMethod(GenerateUniqueMethodName("set_" + Name, typeof(void), new Type[] { propType }), attribs, null, new Type[] { propType });
					AttributeHelper.HideFromJava(setter);
					pb.SetSetMethod(setter);
					ilgen = CodeEmitter.Create(setter);
					ilgen.Emit(OpCodes.Ldarg_0);
					if(!basefield.IsStatic)
					{
						ilgen.Emit(OpCodes.Ldarg_1);
					}
					basefield.EmitSet(ilgen);
					ilgen.Emit(OpCodes.Ret);
				}
			}
		}

		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			if(basefield is ConstantFieldWrapper)
			{
				basefield.EmitGet(ilgen);
			}
			else
			{
				ilgen.Emit(OpCodes.Call, getter);
			}
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, setter);
		}

#if !STATIC_COMPILER
		internal override object GetValue(object obj)
		{
			// We're MemberFlags.HideFromReflection, so we should never be called
			throw new InvalidOperationException();
		}

		internal override void SetValue(object obj, object val)
		{
			// We're MemberFlags.HideFromReflection, so we should never be called
			throw new InvalidOperationException();
		}
#endif // !STATIC_COMPILER
	}
#endif

	sealed class CompiledAccessStubFieldWrapper : FieldWrapper
	{
		private MethodInfo getter;
		private MethodInfo setter;

		private static Modifiers GetModifiers(PropertyInfo property)
		{
			// NOTE we only support the subset of modifiers that is expected for "access stub" properties
			MethodInfo getter = property.GetGetMethod(true);
			Modifiers modifiers = getter.IsPublic ? Modifiers.Public : Modifiers.Protected;
			if(!property.CanWrite)
			{
				modifiers |= Modifiers.Final;
			}
			if(getter.IsStatic)
			{
				modifiers |= Modifiers.Static;
			}
			return modifiers;
		}

		internal CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property)
			: base(wrapper, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType), property.Name, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType).SigName, GetModifiers(property), null, MemberFlags.AccessStub | MemberFlags.HideFromReflection)
		{
			this.getter = property.GetGetMethod(true);
			this.setter = property.GetSetMethod(true);
		}

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, getter);
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, setter);
		}
#endif

#if !STATIC_COMPILER
		internal override object GetValue(object obj)
		{
			// We're MemberFlags.HideFromReflection, so we should never be called
			throw new InvalidOperationException();
		}

		internal override void SetValue(object obj, object val)
		{
			// We're MemberFlags.HideFromReflection, so we should never be called
			throw new InvalidOperationException();
		}
#endif // !STATIC_COMPILER
	}
}
