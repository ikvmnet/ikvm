using System.Collections.Generic;
using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Compares two ReflectionTypeSymbol array instances for equality.
    /// </summary>
    class TypeSymbolListEqualityComparer : IEqualityComparer<IReadOnlyList<TypeSymbol>>, IEqualityComparer<ImmutableArray<TypeSymbol>>
    {

        public static readonly TypeSymbolListEqualityComparer Instance = new();

        /// <inheritdoc />
        public bool Equals(IReadOnlyList<TypeSymbol>? x, IReadOnlyList<TypeSymbol>? y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            if (x.Count != y.Count)
                return false;

            for (int i = 0; i < x.Count; i++)
                if (x[i] != y[i])
                    return false;

            return true;
        }

        /// <inheritdoc />
        public int GetHashCode(IReadOnlyList<TypeSymbol> obj)
        {
            int result = 17;

            for (int i = 0; i < obj.Count; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i].GetHashCode();
                }
            }

            return result;
        }

        /// <inheritdoc />
        public bool Equals(ImmutableArray<TypeSymbol> x, ImmutableArray<TypeSymbol> y)
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

        /// <inheritdoc />
        public int GetHashCode(ImmutableArray<TypeSymbol> obj)
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
