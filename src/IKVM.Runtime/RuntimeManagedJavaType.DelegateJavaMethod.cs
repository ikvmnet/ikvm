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

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed class DelegateJavaMethod : RuntimeJavaMethod
        {

            readonly ConstructorInfo delegateConstructor;
            readonly DelegateInnerClassJavaType iface;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="iface"></param>
            internal DelegateJavaMethod(RuntimeJavaType declaringType, DelegateInnerClassJavaType iface) :
                base(declaringType, "<init>", "(" + iface.SignatureName + ")V", null, declaringType.Context.PrimitiveJavaTypeFactory.VOID, [iface], Modifiers.Public, MemberFlags.Intrinsic)
            {
                this.delegateConstructor = declaringType.TypeAsTBD.GetConstructor([DeclaringType.Context.Types.Object, DeclaringType.Context.Types.IntPtr]);
                this.iface = iface;
            }

#if EMITTERS

            internal override bool EmitIntrinsic(EmitIntrinsicContext context)
            {
                var targetType = context.GetStackTypeWrapper(0, 0);
                if (targetType.IsUnloadable || targetType.IsInterface)
                    return false;

                // we know that a DelegateInnerClassTypeWrapper has only one method
                Debug.Assert(iface.GetMethods().Length == 1);

                var mw = targetType.GetMethod(GetDelegateInvokeStubName(DeclaringType.TypeAsTBD), iface.GetMethods()[0].Signature, true);
                if (mw == null || mw.IsStatic || !mw.IsPublic)
                {
                    context.Emitter.Emit(OpCodes.Ldftn, CreateErrorStub(context, targetType, mw == null || mw.IsStatic));
                    context.Emitter.Emit(OpCodes.Newobj, delegateConstructor);
                    return true;
                }

                // TODO linking here is not safe
                mw.Link();
                context.Emitter.Emit(OpCodes.Dup);
                context.Emitter.Emit(OpCodes.Ldvirtftn, mw.GetMethod());
                context.Emitter.Emit(OpCodes.Newobj, delegateConstructor);
                return true;
            }

            MethodInfo CreateErrorStub(EmitIntrinsicContext context, RuntimeJavaType targetType, bool isAbstract)
            {
                var invoke = delegateConstructor.DeclaringType.GetMethod("Invoke");

                var parameters = invoke.GetParameters();
                var parameterTypes = new Type[parameters.Length + 1];
                parameterTypes[0] = DeclaringType.Context.Types.Object;
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i + 1] = parameters[i].ParameterType;

                var mb = context.Context.DefineDelegateInvokeErrorStub(invoke.ReturnType, parameterTypes);
                var ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mb);
                ilgen.EmitThrow(isAbstract ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", targetType.Name + ".Invoke" + iface.GetMethods()[0].Signature);
                ilgen.DoEmit();
                return mb;
            }

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Ldtoken, delegateConstructor.DeclaringType);
                ilgen.Emit(OpCodes.Call, DeclaringType.Context.CompilerFactory.GetTypeFromHandleMethod);
                ilgen.Emit(OpCodes.Ldstr, GetDelegateInvokeStubName(DeclaringType.TypeAsTBD));
                ilgen.Emit(OpCodes.Ldstr, iface.GetMethods()[0].Signature);
                ilgen.Emit(OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.DynamicCreateDelegate);
                ilgen.Emit(OpCodes.Castclass, delegateConstructor.DeclaringType);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // This is a bit of a hack. We bind the existing delegate to a new delegate to be able to reuse the DynamicCreateDelegate error
                // handling. This leaks out to the user because Delegate.Target will return the target delegate instead of the bound object.

                // create the target delegate
                EmitNewobj(ilgen);

                // invoke the constructor, binding the delegate to the target delegate
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Ldvirtftn, DeclaringType.Context.MethodHandleUtil.GetDelegateInvokeMethod(delegateConstructor.DeclaringType));
                ilgen.Emit(OpCodes.Call, delegateConstructor);
            }

#endif

        }

    }

}
