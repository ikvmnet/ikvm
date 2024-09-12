using System;

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

		public System.Reflection.PropertyAttributes Attributes => (System.Reflection.PropertyAttributes)_property.Attributes;

		public bool CanRead => _property.CanRead;

		public bool CanWrite => _property.CanWrite;

		public bool IsSpecialName => _property.IsSpecialName;

		public ITypeSymbol PropertyType => ResolveTypeSymbol(_property.PropertyType);

		public IMethodSymbol? GetMethod => _property.GetMethod is { } m ? ResolveMethodSymbol(m) : null;

		public IMethodSymbol? SetMethod => _property.SetMethod is { } m ? ResolveMethodSymbol(m) : null;

	}

}