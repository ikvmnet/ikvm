using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Symbols.Reflection
{

	/// <summary>
	/// Holds references to symbols derived from System.Reflection.
	/// </summary>
	class ReflectionSymbolContext
	{

		readonly ConditionalWeakTable<Assembly, ReflectionAssemblySymbol> _assemblies = new();

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public ReflectionSymbolContext()
		{

		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public ReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
		{
			return _assemblies.GetValue(assembly, _ => new ReflectionAssemblySymbol(this, _));
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
		{
			return GetOrCreateAssemblySymbol(type.Assembly).GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
		/// </summary>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public ReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
		{
			return GetOrCreateAssemblySymbol(ctor.Module.Assembly).GetOrCreateModuleSymbol(ctor.Module).GetOrCreateTypeSymbol(ctor.DeclaringType!).GetOrCreateConstructorSymbol(ctor);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionMethodBaseSymbol"/> for the specified <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public ReflectionMethodBaseSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return GetOrCreateAssemblySymbol(method.Module.Assembly).GetOrCreateModuleSymbol(method.Module).GetOrCreateTypeSymbol(method.DeclaringType!).GetOrCreateMethodSymbol(method);
		}

	}

}
