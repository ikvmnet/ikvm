/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using IKVM.CoreLib.Symbols.IkvmReflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class ManagedResolver : ISymbolResolver
    {

        readonly StaticCompiler compiler;
        readonly IkvmReflectionSymbolContext context = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        public ManagedResolver(StaticCompiler compiler)
        {
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
        }

        public IAssemblySymbol ResolveBaseAssembly()
        {
            return compiler.baseAssembly != null ? context.GetOrCreateAssemblySymbol(compiler.baseAssembly) : null;
        }

        public IAssemblySymbol ResolveAssembly(string assemblyName)
        {
            return compiler.Universe.Load(assemblyName) is { } a ? context.GetOrCreateAssemblySymbol(a) : null;
        }

        public ITypeSymbol ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return context.GetOrCreateTypeSymbol(t);

            return null;
        }

        public ITypeSymbol ResolveRuntimeType(string typeName)
        {
            return compiler.GetRuntimeType(typeName) is { } t ? context.GetOrCreateTypeSymbol(t) : null;
        }

    }

}
