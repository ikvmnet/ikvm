/*
 Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;

using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;
using IKVM.Runtime.Util.Java.Security;

#if !NO_REF_EMIT
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
#endif

namespace IKVM.Java.Externs.sun.reflect
{

    static class ReflectionFactory
    {

#if FIRST_PASS == false

        static SystemAccessor systemAccessor;

        static SystemAccessor SystemAccessor => JVM.Internal.BaseAccessors.Get(ref systemAccessor);

#endif

#if !FIRST_PASS

        static object ConvertPrimitive(RuntimeJavaType tw, object value)
        {
            if (tw == tw.Context.PrimitiveJavaTypeFactory.BOOLEAN)
            {
                if (value is global::java.lang.Boolean boolean)
                    return boolean.booleanValue();
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.BYTE)
            {
                if (value is global::java.lang.Byte @byte)
                    return @byte.byteValue();
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.CHAR)
            {
                if (value is global::java.lang.Character character)
                    return character.charValue();
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.SHORT)
            {
                if (value is global::java.lang.Short || value is global::java.lang.Byte)
                {
                    return ((global::java.lang.Number)value).shortValue();
                }
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.INT)
            {
                if (value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                {
                    return ((global::java.lang.Number)value).intValue();
                }
                else if (value is global::java.lang.Character)
                {
                    return (int)((global::java.lang.Character)value).charValue();
                }
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.LONG)
            {
                if (value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                {
                    return ((global::java.lang.Number)value).longValue();
                }
                else if (value is global::java.lang.Character)
                {
                    return (long)((global::java.lang.Character)value).charValue();
                }
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.FLOAT)
            {
                if (value is global::java.lang.Float || value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                {
                    return ((global::java.lang.Number)value).floatValue();
                }
                else if (value is global::java.lang.Character)
                {
                    return (float)((global::java.lang.Character)value).charValue();
                }
            }
            else if (tw == tw.Context.PrimitiveJavaTypeFactory.DOUBLE)
            {
                if (value is global::java.lang.Double || value is global::java.lang.Float || value is global::java.lang.Long || value is global::java.lang.Integer || value is global::java.lang.Short || value is global::java.lang.Byte)
                {
                    return ((global::java.lang.Number)value).doubleValue();
                }
                else if (value is global::java.lang.Character)
                {
                    return (double)((global::java.lang.Character)value).charValue();
                }
            }
            throw new global::java.lang.IllegalArgumentException();
        }

        static object[] ConvertArgs(RuntimeClassLoader loader, RuntimeJavaType[] argumentTypes, object[] args)
        {
            var nargs = new object[args == null ? 0 : args.Length];
            if (nargs.Length != argumentTypes.Length)
                throw new global::java.lang.IllegalArgumentException("wrong number of arguments");

            for (int i = 0; i < nargs.Length; i++)
            {
                if (argumentTypes[i].IsPrimitive)
                {
                    nargs[i] = ConvertPrimitive(argumentTypes[i], args[i]);
                }
                else
                {
                    if (args[i] != null && !argumentTypes[i].EnsureLoadable(loader).IsInstance(args[i]))
                        throw new global::java.lang.IllegalArgumentException();

                    nargs[i] = argumentTypes[i].GhostWrap(args[i]);
                }
            }

            return nargs;
        }

        sealed class MethodAccessorImpl : global::sun.reflect.MethodAccessor
        {

            readonly RuntimeJavaMethod mw;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="mw"></param>
            internal MethodAccessorImpl(RuntimeJavaMethod mw)
            {
                this.mw = mw ?? throw new ArgumentNullException(nameof(mw));
                mw.Link();
                mw.ResolveMethod();
            }

            [IKVM.Attributes.HideFromJava]
            public object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
            {
                try
                {
                    if (!mw.IsStatic && !mw.DeclaringType.IsInstance(obj))
                    {
                        if (obj == null)
                            throw new global::java.lang.NullPointerException();

                        throw new global::java.lang.IllegalArgumentException("object is not an instance of declaring class");
                    }

                    try
                    {
                        args = ConvertArgs(mw.DeclaringType.ClassLoader, mw.GetParameters(), args);
                    }
                    catch (InvalidCastException e)
                    {
                        throw new global::java.lang.IllegalArgumentException(e);
                    }
                    catch (NullReferenceException e)
                    {
                        throw new global::java.lang.IllegalArgumentException(e);
                    }

                    // if the method is an interface method, we must explicitly run <clinit>,
                    // because .NET reflection doesn't
                    if (mw.DeclaringType.IsInterface)
                        mw.DeclaringType.RunClassInit();

                    if (mw.HasCallerID)
                        args = ArrayUtil.Concat(args, callerID);

                    object retval;
                    try
                    {
                        retval = mw.Invoke(obj, args);
                    }
                    catch (Exception e)
                    {
                        throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(e));
                    }

                    if (mw.ReturnType.IsPrimitive && mw.ReturnType != mw.DeclaringType.Context.PrimitiveJavaTypeFactory.VOID)
                        retval = JVM.Box(retval);
                    else
                        retval = mw.ReturnType.GhostUnwrap(retval);

                    return retval;
                }
                catch (NullReferenceException e)
                {
                    Console.Error.WriteLine("got nre " + e);
                    throw;
                }
                catch (global::java.lang.NullPointerException e)
                {
                    Console.Error.WriteLine("got npe " + e);
                    throw;
                }
            }
        }

        sealed class ConstructorAccessorImpl : global::sun.reflect.ConstructorAccessor
        {

            readonly RuntimeJavaMethod mw;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="mw"></param>
            internal ConstructorAccessorImpl(RuntimeJavaMethod mw)
            {
                this.mw = mw;
                mw.Link();
                mw.ResolveMethod();
            }

            [IKVM.Attributes.HideFromJava]
            public object newInstance(object[] args)
            {
                args = ConvertArgs(mw.DeclaringType.ClassLoader, mw.GetParameters(), args);
                try
                {
                    return mw.CreateInstance(args);
                }
                catch (Exception x)
                {
                    throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(x));
                }
            }
        }

        sealed class SerializationConstructorAccessorImpl : global::sun.reflect.ConstructorAccessor
        {

            readonly RuntimeJavaMethod mw;
            readonly Type type;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="constructorToCall"></param>
            /// <param name="classToInstantiate"></param>
            internal SerializationConstructorAccessorImpl(global::java.lang.reflect.Constructor constructorToCall, global::java.lang.Class classToInstantiate)
            {
                this.type = RuntimeJavaType.FromClass(classToInstantiate).TypeAsBaseType;
                var mw = RuntimeJavaMethod.FromExecutable(constructorToCall);
                if (mw.DeclaringType != JVM.Context.JavaBase.TypeOfJavaLangObject)
                {
                    this.mw = mw;
                    mw.Link();
                    mw.ResolveMethod();
                }
            }

            [IKVM.Attributes.HideFromJava]
            [SecuritySafeCritical]
            public object newInstance(object[] args)
            {
#if NETFRAMEWORK
                var obj = FormatterServices.GetUninitializedObject(type);
#else
                var obj = RuntimeHelpers.GetUninitializedObject(type);
#endif
                if (mw != null)
                    mw.Invoke(obj, ConvertArgs(mw.DeclaringType.ClassLoader, mw.GetParameters(), args));

                return obj;
            }

        }

#if !NO_REF_EMIT

        sealed class BoxUtil
        {

            static readonly MethodInfo valueOfByte = typeof(global::java.lang.Byte).GetMethod("valueOf", new Type[] { typeof(byte) });
            static readonly MethodInfo valueOfBoolean = typeof(global::java.lang.Boolean).GetMethod("valueOf", new Type[] { typeof(bool) });
            static readonly MethodInfo valueOfChar = typeof(global::java.lang.Character).GetMethod("valueOf", new Type[] { typeof(char) });
            static readonly MethodInfo valueOfShort = typeof(global::java.lang.Short).GetMethod("valueOf", new Type[] { typeof(short) });
            static readonly MethodInfo valueOfInt = typeof(global::java.lang.Integer).GetMethod("valueOf", new Type[] { typeof(int) });
            static readonly MethodInfo valueOfFloat = typeof(global::java.lang.Float).GetMethod("valueOf", new Type[] { typeof(float) });
            static readonly MethodInfo valueOfLong = typeof(global::java.lang.Long).GetMethod("valueOf", new Type[] { typeof(long) });
            static readonly MethodInfo valueOfDouble = typeof(global::java.lang.Double).GetMethod("valueOf", new Type[] { typeof(double) });
            static readonly MethodInfo byteValue = typeof(global::java.lang.Byte).GetMethod("byteValue", Type.EmptyTypes);
            static readonly MethodInfo booleanValue = typeof(global::java.lang.Boolean).GetMethod("booleanValue", Type.EmptyTypes);
            static readonly MethodInfo charValue = typeof(global::java.lang.Character).GetMethod("charValue", Type.EmptyTypes);
            static readonly MethodInfo shortValue = typeof(global::java.lang.Short).GetMethod("shortValue", Type.EmptyTypes);
            static readonly MethodInfo intValue = typeof(global::java.lang.Integer).GetMethod("intValue", Type.EmptyTypes);
            static readonly MethodInfo floatValue = typeof(global::java.lang.Float).GetMethod("floatValue", Type.EmptyTypes);
            static readonly MethodInfo longValue = typeof(global::java.lang.Long).GetMethod("longValue", Type.EmptyTypes);
            static readonly MethodInfo doubleValue = typeof(global::java.lang.Double).GetMethod("doubleValue", Type.EmptyTypes);

            internal static void EmitUnboxArg(CodeEmitter ilgen, RuntimeJavaType type)
            {
                if (type == ilgen.Context.PrimitiveJavaTypeFactory.BYTE)
                {
                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Byte));
                    ilgen.Emit(OpCodes.Call, byteValue);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                {
                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Boolean));
                    ilgen.Emit(OpCodes.Call, booleanValue);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.CHAR)
                {
                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Character));
                    ilgen.Emit(OpCodes.Call, charValue);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.SHORT
                    || type == ilgen.Context.PrimitiveJavaTypeFactory.INT
                    || type == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT
                    || type == ilgen.Context.PrimitiveJavaTypeFactory.LONG
                    || type == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Byte));
                    CodeEmitterLabel next = ilgen.DefineLabel();
                    ilgen.EmitBrfalse(next);
                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Byte));
                    ilgen.Emit(OpCodes.Call, byteValue);
                    ilgen.Emit(OpCodes.Conv_I1);
                    Expand(ilgen, type);
                    CodeEmitterLabel done = ilgen.DefineLabel();
                    ilgen.EmitBr(done);
                    ilgen.MarkLabel(next);
                    if (type == ilgen.Context.PrimitiveJavaTypeFactory.SHORT)
                    {
                        ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Short));
                        ilgen.Emit(OpCodes.Call, shortValue);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Dup);
                        ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Short));
                        next = ilgen.DefineLabel();
                        ilgen.EmitBrfalse(next);
                        ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Short));
                        ilgen.Emit(OpCodes.Call, shortValue);
                        Expand(ilgen, type);
                        ilgen.EmitBr(done);
                        ilgen.MarkLabel(next);
                        ilgen.Emit(OpCodes.Dup);
                        ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Character));
                        next = ilgen.DefineLabel();
                        ilgen.EmitBrfalse(next);
                        ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Character));
                        ilgen.Emit(OpCodes.Call, charValue);
                        Expand(ilgen, type);
                        ilgen.EmitBr(done);
                        ilgen.MarkLabel(next);
                        if (type == ilgen.Context.PrimitiveJavaTypeFactory.INT)
                        {
                            ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Integer));
                            ilgen.Emit(OpCodes.Call, intValue);
                        }
                        else
                        {
                            ilgen.Emit(OpCodes.Dup);
                            ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Integer));
                            next = ilgen.DefineLabel();
                            ilgen.EmitBrfalse(next);
                            ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Integer));
                            ilgen.Emit(OpCodes.Call, intValue);
                            Expand(ilgen, type);
                            ilgen.EmitBr(done);
                            ilgen.MarkLabel(next);
                            if (type == ilgen.Context.PrimitiveJavaTypeFactory.LONG)
                            {
                                ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Long));
                                ilgen.Emit(OpCodes.Call, longValue);
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Dup);
                                ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Long));
                                next = ilgen.DefineLabel();
                                ilgen.EmitBrfalse(next);
                                ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Long));
                                ilgen.Emit(OpCodes.Call, longValue);
                                Expand(ilgen, type);
                                ilgen.EmitBr(done);
                                ilgen.MarkLabel(next);
                                if (type == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT)
                                {
                                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Float));
                                    ilgen.Emit(OpCodes.Call, floatValue);
                                }
                                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
                                {
                                    ilgen.Emit(OpCodes.Dup);
                                    ilgen.Emit(OpCodes.Isinst, typeof(global::java.lang.Float));
                                    next = ilgen.DefineLabel();
                                    ilgen.EmitBrfalse(next);
                                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Float));
                                    ilgen.Emit(OpCodes.Call, floatValue);
                                    ilgen.EmitBr(done);
                                    ilgen.MarkLabel(next);
                                    ilgen.Emit(OpCodes.Castclass, typeof(global::java.lang.Double));
                                    ilgen.Emit(OpCodes.Call, doubleValue);
                                }
                                else
                                {
                                    throw new InvalidOperationException();
                                }
                            }
                        }
                    }
                    ilgen.MarkLabel(done);
                }
                else
                {
                    type.EmitCheckcast(ilgen);
                }
            }

            internal static void BoxReturnValue(CodeEmitter ilgen, RuntimeJavaType type)
            {
                if (type == ilgen.Context.PrimitiveJavaTypeFactory.VOID)
                {
                    ilgen.Emit(OpCodes.Ldnull);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.BYTE)
                {
                    ilgen.Emit(OpCodes.Call, valueOfByte);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                {
                    ilgen.Emit(OpCodes.Call, valueOfBoolean);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.CHAR)
                {
                    ilgen.Emit(OpCodes.Call, valueOfChar);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.SHORT)
                {
                    ilgen.Emit(OpCodes.Call, valueOfShort);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.INT)
                {
                    ilgen.Emit(OpCodes.Call, valueOfInt);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT)
                {
                    ilgen.Emit(OpCodes.Call, valueOfFloat);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.LONG)
                {
                    ilgen.Emit(OpCodes.Call, valueOfLong);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    ilgen.Emit(OpCodes.Call, valueOfDouble);
                }
            }

            static void Expand(CodeEmitter ilgen, RuntimeJavaType type)
            {
                if (type == ilgen.Context.PrimitiveJavaTypeFactory.FLOAT)
                {
                    ilgen.Emit(OpCodes.Conv_R4);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.LONG)
                {
                    ilgen.Emit(OpCodes.Conv_I8);
                }
                else if (type == ilgen.Context.PrimitiveJavaTypeFactory.DOUBLE)
                {
                    ilgen.Emit(OpCodes.Conv_R8);
                }
            }
        }

        /// <summary>
        /// Fast implementation of <see cref="global::sun.reflect.MethodAccessor"/> using dynamic methods.
        /// </summary>
        sealed class FastMethodAccessorImpl : global::sun.reflect.MethodAccessor
        {

            internal static readonly ConstructorInfo nullPointerExceptionCtor;
            internal static readonly ConstructorInfo nullPointerExceptionWithMessageCtor;
            internal static readonly ConstructorInfo illegalArgumentExceptionCtor;
            internal static readonly ConstructorInfo illegalArgumentExceptionWithMessageCtor;
            internal static readonly ConstructorInfo illegalArgumentExceptionWithMessageAndCauseCtor;
            internal static readonly ConstructorInfo illegalArgumentExceptionWithCauseCtor;
            internal static readonly ConstructorInfo invocationTargetExceptionWithCauseCtor;

            delegate object Invoker(object obj, object[] args, global::ikvm.@internal.CallerID callerID);
            Invoker invoker;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            static FastMethodAccessorImpl()
            {
                nullPointerExceptionCtor = typeof(global::java.lang.NullPointerException).GetConstructor(Type.EmptyTypes);
                nullPointerExceptionWithMessageCtor = typeof(global::java.lang.NullPointerException).GetConstructor(new[] { typeof(string) });
                illegalArgumentExceptionCtor = typeof(global::java.lang.IllegalArgumentException).GetConstructor(Type.EmptyTypes);
                illegalArgumentExceptionWithMessageCtor = typeof(global::java.lang.IllegalArgumentException).GetConstructor(new[] { typeof(string) });
                illegalArgumentExceptionWithMessageAndCauseCtor = typeof(global::java.lang.IllegalArgumentException).GetConstructor(new[] { typeof(string), typeof(Exception) });
                illegalArgumentExceptionWithCauseCtor = typeof(global::java.lang.IllegalArgumentException).GetConstructor(new[] { typeof(Exception) });
                invocationTargetExceptionWithCauseCtor = typeof(global::java.lang.reflect.InvocationTargetException).GetConstructor(new[] { typeof(Exception) });
            }

            sealed class RunClassInit
            {

                readonly FastMethodAccessorImpl outer;
                readonly RuntimeJavaType tw;
                readonly Invoker invoker;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="outer"></param>
                /// <param name="tw"></param>
                /// <param name="invoker"></param>
                internal RunClassInit(FastMethodAccessorImpl outer, RuntimeJavaType tw, Invoker invoker)
                {
                    this.outer = outer;
                    this.tw = tw;
                    this.invoker = invoker;
                }

                [IKVM.Attributes.HideFromJava]
                internal object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
                {
                    // FXBUG pre-SP1 a DynamicMethod that calls a static method doesn't trigger the cctor, so we do that explicitly.
                    // even on .NET 2.0 SP2, interface method invocations don't run the interface cctor
                    // NOTE when testing, please test both the x86 and x64 CLR JIT, because they have different bugs (even on .NET 2.0 SP2)
                    tw.RunClassInit();
                    outer.invoker = invoker;
                    return invoker(obj, args, callerID);
                }

            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="mw"></param>
            internal unsafe FastMethodAccessorImpl(RuntimeJavaMethod mw)
            {
                try
                {
                    mw.DeclaringType.Finish();
                    var parameters = mw.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.ClassLoader);
                        parameters[i].Finish();
                    }

                    // resolve the runtime method info
                    mw.ResolveMethod();

                    // generate new dynamic method
                    var np = !mw.IsPublic || !mw.DeclaringType.IsPublic;
                    var dm = DynamicMethodUtil.Create($"__<FastMethodAccessor>__{mw.DeclaringType.Name.Replace(".", "_")}__{mw.Name}", mw.DeclaringType.TypeAsBaseType, np, typeof(object), new[] { typeof(object), typeof(object[]), typeof(global::ikvm.@internal.CallerID) });
                    var il = JVM.Context.CodeEmitterFactory.Create(dm);

                    // labels
                    var postLabel = il.DefineLabel();

                    // arguments passed to the method
                    var s = default(CodeEmitterLocal);
                    var n = 0;
                    var p = new CodeEmitterLocal[parameters.Length];

                    // load instance if not static
                    if (mw.IsStatic == false)
                    {
                        // declare variable to hold this instance
                        s = il.AllocTempLocal(mw.DeclaringType.TypeAsSignatureType);

                        // explicit null check for target
                        var endIsNullLabel = il.DefineLabel();
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldnull);
                        il.EmitBne_Un(endIsNullLabel);
                        il.Emit(OpCodes.Ldstr, "object is not an instance of declaring class");
                        il.Emit(OpCodes.Newobj, nullPointerExceptionWithMessageCtor);
                        il.Emit(OpCodes.Throw);
                        il.MarkLabel(endIsNullLabel);

                        // temporary variables
                        var e = il.AllocTempLocal(typeof(Exception));

                        // cast target to appropriate type
                        var endConvSelf = il.DefineLabel();
                        il.BeginExceptionBlock();
                        il.Emit(OpCodes.Ldarg_0);
                        mw.DeclaringType.EmitCheckcast(il);
                        mw.DeclaringType.EmitConvStackTypeToSignatureType(il, null);
                        il.Emit(OpCodes.Stloc, s);
                        il.EmitLeave(endConvSelf);

                        // catch InvalidCastException, store, add message, and wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(InvalidCastException));
                        il.Emit(OpCodes.Stloc, e);
                        il.Emit(OpCodes.Ldstr, "object is not an instance of declaring class");
                        il.Emit(OpCodes.Ldloc, e);
                        il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithMessageAndCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // catch Exception, wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(Exception));
                        il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // end of convert self block
                        il.EndExceptionBlock();
                        il.MarkLabel(endConvSelf);
                        il.ReleaseTempLocal(e);
                    }

                    // where we start converting arguemnts
                    var convArgsLabel = il.DefineLabel();

                    // zero length array may be null
                    if (parameters.Length == 0)
                    {
                        il.Emit(OpCodes.Ldarg_1);
                        il.EmitBrfalse(convArgsLabel);
                    }

                    // check that arguments array is not null
                    var chckArgsLabel = il.DefineLabel();
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldnull);
                    il.EmitBne_Un(chckArgsLabel);
                    il.Emit(OpCodes.Ldstr, "wrong number of arguments");
                    il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithMessageCtor);
                    il.Emit(OpCodes.Throw);

                    // parameters length must match number of parameters on array
                    il.MarkLabel(chckArgsLabel);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldlen);
                    il.EmitLdc_I4(parameters.Length);
                    il.EmitBeq(convArgsLabel);
                    il.Emit(OpCodes.Ldstr, "wrong number of arguments");
                    il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithMessageCtor);
                    il.Emit(OpCodes.Throw);

                    // begin parameter conversion
                    il.MarkLabel(convArgsLabel);

                    // emit conversion code for the remainder of the arguments
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var tw = parameters[i];

                        // declare variable to hold argument
                        var o = p[n++] = il.AllocTempLocal(tw.TypeAsSignatureType);

                        // temporary variable for exceptions
                        var e = il.AllocTempLocal(typeof(Exception));

                        // load and convert argument
                        var endConvArgn = il.DefineLabel();
                        il.BeginExceptionBlock();
                        il.Emit(OpCodes.Ldarg_1);
                        il.EmitLdc_I4(i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        BoxUtil.EmitUnboxArg(il, tw);
                        tw.EmitConvStackTypeToSignatureType(il, null);
                        il.Emit(OpCodes.Stloc, o);
                        il.EmitLeave(endConvArgn);

                        // catch InvalidCastException, store, add message, and wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(InvalidCastException));
                        il.Emit(OpCodes.Stloc, e);
                        il.Emit(OpCodes.Ldstr, $"argument type mismatch on parameter {i}");
                        il.Emit(OpCodes.Ldloc, e);
                        il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithMessageAndCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // catch Exception, wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(Exception));
                        il.Emit(OpCodes.Stloc, e);
                        il.Emit(OpCodes.Ldstr, $"exception on parameter {i}");
                        il.Emit(OpCodes.Ldloc, e);
                        il.Emit(OpCodes.Newobj, illegalArgumentExceptionWithMessageAndCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // end convert
                        il.EndExceptionBlock();
                        il.MarkLabel(endConvArgn);
                        il.ReleaseTempLocal(e);
                    }

                    // storage for return value
                    var rt = il.AllocTempLocal(typeof(object));

                    // call method and convert result
                    il.BeginExceptionBlock();

                    // this instance
                    if (s != null)
                    {
                        // reference for valuetype/ghost
                        if (mw.DeclaringType.IsNonPrimitiveValueType || mw.DeclaringType.IsGhost)
                            il.Emit(OpCodes.Ldloca, s);
                        else
                            il.Emit(OpCodes.Ldloc, s);

                        // finished with s
                        il.ReleaseTempLocal(s);
                    }

                    // load converted arguments
                    for (int i = 0; i < n; i++)
                    {
                        il.Emit(OpCodes.Ldloc, p[i]);
                        il.ReleaseTempLocal(p[i]);
                    }

                    // method requires caller ID passed as final argument
                    if (mw.HasCallerID)
                        il.Emit(OpCodes.Ldarg_2);

                    // static Java method is always a call
                    if (s == null)
                        mw.EmitCall(il);
                    else
                        mw.EmitCallvirtReflect(il); // reflection virt may be static (remapped) or non-static

                    // handle return value
                    mw.ReturnType.EmitConvSignatureTypeToStackType(il);
                    BoxUtil.BoxReturnValue(il, mw.ReturnType);
                    il.Emit(OpCodes.Stloc, rt);
                    il.EmitLeave(postLabel);

                    // catch exception from call and wrap
                    il.BeginCatchBlock(typeof(Exception));
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Call, il.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(il.Context.Types.Exception));
                    il.Emit(OpCodes.Newobj, invocationTargetExceptionWithCauseCtor);
                    il.Emit(OpCodes.Throw);
                    il.EndExceptionBlock();

                    // return from method with last return value
                    il.MarkLabel(postLabel);
                    il.Emit(OpCodes.Ldloc, rt);
                    il.ReleaseTempLocal(rt);
                    il.Emit(OpCodes.Ret);
                    il.DoEmit();

                    // generate invoker
                    invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));

                    // invoker needs to run clinit, wrap in an invoker that does so
                    if ((mw.IsStatic || mw.DeclaringType.IsInterface) && mw.DeclaringType.HasStaticInitializer)
                        invoker = new Invoker(new RunClassInit(this, mw.DeclaringType, invoker).invoke);
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }
            }

            [IKVM.Attributes.HideFromJava]
            public object invoke(object obj, object[] args, global::ikvm.@internal.CallerID callerID)
            {
                try
                {
                    return invoker(obj, args, callerID);
                }
                catch (MethodAccessException x)
                {
                    // this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
                    throw new global::java.lang.IllegalAccessException().initCause(x);
                }
                catch (NullReferenceException e)
                {
                    global::java.lang.System.err.println("got NRE " + e);
                    throw;
                }
            }

        }

        sealed class FastConstructorAccessorImpl : global::sun.reflect.ConstructorAccessor
        {

            delegate object Invoker(object[] args);
            Invoker invoker;

            internal FastConstructorAccessorImpl(global::java.lang.reflect.Constructor constructor)
            {
                try
                {
                    var mw = RuntimeJavaMethod.FromExecutable(constructor);

                    mw.DeclaringType.Finish();
                    var parameters = mw.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i] = parameters[i].EnsureLoadable(mw.DeclaringType.ClassLoader);
                        parameters[i].Finish();
                    }

                    // resolve the runtime method info
                    mw.ResolveMethod();
                    var np = !mw.IsPublic || !mw.DeclaringType.IsPublic;
                    var dm = DynamicMethodUtil.Create($"__<FastConstructorAccessor>__{mw.DeclaringType.Name.Replace(".", "_")}__{mw.Name}", mw.DeclaringType.TypeAsTBD, np, typeof(object), new[] { typeof(object[]) });
                    var il = JVM.Context.CodeEmitterFactory.Create(dm);

                    // labels
                    var postLabel = il.DefineLabel();

                    // local variables for arguments passed to constructor
                    var n = 0;
                    var p = new CodeEmitterLocal[parameters.Length];

                    // where we start converting arguemnts
                    var convArgsLabel = il.DefineLabel();

                    // zero length array may be null
                    if (parameters.Length == 0)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitBrfalse(convArgsLabel);
                    }

                    // check that arguments array is not null
                    var chckArgsLabel = il.DefineLabel();
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldnull);
                    il.EmitBne_Un(chckArgsLabel);
                    il.Emit(OpCodes.Ldstr, "wrong number of arguments");
                    il.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionWithMessageCtor);
                    il.Emit(OpCodes.Throw);

                    // parameters length must match number of parameters on array
                    il.MarkLabel(chckArgsLabel);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldlen);
                    il.EmitLdc_I4(parameters.Length);
                    il.EmitBeq(convArgsLabel);
                    il.Emit(OpCodes.Ldstr, "wrong number of arguments");
                    il.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionWithMessageCtor);
                    il.Emit(OpCodes.Throw);

                    // begin parameter conversion
                    il.MarkLabel(convArgsLabel);

                    // emit conversion code for the remainder of the arguments
                    for (var i = 0; i < parameters.Length; i++)
                    {
                        var tw = parameters[i];

                        // declare variable to hold argument
                        var o = p[n++] = il.AllocTempLocal(tw.TypeAsSignatureType);

                        // temporary variable for exceptions
                        var e = il.AllocTempLocal(typeof(Exception));

                        // load and convert argument
                        var endConvArgn = il.DefineLabel();
                        il.BeginExceptionBlock();
                        il.Emit(OpCodes.Ldarg_0);
                        il.EmitLdc_I4(i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        BoxUtil.EmitUnboxArg(il, tw);
                        tw.EmitConvStackTypeToSignatureType(il, null);
                        il.Emit(OpCodes.Stloc, o);
                        il.EmitLeave(endConvArgn);

                        // catch InvalidCastException, store, add message, and wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(InvalidCastException));
                        il.Emit(OpCodes.Stloc, e);
                        il.Emit(OpCodes.Ldstr, $"argument type mismatch on parameter {i}");
                        il.Emit(OpCodes.Ldloc, e);
                        il.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionWithMessageAndCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // catch Exception, wrap with IllegalArgumentException
                        il.BeginCatchBlock(typeof(Exception));
                        il.Emit(OpCodes.Stloc, e);
                        il.Emit(OpCodes.Ldstr, $"exception on parameter {i}");
                        il.Emit(OpCodes.Ldloc, e);
                        il.Emit(OpCodes.Newobj, FastMethodAccessorImpl.illegalArgumentExceptionWithMessageAndCauseCtor);
                        il.Emit(OpCodes.Throw);

                        // end of convert self block
                        il.EndExceptionBlock();
                        il.MarkLabel(endConvArgn);
                        il.ReleaseTempLocal(e);
                    }

                    // handle exceptions in constructor
                    il.BeginExceptionBlock();

                    // load converted arguments
                    for (int i = 0; i < n; i++)
                    {
                        il.Emit(OpCodes.Ldloc, p[i]);
                        il.ReleaseTempLocal(p[i]);
                    }

                    // call constructor
                    var rt = il.AllocTempLocal(typeof(object));
                    mw.EmitNewobj(il);
                    il.Emit(OpCodes.Stloc, rt);
                    il.EmitLeave(postLabel);

                    // catch exception from call and wrap
                    il.BeginCatchBlock(typeof(Exception));
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Call, il.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(il.Context.Types.Exception));
                    il.Emit(OpCodes.Newobj, FastMethodAccessorImpl.invocationTargetExceptionWithCauseCtor);
                    il.Emit(OpCodes.Throw);
                    il.EndExceptionBlock();

                    // return from method with last return value
                    il.MarkLabel(postLabel);
                    il.Emit(OpCodes.Ldloc, rt);
                    il.ReleaseTempLocal(rt);
                    il.Emit(OpCodes.Ret);
                    il.DoEmit();

                    // generate invoker
                    invoker = (Invoker)dm.CreateDelegate(typeof(Invoker));
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }
            }

            [IKVM.Attributes.HideFromJava]
            public object newInstance(object[] args)
            {
                try
                {
                    return invoker(args);
                }
                catch (MethodAccessException e)
                {
                    // this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
                    throw new global::java.lang.IllegalAccessException().initCause(e);
                }
            }

        }

        sealed class FastSerializationConstructorAccessorImpl : global::sun.reflect.ConstructorAccessor
        {

            static readonly MethodInfo GetTypeFromHandleMethod = typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle), new[] { typeof(RuntimeTypeHandle) });
#if NETFRAMEWORK
            static readonly MethodInfo GetUninitializedObjectMethod = typeof(FormatterServices).GetMethod(nameof(FormatterServices.GetUninitializedObject), new[] { typeof(Type) });
#else
            static readonly MethodInfo GetUninitializedObjectMethod = typeof(RuntimeHelpers).GetMethod(nameof(RuntimeHelpers.GetUninitializedObject), new[] { typeof(Type) });
#endif
            delegate object InvokeCtor();
            InvokeCtor invoker;

            internal FastSerializationConstructorAccessorImpl(global::java.lang.reflect.Constructor constructorToCall, global::java.lang.Class classToInstantiate)
            {
                var constructor = RuntimeJavaMethod.FromExecutable(constructorToCall);
                if (constructor.GetParameters().Length != 0)
                    throw new NotImplementedException("Serialization constructor cannot have parameters");

                constructor.Link();
                constructor.ResolveMethod();

                Type type;
                try
                {
                    var wrapper = RuntimeJavaType.FromClass(classToInstantiate);
                    wrapper.Finish();
                    type = wrapper.TypeAsBaseType;
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }

                DynamicMethod dm = DynamicMethodUtil.Create("__<SerializationCtor>", constructor.DeclaringType.TypeAsBaseType, true, typeof(object), null);
                CodeEmitter ilgen = JVM.Context.CodeEmitterFactory.Create(dm);
                ilgen.Emit(OpCodes.Ldtoken, type);
                ilgen.Emit(OpCodes.Call, GetTypeFromHandleMethod);
                ilgen.Emit(OpCodes.Call, GetUninitializedObjectMethod);
                ilgen.Emit(OpCodes.Dup);
                constructor.EmitCall(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
                invoker = (InvokeCtor)dm.CreateDelegate(typeof(InvokeCtor));
            }

            [IKVM.Attributes.HideFromJava]
            public object newInstance(object[] args)
            {
                try
                {
                    return invoker();
                }
                catch (MethodAccessException x)
                {
                    // this can happen if we're calling a non-public method and the call stack doesn't have ReflectionPermission.MemberAccess
                    throw new global::java.lang.IllegalAccessException().initCause(x);
                }
            }
        }
#endif // !NO_REF_EMIT

        sealed class ActivatorConstructorAccessor : global::sun.reflect.ConstructorAccessor
        {

            private readonly Type type;

            internal ActivatorConstructorAccessor(RuntimeJavaMethod mw)
            {
                this.type = mw.DeclaringType.TypeAsBaseType;
            }

            public object newInstance(object[] objarr)
            {
                if (objarr != null && objarr.Length != 0)
                    throw new global::java.lang.IllegalArgumentException();

                try
                {
                    return Activator.CreateInstance(type);
                }
                catch (TargetInvocationException x)
                {
                    throw new global::java.lang.reflect.InvocationTargetException(global::ikvm.runtime.Util.mapException(x.InnerException));
                }
            }

            internal static bool IsSuitable(RuntimeJavaMethod mw)
            {
                var mb = mw.GetMethod();
                return mb != null
                    && mb.IsConstructor
                    && mb.IsPublic
                    && mb.DeclaringType.IsPublic
                    && mb.DeclaringType == mw.DeclaringType.TypeAsBaseType
                    && mb.GetParameters().Length == 0;
            }

        }

        private abstract class FieldAccessorImplBase : global::sun.reflect.FieldAccessor, IReflectionException
        {

            protected static readonly ushort inflationThreshold = 15;
            protected readonly RuntimeJavaField fw;
            protected readonly bool isFinal;
            protected ushort numInvocations;

            /// <summary>
            /// Initializes the static instance.
            /// </summary>
            static FieldAccessorImplBase()
            {
                if (global::java.security.AccessController.doPrivileged(new FuncPrivilegedAction<string>(() => SystemAccessor.InvokeGetProperty("ikvm.reflect.field.inflationThreshold"))) is string s && ushort.TryParse(s, out var value))
                    inflationThreshold = value;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="fw"></param>
            /// <param name="isFinal"></param>
            FieldAccessorImplBase(RuntimeJavaField fw, bool isFinal)
            {
                this.fw = fw;
                this.isFinal = isFinal;
            }

            private string GetQualifiedFieldName()
            {
                return fw.DeclaringType.Name + "." + fw.Name;
            }

            private string GetFieldTypeName()
            {
                return fw.FieldTypeWrapper.IsPrimitive
                    ? fw.FieldTypeWrapper.ClassObject.getName()
                    : fw.FieldTypeWrapper.Name;
            }

            public global::java.lang.IllegalArgumentException GetIllegalArgumentException(object obj)
            {
                // LAME like JDK 6 we return the wrong exception message (talking about setting the field, instead of getting)
                return SetIllegalArgumentException(obj);
            }

            public global::java.lang.IllegalArgumentException SetIllegalArgumentException(object obj)
            {
                // LAME like JDK 6 we return the wrong exception message (when obj is the object, instead of the value)
                return SetIllegalArgumentException(obj != null ? global::ikvm.runtime.Util.getClassFromObject(obj).getName() : "", "");
            }

            private global::java.lang.IllegalArgumentException SetIllegalArgumentException(string attemptedType, string attemptedValue)
            {
                return new global::java.lang.IllegalArgumentException(GetSetMessage(attemptedType, attemptedValue));
            }

            protected global::java.lang.IllegalAccessException FinalFieldIllegalAccessException(object obj)
            {
                return FinalFieldIllegalAccessException(obj != null ? global::ikvm.runtime.Util.getClassFromObject(obj).getName() : "", "");
            }

            private global::java.lang.IllegalAccessException FinalFieldIllegalAccessException(string attemptedType, string attemptedValue)
            {
                return new global::java.lang.IllegalAccessException(GetSetMessage(attemptedType, attemptedValue));
            }

            private global::java.lang.IllegalArgumentException GetIllegalArgumentException(string type)
            {
                return new global::java.lang.IllegalArgumentException("Attempt to get " + GetFieldTypeName() + " field \"" + GetQualifiedFieldName() + "\" with illegal data type conversion to " + type);
            }

            // this message comes from global::sun.reflect.UnsafeFieldAccessorImpl
            private string GetSetMessage(String attemptedType, String attemptedValue)
            {
                String err = "Can not set";
                if (fw.IsStatic)
                    err += " static";
                if (isFinal)
                    err += " final";
                err += " " + GetFieldTypeName() + " field " + GetQualifiedFieldName() + " to ";
                if (attemptedValue.Length > 0)
                {
                    err += "(" + attemptedType + ")" + attemptedValue;
                }
                else
                {
                    if (attemptedType.Length > 0)
                        err += attemptedType;
                    else
                        err += "null value";
                }
                return err;
            }

            public virtual bool getBoolean(object obj)
            {
                throw GetIllegalArgumentException("boolean");
            }

            public virtual byte getByte(object obj)
            {
                throw GetIllegalArgumentException("byte");
            }

            public virtual char getChar(object obj)
            {
                throw GetIllegalArgumentException("char");
            }

            public virtual short getShort(object obj)
            {
                throw GetIllegalArgumentException("short");
            }

            public virtual int getInt(object obj)
            {
                throw GetIllegalArgumentException("int");
            }

            public virtual long getLong(object obj)
            {
                throw GetIllegalArgumentException("long");
            }

            public virtual float getFloat(object obj)
            {
                throw GetIllegalArgumentException("float");
            }

            public virtual double getDouble(object obj)
            {
                throw GetIllegalArgumentException("double");
            }

            public virtual void setBoolean(object obj, bool z)
            {
                throw SetIllegalArgumentException("boolean", global::java.lang.Boolean.toString(z));
            }

            public virtual void setByte(object obj, byte b)
            {
                throw SetIllegalArgumentException("byte", global::java.lang.Byte.toString(b));
            }

            public virtual void setChar(object obj, char c)
            {
                throw SetIllegalArgumentException("char", global::java.lang.Character.toString(c));
            }

            public virtual void setShort(object obj, short s)
            {
                throw SetIllegalArgumentException("short", global::java.lang.Short.toString(s));
            }

            public virtual void setInt(object obj, int i)
            {
                throw SetIllegalArgumentException("int", global::java.lang.Integer.toString(i));
            }

            public virtual void setLong(object obj, long l)
            {
                throw SetIllegalArgumentException("long", global::java.lang.Long.toString(l));
            }

            public virtual void setFloat(object obj, float f)
            {
                throw SetIllegalArgumentException("float", global::java.lang.Float.toString(f));
            }

            public virtual void setDouble(object obj, double d)
            {
                throw SetIllegalArgumentException("double", global::java.lang.Double.toString(d));
            }

            public abstract object get(object obj);
            public abstract void set(object obj, object value);

            private abstract class FieldAccessor<T> : FieldAccessorImplBase
            {

                static readonly RuntimeContext context = JVM.Context;

                protected delegate void Setter(object obj, T value, FieldAccessor<T> acc);
                protected delegate T Getter(object obj, FieldAccessor<T> acc);
                private static readonly Setter initialSetter = lazySet;
                private static readonly Getter initialGetter = lazyGet;
                protected Setter setter = initialSetter;
                protected Getter getter = initialGetter;

                internal FieldAccessor(RuntimeJavaField fw, bool isFinal)
                    : base(fw, isFinal)
                {
                    if (!IsSlowPathCompatible(fw))
                    {
                        // prevent slow path
                        numInvocations = inflationThreshold;
                    }
                }

                private bool IsSlowPathCompatible(RuntimeJavaField fw)
                {
#if !NO_REF_EMIT
                    if (fw.IsVolatile && (fw.FieldTypeWrapper == context.PrimitiveJavaTypeFactory.LONG || fw.FieldTypeWrapper == context.PrimitiveJavaTypeFactory.DOUBLE))
                    {
                        return false;
                    }
#endif
                    fw.Link();
                    return true;
                }

                private static T lazyGet(object obj, FieldAccessor<T> acc)
                {
                    return acc.lazyGet(obj);
                }

                private static void lazySet(object obj, T value, FieldAccessor<T> acc)
                {
                    acc.lazySet(obj, value);
                }

                private T lazyGet(object obj)
                {
#if !NO_REF_EMIT
                    if (numInvocations >= inflationThreshold)
                    {
                        // FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
                        // and if we didn't use the slow path, we haven't yet initialized the class
                        fw.DeclaringType.RunClassInit();
                        getter = (Getter)GenerateFastGetter(typeof(Getter), typeof(T), fw);
                        return getter(obj, this);
                    }
#endif // !NO_REF_EMIT
                    if (fw.IsStatic)
                    {
                        obj = null;
                    }
                    else if (obj == null)
                    {
                        throw new global::java.lang.NullPointerException();
                    }
                    else if (!fw.DeclaringType.IsInstance(obj))
                    {
                        throw GetIllegalArgumentException(obj);
                    }
                    else if (fw.DeclaringType.IsRemapped && !fw.DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
                    {
                        throw GetUnsupportedRemappedFieldException(obj);
                    }
                    if (numInvocations == 0)
                    {
                        fw.DeclaringType.RunClassInit();
                        fw.DeclaringType.Finish();
                        fw.ResolveField();
                    }
                    numInvocations++;
                    return (T)fw.FieldTypeWrapper.GhostUnwrap(fw.GetValue(obj));
                }

                private void lazySet(object obj, T value)
                {
                    if (isFinal)
                    {
                        // for some reason Java runs class initialization before checking if the field is final
                        fw.DeclaringType.RunClassInit();
                        throw FinalFieldIllegalAccessException(JavaBox(value));
                    }
#if !NO_REF_EMIT
                    if (numInvocations >= inflationThreshold)
                    {
                        // FXBUG it appears that a ldsfld/stsfld in a DynamicMethod doesn't trigger the class constructor
                        // and if we didn't use the slow path, we haven't yet initialized the class
                        fw.DeclaringType.RunClassInit();
                        setter = (Setter)GenerateFastSetter(typeof(Setter), typeof(T), fw);
                        setter(obj, value, this);
                        return;
                    }
#endif // !NO_REF_EMIT
                    if (fw.IsStatic)
                    {
                        obj = null;
                    }
                    else if (obj == null)
                    {
                        throw new global::java.lang.NullPointerException();
                    }
                    else if (!fw.DeclaringType.IsInstance(obj))
                    {
                        throw SetIllegalArgumentException(obj);
                    }
                    else if (fw.DeclaringType.IsRemapped && !fw.DeclaringType.TypeAsBaseType.IsInstanceOfType(obj))
                    {
                        throw GetUnsupportedRemappedFieldException(obj);
                    }
                    CheckValue(value);
                    if (numInvocations == 0)
                    {
                        fw.DeclaringType.RunClassInit();
                        fw.DeclaringType.Finish();
                        fw.ResolveField();
                    }
                    numInvocations++;
                    fw.SetValue(obj, fw.FieldTypeWrapper.GhostWrap(value));
                }

                private Exception GetUnsupportedRemappedFieldException(object obj)
                {
                    return new global::java.lang.IllegalAccessException("Accessing field " + fw.DeclaringType.Name + "." + fw.Name + " in an object of type " + global::ikvm.runtime.Util.getClassFromObject(obj).getName() + " is not supported");
                }

                protected virtual void CheckValue(T value)
                {
                }

                protected abstract object JavaBox(T value);
            }

            private sealed class ByteField : FieldAccessor<byte>
            {
                internal ByteField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override short getShort(object obj)
                {
                    return (sbyte)getByte(obj);
                }

                public sealed override int getInt(object obj)
                {
                    return (sbyte)getByte(obj);
                }

                public sealed override long getLong(object obj)
                {
                    return (sbyte)getByte(obj);
                }

                public sealed override float getFloat(object obj)
                {
                    return (sbyte)getByte(obj);
                }

                public sealed override double getDouble(object obj)
                {
                    return (sbyte)getByte(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Byte.valueOf(getByte(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (!(val is global::java.lang.Byte))
                    {
                        throw SetIllegalArgumentException(val);
                    }
                    setByte(obj, ((global::java.lang.Byte)val).byteValue());
                }

                public sealed override byte getByte(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setByte(object obj, byte value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(byte value)
                {
                    return global::java.lang.Byte.valueOf(value);
                }
            }

            private sealed class BooleanField : FieldAccessor<bool>
            {
                internal BooleanField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Boolean.valueOf(getBoolean(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (!(val is global::java.lang.Boolean))
                    {
                        throw SetIllegalArgumentException(val);
                    }
                    setBoolean(obj, ((global::java.lang.Boolean)val).booleanValue());
                }

                public sealed override bool getBoolean(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setBoolean(object obj, bool value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(bool value)
                {
                    return global::java.lang.Boolean.valueOf(value);
                }
            }

            private sealed class CharField : FieldAccessor<char>
            {
                internal CharField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override int getInt(object obj)
                {
                    return getChar(obj);
                }

                public sealed override long getLong(object obj)
                {
                    return getChar(obj);
                }

                public sealed override float getFloat(object obj)
                {
                    return getChar(obj);
                }

                public sealed override double getDouble(object obj)
                {
                    return getChar(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Character.valueOf(getChar(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Character)
                        setChar(obj, ((global::java.lang.Character)val).charValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override char getChar(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setChar(object obj, char value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(char value)
                {
                    return global::java.lang.Character.valueOf(value);
                }
            }

            private sealed class ShortField : FieldAccessor<short>
            {
                internal ShortField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override int getInt(object obj)
                {
                    return getShort(obj);
                }

                public sealed override long getLong(object obj)
                {
                    return getShort(obj);
                }

                public sealed override float getFloat(object obj)
                {
                    return getShort(obj);
                }

                public sealed override double getDouble(object obj)
                {
                    return getShort(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Short.valueOf(getShort(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Byte
                        || val is global::java.lang.Short)
                        setShort(obj, ((global::java.lang.Number)val).shortValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override void setByte(object obj, byte b)
                {
                    setShort(obj, (sbyte)b);
                }

                public sealed override short getShort(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setShort(object obj, short value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(short value)
                {
                    return global::java.lang.Short.valueOf(value);
                }
            }

            private sealed class IntField : FieldAccessor<int>
            {
                internal IntField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override long getLong(object obj)
                {
                    return getInt(obj);
                }

                public sealed override float getFloat(object obj)
                {
                    return getInt(obj);
                }

                public sealed override double getDouble(object obj)
                {
                    return getInt(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Integer.valueOf(getInt(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Byte
                        || val is global::java.lang.Short
                        || val is global::java.lang.Integer)
                        setInt(obj, ((global::java.lang.Number)val).intValue());
                    else if (val is global::java.lang.Character)
                        setInt(obj, ((global::java.lang.Character)val).charValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override void setByte(object obj, byte b)
                {
                    setInt(obj, (sbyte)b);
                }

                public sealed override void setChar(object obj, char c)
                {
                    setInt(obj, c);
                }

                public sealed override void setShort(object obj, short s)
                {
                    setInt(obj, s);
                }

                public sealed override int getInt(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setInt(object obj, int value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(int value)
                {
                    return global::java.lang.Integer.valueOf(value);
                }
            }

            private sealed class FloatField : FieldAccessor<float>
            {
                internal FloatField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override double getDouble(object obj)
                {
                    return getFloat(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Float.valueOf(getFloat(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Float
                        || val is global::java.lang.Byte
                        || val is global::java.lang.Short
                        || val is global::java.lang.Integer
                        || val is global::java.lang.Long)
                        setFloat(obj, ((global::java.lang.Number)val).floatValue());
                    else if (val is global::java.lang.Character)
                        setFloat(obj, ((global::java.lang.Character)val).charValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override void setByte(object obj, byte b)
                {
                    setFloat(obj, (sbyte)b);
                }

                public sealed override void setChar(object obj, char c)
                {
                    setFloat(obj, c);
                }

                public sealed override void setShort(object obj, short s)
                {
                    setFloat(obj, s);
                }

                public sealed override void setInt(object obj, int i)
                {
                    setFloat(obj, i);
                }

                public sealed override void setLong(object obj, long l)
                {
                    setFloat(obj, l);
                }

                public sealed override float getFloat(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setFloat(object obj, float value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(float value)
                {
                    return global::java.lang.Float.valueOf(value);
                }
            }

            private sealed class LongField : FieldAccessor<long>
            {
                internal LongField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override float getFloat(object obj)
                {
                    return getLong(obj);
                }

                public sealed override double getDouble(object obj)
                {
                    return getLong(obj);
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Long.valueOf(getLong(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Long
                        || val is global::java.lang.Byte
                        || val is global::java.lang.Short
                        || val is global::java.lang.Integer)
                        setLong(obj, ((global::java.lang.Number)val).longValue());
                    else if (val is global::java.lang.Character)
                        setLong(obj, ((global::java.lang.Character)val).charValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override void setByte(object obj, byte b)
                {
                    setLong(obj, (sbyte)b);
                }

                public sealed override void setChar(object obj, char c)
                {
                    setLong(obj, c);
                }

                public sealed override void setShort(object obj, short s)
                {
                    setLong(obj, s);
                }

                public sealed override void setInt(object obj, int i)
                {
                    setLong(obj, i);
                }

                public sealed override long getLong(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setLong(object obj, long value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(long value)
                {
                    return global::java.lang.Long.valueOf(value);
                }
            }

            private sealed class DoubleField : FieldAccessor<double>
            {
                internal DoubleField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                public sealed override object get(object obj)
                {
                    return global::java.lang.Double.valueOf(getDouble(obj));
                }

                public sealed override void set(object obj, object val)
                {
                    if (val is global::java.lang.Double
                        || val is global::java.lang.Float
                        || val is global::java.lang.Byte
                        || val is global::java.lang.Short
                        || val is global::java.lang.Integer
                        || val is global::java.lang.Long)
                        setDouble(obj, ((global::java.lang.Number)val).doubleValue());
                    else if (val is global::java.lang.Character)
                        setDouble(obj, ((global::java.lang.Character)val).charValue());
                    else
                        throw SetIllegalArgumentException(val);
                }

                public sealed override void setByte(object obj, byte b)
                {
                    setDouble(obj, (sbyte)b);
                }

                public sealed override void setChar(object obj, char c)
                {
                    setDouble(obj, c);
                }

                public sealed override void setShort(object obj, short s)
                {
                    setDouble(obj, s);
                }

                public sealed override void setInt(object obj, int i)
                {
                    setDouble(obj, i);
                }

                public sealed override void setLong(object obj, long l)
                {
                    setDouble(obj, l);
                }

                public sealed override void setFloat(object obj, float f)
                {
                    setDouble(obj, f);
                }

                public sealed override double getDouble(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void setDouble(object obj, double value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(double value)
                {
                    return global::java.lang.Double.valueOf(value);
                }
            }

            private sealed class ObjectField : FieldAccessor<object>
            {
                internal ObjectField(RuntimeJavaField field, bool isFinal)
                    : base(field, isFinal)
                {
                }

                protected sealed override void CheckValue(object value)
                {
                    if (value != null && !fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.ClassLoader).IsInstance(value))
                    {
                        throw SetIllegalArgumentException(value);
                    }
                }

                public sealed override object get(object obj)
                {
                    try
                    {
                        return getter(obj, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                public sealed override void set(object obj, object value)
                {
                    try
                    {
                        setter(obj, value, this);
                    }
                    catch (FieldAccessException x)
                    {
                        throw new global::java.lang.IllegalAccessException().initCause(x);
                    }
                }

                protected sealed override object JavaBox(object value)
                {
                    return value;
                }
            }

#if !NO_REF_EMIT
            private Delegate GenerateFastGetter(Type delegateType, Type fieldType, RuntimeJavaField fw)
            {
                RuntimeJavaType fieldTypeWrapper;
                try
                {
                    fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.ClassLoader);
                    fieldTypeWrapper.Finish();
                    fw.DeclaringType.Finish();
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }

                fw.ResolveField();
                DynamicMethod dm = DynamicMethodUtil.Create("__<Getter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, fieldType, new Type[] { typeof(IReflectionException), typeof(object), typeof(object) });
                CodeEmitter ilgen = JVM.Context.CodeEmitterFactory.Create(dm);
                if (fw.IsStatic)
                {
                    fw.EmitGet(ilgen);
                    fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
                }
                else
                {
                    ilgen.BeginExceptionBlock();
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
                    fw.EmitGet(ilgen);
                    fieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
                    CodeEmitterLocal local = ilgen.DeclareLocal(fieldType);
                    ilgen.Emit(OpCodes.Stloc, local);
                    CodeEmitterLabel label = ilgen.DefineLabel();
                    ilgen.EmitLeave(label);
                    ilgen.BeginCatchBlock(typeof(InvalidCastException));
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("GetIllegalArgumentException"));
                    ilgen.Emit(OpCodes.Throw);
                    ilgen.EndExceptionBlock();
                    ilgen.MarkLabel(label);
                    ilgen.Emit(OpCodes.Ldloc, local);
                }
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
                return dm.CreateDelegate(delegateType, this);
            }

            private Delegate GenerateFastSetter(Type delegateType, Type fieldType, RuntimeJavaField fw)
            {
                RuntimeJavaType fieldTypeWrapper;
                try
                {
                    fieldTypeWrapper = fw.FieldTypeWrapper.EnsureLoadable(fw.DeclaringType.ClassLoader);
                    fieldTypeWrapper.Finish();
                    fw.DeclaringType.Finish();
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }

                fw.ResolveField();
                DynamicMethod dm = DynamicMethodUtil.Create("__<Setter>", fw.DeclaringType.TypeAsBaseType, !fw.IsPublic || !fw.DeclaringType.IsPublic, null, new Type[] { typeof(IReflectionException), typeof(object), fieldType, typeof(object) });
                CodeEmitter ilgen = JVM.Context.CodeEmitterFactory.Create(dm);
                if (fw.IsStatic)
                {
                    if (fieldType == typeof(object))
                    {
                        ilgen.BeginExceptionBlock();
                        ilgen.Emit(OpCodes.Ldarg_2);
                        fieldTypeWrapper.EmitCheckcast(ilgen);
                        fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
                        fw.EmitSet(ilgen);
                        CodeEmitterLabel label = ilgen.DefineLabel();
                        ilgen.EmitLeave(label);
                        ilgen.BeginCatchBlock(typeof(InvalidCastException));
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Ldarg_1);
                        ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
                        ilgen.Emit(OpCodes.Throw);
                        ilgen.EndExceptionBlock();
                        ilgen.MarkLabel(label);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Ldarg_2);
                        fw.EmitSet(ilgen);
                    }
                }
                else
                {
                    ilgen.BeginExceptionBlock();
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Castclass, fw.DeclaringType.TypeAsBaseType);
                    ilgen.Emit(OpCodes.Ldarg_2);
                    if (fieldType == typeof(object))
                    {
                        fieldTypeWrapper.EmitCheckcast(ilgen);
                    }
                    fieldTypeWrapper.EmitConvStackTypeToSignatureType(ilgen, null);
                    fw.EmitSet(ilgen);
                    CodeEmitterLabel label = ilgen.DefineLabel();
                    ilgen.EmitLeave(label);
                    ilgen.BeginCatchBlock(typeof(InvalidCastException));
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Callvirt, typeof(IReflectionException).GetMethod("SetIllegalArgumentException"));
                    ilgen.Emit(OpCodes.Throw);
                    ilgen.EndExceptionBlock();
                    ilgen.MarkLabel(label);
                }
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
                return dm.CreateDelegate(delegateType, this);
            }
#endif // !NO_REF_EMIT

            internal static FieldAccessorImplBase Create(RuntimeJavaField field, bool isFinal)
            {
                RuntimeJavaType type = field.FieldTypeWrapper;
                if (type.IsPrimitive)
                {
                    if (type == type.Context.PrimitiveJavaTypeFactory.BYTE)
                    {
                        return new ByteField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                    {
                        return new BooleanField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.CHAR)
                    {
                        return new CharField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.SHORT)
                    {
                        return new ShortField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.INT)
                    {
                        return new IntField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.FLOAT)
                    {
                        return new FloatField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.LONG)
                    {
                        return new LongField(field, isFinal);
                    }
                    if (type == type.Context.PrimitiveJavaTypeFactory.DOUBLE)
                    {
                        return new DoubleField(field, isFinal);
                    }
                    throw new InvalidOperationException("field type: " + type);
                }
                else
                {
                    return new ObjectField(field, isFinal);
                }
            }
        }
#endif

        public static global::sun.reflect.FieldAccessor newFieldAccessor(object thisFactory, global::java.lang.reflect.Field field, bool overrideAccessCheck)
        {
#if FIRST_PASS
            return null;
#else
            // we look at the modifiers of the Field object to allow Unsafe to give us a fake Field take doesn't have the final flag set
            int modifiers = field.getModifiers();
            bool isStatic = global::java.lang.reflect.Modifier.isStatic(modifiers);
            bool isFinal = global::java.lang.reflect.Modifier.isFinal(modifiers);
            return FieldAccessorImplBase.Create(RuntimeJavaField.FromField(field), isFinal && (!overrideAccessCheck || isStatic));
#endif
        }

#if !FIRST_PASS
        internal static global::sun.reflect.FieldAccessor NewFieldAccessorJNI(RuntimeJavaField field)
        {
            return FieldAccessorImplBase.Create(field, false);
        }
#endif

        public static global::sun.reflect.MethodAccessor newMethodAccessor(object thisFactory, global::java.lang.reflect.Method method)
        {
#if FIRST_PASS
            return null;
#else
            RuntimeJavaMethod mw = RuntimeJavaMethod.FromExecutable(method);
#if !NO_REF_EMIT
            if (!mw.IsDynamicOnly)
            {
                return new FastMethodAccessorImpl(mw);
            }
#endif
            return new MethodAccessorImpl(mw);
#endif
        }

        public static global::sun.reflect.ConstructorAccessor newConstructorAccessor0(object thisFactory, global::java.lang.reflect.Constructor constructor)
        {
#if FIRST_PASS
            return null;
#else
            RuntimeJavaMethod mw = RuntimeJavaMethod.FromExecutable(constructor);
            if (ActivatorConstructorAccessor.IsSuitable(mw))
            {
                // we special case public default constructors, because in that case using Activator.CreateInstance()
                // is almost as fast as FastConstructorAccessorImpl, but it saves us significantly in working set and
                // startup time (because often during startup a sun.nio.cs.* encoder is instantiated using reflection)
                return new ActivatorConstructorAccessor(mw);
            }
            else
            {
#if NO_REF_EMIT
			return new ConstructorAccessorImpl(mw);
#else
                return new FastConstructorAccessorImpl(constructor);
#endif
            }
#endif
        }

        public static global::sun.reflect.ConstructorAccessor newConstructorAccessorForSerialization(global::java.lang.Class classToInstantiate, global::java.lang.reflect.Constructor constructorToCall)
        {
#if FIRST_PASS
            return null;
#else
            try
            {
#if NO_REF_EMIT
			return new SerializationConstructorAccessorImpl(constructorToCall, classToInstantiate);
#else
                return new FastSerializationConstructorAccessorImpl(constructorToCall, classToInstantiate);
#endif
            }
            catch (SecurityException x)
            {
                throw new global::java.lang.SecurityException(x.Message, global::ikvm.runtime.Util.mapException(x));
            }
#endif
        }

    }

}
