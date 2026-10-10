/*
  Copyright (C) 2008-2012 Jeroen Frijters

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
using System.Reflection.Metadata;

using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    internal sealed class FieldBuilder : FieldInfo
    {

        readonly TypeBuilder _type;
        readonly string _name;
        readonly int _pseudoToken;
        FieldAttributes _attributes;
        readonly FieldSignature _signature;
        int _offset = -1;

        BlobHandle _signatureBlobHandle;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="fieldType"></param>
        /// <param name="customModifiers"></param>
        /// <param name="attributes"></param>
        internal FieldBuilder(TypeBuilder type, string name, Type fieldType, CustomModifiers customModifiers, FieldAttributes attributes)
        {
            this._type = type;
            this._name = name;
            this._attributes = attributes;
            this._pseudoToken = type.ModuleBuilder.AllocPseudoToken();
            this._signature = FieldSignature.Create(fieldType, customModifiers);

            // create signature blob
            var buf = new BlobBuilder(5);
            _signature.Write(type.ModuleBuilder, buf);
            _signatureBlobHandle = type.ModuleBuilder.GetOrAddBlob(buf);
        }

        public void SetConstant(object defaultValue)
        {
            _attributes |= FieldAttributes.HasDefault;
            _type.ModuleBuilder.AddConstant(_pseudoToken, defaultValue);
        }

        public override object GetRawConstantValue()
        {
            if (!_type.IsCreated())
            {
                // the .NET FieldBuilder doesn't support this method
                // (since we dont' have a different FieldInfo object after baking, we will support it once we're baked)
                throw new NotSupportedException();
            }

            return _type.ModuleBuilder.GetConstant(GetCurrentToken());
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            return (offset = this._offset) != -1;
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.FieldOffsetAttribute:
                    SetOffset((int)customBuilder.DecodeBlob(Module.Assembly).GetConstructorArgument(0));
                    break;
                case KnownCA.MarshalAsAttribute:
                    FieldMarshal.SetMarshalAsAttribute(_type.ModuleBuilder, _pseudoToken, customBuilder);
                    _attributes |= FieldAttributes.HasFieldMarshal;
                    break;
                case KnownCA.NonSerializedAttribute:
                    _attributes |= FieldAttributes.NotSerialized;
                    break;
                case KnownCA.SpecialNameAttribute:
                    _attributes |= FieldAttributes.SpecialName;
                    break;
                default:
                    _type.ModuleBuilder.SetCustomAttribute(_pseudoToken, customBuilder);
                    break;
            }
        }

        public void SetOffset(int iOffset)
        {
            _offset = iOffset;
        }

        public override FieldAttributes Attributes
        {
            get { return _attributes; }
        }

        public override Type DeclaringType
        {
            get { return _type.IsModulePseudoType ? null : _type; }
        }

        public override string Name
        {
            get { return _name; }
        }

        public override int MetadataToken
        {
            get { return _pseudoToken; }
        }

        public override Module Module
        {
            get { return _type.Module; }
        }

        internal void WriteFieldRecords()
        {
            _type.ModuleBuilder.Metadata.AddFieldDefinition(
                (System.Reflection.FieldAttributes)_attributes,
                _type.ModuleBuilder.GetOrAddString(_name),
                _signatureBlobHandle);
        }

        internal void FixupToken(int token)
        {
            _type.ModuleBuilder.RegisterTokenFixup(_pseudoToken, token);
        }

        internal override FieldSignature FieldSignature
        {
            get { return _signature; }
        }

        internal override int ImportTo(ModuleBuilder other)
        {
            return other.ImportMethodOrField(_type, _name, _signature);
        }

        internal override int GetCurrentToken()
        {
            if (_type.ModuleBuilder.IsSaved)
                return _type.ModuleBuilder.ResolvePseudoToken(_pseudoToken);
            else
                return _pseudoToken;
        }

        internal override bool IsBaked
        {
            get { return _type.IsBaked; }
        }

    }

}
