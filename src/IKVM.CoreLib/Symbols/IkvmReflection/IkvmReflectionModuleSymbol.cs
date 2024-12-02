using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionModuleSymbol : ModuleSymbol
    {

        internal readonly Module _underlyingModule;

        ImmutableArray<FieldSymbol> _fields;
        ImmutableArray<MethodSymbol> _methods;
        ImmutableArray<TypeSymbol> _types;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        /// <param name="module"></param>
        public IkvmReflectionModuleSymbol(IkvmReflectionSymbolContext context, IkvmReflectionAssemblySymbol assembly, Module module) :
            base(context, assembly)
        {
            _underlyingModule = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override string FullyQualifiedName => _underlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public sealed override string Name => _underlyingModule.Name;

        /// <inheritdoc />
        public sealed override string ScopeName => _underlyingModule.ScopeName;

        /// <inheritdoc />
        public override Guid ModuleVersionId => _underlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public sealed override bool IsMissing => _underlyingModule.__IsMissing;

        /// <inheritdoc />
        internal sealed override ImmutableArray<FieldSymbol> GetDeclaredFields()
        {
            if (_fields.IsDefault)
            {
                var l = _underlyingModule.GetFields((BindingFlags)DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<FieldSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new IkvmReflectionFieldSymbol(Context, this, null, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _fields, b.DrainToImmutable());
            }

            return _fields;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<MethodSymbol> GetDeclaredMethods()
        {
            if (_methods.IsDefault)
            {
                var l = _underlyingModule.GetMethods((BindingFlags)DeclaredOnlyLookup);
                var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new IkvmReflectionMethodSymbol(Context, this, null, l[i]));

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
                for (int i = 0; i < l.Length; i++)
                    if (l[i].IsNested == false)
                        b.Add(new IkvmReflectionTypeSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _types, b.DrainToImmutable());
            }

            return _types;
        }

        /// <inheritdoc />
        public sealed override bool IsResource()
        {
            return _underlyingModule.IsResource();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingModule.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
