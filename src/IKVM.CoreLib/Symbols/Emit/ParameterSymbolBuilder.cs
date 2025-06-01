using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class ParameterSymbolBuilder : DefinitionParameterSymbol, ICustomAttributeBuilder
    {

        readonly MemberSymbol _declaringMember;
        readonly TypeSymbol _parameterType;
        readonly int _position;

        internal string? _name;
        internal ParameterAttributes _attributes;
        object? _defaultValue;

        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        bool _frozen;
        object? _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="parameterType"></param>
        /// <param name="position"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal ParameterSymbolBuilder(SymbolContext context, MemberSymbol declaringMember, TypeSymbol parameterType, int position, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers) :
            base(context)
        {
            _declaringMember = declaringMember ?? throw new ArgumentNullException(nameof(declaringMember));
            _parameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            _position = position;
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MemberSymbol Member => _declaringMember;

        /// <inheritdoc />
        public sealed override ParameterAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol ParameterType => _parameterType;

        /// <inheritdoc />
        public sealed override string? Name => _name;

        /// <inheritdoc />
        public sealed override int Position => _position;

        /// <inheritdoc />
        public sealed override object? DefaultValue => _defaultValue;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _optionalCustomModifiers;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _requiredCustomModifiers;

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _customAttributes.ToImmutable();

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
                    throw new InvalidOperationException("PropertySymbolBuilder is frozen.");
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
        internal TWriter Writer<TWriter>(Func<ParameterSymbolBuilder, TWriter> create)
        {
            if (_writer is null)
                Interlocked.CompareExchange(ref _writer, create(this), null);

            return (TWriter)(_writer ?? throw new InvalidOperationException());
        }

    }

}
