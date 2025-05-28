using System;
using System.Collections.Immutable;
using System.IO;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionAssemblyLoader : IAssemblyLoader
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Assembly _underlyingAssembly;

        ImmutableArray<AssemblyIdentity> _references;
        ImmutableArray<ModuleSymbol> _modules;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionAssemblyLoader(IkvmReflectionSymbolContext context, Assembly underlyingAssembly)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
        }

        /// <summary>
        /// Gets the associated <see cref="Assembly"/>.
        /// </summary>
        public Assembly UnderlyingAssembly => _underlyingAssembly;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public AssemblyIdentity GetIdentity() => _underlyingAssembly.GetName().Pack();

        /// <inheritdoc />
        public MethodSymbol? GetEntryPoint() => _context.ResolveMethodSymbol(_underlyingAssembly.EntryPoint);

        /// <inheritdoc />
        public string GetImageRuntimeVersion() => _underlyingAssembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public string GetLocation() => _underlyingAssembly.Location;

        /// <inheritdoc />
        public ModuleSymbol GetManifestModule() => _context.ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modules == default)
            {
                var l = _underlyingAssembly.GetModules();
                var b = ImmutableArray.CreateBuilder<ModuleSymbol>(l.Length);
                foreach (var module in l)
                    b.Add(new DefinitionModuleSymbol(_context, new IkvmReflectionModuleLoader(_context, module)));

                ImmutableInterlocked.InterlockedInitialize(ref _modules, b.DrainToImmutable());
            }

            return _modules;
        }

        /// <inheritdoc />
        public ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            if (_references == default)
                ImmutableInterlocked.InterlockedInitialize(ref _references, _underlyingAssembly.GetReferencedAssemblies().Pack());

            return _references;
        }

        /// <inheritdoc />
        public ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Stream? GetManifestResourceStream(string name)
        {
            return _underlyingAssembly.GetManifestResourceStream(name);
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
