#nullable enable

using System;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Exporter
{

    class ManagedTypeResolver : IRuntimeSymbolResolver
    {

        readonly Universe universe;
        readonly Assembly runtimeAssembly;
        readonly Assembly baseAssembly;
        readonly IkvmReflectionSymbolContext symbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="baseAssembly"></param>
        public ManagedTypeResolver(IkvmReflectionSymbolContext symbols, Universe universe, Assembly runtimeAssembly, Assembly baseAssembly)
        {
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.universe = universe ?? throw new ArgumentNullException(nameof(universe));
            this.runtimeAssembly = runtimeAssembly ?? throw new ArgumentNullException(nameof(runtimeAssembly));
            this.baseAssembly = baseAssembly;
        }

        /// <inheritdoc />
        public ISymbolContext Symbols => symbols;

        /// <inheritdoc />
        public IAssemblySymbol ResolveCoreAssembly()
        {
            return ImportAssembly(universe.CoreLib);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveCoreType(string typeName)
        {
            return ResolveCoreAssembly().GetType(typeName) ?? throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveRuntimeAssembly()
        {
            return ImportAssembly(runtimeAssembly);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveRuntimeType(string typeName)
        {
            return ResolveRuntimeAssembly().GetType(typeName) ?? throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveBaseAssembly()
        {
            return ImportAssembly(baseAssembly);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveBaseType(string typeName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveType(string typeName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IAssemblySymbol ResolveAssembly(string assemblyName)
        {
            return ImportAssembly(universe.Load(assemblyName));
        }

        /// <inheritdoc />
        public IAssemblySymbol ImportAssembly(Assembly assembly)
        {
            return symbols.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder ImportAssembly(AssemblyBuilder assembly)
        {
            return symbols.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public IModuleSymbol ImportModule(Module module)
        {
            return symbols.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder ImportModule(ModuleBuilder module)
        {
            return symbols.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public ITypeSymbol ImportType(Type type)
        {
            return symbols.GetOrCreateTypeSymbol(type) ;
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder ResolveType(TypeBuilder type)
        {
            return symbols.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IMemberSymbol ImportMember(MemberInfo module)
        {
            return symbols.GetOrCreateMemberSymbol(module);
        }

        /// <inheritdoc />
        public IMethodBaseSymbol ImportMethodBase(MethodBase method)
        {
            return symbols.GetOrCreateMethodBaseSymbol(method);
        }

        /// <inheritdoc />
        public IConstructorSymbol ImportConstructor(ConstructorInfo ctor)
        {
            return symbols.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder ResolveConstructor(ConstructorBuilder ctor)
        {
            return symbols.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IMethodSymbol ImportMethod(MethodInfo method)
        {
            return symbols.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder ResolveMethod(MethodBuilder method)
        {
            return symbols.GetOrCreateMethodSymbol(method);
        }

    }

}
