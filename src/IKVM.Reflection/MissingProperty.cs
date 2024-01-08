/*
  Copyright (C) 2011-2012 Jeroen Frijters

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

    // NOTE this is currently only used by CustomAttributeData (because there is no other way to refer to a property)
    sealed class MissingProperty : PropertyInfo
	{
		private readonly Type declaringType;
		private readonly string name;
		private readonly PropertySignature signature;

		internal MissingProperty(Type declaringType, string name, PropertySignature signature)
		{
			this.declaringType = declaringType;
			this.name = name;
			this.signature = signature;
		}

		public override PropertyAttributes Attributes
		{
			get { throw new MissingMemberException(this); }
		}

		public override bool CanRead
		{
			get { throw new MissingMemberException(this); }
		}

		public override bool CanWrite
		{
			get { throw new MissingMemberException(this); }
		}

		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			throw new MissingMemberException(this);
		}

		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			throw new MissingMemberException(this);
		}

		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			throw new MissingMemberException(this);
		}

		public override object GetRawConstantValue()
		{
			throw new MissingMemberException(this);
		}

		internal override bool IsPublic
		{
			get { throw new MissingMemberException(this); }
		}

		internal override bool IsNonPrivate
		{
			get { throw new MissingMemberException(this); }
		}

		internal override bool IsStatic
		{
			get { throw new MissingMemberException(this); }
		}

		internal override PropertySignature PropertySignature
		{
			get { return signature; }
		}

		public override string Name
		{
			get { return name; }
		}

		public override Type DeclaringType
		{
			get { return declaringType; }
		}

		public override Module Module
		{
			get { return declaringType.Module; }
		}

		internal override bool IsBaked
		{
			get { return declaringType.IsBaked; }
		}

		internal override int GetCurrentToken()
		{
			throw new MissingMemberException(this);
		}
	}

}
