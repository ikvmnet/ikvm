namespace IKVM.CoreLib.Symbols
{

    abstract class TypeSymbol : NamespaceOrTypeSymbol, ITypeSymbol
    {

        /// <summary>
        /// The original definition of this symbol. If this symbol is constructed from another
        /// symbol by type substitution then OriginalDefinition gets the original symbol as it was defined in
        /// source or metadata.
        /// </summary>
        public new TypeSymbol OriginalDefinition
        {
            get
            {
                return OriginalTypeSymbolDefinition;
            }
        }

        protected virtual TypeSymbol OriginalTypeSymbolDefinition
        {
            get
            {
                return this;
            }
        }

        protected sealed override Symbol OriginalSymbolDefinition
        {
            get
            {
                return this.OriginalTypeSymbolDefinition;
            }
        }


    }

}