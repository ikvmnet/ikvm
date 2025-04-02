using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : ReflectionSymbol, IParameterSymbol
    {

        readonly ParameterInfo _underlyingParameter;
        readonly ReflectionMethodBaseSymbol _method;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <param name="underlyingParameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, ReflectionMethodBaseSymbol method, ParameterInfo underlyingParameter) :
            base(context)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _underlyingParameter = underlyingParameter ?? throw new ArgumentNullException(nameof(underlyingParameter));
        }

        internal ReflectionMethodBaseSymbol ContainingMethod => _method;

        public ParameterAttributes Attributes => _underlyingParameter.Attributes;

        public object? DefaultValue => _underlyingParameter.DefaultValue;

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
            return ResolveCustomAttributes(_underlyingParameter.GetCustomAttributesData()).Where(i => i.AttributeType == attributeType).ToImmutableArray();
        }

        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingParameter.IsDefined(((ReflectionTypeSymbol)attributeType).UnderlyingType);
        }

    }

}