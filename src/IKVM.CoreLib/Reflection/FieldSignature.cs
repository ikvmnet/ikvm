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

using IKVM.Reflection.Emit;
using IKVM.Reflection.Reader;
using IKVM.Reflection.Writer;

namespace IKVM.Reflection
{

    /// <summary>
    /// The signature of a field: its type and the custom modifiers on it (ECMA-335 II.23.2.4).
    /// </summary>
    sealed class FieldSignature : Signature
    {

        readonly Type _fieldType;
        readonly CustomModifiers _mods;

        /// <summary>
        /// Creates a field signature.
        /// </summary>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="customModifiers">The custom modifiers on the field type.</param>
        /// <returns>The new signature.</returns>
        internal static FieldSignature Create(Type fieldType, CustomModifiers customModifiers) => new FieldSignature(fieldType, customModifiers);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="fieldType">The type of the field.</param>
        /// <param name="mods">The custom modifiers on the field type.</param>
        FieldSignature(Type fieldType, CustomModifiers mods)
        {
            _fieldType = fieldType;
            _mods = mods;
        }

        /// <summary>
        /// Determines whether the specified object is a field signature with the same type and custom modifiers.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if the signatures are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            return obj is FieldSignature other
                && other._fieldType.Equals(_fieldType)
                && other._mods.Equals(_mods);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _fieldType.GetHashCode() ^ _mods.GetHashCode();

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        internal Type FieldType => _fieldType;

        /// <summary>
        /// Gets the custom modifiers on the field type.
        /// </summary>
        /// <returns>The custom modifiers.</returns>
        internal CustomModifiers GetCustomModifiers() => _mods;

        /// <summary>
        /// Returns a copy of this signature with the type parameters in the field type and custom modifiers bound to
        /// the type arguments of the given type.
        /// </summary>
        /// <param name="declaringType">The constructed type that supplies the type arguments.</param>
        /// <returns>The bound signature.</returns>
        internal FieldSignature ExpandTypeParameters(Type declaringType)
        {
            return new FieldSignature(
                _fieldType.BindTypeParameters(declaringType),
                _mods.Bind(declaringType));
        }

        /// <summary>
        /// Reads a field signature from a blob.
        /// </summary>
        /// <param name="module">The module the signature belongs to.</param>
        /// <param name="br">The reader positioned at the start of the signature.</param>
        /// <param name="context">The generic context used to resolve type parameters.</param>
        /// <returns>The signature that was read.</returns>
        /// <exception cref="BadImageFormatException">The blob is not a field signature.</exception>
        internal static FieldSignature ReadSig(ModuleReader module, BlobReader br, IGenericContext context)
        {
            if (br.ReadByte() != FIELD)
                throw new BadImageFormatException();

            var mods = CustomModifiers.Read(module, ref br, context);
            var fieldType = ReadType(module, ref br, context);
            return new FieldSignature(fieldType, mods);
        }

        /// <inheritdoc />
        internal override void Write(ModuleBuilder module, BlobBuilder bb)
        {
            bb.WriteByte(FIELD);
            WriteCustomModifiers(module, bb, _mods);
            WriteType(module, bb, _fieldType);
        }

    }

}
