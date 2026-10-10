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

    /// <summary>
    /// A placeholder type that is never a real type. Instances are used as markers inside type lists, for example to
    /// separate optional from required custom modifiers, and as a sentinel while a type is being resolved. Every
    /// member that would describe a real type throws <see cref="InvalidOperationException"/>.
    /// </summary>
    sealed class MarkerType : Type
    {

        /// <summary>
        /// Marks the start of optional custom modifiers in a modifier list. Used by CustomModifiers and SignatureHelper.
        /// </summary>
        internal static readonly Type ModOpt = new MarkerType(Signature.ELEMENT_TYPE_CMOD_OPT);

        /// <summary>
        /// Marks the start of required custom modifiers in a modifier list. Used by CustomModifiers and SignatureHelper.
        /// </summary>
        internal static readonly Type ModReq = new MarkerType(Signature.ELEMENT_TYPE_CMOD_REQD);

        /// <summary>
        /// Marks the start of the variable arguments in a call site signature. Used by SignatureHelper.
        /// </summary>
        internal static readonly Type Sentinel = new MarkerType(Signature.SENTINEL);

        /// <summary>
        /// Marks a pinned local variable. Used by SignatureHelper.
        /// </summary>
        internal static readonly Type Pinned = new MarkerType(Signature.ELEMENT_TYPE_PINNED);

        /// <summary>
        /// Stored in place of a type while it is being resolved, to detect recursive resolution. Used by
        /// ModuleReader.LazyForwardedType and TypeSpec resolution.
        /// </summary>
        internal static readonly Type LazyResolveInProgress = new MarkerType(0xFF);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sigElementType">The signature element type the marker stands for.</param>
        MarkerType(byte sigElementType) :
            base(sigElementType)
        {

        }

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a marker has no base type.</exception>
        public override Type BaseType => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a marker has no attributes.</exception>
        public override TypeAttributes Attributes => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a marker has no name.</exception>
        public override string Name => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a marker has no name.</exception>
        public override string FullName => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown; a marker belongs to no module.</exception>
        public override Module Module => throw new InvalidOperationException();

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        internal override bool IsBaked => throw new InvalidOperationException();

        /// <summary>
        /// Gets whether the type is missing, which is never the case for a marker.
        /// </summary>
        public override bool __IsMissing => false;

        /// <inheritdoc />
        /// <exception cref="InvalidOperationException">Always thrown.</exception>
        protected override bool IsValueTypeImpl => throw new InvalidOperationException();

    }

}
