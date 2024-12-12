using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    public abstract class EventSymbol : MemberSymbol
    {

        readonly TypeSymbol _declaringType;

        ImmutableArray<MethodSymbol> _otherMethodsPublic;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected EventSymbol(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType.Module)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
        }

        /// <inheritdoc />
        public override TypeSymbol DeclaringType => _declaringType;

        /// <summary>
        /// Gets the attributes for this event.
        /// </summary>
        public abstract EventAttributes Attributes { get; }

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => MemberTypes.Event;

        /// <summary>
        /// Gets the <see cref="TypeSymbol"> object of the underlying event-handler delegate associated with this event.
        /// </summary>
        public abstract TypeSymbol? EventHandlerType { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="EventSymbol"/> has a name with a special meaning.
        /// </summary>
        public bool IsSpecialName => (Attributes & EventAttributes.SpecialName) != 0;

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source.
        /// </summary>
        public abstract MethodSymbol? AddMethod { get; }

        /// <summary>
        /// Returns the method used to remove an event handler delegate from the event source.
        /// </summary>
        public abstract MethodSymbol? RemoveMethod { get; }

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        public abstract MethodSymbol? RaiseMethod { get; }

        /// <summary>
        /// Returns the methods that have been associated with the event in metadata using the .other directive.
        /// </summary>
        public abstract ImmutableArray<MethodSymbol> OtherMethods { get; }

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source.
        /// </summary>
        /// <returns></returns>
        public MethodSymbol? GetAddMethod() => GetAddMethod(false);

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public MethodSymbol? GetAddMethod(bool nonPublic) => nonPublic || (AddMethod is { } m && m.IsPublic) ? AddMethod : null;

        /// <summary>
        /// Returns the method used to remove an event handler delegate from the event source.
        /// </summary>
        /// <returns></returns>
        public MethodSymbol? GetRemoveMethod() => GetRemoveMethod(false);

        /// <summary>
        /// When overridden in a derived class, retrieves the <see cref="MethodSymbol"/> object for removing a method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public MethodSymbol? GetRemoveMethod(bool nonPublic) => nonPublic || (RemoveMethod is { } m && m.IsPublic) ? RemoveMethod : null;

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        /// <returns></returns>
        public MethodSymbol? GetRaiseMethod() => GetRaiseMethod(false);

        /// <summary>
        /// When overridden in a derived class, returns the method that is called when the event is raised, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public MethodSymbol? GetRaiseMethod(bool nonPublic) => nonPublic || (RaiseMethod is { } m && m.IsPublic) ? RaiseMethod : null;

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<MethodSymbol> GetOtherMethods() => GetOtherMethods(false);

        /// <summary>
        /// Returns the methods that have been associated with the event in metadata using the .other directive, specifying whether to include non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public ImmutableArray<MethodSymbol> GetOtherMethods(bool nonPublic)
        {
            if (nonPublic)
                return OtherMethods;
            else
            {
                if (_otherMethodsPublic.IsDefault)
                    ImmutableInterlocked.InterlockedInitialize(ref _otherMethodsPublic, OtherMethods.Where(i => i.IsPublic).ToImmutableArray());

                return _otherMethodsPublic;
            }
        }
    }

}
