using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a property of a <see cref="SpecializedTypeSymbol"/>.
    /// </summary>
    class SpecializedPropertySymbol : PropertySymbol
    {

        readonly TypeSymbol _declaringType;
        readonly PropertySymbol _definition;
        readonly GenericContext _genericContext;

        LazyField<TypeSymbol> _propertyType;
        ImmutableArray<ParameterSymbol> _indexParameters;
        LazyField<MethodSymbol?> _getMethod;
        LazyField<MethodSymbol?> _setMethod;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpecializedPropertySymbol(SymbolContext context, TypeSymbol declaringType, PropertySymbol definition, GenericContext genericContext) :
            base(context)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override PropertyAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol PropertyType => _propertyType.IsDefault ? _propertyType.InterlockedInitialize(ComputePropertyType()) : _propertyType.Value;

        /// <summary>
        /// Computes the value for <see cref="PropertyType"/>.
        /// </summary>
        /// <returns></returns>
        TypeSymbol ComputePropertyType()
        {
            return _definition.PropertyType.Specialize(_genericContext);
        }

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetMethod => _getMethod.IsDefault ? _getMethod.InterlockedInitialize(ComputeGetMethod()) : _getMethod.Value;

        /// <summary>
        /// Computes the value for <see cref="GetMethod"/>.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? ComputeGetMethod()
        {
            var baseMethod = _definition.GetMethod;
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is SpecializedMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? SetMethod => _getMethod.IsDefault ? _setMethod.InterlockedInitialize(ComputeSetMethod()) : _setMethod.Value;

        /// <summary>
        /// Computes the value for <see cref="SetMethod"/>.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? ComputeSetMethod()
        {
            var baseMethod = _definition.SetMethod;
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is SpecializedMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters.IsDefault)
            {
                var l = _definition.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedParameterSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _indexParameters, b.DrainToImmutable());
            }

            return _indexParameters;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetModifiedPropertyType()
        {
            return _definition.GetModifiedPropertyType().Specialize(_genericContext);
        }

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
