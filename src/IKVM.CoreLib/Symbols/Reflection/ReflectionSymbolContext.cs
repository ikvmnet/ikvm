using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
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
    class ReflectionSymbolContext : ISymbolContext
    {

        readonly ConcurrentDictionary<string, WeakReference<IReflectionAssemblySymbol?>> _symbolByName = new();
        readonly ConditionalWeakTable<Assembly, IReflectionAssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionSymbolContext()
        {

        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, bool collectable, bool saveable)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (collectable && saveable)
                throw new NotSupportedException("Assembly cannot be both colletable and saveable.");

#if NET
            if (saveable)
                throw new NotSupportedException("Assembly cannot be saveable.");
            else if (collectable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndCollect));
            else
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.Run));
#else
            if (saveable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndSave));
            else if (collectable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndCollect));
            else
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.Run));
#endif
        }

        /// <inheritdoc />
        public IAssemblySymbolBuilder DefineAssembly(AssemblyIdentity name, ImmutableArray<CustomAttribute> attributes, bool collectable, bool saveable)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (collectable && saveable)
                throw new NotSupportedException("Assembly cannot be both colletable and saveable.");

#if NET
            if (saveable)
                throw new NotSupportedException("Assembly cannot be saveable.");
            else if (collectable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndCollect, attributes.Unpack()));
            else
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.Run, attributes.Unpack()));
#else
            if (saveable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndSave, attributes.Unpack()));
            else if (collectable)
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.RunAndCollect, attributes.Unpack()));
            else
                return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name.Unpack(), AssemblyBuilderAccess.Run, attributes.Unpack()));
#endif
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionAssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IReflectionAssemblySymbol GetOrCreateAssemblySymbolByName(Assembly assembly)
        {
            var r = _symbolByName.GetOrAdd(assembly.FullName ?? throw new InvalidOperationException(), _ => new(null));

            lock (r)
            {
                // reference has no target, reset
                if (r.TryGetTarget(out var s) == false)
                {
                    if (assembly is AssemblyBuilder builder)
                    {
                        // we were passed in a builder, so generate a symbol builder and set it as the builder and symbol.
                        r.SetTarget(s = new ReflectionAssemblySymbolBuilder(this, builder));
                    }
                    else
                    {
                        // we were passed a non builder, so generate a symbol and set it to the symbol
                        // TODO the weakness here is if we pass it the RuntimeAssembly from a non-associated builder
                        r.SetTarget(s = new ReflectionAssemblySymbol(this, assembly));
                    }
                }

                return s ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionAssemblySymbolBuilder"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IReflectionAssemblySymbolBuilder GetOrCreateAssemblySymbolByName(AssemblyBuilder assembly)
        {
            return (IReflectionAssemblySymbolBuilder)GetOrCreateAssemblySymbolByName((Assembly)assembly);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
        {
            return _symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IReflectionAssemblySymbolBuilder GetOrCreateAssemblySymbol(AssemblyBuilder assembly)
        {
            return (IReflectionAssemblySymbolBuilder)_symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is ModuleBuilder builder)
                return GetOrCreateModuleSymbol(builder);
            else
                return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionModuleSymbolBuilder"/> for the specified <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return GetOrCreateAssemblySymbol((AssemblyBuilder)module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is TypeBuilder builder)
                return GetOrCreateTypeSymbol(builder);
            else
                return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbolBuilder"/> for the specified <see cref="TypeBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMemberSymbol"/> for the specified <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public IReflectionMemberSymbol GetOrCreateMemberSymbol(MemberInfo member)
        {
            return member switch
            {
                MethodBase method => GetOrCreateMethodBaseSymbol(method),
                FieldInfo field => GetOrCreateFieldSymbol(field),
                PropertyInfo property => GetOrCreatePropertySymbol(property),
                EventInfo @event => GetOrCreateEventSymbol(@event),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            return method switch
            {
                ConstructorInfo ctor => GetOrCreateConstructorSymbol(ctor),
                _ => GetOrCreateMethodSymbol((MethodInfo)method)
            };
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return ctor switch
            {
                ConstructorBuilder builder => GetOrCreateConstructorSymbol(builder),
                _ => GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor)
            };
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return method switch
            {
                MethodBuilder builder => GetOrCreateMethodSymbol(builder),
                _ => GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method)
            };
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbolBuilder"/> for the specified <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return GetOrCreateModuleSymbol(parameter.Member.Module).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return property switch
            {
                PropertyBuilder builder => GetOrCreatePropertySymbol(builder),
                _ => GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property)
            };
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return GetOrCreateModuleSymbol(@event.Module).GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionEventSymbolBuilder"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return GetOrCreateModuleSymbol(@event.GetModuleBuilder()).GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionGenericTypeParameterSymbolBuilder"/> for the specified <see cref="GenericTypeParameterBuilder"/>.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return GetOrCreateModuleSymbol((ModuleBuilder)genericTypeParameter.Module).GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        /// <summary>
        /// Unpacks a single constructor argument.
        /// </summary>
        /// <param name="constructorArg"></param>
        /// <returns></returns>
        object? UnpackArgument(object? constructorArg)
        {
            if (constructorArg is ITypeSymbol type)
                return type.Unpack();

            return constructorArg;
        }

        /// <summary>
        /// Unpacks a set of constructor arguments.
        /// </summary>
        /// <param name="constructorArgs"></param>
        /// <returns></returns>
        object?[] UnpackArguments(object?[] constructorArgs)
        {
            var l = new object?[constructorArgs.Length];
            for (int i = 0; i < constructorArgs.Length; i++)
                l[i] = UnpackArgument(constructorArgs[i]);

            return l;
        }

    }

}
