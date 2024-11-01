using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionModuleSymbol : IIkvmReflectionSymbol, IModuleSymbol
    {

        /// <summary>
        /// Gets the <see cref="IIkvmReflectionAssemblySymbol"/> which contains this <see cref="IIkvmReflectionModuleSymbol"/>.
        /// </summary>
        IIkvmReflectionAssemblySymbol ResolvingAssembly { get; }

        /// <summary>
        /// Gets the underlying <see cref="Module"/>.
        /// </summary>
        Module UnderlyingModule { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodBaseSymbol"/> for the specified <see cref="MethodBase"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

    }

}
