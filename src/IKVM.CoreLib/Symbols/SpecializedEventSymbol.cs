using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes an event of a <see cref="SpecializedTypeSymbol"/>.
    /// </summary>
    class SpecializedEventSymbol : EventSymbol
    {

        readonly TypeSymbol _declaringType;
        readonly EventSymbol _definition;
        readonly GenericContext _genericContext;

        LazyField<TypeSymbol?> _eventHandlerType;
        LazyField<MethodSymbol?> _addMethod;
        LazyField<MethodSymbol?> _removeMethod;
        LazyField<MethodSymbol?> _raiseMethod;
        ImmutableArray<MethodSymbol> _otherMethods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpecializedEventSymbol(SymbolContext context, TypeSymbol declaringType, EventSymbol definition, GenericContext genericContext) :
            base(context)
        {
            _declaringType = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override TypeSymbol? DeclaringType => _declaringType;

        /// <inheritdoc />
        public sealed override EventAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override TypeSymbol? EventHandlerType => _eventHandlerType.IsDefault ? _eventHandlerType.InterlockedInitialize(ComputeEventHandlerType()) : _eventHandlerType.Value;

        /// <summary>
        /// Computes the value for <see cref="EventHandlerType"/>.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? ComputeEventHandlerType() => _definition.EventHandlerType?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public override MethodSymbol? AddMethod => _addMethod.IsDefault ? _addMethod.InterlockedInitialize(ComputeAddMethod()) : _addMethod.Value;

        /// <summary>
        /// Computes the value of <see cref="AddMethod"/>.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? ComputeAddMethod()
        {
            var baseMethod = _definition.AddMethod;
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetDeclaredMethods())
                if (i is SpecializedMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? RemoveMethod => _removeMethod.IsDefault ? _removeMethod.InterlockedInitialize(ComputeRemoveMethod()) : _removeMethod.Value;

        /// <summary>
        /// Computes the value of <see cref="RemoveMethod"/>.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? ComputeRemoveMethod()
        {
            var baseMethod = _definition.RemoveMethod;
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetDeclaredMethods())
                if (i is SpecializedMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public override MethodSymbol? RaiseMethod => _raiseMethod.IsDefault ? _raiseMethod.InterlockedInitialize(ComputeRaiseMethod()) : _raiseMethod.Value;

        /// <summary>
        /// Computes the value of <see cref="RaiseMethod"/>.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? ComputeRaiseMethod()
        {
            var baseMethod = _definition.RaiseMethod;
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetDeclaredMethods())
                if (i is SpecializedMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> OtherMethods => GetOtherMethods();

        /// <summary>
        /// Gets the value for <see cref="OtherMethods"/>.
        /// </summary>
        /// <returns></returns>
        new ImmutableArray<MethodSymbol> GetOtherMethods()
        {
            if (_otherMethods.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _otherMethods, ComputeOtherMethods());

            return _otherMethods;
        }

        /// <summary>
        /// Computes the value of <see cref="OtherMethods"/>.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<MethodSymbol> ComputeOtherMethods()
        {
            var l = _definition.OtherMethods;
            var b = ImmutableArray.CreateBuilder<MethodSymbol>(l.Length);

            foreach (var baseMethod in l)
            {
                if (baseMethod is not null)
                    foreach (var i in DeclaringType!.GetDeclaredMethods())
                        if (i is SpecializedMethodSymbol m)
                            if (m._definition == baseMethod)
                                b.Add(m);
            }

            return b.DrainToImmutable();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

    }

}
