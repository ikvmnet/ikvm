using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionEventSymbolBuilder : ReflectionMemberSymbolBuilder, IReflectionEventSymbolBuilder
    {

        readonly EventBuilder _builder;
        readonly ReflectionEventBuilderInfo _builderInfo;
        EventInfo? _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionEventSymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbolBuilder resolvingModule, IReflectionTypeSymbolBuilder resolvingType, EventBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _builderInfo = new ReflectionEventBuilderInfo(_builder);
        }

        /// <inheritdoc />
        public EventInfo UnderlyingEvent => _event ?? _builderInfo;

        /// <inheritdoc />
        public EventInfo UnderlyingEmitEvent => UnderlyingEvent;

        /// <inheritdoc />
        public EventBuilder UnderlyingEventBuilder => _builder ?? throw new NotImplementedException();

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingEvent;

        #region IEventSymbolBuilder

        /// <inheritdoc />
        public void SetAddOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetAddOnMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetRemoveOnMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetRemoveOnMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetRaiseMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.SetRaiseMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingEventBuilder.AddOtherMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingEventBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingEventBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        #region IEventSymbol

        /// <inheritdoc />
        public System.Reflection.EventAttributes Attributes => UnderlyingEvent.Attributes;

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
        public override bool IsComplete => _event != null;

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
            _event = (EventInfo?)ResolvingModule.UnderlyingModule.ResolveMember(MetadataToken) ?? throw new InvalidOperationException();
            base.OnComplete();
        }

    }

}
