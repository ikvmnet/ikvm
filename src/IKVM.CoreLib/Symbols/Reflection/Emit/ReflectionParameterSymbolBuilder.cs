using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionParameterSymbolBuilder : ReflectionSymbolBuilder, IReflectionParameterSymbolBuilder
    {

        readonly IReflectionModuleSymbol _module;
        readonly IReflectionMemberSymbol _member;

        readonly ParameterBuilder _builder;
        readonly ReflectionParameterBuilderInfo _builderInfo;
        ParameterInfo? _parameter;

        object? _constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="member"></param>
        /// <param name="builder"></param>
        public ReflectionParameterSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionMemberSymbol member, ParameterBuilder builder) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _builderInfo = new ReflectionParameterBuilderInfo(builder, () => _constant);
        }

        /// <inheritdoc />
        public ParameterBuilder UnderlyingParameterBuilder => _builder;

        /// <inheritdoc />
        public ParameterInfo UnderlyingParameter => _builderInfo;

        /// <inheritdoc />
        public ParameterInfo UnderlyingRuntimeParameter => _parameter ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbol ResolvingModule => _module;

        /// <inheritdoc />
        public IReflectionMemberSymbol ResolvingMember => _member;

        #region IParameterSymbolBuilder

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            UnderlyingParameterBuilder.SetConstant(_constant = defaultValue);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingParameterBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

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
        public override bool IsComplete => _parameter != null;

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData().Where(i => attributeType.IsAssignableFrom(attributeType)).ToArray());
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
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
        public void OnComplete(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            _parameter = parameter;
        }

    }

}
