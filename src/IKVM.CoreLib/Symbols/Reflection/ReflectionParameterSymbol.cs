using System;
using System.Linq;
using System.Reflection;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : ReflectionSymbol, IParameterSymbol
    {

        readonly ReflectionMethodBaseSymbol _containingMethod;
        ParameterInfo? _parameter;
        ReflectionParameterSymbolBuilder? _builder;

        CustomAttribute[]? _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingMethod"></param>
        /// <param name="parameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, ReflectionMethodBaseSymbol containingMethod, ParameterInfo parameter) :
            base(context)
        {
            _containingMethod = containingMethod ?? throw new ArgumentNullException(nameof(containingMethod));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingMethod"></param>
        /// <param name="builder"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, ReflectionMethodBaseSymbol containingMethod, ReflectionParameterSymbolBuilder builder) :
            base(context)
        {
            _containingMethod = containingMethod ?? throw new ArgumentNullException(nameof(containingMethod));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        internal ReflectionMethodBaseSymbol ContainingMethod => _containingMethod;

        /// <inheritdoc />
        public ParameterAttributes Attributes => _parameter is { } p ? p.Attributes : _builder is { } b ? (ParameterAttributes)b.ReflectionBuilder.Attributes : throw new InvalidOperationException();

        /// <inheritdoc />
        public object? DefaultValue => _parameter is { } p ? p.DefaultValue : _builder is { } b ? b.GetConstant() : throw new InvalidOperationException();

        /// <inheritdoc />
        public bool HasDefaultValue => (Attributes & ParameterAttributes.HasDefault) != 0;

        /// <inheritdoc />
        public bool IsIn => (Attributes & ParameterAttributes.In) != 0;

        /// <inheritdoc />
        public bool IsLcid => (Attributes & ParameterAttributes.Lcid) != 0;

        /// <inheritdoc />
        public bool IsOptional => (Attributes & ParameterAttributes.Optional) != 0;

        /// <inheritdoc />
        public bool IsOut => (Attributes & ParameterAttributes.Out) != 0;

        /// <inheritdoc />
        public bool IsRetval => (Attributes & ParameterAttributes.Retval) != 0;

        /// <inheritdoc />
        public IMemberSymbol Member => _parameter is { } p ? ResolveMemberSymbol(p.Member) : _containingMethod;

#if NETFRAMEWORK

        /// <inheritdoc />
        public int MetadataToken => _parameter is { } p ? p.MetadataToken : _builder is { } b ? b.ReflectionBuilder.GetToken().Token : throw new InvalidOperationException();

#else

        /// <inheritdoc />
        public int MetadataToken => _parameter is { } p ? p.MetadataToken : _builder is { } b ? throw new NotSupportedException() : throw new InvalidOperationException();

#endif

        /// <inheritdoc />
        public string? Name => _parameter is { } p ? p.Name : _builder is { } b ? b.ReflectionBuilder.Name : throw new InvalidOperationException();

        /// <inheritdoc />
        public ITypeSymbol ParameterType => _parameter is { } p ? ResolveTypeSymbol(p.ParameterType) : _builder is { } b ? throw new NotSupportedException() : throw new InvalidOperationException();

        /// <inheritdoc />
        public int Position => _parameter is { } p ? p.Position : _builder is { } b ? b.ReflectionBuilder.Position : throw new InvalidOperationException();

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (_parameter is not null)
                return _customAttributes ??= ResolveCustomAttributes(_parameter.GetCustomAttributesData());

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            if (_parameter is not null)
                return _parameter.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject, inherit);

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            if (_parameter is not null)
                return ResolveTypeSymbols(_parameter.GetOptionalCustomModifiers());

            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            if (_parameter is not null)
                return ResolveTypeSymbols(_parameter.GetRequiredCustomModifiers());

            throw new NotSupportedException();
        }

        /// <summary>
        /// Finishes the symbol by replacing the builder.
        /// </summary>
        /// <param name="parameter"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void Finish(ParameterInfo parameter)
        {
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
            _builder = null;
        }

    }

}
