using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionParameterSymbol : ReflectionSymbol, IParameterSymbol
	{

		readonly ParameterInfo _parameter;
		readonly ReflectionMethodBaseSymbol _method;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="method"></param>
		/// <param name="parameter"></param>
		public ReflectionParameterSymbol(ReflectionSymbolContext context, ReflectionMethodBaseSymbol method, ParameterInfo parameter) :
			base(context)
		{
			_method = method ?? throw new ArgumentNullException(nameof(method));
			_parameter = parameter ?? throw new ArgumentNullException(nameof(parameter));
		}

		internal ReflectionMethodBaseSymbol ContainingMethod => _method;

		public ParameterAttributes Attributes => _parameter.Attributes;

		public object? DefaultValue => _parameter.DefaultValue;

		public bool HasDefaultValue => _parameter.HasDefaultValue;

		public bool IsIn => _parameter.IsIn;

		public bool IsLcid => _parameter.IsLcid;

		public bool IsOptional => _parameter.IsOptional;

		public bool IsOut => _parameter.IsOut;

		public bool IsRetval => _parameter.IsRetval;

		public IMemberSymbol Member => ResolveMemberSymbol(_parameter.Member);

		public int MetadataToken => _parameter.MetadataToken;

		public string? Name => _parameter.Name;

		public ITypeSymbol ParameterType => ResolveTypeSymbol(_parameter.ParameterType);

		public int Position => _parameter.Position;

		public CustomAttributeSymbol[] GetCustomAttributes(bool inherit)
		{
			throw new NotImplementedException();
		}

		public CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

		public bool IsDefined(ITypeSymbol attributeType, bool inherit)
		{
			throw new NotImplementedException();
		}

	}

}