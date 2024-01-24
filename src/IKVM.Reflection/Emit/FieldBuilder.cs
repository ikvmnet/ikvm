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

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed class FieldBuilder : FieldInfo
    {

        readonly TypeBuilder type;
        readonly string name;
        readonly int pseudoToken;
        FieldAttributes attributes;
        readonly FieldSignature signature;
        int offset = -1;

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
            this.type = type;
            this.name = name;
            this.attributes = attributes;
            this.pseudoToken = type.ModuleBuilder.AllocPseudoToken();
            this.signature = FieldSignature.Create(fieldType, customModifiers);
        }

        public void SetConstant(object defaultValue)
        {
            attributes |= FieldAttributes.HasDefault;
            type.ModuleBuilder.AddConstant(pseudoToken, defaultValue);
        }

        public override object GetRawConstantValue()
        {
            if (!type.IsCreated())
            {
                // the .NET FieldBuilder doesn't support this method
                // (since we dont' have a different FieldInfo object after baking, we will support it once we're baked)
                throw new NotSupportedException();
            }

            return type.Module.ConstantTable.GetRawConstantValue(type.Module, GetCurrentToken());
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            return (offset = this.offset) != -1;
        }

        public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
        {
            SetCustomAttribute(new CustomAttributeBuilder(con, binaryAttribute));
        }

        public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
        {
            switch (customBuilder.KnownCA)
            {
                case KnownCA.FieldOffsetAttribute:
                    SetOffset((int)customBuilder.DecodeBlob(Module.Assembly).GetConstructorArgument(0));
                    break;
                case KnownCA.MarshalAsAttribute:
                    FieldMarshal.SetMarshalAsAttribute(type.ModuleBuilder, pseudoToken, customBuilder);
                    attributes |= FieldAttributes.HasFieldMarshal;
                    break;
                case KnownCA.NonSerializedAttribute:
                    attributes |= FieldAttributes.NotSerialized;
                    break;
                case KnownCA.SpecialNameAttribute:
                    attributes |= FieldAttributes.SpecialName;
                    break;
                default:
                    type.ModuleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
                    break;
            }
        }

        public void SetOffset(int iOffset)
        {
            offset = iOffset;
        }

        public override FieldAttributes Attributes
        {
            get { return attributes; }
        }

        public override Type DeclaringType
        {
            get { return type.IsModulePseudoType ? null : type; }
        }

        public override string Name
        {
            get { return name; }
        }

        public override int MetadataToken
        {
            get { return pseudoToken; }
        }

        public override Module Module
        {
            get { return type.Module; }
        }

        public FieldToken GetToken()
        {
            return new FieldToken(pseudoToken);
        }

        internal void WriteFieldRecords()
        {
            if (offset > -1)
            {
                var rec = new FieldLayoutTable.Record();
                rec.Offset = offset;
                rec.Field = pseudoToken;
                type.ModuleBuilder.FieldLayoutTable.AddRecord(rec);
            }

            var buf = new ByteBuffer(5);
            signature.Write(type.ModuleBuilder, buf);

            type.ModuleBuilder.Metadata.AddFieldDefinition(
                (System.Reflection.FieldAttributes)attributes,
                type.ModuleBuilder.GetOrAddString(name),
                type.ModuleBuilder.GetOrAddBlob(buf.ToArray()));
        }

        internal void FixupToken(int token)
        {
            type.ModuleBuilder.RegisterTokenFixup(pseudoToken, token);
        }

        internal override FieldSignature FieldSignature
        {
            get { return signature; }
        }

        internal override int ImportTo(ModuleBuilder other)
        {
            return other.ImportMethodOrField(type, name, signature);
        }

        internal override int GetCurrentToken()
        {
            if (type.ModuleBuilder.IsSaved)
                return type.ModuleBuilder.ResolvePseudoToken(pseudoToken);
            else
                return pseudoToken;
        }

        internal override bool IsBaked
        {
            get { return type.IsBaked; }
        }

    }

}
