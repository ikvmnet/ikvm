/*
  Copyright (C) 2008, 2009 Jeroen Frijters

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
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using IKVM.Reflection.Emit.Writer;
using IKVM.Reflection.Emit.Impl;
using System.Runtime.CompilerServices;

namespace IKVM.Reflection.Emit
{
	public class GenericTypeParameterBuilder : Impl.TypeBase
	{
		private readonly ModuleBuilder moduleBuilder;
		private readonly string name;
		private readonly Type type;
		private readonly MethodInfo method;
		private readonly int owner;
		private readonly int position;
		private int token;

		internal GenericTypeParameterBuilder(ModuleBuilder moduleBuilder, string name, Type type, MethodInfo method, int owner, int position)
		{
			this.moduleBuilder = moduleBuilder;
			this.name = name;
			this.type = type;
			this.method = method;
			this.owner = owner;
			this.position = position;
		}

		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			TableHeap.GenericParamConstraintTable.Record rec = new TableHeap.GenericParamConstraintTable.Record();
			rec.Owner = owner;
			rec.Constraint = moduleBuilder.GetTypeToken(baseTypeConstraint).Token;
			moduleBuilder.Tables.GenericParamConstraint.AddRecord(rec);
		}

		public override bool IsGenericParameter
		{
			get { return true; }
		}

		public override int GenericParameterPosition
		{
			get { return position; }
		}

		public override MethodBase DeclaringMethod
		{
			get { return method; }
		}

		public override Type DeclaringType
		{
			get { return type; }
		}

		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { return name; }
		}

		public override string FullName
		{
			get { return null; }
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		protected override bool IsArrayImpl()
		{
			throw new NotImplementedException();
		}

		protected override bool IsByRefImpl()
		{
			throw new NotImplementedException();
		}

		internal override int GetTypeToken()
		{
			if (token == 0)
			{
				ByteBuffer spec = new ByteBuffer(5);
				SignatureHelper.WriteType(moduleBuilder, spec, this);
				token = 0x1B000000 | this.ModuleBuilder.Tables.TypeSpec.AddRecord(this.ModuleBuilder.Blobs.Add(spec));
			}
			return token;
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return moduleBuilder; }
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return moduleBuilder.Assembly; }
		}

		public override Module Module
		{
			get { return moduleBuilder; }
		}
#endif
	}

	public sealed class TypeBuilder : Impl.TypeBase, ITypeOwner
	{
		private readonly ITypeOwner owner;
		private readonly int token;
		private int extends;
		private Type baseType;
		private readonly int typeName;
		private readonly int typeNameSpace;
		private readonly string nameOrFullName;
		private readonly List<MethodBuilder> methods = new List<MethodBuilder>();
		private readonly List<FieldBuilder> fields = new List<FieldBuilder>();
		private List<PropertyBuilder> properties;
		private TypeAttributes attribs;
		private TypeFlags typeFlags;
		private GenericTypeParameterBuilder[] gtpb;

		[Flags]
		private enum TypeFlags
		{
			IsGenericTypeDefinition = 1,
			HasNestedTypes = 2,
		}

		internal TypeBuilder(ITypeOwner owner, string name, Type baseType, TypeAttributes attribs)
		{
			this.owner = owner;
			this.token = this.ModuleBuilder.Tables.TypeDef.AllocToken();
			this.nameOrFullName = Escape(name);
			SetParent(baseType);
			this.attribs = attribs;
			if (!this.IsNested)
			{
				int lastdot = name.LastIndexOf('.');
				if (lastdot > 0)
				{
					this.typeNameSpace = this.ModuleBuilder.Strings.Add(name.Substring(0, lastdot));
					name = name.Substring(lastdot + 1);
				}
			}
			this.typeName = this.ModuleBuilder.Strings.Add(name);
		}

		private static string Escape(string name)
		{
			System.Text.StringBuilder sb = null;
			int pos;
			for (pos = 0; pos < name.Length; pos++)
			{
				if ("+\\[],*&".IndexOf(name[pos]) != -1)
				{
					if (sb == null)
					{
						sb = new System.Text.StringBuilder(name, 0, pos, name.Length + 3);
					}
					sb.Append('\\').Append(name[pos]);
				}
			}
			return sb != null ? sb.ToString() : name;
		}

		public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callConv, Type[] parameterTypes)
		{
			return DefineConstructor(attribs, callConv, parameterTypes, null, null);
		}

		public ConstructorBuilder DefineConstructor(MethodAttributes attribs, CallingConventions callingConvention, Type[] parameterTypes, Type[][] requiredCustomModifiers, Type[][] optionalCustomModifiers)
		{
			attribs |= MethodAttributes.RTSpecialName | MethodAttributes.SpecialName;
			string name = ".ctor";
			if ((attribs & MethodAttributes.Static) != 0)
			{
				name = ".cctor";
			}
			MethodBuilder mb = DefineMethod(name, attribs, CallingConventions.Standard, typeof(void), null, null, parameterTypes, requiredCustomModifiers, optionalCustomModifiers);
			return new ConstructorBuilder(mb);
		}

		public ConstructorBuilder DefineTypeInitializer()
		{
			MethodBuilder mb = DefineMethod(".cctor", MethodAttributes.Private | MethodAttributes.Static | MethodAttributes.RTSpecialName | MethodAttributes.SpecialName, typeof(void), Type.EmptyTypes);
			return new ConstructorBuilder(mb);
		}

		private MethodBuilder CreateMethodBuilder(string name, MethodAttributes attributes, CallingConventions callingConvention)
		{
			this.ModuleBuilder.Tables.MethodDef.AddRow();
			MethodBuilder mb = new MethodBuilder(this, name, attributes, callingConvention);
			methods.Add(mb);
			return mb;
		}

		public MethodBuilder DefineMethod(string name, MethodAttributes attribs)
		{
			return CreateMethodBuilder(name, attribs, CallingConventions.Standard);
		}

		public MethodBuilder DefineMethod(string name, MethodAttributes attribs, Type returnType, Type[] parameterTypes)
		{
			return DefineMethod(name, attribs, CallingConventions.Standard, returnType, null, null, parameterTypes, null, null);
		}

		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			return DefineMethod(name, attributes, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		public MethodBuilder DefineMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, Type returnType, Type[] returnTypeRequiredCustomModifiers, Type[] returnTypeOptionalCustomModifiers, Type[] parameterTypes, Type[][] parameterTypeRequiredCustomModifiers, Type[][] parameterTypeOptionalCustomModifiers)
		{
			MethodBuilder mb = CreateMethodBuilder(name, attributes, callingConvention);
			mb.SetSignature(returnType, returnTypeRequiredCustomModifiers, returnTypeOptionalCustomModifiers, parameterTypes, parameterTypeRequiredCustomModifiers, parameterTypeOptionalCustomModifiers);
			return mb;
		}

		public void DefineMethodOverride(MethodInfo methodInfoBody, MethodInfo methodInfoDeclaration)
		{
			TableHeap.MethodImplTable.Record rec = new TableHeap.MethodImplTable.Record();
			rec.Class = token;
			rec.MethodBody = this.ModuleBuilder.GetMethodToken(methodInfoBody).Token;
			rec.MethodDeclaration = this.ModuleBuilder.GetMethodToken(methodInfoDeclaration).Token;
			this.ModuleBuilder.Tables.MethodImpl.AddRecord(rec);
		}

		public FieldBuilder DefineField(string name, Type fieldType, FieldAttributes attribs)
		{
			return DefineField(name, fieldType, null, null, attribs);
		}

		public FieldBuilder DefineField(string fieldName, Type type, Type[] requiredCustomModifiers, Type[] optionalCustomModifiers, FieldAttributes attributes)
		{
			FieldBuilder fb = new FieldBuilder(this, fieldName, type, requiredCustomModifiers, optionalCustomModifiers, attributes);
			fields.Add(fb);
			return fb;
		}

		public PropertyBuilder DefineProperty(string name, PropertyAttributes attributes, Type returnType, Type[] parameterTypes)
		{
			if (properties == null)
			{
				properties = new List<PropertyBuilder>();
			}
			PropertyBuilder pb = new PropertyBuilder(this.ModuleBuilder, name, attributes, returnType, parameterTypes);
			properties.Add(pb);
			return pb;
		}

		public TypeBuilder DefineNestedType(string name)
		{
			return DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPrivate);
		}

		public TypeBuilder DefineNestedType(string name, TypeAttributes attribs)
		{
			return DefineNestedType(name, attribs, null);
		}

		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent)
		{
			this.typeFlags |= TypeFlags.HasNestedTypes;
			return this.ModuleBuilder.DefineNestedTypeHelper(this, name, attr, parent, PackingSize.Unspecified, 0);
		}

		public TypeBuilder DefineNestedType(string name, TypeAttributes attr, Type parent, Type[] interfaces)
		{
			TypeBuilder tb = DefineNestedType(name, attr, parent);
			foreach (Type iface in interfaces)
			{
				tb.AddInterfaceImplementation(iface);
			}
			return tb;
		}

		public void SetParent(Type parent)
		{
			baseType = parent;
			if (parent == null)
			{
				extends = 0;
			}
			else
			{
				extends = this.ModuleBuilder.GetTypeToken(parent).Token;
			}
		}

		public void AddInterfaceImplementation(Type interfaceType)
		{
			TableHeap.InterfaceImplTable.Record rec = new TableHeap.InterfaceImplTable.Record();
			rec.Class = token;
			rec.Interface = this.ModuleBuilder.GetTypeToken(interfaceType).Token;
			this.ModuleBuilder.Tables.InterfaceImpl.AddRecord(rec);
		}

		private void SetStructLayoutPseudoCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			object val = customBuilder.GetConstructorArgument(0);
			LayoutKind layout;
			if (val is short)
			{
				layout = (LayoutKind)(short)val;
			}
			else
			{
				layout = (LayoutKind)val;
			}
			int? pack = (int?)customBuilder.GetFieldValue("Pack");
			int? size = (int?)customBuilder.GetFieldValue("Size");
			if (pack.HasValue || size.HasValue)
			{
				TableHeap.ClassLayoutTable.Record rec = new TableHeap.ClassLayoutTable.Record();
				rec.PackingSize = (short)(pack ?? 0);
				rec.ClassSize = size ?? 0;
				rec.Parent = token;
				this.ModuleBuilder.Tables.ClassLayout.AddRecord(rec);
			}
			attribs &= ~TypeAttributes.LayoutMask;
			switch (layout)
			{
				case LayoutKind.Auto:
					attribs |= TypeAttributes.AutoLayout;
					break;
				case LayoutKind.Explicit:
					attribs |= TypeAttributes.ExplicitLayout;
					break;
				case LayoutKind.Sequential:
					attribs |= TypeAttributes.SequentialLayout;
					break;
			}
			CharSet? charSet = (CharSet?)customBuilder.GetFieldValue("CharSet");
			attribs &= ~TypeAttributes.StringFormatMask;
			switch (charSet ?? CharSet.None)
			{
				case CharSet.None:
				case CharSet.Ansi:
					attribs |= TypeAttributes.AnsiClass;
					break;
				case CharSet.Auto:
					attribs |= TypeAttributes.AutoClass;
					break;
				case CharSet.Unicode:
					attribs |= TypeAttributes.UnicodeClass;
					break;
			}
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder.Constructor.DeclaringType == typeof(StructLayoutAttribute))
			{
				SetStructLayoutPseudoCustomAttribute(customBuilder);
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(SerializableAttribute))
			{
				attribs |= TypeAttributes.Serializable;
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(ComImportAttribute))
			{
				attribs |= TypeAttributes.Import;
			}
			else if (customBuilder.Constructor.DeclaringType == typeof(SpecialNameAttribute))
			{
				attribs |= TypeAttributes.SpecialName;
			}
			else
			{
				this.ModuleBuilder.SetCustomAttribute(token, customBuilder);
			}
		}

		public void AddDeclarativeSecurity(System.Security.Permissions.SecurityAction securityAction, System.Security.PermissionSet permissionSet)
		{
			this.ModuleBuilder.AddDeclaritiveSecurity(token, securityAction, permissionSet);
			this.attribs |= TypeAttributes.HasSecurity;
		}

		public GenericTypeParameterBuilder[] DefineGenericParameters(params string[] names)
		{
			typeFlags |= TypeFlags.IsGenericTypeDefinition;
			gtpb = new GenericTypeParameterBuilder[names.Length];
			for (int i = 0; i < names.Length; i++)
			{
				TableHeap.GenericParamTable.Record rec = new TableHeap.GenericParamTable.Record();
				rec.Number = (short)i;
				rec.Flags = 0;
				rec.Owner = token;
				rec.Name = this.ModuleBuilder.Strings.Add(names[i]);
				gtpb[i] = new GenericTypeParameterBuilder(this.ModuleBuilder, names[i], this, null, this.ModuleBuilder.Tables.GenericParam.AddRecord(rec), i);
			}
			return (GenericTypeParameterBuilder[])gtpb.Clone();
		}

		public override Type[] GetGenericArguments()
		{
			return gtpb == null ? Type.EmptyTypes : (Type[])gtpb.Clone();
		}

		public Type CreateType()
		{
			foreach (MethodBuilder mb in methods)
			{
				mb.Bake();
			}
			if (properties != null)
			{
				TableHeap.PropertyMapTable.Record rec = new TableHeap.PropertyMapTable.Record();
				rec.Parent = token;
				rec.PropertyList = this.ModuleBuilder.Tables.Property.RowCount + 1;
				this.ModuleBuilder.Tables.PropertyMap.AddRecord(rec);
				foreach (PropertyBuilder pb in properties)
				{
					pb.Bake();
				}
				properties = null;
			}
			return new BakedType(this);
		}

		public override string AssemblyQualifiedName
		{
			get { return FullName + ", " + this.ModuleBuilder.Assembly.FullName; }
		}

		public override Type BaseType
		{
			get { return baseType; }
		}

		public override string FullName
		{
			get
			{
				if (this.IsNested)
				{
					return this.DeclaringType.FullName + "+" + nameOrFullName;
				}
				else
				{
					return nameOrFullName;
				}
			}
		}

		public override string Name
		{
			get
			{
				if (this.IsNested)
				{
					return nameOrFullName;
				}
				else
				{
					return base.Name;
				}
			}
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return attribs;
		}

		internal MethodInfo __GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			const BindingFlags supportedFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
			if (binder != null || (bindingAttr & ~supportedFlags) != 0)
			{
				throw new NotSupportedException();
			}
			foreach (MethodBuilder mb in methods)
			{
				if (mb.Name == name
					&& ((mb.IsPublic && ((bindingAttr & BindingFlags.Public) != 0)) || (!mb.IsPublic && ((bindingAttr & BindingFlags.NonPublic) != 0)))
					&& ((mb.IsStatic && ((bindingAttr & BindingFlags.Static) != 0)) || (!mb.IsStatic && ((bindingAttr & BindingFlags.Instance) != 0)))
					&& (types == null || mb.MatchParameters(types)))
				{
					return mb;
				}
			}
			if (baseType == null || (bindingAttr & BindingFlags.DeclaredOnly) != 0)
			{
				return null;
			}
			return baseType.GetMethod(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		protected override bool IsArrayImpl()
		{
			return false;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		protected override bool IsValueTypeImpl()
		{
			return baseType == typeof(ValueType) || baseType == typeof(Enum);
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			return GenericType.Make(this, typeArguments);
		}

		public override StructLayoutAttribute StructLayoutAttribute
		{
			get
			{
				StructLayoutAttribute attr;
				if ((attribs & TypeAttributes.ExplicitLayout) != 0)
				{
					attr = new StructLayoutAttribute(LayoutKind.Explicit);
					attr.Pack = 8;
					attr.Size = 0;
					this.ModuleBuilder.Tables.ClassLayout.GetLayout(token, ref attr.Pack, ref attr.Size);
				}
				else
				{
					attr = new StructLayoutAttribute((attribs & TypeAttributes.SequentialLayout) != 0 ? LayoutKind.Sequential : LayoutKind.Auto);
					attr.Pack = 8;
					attr.Size = 0;
				}
				switch (attribs & TypeAttributes.StringFormatMask)
				{
					case TypeAttributes.AutoClass:
						attr.CharSet = CharSet.Auto;
						break;
					case TypeAttributes.UnicodeClass:
						attr.CharSet = CharSet.Unicode;
						break;
					case TypeAttributes.AnsiClass:
						attr.CharSet = CharSet.Ansi;
						break;
				}
				return attr;
			}
		}

		public override Type DeclaringType
		{
			get
			{
				return owner as TypeBuilder;
			}
		}

		public override bool IsGenericType
		{
			get
			{
				return IsGenericTypeDefinition;
			}
		}

		public override bool IsGenericTypeDefinition
		{
			get
			{
				return (typeFlags & TypeFlags.IsGenericTypeDefinition) != 0;
			}
		}

		public override int MetadataToken
		{
			get { return token; }
		}

		public FieldBuilder DefineInitializedData(string name, byte[] data, FieldAttributes attributes)
		{
			Type fieldType = this.ModuleBuilder.GetType("$ArrayType$" + data.Length);
			if (fieldType == null)
			{
				fieldType = this.ModuleBuilder.DefineType("$ArrayType$" + data.Length, TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.ExplicitLayout, typeof(ValueType), PackingSize.Size1, data.Length);
			}
			FieldBuilder fb = DefineField(name, fieldType, attributes | FieldAttributes.Static | FieldAttributes.HasFieldRVA);
			TableHeap.FieldRVATable.Record rec = new TableHeap.FieldRVATable.Record();
			rec.RVA = this.ModuleBuilder.initializedData.Position;
			rec.Field = fb.MetadataToken;
			this.ModuleBuilder.Tables.FieldRVA.AddRecord(rec);
			this.ModuleBuilder.initializedData.Write(data);
			return fb;
		}

		public static MethodInfo GetMethod(Type type, MethodInfo method)
		{
			return new MethodInstance(type, method);
		}

		public static ConstructorInfo GetConstructor(Type type, ConstructorInfo constructor)
		{
			return new ConstructorInstance(type, constructor);
		}

		public static FieldInfo GetField(Type type, FieldInfo field)
		{
			return new FieldInstance(type, field);
		}

		internal void WriteTypeDefRecord(MetadataWriter mw, ref int fieldList, ref int methodList)
		{
			mw.Write((int)attribs);
			mw.WriteStringIndex(typeName);
			mw.WriteStringIndex(typeNameSpace);
			mw.WriteTypeDefOrRef(extends);
			mw.WriteField(fieldList);
			mw.WriteMethodDef(methodList);
			methodList += methods.Count;
			fieldList += fields.Count;
		}

		internal void WriteMethodDefRecords(int baseRVA, MetadataWriter mw, ref int paramList)
		{
			foreach (MethodBuilder mb in methods)
			{
				mb.WriteMethodDefRecord(baseRVA, mw, ref paramList);
			}
		}

		internal void ResolveMethodAndFieldTokens(ref int methodToken, ref int fieldToken, ref int parameterToken)
		{
			foreach (MethodBuilder method in methods)
			{
				method.FixupToken(methodToken++, ref parameterToken);
			}
			foreach (FieldBuilder field in fields)
			{
				field.FixupToken(fieldToken++);
			}
		}

		internal void WriteParamRecords(MetadataWriter mw)
		{
			foreach (MethodBuilder mb in methods)
			{
				mb.WriteParamRecords(mw);
			}
		}

		internal void WriteFieldRecords(MetadataWriter mw)
		{
			foreach (FieldBuilder fb in fields)
			{
				fb.WriteFieldRecords(mw);
			}
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return owner.ModuleBuilder; }
		}

		ModuleBuilder ITypeOwner.ModuleBuilder
		{
			get { return owner.ModuleBuilder; }
		}

		internal bool HasNestedTypes
		{
			get { return (typeFlags & TypeFlags.HasNestedTypes) != 0; }
		}

		// helper for ModuleBuilder.ResolveMethod()
		internal MethodBase LookupMethod(int token)
		{
			foreach (MethodBuilder method in methods)
			{
				if (method.MetadataToken == token)
				{
					return method;
				}
			}
			return null;
		}

		internal Type GetEnumUnderlyingType()
		{
			Debug.Assert(this.IsEnum);
			foreach (FieldInfo field in fields)
			{
				// the CLR assumes that an enum has only one instance field, so we can do the same
				if (!field.IsStatic)
				{
					return field.FieldType;
				}
			}
			throw new InvalidOperationException();
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return owner.ModuleBuilder.Assembly; }
		}

		public override Module Module
		{
			get { return owner.ModuleBuilder; }
		}
#endif
	}

	sealed class ArrayType : Impl.TypeBase
	{
		private readonly Impl.TypeBase type;
		private int token;

		internal static Type Make(Impl.TypeBase type)
		{
			return type.ModuleBuilder.CanonicalizeType(new ArrayType(type));
		}

		private ArrayType(Impl.TypeBase type)
		{
			this.type = type;
		}

		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { return typeof(System.Array); }
		}

		public override string Name
		{
			get { return type.Name + "[]"; }
		}

		public override string FullName
		{
			get { return type.FullName + "[]"; }
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		public override Type GetElementType()
		{
			return type;
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return true;
		}

		protected override bool IsArrayImpl()
		{
			return true;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		protected override bool IsValueTypeImpl()
		{
			return false;
		}

		public override bool Equals(object o)
		{
			ArrayType at = o as ArrayType;
			// MONOBUG we need to call Equals(object) to end up in our version (in TypeBase), because Mono's Type.Equals is broken
			return at != null && at.type.Equals((object)type);
		}

		public override int GetHashCode()
		{
			return type.GetHashCode() * 5;
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return type.ModuleBuilder; }
		}

		internal override int GetTypeToken()
		{
			if (token == 0)
			{
				ByteBuffer spec = new ByteBuffer(5);
				SignatureHelper.WriteType(this.ModuleBuilder, spec, this);
				token = 0x1B000000 | this.ModuleBuilder.Tables.TypeSpec.AddRecord(this.ModuleBuilder.Blobs.Add(spec));
			}
			return token;
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return type.Assembly; }
		}

		public override Module Module
		{
			get { return type.Module; }
		}
#endif
	}

	sealed class BakedType : Impl.TypeBase
	{
		private readonly TypeBuilder typeBuilder;

		internal BakedType(TypeBuilder typeBuilder)
		{
			this.typeBuilder = typeBuilder;
		}

		public override string AssemblyQualifiedName
		{
			get { return typeBuilder.AssemblyQualifiedName; }
		}

		public override Type BaseType
		{
			get { return typeBuilder.BaseType; }
		}

		public override string Name
		{
			get { return typeBuilder.Name; }
		}

		public override string FullName
		{
			get { return typeBuilder.FullName; }
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			return typeBuilder.Attributes;
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			return typeBuilder.__GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers);
		}

		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return typeBuilder.GetMethods(bindingAttr);
		}

		public override Type[] GetNestedTypes(BindingFlags bindingAttr)
		{
			if (typeBuilder.HasNestedTypes)
			{
				List<Type> types = new List<Type>();
				List<int> classes = typeBuilder.ModuleBuilder.Tables.NestedClass.GetNestedClasses(typeBuilder.MetadataToken);
				foreach (int nestedClass in classes)
				{
					Type type = typeBuilder.ModuleBuilder.ResolveType(nestedClass);
					if ((type.IsNestedPublic && (bindingAttr & BindingFlags.Public) != 0) || (!type.IsNestedPublic && (bindingAttr & BindingFlags.NonPublic) != 0))
					{
						types.Add(type);
					}
				}
				return types.ToArray();
			}
			else
			{
				return Type.EmptyTypes;
			}
		}

		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		protected override bool IsArrayImpl()
		{
			return false;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		protected override bool IsValueTypeImpl()
		{
			return typeBuilder.IsValueType;
		}

		public override Type DeclaringType
		{
			get { return typeBuilder.DeclaringType; }
		}

		public override Type MakeArrayType()
		{
			return typeBuilder.MakeArrayType();
		}

		public override Type MakeGenericType(params Type[] typeArguments)
		{
			return typeBuilder.MakeGenericType(typeArguments);
		}

		public override System.Runtime.InteropServices.StructLayoutAttribute StructLayoutAttribute
		{
			get { return typeBuilder.StructLayoutAttribute; }
		}

		public override Type UnderlyingSystemType
		{
			// Type.Equals/GetHashCode relies on this
			get { return typeBuilder; }
		}

		public override bool IsGenericType
		{
			get
			{
				return typeBuilder.IsGenericType;
			}
		}

		public override bool IsGenericTypeDefinition
		{
			get
			{
				return typeBuilder.IsGenericTypeDefinition;
			}
		}

		public override int MetadataToken
		{
			get { return typeBuilder.MetadataToken; }
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return typeBuilder.ModuleBuilder; }
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return typeBuilder.Assembly; }
		}

		public override Module Module
		{
			get { return typeBuilder.Module; }
		}
#endif
	}

	sealed class GenericType : Impl.TypeBase
	{
		private readonly TypeBuilder typeBuilder;
		private readonly Type[] typeArguments;
		private int token;

		internal static Type Make(TypeBuilder typeBuilder, Type[] typeArguments)
		{
			return typeBuilder.ModuleBuilder.CanonicalizeType(new GenericType(typeBuilder, typeArguments));
		}

		private GenericType(TypeBuilder typeBuilder, Type[] typeArguments)
		{
			this.typeBuilder = typeBuilder;
			this.typeArguments = typeArguments;
		}

		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override Type[] GetGenericArguments()
		{
			return (Type[])typeArguments.Clone();
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		protected override bool IsArrayImpl()
		{
			return false;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		public override Type DeclaringType
		{
			get { return typeBuilder.DeclaringType; }
		}

		public override bool Equals(object o)
		{
			GenericType gt = o as GenericType;
			if (gt != null && gt.typeBuilder == typeBuilder)
			{
				for (int i = 0; i < typeArguments.Length; i++)
				{
					if (typeArguments[i] != gt.typeArguments[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hash = typeBuilder.GetHashCode();
			for (int i = 0; i < typeArguments.Length; i++)
			{
				hash *= 37;
				hash ^= typeArguments[i].GetHashCode();
			}
			return hash;
		}

		public override bool IsGenericType
		{
			get { return true; }
		}

		public override bool IsGenericTypeDefinition
		{
			get { return false; }
		}

		public override Type GetGenericTypeDefinition()
		{
			return typeBuilder;
		}

		internal override int GetTypeToken()
		{
			if (token == 0)
			{
				ByteBuffer spec = new ByteBuffer(5);
				SignatureHelper.WriteGenericSignature(typeBuilder.ModuleBuilder, spec, typeBuilder, typeArguments);
				token = 0x1B000000 | this.ModuleBuilder.Tables.TypeSpec.AddRecord(this.ModuleBuilder.Blobs.Add(spec));
			}
			return token;
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return typeBuilder.ModuleBuilder; }
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return typeBuilder.Assembly; }
		}

		public override Module Module
		{
			get { return typeBuilder.Module; }
		}
#endif
	}

	public sealed class MonoHackGenericType : Impl.TypeBase
	{
		private static readonly Dictionary<MonoHackGenericType, MonoHackGenericType> canonical = new Dictionary<MonoHackGenericType, MonoHackGenericType>();
		private readonly Type type;
		private readonly Type[] typeArguments;

		public static Type Make(Type type, params Type[] typeArguments)
		{
			MonoHackGenericType newType = new MonoHackGenericType(type, (Type[])typeArguments.Clone());
			MonoHackGenericType canonicalType;
			if (!canonical.TryGetValue(newType, out canonicalType))
			{
				canonicalType = newType;
				canonical.Add(canonicalType, canonicalType);
			}
			return canonicalType;
		}

		private MonoHackGenericType(Type type, Type[] typeArguments)
		{
			this.type = type;
			this.typeArguments = typeArguments;
		}

		public override string AssemblyQualifiedName
		{
			get { throw new NotImplementedException(); }
		}

		public override Type BaseType
		{
			get { throw new NotImplementedException(); }
		}

		public override string FullName
		{
			get { throw new NotImplementedException(); }
		}

		public override string Name
		{
			get { throw new NotImplementedException(); }
		}

		public override Type[] GetGenericArguments()
		{
			return (Type[])typeArguments.Clone();
		}

		protected override TypeAttributes GetAttributeFlagsImpl()
		{
			throw new NotImplementedException();
		}

		protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
		{
			throw new NotImplementedException();
		}

		protected override bool HasElementTypeImpl()
		{
			return false;
		}

		protected override bool IsArrayImpl()
		{
			return false;
		}

		protected override bool IsByRefImpl()
		{
			return false;
		}

		public override Type DeclaringType
		{
			get { return type.DeclaringType; }
		}

		public override bool Equals(object o)
		{
			MonoHackGenericType gt = o as MonoHackGenericType;
			if (gt != null && gt.type == type)
			{
				for (int i = 0; i < typeArguments.Length; i++)
				{
					if (typeArguments[i] != gt.typeArguments[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		public override int GetHashCode()
		{
			int hash = type.GetHashCode();
			for (int i = 0; i < typeArguments.Length; i++)
			{
				hash *= 37;
				hash ^= typeArguments[i].GetHashCode();
			}
			return hash;
		}

		public override bool IsGenericType
		{
			get { return true; }
		}

		public override bool IsGenericTypeDefinition
		{
			get { return false; }
		}

		public override Type GetGenericTypeDefinition()
		{
			return type;
		}

		internal override int GetTypeToken()
		{
			throw new InvalidOperationException();
		}

		internal override ModuleBuilder ModuleBuilder
		{
			get { return null; }
		}

#if NET_4_0
		public override Assembly Assembly
		{
			get { return null; }
		}

		public override Module Module
		{
			get { return null; }
		}
#endif
	}
}
