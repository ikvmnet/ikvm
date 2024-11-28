using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    public abstract class MemberSymbol : Symbol, ICustomAttributeProviderInternal
    {

        readonly ModuleSymbol _module;

        CustomAttributeImpl _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        protected MemberSymbol(SymbolContext context, ModuleSymbol module) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _customAttributes = new CustomAttributeImpl(context, this);
        }

        /// <summary>
        /// Gets the module in which the type that declares the member represented by the current <see cref="MemberSymbol"> is defined.
        /// </summary>
        public ModuleSymbol Module => _module;

        /// <summary>
        /// Gets the appropriate <see cref="AssemblySymbol"/> for this instance.
        /// </summary>
        public AssemblySymbol Assembly => _module.Assembly;

        /// <summary>
        /// Gets the class that declares this member.
        /// </summary>
        public abstract TypeSymbol? DeclaringType { get; }

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
        /// Returns <c>true</c> if the symbol is complete. That is, was created with a builder and completed.
        /// </summary>
        public abstract bool IsComplete { get; }

        /// <inheritdoc />
        ImmutableArray<CustomAttribute> ICustomAttributeProviderInternal.GetDeclaredCustomAttributes() => GetDeclaredCustomAttributes();

        /// <summary>
        /// Gets the custom attributes on this provider.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ICustomAttributeProviderInternal? ICustomAttributeProviderInternal.GetInheritedCustomAttributeProvider() => GetInheritedCustomAttributeProvider();

        /// <summary>
        /// Override to return a inherited custom attribute provider.
        /// </summary>
        /// <returns></returns>
        internal virtual ICustomAttributeProviderInternal? GetInheritedCustomAttributeProvider() => null;

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(bool inherit = false) => _customAttributes.GetCustomAttributes(inherit);

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> GetCustomAttributes(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttributes(attributeType, inherit);

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false) => _customAttributes.GetCustomAttribute(attributeType, inherit);

        /// <inheritdoc />
        public bool IsDefined(TypeSymbol attributeType, bool inherit = false) => _customAttributes.IsDefined(attributeType, inherit);

        /// <inheritdoc />
        public override string? ToString() => Name;

    }

}
