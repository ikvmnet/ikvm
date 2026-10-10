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

        readonly Type _reflectedType;
        readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="property"></param>
        internal PropertyInfoWithReflectedType(Type reflectedType, PropertyInfo property)
        {
            this._reflectedType = reflectedType;
            this._property = property;
        }

        public override PropertyAttributes Attributes
        {
            get { return _property.Attributes; }
        }

        public override bool CanRead
        {
            get { return _property.CanRead; }
        }

        public override bool CanWrite
        {
            get { return _property.CanWrite; }
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return SetReflectedType(_property.GetGetMethod(nonPublic), _reflectedType);
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return SetReflectedType(_property.GetSetMethod(nonPublic), _reflectedType);
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return SetReflectedType(_property.GetAccessors(nonPublic), _reflectedType);
        }

        public override object GetRawConstantValue()
        {
            return _property.GetRawConstantValue();
        }

        internal override bool IsPublic
        {
            get { return _property.IsPublic; }
        }

        internal override bool IsNonPrivate
        {
            get { return _property.IsNonPrivate; }
        }

        internal override bool IsStatic
        {
            get { return _property.IsStatic; }
        }

        internal override PropertySignature PropertySignature
        {
            get { return _property.PropertySignature; }
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            var parameters = _property.GetIndexParameters();
            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        internal override PropertyInfo BindTypeParameters(Type type)
        {
            return _property.BindTypeParameters(type);
        }

        public override string ToString()
        {
            return _property.ToString();
        }

        public override bool __IsMissing
        {
            get { return _property.__IsMissing; }
        }

        public override Type DeclaringType
        {
            get { return _property.DeclaringType; }
        }

        public override bool Equals(object obj)
        {
            var other = obj as PropertyInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._property == _property;
        }

        public override int GetHashCode()
        {
            return _reflectedType.GetHashCode() ^ _property.GetHashCode();
        }

        public override int MetadataToken
        {
            get { return _property.MetadataToken; }
        }

        public override Module Module
        {
            get { return _property.Module; }
        }

        public override string Name
        {
            get { return _property.Name; }
        }

        internal override bool IsBaked
        {
            get { return _property.IsBaked; }
        }

        internal override int GetCurrentToken()
        {
            return _property.GetCurrentToken();
        }

    }

}
