using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionModuleSymbol : DefinitionModuleSymbol
    {

        readonly IkvmReflectionSymbolContext _context;
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
        public IkvmReflectionModuleSymbol(IkvmReflectionSymbolContext context, Module underlyingModule) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingModule = underlyingModule ?? throw new ArgumentNullException(nameof(underlyingModule));
        }

        /// <summary>
        /// Gets the underlying module.
        /// </summary>
        public Module UnderlyingModule => _underlyingModule;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override AssemblySymbol Assembly => _assembly.IsDefault ? _assembly.InterlockedInitialize(_context.ResolveAssemblySymbol(_underlyingModule.Assembly)) : _assembly.Value;

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public sealed override string Name => _underlyingModule.Name;

        /// <inheritdoc />
        public sealed override string ScopeName => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public sealed override Guid ModuleVersionId => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public sealed override bool IsResource() => _underlyingModule.IsResource();

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var l = _underlyingModule.GetFields((BindingFlags)Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new IkvmReflectionFieldSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var l = _underlyingModule.GetMethods((BindingFlags)Symbol.DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new IkvmReflectionMethodSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _methods, b.DrainToImmutable());
            }

            return _methods;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredTypes()
        {
            if (_types.IsDefault)
            {
                var l = _underlyingModule.GetTypes();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    if (i.IsNested == false)
                        b.Add(new IkvmReflectionTypeSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _types, b.DrainToImmutable());
            }

            return _types;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingModule.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
