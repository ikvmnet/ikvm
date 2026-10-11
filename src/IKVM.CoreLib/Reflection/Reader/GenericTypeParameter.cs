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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a generic type or method parameter defined in the GenericParam table of a module that was read from a file.
    /// </summary>
    sealed class GenericTypeParameter : TypeParameterType
    {

        readonly ModuleReader _module;
        readonly GenericParameterHandle _handle;
        readonly GenericParameter _definition;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module">The module that defines the generic parameter.</param>
        /// <param name="handle">The handle of the generic parameter in the GenericParam table.</param>
        /// <param name="sigElementType">The signature element type: <c>ELEMENT_TYPE_VAR</c> for a type parameter or <c>ELEMENT_TYPE_MVAR</c> for a method parameter.</param>
        internal GenericTypeParameter(ModuleReader module, GenericParameterHandle handle, byte sigElementType) :
            base(sigElementType)
        {
            _module = module;
            _handle = handle;
            _definition = module.Metadata.GetGenericParameter(handle);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) => base.Equals(obj);

        /// <inheritdoc />
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc />
        public override string Namespace => DeclaringType.Namespace;

        /// <inheritdoc />
        public override string Name => _module.GetString(_definition.Name);

        /// <inheritdoc />
        public override Module Module => _module;

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <inheritdoc />
        public override int GenericParameterPosition => _definition.Index;

        /// <inheritdoc />
        public override Type DeclaringType =>
            _definition.Parent.Kind == HandleKind.TypeDefinition ? _module.ResolveType(MetadataTokens.GetToken(_definition.Parent)) : null;

        /// <inheritdoc />
        public override MethodBase DeclaringMethod =>
            _definition.Parent.Kind == HandleKind.MethodDefinition ? _module.ResolveMethod(MetadataTokens.GetToken(_definition.Parent)) : null;

        /// <inheritdoc />
        public override Type[] GetGenericParameterConstraints()
        {
            var context = (DeclaringMethod as IGenericContext) ?? DeclaringType;
            var handles = _definition.GetConstraints();
            var constraints = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
            for (var i = 0; i < constraints.Length; i++)
                constraints[i] = _module.ResolveType(MetadataTokens.GetToken(_module.Metadata.GetGenericParameterConstraint(handles[i]).Type), context);

            return constraints;
        }

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => (GenericParameterAttributes)_definition.Attributes;

        /// <inheritdoc />
        internal override Type BindTypeParameters(IGenericBinder binder) =>
            _definition.Parent.Kind == HandleKind.MethodDefinition ? binder.BindMethodParameter(this) : binder.BindTypeParameter(this);

        /// <inheritdoc />
        internal override bool IsBaked => true;

    }

}
