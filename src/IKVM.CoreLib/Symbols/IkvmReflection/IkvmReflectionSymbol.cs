using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Base class for managed symbols.
    /// </summary>
    abstract class IkvmReflectionSymbol : IIkvmReflectionSymbol
    {

        readonly IkvmReflectionSymbolContext _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public IkvmReflectionSymbol(IkvmReflectionSymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the associated <see cref="IkvmReflectionSymbolContext"/>.
        /// </summary>
        public IkvmReflectionSymbolContext Context => _context;

        /// <inheritdoc />
        public virtual bool IsMissing => false;

        /// <inheritdoc />
        public virtual bool ContainsMissing => false;

        /// <inheritdoc />
        public virtual bool IsComplete => true;

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(assembly))]
        public virtual IIkvmReflectionAssemblySymbol? ResolveAssemblySymbol(Assembly? assembly)
        {
            return assembly == null ? null : _context.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(assembly))]
        public virtual IIkvmReflectionAssemblySymbolBuilder ResolveAssemblySymbol(AssemblyBuilder assembly)
        {
            if (assembly is null)
                throw new ArgumentNullException(nameof(assembly));

            return _context.GetOrCreateAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(assemblies))]
        public IIkvmReflectionAssemblySymbol[]? ResolveAssemblySymbols(Assembly[]? assemblies)
        {
            if (assemblies == null)
                return null;
            if (assemblies.Length == 0)
                return [];

            var a = new IIkvmReflectionAssemblySymbol[assemblies.Length];
            for (int i = 0; i < assemblies.Length; i++)
                if (ResolveAssemblySymbol(assemblies[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(module))]
        public virtual IIkvmReflectionModuleSymbol? ResolveModuleSymbol(Module? module)
        {
            return module == null ? null : _context.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(module))]
        public virtual IIkvmReflectionModuleSymbolBuilder ResolveModuleSymbol(ModuleBuilder module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            return _context.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(modules))]
        public IIkvmReflectionModuleSymbol[]? ResolveModuleSymbols(Module[]? modules)
        {
            if (modules == null)
                return null;
            if (modules.Length == 0)
                return [];

            var a = new IIkvmReflectionModuleSymbol[modules.Length];
            for (int i = 0; i < modules.Length; i++)
                if (ResolveModuleSymbol(modules[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        public IEnumerable<IIkvmReflectionModuleSymbol> ResolveModuleSymbols(IEnumerable<Module> modules)
        {
            foreach (var module in modules)
                if (ResolveModuleSymbol(module) is { } symbol)
                    yield return symbol;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(member))]
        public virtual IIkvmReflectionMemberSymbol? ResolveMemberSymbol(MemberInfo? member)
        {
            if (member == null)
                return null;

            return member switch
            {
                ConstructorInfo ctor => ResolveConstructorSymbol(ctor),
                EventInfo @event => ResolveEventSymbol(@event),
                FieldInfo field => ResolveFieldSymbol(field),
                MethodInfo method => ResolveMethodSymbol(method),
                PropertyInfo property => ResolvePropertySymbol(property),
                Type type => ResolveTypeSymbol(type),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(members))]
        public IIkvmReflectionMemberSymbol[]? ResolveMemberSymbols(MemberInfo[]? members)
        {
            if (members == null)
                return null;
            if (members.Length == 0)
                return [];

            var a = new IIkvmReflectionMemberSymbol[members.Length];
            for (int i = 0; i < members.Length; i++)
                if (ResolveMemberSymbol(members[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public virtual IIkvmReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            return type == null ? null : _context.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(type))]
        public virtual IIkvmReflectionTypeSymbolBuilder ResolveTypeSymbol(TypeBuilder type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            return _context.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(types))]
        public IIkvmReflectionTypeSymbol[]? ResolveTypeSymbols(Type[]? types)
        {
            if (types == null)
                return null;
            if (types.Length == 0)
                return [];

            var a = new IIkvmReflectionTypeSymbol[types.Length];
            for (int i = 0; i < types.Length; i++)
                if (ResolveTypeSymbol(types[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        public virtual IEnumerable<IIkvmReflectionTypeSymbol> ResolveTypeSymbols(IEnumerable<Type> types)
        {
            foreach (var type in types)
                if (ResolveTypeSymbol(type) is { } symbol)
                    yield return symbol;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public virtual IIkvmReflectionMethodBaseSymbol? ResolveMethodBaseSymbol(MethodBase? method)
        {
            return method switch
            {
                ConstructorInfo ctor => ResolveConstructorSymbol(ctor),
                MethodInfo method_ => ResolveMethodSymbol(method_),
                _ => null,
            };
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public virtual IIkvmReflectionConstructorSymbol? ResolveConstructorSymbol(ConstructorInfo? ctor)
        {
            return ctor == null ? null : _context.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctor))]
        public virtual IIkvmReflectionConstructorSymbolBuilder ResolveConstructorSymbol(ConstructorBuilder ctor)
        {
            if (ctor is null)
                throw new ArgumentNullException(nameof(ctor));

            return _context.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(ctors))]
        public IIkvmReflectionConstructorSymbol[]? ResolveConstructorSymbols(ConstructorInfo[]? ctors)
        {
            if (ctors == null)
                return null;
            if (ctors.Length == 0)
                return [];

            var a = new IIkvmReflectionConstructorSymbol[ctors.Length];
            for (int i = 0; i < ctors.Length; i++)
                if (ResolveConstructorSymbol(ctors[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public virtual IIkvmReflectionMethodSymbol? ResolveMethodSymbol(MethodInfo? method)
        {
            return method == null ? null : _context.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(method))]
        public virtual IIkvmReflectionMethodSymbolBuilder ResolveMethodSymbol(MethodBuilder method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            return _context.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(methods))]
        public IIkvmReflectionMethodSymbol[]? ResolveMethodSymbols(MethodInfo[]? methods)
        {
            if (methods == null)
                return null;
            if (methods.Length == 0)
                return [];

            var a = new IIkvmReflectionMethodSymbol[methods.Length];
            for (int i = 0; i < methods.Length; i++)
                if (ResolveMethodSymbol(methods[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public virtual IIkvmReflectionFieldSymbol? ResolveFieldSymbol(FieldInfo? field)
        {
            return field == null ? null : _context.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(field))]
        public virtual IIkvmReflectionFieldSymbolBuilder ResolveFieldSymbol(FieldBuilder field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));

            return _context.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(fields))]
        public IIkvmReflectionFieldSymbol[]? ResolveFieldSymbols(FieldInfo[]? fields)
        {
            if (fields == null)
                return null;
            if (fields.Length == 0)
                return [];

            var a = new IIkvmReflectionFieldSymbol[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                if (ResolveFieldSymbol(fields[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public virtual IIkvmReflectionPropertySymbol? ResolvePropertySymbol(PropertyInfo? property)
        {
            return property == null ? null : _context.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(property))]
        public virtual IIkvmReflectionPropertySymbolBuilder ResolvePropertySymbol(PropertyBuilder property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            return _context.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(properties))]
        public IIkvmReflectionPropertySymbol[]? ResolvePropertySymbols(PropertyInfo[]? properties)
        {
            if (properties == null)
                return null;
            if (properties.Length == 0)
                return [];

            var a = new IIkvmReflectionPropertySymbol[properties.Length];
            for (int i = 0; i < properties.Length; i++)
                if (ResolvePropertySymbol(properties[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public virtual IIkvmReflectionEventSymbol? ResolveEventSymbol(EventInfo? @event)
        {
            if (@event is null)
                return null;

            return _context.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(@event))]
        public virtual IIkvmReflectionEventSymbolBuilder? ResolveEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            return _context.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(events))]
        public IIkvmReflectionEventSymbol[]? ResolveEventSymbols(EventInfo[]? events)
        {
            if (@events == null)
                return null;
            if (@events.Length == 0)
                return [];

            var a = new IIkvmReflectionEventSymbol[events.Length];
            for (int i = 0; i < events.Length; i++)
                if (ResolveEventSymbol(events[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public virtual IIkvmReflectionParameterSymbol? ResolveParameterSymbol(ParameterInfo? parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));

            return _context.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public virtual IIkvmReflectionParameterSymbolBuilder? ResolveParameterSymbol(IIkvmReflectionMethodBaseSymbolBuilder method, ParameterBuilder parameter)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));

            if (parameter is null)
                return null;

            return method.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameter))]
        public virtual IIkvmReflectionParameterSymbolBuilder? ResolveParameterSymbol(IIkvmReflectionPropertySymbolBuilder property, ParameterBuilder parameter)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));

            if (parameter is null)
                return null;

            return property.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(parameters))]
        public IIkvmReflectionParameterSymbol[]? ResolveParameterSymbols(ParameterInfo[]? parameters)
        {
            if (parameters == null)
                return null;
            if (parameters.Length == 0)
                return [];

            var a = new IIkvmReflectionParameterSymbol[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
                if (ResolveParameterSymbol(parameters[i]) is { } symbol)
                    a[i] = symbol;

            return a;
        }

        /// <inheritdoc />
        [return: NotNullIfNotNull(nameof(attributes))]
        public CustomAttribute[]? ResolveCustomAttributes(IList<CustomAttributeData>? attributes)
        {
            if (attributes == null)
                return null;
            if (attributes.Count == 0)
                return [];

            var a = new CustomAttribute[attributes.Count];
            for (int i = 0; i < attributes.Count; i++)
                if (ResolveCustomAttribute(attributes[i]) is { } v)
                    a[i] = v;

            return a;
        }

        /// <inheritdoc />
        public IEnumerable<CustomAttribute> ResolveCustomAttributes(IEnumerable<CustomAttributeData> attributes)
        {
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));

            var a = new List<CustomAttribute>();
            foreach (var i in attributes)
                if (ResolveCustomAttribute(i) is { } v)
                    a.Add(v);

            return a.ToArray();
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
        public ImmutableArray<CustomAttributeTypedArgument> ResolveCustomAttributeTypedArguments(IList<IKVM.Reflection.CustomAttributeTypedArgument> args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            if (args.Count == 0)
                return [];

            var a = new CustomAttributeTypedArgument[args.Count];
            for (int i = 0; i < args.Count; i++)
                a[i] = ResolveCustomAttributeTypedArgument(args[i]);

            return a.ToImmutableArray();
        }

        /// <inheritdoc />
        public CustomAttributeTypedArgument ResolveCustomAttributeTypedArgument(IKVM.Reflection.CustomAttributeTypedArgument arg)
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
                IList<IKVM.Reflection.CustomAttributeTypedArgument> aa => ResolveCustomAttributeTypedArguments(aa),
                Type v => ResolveTypeSymbol(v),
                _ => value
            };
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttributeNamedArgument> ResolveCustomAttributeNamedArguments(IList<IKVM.Reflection.CustomAttributeNamedArgument> args)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));
            if (args.Count == 0)
                return [];

            var a = new CustomAttributeNamedArgument[args.Count];
            for (int i = 0; i < args.Count; i++)
                a[i] = ResolveCustomAttributeNamedArgument(args[i]);

            return ImmutableArray.Create(a);
        }

        /// <inheritdoc />
        public CustomAttributeNamedArgument ResolveCustomAttributeNamedArgument(IKVM.Reflection.CustomAttributeNamedArgument arg)
        {
            return new CustomAttributeNamedArgument(
                arg.IsField,
                ResolveMemberSymbol(arg.MemberInfo)!,
                arg.MemberName,
                ResolveCustomAttributeTypedArgument(arg.TypedValue));
        }

        /// <inheritdoc />
        public InterfaceMapping ResolveInterfaceMapping(IKVM.Reflection.InterfaceMapping mapping)
        {
            return new InterfaceMapping(
                ResolveMethodSymbols(mapping.InterfaceMethods),
                ResolveTypeSymbol(mapping.InterfaceType),
                ResolveMethodSymbols(mapping.TargetMethods),
                ResolveTypeSymbol(mapping.TargetType));
        }

        /// <inheritdoc />
        public ManifestResourceInfo? ResolveManifestResourceInfo(IKVM.Reflection.ManifestResourceInfo? info)
        {
            return info != null ? new ManifestResourceInfo((System.Reflection.ResourceLocation)info.ResourceLocation, info.FileName, ResolveAssemblySymbol(info.ReferencedAssembly)) : null;
        }

    }

}