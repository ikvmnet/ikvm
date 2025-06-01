using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    sealed class ReflectionAssemblySymbol : DefinitionAssemblySymbol
    {

        readonly ReflectionSymbolContext _context;
        readonly Assembly _underlyingAssembly;

        ImmutableArray<AssemblyIdentity> _references;
        ImmutableArray<ModuleSymbol> _modules;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly underlyingAssembly) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
        }

        /// <summary>
        /// Gets the associated symbol context.
        /// </summary>
        public new ReflectionSymbolContext Context => _context;

        /// <summary>
        /// Gets the associated <see cref="Assembly"/>.
        /// </summary>
        public Assembly UnderlyingAssembly => _underlyingAssembly;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

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
        public sealed override ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modules.IsDefault)
            {
                var l = _underlyingAssembly.GetModules();
                var b = ImmutableArray.CreateBuilder<ModuleSymbol>(l.Length);
                foreach (var module in l)
                    b.Add(new ReflectionModuleSymbol(_context, module));

                ImmutableInterlocked.InterlockedInitialize(ref _modules, b.DrainToImmutable());
            }

            return _modules;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            if (_references.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _references, _underlyingAssembly.GetReferencedAssemblies().Pack());

            return _references;
        }

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name)
        {
            return _underlyingAssembly.GetManifestResourceStream(name);
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
