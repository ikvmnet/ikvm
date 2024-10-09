using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionModuleSymbol : IReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Gets the <see cref="IReflectionAssemblySymbol"/> which contains this <see cref="IReflectionModuleSymbol"/>.
        /// </summary>
        IReflectionAssemblySymbol ResolvingAssembly { get; }

        /// <summary>
        /// Gets the underlying <see cref="Module"/>.
        /// </summary>
        Module UnderlyingModule { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodBaseSymbol"/> for the specified <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
