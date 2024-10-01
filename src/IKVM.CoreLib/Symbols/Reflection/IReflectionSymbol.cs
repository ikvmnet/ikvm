using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionSymbol : ISymbol
    {

        /// <summary>
        /// Gets the <see cref="ReflectionSymbolContext"/> that contains this symbol.
        /// </summary>
        ReflectionSymbolContext Context { get; }

        /// <summary>
        /// Resolves the symbol for the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assembly))]
        IReflectionAssemblySymbol? ResolveAssemblySymbol(Assembly? assembly);

        /// <summary>
        /// Resolves the symbols for the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assemblies))]
        IReflectionAssemblySymbol[]? ResolveAssemblySymbols(Assembly[]? assemblies);

        /// <summary>
        /// Resolves the symbol for the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(module))]
        IReflectionModuleSymbol? ResolveModuleSymbol(Module? module);

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(modules))]
        IReflectionModuleSymbol[]? ResolveModuleSymbols(Module[]? modules);

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        IEnumerable<IReflectionModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules);

        /// <summary>
        /// Resolves the symbol for the specified member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(member))]
        IReflectionMemberSymbol? ResolveMemberSymbol(MemberInfo? member);

        /// <summary>
        /// Resolves the symbols for the specified members.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(types))]
        IReflectionMemberSymbol[]? ResolveMemberSymbols(MemberInfo[]? types);

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        IReflectionTypeSymbol? ResolveTypeSymbol(Type? type);

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(types))]
        IReflectionTypeSymbol[]? ResolveTypeSymbols(Type[]? types);

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<IReflectionTypeSymbol> ResolveTypeSymbols(IEnumerable<Type> types);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IReflectionMethodBaseSymbol? ResolveMethodBaseSymbol(MethodBase? method);

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        IReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor);

        /// <summary>
        /// Resolves the symbols for the specified constructors.
        /// </summary>
        /// <param name="ctors"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctors))]
        IReflectionConstructorSymbol[]? ResolveConstructorSymbols(ConstructorInfo[]? ctors);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method);

        /// <summary>
        /// Resolves the symbols for the specified methods.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(methods))]
        IReflectionMethodSymbol[]? ResolveMethodSymbols(MethodInfo[]? methods);

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        IReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field);

        /// <summary>
        /// Resolves the symbols for the specified fields.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(fields))]
        IReflectionFieldSymbol[]? ResolveFieldSymbols(FieldInfo[]? fields);

        /// <summary>
        /// Resolves the symbol for the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(property))]
        IReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property);

        /// <summary>
        /// Resolves the symbols for the specified properties.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(properties))]
        IReflectionPropertySymbol[]? ResolvePropertySymbols(PropertyInfo[]? properties);

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(@event))]
        IReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event);

        /// <summary>
        /// Resolves the symbols for the specified events.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(events))]
        IReflectionEventSymbol[]? ResolveEventSymbols(EventInfo[]? events);

        /// <summary>
        /// Resolves the symbol for the specified parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(parameter))]
        IReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter);

        /// <summary>
        /// Resolves the symbols for the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(parameters))]
        IReflectionParameterSymbol[]? ResolveParameterSymbols(ParameterInfo[]? parameters);

        /// <summary>
        /// Transforms a custom set of custom attribute data records to a symbol record.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(attributes))]
        CustomAttribute[]? ResolveCustomAttributes(IList<CustomAttributeData>? attributes);

        /// <summary>
        /// Transforms a custom set of custom attribute data records to a symbol record.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        IEnumerable<CustomAttribute> ResolveCustomAttributes(IEnumerable<CustomAttributeData> attributes);

        /// <summary>
        /// Transforms a custom attribute data record to a symbol record.
        /// </summary>
        /// <param name="customAttributeData"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(customAttributeData))]
        CustomAttribute? ResolveCustomAttribute(CustomAttributeData? customAttributeData);

        /// <summary>
        /// Transforms a list of <see cref="System.Reflection.CustomAttributeTypedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeTypedArgument> ResolveCustomAttributeTypedArguments(IList<System.Reflection.CustomAttributeTypedArgument> args);

        /// <summary>
        /// Transforms a <see cref="System.Reflection.CustomAttributeTypedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeTypedArgument ResolveCustomAttributeTypedArgument(System.Reflection.CustomAttributeTypedArgument arg);

        /// <summary>
        /// Transforms the type as appropriate.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? ResolveCustomAttributeTypedValue(object? value);

        /// <summary>
        /// Transforms a list of <see cref="System.Reflection.CustomAttributeNamedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeNamedArgument> ResolveCustomAttributeNamedArguments(IList<System.Reflection.CustomAttributeNamedArgument> args);

        /// <summary>
        /// Transforms a <see cref="System.Reflection.CustomAttributeNamedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeNamedArgument ResolveCustomAttributeNamedArgument(System.Reflection.CustomAttributeNamedArgument arg);

        /// <summary>
        /// Transforms a <see cref="System.Reflection.InterfaceMapping"/> into a symbol type.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        InterfaceMapping ResolveInterfaceMapping(System.Reflection.InterfaceMapping arg);

    }

}
