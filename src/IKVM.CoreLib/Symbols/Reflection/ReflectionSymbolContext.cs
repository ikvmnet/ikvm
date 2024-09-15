using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    /// <summary>
    /// Holds references to symbols derived from System.Reflection.
    /// </summary>
    class ReflectionSymbolContext
    {

        readonly ConcurrentDictionary<AssemblyName, WeakReference<ReflectionAssemblySymbol>> _symbolByName = new(AssemblyNameEqualityComparer.Instance);
        readonly ConditionalWeakTable<Assembly, ReflectionAssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ReflectionSymbolContext()
        {

        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionAssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ReflectionAssemblySymbol GetOrCreateAssemblySymbolByName(Assembly assembly)
        {
            var r = _symbolByName.GetOrAdd(assembly.GetName(), _ => new WeakReference<ReflectionAssemblySymbol>(new ReflectionAssemblySymbol(this, assembly)));

            // reference has valid symbol
            if (r.TryGetTarget(out var s))
                return s;

            // no valid symbol, must have been released, lock to restore
            lock (r)
            {
                // still gone, recreate
                if (r.TryGetTarget(out s) == false)
                    r.SetTarget(s = new ReflectionAssemblySymbol(this, assembly));

                return s;
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public ReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
        {
            return _symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionMethodBaseSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public ReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is ConstructorInfo ctor)
                return GetOrCreateConstructorSymbol(ctor);
            else
                return GetOrCreateMethodSymbol((MethodInfo)method);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public ReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public ReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public ReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return GetOrCreateModuleSymbol(parameter.Member.Module).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public ReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public ReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public ReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return GetOrCreateModuleSymbol(@event.Module).GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionEventSymbol"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public ReflectionEventSymbol GetOrCreateEventSymbol(EventBuilder @event)
        {
            return GetOrCreateEventSymbol(new ReflectionEventBuilderInfo(@event));
        }

        /// <summary>
        /// Gets or creates a <see cref="ReflectionEventSymbol"/> for the specified <see cref="EventBuilder"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public ReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return GetOrCreateParameterSymbol(new ReflectionParameterBuilderInfo(parameter));
        }

    }

}
