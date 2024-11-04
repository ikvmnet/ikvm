using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class ParameterSymbol : ICustomAttributeProvider
    {

        readonly ISymbolContext _context;
        readonly MemberSymbol _member;
        readonly int _position;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public ParameterSymbol(ISymbolContext context, MemberSymbol member, int position)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _position = position;
        }

        public ISymbolContext Context => _context;

        /// <summary>
        /// Gets the attributes for this parameter.
        /// </summary>
        public abstract ParameterAttributes Attributes { get; }

        /// <summary>
        /// Gets a value indicating the member in which the parameter is implemented.
        /// </summary>
        public MemberSymbol Member => _member;

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
        public abstract ImmutableList<TypeSymbol> GetOptionalCustomModifiers();

        /// <inheritdoc />
        public abstract ImmutableList<TypeSymbol> GetRequiredCustomModifiers();

        /// <inheritdoc />
        public abstract CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false);

        /// <inheritdoc />
        public abstract CustomAttribute[] GetCustomAttributes(bool inherit = false);

        /// <inheritdoc />
        public abstract CustomAttribute[] GetCustomAttributes(TypeSymbol attributeType, bool inherit = false);

        /// <inheritdoc />
        public abstract bool IsDefined(TypeSymbol attributeType, bool inherit = false);

    }

}
