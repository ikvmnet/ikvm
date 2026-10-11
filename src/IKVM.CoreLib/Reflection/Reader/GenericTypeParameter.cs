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

    sealed class GenericTypeParameter : TypeParameterType
    {

        readonly ModuleReader module;
        readonly GenericParameterHandle handle;
        readonly GenericParameter definition;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="handle"></param>
        /// <param name="sigElementType"></param>
        internal GenericTypeParameter(ModuleReader module, GenericParameterHandle handle, byte sigElementType) :
            base(sigElementType)
        {
            this.module = module;
            this.handle = handle;
            this.definition = module.Metadata.GetGenericParameter(handle);
        }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public override string Namespace => DeclaringType.Namespace;

        public override string Name => module.GetString(definition.Name);

        public override Module Module => module;

        public override int MetadataToken => MetadataTokens.GetToken(handle);

        public override int GenericParameterPosition => definition.Index;

        public override Type DeclaringType => definition.Parent.Kind == HandleKind.TypeDefinition ? module.ResolveType(MetadataTokens.GetToken(definition.Parent)) : null;

        public override MethodBase DeclaringMethod => definition.Parent.Kind == HandleKind.MethodDefinition ? module.ResolveMethod(MetadataTokens.GetToken(definition.Parent)) : null;

        public override Type[] GetGenericParameterConstraints()
        {
            var context = (DeclaringMethod as IGenericContext) ?? DeclaringType;
            var handles = definition.GetConstraints();
            var constraints = handles.Count == 0 ? Type.EmptyTypes : new Type[handles.Count];
            for (int i = 0; i < constraints.Length; i++)
                constraints[i] = module.ResolveType(MetadataTokens.GetToken(module.Metadata.GetGenericParameterConstraint(handles[i]).Type), context);

            return constraints;
        }

        public override GenericParameterAttributes GenericParameterAttributes => (GenericParameterAttributes)definition.Attributes;

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            return definition.Parent.Kind == HandleKind.MethodDefinition ? binder.BindMethodParameter(this) : binder.BindTypeParameter(this);
        }

        internal override bool IsBaked => true;

    }

}
