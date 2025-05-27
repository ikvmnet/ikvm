using System;
using System.Collections.Immutable;
using System.IO;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionAssemblySymbol : AssemblySymbol
    {

        internal readonly Assembly _underlyingAssembly;

        ImmutableArray<AssemblyIdentity> _references;
        ImmutableArray<ModuleSymbol> _modules;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionAssemblySymbol(IkvmReflectionSymbolContext context, Assembly underlyingAssembly) :
            base(context)
        {
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
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
        public sealed override bool IsMissing => _underlyingAssembly.__IsMissing;

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modules == default)
            {
                var l = _underlyingAssembly.GetModules();
                var b = ImmutableArray.CreateBuilder<ModuleSymbol>(l.Length);
                foreach (var module in l)
                    b.Add(new IkvmReflectionModuleSymbol(Context, this, module));

                ImmutableInterlocked.InterlockedInitialize(ref _modules, b.DrainToImmutable());
            }

            return _modules;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            if (_references == default)
                ImmutableInterlocked.InterlockedInitialize(ref _references, _underlyingAssembly.GetReferencedAssemblies().Pack());

            return _references;
        }

        /// <inheritdoc />
        public sealed override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            if (_underlyingAssembly.GetManifestResourceInfo(resourceName) is var r and not null)
                return new ManifestResourceInfo((System.Reflection.ResourceLocation)r.ResourceLocation, r.FileName, Context.ResolveAssemblySymbol(r.ReferencedAssembly));

            return null;
        }

        /// <inheritdoc />
        public sealed override Stream? GetManifestResourceStream(string name)
        {
            return _underlyingAssembly.GetManifestResourceStream(name);
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <summary>
        /// Saves this assembly to disk, specifying the nature of code in the assembly's executables and the target platform.
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="pekind"></param>
        /// <param name="imageFileMachine"></param>
        internal void Save(string assemblyFile, System.Reflection.PortableExecutableKinds pekind, ImageFileMachine imageFileMachine)
        {
            throw new NotImplementedException();
        }

    }

}
