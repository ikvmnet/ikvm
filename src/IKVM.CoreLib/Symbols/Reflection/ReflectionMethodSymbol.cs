using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : DefinitionMethodSymbol
    {

        internal readonly MethodBase _underlyingMethod;

        ImmutableArray<TypeSymbol> _genericArguments;
        ReflectionParameterSymbol? _returnParameter;
        TypeSymbol? _returnType;
        MethodSymbol? _baseDefinition;
        MethodSymbol? _genericMethodDefinition;
        ImmutableArray<ParameterSymbol> _parameters;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        /// <param name="underlyingMethod"></param>
        public ReflectionMethodSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? declaringType, MethodBase underlyingMethod) :
            base(context, module, declaringType)
        {
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <summary>
        /// Gets the underlying object.
        /// </summary>
        internal MethodBase UnderlyingMethod => _underlyingMethod;

        /// <inheritdoc />
        public sealed override string Name => _underlyingMethod.Name;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => GetReturnParameter();

        /// <summary>
        /// Gets the value for <see cref="ReturnParameter"/>;
        /// </summary>
        /// <returns></returns>
        ParameterSymbol GetReturnParameter()
        {
            if (_returnParameter == null)
                Interlocked.CompareExchange(ref _returnParameter, new ReflectionParameterSymbol(Context, this, ((MethodInfo)_underlyingMethod).ReturnParameter), null);

            return _returnParameter;
        }

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _underlyingMethod.Attributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _underlyingMethod.CallingConvention;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => _underlyingMethod.IsGenericMethodDefinition;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _underlyingMethod.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnType ??= Context.ResolveTypeSymbol(((MethodInfo)_underlyingMethod).ReturnType);

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override MethodSymbol? BaseDefinition => _baseDefinition ??= Context.ResolveMethodSymbol(((MethodInfo)_underlyingMethod).GetBaseDefinition());

        /// <inheritdoc />
        public sealed override MethodSymbol? GenericMethodDefinition => _genericMethodDefinition ??= Context.ResolveMethodSymbol(((MethodInfo)_underlyingMethod).GetGenericMethodDefinition());

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

        /// <summary>
        /// Computes the value of <see cref="GenericArguments"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeGenericArguments()
        {
            if (_genericArguments.IsDefault)
            {
                var l = _underlyingMethod.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    if (DeclaringType is ReflectionTypeSymbol dt)
                        b.Add(dt.GetOrCreateGenericMethodParameter(l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _genericArguments, b.DrainToImmutable());
            }

            return _genericArguments;
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> Parameters => ComputeParameters();

        ImmutableArray<ParameterSymbol> ComputeParameters()
        {
            if (_parameters.IsDefault)
            {
                var l = _underlyingMethod.GetParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionParameterSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingMethod.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
