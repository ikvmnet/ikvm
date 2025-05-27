using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionGenericTypeParameterTypeSymbol : GenericTypeParameterTypeSymbol
    {

        readonly IGenericTypeParameterTypeLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionGenericTypeParameterTypeSymbol(SymbolContext context, IGenericTypeParameterTypeLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _loader.GetGenericParameterAttributes();

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _loader.GetGenericParameterPosition();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => _loader.GetGenericParameterConstraints();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
