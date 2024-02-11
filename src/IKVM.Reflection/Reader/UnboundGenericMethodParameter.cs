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

    sealed class UnboundGenericMethodParameter : TypeParameterType
    {

        sealed class DummyModule : NonPEModule
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            internal DummyModule() :
                base(new Universe())
            {

            }

            protected override Exception NotSupportedException()
            {
                return new InvalidOperationException();
            }

            protected override Exception ArgumentOutOfRangeException()
            {
                return new InvalidOperationException();
            }

            public override bool Equals(object obj)
            {
                throw new InvalidOperationException();
            }

            public override int GetHashCode()
            {
                throw new InvalidOperationException();
            }

            public override string ToString()
            {
                throw new InvalidOperationException();
            }

            public override int MDStreamVersion
            {
                get { throw new InvalidOperationException(); }
            }

            public override Assembly Assembly
            {
                get { throw new InvalidOperationException(); }
            }

            internal override Type FindType(TypeName typeName)
            {
                throw new InvalidOperationException();
            }

            internal override Type FindTypeIgnoreCase(TypeName lowerCaseName)
            {
                throw new InvalidOperationException();
            }

            internal override void GetTypesImpl(List<Type> list)
            {
                throw new InvalidOperationException();
            }

            public override string FullyQualifiedName
            {
                get { throw new InvalidOperationException(); }
            }

            public override string Name
            {
                get { throw new InvalidOperationException(); }
            }

            public override Guid ModuleVersionId
            {
                get { throw new InvalidOperationException(); }
            }

            public override string ScopeName
            {
                get { throw new InvalidOperationException(); }
            }

        }

        static readonly DummyModule module = new DummyModule();

        readonly int position;

        internal static Type Make(int position)
        {
            return module.universe.CanonicalizeType(new UnboundGenericMethodParameter(position));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="position"></param>
        UnboundGenericMethodParameter(int position) :
            base(Signature.ELEMENT_TYPE_MVAR)
        {
            this.position = position;
        }

        public override bool Equals(object obj)
        {
            var other = obj as UnboundGenericMethodParameter;
            return other != null && other.position == position;
        }

        public override int GetHashCode()
        {
            return position;
        }

        public override string Namespace
        {
            get { throw new InvalidOperationException(); }
        }

        public override string Name
        {
            get { throw new InvalidOperationException(); }
        }

        public override int MetadataToken
        {
            get { throw new InvalidOperationException(); }
        }

        public override Module Module
        {
            get { return module; }
        }

        public override int GenericParameterPosition
        {
            get { return position; }
        }

        public override Type DeclaringType
        {
            get { return null; }
        }

        public override MethodBase DeclaringMethod
        {
            get { throw new InvalidOperationException(); }
        }

        public override Type[] GetGenericParameterConstraints()
        {
            throw new InvalidOperationException();
        }

        public override CustomModifiers[] __GetGenericParameterConstraintCustomModifiers()
        {
            throw new InvalidOperationException();
        }

        public override GenericParameterAttributes GenericParameterAttributes
        {
            get { throw new InvalidOperationException(); }
        }

        internal override Type BindTypeParameters(IGenericBinder binder)
        {
            return binder.BindMethodParameter(this);
        }

        internal override bool IsBaked
        {
            get { throw new InvalidOperationException(); }
        }

    }

}
