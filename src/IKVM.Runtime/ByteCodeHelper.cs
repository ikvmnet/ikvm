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
using IKVM.Internal;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

#if EXPORTER == false

using IKVM.Java.Externs.java.lang.invoke;
using IKVM.Runtime.Accessors.Java.Lang;

#endif

namespace IKVM.Runtime
{

    public static class ByteCodeHelper
    {

#if FIRST_PASS == false && EXPORTER == FALSE

        static ObjectAccessor objectAccessor;

        static ObjectAccessor ObjectAccessor => JVM.BaseAccessors.Get(ref objectAccessor);

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
            object o = Array.CreateInstance(elemType, lengths[index++]);
            if (index < lengths.Length)
            {
                elemType = elemType.GetElementType();
                object[] a = (object[])o;
                for (int i = 0; i < a.Length; i++)
                {
                    a[i] = MultianewarrayHelper(elemType, lengths, index);
                }
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
            object obj = multianewarray(ArrayTypeWrapper.MakeArrayType(typeof(object), rank).TypeHandle, lengths);
            GhostTag.SetTag(obj, typeHandle);
            return obj;
        }

        [DebuggerStepThrough]
        public static T[] anewarray_ghost<T>(int length, RuntimeTypeHandle typeHandle)
        {
            T[] obj = new T[length];
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
            TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
            object obj = multianewarray(wrapper.TypeAsArrayType.TypeHandle, lengths);
            if (wrapper.IsGhostArray)
            {
                GhostTag.SetTag(obj, wrapper);
            }
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
            {
                throw new global::java.lang.NegativeArraySizeException();
            }
            TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
            Array obj = Array.CreateInstance(wrapper.TypeAsArrayType, length);
            if (wrapper.IsGhost || wrapper.IsGhostArray)
            {
                GhostTag.SetTag(obj, wrapper.MakeArrayType(1));
            }
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
        [DebuggerStepThroughAttribute]
        public static void DynamicNewCheckOnly(global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicNewCheckOnly");
            TypeWrapper wrapper = TypeWrapper.FromClass(clazz);
            if (wrapper.IsAbstract)
            {
                throw new global::java.lang.InstantiationError(wrapper.Name);
            }
            wrapper.RunClassInit();
#endif
        }

        private static TypeWrapper LoadTypeWrapper(string clazz, ikvm.@internal.CallerID callerId)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                TypeWrapper context = TypeWrapper.FromClass(callerId.getCallerClass());
                TypeWrapper wrapper = ClassLoaderWrapper.FromCallerID(callerId).LoadClassByDottedName(clazz);
                global::java.lang.ClassLoader loader = callerId.getCallerClassLoader();
                if (loader != null)
                {
                    ClassLoaderAccessor cla = null;
                    JVM.BaseAccessors.Get(ref cla);
                    cla.InvokeCheckPackageAccess(loader, wrapper.ClassObject, callerId.getCallerClass().pd);
                }
                if (!wrapper.IsAccessibleFrom(context))
                {
                    throw new global::java.lang.IllegalAccessError("Try to access class " + wrapper.Name + " from class " + context.Name);
                }
                wrapper.Finish();
                return wrapper;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

        [DebuggerStepThroughAttribute]
        public static global::java.lang.Class DynamicClassLiteral(string clazz, ikvm.@internal.CallerID callerId)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Count("DynamicClassLiteral");
            return LoadTypeWrapper(clazz, callerId).ClassObject;
#endif
        }

        [DebuggerStepThroughAttribute]
        public static object DynamicCast(object obj, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Debug.Assert(obj != null);
            Profiler.Count("DynamicCast");
            if (!DynamicInstanceOf(obj, clazz))
            {
                throw new global::java.lang.ClassCastException(IKVM.Java.Externs.ikvm.runtime.Util.GetTypeWrapperFromObject(obj).Name + " cannot be cast to " + clazz.getName());
            }
            return obj;
#endif
        }

        [DebuggerStepThroughAttribute]
        public static bool DynamicInstanceOf(object obj, global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Debug.Assert(obj != null);
            Profiler.Count("DynamicInstanceOf");
            TypeWrapper tw = TypeWrapper.FromClass(clazz);
            // we have to mimick the bytecode behavior, which allows these .NET-isms to show through
            if (tw.TypeAsBaseType == typeof(Array))
            {
                return obj is Array;
            }
            if (tw.TypeAsBaseType == typeof(string))
            {
                return obj is string;
            }
            if (tw.TypeAsBaseType == typeof(IComparable))
            {
                return obj is IComparable;
            }
            return tw.IsInstance(obj);
#endif
        }

        [DebuggerStepThrough]
        public static global::java.lang.invoke.MethodType DynamicLoadMethodType(ref global::java.lang.invoke.MethodType cache, string sig, ikvm.@internal.CallerID callerID)
        {
            if (cache == null)
            {
                DynamicLoadMethodTypeImpl(ref cache, sig, callerID);
            }

            return cache;
        }

        private static void DynamicLoadMethodTypeImpl(ref global::java.lang.invoke.MethodType cache, string sig, global::ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                ClassLoaderWrapper loader = ClassLoaderWrapper.FromCallerID(callerID);
                TypeWrapper[] args = loader.ArgTypeWrapperListFromSig(sig, LoadMode.LoadOrThrow);
                global::java.lang.Class[] ptypes = new global::java.lang.Class[args.Length];
                for (int i = 0; i < ptypes.Length; i++)
                {
                    ptypes[i] = args[i].ClassObject;
                }
                Interlocked.CompareExchange(ref cache, global::java.lang.invoke.MethodType.methodType(loader.RetTypeWrapperFromSig(sig, LoadMode.LoadOrThrow).ClassObject, ptypes), null);
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

        [DebuggerStepThrough]
        public static global::java.lang.invoke.MethodHandle DynamicLoadMethodHandle(ref global::java.lang.invoke.MethodHandle cache, int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
        {
            if (cache == null)
            {
                Interlocked.CompareExchange(ref cache, DynamicLoadMethodHandleImpl(kind, clazz, name, sig, callerID), null);
            }

            return cache;
        }

        private static global::java.lang.invoke.MethodHandle DynamicLoadMethodHandleImpl(int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            global::java.lang.Class refc = LoadTypeWrapper(clazz, callerID).ClassObject;
            try
            {
                switch ((ReferenceKind)kind)
                {
                    case ReferenceKind.GetStatic:
                    case ReferenceKind.PutStatic:
                    case ReferenceKind.GetField:
                    case ReferenceKind.PutField:
                        global::java.lang.Class type = ClassLoaderWrapper.FromCallerID(callerID).FieldTypeWrapperFromSig(sig, LoadMode.LoadOrThrow).ClassObject;
                        return global::java.lang.invoke.MethodHandleNatives.linkMethodHandleConstant(callerID.getCallerClass(), kind, refc, name, type);
                    default:
                        global::java.lang.invoke.MethodType mt = null;
                        DynamicLoadMethodType(ref mt, sig, callerID);
                        // HACK linkMethodHandleConstant is broken for MethodHandle.invoke[Exact]
                        if (kind == (int)ReferenceKind.InvokeVirtual && refc == CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject)
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
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

        [DebuggerStepThrough]
        public static T DynamicBinderMemberLookup<T>(int kind, string clazz, string name, string sig, ikvm.@internal.CallerID callerID)
            where T : class /* delegate */
        {
#if FIRST_PASS
            return null;
#else
            try
            {
                global::java.lang.invoke.MethodHandle mh = DynamicLoadMethodHandleImpl(kind, clazz, name, sig, callerID);
                return GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandles.explicitCastArguments(mh, MethodHandleUtil.GetDelegateMethodType(typeof(T))));
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
            return null;
#else
            TypeWrapper tw = TypeWrapper.FromClass(global::ikvm.runtime.Util.getClassFromObject(obj));
            MethodWrapper mw = tw.GetMethodWrapper(name, sig, true);
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
                MethodInfo invoke = delegateType.GetMethod("Invoke");
                ParameterInfo[] parameters = invoke.GetParameters();
                Type[] parameterTypes = new Type[parameters.Length + 1];
                parameterTypes[0] = typeof(object);
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameterTypes[i + 1] = parameters[i].ParameterType;
                }
                System.Reflection.Emit.DynamicMethod dm = new System.Reflection.Emit.DynamicMethod("Invoke", invoke.ReturnType, parameterTypes);
                CodeEmitter ilgen = CodeEmitter.Create(dm);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldstr, tw.Name + ".Invoke" + sig);
                ClassLoaderWrapper.GetBootstrapClassLoader()
                    .LoadClassByDottedName(mw == null || mw.IsStatic ? "global::java.lang.AbstractMethodError" : "global::java.lang.IllegalAccessError")
                    .GetMethodWrapper("<init>", "(Lglobal::java.lang.String;)V", false)
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

        [DebuggerStepThroughAttribute]
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

        [DebuggerStepThroughAttribute]
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

        [DebuggerStepThroughAttribute]
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
#if !FIRST_PASS
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
                object[] src1 = src as object[];
                object[] dst1 = dest as object[];
                if (src1 != null && dst1 != null)
                {
                    // for small copies, don't bother comparing the types as this is relatively expensive
                    if (len > 50 && src.GetType() == dest.GetType())
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
                else if (src.GetType() != dest.GetType() &&
                        (IsPrimitiveArrayType(src.GetType()) || IsPrimitiveArrayType(dest.GetType())))
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
#endif // !FIRST_PASS
        }

        private static bool IsPrimitiveArrayType(Type type)
        {
            return type.IsArray && ClassLoaderWrapper.GetWrapperFromType(type.GetElementType()).IsPrimitive;
        }

        [DebuggerStepThroughAttribute]
        public static void arraycopy_fast(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if !FIRST_PASS
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

        [DebuggerStepThroughAttribute]
        public static void arraycopy_primitive_8(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if !FIRST_PASS
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
#endif // !FIRST_PASS
        }

        [DebuggerStepThroughAttribute]
        public static void arraycopy_primitive_4(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if !FIRST_PASS
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
#endif // !FIRST_PASS
        }

        [DebuggerStepThroughAttribute]
        public static void arraycopy_primitive_2(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if !FIRST_PASS
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
#endif // !FIRST_PASS
        }

        [DebuggerStepThroughAttribute]
        public static void arraycopy_primitive_1(Array src, int srcStart, Array dest, int destStart, int len)
        {
#if !FIRST_PASS
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
#endif // !FIRST_PASS
        }

        [HideFromJava]
        public static void VerboseCastFailure(RuntimeTypeHandle typeHandle, object obj)
        {
#if !FIRST_PASS
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
#endif // !FIRST_PASS
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
            return Volatile.Read(ref location);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a bool. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref bool location, bool value)
        {
            Volatile.Write(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a byte. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static byte VolatileRead(ref byte location)
        {
            return Volatile.Read(ref location);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a byte. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref byte location, byte value)
        {
            Volatile.Write(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a char. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static char VolatileRead(ref char location)
        {
            return (char)Volatile.Read(ref Unsafe.As<char, short>(ref location));
        }

        /// <summary>
        /// Implements a volatile write against a reference to a char. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref char location, char value)
        {
            Volatile.Write(ref Unsafe.As<char, short>(ref location), (short)value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a short. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static short VolatileRead(ref short location)
        {
            return Volatile.Read(ref location);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a short. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref short location, short value)
        {
            Volatile.Write(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a int. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static int VolatileRead(ref int location)
        {
            return Volatile.Read(ref location);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a int. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref int location, int value)
        {
            Volatile.Write(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a long. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static long VolatileRead(ref long location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a long. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref long location, long value)
        {
            Interlocked.Exchange(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a float. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static float VolatileRead(ref float location)
        {
            return Volatile.Read(ref location);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a float. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref float location, float value)
        {
            Volatile.Write(ref location, value);
        }

        /// <summary>
        /// Implements a volatile read against a reference to a double. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static double VolatileRead(ref double location)
        {
            return Interlocked.CompareExchange(ref location, 0, 0);
        }

        /// <summary>
        /// Implements a volatile write against a reference to a double. In Java, a volatile operation is atomic.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        public static void VolatileWrite(ref double location, double value)
        {
            Interlocked.Exchange(ref location, value);
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
            return Interlocked.CompareExchange(ref location, value, expected) == expected;
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
            return Interlocked.CompareExchange(ref location, value, expected) == expected;
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
            return Interlocked.CompareExchange(ref location, value, expected) == expected;
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
            return Interlocked.CompareExchange(ref location, value, expected) == expected;
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
            var classLoader = AssemblyClassLoader.FromAssembly(asm).GetJavaClassLoader();
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
            return ExceptionHelper.MapException<T>(x, (mode & MapFlags.NoRemapping) == 0, (mode & MapFlags.Unused) != 0);
        }

        [HideFromJava]
        public static Exception DynamicMapException(Exception x, MapFlags mode, global::java.lang.Class exceptionClass)
        {
#if FIRST_PASS
            return null;
#else
            TypeWrapper exceptionTypeWrapper = TypeWrapper.FromClass(exceptionClass);
            mode &= ~MapFlags.NoRemapping;
            if (exceptionTypeWrapper.IsSubTypeOf(CoreClasses.cli.System.Exception.Wrapper))
            {
                mode |= MapFlags.NoRemapping;
            }
            Type exceptionType = exceptionTypeWrapper == CoreClasses.java.lang.Throwable.Wrapper ? typeof(System.Exception) : exceptionTypeWrapper.TypeAsBaseType;
            return (Exception)ByteCodeHelperMethods.mapException.MakeGenericMethod(exceptionType).Invoke(null, new object[] { x, mode });
#endif
        }

        public static T GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandle h)
            where T : class
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            T del = h._invokeExactDelegate as T;
            if (del == null)
            {
                del = MethodHandleUtil.GetDelegateForInvokeExact<T>(h);
            }
            return del;
#endif
        }

        public static T GetDelegateForInvoke<T>(global::java.lang.invoke.MethodHandle h, global::java.lang.invoke.MethodType realType, ref InvokeCache<T> cache)
            where T : class
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            T del;
            if (cache.Type == h.type() && (del = (h.isVarargsCollector() ? cache.varArg : cache.fixedArg)) != null)
            {
                return del;
            }
            del = h.form.vmentry.vmtarget as T;
            if (del == null)
            {
                global::java.lang.invoke.MethodHandle adapter = global::java.lang.invoke.MethodHandles.exactInvoker(h.type());
                if (h.isVarargsCollector())
                {
                    adapter = adapter.asVarargsCollector(h.type().parameterType(h.type().parameterCount() - 1));
                }
                // if realType is set, the delegate contains erased unloadable types
                if (realType != null)
                {
                    adapter = adapter.asType(realType.insertParameterTypes(0, (global::java.lang.Class)ClassLiteral<global::java.lang.invoke.MethodHandle>.Value)).asFixedArity();
                }
                adapter = adapter.asType(MethodHandleUtil.GetDelegateMethodType(typeof(T)));
                del = GetDelegateForInvokeExact<T>(adapter);
                if (cache.TrySetType(h.type()))
                {
                    if (h.isVarargsCollector())
                    {
                        cache.varArg = del;
                    }
                    else
                    {
                        cache.fixedArg = del;
                    }
                }
            }
            return del;
#endif
        }

        public static T GetDelegateForInvokeBasic<T>(global::java.lang.invoke.MethodHandle h)
            where T : class
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            T del = h.form.vmentry.vmtarget as T;
            if (del == null)
            {
                del = MethodHandleUtil.GetVoidAdapter(h.form.vmentry) as T;
            }
            return del;
#endif
        }

        public static global::java.lang.invoke.MethodType LoadMethodType<T>()
            where T : class // Delegate
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return MethodHandleUtil.GetDelegateMethodType(typeof(T));
#endif
        }

        [HideFromJava]
        public static void LinkIndyCallSite<T>(ref IndyCallSite<T> site, global::java.lang.invoke.CallSite cs, Exception x)
            where T : class // Delegate
        {
#if !FIRST_PASS
            // when a CallSite is first constructed, it doesn't call MethodHandleNatives.setCallSiteTargetNormal(),
            // so we have to check if we need to initialize it here (i.e. attach an IndyCallSite<T> to it)
            if (cs != null)
            {
                if (cs.ics == null)
                {
                    MethodHandleNatives.InitializeCallSite(cs);
                }
                lock (cs.ics)
                {
                    cs.ics.SetTarget(cs.target);
                }
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
            IndyCallSite<T> curr = site;
            if (curr.IsBootstrap)
            {
                Interlocked.CompareExchange(ref site, ics, curr);
            }
#endif
        }

        [HideFromJava]
        public static void DynamicLinkIndyCallSite<T>(ref IndyCallSite<T> site, global::java.lang.invoke.CallSite cs, Exception x, string signature, ikvm.@internal.CallerID callerID)
            where T : class // Delegate
        {
#if !FIRST_PASS
            // when a CallSite is first constructed, it doesn't call MethodHandleNatives.setCallSiteTargetNormal(),
            // so we have to check if we need to initialize it here (i.e. attach an IndyCallSite<T> to it)
            if (cs != null)
            {
                if (cs.ics == null)
                {
                    MethodHandleNatives.InitializeCallSite(cs);
                }
                lock (cs.ics)
                {
                    cs.ics.SetTarget(cs.target);
                }
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
            {
                Interlocked.CompareExchange(ref site, ics, curr);
            }
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
        where T : class // Delegate
    {
        internal readonly bool IsBootstrap;
        private volatile T target;

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

#if CLASSGC

		private WeakReference weakRef;

		internal global::java.lang.invoke.MethodType Type
		{
			get { return weakRef == null ? null : (global::java.lang.invoke.MethodType)weakRef.Target; }
		}

		internal bool TrySetType(global::java.lang.invoke.MethodType newType)
		{
			if (weakRef == null)
			{
				return Interlocked.CompareExchange(ref weakRef, new WeakReference(newType), null) == null;
			}
			return Type == newType;
		}
#else
        private global::java.lang.invoke.MethodType type;

        internal global::java.lang.invoke.MethodType Type
        {
            get { return type; }
        }

        internal bool TrySetType(global::java.lang.invoke.MethodType newType)
        {
            Interlocked.CompareExchange(ref type, newType, null);
            return type == newType;
        }

#endif

    }

#endif

    [StructLayout(LayoutKind.Explicit)]
    public struct DoubleConverter
    {
        [FieldOffset(0)]
        private double d;
        [FieldOffset(0)]
        private long l;

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
        private float f;
        [FieldOffset(0)]
        private int i;

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
