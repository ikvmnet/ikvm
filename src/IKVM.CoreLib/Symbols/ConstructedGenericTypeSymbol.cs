using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using IKVM.CoreLib.Text;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type that is a constructed generic type.
    /// </summary>
    class ConstructedGenericTypeSymbol : TypeSymbol
    {

        readonly TypeSymbol _definition;
        readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<ConstructorSymbol> _constructors;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<PropertySymbol> _properties;
        ImmutableArray<EventSymbol> _events;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericTypeSymbol(SymbolContext context, TypeSymbol definition, GenericContext genericContext) :
            base(context, definition.Module)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => _definition.TypeCode;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _definition.BaseType?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => true;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => false;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new NotSupportedException();

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override MethodBaseSymbol? DeclaringMethod => _definition.DeclaringMethod;

        /// <inheritdoc />
        public sealed override string? FullName => ComputeFullName();

        /// <summary>
        /// Computes the value for <see cref="FullName"/>.
        /// </summary>
        /// <returns></returns>
        string? ComputeFullName()
        {
            if (ContainsGenericParameters)
                return null;

            using var fullName = new ValueStringBuilder(stackalloc char[256]);
            fullName.Append(_definition.FullName);
            fullName.Append('[');

            if (_genericContext.GenericTypeArguments != null)
            {
                for (int i = 0; i < _genericContext.GenericTypeArguments.Value.Length; i++)
                {
                    if (i != 0)
                        fullName.Append(',');

                    fullName.Append('[');
                    fullName.Append(_genericContext.GenericTypeArguments.Value[i].AssemblyQualifiedName);
                    fullName.Append(']');
                }
            }


            fullName.Append(']');
            return fullName.ToString();
        }

        /// <inheritdoc />
        public sealed override string? Namespace => _definition.Namespace;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _definition.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => _definition.IsPrimitive;

        /// <inheritdoc />
        public sealed override bool IsEnum => _definition.IsEnum;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => _genericContext.GenericTypeArguments != null && _genericContext.GenericTypeArguments.Value.Any(i => i.IsMissing || i.ContainsMissing);

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            return _definition.GetEnumUnderlyingType().Specialize(_genericContext);
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public sealed override int GetArrayRank()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetGenericTypeDefinition()
        {
            return _definition;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetGenericArguments()
        {
            return _genericContext.GenericTypeArguments!.Value;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields == default)
            {
                var b = ImmutableArray.CreateBuilder<FieldSymbol>();
                foreach (var i in _definition.GetFields(DeclaredOnlyLookup))
                    b.Add(new ConstructedGenericFieldSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.ToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal override ImmutableArray<ConstructorSymbol> GetDeclaredConstructors()
        {
            if (_constructors == default)
            {
                var b = ImmutableArray.CreateBuilder<ConstructorSymbol>();

                // add static initializer as a declared constructor, though it is filtered out from GetConstructors
                if (_definition.TypeInitializer != null)
                    b.Add(new ConstructedGenericConstructorSymbol(Context, this, _definition.TypeInitializer, _genericContext));

                foreach (var i in _definition.GetConstructors(DeclaredOnlyLookup))
                    b.Add(new ConstructedGenericConstructorSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _constructors, b.ToImmutable());
            }

            return _constructors;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods == default)
            {
                var b = ImmutableArray.CreateBuilder<MethodSymbol>();
                foreach (var i in _definition.GetMethods(DeclaredOnlyLookup))
                    b.Add(new ConstructedGenericMethodSymbol(Context, Module, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.ToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties == default)
            {
                var b = ImmutableArray.CreateBuilder<PropertySymbol>();
                foreach (var i in _definition.GetProperties(DeclaredOnlyLookup))
                    b.Add(new ConstructedGenericPropertySymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.ToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events == default)
            {
                var b = ImmutableArray.CreateBuilder<EventSymbol>();
                foreach (var i in _definition.GetEvents(DeclaredOnlyLookup))
                    b.Add(new ConstructedGenericEventSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.ToImmutable());
            }

            return _events;
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            return _definition.GetEnumName(value);
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            return _definition.GetEnumNames();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces == default)
            {
                var l = _definition.GetInterfaces();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>();
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, b.ToImmutable());
            }

            return _interfaces;
        }

        /// <inheritdoc />
        public sealed override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            return _definition.IsEnumDefined(value);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            using var sb = new ValueStringBuilder(stackalloc char[256]);
            sb.Append(_definition.ToString());
            sb.Append('[');

            if (_genericContext.GenericTypeArguments != null)
            {
                for (int i = 0; i < _genericContext.GenericTypeArguments.Value.Length; i++)
                {
                    if (i != 0)
                        sb.Append(',');

                    sb.Append(_genericContext.GenericTypeArguments.Value[i].ToString());
                }
            }

            sb.Append(']');

            return sb.ToString();
        }

    }

}
