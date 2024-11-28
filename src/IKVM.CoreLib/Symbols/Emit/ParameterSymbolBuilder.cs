using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public class ParameterSymbolBuilder : ParameterSymbol, ICustomAttributeBuilder
    {

        readonly TypeSymbol _parameterType;
        readonly int _position;

        internal string? _name;
        internal ParameterAttributes _attributes;
        object? _defaultValue;

        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="position"></param>
        public ParameterSymbolBuilder(SymbolContext context, MemberSymbol declaringMember, TypeSymbol parameterType, int position, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers) :
            base(context, declaringMember, position)
        {
            _parameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            _position = position;
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ParameterAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol ParameterType => _parameterType;

        /// <inheritdoc />
        public sealed override string? Name => _name;

        /// <inheritdoc />
        public sealed override object? DefaultValue => _defaultValue;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

    }

}
