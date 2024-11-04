using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a constructor of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericConstructorSymbol : ConstructorSymbol
    {

        readonly ConstructorSymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericConstructorSymbol(ISymbolContext context, TypeSymbol declaringType, ConstructorSymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override MethodAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override CallingConventions CallingConvention => _definition.CallingConvention;

        /// <inheritdoc />
        public override MethodImplAttributes MethodImplementationFlags => _definition.MethodImplementationFlags;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

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
        public override ImmutableList<TypeSymbol> GetGenericArguments()
        {
            return _definition.GetGenericArguments();
        }

        /// <inheritdoc />
        public override MethodImplAttributes GetMethodImplementationFlags()
        {
            return _definition.GetMethodImplementationFlags();
        }

        /// <inheritdoc />
        public override ImmutableList<ParameterSymbol> GetParameters()
        {
            return _definition.GetParameters();
        }

        /// <inheritdoc />
        public override bool IsDefined(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.IsDefined(attributeType, inherit);
        }

    }

}