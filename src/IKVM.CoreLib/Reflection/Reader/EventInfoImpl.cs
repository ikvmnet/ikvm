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

    sealed class EventInfoImpl : EventInfo
    {

        readonly ModuleReader _module;
        readonly Type _declaringType;
        readonly EventDefinitionHandle _handle;
        readonly EventDefinition _definition;
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
        internal EventInfoImpl(ModuleReader module, Type declaringType, EventDefinitionHandle handle)
        {
            _module = module;
            _declaringType = declaringType;
            _handle = handle;
            _definition = module.Metadata.GetEventDefinition(handle);
        }

        public override bool Equals(object obj) => obj is EventInfoImpl other && other._declaringType == _declaringType && other._handle == _handle;

        public override int GetHashCode() => _declaringType.GetHashCode() * 123 + MetadataTokens.GetRowNumber(_handle);

        public override EventAttributes Attributes => (EventAttributes)_definition.Attributes;

        public override MethodInfo GetAddMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Adder, nonPublic);

        public override MethodInfo GetRaiseMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Raiser, nonPublic);

        public override MethodInfo GetRemoveMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Remover, nonPublic);

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var h in _definition.GetAccessors().Others)
                if (Accessor(h, nonPublic) is { } m)
                    list.Add(m);

            return list.ToArray();
        }

        MethodInfo Accessor(MethodDefinitionHandle h, bool nonPublic)
        {
            if (h.IsNil)
                return null;

            var method = (MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        IEnumerable<MethodInfo> AllAccessors()
        {
            var accessors = _definition.GetAccessors();
            foreach (var h in new[] { accessors.Adder, accessors.Remover, accessors.Raiser })
                if (Accessor(h, true) is { } m)
                    yield return m;

            foreach (var h in accessors.Others)
                if (Accessor(h, true) is { } m)
                    yield return m;
        }

        public override Type EventHandlerType => _module.ResolveType(MetadataTokens.GetToken(_definition.Type), _declaringType);

        public override string Name => _module.GetString(_definition.Name);

        public override Type DeclaringType => _declaringType;

        public override Module Module => _module;

        public override int MetadataToken => MetadataTokens.GetToken(_handle);

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
            foreach (var method in AllAccessors())
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
