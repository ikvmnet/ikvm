using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionAssemblySymbol : AssemblySymbol
    {

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
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
        }

        /// <inheritdoc />
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override IEnumerable<TypeSymbol> DefinedTypes => Context.ResolveTypeSymbols(_underlyingAssembly.DefinedTypes);

        /// <inheritdoc />
        public sealed override IEnumerable<TypeSymbol> ExportedTypes => Context.ResolveTypeSymbols(_underlyingAssembly.ExportedTypes);

        /// <inheritdoc />
        public sealed override MethodSymbol? EntryPoint => Context.ResolveMethodSymbol(_underlyingAssembly.EntryPoint);

        /// <inheritdoc />
        public sealed override string? FullName => _underlyingAssembly.FullName;

        /// <inheritdoc />
        public sealed override string ImageRuntimeVersion => _underlyingAssembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public sealed override string Location => _underlyingAssembly.Location;

        /// <inheritdoc />
        public sealed override ModuleSymbol ManifestModule => Context.ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override ImmutableArray<ModuleSymbol> GetModules()
        {
            if (_modules == default)
            {
                var b = ImmutableArray.CreateBuilder<ModuleSymbol>();
                var l = _underlyingAssembly.GetModules();
                foreach (var module in l)
                    b.Add(new ReflectionModuleSymbol(Context, this, module));

                ImmutableInterlocked.InterlockedInitialize(ref _modules, b.ToImmutable());
            }

            return _modules;
        }

        /// <inheritdoc />
        public sealed override AssemblyIdentity GetIdentity()
        {
            return _underlyingAssembly.GetName().Pack();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            if (_references == default)
                ImmutableInterlocked.InterlockedInitialize(ref _references, _underlyingAssembly.GetReferencedAssemblies().Pack());

            return _references;
        }

        /// <inheritdoc />
        public sealed override TypeSymbol? GetType(string name, bool throwOnError)
        {
            return Context.ResolveTypeSymbol(_underlyingAssembly.GetType(name, throwOnError, false));
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetTypes()
        {
            return Context.ResolveTypeSymbols(_underlyingAssembly.GetTypes());
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
        public sealed override Stream? GetManifestResourceStream(TypeSymbol type, string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
