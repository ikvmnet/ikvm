#if NETCOREAPP3_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// Provides wrappers for methods from <see cref="Vector128{T}"/> backfilling their implementation on non-supporting Frameworks.
    /// </summary>
    static class Vector128Util
    {

        /// <summary>
        /// Copies a <see cref="Vector128{T}" /> to a given span.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="vector"></param>
        /// <param name="destination"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyTo<T>(this Vector128<T> vector, Span<T> destination) where T : struct
        {
#if NET8_0_OR_GREATER
            Vector128.CopyTo(vector, destination);
#else
            if (destination.Length < Vector128<T>.Count)
                throw new ArgumentException(null, nameof(destination));

            Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(destination)), vector);
#endif
        }

        /// <summary>
        /// Creates a new <see cref="Vector128{T}" /> from a given readonly span.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<T> Create<T>(ReadOnlySpan<T> values) where T : struct
        {
#if NET8_0_OR_GREATER
            return Vector128.Create(values);
#else
            if (values.Length < Vector128<T>.Count)
                throw new ArgumentOutOfRangeException(nameof(values));

            return Unsafe.ReadUnaligned<Vector128<T>>(ref Unsafe.As<T, byte>(ref MemoryMarshal.GetReference(values)));
#endif
        }

        /// <summary>
        /// Loads a vector from the given source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<T> LoadUnsafe<T>(ref readonly T source) where T : struct
        {
#if NET8_0_OR_GREATER
            return Vector128.LoadUnsafe(in source);
#else
            // Use with caution.
            // Supports blittable primitives only.
            ref byte address = ref Unsafe.As<T, byte>(ref Unsafe.AsRef(in source));
            return Unsafe.ReadUnaligned<Vector128<T>>(ref address);
#endif
        }

        /// <summary>
        /// Loads a vector from the given source and element offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="elementOffset"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector128<T> LoadUnsafe<T>(ref readonly T source, nuint elementOffset) where T : struct
        {
#if NET8_0_OR_GREATER
            return Vector128.LoadUnsafe(in source, elementOffset);
#else
            // Use with caution.
            // Supports blittable primitives only.
            ref byte address = ref Unsafe.As<T, byte>(ref Unsafe.Add(ref Unsafe.AsRef(in source), (nint)elementOffset));
            return Unsafe.ReadUnaligned<Vector128<T>>(ref address);
#endif
        }

        /// <summary>
        /// Stores a vector at the given destination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe<T>(this Vector128<T> source, ref T destination) where T : struct
        {
#if NET8_0_OR_GREATER
            Vector128.StoreUnsafe(source, ref destination);
#else
            // Use with caution.
            // Supports blittable primitives only.
            ref byte address = ref Unsafe.As<T, byte>(ref destination);
            Unsafe.WriteUnaligned(ref address, source);
#endif
        }

        /// <summary>
        /// Stores a vector at the given destination.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="elementOffset"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StoreUnsafe<T>(this Vector128<T> source, ref T destination, nuint elementOffset) where T : struct
        {
#if NET8_0_OR_GREATER
            Vector128.StoreUnsafe(source, ref destination, elementOffset);
#else
            // Use with caution.
            // Supports blittable primitives only.
            destination = ref Unsafe.Add(ref destination, (nint)elementOffset);
            Unsafe.WriteUnaligned(ref Unsafe.As<T, byte>(ref destination), source);
#endif
        }

    }

}

#endif
