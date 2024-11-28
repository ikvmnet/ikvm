using System.Collections.Generic;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    public readonly record struct CustomAttributeTypedArgument(TypeSymbol ArgumentType, object? Value)
    {

        public override string ToString() => ToString(false);

        /// <summary>
        /// Returns a string reprsentation of this typed argument.
        /// </summary>
        /// <param name="typed"></param>
        /// <returns></returns>
        internal string ToString(bool typed)
        {
            if (ArgumentType is null)
                return base.ToString()!;

            if (ArgumentType.IsEnum)
                return typed ? $"{Value}" : $"({ArgumentType.FullName}){Value}";

            if (Value is null)
                return typed ? "null" : $"({ArgumentType.Name})null";

            if (ArgumentType == ArgumentType.Context.ResolveCoreType("System.String"))
                return $"\"{Value}\"";

            if (ArgumentType == ArgumentType.Context.ResolveCoreType("System.Char"))
                return $"'{Value}'";

            if (ArgumentType == ArgumentType.Context.ResolveCoreType("System.Type"))
                return $"typeof({((TypeSymbol)Value!).FullName})";

            if (ArgumentType.IsArray)
            {
                var array = (IReadOnlyList<CustomAttributeTypedArgument>)Value!;
                var elementType = ArgumentType.GetElementType()!;

                using var result = new ValueStringBuilder(stackalloc char[256]);
                result.Append("new ");
                result.Append(elementType.IsEnum ? elementType.FullName : elementType.Name);
                result.Append('[');
                var count = array.Count;
                result.Append(count.ToString());
                result.Append("] { ");

                for (int i = 0; i < count; i++)
                {
                    if (i != 0)
                        result.Append(", ");

                    result.Append(array[i].ToString(elementType != ArgumentType.Context.ResolveCoreType("System.Object")));
                }

                result.Append(" }");

                return result.ToString();
            }

            return typed ? $"{Value}" : $"({ArgumentType.Name}){Value}";
        }

    }

}
