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
    /// Represents a generic parameter of a missing type or method, whose definition, and therefore name and
    /// constraints, is not available. Only its owner and position are known.
    /// </summary>
    sealed class MissingTypeParameter : IKVM.Reflection.Reader.TypeParameterType
    {

        readonly MemberInfo _owner;
        readonly int _index;

        /// <summary>
        /// Initializes a new instance for a generic parameter of a type.
        /// </summary>
        /// <param name="owner">The type that declares the generic parameter.</param>
        /// <param name="index">The position of the parameter in the type's generic parameter list.</param>
        internal MissingTypeParameter(Type owner, int index) :
            this(owner, index, Signature.ELEMENT_TYPE_VAR)
        {
        }

        /// <summary>
        /// Initializes a new instance for a generic parameter of a method.
        /// </summary>
        /// <param name="owner">The method that declares the generic parameter.</param>
        /// <param name="index">The position of the parameter in the method's generic parameter list.</param>
        internal MissingTypeParameter(MethodInfo owner, int index) :
            this(owner, index, Signature.ELEMENT_TYPE_MVAR)
        {
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner">The type or method that declares the generic parameter.</param>
        /// <param name="index">The position of the parameter in the owner's generic parameter list.</param>
        /// <param name="sigElementType">The signature element type: <c>ELEMENT_TYPE_VAR</c> for a type parameter or <c>ELEMENT_TYPE_MVAR</c> for a method parameter.</param>
        MissingTypeParameter(MemberInfo owner, int index, byte sigElementType) :
            base(sigElementType)
        {
            _owner = owner;
            _index = index;
        }

        /// <summary>
        /// Gets the module of the type or method that declares the parameter.
        /// </summary>
        public override Module Module => _owner.Module;

        /// <summary>
        /// Gets the name of the parameter, which is always <see langword="null"/> because it is not known.
        /// </summary>
        public override string Name => null;

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that
        /// declared the parameter.
        /// </summary>
        public override int GenericParameterPosition => _index;

        /// <summary>
        /// Gets the generic method that declares the parameter, or <see langword="null"/> for a parameter of a generic
        /// type.
        /// </summary>
        public override MethodBase DeclaringMethod => _owner as MethodBase;

        /// <summary>
        /// Gets the generic type that declares the parameter, or <see langword="null"/> for a parameter of a generic
        /// method.
        /// </summary>
        public override Type DeclaringType => _owner as Type;

        /// <summary>
        /// Substitutes the parameter with the matching type or method argument supplied by <paramref name="binder"/>.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The bound type argument.</returns>
        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            if (_owner is MethodBase)
                return binder.BindMethodParameter(this);
            else
                return binder.BindTypeParameter(this);
        }

        /// <summary>
        /// Gets a value indicating whether the owner of the parameter is complete.
        /// </summary>
        internal override bool IsBaked => _owner.IsBaked;

    }

}
