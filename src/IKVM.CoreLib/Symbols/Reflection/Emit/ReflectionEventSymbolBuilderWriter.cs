﻿using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionEventSymbolBuilderWriter
    {

        readonly ReflectionSymbolContext _context;
        readonly EventSymbolBuilder _builder;

        ReflectionSymbolState _state = ReflectionSymbolState.Default;
        TypeBuilder? _parentTypeBuilder;
        EventBuilder? _eventBuilder;
        Type? _parentType;
        EventInfo? _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionEventSymbolBuilderWriter(ReflectionSymbolContext context, EventSymbolBuilder builder)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <summary>
        /// Gets the most up to date underlying event object.
        /// </summary>
        public EventInfo? Event => _event ?? throw new InvalidOperationException();

        /// <summary>
        /// Advances the builder to the given phase.
        /// </summary>
        /// <param name="state"></param>
        public void AdvanceTo(ReflectionSymbolState state)
        {
            lock (this)
            {
                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    _parentTypeBuilder = (TypeBuilder)_context.ResolveType((TypeSymbolBuilder)(_builder.DeclaringType ?? throw new InvalidOperationException()), ReflectionSymbolState.Emitted);

                if (state >= ReflectionSymbolState.Defined && _state < ReflectionSymbolState.Defined)
                    Define();

                if (state >= ReflectionSymbolState.Emitted && _state < ReflectionSymbolState.Emitted)
                    Emit();

                if (state >= ReflectionSymbolState.Finished && _state < ReflectionSymbolState.Finished)
                    _parentType = _context.ResolveType((TypeSymbolBuilder)_builder.DeclaringType, ReflectionSymbolState.Finished);

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

            // define event
            if (_parentTypeBuilder is not null)
                _eventBuilder = _parentTypeBuilder.DefineEvent(_builder.Name, _builder.Attributes, _context.ResolveType(_builder.EventHandlerType, ReflectionSymbolState.Defined));
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
            if (_eventBuilder == null)
                throw new InvalidOperationException();

            // lock the builder so no changes can be made to it
            _builder.Freeze();

            // parent type must be finished
            var parentType = (TypeBuilder)_context.ResolveType(_builder.DeclaringType ?? throw new InvalidOperationException(), ReflectionSymbolState.Emitted);

            // parent resolution caused declaration of us
            if (_state != ReflectionSymbolState.Default)
                return;

            // set add method
            if (_builder.AddMethod is MethodSymbolBuilder addMethod)
                _eventBuilder.SetAddOnMethod((MethodBuilder)_context.ResolveMethod(addMethod, ReflectionSymbolState.Defined));

            // set remove method
            if (_builder.RemoveMethod is MethodSymbolBuilder removeMethod)
                _eventBuilder.SetRemoveOnMethod((MethodBuilder)_context.ResolveMethod(removeMethod, ReflectionSymbolState.Defined));

            // set raise method
            if (_builder.RaiseMethod is MethodSymbolBuilder raiseMethod)
                _eventBuilder.SetRemoveOnMethod((MethodBuilder)_context.ResolveMethod(raiseMethod, ReflectionSymbolState.Defined));

            // add other methods
            foreach (var m in _builder.GetOtherMethods())
                if (m is MethodSymbolBuilder otherMethod)
                    _eventBuilder.AddOtherMethod((MethodBuilder)_context.ResolveMethod(otherMethod, ReflectionSymbolState.Defined));

            // set custom attributes
            foreach (var customAttribute in _builder.GetDeclaredCustomAttributes())
                _eventBuilder.SetCustomAttribute(_context.CreateCustomAttributeBuilder(customAttribute));

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

            if (_parentType is not null)
                _event = _parentType.GetEvent(_builder.Name, TypeSymbol.DeclaredOnlyLookup) ?? throw new InvalidOperationException();
            else
                throw new InvalidOperationException();
        }

    }

}
