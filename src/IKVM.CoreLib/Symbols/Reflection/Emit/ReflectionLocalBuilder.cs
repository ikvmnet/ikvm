using System;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionLocalBuilder : ILocalBuilder
    {

        readonly ReflectionSymbolContext _context;
        readonly LocalBuilder _builder;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionLocalBuilder(ReflectionSymbolContext context, LocalBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the underlying <see cref="LocalBuilder"/>.
        /// </summary>
        public LocalBuilder UnderlyingLocalBuilder => _builder;

        /// <inheritdoc />
        public bool IsPinned => _builder.IsPinned;

        /// <inheritdoc />
        public int LocalIndex => _builder.LocalIndex;

        /// <inheritdoc />
        public ITypeSymbol LocalType => _context.GetOrCreateTypeSymbol(_builder.LocalType);

        /// <inheritdoc />
        public void SetLocalSymInfo(string name)
        {
#if NETFRAMEWORK
            _builder.SetLocalSymInfo(name);
#endif
        }

        /// <inheritdoc />
        public void SetLocalSymInfo(string name, int startOffset, int endOffset)
        {
#if NETFRAMEWORK
            _builder.SetLocalSymInfo(name, startOffset, endOffset);
#endif
        }

    }

}