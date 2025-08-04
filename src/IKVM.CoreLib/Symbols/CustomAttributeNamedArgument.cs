namespace IKVM.CoreLib.Symbols
{

    public readonly record struct CustomAttributeNamedArgument(MemberSymbol MemberInfo, CustomAttributeTypedArgument TypedValue)
    {

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        internal TypeSymbol ArgumentType => MemberInfo is FieldSymbol fi ? fi.FieldType : ((PropertySymbol)MemberInfo).PropertyType;

        /// <inheritdoc />
        public override string? ToString()
        {
            return $"{MemberInfo.Name} = {TypedValue.ToString(ArgumentType != ArgumentType.Context.ResolveCoreType("System.Object"))}";
        }

    }

}
