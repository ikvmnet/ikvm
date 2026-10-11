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

    /// <summary>
    /// Represents a field defined in the FieldDef table of a module that was read from a file.
    /// </summary>
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
        /// <param name="module">The module that defines the field.</param>
        /// <param name="declaringType">The type that defines the field, which may be the module's &lt;Module&gt; pseudo type.</param>
        /// <param name="handle">The handle of the field's FieldDef row.</param>
        internal FieldDefImpl(ModuleReader module, TypeDefImpl declaringType, FieldDefinitionHandle handle)
        {
            _module = module;
            _declaringType = declaringType;
            _handle = handle;
            _definition = module.Metadata.GetFieldDefinition(handle);
        }

        /// <inheritdoc />
        public override FieldAttributes Attributes => (FieldAttributes)_definition.Attributes;

        /// <summary>
        /// Gets the class that declares this field, or <see langword="null"/> if the field is a global field defined on
        /// the module's &lt;Module&gt; pseudo type.
        /// </summary>
        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        /// <inheritdoc />
        public override string Name => _module.GetString(_definition.Name);

        /// <inheritdoc />
        public override string ToString() => FieldType.Name + " " + Name;

        /// <inheritdoc />
        public override Module Module => _module;

        /// <inheritdoc />
        public override int MetadataToken => MetadataTokens.GetToken(_handle);

        /// <inheritdoc />
        public override object GetRawConstantValue() => _module.GetConstantValue(_definition.GetDefaultValue());

        /// <inheritdoc />
        public override bool __TryGetFieldOffset(out int offset)
        {
            offset = _definition.GetOffset();
            if (offset != -1)
                return true;

            offset = 0;
            return false;
        }

        /// <summary>
        /// Gets the signature of the field, reading it from the metadata on first use.
        /// </summary>
        internal override FieldSignature FieldSignature =>
            _lazyFieldSig ??= FieldSignature.ReadSig(_module, _module.GetBlobReader(_definition.Signature), _declaringType);

        /// <inheritdoc />
        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(_declaringType, Name, FieldSignature);

        /// <inheritdoc />
        internal override int GetCurrentToken() => MetadataToken;

        /// <summary>
        /// Gets a value indicating whether the field is complete. A field read from a file is always complete.
        /// </summary>
        internal override bool IsBaked => true;

    }

}
