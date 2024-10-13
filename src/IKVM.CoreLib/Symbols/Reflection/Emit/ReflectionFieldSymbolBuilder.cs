using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionFieldSymbolBuilder : ReflectionFieldSymbol, IReflectionFieldSymbolBuilder
    {

        readonly FieldBuilder _builder;
        FieldInfo? _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionFieldSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder? resolvingType, FieldBuilder builder) :
            base(context, resolvingModule, resolvingType, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
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
