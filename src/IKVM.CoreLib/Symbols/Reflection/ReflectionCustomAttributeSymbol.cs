using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionCustomAttributeSymbol : ICustomAttributeSymbol
	{

		readonly ReflectionSymbolContext context;
		readonly ICustomAttributeSymbolProvider provider;
		readonly object attribute;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="provider"></param>
		/// <param name="attribute"></param>
		public ReflectionCustomAttributeSymbol(ReflectionSymbolContext context, ICustomAttributeSymbolProvider provider, object attribute)
		{
			this.context = context;
			this.provider = provider;
			this.attribute = attribute;
		}

		public ITypeSymbol AttributeType => throw new System.NotImplementedException();

		public IConstructorSymbol Constructor => throw new System.NotImplementedException();

		public ImmutableArray<CustomAttributeTypedArgument> ConstructorArguments => throw new System.NotImplementedException();

		public ImmutableArray<CustomAttributeNamedArgument> NamedArguments => throw new System.NotImplementedException();
	}

}