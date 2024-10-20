using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionFieldSymbol : ReflectionMemberSymbol, IReflectionFieldSymbol
    {

        readonly FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="field"></param>
        public ReflectionFieldSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol? resolvingType, FieldInfo field) :
            base(context, resolvingModule, resolvingType)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <inheritdoc />
        public virtual FieldInfo UnderlyingField => _field;

        /// <inheritdoc />
        public virtual FieldInfo UnderlyingRuntimeField => _field;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingField;

        /// <inheritdoc />
        public override MemberInfo UnderlyingRuntimeMember => UnderlyingRuntimeField;

        #region IFieldSymbol

        /// <inheritdoc/>
        public FieldAttributes Attributes => UnderlyingField.Attributes;

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
        public virtual ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingField.GetOptionalCustomModifiers());
        }

        /// <inheritdoc/>
        public virtual ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingField.GetRequiredCustomModifiers());
        }

        /// <inheritdoc/>
        public object? GetRawConstantValue()
        {
            return UnderlyingField.GetRawConstantValue();
        }

        #endregion

    }

}