using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    public abstract class ParameterSymbol : ICustomAttributeProviderInternal
    {

        readonly SymbolContext _context;
        readonly MemberSymbol _declaringMember;
        readonly int _position;

        CustomAttributeImpl _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ParameterSymbol(SymbolContext context, MemberSymbol declaringMember, int position)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _declaringMember = declaringMember ?? throw new ArgumentNullException(nameof(declaringMember));
            _position = position;
            _customAttributes = new CustomAttributeImpl(context, this);
        }

        /// <summary>
        /// Gets the context which owns this parameter.
        /// </summary>
        public SymbolContext Context => _context;

        /// <summary>
        /// Gets the attributes for this parameter.
        /// </summary>
        public abstract ParameterAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        public MemberSymbol Member => _declaringMember;

        /// <summary>
        /// Gets the Type of this parameter.
        /// </summary>
        public abstract TypeSymbol ParameterType { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        public abstract string? Name { get; }

        /// <summary>
        /// Gets the zero-based position of the parameter in the formal parameter list.
        /// </summary>
        public int Position => _position;

        /// <summary>
        /// Gets a value that indicates whether this parameter has a default value.
        /// </summary>
        public bool HasDefaultValue => (Attributes & ParameterAttributes.HasDefault) == ParameterAttributes.HasDefault;

        /// <summary>
        /// Gets a value indicating the default value if the parameter has a default value.
        /// </summary>
        public abstract object? DefaultValue { get; }

        /// <summary>
        /// Gets a value indicating whether this is an input parameter.
        /// </summary>
        public bool IsIn => (Attributes & ParameterAttributes.In) == ParameterAttributes.In;

        /// <summary>
        /// Gets a value indicating whether this is an output parameter.
        /// </summary>
        public bool IsOut => (Attributes & ParameterAttributes.Out) == ParameterAttributes.Out;

        /// <summary>
        /// Gets a value indicating whether this parameter is optional.
        /// </summary>
        public bool IsOptional => (Attributes & ParameterAttributes.Optional) == ParameterAttributes.Optional;

        /// <summary>
        /// Gets a value indicating whether this parameter is a locale identifier (lcid).
        /// </summary>
        public bool IsLcid => (Attributes & ParameterAttributes.Lcid) == ParameterAttributes.Lcid;

        /// <summary>
        /// Gets a value indicating whether this is a Retval parameter.
        /// </summary>
        public bool IsRetval => (Attributes & ParameterAttributes.Retval) == ParameterAttributes.Retval;

        /// <summary>
        /// Returns <c>true</c> if the symbol is missing.
        /// </summary>
        public abstract bool IsMissing { get; }

        /// <summary>
        /// Returns <c>true</c> if the symbol contains a missing symbol.
        /// </summary>
        public abstract bool ContainsMissing { get; }

        /// <summary>
        /// Returns <c>true</c> if the symbol is complete. That is, was created with a builder and completed.
        /// </summary>
        public abstract bool IsComplete { get; }

        /// <inheritdoc />
        public abstract ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <inheritdoc />
        public abstract ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

        /// <inheritdoc />
        ImmutableArray<CustomAttribute> ICustomAttributeProviderInternal.GetDeclaredCustomAttributes() => GetDeclaredCustomAttributes();

        /// <inheritdoc />
        internal abstract ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ICustomAttributeProviderInternal? ICustomAttributeProviderInternal.GetInheritedCustomAttributeProvider() => GetInheritedCustomAttributeProvider();

        /// <summary>
        /// Override to return a inherited custom attribute provider.
        /// </summary>
        /// <returns></returns>
        protected virtual ICustomAttributeProviderInternal? GetInheritedCustomAttributeProvider() => null;

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(bool inherit = false) => _customAttributes.GetCustomAttributes(inherit);

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttributes(attributeType, inherit);

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttribute(attributeType, inherit);

        /// <inheritdoc />
        public bool IsDefined(TypeSymbol attributeType, bool inherit = false) => _customAttributes.IsDefined(attributeType, inherit);

        /// <inheritdoc />
        public override string? ToString()
        {
            var typeName = ParameterType.FormatTypeName();
            var name = Name;
            return name is null ? typeName : typeName + " " + name;
        }

    }

}
