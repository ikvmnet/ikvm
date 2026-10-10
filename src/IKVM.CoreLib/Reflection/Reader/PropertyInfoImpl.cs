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
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        internal PropertyInfoImpl(ModuleReader module, Type declaringType, PropertyDefinitionHandle handle)
        {
            this._module = module;
            this._declaringType = declaringType;
            this._handle = handle;
            this._definition = module.Metadata.GetPropertyDefinition(handle);
        }

        public override bool Equals(object obj) => obj is PropertyInfoImpl other && other.DeclaringType == _declaringType && other._handle == _handle;

        public override int GetHashCode() => _declaringType.GetHashCode() * 77 + MetadataTokens.GetRowNumber(_handle);

        internal override PropertySignature PropertySignature => _sig ??= PropertySignature.ReadSig(_module, _module.GetBlobReader(_definition.Signature), _declaringType);

        public override PropertyAttributes Attributes => (PropertyAttributes)_definition.Attributes;

        public override object GetRawConstantValue() => _module.GetConstantValue(_definition.GetDefaultValue());

        public override bool CanRead => GetGetMethod(true) != null;

        public override bool CanWrite => GetSetMethod(true) != null;

        public override MethodInfo GetGetMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Getter, nonPublic);

        public override MethodInfo GetSetMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Setter, nonPublic);

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

        MethodInfo Accessor(MethodDefinitionHandle h, bool nonPublic)
        {
            if (h.IsNil)
                return null;

            var method = (MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        public override Type DeclaringType => _declaringType;

        public override Module Module => _module;

        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        public override string Name => _module.GetString(_definition.Name);

        internal override bool IsPublic
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isPublic;
            }
        }

        internal override bool IsNonPrivate
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isNonPrivate;
            }
        }

        internal override bool IsStatic
        {
            get
            {
                if (!_flagsCached)
                    ComputeFlags();

                return _isStatic;
            }
        }

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

        internal override bool IsBaked => true;

        internal override int GetCurrentToken() => MetadataToken;

    }

}
