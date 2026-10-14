/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{

    /// <summary>
    /// Wraps a <see cref="FieldInfo"/> obtained through a type other than its declaring type. Every member forwards to
    /// the wrapped field; the reflected type only takes part in equality, so that the same field obtained through
    /// different types compares unequal, as it does in System.Reflection.
    /// </summary>
    sealed class FieldInfoWithReflectedType : FieldInfo
    {

        readonly Type _reflectedType;
        readonly FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType">The type through which the field was obtained. Must not be the field's declaring type.</param>
        /// <param name="field">The field to wrap.</param>
        internal FieldInfoWithReflectedType(Type reflectedType, FieldInfo field)
        {
            Debug.Assert(reflectedType != field.DeclaringType);
            _reflectedType = reflectedType;
            _field = field;
        }

        /// <inheritdoc />
        public override FieldAttributes Attributes => _field.Attributes;

        /// <inheritdoc />
        public override bool __TryGetFieldOffset(out int offset) => _field.__TryGetFieldOffset(out offset);

        /// <inheritdoc />
        public override object GetRawConstantValue() => _field.GetRawConstantValue();

        /// <inheritdoc />
        internal override FieldSignature FieldSignature => _field.FieldSignature;

        /// <inheritdoc />
        internal override int ImportTo(Emit.ModuleBuilder module) => _field.ImportTo(module);

        /// <inheritdoc />
        internal override FieldInfo BindTypeParameters(Type type) => _field.BindTypeParameters(type);

        /// <inheritdoc />
        public override bool __IsMissing => _field.__IsMissing;

        /// <inheritdoc />
        public override Type DeclaringType => _field.DeclaringType;

        /// <summary>
        /// Determines whether the specified object is a <see cref="FieldInfoWithReflectedType"/> that wraps the same
        /// field obtained through the same reflected type.
        /// </summary>
        /// <param name="obj">The object to compare with this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> wraps the same field and reflected type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as FieldInfoWithReflectedType;
            return other != null
                && other._reflectedType == _reflectedType
                && other._field == _field;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _reflectedType.GetHashCode() ^ _field.GetHashCode();

        /// <inheritdoc />
        public override int MetadataToken => _field.MetadataToken;

        /// <inheritdoc />
        public override Module Module => _field.Module;

        /// <inheritdoc />
        public override string Name => _field.Name;

        /// <inheritdoc />
        public override string ToString() => _field.ToString();

        /// <inheritdoc />
        internal override int GetCurrentToken() => _field.GetCurrentToken();

        /// <inheritdoc />
        internal override bool IsBaked => _field.IsBaked;

    }

}
