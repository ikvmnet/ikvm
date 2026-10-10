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

    /// <summary>
    /// Represents a property of a constructed generic type, wrapping the property of the generic type definition.
    /// </summary>
    sealed class GenericPropertyInfo : PropertyInfo
    {

        readonly Type _typeInstance;
        readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeInstance">The constructed generic type that declares the property.</param>
        /// <param name="property">The property of the generic type definition.</param>
        internal GenericPropertyInfo(Type typeInstance, PropertyInfo property)
        {
            _typeInstance = typeInstance;
            _property = property;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = obj as GenericPropertyInfo;
            return other != null && other._typeInstance == _typeInstance && other._property == _property;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _typeInstance.GetHashCode() * 537 + _property.GetHashCode();

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _property.Attributes;

        /// <inheritdoc />
        public override bool CanRead => _property.CanRead;

        /// <inheritdoc />
        public override bool CanWrite => _property.CanWrite;

        /// <summary>
        /// Wraps an accessor of the underlying property as a method of the constructed generic type.
        /// </summary>
        /// <param name="method">The accessor method, or <see langword="null"/>.</param>
        /// <returns>The accessor as a member of the constructed type, or <see langword="null"/> if <paramref name="method"/> is <see langword="null"/>.</returns>
        MethodInfo Wrap(MethodInfo method)
        {
            if (method == null)
                return null;

            return new GenericMethodInstance(_typeInstance, method, null);
        }

        /// <inheritdoc />
        public override MethodInfo GetGetMethod(bool nonPublic) => Wrap(_property.GetGetMethod(nonPublic));

        /// <inheritdoc />
        public override MethodInfo GetSetMethod(bool nonPublic) => Wrap(_property.GetSetMethod(nonPublic));

        /// <inheritdoc />
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var accessors = _property.GetAccessors(nonPublic);
            for (var i = 0; i < accessors.Length; i++)
                accessors[i] = Wrap(accessors[i]);

            return accessors;
        }

        /// <inheritdoc />
        public override object GetRawConstantValue() => _property.GetRawConstantValue();

        /// <inheritdoc />
        internal override bool IsPublic => _property.IsPublic;

        /// <inheritdoc />
        internal override bool IsNonPrivate => _property.IsNonPrivate;

        /// <inheritdoc />
        internal override bool IsStatic => _property.IsStatic;

        /// <summary>
        /// Gets the signature of the property, with the generic type parameters replaced by the type arguments of the declaring type.
        /// </summary>
        internal override PropertySignature PropertySignature => _property.PropertySignature.ExpandTypeParameters(_typeInstance);

        /// <inheritdoc />
        public override string Name => _property.Name;

        /// <inheritdoc />
        public override Type DeclaringType => _typeInstance;

        /// <inheritdoc />
        public override Module Module => _typeInstance.Module;

        /// <inheritdoc />
        public override int MetadataToken => _property.MetadataToken;

        /// <inheritdoc />
        internal override PropertyInfo BindTypeParameters(Type type) => new GenericPropertyInfo(_typeInstance.BindTypeParameters(type), _property);

        /// <inheritdoc />
        internal override bool IsBaked => _property.IsBaked;

        /// <inheritdoc />
        internal override int GetCurrentToken() => _property.GetCurrentToken();

    }

}
