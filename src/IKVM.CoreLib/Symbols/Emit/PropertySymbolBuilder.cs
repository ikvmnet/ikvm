using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class PropertySymbolBuilder : PropertySymbol, ICustomAttributeBuilder
    {

        static ImmutableArray<TypeSymbol> GetIndexOrNull(ImmutableArray<ImmutableArray<TypeSymbol>> a, int i) => a.Length >= i ? a[i] : [];

        readonly string _name;
        readonly PropertyAttributes _attributes;
        readonly CallingConventions _callingConventions;
        readonly TypeSymbol _propertyType;
        readonly ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        readonly ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        readonly ImmutableArray<ParameterSymbolBuilder>.Builder _parameters = ImmutableArray.CreateBuilder<ParameterSymbolBuilder>();

        MethodSymbolBuilder? _getMethod;
        MethodSymbolBuilder? _setMethod;
        readonly ImmutableArray<MethodSymbolBuilder>.Builder _accessorMethods = ImmutableArray.CreateBuilder<MethodSymbolBuilder>();

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
        /// <param name="callingConvention"></param>
        /// <param name="propertyType"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="parameterRequiredCustomModifiers"></param>
        /// <param name="parameterOptionalCustomModifiers"></param>
        internal PropertySymbolBuilder(SymbolContext context, TypeSymbol declaringType, string name, PropertyAttributes attributes, CallingConventions callingConvention, TypeSymbol propertyType, ImmutableArray<TypeSymbol> requiredCustomModifiers, ImmutableArray<TypeSymbol> optionalCustomModifiers, ImmutableArray<TypeSymbol> parameterTypes, ImmutableArray<ImmutableArray<TypeSymbol>> parameterRequiredCustomModifiers, ImmutableArray<ImmutableArray<TypeSymbol>> parameterOptionalCustomModifiers) :
            base(context, declaringType)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _propertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
            _callingConventions = callingConvention;
            _requiredCustomModifiers = requiredCustomModifiers;
            _optionalCustomModifiers = optionalCustomModifiers;
            _parameters.AddRange(parameterTypes.Select((i, j) => new ParameterSymbolBuilder(Context, this, i, j, GetIndexOrNull(parameterRequiredCustomModifiers, j), GetIndexOrNull(parameterOptionalCustomModifiers, j))));
        }

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _attributes;

        /// <summary>
        /// Gets the calling conventions to apply to the property.
        /// </summary>
        public CallingConventions CallingConventions => _callingConventions;

        /// <inheritdoc />
        public override TypeSymbol PropertyType => _propertyType;

        /// <inheritdoc />
        public override bool CanRead => GetGetMethod(true) != null;

        /// <inheritdoc />
        public override bool CanWrite => GetSetMethod(true) != null;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            if (nonPublic && _getMethod != null && _getMethod.IsPublic == false)
                return _getMethod;
            else if (nonPublic == false && _getMethod != null && _getMethod.IsPublic)
                return _getMethod;
            else
                return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            if (nonPublic && _setMethod != null && _setMethod.IsPublic == false)
                return _setMethod;
            else if (nonPublic == false && _setMethod != null && _setMethod.IsPublic)
                return _setMethod;
            else
                return null;
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>();

            foreach (var i in _accessorMethods)
            {
                if (nonPublic && i != null && i.IsPublic == false)
                    b.Add(i);
                else if (nonPublic == false && i != null && i.IsPublic)
                    b.Add(i);
            }

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override TypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _constantValue;
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            return _parameters.ToImmutable().CastArray<ParameterSymbol>();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void Freeze()
        {
            _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
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

        /// <summary>
        /// Adds one of the other methods associated with this property.
        /// </summary>
        /// <param name="method"></param>
        public void AddOtherMethod(MethodSymbolBuilder method)
        {
            ThrowIfFrozen();
            _accessorMethods.Add(method);
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
