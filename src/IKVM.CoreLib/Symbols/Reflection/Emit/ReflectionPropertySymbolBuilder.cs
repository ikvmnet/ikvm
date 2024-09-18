using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionPropertySymbolBuilder : ReflectionMemberSymbolBuilder, IReflectionPropertySymbolBuilder
    {

        PropertyBuilder? _builder;
        PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionPropertySymbolBuilder(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol resolvingType, PropertyBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _property = _builder;
        }

        /// <inheritdoc />
        public PropertyInfo UnderlyingProperty => _property;

        /// <inheritdoc />
        public PropertyBuilder UnderlyingPropertyBuilder => _builder ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingProperty;

        #region IPropertySymbol

        /// <inheritdoc />
        public void SetConstant(object? defaultValue)
        {
            UnderlyingPropertyBuilder.SetConstant(defaultValue);
        }

        /// <inheritdoc />
        public void SetGetMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.SetGetMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetSetMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.SetSetMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void AddOtherMethod(IMethodSymbolBuilder mdBuilder)
        {
            UnderlyingPropertyBuilder.AddOtherMethod(((IReflectionMethodSymbolBuilder)mdBuilder).UnderlyingMethodBuilder);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingPropertyBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingPropertyBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        #region IPropertySymbol

        /// <inheritdoc />
        public PropertyAttributes Attributes => UnderlyingProperty.Attributes;

        /// <inheritdoc />
        public bool CanRead => UnderlyingProperty.CanRead;

        /// <inheritdoc />
        public bool CanWrite => UnderlyingProperty.CanWrite;

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingProperty.IsSpecialName;

        /// <inheritdoc />
        public ITypeSymbol PropertyType => ResolveTypeSymbol(UnderlyingProperty.PropertyType);

        /// <inheritdoc />
        public IMethodSymbol? GetMethod => ResolveMethodSymbol(UnderlyingProperty.GetMethod);

        /// <inheritdoc />
        public IMethodSymbol? SetMethod => ResolveMethodSymbol(UnderlyingProperty.SetMethod);

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        /// <inheritdoc />
        public object? GetRawConstantValue()
        {
            return UnderlyingProperty.GetRawConstantValue();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetAccessors()
        {
            return ResolveMethodSymbols(UnderlyingProperty.GetAccessors());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetAccessors(bool nonPublic)
        {
            return ResolveMethodSymbols(UnderlyingProperty.GetAccessors(nonPublic));
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetIndexParameters()
        {
            return ResolveParameterSymbols(UnderlyingProperty.GetIndexParameters());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetGetMethod()
        {
            return ResolveMethodSymbol(UnderlyingProperty.GetGetMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetGetMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingProperty.GetGetMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetSetMethod()
        {
            return ResolveMethodSymbol(UnderlyingProperty.GetSetMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetSetMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingProperty.GetSetMethod(nonPublic));
        }

        /// <inheritdoc />
        public ITypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingProperty.GetOptionalCustomModifiers());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(UnderlyingProperty.GetRequiredCustomModifiers());
        }

#endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            _property = (PropertyInfo?)ResolvingModule.UnderlyingModule.ResolveMember(MetadataToken) ?? throw new InvalidOperationException();
            _builder = null;
            base.OnComplete();
        }

    }

}
