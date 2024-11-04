using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a method of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericMethodSymbol : MethodSymbol
    {

        readonly MethodSymbol _definition;
        readonly GenericContext _genericContext;

        TypeSymbol? _returnType;
        ConstructedGenericParameterSymbol? _returnParameter;
        ImmutableList<ParameterSymbol>? _parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericMethodSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType, MethodSymbol definition, GenericContext genericContext) :
            base(context, module, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override bool IsGenericMethod => true;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => false;

        /// <inheritdoc />
        public override bool ContainsGenericParameters => false;

        /// <inheritdoc />
        public override ParameterSymbol ReturnParameter => GetReturnParameter();

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
        public override TypeSymbol ReturnType => _returnType ??= _definition.ReturnType.Specialize(_genericContext);

        /// <inheritdoc />
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => _definition.ReturnTypeCustomAttributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override MethodSymbol GetBaseDefinition()
        {
            return _definition.GetBaseDefinition();
        }

        /// <inheritdoc />
        public override ImmutableList<TypeSymbol> GetGenericArguments()
        {
            return _definition.GetGenericArguments();
        }

        /// <inheritdoc />
        public override MethodSymbol GetGenericMethodDefinition()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _definition.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public override ImmutableList<ParameterSymbol> GetParameters()
        {
            if (_parameters == null)
                Interlocked.CompareExchange(ref _parameters, ComputeGetParameters(), null);

            return _parameters;
        }

        /// <summary>
        /// Computs the value for <see cref="GetParameters"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableList<ParameterSymbol> ComputeGetParameters()
        {
            var b = ImmutableList.CreateBuilder<ParameterSymbol>();
            foreach (var i in _definition.GetParameters())
                b.Add(new ConstructedGenericParameterSymbol(Context, this, i, _genericContext));

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
