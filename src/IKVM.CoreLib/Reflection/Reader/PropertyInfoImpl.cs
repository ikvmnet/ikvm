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

        readonly ModuleReader module;
        readonly Type declaringType;
        readonly PropertyDefinitionHandle handle;
        readonly PropertyDefinition definition;

        PropertySignature sig;
        bool isPublic;
        bool isNonPrivate;
        bool isStatic;
        bool flagsCached;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        internal PropertyInfoImpl(ModuleReader module, Type declaringType, PropertyDefinitionHandle handle)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.handle = handle;
            this.definition = module.Metadata.GetPropertyDefinition(handle);
        }

        public override bool Equals(object obj) => obj is PropertyInfoImpl other && other.DeclaringType == declaringType && other.handle == handle;

        public override int GetHashCode() => declaringType.GetHashCode() * 77 + MetadataTokens.GetRowNumber(handle);

        internal override PropertySignature PropertySignature => sig ??= PropertySignature.ReadSig(module, module.GetBlobReader(definition.Signature), declaringType);

        public override PropertyAttributes Attributes => (PropertyAttributes)definition.Attributes;

        public override object GetRawConstantValue() => module.GetConstantValue(definition.GetDefaultValue());

        public override bool CanRead => GetGetMethod(true) != null;

        public override bool CanWrite => GetSetMethod(true) != null;

        public override MethodInfo GetGetMethod(bool nonPublic) => Accessor(definition.GetAccessors().Getter, nonPublic);

        public override MethodInfo GetSetMethod(bool nonPublic) => Accessor(definition.GetAccessors().Setter, nonPublic);

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            var accessors = definition.GetAccessors();
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

            var method = (MethodInfo)module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        public override Type DeclaringType => declaringType;

        public override Module Module => module;

        public override int MetadataToken => MetadataTokens.GetToken(handle);

        public override string Name => module.GetString(definition.Name);

        internal override bool IsPublic
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isPublic;
            }
        }

        internal override bool IsNonPrivate
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isNonPrivate;
            }
        }

        internal override bool IsStatic
        {
            get
            {
                if (!flagsCached)
                    ComputeFlags();

                return isStatic;
            }
        }

        void ComputeFlags()
        {
            foreach (var method in GetAccessors(true))
            {
                isPublic |= method.IsPublic;
                isNonPrivate |= (method.Attributes & MethodAttributes.MemberAccessMask) > MethodAttributes.Private;
                isStatic |= method.IsStatic;
            }

            flagsCached = true;
        }

        internal override bool IsBaked => true;

        internal override int GetCurrentToken() => MetadataToken;

    }

}
