using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes an event of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericEventSymbol : EventSymbol
    {

        readonly EventSymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericEventSymbol(ISymbolContext context, TypeSymbol declaringType, EventSymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override EventAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override TypeSymbol? EventHandlerType => _definition.EventHandlerType?.Specialize(_genericContext);

        /// <inheritdoc />
        public override MethodSymbol? AddMethod => _definition.AddMethod;

        /// <inheritdoc />
        public override MethodSymbol? RemoveMethod => _definition.RemoveMethod;

        /// <inheritdoc />
        public override MethodSymbol? RaiseMethod => _definition.RaiseMethod;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override MethodSymbol? GetAddMethod()
        {
            return _definition.GetAddMethod();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetAddMethod(bool nonPublic)
        {
            return _definition.GetAddMethod(nonPublic);
        }

        /// <inheritdoc />
        public override CustomAttribute? GetCustomAttribute(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttribute(attributeType, inherit);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return _definition.GetCustomAttributes(inherit);
        }

        /// <inheritdoc />
        public override CustomAttribute[] GetCustomAttributes(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.GetCustomAttributes(attributeType, inherit);
        }

        /// <inheritdoc />
        public override MethodSymbol[] GetOtherMethods()
        {
            return _definition.GetOtherMethods();
        }

        /// <inheritdoc />
        public override MethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return _definition.GetOtherMethods(nonPublic);
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRaiseMethod()
        {
            return _definition.GetRaiseMethod();
        }
        /// <inheritdoc />

        public override MethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return _definition.GetRaiseMethod(nonPublic);
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRemoveMethod()
        {
            return _definition.GetRemoveMethod();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return _definition.GetRemoveMethod(nonPublic);
        }

        /// <inheritdoc />
        public override bool IsDefined(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.IsDefined(attributeType, inherit);
        }

    }

}