using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a field of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericFieldSymbol : FieldSymbol
    {

        readonly FieldSymbol _definition;
        readonly GenericContext _genericContext;

        TypeSymbol? _fieldType;
        ImmutableList<TypeSymbol>? _optionalCustomModifiers;
        ImmutableList<TypeSymbol>? _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        public ConstructedGenericFieldSymbol(ISymbolContext context, TypeSymbol declaringType, FieldSymbol definition, GenericContext genericContext) :
            base(context, declaringType.Module, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override FieldAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol FieldType => _fieldType ??= _definition.FieldType.Specialize(_genericContext);

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _definition.GetRawConstantValue();
        }

        /// <inheritdoc />
        public override IImmutableList<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers ??= ComputeOptionalCustomModifiers();
        }

        /// <summary>
        /// Computes the value for <see cref="GetOptionalCustomModifiers"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableList<TypeSymbol> ComputeOptionalCustomModifiers()
        {
            var b = ImmutableList.CreateBuilder<TypeSymbol>();
            foreach (var i in _definition.GetOptionalCustomModifiers())
                b.Add(i.Specialize(_genericContext));

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override IImmutableList<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _optionalCustomModifiers ??= ComputeRequiredCustomModifiers();
        }

        /// <summary>
        /// Computes the value for <see cref="ComputeRequiredCustomModifiers"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableList<TypeSymbol> ComputeRequiredCustomModifiers()
        {
            var b = ImmutableList.CreateBuilder<TypeSymbol>();
            foreach (var i in _definition.GetRequiredCustomModifiers())
                b.Add(i.Specialize(_genericContext));

            return b.ToImmutable();
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