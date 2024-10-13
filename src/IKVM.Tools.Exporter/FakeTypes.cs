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

using IKVM.CoreLib.Symbols;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    class FakeTypes
    {

        readonly RuntimeContext context;
        readonly ITypeSymbol genericType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public FakeTypes(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            genericType = context.Resolver.ResolveRuntimeType("IKVM.Runtime.ValueObject`1");
        }

        internal ITypeSymbol GetAttributeType(ITypeSymbol type)
        {
            return genericType.MakeGenericType(type);
        }

        internal ITypeSymbol GetAttributeReturnValueType(ITypeSymbol type)
        {
            return genericType.MakeGenericType(type);
        }

        internal ITypeSymbol GetAttributeMultipleType(ITypeSymbol type)
        {
            return genericType.MakeGenericType(type);
        }

        internal ITypeSymbol GetDelegateType(ITypeSymbol type)
        {
            return genericType.MakeGenericType(type);
        }

        internal ITypeSymbol GetEnumType(ITypeSymbol type)
        {
            return genericType.MakeGenericType(type);
        }

    }

}
