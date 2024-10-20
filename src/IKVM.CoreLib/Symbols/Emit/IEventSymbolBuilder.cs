namespace IKVM.CoreLib.Symbols.Emit
{

    interface IEventSymbolBuilder : ISymbolBuilder<IEventSymbol>, IMemberSymbolBuilder, IEventSymbol
    {

        /// <summary>
        /// Sets the method used to subscribe to this event.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void SetAddOnMethod(IMethodSymbolBuilder mdBuilder);

        /// <summary>
        /// Sets the method used to unsubscribe to this event.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void SetRemoveOnMethod(IMethodSymbolBuilder mdBuilder);

        /// <summary>
        /// Sets the method used to raise this event.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void SetRaiseMethod(IMethodSymbolBuilder mdBuilder);

        /// <summary>
        /// Adds one of the "other" methods associated with this event. "Other" methods are methods other than the "on" and "raise" methods associated with an event. This function can be called many times to add as many "other" methods.
        /// </summary>
        /// <param name="mdBuilder"></param>
        void AddOtherMethod(IMethodSymbolBuilder mdBuilder);

    }

}