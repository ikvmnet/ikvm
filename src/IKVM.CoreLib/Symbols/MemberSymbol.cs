using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;

namespace IKVM.CoreLib.Symbols
{

    abstract class MemberSymbol : Symbol, ICustomAttributeProviderInternal
    {

        readonly ModuleSymbol _module;
        readonly TypeSymbol? _declaringType;

        CustomAttributeImpl _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        public MemberSymbol(SymbolContext context, ModuleSymbol module, TypeSymbol? declaringType) :
            base(context)
        {
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _declaringType = declaringType;
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
        ImmutableArray<CustomAttribute> ICustomAttributeProviderInternal.GetDeclaredCustomAttributes() => GetDeclaredCustomAttributes();

        /// <inheritdoc />
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
        public override string ToString() => Name;

    }

}
