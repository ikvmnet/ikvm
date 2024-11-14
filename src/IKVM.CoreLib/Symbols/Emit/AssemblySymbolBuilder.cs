using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Symbols.Emit
{

    class AssemblySymbolBuilder : AssemblySymbol
    {

        readonly AssemblyIdentity _identity;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public AssemblySymbolBuilder(SymbolContext context, AssemblyIdentity identity) :
            base(context)
        {
            _identity = identity ?? throw new ArgumentNullException(nameof(identity));
        }

        /// <inheritdoc />
        public override string? FullName => throw new NotImplementedException();

        /// <inheritdoc />
        public override string ImageRuntimeVersion => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Location => throw new NotImplementedException();

        /// <inheritdoc />
        public override ModuleSymbol ManifestModule => throw new NotImplementedException();

        /// <inheritdoc />
        public override IEnumerable<TypeSymbol> DefinedTypes => throw new NotImplementedException();

        /// <inheritdoc />
        public override IEnumerable<TypeSymbol> ExportedTypes => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodSymbol? EntryPoint => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => false;

        /// <inheritdoc />
        public override AssemblyIdentity GetIdentity()
        {
            return _identity;
        }

        /// <inheritdoc />
        public override ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Stream? GetManifestResourceStream(string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override Stream? GetManifestResourceStream(TypeSymbol type, string name)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<ModuleSymbol> GetModules()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<AssemblyIdentity> GetReferencedAssemblies()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override TypeSymbol? GetType(string name, bool throwOnError)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetTypes()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
