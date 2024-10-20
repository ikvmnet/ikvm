#nullable enable

using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides an interface to resolve a managed type symbols extended for the Runtime classes.
    /// </summary>
    interface IRuntimeSymbolResolver : ISymbolResolver
    {

        /// <summary>
        /// Gets the <see cref="ISymbolContext"/> that manages access to symbols.
        /// </summary>
        ISymbolContext Symbols { get; }

        /// <summary>
        /// Resolves the named type from any of the known System assemblies.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol ResolveSystemType(string typeName);

        /// <summary>
        /// Gets the known runtime assembly.
        /// </summary>
        /// <returns></returns>
        IAssemblySymbol GetRuntimeAssembly();

        /// <summary>
        /// Resolves the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol ResolveRuntimeType(string typeName);

        /// <summary>
        /// Attempts to resolve the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool TryResolveRuntimeType(string typeName, out ITypeSymbol? type);

        /// <summary>
        /// Resolves the known Java base assembly.
        /// </summary>
        /// <returns></returns>
        IAssemblySymbol? GetBaseAssembly();

        /// <summary>
        /// Resolves the named type from the IKVM Java assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol ResolveBaseType(string typeName);

        /// <summary>
        /// Gets the <see cref="IAssemblySymbol"/> associated with the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IAssemblySymbol GetSymbol(Assembly assembly);

        /// <summary>
        /// Gets the <see cref="IAssemblySymbolBuilder"/> associated with the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder GetSymbol(AssemblyBuilder assembly);

        /// <summary>
        /// Gets the <see cref="IModuleSymbol"/> associated with the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IModuleSymbol GetSymbol(Module module);

        /// <summary>
        /// Gets the <see cref="IModuleSymbolBuilder"/> associated with the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IModuleSymbolBuilder GetSymbol(ModuleBuilder module);

        /// <summary>
        /// Gets the <see cref="IMemberSymbol"/> associated with the specified member.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        IMemberSymbol GetSymbol(MemberInfo memberInfo);

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeSymbol GetSymbol(Type type);

        /// <summary>
        /// Gets the <see cref="IMethodBaseSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IMethodBaseSymbol GetSymbol(MethodBase type);

        /// <summary>
        /// Gets the <see cref="IConstructorSymbol"/> associated with the specified constructor.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IConstructorSymbol GetSymbol(ConstructorInfo ctor);

        /// <summary>
        /// Gets the <see cref="IMethodSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IMethodSymbol GetSymbol(MethodInfo method);

        /// <summary>
        /// Gets the <see cref="IFieldSymbol"/> associated with the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        IFieldSymbol GetSymbol(FieldInfo field);

        /// <summary>
        /// Gets the <see cref="IPropertySymbol"/> associated with the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IPropertySymbol GetSymbol(PropertyInfo property);

        /// <summary>
        /// Gets the <see cref="IEventSymbol"/> associated with the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        IEventSymbol GetSymbol(EventInfo @event);

    }


}
