namespace IKVM.CoreLib.Symbols
{

	/// <summary>
	/// Provides common properties and methods for managed symbols.
	/// </summary>
	interface ISymbol
	{

		/// <summary>
		/// Returns <c>true</c> if the symbol is missing.
		/// </summary>
		bool IsMissing { get; }

	}

}
