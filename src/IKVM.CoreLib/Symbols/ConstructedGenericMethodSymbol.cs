using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a method of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericMethodSymbol : MethodSymbol
    {

        internal readonly MethodSymbol _definition;
        internal readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _typeParameters;
        TypeSymbol? _returnType;
        ConstructedGenericParameterSymbol? _returnParameter;
        ImmutableArray<ParameterSymbol> _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericMethodSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType, MethodSymbol definition, GenericContext genericContext) :
            base(context, module, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => IsConstructedGenericMethod == false && GenericArguments.Length > 0;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => _genericContext.GenericMethodArguments.IsDefault == false;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => GetReturnParameter();

        /// <summary>
        /// Computes the value for <see cref="ReturnParameter"/>.
        /// </summary>
        /// <returns></returns>
        ConstructedGenericParameterSymbol GetReturnParameter()
        {
            if (_returnParameter == null)
                Interlocked.CompareExchange(ref _returnParameter, new ConstructedGenericParameterSymbol(Context, this, _definition.ReturnParameter, _genericContext), null);

            return _returnParameter;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnType ??= _definition.ReturnType.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => _definition.ReturnTypeCustomAttributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => _definition.BaseDefinition;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_typeParameters.IsDefault)
            {
                var l = _definition.GenericArguments;
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

        ImmutableArray<ParameterSymbol> ComputeParameters()
        {
            if (_parameters.IsDefault)
            {
                var l = _definition.Parameters;
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ConstructedGenericParameterSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

        public override string? ToString()
        {
            return base.ToString();
        }

    }

}
