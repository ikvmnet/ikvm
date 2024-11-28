using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace IKVM.CoreLib.Collections
{

    public static class ImmutableExtensions
    {

        /// <summary>
        /// Value-type implementation of <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public readonly struct ImmutableArrayValueComparer<T, TComparer> : IEqualityComparer<ImmutableArray<T>>
            where TComparer : IEqualityComparer<T>
        {

            readonly TComparer comparer;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="comparer"></param>
            public ImmutableArrayValueComparer(TComparer comparer)
            {
                this.comparer = comparer;
            }

            /// <inheritdoc />
            public bool Equals(ImmutableArray<T> x, ImmutableArray<T> y)
            {
                if (x == y)
                    return true;

                if (x.Length != y.Length)
                    return false;

                for (int i = 0; i < x.Length; i++)
                    if (comparer.Equals(x[i], y[i]) == false)
                        return false;

                return true;
            }

            /// <inheritdoc />
            public int GetHashCode([DisallowNull] ImmutableArray<T> obj)
            {
                var h = new HashCode();
                h.Add(obj.Length);

                for (int i = 0; i < obj.Length; i++)
                    h.Add(obj[i], comparer);

                return h.ToHashCode();
            }
        }

        /// <summary>
        /// Value-type implementation of <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public readonly struct ValueReferenceEqualityComparer<T> : IEqualityComparer<T>
            where T : class
        {

            /// <inheritdoc />
            public bool Equals(T? x, T? y)
            {
                return x == y;
            }

            /// <inheritdoc />
            public int GetHashCode([DisallowNull] T obj)
            {
                return obj.GetHashCode();
            }

        }

        /// <summary>
        /// Returns <c>true</c> if the two given <see cref="ImmutableArray{T}"/> instances are exactly equal, including their contents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool ImmutableArrayReferenceEquals<T>(this ImmutableArray<T> x, ImmutableArray<T> y)
            where T : class
        {
            return ImmutableArrayEquals(x, y, new ValueReferenceEqualityComparer<T>());
        }

        /// <summary>
        /// Returns <c>true</c> if the two given <see cref="ImmutableArray{T}"/> instances are exactly equal, including their contents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool ImmutableArrayEquals<T>(this ImmutableArray<T> x, ImmutableArray<T> y)
        {
            return ImmutableArrayEquals(x, y, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Returns <c>true</c> if the two given <see cref="ImmutableArray{T}"/> instances are exactly equal, including their contents.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool ImmutableArrayEquals<T, TComparer>(this ImmutableArray<T> x, ImmutableArray<T> y, TComparer comparer)
            where TComparer : IEqualityComparer<T>
        {
            return new ImmutableArrayValueComparer<T, TComparer>(comparer).Equals(x, y);
        }

    }

}
