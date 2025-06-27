using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class FieldSymbolBuilder : DefinitionFieldSymbol, ICustomAttributeBuilder
    {

        readonly ModuleSymbol _declaringModule;
        readonly TypeSymbolBuilder? _declaringType;
        readonly string _name;
        readonly FieldAttributes _attributes;
        readonly TypeSymbol _fieldType;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        object? _constantValue;
        int? _offset;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        bool _frozen;
        object? _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringModule"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="fieldType"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        internal FieldSymbolBuilder(SymbolContext context, ModuleSymbol declaringModule, TypeSymbolBuilder? declaringType, string name, FieldAttributes attributes, TypeSymbol fieldType, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers) :
            base(context)
        {
            _declaringModule = declaringModule ?? throw new ArgumentNullException(nameof(declaringModule));
            _declaringType = declaringType;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _fieldType = fieldType ?? throw new ArgumentNullException(nameof(fieldType));
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _declaringModule;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override FieldAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override TypeSymbol FieldType => _fieldType;

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue()
        {
            return _constantValue;
        }

        /// <summary>
        /// Gets the defined field offset.
        /// </summary>
        public int? Offset => _offset;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void Freeze()
        {
            lock (this)
                _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
            lock (this)
                if (_frozen)
                    throw new InvalidOperationException("FieldSymbolBuilder is frozen.");
        }

        /// <summary>
        /// Sets the default value of this field.
        /// </summary>
        /// <param name="defaultValue"></param>
        public void SetConstant(object? defaultValue)
        {
            ThrowIfFrozen();
            _constantValue = default;
        }

        /// <summary>
        /// Specifies the field layout.
        /// </summary>
        /// <param name="iOffset"></param>
        public void SetOffset(int iOffset)
        {
            ThrowIfFrozen();
            _offset = iOffset;
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            ThrowIfFrozen();
            _customAttributes.Add(attribute);
        }

        /// <summary>
        /// Gets the writer object associated with this builder.
        /// </summary>
        /// <typeparam name="TWriter"></typeparam>
        /// <param name="create"></param>
        /// <returns></returns>
        internal TWriter Writer<TWriter>(Func<FieldSymbolBuilder, TWriter> create)
        {
            if (_writer is null)
                Interlocked.CompareExchange(ref _writer, create(this), null);

            return (TWriter)(_writer ?? throw new InvalidOperationException());
        }

    }

}
