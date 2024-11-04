using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class ConstructedGenericParameterSymbol : ParameterSymbol
    {

        readonly ParameterSymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericParameterSymbol(ISymbolContext context, MemberSymbol declaringMember, ParameterSymbol definition, GenericContext genericContext) :
            base(context, declaringMember, definition.Position)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _definition.ParameterType.Specialize(_genericContext);

        /// <inheritdoc />
        public override string? Name => _definition.Name;

        /// <inheritdoc />
        public override object? DefaultValue => _definition.DefaultValue;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override ImmutableList<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _definition.GetOptionalCustomModifiers();
        }

        /// <inheritdoc />
        public override ImmutableList<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _definition.GetRequiredCustomModifiers();
        }

        /// <inheritdoc />
        public override CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttribute(attributeType, inherit);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return _definition.GetCustomAttributes(inherit);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttributes(attributeType, inherit);
        }

        /// <inheritdoc />
        public override bool IsDefined(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.IsDefined(attributeType, inherit);
        }

    }

}
