using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Holds references to symbols derived from System.Reflection.
    /// </summary>
    class IkvmReflectionSymbolContext : ISymbolContext
    {

        readonly ConcurrentDictionary<string, WeakReference<IIkvmReflectionAssemblySymbol?>> _symbolByName = new();
        readonly ConditionalWeakTable<Assembly, IIkvmReflectionAssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReflectionSymbolContext()
        {

        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionAssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IIkvmReflectionAssemblySymbol GetOrCreateAssemblySymbolByName(Assembly assembly)
        {
            var r = _symbolByName.GetOrAdd(assembly.FullName, _ => new(null));

            lock (r)
            {
                // reference has no target, reset
                if (r.TryGetTarget(out var s) == false)
                {
                    if (assembly is AssemblyBuilder builder)
                    {
                        // we were passed in a builder, so generate a symbol builder and set it as the builder and symbol.
                        r.SetTarget(s = new IkvmReflectionAssemblySymbolBuilder(this, builder));
                    }
                    else
                    {
                        // we were passed a non builder, so generate a symbol and set it to the symbol
                        // TODO the weakness here is if we pass it the RuntimeAssembly from a non-associated builder
                        r.SetTarget(s = new IkvmReflectionAssemblySymbol(this, assembly));
                    }
                }

                return s ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionAssemblySymbolBuilder"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IIkvmReflectionAssemblySymbolBuilder GetOrCreateAssemblySymbolByName(AssemblyBuilder assembly)
        {
            return (IIkvmReflectionAssemblySymbolBuilder)GetOrCreateAssemblySymbolByName((Assembly)assembly);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IIkvmReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
        {
            return _symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IIkvmReflectionAssemblySymbolBuilder GetOrCreateAssemblySymbol(AssemblyBuilder assembly)
        {
            return (IIkvmReflectionAssemblySymbolBuilder)_symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IIkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is ModuleBuilder builder)
                return GetOrCreateModuleSymbol(builder);
            else
                return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionModuleSymbolBuilder"/> for the specified <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IIkvmReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is TypeBuilder builder)
                return GetOrCreateTypeSymbol(builder);
            else
                return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbolBuilder"/> for the specified <see cref="TypeBuilder"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            if (ctor is ConstructorBuilder builder)
                return GetOrCreateConstructorSymbol(builder);
            else
                return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method is MethodBuilder builder)
                return GetOrCreateMethodSymbol(builder);
            else
                return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionMethodSymbolBuilder"/> for the specified <see cref="MethodBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return GetOrCreateModuleSymbol(parameter.Module).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionParameterSymbolBuilder"/> for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return GetOrCreateModuleSymbol(parameter.Module).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is FieldBuilder builder)
                return GetOrCreateFieldSymbol(builder);
            else
                return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is PropertyBuilder builder)
                return GetOrCreatePropertySymbol(builder);
            else
                return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionPropertySymbolBuilder"/> for the specified <see cref="PropertyBuilder"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return GetOrCreateModuleSymbol(@event.Module).GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionEventSymbolBuilder"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return GetOrCreateModuleSymbol(@event.Module).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs)
        {
            return new IkvmReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new IkvmReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedFields.Unpack(), fieldValues));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues)
        {
            return new IkvmReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedProperties.Unpack(), propertyValues));
        }

        /// <inheritdoc />
        public ICustomAttributeBuilder CreateCustomAttribute(IConstructorSymbol con, object?[] constructorArgs, IPropertySymbol[] namedProperties, object?[] propertyValues, IFieldSymbol[] namedFields, object?[] fieldValues)
        {
            return new IkvmReflectionCustomAttributeBuilder(new CustomAttributeBuilder(con.Unpack(), constructorArgs, namedProperties.Unpack(), propertyValues, namedFields.Unpack(), fieldValues));
        }

    }

}
