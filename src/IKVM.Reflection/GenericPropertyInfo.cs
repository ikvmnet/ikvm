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

    sealed class GenericPropertyInfo : PropertyInfo
	{

		private readonly Type typeInstance;
		private readonly PropertyInfo property;

		internal GenericPropertyInfo(Type typeInstance, PropertyInfo property)
		{
			this.typeInstance = typeInstance;
			this.property = property;
		}

		public override bool Equals(object obj)
		{
			GenericPropertyInfo other = obj as GenericPropertyInfo;
			return other != null && other.typeInstance == typeInstance && other.property == property;
		}

		public override int GetHashCode()
		{
			return typeInstance.GetHashCode() * 537 + property.GetHashCode();
		}

		public override PropertyAttributes Attributes
		{
			get { return property.Attributes; }
		}

		public override bool CanRead
		{
			get { return property.CanRead; }
		}

		public override bool CanWrite
		{
			get { return property.CanWrite; }
		}

		private MethodInfo Wrap(MethodInfo method)
		{
			if (method == null)
			{
				return null;
			}
			return new GenericMethodInstance(typeInstance, method, null);
		}

		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return Wrap(property.GetGetMethod(nonPublic));
		}

		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return Wrap(property.GetSetMethod(nonPublic));
		}

		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			MethodInfo[] accessors = property.GetAccessors(nonPublic);
			for (int i = 0; i < accessors.Length; i++)
			{
				accessors[i] = Wrap(accessors[i]);
			}
			return accessors;
		}

		public override object GetRawConstantValue()
		{
			return property.GetRawConstantValue();
		}

		internal override bool IsPublic
		{
			get { return property.IsPublic; }
		}

		internal override bool IsNonPrivate
		{
			get { return property.IsNonPrivate; }
		}

		internal override bool IsStatic
		{
			get { return property.IsStatic; }
		}

		internal override PropertySignature PropertySignature
		{
			get { return property.PropertySignature.ExpandTypeParameters(typeInstance); }
		}

		public override string Name
		{
			get { return property.Name; }
		}

		public override Type DeclaringType
		{
			get { return typeInstance; }
		}

		public override Module Module
		{
			get { return typeInstance.Module; }
		}

		public override int MetadataToken
		{
			get { return property.MetadataToken; }
		}

		internal override PropertyInfo BindTypeParameters(Type type)
		{
			return new GenericPropertyInfo(typeInstance.BindTypeParameters(type), property);
		}

		internal override bool IsBaked
		{
			get { return property.IsBaked; }
		}

		internal override int GetCurrentToken()
		{
			return property.GetCurrentToken();
		}

	}

}
