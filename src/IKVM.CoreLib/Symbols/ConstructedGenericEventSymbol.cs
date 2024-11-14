using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes an event of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericEventSymbol : EventSymbol
    {

        internal readonly EventSymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericEventSymbol(SymbolContext context, TypeSymbol declaringType, EventSymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public sealed override EventAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public sealed override string Name => _definition.Name;

        /// <inheritdoc />
        public sealed override TypeSymbol? EventHandlerType => _definition.EventHandlerType?.Specialize(_genericContext);

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetAddMethod(bool nonPublic)
        {
            var baseMethod = _definition.GetAddMethod(nonPublic);
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is ConstructedGenericMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            var baseMethod = _definition.GetRemoveMethod(nonPublic);
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is ConstructedGenericMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            var baseMethod = _definition.GetRaiseMethod(nonPublic);
            if (baseMethod is null)
                return null;

            foreach (var i in DeclaringType!.GetMethods())
                if (i is ConstructedGenericMethodSymbol m)
                    if (m._definition == baseMethod)
                        return m;

            return null;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> GetOtherMethods(bool nonPublic)
        {
            var b = ImmutableArray.CreateBuilder<MethodSymbol>();

            foreach (var baseMethod in _definition.GetOtherMethods(nonPublic))
            {
                if (baseMethod is not null)
                    foreach (var i in DeclaringType!.GetMethods())
                        if (i is ConstructedGenericMethodSymbol m)
                            if (m._definition == baseMethod)
                                b.Add(m);
            }

            return b.ToImmutable();
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}