using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    struct MethodSpecTable
    {

        readonly ISymbolContext _context;
        readonly IModuleSymbol _module;
        readonly TypeSymbol? _type;
        readonly MethodSymbol _method;
        readonly ImmutableList<TypeSymbol> _typeArguments;

        ConcurrentDictionary<ImmutableList<TypeSymbol>, ConstructedGenericMethodSymbol>? _cache;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="typeArguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MethodSpecTable(ISymbolContext context, IModuleSymbol module, TypeSymbol? type, MethodSymbol method, ImmutableList<TypeSymbol> typeArguments)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _typeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }

        /// <summary>
        /// Gets or creates a <see cref="MethodSymbol"/> representing a specialized generic of this method.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public MethodSymbol GetOrCreateGenericMethodSymbol(ImmutableList<TypeSymbol> typeArguments)
        {
            if (typeArguments is null)
                throw new ArgumentNullException(nameof(typeArguments));

            if (_method.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            if (_cache == null)
                Interlocked.CompareExchange(ref _cache, new(TypeSymbolListEqualityComparer.Instance), null);

            return _cache.GetOrAdd(typeArguments, CreateGenericMethodSymbol);
        }

        /// <summary>
        /// Creates a new <see cref="MethodSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        readonly ConstructedGenericMethodSymbol CreateGenericMethodSymbol(ImmutableList<TypeSymbol> typeArguments)
        {
            return new ConstructedGenericMethodSymbol(_context, _module, _type, _method, new GenericContext(_typeArguments, typeArguments));
        }
    }

}
