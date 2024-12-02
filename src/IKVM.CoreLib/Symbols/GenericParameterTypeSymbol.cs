using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    public abstract class GenericParameterTypeSymbol : TypeSymbol
    {

        readonly TypeSymbol _declaringType;

        ImmutableArray<TypeSymbol> _constraints;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected GenericParameterTypeSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol declaringType) :
            base(context, module)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => TypeAttributes.Public;

        /// <inheritdoc />
        public override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public override MethodSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => false;

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
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissingType => GenericParameterConstraints.Any(i => i.ContainsMissingType);

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
        public sealed override string? GetEnumName(object value)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            return ImmutableArray<FieldSymbol>.Empty;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            return ImmutableArray<MethodSymbol>.Empty;
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            throw new NotImplementedException();
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
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => [];

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => ComputeGenericParameterConstraints();

        /// <inheritdoc />
        ImmutableArray<TypeSymbol> ComputeGenericParameterConstraints()
        {
            if (_constraints.IsDefault)
            {
                var interfaces = GetDeclaredInterfaces();

                if (BaseType != null)
                {
                    var b = ImmutableArray.CreateBuilder<TypeSymbol>(interfaces.Length + 1);
                    b.Add(BaseType);
                    for (int i = 0; i < interfaces.Length; i++)
                        b.Add(interfaces[i]);

                    ImmutableInterlocked.InterlockedInitialize(ref _constraints, b.DrainToImmutable());
                }
                else
                {
                    ImmutableInterlocked.InterlockedInitialize(ref _constraints, interfaces);
                }
            }

            return _constraints;
        }

        /// <summary>
        /// Invoke this method upon altering the base type or interfaces to clear any cached generic parameter constraints.
        /// </summary>
        protected void ClearGenericParameterConstraints()
        {
            _constraints = default;
        }

        public override TypeSymbol GenericTypeDefinition => throw new InvalidOperationException();

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            throw new InvalidOperationException();
        }

    }

}
