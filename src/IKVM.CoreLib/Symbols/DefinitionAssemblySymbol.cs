using System;
using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents an assembly definition.
    /// </summary>
    class DefinitionAssemblySymbol : AssemblySymbol
    {

        readonly IAssemblyLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefinitionAssemblySymbol(SymbolContext context, IAssemblyLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public IAssemblyLoader Loader => _loader;

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override AssemblyIdentity Identity => _loader.GetIdentity();

        /// <inheritdoc />
        public sealed override string ImageRuntimeVersion => _loader.GetImageRuntimeVersion();

        /// <inheritdoc />
        public sealed override string Location => _loader.GetLocation();

        /// <inheritdoc />
        public sealed override ModuleSymbol ManifestModule => _loader.GetManifestModule();

        /// <inheritdoc />
        public sealed override MethodSymbol? EntryPoint => _loader.GetEntryPoint();

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName) => _loader.GetManifestResourceInfo(resourceName);

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name) => _loader.GetManifestResourceStream(name);

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules() => _loader.GetModules();

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies() => _loader.GetReferencedAssemblies();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
