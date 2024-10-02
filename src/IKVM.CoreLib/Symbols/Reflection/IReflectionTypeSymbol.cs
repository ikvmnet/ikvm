using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionTypeSymbol : IReflectionMemberSymbol, ITypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="Type"/>.
        /// </summary>
        Type UnderlyingType { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodBaseSymbol"/> for the given <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the given <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbol"/> for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbol"/> for the given <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbol"/> for the given <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given generic type parameter type.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreatePointerTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateByRefTypeSymbol();

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeDefinition);

    }

}
