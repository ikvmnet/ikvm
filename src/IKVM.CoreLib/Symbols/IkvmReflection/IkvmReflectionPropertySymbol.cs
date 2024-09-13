using System;

using IKVM.Reflection;

using PropertyInfo = IKVM.Reflection.PropertyInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	class IkvmReflectionPropertySymbol : IkvmReflectionMemberSymbol, IPropertySymbol
	{

		readonly PropertyInfo _property;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="property"></param>
		public IkvmReflectionPropertySymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, PropertyInfo property) :
			base(context, type.ContainingModule, type, property)
		{
			_property = property ?? throw new ArgumentNullException(nameof(property));
		}

		public new PropertyInfo ReflectionObject => (PropertyInfo)base.ReflectionObject;

		/// <inheritdoc />
		public System.Reflection.PropertyAttributes Attributes => (System.Reflection.PropertyAttributes)_property.Attributes;

		/// <inheritdoc />
		public ITypeSymbol PropertyType => ResolveTypeSymbol(_property.PropertyType);

		/// <inheritdoc />
		public bool CanRead => _property.CanRead;

		/// <inheritdoc />
		public bool CanWrite => _property.CanWrite;

		/// <inheritdoc />
		public bool IsSpecialName => _property.IsSpecialName;

		/// <inheritdoc />
		public IMethodSymbol? GetMethod => _property.GetMethod is { } m ? ResolveMethodSymbol(m) : null;

		/// <inheritdoc />
		public IMethodSymbol? SetMethod => _property.SetMethod is { } m ? ResolveMethodSymbol(m) : null;

		/// <inheritdoc />
		public object? GetRawConstantValue()
		{
			return _property.GetRawConstantValue();
		}

		/// <inheritdoc />
		public IParameterSymbol[] GetIndexParameters()
		{
			return ResolveParameterSymbols(_property.GetIndexParameters());
		}

		/// <inheritdoc />
		public ITypeSymbol GetModifiedPropertyType()
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IMethodSymbol[] GetAccessors()
		{
			return ResolveMethodSymbols(_property.GetAccessors());
		}

		/// <inheritdoc />
		public IMethodSymbol[] GetAccessors(bool nonPublic)
		{
			throw new NotImplementedException();
		}

		/// <inheritdoc />
		public IMethodSymbol? GetGetMethod()
		{
			return _property.GetGetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		/// <inheritdoc />
		public IMethodSymbol? GetGetMethod(bool nonPublic)
		{
			return _property.GetGetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		/// <inheritdoc />
		public IMethodSymbol? GetSetMethod()
		{
			return _property.GetSetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		/// <inheritdoc />
		public IMethodSymbol? GetSetMethod(bool nonPublic)
		{
			return _property.GetSetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		/// <inheritdoc />
		public ITypeSymbol[] GetOptionalCustomModifiers()
		{
			return ResolveTypeSymbols(_property.GetOptionalCustomModifiers());
		}

		/// <inheritdoc />
		public ITypeSymbol[] GetRequiredCustomModifiers()
		{
			return ResolveTypeSymbols(_property.GetRequiredCustomModifiers());
		}
	}

}