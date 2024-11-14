using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodSymbol : DefinitionMethodSymbol
    {

        readonly MethodInfo _underlyingMethod;

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
        public ReflectionMethodSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, ReflectionTypeSymbol? declaringType, MethodInfo underlyingMethod) :
            base(context, module, declaringType)
        {
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public override string Name => _underlyingMethod.Name;

        /// <inheritdoc />
        public override ParameterSymbol ReturnParameter => GetReturnParameter();

        /// <summary>
        /// Gets the value for <see cref="ReturnParameter"/>;
        /// </summary>
        /// <returns></returns>
        ParameterSymbol GetReturnParameter()
        {
            if (_returnParameter == null)
                Interlocked.CompareExchange(ref _returnParameter, new ReflectionParameterSymbol(Context, this, _underlyingMethod.ReturnParameter), null);

            return _returnParameter;
        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => _underlyingMethod.Attributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _underlyingMethod.CallingConvention;

        /// <inheritdoc />
        public override bool ContainsGenericParameters => _underlyingMethod.ContainsGenericParameters;

        /// <inheritdoc />
        public override bool IsGenericMethod => _underlyingMethod.IsGenericMethod;

        /// <inheritdoc />
        public override bool IsGenericMethodDefinition => _underlyingMethod.IsGenericMethodDefinition;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => _underlyingMethod.MethodImplementationFlags;

        /// <inheritdoc />
        public override TypeSymbol ReturnType => _returnType ??= Context.ResolveTypeSymbol(_underlyingMethod.ReturnType);

        /// <inheritdoc />
        public override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => true;

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _underlyingMethod.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public override MethodSymbol GetBaseDefinition()
        {
            return _baseDefinition ??= Context.ResolveMethodSymbol(_underlyingMethod.GetBaseDefinition());
        }

        /// <inheritdoc />
        public override MethodSymbol GetGenericMethodDefinition()
        {
            return _genericMethodDefinition ??= Context.ResolveMethodSymbol(_underlyingMethod.GetGenericMethodDefinition());
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            if (_genericArguments == default)
            {
                var l = _underlyingMethod.GetGenericArguments();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>();
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionGenericMethodParameterTypeSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _genericArguments, b.ToImmutable());
            }

            return _genericArguments;
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetParameters()
        {
            if (_parameters == default)
            {
                var l = _underlyingMethod.GetParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>();
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionParameterSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.ToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingMethod.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
