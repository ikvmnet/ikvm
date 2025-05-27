using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionFieldLoader : IFieldLoader
    {

        readonly ReflectionSymbolContext _context;
        readonly FieldInfo _underlyingField;

        LazyField<AssemblySymbol> _assembly;
        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol> _fieldType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingField"></param>
        public ReflectionFieldLoader(ReflectionSymbolContext context, FieldInfo underlyingField)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingField = underlyingField ?? throw new ArgumentNullException(nameof(underlyingField));
        }

        /// <summary>
        /// Gets the underlying field.
        /// </summary>
        public FieldInfo UnderlyingField => _underlyingField;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public AssemblySymbol GetAssembly() => _assembly.IsDefault ? _assembly.InterlockedInitialize(_context.ResolveAssemblySymbol(_underlyingField.Module.Assembly)) : _assembly.Value;

        /// <inheritdoc />
        public ModuleSymbol GetModule() => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingField.Module)) : _module.Value;

        /// <inheritdoc />
        public TypeSymbol GetDeclaringType() => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingField.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public FieldAttributes GetAttributes() => _underlyingField.Attributes;

        /// <inheritdoc />
        public TypeSymbol GetFieldType() => _fieldType.IsDefault ? _fieldType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingField.FieldType)) : _fieldType.Value;

        /// <inheritdoc />
        public string GetName() => _underlyingField.Name;

        /// <inheritdoc />
        public object? GetConstantValue() => _underlyingField.GetRawConstantValue();

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingField.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingField.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingField.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
