using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IMethodBaseSymbol : IMemberSymbol
	{

		MethodAttributes Attributes { get; }

		CallingConventions CallingConvention { get; }

		bool ContainsGenericParameters { get; }

		bool IsAbstract { get; }

		bool IsAssembly { get; }

		bool IsConstructor { get; }

		bool IsFamily { get; }

		bool IsFamilyAndAssembly { get; }

		bool IsFamilyOrAssembly { get; }

		bool IsFinal { get; }

		bool IsGenericMethod { get; }

		bool IsGenericMethodDefinition { get; }

		bool IsHideBySig { get; }

		bool IsPrivate { get; }

		bool IsPublic { get; }

		bool IsSpecialName { get; }

		bool IsStatic { get; }

		bool IsVirtual { get; }

		MethodImplAttributes MethodImplementationFlags { get; }

		ITypeSymbol[] GetGenericArguments();

		MethodImplAttributes GetMethodImplementationFlags();

		IParameterSymbol[] GetParameters();

	}

}