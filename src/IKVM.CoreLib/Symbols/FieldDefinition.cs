using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class FieldDefinition
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected FieldDefinition(SymbolContext context)
        {

        }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        /// <returns></returns>
        public abstract string GetName();

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        /// <returns></returns>
        public abstract FieldAttributes GetAttributes();

        public abstract object? GetConstantValue();

        public abstract TypeSymbol GetFieldType();

        public abstract ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        public abstract ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}
