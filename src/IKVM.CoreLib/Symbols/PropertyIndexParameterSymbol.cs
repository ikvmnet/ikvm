namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Base class for all <see cref="ParameterSymbol"/>s returned by <see cref="PropertySymbol.GetIndexParameters()"/>.
    /// </summary>
    abstract class PropertyIndexParameterSymbol : ParameterSymbol
    {

        readonly PropertySymbol _declaringProperty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringProperty"></param>
        /// <param name="position"></param>
        protected PropertyIndexParameterSymbol(SymbolContext context, PropertySymbol declaringProperty, int position) : 
            base(context)
        {
            _declaringProperty = declaringProperty;
        }

    }

}
