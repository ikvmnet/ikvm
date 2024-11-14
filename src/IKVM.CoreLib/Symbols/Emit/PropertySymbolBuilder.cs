using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    class PropertySymbolBuilder : PropertySymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public PropertySymbolBuilder(SymbolContext context, TypeSymbol declaringType) :
            base(context, declaringType)
        {

        }

        /// <inheritdoc />
        public override PropertyAttributes Attributes => throw new NotImplementedException();

        /// <inheritdoc />
        public override TypeSymbol PropertyType => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool CanRead => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool CanWrite => throw new NotImplementedException();

        /// <inheritdoc />
        public override string Name => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override TypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the default value of this property.
        /// </summary>
        /// <param name="defaultValue"></param>
        public void SetConstant(object? defaultValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the method that gets the property value.
        /// </summary>
        /// <param name="mdBuilder"></param>
        public void SetGetMethod(MethodSymbolBuilder mdBuilder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the method that sets the property value.
        /// </summary>
        /// <param name="mdBuilder"></param>
        public void SetSetMethod(MethodSymbolBuilder mdBuilder)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds one of the other methods associated with this property.
        /// </summary>
        /// <param name="mdBuilder"></param>
        public void AddOtherMethod(MethodSymbolBuilder mdBuilder)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
