using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.Reflection.Emit;

using Assembly = IKVM.Reflection.Assembly;
using ConstructorInfo = IKVM.Reflection.ConstructorInfo;
using EventInfo = IKVM.Reflection.EventInfo;
using FieldInfo = IKVM.Reflection.FieldInfo;
using MethodBase = IKVM.Reflection.MethodBase;
using MethodInfo = IKVM.Reflection.MethodInfo;
using Module = IKVM.Reflection.Module;
using ParameterInfo = IKVM.Reflection.ParameterInfo;
using PropertyInfo = IKVM.Reflection.PropertyInfo;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    /// <summary>
    /// Holds references to symbols derived from System.Reflection.
    /// </summary>
    class IkvmReflectionSymbolContext
    {

        readonly ConcurrentDictionary<System.Reflection.AssemblyName, WeakReference<IkvmReflectionAssemblySymbol>> _symbolByName = new(AssemblyNameEqualityComparer.Instance);
        readonly ConditionalWeakTable<Assembly, IkvmReflectionAssemblySymbol> _symbolByAssembly = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReflectionSymbolContext()
        {

        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionAssemblySymbol"/> indexed based on the assembly's name.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IkvmReflectionAssemblySymbol GetOrCreateAssemblySymbolByName(Assembly assembly)
        {
            var r = _symbolByName.GetOrAdd(assembly.GetName().ToAssemblyName(), _ => new WeakReference<IkvmReflectionAssemblySymbol>(new IkvmReflectionAssemblySymbol(this, assembly)));

            // reference has valid symbol
            if (r.TryGetTarget(out var s))
                return s;

            // no valid symbol, must have been released, lock to restore
            lock (r)
            {
                // still gone, recreate
                if (r.TryGetTarget(out s) == false)
                    r.SetTarget(s = new IkvmReflectionAssemblySymbol(this, assembly));

                return s;
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionAssemblySymbol"/> for the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public IkvmReflectionAssemblySymbol GetOrCreateAssemblySymbol(Assembly assembly)
        {
            return _symbolByAssembly.GetValue(assembly, GetOrCreateAssemblySymbolByName);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return GetOrCreateAssemblySymbol(module.Assembly).GetOrCreateModuleSymbol(module);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionTypeSymbol"/> for the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            return GetOrCreateModuleSymbol(type.Module).GetOrCreateTypeSymbol(type);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionMethodBaseSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is ConstructorInfo ctor)
                return GetOrCreateConstructorSymbol(ctor);
            else
                return GetOrCreateMethodSymbol((MethodInfo)method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionConstructorSymbol"/> for the specified <see cref="ConstructorInfo"/>.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return GetOrCreateModuleSymbol(ctor.Module).GetOrCreateConstructorSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionMethodSymbol"/> for the specified <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return GetOrCreateModuleSymbol(method.Module).GetOrCreateMethodSymbol(method);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterInfo"/>.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return GetOrCreateModuleSymbol(parameter.Member.Module).GetOrCreateParameterSymbol(parameter);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionFieldSymbol"/> for the specified <see cref="FieldInfo"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return GetOrCreateModuleSymbol(field.Module).GetOrCreateFieldSymbol(field);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionPropertySymbol"/> for the specified <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return GetOrCreateModuleSymbol(property.Module).GetOrCreatePropertySymbol(property);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionEventSymbol"/> for the specified <see cref="EventInfo"/>.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return GetOrCreateModuleSymbol(@event.Module).GetOrCreateEventSymbol(@event);
        }

        /// <summary>
        /// Gets or creates a <see cref="IkvmReflectionParameterSymbol"/> for the specified <see cref="ParameterBuilder"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IkvmReflectionParameterSymbol GetOrCreateParameterSymbol(MethodBuilder method, ParameterBuilder parameter)
        {
            return GetOrCreateParameterSymbol(new IkvmReflectionParameterBuilderInfo(method, parameter));
        }

    }

}
