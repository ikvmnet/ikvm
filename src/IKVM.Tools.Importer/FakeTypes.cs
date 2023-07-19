/*
  Copyright (C) 2008 Jeroen Frijters

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
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    static class FakeTypes
    {

        static Type genericEnumEnumType;
        static Type genericDelegateInterfaceType;
        static Type genericAttributeAnnotationType;
        static Type genericAttributeAnnotationMultipleType;
        static Type genericAttributeAnnotationReturnValueType;

        internal static Type GetEnumType(Type enumType)
        {
            return genericEnumEnumType.MakeGenericType(enumType);
        }

        internal static Type GetDelegateType(Type delegateType)
        {
            return genericDelegateInterfaceType.MakeGenericType(delegateType);
        }

        internal static Type GetAttributeType(Type attributeType)
        {
            return genericAttributeAnnotationType.MakeGenericType(attributeType);
        }

        internal static Type GetAttributeMultipleType(Type attributeType)
        {
            return genericAttributeAnnotationMultipleType.MakeGenericType(attributeType);
        }

        internal static Type GetAttributeReturnValueType(Type attributeType)
        {
            return genericAttributeAnnotationReturnValueType.MakeGenericType(attributeType);
        }

        internal static void Create(ModuleBuilder modb, RuntimeClassLoader loader)
        {
            var tb = modb.DefineType(RuntimeManagedJavaType.GenericDelegateInterfaceTypeName, TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);
            tb.DefineGenericParameters("T")[0].SetBaseTypeConstraint(Types.MulticastDelegate);
            genericDelegateInterfaceType = tb.CreateType();

            genericAttributeAnnotationType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationTypeName);
            genericAttributeAnnotationMultipleType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationMultipleTypeName);
            genericAttributeAnnotationReturnValueType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationReturnValueTypeName);
            CreateEnumEnum(modb, loader);
        }

        internal static void Finish(RuntimeClassLoader loader)
        {
            var tb = (TypeBuilder)genericEnumEnumType;
            var enumTypeWrapper = loader.LoadClassByName("java.lang.Enum");
            enumTypeWrapper.Finish();
            tb.SetParent(enumTypeWrapper.TypeAsBaseType);
            var ilgen = CodeEmitter.Create(ReflectUtil.DefineConstructor(tb, MethodAttributes.Private, new Type[] { Types.String, Types.Int32 }));
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldarg_1);
            ilgen.Emit(OpCodes.Ldarg_2);
            enumTypeWrapper.GetMethodWrapper("<init>", "(Ljava.lang.String;I)V", false).EmitCall(ilgen);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
            genericEnumEnumType = tb.CreateType();
        }

        private static void CreateEnumEnum(ModuleBuilder modb, RuntimeClassLoader loader)
        {
            var tb = modb.DefineType(RuntimeManagedJavaType.GenericEnumEnumTypeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public);
            var gtpb = tb.DefineGenericParameters("T")[0];
            gtpb.SetBaseTypeConstraint(Types.Enum);
            genericEnumEnumType = tb;
        }

        private static Type CreateAnnotationType(ModuleBuilder modb, string name)
        {
            var tb = modb.DefineType(name, TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);
            tb.DefineGenericParameters("T")[0].SetBaseTypeConstraint(Types.Attribute);
            return tb.CreateType();
        }

        internal static void Load(Assembly assembly)
        {
            genericEnumEnumType = assembly.GetType(RuntimeManagedJavaType.GenericEnumEnumTypeName);
            genericDelegateInterfaceType = assembly.GetType(RuntimeManagedJavaType.GenericDelegateInterfaceTypeName);
            genericAttributeAnnotationType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationTypeName);
            genericAttributeAnnotationMultipleType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationMultipleTypeName);
            genericAttributeAnnotationReturnValueType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationReturnValueTypeName);
        }

    }

}
