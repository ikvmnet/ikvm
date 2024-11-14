using System;
using System.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionGenericTypeParameterSymbolBuilder : ReflectionGenericTypeParameterSymbol, IReflectionGenericTypeParameterSymbolBuilder
    {

        readonly GenericTypeParameterBuilder _builder;
        Type? _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMember"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionGenericTypeParameterSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionMemberSymbolBuilder resolvingMember) :
            base(context, resolvingModule, resolvingMember)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public override Type UnderlyingRuntimeType => _type ?? throw new InvalidOperationException();

        #region IReflectionGenericTypeParameterSymbolBuilder

        /// <inheritdoc />
        public GenericTypeParameterBuilder UnderlyingGenericTypeParameterBuilder => _builder ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public void OnComplete(Type type)
        {
            _type = type;
        }

        #endregion

        #region IGenericTypeParameterSymbolBuilder

        /// <inheritdoc />
        public void SetBaseTypeConstraint(ITypeSymbol? baseTypeConstraint)
        {
            UnderlyingGenericTypeParameterBuilder.SetBaseTypeConstraint(baseTypeConstraint?.Unpack());
        }

        /// <inheritdoc />
        public void SetGenericParameterAttributes(System.Reflection.GenericParameterAttributes genericParameterAttributes)
        {
            UnderlyingGenericTypeParameterBuilder.SetGenericParameterAttributes(genericParameterAttributes);
        }

        /// <inheritdoc />
        public void SetInterfaceConstraints(params ITypeSymbol[]? interfaceConstraints)
        {
            UnderlyingGenericTypeParameterBuilder.SetInterfaceConstraints(interfaceConstraints?.Unpack());
        }

        #endregion

        #region ITypeSymbol

        /// <inheritdoc />
        public override bool IsComplete => _type != null;

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingGenericTypeParameterBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

    }

}
