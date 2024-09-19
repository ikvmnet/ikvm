using System;
using System.Linq;

using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class IkvmReflectionModuleSymbol : IkvmReflectionSymbol, IIkvmReflectionModuleSymbol
    {

        readonly IIkvmReflectionAssemblySymbol _resolvingAssembly;
        readonly Module _module;
        IkvmReflectionModuleMetadata _impl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionAssemblySymbol resolvingAssembly, Module module) :
            base(context)
        {
            _resolvingAssembly = resolvingAssembly ?? throw new ArgumentNullException(nameof(resolvingAssembly));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _impl = new IkvmReflectionModuleMetadata(this);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => _module;

        /// <inheritdoc />
        public IIkvmReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

        #region IModuleSymbol

        /// <inheritdoc />
        public IAssemblySymbol Assembly => ResolveAssemblySymbol(UnderlyingModule.Assembly);

        /// <inheritdoc />
        public string FullyQualifiedName => UnderlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public int MetadataToken => UnderlyingModule.MetadataToken;

        /// <inheritdoc />
        public Guid ModuleVersionId => UnderlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public string Name => UnderlyingModule.Name;

        /// <inheritdoc />
        public string ScopeName => UnderlyingModule.ScopeName;

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields((BindingFlags)bindingFlags))!;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields())!;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods())!;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods((BindingFlags)bindingFlags))!;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className, throwOnError, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(UnderlyingModule.GetTypes())!;
        }

        /// <inheritdoc />
        public bool IsResource()
        {
            return UnderlyingModule.IsResource();
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken)
        {
            return ResolveFieldSymbol(UnderlyingModule.ResolveField(metadataToken));
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveFieldSymbol(UnderlyingModule.ResolveField(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken)
        {
            return ResolveMemberSymbol(UnderlyingModule.ResolveMember(metadataToken));
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveMemberSymbol(UnderlyingModule.ResolveMember(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveMethodBaseSymbol(UnderlyingModule.ResolveMethod(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken)
        {
            return ResolveMethodBaseSymbol(UnderlyingModule.ResolveMethod(metadataToken));
        }

        /// <inheritdoc />
        public byte[] ResolveSignature(int metadataToken)
        {
            return UnderlyingModule.ResolveSignature(metadataToken);
        }

        /// <inheritdoc />
        public string ResolveString(int metadataToken)
        {
            return UnderlyingModule.ResolveString(metadataToken);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken)
        {
            return ResolveTypeSymbol(UnderlyingModule.ResolveType(metadataToken));
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveTypeSymbol(UnderlyingModule.ResolveType(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingModule.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingModule.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttribute(UnderlyingModule.__GetCustomAttributes(attributeType.Unpack(), inherit).FirstOrDefault());
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
        }

        #endregion

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            return _impl.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return _impl.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _impl.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _impl.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return _impl.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _impl.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _impl.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _impl.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _impl.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _impl.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _impl.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _impl.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _impl.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _impl.GetOrCreateParameterSymbol(parameter);
        }

    }

}
