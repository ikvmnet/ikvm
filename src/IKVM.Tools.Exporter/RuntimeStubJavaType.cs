/*
  Copyright (C) 2002-2013 Jeroen Frijters

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

using IKVM.Attributes;

using Type = IKVM.Reflection.Type;

namespace IKVM.Runtime
{

    /// <summary>
    /// Defines a runtime java type with no implementation.
    /// </summary>
    sealed class RuntimeStubJavaType : RuntimeJavaType
    {

        readonly bool remapped;
        readonly RuntimeJavaType baseWrapper;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="modifiers"></param>
        /// <param name="name"></param>
        /// <param name="baseWrapper"></param>
        /// <param name="remapped"></param>
        internal RuntimeStubJavaType(RuntimeContext context, Modifiers modifiers, string name, RuntimeJavaType baseWrapper, bool remapped) :
            base(context, TypeFlags.None, modifiers, name)
        {
            this.remapped = remapped;
            this.baseWrapper = baseWrapper;
        }

        internal override RuntimeJavaType BaseTypeWrapper
        {
            get { return baseWrapper; }
        }

        internal override RuntimeClassLoader ClassLoader => Context.ClassLoaderFactory.GetBootstrapClassLoader();

        internal override Type TypeAsTBD
        {
            get { throw new NotSupportedException(); }
        }

        internal override bool IsRemapped
        {
            get { return remapped; }
        }

    }

}
