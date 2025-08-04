using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    sealed class IkvmReflectionFieldSymbol : DefinitionFieldSymbol
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly FieldInfo _underlyingField;

        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol> _fieldType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingField"></param>
        public IkvmReflectionFieldSymbol(IkvmReflectionSymbolContext context, FieldInfo underlyingField) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingField = underlyingField ?? throw new ArgumentNullException(nameof(underlyingField));
        }

        /// <summary>
        /// Gets the underlying field.
        /// </summary>
        public FieldInfo UnderlyingField => _underlyingField;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingField.Module)) : _module.Value;

        /// <inheritdoc />
        public sealed override TypeSymbol DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingField.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public sealed override global::System.Reflection.FieldAttributes Attributes => (global::System.Reflection.FieldAttributes)_underlyingField.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol FieldType => _fieldType.IsDefault ? _fieldType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingField.FieldType)) : _fieldType.Value;

        /// <inheritdoc />
        public sealed override string Name => _underlyingField.Name;

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue() => _underlyingField.GetRawConstantValue();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingField.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingField.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingField.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
