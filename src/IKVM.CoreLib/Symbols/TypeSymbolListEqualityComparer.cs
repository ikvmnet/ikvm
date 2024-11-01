using System.Collections.Generic;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Compares two ReflectionTypeSymbol array instances for equality.
    /// </summary>
    class TypeSymbolListEqualityComparer : IEqualityComparer<ITypeSymbol[]>
    {

        public static readonly TypeSymbolListEqualityComparer Instance = new();

        public bool Equals(ITypeSymbol[]? x, ITypeSymbol[]? y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
                if (x[i] != y[i])
                    return false;

            return true;
        }

        public int GetHashCode(ITypeSymbol[] obj)
        {
            int result = 17;

            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i].GetHashCode();
                }
            }

            return result;
        }

    }

}
