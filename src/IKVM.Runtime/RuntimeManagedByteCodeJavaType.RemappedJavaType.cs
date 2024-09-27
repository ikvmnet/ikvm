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
using System.Collections.Generic;

using IKVM.Attributes;
using IKVM.CoreLib.Symbols;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeManagedByteCodeJavaType
    {

        internal sealed class RemappedJavaType : RuntimeManagedByteCodeJavaType
        {

            readonly ITypeSymbol remappedType;

            /// <summary>
            /// initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="name"></param>
            /// <param name="type"></param>
            /// <exception cref="InvalidOperationException"></exception>
            internal RemappedJavaType(RuntimeContext context, string name, ITypeSymbol type) :
                base(context, name, type)
            {
                var attr = Context.AttributeHelper.GetRemappedType(type) ?? throw new InvalidOperationException();
                remappedType = Context.Resolver.GetSymbol(attr.Type);
            }

            internal override ITypeSymbol TypeAsTBD => remappedType;

            internal override bool IsRemapped => true;

            protected override void LazyPublishMethods()
            {
                const System.Reflection.BindingFlags bindingFlags =
                    System.Reflection.BindingFlags.DeclaredOnly |
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Static |
                    System.Reflection.BindingFlags.Instance;

                var list = new List<RuntimeJavaMethod>();

                foreach (var ctor in type.GetConstructors(bindingFlags))
                    AddMethod(list, ctor);

                foreach (var method in type.GetMethods(bindingFlags))
                    AddMethod(list, method);

                // if we're a remapped interface, we need to get the methods from the real interface
                if (remappedType.IsInterface)
                {
                    var nestedHelper = type.GetNestedType("__Helper", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    foreach (var m in Context.AttributeHelper.GetRemappedInterfaceMethods(type))
                    {
                        var method = remappedType.GetMethod(m.MappedTo);
                        var mbHelper = method;
                        var modifiers = Context.AttributeHelper.GetModifiers(method, false);
                        var flags = MemberFlags.None;

                        GetNameSigFromMethodBase(method, out var name, out var sig, out var retType, out var paramTypes, ref flags);
                        if (nestedHelper != null)
                        {
                            mbHelper = nestedHelper.GetMethod(m.Name);
                            if (mbHelper == null)
                                mbHelper = method;
                        }

                        var mw = new RemappedJavaMethod(this, m.Name, sig, method, retType, paramTypes, modifiers, false, mbHelper, null);
                        mw.SetDeclaredExceptions(m.Throws);
                        list.Add(mw);
                    }
                }

                SetMethods(list.ToArray());
            }

            private void AddMethod(List<RuntimeJavaMethod> list, IMethodBaseSymbol method)
            {
                var flags = Context.AttributeHelper.GetHideFromJavaFlags(method);
                if ((flags & HideFromJavaFlags.Code) == 0 && (remappedType.IsSealed || !method.Name.StartsWith("instancehelper_")) && (!remappedType.IsSealed || method.IsStatic))
                    list.Add(CreateRemappedMethodWrapper(method, flags));
            }

            protected override void LazyPublishFields()
            {
                var list = new List<RuntimeJavaField>();
                var fields = type.GetFields(System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Instance);
                foreach (var field in fields)
                {
                    var hideFromJavaFlags = Context.AttributeHelper.GetHideFromJavaFlags(field);
                    if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                        list.Add(CreateFieldWrapper(field, hideFromJavaFlags));
                }

                SetFields(list.ToArray());
            }

            RuntimeJavaMethod CreateRemappedMethodWrapper(IMethodBaseSymbol mb, HideFromJavaFlags hideFromJavaflags)
            {
                var modifiers = Context.AttributeHelper.GetModifiers(mb, false);
                var flags = MemberFlags.None;
                GetNameSigFromMethodBase(mb, out var name, out var sig, out var retType, out var paramTypes, ref flags);

                var mbHelper = mb as IMethodSymbol;
                var hideFromReflection = mbHelper != null && (hideFromJavaflags & HideFromJavaFlags.Reflection) != 0;
                IMethodSymbol mbNonvirtualHelper = null;
                if (!mb.IsStatic && !mb.IsConstructor)
                {
                    var parameters = mb.GetParameters();
                    var argTypes = new ITypeSymbol[parameters.Length + 1];
                    argTypes[0] = remappedType;
                    for (int i = 0; i < parameters.Length; i++)
                        argTypes[i + 1] = parameters[i].ParameterType;

                    var helper = type.GetMethod("instancehelper_" + mb.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static, argTypes);
                    if (helper != null)
                        mbHelper = helper;

                    mbNonvirtualHelper = type.GetMethod("nonvirtualhelper/" + mb.Name, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static, argTypes);
                }

                return new RemappedJavaMethod(this, name, sig, mb, retType, paramTypes, modifiers, hideFromReflection, mbHelper, mbNonvirtualHelper);
            }
        }

    }

}
