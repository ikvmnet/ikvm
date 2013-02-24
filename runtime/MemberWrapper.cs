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
using IKVM.Attributes;
using System.Threading;
using System.Runtime.InteropServices;

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
		NonPublicTypeInSignature = 512,	// this flag is only available after linking and is not set for access stubs
		DelegateInvokeWithByRefParameter = 1024,
		Type2FinalField = 2048,
	}

	abstract class MemberWrapper
	{
		private HandleWrapper handle;
		private readonly TypeWrapper declaringType;
		private readonly Modifiers modifiers;
		private MemberFlags flags;
		private readonly string name;
		private readonly string sig;

		private sealed class HandleWrapper
		{
			internal readonly IntPtr Value;

			[System.Security.SecurityCritical]
			internal HandleWrapper(MemberWrapper obj)
			{
				Value = (IntPtr)GCHandle.Alloc(obj, GCHandleType.WeakTrackResurrection);
			}

#if CLASSGC
			[System.Security.SecuritySafeCritical]
			~HandleWrapper()
			{
				if (!Environment.HasShutdownStarted)
				{
					GCHandle h = (GCHandle)Value;
					if (h.Target == null)
					{
						h.Free();
					}
					else
					{
						GC.ReRegisterForFinalize(this);
					}
				}
			}
#endif
		}

		protected MemberWrapper(TypeWrapper declaringType, string name, string sig, Modifiers modifiers, MemberFlags flags)
		{
			Debug.Assert(declaringType != null);
			this.declaringType = declaringType;
			this.name = String.Intern(name);
			this.sig = String.Intern(sig);
			this.modifiers = modifiers;
			this.flags = flags;
		}

		internal IntPtr Cookie
		{
			[System.Security.SecurityCritical]
			get
			{
				lock(this)
				{
					if(handle == null)
					{
						handle = new HandleWrapper(this);
					}
				}
				return handle.Value;
			}
		}

		[System.Security.SecurityCritical]
		internal static MemberWrapper FromCookieImpl(IntPtr cookie)
		{
			return (MemberWrapper)GCHandle.FromIntPtr(cookie).Target;
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
				return (
					caller == DeclaringType ||
					IsPublicOrProtectedMemberAccessible(caller, instance) ||
					(IsInternal && DeclaringType.InternalsVisibleTo(caller)) ||
					(!IsPrivate && DeclaringType.IsPackageAccessibleFrom(caller)))
					// The JVM supports accessing members that have non-public types in their signature from another package,
					// but the CLI doesn't. It would be nice if we worked around that by emitting extra accessors, but for now
					// we'll simply disallow such access across assemblies (unless the appropriate InternalsVisibleToAttribute exists).
					&& (!(HasNonPublicTypeInSignature || IsType2FinalField) || InPracticeInternalsVisibleTo(caller));
			}
			return false;
		}

		private bool IsPublicOrProtectedMemberAccessible(TypeWrapper caller, TypeWrapper instance)
		{
			if (IsPublic || (IsProtected && caller.IsSubTypeOf(DeclaringType) && (IsStatic || instance.IsSubTypeOf(caller))))
			{
				return DeclaringType.IsPublic || InPracticeInternalsVisibleTo(caller);
			}
			return false;
		}

		private bool InPracticeInternalsVisibleTo(TypeWrapper caller)
		{
#if !STATIC_COMPILER
			if (DeclaringType.TypeAsTBD.Assembly.Equals(caller.TypeAsTBD.Assembly))
			{
				// both the caller and the declaring type are in the same assembly
				// so we know that the internals are visible
				// (this handles the case where we're running in dynamic mode)
				return true;
			}
#endif
#if CLASSGC
			if (DeclaringType is DynamicTypeWrapper)
			{
				// if we are dynamic, we can just become friends with the caller
				DeclaringType.GetClassLoader().GetTypeWrapperFactory().AddInternalsVisibleTo(caller.TypeAsTBD.Assembly);
				return true;
			}
#endif
			return DeclaringType.InternalsVisibleTo(caller);
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

		protected void SetNonPublicTypeInSignatureFlag()
		{
			flags |= MemberFlags.NonPublicTypeInSignature;
		}

		internal bool HasNonPublicTypeInSignature
		{
			get { return (flags & MemberFlags.NonPublicTypeInSignature) != 0; }
		}

		protected void SetType2FinalField()
		{
			flags |= MemberFlags.Type2FinalField;
		}
	
		internal bool IsType2FinalField
		{
			get { return (flags & MemberFlags.Type2FinalField) != 0; }
		}

		internal bool HasCallerID
		{
			get
			{
				return (flags & MemberFlags.CallerID) != 0;
			}
		}

		internal bool IsDelegateInvokeWithByRefParameter
		{
			get { return (flags & MemberFlags.DelegateInvokeWithByRefParameter) != 0; }
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

	interface ICustomInvoke
	{
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		object Invoke(object obj, object[] args);
#endif
	}

	abstract class MethodWrapper : MemberWrapper
	{
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		private static Dictionary<MethodWrapper, sun.reflect.MethodAccessor> invokenonvirtualCache;
		private volatile object reflectionMethod;
#endif
		internal static readonly MethodWrapper[] EmptyArray  = new MethodWrapper[0];
		private MethodBase method;
		private string[] declaredExceptions;
		private TypeWrapper returnTypeWrapper;
		private TypeWrapper[] parameterTypeWrappers;

#if !STUB_GENERATOR
		internal virtual void EmitCall(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal virtual void EmitCallvirt(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal virtual void EmitCallvirtReflect(CodeEmitter ilgen)
		{
			EmitCallvirt(ilgen);
		}

		internal virtual void EmitNewobj(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal virtual bool EmitIntrinsic(EmitIntrinsicContext context)
		{
			return Intrinsics.Emit(context);
		}
#endif // STUB_GENERATOR

		internal virtual bool IsDynamicOnly
		{
			get
			{
				return false;
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
			UpdateNonPublicTypeInSignatureFlag();
		}

		private void UpdateNonPublicTypeInSignatureFlag()
		{
			if ((IsPublic || IsProtected) && (returnTypeWrapper != null && parameterTypeWrappers != null) && !(this is AccessStubMethodWrapper) && !(this is AccessStubConstructorMethodWrapper))
			{
				if (!returnTypeWrapper.IsPublic && !returnTypeWrapper.IsUnloadable)
				{
					SetNonPublicTypeInSignatureFlag();
				}
				else
				{
					foreach (TypeWrapper tw in parameterTypeWrappers)
					{
						if (!tw.IsPublic && !tw.IsUnloadable)
						{
							SetNonPublicTypeInSignatureFlag();
							break;
						}
					}
				}
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

		internal string[] GetDeclaredExceptions()
		{
			return declaredExceptions;
		}

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal object ToMethodOrConstructor(bool copy)
		{
#if FIRST_PASS
			return null;
#else
			object method = reflectionMethod;
			if (method == null)
			{
				Link();
				ClassLoaderWrapper loader = this.DeclaringType.GetClassLoader();
				TypeWrapper[] argTypes = GetParameters();
				java.lang.Class[] parameterTypes = new java.lang.Class[argTypes.Length];
				for (int i = 0; i < argTypes.Length; i++)
				{
					parameterTypes[i] = argTypes[i].EnsureLoadable(loader).ClassObject;
				}
				java.lang.Class[] checkedExceptions = GetExceptions();
				if (this.IsConstructor)
				{
					method = new java.lang.reflect.Constructor(
						this.DeclaringType.ClassObject,
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
					method = new java.lang.reflect.Method(
						this.DeclaringType.ClassObject,
						this.Name,
						parameterTypes,
						this.ReturnType.EnsureLoadable(loader).ClassObject,
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
					return ctor.copy();
				}
				return ((java.lang.reflect.Method)method).copy();
			}
			return method;
#endif
		}

#if !FIRST_PASS
		private java.lang.Class[] GetExceptions()
		{
			string[] classes = declaredExceptions;
			Type[] types = Type.EmptyTypes;
			if (classes == null)
			{
				// NOTE if method is a MethodBuilder, GetCustomAttributes doesn't work (and if
				// the method had any declared exceptions, the declaredExceptions field would have
				// been set)
				if (method != null && !(method is MethodBuilder))
				{
					ThrowsAttribute attr = AttributeHelper.GetThrows(method);
					if (attr != null)
					{
						classes = attr.classes;
						types = attr.types;
					}
				}
			}
			if (classes != null)
			{
				java.lang.Class[] array = new java.lang.Class[classes.Length];
				for (int i = 0; i < classes.Length; i++)
				{
					array[i] = this.DeclaringType.GetClassLoader().LoadClassByDottedName(classes[i]).ClassObject;
				}
				return array;
			}
			else
			{
				java.lang.Class[] array = new java.lang.Class[types.Length];
				for (int i = 0; i < types.Length; i++)
				{
					array[i] = types[i];
				}
				return array;
			}
		}
#endif // !FIRST_PASS

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
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		[System.Security.SecurityCritical]
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
			TypeWrapper ret = loader.RetTypeWrapperFromSigNoThrow(Signature);
			TypeWrapper[] parameters = loader.ArgTypeWrapperListFromSigNoThrow(Signature);
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					if(parameterTypeWrappers == null)
					{
						Debug.Assert(returnTypeWrapper == null || returnTypeWrapper == PrimitiveTypeWrapper.VOID);
						returnTypeWrapper = ret;
						parameterTypeWrappers = parameters;
						UpdateNonPublicTypeInSignatureFlag();
						if(method == null)
						{
							try
							{
								DoLinkMethod();
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
		}

		protected virtual void DoLinkMethod()
		{
			method = this.DeclaringType.LinkMethod(this);
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

#if !STUB_GENERATOR
		internal DefineMethodHelper GetDefineMethodHelper()
		{
			return new DefineMethodHelper(this);
		}
#endif

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

#if !STATIC_COMPILER && !STUB_GENERATOR
		[HideFromJava]
		internal object InvokeJNI(object obj, object[] args, bool nonVirtual, object callerID)
		{
#if FIRST_PASS
			return null;
#else
			if (IsConstructor)
			{
				java.lang.reflect.Constructor cons = (java.lang.reflect.Constructor)ToMethodOrConstructor(false);
				if (obj == null)
				{
					sun.reflect.ConstructorAccessor acc = cons.getConstructorAccessor();
					if (acc == null)
					{
						acc = (sun.reflect.ConstructorAccessor)IKVM.NativeCode.sun.reflect.ReflectionFactory.newConstructorAccessor0(null, cons);
						cons.setConstructorAccessor(acc);
					}
					return acc.newInstance(args);
				}
				else if (!ReflectUtil.IsConstructor(method))
				{
					Debug.Assert(method.IsStatic);
					// we're dealing with a constructor on a remapped type, if obj is supplied, it means
					// that we should call the constructor on an already existing instance, but that isn't
					// possible with remapped types
					// the type of this exception is a bit random (note that this can only happen through JNI reflection)
					throw new java.lang.IncompatibleClassChangeError(string.Format("Remapped type {0} doesn't support constructor invocation on an existing instance", DeclaringType.Name));
				}
				else if (!method.DeclaringType.IsInstanceOfType(obj))
				{
					// we're trying to initialize an existing instance of a remapped type
					throw new NotSupportedException("Unable to partially construct object of type " + obj.GetType().FullName + " to type " + method.DeclaringType.FullName);
				}
				else
				{
					try
					{
						ResolveMethod();
						InvokeArgsProcessor proc = new InvokeArgsProcessor(this, method, obj, UnboxArgs(args), (ikvm.@internal.CallerID)callerID);
						object o = method.Invoke(proc.GetObj(), proc.GetArgs());
						TypeWrapper retType = this.ReturnType;
						if (!retType.IsUnloadable && retType.IsGhost)
						{
							o = retType.GhostRefField.GetValue(o);
						}
						return o;
					}
					catch (ArgumentException x1)
					{
						throw new java.lang.IllegalArgumentException(x1.Message);
					}
					catch (TargetInvocationException x)
					{
						throw new java.lang.reflect.InvocationTargetException(ikvm.runtime.Util.mapException(x.InnerException));
					}
				}
			}
			else if (nonVirtual
				&& !this.IsStatic
				&& !this.IsPrivate
				&& !this.IsAbstract
				&& !this.IsFinal
				&& !this.DeclaringType.IsFinal)
			{
				if (this.DeclaringType.IsRemapped && !this.DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
				{
					ResolveMethod();
					return InvokeNonvirtualRemapped(obj, UnboxArgs(args));
				}
				else
				{
					if (invokenonvirtualCache == null)
					{
						Interlocked.CompareExchange(ref invokenonvirtualCache, new Dictionary<MethodWrapper, sun.reflect.MethodAccessor>(), null);
					}
					sun.reflect.MethodAccessor acc;
					lock (invokenonvirtualCache)
					{
						if (!invokenonvirtualCache.TryGetValue(this, out acc))
						{
							acc = new IKVM.NativeCode.sun.reflect.ReflectionFactory.FastMethodAccessorImpl((java.lang.reflect.Method)ToMethodOrConstructor(false), true);
							invokenonvirtualCache.Add(this, acc);
						}
					}
					object val = acc.invoke(obj, args, (ikvm.@internal.CallerID)callerID);
					if (this.ReturnType.IsPrimitive && this.ReturnType != PrimitiveTypeWrapper.VOID)
					{
						val = JVM.Unbox(val);
					}
					return val;
				}
			}
			else
			{
				java.lang.reflect.Method method = (java.lang.reflect.Method)ToMethodOrConstructor(false);
				sun.reflect.MethodAccessor acc = method.getMethodAccessor();
				if (acc == null)
				{
					acc = (sun.reflect.MethodAccessor)IKVM.NativeCode.sun.reflect.ReflectionFactory.newMethodAccessor(null, method);
					method.setMethodAccessor(acc);
				}
				object val = acc.invoke(obj, args, (ikvm.@internal.CallerID)callerID);
				if (this.ReturnType.IsPrimitive && this.ReturnType != PrimitiveTypeWrapper.VOID)
				{
					val = JVM.Unbox(val);
				}
				return val;
			}
#endif
		}

		private object[] UnboxArgs(object[] args)
		{
			TypeWrapper[] paramTypes = GetParameters();
			for (int i = 0; i < paramTypes.Length; i++)
			{
				if (paramTypes[i].IsPrimitive)
				{
					args[i] = JVM.Unbox(args[i]);
				}
			}
			return args;
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		internal void ResolveMethod()
		{
			// if we've still got the builder object, we need to replace it with the real thing before we can call it
			if(method is MethodBuilder)
			{
				method = method.Module.ResolveMethod(((MethodBuilder)method).GetToken().Token);
			}
		}

		[HideFromJava]
		protected virtual object InvokeNonvirtualRemapped(object obj, object[] args)
		{
			throw new InvalidOperationException();
		}

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
#endif // !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR

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

		internal virtual bool IsOptionalAttributeAnnotationValue
		{
			get { return false; }
		}

		internal bool IsConstructor
		{
			get { return (object)Name == (object)StringConstants.INIT; }
		}
	}

	// placeholder for <clinit> method that exist in ClassFile but not in TypeWrapper
	// (because it is optimized away)
	sealed class DummyMethodWrapper : MethodWrapper
	{
		internal DummyMethodWrapper(TypeWrapper tw)
			: base(tw, StringConstants.CLINIT, StringConstants.SIG_VOID, null, PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Static, MemberFlags.None)
		{
		}

		protected override void DoLinkMethod()
		{
			// we're pre-linked (because we pass the signature types to the base constructor)
			throw new InvalidOperationException();
		}
	}

	abstract class SmartMethodWrapper : MethodWrapper
	{
		internal SmartMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
		}

#if !STUB_GENERATOR
		internal sealed override void EmitCall(CodeEmitter ilgen)
		{
			AssertLinked();
			CallImpl(ilgen);
		}

		protected virtual void CallImpl(CodeEmitter ilgen)
		{
			throw new InvalidOperationException();
		}

		internal sealed override void EmitCallvirt(CodeEmitter ilgen)
		{
			AssertLinked();
			if(DeclaringType.IsNonPrimitiveValueType)
			{
				// callvirt isn't allowed on a value type
				// (we don't need to check for a null reference, because we're always dealing with an unboxed value)
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

		internal sealed override void EmitNewobj(CodeEmitter ilgen)
		{
			AssertLinked();
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
#endif // STUB_GENERATOR
	}

	enum SimpleOpCode : byte
	{
		Call,
		Callvirt,
		Newobj
	}

	sealed class SimpleCallMethodWrapper : MethodWrapper
	{
		private readonly SimpleOpCode call;
		private readonly SimpleOpCode callvirt;

		internal SimpleCallMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodInfo method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags, SimpleOpCode call, SimpleOpCode callvirt)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
			this.call = call;
			this.callvirt = callvirt;
		}

#if !STUB_GENERATOR
		internal override void EmitCall(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(call), GetMethod());
		}

		internal override void EmitCallvirt(CodeEmitter ilgen)
		{
			ilgen.Emit(SimpleOpCodeToOpCode(callvirt), GetMethod());
		}
#endif // !STUB_GENERATOR
	}

	sealed class TypicalMethodWrapper : SmartMethodWrapper
	{
		internal TypicalMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
		}

#if !STUB_GENERATOR
		protected override void CallImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, GetMethod());
		}

		protected override void CallvirtImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Callvirt, GetMethod());
		}

		protected override void NewobjImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Newobj, GetMethod());
		}
#endif // !STUB_GENERATOR
	}

	sealed class GhostMethodWrapper : SmartMethodWrapper
	{
		private MethodInfo ghostMethod;

		internal GhostMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, MethodInfo ghostMethod, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
		{
			// make sure we weren't handed the ghostMethod in the wrapper value type
			Debug.Assert(method == null || method.DeclaringType.IsInterface);
			this.ghostMethod = ghostMethod;
		}

		private void ResolveGhostMethod()
		{
			if (ghostMethod == null)
			{
				ghostMethod = DeclaringType.TypeAsSignatureType.GetMethod(this.Name, this.GetParametersForDefineMethod());
				if (ghostMethod == null)
				{
					throw new InvalidOperationException("Unable to resolve ghost method");
				}
			}
		}

#if !STUB_GENERATOR
		protected override void CallvirtImpl(CodeEmitter ilgen)
		{
			ResolveGhostMethod();
			ilgen.Emit(OpCodes.Call, ghostMethod);
		}
#endif
	}

	sealed class AccessStubMethodWrapper : SmartMethodWrapper
	{
		private readonly MethodInfo stubVirtual;
		private readonly MethodInfo stubNonVirtual;

		internal AccessStubMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodInfo core, MethodInfo stubVirtual, MethodInfo stubNonVirtual, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, name, sig, core, returnType, parameterTypes, modifiers, flags)
		{
			this.stubVirtual = stubVirtual;
			this.stubNonVirtual = stubNonVirtual;
		}

#if !STUB_GENERATOR
		protected override void CallImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, stubNonVirtual);
		}

		protected override void CallvirtImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Callvirt, stubVirtual);
		}
#endif // !STUB_GENERATOR
	}

	sealed class AccessStubConstructorMethodWrapper : SmartMethodWrapper
	{
		private readonly ConstructorInfo stub;

		internal AccessStubConstructorMethodWrapper(TypeWrapper declaringType, string sig, ConstructorInfo core, ConstructorInfo stub, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags)
			: base(declaringType, StringConstants.INIT, sig, core, PrimitiveTypeWrapper.VOID, parameterTypes, modifiers, flags)
		{
			this.stub = stub;
		}

#if !STUB_GENERATOR
		protected override void CallImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, stub);
		}

		protected override void NewobjImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Newobj, stub);
		}
#endif // !STUB_GENERATOR
	}

	abstract class FieldWrapper : MemberWrapper
	{
#if !STATIC_COMPILER && !FIRST_PASS && !STUB_GENERATOR
		private volatile java.lang.reflect.Field reflectionField;
		private sun.reflect.FieldAccessor jniAccessor;
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
			UpdateNonPublicTypeInSignatureFlag();
#if STATIC_COMPILER
			if (IsFinal
				&& DeclaringType.IsPublic
				&& !DeclaringType.IsInterface
				&& (IsPublic || (IsProtected && !DeclaringType.IsFinal))
				&& !DeclaringType.GetClassLoader().StrictFinalFieldSemantics
				&& DeclaringType is DynamicTypeWrapper
				&& !(this is ConstantFieldWrapper)
				&& !(this is DynamicPropertyFieldWrapper))
			{
				SetType2FinalField();
			}
#endif
		}

		internal FieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, ExModifiers modifiers, FieldInfo field)
			: this(declaringType, fieldType, name, sig, modifiers.Modifiers, field,
					(modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None))
		{
		}

		private void UpdateNonPublicTypeInSignatureFlag()
		{
			if ((IsPublic || IsProtected) && fieldType != null && !IsAccessStub)
			{
				if (!fieldType.IsPublic && !fieldType.IsUnloadable)
				{
					SetNonPublicTypeInSignatureFlag();
				}
			}
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

#if !STATIC_COMPILER && !STUB_GENERATOR
		// NOTE used (thru IKVM.Runtime.Util.GetFieldConstantValue) by ikvmstub to find out if the
		// field is a constant (and if it is, to get its value)
		internal object GetConstant()
		{
			AssertLinked();
			// only primitives and string can be literals in Java (because the other "primitives" (like uint),
			// are treated as NonPrimitiveValueTypes)
			if(field != null && field.IsLiteral && (fieldType.IsPrimitive || fieldType == CoreClasses.java.lang.String.Wrapper))
			{
				object val = field.GetRawConstantValue();
				if(field.FieldType.IsEnum)
				{
					val = EnumHelper.GetPrimitiveValue(EnumHelper.GetUnderlyingType(field.FieldType), val);
				}
				if(fieldType.IsPrimitive)
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
			int slot = f._slot();
			if (slot == -1)
			{
				// it's a Field created by Unsafe.objectFieldOffset(Class,String) so we must resolve based on the name
				foreach (FieldWrapper fw in TypeWrapper.FromClass(f.getDeclaringClass()).GetFields())
				{
					if (fw.Name == f.getName())
					{
						return fw;
					}
				}
			}
			return TypeWrapper.FromClass(f.getDeclaringClass()).GetFields()[slot];
#endif
		}

		internal object ToField(bool copy)
		{
			return ToField(copy, null);
		}

		internal object ToField(bool copy, int? fieldIndex)
		{
#if FIRST_PASS
			return null;
#else
			java.lang.reflect.Field field = reflectionField;
			if (field == null)
			{
				const Modifiers ReflectionFieldModifiersMask = Modifiers.Public | Modifiers.Private | Modifiers.Protected | Modifiers.Static
					| Modifiers.Final | Modifiers.Volatile | Modifiers.Transient | Modifiers.Synthetic | Modifiers.Enum;
				Link();
				field = new java.lang.reflect.Field(
					this.DeclaringType.ClassObject,
					this.Name,
					this.FieldTypeWrapper.EnsureLoadable(this.DeclaringType.GetClassLoader()).ClassObject,
					(int)(this.Modifiers & ReflectionFieldModifiersMask) | (this.IsInternal ? 0x40000000 : 0),
					fieldIndex ?? Array.IndexOf(this.DeclaringType.GetFields(), this),
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
				field = field.copy();
			}
			return field;
#endif // FIRST_PASS
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR

		[System.Security.SecurityCritical]
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

#if !STUB_GENERATOR
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
#endif // !STUB_GENERATOR


#if STATIC_COMPILER
		internal bool IsLinked
		{
			get { return fieldType != null; }
		}
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
			TypeWrapper fld = this.DeclaringType.GetClassLoader().FieldTypeWrapperFromSigNoThrow(Signature);
			lock(this)
			{
				try
				{
					// critical code in the finally block to avoid Thread.Abort interrupting the thread
				}
				finally
				{
					if(fieldType == null)
					{
						fieldType = fld;
						UpdateNonPublicTypeInSignatureFlag();
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

#if !STATIC_COMPILER && !STUB_GENERATOR
		internal virtual void ResolveField()
		{
			FieldBuilder fb = field as FieldBuilder;
			if(fb != null)
			{
				field = field.Module.ResolveField(fb.GetToken().Token);
			}
		}

		internal object GetFieldAccessorJNI()
		{
#if FIRST_PASS
			return null;
#else
			if (jniAccessor == null)
			{
				Interlocked.CompareExchange(ref jniAccessor, IKVM.NativeCode.sun.reflect.ReflectionFactory.NewFieldAccessorJNI(this), null);
			}
			return jniAccessor;
#endif
		}
#endif // !STATIC_COMPILER && !STUB_GENERATOR
	}

	sealed class SimpleFieldWrapper : FieldWrapper
	{
		internal SimpleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(!(fieldType == PrimitiveTypeWrapper.DOUBLE || fieldType == PrimitiveTypeWrapper.LONG) || !IsVolatile);
		}

#if !STUB_GENERATOR
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
				CodeEmitterLocal temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
				ilgen.Emit(OpCodes.Stloc, temp);
				ilgen.Emit(OpCodes.Unbox, DeclaringType.TypeAsTBD);
				ilgen.Emit(OpCodes.Ldloc, temp);
			}
			if(IsVolatile)
			{
				ilgen.Emit(OpCodes.Volatile);
			}
			ilgen.Emit(IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fi);
			if(IsVolatile)
			{
				ilgen.EmitMemoryBarrier();
			}
		}
#endif // !STUB_GENERATOR
	}

	sealed class VolatileLongDoubleFieldWrapper : FieldWrapper
	{
		internal VolatileLongDoubleFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, FieldInfo fi, string name, string sig, ExModifiers modifiers)
			: base(declaringType, fieldType, name, sig, modifiers, fi)
		{
			Debug.Assert(IsVolatile);
			Debug.Assert(sig == "J" || sig == "D");
		}

#if !STUB_GENERATOR
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
			CodeEmitterLocal temp = ilgen.DeclareLocal(FieldTypeWrapper.TypeAsSignatureType);
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
#endif // !STUB_GENERATOR
	}

#if !STUB_GENERATOR
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
			ilgen.EmitBrtrue(label);
			ilgen.EmitThrow("java.lang.NoSuchMethodError");
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
			ilgen.EmitBrtrue(label);
			ilgen.EmitThrow("java.lang.NoSuchMethodError");
			ilgen.MarkLabel(label);
			ilgen.Emit(OpCodes.Pop);
			if (!isStatic)
			{
				ilgen.Emit(OpCodes.Pop);
			}
		}
	}
#endif // !STUB_GENERATOR

	// this class represents a .NET property defined in Java with the ikvm.lang.Property annotation
	sealed class CompiledPropertyFieldWrapper : FieldWrapper
	{
		private readonly PropertyInfo property;

		internal CompiledPropertyFieldWrapper(TypeWrapper declaringType, PropertyInfo property, ExModifiers modifiers)
			: base(declaringType, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType), property.Name, ClassLoaderWrapper.GetWrapperFromType(property.PropertyType).SigName, modifiers, null)
		{
			this.property = property;
		}

#if !STUB_GENERATOR
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
#endif // !STUB_GENERATOR

		internal PropertyInfo GetProperty()
		{
			return property;
		}
	}

	sealed class ConstantFieldWrapper : FieldWrapper
	{
		// NOTE this field wrapper can resprent a .NET enum, but in that case "constant" contains the raw constant value (i.e. the boxed underlying primitive value, not a boxed enum)
		private object constant;

		internal ConstantFieldWrapper(TypeWrapper declaringType, TypeWrapper fieldType, string name, string sig, Modifiers modifiers, FieldInfo field, object constant, MemberFlags flags)
			: base(declaringType, fieldType, name, sig, modifiers, field, flags)
		{
			Debug.Assert(IsStatic);
            Debug.Assert(constant == null || constant.GetType().IsPrimitive || constant is string);
			this.constant = constant;
		}

#if !STUB_GENERATOR
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
				ilgen.EmitLdc_I4(((IConvertible)constant).ToInt32(null));
			}
			else if(constant is string)
			{
				ilgen.Emit(OpCodes.Ldstr, (string)constant);
			}
			else if(constant is float)
			{
				ilgen.EmitLdc_R4((float)constant);
			}
			else if(constant is double)
			{
				ilgen.EmitLdc_R8((double)constant);
			}
			else if(constant is long)
			{
				ilgen.EmitLdc_I8((long)constant);
			}
			else if(constant is uint)
			{
				ilgen.EmitLdc_I4(unchecked((int)((IConvertible)constant).ToUInt32(null)));
			}
			else if(constant is ulong)
			{
				ilgen.EmitLdc_I8(unchecked((long)(ulong)constant));
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
#endif // !STUB_GENERATOR

		internal object GetConstantValue()
		{
			if(constant == null)
			{
				FieldInfo field = GetField();
				constant = field.GetRawConstantValue();
			}
			return constant;
		}
	}

	sealed class CompiledAccessStubFieldWrapper : FieldWrapper
	{
		private readonly MethodInfo getter;
		private readonly MethodInfo setter;

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

		// constructor for type 1 access stubs
		internal CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, TypeWrapper propertyType)
			: this(wrapper, property, null, propertyType, GetModifiers(property), MemberFlags.HideFromReflection | MemberFlags.AccessStub)
		{
		}

		// constructor for type 2 access stubs
		internal CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, FieldInfo field, TypeWrapper propertyType)
			: this(wrapper, property, field, propertyType, AttributeHelper.GetModifiersAttribute(property).Modifiers, MemberFlags.AccessStub)
		{
		}

		private CompiledAccessStubFieldWrapper(TypeWrapper wrapper, PropertyInfo property, FieldInfo field, TypeWrapper propertyType, Modifiers modifiers, MemberFlags flags)
			: base(wrapper, propertyType, property.Name, propertyType.SigName, modifiers, field, flags)
		{
			this.getter = property.GetGetMethod(true);
			this.setter = property.GetSetMethod(true);
		}

#if !STUB_GENERATOR
		protected override void EmitGetImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, getter);
		}

		protected override void EmitSetImpl(CodeEmitter ilgen)
		{
			ilgen.Emit(OpCodes.Call, setter);
		}
#endif // !STUB_GENERATOR
	}
}
