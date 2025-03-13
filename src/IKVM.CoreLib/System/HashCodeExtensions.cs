using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace IKVM.CoreLib.System
{

    internal static class HashCodeExtensions
    {

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ref HashCode self, IEnumerable<T>? items)
        {
            if (items is null)
            {
                self.Add(0);
            }
            else
            {
                self.Add(1);
                foreach (var i in items)
                    self.Add(i);
            }
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ref HashCode self, T[]? items)
        {
            if (items is null)
            {
                self.Add(0);
            }
            else
            {
                self.Add(1);
                foreach (var i in items)
                    self.Add(i);
            }
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ref HashCode self, ImmutableArray<T> items)
        {
            if (items.IsDefault)
            {
                self.Add(0);
            }
            else
            {
                self.Add(1);
                foreach (var i in items)
                    self.Add(i);
            }
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        public static void AddRange<T>(this ref HashCode self, ImmutableHashSet<T> items, IComparer<T> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (items is null)
            {
                self.Add(0);
            }
            else
            {
                var l = items.ToArray();
                Array.Sort(l, comparer);

                self.Add(1);
                foreach (var i in items)
                    self.Add(i);
            }
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ref HashCode self, ImmutableHashSet<T> items)
        {
            AddRange(ref self, items, Comparer<T>.Default);
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        /// <param name="comparer"></param>
        public static void AddRange<T>(this ref HashCode self, ISet<T> items, IComparer<T> comparer)
        {
            if (comparer is null)
                throw new ArgumentNullException(nameof(comparer));

            if (items is null)
            {
                self.Add(0);
            }
            else
            {
                var l = items.ToArray();
                Array.Sort(l, comparer);

                self.Add(1);
                foreach (var i in items)
                    self.Add(i);
            }
        }

        /// <summary>
        /// Adds the enumerable of items to the <see cref="HashCode"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="items"></param>
        public static void AddRange<T>(this ref HashCode self, ISet<T> items)
        {
            AddRange(ref self, items, Comparer<T>.Default);
        }

    }

}
