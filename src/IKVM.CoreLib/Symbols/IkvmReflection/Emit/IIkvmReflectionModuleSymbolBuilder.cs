using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionModuleSymbolBuilder : IIkvmReflectionSymbolBuilder, IModuleSymbolBuilder, IIkvmReflectionModuleSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ModuleBuilder"/>.
        /// </summary>
        ModuleBuilder UnderlyingModuleBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbolBuilder"/> for the specified <see cref="TypeBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbolBuilder"/> for the specified <see cref="ConstructorBuilder"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbolBuilder"/> for the specified <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbolBuilder"/> for the specified <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbolBuilder"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbolBuilder"/> for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionGenericTypeParameterSymbolBuilder"/> for the specified <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

    }

}
