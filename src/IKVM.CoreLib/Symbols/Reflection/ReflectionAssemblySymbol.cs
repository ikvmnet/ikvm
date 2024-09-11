using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionAssemblySymbol : IAssemblySymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly Assembly _assembly;
		readonly ConditionalWeakTable<Module, ReflectionModuleSymbol> _modules = new();

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="assembly"></param>
		public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
		}

		/// <summary>
		/// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
		{
			Debug.Assert(module.Assembly == _assembly);
			return _modules.GetValue(module, _ => new ReflectionModuleSymbol(_context, _));
		}

		public ImmutableArray<ICustomAttributeSymbol> CustomAttributes => throw new System.NotImplementedException();

		public ImmutableArray<ITypeSymbol> DefinedTypes => throw new System.NotImplementedException();

		public IMethodSymbol? EntryPoint => throw new System.NotImplementedException();

		public ImmutableArray<ITypeSymbol> ExportedTypes => throw new System.NotImplementedException();

		public string? FullName => _assembly.FullName;

		public string ImageRuntimeVersion => _assembly.ImageRuntimeVersion;

		public IModuleSymbol ManifestModule => throw new System.NotImplementedException();

		public ImmutableArray<IModuleSymbol> Modules => throw new System.NotImplementedException();

		public bool IsMissing => throw new System.NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributesData()
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<ITypeSymbol> GetExportedTypes()
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<ITypeSymbol> GetForwardedTypes()
		{
			throw new System.NotImplementedException();
		}

		public IModuleSymbol? GetModule(string name)
		{
			return _assembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
		}

		public ImmutableArray<IModuleSymbol> GetModules()
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<IModuleSymbol> GetModules(bool getResourceModules)
		{
			throw new System.NotImplementedException();
		}

		public AssemblyName GetName()
		{
			return _assembly.GetName();
		}

		public AssemblyName GetName(bool copiedName)
		{
			return _assembly.GetName(copiedName);
		}

		public ImmutableArray<AssemblyName> GetReferencedAssemblies()
		{
			throw new System.NotImplementedException();
		}

		public ITypeSymbol? GetType(string name, bool throwOnError)
		{
			return _assembly.GetType(name, throwOnError) is Type t ? _context.GetOrCreateTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
		{
			return _assembly.GetType(name, throwOnError, ignoreCase) is Type t ? _context.GetOrCreateTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetType(string name)
		{
			return _assembly.GetType(name) is Type t ? _context.GetOrCreateTypeSymbol(t) : null;
		}

		public ImmutableArray<ITypeSymbol> GetTypes()
		{
			throw new System.NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			return _assembly.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionType, inherit);
		}

	}

}