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
using System;

namespace IKVM.Reflection
{

    sealed class FunctionPointerType : TypeInfo
    {

        readonly Universe universe;
        readonly __StandAloneMethodSig sig;

        internal static Type Make(Universe universe, __StandAloneMethodSig sig)
        {
            return universe.CanonicalizeType(new FunctionPointerType(universe, sig));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="sig"></param>
        FunctionPointerType(Universe universe, __StandAloneMethodSig sig) :
            base(Signature.ELEMENT_TYPE_FNPTR)
        {
            this.universe = universe;
            this.sig = sig;
        }

        public override bool Equals(object obj)
        {
            var other = obj as FunctionPointerType;
            return other != null
                && other.universe == universe
                && other.sig.Equals(sig);
        }

        public override int GetHashCode()
        {
            return sig.GetHashCode();
        }

        public override __StandAloneMethodSig __MethodSignature
        {
            get { return sig; }
        }

        public override Type BaseType
        {
            get { return null; }
        }

        public override TypeAttributes Attributes
        {
            get { return 0; }
        }

        public override string Name
        {
            get { throw new InvalidOperationException(); }
        }

        public override string FullName
        {
            get { throw new InvalidOperationException(); }
        }

        public override Module Module
        {
            get { throw new InvalidOperationException(); }
        }

        internal override Universe Universe
        {
            get { return universe; }
        }

        public override string ToString()
        {
            return "<FunctionPtr>";
        }

        protected override bool ContainsMissingTypeImpl
        {
            get { return sig.ContainsMissingType; }
        }

        internal override bool IsBaked
        {
            get { return true; }
        }

        protected override bool IsValueTypeImpl
        {
            get { return false; }
        }

    }

}
