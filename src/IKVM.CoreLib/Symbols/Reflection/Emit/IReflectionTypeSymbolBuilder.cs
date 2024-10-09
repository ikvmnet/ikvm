using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionTypeSymbolBuilder : IReflectionMemberSymbolBuilder, ITypeSymbolBuilder, IReflectionTypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="TypeBuilder"/>.
        /// </summary>
        TypeBuilder UnderlyingTypeBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbolBuilder"/> for the given <see cref="ConstructorBuilder"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbolBuilder"/> for the given <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbolBuilder"/> for the given <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the given <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbolBuilder"/> for the given <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionGenericTypeParameterSymbolBuilder"/> for the given <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

    }

}
