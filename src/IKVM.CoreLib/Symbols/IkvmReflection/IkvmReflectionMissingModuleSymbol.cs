using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    sealed class IkvmReflectionMissingModuleSymbol : DefinitionModuleSymbol
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Module _underlyingModule;

        readonly ImmutableArray<TypeSymbol>.Builder _types = ImmutableArray.CreateBuilder<TypeSymbol>();
        ImmutableArray<TypeSymbol> _typesCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingModule"></param>
        public IkvmReflectionMissingModuleSymbol(IkvmReflectionSymbolContext context, Module underlyingModule) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingModule = underlyingModule ?? throw new ArgumentNullException(nameof(underlyingModule));
        }

        /// <summary>
        /// Gets the underlying module.
        /// </summary>
        public Module UnderlyingModule => _underlyingModule;

        /// <inheritdoc />
        public sealed override bool IsMissing => true;

        /// <inheritdoc />
        public sealed override AssemblySymbol Assembly => _context.ResolveAssemblySymbol(_underlyingModule.Assembly);

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public sealed override string Name => _underlyingModule.Name;

        /// <inheritdoc />
        public sealed override string ScopeName => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public sealed override Guid ModuleVersionId => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public sealed override bool IsResource() => _underlyingModule.IsResource();

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields() => [];

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods() => [];

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            if (_typesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typesCache, _types.ToImmutable().CastArray<TypeSymbol>());

            return _typesCache;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => [];

        /// <summary>
        /// Attachs the missing type to this module.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal TypeSymbol ImportMissingType(IKVM.Reflection.Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            lock (this)
            {
                // check if the type already exists
                foreach (var s in _types)
                    if (s is IkvmReflectionTypeSymbol t)
                        if (t.UnderlyingType == type)
                            return s;

                var symbol = new IkvmReflectionTypeSymbol(_context, type);
                _types.Add(symbol);
                _typesCache = default;

                return symbol;
            }
        }

    }

}
