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
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a property defined by a row of the Property table of a module read from metadata.
    /// </summary>
    sealed class PropertyInfoImpl : PropertyInfo
    {

        readonly ModuleReader _module;
        readonly Type _declaringType;
        readonly PropertyDefinitionHandle _handle;
        readonly PropertyDefinition _definition;

        PropertySignature _sig;
        bool _isPublic;
        bool _isNonPrivate;
        bool _isStatic;
        bool _flagsCached;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module that contains the property definition.</param>
        /// <param name="declaringType">The type that declares the property.</param>
        /// <param name="handle">The handle of the property definition in the module's metadata.</param>
        internal PropertyInfoImpl(ModuleReader module, Type declaringType, PropertyDefinitionHandle handle)
        {
            _module = module;
            _declaringType = declaringType;
            _handle = handle;
            _definition = module.Metadata.GetPropertyDefinition(handle);
        }

        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object. Two instances are equal
        /// when they have the same declaring type and property definition.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> represents the same property; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is PropertyInfoImpl other && other.DeclaringType == _declaringType && other._handle == _handle;

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() * 77 + MetadataTokens.GetRowNumber(_handle);

        /// <summary>
        /// Gets the signature of the property, decoded from the property's signature blob on first use.
        /// </summary>
        internal override PropertySignature PropertySignature =>
            _sig ??= PropertySignature.ReadSig(_module, _module.GetBlobReader(_definition.Signature), _declaringType);

        /// <inheritdoc />
        public override PropertyAttributes Attributes => (PropertyAttributes)_definition.Attributes;

        /// <inheritdoc />
        public override object GetRawConstantValue() => _module.GetConstantValue(_definition.GetDefaultValue());

        /// <inheritdoc />
        public override bool CanRead => GetGetMethod(true) != null;

        /// <inheritdoc />
        public override bool CanWrite => GetSetMethod(true) != null;

        /// <inheritdoc />
        public override MethodInfo GetGetMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Getter, nonPublic);

        /// <inheritdoc />
        public override MethodInfo GetSetMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Setter, nonPublic);

        /// <inheritdoc />
        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var accessors = _definition.GetAccessors();
            var list = new List<MethodInfo>();
            Add(accessors.Getter);
            Add(accessors.Setter);
            foreach (var h in accessors.Others)
                Add(h);

            return list.ToArray();

            void Add(MethodDefinitionHandle h)
            {
                if (Accessor(h, nonPublic) is { } m)
                    list.Add(m);
            }
        }

        /// <summary>
        /// Resolves an accessor method of the property.
        /// </summary>
        /// <param name="h">The handle of the accessor method, which may be nil.</param>
        /// <param name="nonPublic"><see langword="true"/> to return a non-public accessor; otherwise, <see langword="false"/>.</param>
        /// <returns>The accessor, or <see langword="null"/> if <paramref name="h"/> is nil or the accessor is non-public and <paramref name="nonPublic"/> is <see langword="false"/>.</returns>
        MethodInfo Accessor(MethodDefinitionHandle h, bool nonPublic)
        {
            if (h.IsNil)
                return null;

            var method = (MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        /// <inheritdoc />
        public override Type DeclaringType => _declaringType;

        /// <inheritdoc />
        public override Module Module => _module;

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <inheritdoc />
        public override string Name => _module.GetString(_definition.Name);

        /// <summary>
        /// Gets a value indicating whether any accessor of the property is public.
        /// </summary>
        internal override bool IsPublic
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isPublic;
            }
        }

        /// <summary>
        /// Gets a value indicating whether any accessor of the property has an access level above private.
        /// </summary>
        internal override bool IsNonPrivate
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isNonPrivate;
            }
        }

        /// <summary>
        /// Gets a value indicating whether any accessor of the property is static.
        /// </summary>
        internal override bool IsStatic
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isStatic;
            }
        }

        /// <summary>
        /// Computes and caches the public, non-private and static flags from the property's accessors.
        /// </summary>
        void ComputeFlags()
        {
            foreach (var method in GetAccessors(true))
            {
                _isPublic |= method.IsPublic;
                _isNonPrivate |= (method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private;
                _isStatic |= method.IsStatic;
            }

            _flagsCached = true;
        }

        /// <summary>
        /// Gets a value indicating whether the property is complete; a property read from metadata always is.
        /// </summary>
        internal override bool IsBaked => true;

        /// <inheritdoc />
        internal override int GetCurrentToken() => MetadataToken;

    }

}
