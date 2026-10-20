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

using IKVM.Attributes;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using RuntimeDynamicOrImportJavaType = IKVM.Tools.Importer.RuntimeImportByteCodeJavaType;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        private sealed partial class JavaTypeImpl
        {

            private sealed class DelegateInvokeStubMethodWrapper : RuntimeJavaMethod
            {

                readonly Type delegateType;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                /// <param name="delegateType"></param>
                /// <param name="sig"></param>
                internal DelegateInvokeStubMethodWrapper(RuntimeJavaType declaringType, Type delegateType, string sig) :
                    base(declaringType, RuntimeManagedJavaType.GetDelegateInvokeStubName(delegateType), sig, null, null, null, Modifiers.Public | Modifiers.Final, MemberFlags.HideFromReflection)
                {
                    this.delegateType = delegateType;
                }

                internal MethodInfo DoLink(TypeBuilder tb)
                {
                    var mw = this.DeclaringType.GetMethod("Invoke", Signature, true);
                    var invoke = delegateType.GetMethod("Invoke");
                    var parameters = invoke.GetParameters();
                    var parameterTypes = new Type[parameters.Length];
                    for (int i = 0; i < parameterTypes.Length; i++)
                        parameterTypes[i] = parameters[i].ParameterType;

                    var mb = tb.DefineMethod(Name, MethodAttributes.Public, invoke.ReturnType, parameterTypes);
                    DeclaringType.Context.AttributeHelper.HideFromReflection(mb);

                    var ilgen = DeclaringType.Context.CodeEmitterFactory.Create(mb);
                    if (mw == null || mw.IsStatic || !mw.IsPublic)
                    {
                        ilgen.EmitThrow(mw == null || mw.IsStatic ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", DeclaringType.Name + ".Invoke" + Signature);
                        ilgen.DoEmit();
                        return mb;
                    }

                    var byrefs = new CodeEmitterLocal[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType.IsByRef)
                        {
                            var elemType = parameters[i].ParameterType.GetElementType();
                            var local = ilgen.DeclareLocal(RuntimeArrayJavaType.MakeArrayType(elemType, 1));
                            byrefs[i] = local;
                            ilgen.Emit(OpCodes.Ldc_I4_1);
                            ilgen.Emit(OpCodes.Newarr, elemType);
                            ilgen.Emit(OpCodes.Stloc, local);
                            ilgen.Emit(OpCodes.Ldloc, local);
                            ilgen.Emit(OpCodes.Ldc_I4_0);
                            ilgen.EmitLdarg(i + 1);
                            ilgen.Emit(OpCodes.Ldobj, elemType);
                            ilgen.Emit(OpCodes.Stelem, elemType);
                        }
                    }

                    ilgen.BeginExceptionBlock();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (byrefs[i] != null)
                        {
                            ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
                        }
                        else
                        {
                            ilgen.EmitLdarg(i + 1);
                        }
                    }
                    mw.Link();
                    mw.EmitCallvirt(ilgen);
                    CodeEmitterLocal returnValue = null;
                    if (mw.ReturnType != DeclaringType.Context.PrimitiveJavaTypeFactory.VOID)
                    {
                        returnValue = ilgen.DeclareLocal(mw.ReturnType.TypeAsSignatureType);
                        ilgen.Emit(OpCodes.Stloc, returnValue);
                    }
                    CodeEmitterLabel exit = ilgen.DefineLabel();
                    ilgen.EmitLeave(exit);
                    ilgen.BeginFinallyBlock();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (byrefs[i] != null)
                        {
                            Type elemType = byrefs[i].LocalType.GetElementType();
                            ilgen.EmitLdarg(i + 1);
                            ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
                            ilgen.Emit(OpCodes.Ldc_I4_0);
                            ilgen.Emit(OpCodes.Ldelem, elemType);
                            ilgen.Emit(OpCodes.Stobj, elemType);
                        }
                    }
                    ilgen.Emit(OpCodes.Endfinally);
                    ilgen.EndExceptionBlock();
                    ilgen.MarkLabel(exit);
                    if (returnValue != null)
                    {
                        ilgen.Emit(OpCodes.Ldloc, returnValue);
                    }
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    return mb;
                }
            }
        }
    }

}