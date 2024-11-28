/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Manages instances of <see cref="RuntimeManagedJavaType"/>.
    /// </summary>
    class RuntimeManagedJavaTypeFactory
    {

        readonly RuntimeContext context;
        readonly ConditionalWeakTable<TypeSymbol, RuntimeJavaType> cache = new ConditionalWeakTable<TypeSymbol, RuntimeJavaType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public RuntimeManagedJavaTypeFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the <see cref="RuntimeJavaType"/> associated with the specified managed type, or creates one on demand.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RuntimeJavaType GetJavaTypeFromManagedType(TypeSymbol type)
        {
            return cache.GetValue(type, _ => context.AssemblyClassLoaderFactory.FromAssembly(_.Assembly).GetJavaTypeFromAssemblyType(_));
        }

        /// <summary>
        /// Gets the <see cref="RuntimeJavaType"/> associated with the base type of the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public RuntimeJavaType GetBaseJavaType(TypeSymbol type)
        {
            // interfaces have no base type
            if (type.IsInterface)
                return null;

            // remapped types extend their alter ego (e.g. cli.System.Object must appear to be derived from java.lang.Object)
            if (context.ClassLoaderFactory.IsRemappedType(type))
            {
                // except when they're sealed.
                if (type.IsSealed)
                    return context.JavaBase.TypeOfJavaLangObject;

                return context.ClassLoaderFactory.GetJavaTypeFromType(type);
            }

            // if base type is remapped, return java type of remapped base type
            if (context.ClassLoaderFactory.IsRemappedType(type.BaseType))
                return GetJavaTypeFromManagedType(type.BaseType);

            // return java type of base type
            return context.ClassLoaderFactory.GetJavaTypeFromType(type.BaseType);
        }

    }

}
