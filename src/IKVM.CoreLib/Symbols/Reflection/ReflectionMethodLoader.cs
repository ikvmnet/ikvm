using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionMethodLoader : IMethodLoader
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
        public ReflectionMethodLoader(ReflectionSymbolContext context, MethodBase underlyingMethod)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingMethod = underlyingMethod ?? throw new ArgumentNullException(nameof(underlyingMethod));
        }

        /// <summary>
        /// Gets the underlying method.
        /// </summary>
        public MethodBase UnderlyingMethod => _underlyingMethod;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public ModuleSymbol GetModule() => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingMethod.Module)) : _module.Value;

        /// <inheritdoc />
        public TypeSymbol? GetDeclaringType() => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingMethod.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public string GetName() => _underlyingMethod.Name;

        /// <inheritdoc />
        public ParameterSymbol GetReturnParameter() => _returnParameter.IsDefault ? _returnParameter.InterlockedInitialize(new DefinitionParameterSymbol(_context, new ReflectionParameterLoader(_context, ((MethodInfo)_underlyingMethod).ReturnParameter))) : _returnParameter.Value;

        /// <inheritdoc />
        public MethodAttributes GetAttributes() => _underlyingMethod.Attributes;

        /// <inheritdoc />
        public CallingConventions GetCallingConvention() => _underlyingMethod.CallingConvention;

        /// <inheritdoc />
        public MethodImplAttributes GetMethodImplementationFlags() => _underlyingMethod.MethodImplementationFlags;

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetGenericArguments()
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

        /// <inheritdoc />
        public ImmutableArray<ParameterSymbol> GetParameters()
        {
            if (_parameters.IsDefault)
            {
                var l = _underlyingMethod.GetParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionParameterSymbol(_context, new ReflectionParameterLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _parameters, b.DrainToImmutable());
            }

            return _parameters;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingMethod.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
