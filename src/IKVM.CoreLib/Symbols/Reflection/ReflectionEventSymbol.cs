using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionEventSymbol : EventSymbol
    {

        internal readonly EventInfo _underlyingEvent;

        TypeSymbol? _eventHandlerType;
        MethodSymbol? _addMethod;
        MethodSymbol? _nonPublicAddMethod;
        MethodSymbol? _removeMethod;
        MethodSymbol? _nonPublicRemoveMethod;
        MethodSymbol? _raiseMethod;
        MethodSymbol? _nonPublicRaiseMethod;
        ImmutableArray<MethodSymbol> _otherMethods;
        ImmutableArray<MethodSymbol> _nonPublicOtherMethods;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="underlyingEvent"></param>
        public ReflectionEventSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol declaringType, EventInfo underlyingEvent) :
            base(context, declaringType)
        {
            _underlyingEvent = underlyingEvent ?? throw new ArgumentNullException(nameof(underlyingEvent));
        }

        /// <summary>
        /// Gets the associated symbol context.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <summary>
        /// Gets the underlying object.
        /// </summary>
        internal EventInfo UnderlyingEvent => _underlyingEvent;

        /// <inheritdoc />
        public sealed override EventAttributes Attributes => _underlyingEvent.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol? EventHandlerType => _eventHandlerType ??= Context.ResolveTypeSymbol(_underlyingEvent.EventHandlerType);

        /// <inheritdoc />
        public sealed override string Name => _underlyingEvent.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetAddMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicAddMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetAddMethod(true));
            else
                return _addMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetAddMethod(false));
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicRemoveMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetRemoveMethod(true));
            else
                return _removeMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetRemoveMethod(false));
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicRaiseMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetRaiseMethod(true));
            else
                return _raiseMethod ??= Context.ResolveMethodSymbol(_underlyingEvent.GetRaiseMethod(false));
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> GetOtherMethods(bool nonPublic)
        {
            if (nonPublic)
            {
                if (_nonPublicOtherMethods == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _nonPublicOtherMethods, Context.ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(true)));

                return _nonPublicOtherMethods;
            }
            else
            {
                if (_otherMethods == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _otherMethods, Context.ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(false)));

                return _otherMethods;
            }
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingEvent.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
