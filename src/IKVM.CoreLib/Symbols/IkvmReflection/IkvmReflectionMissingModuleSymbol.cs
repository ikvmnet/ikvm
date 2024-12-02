using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMissingModuleSymbol : ModuleSymbol
    {

        internal readonly Module _underlyingModule;

        readonly ImmutableArray<IkvmReflectionTypeSymbol>.Builder _types = ImmutableArray.CreateBuilder<IkvmReflectionTypeSymbol>();
        ImmutableArray<TypeSymbol> _typesCache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        public IkvmReflectionMissingModuleSymbol(IkvmReflectionSymbolContext context, IkvmReflectionMissingAssemblySymbol assembly, Module module) :
            base(context, assembly)
        {
            _underlyingModule = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public sealed override string Name => _underlyingModule.Name;

        /// <inheritdoc />
        public sealed override string ScopeName => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public override Guid ModuleVersionId => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public sealed override bool IsMissing => true;

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            if (_typesCache.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _typesCache, _types.ToImmutable().CastArray<TypeSymbol>());

            return _typesCache;
        }

        /// <inheritdoc />
        public sealed override bool IsResource()
        {
            return _underlyingModule.IsResource();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return [];
        }

        /// <summary>
        /// Attachs the missing type to this module.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal TypeSymbol ImportMissingType(IKVM.Reflection.Type type)
        {
            lock (this)
            {
                foreach (var s in _types)
                    if (s._underlyingType == type)
                        return s;

                var symbol = new IkvmReflectionTypeSymbol(Context, this, type);
                _types.Add(symbol);
                _typesCache = default;

                return symbol;
            }
        }

    }

}
