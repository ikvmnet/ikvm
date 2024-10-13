using System;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : ReflectionSymbol, IReflectionParameterSymbol
    {

        readonly IReflectionModuleSymbol _resolvingModule;
        readonly IReflectionMemberSymbol _resolvingMember;
        readonly ParameterInfo _parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMember"></param>
        /// <param name="parameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionMemberSymbol? resolvingMember, ParameterInfo parameter) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingMember = resolvingMember ?? throw new ArgumentNullException(nameof(resolvingMember));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <inheritdoc />
        public IReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <inheritdoc />
        public IReflectionMemberSymbol ResolvingMember => _resolvingMember;

        /// <inheritdoc />
        public ParameterInfo UnderlyingParameter => _parameter;

        /// <inheritdoc />
        public ParameterInfo UnderlyingRuntimeParameter => UnderlyingParameter;

        #region IParameterSymbol

        /// <inheritdoc />
        public System.Reflection.ParameterAttributes Attributes => (System.Reflection.ParameterAttributes)UnderlyingParameter.Attributes;

        /// <inheritdoc />
        public object? DefaultValue => UnderlyingParameter.RawDefaultValue;

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
        public virtual CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            if (inherit == true)
                throw new NotSupportedException();

            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            if (inherit == true)
                throw new NotSupportedException();

            var _attribyteType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData().Where(i => i.AttributeType == _attribyteType).ToArray());
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            if (inherit == true)
                throw new NotSupportedException();

            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttribute(UnderlyingParameter.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).FirstOrDefault());
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

        /// <inheritdoc />
        public override string? ToString() => UnderlyingParameter.ToString();

    }

}
