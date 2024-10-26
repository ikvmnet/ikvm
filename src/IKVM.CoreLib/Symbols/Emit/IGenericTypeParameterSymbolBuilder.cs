namespace IKVM.CoreLib.Symbols.Emit
{

    interface IGenericTypeParameterSymbolBuilder : ISymbolBuilder<ITypeSymbol>, ITypeSymbol
    {

        /// <summary>
        /// Sets the base type that a type must inherit in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="baseTypeConstraint"></param>
        void SetBaseTypeConstraint(ITypeSymbol? baseTypeConstraint);

        /// <summary>
        /// Sets the variance characteristics and special constraints of the generic parameter, such as the parameterless constructor constraint.
        /// </summary>
        /// <param name="genericParameterAttributes"></param>
        void SetGenericParameterAttributes(System.Reflection.GenericParameterAttributes genericParameterAttributes);

        /// <summary>
        /// Sets the interfaces a type must implement in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="interfaceConstraints"></param>
        void SetInterfaceConstraints(params ITypeSymbol[] interfaceConstraints);


    }

}
