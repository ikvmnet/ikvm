using System;
using System.Collections.Immutable;
using System.IO;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionMissingAssemblySymbol : AssemblySymbol
    {

        internal readonly Assembly _underlyingAssembly;

        readonly ImmutableArray<ModuleSymbol> _modules;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionMissingAssemblySymbol(IkvmReflectionSymbolContext context, Assembly underlyingAssembly) :
            base(context)
        {
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
            _modules = [new IkvmReflectionMissingModuleSymbol(context, this, _underlyingAssembly.ManifestModule)];
        }

        /// <inheritdoc />
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override AssemblyIdentity Identity => _underlyingAssembly.GetName().Pack();

        /// <inheritdoc />
        public sealed override MethodSymbol? EntryPoint => Context.ResolveMethodSymbol(_underlyingAssembly.EntryPoint);

        /// <inheritdoc />
        public sealed override string ImageRuntimeVersion => _underlyingAssembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public sealed override string Location => _underlyingAssembly.Location;

        /// <inheritdoc />
        public sealed override ModuleSymbol ManifestModule => Context.ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public sealed override bool IsMissing => true;

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules()
        {
            return _modules;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            return [];
        }

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            return null;
        }

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name)
        {
            return null;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return [];
        }

    }

}
