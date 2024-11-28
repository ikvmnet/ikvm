using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class EventSymbolBuilder : EventSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        readonly EventAttributes _attributes;
        readonly TypeSymbol _eventType;

        MethodSymbolBuilder? _addMethod;
        MethodSymbolBuilder? _removeMethod;
        MethodSymbolBuilder? _raiseMethod;
        readonly ImmutableArray<MethodSymbolBuilder>.Builder _otherMethods = ImmutableArray.CreateBuilder<MethodSymbolBuilder>();
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="eventType"></param>
        internal EventSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType, string name, EventAttributes attributes, TypeSymbol eventType) :
            base(context, declaringType)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _eventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
        }

        /// <inheritdoc />
        public override EventAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override TypeSymbol? EventHandlerType => _eventType;

        /// <inheritdoc />
        public override string Name => _name;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => false;

        /// <inheritdoc />
        public override MethodSymbol? GetAddMethod(bool nonPublic)
        {
            if (nonPublic && _addMethod != null && _addMethod.IsPublic == false)
                return _addMethod;
            else if (nonPublic == false && _addMethod != null && _addMethod.IsPublic)
                return _addMethod;
            else
                return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            if (nonPublic && _raiseMethod != null && _raiseMethod.IsPublic == false)
                return _raiseMethod;
            else if (nonPublic == false && _raiseMethod != null && _raiseMethod.IsPublic)
                return _raiseMethod;
            else
                return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            if (nonPublic && _removeMethod != null && _removeMethod.IsPublic == false)
                return _removeMethod;
            else if (nonPublic == false && _removeMethod != null && _removeMethod.IsPublic)
                return _removeMethod;
            else
                return null;
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetOtherMethods(bool nonPublic)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>(_otherMethods.Count);

            foreach (var i in _otherMethods)
            {
                if (nonPublic && i != null && i.IsPublic == false)
                    b.Add(i);
                else if (nonPublic == false && i != null && i.IsPublic)
                    b.Add(i);
            }

            return b.DrainToImmutable();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <summary>
        /// Sets the method used to subscribe to this event.
        /// </summary>
        /// <param name="method"></param>
        public void SetAddOnMethod(MethodSymbolBuilder method)
        {
            _addMethod = method;
        }

        /// <summary>
        /// Sets the method used to unsubscribe to this event.
        /// </summary>
        /// <param name="method"></param>
        public void SetRemoveOnMethod(MethodSymbolBuilder method)
        {
            _removeMethod = method;
        }

        /// <summary>
        /// Sets the method used to raise this event.
        /// </summary>
        /// <param name="method"></param>
        public void SetRaiseMethod(MethodSymbolBuilder method)
        {
            _raiseMethod = method;
        }

        /// <summary>
        /// Adds one of the "other" methods associated with this event. "Other" methods are methods other than the "on" and "raise" methods associated with an event. This function can be called many times to add as many "other" methods.
        /// </summary>
        /// <param name="method"></param>
        public void AddOtherMethod(MethodSymbolBuilder method)
        {
            _otherMethods.Add(method);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

    }

}
