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
            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModuleBuilder = (ModuleBuilder)_context.ResolveModule((ModuleSymbolBuilder)_builder.Module, IkvmReflectionSymbolState.Finished);
                else
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Finished);
            }

            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
                Declare();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();

            if (state >= IkvmReflectionSymbolState.Completed && _state < IkvmReflectionSymbolState.Completed)
            {
                // require parent to be finished; this is reentrant
                if (_builder.DeclaringType == null)
                    _parentModule = _context.ResolveModule(_builder.Module, IkvmReflectionSymbolState.Completed);
                else
                    _parentType = _context.ResolveType(_builder.DeclaringType, IkvmReflectionSymbolState.Completed);
            }

            if (state >= IkvmReflectionSymbolState.Completed && _state < IkvmReflectionSymbolState.Completed)
                Complete();
        }

        /// <summary>
        /// Executes the declare phase.
        /// </summary>
        void Declare()
        {
            if (_state != IkvmReflectionSymbolState.Default)
                throw new InvalidOperationException();

            var requiredCustomModifiers = _context.ResolveTypes(_builder.GetRequiredCustomModifiers(), IkvmReflectionSymbolState.Declared);
            var optionalCustomModifiers = _context.ResolveTypes(_builder.GetOptionalCustomModifiers(), IkvmReflectionSymbolState.Declared);
            var customModifiers = new CustomModifiersBuilder();
            customModifiers.Add(requiredCustomModifiers, optionalCustomModifiers);

            if (_parentModuleBuilder is not null)
                _fieldBuilder = _parentModuleBuilder.__DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, IkvmReflectionSymbolState.Declared), customModifiers.Create(), (IKVM.Reflection.FieldAttributes)_builder.Attributes);
            else if (_parentTypeBuilder is not null)
                _fieldBuilder = _parentTypeBuilder.DefineField(_builder.Name, _context.ResolveType(_builder.FieldType, IkvmReflectionSymbolState.Declared), requiredCustomModifiers, optionalCustomModifiers, (IKVM.Reflection.FieldAttributes)_builder.Attributes);
            else
                throw new InvalidOperationException();

            _state = IkvmReflectionSymbolState.Declared;
        }

        /// <summary>
        /// Executes the finish phase.
        /// </summary>
        void Finish()
        {
            if (_state != IkvmReflectionSymbolState.Declared)
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

            _state = IkvmReflectionSymbolState.Finished;
        }

        /// <summary>
        /// Executes the complete phase.
        /// </summary>
        void Complete()
        {
            if (_state != IkvmReflectionSymbolState.Finished)
                throw new InvalidOperationException();

            // set state
            _state = IkvmReflectionSymbolState.Completed;

            if (_parentModule is not null)
                _field = _parentModule.GetField(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else if (_parentType is not null)
                _field = _parentType.GetField(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
