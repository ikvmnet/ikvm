using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a property of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericPropertySymbol : PropertySymbol
    {

        readonly PropertySymbol _definition;
        readonly GenericContext _genericContext;

        ImmutableArray<ParameterSymbol> _indexParameters;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="typeArguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericPropertySymbol(SymbolContext context, TypeSymbol declaringType, PropertySymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol PropertyType => _definition.PropertyType.Specialize(_genericContext);

        /// <inheritdoc />
        public override bool CanRead => _definition.CanRead;

        /// <inheritdoc />
        public override bool CanWrite => _definition.CanWrite;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>();

            foreach (var baseMethod in _definition.GetAccessors(nonPublic))
            {
                if (baseMethod is not null)
                    foreach (var i in DeclaringType!.GetMethods())
                        if (i is ConstructedGenericMethodSymbol m)
                            if (m._definition == baseMethod)
                                b.Add(m);
            }

            return b.DrainToImmutable();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            var baseMethod = _definition.GetGetMethod(nonPublic);
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is ConstructedGenericMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            var baseMethod = _definition.GetSetMethod(nonPublic);
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is ConstructedGenericMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters == default)
            {
                var l = _definition.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ConstructedGenericParameterSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _indexParameters, b.DrainToImmutable());
            }

            return _indexParameters;
        }

        /// <inheritdoc />
        public override TypeSymbol GetModifiedPropertyType()
        {
            return _definition.GetModifiedPropertyType().Specialize(_genericContext);
        }

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _definition.GetRawConstantValue();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
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
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
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
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}