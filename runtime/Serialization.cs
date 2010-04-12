/*
  Copyright (C) 2009 Jeroen Frijters

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
using System.Security.Permissions;
#if STATIC_COMPILER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{
	// This class deals with .NET serialization. When a class is Java serializable it will attempt to automagically make it .NET serializable.
	public static class Serialization
	{
		private static readonly CustomAttributeBuilder serializableAttribute = new CustomAttributeBuilder(JVM.Import(typeof(SerializableAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
		private static readonly CustomAttributeBuilder securityCriticalAttribute = new CustomAttributeBuilder(JVM.Import(typeof(SecurityCriticalAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
		private static readonly TypeWrapper iserializable = ClassLoaderWrapper.GetWrapperFromType(JVM.Import(typeof(ISerializable)));
		private static readonly TypeWrapper iobjectreference = ClassLoaderWrapper.GetWrapperFromType(JVM.Import(typeof(IObjectReference)));
		private static readonly TypeWrapper serializable = ClassLoaderWrapper.LoadClassCritical("java.io.Serializable");
		private static readonly TypeWrapper externalizable = ClassLoaderWrapper.LoadClassCritical("java.io.Externalizable");
		private static readonly PermissionSet psetSerializationFormatter;

		static Serialization()
		{
			psetSerializationFormatter = new PermissionSet(PermissionState.None);
			psetSerializationFormatter.AddPermission(new SecurityPermission(SecurityPermissionFlag.SerializationFormatter));
		}

		private static bool IsSafeForAutomagicSerialization(TypeWrapper wrapper)
		{
			if (wrapper.TypeAsBaseType.IsSerializable)
			{
				return false;
			}
			if (wrapper.IsSubTypeOf(iserializable))
			{
				return false;
			}
			if (wrapper.IsSubTypeOf(iobjectreference))
			{
				return false;
			}
			if (wrapper.GetMethodWrapper("GetObjectData", "(Lcli.System.Runtime.Serialization.SerializationInfo;Lcli.System.Runtime.Serialization.StreamingContext;)V", false) != null)
			{
				return false;
			}
			if (wrapper.GetMethodWrapper("<init>", "(Lcli.System.Runtime.Serialization.SerializationInfo;Lcli.System.Runtime.Serialization.StreamingContext;)V", false) != null)
			{
				return false;
			}
			return true;
		}

		internal static ConstructorInfo AddAutomagicSerialization(TypeWrapper wrapper)
		{
			ConstructorInfo serializationCtor = null;
			if ((wrapper.Modifiers & IKVM.Attributes.Modifiers.Enum) != 0)
			{
				MarkSerializable(wrapper);
			}
			else if (wrapper.IsSubTypeOf(serializable) && IsSafeForAutomagicSerialization(wrapper))
			{
				if (wrapper.IsSubTypeOf(externalizable))
				{
					MethodWrapper ctor = wrapper.GetMethodWrapper("<init>", "()V", false);
					if (ctor != null && ctor.IsPublic)
					{
						MarkSerializable(wrapper);
						ctor.Link();
						serializationCtor = AddConstructor(wrapper.TypeAsBuilder, ctor, null, true);
						if (!wrapper.BaseTypeWrapper.IsSubTypeOf(serializable))
						{
							AddGetObjectData(wrapper);
						}
						if (wrapper.BaseTypeWrapper.GetMethodWrapper("readResolve", "()Ljava.lang.Object;", true) != null)
						{
							RemoveReadResolve(wrapper);
						}
					}
				}
				else if (wrapper.BaseTypeWrapper.IsSubTypeOf(serializable))
				{
					ConstructorInfo baseCtor = wrapper.GetBaseSerializationConstructor();
					if (baseCtor != null && (baseCtor.IsFamily || baseCtor.IsFamilyOrAssembly))
					{
						MarkSerializable(wrapper);
						serializationCtor = AddConstructor(wrapper.TypeAsBuilder, null, baseCtor, false);
						AddReadResolve(wrapper);
					}
				}
				else
				{
					MethodWrapper baseCtor = wrapper.BaseTypeWrapper.GetMethodWrapper("<init>", "()V", false);
					if (baseCtor != null && baseCtor.IsAccessibleFrom(wrapper.BaseTypeWrapper, wrapper, wrapper))
					{
						MarkSerializable(wrapper);
						AddGetObjectData(wrapper);
#if STATIC_COMPILER
						// because the base type can be a __WorkaroundBaseClass__, we may need to replace the constructor
						baseCtor = ((AotTypeWrapper)wrapper).ReplaceMethodWrapper(baseCtor);
#endif
						baseCtor.Link();
						serializationCtor = AddConstructor(wrapper.TypeAsBuilder, baseCtor, null, true);
						AddReadResolve(wrapper);
					}
				}
			}
			return serializationCtor;
		}

		internal static ConstructorInfo AddAutomagicSerializationToWorkaroundBaseClass(TypeBuilder typeBuilderWorkaroundBaseClass, ConstructorInfo baseCtor)
		{
			if (typeBuilderWorkaroundBaseClass.BaseType.IsSerializable)
			{
				typeBuilderWorkaroundBaseClass.SetCustomAttribute(serializableAttribute);
				if (baseCtor != null && (baseCtor.IsFamily || baseCtor.IsFamilyOrAssembly))
				{
					return AddConstructor(typeBuilderWorkaroundBaseClass, null, baseCtor, false);
				}
			}
			return null;
		}

		private static void MarkSerializable(TypeWrapper wrapper)
		{
			TypeBuilder tb = wrapper.TypeAsBuilder;
			tb.SetCustomAttribute(serializableAttribute);
		}

		private static void AddGetObjectData(TypeWrapper wrapper)
		{
			TypeBuilder tb = wrapper.TypeAsBuilder;
			tb.AddInterfaceImplementation(JVM.Import(typeof(ISerializable)));
			MethodBuilder getObjectData = tb.DefineMethod("GetObjectData", MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.NewSlot, null,
				new Type[] { JVM.Import(typeof(SerializationInfo)), JVM.Import(typeof(StreamingContext)) });
			getObjectData.SetCustomAttribute(securityCriticalAttribute);
			AttributeHelper.HideFromJava(getObjectData);
			getObjectData.AddDeclarativeSecurity(SecurityAction.Demand, psetSerializationFormatter);
			tb.DefineMethodOverride(getObjectData, JVM.Import(typeof(ISerializable)).GetMethod("GetObjectData"));
			CodeEmitter ilgen = CodeEmitter.Create(getObjectData);
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ldarg_1);
			TypeWrapper serializationHelper = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.Serialization");
			MethodWrapper mw = serializationHelper.GetMethodWrapper("writeObject", "(Ljava.lang.Object;Lcli.System.Runtime.Serialization.SerializationInfo;)V", false);
			mw.Link();
			mw.EmitCall(ilgen);
			ilgen.Emit(OpCodes.Ret);
		}

		private static ConstructorInfo AddConstructor(TypeBuilder tb, MethodWrapper defaultConstructor, ConstructorInfo serializationConstructor, bool callReadObject)
		{
			ConstructorBuilder ctor = tb.DefineConstructor(MethodAttributes.Family, CallingConventions.Standard, new Type[] { JVM.Import(typeof(SerializationInfo)), JVM.Import(typeof(StreamingContext)) });
			AttributeHelper.HideFromJava(ctor);
			ctor.AddDeclarativeSecurity(SecurityAction.Demand, psetSerializationFormatter);
			CodeEmitter ilgen = CodeEmitter.Create(ctor);
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
				TypeWrapper serializationHelper = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.Serialization");
				MethodWrapper mw = serializationHelper.GetMethodWrapper("readObject", "(Ljava.lang.Object;Lcli.System.Runtime.Serialization.SerializationInfo;)V", false);
				mw.Link();
				mw.EmitCall(ilgen);
			}
			ilgen.Emit(OpCodes.Ret);
			return ctor;
		}

		private static void AddReadResolve(TypeWrapper wrapper)
		{
			MethodWrapper mw = wrapper.GetMethodWrapper("readResolve", "()Ljava.lang.Object;", false);
			if (mw != null && !wrapper.IsSubTypeOf(iobjectreference))
			{
				TypeBuilder tb = wrapper.TypeAsBuilder;
				tb.AddInterfaceImplementation(JVM.Import(typeof(IObjectReference)));
				MethodBuilder getRealObject = tb.DefineMethod("IObjectReference.GetRealObject", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final,
					Types.Object, new Type[] { JVM.Import(typeof(StreamingContext)) });
				getRealObject.SetCustomAttribute(securityCriticalAttribute);
				AttributeHelper.HideFromJava(getRealObject);
				tb.DefineMethodOverride(getRealObject, JVM.Import(typeof(IObjectReference)).GetMethod("GetRealObject"));
				CodeEmitter ilgen = CodeEmitter.Create(getRealObject);
				mw.Link();
				ilgen.Emit(OpCodes.Ldarg_0);
				mw.EmitCall(ilgen);
				ilgen.Emit(OpCodes.Ret);
			}
		}

		private static void RemoveReadResolve(TypeWrapper wrapper)
		{
			TypeBuilder tb = wrapper.TypeAsBuilder;
			tb.AddInterfaceImplementation(JVM.Import(typeof(IObjectReference)));
			MethodBuilder getRealObject = tb.DefineMethod("IObjectReference.GetRealObject", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final,
				Types.Object, new Type[] { JVM.Import(typeof(StreamingContext)) });
			getRealObject.SetCustomAttribute(securityCriticalAttribute);
			AttributeHelper.HideFromJava(getRealObject);
			tb.DefineMethodOverride(getRealObject, JVM.Import(typeof(IObjectReference)).GetMethod("GetRealObject"));
			CodeEmitter ilgen = CodeEmitter.Create(getRealObject);
			ilgen.Emit(OpCodes.Ldarg_0);
			ilgen.Emit(OpCodes.Ret);
		}
	}
}
