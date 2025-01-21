using System;
using System.Collections.Immutable;

using ParameterInfo = IKVM.Reflection.ParameterInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionParameterSymbol : IkvmReflectionSymbol, IParameterSymbol
    {

        readonly ParameterInfo _underlyingParameter;
        readonly IkvmReflectionMethodBaseSymbol _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="underlyingParameter"></param>
        public IkvmReflectionParameterSymbol(IkvmReflectionSymbolContext context, IkvmReflectionMethodBaseSymbol method, ParameterInfo underlyingParameter) :
            base(context)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _underlyingParameter = underlyingParameter ?? throw new ArgumentNullException(nameof(underlyingParameter));
        }

        internal IkvmReflectionMethodBaseSymbol ContainingMethod => _method;

        public System.Reflection.ParameterAttributes Attributes => (System.Reflection.ParameterAttributes)_underlyingParameter.Attributes;

        public object DefaultValue => _underlyingParameter.RawDefaultValue;

        public bool HasDefaultValue => _underlyingParameter.HasDefaultValue;

        public bool IsIn => _underlyingParameter.IsIn;

        public bool IsLcid => _underlyingParameter.IsLcid;

        public bool IsOptional => _underlyingParameter.IsOptional;

        public bool IsOut => _underlyingParameter.IsOut;

        public bool IsRetval => _underlyingParameter.IsRetval;

        public IMemberSymbol Member => ResolveMemberSymbol(_underlyingParameter.Member);

        public int MetadataToken => _underlyingParameter.MetadataToken;

        public string? Name => _underlyingParameter.Name;

        public ITypeSymbol ParameterType => ResolveTypeSymbol(_underlyingParameter.ParameterType);

        public int Position => _underlyingParameter.Position;

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return ResolveCustomAttributes(_underlyingParameter.GetCustomAttributesData());
        }

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_underlyingParameter.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).UnderlyingType, false));
        }

        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingParameter.IsDefined(((IkvmReflectionTypeSymbol)attributeType).UnderlyingType, false);
        }

    }

}