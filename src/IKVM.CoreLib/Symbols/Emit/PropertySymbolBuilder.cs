using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class PropertySymbolBuilder : DefinitionPropertySymbol, ICustomAttributeBuilder
    {

        static ImmutableArray<TypeSymbol> GetIndexOrNull(ImmutableArray<ImmutableArray<TypeSymbol>> a, int i) => a.Length >= i ? a[i] : [];

        private readonly TypeSymbol _declaringType;
        readonly string _name;
        readonly PropertyAttributes _attributes;
        readonly TypeSymbol _propertyType;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        readonly ImmutableArray<ParameterSymbolBuilder>.Builder _parameters = ImmutableArray.CreateBuilder<ParameterSymbolBuilder>();

        MethodSymbolBuilder? _getMethod;
        MethodSymbolBuilder? _setMethod;

        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        object? _constantValue;

        bool _frozen;
        object? _writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="propertyType"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        internal PropertySymbolBuilder(SymbolContext context, TypeSymbol declaringType, string name, PropertyAttributes attributes, TypeSymbol propertyType, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers) :
            base(context)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _propertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
            _parameters.AddRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, GetIndexOrNull(parameterRequiredCustomModifiers, j), GetIndexOrNull(parameterOptionalCustomModifiers, j))));
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override PropertyAttributes Attributes => _attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol PropertyType => _propertyType;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetMethod => _getMethod;

        /// <inheritdoc />
        public sealed override MethodSymbol? SetMethod => _setMethod;

        /// <inheritdoc />
        public sealed override TypeSymbol GetModifiedPropertyType() => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue() => _constantValue;

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> GetIndexParameters() => _parameters.ToImmutable().CastArray<ParameterSymbol>();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _optionalCustomModifiers;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _requiredCustomModifiers;

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _customAttributes.ToImmutable();

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

        /// <summary>
        /// Sets the default value of this property.
        /// </summary>
        /// <param name="defaultValue"></param>
        public void SetConstant(object? defaultValue)
        {
            ThrowIfFrozen();
            _constantValue = defaultValue;
        }

        /// <summary>
        /// Sets the method that gets the property value.
        /// </summary>
        /// <param name="method"></param>
        public void SetGetMethod(MethodSymbolBuilder method)
        {
            ThrowIfFrozen();
            _getMethod = method;
        }

        /// <summary>
        /// Sets the method that sets the property value.
        /// </summary>
        /// <param name="method"></param>
        public void SetSetMethod(MethodSymbolBuilder method)
        {
            ThrowIfFrozen();
            _setMethod = method;
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
        internal TWriter Writer<TWriter>(Func<PropertySymbolBuilder, TWriter> create)
        {
            if (_writer is null)
                Interlocked.CompareExchange(ref _writer, create(this), null);

            return (TWriter)(_writer ?? throw new InvalidOperationException());
        }

    }

}
