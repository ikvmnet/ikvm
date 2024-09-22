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
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class ImportRuntimeSymbolResolver : IRuntimeSymbolResolver
    {

        readonly StaticCompiler compiler;
        readonly IkvmReflectionSymbolContext context;

        /// <inheritdoc />
        public ISymbolContext Symbols => context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        public ImportRuntimeSymbolResolver(StaticCompiler compiler)
        {
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            this.context = new IkvmReflectionSymbolContext(compiler.Universe);
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveBaseAssembly()
        {
            return compiler.baseAssembly;
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveAssembly(string assemblyName)
        {
            return compiler.Universe.Load(assemblyName) is { } a ? context.GetOrCreateAssemblySymbol(a) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return context.GetOrCreateTypeSymbol(t);

            return null;
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveRuntimeType(string typeName)
        {
            return compiler.GetRuntimeType(typeName);
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveAssembly(Assembly assembly)
        {
            return context.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder ResolveAssembly(AssemblyBuilder assembly)
        {
            return context.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IModuleSymbol ResolveModule(Module module)
        {
            return context.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder ResolveModule(ModuleBuilder module)
        {
            return context.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(Type type)
        {
            return context.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IMemberSymbol ResolveMember(MemberInfo memberInfo)
        {
            return context.GetOrCreateMemberSymbol(memberInfo);
        }

        /// <inheritdoc />
        public IMethodBaseSymbol ResolveMethodBase(MethodBase type)
        {
            return context.GetOrCreateMethodBaseSymbol(type);
        }

        /// <inheritdoc />
        public IConstructorSymbol ResolveConstructor(ConstructorInfo ctor)
        {
            return context.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IMethodSymbol ResolveMethod(MethodInfo method)
        {
            return context.GetOrCreateMethodSymbol(method);
        }

    }

}