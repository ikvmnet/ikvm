using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using IKVM.Reflection;

using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MemberInfo = IKVM.Reflection.MemberInfo;
using MethodBase = IKVM.Reflection.MethodBase;
using MethodInfo = IKVM.Reflection.MethodInfo;
using Module = IKVM.Reflection.Module;
using ParameterInfo = IKVM.Reflection.ParameterInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	abstract class IkvmReflectionSymbol : ISymbol
	{

		readonly IkvmReflectionSymbolContext _context;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		public IkvmReflectionSymbol(IkvmReflectionSymbolContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		/// <summary>
		/// Gets the associated <see cref="IkvmReflectionSymbolContext"/>.
		/// </summary>
		protected IkvmReflectionSymbolContext Context => _context;

		/// <inheritdoc />
		public virtual bool IsMissing => false;

		/// <summary>
		/// Resolves the symbol for the specified type.
		/// </summary>
		/// <param name="module"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionModuleSymbol ResolveModuleSymbol(Module module)
		{
			return _context.GetOrCreateModuleSymbol(module);
		}

		/// <summary>
		/// Resolves the symbols for the specified modules.
		/// </summary>
		/// <param name="modules"></param>
		/// <returns></returns>
		protected internal IkvmReflectionModuleSymbol[] ResolveModuleSymbols(Module[] modules)
		{
			var a = new IkvmReflectionModuleSymbol[modules.Length];
			for (int i = 0; i < modules.Length; i++)
				a[i] = ResolveModuleSymbol(modules[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbols for the specified modules.
		/// </summary>
		/// <param name="modules"></param>
		/// <returns></returns>
		protected internal IEnumerable<IkvmReflectionModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules)
		{
			foreach (var module in modules)
				yield return ResolveModuleSymbol(module);
		}

		/// <summary>
		/// Unpacks the symbols into their original type.
		/// </summary>
		/// <param name="modules"></param>
		/// <returns></returns>
		protected internal Module[] UnpackModuleSymbols(IModuleSymbol[] modules)
		{
			var a = new Module[modules.Length];
			for (int i = 0; i < modules.Length; i++)
				a[i] = ((IkvmReflectionModuleSymbol)modules[i]).ReflectionObject;

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified type.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		protected virtual IkvmReflectionMemberSymbol ResolveMemberSymbol(MemberInfo member)
		{
			return member.MemberType switch
			{
				MemberTypes.Constructor => ResolveConstructorSymbol((ConstructorInfo)member),
				MemberTypes.Event => ResolveEventSymbol((EventInfo)member),
				MemberTypes.Field => ResolveFieldSymbol((FieldInfo)member),
				MemberTypes.Method => ResolveMethodSymbol((MethodInfo)member),
				MemberTypes.Property => ResolvePropertySymbol((PropertyInfo)member),
				MemberTypes.TypeInfo => ResolveTypeSymbol((Type)member),
				MemberTypes.NestedType => ResolveTypeSymbol((Type)member),
				MemberTypes.Custom => throw new NotImplementedException(),
				MemberTypes.All => throw new NotImplementedException(),
				_ => throw new InvalidOperationException(),
			};
		}

		/// <summary>
		/// Resolves the symbols for the specified types.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		protected internal IkvmReflectionMemberSymbol[] ResolveMemberSymbols(MemberInfo[] types)
		{
			var a = new IkvmReflectionMemberSymbol[types.Length];
			for (int i = 0; i < types.Length; i++)
				a[i] = ResolveMemberSymbol(types[i]);

			return a;
		}

		/// <summary>
		/// Unpacks the symbols into their original type.
		/// </summary>
		/// <param name="members"></param>
		/// <returns></returns>
		protected internal MemberInfo[] UnpackMemberSymbols(IMemberSymbol[] members)
		{
			var a = new MemberInfo[members.Length];
			for (int i = 0; i < members.Length; i++)
				a[i] = ((IkvmReflectionMemberSymbol)members[i]).ReflectionObject;

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionTypeSymbol ResolveTypeSymbol(Type type)
		{
			return _context.GetOrCreateTypeSymbol(type);
		}

		/// <summary>
		/// Resolves the symbols for the specified types.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		protected internal IkvmReflectionTypeSymbol[] ResolveTypeSymbols(Type[] types)
		{
			var a = new IkvmReflectionTypeSymbol[types.Length];
			for (int i = 0; i < types.Length; i++)
				a[i] = ResolveTypeSymbol(types[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbols for the specified types.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		protected internal IEnumerable<IkvmReflectionTypeSymbol> ResolveTypeSymbols(IEnumerable<Type> types)
		{
			foreach (var type in types)
				yield return ResolveTypeSymbol(type);
		}

		/// <summary>
		/// Unpacks the symbols into their original type.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		protected internal Type[] UnpackTypeSymbols(ITypeSymbol[] types)
		{
			var a = new Type[types.Length];
			for (int i = 0; i < types.Length; i++)
				a[i] = ((IkvmReflectionTypeSymbol)types[i]).ReflectionObject;

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionMethodBaseSymbol ResolveMethodBaseSymbol(MethodBase method)
		{
			if (method.IsConstructor)
				return ResolveConstructorSymbol((ConstructorInfo)method);
			else
				return ResolveMethodSymbol((MethodInfo)method);
		}

		/// <summary>
		/// Resolves the symbol for the specified constructor.
		/// </summary>
		/// <param name="ctor"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionConstructorSymbol ResolveConstructorSymbol(ConstructorInfo ctor)
		{
			return _context.GetOrCreateConstructorSymbol(ctor);
		}

		/// <summary>
		/// Resolves the symbols for the specified constructors.
		/// </summary>
		/// <param name="ctors"></param>
		/// <returns></returns>
		protected internal IkvmReflectionConstructorSymbol[] ResolveConstructorSymbols(ConstructorInfo[] ctors)
		{
			var a = new IkvmReflectionConstructorSymbol[ctors.Length];
			for (int i = 0; i < ctors.Length; i++)
				a[i] = ResolveConstructorSymbol(ctors[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified method.
		/// </summary>
		/// <param name="method"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionMethodSymbol ResolveMethodSymbol(MethodInfo method)
		{
			return _context.GetOrCreateMethodSymbol(method);
		}

		/// <summary>
		/// Resolves the symbols for the specified methods.
		/// </summary>
		/// <param name="methods"></param>
		/// <returns></returns>
		protected internal IkvmReflectionMethodSymbol[] ResolveMethodSymbols(MethodInfo[] methods)
		{
			var a = new IkvmReflectionMethodSymbol[methods.Length];
			for (int i = 0; i < methods.Length; i++)
				a[i] = ResolveMethodSymbol(methods[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified parameter.
		/// </summary>
		/// <param name="parameter"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionParameterSymbol ResolveParameterSymbol(ParameterInfo parameter)
		{
			return _context.GetOrCreateParameterSymbol(parameter);
		}

		/// <summary>
		/// Resolves the symbols for the specified parameters.
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected internal IkvmReflectionParameterSymbol[] ResolveParameterSymbols(ParameterInfo[] parameters)
		{
			var a = new IkvmReflectionParameterSymbol[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
				a[i] = ResolveParameterSymbol(parameters[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified field.
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionFieldSymbol ResolveFieldSymbol(FieldInfo field)
		{
			return _context.GetOrCreateFieldSymbol(field);
		}

		/// <summary>
		/// Resolves the symbols for the specified fields.
		/// </summary>
		/// <param name="fields"></param>
		/// <returns></returns>
		protected internal IkvmReflectionFieldSymbol[] ResolveFieldSymbols(FieldInfo[] fields)
		{
			var a = new IkvmReflectionFieldSymbol[fields.Length];
			for (int i = 0; i < fields.Length; i++)
				a[i] = ResolveFieldSymbol(fields[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified field.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionPropertySymbol ResolvePropertySymbol(PropertyInfo property)
		{
			return _context.GetOrCreatePropertySymbol(property);
		}

		/// <summary>
		/// Resolves the symbols for the specified properties.
		/// </summary>
		/// <param name="properties"></param>
		/// <returns></returns>
		protected internal IkvmReflectionPropertySymbol[] ResolvePropertySymbols(PropertyInfo[] properties)
		{
			var a = new IkvmReflectionPropertySymbol[properties.Length];
			for (int i = 0; i < properties.Length; i++)
				a[i] = ResolvePropertySymbol(properties[i]);

			return a;
		}

		/// <summary>
		/// Resolves the symbol for the specified event.
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		protected virtual internal IkvmReflectionEventSymbol ResolveEventSymbol(EventInfo @event)
		{
			return _context.GetOrCreateEventSymbol(@event);
		}

		/// <summary>
		/// Resolves the symbols for the specified events.
		/// </summary>
		/// <param name="events"></param>
		/// <returns></returns>
		protected internal IkvmReflectionEventSymbol[] ResolveEventSymbols(EventInfo[] events)
		{
			var a = new IkvmReflectionEventSymbol[events.Length];
			for (int i = 0; i < events.Length; i++)
				a[i] = ResolveEventSymbol(events[i]);

			return a;
		}

		/// <summary>
		/// Transforms a custom set of custom attribute data records to a symbol record.
		/// </summary>
		/// <param name="attributes"></param>
		/// <returns></returns>
		protected internal CustomAttributeSymbol[] ResolveCustomAttributes(IList<CustomAttributeData> attributes)
		{
			var a = new CustomAttributeSymbol[attributes.Count];
			for (int i = 0; i < attributes.Count; i++)
				a[i] = ResolveCustomAttribute(attributes[i]);

			return a;
		}

		/// <summary>
		/// Transforms a custom attribute data record to a symbol record.
		/// </summary>
		/// <param name="customAttributeData"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		protected internal CustomAttributeSymbol ResolveCustomAttribute(CustomAttributeData customAttributeData)
		{
			return new CustomAttributeSymbol(
				ResolveTypeSymbol(customAttributeData.AttributeType),
				ResolveConstructorSymbol(customAttributeData.Constructor),
				ResolveCustomAttributeTypedArguments(customAttributeData.ConstructorArguments),
				ResolveCustomAttributeNamedArguments(customAttributeData.NamedArguments));
		}

		/// <summary>
		/// Transforms a list of <see cref="CustomAttributeTypedArgument"/> values into symbols.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		ImmutableArray<CustomAttributeSymbolTypedArgument> ResolveCustomAttributeTypedArguments(IList<CustomAttributeTypedArgument> args)
		{
			var a = new CustomAttributeSymbolTypedArgument[args.Count];
			for (int i = 0; i < args.Count; i++)
				a[i] = ResolveCustomAttributeTypedArgument(args[i]);

			return a.ToImmutableArray();
		}

		/// <summary>
		/// Transforms a <see cref="CustomAttributeTypedArgument"/> values into a symbol.
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		CustomAttributeSymbolTypedArgument ResolveCustomAttributeTypedArgument(CustomAttributeTypedArgument arg)
		{
			return new CustomAttributeSymbolTypedArgument(ResolveTypeSymbol(arg.ArgumentType), arg.Value);
		}

		/// <summary>
		/// Transforms a list of <see cref="CustomAttributeNamedArgument"/> values into symbols.
		/// </summary>
		/// <param name="args"></param>
		/// <returns></returns>
		ImmutableArray<CustomAttributeSymbolNamedArgument> ResolveCustomAttributeNamedArguments(IList<CustomAttributeNamedArgument> args)
		{
			var a = new CustomAttributeSymbolNamedArgument[args.Count];
			for (int i = 0; i < args.Count; i++)
				a[i] = ResolveCustomAttributeNamedArgument(args[i]);

			return a.ToImmutableArray();
		}

		/// <summary>
		/// Transforms a <see cref="CustomAttributeNamedArgument"/> values into a symbol.
		/// </summary>
		/// <param name="arg"></param>
		/// <returns></returns>
		CustomAttributeSymbolNamedArgument ResolveCustomAttributeNamedArgument(CustomAttributeNamedArgument arg)
		{
			return new CustomAttributeSymbolNamedArgument(arg.IsField, ResolveMemberSymbol(arg.MemberInfo), arg.MemberName, ResolveCustomAttributeTypedArgument(arg.TypedValue));
		}

	}

}