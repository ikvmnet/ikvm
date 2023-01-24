using System.Runtime.CompilerServices;

#if NETCOREAPP3_1
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
#endif

namespace IKVM.ByteCode.Buffers
{

    static class RawBitConverter
    {

#if NETFRAMEWORK || NETCOREAPP3_1

        /// <summary>
        /// Converts the specified 32-bit signed integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float Int32BitsToSingle(int value)
        {
#if NETCOREAPP3_1
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            if (Sse2.IsSupported)
            {
                Vector128<float> vec = Vector128.CreateScalarUnsafe(value).AsSingle();
                return vec.ToScalar();
            }
#endif

            return *((float*)&value);
        }

        /// <summary>
        /// Converts the specified 32-bit signed integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double Int64BitsToDouble(long value)
        {
#if NETCOREAPP3_1
            // Workaround for https://github.com/dotnet/runtime/issues/11413
            if (Sse2.X64.IsSupported)
            {
                Vector128<double> vec = Vector128.CreateScalarUnsafe(value).AsDouble();
                return vec.ToScalar();
            }
#endif

            return *((double*)&value);
        }

#endif

#if NETFRAMEWORK || NETCOREAPP3_1

        /// <summary>
        /// Converts the specified 32-bit unsigned integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float UInt32BitsToSingle(uint value) => Int32BitsToSingle((int)value);

        /// <summary>
        /// Converts the specified 32-bit unsigned integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double UInt64BitsToDouble(ulong value) => Int64BitsToDouble((long)value);

#endif

    }

}
