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
            if (state >= ReflectionSymbolState.Declared && _state < ReflectionSymbolState.Declared)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, ReflectionSymbolState.Finished);
                else
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Finished);
            }

            if (state >= ReflectionSymbolState.Declared && _state < ReflectionSymbolState.Declared)
                Declare();

            if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                Finish();

            if (state >= ReflectionSymbolState.Completed && _state < ReflectionSymbolState.Completed)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModule = _context.ResolveModule(_builder.Module, ReflectionSymbolState.Completed);
                else
                    _parentType = _context.ResolveType(_builder.DeclaringType, ReflectionSymbolState.Completed);
            }

            if (state >= ReflectionSymbolState.Completed && _state < ReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Declare()
        {
            if (_state != ReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var requiredCustomModifiers = _context.ResolveTypes(_builder.GetRequiredCustomModifiers(), ReflectionSymbolState.Declared);
            var optionalCustomModifiers = _context.ResolveTypes(_builder.GetOptionalCustomModifiers(), ReflectionSymbolState.Declared);

            if (_parentModuleBuilder is not null)
                throw new NotSupportedException();
            else if (_parentTypeBuilder is not null)
                _fieldBuilder = _parentTypeBuilder.DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, ReflectionSymbolState.Declared), requiredCustomModifiers, optionalCustomModifiers, _builder.Attributes);
            else
                throw new InvalidOperationException();

            _state = ReflectionSymbolState.Declared;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Finish()
        {
            if (_state != ReflectionSymbolState.Declared)
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

            _state = ReflectionSymbolState.Finished;
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Complete()
        {
            if (_state != ReflectionSymbolState.Finished)
                throw new InvalidOperationException();

            // set state
            _state = ReflectionSymbolState.Completed;

            if (_parentModule is not null)
                _field = _parentModule.GetField(_builder.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _field = _parentType.GetField(_builder.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
