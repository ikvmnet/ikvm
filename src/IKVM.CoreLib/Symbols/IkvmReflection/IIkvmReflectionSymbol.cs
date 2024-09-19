using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionSymbol : ISymbol
    {

        /// <summary>
        /// Gets the <see cref="IkvmReflectionSymbolContext"/> that contains this symbol.
        /// </summary>
        IkvmReflectionSymbolContext Context { get; }

        /// <summary>
        /// Resolves the symbol for the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assembly))]
        IIkvmReflectionAssemblySymbol? ResolveAssemblySymbol(Assembly? assembly);

        /// <summary>
        /// Resolves the symbol for the specified assembly builder.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assembly))]
        IIkvmReflectionAssemblySymbolBuilder ResolveAssemblySymbol(AssemblyBuilder assembly);

        /// <summary>
        /// Resolves the symbols for the specified assemblies.
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assemblies))]
        IIkvmReflectionAssemblySymbol[]? ResolveAssemblySymbols(Assembly[]? assemblies);

        /// <summary>
        /// Resolves the symbol for the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(module))]
        IIkvmReflectionModuleSymbol? ResolveModuleSymbol(Module? module);

        /// <summary>
        /// Resolves the symbol for the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(module))]
        IIkvmReflectionModuleSymbolBuilder ResolveModuleSymbol(ModuleBuilder module);

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(modules))]
        IIkvmReflectionModuleSymbol[]? ResolveModuleSymbols(Module[]? modules);

        /// <summary>
        /// Resolves the symbols for the specified modules.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        IEnumerable<IIkvmReflectionModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules);

        /// <summary>
        /// Resolves the symbol for the specified member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(member))]
        IIkvmReflectionMemberSymbol? ResolveMemberSymbol(MemberInfo? member);

        /// <summary>
        /// Resolves the symbols for the specified members.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(types))]
        IIkvmReflectionMemberSymbol[]? ResolveMemberSymbols(MemberInfo[]? types);

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        IIkvmReflectionTypeSymbol? ResolveTypeSymbol(Type? type);

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        IIkvmReflectionTypeSymbolBuilder ResolveTypeSymbol(TypeBuilder type);

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(types))]
        IIkvmReflectionTypeSymbol[]? ResolveTypeSymbols(Type[]? types);

        /// <summary>
        /// Resolves the symbols for the specified types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IEnumerable<IIkvmReflectionTypeSymbol> ResolveTypeSymbols(IEnumerable<Type> types);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IIkvmReflectionMethodBaseSymbol? ResolveMethodBaseSymbol(MethodBase? method);

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        IIkvmReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor);

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        IIkvmReflectionConstructorSymbolBuilder ResolveConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Resolves the symbols for the specified constructors.
        /// </summary>
        /// <param name="ctors"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctors))]
        IIkvmReflectionConstructorSymbol[]? ResolveConstructorSymbols(ConstructorInfo[]? ctors);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IIkvmReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IIkvmReflectionMethodSymbolBuilder ResolveMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Resolves the symbols for the specified methods.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(methods))]
        IIkvmReflectionMethodSymbol[]? ResolveMethodSymbols(MethodInfo[]? methods);

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        IIkvmReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field);

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        IIkvmReflectionFieldSymbolBuilder ResolveFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Resolves the symbols for the specified fields.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(fields))]
        IIkvmReflectionFieldSymbol[]? ResolveFieldSymbols(FieldInfo[]? fields);

        /// <summary>
        /// Resolves the symbol for the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(property))]
        IIkvmReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property);

        /// <summary>
        /// Resolves the symbol for the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(property))]
        IIkvmReflectionPropertySymbolBuilder ResolvePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Resolves the symbols for the specified properties.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(properties))]
        IIkvmReflectionPropertySymbol[]? ResolvePropertySymbols(PropertyInfo[]? properties);

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(@event))]
        IIkvmReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event);

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(@event))]
        IIkvmReflectionEventSymbolBuilder ResolveEventSymbol(EventBuilder @event);

        /// <summary>
        /// Resolves the symbols for the specified events.
        /// </summary>
        /// <param name="events"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(events))]
        IIkvmReflectionEventSymbol[]? ResolveEventSymbols(EventInfo[]? events);

        /// <summary>
        /// Resolves the symbol for the specified parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(parameter))]
        IIkvmReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter);

        /// <summary>
        /// Resolves the symbol for the specified parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(parameter))]
        IIkvmReflectionParameterSymbolBuilder? ResolveParameterSymbol(ParameterBuilder parameter);

        /// <summary>
        /// Resolves the symbols for the specified parameters.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(parameters))]
        IIkvmReflectionParameterSymbol[]? ResolveParameterSymbols(ParameterInfo[]? parameters);

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
        /// Transforms a list of <see cref="IKVM.Reflection.CustomAttributeTypedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeTypedArgument> ResolveCustomAttributeTypedArguments(IList<IKVM.Reflection.CustomAttributeTypedArgument> args);

        /// <summary>
        /// Transforms a <see cref="IKVM.Reflection.CustomAttributeTypedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeTypedArgument ResolveCustomAttributeTypedArgument(IKVM.Reflection.CustomAttributeTypedArgument arg);

        /// <summary>
        /// Transforms the type as appropriate.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        object? ResolveCustomAttributeTypedValue(object? value);

        /// <summary>
        /// Transforms a list of <see cref="IKVM.Reflection.CustomAttributeNamedArgument"/> values into symbols.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        ImmutableArray<CustomAttributeNamedArgument> ResolveCustomAttributeNamedArguments(IList<IKVM.Reflection.CustomAttributeNamedArgument> args);

        /// <summary>
        /// Transforms a <see cref="IKVM.Reflection.CustomAttributeNamedArgument"/> values into a symbol.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        CustomAttributeNamedArgument ResolveCustomAttributeNamedArgument(IKVM.Reflection.CustomAttributeNamedArgument arg);

    }

}
