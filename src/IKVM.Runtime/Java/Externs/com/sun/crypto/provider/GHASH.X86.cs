#if NETCOREAPP3_0_OR_GREATER
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IKVM.Java.Externs.com.sun.crypto.provider.X86;

internal static class GHASH
{
    public static bool IsSupported => Pclmulqdq.IsSupported && Ssse3.IsSupported;

    private static ReadOnlySpan<int> ByteSwapMask => [0x0c0d0e0f, 0x08090a0b, 0x04050607, 0x00010203];

    private static ReadOnlySpan<int> LongSwapMask => [0x0b0a0908, 0x0f0e0d0c, 0x03020100, 0x07060504];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ProcessBlocks(ReadOnlySpan<byte> data, int blocks, Span<long> state, ReadOnlySpan<long> subH)
    {
        // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2748-2883
        Vector128<int>
            xmm_temp0,
            xmm_temp1,
            xmm_temp2,
            xmm_temp3,
            xmm_temp4,
            xmm_temp5,
            xmm_temp6,
            xmm_temp7;

#if NET8_0_OR_GREATER
        xmm_temp0 = Vector128.LoadUnsafe(in MemoryMarshal.GetReference(state)).AsInt32();
        xmm_temp0 = Ssse3.Shuffle(xmm_temp0.AsByte(), Vector128.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte()).AsInt32();
        xmm_temp1 = Vector128.LoadUnsafe(in MemoryMarshal.GetReference(subH)).AsInt32();
        xmm_temp1 = Ssse3.Shuffle(xmm_temp1.AsByte(), Vector128.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte()).AsInt32();
#else
        xmm_temp0 = Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(state)).AsInt32();
        xmm_temp0 = Ssse3.Shuffle(xmm_temp0.AsByte(), Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte()).AsInt32();
        xmm_temp1 = Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(subH)).AsInt32();
        xmm_temp1 = Ssse3.Shuffle(xmm_temp1.AsByte(), Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte()).AsInt32();
#endif

    ghash_loop:
#if NET8_0_OR_GREATER
        xmm_temp2 = Vector128.LoadUnsafe(in MemoryMarshal.GetReference(data)).AsInt32();
        xmm_temp2 = Ssse3.Shuffle(xmm_temp2.AsByte(), Vector128.LoadUnsafe(in MemoryMarshal.GetReference(ByteSwapMask)).AsByte()).AsInt32();
#else
        xmm_temp2 = Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(data)).AsInt32();
        xmm_temp2 = Ssse3.Shuffle(xmm_temp2.AsByte(), Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(ByteSwapMask)).AsByte()).AsInt32();
#endif
        xmm_temp0 = Sse2.Xor(xmm_temp0, xmm_temp2);

        xmm_temp3 = xmm_temp0;
        xmm_temp3 = Pclmulqdq.CarrylessMultiply(xmm_temp3.AsInt64(), xmm_temp1.AsInt64(), 0).AsInt32();
        xmm_temp4 = xmm_temp0;
        xmm_temp4 = Pclmulqdq.CarrylessMultiply(xmm_temp4.AsInt64(), xmm_temp1.AsInt64(), 16).AsInt32();

        xmm_temp5 = xmm_temp0;
        xmm_temp5 = Pclmulqdq.CarrylessMultiply(xmm_temp5.AsInt64(), xmm_temp1.AsInt64(), 1).AsInt32();
        xmm_temp6 = xmm_temp0;
        xmm_temp6 = Pclmulqdq.CarrylessMultiply(xmm_temp6.AsInt64(), xmm_temp1.AsInt64(), 17).AsInt32();

        xmm_temp4 = Sse2.Xor(xmm_temp4, xmm_temp5);

        xmm_temp5 = xmm_temp4;
        xmm_temp4 = Sse2.ShiftRightLogical128BitLane(xmm_temp4, 8);
        xmm_temp5 = Sse2.ShiftLeftLogical128BitLane(xmm_temp5, 8);
        xmm_temp3 = Sse2.Xor(xmm_temp3, xmm_temp5);
        xmm_temp6 = Sse2.Xor(xmm_temp6, xmm_temp4);

        xmm_temp7 = xmm_temp3;
        xmm_temp4 = xmm_temp6;
        xmm_temp3 = Sse2.ShiftLeftLogical(xmm_temp3, 1);
        xmm_temp6 = Sse2.ShiftLeftLogical(xmm_temp6, 1);
        xmm_temp7 = Sse2.ShiftRightLogical(xmm_temp7, 31);
        xmm_temp4 = Sse2.ShiftRightLogical(xmm_temp4, 31);
        xmm_temp5 = xmm_temp7;
        xmm_temp4 = Sse2.ShiftLeftLogical128BitLane(xmm_temp4, 4);
        xmm_temp7 = Sse2.ShiftLeftLogical128BitLane(xmm_temp7, 4);
        xmm_temp5 = Sse2.ShiftRightLogical128BitLane(xmm_temp5, 12);
        xmm_temp3 = Sse2.Or(xmm_temp3, xmm_temp7);
        xmm_temp6 = Sse2.Or(xmm_temp6, xmm_temp4);
        xmm_temp6 = Sse2.Or(xmm_temp6, xmm_temp5);

        xmm_temp7 = xmm_temp3;
        xmm_temp4 = xmm_temp3;
        xmm_temp5 = xmm_temp3;
        xmm_temp7 = Sse2.ShiftLeftLogical(xmm_temp7, 31);
        xmm_temp4 = Sse2.ShiftLeftLogical(xmm_temp4, 30);
        xmm_temp5 = Sse2.ShiftLeftLogical(xmm_temp5, 25);
        xmm_temp7 = Sse2.Xor(xmm_temp7, xmm_temp4);
        xmm_temp7 = Sse2.Xor(xmm_temp7, xmm_temp5);
        xmm_temp4 = xmm_temp7;
        xmm_temp7 = Sse2.ShiftLeftLogical128BitLane(xmm_temp7, 12);
        xmm_temp4 = Sse2.ShiftRightLogical128BitLane(xmm_temp4, 4);
        xmm_temp3 = Sse2.Xor(xmm_temp3, xmm_temp7);

        xmm_temp2 = xmm_temp3;
        xmm_temp7 = xmm_temp3;
        xmm_temp5 = xmm_temp3;
        xmm_temp2 = Sse2.ShiftRightLogical(xmm_temp2, 1);
        xmm_temp7 = Sse2.ShiftRightLogical(xmm_temp7, 2);
        xmm_temp5 = Sse2.ShiftRightLogical(xmm_temp5, 7);
        xmm_temp2 = Sse2.Xor(xmm_temp2, xmm_temp7);
        xmm_temp2 = Sse2.Xor(xmm_temp2, xmm_temp5);
        xmm_temp2 = Sse2.Xor(xmm_temp2, xmm_temp4);
        xmm_temp3 = Sse2.Xor(xmm_temp3, xmm_temp2);
        xmm_temp6 = Sse2.Xor(xmm_temp6, xmm_temp3);
        if ((--blocks) == 0)
        {
            goto exit;
        }

        xmm_temp0 = xmm_temp6;
        data = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.Add(ref Unsafe.AsRef(in MemoryMarshal.GetReference(data)), 16), data.Length - 16);
        goto ghash_loop;

    exit:;
#if NET8_0_OR_GREATER
        Ssse3.Shuffle(xmm_temp6.AsByte(), Vector128.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte())
            .AsInt64().CopyTo(state);
#else
        Ssse3.Shuffle(xmm_temp6.AsByte(), Vector128Polyfill.LoadUnsafe(in MemoryMarshal.GetReference(LongSwapMask)).AsByte())
            .AsInt64().CopyTo(state);
#endif
    }
}
#endif
