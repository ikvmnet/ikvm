using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionPropertySymbol : ReflectionMemberSymbol, IPropertySymbol
	{

		readonly PropertyInfo _property;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="property"></param>
		public ReflectionPropertySymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, PropertyInfo property) :
			base(context, type.ContainingModule, type, property)
		{
			_property = property ?? throw new ArgumentNullException(nameof(property));
		}

		public PropertyAttributes Attributes => _property.Attributes;

		public bool CanRead => _property.CanRead;

		public bool CanWrite => _property.CanWrite;

		public bool IsSpecialName => _property.IsSpecialName;

		public ITypeSymbol PropertyType => ResolveTypeSymbol(_property.PropertyType);

		public IMethodSymbol? GetMethod => _property.GetMethod is { } m ? ResolveMethodSymbol(m) : null;

		public IMethodSymbol? SetMethod => _property.SetMethod is { } m ? ResolveMethodSymbol(m) : null;

	}

}