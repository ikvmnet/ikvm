using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Holds references to symbols derived from System.Reflection.
    /// </summary>
    class ReflectionSymbolContext : SymbolContext
    {

        readonly AssemblySymbol _coreLib;
        readonly ConcurrentDictionary<string, WeakReference<AssemblySymbol?>> _symbolByName = new();
        readonly ConditionalWeakTable<Assembly, AssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionSymbolContext(Assembly coreLib)
        {
            _coreLib = ResolveAssemblySymbol(coreLib);

            // check that we can successfully resolve the object type
            if (coreLib.GetType("System.Object") == null)
                throw new InvalidOperationException("Cannot resolve System.Object from corelib.");
        }

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
                throw new ArgumentNullException(nameof(assembly));
            if (assembly is AssemblyBuilder)
                throw new NotSupportedException();

            // find or create weak-reference to name
            var r = _symbolByName.GetOrAdd(assembly.GetName().FullName, _ => new(null));

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
        public ImmutableList<ModuleSymbol> ResolveModuleSymbols(Module[] modules)
        {
            var b = ImmutableList.CreateBuilder<ModuleSymbol>();
            foreach (var i in modules)
                b.Add(ResolveModuleSymbol(i));

            return b.ToImmutable();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="ModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public ImmutableList<ModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules)
        {
            var b = ImmutableList.CreateBuilder<ModuleSymbol>();
            foreach (var i in modules)
                b.Add(ResolveModuleSymbol(i));

            return b.ToImmutable();
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
            else if (type.IsTypeDefinition())
                return ResolveModuleSymbol(type.Module).GetType(type.FullName ?? throw new InvalidOperationException()) ?? throw new InvalidOperationException();
            else if (type.IsSZArray())
                return ResolveTypeSymbol(type.GetElementType()!).MakeArrayType();
            else if (type.IsArray)
                return ResolveTypeSymbol(type.GetElementType()!).MakeArrayType(type.GetArrayRank());
            else if (type.IsPointer)
                return ResolveTypeSymbol(type.GetElementType()!).MakePointerType();
            else if (type.IsByRef)
                return ResolveTypeSymbol(type.GetElementType()!).MakeByRefType();
            else if (type.IsGenericParameter && type.DeclaringMethod is MethodInfo m)
                return ResolveMethodSymbol(m).GetGenericArguments()[type.GenericParameterPosition];
            else if (type.IsGenericParameter && type.DeclaringType is Type t)
                return ResolveTypeSymbol(t).GetGenericArguments()[type.GenericParameterPosition];

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

            return b.ToImmutable();
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
        /// Gets or creates a <see cref="ConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(ctor))]
        public ConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
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
                return ResolveTypeSymbol(method.DeclaringType)!.GetMethod(method.Name, method.GetGenericArguments().Length, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly, CallingConventions.Any, ResolveTypeSymbols(method.GetParameterTypes()), null) ?? throw new InvalidOperationException();
            else
                return ResolveModuleSymbol(method.Module)!.GetMethod(method.Name, BindingFlags.Public | BindingFlags.NonPublic, CallingConventions.Any, ResolveTypeSymbols(method.GetParameterTypes()), null) ?? throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets or creates a list of <see cref="MethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ImmutableArray<MethodSymbol> ResolveMethodSymbols(MethodInfo[] type)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>();
            foreach (var i in type)
                b.Add(ResolveMethodSymbol(i));

            return b.ToImmutable();
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

            var a = ImmutableArray.CreateBuilder<CustomAttribute>();
            for (int i = 0; i < attributes.Count; i++)
                if (ResolveCustomAttribute(attributes[i]) is { } v)
                    a.Add(v);

            return a.ToImmutable();
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

            return a.ToImmutable();
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

            return a.ToImmutable();
        }

        /// <inheritdoc />
        public CustomAttributeNamedArgument ResolveCustomAttributeNamedArgument(System.Reflection.CustomAttributeNamedArgument arg)
        {
            return new CustomAttributeNamedArgument(
                arg.IsField,
                ResolveMemberSymbol(arg.MemberInfo)!,
                arg.MemberName,
                ResolveCustomAttributeTypedArgument(arg.TypedValue));
        }

    }

}
