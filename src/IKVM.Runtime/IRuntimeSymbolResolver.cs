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
        /// Gets the <see cref="IAssemblySymbol"/> associated with the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IAssemblySymbol? ResolveAssembly(Assembly? assembly);

        /// <summary>
        /// Gets the <see cref="IAssemblySymbolBuilder"/> associated with the specified assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IAssemblySymbolBuilder? ResolveAssembly(AssemblyBuilder? assembly);

        /// <summary>
        /// Gets the <see cref="IModuleSymbol"/> associated with the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IModuleSymbol? ResolveModule(Module? module);

        /// <summary>
        /// Gets the <see cref="IModuleSymbolBuilder"/> associated with the specified module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IModuleSymbolBuilder? ResolveModule(ModuleBuilder? module);

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeSymbol? ResolveType(Type? type);

        /// <summary>
        /// Gets the <see cref="IMemberSymbol"/> associated with the specified member.
        /// </summary>
        /// <param name="memberInfo"></param>
        /// <returns></returns>
        IMemberSymbol? ResolveMember(MemberInfo? memberInfo);

        /// <summary>
        /// Gets the <see cref="IMethodBaseSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IMethodBaseSymbol? ResolveMethodBase(MethodBase? type);

        /// <summary>
        /// Gets the <see cref="IConstructorSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        IConstructorSymbol? ResolveConstructor(ConstructorInfo? ctor);

        /// <summary>
        /// Gets the <see cref="IMethodSymbol"/> associated with the specified method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        IMethodSymbol? ResolveMethod(MethodInfo? method);

    }


}
