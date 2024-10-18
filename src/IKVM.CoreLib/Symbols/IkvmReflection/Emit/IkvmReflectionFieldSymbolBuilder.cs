using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionFieldSymbolBuilder : IkvmReflectionMemberSymbolBuilder, IIkvmReflectionFieldSymbolBuilder
    {

        FieldBuilder? _builder;
        FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionFieldSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionTypeSymbolBuilder? resolvingType, FieldBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _field = _builder;
        }

        /// <inheritdoc />
        public FieldInfo UnderlyingField => _field;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingField;

        /// <inheritdoc />
        public FieldBuilder UnderlyingFieldBuilder => _builder ?? throw new InvalidOperationException();

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

        #region IFieldSymbol

        /// <inheritdoc/>
        public System.Reflection.FieldAttributes Attributes => (System.Reflection.FieldAttributes)UnderlyingField.Attributes;

        /// <inheritdoc/>
        public ITypeSymbol FieldType => ResolveTypeSymbol(UnderlyingField.FieldType);

        /// <inheritdoc/>
        public bool IsSpecialName => UnderlyingField.IsSpecialName;

        /// <inheritdoc/>
        public bool IsAssembly => UnderlyingField.IsAssembly;

        /// <inheritdoc/>
        public bool IsFamily => UnderlyingField.IsFamily;

        /// <inheritdoc/>
        public bool IsFamilyAndAssembly => UnderlyingField.IsFamilyAndAssembly;

        /// <inheritdoc/>
        public bool IsFamilyOrAssembly => UnderlyingField.IsFamilyOrAssembly;

        /// <inheritdoc/>
        public bool IsInitOnly => UnderlyingField.IsInitOnly;

        /// <inheritdoc/>
        public bool IsLiteral => UnderlyingField.IsLiteral;

        /// <inheritdoc/>
        public bool IsNotSerialized => UnderlyingField.IsNotSerialized;

        /// <inheritdoc/>
        public bool IsPinvokeImpl => UnderlyingField.IsPinvokeImpl;

        /// <inheritdoc/>
        public bool IsPrivate => UnderlyingField.IsPrivate;

        /// <inheritdoc/>
        public bool IsPublic => UnderlyingField.IsPublic;

        /// <inheritdoc/>
        public bool IsStatic => UnderlyingField.IsStatic;

        /// <inheritdoc/>
        public override bool IsComplete => _builder == null;

        /// <inheritdoc/>
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingField.GetOptionalCustomModifiers());
        }

        /// <inheritdoc/>
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingField.GetRequiredCustomModifiers());
        }

        /// <inheritdoc/>
        public object? GetRawConstantValue()
        {
            return UnderlyingField.GetRawConstantValue();
        }

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public override void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingFieldBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        /// <inheritdoc/>
        public override void OnComplete()
        {
            _builder = null;
            base.OnComplete();
        }

    }

}
