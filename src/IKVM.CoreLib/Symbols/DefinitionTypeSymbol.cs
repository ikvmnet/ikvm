using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type definition.
    /// </summary>
    class DefinitionTypeSymbol : TypeSymbol
    {

        readonly string _name;
        readonly string _namespace;

        TypeDefinition? _def;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="name"></param>
        /// <param name="ns"></param>
        /// <param name="def"></param>
        public DefinitionTypeSymbol(SymbolContext context, ModuleSymbol module, string name, string ns, TypeDefinition? def) :
            base(context, module)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _namespace = ns ?? throw new ArgumentNullException(nameof(ns));
            _def = def;
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        TypeDefinition? Def => GetDefinition();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        TypeDefinition? GetDefinition()
        {
            if (_def is null)
                Interlocked.CompareExchange(ref _def, Context.ResolveTypeSource(this), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        TypeDefinition DefOrThrow => Def ?? throw new MissingTypeSymbolException(this);

        /// <inheritdoc />
        public override bool IsMissing => Def == null;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override string? Namespace => _namespace;

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
        public sealed override TypeAttributes Attributes => DefOrThrow.GetAttributes();

        /// <inheritdoc />
        public sealed override TypeCode TypeCode => TypeSymbolExtensions.GetTypeCode(this);

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => DefOrThrow.GetBaseType();

        /// <inheritdoc />
        public sealed override bool ContainsGenericParameters => DefOrThrow.GetGenericArguments().Length > 0;

        /// <inheritdoc />
        public sealed override bool IsGenericTypeDefinition => ContainsGenericParameters;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => DefOrThrow.GetGenericParameterAttributes();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericArguments => DefOrThrow.GetGenericArguments();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints => DefOrThrow.GetGenericParameterConstraints();

        /// <inheritdoc />
        public sealed override bool IsPrimitive => TypeCode is TypeCode.Boolean or TypeCode.Byte or TypeCode.SByte or TypeCode.Int16 or TypeCode.UInt16 or TypeCode.Int32 or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Char or TypeCode.Double or TypeCode.Single || this == Context.ResolveCoreType("System.IntPtr") || this == Context.ResolveCoreType("System.UIntPtr");

        /// <inheritdoc />
        public sealed override bool IsEnum => BaseType != null && BaseType == Context.ResolveCoreType("System.Enum");

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => DefOrThrow.GetDeclaringType();

        /// <inheritdoc />
        public sealed override int GetArrayRank() => throw new ArgumentException("Must be an array type.");

        /// <inheritdoc />
        public override string? GetEnumName(object value)
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
        public override ImmutableArray<string> GetEnumNames()
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
        public override TypeSymbol GetEnumUnderlyingType()
        {
            if (!IsEnum)
                throw new ArgumentException();

            foreach (var field in GetDeclaredFields())
                if (!field.IsStatic)
                    return field.FieldType;

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public override bool IsEnumDefined(object value)
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
        internal override ImmutableArray<EventSymbol> GetDeclaredEvents()
        {
            return DefOrThrow.GetEvents();
        }

        /// <inheritdoc />
        internal override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            return DefOrThrow.GetFields();
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            return DefOrThrow.GetInterfaces();
        }

        /// <inheritdoc />
        internal override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            return DefOrThrow.GetMethods();
        }

        /// <inheritdoc />
        internal override MethodImplementationMapping GetMethodImplementations()
        {
            return DefOrThrow.GetMethodImplementations();
        }

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredNestedTypes()
        {
            return DefOrThrow.GetNestedTypes();
        }

        /// <inheritdoc />
        internal override ImmutableArray<PropertySymbol> GetDeclaredProperties()
        {
            return DefOrThrow.GetProperties();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return DefOrThrow.GetRequiredCustomModifiers();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return DefOrThrow.GetOptionalCustomModifiers();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return DefOrThrow.GetCustomAttributes();
        }

        /// <inheritdoc />
        internal override TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GenericArguments;
            for (int i = 0; i < args.Length; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(context));

            return MakeGenericType(args);
        }

        /// <summary>
        /// Attempts to resolve the definition for the specified nested type.
        /// </summary>
        /// <param name="name"></param>
        internal TypeDefinition? ResolveNestedTypeDef(string name) => Def?.ResolveNestedTypeDef(name);

        /// <summary>
        /// Attempts to resolve the definition for the specified field.
        /// </summary>
        /// <param name="name"></param>
        internal FieldDefinition? ResolveFieldDef(string name) => Def?.ResolveFieldDef(name);

        /// <summary>
        /// Attempts to resolve the definition for the specified method.
        /// </summary>
        /// <param name="signature"></param>
        internal MethodDefinition? ResolveMethodDef(MethodSymbolSignature signature) => Def?.ResolveMethodDef(signature);

        /// <summary>
        /// Attempts to resolve the definition for the specified property.
        /// </summary>
        /// <param name="name"></param>
        internal PropertyDefinition? ResolvePropertyDef(string name) => Def?.ResolvePropertyDef(name);

        /// <summary>
        /// Attempts to resolve the definition for the specified event.
        /// </summary>
        /// <param name="name"></param>
        internal EventDefinition? ResolveEventDef(string name) => Def?.ResolveEventDef(name);

    }

}
