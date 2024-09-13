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

		internal new PropertyInfo ReflectionObject => (PropertyInfo)base.ReflectionObject;

		public PropertyAttributes Attributes => _property.Attributes;

		public bool CanRead => _property.CanRead;

		public bool CanWrite => _property.CanWrite;

		public bool IsSpecialName => _property.IsSpecialName;

		public ITypeSymbol PropertyType => ResolveTypeSymbol(_property.PropertyType);

		public IMethodSymbol? GetMethod => _property.GetMethod is { } m ? ResolveMethodSymbol(m) : null;

		public IMethodSymbol? SetMethod => _property.SetMethod is { } m ? ResolveMethodSymbol(m) : null;

		public IMethodSymbol[] GetAccessors()
		{
			return ResolveMethodSymbols(_property.GetAccessors());
		}

		public object? GetConstantValue()
		{
			return _property.GetConstantValue();
		}

		public IMethodSymbol? GetGetMethod()
		{
			return _property.GetGetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetGetMethod(bool nonPublic)
		{
			return _property.GetGetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		public IParameterSymbol[] GetIndexParameters()
		{
			return ResolveParameterSymbols(_property.GetIndexParameters());
		}

		public ITypeSymbol GetModifiedPropertyType()
		{
			throw new NotImplementedException();
		}

		public ITypeSymbol[] GetOptionalCustomModifiers()
		{
			return ResolveTypeSymbols(_property.GetOptionalCustomModifiers());
		}

		public ITypeSymbol[] GetRequiredCustomModifiers()
		{
			return ResolveTypeSymbols(_property.GetRequiredCustomModifiers());
		}

		public object? GetRawConstantValue()
		{
			return _property.GetRawConstantValue();
		}

		public IMethodSymbol? GetSetMethod()
		{
			return _property.GetSetMethod() is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

		public IMethodSymbol? GetSetMethod(bool nonPublic)
		{
			return _property.GetSetMethod(nonPublic) is MethodInfo m ? ResolveMethodSymbol(m) : null;
		}

	}

}