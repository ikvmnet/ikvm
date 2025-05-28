using System;
using System.Collections.Immutable;
using System.IO;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMissingAssemblyLoader : IAssemblyLoader
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Assembly _underlyingAssembly;

        ImmutableArray<ModuleSymbol> _modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionMissingAssemblyLoader(IkvmReflectionSymbolContext context, Assembly underlyingAssembly)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
            _modules = [new DefinitionModuleSymbol(_context, new IkvmReflectionMissingModuleLoader(_context, _underlyingAssembly.ManifestModule))];
        }

        /// <summary>
        /// Gets the associated <see cref="Assembly"/>.
        /// </summary>
        public Assembly UnderlyingAssembly => _underlyingAssembly;

        /// <inheritdoc />
        public bool GetIsMissing() => true;

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
        public ImmutableArray<ModuleSymbol> GetModules() => _modules;

        /// <inheritdoc />
        public ImmutableArray<AssemblyIdentity> GetReferencedAssemblies() => [];

        /// <inheritdoc />
        public ManifestResourceInfo? GetManifestResourceInfo(string resourceName) => null;

        /// <inheritdoc />
        public Stream? GetManifestResourceStream(string name) => _underlyingAssembly.GetManifestResourceStream(name);

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes() => [];

    }

}
