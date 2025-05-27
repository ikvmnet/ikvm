using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a method of a <see cref="SpecializedTypeSymbol"/>.
    /// </summary>
    class SpecializedMethodSymbol : MethodSymbol
    {

        readonly TypeSymbol? _declaringType;
        internal readonly MethodSymbol _definition;
        readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _typeParameters;
        LazyField<SpecializedParameterSymbol> _returnParameter;
        ImmutableArray<ParameterSymbol> _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpecializedMethodSymbol(SymbolContext context, TypeSymbol? declaringType, MethodSymbol definition, GenericContext genericContext) :
            base(context)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(definition));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override ModuleSymbol Module => _definition.Module;

        /// <inheritdoc />
        public override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => IsConstructedGenericMethod == false && GenericParameters.Length > 0;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => _genericContext.GenericMethodArguments.IsDefault == false;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _returnParameter.IsDefault ? _returnParameter.InterlockedInitialize(ComputeReturnParameter()) : _returnParameter.Value;

        /// <summary>
        /// Computes the value for <see cref="ReturnParameter"/>.
        /// </summary>
        /// <returns></returns>
        SpecializedParameterSymbol ComputeReturnParameter() => new SpecializedParameterSymbol(Context, this, _definition.ReturnParameter, _genericContext);

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => _definition.BaseDefinition;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericParameters => ComputeGenericArguments();

        /// <summary>
        /// Computes the values for <see cref="GenericParameters"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_typeParameters.IsDefault)
            {
                var l = _definition.GenericParameters;
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _typeParameters, b.DrainToImmutable());
            }

            return _typeParameters;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol GenericMethodDefinition => _definition;

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> Parameters => ComputeParameters();

        /// <summary>
        /// Computes the value for <see cref="Parameters"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<ParameterSymbol> ComputeParameters()
        {
            if (_parameters.IsDefault)
            {
                var l = _definition.Parameters;
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedParameterSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

    }

}
