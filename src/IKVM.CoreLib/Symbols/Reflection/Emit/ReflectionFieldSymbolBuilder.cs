using System;
using System.Reflection;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionFieldSymbolBuilder : ReflectionFieldSymbol, IReflectionFieldSymbolBuilder
    {

        readonly FieldBuilder _builder;
        FieldInfo? _field;

        readonly ITypeSymbol[]? _requiredCustomModifiers;
        readonly ITypeSymbol[]? _optionalCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionFieldSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder? resolvingType, FieldBuilder builder, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers) :
            base(context, resolvingModule, resolvingType, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _requiredCustomModifiers = requiredCustomModifiers ?? throw new ArgumentNullException(nameof(requiredCustomModifiers));
            _optionalCustomModifiers = optionalCustomModifiers ?? throw new ArgumentNullException(nameof(optionalCustomModifiers));
        }

        /// <inheritdoc />
        public FieldBuilder UnderlyingFieldBuilder => _builder;

        /// <inheritdoc />
        public override FieldInfo UnderlyingRuntimeField => _field ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

        #region IFieldSymbol

        /// <inheritdoc/>
        public override bool IsComplete => _field != null;

        /// <inheritdoc/>
        public override ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return _requiredCustomModifiers ?? [];
        }

        /// <inheritdoc/>
        public override ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers ?? [];
        }

        #endregion

        #region IFieldSymbolBuilder

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            UnderlyingFieldBuilder.SetConstant(defaultValue);
        }

        /// <inheritdoc />
        public void SetOffset(int iOffset)
        {
            UnderlyingFieldBuilder.SetOffset(iOffset);
        }

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingFieldBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        /// <inheritdoc/>
        public void OnComplete()
        {
            _field = ResolvingModule.UnderlyingModule.ResolveField(MetadataToken) ?? throw new InvalidOperationException();
        }

    }

}
