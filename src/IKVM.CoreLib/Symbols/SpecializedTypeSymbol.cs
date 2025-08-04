using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type that is a constructed generic type.
    /// </summary>
    class SpecializedTypeSymbol : TypeSymbol
    {

        readonly TypeSymbol _definition;
        readonly GenericContext _genericContext;

        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol?> _baseType;
        LazyField<TypeSymbol?> _elementType;
        ImmutableArray<TypeSymbol> _genericParameters;
        ImmutableArray<TypeSymbol> _genericParameterConstraints;
        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<PropertySymbol> _properties;
        ImmutableArray<EventSymbol> _events;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        MethodImplementationMapping _methodImpl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpecializedTypeSymbol(SymbolContext context, TypeSymbol definition, GenericContext genericContext) :
            base(context)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _definition.Module;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(ComputeDeclaringType()) : _declaringType.Value;

        /// <summary>
        /// Computes the value for <see cref="DeclaringType"/>.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? ComputeDeclaringType() => _definition.DeclaringType?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _definition.Namespace;

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => _definition.TypeCode;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _baseType.IsDefault ? _baseType.InterlockedInitialize(_definition.BaseType?.Specialize(_genericContext)) : _baseType.Value;

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
        public sealed override bool ContainsGenericParameters => GenericParameters.Any(i => i.ContainsGenericParameters);

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
        public sealed override MethodSymbol? DeclaringMethod => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _definition.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => _definition.IsPrimitive;

        /// <inheritdoc />
        public sealed override bool IsVisible => base.IsVisible && GenericParameters.All(i => i.IsVisible);

        /// <inheritdoc />
        public sealed override bool IsEnum => _definition.IsEnum;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissingType => _definition.ContainsMissingType || GenericParameters.Any(i => i.ContainsMissingType);

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            return _definition.GetEnumUnderlyingType().Specialize(_genericContext);
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType() => _elementType.IsDefault ? _elementType.InterlockedInitialize(ComputeElementType()) : _elementType.Value;

        /// <summary>
        /// Computes the value of <see cref="GetElementType"/>.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? ComputeElementType() => _definition.GetElementType()?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override int GetArrayRank() => _definition.GetArrayRank();

        /// <inheritdoc />
        public override TypeSymbol GenericTypeDefinition => _definition;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GenericParameters => ComputeGenericParameters();

        /// <summary>
        /// Computes the value for <see cref="GenericParameters"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeGenericParameters()
        {
            if (_genericParameters.IsDefault)
            {
                var l = _definition.GenericParameters;
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(l[i].Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _genericParameters, b.DrainToImmutable());
            }

            return _genericParameters;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => ComputeGenericParameterConstraints();

        /// <summary>
        /// Computes the values for <see cref="GenericParameterConstraints"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<TypeSymbol> ComputeGenericParameterConstraints()
        {
            if (_genericParameterConstraints.IsDefault)
            {
                var l = _definition.GenericParameterConstraints;
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _genericParameterConstraints, b.DrainToImmutable());
            }

            return _genericParameterConstraints;
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var l = _definition.GetDeclaredFields();
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedFieldSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var l = _definition.GetDeclaredMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedMethodSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException();

            //if (_methodImpl.Type == null)
            //{
            //    var m = _definition.GetMethodImplementations();
            //    var impl = ImmutableArray.CreateBuilder<MethodSymbol>(m.Implementations.Length);
            //    var decl = ImmutableArray.CreateBuilder<ImmutableArray<MethodSymbol>>(m.Implementations.Length);
            //    for (int i = 0; i < m.Implementations.Length; i++)
            //    {
            //        impl.Add(Specialize(m.Implementations[i]) ?? throw new InvalidOperationException());

            //        var declBuilder = ImmutableArray.CreateBuilder<MethodSymbol>(m.Declarations[i].Length);
            //        for (int j = 0; j < m.Declarations[i].Length; j++)
            //            declBuilder.Add(Specialize(m.Declarations[i][j]) ?? throw new InvalidOperationException());

            //        decl.Add(declBuilder.DrainToImmutable());
            //    }

            //    lock (this)
            //        if (_methodImpl.Type == null)
            //            _methodImpl = new MethodImplementationMapping(this, impl.DrainToImmutable(), decl.DrainToImmutable());
            //}

            //return _methodImpl;
        }

        /// <inheritdoc />
        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            if (_properties.IsDefault)
            {
                var l = _definition.GetDeclaredProperties();
                var b = ImmutableArray.CreateBuilder<PropertySymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedPropertySymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _properties, b.DrainToImmutable());
            }

            return _properties;
        }

        /// <inheritdoc />
        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            if (_events.IsDefault)
            {
                var l = _definition.GetDeclaredEvents();
                var b = ImmutableArray.CreateBuilder<EventSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new SpecializedEventSymbol(Context, this, i, _genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _events, b.DrainToImmutable());
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
        internal override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
            {
                var l = _definition.GetDeclaredInterfaces();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, b.DrainToImmutable());
            }

            return _interfaces;
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            return _definition.IsEnumDefined(value);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
            {
                var l = _definition.GetOptionalCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, b.DrainToImmutable());
            }

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
            {
                var l = _definition.GetRequiredCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, b.DrainToImmutable());
            }

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override TypeSymbol Specialize(GenericContext genericContext)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GenericParameters;
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(genericContext));

            return _definition.MakeGenericType(args);
        }

    }

}
