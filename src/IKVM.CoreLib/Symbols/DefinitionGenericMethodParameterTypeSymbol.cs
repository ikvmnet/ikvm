using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionGenericMethodParameterTypeSymbol : GenericMethodParameterTypeSymbol
    {

        readonly IGenericMethodParameterTypeLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionGenericMethodParameterTypeSymbol(SymbolContext context, IGenericMethodParameterTypeLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => _loader.GetDeclaringMethod();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _loader.GetGenericParameterPosition();

        /// <inheritdoc />
        public sealed override bool ContainsMissingType => GenericParameterConstraints.Any(i => i.ContainsMissingType);

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _loader.GetGenericParameterAttributes();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => _loader.GetGenericParameterConstraints();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
