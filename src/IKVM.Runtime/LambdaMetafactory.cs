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
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;
using IKVM.ByteCode.Decoding;


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

    /// <summary>
    /// IL based implementation of InvokeDynamic. Supports LambdaMetafactory specific boot strap methods.
    /// </summary>
    sealed class LambdaMetafactory
    {

        MethodBuilder getInstance;

        /// <summary>
        /// Emits the required IL for the invokedynamic constant item.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="classFile"></param>
        /// <param name="constantPoolIndex"></param>
        /// <param name="cpi"></param>
        /// <param name="ilgen"></param>
        /// <returns></returns>
        internal static bool Emit(RuntimeByteCodeJavaType.FinishContext context, ClassFile classFile, int constantPoolIndex, ConstantPoolItemInvokeDynamic cpi, CodeEmitter ilgen)
        {
            var bsm = classFile.GetBootstrapMethod(cpi.BootstrapMethod);

            // only support emitting IL for LambdaMetafactory bootstrap methods
            if (!IsLambdaMetafactory(classFile, bsm) && !IsLambdaAltMetafactory(classFile, bsm))
                return false;

            // we associate a single LambdaMetafactory instance for each dynamic invoke constant
            var lmf = context.GetValue<LambdaMetafactory>(constantPoolIndex);
            if (lmf.getInstance == null && !lmf.EmitImpl(context, classFile, cpi, bsm, ilgen))
            {
#if IMPORTER
                if (context.TypeWrapper.ClassLoader.DisableDynamicBinding)
                    context.TypeWrapper.ClassLoader.Diagnostics.UnableToCreateLambdaFactory();
#endif
                return false;
            }

            // call getInstance at the call site
            ilgen.Emit(OpCodes.Call, lmf.getInstance);
            return true;
        }

        /// <summary>
        /// Emits the getInstance and supporting infrastructure.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="classFile"></param>
        /// <param name="cpi"></param>
        /// <param name="bsm"></param>
        /// <param name="ilgen"></param>
        /// <returns></returns>
        bool EmitImpl(RuntimeByteCodeJavaType.FinishContext context, ClassFile classFile, ConstantPoolItemInvokeDynamic cpi, BootstrapMethod bsm, CodeEmitter ilgen)
        {
            if (HasUnloadable(cpi))
            {
                Fail("cpi has unloadable");
                return false;
            }

            var serializable = false;
            var markers = Array.Empty<RuntimeJavaType>();
            ConstantPoolItemMethodType[] bridges = null;

            // argument count >3 indicates altMetafactory call
            // scan for flags, markers and bridges
            if (bsm.Arguments.Count > 3)
            {
                var flags = (AltFlags)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)bsm.Arguments[3]);
                serializable = (flags & AltFlags.Serializable) != 0;
                int argpos = 4;

                if ((flags & AltFlags.Markers) != 0)
                {
                    markers = new RuntimeJavaType[classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)bsm.Arguments[argpos++])];
                    for (int i = 0; i < markers.Length; i++)
                    {
                        if ((markers[i] = classFile.GetConstantPoolClassType((ClassConstantHandle)bsm.Arguments[argpos++])).IsUnloadable)
                        {
                            Fail("unloadable marker");
                            return false;
                        }
                    }
                }

                if ((flags & AltFlags.Bridges) != 0)
                {
                    bridges = new ConstantPoolItemMethodType[classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)bsm.Arguments[argpos++])];
                    for (int i = 0; i < bridges.Length; i++)
                    {
                        bridges[i] = classFile.GetConstantPoolConstantMethodType((MethodTypeConstantHandle)bsm.Arguments[argpos++]);
                        if (HasUnloadable(bridges[i]))
                        {
                            Fail("unloadable bridge");
                            return false;
                        }
                    }
                }
            }

            var samMethodType = classFile.GetConstantPoolConstantMethodType((MethodTypeConstantHandle)bsm.Arguments[0]);
            var implMethod = classFile.GetConstantPoolConstantMethodHandle((MethodHandleConstantHandle)bsm.Arguments[1]);
            var instantiatedMethodType = classFile.GetConstantPoolConstantMethodType((MethodTypeConstantHandle)bsm.Arguments[2]);
            if (HasUnloadable(samMethodType) || HasUnloadable((ConstantPoolItemMI)implMethod.MemberConstantPoolItem) || HasUnloadable(instantiatedMethodType))
            {
                Fail("bsm args has unloadable");
                return false;
            }

            var interfaceType = cpi.GetRetType();
            if (!CheckSupportedInterfaces(context.TypeWrapper, interfaceType, markers, bridges, out var methodList))
            {
                Fail("unsupported interface");
                return false;
            }

            if (serializable && Array.Exists(methodList, mw => mw.Name == "writeReplace" && mw.Signature == "()Ljava.lang.Object;"))
            {
                Fail("writeReplace");
                return false;
            }

            if (!IsSupportedImplMethod(implMethod, context.TypeWrapper, cpi.GetArgTypes(), instantiatedMethodType))
            {
                Fail("implMethod " + implMethod.MemberConstantPoolItem.Class + "::" + implMethod.MemberConstantPoolItem.Name + implMethod.MemberConstantPoolItem.Signature);
                return false;
            }

            // get implementation parameters
            var implParameters = GetImplParameters(implMethod);
            CheckConstraints(instantiatedMethodType, samMethodType, cpi.GetArgTypes(), implParameters);

            // check bridges
            if (bridges != null)
            {
                foreach (var bridge in bridges)
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

            // check that we can adapt instantiated return type to interface return type
            if (instantiatedMethodType.GetRetType() != context.Context.PrimitiveJavaTypeFactory.VOID)
            {
                var Rt = instantiatedMethodType.GetRetType();
                var Ra = GetImplReturnType(implMethod);
                if (Ra == context.Context.PrimitiveJavaTypeFactory.VOID || !IsAdaptable(Ra, Rt, true))
                {
                    Fail("The return type Rt is void, or the return type Ra is not void and is adaptable to Rt");
                    return false;
                }
            }

            // find desired functional interface method
            RuntimeJavaMethod interfaceMethod = null;
            var methods = new List<RuntimeJavaMethod>();
            foreach (var mw in methodList)
            {
                // method exactly matches interface method signature
                if (mw.Name == cpi.Name && mw.Signature == samMethodType.Signature)
                {
                    interfaceMethod = mw;
                    methods.Add(mw);
                    continue;
                }

                if (mw.IsAbstract && !IsObjectMethod(mw))
                {
                    methods.Add(mw);
                    continue;
                }
            }

            // check interface method
            if (interfaceMethod == null || !interfaceMethod.IsAbstract || IsObjectMethod(interfaceMethod) || !MatchSignatures(interfaceMethod, samMethodType))
            {
                Fail("interfaceMethod");
                return false;
            }

            // define a new anonymous class to implement the functional interface
            var tb = context.DefineAnonymousClass();

            // we're not implementing the interfaces recursively (because we don't care about .NET Compact anymore),
            // but should we decide to do that, we'd need to somehow communicate to AnonymousTypeWrapper what the 'real' interface is
            tb.AddInterfaceImplementation(interfaceType.TypeAsBaseType);
            if (serializable && Array.IndexOf(markers, context.Context.JavaBase.TypeOfJavaIoSerializable) == -1)
                tb.AddInterfaceImplementation(context.Context.JavaBase.TypeOfJavaIoSerializable.TypeAsBaseType);

            // implement marker interfaces
            foreach (var marker in markers)
                tb.AddInterfaceImplementation(marker.TypeAsBaseType);

            getInstance = CreateConstructorAndDispatch(context, cpi, tb, methods, implParameters, samMethodType, implMethod, instantiatedMethodType, serializable);
            AddDefaultInterfaceMethods(context, methodList, tb);
            return true;
        }

        [Conditional("TRACE_LAMBDA_METAFACTORY")]
        static void Fail(string msg)
        {
            Console.WriteLine("Fail: " + msg);
        }

        /// <summary>
        /// Returns <c>true</c> if the various signatures are compatible or adaptable to each other.
        /// </summary>
        /// <param name="instantiatedMethodType"></param>
        /// <param name="methodType"></param>
        /// <param name="args"></param>
        /// <param name="implParameters"></param>
        /// <returns></returns>
        static bool CheckConstraints(ConstantPoolItemMethodType instantiatedMethodType, ConstantPoolItemMethodType methodType, RuntimeJavaType[] args, RuntimeJavaType[] implParameters)
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

        static RuntimeJavaType[] GetImplParameters(ConstantPoolItemMethodHandle implMethod)
        {
            var mw = (RuntimeJavaMethod)implMethod.Member;
            var parameters = mw.GetParameters();
            if (mw.IsStatic || mw.IsConstructor)
                return parameters;

            return ArrayUtil.Concat(mw.DeclaringType, parameters);
        }

        static RuntimeJavaType GetImplReturnType(ConstantPoolItemMethodHandle implMethod)
        {
            return implMethod.Kind == MethodHandleKind.NewInvokeSpecial ? implMethod.Member.DeclaringType : ((RuntimeJavaMethod)implMethod.Member).ReturnType;
        }

        static bool IsAdaptable(RuntimeJavaType Q, RuntimeJavaType S, bool isReturn)
        {
            if (Q == S)
                return true;

            if (Q.IsPrimitive)
            {
                if (S.IsPrimitive)
                {
                    // Q can be converted to S via a primitive widening conversion
                    switch (Q.SignatureName[0] | S.SignatureName[0] << 8)
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
                    return GetBoxedPrimitiveType(Q).IsAssignableTo(S);
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
                    var primitive = GetUnboxedPrimitiveType(Q);
                    return primitive != null && IsAdaptable(primitive, S, isReturn);
                }
                else
                {
                    // for parameter types: S is a supertype of Q
                    return Q.IsAssignableTo(S);
                }
            }
        }

        /// <summary>
        /// For a given primitive type, returns the boxed primitive type.
        /// </summary>
        /// <param name="primitive"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static RuntimeJavaType GetBoxedPrimitiveType(RuntimeJavaType primitive)
        {
            Debug.Assert(primitive.IsPrimitive);
            switch (primitive.SignatureName[0])
            {
                case 'Z':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Boolean");
                case 'B':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Byte");
                case 'S':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Short");
                case 'C':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Character");
                case 'I':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Integer");
                case 'J':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Long");
                case 'F':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Float");
                case 'D':
                    return primitive.Context.ClassLoaderFactory.LoadClassCritical("java.lang.Double");
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// For a given boxed primitive type, returns the primitive type.
        /// </summary>
        /// <param name="wrapper"></param>
        /// <returns></returns>
        static RuntimeJavaType GetUnboxedPrimitiveType(RuntimeJavaType wrapper)
        {
            switch (wrapper.Name)
            {
                case "java.lang.Boolean":
                    return wrapper.Context.PrimitiveJavaTypeFactory.BOOLEAN;
                case "java.lang.Byte":
                    return wrapper.Context.PrimitiveJavaTypeFactory.BYTE;
                case "java.lang.Short":
                    return wrapper.Context.PrimitiveJavaTypeFactory.SHORT;
                case "java.lang.Character":
                    return wrapper.Context.PrimitiveJavaTypeFactory.CHAR;
                case "java.lang.Integer":
                    return wrapper.Context.PrimitiveJavaTypeFactory.INT;
                case "java.lang.Long":
                    return wrapper.Context.PrimitiveJavaTypeFactory.LONG;
                case "java.lang.Float":
                    return wrapper.Context.PrimitiveJavaTypeFactory.FLOAT;
                case "java.lang.Double":
                    return wrapper.Context.PrimitiveJavaTypeFactory.DOUBLE;
                default:
                    return null;
            }
        }

        static bool IsSubTypeOf(ConstantPoolItemMethodType instantiatedMethodType, ConstantPoolItemMethodType samMethodType)
        {
            var T = instantiatedMethodType.GetArgTypes();
            var U = samMethodType.GetArgTypes();
            if (T.Length != U.Length)
                return false;

            for (int i = 0; i < T.Length; i++)
                if (!T[i].IsAssignableTo(U[i]))
                    return false;

            var Rt = instantiatedMethodType.GetRetType();
            var Ru = samMethodType.GetRetType();
            return Rt.IsAssignableTo(Ru);
        }

        /// <summary>
        /// Creates the constrctor and dispatch methods for the indy.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="cpi"></param>
        /// <param name="tb"></param>
        /// <param name="methods"></param>
        /// <param name="implParameters"></param>
        /// <param name="samMethodType"></param>
        /// <param name="implMethod"></param>
        /// <param name="instantiatedMethodType"></param>
        /// <param name="serializable"></param>
        /// <returns></returns>
        static MethodBuilder CreateConstructorAndDispatch(RuntimeByteCodeJavaType.FinishContext context, ConstantPoolItemInvokeDynamic cpi, TypeBuilder tb, List<RuntimeJavaMethod> methods, RuntimeJavaType[] implParameters, ConstantPoolItemMethodType samMethodType, ConstantPoolItemMethodHandle implMethod, ConstantPoolItemMethodType instantiatedMethodType, bool serializable)
        {
            var args = cpi.GetArgTypes();

            // captured values
            var capturedTypes = new Type[args.Length];
            var capturedFields = new FieldBuilder[capturedTypes.Length];
            for (int i = 0; i < capturedTypes.Length; i++)
            {
                capturedTypes[i] = args[i].TypeAsSignatureType;
                var attr = FieldAttributes.Private;
                if (i > 0 || !args[0].IsGhost)
                    attr |= FieldAttributes.InitOnly;

                capturedFields[i] = tb.DefineField("arg$" + (i + 1), capturedTypes[i], attr);
            }

            // constructor
            var ctor = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, capturedTypes);
            var ilgen = context.Context.CodeEmitterFactory.Create(ctor);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Call, context.Context.Types.Object.GetConstructor(Type.EmptyTypes));
            for (int i = 0; i < capturedTypes.Length; i++)
            {
                ilgen.EmitLdarg(0);
                ilgen.EmitLdarg(i + 1);
                ilgen.Emit(OpCodes.Stfld, capturedFields[i]);
            }
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();

            // instance getter
            var getInstance = tb.DefineMethod("__<GetInstance>", MethodAttributes.Assembly | MethodAttributes.Static, cpi.GetRetType().TypeAsBaseType, capturedTypes);
            var ilgenGet = context.Context.CodeEmitterFactory.Create(getInstance);

            if (capturedTypes.Length == 0)
            {
                // use singleton for lambdas with no captures
                var instField = tb.DefineField("inst", tb, FieldAttributes.Private | FieldAttributes.Static);

                // static constructor
                var cctor = ReflectUtil.DefineTypeInitializer(tb, context.TypeWrapper.ClassLoader);
                var ilgenCCtor = context.Context.CodeEmitterFactory.Create(cctor);
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
            foreach (var mw in methods)
                EmitDispatch(context, args, tb, mw, implParameters, implMethod, instantiatedMethodType, capturedFields);

            // writeReplace method
            if (serializable)
            {
                var writeReplace = tb.DefineMethod("writeReplace", MethodAttributes.Private, context.Context.Types.Object, Type.EmptyTypes);
                ilgen = context.Context.CodeEmitterFactory.Create(writeReplace);
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
                ilgen.Emit(OpCodes.Newarr, context.Context.Types.Object);
                for (int i = 0; i < capturedFields.Length; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitLdc_I4(i);
                    ilgen.EmitLdarg(0);
                    ilgen.Emit(OpCodes.Ldfld, capturedFields[i]);
                    if (args[i].IsPrimitive)
                    {
                        context.Context.Boxer.EmitBox(ilgen, args[i]);
                    }
                    else if (args[i].IsGhost)
                    {
                        args[i].EmitConvSignatureTypeToStackType(ilgen);
                    }
                    ilgen.Emit(OpCodes.Stelem, context.Context.Types.Object);
                }

                var ctorSerializedLambda = context.Context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.SerializedLambda").GetMethod(StringConstants.INIT, "(Ljava.lang.Class;Ljava.lang.String;Ljava.lang.String;Ljava.lang.String;ILjava.lang.String;Ljava.lang.String;Ljava.lang.String;Ljava.lang.String;[Ljava.lang.Object;)V", false);
                ctorSerializedLambda.Link();
                ctorSerializedLambda.EmitNewobj(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();

                if (!context.TypeWrapper.ClassLoader.NoAutomagicSerialization)
                {
                    // add .NET serialization interop support
                    context.Context.Serialization.MarkSerializable(tb);
                    context.Context.Serialization.AddGetObjectData(tb);
                }
            }

            return getInstance;
        }

        /// <summary>
        /// Emits the Dispatch method. That is, the implementation of the functional interface method who's job it is to call the final implementation method.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        /// <param name="tb"></param>
        /// <param name="interfaceMethod"></param>
        /// <param name="implParameters"></param>
        /// <param name="implMethod"></param>
        /// <param name="instantiatedMethodType"></param>
        /// <param name="capturedFields"></param>
        /// <exception cref="InvalidOperationException"></exception>
        static void EmitDispatch(RuntimeByteCodeJavaType.FinishContext context, RuntimeJavaType[] args, TypeBuilder tb, RuntimeJavaMethod interfaceMethod, RuntimeJavaType[] implParameters, ConstantPoolItemMethodHandle implMethod, ConstantPoolItemMethodType instantiatedMethodType, FieldBuilder[] capturedFields)
        {
            var mb = interfaceMethod.GetDefineMethodHelper().DefineMethod(context.TypeWrapper, tb, interfaceMethod.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final);
            if (interfaceMethod.Name != interfaceMethod.RealName)
                tb.DefineMethodOverride(mb, (MethodInfo)interfaceMethod.GetMethod());

            context.Context.AttributeHelper.HideFromJava(mb);

            var ilgen = context.Context.CodeEmitterFactory.Create(mb);
            for (int i = 0; i < capturedFields.Length; i++)
            {
                ilgen.EmitLdarg(0);
                var opc = OpCodes.Ldfld;
                if (i == 0 && args[0].IsGhost)
                {
                    switch (implMethod.Kind)
                    {
                        case MethodHandleKind.InvokeInterface:
                        case MethodHandleKind.InvokeVirtual:
                        case MethodHandleKind.InvokeSpecial:
                            opc = OpCodes.Ldflda;
                            break;
                    }
                }

                ilgen.Emit(opc, capturedFields[i]);
            }

            // emit conversions for interface arguments
            // Ui represents the type on the interface, which for generics may be erased to Object
            // Ti represents the incoming type on the interface, which usually matches Ui, but may not in the case of generics
            // Aj represents the outgoing type on the implementation, to which we need to convert the argument to before calling
            for (int i = 0, count = interfaceMethod.GetParameters().Length, k = capturedFields.Length; i < count; i++)
            {
                ilgen.EmitLdarg(i + 1);
                var Ui = interfaceMethod.GetParameters()[i];
                var Ti = instantiatedMethodType.GetArgTypes()[i];
                var Aj = implParameters[i + k];

                // incoming byte arguments are first converted to integers
                if (Ui == context.Context.PrimitiveJavaTypeFactory.BYTE)
                    ilgen.Emit(OpCodes.Conv_I1);

                // instantiation type does not equal interface type
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

                // implementation type does not equal instantiation type
                if (Aj != Ti)
                {
                    if (Ti.IsPrimitive && !Aj.IsPrimitive)
                    {
                        // box primitive
                        context.Context.Boxer.EmitBox(ilgen, Ti);
                    }
                    else if (!Ti.IsPrimitive && Aj.IsPrimitive)
                    {
                        // unbox primitive
                        var primitive = GetUnboxedPrimitiveType(Ti);
                        context.Context.Boxer.EmitUnbox(ilgen, primitive, false);
                        if (primitive == context.Context.PrimitiveJavaTypeFactory.BYTE)
                            ilgen.Emit(OpCodes.Conv_I1);
                    }
                    else if (Aj == context.Context.PrimitiveJavaTypeFactory.LONG)
                    {
                        // long is i8
                        ilgen.Emit(OpCodes.Conv_I8);
                    }
                    else if (Aj == context.Context.PrimitiveJavaTypeFactory.FLOAT)
                    {
                        // float is r4
                        ilgen.Emit(OpCodes.Conv_R4);
                    }
                    else if (Aj == context.Context.PrimitiveJavaTypeFactory.DOUBLE)
                    {
                        // double is r8
                        ilgen.Emit(OpCodes.Conv_R8);
                    }
                    else if (Aj.IsGhost)
                    {
                        // convert to ghost type
                        Aj.EmitConvStackTypeToSignatureType(ilgen, Ti);
                    }
                    else if (Ti.IsGhost)
                    {
                        // FIX: ghost signature type -> Object (unwrap __<ref>)
                        Ti.EmitConvSignatureTypeToStackType(ilgen);
                    }
                }
            }

            switch (implMethod.Kind)
            {
                case MethodHandleKind.InvokeVirtual:
                case MethodHandleKind.InvokeInterface:
                    ((RuntimeJavaMethod)implMethod.Member).EmitCallvirt(ilgen);
                    break;
                case MethodHandleKind.NewInvokeSpecial:
                    ((RuntimeJavaMethod)implMethod.Member).EmitNewobj(ilgen);
                    break;
                case MethodHandleKind.InvokeStatic:
                case MethodHandleKind.InvokeSpecial:
                    ((RuntimeJavaMethod)implMethod.Member).EmitCall(ilgen);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            var Ru = interfaceMethod.ReturnType;
            var Ra = GetImplReturnType(implMethod);
            var Rt = instantiatedMethodType.GetRetType();

            if (Ra == context.Context.PrimitiveJavaTypeFactory.BYTE)
            {
                ilgen.Emit(OpCodes.Conv_I1);
            }

            if (Ra != Ru)
            {
                if (Ru == context.Context.PrimitiveJavaTypeFactory.VOID)
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
                    if (Rt == context.Context.PrimitiveJavaTypeFactory.VOID)
                    {
                        // already popped
                    }
                    else if (!Ra.IsPrimitive)
                    {
                        var primitive = GetUnboxedPrimitiveType(Ra);
                        if (primitive != null)
                        {
                            context.Context.Boxer.EmitUnbox(ilgen, primitive, false);
                        }
                        else
                        {
                            // If Q is not a primitive wrapper, cast Q to the base Wrapper(S); for example Number for numeric types
                            EmitConvertingUnbox(ilgen, Rt);
                        }
                    }
                    else if (Rt == context.Context.PrimitiveJavaTypeFactory.LONG)
                    {
                        ilgen.Emit(OpCodes.Conv_I8);
                    }
                    else if (Rt == context.Context.PrimitiveJavaTypeFactory.FLOAT)
                    {
                        ilgen.Emit(OpCodes.Conv_R4);
                    }
                    else if (Rt == context.Context.PrimitiveJavaTypeFactory.DOUBLE)
                    {
                        ilgen.Emit(OpCodes.Conv_R8);
                    }
                }
                else if (Ra.IsPrimitive)
                {
                    var tw = GetUnboxedPrimitiveType(Rt);
                    if (tw == null)
                        tw = Ra;

                    context.Context.Boxer.EmitBox(ilgen, tw);
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

        static void EmitConvertingUnbox(CodeEmitter ilgen, RuntimeJavaType tw)
        {
            switch (tw.SignatureName[0])
            {
                case 'Z':
                case 'C':
                    tw.Context.Boxer.EmitUnbox(ilgen, tw, true);
                    break;
                case 'B':
                    EmitUnboxNumber(tw.Context, ilgen, "byteValue", "()B");
                    break;
                case 'S':
                    EmitUnboxNumber(tw.Context, ilgen, "shortValue", "()S");
                    break;
                case 'I':
                    EmitUnboxNumber(tw.Context, ilgen, "intValue", "()I");
                    break;
                case 'J':
                    EmitUnboxNumber(tw.Context, ilgen, "longValue", "()J");
                    break;
                case 'F':
                    EmitUnboxNumber(tw.Context, ilgen, "floatValue", "()F");
                    break;
                case 'D':
                    EmitUnboxNumber(tw.Context, ilgen, "doubleValue", "()D");
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        static void EmitUnboxNumber(RuntimeContext context, CodeEmitter ilgen, string methodName, string methodSig)
        {
            var tw = context.ClassLoaderFactory.LoadClassCritical("java.lang.Number");
            tw.EmitCheckcast(ilgen);
            var mw = tw.GetMethod(methodName, methodSig, false);
            mw.Link();
            mw.EmitCallvirt(ilgen);
        }

        static void AddDefaultInterfaceMethods(RuntimeByteCodeJavaType.FinishContext context, RuntimeJavaMethod[] methodList, TypeBuilder tb)
        {
            // we use special name to hide these from Java reflection
            const MethodAttributes attr = MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName;

            var factory = context.TypeWrapper.ClassLoader.GetTypeWrapperFactory();
            foreach (var mw in methodList)
            {
                if (!mw.IsAbstract)
                {
                    var mb = mw.GetDefineMethodHelper().DefineMethod(factory, tb, mw.Name, attr);
                    if (mw.Name != mw.RealName)
                        tb.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());

                    context.EmitCallDefaultInterfaceMethod(mb, mw);
                }
                else if (IsObjectMethod(mw))
                {
                    var mb = mw.GetDefineMethodHelper().DefineMethod(factory, tb, mw.Name, attr);
                    if (mw.Name != mw.RealName)
                        tb.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());

                    var ilgen = context.Context.CodeEmitterFactory.Create(mb);
                    for (int i = 0, count = mw.GetParameters().Length; i <= count; i++)
                        ilgen.EmitLdarg(i);

                    context.Context.JavaBase.TypeOfJavaLangObject.GetMethod(mw.Name, mw.Signature, false).EmitCallvirt(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                }
            }
        }

        static bool IsSupportedImplMethod(ConstantPoolItemMethodHandle implMethod, RuntimeJavaType caller, RuntimeJavaType[] captured, ConstantPoolItemMethodType instantiatedMethodType)
        {
            switch (implMethod.Kind)
            {
                case MethodHandleKind.InvokeVirtual:
                case MethodHandleKind.InvokeInterface:
                case MethodHandleKind.NewInvokeSpecial:
                case MethodHandleKind.InvokeStatic:
                case MethodHandleKind.InvokeSpecial:
                    break;
                default:
                    return false;
            }

            var mw = (RuntimeJavaMethod)implMethod.Member;
            if (mw == null || mw.HasCallerID || RuntimeByteCodeJavaType.RequiresDynamicReflectionCallerClass(mw.DeclaringType.Name, mw.Name, mw.Signature))
                return false;

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
                    return false;
            }

            if (!mw.IsAccessibleFrom(mw.DeclaringType, caller, instance))
                return false;

            mw.Link();
            return true;
        }

        static bool IsSupportedInterface(RuntimeJavaType tw, RuntimeJavaType caller)
        {
            return tw.IsInterface && !tw.IsGhost && tw.IsAccessibleFrom(caller) && !tw.Context.Serialization.IsISerializable(tw);
        }

        static bool CheckSupportedInterfaces(RuntimeJavaType caller, RuntimeJavaType tw, RuntimeJavaType[] markers, ConstantPoolItemMethodType[] bridges, out RuntimeJavaMethod[] methodList)
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
                foreach (var marker in markers)
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

        static bool GatherAllInterfaceMethods(RuntimeJavaType tw, ConstantPoolItemMethodType[] bridges, Dictionary<MethodKey, RuntimeJavaMethod> methods, ref int abstractMethodCount, ref int bridgeMethodCount)
        {
            foreach (var mw in tw.GetMethods())
            {
                if (mw.IsVirtual)
                {
                    if (mw is RuntimeMirandaJavaMethod mmw)
                    {
                        if (mmw.Error != null)
                            return false;

                        continue;
                    }

                    var key = new MethodKey("", mw.Name, mw.Signature);
                    if (methods.TryGetValue(key, out var current))
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
                                bridgeMethodCount++;
                            else
                                abstractMethodCount++;
                        }
                    }

                    mw.Link();
                    if (mw.GetMethod() == null)
                        return false;

                    if (current != null && mw.RealName != current.RealName)
                        return false;
                }
            }

            foreach (var tw1 in tw.Interfaces)
                if (!GatherAllInterfaceMethods(tw1, bridges, methods, ref abstractMethodCount, ref bridgeMethodCount))
                    return false;

            return true;
        }

        static bool IsBridge(RuntimeJavaMethod mw, ConstantPoolItemMethodType[] bridges)
        {
            foreach (ConstantPoolItemMethodType bridge in bridges)
                if (bridge.Signature == mw.Signature)
                    return true;

            return false;
        }

        static bool IsObjectMethod(RuntimeJavaMethod mw)
        {
            RuntimeJavaMethod objectMethod;
            return (objectMethod = mw.DeclaringType.Context.JavaBase.TypeOfJavaLangObject.GetMethod(mw.Name, mw.Signature, false)) != null && objectMethod.IsPublic;
        }

        static bool MatchSignatures(RuntimeJavaMethod interfaceMethod, ConstantPoolItemMethodType samMethodType)
        {
            return interfaceMethod.ReturnType == samMethodType.GetRetType() && MatchTypes(interfaceMethod.GetParameters(), samMethodType.GetArgTypes());
        }

        static bool MatchSignatures(RuntimeJavaMethod mw1, RuntimeJavaMethod mw2)
        {
            mw1.Link();
            mw2.Link();
            return mw1.ReturnType == mw2.ReturnType && MatchTypes(mw1.GetParameters(), mw2.GetParameters());
        }

        static bool MatchTypes(RuntimeJavaType[] ar1, RuntimeJavaType[] ar2)
        {
            if (ar1.Length != ar2.Length)
                return false;

            for (int i = 0; i < ar1.Length; i++)
                if (ar1[i] != ar2[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Returns <c>true</c> if the given class file bootstrap method is a reference to the java.lang.invoke.LambdaMetafactory:metafactory method.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="bootstrapMethod"></param>
        /// <returns></returns>
        static bool IsLambdaMetafactory(ClassFile classFile, BootstrapMethod bootstrapMethod)
        {
            return bootstrapMethod.Arguments.Count == 3 &&
                classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[0]) == ConstantType.MethodType &&
                classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[1]) == ConstantType.MethodHandle &&
                classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[2]) == ConstantType.MethodType &&
                classFile.GetConstantPoolConstantMethodHandle(bootstrapMethod.Method) is { Kind: MethodHandleKind.InvokeStatic, Member: not null } mh &&
                IsLambdaMetafactory(mh.Member);
        }

        /// <summary>
        /// Returns <c>true</c> if the given member is a reference to the java.lang.invoke.LambdaMetafactory:metafactory method.
        /// </summary>
        /// <param name="mw"></param>
        /// <returns></returns>
        static bool IsLambdaMetafactory(RuntimeJavaMember mw)
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

        private static bool IsLambdaAltMetafactory(ClassFile classFile, BootstrapMethod bootstrapMethod)
        {
            ConstantPoolItemMethodHandle mh;
            AltFlags flags;
            int argpos = 4;
            return bootstrapMethod.Arguments.Count >= 4
                && (mh = classFile.GetConstantPoolConstantMethodHandle(bootstrapMethod.Method)).Kind == MethodHandleKind.InvokeStatic
                && mh.Member != null
                && IsLambdaAltMetafactory(mh.Member)
                && classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[0]) == ConstantType.MethodType
                && classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[1]) == ConstantType.MethodHandle
                && classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[2]) == ConstantType.MethodType
                && classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[3]) == ConstantType.Integer
                && ((flags = (AltFlags)classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)bootstrapMethod.Arguments[3])) & ~AltFlags.Mask) == 0
                && ((flags & AltFlags.Markers) == 0 || CheckOptionalArgs(classFile, bootstrapMethod, ConstantType.Class, ref argpos))
                && ((flags & AltFlags.Bridges) == 0 || CheckOptionalArgs(classFile, bootstrapMethod, ConstantType.MethodType, ref argpos))
                && argpos == bootstrapMethod.Arguments.Count;
        }

        static bool IsLambdaAltMetafactory(RuntimeJavaMember mw)
        {
            return mw.Name == "altMetafactory"
                && mw.Signature == "(Ljava.lang.invoke.MethodHandles$Lookup;Ljava.lang.String;Ljava.lang.invoke.MethodType;[Ljava.lang.Object;)Ljava.lang.invoke.CallSite;"
                && mw.DeclaringType.Name == "java.lang.invoke.LambdaMetafactory";
        }

        static bool CheckOptionalArgs(ClassFile classFile, BootstrapMethod bootstrapMethod, ConstantType type, ref int argpos)
        {
            if (bootstrapMethod.Arguments.Count - argpos < 1)
                return false;

            if (classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[argpos]) != ConstantType.Integer)
                return false;

            int count = classFile.GetConstantPoolConstantInteger((IntegerConstantHandle)bootstrapMethod.Arguments[argpos++]);
            if (count < 0 || bootstrapMethod.Arguments.Count - argpos < count)
                return false;

            for (int i = 0; i < count; i++)
                if (classFile.GetConstantPoolConstantType(bootstrapMethod.Arguments[argpos++]) != type)
                    return false;

            return true;
        }

        private static bool HasUnloadable(ConstantPoolItemInvokeDynamic cpi)
        {
            return HasUnloadable(cpi.GetArgTypes()) || cpi.GetRetType().IsUnloadable;
        }

        private static bool HasUnloadable(ConstantPoolItemMethodType cpi)
        {
            return HasUnloadable(cpi.GetArgTypes()) || cpi.GetRetType().IsUnloadable;
        }

        private static bool HasUnloadable(ConstantPoolItemMI cpi)
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
