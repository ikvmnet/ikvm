/*
  Copyright (C) 2011-2015 Jeroen Frijters

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
#if IMPORTER == false

using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class MethodHandleUtil
    {

        internal ITypeSymbol GetMemberWrapperDelegateType(global::java.lang.invoke.MethodType type)
        {
#if FIRST_PASS
		    throw new NotImplementedException();
#else
            return GetDelegateTypeForInvokeExact(type.basicType());
#endif
        }

#if FIRST_PASS == false

        Type CreateMethodHandleDelegateType(java.lang.invoke.MethodType type)
        {
            var args = new RuntimeJavaType[type.parameterCount()];
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = RuntimeJavaType.FromClass(type.parameterType(i));
                args[i].Finish();
            }
            var ret = RuntimeJavaType.FromClass(type.returnType());
            ret.Finish();

            return CreateMethodHandleDelegateType(args, ret).GetUnderlyingType();
        }

        static ITypeSymbol[] GetParameterTypes(IMethodBaseSymbol mb)
        {
            var pi = mb.GetParameters();
            var args = new ITypeSymbol[pi.Length];
            for (int i = 0; i < args.Length; i++)
                args[i] = pi[i].ParameterType;

            return args;
        }

        internal static ITypeSymbol[] GetParameterTypes(ITypeSymbol thisType, IMethodBaseSymbol mb)
        {
            var pi = mb.GetParameters();
            var args = new ITypeSymbol[pi.Length + 1];
            args[0] = thisType;
            for (int i = 1; i < args.Length; i++)
                args[i] = pi[i - 1].ParameterType;

            return args;
        }

        internal java.lang.invoke.MethodType GetDelegateMethodType(Type type)
        {
            java.lang.Class[] types;
            var mi = GetDelegateInvokeMethod(context.Resolver.GetSymbol(type));
            var pi = mi.GetParameters();
            if (pi.Length > 0 && IsPackedArgsContainer(pi[pi.Length - 1].ParameterType))
            {
                var list = new System.Collections.Generic.List<java.lang.Class>();
                for (int i = 0; i < pi.Length - 1; i++)
                    list.Add(context.ClassLoaderFactory.GetJavaTypeFromType(pi[i].ParameterType).ClassObject);

                var args = pi[pi.Length - 1].ParameterType.GetGenericArguments();
                while (IsPackedArgsContainer(args[args.Length - 1]))
                {
                    for (int i = 0; i < args.Length - 1; i++)
                        list.Add(context.ClassLoaderFactory.GetJavaTypeFromType(args[i]).ClassObject);

                    args = args[args.Length - 1].GetGenericArguments();
                }

                for (int i = 0; i < args.Length; i++)
                    list.Add(context.ClassLoaderFactory.GetJavaTypeFromType(args[i]).ClassObject);

                types = list.ToArray();
            }
            else
            {
                types = new java.lang.Class[pi.Length];
                for (int i = 0; i < types.Length; i++)
                    types[i] = context.ClassLoaderFactory.GetJavaTypeFromType(pi[i].ParameterType).ClassObject;
            }

            return java.lang.invoke.MethodType.methodType(context.ClassLoaderFactory.GetJavaTypeFromType(mi.ReturnType).ClassObject, types);
        }

        internal sealed class DynamicMethodBuilder
        {

            const string METHOD_HANDLE_FORM_FIELD = "form";
            const string LAMBDA_FORM_VMENTRY_FIELD = "vmentry";
            const string MEMBER_NAME_VMTARGET_FIELD = "vmtarget";

            readonly RuntimeContext context;
            readonly java.lang.invoke.MethodType type;
            readonly int firstArg;
            readonly ITypeSymbol delegateType;
            readonly object firstBoundValue;
            readonly object secondBoundValue;
            readonly ITypeSymbol container;
            readonly DynamicMethod dm;
            readonly CodeEmitter ilgen;
            readonly ITypeSymbol packedArgType;
            readonly int packedArgPos;

            sealed class Container<T1, T2>
            {

                public T1 target;
                public T2 value;

                public Container(T1 target, T2 value)
                {
                    this.target = target;
                    this.value = value;
                }

            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="type"></param>
            /// <param name="container"></param>
            /// <param name="target"></param>
            /// <param name="value"></param>
            /// <param name="owner"></param>
            /// <param name="useBasicTypes"></param>
            /// <exception cref="ArgumentNullException"></exception>
            DynamicMethodBuilder(RuntimeContext context, string name, java.lang.invoke.MethodType type, ITypeSymbol container, object target, object value, Type owner, bool useBasicTypes)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                this.type = type;
                this.container = container;

                delegateType = useBasicTypes ? context.MethodHandleUtil.GetMemberWrapperDelegateType(type) : context.MethodHandleUtil.GetDelegateTypeForInvokeExact(type);
                firstBoundValue = target;
                secondBoundValue = value;

                var mi = context.MethodHandleUtil.GetDelegateInvokeMethod(delegateType);

                ITypeSymbol[] paramTypes;
                if (container != null)
                {
                    firstArg = 1;
                    paramTypes = GetParameterTypes(container, mi);
                }
                else if (target != null)
                {
                    firstArg = 1;
                    paramTypes = GetParameterTypes(context.Resolver.GetSymbol(target.GetType()), mi);
                }
                else
                {
                    paramTypes = GetParameterTypes(mi);
                }

                if (!ReflectUtil.CanOwnDynamicMethod(owner))
                    owner = typeof(DynamicMethodBuilder);

                dm = new DynamicMethod(name, mi.ReturnType.GetUnderlyingType(), paramTypes.GetUnderlyingTypes(), owner, true);
                ilgen = context.CodeEmitterFactory.Create(dm);

                if (type.parameterCount() > MaxArity)
                {
                    var pi = mi.GetParameters();
                    packedArgType = pi[pi.Length - 1].ParameterType;
                    packedArgPos = pi.Length - 1 + firstArg;
                }
                else
                {
                    packedArgPos = int.MaxValue;
                }
            }

            internal static Delegate CreateVoidAdapter(RuntimeContext context, global::java.lang.invoke.MethodType type)
            {
                var dm = new DynamicMethodBuilder(context, "VoidAdapter", type.changeReturnType(global::java.lang.Void.TYPE), null, null, null, null, true);
                var targetDelegateType = context.MethodHandleUtil.GetMemberWrapperDelegateType(type);
                dm.Ldarg(0);
                dm.EmitCheckcast(context.JavaBase.TypeOfJavaLangInvokeMethodHandle);
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(global::java.lang.invoke.MethodHandle)).GetField(METHOD_HANDLE_FORM_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(global::java.lang.invoke.LambdaForm)).GetField(LAMBDA_FORM_VMENTRY_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(global::java.lang.invoke.MemberName)).GetField(MEMBER_NAME_VMTARGET_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                dm.ilgen.Emit(OpCodes.Castclass, targetDelegateType);
                for (int i = 0; i < type.parameterCount(); i++)
                    dm.Ldarg(i);

                dm.CallDelegate(targetDelegateType);
                dm.ilgen.Emit(OpCodes.Pop);

                dm.Ret();
                return dm.CreateDelegate();
            }

            internal static DynamicMethod CreateInvokeExact(RuntimeContext context, global::java.lang.invoke.MethodType type)
            {
                FinishTypes(type);

                var dm = new DynamicMethodBuilder(context, "InvokeExact", type, context.Resolver.GetSymbol(typeof(java.lang.invoke.MethodHandle)), null, null, null, false);
                var targetDelegateType = context.MethodHandleUtil.GetMemberWrapperDelegateType(type.insertParameterTypes(0, context.JavaBase.TypeOfJavaLangInvokeMethodHandle.ClassObject));
                dm.ilgen.Emit(OpCodes.Ldarg_0);
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(global::java.lang.invoke.MethodHandle)).GetField(METHOD_HANDLE_FORM_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(global::java.lang.invoke.LambdaForm)).GetField(LAMBDA_FORM_VMENTRY_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                if (type.returnType() == java.lang.Void.TYPE)
                    dm.ilgen.Emit(OpCodes.Call, context.Resolver.GetSymbol(typeof(MethodHandleUtil)).GetMethod(nameof(GetVoidAdapter), BindingFlags.Static | BindingFlags.NonPublic));
                else
                    dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(java.lang.invoke.MemberName)).GetField(MEMBER_NAME_VMTARGET_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));

                dm.ilgen.Emit(OpCodes.Castclass, targetDelegateType);
                dm.ilgen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < type.parameterCount(); i++)
                {
                    dm.Ldarg(i);
                    var tw = RuntimeJavaType.FromClass(type.parameterType(i));
                    if (tw.IsNonPrimitiveValueType)
                        tw.EmitBox(dm.ilgen);
                    else if (tw.IsGhost)
                        tw.EmitConvSignatureTypeToStackType(dm.ilgen);
                    else if (tw == context.PrimitiveJavaTypeFactory.BYTE)
                        dm.ilgen.Emit(OpCodes.Conv_I1);
                }
                dm.CallDelegate(targetDelegateType);

                var retType = RuntimeJavaType.FromClass(type.returnType());
                if (retType.IsNonPrimitiveValueType)
                    retType.EmitUnbox(dm.ilgen);
                else if (retType.IsGhost)
                    retType.EmitConvStackTypeToSignatureType(dm.ilgen, null);
                else if (!retType.IsPrimitive && retType != context.JavaBase.TypeOfJavaLangObject)
                    dm.EmitCheckcast(retType);

                dm.Ret();
                dm.ilgen.DoEmit();
                return dm.dm;
            }

            internal static Delegate CreateMethodHandleLinkTo(RuntimeContext context, java.lang.invoke.MemberName mn)
            {
                var type = mn.getMethodType();
                var delegateType = context.MethodHandleUtil.GetMemberWrapperDelegateType(type.dropParameterTypes(type.parameterCount() - 1, type.parameterCount()));
                var dm = new DynamicMethodBuilder(context, "DirectMethodHandle." + mn.getName() + type, type, null, null, null, null, true);
                dm.Ldarg(type.parameterCount() - 1);
                dm.ilgen.EmitCastclass(context.Resolver.GetSymbol(typeof(java.lang.invoke.MemberName)));
                dm.ilgen.Emit(OpCodes.Ldfld, context.Resolver.GetSymbol(typeof(java.lang.invoke.MemberName)).GetField(MEMBER_NAME_VMTARGET_FIELD, BindingFlags.Instance | BindingFlags.NonPublic));
                dm.ilgen.Emit(OpCodes.Castclass, delegateType);
                for (int i = 0, count = type.parameterCount() - 1; i < count; i++)
                    dm.Ldarg(i);
                dm.CallDelegate(delegateType);

                dm.Ret();
                return dm.CreateDelegate();
            }

            internal static Delegate CreateMethodHandleInvoke(RuntimeContext context, java.lang.invoke.MemberName mn)
            {
                var type = mn.getMethodType().insertParameterTypes(0, mn.getDeclaringClass());
                var targetDelegateType = context.MethodHandleUtil.GetMemberWrapperDelegateType(type);
                var dm = new DynamicMethodBuilder(context, "DirectMethodHandle." + mn.getName() + type, type, context.Resolver.GetSymbol(typeof(Container<,>)).MakeGenericType(context.Types.Object, context.Resolver.GetSymbol(typeof(IKVM.Runtime.InvokeCache<>)).MakeGenericType(targetDelegateType)), null, null, null, true);
                dm.Ldarg(0);
                dm.EmitCheckcast(context.JavaBase.TypeOfJavaLangInvokeMethodHandle);
                switch (mn.getName())
                {
                    case "invokeExact":
                        dm.Call(context.ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(targetDelegateType));
                        break;
                    case "invoke":
                        dm.LoadValueAddress();
                        dm.Call(context.ByteCodeHelperMethods.GetDelegateForInvoke.MakeGenericMethod(targetDelegateType));
                        break;
                    case "invokeBasic":
                        dm.Call(context.ByteCodeHelperMethods.GetDelegateForInvokeBasic.MakeGenericMethod(targetDelegateType));
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                dm.Ldarg(0);
                for (int i = 1, count = type.parameterCount(); i < count; i++)
                    dm.Ldarg(i);
                dm.CallDelegate(targetDelegateType);

                dm.Ret();
                return dm.CreateDelegate();
            }

            internal static Delegate CreateDynamicOnly(RuntimeContext context, RuntimeJavaMethod mw, java.lang.invoke.MethodType type)
            {
                FinishTypes(type);

                var dm = new DynamicMethodBuilder(context, "CustomInvoke:" + mw.Name, type, null, mw, null, null, true);
                dm.ilgen.Emit(OpCodes.Ldarg_0);
                if (mw.IsStatic)
                {
                    dm.LoadNull();
                    dm.BoxArgs(0);
                }
                else
                {
                    dm.Ldarg(0);
                    dm.BoxArgs(1);
                }
                dm.Callvirt(context.Resolver.GetSymbol(typeof(RuntimeJavaMethod)).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic));
                dm.UnboxReturnValue();

                dm.Ret();
                return dm.CreateDelegate();
            }

            internal static Delegate CreateMemberName(RuntimeContext context, RuntimeJavaMethod mw, global::java.lang.invoke.MethodType type, bool doDispatch)
            {
                FinishTypes(type);

                var tw = mw.DeclaringType;
                var owner = tw.TypeAsBaseType.GetUnderlyingType();
                var dm = new DynamicMethodBuilder(context, "MemberName:" + mw.DeclaringType.Name + "::" + mw.Name + mw.Signature, type, null, mw.HasCallerID ? DynamicCallerIDProvider.Instance : null, null, owner, true);
                for (int i = 0, count = type.parameterCount(); i < count; i++)
                {
                    if (i == 0 && !mw.IsStatic && (tw.IsGhost || tw.IsNonPrimitiveValueType || tw.IsRemapped) && (!mw.IsConstructor || tw != context.JavaBase.TypeOfJavaLangString))
                    {
                        if (tw.IsGhost || tw.IsNonPrimitiveValueType)
                        {
                            dm.LoadFirstArgAddress(tw);
                        }
                        else
                        {
                            Debug.Assert(tw.IsRemapped);

                            // TODO this must be checked
                            dm.Ldarg(0);
                            if (mw.IsConstructor)
                                dm.EmitCastclass(tw.TypeAsBaseType);
                            else if (tw != context.JavaBase.TypeOfCliSystemObject)
                                dm.EmitCheckcast(tw);
                        }
                    }
                    else
                    {
                        dm.Ldarg(i);
                        var argType = RuntimeJavaType.FromClass(type.parameterType(i));
                        if (!argType.IsPrimitive)
                        {
                            if (argType.IsUnloadable)
                            {

                            }
                            else if (argType.IsNonPrimitiveValueType)
                            {
                                dm.Unbox(argType);
                            }
                            else if (argType.IsGhost)
                            {
                                dm.UnboxGhost(argType);
                            }
                            else
                            {
                                dm.EmitCheckcast(argType);
                            }
                        }
                    }
                }

                if (mw.HasCallerID)
                    dm.LoadCallerID();

                // special case for Object.clone() and Object.finalize()
                if (mw.IsFinalizeOrClone)
                {
                    if (doDispatch)
                    {
                        mw.EmitCallvirtReflect(dm.ilgen);
                    }
                    else
                    {
                        // we can re-use the implementations from cli.System.Object (even though the object may not in-fact extend cli.System.Object)
                        context.JavaBase.TypeOfCliSystemObject.GetMethodWrapper(mw.Name, mw.Signature, false).EmitCall(dm.ilgen);
                    }
                }
                else if (doDispatch && !mw.IsStatic)
                {
                    dm.Callvirt(mw);
                }
                else
                {
                    dm.Call(mw);
                }

                var retType = RuntimeJavaType.FromClass(type.returnType());
                if (retType.IsUnloadable)
                {

                }
                else if (retType.IsNonPrimitiveValueType)
                {
                    dm.Box(retType);
                }
                else if (retType.IsGhost)
                {
                    dm.BoxGhost(retType);
                }
                else if (retType == context.PrimitiveJavaTypeFactory.BYTE)
                {
                    dm.CastByte();
                }

                dm.Ret();
                return dm.CreateDelegate();
            }

            internal void Call(IMethodSymbol method)
            {
                ilgen.Emit(OpCodes.Call, method);
            }

            internal void Callvirt(IMethodSymbol method)
            {
                ilgen.Emit(OpCodes.Callvirt, method);
            }

            internal void Call(RuntimeJavaMethod mw)
            {
                mw.EmitCall(ilgen);
            }

            internal void Callvirt(RuntimeJavaMethod mw)
            {
                mw.EmitCallvirt(ilgen);
            }

            internal void CallDelegate(ITypeSymbol delegateType)
            {
                context.MethodHandleUtil.EmitCallDelegateInvokeMethod(ilgen, delegateType);
            }

            internal void LoadFirstArgAddress(RuntimeJavaType tw)
            {
                ilgen.EmitLdarg(0);
                if (tw.IsGhost)
                {
                    tw.EmitConvStackTypeToSignatureType(ilgen, null);
                    var local = ilgen.DeclareLocal(tw.TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Stloc, local);
                    ilgen.Emit(OpCodes.Ldloca, local);
                }
                else if (tw.IsNonPrimitiveValueType)
                {
                    ilgen.Emit(OpCodes.Unbox, tw.TypeAsSignatureType);
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            internal void Ldarg(int i)
            {
                context.MethodHandleUtil.LoadPackedArg(ilgen, i, firstArg, packedArgPos, packedArgType);
            }

            internal void LoadCallerID()
            {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Call, context.ByteCodeHelperMethods.DynamicCallerID);
            }

            internal void LoadValueAddress()
            {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldflda, container.GetField("value"));
            }

            internal void Ret()
            {
                ilgen.Emit(OpCodes.Ret);
            }

            internal Delegate CreateDelegate()
            {
                //Console.WriteLine(delegateType);
                //ilgen.DumpMethod();
                ilgen.DoEmit();
                return ValidateDelegate(firstArg == 0
                    ? dm.CreateDelegate(delegateType.GetUnderlyingType())
                    : dm.CreateDelegate(delegateType.GetUnderlyingType(), container == null ? firstBoundValue : Activator.CreateInstance(container.GetUnderlyingType(), firstBoundValue, secondBoundValue)));
            }

            internal void BoxArgs(int start)
            {
                int paramCount = type.parameterCount();
                ilgen.EmitLdc_I4(paramCount - start);
                ilgen.Emit(OpCodes.Newarr, context.Types.Object);
                for (int i = start; i < paramCount; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitLdc_I4(i - start);
                    Ldarg(i);
                    var tw = RuntimeJavaType.FromClass(type.parameterType(i));
                    if (tw.IsPrimitive)
                        ilgen.Emit(OpCodes.Box, tw.TypeAsSignatureType);

                    ilgen.Emit(OpCodes.Stelem_Ref);
                }
            }

            internal void UnboxReturnValue()
            {
                var tw = RuntimeJavaType.FromClass(type.returnType());
                if (tw == context.PrimitiveJavaTypeFactory.VOID)
                {
                    ilgen.Emit(OpCodes.Pop);
                }
                else if (tw.IsPrimitive)
                {
                    ilgen.Emit(OpCodes.Unbox, tw.TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Ldobj, tw.TypeAsSignatureType);
                }
            }

            internal void LoadNull()
            {
                ilgen.Emit(OpCodes.Ldnull);
            }

            internal void Unbox(RuntimeJavaType tw)
            {
                tw.EmitUnbox(ilgen);
            }

            internal void Box(RuntimeJavaType tw)
            {
                tw.EmitBox(ilgen);
            }

            internal void UnboxGhost(RuntimeJavaType tw)
            {
                tw.EmitConvStackTypeToSignatureType(ilgen, null);
            }

            internal void BoxGhost(RuntimeJavaType tw)
            {
                tw.EmitConvSignatureTypeToStackType(ilgen);
            }

            internal void EmitCheckcast(RuntimeJavaType tw)
            {
                tw.EmitCheckcast(ilgen);
            }

            internal void EmitCastclass(ITypeSymbol type)
            {
                ilgen.EmitCastclass(type);
            }

            internal void EmitWriteLine()
            {
                ilgen.Emit(OpCodes.Call, context.Resolver.GetSymbol(typeof(Console)).GetMethod("WriteLine", [context.Types.Object]));
            }

            internal void CastByte()
            {
                ilgen.Emit(OpCodes.Conv_I1);
            }

            internal void DumpMethod()
            {
                Console.WriteLine(dm.Name + ", type = " + delegateType);
                ilgen.DumpMethod();
            }

            private static void FinishTypes(global::java.lang.invoke.MethodType type)
            {
                // FXBUG(?) DynamicILGenerator doesn't like SymbolType (e.g. an array of a TypeBuilder)
                // so we have to finish the signature types
                RuntimeJavaType.FromClass(type.returnType()).Finish();
                for (int i = 0; i < type.parameterCount(); i++)
                    RuntimeJavaType.FromClass(type.parameterType(i)).Finish();
            }
        }

#if DEBUG
        [System.Security.SecuritySafeCritical]
#endif
        static Delegate ValidateDelegate(Delegate d)
        {
#if DEBUG
            try
            {
                System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(d);
            }
            catch (Exception e)
            {
                throw new InternalException("Delegate failed to JIT", e);
            }
#endif
            return d;
        }

        internal ITypeSymbol GetDelegateTypeForInvokeExact(global::java.lang.invoke.MethodType type)
        {
            type._invokeExactDelegateType ??= CreateMethodHandleDelegateType(type);
            return context.Resolver.GetSymbol(type._invokeExactDelegateType);
        }

        internal T GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandle mh)
            where T : class, Delegate
        {
            var type = mh.type();
            if (mh._invokeExactDelegate == null)
            {
                type._invokeExactDynamicMethod ??= DynamicMethodBuilder.CreateInvokeExact(context, type);
                mh._invokeExactDelegate = type._invokeExactDynamicMethod.CreateDelegate(GetDelegateTypeForInvokeExact(type).GetUnderlyingType(), mh);
                var del = mh._invokeExactDelegate as T;
                if (del != null)
                    return del;
            }

            throw java.lang.invoke.Invokers.newWrongMethodTypeException(GetDelegateMethodType(typeof(T)), type);
        }

        /// <summary>
        /// Called from the InvokeExact DynamicMethod and BytecodeHelper.GetDelegateForInvokeBasic.
        /// </summary>
        /// <param name="mn"></param>
        /// <returns></returns>
        internal static object GetVoidAdapter(java.lang.invoke.MemberName mn)
        {
            var type = mn.getMethodType();
            if (type.voidAdapter == null)
            {
                if (type.returnType() == global::java.lang.Void.TYPE)
                    return mn.vmtarget;

                type.voidAdapter = DynamicMethodBuilder.CreateVoidAdapter(JVM.Context, type);
            }

            return type.voidAdapter;
        }

        internal void LoadPackedArg(CodeEmitter ilgen, int index, int firstArg, int packedArgPos, ITypeSymbol packedArgType)
        {
            index += firstArg;
            if (index >= packedArgPos)
            {
                ilgen.EmitLdarga(packedArgPos);
                var fieldPos = index - packedArgPos;
                var type = packedArgType;
                while (fieldPos >= MaxArity || (fieldPos == MaxArity - 1 && IsPackedArgsContainer(type.GetField("t8").FieldType)))
                {
                    var field = type.GetField("t8");
                    type = field.FieldType;
                    ilgen.Emit(OpCodes.Ldflda, field);
                    fieldPos -= MaxArity - 1;
                }
                ilgen.Emit(OpCodes.Ldfld, type.GetField("t" + (1 + fieldPos)));
            }
            else
            {
                ilgen.EmitLdarg(index);
            }
        }

#endif

    }

}

#endif
