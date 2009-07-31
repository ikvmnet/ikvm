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
#if IKVM_REF_EMIT
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace IKVM.Internal
{
	// This class deals with .NET serialization. When a class is Java serializable it will attempt to automagically make it .NET serializable.
	public static class Serialization
	{
		private static CustomAttributeBuilder serializableAttribute = new CustomAttributeBuilder(typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes), new object[0]);
		private static TypeWrapper iserializable = ClassLoaderWrapper.GetWrapperFromType(typeof(System.Runtime.Serialization.ISerializable));
		private static TypeWrapper serializable = ClassLoaderWrapper.LoadClassCritical("java.io.Serializable");
		private static TypeWrapper externalizable = ClassLoaderWrapper.LoadClassCritical("java.io.Externalizable");
		private static TypeWrapper objectStreamClass = ClassLoaderWrapper.LoadClassCritical("java.io.ObjectStreamClass");
		private static TypeWrapper javaLangEnum = ClassLoaderWrapper.LoadClassCritical("java.lang.Enum");

		private static bool IsTriviallySerializable(TypeWrapper wrapper)
		{
			if ((wrapper.Modifiers & IKVM.Attributes.Modifiers.Enum) != 0)
			{
				return true;
			}
			if (wrapper == CoreClasses.java.lang.Object.Wrapper)
			{
				return true;
			}
			if (!IsTriviallySerializable(wrapper.BaseTypeWrapper))
			{
				return false;
			}
			if (wrapper.IsSubTypeOf(iserializable))
			{
				return false;
			}
			if (wrapper.IsSubTypeOf(serializable))
			{
				if (wrapper.IsSubTypeOf(externalizable))
				{
					return false;
				}
				if (wrapper == objectStreamClass || wrapper == CoreClasses.java.lang.Class.Wrapper || wrapper == javaLangEnum)
				{
					return false;
				}
				if (wrapper.GetMethodWrapper("readObject", "(Ljava.io.ObjectInputStream;)V", false) != null)
				{
					return false;
				}
				if (wrapper.GetMethodWrapper("writeObject", "(Ljava.io.ObjectOutputStream;)V", false) != null)
				{
					return false;
				}
				if (wrapper.GetMethodWrapper("readObjectNoData", "()V", false) != null)
				{
					return false;
				}
				if (wrapper.GetMethodWrapper("writeReplace", "()Ljava.lang.Object;", false) != null)
				{
					return false;
				}
				if (wrapper.GetMethodWrapper("readResolve", "()Ljava.lang.Object;", false) != null)
				{
					return false;
				}
				if (wrapper.GetFieldWrapper("serialPersistentFields", "[Ljava.io.ObjectStreamField;") != null)
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
			return false;
		}

		internal static void AddAutomagicSerialization(DynamicTypeWrapper wrapper)
		{
			if (IsTriviallySerializable(wrapper))
			{
				wrapper.TypeAsBuilder.SetCustomAttribute(serializableAttribute);
			}
		}
	}
}
