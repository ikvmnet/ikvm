using System;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionModuleSymbolBuilder : IkvmReflectionSymbolBuilder, IIkvmReflectionModuleSymbolBuilder
    {

        readonly IIkvmReflectionAssemblySymbol _resolvingAssembly;

        readonly ModuleBuilder _builder;
        IkvmReflectionModuleMetadata _metadata;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionAssemblySymbol resolvingAssembly, ModuleBuilder builder) :
            base(context)
        {
            _resolvingAssembly = resolvingAssembly ?? throw new ArgumentNullException(nameof(resolvingAssembly));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _metadata = new IkvmReflectionModuleMetadata(this);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => UnderlyingModuleBuilder;

        /// <inheritdoc />
        public ModuleBuilder UnderlyingModuleBuilder => _builder;

        /// <inheritdoc />
        public IIkvmReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

        #region IModuleSymbolBuilder

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typesize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packsize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packsize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packingSize, int typesize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packingSize, typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

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
        public override bool IsComplete => _builder == null;

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
            return _metadata.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return _metadata.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _metadata.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _metadata.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return _metadata.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _metadata.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _metadata.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _metadata.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _metadata.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _metadata.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _metadata.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _metadata.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _metadata.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _metadata.GetOrCreateParameterSymbol(parameter);
        }

    }

}
