using System;

using FieldInfo = IKVM.Reflection.FieldInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionFieldSymbol : IkvmReflectionMemberSymbol, IFieldSymbol
    {

        readonly FieldInfo _underlyingField;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingField"></param>
        public IkvmReflectionFieldSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, FieldInfo underlyingField) :
            base(context, type.ContainingModule, type, underlyingField)
        {
            _underlyingField = underlyingField ?? throw new ArgumentNullException(nameof(underlyingField));
        }

        internal FieldInfo UnderlyingField => _underlyingField;

        public global::System.Reflection.FieldAttributes Attributes => (global::System.Reflection.FieldAttributes)_underlyingField.Attributes;

        public ITypeSymbol FieldType => ResolveTypeSymbol(_underlyingField.FieldType);

        public bool IsAssembly => _underlyingField.IsAssembly;

        public bool IsFamily => _underlyingField.IsFamily;

        public bool IsFamilyAndAssembly => _underlyingField.IsFamilyAndAssembly;

        public bool IsFamilyOrAssembly => _underlyingField.IsFamilyOrAssembly;

        public bool IsInitOnly => _underlyingField.IsInitOnly;

        public bool IsLiteral => _underlyingField.IsLiteral;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        public bool IsNotSerialized => _underlyingField.IsNotSerialized;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

        public bool IsPinvokeImpl => _underlyingField.IsPinvokeImpl;

        public bool IsPrivate => _underlyingField.IsPrivate;

        public bool IsPublic => _underlyingField.IsPublic;

        public bool IsSpecialName => _underlyingField.IsSpecialName;

        public bool IsStatic => _underlyingField.IsStatic;

        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingField.GetOptionalCustomModifiers());
        }

        public object? GetRawConstantValue()
        {
            return _underlyingField.GetRawConstantValue();
        }

        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingField.GetRequiredCustomModifiers());
        }

    }

}