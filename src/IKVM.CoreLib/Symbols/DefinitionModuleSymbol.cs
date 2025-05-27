using System;
using System.Collections.Immutable;

using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionModuleSymbol : ModuleSymbol
    {

        readonly IModuleLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionModuleSymbol(SymbolContext context, IModuleLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the underlying module loader.
        /// </summary>
        public IModuleLoader Loader => _loader;

        /// <inheritdoc />
        public sealed override AssemblySymbol Assembly => _loader.GetAssembly();

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => _loader.GetFullyQualifiedName();

        /// <inheritdoc />
        public sealed override string ScopeName => _loader.GetScopeName();

        /// <inheritdoc />
        public sealed override Guid ModuleVersionId => _loader.GetModuleVersionId();

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields() => _loader.GetDeclaredFields();

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods() => _loader.GetDeclaredMethods();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredTypes() => _loader.GetDeclaredTypes();

        /// <inheritdoc />
        public sealed override bool IsResource() => _loader.GetIsResource();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
