using System;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Reflection-specific implementation of <see cref="ISymbolBuilder"/>.
    /// </summary>
    abstract class ReflectionSymbolBuilder : ISymbolBuilder
    {

        readonly ReflectionSymbolContext _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReflectionSymbolBuilder(ReflectionSymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the associated <see cref="ReflectionSymbolContext"/>.
        /// </summary>
        public ReflectionSymbolContext Context => _context;

        /// <inheritdoc />
        public ISymbol Symbol => GetSymbol();

        /// <summary>
        /// Implement to get the symbol.
        /// </summary>
        /// <returns></returns>
        protected abstract ISymbol GetSymbol();

    }

    /// <summary>
    /// Reflection-specific implementation of <see cref="ISymbolBuilder{TSymbol}"/>.
    /// </summary>
    abstract class ReflectionSymbolBuilder<TSymbol> : ReflectionSymbolBuilder, ISymbolBuilder<TSymbol>
        where TSymbol : ISymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReflectionSymbolBuilder(ReflectionSymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public abstract new TSymbol Symbol { get; }

        /// <inheritdoc />
        protected sealed override ISymbol GetSymbol() => Symbol;

    }

    /// <summary>
    /// Base implementation for reflection symbol builders.
    /// </summary>
    abstract class ReflectionSymbolBuilder<TSymbol, TReflectionSymbol> : ReflectionSymbolBuilder<TSymbol>
        where TSymbol : ISymbol
        where TReflectionSymbol : ReflectionSymbol, TSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReflectionSymbolBuilder(ReflectionSymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override TSymbol Symbol => ReflectionSymbol;

        /// <summary>
        /// Gets the <see cref="TReflectionSymbol"/> that is currently being built. Portions of this interface will be non-functional until the build is completed.
        /// </summary>
        internal abstract TReflectionSymbol ReflectionSymbol { get; }

    }

}
