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
using System.Text;

using IKVM.Attributes;
using IKVM.CoreLib.Runtime;

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

#if EXPORTER
using IKVM.Tools.Exporter;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed class DelegateInnerClassJavaType : FakeJavaType
        {

            readonly Type fakeType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="delegateType"></param>
            internal DelegateInnerClassJavaType(RuntimeContext context, string name, Type delegateType) :
                base(context, Modifiers.Public | Modifiers.Interface | Modifiers.Abstract, name, null)
            {
#if IMPORTER || EXPORTER
                this.fakeType = context.FakeTypes.GetDelegateType(delegateType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.DelegateInterface<>).MakeGenericType(delegateType);
#endif
                var invoke = delegateType.GetMethod("Invoke");
                var parameters = invoke.GetParameters();
                var argTypeWrappers = new RuntimeJavaType[parameters.Length];
                var sb = new ValueStringBuilder();
                sb.Append("(");
                var flags = MemberFlags.None;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    if (parameterType.IsByRef)
                    {
                        flags |= MemberFlags.DelegateInvokeWithByRefParameter;
                        parameterType = RuntimeArrayJavaType.MakeArrayType(parameterType.GetElementType(), 1);
                    }
                    argTypeWrappers[i] = Context.ClassLoaderFactory.GetJavaTypeFromType(parameterType);
                    sb.Append(argTypeWrappers[i].SignatureName);
                }

                var returnType = Context.ClassLoaderFactory.GetJavaTypeFromType(invoke.ReturnType);
                sb.Append(")");
                sb.Append(returnType.SignatureName);
                SetMethods([new DynamicOnlyJavaMethod(this, "Invoke", sb.ToString(), returnType, argTypeWrappers, flags)]);
                SetFields([]);
            }

            internal override RuntimeJavaType DeclaringTypeWrapper => Context.ClassLoaderFactory.GetJavaTypeFromType(fakeType.GetGenericArguments()[0]);

            internal override RuntimeClassLoader ClassLoader => DeclaringTypeWrapper.ClassLoader;

            internal override Type TypeAsTBD => fakeType;

            internal override bool IsFastClassLiteralSafe => true;

            internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
            {
                return DeclaringTypeWrapper.GetMethodParameters(DeclaringTypeWrapper.GetMethod(mw.Name, mw.Signature, false));
            }

        }

    }

}
