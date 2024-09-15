using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionFieldSymbol : ReflectionMemberSymbol, IFieldSymbol
    {

        FieldInfo _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingType"></param>
        /// <param name="field"></param>
        public ReflectionFieldSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol containingModule, FieldInfo field) :
            base(context, containingModule, field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="containingType"></param>
        /// <param name="field"></param>
        public ReflectionFieldSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol containingType, FieldInfo field) :
            base(context, containingType, field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
        }

        /// <summary>
        /// Gets the underlying <see cref="FieldInfo"/> wrapped by this symbol.
        /// </summary>
        internal new FieldInfo ReflectionObject => (FieldInfo)base.ReflectionObject;

        /// <inheritdoc/>
        public FieldAttributes Attributes => _field.Attributes;

        /// <inheritdoc/>
        public ITypeSymbol FieldType => ResolveTypeSymbol(_field.FieldType);

        /// <inheritdoc/>
        public bool IsSpecialName => _field.IsSpecialName;

        /// <inheritdoc/>
        public bool IsAssembly => _field.IsAssembly;

        /// <inheritdoc/>
        public bool IsFamily => _field.IsFamily;

        /// <inheritdoc/>
        public bool IsFamilyAndAssembly => _field.IsFamilyAndAssembly;

        /// <inheritdoc/>
        public bool IsFamilyOrAssembly => _field.IsFamilyOrAssembly;

        /// <inheritdoc/>
        public bool IsInitOnly => _field.IsInitOnly;

        /// <inheritdoc/>
        public bool IsLiteral => _field.IsLiteral;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
        /// <inheritdoc/>
        public bool IsNotSerialized => _field.IsNotSerialized;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

        /// <inheritdoc/>
        public bool IsPinvokeImpl => _field.IsPinvokeImpl;

        /// <inheritdoc/>
        public bool IsPrivate => _field.IsPrivate;

        /// <inheritdoc/>
        public bool IsPublic => _field.IsPublic;

        /// <inheritdoc/>
        public bool IsStatic => _field.IsStatic;

        /// <inheritdoc/>
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(_field.GetOptionalCustomModifiers());
        }

        /// <inheritdoc/>
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(_field.GetRequiredCustomModifiers());
        }

        /// <inheritdoc/>
        public object? GetRawConstantValue()
        {
            return _field.GetRawConstantValue();
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="field"></param>
        internal void Complete(FieldInfo field)
        {
            ResolveFieldSymbol(_field = field);
            base.Complete(_field);
        }

    }

}