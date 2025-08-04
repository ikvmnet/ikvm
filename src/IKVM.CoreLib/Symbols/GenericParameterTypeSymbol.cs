using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a parameter type. That is a type which is a generic type or method parameter.
    /// </summary>
    public abstract class GenericParameterTypeSymbol : TypeSymbol
    {

        ImmutableArray<TypeSymbol> _interfaces;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected GenericParameterTypeSymbol(SymbolContext context) :
            base(context)
        {

        }
        
        /// <inheritdoc />
        public sealed override ModuleSymbol Module => DeclaringType?.Module ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <inheritdoc />
        public override string? FullName => null;

        /// <inheritdoc />
        public override string? Namespace => "";

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => true;

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeCode.Object;

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => false;

        /// <inheritdoc />
        public sealed override bool IsPrimitive => false;

        /// <inheritdoc />
        public sealed override bool IsEnum => false;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissingType => GenericParameterConstraints.Any(i => i.ContainsMissingType);

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => ComputeBaseType();

        /// <summary>
        /// Computes the base type of the symbol.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        TypeSymbol? ComputeBaseType()
        {
            foreach (var constraint in GenericParameterConstraints)
                if (constraint.IsInterface == false)
                    return constraint;

            return Context.ResolveCoreType("System.Object");
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType() => null;

        /// <inheritdoc />
        public sealed override int GetArrayRank() => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value) => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames() => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType() => throw new InvalidOperationException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
            {
                var interfaces = ImmutableArray.CreateBuilder<TypeSymbol>(GenericParameterConstraints.Length);

                foreach (var constraint in GenericParameterConstraints)
                    if (constraint.IsInterface)
                        interfaces.Add(constraint);

                return _interfaces = interfaces.DrainToImmutable();
            }
            else
            {
                return _interfaces = [];
            }
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields() => [];

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods() => [];

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations() => throw new NotImplementedException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties() => [];

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents() => [];

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameters => [];

        /// <inheritdoc />
        public sealed override TypeSymbol GenericTypeDefinition => throw new InvalidOperationException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes() => [];

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value) => throw new InvalidOperationException();

    }

}
