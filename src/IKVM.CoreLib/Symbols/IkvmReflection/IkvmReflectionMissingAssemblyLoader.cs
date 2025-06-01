using System;
using System.Collections.Immutable;
using System.IO;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    sealed class IkvmReflectionMissingAssemblyLoader : DefinitionAssemblySymbol
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Assembly _underlyingAssembly;

        readonly ImmutableArray<ModuleSymbol> _modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionMissingAssemblyLoader(IkvmReflectionSymbolContext context, Assembly underlyingAssembly) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
            _modules = [new IkvmReflectionMissingModuleSymbol(_context, _underlyingAssembly.ManifestModule)];
        }

        /// <summary>
        /// Gets the associated <see cref="Assembly"/>.
        /// </summary>
        public Assembly UnderlyingAssembly => _underlyingAssembly;

        /// <inheritdoc />
        public sealed override bool IsMissing => true;

        /// <inheritdoc />
        public sealed override AssemblyIdentity Identity => _underlyingAssembly.GetName().Pack();

        /// <inheritdoc />
        public sealed override MethodSymbol? EntryPoint => _context.ResolveMethodSymbol(_underlyingAssembly.EntryPoint);

        /// <inheritdoc />
        public sealed override string ImageRuntimeVersion => _underlyingAssembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public sealed override string Location => _underlyingAssembly.Location;

        /// <inheritdoc />
        public sealed override ModuleSymbol ManifestModule => _context.ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules() => _modules;

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies() => [];

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName) => null;

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name) => _underlyingAssembly.GetManifestResourceStream(name);

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => [];

    }

}
