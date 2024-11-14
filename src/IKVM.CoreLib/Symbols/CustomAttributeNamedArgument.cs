using System.Runtime.InteropServices;

namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttributeNamedArgument(
        bool IsField,
        MemberSymbol MemberInfo,
        string MemberName,
        CustomAttributeTypedArgument TypedValue)
    {

        /// <inheritdoc />
        public override string ToString()
        {
            if (MemberInfo is null)
                return base.ToString()!;

            return $"{MemberInfo.Name} = {TypedValue.ToString(ArgumentType != ArgumentType.Context.ResolveCoreType("System.Object"))}";
        }

        /// <summary>
        /// Gets the type of the argument.
        /// </summary>
        internal TypeSymbol ArgumentType => MemberInfo is FieldSymbol fi ? fi.FieldType : ((PropertySymbol)MemberInfo).PropertyType;

    }

}
