using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionAssemblySymbol : ReflectionSymbol, IAssemblySymbol
	{

		readonly Assembly _assembly;
		readonly ConditionalWeakTable<Module, ReflectionModuleSymbol> _modules = new();

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="assembly"></param>
		public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly) :
			base(context)
		{
			_assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
		}

		internal Assembly ReflectionObject => _assembly;

		/// <summary>
		/// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		internal ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
		{
			Debug.Assert(module.Assembly == _assembly);
			return _modules.GetValue(module, _ => new ReflectionModuleSymbol(Context, _));
		}

		public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_assembly.DefinedTypes);

		public IMethodSymbol? EntryPoint => _assembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

		public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_assembly.ExportedTypes);

		public string? FullName => _assembly.FullName;

		public string ImageRuntimeVersion => _assembly.ImageRuntimeVersion;

		public IModuleSymbol ManifestModule => ResolveModuleSymbol(_assembly.ManifestModule);

		public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_assembly.Modules);

		public ITypeSymbol[] GetExportedTypes()
		{
			return ResolveTypeSymbols(_assembly.GetExportedTypes());
		}

		public IModuleSymbol? GetModule(string name)
		{
			return _assembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
		}

		public IModuleSymbol[] GetModules()
		{
			return ResolveModuleSymbols(_assembly.GetModules());
		}

		public IModuleSymbol[] GetModules(bool getResourceModules)
		{
			return ResolveModuleSymbols(_assembly.GetModules(getResourceModules));
		}

		public AssemblyName GetName()
		{
			return _assembly.GetName();
		}

		public AssemblyName GetName(bool copiedName)
		{
			return _assembly.GetName(copiedName);
		}

		public AssemblyName[] GetReferencedAssemblies()
		{
			return _assembly.GetReferencedAssemblies();
		}

		public ITypeSymbol? GetType(string name, bool throwOnError)
		{
			return _assembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
		{
			return _assembly.GetType(name, throwOnError, ignoreCase) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
		}

		public ITypeSymbol? GetType(string name)
		{
			return _assembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
		}

		public ITypeSymbol[] GetTypes()
		{
			return ResolveTypeSymbols(_assembly.GetTypes());
		}

		public CustomAttributeSymbol[] GetCustomAttributes()
		{
			return ResolveCustomAttributes(_assembly.GetCustomAttributesData());
		}

		public CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType)
		{
			return ResolveCustomAttributes(_assembly.GetCustomAttributesData()).Where(i => i.AttributeType == attributeType).ToArray();
		}

		public bool IsDefined(ITypeSymbol attributeType)
		{
			return _assembly.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject);
		}

	}

}
