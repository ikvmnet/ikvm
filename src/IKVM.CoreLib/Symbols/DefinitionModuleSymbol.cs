using System;
using System.Collections.Immutable;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionModuleSymbol : ModuleSymbol
    {

        readonly string _name;
        readonly DefinitionAssemblySymbol _assemblyDef;

        ModuleDefinition? _def;

        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<TypeSymbol> _types;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assemblyDef"></param>
        /// <param name="name"></param>
        /// <param name="def"></param>
        public DefinitionModuleSymbol(SymbolContext context, DefinitionAssemblySymbol assemblyDef, string name, ModuleDefinition? def) :
            base(context, assemblyDef)
        {
            _assemblyDef = assemblyDef ?? throw new ArgumentNullException(nameof(assemblyDef));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _def = def;
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        ModuleDefinition? Def => GetDef();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        ModuleDefinition? GetDef()
        {
            if (_def is null)
                Interlocked.CompareExchange(ref _def, _assemblyDef.ResolveModuleDef(_name), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        ModuleDefinition DefOrThrow => Def ?? throw new MissingModuleSymbolException(this);

        /// <inheritdoc />
        public override bool IsMissing => Def == null;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => DefOrThrow.GetFullyQualifiedName();

        /// <inheritdoc />
        public sealed override string ScopeName => DefOrThrow.GetScopeName();

        /// <inheritdoc />
        public sealed override Guid ModuleVersionId => DefOrThrow.GetModuleVersionId();

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            // TODO integrate missing?
            if (_fields.IsDefault)
            {
                var l = DefOrThrow.GetFields();
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new DefinitionFieldSymbol(Context, this, null, l[i].GetName(), l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            // TODO integrate missing?
            if (_methods.IsDefault)
            {
                var l = DefOrThrow.GetMethods();
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new DefinitionMethodSymbol(Context, this, null, l[i].GetName(), l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            // TODO integrate missing?
            if (_types.IsDefault)
            {
                var l = DefOrThrow.GetTypes();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new DefinitionTypeSymbol(Context, this, l[i].GetName(), l[i].GetNamespace(), l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _types, b.DrainToImmutable());
            }

            return _types;
        }

        /// <inheritdoc />
        public sealed override bool IsResource() => DefOrThrow.GetIsResource();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => DefOrThrow.GetCustomAttributes();

        /// <summary>
        /// Attempts to resolve the definition for the specified field.
        /// </summary>
        /// <param name="name"></param>
        internal FieldDefinition? ResolveFieldDef(string name) => Def?.ResolveFieldDef(name);

        /// <summary>
        /// Attempts to resolve the definition for the specified method.
        /// </summary>
        /// <param name="signature"></param>
        internal MethodDefinition? ResolveMethodDef(MethodSymbolSignature signature) => Def?.ResolveMethodDef(signature);

        /// <summary>
        /// Attempts to resolve the definition for the specified type.
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="name"></param>
        internal TypeDefinition? ResolveTypeDef(string ns, string name) => Def?.ResolveTypeDef(ns, name);

    }

}
