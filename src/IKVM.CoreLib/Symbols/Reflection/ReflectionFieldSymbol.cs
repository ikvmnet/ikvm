using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionFieldSymbol : ReflectionMemberSymbol, IFieldSymbol
    {

        readonly FieldInfo _underlyingField;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingField"></param>
        public ReflectionFieldSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, FieldInfo underlyingField) :
            base(context, type.ContainingModule, type, underlyingField)
        {
            _underlyingField = underlyingField ?? throw new ArgumentNullException(nameof(underlyingField));
        }

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/> wrapped by this symbol.
        /// </summary>
        internal FieldInfo UnderlyingField => _underlyingField;

        /// <inheritdoc/>
        public FieldAttributes Attributes => _underlyingField.Attributes;

        /// <inheritdoc/>
        public ITypeSymbol FieldType => ResolveTypeSymbol(_underlyingField.FieldType);

        /// <inheritdoc/>
        public bool IsSpecialName => _underlyingField.IsSpecialName;

        /// <inheritdoc/>
        public bool IsAssembly => _underlyingField.IsAssembly;

        /// <inheritdoc/>
        public bool IsFamily => _underlyingField.IsFamily;

        /// <inheritdoc/>
        public bool IsFamilyAndAssembly => _underlyingField.IsFamilyAndAssembly;

        /// <inheritdoc/>
        public bool IsFamilyOrAssembly => _underlyingField.IsFamilyOrAssembly;

        /// <inheritdoc/>
        public bool IsInitOnly => _underlyingField.IsInitOnly;

        /// <inheritdoc/>
        public bool IsLiteral => _underlyingField.IsLiteral;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        /// <inheritdoc/>
        public bool IsNotSerialized => _underlyingField.IsNotSerialized;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

        /// <inheritdoc/>
        public bool IsPinvokeImpl => _underlyingField.IsPinvokeImpl;

        /// <inheritdoc/>
        public bool IsPrivate => _underlyingField.IsPrivate;

        /// <inheritdoc/>
        public bool IsPublic => _underlyingField.IsPublic;

        /// <inheritdoc/>
        public bool IsStatic => _underlyingField.IsStatic;

        /// <inheritdoc/>
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingField.GetOptionalCustomModifiers());
        }

        /// <inheritdoc/>
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingField.GetRequiredCustomModifiers());
        }

        /// <inheritdoc/>
        public object? GetRawConstantValue()
        {
            return _underlyingField.GetRawConstantValue();
        }

    }

}