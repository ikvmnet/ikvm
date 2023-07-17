/*
  Copyright (C) 2014 Jeroen Frijters

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
using System.Diagnostics;

using IKVM.ByteCode;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;

#endif

namespace IKVM.Runtime
{

    sealed class LambdaMetafactory
    {

        private MethodBuilder getInstance;

        internal static bool Emit(RuntimeByteCodeJavaType.FinishContext context, ClassFile classFile, int constantPoolIndex, ClassFile.ConstantPoolItemInvokeDynamic cpi, CodeEmitter ilgen)
        {
            ClassFile.BootstrapMethod bsm = classFile.GetBootstrapMethod(cpi.BootstrapMethod);
            if (!IsLambdaMetafactory(classFile, bsm) && !IsLambdaAltMetafactory(classFile, bsm))
            {
                return false;
            }
            LambdaMetafactory lmf = context.GetValue<LambdaMetafactory>(constantPoolIndex);
            if (lmf.getInstance == null && !lmf.EmitImpl(context, classFile, cpi, bsm, ilgen))
            {
#if IMPORTER
                if (context.TypeWrapper.GetClassLoader().DisableDynamicBinding)
                {
                    StaticCompiler.IssueMessage(Message.UnableToCreateLambdaFactory);
                }
#endif
                return false;
            }
            ilgen.Emit(OpCodes.Call, lmf.getInstance);
            return true;
        }

        private bool EmitImpl(RuntimeByteCodeJavaType.FinishContext context, ClassFile classFile, ClassFile.ConstantPoolItemInvokeDynamic cpi, ClassFile.BootstrapMethod bsm, CodeEmitter ilgen)
        {
            if (HasUnloadable(cpi))
            {
                Fail("cpi has unloadable");
                return false;
            }
            bool serializable = false;
            var markers = Array.Empty<RuntimeJavaType>();
            ClassFile.ConstantPoolItemMethodType[] bridges = null;
            if (bsm.ArgumentCount > 3)
            {
                AltFlags flags = (AltFlags)classFile.GetConstantPoolConstantInteger(bsm.GetArgument(3));
                serializable = (flags & AltFlags.Serializable) != 0;
                int argpos = 4;
                if ((flags & AltFlags.Markers) != 0)
                {
                    markers = new RuntimeJavaType[classFile.GetConstantPoolConstantInteger(bsm.GetArgument(argpos++))];
                    for (int i = 0; i < markers.Length; i++)
                    {
                        if ((markers[i] = classFile.GetConstantPoolClassType(bsm.GetArgument(argpos++))).IsUnloadable)
                        {
                            Fail("unloadable marker");
                            return false;
                        }
                    }
                }
                if ((flags & AltFlags.Bridges) != 0)
                {
                    bridges = new ClassFile.ConstantPoolItemMethodType[classFile.GetConstantPoolConstantInteger(bsm.GetArgument(argpos++))];
                    for (int i = 0; i < bridges.Length; i++)
                    {
                        bridges[i] = classFile.GetConstantPoolConstantMethodType(bsm.GetArgument(argpos++));
                        if (HasUnloadable(bridges[i]))
                        {
                            Fail("unloadable bridge");
                            return false;
                        }
                    }
                }
            }
            ClassFile.ConstantPoolItemMethodType samMethodType = classFile.GetConstantPoolConstantMethodType(bsm.GetArgument(0));
            ClassFile.ConstantPoolItemMethodHandle implMethod = classFile.GetConstantPoolConstantMethodHandle(bsm.GetArgument(1));
            ClassFile.ConstantPoolItemMethodType instantiatedMethodType = classFile.GetConstantPoolConstantMethodType(bsm.GetArgument(2));
            if (HasUnloadable(samMethodType)
                || HasUnloadable((ClassFile.ConstantPoolItemMI)implMethod.MemberConstantPoolItem)
                || HasUnloadable(instantiatedMethodType))
            {
                Fail("bsm args has unloadable");
                return false;
            }
            RuntimeJavaType interfaceType = cpi.GetRetType();
            RuntimeJavaMethod[] methodList;
            if (!CheckSupportedInterfaces(context.TypeWrapper, interfaceType, markers, bridges, out methodList))
            {
                Fail("unsupported interface");
                return false;
            }
            if (serializable && Array.Exists(methodList, delegate (RuntimeJavaMethod mw) { return mw.Name == "writeReplace" && mw.Signature == "()Ljava.lang.Object;"; }))
            {
                Fail("writeReplace");
                return false;
            }
            if (!IsSupportedImplMethod(implMethod, context.TypeWrapper, cpi.GetArgTypes(), instantiatedMethodType))
            {
                Fail("implMethod " + implMethod.MemberConstantPoolItem.Class + "::" + implMethod.MemberConstantPoolItem.Name + implMethod.MemberConstantPoolItem.Signature);
                return false;
            }
            RuntimeJavaType[] implParameters = GetImplParameters(implMethod);
            CheckConstraints(instantiatedMethodType, samMethodType, cpi.GetArgTypes(), implParameters);
            if (bridges != null)
            {
                foreach (ClassFile.ConstantPoolItemMethodType bridge in bridges)
                {
                    if (bridge.Signature == samMethodType.Signature)
                    {
                        Fail("bridge signature matches sam");
                        return false;
                    }
                    if (!CheckConstraints(instantiatedMethodType, bridge, cpi.GetArgTypes(), implParameters))
                    {
                        Fail("bridge constraints");
                        return false;
                    }
                }
            }
            if (instantiatedMethodType.GetRetType() != RuntimePrimitiveJavaType.VOID)
            {
                RuntimeJavaType Rt = instantiatedMethodType.GetRetType();
                RuntimeJavaType Ra = GetImplReturnType(implMethod);
                if (Ra == RuntimePrimitiveJavaType.VOID || !IsAdaptable(Ra, Rt, true))
                {
                    Fail("The return type Rt is void, or the return type Ra is not void and is adaptable to Rt");
                    return false;
                }
            }
            RuntimeJavaMethod interfaceMethod = null;
            List<RuntimeJavaMethod> methods = new List<RuntimeJavaMethod>();
            foreach (RuntimeJavaMethod mw in methodList)
            {
                if (mw.Name == cpi.Name && mw.Signature == samMethodType.Signature)
                {
                    interfaceMethod = mw;
                    methods.Add(mw);
                }
                else if (mw.IsAbstract && !IsObjectMethod(mw))
                {
                    methods.Add(mw);
                }
            }
            if (interfaceMethod == null || !interfaceMethod.IsAbstract || IsObjectMethod(interfaceMethod) || !MatchSignatures(interfaceMethod, samMethodType))
            {
                Fail("interfaceMethod");
                return false;
            }

            TypeBuilder tb = context.DefineAnonymousClass();
            // we're not implementing the interfaces recursively (because we don't care about .NET Compact anymore),
            // but should we decide to do that, we'd need to somehow communicate to AnonymousTypeWrapper what the 'real' interface is
            tb.AddInterfaceImplementation(interfaceType.TypeAsBaseType);
            if (serializable && Array.IndexOf(markers, CoreClasses.java.io.Serializable.Wrapper) == -1)
            {
                tb.AddInterfaceImplementation(CoreClasses.java.io.Serializable.Wrapper.TypeAsBaseType);
            }
            foreach (RuntimeJavaType marker in markers)
            {
                tb.AddInterfaceImplementation(marker.TypeAsBaseType);
            }
            getInstance = CreateConstructorAndDispatch(context, cpi, tb, methods, implParameters, samMethodType, implMethod, instantiatedMethodType, serializable);
            AddDefaultInterfaceMethods(context, methodList, tb);
            return true;
        }

        [Conditional("TRACE_LAMBDA_METAFACTORY")]
        private static void Fail(string msg)
        {
            Console.WriteLine("Fail: " + msg);
        }

        private static bool CheckConstraints(ClassFile.ConstantPoolItemMethodType instantiatedMethodType, ClassFile.ConstantPoolItemMethodType methodType, RuntimeJavaType[] args, RuntimeJavaType[] implParameters)
        {
            if (!IsSubTypeOf(instantiatedMethodType, methodType))
            {
                Fail("instantiatedMethodType <= methodType");
                return false;
            }
            if (args.Length + methodType.GetArgTypes().Length != implParameters.Length)
            {
                Fail("K + N = M");
                return false;
            }
            for (int i = 0, K = args.Length; i < K; i++)
            {
                if (args[i] == implParameters[i])
                {
                    // ok
                }
                else if (args[i].IsPrimitive || implParameters[i].IsPrimitive || !args[i].IsSubTypeOf(implParameters[i]))
                {
                    Fail("For i=1..K, Di = Ai");
                    return false;
                }
            }
            for (int i = 0, N = methodType.GetArgTypes().Length, k = args.Length; i < N; i++)
            {
                if (!IsAdaptable(instantiatedMethodType.GetArgTypes()[i], implParameters[i + k], false))
                {
                    Fail("For i=1..N, Ti is adaptable to Aj, where j=i+k");
                    return false;
                }
            }
            return true;
        }

        private static RuntimeJavaType[] GetImplParameters(ClassFile.ConstantPoolItemMethodHandle implMethod)
        {
            RuntimeJavaMethod mw = (RuntimeJavaMethod)implMethod.Member;
            RuntimeJavaType[] parameters = mw.GetParameters();
            if (mw.IsStatic || mw.IsConstructor)
            {
                return parameters;
            }
            return ArrayUtil.Concat(mw.DeclaringType, parameters);
        }

        private static RuntimeJavaType GetImplReturnType(ClassFile.ConstantPoolItemMethodHandle implMethod)
        {
            return implMethod.Kind == ReferenceKind.NewInvokeSpecial
                ? implMethod.Member.DeclaringType
                : ((RuntimeJavaMethod)implMethod.Member).ReturnType;
        }

        private static bool IsAdaptable(RuntimeJavaType Q, RuntimeJavaType S, bool isReturn)
        {
            if (Q == S)
            {
                return true;
            }

            if (Q.IsPrimitive)
            {
                if (S.IsPrimitive)
                {
                    // Q can be converted to S via a primitive widening conversion
                    switch (Q.SigName[0] | S.SigName[0] << 8)
                    {
                        case 'B' | 'S' << 8:
                        case 'B' | 'I' << 8:
                        case 'B' | 'J' << 8:
                        case 'B' | 'F' << 8:
                        case 'B' | 'D' << 8:
                        case 'S' | 'I' << 8:
                        case 'S' | 'J' << 8:
                        case 'S' | 'F' << 8:
                        case 'S' | 'D' << 8:
                        case 'C' | 'I' << 8:
                        case 'C' | 'J' << 8:
                        case 'C' | 'F' << 8:
                        case 'C' | 'D' << 8:
                        case 'I' | 'J' << 8:
                        case 'I' | 'F' << 8:
                        case 'I' | 'D' << 8:
                        case 'J' | 'F' << 8:
                        case 'J' | 'D' << 8:
                        case 'F' | 'D' << 8:
                            return true;
                        default:
                            return false;
                    }
                }
                else
                {
                    // S is a supertype of the Wrapper(Q)
                    return GetWrapper(Q).IsAssignableTo(S);
                }
            }
            else if (isReturn)
            {
                return true;
            }
            else
            {
                if (S.IsPrimitive)
                {
                    // If Q is a primitive wrapper, check that Primitive(Q) can be widened to S
                    RuntimeJavaType primitive = GetPrimitiveFromWrapper(Q);
                    return primitive != null && IsAdaptable(primitive, S, isReturn);
                }
                else
                {
                    // for parameter types: S is a supertype of Q
                    return Q.IsAssignableTo(S);
                }
            }
        }

        private static RuntimeJavaType GetWrapper(RuntimeJavaType primitive)
        {
            Debug.Assert(primitive.IsPrimitive);
            switch (primitive.SigName[0])
            {
                case 'Z':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Boolean");
                case 'B':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Byte");
                case 'S':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Short");
                case 'C':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Character");
                case 'I':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Integer");
                case 'J':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Long");
                case 'F':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Float");
                case 'D':
                    return RuntimeClassLoader.LoadClassCritical("java.lang.Double");
                default:
                    throw new InvalidOperationException();
            }
        }

        private static RuntimeJavaType GetPrimitiveFromWrapper(RuntimeJavaType wrapper)
        {
            switch (wrapper.Name)
            {
                case "java.lang.Boolean":
                    return RuntimePrimitiveJavaType.BOOLEAN;
                case "java.lang.Byte":
                    return RuntimePrimitiveJavaType.BYTE;
                case "java.lang.Short":
                    return RuntimePrimitiveJavaType.SHORT;
                case "java.lang.Character":
                    return RuntimePrimitiveJavaType.CHAR;
                case "java.lang.Integer":
                    return RuntimePrimitiveJavaType.INT;
                case "java.lang.Long":
                    return RuntimePrimitiveJavaType.LONG;
                case "java.lang.Float":
                    return RuntimePrimitiveJavaType.FLOAT;
                case "java.lang.Double":
                    return RuntimePrimitiveJavaType.DOUBLE;
                default:
                    return null;
            }
        }

        private static bool IsSubTypeOf(ClassFile.ConstantPoolItemMethodType instantiatedMethodType, ClassFile.ConstantPoolItemMethodType samMethodType)
        {
            RuntimeJavaType[] T = instantiatedMethodType.GetArgTypes();
            RuntimeJavaType[] U = samMethodType.GetArgTypes();
            if (T.Length != U.Length)
            {
                return false;
            }
            for (int i = 0; i < T.Length; i++)
            {
                if (!T[i].IsAssignableTo(U[i]))
                {
                    return false;
                }
            }
            RuntimeJavaType Rt = instantiatedMethodType.GetRetType();
            RuntimeJavaType Ru = samMethodType.GetRetType();
            return Rt.IsAssignableTo(Ru);
        }

        private static MethodBuilder CreateConstructorAndDispatch(RuntimeByteCodeJavaType.FinishContext context, ClassFile.ConstantPoolItemInvokeDynamic cpi, TypeBuilder tb,
            List<RuntimeJavaMethod> methods, RuntimeJavaType[] implParameters, ClassFile.ConstantPoolItemMethodType samMethodType, ClassFile.ConstantPoolItemMethodHandle implMethod,
            ClassFile.ConstantPoolItemMethodType instantiatedMethodType, bool serializable)
        {
            RuntimeJavaType[] args = cpi.GetArgTypes();

            // captured values
            Type[] capturedTypes = new Type[args.Length];
            FieldBuilder[] capturedFields = new FieldBuilder[capturedTypes.Length];
            for (int i = 0; i < capturedTypes.Length; i++)
            {
                capturedTypes[i] = args[i].TypeAsSignatureType;
                FieldAttributes attr = FieldAttributes.Private;
                if (i > 0 || !args[0].IsGhost)
                {
                    attr |= FieldAttributes.InitOnly;
                }
                capturedFields[i] = tb.DefineField("arg$" + (i + 1), capturedTypes[i], attr);
            }

            // constructor
            MethodBuilder ctor = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, capturedTypes);
            CodeEmitter ilgen = CodeEmitter.Create(ctor);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, Types.Object.GetConstructor(Type.EmptyTypes));
            for (int i = 0; i < capturedTypes.Length; i++)
            {
                ilgen.EmitLdarg(0);
                ilgen.EmitLdarg(i + 1);
                ilgen.Emit(OpCodes.Stfld, capturedFields[i]);
            }
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();

            // instance getter
            MethodBuilder getInstance = tb.DefineMethod("__<GetInstance>", MethodAttributes.Assembly | MethodAttributes.Static, cpi.GetRetType().TypeAsBaseType, capturedTypes);
            CodeEmitter ilgenGet = CodeEmitter.Create(getInstance);

            if (capturedTypes.Length == 0)
            {
                // use singleton for lambdas with no captures
                FieldBuilder instField = tb.DefineField("inst", tb, FieldAttributes.Private | FieldAttributes.Static);

                // static constructor
                MethodBuilder cctor = ReflectUtil.DefineTypeInitializer(tb, context.TypeWrapper.GetClassLoader());
                CodeEmitter ilgenCCtor = CodeEmitter.Create(cctor);
                ilgenCCtor.Emit(OpCodes.Newobj, ctor);
                ilgenCCtor.Emit(OpCodes.Stsfld, instField);
                ilgenCCtor.Emit(OpCodes.Ret);
                ilgenCCtor.DoEmit();

                // singleton instance
                ilgenGet.Emit(OpCodes.Ldsfld, instField);
            }
            else
            {
                // new instance
                for (int i = 0; i < capturedTypes.Length; i++)
                    ilgenGet.EmitLdarg(i);
                ilgenGet.Emit(OpCodes.Newobj, ctor);
            }

            // the CLR verification rules about type merging mean we have to explicitly cast to the interface type here
            ilgenGet.Emit(OpCodes.Castclass, cpi.GetRetType().TypeAsBaseType);
            ilgenGet.Emit(OpCodes.Ret);
            ilgenGet.DoEmit();

            // dispatch methods
            foreach (RuntimeJavaMethod mw in methods)
            {
                EmitDispatch(context, args, tb, mw, implParameters, implMethod, instantiatedMethodType, capturedFields);
            }

            // writeReplace method
            if (serializable)
            {
                MethodBuilder writeReplace = tb.DefineMethod("writeReplace", MethodAttributes.Private, Types.Object, Type.EmptyTypes);
                ilgen = CodeEmitter.Create(writeReplace);
                context.TypeWrapper.EmitClassLiteral(ilgen);
                ilgen.Emit(OpCodes.Ldstr, cpi.GetRetType().Name.Replace('.', '/'));
                ilgen.Emit(OpCodes.Ldstr, cpi.Name);
                ilgen.Emit(OpCodes.Ldstr, samMethodType.Signature.Replace('.', '/'));
                ilgen.EmitLdc_I4((int)implMethod.Kind);
                ilgen.Emit(OpCodes.Ldstr, implMethod.Class.Replace('.', '/'));
                ilgen.Emit(OpCodes.Ldstr, implMethod.Name);
                ilgen.Emit(OpCodes.Ldstr, implMethod.Signature.Replace('.', '/'));
                ilgen.Emit(OpCodes.Ldstr, instantiatedMethodType.Signature.Replace('.', '/'));
                ilgen.EmitLdc_I4(capturedFields.Length);
                ilgen.Emit(OpCodes.Newarr, Types.Object);
                for (int i = 0; i < capturedFields.Length; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitLdc_I4(i);
                    ilgen.EmitLdarg(0);
                    ilgen.Emit(OpCodes.Ldfld, capturedFields[i]);
                    if (args[i].IsPrimitive)
                    {
                        Boxer.EmitBox(ilgen, args[i]);
                    }
                    else if (args[i].IsGhost)
                    {
                        args[i].EmitConvSignatureTypeToStackType(ilgen);
                    }
                    ilgen.Emit(OpCodes.Stelem, Types.Object);
                }
                RuntimeJavaMethod ctorSerializedLambda = RuntimeClassLoader.LoadClassCritical("java.lang.invoke.SerializedLambda").GetMethodWrapper(StringConstants.INIT,
                    "(Ljava.lang.Class;Ljava.lang.String;Ljava.lang.String;Ljava.lang.String;ILjava.lang.String;Ljava.lang.String;Ljava.lang.String;Ljava.lang.String;[Ljava.lang.Object;)V", false);
                ctorSerializedLambda.Link();
                ctorSerializedLambda.EmitNewobj(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();

                if (!context.TypeWrapper.GetClassLoader().NoAutomagicSerialization)
                {
                    // add .NET serialization interop support
                    Serialization.MarkSerializable(tb);
                    Serialization.AddGetObjectData(tb);
                }
            }

            return getInstance;
        }

        private static void EmitDispatch(RuntimeByteCodeJavaType.FinishContext context, RuntimeJavaType[] args, TypeBuilder tb, RuntimeJavaMethod interfaceMethod, RuntimeJavaType[] implParameters,
            ClassFile.ConstantPoolItemMethodHandle implMethod, ClassFile.ConstantPoolItemMethodType instantiatedMethodType, FieldBuilder[] capturedFields)
        {
            MethodBuilder mb = interfaceMethod.GetDefineMethodHelper().DefineMethod(context.TypeWrapper, tb, interfaceMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final);
            if (interfaceMethod.Name != interfaceMethod.RealName)
            {
                tb.DefineMethodOverride(mb, (MethodInfo)interfaceMethod.GetMethod());
            }

            AttributeHelper.HideFromJava(mb);

            CodeEmitter ilgen = CodeEmitter.Create(mb);
            for (int i = 0; i < capturedFields.Length; i++)
            {
                ilgen.EmitLdarg(0);
                OpCode opc = OpCodes.Ldfld;
                if (i == 0 && args[0].IsGhost)
                {
                    switch (implMethod.Kind)
                    {
                        case ReferenceKind.InvokeInterface:
                        case ReferenceKind.InvokeVirtual:
                        case ReferenceKind.InvokeSpecial:
                            opc = OpCodes.Ldflda;
                            break;
                    }
                }
                ilgen.Emit(opc, capturedFields[i]);
            }
            for (int i = 0, count = interfaceMethod.GetParameters().Length, k = capturedFields.Length; i < count; i++)
            {
                ilgen.EmitLdarg(i + 1);
                RuntimeJavaType Ui = interfaceMethod.GetParameters()[i];
                RuntimeJavaType Ti = instantiatedMethodType.GetArgTypes()[i];
                RuntimeJavaType Aj = implParameters[i + k];
                if (Ui == RuntimePrimitiveJavaType.BYTE)
                {
                    ilgen.Emit(OpCodes.Conv_I1);
                }
                if (Ti != Ui)
                {
                    if (Ti.IsGhost)
                    {
                        Ti.EmitConvStackTypeToSignatureType(ilgen, Ui);
                    }
                    else if (Ui.IsGhost)
                    {
                        Ui.EmitConvSignatureTypeToStackType(ilgen);
                    }
                    else
                    {
                        Ti.EmitCheckcast(ilgen);
                    }
                }
                if (Ti != Aj)
                {
                    if (Ti.IsPrimitive && !Aj.IsPrimitive)
                    {
                        Boxer.EmitBox(ilgen, Ti);
                    }
                    else if (!Ti.IsPrimitive && Aj.IsPrimitive)
                    {
                        RuntimeJavaType primitive = GetPrimitiveFromWrapper(Ti);
                        Boxer.EmitUnbox(ilgen, primitive, false);
                        if (primitive == RuntimePrimitiveJavaType.BYTE)
                        {
                            ilgen.Emit(OpCodes.Conv_I1);
                        }
                    }
                    else if (Aj == RuntimePrimitiveJavaType.LONG)
                    {
                        ilgen.Emit(OpCodes.Conv_I8);
                    }
                    else if (Aj == RuntimePrimitiveJavaType.FLOAT)
                    {
                        ilgen.Emit(OpCodes.Conv_R4);
                    }
                    else if (Aj == RuntimePrimitiveJavaType.DOUBLE)
                    {
                        ilgen.Emit(OpCodes.Conv_R8);
                    }
                }
            }
            switch (implMethod.Kind)
            {
                case ReferenceKind.InvokeVirtual:
                case ReferenceKind.InvokeInterface:
                    ((RuntimeJavaMethod)implMethod.Member).EmitCallvirt(ilgen);
                    break;
                case ReferenceKind.NewInvokeSpecial:
                    ((RuntimeJavaMethod)implMethod.Member).EmitNewobj(ilgen);
                    break;
                case ReferenceKind.InvokeStatic:
                case ReferenceKind.InvokeSpecial:
                    ((RuntimeJavaMethod)implMethod.Member).EmitCall(ilgen);
                    break;
                default:
                    throw new InvalidOperationException();
            }
            RuntimeJavaType Ru = interfaceMethod.ReturnType;
            RuntimeJavaType Ra = GetImplReturnType(implMethod);
            RuntimeJavaType Rt = instantiatedMethodType.GetRetType();
            if (Ra == RuntimePrimitiveJavaType.BYTE)
            {
                ilgen.Emit(OpCodes.Conv_I1);
            }
            if (Ra != Ru)
            {
                if (Ru == RuntimePrimitiveJavaType.VOID)
                {
                    ilgen.Emit(OpCodes.Pop);
                }
                else if (Ra.IsGhost)
                {
                    Ra.EmitConvSignatureTypeToStackType(ilgen);
                }
                else if (Ru.IsGhost)
                {
                    Ru.EmitConvStackTypeToSignatureType(ilgen, Ra);
                }
            }
            if (Ra != Rt)
            {
                if (Rt.IsPrimitive)
                {
                    if (Rt == RuntimePrimitiveJavaType.VOID)
                    {
                        // already popped
                    }
                    else if (!Ra.IsPrimitive)
                    {
                        RuntimeJavaType primitive = GetPrimitiveFromWrapper(Ra);
                        if (primitive != null)
                        {
                            Boxer.EmitUnbox(ilgen, primitive, false);
                        }
                        else
                        {
                            // If Q is not a primitive wrapper, cast Q to the base Wrapper(S); for example Number for numeric types
                            EmitConvertingUnbox(ilgen, Rt);
                        }
                    }
                    else if (Rt == RuntimePrimitiveJavaType.LONG)
                    {
                        ilgen.Emit(OpCodes.Conv_I8);
                    }
                    else if (Rt == RuntimePrimitiveJavaType.FLOAT)
                    {
                        ilgen.Emit(OpCodes.Conv_R4);
                    }
                    else if (Rt == RuntimePrimitiveJavaType.DOUBLE)
                    {
                        ilgen.Emit(OpCodes.Conv_R8);
                    }
                }
                else if (Ra.IsPrimitive)
                {
                    RuntimeJavaType tw = GetPrimitiveFromWrapper(Rt);
                    if (tw == null)
                    {
                        tw = Ra;
                    }
                    Boxer.EmitBox(ilgen, tw);
                }
                else
                {
                    Rt.EmitCheckcast(ilgen);
                }
            }
            ilgen.EmitTailCallPrevention();
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        private static void EmitConvertingUnbox(CodeEmitter ilgen, RuntimeJavaType tw)
        {
            switch (tw.SigName[0])
            {
                case 'Z':
                case 'C':
                    Boxer.EmitUnbox(ilgen, tw, true);
                    break;
                case 'B':
                    EmitUnboxNumber(ilgen, "byteValue", "()B");
                    break;
                case 'S':
                    EmitUnboxNumber(ilgen, "shortValue", "()S");
                    break;
                case 'I':
                    EmitUnboxNumber(ilgen, "intValue", "()I");
                    break;
                case 'J':
                    EmitUnboxNumber(ilgen, "longValue", "()J");
                    break;
                case 'F':
                    EmitUnboxNumber(ilgen, "floatValue", "()F");
                    break;
                case 'D':
                    EmitUnboxNumber(ilgen, "doubleValue", "()D");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        private static void EmitUnboxNumber(CodeEmitter ilgen, string methodName, string methodSig)
        {
            RuntimeJavaType tw = RuntimeClassLoader.LoadClassCritical("java.lang.Number");
            tw.EmitCheckcast(ilgen);
            RuntimeJavaMethod mw = tw.GetMethodWrapper(methodName, methodSig, false);
            mw.Link();
            mw.EmitCallvirt(ilgen);
        }

        private static void AddDefaultInterfaceMethods(RuntimeByteCodeJavaType.FinishContext context, RuntimeJavaMethod[] methodList, TypeBuilder tb)
        {
            // we use special name to hide these from Java reflection
            const MethodAttributes attr = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName;
            RuntimeJavaTypeFactory factory = context.TypeWrapper.GetClassLoader().GetTypeWrapperFactory();
            foreach (RuntimeJavaMethod mw in methodList)
            {
                if (!mw.IsAbstract)
                {
                    MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(factory, tb, mw.Name, attr);
                    if (mw.Name != mw.RealName)
                    {
                        tb.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
                    }
                    RuntimeByteCodeJavaType.FinishContext.EmitCallDefaultInterfaceMethod(mb, mw);
                }
                else if (IsObjectMethod(mw))
                {
                    MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(factory, tb, mw.Name, attr);
                    if (mw.Name != mw.RealName)
                    {
                        tb.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
                    }
                    CodeEmitter ilgen = CodeEmitter.Create(mb);
                    for (int i = 0, count = mw.GetParameters().Length; i <= count; i++)
                    {
                        ilgen.EmitLdarg(i);
                    }
                    CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, false).EmitCallvirt(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                }
            }
        }

        private static bool IsSupportedImplMethod(ClassFile.ConstantPoolItemMethodHandle implMethod, RuntimeJavaType caller, RuntimeJavaType[] captured, ClassFile.ConstantPoolItemMethodType instantiatedMethodType)
        {
            switch (implMethod.Kind)
            {
                case ReferenceKind.InvokeVirtual:
                case ReferenceKind.InvokeInterface:
                case ReferenceKind.NewInvokeSpecial:
                case ReferenceKind.InvokeStatic:
                case ReferenceKind.InvokeSpecial:
                    break;
                default:
                    return false;
            }
            RuntimeJavaMethod mw = (RuntimeJavaMethod)implMethod.Member;
            if (mw == null || mw.HasCallerID || RuntimeByteCodeJavaType.RequiresDynamicReflectionCallerClass(mw.DeclaringType.Name, mw.Name, mw.Signature))
            {
                return false;
            }
            RuntimeJavaType instance;
            if (mw.IsConstructor)
            {
                instance = mw.DeclaringType;
            }
            else if (mw.IsStatic)
            {
                instance = null;
            }
            else
            {
                // if implMethod is an instance method, the type of the first captured value must be subtype of implMethod.DeclaringType
                instance = captured.Length == 0 ? instantiatedMethodType.GetArgTypes()[0] : captured[0];
                if (!instance.IsAssignableTo(mw.DeclaringType))
                {
                    return false;
                }
            }
            if (!mw.IsAccessibleFrom(mw.DeclaringType, caller, instance))
            {
                return false;
            }
            mw.Link();
            return true;
        }

        private static bool IsSupportedInterface(RuntimeJavaType tw, RuntimeJavaType caller)
        {
            return tw.IsInterface
                && !tw.IsGhost
                && tw.IsAccessibleFrom(caller)
                && !Serialization.IsISerializable(tw);
        }

        private static bool CheckSupportedInterfaces(RuntimeJavaType caller, RuntimeJavaType tw, RuntimeJavaType[] markers, ClassFile.ConstantPoolItemMethodType[] bridges, out RuntimeJavaMethod[] methodList)
        {
            // we don't need to check for unloadable, because we already did that while validating the invoke signature
            if (!IsSupportedInterface(tw, caller))
            {
                methodList = null;
                return false;
            }
            Dictionary<MethodKey, RuntimeJavaMethod> methods = new Dictionary<MethodKey, RuntimeJavaMethod>();
            int abstractMethodCount = 0;
            int bridgeMethodCount = 0;
            if (GatherAllInterfaceMethods(tw, bridges, methods, ref abstractMethodCount, ref bridgeMethodCount) && abstractMethodCount == 1)
            {
                foreach (RuntimeJavaType marker in markers)
                {
                    if (!IsSupportedInterface(marker, caller))
                    {
                        methodList = null;
                        return false;
                    }
                    if (!GatherAllInterfaceMethods(marker, null, methods, ref abstractMethodCount, ref bridgeMethodCount) || abstractMethodCount != 1)
                    {
                        methodList = null;
                        return false;
                    }
                }
                if (bridges != null && bridgeMethodCount != bridges.Length)
                {
                    methodList = null;
                    return false;
                }
                methodList = new RuntimeJavaMethod[methods.Count];
                methods.Values.CopyTo(methodList, 0);
                return true;
            }
            methodList = null;
            return false;
        }

        private static bool GatherAllInterfaceMethods(RuntimeJavaType tw, ClassFile.ConstantPoolItemMethodType[] bridges, Dictionary<MethodKey, RuntimeJavaMethod> methods, ref int abstractMethodCount, ref int bridgeMethodCount)
        {
            foreach (RuntimeJavaMethod mw in tw.GetMethods())
            {
                if (mw.IsVirtual)
                {
                    RuntimeMirandaJavaMethod mmw = mw as RuntimeMirandaJavaMethod;
                    if (mmw != null)
                    {
                        if (mmw.Error != null)
                        {
                            return false;
                        }
                        continue;
                    }
                    MethodKey key = new MethodKey("", mw.Name, mw.Signature);
                    RuntimeJavaMethod current;
                    if (methods.TryGetValue(key, out current))
                    {
                        if (!MatchSignatures(mw, current))
                        {
                            // linkage error (or unloadable type)
                            return false;
                        }
                    }
                    else
                    {
                        methods.Add(key, mw);
                        if (mw.IsAbstract && !IsObjectMethod(mw))
                        {
                            if (bridges != null && IsBridge(mw, bridges))
                            {
                                bridgeMethodCount++;
                            }
                            else
                            {
                                abstractMethodCount++;
                            }
                        }
                    }
                    mw.Link();
                    if (mw.GetMethod() == null)
                    {
                        return false;
                    }
                    if (current != null && mw.RealName != current.RealName)
                    {
                        return false;
                    }
                }
            }
            foreach (RuntimeJavaType tw1 in tw.Interfaces)
            {
                if (!GatherAllInterfaceMethods(tw1, bridges, methods, ref abstractMethodCount, ref bridgeMethodCount))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsBridge(RuntimeJavaMethod mw, ClassFile.ConstantPoolItemMethodType[] bridges)
        {
            foreach (ClassFile.ConstantPoolItemMethodType bridge in bridges)
            {
                if (bridge.Signature == mw.Signature)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsObjectMethod(RuntimeJavaMethod mw)
        {
            RuntimeJavaMethod objectMethod;
            return (objectMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper(mw.Name, mw.Signature, false)) != null
                && objectMethod.IsPublic;
        }

        private static bool MatchSignatures(RuntimeJavaMethod interfaceMethod, ClassFile.ConstantPoolItemMethodType samMethodType)
        {
            return interfaceMethod.ReturnType == samMethodType.GetRetType()
                && MatchTypes(interfaceMethod.GetParameters(), samMethodType.GetArgTypes());
        }

        private static bool MatchSignatures(RuntimeJavaMethod mw1, RuntimeJavaMethod mw2)
        {
            return mw1.ReturnType == mw2.ReturnType
                && MatchTypes(mw1.GetParameters(), mw2.GetParameters());
        }

        private static bool MatchTypes(RuntimeJavaType[] ar1, RuntimeJavaType[] ar2)
        {
            if (ar1.Length != ar2.Length)
            {
                return false;
            }
            for (int i = 0; i < ar1.Length; i++)
            {
                if (ar1[i] != ar2[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsLambdaMetafactory(ClassFile classFile, ClassFile.BootstrapMethod bsm)
        {
            ClassFile.ConstantPoolItemMethodHandle mh;
            return bsm.ArgumentCount == 3
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(0)) == ClassFile.ConstantType.MethodType
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(1)) == ClassFile.ConstantType.MethodHandle
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(2)) == ClassFile.ConstantType.MethodType
                && (mh = classFile.GetConstantPoolConstantMethodHandle(bsm.BootstrapMethodIndex)).Kind == ReferenceKind.InvokeStatic
                && mh.Member != null
                && IsLambdaMetafactory(mh.Member);
        }

        private static bool IsLambdaMetafactory(RuntimeJavaMember mw)
        {
            return mw.Name == "metafactory"
                && mw.Signature == "(Ljava.lang.invoke.MethodHandles$Lookup;Ljava.lang.String;Ljava.lang.invoke.MethodType;Ljava.lang.invoke.MethodType;Ljava.lang.invoke.MethodHandle;Ljava.lang.invoke.MethodType;)Ljava.lang.invoke.CallSite;"
                && mw.DeclaringType.Name == "java.lang.invoke.LambdaMetafactory";
        }

        [Flags]
        enum AltFlags
        {
            Serializable = 1,
            Markers = 2,
            Bridges = 4,
            Mask = Serializable | Markers | Bridges
        }

        private static bool IsLambdaAltMetafactory(ClassFile classFile, ClassFile.BootstrapMethod bsm)
        {
            ClassFile.ConstantPoolItemMethodHandle mh;
            AltFlags flags;
            int argpos = 4;
            return bsm.ArgumentCount >= 4
                && (mh = classFile.GetConstantPoolConstantMethodHandle(bsm.BootstrapMethodIndex)).Kind == ReferenceKind.InvokeStatic
                && mh.Member != null
                && IsLambdaAltMetafactory(mh.Member)
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(0)) == ClassFile.ConstantType.MethodType
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(1)) == ClassFile.ConstantType.MethodHandle
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(2)) == ClassFile.ConstantType.MethodType
                && classFile.GetConstantPoolConstantType(bsm.GetArgument(3)) == ClassFile.ConstantType.Integer
                && ((flags = (AltFlags)classFile.GetConstantPoolConstantInteger(bsm.GetArgument(3))) & ~AltFlags.Mask) == 0
                && ((flags & AltFlags.Markers) == 0 || CheckOptionalArgs(classFile, bsm, ClassFile.ConstantType.Class, ref argpos))
                && ((flags & AltFlags.Bridges) == 0 || CheckOptionalArgs(classFile, bsm, ClassFile.ConstantType.MethodType, ref argpos))
                && argpos == bsm.ArgumentCount;
        }

        private static bool IsLambdaAltMetafactory(RuntimeJavaMember mw)
        {
            return mw.Name == "altMetafactory"
                && mw.Signature == "(Ljava.lang.invoke.MethodHandles$Lookup;Ljava.lang.String;Ljava.lang.invoke.MethodType;[Ljava.lang.Object;)Ljava.lang.invoke.CallSite;"
                && mw.DeclaringType.Name == "java.lang.invoke.LambdaMetafactory";
        }

        private static bool CheckOptionalArgs(ClassFile classFile, ClassFile.BootstrapMethod bsm, ClassFile.ConstantType type, ref int argpos)
        {
            if (bsm.ArgumentCount - argpos < 1)
            {
                return false;
            }
            if (classFile.GetConstantPoolConstantType(bsm.GetArgument(argpos)) != ClassFile.ConstantType.Integer)
            {
                return false;
            }
            int count = classFile.GetConstantPoolConstantInteger(bsm.GetArgument(argpos++));
            if (count < 0 || bsm.ArgumentCount - argpos < count)
            {
                return false;
            }
            for (int i = 0; i < count; i++)
            {
                if (classFile.GetConstantPoolConstantType(bsm.GetArgument(argpos++)) != type)
                {
                    return false;
                }
            }
            return true;
        }

        private static bool HasUnloadable(ClassFile.ConstantPoolItemInvokeDynamic cpi)
        {
            return HasUnloadable(cpi.GetArgTypes()) || cpi.GetRetType().IsUnloadable;
        }

        private static bool HasUnloadable(ClassFile.ConstantPoolItemMethodType cpi)
        {
            return HasUnloadable(cpi.GetArgTypes()) || cpi.GetRetType().IsUnloadable;
        }

        private static bool HasUnloadable(ClassFile.ConstantPoolItemMI cpi)
        {
            return HasUnloadable(cpi.GetArgTypes()) || cpi.GetRetType().IsUnloadable;
        }

        private static bool HasUnloadable(RuntimeJavaType[] wrappers)
        {
            foreach (RuntimeJavaType tw in wrappers)
            {
                if (tw.IsUnloadable)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
