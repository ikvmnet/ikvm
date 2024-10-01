using System;
using System.Collections.Generic;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Compares two <see cref="Type"/> array instances for equality.
    /// </summary>
    class TypeListEqualityComparer : IEqualityComparer<Type[]>
    {

        public static readonly TypeListEqualityComparer Instance = new();

        public bool Equals(Type[]? x, Type[]? y)
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

        public int GetHashCode(Type[] obj)
        {
            int result = 17;

            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 41 + obj[i].GetHashCode();
                }
            }

            return result;
        }

    }

}
