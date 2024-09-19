using System;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionPropertySymbol : IkvmReflectionMemberSymbol, IIkvmReflectionPropertySymbol
    {

        readonly PropertyInfo _property;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="property"></param>
        public IkvmReflectionPropertySymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol type, PropertyInfo property) :
            base(context, module, type)
        {
            _property = property ?? throw new ArgumentNullException(nameof(property));
        }

        /// <inheritdoc />
        public PropertyInfo UnderlyingProperty => _property;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingProperty;

        /// <inheritdoc />
        public System.Reflection.PropertyAttributes Attributes => (System.Reflection.PropertyAttributes)UnderlyingProperty.Attributes;

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

    }

}