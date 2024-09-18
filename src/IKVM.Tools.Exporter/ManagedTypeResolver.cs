using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

	class ManagedTypeResolver : ISymbolResolver
	{

		readonly StaticCompiler compiler;
		readonly Assembly baseAssembly;
		readonly IkvmReflectionSymbolContext context = new();

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

		/// <summary>
		/// Attempts to resolve the base Java assembly.
		/// </summary>
		/// <returns></returns>
		public IAssemblySymbol ResolveBaseAssembly()
		{
			return baseAssembly != null ? context.GetOrCreateAssemblySymbol(baseAssembly) : null;
		}

		/// <summary>
		/// Attempts to resolve an assembly from one of the assembly sources.
		/// </summary>
		/// <param name="assemblyName"></param>
		/// <returns></returns>
		public IAssemblySymbol ResolveAssembly(string assemblyName)
		{
			return compiler.Load(assemblyName) is { } a ? context.GetOrCreateAssemblySymbol(a) : null;
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
					return context.GetOrCreateTypeSymbol(t);

			return null;
		}

		/// <summary>
		/// Attempts to resolve a type from the IKVM runtime assembly.
		/// </summary>
		/// <param name="typeName"></param>
		/// <returns></returns>
		public ITypeSymbol ResolveRuntimeType(string typeName)
		{
			return compiler.GetRuntimeType(typeName) is { } t ? context.GetOrCreateTypeSymbol(t) : null;
		}

	}

}
