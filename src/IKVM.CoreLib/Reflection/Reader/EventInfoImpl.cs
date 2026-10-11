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

        readonly ModuleReader module;
        readonly Type declaringType;
        readonly EventDefinitionHandle handle;
        readonly EventDefinition definition;
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
        internal EventInfoImpl(ModuleReader module, Type declaringType, EventDefinitionHandle handle)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.handle = handle;
            this.definition = module.Metadata.GetEventDefinition(handle);
        }

        public override bool Equals(object obj) => obj is EventInfoImpl other && other.declaringType == declaringType && other.handle == handle;

        public override int GetHashCode() => declaringType.GetHashCode() * 123 + MetadataTokens.GetRowNumber(handle);

        public override EventAttributes Attributes => (EventAttributes)definition.Attributes;

        public override MethodInfo GetAddMethod(bool nonPublic) => Accessor(definition.GetAccessors().Adder, nonPublic);

        public override MethodInfo GetRaiseMethod(bool nonPublic) => Accessor(definition.GetAccessors().Raiser, nonPublic);

        public override MethodInfo GetRemoveMethod(bool nonPublic) => Accessor(definition.GetAccessors().Remover, nonPublic);

        public override MethodInfo[] GetOtherMethods(bool nonPublic)
        {
            var list = new List<MethodInfo>();
            foreach (var h in definition.GetAccessors().Others)
                if (Accessor(h, nonPublic) is { } m)
                    list.Add(m);

            return list.ToArray();
        }

        MethodInfo Accessor(MethodDefinitionHandle h, bool nonPublic)
        {
            if (h.IsNil)
                return null;

            var method = (MethodInfo)module.ResolveMethod(MetadataTokens.GetToken(h));
            return nonPublic || method.IsPublic ? method : null;
        }

        IEnumerable<MethodInfo> AllAccessors()
        {
            var accessors = definition.GetAccessors();
            foreach (var h in new[] { accessors.Adder, accessors.Remover, accessors.Raiser })
                if (Accessor(h, true) is { } m)
                    yield return m;

            foreach (var h in accessors.Others)
                if (Accessor(h, true) is { } m)
                    yield return m;
        }

        public override Type EventHandlerType => module.ResolveType(MetadataTokens.GetToken(definition.Type), declaringType);

        public override string Name => module.GetString(definition.Name);

        public override Type DeclaringType => declaringType;

        public override Module Module => module;

        public override int MetadataToken => MetadataTokens.GetToken(handle);

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
            foreach (var method in AllAccessors())
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
