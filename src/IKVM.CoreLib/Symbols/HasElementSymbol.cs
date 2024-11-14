using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class HasElementSymbol : TypeSymbol
    {

        readonly TypeSymbol _elementType;

        string? _name;
        string? _fullName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        public HasElementSymbol(SymbolContext context, TypeSymbol elementType) :
            base(context, elementType.Module)
        {
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
        }

        /// <inheritdoc />
        public override MethodBaseSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override string? Namespace => _elementType.Namespace;

        /// <inheritdoc />
        public sealed override string Name => _name ??= _elementType.Name + NameSuffix;

        /// <inheritdoc />
        public sealed override string? FullName => _fullName ??= ComputeFullName();

        /// <summary>
        /// Computes the value of <see cref="FullName"/>.
        /// </summary>
        /// <returns></returns>
        string? ComputeFullName()
        {
            var fullName = _elementType.FullName;
            return fullName == null ? null : fullName + NameSuffix;
        }

        /// <summary>
        /// Gets the suffix of the type name.
        /// </summary>
        protected abstract string NameSuffix { get; }

        /// <inheritdoc />
        public sealed override bool HasElementType => true;

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _elementType.ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new NotSupportedException();

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => throw new NotSupportedException();

        /// <inheritdoc />
        public sealed override bool IsPrimitive => false;

        /// <inheritdoc />
        public sealed override bool IsEnum => false;

        /// <inheritdoc />
        public override bool IsArray => false;

        /// <inheritdoc />
        public override bool IsSZArray => false;

        /// <inheritdoc />
        public override bool IsByRef => false;

        /// <inheritdoc />
        public override bool IsPointer => false;

        /// <inheritdoc />
        public override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeCode.Object;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => _elementType.IsMissing;

        /// <inheritdoc />
        public sealed override bool IsComplete => _elementType.IsComplete;

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return _elementType;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetGenericTypeDefinition()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            return ImmutableArray<FieldSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            return ImmutableArray<PropertySymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            return ImmutableArray<EventSymbol>.Empty;
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            throw new NotSupportedException();
        }

    }

}
