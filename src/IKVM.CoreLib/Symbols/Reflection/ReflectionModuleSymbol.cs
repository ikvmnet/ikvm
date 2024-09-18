using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Implementation of <see cref="IModuleSymbol"/> derived from System.Reflection.
    /// </summary>
    class ReflectionModuleSymbol : ReflectionSymbol, IReflectionModuleSymbol
    {

        const BindingFlags DefaultBindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        readonly IReflectionAssemblySymbol _resolvingAssembly;
        readonly Module _module;
        ReflectionModuleMetadata _impl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleSymbol(ReflectionSymbolContext context, IReflectionAssemblySymbol resolvingAssembly, Module module) :
            base(context)
        {
            _resolvingAssembly = resolvingAssembly ?? throw new ArgumentNullException(nameof(resolvingAssembly));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _impl = new ReflectionModuleMetadata(this);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => _module;

        /// <inheritdoc />
        public IReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

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
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name, bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields(bindingFlags))!;
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
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods())!;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods(bindingFlags))!;
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
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingModule.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
        }

        #endregion

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            return _impl.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return _impl.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _impl.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _impl.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return _impl.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _impl.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _impl.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _impl.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _impl.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _impl.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _impl.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _impl.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _impl.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _impl.GetOrCreateParameterSymbol(parameter);
        }

    }

}
