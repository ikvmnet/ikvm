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

    /// <summary>
    /// Defines and represents a field.
    /// </summary>
    sealed class FieldBuilder : FieldInfo
    {

        readonly TypeBuilder _type;
        readonly string _name;
        readonly int _pseudoToken;
        FieldAttributes _attributes;
        readonly FieldSignature _signature;
        int _offset = -1;

        readonly BlobHandle _signatureBlobHandle;

        /// <summary>
        /// Initializes a new instance. Allocates a pseudo token for the field and adds its signature to the blob heap.
        /// </summary>
        /// <param name="type">The type that declares the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="customModifiers">The custom modifiers of the field type.</param>
        /// <param name="attributes">The attributes of the field.</param>
        internal FieldBuilder(TypeBuilder type, string name, Type fieldType, CustomModifiers customModifiers, FieldAttributes attributes)
        {
            _type = type;
            _name = name;
            _attributes = attributes;
            _pseudoToken = type.ModuleBuilder.AllocPseudoToken();
            _signature = FieldSignature.Create(fieldType, customModifiers);

            // create signature blob
            var buf = new BlobBuilder(5);
            _signature.Write(type.ModuleBuilder, buf);
            _signatureBlobHandle = type.ModuleBuilder.GetOrAddBlob(buf);
        }

        /// <summary>
        /// Sets the default value of this field.
        /// </summary>
        /// <param name="defaultValue">The new default value for this field.</param>
        public void SetConstant(object defaultValue)
        {
            _attributes |= FieldAttributes.HasDefault;
            _type.ModuleBuilder.AddConstant(_pseudoToken, defaultValue);
        }

        /// <summary>
        /// Returns a literal value associated with the field by a compiler. Unlike the .NET FieldBuilder, this is
        /// supported once the declaring type has been created.
        /// </summary>
        /// <returns>The literal value associated with the field.</returns>
        /// <exception cref="NotSupportedException">The declaring type has not been created yet.</exception>
        public override object GetRawConstantValue()
        {
            if (!_type.IsCreated())
            {
                // the .NET FieldBuilder doesn't support this method (since we don't have a different FieldInfo object
                // after baking, we will support it once we're baked)
                throw new NotSupportedException();
            }

            return _type.ModuleBuilder.GetConstant(GetCurrentToken());
        }

        /// <summary>
        /// Gets the explicit offset of the field set with <see cref="SetOffset"/> or a FieldOffsetAttribute.
        /// </summary>
        /// <param name="offset">The offset of the field, or -1 if none was set.</param>
        /// <returns><see langword="true"/> if an offset was set; otherwise <see langword="false"/>.</returns>
        public override bool __TryGetFieldOffset(out int offset) => (offset = _offset) != -1;

        /// <summary>
        /// Sets a custom attribute using a custom attribute builder. FieldOffset, MarshalAs, NonSerialized and
        /// SpecialName are pseudo custom attributes and are stored as field metadata instead of as custom attributes.
        /// </summary>
        /// <param name="customBuilder">An instance of a helper class to define the custom attribute.</param>
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

        /// <summary>
        /// Specifies the field layout.
        /// </summary>
        /// <param name="iOffset">The offset of the field within the type containing this field.</param>
        public void SetOffset(int iOffset) => _offset = iOffset;

        /// <summary>
        /// Gets the attributes of this field.
        /// </summary>
        public override FieldAttributes Attributes => _attributes;

        /// <summary>
        /// Gets a reference to the <see cref="Type"/> object for the type in which this field is declared, or
        /// <see langword="null"/> for a global field declared on the module's &lt;Module&gt; type.
        /// </summary>
        public override Type DeclaringType => _type.IsModulePseudoType ? null : _type;

        /// <summary>
        /// Gets the name of this field.
        /// </summary>
        public override string Name => _name;

        /// <summary>
        /// Gets the pseudo token of the field. The real token is assigned when the module is saved.
        /// </summary>
        public override int MetadataToken => _pseudoToken;

        /// <summary>
        /// Gets the module in which the type that contains this field is being defined.
        /// </summary>
        public override Module Module => _type.Module;

        /// <summary>
        /// Adds the Field table row for this field to the module's metadata.
        /// </summary>
        internal void WriteFieldRecords()
        {
            _type.ModuleBuilder.Metadata.AddFieldDefinition(
                (System.Reflection.FieldAttributes)_attributes,
                _type.ModuleBuilder.GetOrAddString(_name),
                _signatureBlobHandle);
        }

        /// <summary>
        /// Records the real token assigned to the field, so that references to its pseudo token can be fixed up.
        /// </summary>
        /// <param name="token">The real FieldDef token.</param>
        internal void FixupToken(int token) => _type.ModuleBuilder.RegisterTokenFixup(_pseudoToken, token);

        /// <inheritdoc />
        internal override FieldSignature FieldSignature => _signature;

        /// <summary>
        /// Imports the field into another module as a MemberRef.
        /// </summary>
        /// <param name="other">The module to import the field into.</param>
        /// <returns>The MemberRef token.</returns>
        internal override int ImportTo(ModuleBuilder other) => other.ImportMethodOrField(_type, _name, _signature);

        /// <summary>
        /// Gets the real token of the field once the module has been saved, or its pseudo token before then.
        /// </summary>
        /// <returns>The current token of the field.</returns>
        internal override int GetCurrentToken()
        {
            if (_type.ModuleBuilder.IsSaved)
                return _type.ModuleBuilder.ResolvePseudoToken(_pseudoToken);
            else
                return _pseudoToken;
        }

        /// <summary>
        /// Gets whether the field is complete, which is the case once its declaring type has been created.
        /// </summary>
        internal override bool IsBaked => _type.IsBaked;

    }

}
