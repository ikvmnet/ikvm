using System;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionEventSymbolBuilder : IkvmReflectionMemberSymbolBuilder, IIkvmReflectionEventSymbolBuilder
    {

        EventBuilder? _builder;
        EventInfo _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionEventSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionTypeSymbolBuilder resolvingType, EventBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _event = _builder;
        }

        /// <inheritdoc />
        public EventInfo UnderlyingEvent => _event;

        /// <inheritdoc />
        public EventBuilder UnderlyingEventBuilder => _builder ?? throw new NotImplementedException();

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingEvent;

        #region IEventSymbolBuilder

        /// <inheritdoc />
        public void SetAddOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetAddOnMethod(((IIkvmReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetRemoveOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetRemoveOnMethod(((IIkvmReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetRaiseMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetRaiseMethod(((IIkvmReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.AddOtherMethod(((IIkvmReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingEventBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingEventBuilder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        #region IEventSymbol

        /// <inheritdoc />
        public System.Reflection.EventAttributes Attributes => (System.Reflection.EventAttributes)UnderlyingEvent.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? EventHandlerType => ResolveTypeSymbol(UnderlyingEvent.EventHandlerType);

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingEvent.IsSpecialName;

        /// <inheritdoc />
        public IMethodSymbol? AddMethod => ResolveMethodSymbol(UnderlyingEvent.AddMethod);

        /// <inheritdoc />
        public IMethodSymbol? RemoveMethod => ResolveMethodSymbol(UnderlyingEvent.RemoveMethod);

        /// <inheritdoc />
        public IMethodSymbol? RaiseMethod => ResolveMethodSymbol(UnderlyingEvent.RaiseMethod);

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetAddMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetAddMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRemoveMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRemoveMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRaiseMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRaiseMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(UnderlyingEvent.GetOtherMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(UnderlyingEvent.GetOtherMethods(nonPublic));
        }

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _builder = null;
            base.OnComplete();
        }

    }

}
