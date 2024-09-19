using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

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
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbolBuilder"/> for the specified <see cref="TypeBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbolBuilder"/> for the specified <see cref="ConstructorBuilder"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbolBuilder"/> for the specified <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbolBuilder"/> for the specified <see cref="FieldBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbolBuilder"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter);

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbolBuilder"/> for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter);

    }

}
