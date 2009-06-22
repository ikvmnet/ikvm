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
using IKVM.Reflection.Emit.Writer;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Emit
{
	public sealed class FieldBuilder : FieldInfo
	{
		private readonly TypeBuilder typeBuilder;
		private readonly string name;
		private readonly int pseudoToken;
		private FieldAttributes attribs;
		private readonly int nameIndex;
		private readonly int signature;
		private readonly Type fieldType;
		private readonly Type[] requiredCustomModifiers;
		private readonly Type[] optionalCustomModifiers;

		internal FieldBuilder(TypeBuilder type, string name, Type fieldType, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attribs)
		{
			this.typeBuilder = type;
			this.name = name;
			this.pseudoToken = type.ModuleBuilder.AllocPseudoToken();
			this.nameIndex = type.ModuleBuilder.Strings.Add(name);
			this.fieldType = fieldType;
			ByteBuffer sig = new ByteBuffer(5);
			SignatureHelper.WriteFieldSig(this.typeBuilder.ModuleBuilder, sig, fieldType, requiredCustomModifiers, optionalCustomModifiers);
			this.signature = this.typeBuilder.ModuleBuilder.Blobs.Add(sig);
			this.attribs = attribs;
			this.typeBuilder.ModuleBuilder.Tables.Field.AddRow();
			this.requiredCustomModifiers = MethodBuilder.Copy(requiredCustomModifiers);
			this.optionalCustomModifiers = MethodBuilder.Copy(optionalCustomModifiers);
		}

		public void SetConstant(object defaultValue)
		{
			attribs |= FieldAttributes.HasDefault;
			this.ModuleBuilder.AddConstant(pseudoToken, defaultValue);
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Constructor.DeclaringType == typeof(FieldOffsetAttribute))
			{
				TableHeap.FieldLayoutTable.Record rec = new TableHeap.FieldLayoutTable.Record();
				rec.Offset = (int)customBuilder.GetConstructorArgument(0);
				rec.Field = pseudoToken;
				this.ModuleBuilder.Tables.FieldLayout.AddRecord(rec);
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(MarshalAsAttribute))
			{
				TableHeap.FieldMarshalTable.Record rec = new TableHeap.FieldMarshalTable.Record();
				rec.Parent = pseudoToken;
				rec.NativeType = WriteMarshallingDescriptor(this.ModuleBuilder, customBuilder);
				this.ModuleBuilder.Tables.FieldMarshal.AddRecord(rec);
				attribs |= FieldAttributes.HasFieldMarshal;
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(NonSerializedAttribute))
			{
				attribs |= FieldAttributes.NotSerialized;
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(SpecialNameAttribute))
			{
				attribs |= FieldAttributes.SpecialName;
			}
			else
			{
				this.ModuleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
			}
		}

		internal static int WriteMarshallingDescriptor(ModuleBuilder moduleBuiler, CustomAttributeBuilder customBuilder)
		{
			UnmanagedType nativeType;
			object val = customBuilder.GetConstructorArgument(0);
			if (val is short)
			{
				nativeType = (UnmanagedType)(short)val;
			}
			else
			{
				nativeType = (UnmanagedType)val;
			}

			ByteBuffer bb = new ByteBuffer(5);
			bb.Write((byte)nativeType);

			object arraySubType = customBuilder.GetFieldValue("ArraySubType");
			if (arraySubType != null)
			{
				bb.Write((byte)(UnmanagedType)arraySubType);
			}
			object sizeParamIndex = customBuilder.GetFieldValue("SizeParamIndex");
			if (sizeParamIndex != null)
			{
				bb.WriteCompressedInt((short)sizeParamIndex + 1);
			}
			object sizeConst = customBuilder.GetFieldValue("SizeConst");
			if (sizeConst != null)
			{
				if (sizeParamIndex == null)
				{
					bb.WriteCompressedInt(0);
				}
				bb.WriteCompressedInt((int)sizeConst);
			}

			if (customBuilder.GetFieldValue("IidParameterIndex") != null
				|| customBuilder.GetFieldValue("MarshalCookie") != null
				|| customBuilder.GetFieldValue("MarshalType") != null
				|| customBuilder.GetFieldValue("MarshalTypeRef") != null
				|| customBuilder.GetFieldValue("SafeArraySubType") != null
				|| customBuilder.GetFieldValue("SafeArrayUserDefinedSubType") != null)
			{
				throw new NotImplementedException();
			}

			return moduleBuiler.Blobs.Add(bb);
		}

		public override FieldAttributes Attributes
		{
			get { return attribs; }
		}

		public override RuntimeFieldHandle FieldHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type FieldType
		{
			get { return fieldType; }
		}

		public override object GetValue(object obj)
		{
			throw new NotImplementedException();
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override Type DeclaringType
		{
			get { return this.ModuleBuilder.IsModuleType(typeBuilder) ? null : typeBuilder; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return name; }
		}

		public override Type ReflectedType
		{
			get { return this.DeclaringType; }
		}

		public override int MetadataToken
		{
			get { return pseudoToken; }
		}

		public override Type[] GetOptionalCustomModifiers()
		{
			return MethodBuilder.Copy(optionalCustomModifiers);
		}

		public override Type[] GetRequiredCustomModifiers()
		{
			return MethodBuilder.Copy(requiredCustomModifiers);
		}

#if NET_4_0
		public override Module Module
		{
			get { return typeBuilder.Module; }
		}
#endif
		internal void WriteFieldRecords(MetadataWriter mw)
		{
			mw.Write((short)attribs);
			mw.WriteStringIndex(nameIndex);
			mw.WriteBlobIndex(signature);
		}

		internal void FixupToken(int token)
		{
			typeBuilder.ModuleBuilder.RegisterTokenFixup(this.pseudoToken, token);
		}

		internal ModuleBuilder ModuleBuilder
		{
			get { return typeBuilder.ModuleBuilder; }
		}

		internal int ImportTo(ModuleBuilder other)
		{
			return other.ImportField(typeBuilder, name, fieldType, optionalCustomModifiers, requiredCustomModifiers);
		}
	}

	sealed class FieldInstance : FieldInfo
	{
		private readonly Type type;
		private readonly FieldInfo field;

		internal FieldInstance(Type type, FieldInfo field)
		{
			this.type = type;
			this.field = field;
		}

		public override bool Equals(object obj)
		{
			FieldInstance other = obj as FieldInstance;
			return other != null && other.type == type && other.field == field;
		}

		public override int GetHashCode()
		{
			return type.GetHashCode() * 23 + field.GetHashCode();
		}

		public override FieldAttributes Attributes
		{
			get { throw new NotImplementedException(); }
		}

		public override RuntimeFieldHandle FieldHandle
		{
			get { throw new NotImplementedException(); }
		}

		public override Type FieldType
		{
			get { return ReplaceGenericParameter(type, field.FieldType); }
		}

		public override object GetValue(object obj)
		{
			throw new NotImplementedException();
		}

		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		public override Type DeclaringType
		{
			get { return type; }
		}

		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public override string Name
		{
			get { return field.Name; }
		}

		public override Type ReflectedType
		{
			get { return type; }
		}

		public override Type[] GetOptionalCustomModifiers()
		{
			return ReplaceGenericParameters(type, field.GetOptionalCustomModifiers());
		}

		public override Type[] GetRequiredCustomModifiers()
		{
			return ReplaceGenericParameters(type, field.GetRequiredCustomModifiers());
		}

		internal static Type ReplaceGenericParameter(Type container, Type type)
		{
			if (type.IsGenericParameter)
			{
				type = container.GetGenericArguments()[type.GenericParameterPosition];
			}
			return type;
		}

		internal static Type[] ReplaceGenericParameters(Type container, Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				if (types[i].IsGenericParameter)
				{
					Type[] newArray = new Type[types.Length];
					Array.Copy(types, newArray, i);
					for (; i < types.Length; i++)
					{
						newArray[i] = ReplaceGenericParameter(container, types[i]);
					}
					return newArray;
				}
			}
			return types;
		}

		internal FieldInfo GetFieldOnTypeDefinition()
		{
			return field;
		}
	}
}
