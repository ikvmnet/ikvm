using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionPropertySymbol : PropertySymbol
    {

        readonly IPropertyLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionPropertySymbol(SymbolContext context, IPropertyLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public IPropertyLoader Loader => _loader;

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override PropertyAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public sealed override TypeSymbol PropertyType => _loader.GetPropertyType();

        /// <inheritdoc />
        public sealed override MethodSymbol? GetMethod => _loader.GetGetMethod();

        /// <inheritdoc />
        public sealed override MethodSymbol? SetMethod => _loader.GetSetMethod();

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> GetIndexParameters() => _loader.GetIndexParameters();

        /// <inheritdoc />
        public sealed override TypeSymbol GetModifiedPropertyType() => _loader.GetModifiedPropertyType();

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue() => _loader.GetRawConstantValue();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
