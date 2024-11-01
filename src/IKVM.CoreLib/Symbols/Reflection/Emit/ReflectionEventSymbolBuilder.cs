using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionEventSymbolBuilder : ReflectionEventSymbol, IReflectionEventSymbolBuilder
    {

        readonly EventBuilder _builder;
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
            base(context, resolvingModule, resolvingType, new ReflectionEventBuilderInfo(builder))
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public EventBuilder UnderlyingEventBuilder => _builder;

        /// <inheritdoc />
        public override EventInfo UnderlyingRuntimeEvent => _event ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder ResolvingModuleBuilder => (IReflectionModuleSymbolBuilder)ResolvingModule;

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

        #endregion

        #region ICustomAttributeProviderBuilder

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingEventBuilder.SetCustomAttribute(attribute.Unpack());
        }

        #endregion

        #region IEventSymbol

        /// <inheritdoc />
        public override bool IsComplete => _event != null;

        #endregion

        /// <inheritdoc />
        public void OnComplete()
        {
            _event = (EventInfo?)ResolvingModule.UnderlyingModule.ResolveMember(MetadataToken) ?? throw new InvalidOperationException();
        }

    }

}
