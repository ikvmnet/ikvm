using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionModuleLoader : IModuleLoader
    {

        readonly ReflectionSymbolContext _context;
        readonly Module _underlyingModule;

        LazyField<AssemblySymbol> _assembly;
        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<TypeSymbol> _types;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingModule"></param>
        public ReflectionModuleLoader(ReflectionSymbolContext context, Module underlyingModule)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingModule = underlyingModule ?? throw new ArgumentNullException(nameof(underlyingModule));
        }

        /// <summary>
        /// Gets the underlying module.
        /// </summary>
        public Module UnderlyingModule => _underlyingModule;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public AssemblySymbol GetAssembly() => _assembly.IsDefault ? _assembly.InterlockedInitialize(_context.ResolveAssemblySymbol(_underlyingModule.Assembly)) : _assembly.Value;

        /// <inheritdoc />
        public string GetFullyQualifiedName() => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public string GetName() => _underlyingModule.Name;

        /// <inheritdoc />
        public string GetScopeName() => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public Guid GetModuleVersionId() => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public bool GetIsResource() => _underlyingModule.IsResource();

        /// <inheritdoc />
        public ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var l = _underlyingModule.GetFields(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionFieldSymbol(_context, new ReflectionFieldLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var l = _underlyingModule.GetMethods(Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionMethodSymbol(_context, new ReflectionMethodLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            if (_types.IsDefault)
            {
                var l = _underlyingModule.GetTypes();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    if (i.IsNested == false)
                        b.Add(new DefinitionTypeSymbol(_context, new ReflectionTypeLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _types, b.DrainToImmutable());
            }

            return _types;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingModule.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
