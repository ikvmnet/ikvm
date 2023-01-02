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
#if !IMPORTER
using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.Internal;
using IKVM.Runtime;

static partial class MethodHandleUtil
{

    internal static Type GetMemberWrapperDelegateType(global::java.lang.invoke.MethodType type)
    {
#if FIRST_PASS
		return null;
#else
        return GetDelegateTypeForInvokeExact(type.basicType());
#endif
    }

#if !FIRST_PASS
    private static Type CreateMethodHandleDelegateType(java.lang.invoke.MethodType type)
    {
        TypeWrapper[] args = new TypeWrapper[type.parameterCount()];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = TypeWrapper.FromClass(type.parameterType(i));
            args[i].Finish();
        }
        TypeWrapper ret = TypeWrapper.FromClass(type.returnType());
        ret.Finish();
        return CreateMethodHandleDelegateType(args, ret);
    }

    private static Type[] GetParameterTypes(MethodBase mb)
    {
        ParameterInfo[] pi = mb.GetParameters();
        Type[] args = new Type[pi.Length];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = pi[i].ParameterType;
        }
        return args;
    }

    internal static Type[] GetParameterTypes(Type thisType, MethodBase mb)
    {
        ParameterInfo[] pi = mb.GetParameters();
        Type[] args = new Type[pi.Length + 1];
        args[0] = thisType;
        for (int i = 1; i < args.Length; i++)
        {
            args[i] = pi[i - 1].ParameterType;
        }
        return args;
    }

    internal static java.lang.invoke.MethodType GetDelegateMethodType(Type type)
    {
        java.lang.Class[] types;
        MethodInfo mi = GetDelegateInvokeMethod(type);
        ParameterInfo[] pi = mi.GetParameters();
        if (pi.Length > 0 && IsPackedArgsContainer(pi[pi.Length - 1].ParameterType))
        {
            System.Collections.Generic.List<java.lang.Class> list = new System.Collections.Generic.List<java.lang.Class>();
            for (int i = 0; i < pi.Length - 1; i++)
            {
                list.Add(ClassLoaderWrapper.GetWrapperFromType(pi[i].ParameterType).ClassObject);
            }
            Type[] args = pi[pi.Length - 1].ParameterType.GetGenericArguments();
            while (IsPackedArgsContainer(args[args.Length - 1]))
            {
                for (int i = 0; i < args.Length - 1; i++)
                {
                    list.Add(ClassLoaderWrapper.GetWrapperFromType(args[i]).ClassObject);
                }
                args = args[args.Length - 1].GetGenericArguments();
            }
            for (int i = 0; i < args.Length; i++)
            {
                list.Add(ClassLoaderWrapper.GetWrapperFromType(args[i]).ClassObject);
            }
            types = list.ToArray();
        }
        else
        {
            types = new java.lang.Class[pi.Length];
            for (int i = 0; i < types.Length; i++)
            {
                types[i] = ClassLoaderWrapper.GetWrapperFromType(pi[i].ParameterType).ClassObject;
            }
        }
        return java.lang.invoke.MethodType.methodType(ClassLoaderWrapper.GetWrapperFromType(mi.ReturnType).ClassObject, types);
    }

    internal sealed class DynamicMethodBuilder
    {
        private readonly java.lang.invoke.MethodType type;
        private readonly int firstArg;
        private readonly Type delegateType;
        private readonly object firstBoundValue;
        private readonly object secondBoundValue;
        private readonly Type container;
        private readonly DynamicMethod dm;
        private readonly CodeEmitter ilgen;
        private readonly Type packedArgType;
        private readonly int packedArgPos;

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

        private DynamicMethodBuilder(string name, java.lang.invoke.MethodType type, Type container, object target, object value, Type owner, bool useBasicTypes)
        {
            this.type = type;
            this.delegateType = useBasicTypes ? GetMemberWrapperDelegateType(type) : GetDelegateTypeForInvokeExact(type);
            this.firstBoundValue = target;
            this.secondBoundValue = value;
            this.container = container;
            MethodInfo mi = GetDelegateInvokeMethod(delegateType);
            Type[] paramTypes;
            if (container != null)
            {
                this.firstArg = 1;
                paramTypes = GetParameterTypes(container, mi);
            }
            else if (target != null)
            {
                this.firstArg = 1;
                paramTypes = GetParameterTypes(target.GetType(), mi);
            }
            else
            {
                paramTypes = GetParameterTypes(mi);
            }
            if (!ReflectUtil.CanOwnDynamicMethod(owner))
            {
                owner = typeof(DynamicMethodBuilder);
            }
            this.dm = new DynamicMethod(name, mi.ReturnType, paramTypes, owner, true);
            this.ilgen = CodeEmitter.Create(dm);

            if (type.parameterCount() > MaxArity)
            {
                ParameterInfo[] pi = mi.GetParameters();
                this.packedArgType = pi[pi.Length - 1].ParameterType;
                this.packedArgPos = pi.Length - 1 + firstArg;
            }
            else
            {
                this.packedArgPos = Int32.MaxValue;
            }
        }

        internal static Delegate CreateVoidAdapter(global::java.lang.invoke.MethodType type)
        {
            DynamicMethodBuilder dm = new DynamicMethodBuilder("VoidAdapter", type.changeReturnType(global::java.lang.Void.TYPE), null, null, null, null, true);
            Type targetDelegateType = GetMemberWrapperDelegateType(type);
            dm.Ldarg(0);
            dm.EmitCheckcast(CoreClasses.java.lang.invoke.MethodHandle.Wrapper);
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.MethodHandle).GetField("form", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.LambdaForm).GetField("vmentry", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.MemberName).GetField("vmtarget", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.ilgen.Emit(OpCodes.Castclass, targetDelegateType);
            for (int i = 0; i < type.parameterCount(); i++)
            {
                dm.Ldarg(i);
            }
            dm.CallDelegate(targetDelegateType);
            dm.ilgen.Emit(OpCodes.Pop);
            dm.Ret();
            return dm.CreateDelegate();
        }

        internal static DynamicMethod CreateInvokeExact(global::java.lang.invoke.MethodType type)
        {
            FinishTypes(type);
            DynamicMethodBuilder dm = new DynamicMethodBuilder("InvokeExact", type, typeof(java.lang.invoke.MethodHandle), null, null, null, false);
            Type targetDelegateType = GetMemberWrapperDelegateType(type.insertParameterTypes(0, CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject));
            dm.ilgen.Emit(OpCodes.Ldarg_0);
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.MethodHandle).GetField("form", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(global::java.lang.invoke.LambdaForm).GetField("vmentry", BindingFlags.Instance | BindingFlags.NonPublic));
            if (type.returnType() == java.lang.Void.TYPE)
            {
                dm.ilgen.Emit(OpCodes.Call, typeof(MethodHandleUtil).GetMethod("GetVoidAdapter", BindingFlags.Static | BindingFlags.NonPublic));
            }
            else
            {
                dm.ilgen.Emit(OpCodes.Ldfld, typeof(java.lang.invoke.MemberName).GetField("vmtarget", BindingFlags.Instance | BindingFlags.NonPublic));
            }
            dm.ilgen.Emit(OpCodes.Castclass, targetDelegateType);
            dm.ilgen.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < type.parameterCount(); i++)
            {
                dm.Ldarg(i);
                TypeWrapper tw = TypeWrapper.FromClass(type.parameterType(i));
                if (tw.IsNonPrimitiveValueType)
                {
                    tw.EmitBox(dm.ilgen);
                }
                else if (tw.IsGhost)
                {
                    tw.EmitConvSignatureTypeToStackType(dm.ilgen);
                }
                else if (tw == PrimitiveTypeWrapper.BYTE)
                {
                    dm.ilgen.Emit(OpCodes.Conv_I1);
                }
            }
            dm.CallDelegate(targetDelegateType);
            TypeWrapper retType = TypeWrapper.FromClass(type.returnType());
            if (retType.IsNonPrimitiveValueType)
            {
                retType.EmitUnbox(dm.ilgen);
            }
            else if (retType.IsGhost)
            {
                retType.EmitConvStackTypeToSignatureType(dm.ilgen, null);
            }
            else if (!retType.IsPrimitive && retType != CoreClasses.java.lang.Object.Wrapper)
            {
                dm.EmitCheckcast(retType);
            }
            dm.Ret();
            dm.ilgen.DoEmit();
            return dm.dm;
        }

        internal static Delegate CreateMethodHandleLinkTo(java.lang.invoke.MemberName mn)
        {
            java.lang.invoke.MethodType type = mn.getMethodType();
            Type delegateType = MethodHandleUtil.GetMemberWrapperDelegateType(type.dropParameterTypes(type.parameterCount() - 1, type.parameterCount()));
            DynamicMethodBuilder dm = new DynamicMethodBuilder("DirectMethodHandle." + mn.getName() + type, type, null, null, null, null, true);
            dm.Ldarg(type.parameterCount() - 1);
            dm.ilgen.EmitCastclass(typeof(java.lang.invoke.MemberName));
            dm.ilgen.Emit(OpCodes.Ldfld, typeof(java.lang.invoke.MemberName).GetField("vmtarget", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.ilgen.Emit(OpCodes.Castclass, delegateType);
            for (int i = 0, count = type.parameterCount() - 1; i < count; i++)
            {
                dm.Ldarg(i);
            }
            dm.CallDelegate(delegateType);
            dm.Ret();
            return dm.CreateDelegate();
        }

        internal static Delegate CreateMethodHandleInvoke(java.lang.invoke.MemberName mn)
        {
            java.lang.invoke.MethodType type = mn.getMethodType().insertParameterTypes(0, mn.getDeclaringClass());
            Type targetDelegateType = MethodHandleUtil.GetMemberWrapperDelegateType(type);
            DynamicMethodBuilder dm = new DynamicMethodBuilder("DirectMethodHandle." + mn.getName() + type, type,
                typeof(Container<,>).MakeGenericType(typeof(object), typeof(IKVM.Runtime.InvokeCache<>).MakeGenericType(targetDelegateType)), null, null, null, true);
            dm.Ldarg(0);
            dm.EmitCheckcast(CoreClasses.java.lang.invoke.MethodHandle.Wrapper);
            switch (mn.getName())
            {
                case "invokeExact":
                    dm.Call(ByteCodeHelperMethods.GetDelegateForInvokeExact.MakeGenericMethod(targetDelegateType));
                    break;
                case "invoke":
                    dm.LoadValueAddress();
                    dm.Call(ByteCodeHelperMethods.GetDelegateForInvoke.MakeGenericMethod(targetDelegateType));
                    break;
                case "invokeBasic":
                    dm.Call(ByteCodeHelperMethods.GetDelegateForInvokeBasic.MakeGenericMethod(targetDelegateType));
                    break;
                default:
                    throw new InvalidOperationException();
            }
            dm.Ldarg(0);
            for (int i = 1, count = type.parameterCount(); i < count; i++)
            {
                dm.Ldarg(i);
            }
            dm.CallDelegate(targetDelegateType);
            dm.Ret();
            return dm.CreateDelegate();
        }

        internal static Delegate CreateDynamicOnly(MethodWrapper mw, java.lang.invoke.MethodType type)
        {
            FinishTypes(type);
            DynamicMethodBuilder dm = new DynamicMethodBuilder("CustomInvoke:" + mw.Name, type, null, mw, null, null, true);
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
            dm.Callvirt(typeof(MethodWrapper).GetMethod("Invoke", BindingFlags.Instance | BindingFlags.NonPublic));
            dm.UnboxReturnValue();
            dm.Ret();
            return dm.CreateDelegate();
        }

        internal static Delegate CreateMemberName(MethodWrapper mw, global::java.lang.invoke.MethodType type, bool doDispatch)
        {
            FinishTypes(type);
            TypeWrapper tw = mw.DeclaringType;
            Type owner = tw.TypeAsBaseType;
#if NET_4_0
			if (!doDispatch && !mw.IsStatic)
			{
				// on .NET 4 we can only do a non-virtual invocation of a virtual method if we skip verification,
				// and to skip verification we need to inject the dynamic method in a critical assembly

				// TODO instead of injecting in mscorlib, we should use DynamicMethodUtils.Create()
				owner = typeof(object);
			}
#endif
            DynamicMethodBuilder dm = new DynamicMethodBuilder("MemberName:" + mw.DeclaringType.Name + "::" + mw.Name + mw.Signature, type, null,
                mw.HasCallerID ? DynamicCallerIDProvider.Instance : null, null, owner, true);
            for (int i = 0, count = type.parameterCount(); i < count; i++)
            {
                if (i == 0 && !mw.IsStatic && (tw.IsGhost || tw.IsNonPrimitiveValueType || tw.IsRemapped) && (!mw.IsConstructor || tw != CoreClasses.java.lang.String.Wrapper))
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
                        {
                            dm.EmitCastclass(tw.TypeAsBaseType);
                        }
                        else if (tw != CoreClasses.cli.System.Object.Wrapper)
                        {
                            dm.EmitCheckcast(tw);
                        }
                    }
                }
                else
                {
                    dm.Ldarg(i);
                    TypeWrapper argType = TypeWrapper.FromClass(type.parameterType(i));
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
            {
                dm.LoadCallerID();
            }
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
                    CoreClasses.cli.System.Object.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, false).EmitCall(dm.ilgen);
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
            TypeWrapper retType = TypeWrapper.FromClass(type.returnType());
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
            else if (retType == PrimitiveTypeWrapper.BYTE)
            {
                dm.CastByte();
            }
            dm.Ret();
            return dm.CreateDelegate();
        }

        internal void Call(MethodInfo method)
        {
            ilgen.Emit(OpCodes.Call, method);
        }

        internal void Callvirt(MethodInfo method)
        {
            ilgen.Emit(OpCodes.Callvirt, method);
        }

        internal void Call(MethodWrapper mw)
        {
            mw.EmitCall(ilgen);
        }

        internal void Callvirt(MethodWrapper mw)
        {
            mw.EmitCallvirt(ilgen);
        }

        internal void CallDelegate(Type delegateType)
        {
            EmitCallDelegateInvokeMethod(ilgen, delegateType);
        }

        internal void LoadFirstArgAddress(TypeWrapper tw)
        {
            ilgen.EmitLdarg(0);
            if (tw.IsGhost)
            {
                tw.EmitConvStackTypeToSignatureType(ilgen, null);
                CodeEmitterLocal local = ilgen.DeclareLocal(tw.TypeAsSignatureType);
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
            LoadPackedArg(ilgen, i, firstArg, packedArgPos, packedArgType);
        }

        internal void LoadCallerID()
        {
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCallerID);
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
                ? dm.CreateDelegate(delegateType)
                : dm.CreateDelegate(delegateType, container == null ? firstBoundValue : Activator.CreateInstance(container, firstBoundValue, secondBoundValue)));
        }

        internal void BoxArgs(int start)
        {
            int paramCount = type.parameterCount();
            ilgen.EmitLdc_I4(paramCount - start);
            ilgen.Emit(OpCodes.Newarr, Types.Object);
            for (int i = start; i < paramCount; i++)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.EmitLdc_I4(i - start);
                Ldarg(i);
                TypeWrapper tw = TypeWrapper.FromClass(type.parameterType(i));
                if (tw.IsPrimitive)
                {
                    ilgen.Emit(OpCodes.Box, tw.TypeAsSignatureType);
                }
                ilgen.Emit(OpCodes.Stelem_Ref);
            }
        }

        internal void UnboxReturnValue()
        {
            TypeWrapper tw = TypeWrapper.FromClass(type.returnType());
            if (tw == PrimitiveTypeWrapper.VOID)
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

        internal void Unbox(TypeWrapper tw)
        {
            tw.EmitUnbox(ilgen);
        }

        internal void Box(TypeWrapper tw)
        {
            tw.EmitBox(ilgen);
        }

        internal void UnboxGhost(TypeWrapper tw)
        {
            tw.EmitConvStackTypeToSignatureType(ilgen, null);
        }

        internal void BoxGhost(TypeWrapper tw)
        {
            tw.EmitConvSignatureTypeToStackType(ilgen);
        }

        internal void EmitCheckcast(TypeWrapper tw)
        {
            tw.EmitCheckcast(ilgen);
        }

        internal void EmitCastclass(Type type)
        {
            ilgen.EmitCastclass(type);
        }

        internal void EmitWriteLine()
        {
            ilgen.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new Type[] { typeof(object) }));
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
            TypeWrapper.FromClass(type.returnType()).Finish();
            for (int i = 0; i < type.parameterCount(); i++)
            {
                TypeWrapper.FromClass(type.parameterType(i)).Finish();
            }
        }
    }

#if DEBUG
    [System.Security.SecuritySafeCritical]
#endif
    private static Delegate ValidateDelegate(Delegate d)
    {
#if DEBUG
        try
        {
            System.Runtime.CompilerServices.RuntimeHelpers.PrepareDelegate(d);
        }
        catch (Exception x)
        {
            throw new InternalException("Delegate failed to JIT", x);
        }
#endif
        return d;
    }

    internal static Type GetDelegateTypeForInvokeExact(global::java.lang.invoke.MethodType type)
    {
        if (type._invokeExactDelegateType == null)
        {
            type._invokeExactDelegateType = CreateMethodHandleDelegateType(type);
        }
        return type._invokeExactDelegateType;
    }

    internal static T GetDelegateForInvokeExact<T>(global::java.lang.invoke.MethodHandle mh)
        where T : class
    {
        global::java.lang.invoke.MethodType type = mh.type();
        if (mh._invokeExactDelegate == null)
        {
            if (type._invokeExactDynamicMethod == null)
            {
                type._invokeExactDynamicMethod = DynamicMethodBuilder.CreateInvokeExact(type);
            }
            mh._invokeExactDelegate = type._invokeExactDynamicMethod.CreateDelegate(GetDelegateTypeForInvokeExact(type), mh);
            T del = mh._invokeExactDelegate as T;
            if (del != null)
            {
                return del;
            }
        }
        throw java.lang.invoke.Invokers.newWrongMethodTypeException(GetDelegateMethodType(typeof(T)), type);
    }

    // called from InvokeExact DynamicMethod and ByteCodeHelper.GetDelegateForInvokeBasic()
    internal static object GetVoidAdapter(java.lang.invoke.MemberName mn)
    {
        global::java.lang.invoke.MethodType type = mn.getMethodType();
        if (type.voidAdapter == null)
        {
            if (type.returnType() == global::java.lang.Void.TYPE)
            {
                return mn.vmtarget;
            }
            type.voidAdapter = DynamicMethodBuilder.CreateVoidAdapter(type);
        }
        return type.voidAdapter;
    }

    internal static void LoadPackedArg(CodeEmitter ilgen, int index, int firstArg, int packedArgPos, Type packedArgType)
    {
        index += firstArg;
        if (index >= packedArgPos)
        {
            ilgen.EmitLdarga(packedArgPos);
            int fieldPos = index - packedArgPos;
            Type type = packedArgType;
            while (fieldPos >= MaxArity || (fieldPos == MaxArity - 1 && IsPackedArgsContainer(type.GetField("t8").FieldType)))
            {
                FieldInfo field = type.GetField("t8");
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

#endif
