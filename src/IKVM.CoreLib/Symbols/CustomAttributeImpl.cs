using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides implementations in support of <see cref="ICustomAttributeProvider"/>.
    /// </summary>
    struct CustomAttributeImpl
    {

        readonly SymbolContext _context;
        readonly ICustomAttributeProviderInternal _provider;

        TypeSymbol? _attributeUsageAttributeType;
        ImmutableArray<CustomAttribute> _declaredCustomAttributes;
        ImmutableArray<CustomAttribute> _declaredAndInheritedCustomAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public CustomAttributeImpl(SymbolContext context, ICustomAttributeProviderInternal provider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _provider = provider ?? throw new System.ArgumentNullException(nameof(provider));
        }

        /// <summary>
        /// Returns the custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public ImmutableArray<CustomAttribute> GetCustomAttributes(bool inherit)
        {
            if (inherit == false)
            {
                if (_declaredCustomAttributes == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _declaredCustomAttributes, _provider.GetDeclaredCustomAttributes());

                return _declaredCustomAttributes;
            }
            else
            {
                if (_declaredAndInheritedCustomAttributes == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _declaredAndInheritedCustomAttributes, ComputeInheritedCustomAttributes());

                return _declaredAndInheritedCustomAttributes;
            }
        }

        /// <summary>
        /// Computes the custom attributes that are applied to this member, including those which are inherited.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> ComputeInheritedCustomAttributes()
        {
            var list = _provider.GetDeclaredCustomAttributes();

            // move through the inherited custom attribute providers
            for (var provider = _provider.GetInheritedCustomAttributeProvider(); provider != null; provider = provider.GetInheritedCustomAttributeProvider())
                foreach (var customAttribute in provider.GetDeclaredCustomAttributes())
                    if (IsInheritable(customAttribute))
                        list = list.Add(customAttribute);

            return list;
        }

        /// <summary>
        /// Returns <c>true</c> if the specified <see cref="CustomAttribute"/> is inherited.
        /// </summary>
        /// <param name="customAttribute"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        bool IsInheritable(CustomAttribute customAttribute)
        {
            _attributeUsageAttributeType ??= _context.ResolveCoreType(typeof(AttributeUsageAttribute).FullName!);
            if (_attributeUsageAttributeType == null)
                throw new InvalidOperationException("Could not find core type System.AttributeUsageAttribute.");

            // find first AttributeUsageAttribute
            var attributeUsageAttribute = customAttribute.AttributeType.GetCustomAttribute(_attributeUsageAttributeType, true);
            if (attributeUsageAttribute.HasValue == false)
                return false;

            // return whether the Inherited property is set
            return GetInheritedValue(attributeUsageAttribute.Value);
        }

        /// <summary>
        /// Gets the boolean value of the <see cref="AttributeUsageAttribute.Inherited"/> property.
        /// </summary>
        /// <param name="attributeUsageAttribute"></param>
        /// <returns></returns>
        bool GetInheritedValue(CustomAttribute attributeUsageAttribute)
        {
            foreach (var i in attributeUsageAttribute.NamedArguments)
                if (i.IsField && i.MemberName == nameof(AttributeUsageAttribute.Inherited) && (bool?)i.TypedValue.Value == true)
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the custom attributes applied to this member.
        /// </summary>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public ImmutableArray<CustomAttribute> GetCustomAttributes(TypeSymbol attributeType, bool inherit)
        {
            return GetCustomAttributes(inherit).Where(i => i.AttributeType == attributeType).ToImmutableArray();

        }

        /// <summary>
        /// Retrieves a custom attribute of a specified type applied to this member.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit)
        {
            return GetCustomAttributes(attributeType, inherit).SingleOrDefaultOrThrow(() => new AmbiguousMatchException());
        }

        /// <summary>
        /// Determines whether any custom attributes of a specified type are applied to an assembly, module, type member, or method parameter.
        /// </summary>
        /// <param name="attributeType"></param>
        /// <param name="inherit"></param>
        /// <returns></returns>
        public bool IsDefined(TypeSymbol attributeType, bool inherit)
        {
            return GetCustomAttributes(attributeType, inherit).Any();
        }

    }

}
