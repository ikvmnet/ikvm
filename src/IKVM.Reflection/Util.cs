/*
  Copyright (C) 2008-2011 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;

namespace IKVM.Reflection
{

    static class Util
    {
        internal static int[] Copy(int[] array)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<int>();

            var copy = new int[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        internal static Type[] Copy(Type[] array)
        {
            if (array == null || array.Length == 0)
                return Type.EmptyTypes;

            var copy = new Type[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        internal static T[] ToArray<T, V>(List<V> list, T[] empty) where V : T
        {
            if (list == null || list.Count == 0)
                return empty;

            var array = new T[list.Count];
            for (int i = 0; i < array.Length; i++)
                array[i] = list[i];

            return array;
        }

        internal static T[] ToArray<T>(IEnumerable<T> values)
        {
            return values == null ? Array.Empty<T>() : new List<T>(values).ToArray();
        }

        // note that an empty array matches a null reference
        internal static bool ArrayEquals(Type[] t1, Type[] t2)
        {
            if (t1 == t2)
                return true;

            if (t1 == null)
                return t2.Length == 0;
            else if (t2 == null)
                return t1.Length == 0;

            if (t1.Length == t2.Length)
            {
                for (int i = 0; i < t1.Length; i++)
                    if (!TypeEquals(t1[i], t2[i]))
                        return false;

                return true;
            }

            return false;
        }

        internal static bool TypeEquals(Type t1, Type t2)
        {
            if (t1 == t2)
                return true;
            if (t1 == null)
                return false;

            return t1.Equals(t2);
        }

        internal static int GetHashCode(Type[] types)
        {
            if (types == null)
                return 0;

            int h = 0;
            foreach (var t in types)
            {
                if (t != null)
                {
                    h *= 3;
                    h ^= t.GetHashCode();
                }
            }

            return h;
        }

        internal static bool ArrayEquals(CustomModifiers[] m1, CustomModifiers[] m2)
        {
            if (m1 == null || m2 == null)
                return m1 == m2;

            if (m1.Length != m2.Length)
                return false;

            for (int i = 0; i < m1.Length; i++)
                if (!m1[i].Equals(m2[i]))
                    return false;

            return true;
        }

        internal static int GetHashCode(CustomModifiers[] mods)
        {
            int h = 0;
            if (mods != null)
                foreach (var mod in mods)
                    h ^= mod.GetHashCode();

            return h;
        }

        internal static T NullSafeElementAt<T>(T[] array, int index)
        {
            return array == null ? default : array[index];
        }

        internal static int NullSafeLength<T>(T[] array)
        {
            return array == null ? 0 : array.Length;
        }

    }

}
