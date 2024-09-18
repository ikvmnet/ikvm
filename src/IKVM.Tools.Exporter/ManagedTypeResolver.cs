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
		readonly IkvmReflectionSymbolContext symbols = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="baseAssembly"></param>
        public ManagedTypeResolver(StaticCompiler compiler, Assembly baseAssembly)
		{
			this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
			this.baseAssembly = baseAssembly;
        }

		/// <inheritdoc />
        public ISymbolContext Symbols => symbols;

        /// <summary>
        /// Attempts to resolve the base Java assembly.
        /// </summary>
        /// <returns></returns>
        public IAssemblySymbol ResolveBaseAssembly()
		{
			return baseAssembly != null ? symbols.GetOrCreateAssemblySymbol(baseAssembly) : null;
		}

		/// <summary>
		/// Attempts to resolve an assembly from one of the assembly sources.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public IAssemblySymbol ResolveAssembly(string assemblyName)
		{
			return compiler.Load(assemblyName) is { } a ? symbols.GetOrCreateAssemblySymbol(a) : null;
		}

		/// <summary>
		/// Attempts to resolve a type from one of the assembly sources.
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public ITypeSymbol ResolveCoreType(string typeName)
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
		public ITypeSymbol ResolveRuntimeType(string typeName)
		{
			return compiler.GetRuntimeType(typeName) is { } t ? symbols.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveAssembly(Assembly assembly)
        {
            return symbols.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IModuleSymbol ResolveModule(Module module)
        {
            return symbols.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(Type type)
        {
			return symbols.GetOrCreateTypeSymbol(type);
        }

    }

}
