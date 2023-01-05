/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System.Diagnostics;

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{

    abstract class MethodWrapper : MemberWrapper
    {

#if !IMPORTER && !FIRST_PASS && !EXPORTER
        volatile java.lang.reflect.Executable reflectionMethod;
#endif
        internal static readonly MethodWrapper[] EmptyArray = new MethodWrapper[0];
        MethodBase method;
        string[] declaredExceptions;
        TypeWrapper returnTypeWrapper;
        TypeWrapper[] parameterTypeWrappers;

#if EMITTERS

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

#endif // EMITTERS

        internal virtual bool IsDynamicOnly => false;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="method"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        internal MethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, Modifiers modifiers, MemberFlags flags) :
            base(declaringType, name, sig, modifiers, flags)
        {
            Profiler.Count("MethodWrapper");

            this.method = method;
            Debug.Assert(((returnType == null) == (parameterTypes == null)) || (returnType == PrimitiveTypeWrapper.VOID));
            this.returnTypeWrapper = returnType;
            this.parameterTypeWrappers = parameterTypes;
            if (Intrinsics.IsIntrinsic(this))
                SetIntrinsicFlag();

            UpdateNonPublicTypeInSignatureFlag();
        }

        void UpdateNonPublicTypeInSignatureFlag()
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
            if (exceptions == null)
            {
                exceptions = new string[0];
            }
            this.declaredExceptions = (string[])exceptions.Clone();
        }

        internal string[] GetDeclaredExceptions()
        {
            return declaredExceptions;
        }

#if !IMPORTER && !EXPORTER
        internal java.lang.reflect.Executable ToMethodOrConstructor(bool copy)
        {
#if FIRST_PASS
            return null;
#else
            java.lang.reflect.Executable method = reflectionMethod;
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

        internal static MethodWrapper FromExecutable(java.lang.reflect.Executable executable)
        {
#if FIRST_PASS
            return null;
#else
            return TypeWrapper.FromClass(executable.getDeclaringClass()).GetMethods()[executable._slot()];
#endif
        }
#endif // !IMPORTER && !EXPORTER

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
            Link(LoadMode.Link);
        }

        internal void Link(LoadMode mode)
        {
            lock (this)
            {
                if (parameterTypeWrappers != null)
                {
                    return;
                }
            }
            ClassLoaderWrapper loader = this.DeclaringType.GetClassLoader();
            TypeWrapper ret = loader.RetTypeWrapperFromSig(Signature, mode);
            TypeWrapper[] parameters = loader.ArgTypeWrapperListFromSig(Signature, mode);
            lock (this)
            {
                try
                {
                    // critical code in the finally block to avoid Thread.Abort interrupting the thread
                }
                finally
                {
                    if (parameterTypeWrappers == null)
                    {
                        Debug.Assert(returnTypeWrapper == null || returnTypeWrapper == PrimitiveTypeWrapper.VOID);
                        returnTypeWrapper = ret;
                        parameterTypeWrappers = parameters;
                        UpdateNonPublicTypeInSignatureFlag();
                        if (method == null)
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
            if (!(parameterTypeWrappers != null && returnTypeWrapper != null))
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

#if !EXPORTER
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
            if (HasCallerID)
            {
                len++;
            }
            Type[] temp = new Type[len];
            for (int i = 0; i < wrappers.Length; i++)
            {
                temp[i] = wrappers[i].TypeAsSignatureType;
            }
            if (HasCallerID)
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

        internal bool IsAbstract
        {
            get
            {
                return (Modifiers & Modifiers.Abstract) != 0;
            }
        }

        internal bool RequiresNonVirtualDispatcher
        {
            get
            {
                return !IsConstructor
                    && !IsStatic
                    && !IsPrivate
                    && !IsAbstract
                    && !IsFinal
                    && !DeclaringType.IsFinal;
            }
        }

#if !IMPORTER && !EXPORTER

        internal Type GetDelegateType()
        {
            var paramTypes = GetParameters();
            if (paramTypes.Length > MethodHandleUtil.MaxArity)
            {
                var type = DeclaringType.TypeAsBaseType.Assembly.GetType(ReturnType == PrimitiveTypeWrapper.VOID ? "__<>NVIV`" + paramTypes.Length : "__<>NVI`" + (paramTypes.Length + 1));
                if (type == null)
                    type = DeclaringType.GetClassLoader().GetTypeWrapperFactory().DefineDelegate(paramTypes.Length, ReturnType == PrimitiveTypeWrapper.VOID);

                var types = new Type[paramTypes.Length + (ReturnType == PrimitiveTypeWrapper.VOID ? 0 : 1)];
                for (int i = 0; i < paramTypes.Length; i++)
                    types[i] = paramTypes[i].TypeAsSignatureType;

                if (ReturnType != PrimitiveTypeWrapper.VOID)
                    types[types.Length - 1] = ReturnType.TypeAsSignatureType;

                return type.MakeGenericType(types);
            }

            return MethodHandleUtil.CreateMemberWrapperDelegateType(paramTypes, ReturnType);
        }

        /// <summary>
        /// Resolves the potentially dynamic member into it's final runtime method.
        /// </summary>
        internal void ResolveMethod()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // if we've still got the builder object, we need to replace it with the real thing before we can call it
            var mb = method as MethodBuilder;
            if (mb != null)
            {
#if NETFRAMEWORK
                method = mb.Module.ResolveMethod(mb.GetToken().Token);
#else
                // though ResolveMethod exists, Core 3.1 does not provide a stable way to obtain the resulting metadata token
                // FIXME .NET 6
                var flags = BindingFlags.DeclaredOnly;
                flags |= mb.IsPublic ? BindingFlags.Public : BindingFlags.NonPublic;
                flags |= mb.IsStatic ? BindingFlags.Static : BindingFlags.Instance;
                method = DeclaringType.TypeAsTBD.GetMethod(mb.Name, flags, null, GetParametersForDefineMethod(), null);
                if (method == null)
                    method = DeclaringType.TypeAsTBD.GetConstructor(flags, null, GetParametersForDefineMethod(), null);
#endif
            }
#endif
        }

        [HideFromJava]
        internal virtual object InvokeNonvirtualRemapped(object obj, object[] args)
        {
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Dynamically invokes the method and potentially unwraps any exceptions.
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [HideFromJava]
        protected static object InvokeAndUnwrapException(MethodBase mb, object obj, object[] args)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return mb.Invoke(obj, args);
            }
            catch (TargetInvocationException e)
            {
                throw ikvm.runtime.Util.mapException(e.InnerException);
            }
#endif
        }

        /// <summary>
        /// Dynamically invokes the method and potentially unwraps any exceptions.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [HideFromJava]
        internal virtual object Invoke(object obj, object[] args)
        {
            return InvokeAndUnwrapException(method, obj, args);
        }

        [HideFromJava]
        internal virtual object CreateInstance(object[] args)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return ((ConstructorInfo)method).Invoke(args);
            }
            catch (TargetInvocationException x)
            {
                throw ikvm.runtime.Util.mapException(x.InnerException);
            }
#endif
        }

#endif // !IMPORTER && !EXPORTER

        internal static OpCode SimpleOpCodeToOpCode(SimpleOpCode opc) => opc switch
        {
            SimpleOpCode.Call => OpCodes.Call,
            SimpleOpCode.Callvirt => OpCodes.Callvirt,
            SimpleOpCode.Newobj => OpCodes.Newobj,
            _ => throw new InvalidOperationException(),
        };

        internal virtual bool IsOptionalAttributeAnnotationValue => false;

        internal bool IsConstructor => Name == (object)StringConstants.INIT;

        internal bool IsClassInitializer => Name == (object)StringConstants.CLINIT;

        internal bool IsVirtual => (modifiers & (Modifiers.Static | Modifiers.Private)) == 0 && !IsConstructor;

        internal bool IsFinalizeOrClone
        {
            get
            {
                return IsProtected && (DeclaringType == CoreClasses.java.lang.Object.Wrapper || DeclaringType == CoreClasses.java.lang.Throwable.Wrapper) && (Name == StringConstants.CLONE || Name == StringConstants.FINALIZE);
            }
        }
    }

}
