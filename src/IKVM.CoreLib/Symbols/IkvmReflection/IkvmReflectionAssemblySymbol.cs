using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Threading;

using Assembly = IKVM.Reflection.Assembly;
using Module = IKVM.Reflection.Module;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionAssemblySymbol : IkvmReflectionSymbol, IAssemblySymbol
    {

        Assembly _assembly;
        System.Reflection.AssemblyName? _assemblyName;

        IndexRangeDictionary<IkvmReflectionModuleSymbol> _moduleSymbols = new(initialCapacity: 1, maxCapacity: 32);
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public IkvmReflectionAssemblySymbol(IkvmReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        internal Assembly ReflectionObject => _assembly;

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            Debug.Assert(AssemblyNameEqualityComparer.Instance.Equals(module.Assembly.GetName().ToAssemblyName(), _assembly.GetName().ToAssemblyName()));

            // create lock on demand
            if (_moduleLock == null)
                Interlocked.CompareExchange(ref _moduleLock, new ReaderWriterLockSlim(), null);

            using (_moduleLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.ModuleReferenceHandle(module.MetadataToken));
                if (_moduleSymbols[row] == null)
                    using (_moduleLock.CreateWriteLock())
                        return _moduleSymbols[row] ??= new IkvmReflectionModuleSymbol(Context, this, module);
                else
                    return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_assembly.DefinedTypes);

        /// <inheritdoc />
        public IMethodSymbol? EntryPoint => _assembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_assembly.ExportedTypes);

        /// <inheritdoc />
        public string? FullName => _assembly.FullName;

        /// <inheritdoc />
        public string ImageRuntimeVersion => _assembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public IModuleSymbol ManifestModule => ResolveModuleSymbol(_assembly.ManifestModule);

        /// <inheritdoc />
        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_assembly.Modules);

        /// <inheritdoc />
        public ITypeSymbol[] GetExportedTypes()
        {
            return ResolveTypeSymbols(_assembly.GetExportedTypes());
        }

        /// <inheritdoc />
        public IModuleSymbol? GetModule(string name)
        {
            return _assembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules()
        {
            return ResolveModuleSymbols(_assembly.GetModules());
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules(bool getResourceModules)
        {
            return ResolveModuleSymbols(_assembly.GetModules(getResourceModules));
        }

        /// <inheritdoc />
        public System.Reflection.AssemblyName GetName()
        {
            return _assemblyName ??= _assembly.GetName().ToAssemblyName();
        }

        /// <inheritdoc />
        public System.Reflection.AssemblyName GetName(bool copiedName)
        {
            return _assembly.GetName().ToAssemblyName();
        }

        /// <inheritdoc />
        public System.Reflection.AssemblyName[] GetReferencedAssemblies()
        {
            var l = _assembly.GetReferencedAssemblies();
            var a = new System.Reflection.AssemblyName[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = l[i].ToAssemblyName();

            return a;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return _assembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return _assembly.GetType(name, throwOnError, ignoreCase) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name)
        {
            return _assembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_assembly.GetTypes());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData().Where(i => i.AttributeType == ((IkvmReflectionTypeSymbol)attributeType).ReflectionObject));
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _assembly.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

        /// <summary>
        /// Sets the reflection type. Used by the builder infrastructure to complete a symbol.
        /// </summary>
        /// <param name="assembly"></param>
        internal void Complete(Assembly assembly)
        {
            Context.GetOrCreateAssemblySymbol(_assembly = assembly);
        }

    }

}
