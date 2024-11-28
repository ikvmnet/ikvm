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
        /// Gets the <see cref="SymbolContext"/> that manages access to symbols.
        /// </summary>
        SymbolContext Symbols { get; }

        /// <summary>
        /// Resolves the named type from any of the known System assemblies.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol ResolveSystemType(string typeName);

        /// <summary>
        /// Gets the known runtime assembly.
        /// </summary>
        /// <returns></returns>
        AssemblySymbol GetRuntimeAssembly();

        /// <summary>
        /// Resolves the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol ResolveRuntimeType(string typeName);

        /// <summary>
        /// Attempts to resolve the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool TryResolveRuntimeType(string typeName, out TypeSymbol? type);

        /// <summary>
        /// Resolves the known Java base assembly.
        /// </summary>
        /// <returns></returns>
        AssemblySymbol? GetBaseAssembly();

        /// <summary>
        /// Resolves the named type from the IKVM Java assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol ResolveBaseType(string typeName);

        /// <summary>
        /// Gets the <see cref="AssemblySymbol"/> associated with the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        AssemblySymbol GetSymbol(Assembly assembly);

        /// <summary>
        /// Gets the <see cref="ModuleSymbol"/> associated with the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        ModuleSymbol GetSymbol(Module module);

        /// <summary>
        /// Gets the <see cref="MemberSymbol"/> associated with the specified member.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        MemberSymbol GetSymbol(MemberInfo member);

        /// <summary>
        /// Gets the <see cref="TypeSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeSymbol GetSymbol(Type type);

        /// <summary>
        /// Gets the <see cref="MethodBaseSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        MethodSymbol GetSymbol(MethodBase type);

        /// <summary>
        /// Gets the <see cref="FieldSymbol"/> associated with the specified field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        FieldSymbol GetSymbol(FieldInfo field);

        /// <summary>
        /// Gets the <see cref="PropertySymbol"/> associated with the specified property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        PropertySymbol GetSymbol(PropertyInfo property);

        /// <summary>
        /// Gets the <see cref="EventSymbol"/> associated with the specified event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        EventSymbol GetSymbol(EventInfo @event);

    }


}
