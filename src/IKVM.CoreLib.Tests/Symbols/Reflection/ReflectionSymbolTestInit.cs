using System.Threading;

using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.CoreLib.Tests.Symbols.Reflection
{

    public class ReflectionSymbolTestInit : SymbolTestInit<ReflectionSymbolContext>
    {

        ReflectionSymbolContext? _symbols;

        /// <summary>
        /// Gets the symbol context.
        /// </summary>
        public override ReflectionSymbolContext Symbols
        {
            get
            {
                if (_symbols == null)
                    Interlocked.CompareExchange(ref _symbols, new ReflectionSymbolContext(typeof(object).Assembly, new ReflectionSymbolOptions(true)), null);

                return _symbols;
            }
        }

    }

}
