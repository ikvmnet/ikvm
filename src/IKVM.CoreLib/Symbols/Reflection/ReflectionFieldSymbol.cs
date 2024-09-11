using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionFieldSymbol : IFieldSymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly ReflectionTypeSymbol _type;
		readonly FieldInfo _field;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="field"></param>
		public ReflectionFieldSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, FieldInfo field)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_field = field ?? throw new ArgumentNullException(nameof(field));
		}

		public FieldAttributes Attributes => throw new System.NotImplementedException();

		public ITypeSymbol FieldType => throw new System.NotImplementedException();

		public bool IsAssembly => throw new System.NotImplementedException();

		public bool IsFamily => throw new System.NotImplementedException();

		public bool IsFamilyAndAssembly => throw new System.NotImplementedException();

		public bool IsFamilyOrAssembly => throw new System.NotImplementedException();

		public bool IsInitOnly => throw new System.NotImplementedException();

		public bool IsLiteral => throw new System.NotImplementedException();

		public bool IsNotSerialized => throw new System.NotImplementedException();

		public bool IsPinvokeImpl => throw new System.NotImplementedException();

		public bool IsPrivate => throw new System.NotImplementedException();

		public bool IsPublic => throw new System.NotImplementedException();

		public bool IsSecurityCritical => throw new System.NotImplementedException();

		public bool IsSecuritySafeCritical => throw new System.NotImplementedException();

		public bool IsSecurityTransparent => throw new System.NotImplementedException();

		public bool IsSpecialName => throw new System.NotImplementedException();

		public bool IsStatic => throw new System.NotImplementedException();

		public ITypeSymbol? DeclaringType => throw new System.NotImplementedException();

		public MemberTypes MemberType => throw new System.NotImplementedException();

		public int MetadataToken => throw new System.NotImplementedException();

		public IModuleSymbol Module => throw new System.NotImplementedException();

		public string Name => throw new System.NotImplementedException();

		public bool IsMissing => throw new System.NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new System.NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new System.NotImplementedException();
		}

		public ITypeSymbol GetModifiedFieldType()
		{
			throw new System.NotImplementedException();
		}

		public ITypeSymbol[] GetOptionalCustomModifiers()
		{
			throw new System.NotImplementedException();
		}

		public object? GetRawConstantValue()
		{
			throw new System.NotImplementedException();
		}

		public ITypeSymbol[] GetRequiredCustomModifiers()
		{
			throw new System.NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new System.NotImplementedException();
		}
	}

}