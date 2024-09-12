using System.Runtime.CompilerServices;

using Assembly = IKVM.Reflection.Assembly;
using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MethodBase = IKVM.Reflection.MethodBase;
using MethodInfo = IKVM.Reflection.MethodInfo;
using Module = IKVM.Reflection.Module;
using ParameterInfo = IKVM.Reflection.ParameterInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	/// <summary>
	/// Holds references to symbols derived from System.Reflection.
	/// </summary>
	class IkvmReflectionSymbolContext
	{

		readonly ConditionalWeakTable<Assembly, IkvmReflectionAssemblySymbol> _assemblies = new();

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public IkvmReflectionSymbolContext()
		{

		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public IkvmReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
		{
			return _assemblies.GetValue(assembly, _ => new IkvmReflectionAssemblySymbol(this, _));
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		public IkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
		{
			return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public IkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
		{
			return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
		/// </summary>
		/// <param name="ctor"></param>
		/// <returns></returns>
		public IkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
		{
			return GetOrCreateAssemblySymbol(ctor.Module.Assembly).GetOrCreateModuleSymbol(ctor.Module).GetOrCreateTypeSymbol(ctor.DeclaringType!).GetOrCreateConstructorSymbol(ctor);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionMethodBaseSymbol"/> for the specified <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		public IkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
		{
			return GetOrCreateAssemblySymbol(method.Module.Assembly).GetOrCreateModuleSymbol(method.Module).GetOrCreateTypeSymbol(method.DeclaringType!).GetOrCreateMethodSymbol(method);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		public IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
		{
			return GetOrCreateAssemblySymbol(parameter.Member.Module.Assembly).GetOrCreateModuleSymbol(parameter.Member.Module).GetOrCreateTypeSymbol(parameter.Member.DeclaringType!).GetOrCreateMethodBaseSymbol((MethodBase)parameter.Member).GetOrCreateParameterSymbol(parameter);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public IkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
		{
			return GetOrCreateAssemblySymbol(field.Module.Assembly).GetOrCreateModuleSymbol(field.Module).GetOrCreateTypeSymbol(field.DeclaringType!).GetOrCreateFieldSymbol(field);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public IkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
		{
			return GetOrCreateAssemblySymbol(property.Module.Assembly).GetOrCreateModuleSymbol(property.Module).GetOrCreateTypeSymbol(property.DeclaringType!).GetOrCreatePropertySymbol(property);
		}

		/// <summary>
		/// Gets or creates a <see cref="IkvmReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public IkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
		{
			return GetOrCreateAssemblySymbol(@event.Module.Assembly).GetOrCreateModuleSymbol(@event.Module).GetOrCreateTypeSymbol(@event.DeclaringType!).GetOrCreateEventSymbol(@event);
		}

	}

}
