using System;

using IKVM.Reflection;

using PropertyInfo = IKVM.Reflection.PropertyInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionPropertySymbol : IkvmReflectionMemberSymbol, IPropertySymbol
    {

        readonly PropertyInfo _underlyingProperty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingProperty"></param>
        public IkvmReflectionPropertySymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, PropertyInfo underlyingProperty) :
            base(context, type.ContainingModule, type, underlyingProperty)
        {
            _underlyingProperty = underlyingProperty ?? throw new ArgumentNullException(nameof(underlyingProperty));
        }

        public PropertyInfo UnderlyingProperty => _underlyingProperty;

        /// <inheritdoc />
        public global::System.Reflection.PropertyAttributes Attributes => (global::System.Reflection.PropertyAttributes)_underlyingProperty.Attributes;

        /// <inheritdoc />
        public ITypeSymbol PropertyType => ResolveTypeSymbol(_underlyingProperty.PropertyType);

        /// <inheritdoc />
        public bool CanRead => _underlyingProperty.CanRead;

        /// <inheritdoc />
        public bool CanWrite => _underlyingProperty.CanWrite;

        /// <inheritdoc />
        public bool IsSpecialName => _underlyingProperty.IsSpecialName;

        /// <inheritdoc />
        public IMethodSymbol? GetMethod => _underlyingProperty.GetMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? SetMethod => _underlyingProperty.SetMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public object? GetRawConstantValue()
        {
            return _underlyingProperty.GetRawConstantValue();
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetIndexParameters()
        {
            return ResolveParameterSymbols(_underlyingProperty.GetIndexParameters());
        }

        /// <inheritdoc />
        public ITypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetAccessors()
        {
            return ResolveMethodSymbols(_underlyingProperty.GetAccessors());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetGetMethod()
        {
            return _underlyingProperty.GetGetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetGetMethod(bool nonPublic)
        {
            return _underlyingProperty.GetGetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetSetMethod()
        {
            return _underlyingProperty.GetSetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetSetMethod(bool nonPublic)
        {
            return _underlyingProperty.GetSetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetOptionalCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingProperty.GetOptionalCustomModifiers());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetRequiredCustomModifiers()
        {
            return ResolveTypeSymbols(_underlyingProperty.GetRequiredCustomModifiers());
        }

    }

}