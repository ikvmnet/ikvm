using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type that is a constructed generic type.
    /// </summary>
    class ConstructedGenericTypeSymbol : TypeSymbol
    {

        readonly TypeSymbol _definition;
        readonly ImmutableList<TypeSymbol> _typeArguments;

        ImmutableList<FieldSymbol>? _fields;
        ImmutableList<ConstructorSymbol>? _constructors;
        ImmutableList<MethodSymbol>? _methods;
        ImmutableList<PropertySymbol>? _properties;
        ImmutableList<EventSymbol>? _events;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="definition"></param>
        /// <param name="typeArguments"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericTypeSymbol(ISymbolContext context, TypeSymbol definition, ImmutableList<TypeSymbol> typeArguments) :
            base(context, definition.Module, definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _typeArguments = typeArguments ?? throw new ArgumentNullException(nameof(typeArguments));
        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => _definition.TypeCode;

        /// <inheritdoc />
        public override TypeSymbol? BaseType => _definition.BaseType?.Specialize(new GenericContext(_typeArguments, null));

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => false;

        /// <inheritdoc />
        public override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public override bool IsConstructedGenericType => true;

        /// <inheritdoc />
        public override bool IsGenericParameter => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _typeArguments.Any(i => i.ContainsGenericParameters);

        /// <inheritdoc />
        public override int GenericParameterPosition => throw new NotSupportedException();

        /// <inheritdoc />
        public override bool HasElementType => false;

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
        public override MethodBaseSymbol? DeclaringMethod => _definition.DeclaringMethod;

        /// <inheritdoc />
        public override string? FullName => _definition.FullName;

        /// <inheritdoc />
        public override string? Namespace => _definition.Namespace;

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => _definition.GenericParameterAttributes;

        /// <inheritdoc />
        public override ImmutableList<TypeSymbol> GenericTypeArguments => _definition.GenericTypeArguments;

        /// <inheritdoc />
        public override bool IsPrimitive => _definition.IsPrimitive;

        /// <inheritdoc />
        public override bool IsEnum => _definition.IsEnum;

        /// <inheritdoc />
        public override bool IsSignatureType => _definition.IsSignatureType;

        /// <inheritdoc />
        public override ConstructorSymbol? TypeInitializer => _definition.TypeInitializer;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override TypeSymbol GetEnumUnderlyingType()
        {
            return _definition.GetEnumUnderlyingType().Specialize(new GenericContext(_typeArguments, null));
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType()
        {
            return null;
        }

        /// <inheritdoc />
        public override int GetArrayRank()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetGenericTypeDefinition()
        {
            return _definition;
        }

        /// <inheritdoc />
        public sealed override ImmutableList<TypeSymbol> GetGenericArguments()
        {
            return _typeArguments;
        }

        /// <inheritdoc />
        public override ImmutableList<TypeSymbol> GetGenericParameterConstraints()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public override ImmutableList<FieldSymbol> GetDeclaredFields()
        {
            if (_fields == null)
                Interlocked.CompareExchange(ref _fields, ComputeDeclaredFields(), null);

            return _fields ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Computes the declared fields of this type.
        /// </summary>
        /// <returns></returns>
        ImmutableList<FieldSymbol> ComputeDeclaredFields()
        {
            var b = ImmutableList.CreateBuilder<FieldSymbol>();
            foreach (var i in _definition.GetDeclaredFields())
                b.Add(new ConstructedGenericFieldSymbol(Context, this, i, new GenericContext(_typeArguments, null)));

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override ImmutableList<ConstructorSymbol> GetDeclaredConstructors()
        {
            if (_constructors == null)
                Interlocked.CompareExchange(ref _constructors, ComputeDeclaredConstructors(), null);

            return _constructors ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Computes the declared constructors of this type.
        /// </summary>
        /// <returns></returns>
        ImmutableList<ConstructorSymbol> ComputeDeclaredConstructors()
        {
            var b = ImmutableList.CreateBuilder<ConstructorSymbol>();
            foreach (var i in _definition.GetDeclaredConstructors())
                b.Add(new ConstructedGenericConstructorSymbol(Context, this, i, new GenericContext(_typeArguments, null)));

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override ImmutableList<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods == null)
                Interlocked.CompareExchange(ref _methods, ComputeDeclaredMethods(), null);

            return _methods ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Computes the declared methods of this type.
        /// </summary>
        /// <returns></returns>
        ImmutableList<MethodSymbol> ComputeDeclaredMethods()
        {
            var b = ImmutableList.CreateBuilder<MethodSymbol>();
            foreach (var i in _definition.GetDeclaredMethods())
                b.Add(new ConstructedGenericMethodSymbol(Context, this, i, new GenericContext(_typeArguments, null)));

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override ImmutableList<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties == null)
                Interlocked.CompareExchange(ref _properties, ComputeDeclaredProperties(), null);

            return _properties ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Computes the declared properties of this type.
        /// </summary>
        /// <returns></returns>
        ImmutableList<PropertySymbol> ComputeDeclaredProperties()
        {
            var b = ImmutableList.CreateBuilder<PropertySymbol>();
            foreach (var i in _definition.GetDeclaredProperties())
                b.Add(new ConstructedGenericPropertySymbol(Context, this, i, new GenericContext(_typeArguments, null)));

            return b.ToImmutable();
        }

        /// <inheritdoc />
        public override ImmutableList<EventSymbol> GetDeclaredEvents()
        {
            if (_events == null)
                Interlocked.CompareExchange(ref _events, ComputeDeclaredEvents(), null);

            return _events ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Computes the declared events of this type.
        /// </summary>
        /// <returns></returns>
        ImmutableList<EventSymbol> ComputeDeclaredEvents()
        {
            var b = ImmutableList.CreateBuilder<EventSymbol>();
            foreach (var i in _definition.GetDeclaredEvents())
                b.Add(new ConstructedGenericEventSymbol(Context, this, i, new GenericContext(_typeArguments, null)));

            return b.ToImmutable();
        }

        public override ConstructorSymbol? GetConstructor(BindingFlags bindingFlags, ImmutableList<TypeSymbol>? types)
        {
            return _definition.GetConstructor(bindingFlags, types);
        }

        public override ImmutableList<ConstructorSymbol> GetConstructors(BindingFlags bindingFlags)
        {
            return _definition.GetConstructors(bindingFlags);
        }

        public override ImmutableList<MemberSymbol> GetDefaultMembers()
        {
            return _definition.GetDefaultMembers();
        }

        public override string? GetEnumName(object value)
        {
            return _definition.GetEnumName(value);
        }

        public override ImmutableList<string> GetEnumNames()
        {
            return _definition.GetEnumNames();
        }

        public override EventSymbol? GetEvent(string name, BindingFlags bindingFlags)
        {
            return _definition.GetEvent(name, bindingFlags);
        }

        public override ImmutableList<EventSymbol> GetEvents(BindingFlags bindingFlags)
        {
            return _definition.GetEvents(bindingFlags);
        }

        public override FieldSymbol? GetField(string name, BindingFlags bindingFlags)
        {
            return _definition.GetField(name, bindingFlags);
        }

        public override ImmutableList<FieldSymbol> GetFields(BindingFlags bindingFlags)
        {
            return _definition.GetFields(bindingFlags);
        }

        public override TypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return _definition.GetInterface(name, ignoreCase);
        }

        public override ImmutableList<TypeSymbol> GetInterfaces(bool inherit = true)
        {
            return _definition.GetInterfaces(inherit);
        }

        public override InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            return _definition.GetInterfaceMap(interfaceType);
        }

        public override MethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, CallingConventions callConvention, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers)
        {
            return _definition.GetMethod(name, genericParameterCount, bindingFlags, callConvention, types, modifiers);
        }

        public override ImmutableList<MethodSymbol> GetMethods(BindingFlags bindingFlags)
        {
            return _definition.GetMethods(bindingFlags);
        }

        public override TypeSymbol? GetNestedType(string name, BindingFlags bindingFlags)
        {
            return _definition.GetNestedType(name, bindingFlags);
        }

        public override ImmutableList<TypeSymbol> GetNestedTypes(BindingFlags bindingFlags)
        {
            return _definition.GetNestedTypes(bindingFlags);
        }

        public override PropertySymbol? GetProperty(string name, BindingFlags bindingFlags)
        {
            return _definition.GetProperty(name, bindingFlags);
        }

        public override PropertySymbol? GetProperty(string name, TypeSymbol? returnType, ImmutableList<TypeSymbol>? types)
        {
            return _definition.GetProperty(name, returnType, types);
        }

        public override ImmutableList<PropertySymbol> GetProperties(BindingFlags bindingFlags)
        {
            return _definition.GetProperties(bindingFlags);
        }

        public override bool IsEnumDefined(object value)
        {
            return _definition.IsEnumDefined(value);
        }

        public override CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttribute(attributeType, inherit);
        }

        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return _definition.GetCustomAttributes(inherit);
        }

        public override CustomAttribute[] GetCustomAttributes(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttributes(attributeType, inherit);
        }

        public override bool IsDefined(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.IsDefined(attributeType, inherit);
        }

    }

}
