/*
  Copyright (C) 2008-2011 Jeroen Frijters

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

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{
    public sealed class GenericTypeParameterBuilder : TypeInfo
	{
		private readonly string name;
		private readonly TypeBuilder type;
		private readonly MethodBuilder method;
		private readonly int paramPseudoIndex;
		private readonly int position;
		private int typeToken;
		private Type baseType;
		private GenericParameterAttributes attr;

		internal GenericTypeParameterBuilder(string name, TypeBuilder type, int position)
			: this(name, type, null, position, Signature.ELEMENT_TYPE_VAR)
		{
		}

		internal GenericTypeParameterBuilder(string name, MethodBuilder method, int position)
			: this(name, null, method, position, Signature.ELEMENT_TYPE_MVAR)
		{
		}

		private GenericTypeParameterBuilder(string name, TypeBuilder type, MethodBuilder method, int position, byte sigElementType)
			: base(sigElementType)
		{
			this.name = name;
			this.type = type;
			this.method = method;
			this.position = position;
			GenericParamTable.Record rec = new GenericParamTable.Record();
			rec.Number = (short)position;
			rec.Flags = 0;
			rec.Owner = type != null ? type.MetadataToken : method.MetadataToken;
			rec.Name = this.ModuleBuilder.Strings.Add(name);
			this.paramPseudoIndex = this.ModuleBuilder.GenericParam.AddRecord(rec);
		}

		public override string AssemblyQualifiedName
		{
			get { return null; }
		}

		protected override bool IsValueTypeImpl
		{
			get { return (this.GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0; }
		}

		public override Type BaseType
		{
			get { return baseType; }
		}

		public override Type[] __GetDeclaredInterfaces()
		{
			throw new NotImplementedException();
		}

		public override TypeAttributes Attributes
		{
			get { return TypeAttributes.Public; }
		}

		public override string Namespace
		{
			get { return DeclaringType.Namespace; }
		}

		public override string Name
		{
			get { return name; }
		}

		public override string FullName
		{
			get { return null; }
		}

		public override string ToString()
		{
			return this.Name;
		}

		private ModuleBuilder ModuleBuilder
		{
			get { return type != null ? type.ModuleBuilder : method.ModuleBuilder; }
		}

		public override Module Module
		{
			get { return ModuleBuilder; }
		}

		public override int GenericParameterPosition
		{
			get { return position; }
		}

		public override Type DeclaringType
		{
			get { return type; }
		}

		public override MethodBase DeclaringMethod
		{
			get { return method; }
		}

		public override Type[] GetGenericParameterConstraints()
		{
			throw new NotImplementedException();
		}

		public override CustomModifiers[] __GetGenericParameterConstraintCustomModifiers()
		{
			throw new NotImplementedException();
		}

		public override GenericParameterAttributes GenericParameterAttributes
		{
			get
			{
				CheckBaked();
				return attr;
			}
		}

		internal override void CheckBaked()
		{
			if (type != null)
			{
				type.CheckBaked();
			}
			else
			{
				method.CheckBaked();
			}
		}

		private void AddConstraint(Type type)
		{
			GenericParamConstraintTable.Record rec = new GenericParamConstraintTable.Record();
			rec.Owner = paramPseudoIndex;
			rec.Constraint = this.ModuleBuilder.GetTypeTokenForMemberRef(type);
			this.ModuleBuilder.GenericParamConstraint.AddRecord(rec);
		}

		public void SetBaseTypeConstraint(Type baseTypeConstraint)
		{
			this.baseType = baseTypeConstraint;
			AddConstraint(baseTypeConstraint);
		}

		public void SetInterfaceConstraints(params Type[] interfaceConstraints)
		{
			foreach (Type type in interfaceConstraints)
			{
				AddConstraint(type);
			}
		}

		public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
		{
			this.attr = genericParameterAttributes;
			// for now we'll back patch the table
			this.ModuleBuilder.GenericParam.PatchAttribute(paramPseudoIndex, genericParameterAttributes);
		}

		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			this.ModuleBuilder.SetCustomAttribute((GenericParamTable.Index << 24) | paramPseudoIndex, customBuilder);
		}

		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
		}

		public override int MetadataToken
		{
			get
			{
				CheckBaked();
				return (GenericParamTable.Index << 24) | paramPseudoIndex;
			}
		}

		internal override int GetModuleBuilderToken()
		{
			if (typeToken == 0)
			{
				ByteBuffer spec = new ByteBuffer(5);
				Signature.WriteTypeSpec(this.ModuleBuilder, spec, this);
				typeToken = 0x1B000000 | this.ModuleBuilder.TypeSpec.AddRecord(this.ModuleBuilder.Blobs.Add(spec));
			}
			return typeToken;
		}

		internal override Type BindTypeParameters(IGenericBinder binder)
		{
			if (type != null)
			{
				return binder.BindTypeParameter(this);
			}
			else
			{
				return binder.BindMethodParameter(this);
			}
		}

		internal override int GetCurrentToken()
		{
			if (this.ModuleBuilder.IsSaved)
			{
				return (GenericParamTable.Index << 24) | this.Module.GenericParam.GetIndexFixup()[paramPseudoIndex - 1] + 1;
			}
			else
			{
				return (GenericParamTable.Index << 24) | paramPseudoIndex;
			}
		}

		internal override bool IsBaked
		{
			get { return ((MemberInfo)type ?? method).IsBaked; }
		}
	}
}
