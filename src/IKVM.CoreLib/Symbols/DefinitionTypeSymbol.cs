using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type definition.
    /// </summary>
    sealed class DefinitionTypeSymbol : TypeSymbol
    {

        readonly ITypeLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionTypeSymbol(SymbolContext context, ITypeLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public ITypeLoader Loader => _loader;

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override ModuleSymbol Module => _loader.GetModule();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override string? Namespace => _loader.GetNamespace();

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => null;

        /// <inheritdoc />
        public sealed override bool IsTypeDefinition => true;

        /// <inheritdoc />
        public sealed override bool IsArray => false;

        /// <inheritdoc />
        public sealed override bool IsByRef => false;

        /// <inheritdoc />
        public sealed override bool IsConstructedGenericType => false;

        /// <inheritdoc />
        public sealed override bool IsFunctionPointer => false;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override bool HasElementType => false;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeParameter => false;

        /// <inheritdoc />
        public sealed override bool IsGenericMethodParameter => false;

        /// <inheritdoc />
        public sealed override bool IsPointer => false;

        /// <inheritdoc />
        public sealed override bool IsSZArray => false;

        /// <inheritdoc />
        public sealed override bool IsUnmanagedFunctionPointer => false;

        /// <inheritdoc />
        public sealed override TypeSymbol? GetElementType() => null;

        /// <inheritdoc />
        public sealed override TypeSymbol GenericTypeDefinition => throw new InvalidOperationException();

        /// <inheritdoc />
        public sealed override TypeAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeSymbolExtensions.GetTypeCode(this);

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => _loader.GetBaseType();

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => _loader.GetGenericArguments().Length > 0;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _loader.GetGenericParameterAttributes();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameters => _loader.GetGenericArguments();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => _loader.GetGenericParameterConstraints();

        /// <inheritdoc />
        public sealed override bool IsPrimitive => TypeCode is TypeCode.Boolean or TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Char or TypeCode.Double or TypeCode.Single || this == Context.ResolveCoreType("System.IntPtr") || this == Context.ResolveCoreType("System.UIntPtr");

        /// <inheritdoc />
        public sealed override bool IsEnum => BaseType != null && BaseType == Context.ResolveCoreType("System.Enum");

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override int GetArrayRank() => throw new ArgumentException("Must be an array type.");

        /// <inheritdoc />
        public sealed override string? GetEnumName(object value)
        {
            if (!IsEnum)
                throw new ArgumentException();
            if (value == null)
                throw new ArgumentNullException();

            try
            {
                value = Convert.ChangeType(value, TypeSymbolExtensions.GetSystemType(GetEnumUnderlyingType()));
            }
            catch (FormatException)
            {
                throw new ArgumentException();
            }
            catch (OverflowException)
            {
                return null;
            }
            catch (InvalidCastException)
            {
                return null;
            }

            foreach (var field in GetDeclaredFields())
                if (field.IsLiteral && field.GetRawConstantValue() is { } v && v.Equals(value))
                    return field.Name;

            return null;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<string> GetEnumNames()
        {
            if (!IsEnum)
                throw new ArgumentException();

            var names = ImmutableArray.CreateBuilder<string>();
            foreach (var field in GetDeclaredFields())
                if (field.IsLiteral)
                    names.Add(field.Name);

            return names.ToImmutable();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetEnumUnderlyingType()
        {
            if (!IsEnum)
                throw new ArgumentException();

            foreach (var field in GetDeclaredFields())
                if (!field.IsStatic)
                    return field.FieldType;

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public sealed override bool IsEnumDefined(object value)
        {
            if (value is string s)
                return GetEnumNames().IndexOf(s) != -1;
            if (IsEnum == false)
                throw new ArgumentException();
            if (value == null)
                throw new ArgumentNullException();
            if (value.GetType() != TypeSymbolExtensions.GetSystemType(GetEnumUnderlyingType()))
                throw new ArgumentException();

            foreach (var field in GetDeclaredFields())
                if (field.IsLiteral && field.GetRawConstantValue() is { } v && v.Equals(value))
                    return true;

            return false;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<EventSymbol> GetDeclaredEvents() => _loader.GetEvents();

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields() => _loader.GetFields();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces() => _loader.GetInterfaces();

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods() => _loader.GetMethods();

        /// <inheritdoc />
        internal sealed override MethodImplementationMapping GetMethodImplementations() => _loader.GetMethodImplementations();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes() => _loader.GetNestedTypes();

        /// <inheritdoc />
        internal sealed override ImmutableArray<PropertySymbol> GetDeclaredProperties() => _loader.GetProperties();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

        /// <inheritdoc />
        internal sealed override TypeSymbol Specialize(GenericContext genericContext)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GenericParameters;
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(genericContext));

            return MakeGenericType(args);

        }

    }

}
