using System;
using System.Collections.Immutable;
using System.IO;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionAssemblySymbol : AssemblySymbol
    {

        readonly AssemblyIdentity _identity;
        AssemblyDefinition? _def;

        ImmutableArray<ModuleSymbol> _modules;
        ImmutableArray<AssemblyIdentity> _referencedAssemblies;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="identity"></param>
        /// <param name="def"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DefinitionAssemblySymbol(SymbolContext symbol, AssemblyIdentity identity, AssemblyDefinition? def) :
            base(symbol)
        {
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
            _def = def;
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        AssemblyDefinition? Def => GetDef();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        AssemblyDefinition? GetDef()
        {
            if (_def is null)
                Interlocked.CompareExchange(ref _def, Context.ResolveAssemblyDef(_identity), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        AssemblyDefinition DefOrThrow => Def ?? throw new MissingAssemblySymbolException(this);

        /// <inheritdoc />
        public sealed override bool IsMissing => Def == null;

        /// <inheritdoc />
        public sealed override AssemblyIdentity Identity => _identity;

        /// <inheritdoc />
        public sealed override string ImageRuntimeVersion => DefOrThrow.GetImageRuntimeVersion();

        /// <inheritdoc />
        public sealed override string Location => DefOrThrow.GetLocation();

        /// <inheritdoc />
        public sealed override ModuleSymbol ManifestModule => DefOrThrow.GetManifestModule();

        /// <inheritdoc />
        public sealed override MethodSymbol? EntryPoint => DefOrThrow.GetEntryPoint();

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName) => DefOrThrow.GetManifestResourceInfo(resourceName);

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name) => DefOrThrow.GetManifestResourceStream(name);

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modules.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _modules, DefOrThrow.GetModules());

            return _modules;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            if (_referencedAssemblies.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _referencedAssemblies, DefOrThrow.GetReferencedAssemblies());

            return _referencedAssemblies;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => DefOrThrow.GetCustomAttributes();

        /// <summary>
        /// Attempts to resolve the module source of this assembly with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        internal ModuleDefinition ResolveModuleDef(string name) => Def?.ResolveModuleDef(name);

    }

}
