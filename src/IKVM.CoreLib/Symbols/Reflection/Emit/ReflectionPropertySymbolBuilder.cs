using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionPropertySymbolBuilder : ReflectionPropertySymbol, IReflectionPropertySymbolBuilder
    {

        readonly PropertyBuilder _builder;
        PropertyInfo? _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionPropertySymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder resolvingType, PropertyBuilder builder) :
            base(context, resolvingModule, resolvingType, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public PropertyBuilder UnderlyingPropertyBuilder => _builder ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override PropertyInfo UnderlyingRuntimeProperty => _property ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingPropertyBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        #region IPropertySymbol

        /// <inheritdoc />
        public override bool IsComplete => _property != null;

        #endregion

        #region IPropertySymbolBuilder

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            UnderlyingPropertyBuilder.SetConstant(defaultValue);
        }

        /// <inheritdoc />
        public void SetGetMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.SetGetMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetSetMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.SetSetMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.AddOtherMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        #endregion

        /// <inheritdoc />
        public void OnComplete()
        {
            _property = (PropertyInfo?)ResolvingModule.UnderlyingModule.ResolveMember(MetadataToken) ?? throw new InvalidOperationException();
        }

    }

}
