using System;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : ReflectionSymbol, IParameterSymbol
    {

        readonly ParameterInfo _parameter;
        readonly ReflectionMethodBaseSymbol _method;

        CustomAttributeSymbol[]? _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, ReflectionMethodBaseSymbol method, ParameterInfo parameter) :
            base(context)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        internal ReflectionMethodBaseSymbol ContainingMethod => _method;

        public ParameterAttributes Attributes => _parameter.Attributes;

        public object? DefaultValue => _parameter.DefaultValue;

        public bool HasDefaultValue => _parameter.HasDefaultValue;

        public bool IsIn => _parameter.IsIn;

        public bool IsLcid => _parameter.IsLcid;

        public bool IsOptional => _parameter.IsOptional;

        public bool IsOut => _parameter.IsOut;

        public bool IsRetval => _parameter.IsRetval;

        public IMemberSymbol Member => ResolveMemberSymbol(_parameter.Member);

        public int MetadataToken => _parameter.MetadataToken;

        public string? Name => _parameter.Name;

        public ITypeSymbol ParameterType => ResolveTypeSymbol(_parameter.ParameterType);

        public int Position => _parameter.Position;

        /// <inheritdoc />
        public CustomAttributeSymbol[] GetCustomAttributes(bool inherit = false)
        {
            return _customAttributes ??= ResolveCustomAttributes(_parameter.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToArray();
        }

        /// <inheritdoc />
        public virtual CustomAttributeSymbol? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _parameter.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject, inherit);
        }

    }

}