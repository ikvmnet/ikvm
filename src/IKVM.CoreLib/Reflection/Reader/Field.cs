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
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

namespace IKVM.Reflection.Reader
{

    sealed class FieldDefImpl : FieldInfo
    {

        readonly ModuleReader _module;
        readonly TypeDefImpl _declaringType;
        readonly FieldDefinitionHandle _handle;
        readonly FieldDefinition _definition;

        FieldSignature _lazyFieldSig;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="handle"></param>
        internal FieldDefImpl(ModuleReader module, TypeDefImpl declaringType, FieldDefinitionHandle handle)
        {
            this._module = module;
            this._declaringType = declaringType;
            this._handle = handle;
            this._definition = module.Metadata.GetFieldDefinition(handle);
        }

        public override FieldAttributes Attributes => (FieldAttributes)_definition.Attributes;

        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        public override string Name => _module.GetString(_definition.Name);

        public override string ToString() => FieldType.Name + " " + Name;

        public override Module Module => _module;

        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        public override object GetRawConstantValue() => _module.GetConstantValue(_definition.GetDefaultValue());

        public override bool __TryGetFieldOffset(out int offset)
        {
            offset = _definition.GetOffset();
            if (offset != -1)
                return true;

            offset = 0;
            return false;
        }

        internal override FieldSignature FieldSignature => _lazyFieldSig ??= FieldSignature.ReadSig(_module, _module.GetBlobReader(_definition.Signature), _declaringType);

        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(_declaringType, Name, FieldSignature);

        internal override int GetCurrentToken() => MetadataToken;

        internal override bool IsBaked => true;

    }

}
