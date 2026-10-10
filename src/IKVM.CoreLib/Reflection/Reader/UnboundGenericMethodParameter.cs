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
using System.Collections.Generic;

namespace IKVM.Reflection.Reader
{

    /// <summary>
    /// Represents a generic method parameter (<c>!!n</c>) identified only by its position, not tied to any particular
    /// method. <see cref="MethodSignature"/> uses it to replace a method's own type parameters so that signatures of
    /// different generic methods can be compared. Only the position is meaningful; most other members throw
    /// <see cref="InvalidOperationException"/>.
    /// </summary>
    sealed class UnboundGenericMethodParameter : TypeParameterType
    {

        /// <summary>
        /// The placeholder module that unbound generic method parameters report as their <see cref="MemberInfo.Module"/>. It
        /// owns a private <see cref="Universe"/> used to canonicalize the parameter instances, and every other member
        /// throws <see cref="InvalidOperationException"/>.
        /// </summary>
        sealed class DummyModule : NonPEModule
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            internal DummyModule() :
                base(new Universe())
            {
            }

            /// <summary>
            /// Creates the exception thrown by members the module does not support.
            /// </summary>
            /// <returns>A new <see cref="InvalidOperationException"/>.</returns>
            protected override Exception NotSupportedException() => new InvalidOperationException();

            /// <summary>
            /// Creates the exception thrown when an argument is out of range.
            /// </summary>
            /// <returns>A new <see cref="InvalidOperationException"/>.</returns>
            protected override Exception ArgumentOutOfRangeException() => new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="obj">The object to compare with.</param>
            /// <returns>Does not return.</returns>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override bool Equals(object obj) => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <returns>Does not return.</returns>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override int GetHashCode() => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <returns>Does not return.</returns>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override string ToString() => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override int MDStreamVersion => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override Assembly Assembly => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="typeName">The name of the type.</param>
            /// <returns>Does not return.</returns>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            internal override Type FindType(TypeName typeName) => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="lowerCaseName">The lower-cased name of the type.</param>
            /// <returns>Does not return.</returns>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            internal override Type FindTypeIgnoreCase(TypeName lowerCaseName) => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <param name="list">The list to add the types to.</param>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            internal override void GetTypesImpl(List<Type> list) => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override string FullyQualifiedName => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override string Name => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override Guid ModuleVersionId => throw new InvalidOperationException();

            /// <summary>
            /// Not supported.
            /// </summary>
            /// <exception cref="InvalidOperationException">Always thrown.</exception>
            public override string ScopeName => throw new InvalidOperationException();

        }

        static readonly DummyModule s_module = new();

        readonly int _position;

        /// <summary>
        /// Gets the canonical unbound generic method parameter at the specified position.
        /// </summary>
        /// <param name="position">The zero-based position of the generic method parameter.</param>
        /// <returns>The shared instance that represents <c>!!<paramref name="position"/></c>.</returns>
        internal static Type Make(int position) => s_module.Universe.CanonicalizeType(new UnboundGenericMethodParameter(position));

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="position">The zero-based position of the generic method parameter.</param>
        UnboundGenericMethodParameter(int position) :
            base(Signature.ELEMENT_TYPE_MVAR)
        {
            _position = position;
        }

        /// <summary>
        /// Determines whether the specified object is an unbound generic method parameter at the same position.
        /// </summary>
        /// <param name="obj">The object to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is an <see cref="UnboundGenericMethodParameter"/> with the same position; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            var other = obj as UnboundGenericMethodParameter;
            return other != null && other._position == _position;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _position;

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override string Namespace => throw new InvalidOperationException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override string Name => throw new InvalidOperationException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override int MetadataToken => throw new InvalidOperationException();

        /// <summary>
        /// Gets the placeholder module shared by all unbound generic method parameters.
        /// </summary>
        public override Module Module => s_module;

        /// <inheritdoc />
        public override int GenericParameterPosition => _position;

        /// <summary>
        /// Gets <see langword="null"/>, because a generic method parameter is not declared by a type.
        /// </summary>
        public override Type DeclaringType => null;

        /// <summary>
        /// Not supported, because the parameter is not bound to a method.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override MethodBase DeclaringMethod => throw new InvalidOperationException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <returns>Does not return.</returns>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override Type[] GetGenericParameterConstraints() => throw new InvalidOperationException();

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        public override GenericParameterAttributes GenericParameterAttributes => throw new InvalidOperationException();

        /// <summary>
        /// Binds this parameter by asking <paramref name="binder"/> for the generic method argument at its position.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The type the binder substitutes for this parameter.</returns>
        internal override Type BindTypeParameters(IGenericBinder binder) => binder.BindMethodParameter(this);

        /// <summary>
        /// Not supported.
        /// </summary>
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        internal override bool IsBaked => throw new InvalidOperationException();

    }

}
