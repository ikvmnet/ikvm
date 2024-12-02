using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Performs reflection on a module.
    /// </summary>
    public abstract class ModuleSymbol : Symbol, ICustomAttributeProviderInternal
    {

        readonly AssemblySymbol _assembly;

        CustomAttributeImpl _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        protected ModuleSymbol(SymbolContext context, AssemblySymbol assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _customAttributes = new CustomAttributeImpl(context, this);
        }

        /// <summary>
        /// Gets the appropriate <see cref="AssemblySymbol"> for this instance of Module.
        /// </summary>
        public AssemblySymbol Assembly => _assembly;

        /// <summary>
        /// Gets a string representing the fully qualified name and path to this module.
        /// </summary>
        public abstract string FullyQualifiedName { get; }

        /// <summary>
        /// Gets a <see cref="string"/> representing the name of the module with the path removed.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Gets a string representing the name of the module.
        /// </summary>
        public abstract string ScopeName { get; }

        /// <summary>
        /// Gets a universally unique identifier (UUID) that can be used to distinguish between two versions of a module.
        /// </summary>
        public abstract Guid ModuleVersionId { get; }

        /// <summary>
        /// Returns <c>true</c> if the symbol is missing.
        /// </summary>
        public abstract bool IsMissing { get; }

        /// <summary>
        /// Gets a value indicating whether the object is a resource.
        /// </summary>
        /// <returns></returns>
        public abstract bool IsResource();

        /// <summary>
        /// Returns the fields declared on the module.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<FieldSymbol> GetDeclaredFields();

        /// <summary>
        /// Returns a field having the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldSymbol? GetField(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a field having the specified name and binding attributes.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public FieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the global fields defined on the module.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FieldSymbol> GetFields() => GetFields(DefaultLookup);

        /// <summary>
        /// Returns the global fields defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<FieldSymbol> GetFields(BindingFlags bindingFlags)
        {
            return new MemberQuery<ModuleSymbol, FieldSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns the methods declared on the module.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<MethodSymbol> GetDeclaredMethods();

        /// <summary>
        /// Returns a method having the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name)
        {
            return GetMethod(name, default);
        }

        /// <summary>
        /// Returns a method having the specified name and parameter types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, ImmutableArray<TypeSymbol> types)
        {
            return GetMethod(name, DefaultLookup, CallingConventions.Any, types, default);
        }

        /// <summary>
        /// Returns a method having the specified name, binding information, calling convention, and parameter types and modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ImmutableArray<TypeSymbol> types, ImmutableArray<ParameterModifier> modifiers)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the global methods defined on the module.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetMethods() => GetMethods(DefaultLookup);

        /// <summary>
        /// Returns the global methods defined on the module that match the specified binding flags.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetMethods(BindingFlags bindingFlags)
        {
            return new MemberQuery<ModuleSymbol, MethodSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns the types declared on the module.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<TypeSymbol> GetDeclaredTypes();

        /// <summary>
        /// Returns the specified type, performing a case-sensitive search.
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public TypeSymbol? GetType(string className)
        {
            return GetType(className, false);
        }

        /// <summary>
        /// Returns the specified type, specifying whether to make a case-sensitive search of the module and whether to throw an exception if the type cannot be found.
        /// </summary>
        /// <param name="className"></param>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public TypeSymbol? GetType(string className, bool throwOnError)
        {
            foreach (var type in GetDeclaredTypes())
                if (type.IsNested == false && type.FullName == className)
                    return type;

            if (throwOnError)
                throw new TypeLoadException();

            return null;
        }

        /// <summary>
        /// Returns all the types defined within this module.
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<TypeSymbol> GetTypes()
        {
            var b = GetDeclaredTypes();

            foreach (var type in b)
                if (type.IsNested)
                    b = b.Remove(type);

            return b;
        }

        /// <inheritdoc />
        internal abstract ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ImmutableArray<CustomAttribute> ICustomAttributeProviderInternal.GetDeclaredCustomAttributes() => GetDeclaredCustomAttributes();

        /// <inheritdoc />
        ICustomAttributeProviderInternal? ICustomAttributeProviderInternal.GetInheritedCustomAttributeProvider() => null;

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
            return ScopeName;
        }

    }

}
