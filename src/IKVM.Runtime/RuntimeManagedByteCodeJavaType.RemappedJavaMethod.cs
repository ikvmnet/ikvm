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
using System.Diagnostics;
using System.Reflection.Emit;

using IKVM.Attributes;
using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    partial class RuntimeManagedByteCodeJavaType
    {

        sealed class RemappedJavaMethod : RuntimeSmartJavaMethod
        {

            readonly IMethodSymbol mbHelper;
#if !IMPORTER
            readonly IMethodSymbol mbNonvirtualHelper;
#endif

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="method"></param>
            /// <param name="returnType"></param>
            /// <param name="parameterTypes"></param>
            /// <param name="modifiers"></param>
            /// <param name="hideFromReflection"></param>
            /// <param name="mbHelper"></param>
            /// <param name="mbNonvirtualHelper"></param>
            internal RemappedJavaMethod(RuntimeJavaType declaringType, string name, string sig, IMethodBaseSymbol method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, ExModifiers modifiers, bool hideFromReflection, IMethodSymbol mbHelper, IMethodSymbol mbNonvirtualHelper) :
                base(declaringType, name, sig, method, returnType, parameterTypes, modifiers.Modifiers, (modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None))
            {
                this.mbHelper = mbHelper;
#if !IMPORTER
                this.mbNonvirtualHelper = mbNonvirtualHelper;
#endif
            }

#if EMITTERS

            protected override void CallImpl(CodeEmitter ilgen)
            {
                var mb = GetMethod();
                var mi = mb as IMethodSymbol;
                if (mi != null)
                {
                    if (!IsStatic && IsFinal)
                    {
                        // When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
                        // then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
                        // is final, this won't affect the semantics.
                        CallvirtImpl(ilgen);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Call, mi);
                    }
                }
                else
                {
                    ilgen.Emit(OpCodes.Call, mb);
                }
            }

            protected override void CallvirtImpl(CodeEmitter ilgen)
            {
                Debug.Assert(!mbHelper.IsStatic || mbHelper.Name.StartsWith("instancehelper_") || mbHelper.DeclaringType.Name == "__Helper");
                if (mbHelper.IsPublic)
                {
                    ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
                }
                else
                {
                    // HACK the helper is not public, this means that we're dealing with finalize or clone
                    ilgen.Emit(OpCodes.Callvirt, GetMethod());
                }
            }

            protected override void NewobjImpl(CodeEmitter ilgen)
            {
                var mb = GetMethod();
                var mi = mb as IMethodSymbol;
                if (mi != null)
                {
                    Debug.Assert(mi.Name == "newhelper");
                    ilgen.Emit(OpCodes.Call, mi);
                }
                else
                {
                    ilgen.Emit(OpCodes.Newobj, mb);
                }
            }

#endif // EMITTERS

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            [HideFromJava]
            internal override object Invoke(object obj, object[] args)
            {
                var mb = mbHelper ?? GetMethod();
                if (mb.IsStatic && !IsStatic)
                {
                    args = ArrayUtil.Concat(obj, args);
                    obj = null;
                }

                return InvokeAndUnwrapException(mb, obj, args);
            }

            [HideFromJava]
            internal override object CreateInstance(object[] args)
            {
                var mb = mbHelper ?? GetMethod();
                if (mb.IsStatic)
                    return InvokeAndUnwrapException(mb, null, args);

                return base.CreateInstance(args);
            }

            [HideFromJava]
            internal override object InvokeNonvirtualRemapped(object obj, object[] args)
            {
                var mi = mbNonvirtualHelper ?? mbHelper;
                return mi.AsReflection().Invoke(null, ArrayUtil.Concat(obj, args));
            }

#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

#if EMITTERS

            internal override void EmitCallvirtReflect(CodeEmitter ilgen)
            {
                var mb = mbHelper ?? GetMethod();
                ilgen.Emit(mb.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mb);
            }

#endif // EMITTERS

            internal string GetGenericSignature()
            {
                var attr = DeclaringType.Context.AttributeHelper.GetSignature(mbHelper != null ? mbHelper : GetMethod());
                if (attr != null)
                    return attr.Signature;

                return null;
            }

        }

    }

}
