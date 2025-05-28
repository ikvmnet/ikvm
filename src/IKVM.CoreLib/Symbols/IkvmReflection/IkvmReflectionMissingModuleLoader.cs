using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMissingModuleLoader : IModuleLoader
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly AssemblySymbol _assembly;
        readonly Module _underlyingModule;

        readonly ImmutableArray<TypeSymbol>.Builder _types = ImmutableArray.CreateBuilder<TypeSymbol>();
        ImmutableArray<TypeSymbol> _typesCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="underlyingModule"></param>
        public IkvmReflectionMissingModuleLoader(IkvmReflectionSymbolContext context, AssemblySymbol assembly, Module underlyingModule)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _underlyingModule = underlyingModule ?? throw new ArgumentNullException(nameof(underlyingModule));
        }

        /// <summary>
        /// Gets the underlying module.
        /// </summary>
        public Module UnderlyingModule => _underlyingModule;

        /// <inheritdoc />
        public bool GetIsMissing() => true;

        /// <inheritdoc />
        public AssemblySymbol GetAssembly() => _assembly;

        /// <inheritdoc />
        public string GetFullyQualifiedName() => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public string GetName() => _underlyingModule.Name;

        /// <inheritdoc />
        public string GetScopeName() => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public Guid GetModuleVersionId() => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public bool GetIsResource() => _underlyingModule.IsResource();

        /// <inheritdoc />
        public ImmutableArray<FieldSymbol> GetFields() => [];

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetMethods() => [];

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetTypes()
        {
            if (_typesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typesCache, _types.ToImmutable().CastArray<TypeSymbol>());

            return _typesCache;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes() => [];

        /// <summary>
        /// Attachs the missing type to this module.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal TypeSymbol ImportMissingType(IKVM.Reflection.Type type)
        {
            lock (this)
            {
                // check if the type already exists
                foreach (var s in _types)
                    if (s is DefinitionTypeSymbol def && def.Loader is IkvmReflectionTypeLoader loader)
                        if (loader.UnderlyingType == type)
                            return s;

                var symbol = new DefinitionTypeSymbol(_context, new IkvmReflectionTypeLoader(_context, type));
                _types.Add(symbol);
                _typesCache = default;

                return symbol;
            }
        }

    }

}
