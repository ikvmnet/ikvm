/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

    sealed class PropertyInfoWithReflectedType : PropertyInfo
	{

		private readonly Type reflectedType;
		private readonly PropertyInfo property;

		internal PropertyInfoWithReflectedType(Type reflectedType, PropertyInfo property)
		{
			this.reflectedType = reflectedType;
			this.property = property;
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

		public override MethodInfo GetGetMethod(bool nonPublic)
		{
			return SetReflectedType(property.GetGetMethod(nonPublic), reflectedType);
		}

		public override MethodInfo GetSetMethod(bool nonPublic)
		{
			return SetReflectedType(property.GetSetMethod(nonPublic), reflectedType);
		}

		public override MethodInfo[] GetAccessors(bool nonPublic)
		{
			return SetReflectedType(property.GetAccessors(nonPublic), reflectedType);
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
			get { return property.PropertySignature; }
		}

		public override ParameterInfo[] GetIndexParameters()
		{
			ParameterInfo[] parameters = property.GetIndexParameters();
			for (int i = 0; i < parameters.Length; i++)
			{
				parameters[i] = new ParameterInfoWrapper(this, parameters[i]);
			}
			return parameters;
		}

		internal override PropertyInfo BindTypeParameters(Type type)
		{
			return property.BindTypeParameters(type);
		}

		public override string ToString()
		{
			return property.ToString();
		}

		public override bool __IsMissing
		{
			get { return property.__IsMissing; }
		}

		public override Type DeclaringType
		{
			get { return property.DeclaringType; }
		}

		public override Type ReflectedType
		{
			get { return reflectedType; }
		}

		public override bool Equals(object obj)
		{
			PropertyInfoWithReflectedType other = obj as PropertyInfoWithReflectedType;
			return other != null
				&& other.reflectedType == reflectedType
				&& other.property == property;
		}

		public override int GetHashCode()
		{
			return reflectedType.GetHashCode() ^ property.GetHashCode();
		}

		public override int MetadataToken
		{
			get { return property.MetadataToken; }
		}

		public override Module Module
		{
			get { return property.Module; }
		}

		public override string Name
		{
			get { return property.Name; }
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
