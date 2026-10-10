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

    /// <summary>
    /// Wraps a property obtained through a type other than the one that declares it, such as an inherited property
    /// returned from a derived type. The wrapper delegates to the underlying property but remembers the type it was
    /// obtained through, which takes part in equality and is carried over to the accessor methods and index
    /// parameters it returns.
    /// </summary>
    sealed class PropertyInfoWithReflectedType : PropertyInfo
    {

        readonly Type _reflectedType;
        readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType">The type the property was obtained through.</param>
        /// <param name="property">The underlying property.</param>
        internal PropertyInfoWithReflectedType(Type reflectedType, PropertyInfo property)
        {
            _reflectedType = reflectedType;
            _property = property;
        }

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _property.Attributes;

        /// <inheritdoc />
        public override bool CanRead => _property.CanRead;

        /// <inheritdoc />
        public override bool CanWrite => _property.CanWrite;

        /// <summary>
        /// Returns the public or non-public get accessor for this property, obtained through the same reflected type
        /// as this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether a non-public get accessor should be returned.</param>
        /// <returns>The get accessor, or <see langword="null"/> if there is none or it is non-public and <paramref name="nonPublic"/> is <see langword="false"/>.</returns>
        public override MethodInfo GetGetMethod(bool nonPublic) => SetReflectedType(_property.GetGetMethod(nonPublic), _reflectedType);

        /// <summary>
        /// Returns the set accessor for this property, obtained through the same reflected type as this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether the accessor should be returned if it is non-public.</param>
        /// <returns>The set accessor, or <see langword="null"/> if there is none or it is non-public and <paramref name="nonPublic"/> is <see langword="false"/>.</returns>
        public override MethodInfo GetSetMethod(bool nonPublic) => SetReflectedType(_property.GetSetMethod(nonPublic), _reflectedType);

        /// <summary>
        /// Returns an array whose elements reflect the public and, if specified, non-public accessors of this
        /// property, obtained through the same reflected type as this property.
        /// </summary>
        /// <param name="nonPublic">Indicates whether non-public methods should be returned in the array.</param>
        /// <returns>An array of the accessors of this property.</returns>
        public override MethodInfo[] GetAccessors(bool nonPublic) => SetReflectedType(_property.GetAccessors(nonPublic), _reflectedType);

        /// <inheritdoc />
        public override object GetRawConstantValue() => _property.GetRawConstantValue();

        /// <inheritdoc />
        internal override bool IsPublic => _property.IsPublic;

        /// <inheritdoc />
        internal override bool IsNonPrivate => _property.IsNonPrivate;

        /// <inheritdoc />
        internal override bool IsStatic => _property.IsStatic;

        /// <inheritdoc />
        internal override PropertySignature PropertySignature => _property.PropertySignature;

        /// <summary>
        /// Returns the index parameters of the underlying property, each wrapped so that its member is this wrapper.
        /// </summary>
        /// <returns>An array containing the parameters for the indexes, or an empty array if the property is not indexed.</returns>
        public override ParameterInfo[] GetIndexParameters()
        {
            var parameters = _property.GetIndexParameters();
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = new ParameterInfoWrapper(this, parameters[i]);

            return parameters;
        }

        /// <inheritdoc />
        internal override PropertyInfo BindTypeParameters(Type type) => _property.BindTypeParameters(type);

        /// <inheritdoc />
        public override string ToString() => _property.ToString();

        /// <inheritdoc />
        public override bool __IsMissing => _property.__IsMissing;

        /// <inheritdoc />
        public override Type DeclaringType => _property.DeclaringType;

        /// <summary>
        /// Determines whether the specified object is a wrapper of the same property obtained through the same type.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> wraps the same property and reflected type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as PropertyInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._property == _property;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _property.GetHashCode();

        /// <inheritdoc />
        public override int MetadataToken => _property.MetadataToken;

        /// <inheritdoc />
        public override Module Module => _property.Module;

        /// <inheritdoc />
        public override string Name => _property.Name;

        /// <inheritdoc />
        internal override bool IsBaked => _property.IsBaked;

        /// <inheritdoc />
        internal override int GetCurrentToken() => _property.GetCurrentToken();

    }

}
