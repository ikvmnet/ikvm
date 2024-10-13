using System;
using System.Collections.Concurrent;
using System.Threading;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionMethodSpecTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol? _type;
        readonly IIkvmReflectionMethodSymbol _method;

        ConcurrentDictionary<IIkvmReflectionTypeSymbol[], IIkvmReflectionMethodSymbol>? _genericMethodSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionMethodSpecTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type, IIkvmReflectionMethodSymbol method)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> representing a specialized generic of this method.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbol GetOrCreateGenericMethodSymbol(Type[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_method.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            if (_genericMethodSymbols == null)
                Interlocked.CompareExchange(ref _genericMethodSymbols, new(TypeSymbolListEqualityComparer.Instance), null);

            return _genericMethodSymbols.GetOrAdd(_module.ResolveTypeSymbols(genericTypeArguments), CreateGenericMethodSymbol);
        }

        /// <summary>
        /// Creates a new <see cref="IIkvmReflectionMethodSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        readonly IIkvmReflectionMethodSymbol CreateGenericMethodSymbol(IIkvmReflectionTypeSymbol[] genericTypeArguments)
        {
            return new IkvmReflectionMethodSymbol(_context, _module, _type, _method.UnderlyingMethod.MakeGenericMethod(genericTypeArguments.Unpack()));
        }
    }

}
