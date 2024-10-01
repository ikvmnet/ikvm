using System;
using System.Linq;

using IKVM.Reflection;

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

        IkvmReflectionTypeTable _typeTable;
        IkvmReflectionMethodTable _methodTable;
        IkvmReflectionFieldTable _fieldTable;

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

            _typeTable = new IkvmReflectionTypeTable(context, this, null);
            _methodTable = new IkvmReflectionMethodTable(context, this, null);
            _fieldTable = new IkvmReflectionFieldTable(context, this, null);
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
        public override bool IsMissing => UnderlyingModule.__IsMissing;

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
            var _attributeType = attributeType.Unpack();
            var a = UnderlyingModule.__GetCustomAttributes(_attributeType, inherit);
            if (a.Count > 0)
                return ResolveCustomAttribute(a[0]);

            return null;
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
        }

        #endregion

        #region IIkvmReflectionModuleSymbol

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type.IsTypeDefinition())
                return _typeTable.GetOrCreateTypeSymbol(type);
            else if (type.IsGenericType)
                return ResolveTypeSymbol(type.GetGenericTypeDefinition()).GetOrCreateGenericTypeSymbol(type.GetGenericArguments());
            else if (type.IsSZArray)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateSZArrayTypeSymbol();
            else if (type.IsArray)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateArrayTypeSymbol(type.GetArrayRank());
            else if (type.IsPointer)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreatePointerTypeSymbol();
            else if (type.IsByRef)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateByRefTypeSymbol();
            else if (type.IsGenericParameter && type.DeclaringMethod is MethodInfo dm)
                return ResolveMethodSymbol(dm).GetOrCreateGenericTypeParameterSymbol(type);
            else if (type.IsGenericParameter && type.DeclaringType is Type t)
                return ResolveTypeSymbol(t).GetOrCreateGenericTypeParameterSymbol(type);

            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodBaseSymbol(method);
            else
                return _methodTable.GetOrCreateMethodBaseSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return ResolveTypeSymbol(ctor.DeclaringType).GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodSymbol(method);
            else
                return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateFieldSymbol(field);
            else
                return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return ResolveTypeSymbol(property.DeclaringType).GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return ResolveTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return ResolveMemberSymbol(parameter.Member) switch
            {
                IIkvmReflectionMethodBaseSymbol method => method.GetOrCreateParameterSymbol(parameter),
                IIkvmReflectionPropertySymbol property => property.GetOrCreateParameterSymbol(parameter),
                _ => throw new InvalidOperationException(),
            };
        }

        #endregion

        /// <inheritdoc />
        public override string ToString() => UnderlyingModule.ToString()!;

    }

}
