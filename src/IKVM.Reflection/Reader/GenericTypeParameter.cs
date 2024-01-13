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

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class GenericTypeParameter : TypeParameterType
    {

        readonly ModuleReader module;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="index"></param>
        /// <param name="sigElementType"></param>
        internal GenericTypeParameter(ModuleReader module, int index, byte sigElementType) :
            base(sigElementType)
        {
            this.module = module;
            this.index = index;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string Namespace
        {
            get { return DeclaringType.Namespace; }
        }

        public override string Name
        {
            get { return module.GetString(module.GenericParam.records[index].Name); }
        }

        public override Module Module
        {
            get { return module; }
        }

        public override int MetadataToken
        {
            get { return (GenericParamTable.Index << 24) + index + 1; }
        }

        public override int GenericParameterPosition
        {
            get { return module.GenericParam.records[index].Number; }
        }

        public override Type DeclaringType
        {
            get
            {
                var owner = module.GenericParam.records[index].Owner;
                return (owner >> 24) == TypeDefTable.Index ? module.ResolveType(owner) : null;
            }
        }

        public override MethodBase DeclaringMethod
        {
            get
            {
                var owner = module.GenericParam.records[index].Owner;
                return (owner >> 24) == MethodDefTable.Index ? module.ResolveMethod(owner) : null;
            }
        }

        public override Type[] GetGenericParameterConstraints()
        {
            var context = (DeclaringMethod as IGenericContext) ?? DeclaringType;
            var list = new List<Type>();
            foreach (int i in module.GenericParamConstraint.Filter(MetadataToken))
                list.Add(module.ResolveType(module.GenericParamConstraint.records[i].Constraint, context));

            return list.ToArray();
        }

        public override CustomModifiers[] __GetGenericParameterConstraintCustomModifiers()
        {
            var context = (this.DeclaringMethod as IGenericContext) ?? DeclaringType;
            var list = new List<CustomModifiers>();
            foreach (var i in module.GenericParamConstraint.Filter(MetadataToken))
            {
                var mods = new CustomModifiers();
                var metadataToken = module.GenericParamConstraint.records[i].Constraint;
                if ((metadataToken >> 24) == TypeSpecTable.Index)
                {
                    var index = (metadataToken & 0xFFFFFF) - 1;
                    mods = CustomModifiers.Read(module, module.GetBlobReader(module.TypeSpec.records[index]), context);
                }

                list.Add(mods);
            }

            return list.ToArray();
        }

        public override GenericParameterAttributes GenericParameterAttributes
        {
            get { return (GenericParameterAttributes)module.GenericParam.records[index].Flags; }
        }

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            var owner = module.GenericParam.records[index].Owner;
            if ((owner >> 24) == MethodDefTable.Index)
            {
                return binder.BindMethodParameter(this);
            }
            else
            {
                return binder.BindTypeParameter(this);
            }
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

    }

}
