using System;
using System.Linq;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionParameterSymbol : IkvmReflectionSymbol, IIkvmReflectionParameterSymbol
    {

        readonly IIkvmReflectionModuleSymbol _resolvingModule;
        readonly IIkvmReflectionMemberSymbol _resolvingMember;
        readonly ParameterInfo _parameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMember"></param>
        /// <param name="parameter"></param>
        public IkvmReflectionParameterSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionMemberSymbol? resolvingMember, ParameterInfo parameter) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingMember = resolvingMember ?? throw new ArgumentNullException(nameof(resolvingMember));
            _parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
        }

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbol ResolvingModule => _resolvingModule;

        /// <inheritdoc />
        public IIkvmReflectionMemberSymbol ResolvingMember => _resolvingMember;

        /// <inheritdoc />
        public ParameterInfo UnderlyingParameter => _parameter;

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
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingParameter.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            var a = UnderlyingParameter.__GetCustomAttributes(_attributeType, inherit);
            if (a.Count > 0)
                return ResolveCustomAttribute(a[0]);

            return null;
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
