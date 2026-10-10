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

        readonly Type _typeInstance;
        readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeInstance"></param>
        /// <param name="property"></param>
        internal GenericPropertyInfo(Type typeInstance, PropertyInfo property)
        {
            this._typeInstance = typeInstance;
            this._property = property;
        }

        public override bool Equals(object obj)
        {
            var other = obj as GenericPropertyInfo;
            return other != null && other._typeInstance == _typeInstance && other._property == _property;
        }

        public override int GetHashCode()
        {
            return _typeInstance.GetHashCode() * 537 + _property.GetHashCode();
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

        private MethodInfo Wrap(MethodInfo method)
        {
            if (method == null)
                return null;

            return new GenericMethodInstance(_typeInstance, method, null);
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return Wrap(_property.GetGetMethod(nonPublic));
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return Wrap(_property.GetSetMethod(nonPublic));
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var accessors = _property.GetAccessors(nonPublic);
            for (int i = 0; i < accessors.Length; i++)
                accessors[i] = Wrap(accessors[i]);

            return accessors;
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
            get { return _property.PropertySignature.ExpandTypeParameters(_typeInstance); }
        }

        public override string Name
        {
            get { return _property.Name; }
        }

        public override Type DeclaringType
        {
            get { return _typeInstance; }
        }

        public override Module Module
        {
            get { return _typeInstance.Module; }
        }

        public override int MetadataToken
        {
            get { return _property.MetadataToken; }
        }

        internal override PropertyInfo BindTypeParameters(Type type)
        {
            return new GenericPropertyInfo(_typeInstance.BindTypeParameters(type), _property);
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
