using System;
using System.Collections.Concurrent;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionMethodSpecTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol? _type;
        readonly IReflectionMethodSymbol _method;

        ConcurrentDictionary<IReflectionTypeSymbol[], IReflectionMethodSymbol>? _genericMethodSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionMethodSpecTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type, IReflectionMethodSymbol method)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> representing a specialized generic of this method.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public IReflectionMethodSymbol GetOrCreateGenericMethodSymbol(IReflectionTypeSymbol[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_method.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            if (_genericMethodSymbols == null)
                Interlocked.CompareExchange(ref _genericMethodSymbols, new(TypeSymbolListEqualityComparer.Instance), null);

            return _genericMethodSymbols.GetOrAdd(genericTypeArguments, CreateGenericMethodSymbol);
        }

        /// <summary>
        /// Creates a new <see cref="IReflectionMethodSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        readonly IReflectionMethodSymbol CreateGenericMethodSymbol(IReflectionTypeSymbol[] genericTypeArguments)
        {
            return new ReflectionMethodSymbol(_context, _module, _type, _method.UnderlyingMethod.MakeGenericMethod(genericTypeArguments.Unpack()));
        }
    }

}
