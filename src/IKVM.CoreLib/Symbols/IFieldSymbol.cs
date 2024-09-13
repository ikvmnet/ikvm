using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	/// <summary>
	/// Discovers the attributes of a field and provides access to field metadata.
	/// </summary>
	interface IFieldSymbol : IMemberSymbol
	{

		/// <summary>
		/// Gets the attributes associated with this field.
		/// </summary>
		FieldAttributes Attributes { get; }

		/// <summary>
		/// Gets the type of this field object.
		/// </summary>
		ITypeSymbol FieldType { get; }

		/// <summary>
		/// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.Assembly"/>; that is, the field is visible at most to other types in the same assembly, and is not visible to derived types outside the assembly.
		/// </summary>
		bool IsAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.Family"/>; that is, the field is visible only within its class and derived classes.
		/// </summary>
		bool IsFamily { get; }

		/// <summary>
		/// Gets a value indicating whether the visibility of this field is described by <see cref="FieldAttributes.FamANDAssem"/>; that is, the field can be accessed from derived classes, but only if they are in the same assembly.
		/// </summary>
		bool IsFamilyAndAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether the potential visibility of this field is described by <see cref="FieldAttributes.FamORAssem"/>; that is, the field can be accessed by derived classes wherever they are, and by classes in the same assembly.
		/// </summary>
		bool IsFamilyOrAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether the field can only be set in the body of the constructor.
		/// </summary>
		bool IsInitOnly { get; }

		/// <summary>
		/// Gets a value indicating whether the value is written at compile time and cannot be changed.
		/// </summary>
		bool IsLiteral { get; }

		/// <summary>
		/// Gets a value indicating whether this field has the NotSerialized attribute.
		/// </summary>
		bool IsNotSerialized { get; }

		/// <summary>
		/// Gets a value indicating whether the corresponding PinvokeImpl attribute is set in FieldAttributes.
		/// </summary>
		bool IsPinvokeImpl { get; }

		/// <summary>
		/// Gets a value indicating whether the field is private.
		/// </summary>
		bool IsPrivate { get; }

		/// <summary>
		/// Gets a value indicating whether the field is public.
		/// </summary>
		bool IsPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the corresponding SpecialName attribute is set in the FieldAttributes enumerator.
		/// </summary>
		bool IsSpecialName { get; }

		/// <summary>
		/// Gets a value indicating whether the field is static.
		/// </summary>
		bool IsStatic { get; }

		/// <summary>
		/// Returns a literal value associated with the field by a compiler.
		/// </summary>
		/// <returns></returns>
		object? GetRawConstantValue();

		/// <summary>
		/// Gets an array of types that identify the optional custom modifiers of the field.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetOptionalCustomModifiers();

		/// <summary>
		/// Gets an array of types that identify the required custom modifiers of the property.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetRequiredCustomModifiers();

	}

}
