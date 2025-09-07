using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    sealed class ReflectionMethodSymbol : DefinitionMethodSymbol
    {

        readonly ReflectionSymbolContext _context;
        readonly MethodBase _underlyingMethod;

        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol?> _declaringType;
        ImmutableArray<TypeSymbol> _genericArguments;
        LazyField<ParameterSymbol> _returnParameter;
        ImmutableArray<ParameterSymbol> _parameters;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingMethod"></param>
        public ReflectionMethodSymbol(ReflectionSymbolContext context, MethodBase underlyingMethod) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets the underlying method.
        /// </summary>
        public MethodBase UnderlyingMethod => _underlyingMethod;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingMethod.Module)) : _module.Value;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingMethod.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public sealed override string Name => _underlyingMethod.Name;

        /// <inheritdoc />
        public sealed override ParameterSymbol ReturnParameter => _returnParameter.IsDefault ? _returnParameter.InterlockedInitialize(new ReflectionParameterSymbol(_context, ((MethodInfo)_underlyingMethod).ReturnParameter)) : _returnParameter.Value;

        /// <inheritdoc />
        public sealed override MethodAttributes Attributes => _underlyingMethod.Attributes;

        /// <inheritdoc />
        public sealed override CallingConventions CallingConvention => _underlyingMethod.CallingConvention;

        /// <inheritdoc />
        public sealed override MethodImplAttributes MethodImplementationFlags => _underlyingMethod.MethodImplementationFlags;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameters
        {
            get
            {
                if (_genericArguments.IsDefault)
                {
                    var l = _underlyingMethod.GetGenericArguments();
                    var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                    foreach (var i in l)
                        b.Add(_context.ResolveTypeSymbol(i));

                    ImmutableInterlocked.InterlockedInitialize(ref _genericArguments, b.DrainToImmutable());
                }

                return _genericArguments;
            }
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> Parameters
        {
            get
            {
                if (_parameters.IsDefault)
                {
                    var l = _underlyingMethod.GetParameters();
                    var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                    foreach (var i in l)
                        b.Add(new ReflectionParameterSymbol(_context, i));

                    ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
                }

                return _parameters;
            }
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingMethod.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
