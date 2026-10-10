/*
  Copyright (C) 2011-2012 Jeroen Frijters

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
    /// A field that is referenced but could not be found, either because its declaring type is missing or because the
    /// type does not declare it and <see cref="Universe.MissingMemberResolution"/> is enabled. The field is described
    /// by its declaring type, name and signature. Members that need more than the signature forward to the real field
    /// if it can be found later; otherwise they throw <see cref="MissingMemberException"/>.
    /// </summary>
    sealed class MissingField : FieldInfo
    {

        readonly Type _declaringType;
        readonly string _name;
        readonly FieldSignature _signature;

        // the real field, once it has been found
        FieldInfo _forwarder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType">The type that should declare the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="signature">The signature of the field.</param>
        internal MissingField(Type declaringType, string name, FieldSignature signature)
        {
            _declaringType = declaringType;
            _name = name;
            _signature = signature;
        }

        /// <summary>
        /// Gets the real field.
        /// </summary>
        /// <exception cref="MissingMemberException">The field still cannot be found.</exception>
        FieldInfo Forwarder => TryGetForwarder() ?? throw new MissingMemberException(this);

        /// <summary>
        /// Looks up the real field in the declaring type, if the declaring type is not itself missing, and caches it.
        /// </summary>
        /// <returns>The real field, or <see langword="null"/> if it cannot be found.</returns>
        FieldInfo TryGetForwarder()
        {
            if (_forwarder == null && !_declaringType.__IsMissing)
                _forwarder = _declaringType.FindField(_name, _signature);

            return _forwarder;
        }

        /// <summary>
        /// Gets whether the field is still missing, that is, whether the real field cannot be found.
        /// </summary>
        public override bool __IsMissing => TryGetForwarder() == null;

        /// <inheritdoc />
        public override FieldAttributes Attributes => Forwarder.Attributes;

        /// <inheritdoc />
        public override bool __TryGetFieldOffset(out int offset) => Forwarder.__TryGetFieldOffset(out offset);

        /// <inheritdoc />
        public override object GetRawConstantValue() => Forwarder.GetRawConstantValue();

        /// <inheritdoc />
        internal override FieldSignature FieldSignature => _signature;

        /// <summary>
        /// Imports the field into a module being built: the real field if it can be found, otherwise a MemberRef made
        /// from the declaring type, name and signature.
        /// </summary>
        /// <param name="module">The module being built.</param>
        /// <returns>The token of the field in <paramref name="module"/>.</returns>
        internal override int ImportTo(IKVM.Reflection.Emit.ModuleBuilder module)
        {
            var field = TryGetForwarder();
            if (field != null)
                return field.ImportTo(module);

            return module.ImportMethodOrField(_declaringType, Name, FieldSignature);
        }

        /// <inheritdoc />
        public override string Name => _name;

        /// <summary>
        /// Gets the type that declares the field, or <see langword="null"/> if it is a global field declared by the
        /// module pseudo type.
        /// </summary>
        public override Type DeclaringType => _declaringType.IsModulePseudoType ? null : _declaringType;

        /// <inheritdoc />
        public override Module Module => _declaringType.Module;

        /// <summary>
        /// Returns this field as a member of the given constructed generic type: the bound real field if it can be
        /// found, otherwise a <see cref="GenericFieldInstance"/> over this missing field.
        /// </summary>
        /// <param name="type">The constructed type that supplies the type arguments.</param>
        /// <returns>The bound field.</returns>
        internal override FieldInfo BindTypeParameters(Type type)
        {
            var forwarder = TryGetForwarder();
            if (forwarder != null)
                return forwarder.BindTypeParameters(type);

            return new GenericFieldInstance(type, this);
        }

        /// <inheritdoc />
        public override int MetadataToken => Forwarder.MetadataToken;

        /// <summary>
        /// Determines whether the specified object is a missing field with the same declaring type, name and signature.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if the fields are equal; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as MissingField;
            return other != null
                && other._declaringType == _declaringType
                && other._name == _name
                && other._signature.Equals(_signature);
        }

        /// <inheritdoc />
        public override int GetHashCode() => _declaringType.GetHashCode() ^ _name.GetHashCode() ^ _signature.GetHashCode();

        /// <summary>
        /// Returns a string that represents the field, in the form <c>FieldTypeName Name</c>.
        /// </summary>
        /// <returns>A string that represents the current field.</returns>
        public override string ToString() => FieldType.Name + " " + Name;

        /// <inheritdoc />
        internal override int GetCurrentToken() => Forwarder.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => Forwarder.IsBaked;

    }

}
