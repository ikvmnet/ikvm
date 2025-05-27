using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionEventSymbolBuilderWriter
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly EventSymbolBuilder _builder;

        IkvmReflectionSymbolState _state = IkvmReflectionSymbolState.Default;
        TypeBuilder? _parentTypeBuilder;
        EventBuilder? _eventBuilder;
        Type? _parentType;
        EventInfo? _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionEventSymbolBuilderWriter(IkvmReflectionSymbolContext context, EventSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date underlying event object.
        /// </summary>
        public EventInfo? Event => _event ?? _eventBuilder;

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(IkvmReflectionSymbolState state)
        {
            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
                _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)(_builder.DeclaringType ?? throw new InvalidOperationException()), IkvmReflectionSymbolState.Finished);

            if (state >= IkvmReflectionSymbolState.Declared && _state < IkvmReflectionSymbolState.Declared)
                Declare();

            if (state >= IkvmReflectionSymbolState.Finished && _state < IkvmReflectionSymbolState.Finished)
                Finish();

            if (state >= IkvmReflectionSymbolState.Completed && _state < IkvmReflectionSymbolState.Completed)
                _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, IkvmReflectionSymbolState.Completed);

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

            // define event
            if (_parentTypeBuilder is not null)
                _eventBuilder = _parentTypeBuilder.DefineEvent(_builder.Name, (IKVM.Reflection.EventAttributes)_builder.Attributes, _context.ResolveType(_builder.EventHandlerType, IkvmReflectionSymbolState.Declared));
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
            if (_eventBuilder == null)
                throw new InvalidOperationException();

            // lock the builder so no changes can be made to it
            _builder.Freeze();

            // parent type must be finished
            var parentType = (TypeBuilder)_context.ResolveType(_builder.DeclaringType ?? throw new InvalidOperationException(), IkvmReflectionSymbolState.Finished);

            // parent resolution caused declaration of us
            if (_state != IkvmReflectionSymbolState.Default)
                return;

            // set add method
            if (_builder.AddMethod is MethodSymbolBuilder addMethod)
                _eventBuilder.SetAddOnMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)addMethod, IkvmReflectionSymbolState.Declared));

            // set remove method
            if (_builder.RemoveMethod is MethodSymbolBuilder removeMethod)
                _eventBuilder.SetRemoveOnMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)removeMethod, IkvmReflectionSymbolState.Declared));

            // set raise method
            if (_builder.RaiseMethod is MethodSymbolBuilder raiseMethod)
                _eventBuilder.SetRemoveOnMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)raiseMethod, IkvmReflectionSymbolState.Declared));

            // add other methods
            foreach (var m in _builder.GetOtherMethods())
                if (m is MethodSymbolBuilder otherMethod)
                    _eventBuilder.AddOtherMethod((MethodBuilder)_context.ResolveMethod((MethodSymbol)otherMethod, IkvmReflectionSymbolState.Declared));

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _eventBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

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

            if (_parentType is not null)
                _event = _parentType.GetEvent(_builder.Name, (BindingFlags)TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
