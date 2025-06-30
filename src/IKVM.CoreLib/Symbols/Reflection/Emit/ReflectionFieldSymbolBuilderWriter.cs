using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionFieldSymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly FieldSymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
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
        public ReflectionFieldSymbolBuilderWriter(ReflectionSymbolContext context, FieldSymbolBuilder builder)
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
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                {
                    // require parent to be finished; this is reentrant
                    if (_builder.DeclaringType == null)
                        _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, ReflectionSymbolState.Emitted);
                    else
                        _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Emitted);
                }

                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                {
                    // require parent to be finished; this is reentrant
                    if (_builder.DeclaringType == null)
                        _parentModule = _context.ResolveModule(_builder.Module, ReflectionSymbolState.Finished);
                    else
                        _parentType = _context.ResolveType(_builder.DeclaringType, ReflectionSymbolState.Finished);
                }

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    Finish();
            }
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Define()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var requiredCustomModifiers = _context.ResolveTypes(_builder.GetRequiredCustomModifiers(), ReflectionSymbolState.Defined);
            var optionalCustomModifiers = _context.ResolveTypes(_builder.GetOptionalCustomModifiers(), ReflectionSymbolState.Defined);

            if (_parentModuleBuilder is not null)
                throw new NotSupportedException();
            else if (_parentTypeBuilder is not null)
                _fieldBuilder = _parentTypeBuilder.DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, ReflectionSymbolState.Defined), requiredCustomModifiers, optionalCustomModifiers, _builder.Attributes);
            else
                throw new InvalidOperationException();

            _state = ReflectionSymbolState.Defined;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Emit()
        {
            if (_state != ReflectionSymbolState.Defined)
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

            _state = ReflectionSymbolState.Emitted;
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Emitted)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Finished;

            if (_parentModule is not null)
                _field = _parentModule.GetField(_builder.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _field = _parentType.GetField(_builder.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
