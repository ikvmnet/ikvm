/*
  Copyright (C) 2009-2015 Jeroen Frijters

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
using IKVM.Reflection.Emit;

namespace IKVM.Reflection
{

    /// <summary>
    /// Base class for types that are composed from a single element type, such as arrays, pointers and by-ref types.
    /// </summary>
    abstract class ElementHolderType : TypeInfo
    {

        /// <summary>
        /// The type of the object encompassed or referred to by this type.
        /// </summary>
        protected readonly Type elementType;

        /// <summary>
        /// The cached TypeSpec token of this type in the <see cref="ModuleBuilder"/> of the element type, or 0 if not yet imported.
        /// </summary>
        int _token;

        readonly CustomModifiers _mods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="elementType">The type of the object encompassed or referred to by this type.</param>
        /// <param name="mods">The custom modifiers applied to this type.</param>
        /// <param name="sigElementType">The signature element type (an <c>ELEMENT_TYPE_*</c> value) of this type.</param>
        protected ElementHolderType(Type elementType, CustomModifiers mods, byte sigElementType) :
            base(sigElementType)
        {
            this.elementType = elementType;
            _mods = mods;
        }

        /// <summary>
        /// Determines whether <paramref name="other"/> has the same element type and custom modifiers as this instance.
        /// </summary>
        /// <param name="other">The type to compare with, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> is not <see langword="null"/> and has an equal element type and equal custom modifiers; otherwise, <see langword="false"/>.</returns>
        protected bool EqualsHelper(ElementHolderType other)
        {
            return other != null
                && other.elementType.Equals(elementType)
                && other._mods.Equals(_mods);
        }

        /// <inheritdoc />
        public override CustomModifiers __GetCustomModifiers() => _mods;

        /// <inheritdoc />
        public sealed override string Name => elementType.Name + GetSuffix();

        /// <inheritdoc />
        public sealed override string Namespace => elementType.Namespace;

        /// <inheritdoc />
        public sealed override string FullName => elementType.FullName + GetSuffix();

        /// <inheritdoc />
        public sealed override string ToString() => elementType.ToString() + GetSuffix();

        /// <inheritdoc />
        public sealed override Type GetElementType() => elementType;

        /// <inheritdoc />
        public sealed override Module Module => elementType.Module;

        /// <summary>
        /// Gets the TypeSpec token for this type in the <see cref="ModuleBuilder"/> of the element type, importing the type on first use.
        /// </summary>
        /// <returns>The metadata token of this type.</returns>
        internal sealed override int GetModuleBuilderToken()
        {
            if (_token == 0)
                _token = ((ModuleBuilder)elementType.Module).ImportType(this);

            return _token;
        }

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters
        {
            get
            {
                var type = elementType;
                while (type.HasElementType)
                    type = type.GetElementType();

                return type.ContainsGenericParameters;
            }
        }

        /// <inheritdoc />
        protected sealed override bool ContainsMissingTypeImpl
        {
            get
            {
                var type = elementType;
                while (type.HasElementType)
                    type = type.GetElementType();

                return type.__ContainsMissingType || _mods.ContainsMissingType;
            }
        }

        /// <inheritdoc />
        protected sealed override bool IsValueTypeImpl => false;

        /// <summary>
        /// Substitutes the generic parameters of the element type and custom modifiers using <paramref name="binder"/>.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>This instance if nothing changed; otherwise, a new type of the same kind wrapping the bound element type.</returns>
        internal sealed override Type BindTypeParameters(IGenericBinder binder)
        {
            var type = elementType.BindTypeParameters(binder);
            var mods = _mods.Bind(binder);
            if (ReferenceEquals(type, elementType) && mods.Equals(_mods))
                return this;

            return Wrap(type, mods);
        }

        /// <inheritdoc />
        internal override void CheckBaked() => elementType.CheckBaked();

        /// <inheritdoc />
        internal sealed override Universe Universe => elementType.Universe;

        /// <inheritdoc />
        internal sealed override bool IsBaked => elementType.IsBaked;

        /// <summary>
        /// Gets the current metadata token of this type, which is always 0 because element holder types have no token of their own.
        /// </summary>
        /// <returns>Always 0.</returns>
        internal sealed override int GetCurrentToken() =>
            // we don't have a token, so we return 0 (which is never a valid token)
            0;

        /// <summary>
        /// Gets the suffix that is appended to the element type name to form the name of this type, such as <c>[]</c>, <c>*</c> or <c>&amp;</c>.
        /// </summary>
        /// <returns>The type name suffix.</returns>
        internal abstract string GetSuffix();

        /// <summary>
        /// Creates a type of the same kind as this instance around a different element type.
        /// </summary>
        /// <param name="type">The new element type.</param>
        /// <param name="mods">The custom modifiers of the new type.</param>
        /// <returns>The constructed type.</returns>
        protected abstract Type Wrap(Type type, CustomModifiers mods);

    }

}
