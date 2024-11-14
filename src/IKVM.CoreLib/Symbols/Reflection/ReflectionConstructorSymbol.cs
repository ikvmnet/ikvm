using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionConstructorSymbol : ConstructorSymbol
    {

        readonly ConstructorInfo _underlyingConstructor;

        ImmutableArray<ParameterSymbol> _parameters;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="underlyingConstructor"></param>
        public ReflectionConstructorSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol declaringType, ConstructorInfo underlyingConstructor) :
            base(context, declaringType)
        {
            _underlyingConstructor = underlyingConstructor ?? throw new ArgumentNullException(nameof(underlyingConstructor));
        }

        /// <summary>
        /// Gets the associated symbol context.
        /// </summary>
        protected new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public override MethodAttributes Attributes => _underlyingConstructor.Attributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _underlyingConstructor.CallingConvention;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => _underlyingConstructor.MethodImplementationFlags;

        /// <inheritdoc />
        public override string Name => _underlyingConstructor.Name;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => false;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _underlyingConstructor.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetParameters()
        {
            if (_parameters == default)
            {
                var l = _underlyingConstructor.GetParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>();
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionParameterSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.ToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingConstructor.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
