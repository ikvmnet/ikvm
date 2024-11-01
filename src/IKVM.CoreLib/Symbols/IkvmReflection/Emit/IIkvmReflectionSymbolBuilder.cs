using System.Diagnostics.CodeAnalysis;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionSymbolBuilder : ISymbolBuilder
    {

        /// <summary>
        /// Resolves the symbol for the specified assembly builder.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(assembly))]
        IIkvmReflectionAssemblySymbolBuilder ResolveAssemblySymbol(AssemblyBuilder assembly);

        /// <summary>
        /// Resolves the symbol for the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(module))]
        IIkvmReflectionModuleSymbolBuilder ResolveModuleSymbol(ModuleBuilder module);

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        IIkvmReflectionTypeSymbolBuilder ResolveTypeSymbol(TypeBuilder type);

        /// <summary>
        /// Resolves the symbol for the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        IIkvmReflectionConstructorSymbolBuilder ResolveConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Resolves the symbol for the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        IIkvmReflectionMethodSymbolBuilder ResolveMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Resolves the symbol for the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        IIkvmReflectionFieldSymbolBuilder ResolveFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Resolves the symbol for the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(property))]
        IIkvmReflectionPropertySymbolBuilder ResolvePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Resolves the symbol for the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(@event))]
        IIkvmReflectionEventSymbolBuilder ResolveEventSymbol(EventBuilder @event);

        /// <summary>
        /// Resolves the symbol for the specified generic type parameter.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(genericTypeParameter))]
        IIkvmReflectionGenericTypeParameterSymbolBuilder? ResolveGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

    }

}
