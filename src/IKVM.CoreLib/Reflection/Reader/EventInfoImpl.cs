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
    /// An event read from a row of the Event table of a module loaded from a PE image.
    /// </summary>
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
        /// <param name="module">The module that contains the event definition.</param>
        /// <param name="declaringType">The type that declares the event.</param>
        /// <param name="handle">The handle of the event definition.</param>
        internal EventInfoImpl(ModuleReader module, Type declaringType, EventDefinitionHandle handle)
        {
            _module = module;
            _declaringType = declaringType;
            _handle = handle;
            _definition = module.Metadata.GetEventDefinition(handle);
        }

        /// <summary>
        /// Determines whether the specified object represents the same event definition of the same declaring type.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="EventInfoImpl"/> for the same declaring type and handle; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => obj is EventInfoImpl other && other._declaringType == _declaringType && other._handle == _handle;

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() * 123 + MetadataTokens.GetRowNumber(_handle);

        /// <inheritdoc />
        public override EventAttributes Attributes => (EventAttributes)_definition.Attributes;

        /// <inheritdoc />
        public override MethodInfo GetAddMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Adder, nonPublic);

        /// <inheritdoc />
        public override MethodInfo GetRaiseMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Raiser, nonPublic);

        /// <inheritdoc />
        public override MethodInfo GetRemoveMethod(bool nonPublic) => Accessor(_definition.GetAccessors().Remover, nonPublic);

        /// <inheritdoc />
        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var h in _definition.GetAccessors().Others)
                if (Accessor(h, nonPublic) is { } m)
                    list.Add(m);

            return list.ToArray();
        }

        /// <summary>
        /// Resolves an accessor method of the event, filtering out non-public accessors unless requested.
        /// </summary>
        /// <param name="h">The handle of the accessor method, which may be nil.</param>
        /// <param name="nonPublic"><see langword="true"/> to return the accessor even if it is not public.</param>
        /// <returns>The accessor method, or <see langword="null"/> if the handle is nil or the accessor is non-public and <paramref name="nonPublic"/> is <see langword="false"/>.</returns>
        MethodInfo Accessor(MethodDefinitionHandle h, bool nonPublic)
        {
            if (h.IsNil)
                return null;

            var method = (MethodInfo)_module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        /// <summary>
        /// Enumerates every accessor method of the event, public or not: add, remove, raise and other methods.
        /// </summary>
        /// <returns>The accessor methods that are present.</returns>
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

        /// <inheritdoc />
        public override Type EventHandlerType => _module.ResolveType(MetadataTokens.GetToken(_definition.Type), _declaringType);

        /// <inheritdoc />
        public override string Name => _module.GetString(_definition.Name);

        /// <inheritdoc />
        public override Type DeclaringType => _declaringType;

        /// <inheritdoc />
        public override Module Module => _module;

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <summary>
        /// Gets a value indicating whether any accessor of the event is public.
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
        /// Gets a value indicating whether any accessor of the event is more accessible than private.
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
        /// Gets a value indicating whether any accessor of the event is static.
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
        /// Computes and caches the accessibility and static flags of the event from all of its accessors.
        /// </summary>
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

        /// <summary>
        /// Gets a value indicating whether the event is complete; an event read from a PE image always is.
        /// </summary>
        internal override bool IsBaked => true;

        /// <inheritdoc />
        internal override int GetCurrentToken() => MetadataToken;

    }

}
