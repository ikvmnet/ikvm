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
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;

using IKVM.Reflection.Metadata;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection.Emit
{

    public sealed class FieldBuilder : FieldInfo
    {

        readonly TypeBuilder typeBuilder;
        readonly string name;
        readonly int pseudoToken;
        FieldAttributes attribs;
        readonly StringHandle nameIndex;
        readonly BlobHandle signature;
        readonly FieldSignature fieldSig;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="fieldType"></param>
        /// <param name="customModifiers"></param>
        /// <param name="attribs"></param>
        internal FieldBuilder(TypeBuilder type, string name, Type fieldType, CustomModifiers customModifiers, FieldAttributes attribs)
        {
            this.typeBuilder = type;
            this.name = name;
            this.pseudoToken = type.ModuleBuilder.AllocPseudoToken();
            this.nameIndex = type.ModuleBuilder.GetOrAddString(name);
            this.fieldSig = FieldSignature.Create(fieldType, customModifiers);
            var sig = new ByteBuffer(5);
            fieldSig.Write(typeBuilder.ModuleBuilder, sig);
            this.signature = typeBuilder.ModuleBuilder.GetOrAddBlob(sig.ToArray());
            this.attribs = attribs;
            this.typeBuilder.ModuleBuilder.FieldTable.AddVirtualRecord();
        }

        public void SetConstant(object defaultValue)
        {
            attribs |= FieldAttributes.HasDefault;
            typeBuilder.ModuleBuilder.AddConstant(pseudoToken, defaultValue);
        }

        public override object GetRawConstantValue()
        {
            if (!typeBuilder.IsCreated())
            {
                // the .NET FieldBuilder doesn't support this method
                // (since we dont' have a different FieldInfo object after baking, we will support it once we're baked)
                throw new NotSupportedException();
            }

            return typeBuilder.Module.ConstantTable.GetRawConstantValue(typeBuilder.Module, GetCurrentToken());
        }

        public void __SetDataAndRVA(byte[] data)
        {
            SetDataAndRvaImpl(data, typeBuilder.ModuleBuilder.initializedData, 0);
        }

        public void __SetReadOnlyDataAndRVA(byte[] data)
        {
            SetDataAndRvaImpl(data, typeBuilder.ModuleBuilder.methodBodies, unchecked((int)0x80000000));
        }

        void SetDataAndRvaImpl(byte[] data, ByteBuffer bb, int readonlyMarker)
        {
            attribs |= FieldAttributes.HasFieldRVA;
            FieldRVATable.Record rec = new FieldRVATable.Record();
            bb.Align(8);
            rec.RVA = bb.Position + readonlyMarker;
            rec.Field = pseudoToken;
            typeBuilder.ModuleBuilder.FieldRVATable.AddRecord(rec);
            bb.Write(data);
        }

        public override void __GetDataFromRVA(byte[] data, int offset, int length)
        {
            throw new NotImplementedException();
        }

        public override int __FieldRVA
        {
            get { throw new NotImplementedException(); }
        }

        public override bool __TryGetFieldOffset(out int offset)
        {
            int pseudoTokenOrIndex = pseudoToken;
            if (typeBuilder.ModuleBuilder.IsSaved)
                pseudoTokenOrIndex = typeBuilder.ModuleBuilder.ResolvePseudoToken(pseudoToken) & 0xFFFFFF;

            foreach (int i in this.Module.FieldLayoutTable.Filter(pseudoTokenOrIndex))
            {
                offset = this.Module.FieldLayoutTable.records[i].Offset;
                return true;
            }

            offset = 0;
            return false;
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
                    SetOffset((int)customBuilder.DecodeBlob(this.Module.Assembly).GetConstructorArgument(0));
                    break;
                case KnownCA.MarshalAsAttribute:
                    FieldMarshal.SetMarshalAsAttribute(typeBuilder.ModuleBuilder, pseudoToken, customBuilder);
                    attribs |= FieldAttributes.HasFieldMarshal;
                    break;
                case KnownCA.NonSerializedAttribute:
                    attribs |= FieldAttributes.NotSerialized;
                    break;
                case KnownCA.SpecialNameAttribute:
                    attribs |= FieldAttributes.SpecialName;
                    break;
                default:
                    typeBuilder.ModuleBuilder.SetCustomAttribute(pseudoToken, customBuilder);
                    break;
            }
        }

        public void SetOffset(int iOffset)
        {
            FieldLayoutTable.Record rec = new FieldLayoutTable.Record();
            rec.Offset = iOffset;
            rec.Field = pseudoToken;
            typeBuilder.ModuleBuilder.FieldLayoutTable.AddRecord(rec);
        }

        public override FieldAttributes Attributes
        {
            get { return attribs; }
        }

        public override Type DeclaringType
        {
            get { return typeBuilder.IsModulePseudoType ? null : typeBuilder; }
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
            get { return typeBuilder.Module; }
        }

        public FieldToken GetToken()
        {
            return new FieldToken(pseudoToken);
        }

        internal void WriteFieldRecords(MetadataBuilder metadata)
        {
            metadata.AddFieldDefinition(
                (System.Reflection.FieldAttributes)attribs,
                nameIndex,
                signature);
        }

        internal void FixupToken(int token)
        {
            typeBuilder.ModuleBuilder.RegisterTokenFixup(pseudoToken, token);
        }

        internal override FieldSignature FieldSignature
        {
            get { return fieldSig; }
        }

        internal override int ImportTo(ModuleBuilder other)
        {
            return other.ImportMethodOrField(typeBuilder, name, fieldSig);
        }

        internal override int GetCurrentToken()
        {
            if (typeBuilder.ModuleBuilder.IsSaved)
            {
                return typeBuilder.ModuleBuilder.ResolvePseudoToken(pseudoToken);
            }
            else
            {
                return pseudoToken;
            }
        }

        internal override bool IsBaked
        {
            get { return typeBuilder.IsBaked; }
        }

    }

}
