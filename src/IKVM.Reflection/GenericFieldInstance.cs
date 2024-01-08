/*
  Copyright (C) 2009, 2010 Jeroen Frijters

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
namespace IKVM.Reflection
{

    sealed class GenericFieldInstance : FieldInfo
	{

		private readonly Type declaringType;
		private readonly FieldInfo field;

		internal GenericFieldInstance(Type declaringType, FieldInfo field)
		{
			this.declaringType = declaringType;
			this.field = field;
		}

		public override bool Equals(object obj)
		{
			GenericFieldInstance other = obj as GenericFieldInstance;
			return other != null && other.declaringType.Equals(declaringType) && other.field.Equals(field);
		}

		public override int GetHashCode()
		{
			return declaringType.GetHashCode() * 3 ^ field.GetHashCode();
		}

		public override FieldAttributes Attributes
		{
			get { return field.Attributes; }
		}

		public override string Name
		{
			get { return field.Name; }
		}

		public override Type DeclaringType
		{
			get { return declaringType; }
		}

		public override Module Module
		{
			get { return declaringType.Module; }
		}

		public override int MetadataToken
		{
			get { return field.MetadataToken; }
		}

		public override object GetRawConstantValue()
		{
			return field.GetRawConstantValue();
		}

		public override void __GetDataFromRVA(byte[] data, int offset, int length)
		{
			field.__GetDataFromRVA(data, offset, length);
		}

		public override int __FieldRVA
		{
			get { return field.__FieldRVA; }
		}

		public override bool __TryGetFieldOffset(out int offset)
		{
			return field.__TryGetFieldOffset(out offset);
		}

		public override FieldInfo __GetFieldOnTypeDefinition()
		{
			return field;
		}

		internal override FieldSignature FieldSignature
		{
			get { return field.FieldSignature.ExpandTypeParameters(declaringType); }
		}

		internal override int ImportTo(Emit.ModuleBuilder module)
		{
			return module.ImportMethodOrField(declaringType, field.Name, field.FieldSignature);
		}

		internal override FieldInfo BindTypeParameters(Type type)
		{
			return new GenericFieldInstance(declaringType.BindTypeParameters(type), field);
		}

		internal override int GetCurrentToken()
		{
			return field.GetCurrentToken();
		}

		internal override bool IsBaked
		{
			get { return field.IsBaked; }
		}

	}

}
