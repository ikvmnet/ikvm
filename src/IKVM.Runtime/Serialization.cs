/*
  Copyright (C) 2009-2011 Jeroen Frijters

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
using System.Runtime.Serialization;
using System.Security;

using IKVM.Attributes;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    class Serialization
    {

        readonly RuntimeContext context;

        CustomAttributeBuilder serializableAttribute;
        CustomAttributeBuilder securityCriticalAttribute;
        RuntimeJavaType typeOfISerializable;
        RuntimeJavaType typeofIObjectreference;
        RuntimeJavaType typeOfExternalizable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public Serialization(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        CustomAttributeBuilder SerializableAttribute => serializableAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(SerializableAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());

        CustomAttributeBuilder SecurityCriticalAttribute => securityCriticalAttribute ??= new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(SecurityCriticalAttribute).FullName).GetConstructor([]).AsReflection(), Array.Empty<object>());

        RuntimeJavaType TypeOfISerializable => typeOfISerializable ??= context.ClassLoaderFactory.GetJavaTypeFromType(context.Resolver.ResolveCoreType(typeof(ISerializable).FullName).AsReflection());

        RuntimeJavaType TypeOfIObjectReference => typeofIObjectreference ??= context.ClassLoaderFactory.GetJavaTypeFromType(context.Resolver.ResolveCoreType(typeof(IObjectReference).FullName).AsReflection());

        RuntimeJavaType TypeOfExternalizable => typeOfExternalizable ??= context.ClassLoaderFactory.LoadClassCritical("java.io.Externalizable");

        internal bool IsISerializable(RuntimeJavaType wrapper)
        {
            return wrapper == TypeOfISerializable;
        }

        bool IsSafeForAutomagicSerialization(RuntimeJavaType wrapper)
        {
            if (wrapper.TypeAsBaseType.IsSerializable)
                return false;
            else if (wrapper.IsSubTypeOf(TypeOfISerializable))
                return false;
            else if (wrapper.IsSubTypeOf(TypeOfIObjectReference))
                return false;
            else if (wrapper.GetMethod("GetObjectData", "(Lcli.System.Runtime.Serialization.SerializationInfo;Lcli.System.Runtime.Serialization.StreamingContext;)V", false) != null)
                return false;
            else if (wrapper.GetMethod("<init>", "(Lcli.System.Runtime.Serialization.SerializationInfo;Lcli.System.Runtime.Serialization.StreamingContext;)V", false) != null)
                return false;
            else
                return true;
        }

        internal MethodBuilder AddAutomagicSerialization(RuntimeByteCodeJavaType wrapper, TypeBuilder typeBuilder)
        {
            MethodBuilder serializationCtor = null;
            if ((wrapper.Modifiers & Modifiers.Enum) != 0)
            {
                MarkSerializable(typeBuilder);
            }
            else if (wrapper.IsSubTypeOf(wrapper.Context.JavaBase.TypeOfJavaIoSerializable) && IsSafeForAutomagicSerialization(wrapper))
            {
                if (wrapper.IsSubTypeOf(TypeOfExternalizable))
                {
                    var ctor = wrapper.GetMethod("<init>", "()V", false);
                    if (ctor != null && ctor.IsPublic)
                    {
                        MarkSerializable(typeBuilder);
                        ctor.Link();

                        serializationCtor = AddConstructor(typeBuilder, ctor, null, true);
                        if (!wrapper.BaseTypeWrapper.IsSubTypeOf(wrapper.Context.JavaBase.TypeOfJavaIoSerializable))
                        {
                            AddGetObjectData(typeBuilder);
                        }
                        if (wrapper.BaseTypeWrapper.GetMethod("readResolve", "()Ljava.lang.Object;", true) != null)
                        {
                            RemoveReadResolve(typeBuilder);
                        }
                    }
                }
                else if (wrapper.BaseTypeWrapper.IsSubTypeOf(wrapper.Context.JavaBase.TypeOfJavaIoSerializable))
                {
                    var baseCtor = wrapper.GetBaseSerializationConstructor();
                    if (baseCtor != null && (baseCtor.IsFamily || baseCtor.IsFamilyOrAssembly))
                    {
                        MarkSerializable(typeBuilder);
                        serializationCtor = AddConstructor(typeBuilder, null, baseCtor, false);
                        AddReadResolve(wrapper, typeBuilder);
                    }
                }
                else
                {
                    var baseCtor = wrapper.BaseTypeWrapper.GetMethod("<init>", "()V", false);
                    if (baseCtor != null && baseCtor.IsAccessibleFrom(wrapper.BaseTypeWrapper, wrapper, wrapper))
                    {
                        MarkSerializable(typeBuilder);
                        AddGetObjectData(typeBuilder);
#if IMPORTER
                        // because the base type can be a __WorkaroundBaseClass__, we may need to replace the constructor
                        baseCtor = ((RuntimeImportByteCodeJavaType)wrapper).ReplaceMethodWrapper(baseCtor);
#endif
                        baseCtor.Link();
                        serializationCtor = AddConstructor(typeBuilder, baseCtor, null, true);
                        AddReadResolve(wrapper, typeBuilder);
                    }
                }
            }

            return serializationCtor;
        }

        internal MethodBuilder AddAutomagicSerializationToWorkaroundBaseClass(TypeBuilder typeBuilderWorkaroundBaseClass, MethodBase baseCtor)
        {
            if (typeBuilderWorkaroundBaseClass.BaseType.IsSerializable)
            {
                typeBuilderWorkaroundBaseClass.SetCustomAttribute(SerializableAttribute);
                if (baseCtor != null && (baseCtor.IsFamily || baseCtor.IsFamilyOrAssembly))
                    return AddConstructor(typeBuilderWorkaroundBaseClass, null, baseCtor, false);
            }

            return null;
        }

        internal void MarkSerializable(TypeBuilder tb)
        {
            tb.SetCustomAttribute(SerializableAttribute);
        }

        internal void AddGetObjectData(TypeBuilder tb)
        {
            var name = tb.IsSealed ? "System.Runtime.Serialization.ISerializable.GetObjectData" : "GetObjectData";
            var attr = tb.IsSealed
                ? MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final
                : MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
            tb.AddInterfaceImplementation(context.Resolver.ResolveCoreType(typeof(ISerializable).FullName).AsReflection());
            var getObjectData = tb.DefineMethod(name, attr, null, new Type[] { context.Resolver.ResolveCoreType(typeof(SerializationInfo).FullName).AsReflection(), context.Resolver.ResolveCoreType(typeof(StreamingContext).FullName).AsReflection() });
            getObjectData.SetCustomAttribute(SecurityCriticalAttribute);
            context.AttributeHelper.HideFromJava(getObjectData);
            tb.DefineMethodOverride(getObjectData, context.Resolver.ResolveCoreType(typeof(ISerializable).FullName).GetMethod("GetObjectData").AsReflection());
            CodeEmitter ilgen = context.CodeEmitterFactory.Create(getObjectData);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldarg_1);
            RuntimeJavaType serializationHelper = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.Serialization");
            RuntimeJavaMethod mw = serializationHelper.GetMethod("writeObject", "(Ljava.lang.Object;Lcli.System.Runtime.Serialization.SerializationInfo;)V", false);
            mw.Link();
            mw.EmitCall(ilgen);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        MethodBuilder AddConstructor(TypeBuilder tb, RuntimeJavaMethod defaultConstructor, MethodBase serializationConstructor, bool callReadObject)
        {
            MethodBuilder ctor = ReflectUtil.DefineConstructor(tb, MethodAttributes.Family, new Type[] { context.Resolver.ResolveCoreType(typeof(SerializationInfo).FullName).AsReflection(), context.Resolver.ResolveCoreType(typeof(StreamingContext).FullName).AsReflection() });
            context.AttributeHelper.HideFromJava(ctor);
            CodeEmitter ilgen = context.CodeEmitterFactory.Create(ctor);
            ilgen.Emit(OpCodes.Ldarg_0);
            if (defaultConstructor != null)
            {
                defaultConstructor.EmitCall(ilgen);
            }
            else
            {
                ilgen.Emit(OpCodes.Ldarg_1);
                ilgen.Emit(OpCodes.Ldarg_2);
                ilgen.Emit(OpCodes.Call, serializationConstructor);
            }
            if (callReadObject)
            {
                ilgen.Emit(OpCodes.Ldarg_0);
                ilgen.Emit(OpCodes.Ldarg_1);
                RuntimeJavaType serializationHelper = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.Serialization");
                RuntimeJavaMethod mw = serializationHelper.GetMethod("readObject", "(Ljava.lang.Object;Lcli.System.Runtime.Serialization.SerializationInfo;)V", false);
                mw.Link();
                mw.EmitCall(ilgen);
            }
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
            return ctor;
        }

        void AddReadResolve(RuntimeByteCodeJavaType wrapper, TypeBuilder tb)
        {
            var mw = wrapper.GetMethod("readResolve", "()Ljava.lang.Object;", false);
            if (mw != null && !wrapper.IsSubTypeOf(TypeOfIObjectReference))
            {
                tb.AddInterfaceImplementation(wrapper.Context.Resolver.ResolveCoreType(typeof(IObjectReference).FullName).AsReflection());
                var getRealObject = tb.DefineMethod("IObjectReference.GetRealObject", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final, wrapper.Context.Types.Object, new Type[] { wrapper.Context.Resolver.ResolveCoreType(typeof(StreamingContext).FullName).AsReflection() });
                getRealObject.SetCustomAttribute(SecurityCriticalAttribute);
                wrapper.Context.AttributeHelper.HideFromJava(getRealObject);
                tb.DefineMethodOverride(getRealObject, wrapper.Context.Resolver.ResolveCoreType(typeof(IObjectReference).FullName).GetMethod("GetRealObject").AsReflection());
                var ilgen = context.CodeEmitterFactory.Create(getRealObject);
                mw.Link();
                if (!wrapper.IsFinal)
                {
                    // readResolve is only applicable if it exists on the actual type of the object, so if we're a subclass don't call it
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Callvirt, wrapper.Context.CompilerFactory.GetTypeMethod);
                    ilgen.Emit(OpCodes.Ldtoken, wrapper.TypeAsBaseType);
                    ilgen.Emit(OpCodes.Call, wrapper.Context.CompilerFactory.GetTypeFromHandleMethod);
                    CodeEmitterLabel label = ilgen.DefineLabel();
                    ilgen.EmitBeq(label);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.MarkLabel(label);
                }
                ilgen.Emit(OpCodes.Ldarg_0);
                mw.EmitCall(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }
        }

        void RemoveReadResolve(TypeBuilder tb)
        {
            tb.AddInterfaceImplementation(context.Resolver.ResolveCoreType(typeof(IObjectReference).FullName).AsReflection());
            MethodBuilder getRealObject = tb.DefineMethod("IObjectReference.GetRealObject", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final, context.Types.Object, [context.Resolver.ResolveCoreType(typeof(StreamingContext).FullName).AsReflection()]);
            getRealObject.SetCustomAttribute(SecurityCriticalAttribute);
            context.AttributeHelper.HideFromJava(getRealObject);
            tb.DefineMethodOverride(getRealObject, context.Resolver.ResolveCoreType(typeof(IObjectReference).FullName).GetMethod("GetRealObject").AsReflection());
            CodeEmitter ilgen = context.CodeEmitterFactory.Create(getRealObject);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

    }

}
