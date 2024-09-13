using System;

using FieldInfo = IKVM.Reflection.FieldInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

	class IkvmReflectionFieldSymbol : IkvmReflectionMemberSymbol, IFieldSymbol
	{

		readonly FieldInfo _field;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="field"></param>
		public IkvmReflectionFieldSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, FieldInfo field) :
			base(context, type.ContainingModule, type, field)
		{
			_field = field ?? throw new ArgumentNullException(nameof(field));
		}

		internal new FieldInfo ReflectionObject => (FieldInfo)base.ReflectionObject;

		public System.Reflection.FieldAttributes Attributes => (System.Reflection.FieldAttributes)_field.Attributes;

		public ITypeSymbol FieldType => ResolveTypeSymbol(_field.FieldType);

		public bool IsAssembly => _field.IsAssembly;

		public bool IsFamily => _field.IsFamily;

		public bool IsFamilyAndAssembly => _field.IsFamilyAndAssembly;

		public bool IsFamilyOrAssembly => _field.IsFamilyOrAssembly;

		public bool IsInitOnly => _field.IsInitOnly;

		public bool IsLiteral => _field.IsLiteral;

#pragma warning disable SYSLIB0050 // Type or member is obsolete
		public bool IsNotSerialized => _field.IsNotSerialized;
#pragma warning restore SYSLIB0050 // Type or member is obsolete

		public bool IsPinvokeImpl => _field.IsPinvokeImpl;

		public bool IsPrivate => _field.IsPrivate;

		public bool IsPublic => _field.IsPublic;

		public bool IsSpecialName => _field.IsSpecialName;

		public bool IsStatic => _field.IsStatic;

		public ITypeSymbol[] GetOptionalCustomModifiers()
		{
			return ResolveTypeSymbols(_field.GetOptionalCustomModifiers());
		}

		public object? GetRawConstantValue()
		{
			return _field.GetRawConstantValue();
		}

		public ITypeSymbol[] GetRequiredCustomModifiers()
		{
			return ResolveTypeSymbols(_field.GetRequiredCustomModifiers());
		}

	}

}