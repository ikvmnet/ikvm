using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionConstructorSymbol : ReflectionMethodBaseSymbol, IConstructorSymbol
	{

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="type"></param>
		/// <param name="ctor"></param>
		public ReflectionConstructorSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, ConstructorInfo ctor) :
			base(context, type, ctor)
		{

		}

	}

}