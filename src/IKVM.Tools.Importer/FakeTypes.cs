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
using System;
using System.Reflection;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.Runtime;

namespace IKVM.Tools.Importer
{

    class FakeTypes
    {

        readonly RuntimeContext context;

        ITypeSymbol genericEnumEnumType;
        ITypeSymbol genericDelegateInterfaceType;
        ITypeSymbol genericAttributeAnnotationType;
        ITypeSymbol genericAttributeAnnotationMultipleType;
        ITypeSymbol genericAttributeAnnotationReturnValueType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public FakeTypes(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        internal ITypeSymbol GetEnumType(ITypeSymbol enumType)
        {
            return genericEnumEnumType.MakeGenericType(enumType);
        }

        internal ITypeSymbol GetDelegateType(ITypeSymbol delegateType)
        {
            return genericDelegateInterfaceType.MakeGenericType(delegateType);
        }

        internal ITypeSymbol GetAttributeType(ITypeSymbol attributeType)
        {
            return genericAttributeAnnotationType.MakeGenericType(attributeType);
        }

        internal ITypeSymbol GetAttributeMultipleType(ITypeSymbol attributeType)
        {
            return genericAttributeAnnotationMultipleType.MakeGenericType(attributeType);
        }

        internal ITypeSymbol GetAttributeReturnValueType(ITypeSymbol attributeType)
        {
            return genericAttributeAnnotationReturnValueType.MakeGenericType(attributeType);
        }

        internal void Create(IModuleSymbolBuilder modb, RuntimeClassLoader loader)
        {
            var tb = modb.DefineType(RuntimeManagedJavaType.GenericDelegateInterfaceTypeName, TypeAttributes.Interface | System.Reflection.TypeAttributes.Abstract | System.Reflection.TypeAttributes.Public);
            tb.DefineGenericParameters("T")[0].SetBaseTypeConstraint(context.Types.MulticastDelegate);
            tb.Complete();

            genericDelegateInterfaceType = tb;
            genericAttributeAnnotationType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationTypeName);
            genericAttributeAnnotationMultipleType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationMultipleTypeName);
            genericAttributeAnnotationReturnValueType = CreateAnnotationType(modb, RuntimeManagedJavaType.GenericAttributeAnnotationReturnValueTypeName);
            CreateEnumEnum(modb, loader);
        }

        internal void Finish(RuntimeClassLoader loader)
        {
            var tb = (ITypeSymbolBuilder)genericEnumEnumType;
            var enumTypeWrapper = loader.LoadClassByName("java.lang.Enum");
            enumTypeWrapper.Finish();
            tb.SetParent(enumTypeWrapper.TypeAsBaseType);
            var ilgen = context.CodeEmitterFactory.Create(ReflectUtil.DefineConstructor(tb, MethodAttributes.Private, [context.Types.String, context.Types.Int32]));
            ilgen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
            ilgen.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
            ilgen.Emit(System.Reflection.Emit.OpCodes.Ldarg_2);
            enumTypeWrapper.GetMethodWrapper("<init>", "(Ljava.lang.String;I)V", false).EmitCall(ilgen);
            ilgen.Emit(System.Reflection.Emit.OpCodes.Ret);
            ilgen.DoEmit();
            tb.Complete();
            genericEnumEnumType = tb;
        }

        void CreateEnumEnum(IModuleSymbolBuilder modb, RuntimeClassLoader loader)
        {
            var tb = modb.DefineType(RuntimeManagedJavaType.GenericEnumEnumTypeName, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.Public);
            var gtpb = tb.DefineGenericParameters("T")[0];
            gtpb.SetBaseTypeConstraint(context.Types.Enum);
            genericEnumEnumType = tb;
        }

        ITypeSymbol CreateAnnotationType(IModuleSymbolBuilder modb, string name)
        {
            var tb = modb.DefineType(name, TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.Public);
            tb.DefineGenericParameters("T")[0].SetBaseTypeConstraint(context.Types.Attribute);
            tb.Complete();
            return tb;
        }

        internal void Load(IAssemblySymbol assembly)
        {
            genericEnumEnumType = assembly.GetType(RuntimeManagedJavaType.GenericEnumEnumTypeName);
            genericDelegateInterfaceType = assembly.GetType(RuntimeManagedJavaType.GenericDelegateInterfaceTypeName);
            genericAttributeAnnotationType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationTypeName);
            genericAttributeAnnotationMultipleType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationMultipleTypeName);
            genericAttributeAnnotationReturnValueType = assembly.GetType(RuntimeManagedJavaType.GenericAttributeAnnotationReturnValueTypeName);
        }

    }

}
