using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionTypeSymbolBuilder : IIkvmReflectionMemberSymbolBuilder, ITypeSymbolBuilder, IIkvmReflectionTypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="TypeBuilder"/>.
        /// </summary>
        TypeBuilder UnderlyingTypeBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbolBuilder"/> for the given <see cref="ConstructorBuilder"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbolBuilder"/> for the given <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbolBuilder"/> for the given <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbolBuilder"/> for the given <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbolBuilder"/> for the given <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionGenericTypeParameterSymbolBuilder"/> for the given <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

    }

}
