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
using System.Reflection;
using System.Text;

using IKVM.Attributes;

namespace IKVM.Runtime
{

#if !IMPORTER && !EXPORTER

    /// <summary>
    /// Represents an intrinsified anonymous class. Currently only used by LambdaMetafactory.
    /// </summary>
    sealed class RuntimeAnonymousJavaType : RuntimeJavaType
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        internal RuntimeAnonymousJavaType(RuntimeContext context, Type type) :
            base(context, TypeFlags.Anonymous, Modifiers.Final | Modifiers.Synthetic, GetName(context, type))
        {
            this.type = type;
        }

        internal static bool IsAnonymous(RuntimeContext context, Type type)
        {
            return type.IsSpecialName && type.Name.StartsWith(NestedTypeName.IntrinsifiedAnonymousClass, StringComparison.Ordinal) && context.AttributeHelper.IsJavaModule(type.Module);
        }

        private static string GetName(RuntimeContext context, Type type)
        {
            return context.ClassLoaderFactory.GetJavaTypeFromType(type.DeclaringType).Name + type.Name.Replace(NestedTypeName.IntrinsifiedAnonymousClass, "$$Lambda$");
        }

        internal override RuntimeClassLoader ClassLoader => Context.ClassLoaderFactory.GetJavaTypeFromType(type.DeclaringType).ClassLoader;

        internal override Type TypeAsTBD
        {
            get { return type; }
        }

        internal override RuntimeJavaType BaseTypeWrapper
        {
            get { return Context.JavaBase.TypeOfJavaLangObject; }
        }

        internal override RuntimeJavaType[] Interfaces
        {
            get
            {
                var interfaces = GetImplementedInterfacesAsTypeWrappers(Context, type);
                if (type.IsSerializable)
                {
                    // we have to remove the System.Runtime.Serialization.ISerializable interface
                    var list = new List<RuntimeJavaType>(interfaces);
                    list.RemoveAll(Context.Serialization.IsISerializable);
                    return list.ToArray();
                }

                return interfaces;
            }
        }

        protected override void LazyPublishMembers()
        {
            var methods = new List<RuntimeJavaMethod>();
            foreach (var mi in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (mi.IsSpecialName)
                {
                    // we use special name to hide default methods
                }
                else if (mi.IsPublic)
                {
                    GetSig(mi, out var returnType, out var parameterTypes, out var signature);
                    methods.Add(new RuntimeTypicalJavaMethod(this, mi.Name, signature, mi, returnType, parameterTypes, Modifiers.Public, MemberFlags.None));
                }
                else if (mi.Name == "writeReplace")
                {
                    methods.Add(new RuntimeTypicalJavaMethod(this, "writeReplace", "()Ljava.lang.Object;", mi, Context.JavaBase.TypeOfJavaLangObject, [], Modifiers.Private | Modifiers.Final, MemberFlags.None));
                }
            }

            SetMethods(methods.ToArray());

            var fields = new List<RuntimeJavaField>();
            foreach (var fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                var fieldType = RuntimeManagedByteCodeJavaType.GetFieldTypeWrapper(Context, fi);
                fields.Add(new RuntimeSimpleJavaField(this, fieldType, fi, fi.Name, fieldType.SignatureName, new ExModifiers(Modifiers.Private | Modifiers.Final, false)));
            }

            SetFields(fields.ToArray());
        }

        void GetSig(MethodInfo mi, out RuntimeJavaType returnType, out RuntimeJavaType[] parameterTypes, out string signature)
        {
            returnType = RuntimeManagedByteCodeJavaType.GetParameterTypeWrapper(Context, mi.ReturnParameter);
            var parameters = mi.GetParameters();
            parameterTypes = new RuntimeJavaType[parameters.Length];

            var sb = new ValueStringBuilder(256);
            sb.Append("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = RuntimeManagedByteCodeJavaType.GetParameterTypeWrapper(Context, parameters[i]);
                sb.Append(parameterTypes[i].SignatureName);
            }
            sb.Append(')');
            sb.Append(returnType.SignatureName);
            signature = sb.ToString();
        }
    }

#endif

}
