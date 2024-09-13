using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IPropertySymbol : IMemberSymbol
	{

		PropertyAttributes Attributes { get; }

		ITypeSymbol PropertyType { get; }

		bool CanRead { get; }

		bool CanWrite { get; }

		bool IsSpecialName { get; }

		IMethodSymbol? GetMethod { get; }

		IMethodSymbol? SetMethod { get; }

		IMethodSymbol[] GetAccessors();

		object? GetConstantValue();

		object? GetRawConstantValue();

		ITypeSymbol[] GetOptionalCustomModifiers();

		ITypeSymbol[] GetRequiredCustomModifiers();

		ITypeSymbol GetModifiedPropertyType();

		IParameterSymbol[] GetIndexParameters();

		IMethodSymbol? GetGetMethod();

		IMethodSymbol? GetGetMethod(bool nonPublic);

		IMethodSymbol? GetSetMethod();

		IMethodSymbol? GetSetMethod(bool nonPublic);

	}

}
