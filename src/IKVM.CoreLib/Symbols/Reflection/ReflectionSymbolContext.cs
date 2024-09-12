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
		/// Gets or creates a <see cref="ReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		public ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
		{
			return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
		{
			return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
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
		public ReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return GetOrCreateAssemblySymbol(method.Module.Assembly).GetOrCreateModuleSymbol(method.Module).GetOrCreateTypeSymbol(method.DeclaringType!).GetOrCreateMethodSymbol(method);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public ReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
		{
			return GetOrCreateAssemblySymbol(parameter.Member.Module.Assembly).GetOrCreateModuleSymbol(parameter.Member.Module).GetOrCreateTypeSymbol(parameter.Member.DeclaringType!).GetOrCreateMethodBaseSymbol((MethodBase)parameter.Member).GetOrCreateParameterSymbol(parameter);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public ReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
		{
			return GetOrCreateAssemblySymbol(field.Module.Assembly).GetOrCreateModuleSymbol(field.Module).GetOrCreateTypeSymbol(field.DeclaringType!).GetOrCreateFieldSymbol(field);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public ReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
		{
			return GetOrCreateAssemblySymbol(property.Module.Assembly).GetOrCreateModuleSymbol(property.Module).GetOrCreateTypeSymbol(property.DeclaringType!).GetOrCreatePropertySymbol(property);
		}

		/// <summary>
		/// Gets or creates a <see cref="ReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public ReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
		{
			return GetOrCreateAssemblySymbol(@event.Module.Assembly).GetOrCreateModuleSymbol(@event.Module).GetOrCreateTypeSymbol(@event.DeclaringType!).GetOrCreateEventSymbol(@event);
		}

	}

}
