using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    sealed class ReflectionEventSymbol : DefinitionEventSymbol
    {

        readonly ReflectionSymbolContext _context;
        readonly EventInfo _underlyingEvent;

        LazyField<AssemblySymbol> _assembly;
        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol> _declaringType;
        LazyField<TypeSymbol> _eventHandlerType;
        LazyField<MethodSymbol> _addMethod;
        LazyField<MethodSymbol> _removeMethod;
        LazyField<MethodSymbol> _raiseMethod;
        ImmutableArray<MethodSymbol> _otherMethods;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingEvent"></param>
        public ReflectionEventSymbol(ReflectionSymbolContext context, EventInfo underlyingEvent) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingEvent = underlyingEvent ?? throw new ArgumentNullException(nameof(underlyingEvent));
        }

        /// <summary>
        /// Gets the underlying event.
        /// </summary>
        public EventInfo UnderlyingEvent => _underlyingEvent;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override TypeSymbol DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingEvent.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public sealed override EventAttributes Attributes => _underlyingEvent.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol EventHandlerType => _eventHandlerType.IsDefault ? _eventHandlerType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingEvent.EventHandlerType)) : _eventHandlerType.Value;

        /// <inheritdoc />
        public sealed override string Name => _underlyingEvent.Name;

        /// <inheritdoc />
        public sealed override MethodSymbol? AddMethod => _addMethod.IsDefault ? _addMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.AddMethod)) : _addMethod.Value;

        /// <inheritdoc />
        public sealed override MethodSymbol? RemoveMethod => _removeMethod.IsDefault ? _removeMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.RemoveMethod)) : _removeMethod.Value;

        /// <inheritdoc />
        public sealed override MethodSymbol? RaiseMethod => _raiseMethod.IsDefault ? _raiseMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.RaiseMethod)) : _raiseMethod.Value;

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> OtherMethods
        {
            get
            {
                if (_otherMethods.IsDefault)
                    ImmutableInterlocked.InterlockedInitialize(ref _otherMethods, _context.ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(true)));

                return _otherMethods;
            }
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingEvent.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
