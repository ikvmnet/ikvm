using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class MemberSymbol : ICustomAttributeProvider
    {

        readonly ISymbolContext _context;
        readonly IModuleSymbol _module;
        readonly TypeSymbol? _declaringType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        public MemberSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _declaringType = declaringType;
        }

        public ISymbolContext Context => _context;

        /// <summary>
        /// Gets the module in which the type that declares the member represented by the current <see cref="IMemberSymbol"> is defined.
        /// </summary>
        public IModuleSymbol Module => _module;

        /// <summary>
        /// Gets the appropriate <see cref="IAssemblySymbol"/> for this instance.
        /// </summary>
        public IAssemblySymbol Assembly => _module.Assembly;

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public TypeSymbol? DeclaringType => _declaringType;

        /// <summary>
        /// When overridden in a derived class, gets a <see cref="MemberTypes"> value indicating the type of the member - method, constructor, event, and so on.
        /// </summary>
        public abstract MemberTypes MemberType { get; }

        /// <summary>
        /// Gets the name of the current member.
        /// </summary>
        public abstract string Name { get; }

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
        public abstract CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false);

        /// <inheritdoc />
        public abstract CustomAttribute[] GetCustomAttributes(bool inherit = false);

        /// <inheritdoc />
        public abstract CustomAttribute[] GetCustomAttributes(TypeSymbol attributeType, bool inherit = false);

        /// <inheritdoc />
        public abstract bool IsDefined(TypeSymbol attributeType, bool inherit = false);

    }

}
