using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a property of a <see cref="ConstructedGenericTypeSymbol"/>.
    /// </summary>
    class ConstructedGenericPropertySymbol : PropertySymbol
    {

        readonly PropertySymbol _definition;
        readonly GenericContext _genericContext;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericPropertySymbol(ISymbolContext context, TypeSymbol declaringType, PropertySymbol definition, GenericContext genericContext) :
            base(context, declaringType)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol PropertyType => _definition.PropertyType.Specialize(_genericContext);

        /// <inheritdoc />
        public override bool CanRead => _definition.CanRead;

        /// <inheritdoc />
        public override bool CanWrite => _definition.CanWrite;

        /// <inheritdoc />
        public override MethodSymbol? GetMethod => _definition.GetMethod;

        /// <inheritdoc />
        public override MethodSymbol? SetMethod => _definition.SetMethod;

        /// <inheritdoc />
        public override MemberTypes MemberType => _definition.MemberType;

        /// <inheritdoc />
        public override string Name => _definition.Name;

        /// <inheritdoc />
        public override bool IsMissing => _definition.IsMissing;

        /// <inheritdoc />
        public override bool ContainsMissing => _definition.ContainsMissing;

        /// <inheritdoc />
        public override bool IsComplete => _definition.IsComplete;

        /// <inheritdoc />
        public override MethodSymbol[] GetAccessors()
        {
            return _definition.GetAccessors();
        }

        /// <inheritdoc />
        public override MethodSymbol[] GetAccessors(bool nonPublic)
        {
            return _definition.GetAccessors(nonPublic);
        }

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod()
        {
            return _definition.GetGetMethod();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            return _definition.GetGetMethod(nonPublic);
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod()
        {
            return _definition.GetSetMethod();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            return _definition.GetSetMethod(nonPublic);
        }

        /// <inheritdoc />
        public override ParameterSymbol[] GetIndexParameters()
        {
            return _definition.GetIndexParameters();
        }

        /// <inheritdoc />
        public override TypeSymbol GetModifiedPropertyType()
        {
            return _definition.GetModifiedPropertyType();
        }

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _definition.GetRawConstantValue();
        }

        /// <inheritdoc />
        public override TypeSymbol[] GetOptionalCustomModifiers()
        {
            return _definition.GetOptionalCustomModifiers();
        }

        /// <inheritdoc />
        public override TypeSymbol[] GetRequiredCustomModifiers()
        {
            return _definition.GetRequiredCustomModifiers();
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
        public override bool IsDefined(TypeSymbol attributeType, bool inherit = false)
        {
            return _definition.IsDefined(attributeType, inherit);
        }

    }

}