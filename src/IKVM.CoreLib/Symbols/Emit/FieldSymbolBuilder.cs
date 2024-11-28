using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class FieldSymbolBuilder : FieldSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        readonly FieldAttributes _attributes;
        readonly TypeSymbol _fieldType;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        object? _constantValue;
        int? _offset;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="fieldType"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        internal FieldSymbolBuilder(SymbolContext context, ModuleSymbol declaringModule, TypeSymbolBuilder? declaringType, string name, FieldAttributes attributes, TypeSymbol fieldType, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers) :
            base(context, declaringModule, declaringType)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _fieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override FieldAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override TypeSymbol FieldType => _fieldType;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => false;

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _constantValue;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Sets the default value of this field.
        /// </summary>
        /// <param name="defaultValue"></param>
        public void SetConstant(object? defaultValue)
        {
            _constantValue = default;
        }

        /// <summary>
        /// Specifies the field layout.
        /// </summary>
        /// <param name="iOffset"></param>
        public void SetOffset(int iOffset)
        {
            _offset = iOffset;
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

    }

}
