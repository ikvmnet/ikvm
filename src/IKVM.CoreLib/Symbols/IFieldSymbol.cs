namespace IKVM.CoreLib.Symbols
{

	interface IFieldSymbol : IMemberSymbol
	{

		System.Reflection.FieldAttributes Attributes { get; }

		ITypeSymbol FieldType { get; }

		bool IsAssembly { get; }

		bool IsFamily { get; }

		bool IsFamilyAndAssembly { get; }

		bool IsFamilyOrAssembly { get; }

		bool IsInitOnly { get; }

		bool IsLiteral { get; }

		bool IsNotSerialized { get; }

		bool IsPinvokeImpl { get; }

		bool IsPrivate { get; }

		bool IsPublic { get; }

		bool IsSpecialName { get; }

		bool IsStatic { get; }

		ITypeSymbol[] GetOptionalCustomModifiers();

		object? GetRawConstantValue();

		ITypeSymbol[] GetRequiredCustomModifiers();

	}

}
