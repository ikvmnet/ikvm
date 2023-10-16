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

using IKVM.Reflection;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Holds the static compiler information.
    /// </summary>
    class StaticCompiler
    {

        readonly Universe universe;
        readonly AssemblyResolver resolver;
        readonly Assembly runtimeAssembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="universe"></param>
        /// <param name="resolver"></param>
        /// <param name="runtimeAssembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public StaticCompiler(Universe universe, AssemblyResolver resolver, Assembly runtimeAssembly)
        {
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            this.runtimeAssembly = runtimeAssembly ?? throw new ArgumentNullException(nameof(runtimeAssembly));
        }

        /// <summary>
        /// Gets the universe.
        /// </summary>
        internal Universe Universe => universe;

        internal Type GetRuntimeType(string typeName)
        {
            return runtimeAssembly.GetType(typeName, true);
        }

        internal Assembly LoadFile(string fileName)
        {
            return resolver.LoadFile(fileName);
        }

        internal Assembly Load(string name)
        {
            return universe.Load(name);
        }

    }

}