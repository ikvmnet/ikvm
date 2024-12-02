using System;
using System.Collections.Immutable;
using System.Threading;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMethodSymbol : DefinitionMethodSymbol
    {

        internal readonly MethodBase _underlyingMethod;

        ImmutableArray<TypeSymbol> _genericArguments;
        IkvmReflectionParameterSymbol? _returnParameter;
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
        public IkvmReflectionMethodSymbol(IkvmReflectionSymbolContext context, IkvmReflectionModuleSymbol module, IkvmReflectionTypeSymbol? declaringType, MethodBase underlyingMethod) :
            base(context, module, declaringType)
        {
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

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
            if (_underlyingMethod.IsConstructor)
                throw new InvalidOperationException();
            if (_returnParameter == null)
                Interlocked.CompareExchange(ref _returnParameter, new IkvmReflectionParameterSymbol(Context, this, ((MethodInfo)_underlyingMethod).ReturnParameter), null);

            return _returnParameter;
        }

        /// <inheritdoc />
        public sealed override System.Reflection.MethodAttributes Attributes => (System.Reflection.MethodAttributes)_underlyingMethod.Attributes;

        /// <inheritdoc />
        public sealed override System.Reflection.CallingConventions CallingConvention => (System.Reflection.CallingConventions)_underlyingMethod.CallingConvention;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodDefinition => _underlyingMethod.IsGenericMethodDefinition;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericMethod => false;

        /// <inheritdoc />
        public sealed override System.Reflection.MethodImplAttributes MethodImplementationFlags => (System.Reflection.MethodImplAttributes)_underlyingMethod.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override TypeSymbol ReturnType => _returnType ??= _underlyingMethod is MethodInfo m ? Context.ResolveTypeSymbol(m.ReturnType) : Context.ResolveCoreType("System.Void");

        /// <inheritdoc />
        public sealed override ICustomAttributeProvider ReturnTypeCustomAttributes => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override bool IsMissing => _underlyingMethod.__IsMissing;

        /// <inheritdoc />
        public override MethodSymbol? BaseDefinition => _underlyingMethod is MethodInfo m ? (_baseDefinition ??= Context.ResolveMethodSymbol(m.GetBaseDefinition())) : this;

        /// <inheritdoc />
        public override MethodSymbol? GenericMethodDefinition => _underlyingMethod.IsGenericMethod ? (_genericMethodDefinition ??= Context.ResolveMethodSymbol(((MethodInfo)_underlyingMethod).GetGenericMethodDefinition())) : null;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericArguments => ComputeGenericArguments();

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
                    if (DeclaringType is IkvmReflectionTypeSymbol dt)
                        b.Add(dt.GetOrCreateGenericMethodParameter(l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _genericArguments, b.DrainToImmutable());
            }

            return _genericArguments;
        }

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
                var l = _underlyingMethod.GetParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new IkvmReflectionParameterSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingMethod.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
