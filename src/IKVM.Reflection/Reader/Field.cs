/*
  Copyright (C) 2009 Jeroen Frijters

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
using System;

using IKVM.Reflection.Metadata;

namespace IKVM.Reflection.Reader
{

    sealed class FieldDefImpl : FieldInfo
    {

        readonly ModuleReader module;
        readonly TypeDefImpl declaringType;
        readonly int index;

        FieldSignature lazyFieldSig;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="index"></param>
        internal FieldDefImpl(ModuleReader module, TypeDefImpl declaringType, int index)
        {
            this.module = module;
            this.declaringType = declaringType;
            this.index = index;
        }

        public override FieldAttributes Attributes
        {
            get { return (FieldAttributes)module.FieldTable.records[index].Flags; }
        }

        public override Type DeclaringType
        {
            get { return declaringType.IsModulePseudoType ? null : declaringType; }
        }

        public override string Name
        {
            get { return module.GetString(module.FieldTable.records[index].Name); }
        }

        public override string ToString()
        {
            return this.FieldType.Name + " " + this.Name;
        }

        public override Module Module
        {
            get { return module; }
        }

        public override int MetadataToken
        {
            get { return (FieldTable.Index << 24) + index + 1; }
        }

        public override object GetRawConstantValue()
        {
            return module.ConstantTable.GetRawConstantValue(module, this.MetadataToken);
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            foreach (int i in this.Module.FieldLayoutTable.Filter(index + 1))
            {
                offset = Module.FieldLayoutTable.records[i].Offset;
                return true;
            }

            offset = 0;
            return false;
        }

        internal override FieldSignature FieldSignature
        {
            get { return lazyFieldSig ??= FieldSignature.ReadSig(module, module.GetBlobReader(module.FieldTable.records[index].Signature), declaringType); }
        }

        internal override int ImportTo(Emit.ModuleBuilder module)
        {
            return module.ImportMethodOrField(declaringType, this.Name, this.FieldSignature);
        }

        internal override int GetCurrentToken()
        {
            return this.MetadataToken;
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

    }

}
