using System;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : ReflectionSymbol, IReflectionParameterSymbol
    {

        readonly IReflectionModuleSymbol _resolvingModule;
        readonly IReflectionMethodBaseSymbol _resolvingMethod;
        readonly ParameterInfo _parameter;

        CustomAttribute[]? _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMethod"></param>
        /// <param name="parameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionMethodBaseSymbol resolvingMethod, ParameterInfo parameter) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingMethod = resolvingMethod ?? throw new ArgumentNullException(nameof(resolvingMethod));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <inheritdoc />
        public IReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <inheritdoc />
        public IReflectionMethodBaseSymbol ResolvingMethod => _resolvingMethod;

        /// <inheritdoc />
        public ParameterInfo UnderlyingParameter => _parameter;

        #region IParameterSymbol

        /// <inheritdoc />
        public ParameterAttributes Attributes => UnderlyingParameter.Attributes;

        /// <inheritdoc />
        public object? DefaultValue => UnderlyingParameter.DefaultValue;

        /// <inheritdoc />
        public bool HasDefaultValue => UnderlyingParameter.HasDefaultValue;

        /// <inheritdoc />
        public bool IsIn => UnderlyingParameter.IsIn;

        /// <inheritdoc />
        public bool IsLcid => UnderlyingParameter.IsLcid;

        /// <inheritdoc />
        public bool IsOptional => UnderlyingParameter.IsOptional;

        /// <inheritdoc />
        public bool IsOut => UnderlyingParameter.IsOut;

        /// <inheritdoc />
        public bool IsRetval => UnderlyingParameter.IsRetval;

        /// <inheritdoc />
        public IMemberSymbol Member => ResolveMemberSymbol(UnderlyingParameter.Member);

        /// <inheritdoc />
        public int MetadataToken => UnderlyingParameter.MetadataToken;

        /// <inheritdoc />
        public string? Name => UnderlyingParameter.Name;

        /// <inheritdoc />
        public ITypeSymbol ParameterType => ResolveTypeSymbol(UnderlyingParameter.ParameterType);

        /// <inheritdoc />
        public int Position => UnderlyingParameter.Position;

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return _customAttributes ??= ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData());
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
            return UnderlyingParameter.IsDefined(attributeType.Unpack(), inherit);
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingParameter.GetOptionalCustomModifiers());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingParameter.GetRequiredCustomModifiers());
        }

        #endregion

    }

}
