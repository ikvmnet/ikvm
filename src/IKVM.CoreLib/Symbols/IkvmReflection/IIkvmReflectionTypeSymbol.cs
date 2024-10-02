using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionTypeSymbol : IIkvmReflectionMemberSymbol, ITypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="Type"/>.
        /// </summary>
        Type UnderlyingType { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodBaseSymbol"/> for the given <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbol"/> for the given <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> for the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbol"/> for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbol"/> for the given <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbol"/> for the given <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given generic type parameter type.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreatePointerTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateByRefTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IIkvmReflectionTypeSymbol[] genericTypeDefinition);

    }

}
