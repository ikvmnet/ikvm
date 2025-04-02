using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionPropertySymbol : ReflectionMemberSymbol, IPropertySymbol
    {

        readonly PropertyInfo _underlyingProperty;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingProperty"></param>
        public ReflectionPropertySymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, PropertyInfo underlyingProperty) :
            base(context, type.ContainingModule, type, underlyingProperty)
        {
            _underlyingProperty = underlyingProperty ?? throw new ArgumentNullException(nameof(underlyingProperty));
        }

        /// <summary>
        /// Gets the underlying <see cref="PropertyInfo"/> wrapped by this symbol.
        /// </summary>
        internal PropertyInfo UnderlyingProperty => _underlyingProperty;

        /// <inheritdoc />
        public PropertyAttributes Attributes => _underlyingProperty.Attributes;

        /// <inheritdoc />
        public bool CanRead => _underlyingProperty.CanRead;

        /// <inheritdoc />
        public bool CanWrite => _underlyingProperty.CanWrite;

        /// <inheritdoc />
        public bool IsSpecialName => _underlyingProperty.IsSpecialName;

        /// <inheritdoc />
        public ITypeSymbol PropertyType => ResolveTypeSymbol(_underlyingProperty.PropertyType);

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
        public IMethodSymbol[] GetAccessors()
        {
            return ResolveMethodSymbols(_underlyingProperty.GetAccessors());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetAccessors(bool nonPublic)
        {
            return ResolveMethodSymbols(_underlyingProperty.GetAccessors(nonPublic));
        }

        /// <inheritdoc />
        public IParameterSymbol[] GetIndexParameters()
        {
            return ResolveParameterSymbols(_underlyingProperty.GetIndexParameters());
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
        public ITypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
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