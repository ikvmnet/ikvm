using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionParameterSymbolBuilder : ReflectionSymbolBuilder, IReflectionParameterSymbolBuilder
    {

        readonly IReflectionModuleSymbol _resolvingModule;
        readonly IReflectionMethodBaseSymbol _resolvingMethod;

        ParameterBuilder? _builder;
        ParameterInfo _parameter;

        object? _constant;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMethod"></param>
        /// <param name="builder"></param>
        public ReflectionParameterSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionMethodBaseSymbol resolvingMethod, ParameterBuilder builder) :
            base(context)
        {
            _resolvingModule = resolvingModule ?? throw new ArgumentNullException(nameof(resolvingModule));
            _resolvingMethod = resolvingMethod ?? throw new ArgumentNullException(nameof(resolvingMethod));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _parameter = new ReflectionParameterBuilderInfo(_builder, () => _constant);
        }

        /// <inheritdoc />
        public IReflectionModuleSymbol ResolvingModule => _resolvingMethod.ResolvingModule;

        /// <inheritdoc />
        public IReflectionMethodBaseSymbol ResolvingMethod => _resolvingMethod;

        /// <inheritdoc />
        public ParameterInfo UnderlyingParameter => _parameter;

        /// <inheritdoc />
        public ParameterBuilder UnderlyingParameterBuilder => _builder ?? throw new InvalidOperationException();

        #region IParameterSymbolBuilder

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            UnderlyingParameterBuilder.SetConstant(_constant = defaultValue);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingParameterBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingParameterBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

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
        public override bool IsComplete => _builder == null;

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingParameter.GetCustomAttributesData());
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

        /// <inheritdoc />
        public void OnComplete()
        {
            var p = ResolvingMethod.UnderlyingMethodBase.GetParameters();
            _parameter = p[Position];
            _builder = null;
        }

    }

}
