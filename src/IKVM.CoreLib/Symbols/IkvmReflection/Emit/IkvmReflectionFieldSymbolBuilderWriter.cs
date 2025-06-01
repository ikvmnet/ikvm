using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionFieldSymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly FieldSymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        ModuleBuilder? _parentModuleBuilder;
        TypeBuilder? _parentTypeBuilder;
        FieldBuilder? _fieldBuilder;
        Module? _parentModule;
        Type? _parentType;
        FieldInfo? _field;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionFieldSymbolBuilderWriter(IkvmReflectionSymbolContext context, FieldSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date underlying field object.
        /// </summary>
        public FieldInfo? Field => _field ?? _fieldBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, IkvmReflectionSymbolState.Emitted);
                else
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Emitted);
            }

            if (state >= IkvmReflectionSymbolState.Defined && _state < IkvmReflectionSymbolState.Defined)
                Define();

            if (state >= IkvmReflectionSymbolState.Emitted && _state < IkvmReflectionSymbolState.Emitted)
                Emit();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModule = _context.ResolveModule(_builder.Module, IkvmReflectionSymbolState.Finished);
                else
                    _parentType = _context.ResolveType(_builder.DeclaringType, IkvmReflectionSymbolState.Finished);
            }

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var requiredCustomModifiers = _context.ResolveTypes(_builder.GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Defined);
            var optionalCustomModifiers = _context.ResolveTypes(_builder.GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Defined);
            var customModifiers = new CustomModifiersBuilder();
            customModifiers.Add(requiredCustomModifiers, optionalCustomModifiers);

            if (_parentModuleBuilder is not null)
                _fieldBuilder = _parentModuleBuilder.__DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, IkvmReflectionSymbolState.Defined), customModifiers.Create(), (IKVM.Reflection.FieldAttributes)_builder.Attributes);
            else if (_parentTypeBuilder is not null)
                _fieldBuilder = _parentTypeBuilder.DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, IkvmReflectionSymbolState.Defined), requiredCustomModifiers, optionalCustomModifiers, (IKVM.Reflection.FieldAttributes)_builder.Attributes);
            else
                throw new InvalidOperationException();

            _state = IkvmReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != IkvmReflectionSymbolState.Defined)
                throw new InvalidOperationException();
            if (_fieldBuilder is null)
                throw new InvalidOperationException();

            // lock the builder so no changes can be made to it
            _builder.Freeze();

            // set properties
            _fieldBuilder.SetConstant(_builder.GetRawConstantValue());
            if (_builder.Offset is { } offset)
                _fieldBuilder.SetOffset(offset);

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _fieldBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

            _state = IkvmReflectionSymbolState.Emitted;
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Emitted)
                throw new InvalidOperationException();

            // set state
            _state = IkvmReflectionSymbolState.Finished;

            if (_parentModule is not null)
                _field = _parentModule.GetField(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _field = _parentType.GetField(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
