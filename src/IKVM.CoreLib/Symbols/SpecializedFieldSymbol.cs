using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a field of a <see cref="SpecializedTypeSymbol"/>.
    /// </summary>
    class SpecializedFieldSymbol : FieldSymbol
    {

        readonly TypeSymbol? _declaringType;
        readonly FieldSymbol _definition;
        readonly GenericContext _genericContext;

        LazyField<TypeSymbol> _fieldType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        public SpecializedFieldSymbol(SymbolContext context, TypeSymbol? declaringType, FieldSymbol definition, GenericContext genericContext) :
            base(context)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _definition.Module;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override FieldAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol FieldType => _fieldType.IsDefault ? _fieldType.InterlockedInitialize(ComputeFieldType()) : _fieldType.Value;

        /// <summary>
        /// Computes the value for <see cref="FieldType"/>.
        /// </summary>
        /// <returns></returns>
        TypeSymbol ComputeFieldType() => _definition.FieldType.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue()
        {
            return _definition.GetRawConstantValue();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
            {
                var l = _definition.GetOptionalCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, b.DrainToImmutable());
            }

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
            {
                var l = _definition.GetRequiredCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, b.DrainToImmutable());
            }

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

    }

}