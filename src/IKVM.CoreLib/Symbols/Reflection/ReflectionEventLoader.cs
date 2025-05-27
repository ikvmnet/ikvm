using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionEventLoader : IEventLoader
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
        public ReflectionEventLoader(ReflectionSymbolContext context, EventInfo underlyingEvent)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingEvent = underlyingEvent ?? throw new ArgumentNullException(nameof(underlyingEvent));
        }

        /// <summary>
        /// Gets the underlying event.
        /// </summary>
        public EventInfo UnderlyingEvent => _underlyingEvent;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public AssemblySymbol GetAssembly() => _assembly.IsDefault ? _assembly.InterlockedInitialize(_context.ResolveAssemblySymbol(_underlyingEvent.Module.Assembly)) : _assembly.Value;

        /// <inheritdoc />
        public ModuleSymbol GetModule() => _module.IsDefault ? _module.InterlockedInitialize(_context.ResolveModuleSymbol(_underlyingEvent.Module)) : _module.Value;

        /// <inheritdoc />
        public TypeSymbol GetDeclaringType() => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingEvent.DeclaringType)) : _declaringType.Value;


        /// <inheritdoc />
        public EventAttributes GetAttributes() => _underlyingEvent.Attributes;

        /// <inheritdoc />
        public TypeSymbol GetEventHandlerType() => _eventHandlerType.IsDefault ? _eventHandlerType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingEvent.EventHandlerType)) : _eventHandlerType.Value;

        /// <inheritdoc />
        public string GetName() => _underlyingEvent.Name;

        /// <inheritdoc />
        public MethodSymbol? GetAddMethod() => _addMethod.IsDefault ? _addMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.AddMethod)) : _addMethod.Value;

        /// <inheritdoc />
        public MethodSymbol? GetRemoveMethod() => _removeMethod.IsDefault ? _removeMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.RemoveMethod)) : _removeMethod.Value;

        /// <inheritdoc />
        public MethodSymbol? GetRaiseMethod() => _raiseMethod.IsDefault ? _raiseMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingEvent.RaiseMethod)) : _raiseMethod.Value;

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetOtherMethods()
        {
            if (_otherMethods.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _otherMethods, _context.ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(true)));

            return _otherMethods;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingEvent.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
