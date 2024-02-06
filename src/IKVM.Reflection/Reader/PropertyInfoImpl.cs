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
using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class PropertyInfoImpl : PropertyInfo
    {

        readonly ModuleReader module;
        readonly Type declaringType;
        readonly int index;
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
        /// <param name="index"></param>
        internal PropertyInfoImpl(ModuleReader module, Type declaringType, int index)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.index = index;
        }

        public override bool Equals(object obj)
        {
            return obj is PropertyInfoImpl other && other.DeclaringType == declaringType && other.index == index;
        }

        public override int GetHashCode() => declaringType.GetHashCode() * 77 + index;

        internal override PropertySignature PropertySignature => sig ??= PropertySignature.ReadSig(module, module.GetBlob(module.Property.records[index].Type), declaringType);

        public override PropertyAttributes Attributes => (PropertyAttributes)module.Property.records[index].Flags;

        public override object GetRawConstantValue() => module.Constant.GetRawConstantValue(module, this.MetadataToken);

        public override bool CanRead => GetGetMethod(true) != null;

        public override bool CanWrite => GetSetMethod(true) != null;

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            return module.MethodSemantics.GetMethod(module, this.MetadataToken, nonPublic, MethodSemanticsTable.Getter);
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            return module.MethodSemantics.GetMethod(module, this.MetadataToken, nonPublic, MethodSemanticsTable.Setter);
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            return module.MethodSemantics.GetMethods(module, this.MetadataToken, nonPublic, MethodSemanticsTable.Getter | MethodSemanticsTable.Setter | MethodSemanticsTable.Other);
        }

        public override Type DeclaringType => declaringType;

        public override Module Module => module;

        public override int MetadataToken => (PropertyTable.Index << 24) + index + 1;

        public override string Name => module.GetString(module.Property.records[index].Name);

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
            module.MethodSemantics.ComputeFlags(module, MetadataToken, out isPublic, out isNonPrivate, out isStatic);
            flagsCached = true;
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

        internal override int GetCurrentToken()
        {
            return this.MetadataToken;
        }

    }

}
