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
using System.IO;
using System.Threading;

using IKVM.CoreLib.Diagnostics;
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

        readonly IDiagnosticHandler diagnostics;
        readonly Universe universe;
        readonly IkvmReflectionSymbolContext symbols;
        readonly ImportOptions options;
        IAssemblySymbol runtimeAssembly;
        IAssemblySymbol baseAssembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="universe"></param>
        /// <param name="symbols"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportRuntimeSymbolResolver(IDiagnosticHandler diagnostics, Universe universe, IkvmReflectionSymbolContext symbols, ImportOptions options)
        {
            this.diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public ISymbolContext Symbols => symbols;

        /// <inheritdoc />
        public IAssemblySymbol ResolveCoreAssembly()
        {
            try
            {
                if (ImportAssembly(universe.CoreLib) is { } a)
                    return a;
            }
            catch (FileNotFoundException)
            {

            }

            throw new FatalCompilerErrorException(DiagnosticEvent.CoreClassesMissing());
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveRuntimeAssembly()
        {
            if (runtimeAssembly == null)
                Interlocked.CompareExchange(ref runtimeAssembly, LoadRuntimeAssembly(), null);

            return runtimeAssembly;
        }

        /// <summary>
        /// Attempts to load the runtime assembly.
        /// </summary>
        /// <exception cref="FatalCompilerErrorException"></exception>
        IAssemblySymbol LoadRuntimeAssembly()
        {
            foreach (var assembly in universe.GetAssemblies())
                if (assembly.GetType("IKVM.Runtime.JVM") is Type)
                    return ImportAssembly(assembly);

            throw new FatalCompilerErrorException(DiagnosticEvent.RuntimeNotFound());
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveBaseAssembly()
        {
            if (baseAssembly == null)
                Interlocked.CompareExchange(ref baseAssembly, LoadBaseAssembly(), null);

            return baseAssembly;
        }

        /// <summary>
        /// Attempts to load the base assembly.
        /// </summary>
        IAssemblySymbol LoadBaseAssembly()
        {
            foreach (var assembly in universe.GetAssemblies())
                if (assembly.GetType("java.lang.Object") is Type)
                    return ImportAssembly(assembly);

            throw new Exception();
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveCoreType(string typeName)
        {
            return ResolveCoreAssembly().GetType(typeName);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveRuntimeType(string typeName)
        {
            return ResolveRuntimeAssembly().GetType(typeName);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveBaseType(string typeName)
        {
            return ResolveBaseAssembly().GetType(typeName);
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveAssembly(string assemblyName)
        {
            return universe.Load(assemblyName) is { } a ? symbols.GetOrCreateAssemblySymbol(a) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(string typeName)
        {
            foreach (var assembly in universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return ImportType(t);

            return null;
        }

        /// <inheritdoc />
        public IAssemblySymbol ImportAssembly(Assembly assembly)
        {
            return symbols.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder ImportAssembly(AssemblyBuilder assembly)
        {
            return symbols.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IModuleSymbol ImportModule(Module module)
        {
            return symbols.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder ImportModule(ModuleBuilder module)
        {
            return symbols.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public ITypeSymbol ImportType(Type type)
        {
            return symbols.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IMemberSymbol ImportMember(MemberInfo memberInfo)
        {
            return symbols.GetOrCreateMemberSymbol(memberInfo);
        }

        /// <inheritdoc />
        public IMethodBaseSymbol ImportMethodBase(MethodBase type)
        {
            return symbols.GetOrCreateMethodBaseSymbol(type);
        }

        /// <inheritdoc />
        public IConstructorSymbol ImportConstructor(ConstructorInfo ctor)
        {
            return symbols.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IMethodSymbol ImportMethod(MethodInfo method)
        {
            return symbols.GetOrCreateMethodSymbol(method);
        }
    }

}