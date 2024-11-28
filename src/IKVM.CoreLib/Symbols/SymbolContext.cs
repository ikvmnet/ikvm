namespace IKVM.CoreLib.Symbols
{

    public abstract class SymbolContext
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected SymbolContext()
        {
            DefaultBinder = new DefaultBinder(this);
        }

        /// <summary>
        /// Gets the binder.
        /// </summary>
        internal DefaultBinder DefaultBinder { get; }

        /// <summary>
        /// Resolves the named type from the core assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public abstract TypeSymbol ResolveCoreType(string typeName);

    }

}
