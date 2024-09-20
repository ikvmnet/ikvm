#nullable enable

using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    class ManagedTypeResolver : IRuntimeSymbolResolver
    {

        readonly StaticCompiler compiler;
        readonly Assembly baseAssembly;
        readonly IkvmReflectionSymbolContext symbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="baseAssembly"></param>
        public ManagedTypeResolver(IkvmReflectionSymbolContext symbols, StaticCompiler compiler, Assembly baseAssembly)
        {
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            this.baseAssembly = baseAssembly;
        }

        /// <inheritdoc />
        public ISymbolContext Symbols => symbols;

        /// <summary>
        /// Attempts to resolve the base Java assembly.
        /// </summary>
        /// <returns></returns>
        public IAssemblySymbol? ResolveBaseAssembly()
        {
            return ResolveAssembly(baseAssembly);
        }

        /// <summary>
        /// Attempts to resolve an assembly from one of the assembly sources.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        public IAssemblySymbol? ResolveAssembly(string assemblyName)
        {
            return ResolveAssembly(compiler.Load(assemblyName));
        }

        /// <summary>
        /// Attempts to resolve a type from one of the assembly sources.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ITypeSymbol? ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return ResolveType(t);

            return null;
        }

        /// <summary>
        /// Attempts to resolve a type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public ITypeSymbol? ResolveRuntimeType(string typeName)
        {
            return ResolveType(compiler.GetRuntimeType(typeName));
        }

        /// <inheritdoc />
        public IAssemblySymbol? ResolveAssembly(Assembly? assembly)
        {
            return assembly != null ? symbols.GetOrCreateAssemblySymbol(assembly) : null;
        }

        /// <inheritdoc />
        public IModuleSymbol? ResolveModule(Module? module)
        {
            return module != null ? symbols.GetOrCreateModuleSymbol(module) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveType(Type? type)
        {
            return type != null ? symbols.GetOrCreateTypeSymbol(type) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethodBase(MethodBase? method)
        {
            return method != null ? symbols.GetOrCreateMethodBaseSymbol(method) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbol? ResolveConstructor(ConstructorInfo? ctor)
        {
            return ctor != null ? symbols.GetOrCreateConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? ResolveMethod(MethodInfo? method)
        {
            return method != null ? symbols.GetOrCreateMethodSymbol(method) : null;
        }

    }

}
