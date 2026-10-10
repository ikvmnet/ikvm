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

    /// <summary>
    /// Provides array copying, comparison and hashing helpers used throughout IKVM.Reflection.
    /// </summary>
    static class Util
    {

        /// <summary>
        /// Returns a copy of the array.
        /// </summary>
        /// <param name="array">The array to copy, or <see langword="null"/>.</param>
        /// <returns>A new array with the same elements, or an empty array if <paramref name="array"/> is <see langword="null"/> or empty.</returns>
        internal static int[] Copy(int[] array)
        {
            if (array == null || array.Length == 0)
                return Array.Empty<int>();

            var copy = new int[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        /// <summary>
        /// Returns a copy of the array.
        /// </summary>
        /// <param name="array">The array to copy, or <see langword="null"/>.</param>
        /// <returns>A new array with the same elements, or <see cref="Type.EmptyTypes"/> if <paramref name="array"/> is <see langword="null"/> or empty.</returns>
        internal static Type[] Copy(Type[] array)
        {
            if (array == null || array.Length == 0)
                return Type.EmptyTypes;

            var copy = new Type[array.Length];
            Array.Copy(array, copy, array.Length);
            return copy;
        }

        /// <summary>
        /// Copies the elements of a list into a new array of a base element type.
        /// </summary>
        /// <typeparam name="T">The element type of the resulting array.</typeparam>
        /// <typeparam name="V">The element type of the list, which derives from <typeparamref name="T"/>.</typeparam>
        /// <param name="list">The list to copy, or <see langword="null"/>.</param>
        /// <param name="empty">The array to return when <paramref name="list"/> is <see langword="null"/> or empty.</param>
        /// <returns>A new array with the elements of <paramref name="list"/>, or <paramref name="empty"/>.</returns>
        internal static T[] ToArray<T, V>(List<V> list, T[] empty) where V : T
        {
            if (list == null || list.Count == 0)
                return empty;

            var array = new T[list.Count];
            for (var i = 0; i < array.Length; i++)
                array[i] = list[i];

            return array;
        }

        /// <summary>
        /// Determines whether two type arrays contain equal types in the same order. An empty array matches a
        /// <see langword="null"/> reference.
        /// </summary>
        /// <param name="t1">The first array, or <see langword="null"/>.</param>
        /// <param name="t2">The second array, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the arrays are equal; otherwise, <see langword="false"/>.</returns>
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
                for (var i = 0; i < t1.Length; i++)
                    if (!TypeEquals(t1[i], t2[i]))
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether two types are equal, allowing either to be <see langword="null"/>.
        /// </summary>
        /// <param name="t1">The first type, or <see langword="null"/>.</param>
        /// <param name="t2">The second type, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if both are <see langword="null"/> or the types are equal; otherwise, <see langword="false"/>.</returns>
        internal static bool TypeEquals(Type t1, Type t2)
        {
            if (t1 == t2)
                return true;
            if (t1 == null)
                return false;

            return t1.Equals(t2);
        }

        /// <summary>
        /// Computes a hash code for a type array, skipping <see langword="null"/> elements.
        /// </summary>
        /// <param name="types">The types to hash, or <see langword="null"/>.</param>
        /// <returns>The combined hash code, or 0 if <paramref name="types"/> is <see langword="null"/>.</returns>
        internal static int GetHashCode(Type[] types)
        {
            if (types == null)
                return 0;

            var h = 0;
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

        /// <summary>
        /// Determines whether two custom modifier arrays contain equal elements in the same order. Unlike the
        /// <see cref="Type"/> array overload, a <see langword="null"/> array only matches another <see langword="null"/>.
        /// </summary>
        /// <param name="m1">The first array, or <see langword="null"/>.</param>
        /// <param name="m2">The second array, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if the arrays are equal; otherwise, <see langword="false"/>.</returns>
        internal static bool ArrayEquals(CustomModifiers[] m1, CustomModifiers[] m2)
        {
            if (m1 == null || m2 == null)
                return m1 == m2;

            if (m1.Length != m2.Length)
                return false;

            for (var i = 0; i < m1.Length; i++)
                if (!m1[i].Equals(m2[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// Computes a hash code for a custom modifier array.
        /// </summary>
        /// <param name="mods">The custom modifiers to hash, or <see langword="null"/>.</param>
        /// <returns>The combined hash code, or 0 if <paramref name="mods"/> is <see langword="null"/>.</returns>
        internal static int GetHashCode(CustomModifiers[] mods)
        {
            var h = 0;
            if (mods != null)
                foreach (var mod in mods)
                    h ^= mod.GetHashCode();

            return h;
        }

        /// <summary>
        /// Gets the element at the specified index, or the default value if the array is <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The array, or <see langword="null"/>.</param>
        /// <param name="index">The index of the element.</param>
        /// <returns>The element at <paramref name="index"/>, or <see langword="default"/> if <paramref name="array"/> is <see langword="null"/>.</returns>
        internal static T NullSafeElementAt<T>(T[] array, int index) => array == null ? default : array[index];

        /// <summary>
        /// Gets the length of the array, treating <see langword="null"/> as empty.
        /// </summary>
        /// <typeparam name="T">The element type of the array.</typeparam>
        /// <param name="array">The array, or <see langword="null"/>.</param>
        /// <returns>The length of <paramref name="array"/>, or 0 if it is <see langword="null"/>.</returns>
        internal static int NullSafeLength<T>(T[] array) => array == null ? 0 : array.Length;

    }

}
