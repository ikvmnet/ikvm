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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.ByteCode;
using IKVM.CoreLib.Runtime;

#if IMPORTER || EXPORTER
#else
using System.Reflection;
#endif

#if EXPORTER == false

using IKVM.Java.Externs.java.lang.invoke;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.CoreLib.Exceptions;

#endif

namespace IKVM.Runtime
{

    public static class ByteCodeHelper
    {

#if FIRST_PASS == false && EXPORTER == false

        static ObjectAccessor objectAccessor;

        static ObjectAccessor ObjectAccessor => JVM.Internal.BaseAccessors.Get(ref objectAccessor);

#endif

#if EXPORTER == false

        [DebuggerStepThrough]
        public static object multianewarray(RuntimeTypeHandle typeHandle, int[] lengths)
        {
            for (int i = 0; i < lengths.Length; i++)
            {
                if (lengths[i] < 0)
                {
#if !FIRST_PASS
                    throw new global::java.lang.NegativeArraySizeException();
#endif
                }
            }

            return MultianewarrayHelper(Type.GetTypeFromHandle(typeHandle).GetElementType(), lengths, 0);
        }


        static object MultianewarrayHelper(Type elemType, int[] lengths, int index)
        {
            var o = Array.CreateInstance(elemType, lengths[index++]);
            if (index < lengths.Length)
            {
                elemType = elemType.GetElementType();
                var a = (object[])o;
                for (int i = 0; i < a.Length; i++)
                    a[i] = MultianewarrayHelper(elemType, lengths, index);
            }

            return o;
        }

        [DebuggerStepThrough]
        public static object multianewarray_ghost(RuntimeTypeHandle typeHandle, int[] lengths)
        {
            var type = Type.GetTypeFromHandle(typeHandle);
            int rank = 0;
            while (ReflectUtil.IsVector(type))
            {
                rank++;
                type = type.GetElementType();
            }

            var obj = multianewarray(RuntimeArrayJavaType.MakeArrayType(typeof(object), rank).TypeHandle, lengths);
            GhostTag.SetTag(obj, typeHandle);
            return obj;
        }

        [DebuggerStepThrough]
        public static T[] anewarray_ghost<T>(int length, RuntimeTypeHandle typeHandle)
        {
            var obj = new T[length];
            GhostTag.SetTag(obj, typeHandle);
            return obj;
        }

        [DebuggerStepThrough]
        public static object DynamicMultianewarray(int[] lengths, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicMultianewarray");

            var wrapper = RuntimeJavaType.FromClass(clazz);
            var obj = multianewarray(wrapper.TypeAsArrayType.TypeHandle, lengths);
            if (wrapper.IsGhostArray)
                GhostTag.SetTag(obj, wrapper);

            return obj;
#endif
        }

        [DebuggerStepThrough]
        public static object DynamicNewarray(int length, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicNewarray");
            if (length < 0)
                throw new global::java.lang.NegativeArraySizeException();

            var wrapper = RuntimeJavaType.FromClass(clazz);
            var obj = Array.CreateInstance(wrapper.TypeAsArrayType, length);
            if (wrapper.IsGhost || wrapper.IsGhostArray)
                GhostTag.SetTag(obj, wrapper.MakeArrayType(1));

            return obj;
#endif
        }

#endif

        [DebuggerStepThrough]
        public static void DynamicAastore(object arrayref, int index, object val)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicAastore");
            ((Array)arrayref).SetValue(val, index);
#endif
        }

        [DebuggerStepThrough]
        public static object DynamicAaload(object arrayref, int index)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicAaload");
            return ((Array)arrayref).GetValue(index);
#endif
        }

#if EXPORTER == false

        // the sole purpose of this method is to check whether the clazz can be instantiated (but not to actually do it)
        [DebuggerStepThrough]
        public static void DynamicNewCheckOnly(global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicNewCheckOnly");
            var wrapper = RuntimeJavaType.FromClass(clazz);
            if (wrapper.IsAbstract)
                throw new global::java.lang.InstantiationError(wrapper.Name);

            wrapper.RunClassInit();
#endif
        }

        static RuntimeJavaType LoadTypeWrapper(string clazz, ikvm.@internal.CallerID callerId)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var context = RuntimeJavaType.FromClass(callerId.getCallerClass());
                var wrapper = RuntimeClassLoader.FromCallerID(callerId).LoadClassByName(clazz);
                var loader = callerId.getCallerClassLoader();
                if (loader != null)
                {
                    ClassLoaderAccessor cla = null;
                    JVM.Internal.BaseAccessors.Get(ref cla);
                    cla.InvokeCheckPackageAccess(loader, wrapper.ClassObject, callerId.getCallerClass().pd);
                }

                if (!wrapper.IsAccessibleFrom(context))
                    throw new global::java.lang.IllegalAccessError("Try to access class " + wrapper.Name + " from class " + context.Name);

                wrapper.Finish();
                return wrapper;
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        [DebuggerStepThrough]
        public static global::java.lang.Class DynamicClassLiteral(string clazz, ikvm.@internal.CallerID callerId)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicClassLiteral");
            return LoadTypeWrapper(clazz, callerId).ClassObject;
#endif
        }

        [DebuggerStepThrough]
        public static object DynamicCast(object obj, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Debug.Assert(obj != null);
            Profiler.Count("DynamicCast");
            if (!DynamicInstanceOf(obj, clazz))
                throw new global::java.lang.ClassCastException(IKVM.Java.Externs.ikvm.runtime.Util.GetTypeWrapperFromObject(JVM.Context, obj).Name + " cannot be cast to " + clazz.getName());

            return obj;
#endif
        }

        [DebuggerStepThrough]
        public static bool DynamicInstanceOf(object obj, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Debug.Assert(obj != null);
            Profiler.Count("DynamicInstanceOf");

            var tw = RuntimeJavaType.FromClass(clazz);
            // we have to mimick the bytecode behavior, which allows these .NET-isms to show through
            if (tw.TypeAsBaseType == typeof(Array))
                return obj is Array;

            if (tw.TypeAsBaseType == typeof(string))
                return obj is string;

            if (tw.TypeAsBaseType == typeof(IComparable))
                return obj is IComparable;

            return tw.IsInstance(obj);
#endif
        }

        [DebuggerStepThrough]
        public static global::java.lang.invoke.MethodType DynamicLoadMethodType(ref global::java.lang.invoke.MethodType cache, string sig, ikvm.@internal.CallerID callerID)
        {
            if (cache == null)
                DynamicLoadMethodTypeImpl(ref cache, sig, callerID);

            return cache;
        }

        private static void DynamicLoadMethodTypeImpl(ref global::java.lang.invoke.MethodType cache, string sig, global::ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var loader = RuntimeClassLoader.FromCallerID(callerID);
                var args = loader.ArgJavaTypeListFromSig(sig, LoadMode.LoadOrThrow);
                var ptypes = new global::java.lang.Class[args.Length];
                for (int i = 0; i < ptypes.Length; i++)
                    ptypes[i] = args[i].ClassObject;

                Interlocked.CompareExchange(ref cache, global::java.lang.invoke.MethodType.methodType(loader.RetTypeWrapperFromSig(sig, LoadMode.LoadOrThrow).ClassObject, ptypes), null);
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        [DebuggerStepThrough]
        public static global::java.lang.invoke.MethodHandle DynamicLoadMethodHandle(ref global::java.lang.invoke.MethodHandle cache, int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
        {
            if (cache == null)
                Interlocked.CompareExchange(ref cache, DynamicLoadMethodHandleImpl(kind, clazz, name, sig, callerID), null);

            return cache;
        }

        static global::java.lang.invoke.MethodHandle DynamicLoadMethodHandleImpl(int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var refc = LoadTypeWrapper(clazz, callerID).ClassObject;

            try
            {
                switch ((MethodHandleKind)kind)
                {
                    case MethodHandleKind.GetStatic:
                    case MethodHandleKind.PutStatic:
                    case MethodHandleKind.GetField:
                    case MethodHandleKind.PutField:
                        global::java.lang.Class type = RuntimeClassLoader.FromCallerID(callerID).FieldTypeWrapperFromSig(sig, LoadMode.LoadOrThrow).ClassObject;
                        return global::java.lang.invoke.MethodHandleNatives.linkMethodHandleConstant(callerID.getCallerClass(), kind, refc, name, type);
                    default:
                        global::java.lang.invoke.MethodType mt = null;
                        DynamicLoadMethodType(ref mt, sig, callerID);
                        // HACK linkMethodHandleConstant is broken for MethodHandle.invoke[Exact]
                        if (kind == (int)MethodHandleKind.InvokeVirtual && refc == JVM.Context.JavaBase.TypeOfJavaLangInvokeMethodHandle.ClassObject)
                        {
                            switch (name)
                            {
                                case "invokeExact":
                                    return global::java.lang.invoke.MethodHandles.exactInvoker(mt);
                                case "invoke":
                                    return global::java.lang.invoke.MethodHandles.invoker(mt);
                            }
                        }

                        return global::java.lang.invoke.MethodHandleNatives.linkMethodHandleConstant(callerID.getCallerClass(), kind, refc, name, mt);
                }
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        [DebuggerStepThrough]
        public static T DynamicBinderMemberLookup<T>(int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var mh = DynamicLoadMethodHandleImpl(kind, clazz, name, sig, callerID);
                return GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandles.explicitCastArguments(mh, JVM.Context.MethodHandleUtil.GetDelegateMethodType(typeof(T))));
            }
            catch (global::java.lang.IncompatibleClassChangeError x)
            {
                if (x.getCause() is global::java.lang.NoSuchMethodException)
                {
                    throw new global::java.lang.NoSuchMethodError(x.getCause().Message);
                }
                if (x.getCause() is global::java.lang.NoSuchFieldException)
                {
                    throw new global::java.lang.NoSuchFieldError(x.getCause().Message);
                }
                if (x.getCause() is global::java.lang.IllegalAccessException)
                {
                    throw new global::java.lang.IllegalAccessError(x.getCause().Message);
                }
                throw;
            }
#endif
        }

        [DebuggerStepThrough]
        public static Delegate DynamicCreateDelegate(object obj, Type delegateType, string name, string sig)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            RuntimeJavaType tw = RuntimeJavaType.FromClass(global::ikvm.runtime.Util.getClassFromObject(obj));
            RuntimeJavaMethod mw = tw.GetMethod(name, sig, true);
            if (mw == null || mw.IsStatic || !mw.IsPublic)
            {
#if NO_REF_EMIT
				global::java.lang.invoke.MethodType methodType = MethodHandleUtil.GetDelegateMethodType(delegateType);
				if (methodType.parameterCount() > MethodHandleUtil.MaxArity)
				{
					throw new NotImplementedException();
				}
				global::java.lang.invoke.MethodHandle exception = global::java.lang.invoke.MethodHandles.publicLookup()
					.findConstructor(mw == null || mw.IsStatic ? typeof(global::java.lang.AbstractMethodError) : typeof(global::java.lang.IllegalAccessError),
						global::java.lang.invoke.MethodType.methodType(typeof(void), typeof(string)))
					.bindTo(tw.Name + ".Invoke" + sig);
				return Delegate.CreateDelegate(delegateType,
						global::java.lang.invoke.MethodHandles.dropArguments(
							global::java.lang.invoke.MethodHandles.foldArguments(global::java.lang.invoke.MethodHandles.throwException(methodType.returnType(), exception.type().returnType()), exception),
							0, methodType.parameterArray()).vmtarget, "Invoke");
#else
                var invoke = delegateType.GetMethod("Invoke");
                var parameters = invoke.GetParameters();
                var parameterTypes = new Type[parameters.Length + 1];
                parameterTypes[0] = typeof(object);
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i + 1] = parameters[i].ParameterType;

                var dm = new System.Reflection.Emit.DynamicMethod("Invoke", invoke.ReturnType, parameterTypes);
                var ilgen = JVM.Context.CodeEmitterFactory.Create(dm);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldstr, tw.Name + ".Invoke" + sig);
                JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader()
                    .LoadClassByName(mw == null || mw.IsStatic ? "global::java.lang.AbstractMethodError" : "global::java.lang.IllegalAccessError")
                    .GetMethod("<init>", "(Lglobal::java.lang.String;)V", false)
                    .EmitNewobj(ilgen);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Throw);
                ilgen.DoEmit();
                return dm.CreateDelegate(delegateType, obj);
#endif
            }
            else
            {
                mw.ResolveMethod();
                return Delegate.CreateDelegate(delegateType, obj, (MethodInfo)mw.GetMethod());
            }
#endif
        }

        [DebuggerStepThrough]
        public static ikvm.@internal.CallerID DynamicCallerID(object capability)
        {
            return ((DynamicCallerIDProvider)capability).GetCallerID();
        }

        [DebuggerStepThrough]
        public static global::java.lang.invoke.MethodHandle DynamicEraseInvokeExact(global::java.lang.invoke.MethodHandle mh, global::java.lang.invoke.MethodType expected, global::java.lang.invoke.MethodType target)
        {
#if FIRST_PASS
            return null;
#else
            if (mh.type() != expected)
            {
                throw new global::java.lang.invoke.WrongMethodTypeException();
            }
            return global::java.lang.invoke.MethodHandles.explicitCastArguments(mh, target);
#endif
        }

#endif

        [DebuggerStepThrough]
        public static int f2i(float f)
        {
            if (f > int.MinValue && f < int.MaxValue)
            {
                return (int)f;
            }
            if (f <= int.MinValue)
            {
                return int.MinValue;
            }
            if (f >= int.MaxValue)
            {
                return int.MaxValue;
            }
            return 0;
        }

        [DebuggerStepThrough]
        public static long f2l(float f)
        {
            if (f > long.MinValue && f < long.MaxValue)
            {
                return (long)f;
            }
            if (f <= long.MinValue)
            {
                return long.MinValue;
            }
            if (f >= long.MaxValue)
            {
                return long.MaxValue;
            }
            return 0;
        }

        [DebuggerStepThrough]
        public static int d2i(double d)
        {
            if (d > int.MinValue && d < int.MaxValue)
            {
                return (int)d;
            }
            if (d <= int.MinValue)
            {
                return int.MinValue;
            }
            if (d >= int.MaxValue)
            {
                return int.MaxValue;
            }
            return 0;
        }

        [DebuggerStepThrough]
        public static long d2l(double d)
        {
            if (d > long.MinValue && d < long.MaxValue)
            {
                return (long)d;
            }
            if (d <= long.MinValue)
            {
                return long.MinValue;
            }
            if (d >= long.MaxValue)
            {
                return long.MaxValue;
            }
            return 0;
        }

#if EXPORTER == false

        // This is used by static JNI and synchronized methods that need a class object
        [DebuggerStepThrough]
        public static object GetClassFromTypeHandle(RuntimeTypeHandle typeHandle)
        {
            return IKVM.Java.Externs.ikvm.runtime.Util.getClassFromTypeHandle(typeHandle);
        }

        [DebuggerStepThrough]
        public static void arraycopy(object src, int srcStart, object dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // If the two arrays are the same, we can use the fast path, but we're also required to do so,
            // to get the required memmove semantics.
            if (src == dest)
            {
                try
                {
                    arraycopy_fast((Array)src, srcStart, (Array)dest, destStart, len);
                    return;
                }
                catch (InvalidCastException)
                {
                    throw new global::java.lang.ArrayStoreException();
                }
            }
            else if (src == null || dest == null)
            {
                throw new global::java.lang.NullPointerException();
            }
            else if (len < 0)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            else
            {
                if (src is object[] src1 && dest is object[] dst1)
                {
                    // for small copies, don't bother comparing the types as this is relatively expensive
                    if (len > 100 && src.GetType() == dest.GetType())
                    {
                        arraycopy_fast(src1, srcStart, dst1, destStart, len);
                        return;
                    }
                    else
                    {
                        for (; len > 0; len--)
                        {
                            // NOTE we don't need to catch ArrayTypeMismatchException & IndexOutOfRangeException, because
                            // they automatically get converted to the Java equivalents anyway.
                            dst1[destStart++] = src1[srcStart++];
                        }

                        return;
                    }
                }
                else if (src.GetType() != dest.GetType() && (IsPrimitiveArrayType(src.GetType()) || IsPrimitiveArrayType(dest.GetType())))
                {
                    // we don't want to allow copying a primitive into an object array!
                    throw new global::java.lang.ArrayStoreException();
                }
                else
                {
                    try
                    {
                        arraycopy_fast((Array)src, srcStart, (Array)dest, destStart, len);
                        return;
                    }
                    catch (InvalidCastException)
                    {
                        throw new global::java.lang.ArrayStoreException();
                    }
                }
            }
#endif
        }

        private static bool IsPrimitiveArrayType(Type type)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return type.IsArray && JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(type.GetElementType()).IsPrimitive;
#endif
        }

        [DebuggerStepThrough]
        public static void arraycopy_fast(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Array.Copy(src, srcStart, dest, destStart, len);
            }
            catch (ArgumentNullException)
            {
                throw new global::java.lang.NullPointerException();
            }
            catch (ArgumentException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
#endif
        }

        [DebuggerStepThrough]
        public static void arraycopy_primitive_8(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                checked
                {
                    Buffer.BlockCopy(src, srcStart * 8, dest, destStart * 8, len * 8);
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                throw new global::java.lang.NullPointerException();
            }
            catch (OverflowException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            catch (ArgumentException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
#endif
        }

        [DebuggerStepThrough]
        public static void arraycopy_primitive_4(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                checked
                {
                    Buffer.BlockCopy(src, srcStart * 4, dest, destStart * 4, len * 4);
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                throw new global::java.lang.NullPointerException();
            }
            catch (OverflowException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            catch (ArgumentException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
#endif
        }

        [DebuggerStepThrough]
        public static void arraycopy_primitive_2(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                checked
                {
                    Buffer.BlockCopy(src, srcStart * 2, dest, destStart * 2, len * 2);
                    return;
                }
            }
            catch (ArgumentNullException)
            {
                throw new global::java.lang.NullPointerException();
            }
            catch (OverflowException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            catch (ArgumentException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
#endif
        }

        [DebuggerStepThrough]
        public static void arraycopy_primitive_1(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                Buffer.BlockCopy(src, srcStart, dest, destStart, len);
                return;
            }
            catch (ArgumentNullException)
            {
                throw new global::java.lang.NullPointerException();
            }
            catch (OverflowException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
            catch (ArgumentException)
            {
                throw new global::java.lang.ArrayIndexOutOfBoundsException();
            }
#endif
        }

        [HideFromJava]
        public static void VerboseCastFailure(RuntimeTypeHandle typeHandle, object obj)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Type t1 = obj.GetType();
            Type t2 = Type.GetTypeFromHandle(typeHandle);
            string msg;
            if (t1.Assembly.FullName == t2.Assembly.FullName && t1.Assembly.Location != t2.Assembly.Location)
            {
                string l1 = t1.Assembly.Location;
                string l2 = t2.Assembly.Location;
                if (l1 == "")
                {
                    l1 = "unknown location";
                }
                if (l2 == "")
                {
                    l2 = "unknown location";
                }
                msg = String.Format("Object of type \"{0}\" loaded from {1} cannot be cast to \"{2}\" loaded from {3}", t1.AssemblyQualifiedName, l1, t2.AssemblyQualifiedName, l2);
            }
            else
            {
                msg = String.Format("Object of type \"{0}\" cannot be cast to \"{1}\"", t1.AssemblyQualifiedName, t2.AssemblyQualifiedName);
            }
            throw new global::java.lang.ClassCastException(msg);
#endif
        }

        /// <summary>
        /// Returns <c>true</c> if the Java finalizer should be skipped.
        /// </summary>
        /// <returns></returns>
        public static bool SkipFinalizer()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return Environment.HasShutdownStarted && !global::java.lang.Shutdown.runFinalizersOnExit;
#endif
        }

        /// <summary>
        /// Returns <c>true</c> if the Java finalizer for the given object should be skipped.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool SkipFinalizer(object o)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (SkipFinalizer())
                return true;

            if (o is global::java.lang.Object jlo)
                if (ObjectAccessor.GetInit(jlo) == false)
                    return true;

            return false;
#endif
        }

#endif

        /// <summary>
        /// Implements a volatile read against a reference to a bool. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static bool VolatileRead(ref bool location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Volatile.Read(ref location);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a bool. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref bool location, bool value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a byte. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static byte VolatileRead(ref byte location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Volatile.Read(ref location);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a byte. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref byte location, byte value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a char. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static char VolatileRead(ref char location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return (char)Volatile.Read(ref Unsafe.As<char, short>(ref location));
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a char. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref char location, char value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref Unsafe.As<char, short>(ref location), (short)value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a short. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static short VolatileRead(ref short location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Volatile.Read(ref location);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a short. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref short location, short value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a int. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static int VolatileRead(ref int location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Volatile.Read(ref location);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a int. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref int location, int value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a long. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static long VolatileRead(ref long location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, 0, 0);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a long. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref long location, long value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Interlocked.Exchange(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a float. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static float VolatileRead(ref float location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Volatile.Read(ref location);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a float. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref float location, float value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Volatile.Write(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile read against a reference to a double. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static double VolatileRead(ref double location)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, 0, 0);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a volatile write against a reference to a double. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref double location, double value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                Interlocked.Exchange(ref location, value);
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements the compare and swap operation against a reference to an object.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CompareAndSwap(ref object location, object expected, object value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, value, expected) == expected;
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements the compare and swap operation against a reference to an int.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CompareAndSwap(ref int location, int expected, int value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, value, expected) == expected;
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a compare and swap operation against a reference to a long.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CompareAndSwap(ref long location, long expected, long value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, value, expected) == expected;
            }
            catch (SEHException e)
            {
                throw new global::java.lang.InternalError(e);
            }
#endif
        }

        /// <summary>
        /// Implements a compare and swap operation against a reference to a double.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="expected"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool CompareAndSwap(ref double location, double expected, double value)
        {
#if FIRST_PASS || IMPORTER || EXPORTER
            throw new NotImplementedException();
#else
            try
            {
                return Interlocked.CompareExchange(ref location, value, expected) == expected;
            }
            catch (SEHException)
            {
                throw new global::java.lang.InternalError();
            }
#endif
        }

#if EXPORTER == false

        /// <summary>
        /// Invoked by exported assemblies.
        /// </summary>
        /// <param name="module"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void InitializeModule(Module module)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var asm = Assembly.GetCallingAssembly();
            if (module.Assembly != asm)
                throw new ArgumentOutOfRangeException();

            // check for InitializeModule method present on classloader
            var classLoader = JVM.Context.AssemblyClassLoaderFactory.FromAssembly(asm).GetJavaClassLoader();
            if (classLoader != null)
            {
                var init = (Action<Module>)Delegate.CreateDelegate(typeof(Action<Module>), classLoader, "InitializeModule", false, false);
                if (init != null)
                    init(module);
            }
#endif
        }

        public static T GetDotNetEnumField<T>(string name)
#if !FIRST_PASS
            where T : global::java.lang.Enum
#endif
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return (T)global::java.lang.Enum.valueOf((global::java.lang.Class)ClassLiteral<T>.Value, name);
            }
            catch (global::java.lang.IllegalArgumentException)
            {
                throw new global::java.lang.NoSuchFieldError(((global::java.lang.Class)ClassLiteral<T>.Value).getName() + "." + name);
            }
#endif
        }

        [Flags]
        public enum MapFlags
        {
            None = 0,
            NoRemapping = 1,
            Unused = 2,
        }

        [HideFromJava]
        public static T MapException<T>(Exception x, MapFlags mode) where T : Exception
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.ExceptionHelper.MapException<T>(x, (mode & MapFlags.NoRemapping) == 0, (mode & MapFlags.Unused) != 0);
#endif
        }

        [HideFromJava]
        public static Exception DynamicMapException(Exception x, MapFlags mode, global::java.lang.Class exceptionClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var exceptionTypeWrapper = RuntimeJavaType.FromClass(exceptionClass);
            mode &= ~MapFlags.NoRemapping;
            if (exceptionTypeWrapper.IsSubTypeOf(JVM.Context.JavaBase.TypeOfCliSystemException))
                mode |= MapFlags.NoRemapping;

            var exceptionType = exceptionTypeWrapper == JVM.Context.JavaBase.TypeOfjavaLangThrowable ? typeof(System.Exception) : exceptionTypeWrapper.TypeAsBaseType;
            return (Exception)JVM.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(exceptionType).Invoke(null, new object[] { x, mode });
#endif
        }

        public static T GetDelegateForInvokeExact<T>(object methodHandle)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var methodHandle_ = (global::java.lang.invoke.MethodHandle)methodHandle;
            var del = methodHandle_._invokeExactDelegate as T;
            del ??= JVM.Context.MethodHandleUtil.GetDelegateForInvokeExact<T>(methodHandle_);
            return del;
#endif
        }

        public static T GetDelegateForInvoke<T>(object methodHandle, object realType, ref InvokeCache<T> cache)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var methodHandle_ = (global::java.lang.invoke.MethodHandle)methodHandle;
            var realType_ = (global::java.lang.invoke.MethodType)realType;

            T del;
            if (cache.Type == methodHandle_.type() && (del = (methodHandle_.isVarargsCollector() ? cache.varArg : cache.fixedArg)) != null)
                return del;

            del = methodHandle_.form.vmentry.vmtarget as T;
            if (del == null)
            {
                var adapter = global::java.lang.invoke.MethodHandles.exactInvoker(methodHandle_.type());
                if (methodHandle_.isVarargsCollector())
                    adapter = adapter.asVarargsCollector(methodHandle_.type().parameterType(methodHandle_.type().parameterCount() - 1));

                // if realType is set, the delegate contains erased unloadable types
                if (realType_ != null)
                    adapter = adapter.asType(realType_.insertParameterTypes(0, (global::java.lang.Class)ClassLiteral<global::java.lang.invoke.MethodHandle>.Value)).asFixedArity();

                adapter = adapter.asType(JVM.Context.MethodHandleUtil.GetDelegateMethodType(typeof(T)));
                del = GetDelegateForInvokeExact<T>(adapter);
                if (cache.TrySetType(methodHandle_.type()))
                {
                    if (methodHandle_.isVarargsCollector())
                        cache.varArg = del;
                    else
                        cache.fixedArg = del;
                }
            }

            return del;
#endif
        }

        public static T GetDelegateForInvokeBasic<T>(global::java.lang.invoke.MethodHandle h)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var del = h.form.vmentry.vmtarget as T;
            del ??= MethodHandleUtil.GetVoidAdapter(h.form.vmentry) as T;
            return del;
#endif
        }

        public static global::java.lang.invoke.MethodType LoadMethodType<T>()
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.MethodHandleUtil.GetDelegateMethodType(typeof(T));
#endif
        }

        [HideFromJava]
        public static void LinkIndyCallSite<T>(ref IndyCallSite<T> site, global::java.lang.invoke.CallSite cs, Exception x)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // when a CallSite is first constructed, it doesn't call MethodHandleNatives.setCallSiteTargetNormal(),
            // so we have to check if we need to initialize it here (i.e. attach an IndyCallSite<T> to it)
            if (cs != null)
            {
                if (cs.ics == null)
                    MethodHandleNatives.InitializeCallSite(cs);

                lock (cs.ics)
                    cs.ics.SetTarget(cs.target);
            }

            IndyCallSite<T> ics;
            if (x != null || cs == null || (ics = cs.ics as IndyCallSite<T>) == null)
            {
                x = MapException<Exception>(x ?? (cs == null
                    ? (Exception)new global::java.lang.ClassCastException("bootstrap method failed to produce a CallSite")
                    : new global::java.lang.invoke.WrongMethodTypeException()), MapFlags.None);
                global::java.lang.invoke.MethodType type = LoadMethodType<T>();
                global::java.lang.invoke.MethodHandle exc = x is global::java.lang.BootstrapMethodError
                    ? global::java.lang.invoke.MethodHandles.constant(typeof(global::java.lang.BootstrapMethodError), x)
                    : global::java.lang.invoke.MethodHandles.publicLookup().findConstructor(typeof(global::java.lang.BootstrapMethodError), global::java.lang.invoke.MethodType.methodType(typeof(void), typeof(string), typeof(Exception)))
                        .bindTo("call site initialization exception").bindTo(x);
                ics = new IndyCallSite<T>();
                ((IIndyCallSite)ics).SetTarget(
                    global::java.lang.invoke.MethodHandles.dropArguments(
                        global::java.lang.invoke.MethodHandles.foldArguments(
                            global::java.lang.invoke.MethodHandles.throwException(type.returnType(), typeof(global::java.lang.BootstrapMethodError)),
                                exc),
                        0, type.parameterArray()));
            }

            var curr = site;
            if (curr.IsBootstrap)
                Interlocked.CompareExchange(ref site, ics, curr);
#endif
        }

        [HideFromJava]
        public static void DynamicLinkIndyCallSite<T>(ref IndyCallSite<T> site, global::java.lang.invoke.CallSite cs, Exception x, string signature, ikvm.@internal.CallerID callerID)
            where T : class, Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // when a CallSite is first constructed, it doesn't call MethodHandleNatives.setCallSiteTargetNormal(),
            // so we have to check if we need to initialize it here (i.e. attach an IndyCallSite<T> to it)
            if (cs != null)
            {
                if (cs.ics == null)
                    MethodHandleNatives.InitializeCallSite(cs);

                lock (cs.ics)
                    cs.ics.SetTarget(cs.target);
            }

            global::java.lang.invoke.MethodType typeCache = null;
            IndyCallSite<T> ics;
            if (x != null || cs == null || cs.type() != DynamicLoadMethodType(ref typeCache, signature, callerID))
            {
                x = MapException<Exception>(x ?? (cs == null
                    ? (Exception)new global::java.lang.ClassCastException("bootstrap method failed to produce a CallSite")
                    : new global::java.lang.invoke.WrongMethodTypeException()), MapFlags.None);
                global::java.lang.invoke.MethodType type = LoadMethodType<T>();
                global::java.lang.invoke.MethodHandle exc = x is global::java.lang.BootstrapMethodError
                    ? global::java.lang.invoke.MethodHandles.constant(typeof(global::java.lang.BootstrapMethodError), x)
                    : global::java.lang.invoke.MethodHandles.publicLookup().findConstructor(typeof(global::java.lang.BootstrapMethodError), global::java.lang.invoke.MethodType.methodType(typeof(void), typeof(string), typeof(Exception)))
                        .bindTo("call site initialization exception").bindTo(x);
                ics = new IndyCallSite<T>();
                ((IIndyCallSite)ics).SetTarget(
                    global::java.lang.invoke.MethodHandles.dropArguments(
                        global::java.lang.invoke.MethodHandles.foldArguments(
                            global::java.lang.invoke.MethodHandles.throwException(type.returnType(), typeof(global::java.lang.BootstrapMethodError)),
                                exc),
                        0, type.parameterArray()));
            }
            else
            {
                ics = new IndyCallSite<T>();
                ((IIndyCallSite)ics).SetTarget(cs.dynamicInvoker().asType(LoadMethodType<T>()));
            }

            IndyCallSite<T> curr = site;
            if (curr.IsBootstrap)
                Interlocked.CompareExchange(ref site, ics, curr);
#endif
        }

#endif

    }

#if EXPORTER == false

    interface IIndyCallSite
    {

        void SetTarget(global::java.lang.invoke.MethodHandle mh);

    }

    public sealed class IndyCallSite<T> : IIndyCallSite
        where T : class, Delegate
    {

        internal readonly bool IsBootstrap;
        volatile T target;

        internal IndyCallSite()

        {
        }

        internal IndyCallSite(T target, bool bootstrap)
        {
            this.IsBootstrap = bootstrap;
            this.target = target;
        }

        void IIndyCallSite.SetTarget(global::java.lang.invoke.MethodHandle mh)
        {
#if !FIRST_PASS
            target = ByteCodeHelper.GetDelegateForInvokeExact<T>(mh);
#endif
        }

        public static IndyCallSite<T> CreateBootstrap(T bootstrap)
        {
            return new IndyCallSite<T>(bootstrap, true);
        }

        public T GetTarget()
        {
            return target;
        }

    }

    public struct InvokeCache<T>
        where T : class
    {

        internal T fixedArg;
        internal T varArg;

        global::java.lang.invoke.MethodType type;

        internal global::java.lang.invoke.MethodType Type => type;

        internal bool TrySetType(global::java.lang.invoke.MethodType newType)
        {
            Interlocked.CompareExchange(ref type, newType, null);
            return type == newType;
        }

    }

#endif

    [StructLayout(LayoutKind.Explicit)]
    public struct DoubleConverter
    {

        [FieldOffset(0)]
        double d;

        [FieldOffset(0)]
        long l;

        public static long ToLong(double value, ref DoubleConverter converter)
        {
            converter.d = value;
            return converter.l;
        }

        public static double ToDouble(long value, ref DoubleConverter converter)
        {
            converter.l = value;
            return converter.d;
        }

    }

    [StructLayout(LayoutKind.Explicit)]
    public struct FloatConverter
    {

        [FieldOffset(0)]
        float f;

        [FieldOffset(0)]
        int i;

        public static int ToInt(float value, ref FloatConverter converter)
        {
            converter.f = value;
            return converter.i;
        }

        public static float ToFloat(int value, ref FloatConverter converter)
        {
            converter.i = value;
            return converter.f;
        }

    }

    public struct MHA<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public T1 t1;
        public T2 t2;
        public T3 t3;
        public T4 t4;
        public T5 t5;
        public T6 t6;
        public T7 t7;
        public T8 t8;

        public MHA(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            this.t1 = t1;
            this.t2 = t2;
            this.t3 = t3;
            this.t4 = t4;
            this.t5 = t5;
            this.t6 = t6;
            this.t7 = t7;
            this.t8 = t8;
        }
    }

    public delegate void MHV();
    public delegate void MHV<T1>(T1 t1);
    public delegate void MHV<T1, T2>(T1 t1, T2 t2);
    public delegate void MHV<T1, T2, T3>(T1 t1, T2 t2, T3 t3);
    public delegate void MHV<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate void MHV<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate void MHV<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate void MHV<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate void MHV<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

    public delegate TResult MH<TResult>();
    public delegate TResult MH<T1, TResult>(T1 t1);
    public delegate TResult MH<T1, T2, TResult>(T1 t1, T2 t2);
    public delegate TResult MH<T1, T2, T3, TResult>(T1 t1, T2 t2, T3 t3);
    public delegate TResult MH<T1, T2, T3, T4, TResult>(T1 t1, T2 t2, T3 t3, T4 t4);
    public delegate TResult MH<T1, T2, T3, T4, T5, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);
    public delegate TResult MH<T1, T2, T3, T4, T5, T6, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);
    public delegate TResult MH<T1, T2, T3, T4, T5, T6, T7, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);
    public delegate TResult MH<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

    public static class LiveObjectHolder<T>
    {

        public static object[] values;

    }

}
