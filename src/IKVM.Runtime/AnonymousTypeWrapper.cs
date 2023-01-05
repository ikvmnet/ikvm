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

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Internal
{

#if !IMPORTER && !EXPORTER

    /// <summary>
    /// Represents an intrinsified anonymous class. Currently only used by LambdaMetafactory.
    /// </summary>
    sealed class AnonymousTypeWrapper : TypeWrapper
    {

        readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        internal AnonymousTypeWrapper(Type type) :
            base(TypeFlags.Anonymous, Modifiers.Final | Modifiers.Synthetic, GetName(type))
        {
            this.type = type;
        }

        internal static bool IsAnonymous(Type type)
        {
            return type.IsSpecialName
                && type.Name.StartsWith(NestedTypeName.IntrinsifiedAnonymousClass, StringComparison.Ordinal)
                && AttributeHelper.IsJavaModule(type.Module);
        }

        private static string GetName(Type type)
        {
            return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).Name
                + type.Name.Replace(NestedTypeName.IntrinsifiedAnonymousClass, "$$Lambda$");
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetClassLoader();
        }

        internal override Type TypeAsTBD
        {
            get { return type; }
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return CoreClasses.java.lang.Object.Wrapper; }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                TypeWrapper[] interfaces = GetImplementedInterfacesAsTypeWrappers(type);
                if (type.IsSerializable)
                {
                    // we have to remove the System.Runtime.Serialization.ISerializable interface
                    List<TypeWrapper> list = new List<TypeWrapper>(interfaces);
                    list.RemoveAll(Serialization.IsISerializable);
                    return list.ToArray();
                }
                return interfaces;
            }
        }

        protected override void LazyPublishMembers()
        {
            List<MethodWrapper> methods = new List<MethodWrapper>();
            foreach (MethodInfo mi in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                if (mi.IsSpecialName)
                {
                    // we use special name to hide default methods
                }
                else if (mi.IsPublic)
                {
                    TypeWrapper returnType;
                    TypeWrapper[] parameterTypes;
                    string signature;
                    GetSig(mi, out returnType, out parameterTypes, out signature);
                    methods.Add(new TypicalMethodWrapper(this, mi.Name, signature, mi, returnType, parameterTypes, Modifiers.Public, MemberFlags.None));
                }
                else if (mi.Name == "writeReplace")
                {
                    methods.Add(new TypicalMethodWrapper(this, "writeReplace", "()Ljava.lang.Object;", mi, CoreClasses.java.lang.Object.Wrapper, TypeWrapper.EmptyArray,
                        Modifiers.Private | Modifiers.Final, MemberFlags.None));
                }
            }
            SetMethods(methods.ToArray());
            List<FieldWrapper> fields = new List<FieldWrapper>();
            foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                TypeWrapper fieldType = CompiledTypeWrapper.GetFieldTypeWrapper(fi);
                fields.Add(new SimpleFieldWrapper(this, fieldType, fi, fi.Name, fieldType.SigName, new ExModifiers(Modifiers.Private | Modifiers.Final, false)));
            }
            SetFields(fields.ToArray());
        }

        private void GetSig(MethodInfo mi, out TypeWrapper returnType, out TypeWrapper[] parameterTypes, out string signature)
        {
            returnType = CompiledTypeWrapper.GetParameterTypeWrapper(mi.ReturnParameter);
            ParameterInfo[] parameters = mi.GetParameters();
            parameterTypes = new TypeWrapper[parameters.Length];
            System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
            for (int i = 0; i < parameters.Length; i++)
            {
                parameterTypes[i] = CompiledTypeWrapper.GetParameterTypeWrapper(parameters[i]);
                sb.Append(parameterTypes[i].SigName);
            }
            sb.Append(')');
            sb.Append(returnType.SigName);
            signature = sb.ToString();
        }
    }

#endif

}
