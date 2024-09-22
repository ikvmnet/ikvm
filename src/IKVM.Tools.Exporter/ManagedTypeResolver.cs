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

        readonly StaticCompiler compiler;
        readonly Assembly baseAssembly;
        readonly IkvmReflectionSymbolContext symbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="compiler"></param>
        /// <param name="baseAssembly"></param>
        public ManagedTypeResolver(IkvmReflectionSymbolContext symbols, StaticCompiler compiler, Assembly baseAssembly)
        {
            this.symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            this.compiler = compiler ?? throw new ArgumentNullException(nameof(compiler));
            this.baseAssembly = baseAssembly;
        }

        /// <inheritdoc />
        public ISymbolContext Symbols => symbols;

        /// <inheritdoc />
        public IAssemblySymbol? ResolveBaseAssembly()
        {
            return ResolveAssembly(baseAssembly);
        }

        /// <inheritdoc />
        public IAssemblySymbol? ResolveAssembly(string assemblyName)
        {
            return ResolveAssembly(compiler.Load(assemblyName));
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveCoreType(string typeName)
        {
            foreach (var assembly in compiler.Universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return ResolveType(t);

            return null;
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveRuntimeType(string typeName)
        {
            return ResolveType(compiler.GetRuntimeType(typeName));
        }

        /// <inheritdoc />
        public IAssemblySymbol? ResolveAssembly(Assembly? assembly)
        {
            return assembly != null ? symbols.GetOrCreateAssemblySymbol(assembly) : null;
        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder? ResolveAssembly(AssemblyBuilder? assembly)
        {
            return assembly != null ? symbols.GetOrCreateAssemblySymbol(assembly) : null;
        }

        /// <inheritdoc />
        public IModuleSymbol? ResolveModule(Module? module)
        {
            return module != null ? symbols.GetOrCreateModuleSymbol(module) : null;
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder? ResolveModule(ModuleBuilder? module)
        {
            return module != null ? symbols.GetOrCreateModuleSymbol(module) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveType(Type? type)
        {
            return type != null ? symbols.GetOrCreateTypeSymbol(type) : null;
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder? ResolveType(TypeBuilder? type)
        {
            return type != null ? symbols.GetOrCreateTypeSymbol(type) : null;
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(MemberInfo? module)
        {
            return module != null ? symbols.GetOrCreateMemberSymbol(module) : null;
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethodBase(MethodBase? method)
        {
            return method != null ? symbols.GetOrCreateMethodBaseSymbol(method) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbol? ResolveConstructor(ConstructorInfo? ctor)
        {
            return ctor != null ? symbols.GetOrCreateConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IConstructorSymbolBuilder? ResolveConstructor(ConstructorBuilder? ctor)
        {
            return ctor != null ? symbols.GetOrCreateConstructorSymbol(ctor) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? ResolveMethod(MethodInfo? method)
        {
            return method != null ? symbols.GetOrCreateMethodSymbol(method) : null;
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder? ResolveMethod(MethodBuilder? method)
        {
            return method != null ? symbols.GetOrCreateMethodSymbol(method) : null;
        }

    }

}
