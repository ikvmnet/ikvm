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

    sealed class MissingTypeParameter : IKVM.Reflection.Reader.TypeParameterType
    {

        readonly MemberInfo owner;
        readonly int index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index"></param>
        internal MissingTypeParameter(Type owner, int index) :
            this(owner, index, Signature.ELEMENT_TYPE_VAR)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index"></param>
        internal MissingTypeParameter(MethodInfo owner, int index) :
            this(owner, index, Signature.ELEMENT_TYPE_MVAR)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="index"></param>
        /// <param name="sigElementType"></param>
        MissingTypeParameter(MemberInfo owner, int index, byte sigElementType) :
            base(sigElementType)
        {
            this.owner = owner;
            this.index = index;
        }

        public override Module Module
        {
            get { return owner.Module; }
        }

        public override string Name
        {
            get { return null; }
        }

        public override int GenericParameterPosition
        {
            get { return index; }
        }

        public override MethodBase DeclaringMethod
        {
            get { return owner as MethodBase; }
        }

        public override Type DeclaringType
        {
            get { return owner as Type; }
        }

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            if (owner is MethodBase)
                return binder.BindMethodParameter(this);
            else
                return binder.BindTypeParameter(this);
        }

        internal override bool IsBaked
        {
            get { return owner.IsBaked; }
        }

    }

}
