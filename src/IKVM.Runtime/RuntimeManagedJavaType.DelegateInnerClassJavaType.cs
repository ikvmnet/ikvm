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
using IKVM.CoreLib.Symbols;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed class DelegateInnerClassJavaType : FakeJavaType
        {

            readonly ITypeSymbol fakeType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="delegateType"></param>
            internal DelegateInnerClassJavaType(RuntimeContext context, string name, ITypeSymbol delegateType) :
                base(context, Modifiers.Public | Modifiers.Interface | Modifiers.Abstract, name, null)
            {
#if IMPORTER || EXPORTER
                this.fakeType = context.FakeTypes.GetDelegateType(delegateType);
#elif !FIRST_PASS
                this.fakeType = context.Resolver.GetSymbol(typeof(ikvm.@internal.DelegateInterface<>)).MakeGenericType(delegateType);
#endif
                var invoke = delegateType.GetMethod("Invoke");
                var parameters = invoke.GetParameters();
                var argTypeWrappers = new RuntimeJavaType[parameters.Length];
                var sb = new System.Text.StringBuilder("(");
                var flags = MemberFlags.None;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    if (parameterType.IsByRef)
                    {
                        flags |= MemberFlags.DelegateInvokeWithByRefParameter;
                        parameterType = parameterType.GetElementType().MakeArrayType();
                    }

                    argTypeWrappers[i] = Context.ClassLoaderFactory.GetJavaTypeFromType(parameterType);
                    sb.Append(argTypeWrappers[i].SigName);
                }

                var returnType = Context.ClassLoaderFactory.GetJavaTypeFromType(invoke.ReturnType);
                sb.Append(")").Append(returnType.SigName);

                var invokeMethod = new DynamicOnlyJavaMethod(this, "Invoke", sb.ToString(), returnType, argTypeWrappers, flags);
                SetMethods([invokeMethod]);
                SetFields([]);
            }

            internal override RuntimeJavaType DeclaringTypeWrapper => Context.ClassLoaderFactory.GetJavaTypeFromType(fakeType.GetGenericArguments()[0]);

            internal override RuntimeClassLoader ClassLoader => DeclaringTypeWrapper.ClassLoader;

            internal override ITypeSymbol TypeAsTBD => fakeType;

            internal override bool IsFastClassLiteralSafe => true;

            internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
            {
                return DeclaringTypeWrapper.GetMethodParameters(DeclaringTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, false));
            }

        }

    }

}
