using System;
using System.Linq;

using ParameterInfo = IKVM.Reflection.ParameterInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionParameterSymbol : IkvmReflectionSymbol, IParameterSymbol
    {

        readonly IkvmReflectionMethodBaseSymbol _containingMethod;
        readonly ParameterInfo _parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingMethod"></param>
        /// <param name="parameter"></param>
        public IkvmReflectionParameterSymbol(IkvmReflectionSymbolContext context, IkvmReflectionMethodBaseSymbol containingMethod, ParameterInfo parameter) :
            base(context)
        {
            _containingMethod = containingMethod ?? throw new ArgumentNullException(nameof(containingMethod));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        internal IkvmReflectionMethodBaseSymbol ContainingMethod => _containingMethod;

        /// <inheritdoc />
        public System.Reflection.ParameterAttributes Attributes => (System.Reflection.ParameterAttributes)_parameter.Attributes;

        /// <inheritdoc />
        public object DefaultValue => _parameter.RawDefaultValue;

        /// <inheritdoc />
        public bool HasDefaultValue => _parameter.HasDefaultValue;

        /// <inheritdoc />
        public bool IsIn => _parameter.IsIn;

        /// <inheritdoc />
        public bool IsLcid => _parameter.IsLcid;

        /// <inheritdoc />
        public bool IsOptional => _parameter.IsOptional;

        /// <inheritdoc />
        public bool IsOut => _parameter.IsOut;

        /// <inheritdoc />
        public bool IsRetval => _parameter.IsRetval;

        /// <inheritdoc />
        public IMemberSymbol Member => ResolveMemberSymbol(_parameter.Member);

        /// <inheritdoc />
        public int MetadataToken => _parameter.MetadataToken;

        /// <inheritdoc />
        public string? Name => _parameter.Name;

        /// <inheritdoc />
        public ITypeSymbol ParameterType => ResolveTypeSymbol(_parameter.ParameterType);

        /// <inheritdoc />
        public int Position => _parameter.Position;

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(_parameter.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_parameter.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public virtual bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _parameter.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, inherit);
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(_parameter.GetOptionalCustomModifiers());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(_parameter.GetRequiredCustomModifiers());
        }

    }

}