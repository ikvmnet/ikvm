using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MethodSymbol : MethodBaseSymbol
    {

        MethodSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected MethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {
            _specTable = new MethodSpecTable(context, module, declaringType, this, []);
        }

        /// <inheritdoc />
        public override MemberTypes MemberType => MemberTypes.Method;

        /// <summary>
        /// Gets a <see cref="ParameterSymbol"/> object that contains information about the return type of the method, such as whether the return type has custom modifiers.
        /// </summary>
        public abstract ParameterSymbol ReturnParameter { get; }

        /// <summary>
        /// Gets the return type of this method.
        /// </summary>
        public abstract TypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets the custom attributes for the return type.
        /// </summary>
        public abstract ICustomAttributeProvider ReturnTypeCustomAttributes { get; }

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="MethodSymbol"/> object for the method on the direct or indirect base class in which the method represented by this instance was first declared.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol GetBaseDefinition();

        /// <summary>
        /// Returns a <see cref="MethodSymbol"/> object that represents a generic method definition from which the current method can be constructed.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol GetGenericMethodDefinition();

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic method definition, and returns a MethodInfo object representing the resulting constructed method.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public MethodSymbol MakeGenericMethod(ImmutableArray<TypeSymbol> typeArguments) => _specTable.GetOrCreateGenericMethodSymbol(typeArguments);

    }

}
