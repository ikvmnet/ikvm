using System;

namespace IKVM.CoreLib.Symbols.Reflection
{

	class ReflectionSZArrayTypeSymbol : ReflectionTypeSymbol
	{

		readonly ReflectionTypeSymbol _elementTypeSymbol;

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		/// <param name="context"></param>
		/// <param name="module"></param>
		/// <param name="type"></param>
		public ReflectionSZArrayTypeSymbol(ReflectionSymbolContext context, ReflectionModuleSymbol module, Type type, ReflectionTypeSymbol elementTypeSymbol) :
			base(context, module, type)
		{
			_elementTypeSymbol = elementTypeSymbol ?? throw new ArgumentNullException(nameof(elementTypeSymbol));
		}

	}

}