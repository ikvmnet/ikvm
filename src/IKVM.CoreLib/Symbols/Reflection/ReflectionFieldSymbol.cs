using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionFieldSymbol : FieldSymbol
    {

        readonly FieldInfo _underlyingField;

        TypeSymbol? _fieldType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        public ReflectionFieldSymbol(ReflectionSymbolContext context, ModuleSymbol module, ReflectionTypeSymbol? declaringType, FieldInfo underlyingField) :
            base(context, module, declaringType)
        {
            _underlyingField = underlyingField ?? throw new ArgumentNullException(nameof(underlyingField));
        }

        /// <summary>
        /// Gets the associated symbol context.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <summary>
        /// Gets the underlying object.
        /// </summary>
        internal FieldInfo UnderlyingField => _underlyingField;

        /// <inheritdoc />
        public sealed override FieldAttributes Attributes => _underlyingField.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol FieldType => _fieldType ??= Context.ResolveTypeSymbol(_underlyingField.FieldType);

        /// <inheritdoc />
        public sealed override string Name => _underlyingField.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => false;

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue()
        {
            return _underlyingField.GetRawConstantValue();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingField.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingField.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingField.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
