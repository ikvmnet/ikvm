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
using IKVM.CoreLib.Symbols;
using System.Collections.Immutable;
using System.Linq;



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

            readonly MethodSymbol delegateConstructor;
            readonly DelegateInnerClassJavaType iface;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="iface"></param>
            internal DelegateJavaMethod(RuntimeJavaType declaringType, DelegateInnerClassJavaType iface) :
                base(declaringType, "<init>", "(" + iface.SigName + ")V", null, declaringType.Context.PrimitiveJavaTypeFactory.VOID, [iface], Modifiers.Public, MemberFlags.Intrinsic)
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

                var mw = targetType.GetMethodWrapper(GetDelegateInvokeStubName(DeclaringType.TypeAsTBD), iface.GetMethods()[0].Signature, true);
                if (mw == null || mw.IsStatic || !mw.IsPublic)
                {
                    context.Emitter.Emit(System.Reflection.Emit.OpCodes.Ldftn, CreateErrorStub(context, targetType, mw == null || mw.IsStatic));
                    context.Emitter.Emit(System.Reflection.Emit.OpCodes.Newobj, delegateConstructor);
                    return true;
                }

                // TODO linking here is not safe
                mw.Link();
                context.Emitter.Emit(System.Reflection.Emit.OpCodes.Dup);
                context.Emitter.Emit(System.Reflection.Emit.OpCodes.Ldvirtftn, mw.GetMethod());
                context.Emitter.Emit(System.Reflection.Emit.OpCodes.Newobj, delegateConstructor);
                return true;
            }

            MethodSymbol CreateErrorStub(EmitIntrinsicContext context, RuntimeJavaType targetType, bool isAbstract)
            {
                var invoke = delegateConstructor.DeclaringType.GetMethod("Invoke");
                var parameterTypes = invoke.ParameterTypes.Insert(0, DeclaringType.Context.Types.Object);
                var mb = context.Context.DefineDelegateInvokeErrorStub(invoke.ReturnType, parameterTypes);
                var ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mb);
                ilgen.EmitThrow(isAbstract ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", targetType.Name + ".Invoke" + iface.GetMethods()[0].Signature);
                ilgen.DoEmit();
                return mb;
            }

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldtoken, delegateConstructor.DeclaringType);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, DeclaringType.Context.CompilerFactory.GetTypeFromHandleMethod);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldstr, GetDelegateInvokeStubName(DeclaringType.TypeAsTBD));
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldstr, iface.GetMethods()[0].Signature);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, DeclaringType.Context.ByteCodeHelperMethods.DynamicCreateDelegate);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Castclass, delegateConstructor.DeclaringType);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // This is a bit of a hack. We bind the existing delegate to a new delegate to be able to reuse the DynamicCreateDelegate error
                // handling. This leaks out to the user because Delegate.Target will return the target delegate instead of the bound object.

                // create the target delegate
                EmitNewobj(ilgen);

                // invoke the constructor, binding the delegate to the target delegate
                ilgen.Emit(System.Reflection.Emit.OpCodes.Dup);
                ilgen.Emit(System.Reflection.Emit.OpCodes.Ldvirtftn, DeclaringType.Context.MethodHandleUtil.GetDelegateInvokeMethod(delegateConstructor.DeclaringType));
                ilgen.Emit(System.Reflection.Emit.OpCodes.Call, delegateConstructor);
            }

#endif

        }

    }

}
