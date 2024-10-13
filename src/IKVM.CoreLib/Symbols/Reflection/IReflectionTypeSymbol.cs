using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionTypeSymbol : IReflectionMemberSymbol, ITypeSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="Type"/>.
        /// </summary>
        Type UnderlyingType { get; }

        /// <summary>
        /// Gets the underlying <see cref="Type"/> used for IL emit operations for dynamic methods.
        /// </summary>
        Type UnderlyingRuntimeType { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given element type.
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeDefinition);

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
        /// Gets or creates a <see cref="IReflectionConstructorSymbolBuilder"/> for the given <see cref="ConstructorBuilder"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the given <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbolBuilder"/> for the given <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbol"/> for the given <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbolBuilder"/> for the given <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbol"/> for the given <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the given <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbol"/> for the given <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbolBuilder"/> for the given <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="@event"></param>
        /// <returns></returns>
        IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the given generic type parameter type.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionGenericTypeParameterSymbolBuilder"/> for the given <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter);

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

    }

}
