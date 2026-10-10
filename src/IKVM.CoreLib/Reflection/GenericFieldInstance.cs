/*
  Copyright (C) 2009, 2010 Jeroen Frijters

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
namespace IKVM.Reflection
{

    /// <summary>
    /// Represents a field of a constructed generic type, wrapping the field of the generic type definition.
    /// </summary>
    sealed class GenericFieldInstance : FieldInfo
    {

        readonly Type _declaringType;
        readonly FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType">The constructed generic type that declares the field.</param>
        /// <param name="field">The field of the generic type definition.</param>
        internal GenericFieldInstance(Type declaringType, FieldInfo field)
        {
            _declaringType = declaringType;
            _field = field;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var other = obj as GenericFieldInstance;
            return other != null && other._declaringType.Equals(_declaringType) && other._field.Equals(_field);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() * 3 ^ _field.GetHashCode();

        /// <inheritdoc />
        public override FieldAttributes Attributes => _field.Attributes;

        /// <inheritdoc />
        public override string Name => _field.Name;

        /// <inheritdoc />
        public override Type DeclaringType => _declaringType;

        /// <inheritdoc />
        public override Module Module => _declaringType.Module;

        /// <inheritdoc />
        public override int MetadataToken => _field.MetadataToken;

        /// <inheritdoc />
        public override object GetRawConstantValue() => _field.GetRawConstantValue();

        /// <inheritdoc />
        public override bool __TryGetFieldOffset(out int offset) => _field.__TryGetFieldOffset(out offset);

        /// <summary>
        /// Gets the signature of the field, with the generic type parameters replaced by the type arguments of the declaring type.
        /// </summary>
        internal override FieldSignature FieldSignature => _field.FieldSignature.ExpandTypeParameters(_declaringType);

        /// <summary>
        /// Imports the field into <paramref name="module"/> as a MemberRef on the constructed declaring type.
        /// </summary>
        /// <param name="module">The module to import the field into.</param>
        /// <returns>The metadata token of the field reference in <paramref name="module"/>.</returns>
        internal override int ImportTo(Emit.ModuleBuilder module) => module.ImportMethodOrField(_declaringType, _field.Name, _field.FieldSignature);

        /// <inheritdoc />
        internal override FieldInfo BindTypeParameters(Type type) => new GenericFieldInstance(_declaringType.BindTypeParameters(type), _field);

        /// <inheritdoc />
        internal override int GetCurrentToken() => _field.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => _field.IsBaked;

    }

}
