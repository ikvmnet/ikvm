using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionPropertySymbol : IPropertySymbol
	{

		readonly ReflectionSymbolContext _context;
		readonly ReflectionTypeSymbol _type;
		readonly PropertyInfo _property;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="property"></param>
		public ReflectionPropertySymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, PropertyInfo property)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
			_type = type ?? throw new ArgumentNullException(nameof(type));
			_property = property ?? throw new ArgumentNullException(nameof(property));
		}

		public PropertyAttributes Attributes => throw new NotImplementedException();

		public bool CanRead => throw new NotImplementedException();

		public bool CanWrite => throw new NotImplementedException();

		public IMethodSymbol? GetMethod => throw new NotImplementedException();

		public bool IsSpecialName => throw new NotImplementedException();

		public ITypeSymbol PropertyType => throw new NotImplementedException();

		public IMethodSymbol? SetMethod => throw new NotImplementedException();

		public ITypeSymbol? DeclaringType => throw new NotImplementedException();

		public MemberTypes MemberType => throw new NotImplementedException();

		public int MetadataToken => throw new NotImplementedException();

		public IModuleSymbol Module => throw new NotImplementedException();

		public string Name => throw new NotImplementedException();

		public bool IsMissing => throw new NotImplementedException();

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

	}

}