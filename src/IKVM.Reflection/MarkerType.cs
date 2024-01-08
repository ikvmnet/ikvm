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
    sealed class MarkerType : Type
    {

        // used by CustomModifiers and SignatureHelper
        internal static readonly Type ModOpt = new MarkerType(Signature.ELEMENT_TYPE_CMOD_OPT);
        internal static readonly Type ModReq = new MarkerType(Signature.ELEMENT_TYPE_CMOD_REQD);
        // used by SignatureHelper
        internal static readonly Type Sentinel = new MarkerType(Signature.SENTINEL);
        internal static readonly Type Pinned = new MarkerType(Signature.ELEMENT_TYPE_PINNED);
        // used by ModuleReader.LazyForwardedType and TypeSpec resolution
        internal static readonly Type LazyResolveInProgress = new MarkerType(0xFF);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sigElementType"></param>
        MarkerType(byte sigElementType) :
            base(sigElementType)
        {

        }

        public override Type BaseType
        {
            get { throw new InvalidOperationException(); }
        }

        public override TypeAttributes Attributes
        {
            get { throw new InvalidOperationException(); }
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

        internal override bool IsBaked
        {
            get { throw new InvalidOperationException(); }
        }

        public override bool __IsMissing
        {
            get { return false; }
        }

        protected override bool IsValueTypeImpl
        {
            get { throw new InvalidOperationException(); }
        }

    }

}
