using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Holds references to symbols derived from System.Reflection.
    /// </summary>
    public class ReflectionSymbolContext : SymbolContext
    {

        readonly ReflectionSymbolOptions _options;
        readonly AssemblySymbol _coreLib;
        readonly ConcurrentDictionary<string, WeakReference<AssemblySymbol?>> _symbolByName = new();
        readonly ConditionalWeakTable<Assembly, AssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionSymbolContext(Assembly coreLib, ReflectionSymbolOptions options)
        {
            _options = options;
            _coreLib = ResolveAssemblySymbol(coreLib);

            // check that we can successfully resolve the object type
            if (coreLib.GetType("System.Object") == null)
                throw new InvalidOperationException("Cannot resolve System.Object from corelib.");
        }

        /// <summary>
        /// Gets the options used to configure the context.
        /// </summary>
        public ReflectionSymbolOptions Options => _options;

        /// <inheritdoc />
        public override TypeSymbol ResolveCoreType(string typeName)
        {
            return _coreLib.GetType(typeName) ?? throw new InvalidOperationException($"Failed to resolve core type {typeName}");
        }

        /// <inheritdoc />
        public override AssemblySymbolBuilder DefineAssembly(AssemblyIdentity identity, ImmutableArray<CustomAttribute> attributes)
        {
            if (identity is null)
                throw new ArgumentNullException(nameof(identity));

            // find or create weak-reference to name
            var r = _symbolByName.GetOrAdd(identity.FullName, _ => new(null));

            // look the reference to set it up
            lock (r)
            {
                // reference has no target, reset
                if (r.TryGetTarget(out var s) == false)
                {
                    // we were passed a non builder, so generate a symbol and set it to the symbol
                    // TODO the weakness here is if we pass it the RuntimeAssembly from a non-associated builder
                    r.SetTarget(s = new AssemblySymbolBuilder(this, identity));
                }

                return (AssemblySymbolBuilder?)s ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="AssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public AssemblySymbol ResolveAssemblySymbol(Assembly assembly)
        {
            if (assembly is null)
                return null;
            if (assembly is AssemblyBuilder)
                throw new ArgumentException(nameof(assembly));

            // find or create weak-reference to name
            var r = _symbolByName.GetOrAdd(assembly.FullName ?? throw new InvalidOperationException(), _ => new(null));

            // look the reference to set it up
            lock (r)
            {
                // reference has no target, reset
                if (r.TryGetTarget(out var s) == false)
                {
                    // we were passed a non builder, so generate a symbol and set it to the symbol
                    // TODO the weakness here is if we pass it the RuntimeAssembly from a non-associated builder
                    r.SetTarget(s = new ReflectionAssemblySymbol(this, assembly));
                }

                return (ReflectionAssemblySymbol?)s ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="ModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(module))]
        public ModuleSymbol? ResolveModuleSymbol(Module? module)
        {
            if (module is ModuleBuilder)
                throw new NotSupportedException();
            else if (module is null)
                return null;
            else
                return ResolveAssemblySymbol(module.Assembly).GetModule(module.Name) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="ModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public ImmutableArray<ModuleSymbol> ResolveModuleSymbols(Module[] modules)
        {
            var b = ImmutableArray.CreateBuilder<ModuleSymbol>(modules.Length);
            foreach (var i in modules)
                b.Add(ResolveModuleSymbol(i));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="ModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public IEnumerable<ModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules)
        {
            foreach (var i in modules)
                yield return ResolveModuleSymbol(i);
        }

        /// <summary>
        /// Gets or creates a <see cref="MemberSymbol"/> for the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [return: NotNullIfNotNull(nameof(member))]
        public MemberSymbol? ResolveMemberSymbol(MemberInfo? member)
        {
            if (member is null)
                return null;
            else if (member is Type type)
                return ResolveTypeSymbol(type);
            else if (member is ConstructorInfo ctor)
                return ResolveConstructorSymbol(ctor);
            else if (member is MethodInfo method)
                return ResolveMethodSymbol(method);
            else if (member is FieldInfo field)
                return ResolveFieldSymbol(field);
            else if (member is PropertyInfo property)
                return ResolvePropertySymbol(property);
            else if (member is EventInfo @event)
                return ResolveEventSymbol(@event);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a <see cref="TypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        public TypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type is null)
                return null;
            else if (type is TypeBuilder)
                throw new NotSupportedException();
            else if (type.IsTypeDefinition() && type.IsNested)
                return ResolveTypeSymbol(type.DeclaringType!).GetNestedType(type.Name, Symbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (type.IsTypeDefinition())
                return ResolveModuleSymbol(type.Module).GetType(type.FullName ?? throw new InvalidOperationException(), false) ?? throw new InvalidOperationException();
            else if (type.IsConstructedGenericType)
                return ResolveTypeSymbol(type.GetGenericTypeDefinition()).MakeGenericType(ResolveTypeSymbols(type.GetGenericArguments()));
            else if (type.IsSZArray())
                return ResolveTypeSymbol(type.GetElementType()!).MakeArrayType();
            else if (type.IsArray)
                return ResolveTypeSymbol(type.GetElementType()!).MakeArrayType(type.GetArrayRank());
            else if (type.IsPointer)
                return ResolveTypeSymbol(type.GetElementType()!).MakePointerType();
            else if (type.IsByRef)
                return ResolveTypeSymbol(type.GetElementType()!).MakeByRefType();
            else if (type.IsGenericParameter && type.DeclaringMethod is MethodInfo dm && dm.DeclaringType is var dt && ResolveTypeSymbol(dt) is ReflectionTypeSymbol dts)
                return dts.GetOrCreateGenericMethodParameter(type);
            else if (type.IsGenericParameter && type.DeclaringType is Type t)
                return ResolveTypeSymbol(t).GenericArguments[type.GenericParameterPosition];
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="TypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public ImmutableArray<TypeSymbol> ResolveTypeSymbols(Type[] types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbol>(types.Length);
            for (var i = 0; i < types.Length; i++)
                b.Add(ResolveTypeSymbol(types[i]));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="TypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public ImmutableArray<TypeSymbol> ResolveTypeSymbols(IReadOnlyList<Type> types)
        {
            var b = ImmutableArray.CreateBuilder<TypeSymbol>(types.Count);
            for (var i = 0; i < types.Count; i++)
                b.Add(ResolveTypeSymbol(types[i]));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="TypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<TypeSymbol> ResolveTypeSymbols(IEnumerable<Type> type)
        {
            foreach (var i in type)
                yield return ResolveTypeSymbol(i);
        }

        /// <summary>
        /// Gets or creates a <see cref="FieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(field))]
        public FieldSymbol? ResolveFieldSymbol(FieldInfo? field)
        {
            if (field is null)
                return null;
            else if (field is FieldBuilder)
                throw new NotSupportedException();
            else if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol(field.DeclaringType)!.GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly) ?? throw new InvalidOperationException();
            else
                return ResolveModuleSymbol(field.Module)!.GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a <see cref="MethodSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        public MethodSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
        {
            if (ctor is null)
                return null;
            else if (ctor is ConstructorBuilder)
                throw new NotSupportedException();
            else if (ctor.IsStatic)
                return ResolveTypeSymbol(ctor.DeclaringType)!.TypeInitializer ?? throw new InvalidOperationException();
            else
                return ResolveTypeSymbol(ctor.DeclaringType)!.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly, ResolveTypeSymbols(ctor.GetParameterTypes())) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a <see cref="MethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(method))]
        public MethodSymbol? ResolveMethodSymbol(MethodInfo? method)
        {
            if (method is null)
                return null;
            else if (method is MethodBuilder)
                throw new NotSupportedException();
            else if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetMethod(method.Name, method.GetGenericArguments().Length, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, method.CallingConvention, ResolveTypeSymbols(method.GetParameterTypes()), default) ?? throw new InvalidOperationException();
            else
                return ResolveModuleSymbol(method.Module).GetMethod(method.Name, BindingFlags.Public | BindingFlags.NonPublic, method.CallingConvention, ResolveTypeSymbols(method.GetParameterTypes()), default) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="MethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="methods"></param>
        /// <returns></returns>
        public ImmutableArray<MethodSymbol> ResolveMethodSymbols(MethodInfo[] methods)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>(methods.Length);
            foreach (var i in methods)
                b.Add(ResolveMethodSymbol(i));

            return b.DrainToImmutable();
        }

        /// <summary>
        /// Gets or creates a <see cref="PropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(property))]
        public PropertySymbol? ResolvePropertySymbol(PropertyInfo? property)
        {
            if (property is null)
                return null;
            else if (property is PropertyBuilder)
                throw new NotSupportedException();
            else
                return ResolveTypeSymbol(property.DeclaringType)!.GetProperty(property.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a <see cref="EventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(@event))]
        public EventSymbol? ResolveEventSymbol(EventInfo? @event)
        {
            if (@event is null)
                return null;
            else
                return ResolveTypeSymbol(@event.DeclaringType)!.GetEvent(@event.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly) ?? throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> ResolveCustomAttributes(IList<CustomAttributeData>? attributes)
        {
            if (attributes == null || attributes.Count == 0)
                return [];

            var a = ImmutableArray.CreateBuilder<CustomAttribute>(attributes.Count);
            for (int i = 0; i < attributes.Count; i++)
                if (ResolveCustomAttribute(attributes[i]) is { } v)
                    a.Add(v);

            return a.DrainToImmutable();
        }

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> ResolveCustomAttributes(IEnumerable<CustomAttributeData> attributes)
        {
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));

            foreach (var i in attributes)
                if (ResolveCustomAttribute(i) is { } v)
                    yield return v;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(customAttributeData))]
        public CustomAttribute? ResolveCustomAttribute(CustomAttributeData? customAttributeData)
        {
            if (customAttributeData == null)
                return null;

            return new CustomAttribute(
                ResolveTypeSymbol(customAttributeData.AttributeType)!,
                ResolveConstructorSymbol(customAttributeData.Constructor)!,
                ResolveCustomAttributeTypedArguments(customAttributeData.ConstructorArguments),
                ResolveCustomAttributeNamedArguments(customAttributeData.NamedArguments));
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttributeTypedArgument> ResolveCustomAttributeTypedArguments(IList<System.Reflection.CustomAttributeTypedArgument> args)
        {
            if (args is null || args.Count == 0)
                return [];

            var a = ImmutableArray.CreateBuilder<CustomAttributeTypedArgument>(args.Count);
            for (int i = 0; i < args.Count; i++)
                a.Add(ResolveCustomAttributeTypedArgument(args[i]));

            return a.DrainToImmutable();
        }

        /// <inheritdoc />
        public CustomAttributeTypedArgument ResolveCustomAttributeTypedArgument(System.Reflection.CustomAttributeTypedArgument arg)
        {
            return new CustomAttributeTypedArgument(
                ResolveTypeSymbol(arg.ArgumentType)!,
                ResolveCustomAttributeTypedValue(arg.Value));
        }

        /// <inheritdoc />
        public object? ResolveCustomAttributeTypedValue(object? value)
        {
            return value switch
            {
                IList<System.Reflection.CustomAttributeTypedArgument> aa => ResolveCustomAttributeTypedArguments(aa),
                Type v => ResolveTypeSymbol(v),
                _ => value
            };
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttributeNamedArgument> ResolveCustomAttributeNamedArguments(IList<System.Reflection.CustomAttributeNamedArgument> args)
        {
            if (args is null || args.Count == 0)
                return [];

            var a = ImmutableArray.CreateBuilder<CustomAttributeNamedArgument>(args.Count);
            for (int i = 0; i < args.Count; i++)
                a.Add(ResolveCustomAttributeNamedArgument(args[i]));

            return a.DrainToImmutable();
        }

        /// <inheritdoc />
        public CustomAttributeNamedArgument ResolveCustomAttributeNamedArgument(System.Reflection.CustomAttributeNamedArgument arg)
        {
            return new CustomAttributeNamedArgument(ResolveMemberSymbol(arg.MemberInfo), ResolveCustomAttributeTypedArgument(arg.TypedValue));
        }

        /// <summary>
        /// Copies the <see cref="CustomAttribute"/> into a new <see cref="CustomAttributeBuilder"/>.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        internal CustomAttributeBuilder CreateCustomAttributeBuilder(CustomAttribute attribute)
        {
            return new CustomAttributeBuilder(
                (ConstructorInfo)ResolveMethod(attribute.Constructor),
                attribute.ConstructorArguments.Select(i => i.Value).ToArray(),
                attribute.NamedArguments.Where(i => i.MemberInfo is PropertySymbol).Select(i => ResolveProperty((PropertySymbol)i.MemberInfo)).ToArray(),
                attribute.NamedArguments.Where(i => i.MemberInfo is PropertySymbol).Select(i => i.TypedValue.Value).ToArray(),
                attribute.NamedArguments.Where(i => i.MemberInfo is FieldSymbol).Select(i => ResolveField((FieldSymbol)i.MemberInfo)).ToArray(),
                attribute.NamedArguments.Where(i => i.MemberInfo is FieldSymbol).Select(i => i.TypedValue.Value).ToArray());
        }

        /// <summary>
        /// Copies the array of <see cref="CustomAttribute"/> into a new array of <see cref="CustomAttributeBuilder"/>s.
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        internal CustomAttributeBuilder[] CreateCustomAttributeBuilders(ImmutableArray<CustomAttribute> attributes)
        {
            if (attributes.IsDefaultOrEmpty)
                return [];

            var b = new CustomAttributeBuilder[attributes.Length];
            for (int i = 0; i < attributes.Length; i++)
                b[i] = CreateCustomAttributeBuilder(attributes[i]);

            return b;
        }

        /// <summary>
        /// Obtains the <see cref="Assembly"/> object for the given <see cref="AssemblySymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public Assembly? ResolveAssembly(AssemblySymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is AssemblySymbolBuilder builder)
                return ResolveAssembly(builder, state);
            else if (symbol is ReflectionAssemblySymbol assembly)
                return assembly._underlyingAssembly;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the completed <see cref="Assembly"/> object for the given <see cref="AssemblySymbol"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal Assembly ResolveAssembly(AssemblySymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionAssemblySymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Assembly ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="Module"/> object for the given <see cref="ModuleSymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public Module ResolveModule(ModuleSymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is ModuleSymbolBuilder builder)
                return ResolveModule(symbol, state);
            else if (symbol is ReflectionModuleSymbol module)
                return module._underlyingModule;
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Resolves the specified <see cref="ModuleSymbolBuilder"/> to at least the specified state.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        internal Module ResolveModule(ModuleSymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionModuleSymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Module ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="Type"/> object for the given <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public Type? ResolveType(TypeSymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is TypeSymbolBuilder builder)
                return ResolveType(builder, state);
            else if (symbol is ReflectionTypeSymbol type)
                return type._underlyingType;
            else if (symbol.IsTypeDefinition && symbol.IsNested)
                return ResolveType(symbol.DeclaringType!).GetNestedType(symbol.Name, Symbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (symbol.IsTypeDefinition)
                return ResolveModule(symbol.Module).GetType(symbol.FullName ?? throw new InvalidOperationException(), false) ?? throw new InvalidOperationException();
            else if (symbol.IsConstructedGenericType)
                return ResolveType(symbol.GenericTypeDefinition ?? throw new InvalidOperationException()).MakeGenericType(ResolveTypes(symbol.GenericArguments));
            else if (symbol.IsSZArray)
                return ResolveType(symbol.GetElementType()!).MakeArrayType();
            else if (symbol.IsArray)
                return ResolveType(symbol.GetElementType()!).MakeArrayType(symbol.GetArrayRank());
            else if (symbol.IsPointer)
                return ResolveType(symbol.GetElementType()!).MakePointerType();
            else if (symbol.IsByRef)
                return ResolveType(symbol.GetElementType()!).MakeByRefType();
            else if (symbol.IsGenericMethodParameter)
                return ResolveMethod(symbol.DeclaringMethod ?? throw new InvalidOperationException()).GetGenericArguments()[symbol.GenericParameterPosition];
            else if (symbol.IsGenericTypeParameter)
                return ResolveType(symbol.DeclaringType ?? throw new InvalidOperationException()).GetGenericArguments()[symbol.GenericParameterPosition];
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="Type"/> object for the given <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="types"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Type[] ResolveTypes(ImmutableArray<TypeSymbol> types, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (types.IsDefaultOrEmpty)
                return [];

            var t = new Type[types.Length];
            for (int i = 0; i < types.Length; i++)
                t[i] = ResolveType(types[i]);

            return t;
        }

        /// <summary>
        /// Obtains the completed <see cref="TypeBuilder"/> object for the given <see cref="TypeSymbolBuilder"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal Type ResolveType(TypeSymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionTypeSymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Type ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="FieldInfo"/> object for the given <see cref="FieldSymbol"/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public FieldInfo? ResolveField(FieldSymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is FieldSymbolBuilder builder)
                return ResolveField(builder, state);
            else if (symbol is ReflectionFieldSymbol field)
                return field.UnderlyingField;
            else
                return ResolveType(symbol.DeclaringType ?? throw new InvalidOperationException()).GetField(symbol.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the completed <see cref="FieldBuilder"/> object for the given <see cref="FieldSymbolBuilder"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal FieldInfo ResolveField(FieldSymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionFieldSymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Field ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="MethodBase"/> object for the given <see cref="MethodSymbol "/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public MethodBase? ResolveMethod(MethodSymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is MethodSymbolBuilder builder)
                return ResolveMethod(builder, state);
            else if (symbol is ReflectionMethodSymbol method)
                return method.UnderlyingMethod;
            else if (symbol.DeclaringType is { } dt)
                return ResolveType(dt).GetMethod(symbol.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                return ResolveModule(symbol.Module).GetMethod(symbol.Name, TypeSymbol.DeclaredOnlyLookup, null, CallingConventions.Any, ResolveTypes(symbol.ParameterTypes), null) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the completed <see cref="MethodBuilder"/> object for the given <see cref="MethodSymbolBuilder"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal MethodInfo ResolveMethod(MethodSymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionMethodSymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Method ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="PropertyInfo"/> object for the given <see cref="PropertySymbol "/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public PropertyInfo? ResolveProperty(PropertySymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is PropertySymbolBuilder builder)
                return ResolveProperty(symbol, state);
            else if (symbol is ReflectionPropertySymbol property)
                return property.UnderlyingProperty;
            else
                return ResolveType(symbol.DeclaringType ?? throw new InvalidOperationException()).GetProperty(symbol.Name, TypeSymbol.DeclaredOnlyLookup, null, ResolveType(symbol.PropertyType), ResolveTypes(symbol.GetIndexParameters().Select(i => i.ParameterType).ToImmutableArray()), null) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the completed <see cref="PropertyBuilder"/> object for the given <see cref="PropertySymbolBuilder"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal PropertyInfo ResolveProperty(PropertySymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionPropertySymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Property ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="EventInfo"/> object for the given <see cref="EventSymbol "/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public EventInfo? ResolveEvent(EventSymbol? symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            if (symbol is null)
                return null;
            else if (symbol is EventSymbolBuilder builder)
                return ResolveEvent(builder, state);
            else if (symbol is ReflectionEventSymbol evt)
                return evt.UnderlyingEvent;
            else
                return ResolveType(symbol.DeclaringType ?? throw new InvalidOperationException()).GetEvent(symbol.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the completed <see cref="EventBuilder"/> object for the given <see cref="EventSymbolBuilder"/>. Ensures the phase of the builder is up to the specified phase.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        internal EventInfo ResolveEvent(EventSymbolBuilder builder, ReflectionSymbolState state)
        {
            var writer = builder.Writer(b => new ReflectionEventSymbolBuilderWriter(this, b));
            writer.AdvanceTo(state);
            return writer.Event ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Obtains the <see cref="EventInfo"/> object for the given <see cref="ParameterSymbol "/>.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(symbol))]
        public ParameterInfo ResolveParameter(ParameterSymbol symbol, ReflectionSymbolState state = ReflectionSymbolState.Declared)
        {
            throw new NotImplementedException();
        }

    }

}
