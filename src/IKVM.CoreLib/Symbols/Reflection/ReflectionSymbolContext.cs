using System;
using System.Collections.Concurrent;
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

        readonly ConcurrentDictionary<AssemblyName, WeakReference<IReflectionAssemblySymbol?>> _symbolByName = new(AssemblyNameEqualityComparer.Instance);
        readonly ConditionalWeakTable<Assembly, IReflectionAssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionSymbolContext()
        {

        }

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public IAssemblySymbolBuilder DefineAssembly(AssemblyName name, AssemblyBuilderAccess access)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name, access));
        }

        /// <summary>
        /// Defines a dynamic assembly that has the specified name and access rights.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public IAssemblySymbolBuilder DefineAssembly(AssemblyName name, AssemblyBuilderAccess access, ICustomAttributeBuilder[]? assemblyAttributes)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return GetOrCreateAssemblySymbol(AssemblyBuilder.DefineDynamicAssembly(name, access, assemblyAttributes?.Unpack()));
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionAssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IReflectionAssemblySymbol GetOrCreateAssemblySymbolByName(Assembly assembly)
        {
            var r = _symbolByName.GetOrAdd(assembly.GetName(), _ => new(null));

            lock (r)
            {
                // reference has no target, reset
                if (r.TryGetTarget(out var s) == false)
                {
                    if (assembly is AssemblyBuilder builder)
                    {
                        // we were passed in a builder, so generate a symbol builder and set it as the builder and symbol.
                        var a = builder.GetRuntimeAssembly();
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
            return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
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
            return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            if (ctor is ConstructorBuilder builder)
                return GetOrCreateConstructorSymbol(builder);
            else
                return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method is MethodBuilder builder)
                return GetOrCreateMethodSymbol(builder);
            else
                return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionMethodSymbolBuilder"/> for the specified <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
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
        /// Gets or creates a <see cref="IReflectionParameterSymbolBuilder"/> for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return GetOrCreateModuleSymbol(parameter.GetModuleBuilder()).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is FieldBuilder builder)
                return GetOrCreateFieldSymbol(builder);
            else
                return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is PropertyBuilder builder)
                return GetOrCreatePropertySymbol(builder);
            else
                return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
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

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs)
        {
            return new ReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new ReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedFields.Unpack(), fieldValues));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues)
        {
            return new ReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedProperties.Unpack(), propertyValues));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new ReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedProperties.Unpack(), propertyValues, namedFields.Unpack(), fieldValues));
        }

    }

}
