/*
  Copyright (C) 2002, 2003, 2004, 2005, 2006 Jeroen Frijters

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
using ILGenerator = IKVM.Internal.CountingILGenerator;
#endif
using System.Diagnostics;
using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Internal
{
	[Flags]
	enum MemberFlags : short
	{
		None = 0,
		HideFromReflection = 1,
		ExplicitOverride = 2,
		LiteralField = 4,
		MirandaMethod = 8,
		AccessStub = 16,
		InternalAccess = 32  // member has "internal" access (@ikvm.lang.Internal)
	}

	class MemberWrapper
	{
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
					(IsInternal && caller.Assembly == DeclaringType.Assembly) ||
					(!IsPrivate && caller.IsInSamePackageAs(DeclaringType));
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

		internal bool IsLiteralField
		{
			get
			{
				return (flags & MemberFlags.LiteralField) != 0;
			}
			set
			{
				flags &= ~MemberFlags.LiteralField;
				flags |= value ? MemberFlags.LiteralField : MemberFlags.None;
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
		internal static readonly MethodWrapper[] EmptyArray  = new MethodWrapper[0];
		private MethodBase method;
		private string[] declaredExceptions;
		private TypeWrapper returnTypeWrapper;
		private TypeWrapper[] parameterTypeWrappers;

#if !COMPACT_FRAMEWORK
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
#endif

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
			protected override void CallvirtImpl(ILGenerator ilgen)
			{
				ResolveGhostMethod();
				ilgen.Emit(OpCodes.Call, ghostMethod);
			}
#endif

#if !STATIC_COMPILER
			[HideFromJava]
			internal override object Invoke(object obj, object[] args, bool nonVirtual)
			{
				object wrapper = Activator.CreateInstance(DeclaringType.TypeAsSignatureType);
				DeclaringType.GhostRefField.SetValue(wrapper, obj);

				ResolveGhostMethod();
				return InvokeImpl(ghostMethod, wrapper, args, nonVirtual);
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
		}

		internal void SetDeclaredExceptions(string[] exceptions)
		{
			if(exceptions == null)
			{
				exceptions = new string[0];
			}
			this.declaredExceptions = (string[])exceptions.Clone();
		}

		internal static MethodWrapper FromCookie(IntPtr cookie)
		{
			return (MethodWrapper)FromCookieImpl(cookie);
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
			Type[] temp = new Type[wrappers.Length];
			for(int i = 0; i < wrappers.Length; i++)
			{
				temp[i] = wrappers[i].TypeAsSignatureType;
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
			if(method != null
#if !COMPACT_FRAMEWORK
				&& !(method is MethodBuilder)
#endif
				)
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
		internal virtual object Invoke(object obj, object[] args, bool nonVirtual)
		{
			AssertLinked();
#if !COMPACT_FRAMEWORK
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
#endif // !COMPACT_FRAMEWORK
			return InvokeImpl(method, obj, args, nonVirtual);
		}

		private delegate object Invoker(IntPtr pFunc, object obj, object[] args);

		[HideFromJava]
		internal object InvokeImpl(MethodBase method, object obj, object[] args, bool nonVirtual)
		{
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
							throw JavaException.InvocationTargetException(IKVM.Runtime.Util.MapException(x.InnerException));
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
						InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, args);
						return invoker(method.MethodHandle.GetFunctionPointer(), proc.GetObj(), proc.GetArgs());
					}
					catch(ArgumentException x1)
					{
						throw JavaException.IllegalArgumentException(x1.Message);
					}
					catch(TargetInvocationException x)
					{
						throw JavaException.InvocationTargetException(IKVM.Runtime.Util.MapException(x.InnerException));
					}
#endif
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
				throw JavaException.InvocationTargetException(IKVM.Runtime.Util.MapException(x.InnerException));
			}
		}

#if !COMPACT_FRAMEWORK
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
				AssemblyBuilder ab = AppDomain.CurrentDomain.DefineDynamicAssembly(name, DynamicClassLoader.IsSaveDebugImage ? AssemblyBuilderAccess.RunAndSave : AssemblyBuilderAccess.Run);
				if(DynamicClassLoader.IsSaveDebugImage)
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
				MethodBuilder methodBuilder = typeBuilder.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Static, typeof(object), new Type[] { typeof(IntPtr), typeof(object), typeof(object[]) });
				AttributeHelper.HideFromJava(methodBuilder);
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

			internal InvokeArgsProcessor(MethodWrapper mw, MethodBase method, object original_obj, object[] original_args)
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
#endif // !STATIC_COMPILER

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
		protected virtual void PreEmit(ILGenerator ilgen)
		{
		}
	
		internal sealed override void EmitCall(ILGenerator ilgen)
		{
			AssertLinked();
			PreEmit(ilgen);
			CallImpl(ilgen);
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
				// TODO we need to check for a null reference
				CallImpl(ilgen);
			}
			else
			{
				CallvirtImpl(ilgen);
			}
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
				DeclaringType.EmitBox(ilgen);
			}
		}

		protected virtual void NewobjImpl(ILGenerator ilgen)
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
		internal override void EmitCall(ILGenerator ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(call), (MethodInfo)GetMethod());
		}

		internal override void EmitCallvirt(ILGenerator ilgen)
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
		protected override void CallImpl(ILGenerator ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(call), (MethodInfo)GetMethod());
		}

		protected override void CallvirtImpl(ILGenerator ilgen)
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
		protected override void CallImpl(ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Call, (ConstructorInfo)GetMethod());
		}

		protected override void NewobjImpl(ILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Newobj, (ConstructorInfo)GetMethod());
		}
#endif
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
#if !COMPACT_FRAMEWORK
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
#endif // !COMPACT_FRAMEWORK
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
					(modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (field != null && field.IsLiteral ? MemberFlags.LiteralField : MemberFlags.None))
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
					ReflectionOnConstant.IssueWarning(field);
#if WHIDBEY
					val = field.GetRawConstantValue();
#else
					try
					{
						val = field.GetValue(null);
					}
					catch(TargetInvocationException x)
					{
						throw x.InnerException;
					}
					if(val is Enum)
					{
						val = DotNetTypeWrapper.EnumValueFieldWrapper.GetEnumPrimitiveValue(val);
					}
#endif
				}
				else
				{
#if WHIDBEY
					// TODO
#else
					// In Java, instance fields can also have a ConstantValue attribute so we emulate that
					// with ConstantValueAttribute (for consumption by ikvmstub only)
					object[] attrib = field.GetCustomAttributes(typeof(ConstantValueAttribute), false);
					if(attrib.Length == 1)
					{
						val = ((ConstantValueAttribute)attrib[0]).GetConstantValue();
					}
#endif
				}
				if(val != null && !(val is string))
				{
					return JVM.Library.box(val);
				}
				return val;
			}
			return null;
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
			field = DeclaringType.TypeAsTBD.GetField(Name, bindings);
			this.IsLiteralField = field.IsLiteral;
			Debug.Assert(field != null);
		}

#if !STATIC_COMPILER
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
#endif // !STATIC_COMPILER

		internal virtual object GetValue(object obj)
		{
			AssertLinked();
			// TODO this is a broken implementation (for one thing, it needs to support redirection)
			if(field == null || field is FieldBuilder)
			{
				LookupField();
			}
#if STATIC_COMPILER
			return field.GetValue(null);
#else
			// FieldInfo.IsLiteral is expensive, so we have our own flag
			// TODO we might be able to ensure that we always use ConstantFieldWrapper for literal fields,
			// in that case the we could simply remove the check altogether.
			if(IsLiteralField)
			{
				// on a non-broken CLR GetValue on a literal will not trigger type initialization, but on Java it should
				DeclaringType.RunClassInit();
			}
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
		protected override void EmitGetImpl(ILGenerator ilgen)
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

		protected override void EmitSetImpl(ILGenerator ilgen)
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

		protected override void EmitSetImpl(ILGenerator ilgen)
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

		// NOTE fi may be null!
		internal GetterFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers, MethodInfo getter)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(!IsVolatile);

			this.getter = getter;
		}

		internal void SetGetter(MethodInfo getter)
		{
			this.getter = getter;
		}

#if !COMPACT_FRAMEWORK
		protected override void EmitGetImpl(ILGenerator ilgen)
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

		protected override void EmitSetImpl(ILGenerator ilgen)
		{
			FieldInfo fi = GetField();
			if(!IsStatic && DeclaringType.IsNonPrimitiveValueType)
			{
				LocalBuilder temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, temp);
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, temp);
			}
			ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
		}
#endif
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
		protected override void EmitGetImpl(ILGenerator ilgen)
		{
			// Reading a field should trigger the cctor, but since we're inlining the value
			// we have to trigger it explicitly
			if(DeclaringType.IsInterface)
			{
				if(DeclaringType.HasStaticInitializer)
				{
					// NOTE since Everett doesn't support adding static methods to interfaces,
					// EmitRunClassConstructor doesn't work for interface, so we do it manually.
					// TODO once we're on Whidbey, this won't be necessary anymore.
					ilgen.Emit(OpCodes.Ldtoken, DeclaringType.TypeAsBaseType);
					ilgen.Emit(OpCodes.Call, typeof(System.Runtime.CompilerServices.RuntimeHelpers).GetMethod("RunClassConstructor"));
				}
			}
			else
			{
				DeclaringType.EmitRunClassConstructor(ilgen);
			}

			// NOTE even though you're not supposed to access a constant static final (the compiler is supposed
			// to inline them), we have to support it (because it does happen, e.g. if the field becomes final
			// after the referencing class was compiled, or when we're accessing an unsigned primitive .NET field)
			object v = GetValue(null);
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

		protected override void EmitSetImpl(ILGenerator ilgen)
		{
			// when constant static final fields are updated, the JIT normally doesn't see that (because the
			// constant value is inlined), so we emulate that behavior by emitting a Pop
			ilgen.Emit(OpCodes.Pop);
		}
#endif

		internal override object GetValue(object obj)
		{
			if(constant == null)
			{
				constant = GetField().GetValue(null);
			}
			return constant;
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

		internal void DoLink(TypeBuilder typeBuilder)
		{
			basefield.Link();
			if(basefield.IsLiteralField)
			{
				FieldAttributes attribs = basefield.IsPublic ? FieldAttributes.Public : FieldAttributes.FamORAssem;
				attribs |= FieldAttributes.Static | FieldAttributes.Literal;
				FieldBuilder fb = typeBuilder.DefineField(Name, basefield.FieldTypeWrapper.TypeAsSignatureType, attribs);
				AttributeHelper.HideFromReflection(fb);
				fb.SetConstant(basefield.GetValue(null));
			}
			else
			{
				PropertyBuilder pb = typeBuilder.DefineProperty(Name, PropertyAttributes.None, basefield.FieldTypeWrapper.TypeAsSignatureType, Type.EmptyTypes);
				AttributeHelper.HideFromReflection(pb);
				MethodAttributes attribs = basefield.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
				attribs |= MethodAttributes.HideBySig;
				if(basefield.IsStatic)
				{
					attribs |= MethodAttributes.Static;
				}
				getter = typeBuilder.DefineMethod("get_" + Name, attribs, basefield.FieldTypeWrapper.TypeAsSignatureType, Type.EmptyTypes);
				AttributeHelper.HideFromJava(getter);
				pb.SetGetMethod(getter);
				ILGenerator ilgen = getter.GetILGenerator();
				if(!basefield.IsStatic)
				{
					ilgen.Emit(OpCodes.Ldarg_0);
				}
				basefield.EmitGet(ilgen);
				ilgen.Emit(OpCodes.Ret);
				if(!basefield.IsFinal)
				{
					setter = typeBuilder.DefineMethod("set_" + Name, attribs, null, new Type[] { basefield.FieldTypeWrapper.TypeAsSignatureType });
					AttributeHelper.HideFromJava(setter);
					pb.SetSetMethod(setter);
					ilgen = setter.GetILGenerator();
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

		protected override void EmitGetImpl(CountingILGenerator ilgen)
		{
			if(basefield.IsLiteralField)
			{
				basefield.EmitGet(ilgen);
			}
			else
			{
				ilgen.Emit(OpCodes.Call, getter);
			}
		}

		protected override void EmitSetImpl(CountingILGenerator ilgen)
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
		protected override void EmitGetImpl(CountingILGenerator ilgen)
		{
			ilgen.Emit(OpCodes.Call, getter);
		}

		protected override void EmitSetImpl(CountingILGenerator ilgen)
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
