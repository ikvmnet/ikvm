#if NETCOREAPP3_0_OR_GREATER

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace IKVM.Runtime.Util.Com.Sun.Crypto.Provider
{

    /// <summary>
    /// X86 implementations of the CipherBlockChaining functions.
    /// </summary>
    static class CipherBlockChaining_x86
    {

        const int AESBlockSize = 16;

        /// <summary>
        /// Derived from the OpenJDK C code 'stubGenerator_x86_32.cpp:generate_cipherBlockChaining_decryptAESCrypt'.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="key"></param>
        /// <param name="rvec"></param>
        /// <param name="length"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DecryptAESCrypt(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key, Span<byte> rvec, int length)
        {
            const int XMM_REG_NUM_KEY_FIRST = 2;
            const int XMM_REG_NUM_KEY_LAST = 7;
            const nuint FIRST_NON_REG_KEY_offset = 0x70;

            var xmm_key_shuf_mask = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(AESCrypt_x86.KeyShuffleMask)).AsByte();

            Span<Vector128<byte>> xmm_key = stackalloc Vector128<byte>[6];
            ref Vector128<byte> xmm_key_first = ref MemoryMarshal.GetReference(xmm_key);

            for (int rnum = XMM_REG_NUM_KEY_FIRST, offset = 0x10; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_key[as_XMMRegister(rnum)] = AESCrypt_x86.LoadKey(key, (nuint)offset, xmm_key_shuf_mask);
                offset += 0x10;
            }

            ref byte prev_block_cipher_ptr = ref MemoryMarshal.GetReference(rvec);

            if (key.Length != 44)
            {
                goto key_192_256;
            }

            nuint pos = 0;
        singleBlock_loopTop_128:
            if (length == 0)
            {
                goto exit;
            }

            var xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_key_first);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Decrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (var key_offset = FIRST_NON_REG_KEY_offset; key_offset <= 0xa0; key_offset += 0x10)
            {
                xmm_result = AesDecKey(xmm_result, key, key_offset);
            }

            var xmm_temp = AESCrypt_x86.LoadKey(key, 0x00);
            xmm_result = Aes.DecryptLast(xmm_result, xmm_temp);
            xmm_temp = Vector128Util.LoadUnsafe(in prev_block_cipher_ptr);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            prev_block_cipher_ptr = ref Unsafe.Add(ref MemoryMarshal.GetReference(from), pos);
            pos += AESBlockSize;
            length -= AESBlockSize;
            goto singleBlock_loopTop_128;

        exit:;
            Unsafe.CopyBlockUnaligned(ref MemoryMarshal.GetReference(rvec), ref prev_block_cipher_ptr, 16);
            return;

        key_192_256:;
            if (key.Length != 52)
            {
                goto key_256;
            }

            pos = 0;
        singleBlock_loopTop_192:;
            xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_key_first);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Decrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (var key_offset = FIRST_NON_REG_KEY_offset; key_offset <= 0xc0; key_offset += 0x10)
            {
                xmm_result = AesDecKey(xmm_result, key, key_offset);
            }

            xmm_temp = AESCrypt_x86.LoadKey(key, 0x00);
            xmm_result = Aes.DecryptLast(xmm_result, xmm_temp);
            xmm_temp = Vector128Util.LoadUnsafe(in prev_block_cipher_ptr);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            prev_block_cipher_ptr = ref Unsafe.Add(ref MemoryMarshal.GetReference(from), pos); pos += AESBlockSize;
            length -= AESBlockSize;
            if (length != 0)
            {
                goto singleBlock_loopTop_192;
            }

            goto exit;

        key_256:;
            pos = 0;
        singleBlock_loopTop_256:;
            xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_key_first);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Decrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (var key_offset = FIRST_NON_REG_KEY_offset; key_offset <= 0xe0; key_offset += 0x10)
            {
                xmm_result = AesDecKey(xmm_result, key, key_offset);
            }

            xmm_temp = AESCrypt_x86.LoadKey(key, 0x00);
            xmm_result = Aes.DecryptLast(xmm_result, xmm_temp);
            xmm_temp = Vector128Util.LoadUnsafe(in prev_block_cipher_ptr);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            prev_block_cipher_ptr = ref Unsafe.Add(ref MemoryMarshal.GetReference(from), pos); pos += AESBlockSize;
            length -= AESBlockSize;
            if (length != 0)
            {
                goto singleBlock_loopTop_256;
            }

            goto exit;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void EncryptAESCrypt(ReadOnlySpan<byte> from, Span<byte> to, ReadOnlySpan<int> key, Span<byte> rvec, int length)
        {
            // ext\openjdk\hotspot\src\cpu\x86\vm\stubGenerator_x86_32.cpp:2410-2549
            var xmm_key_shuf_mask = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(AESCrypt_x86.KeyShuffleMask)).AsByte();

            const int XMM_REG_NUM_KEY_FIRST = 2;
            const int XMM_REG_NUM_KEY_LAST = 7;

            Span<Vector128<byte>> xmm_key = stackalloc Vector128<byte>[6];
            ref Vector128<byte> xmm_key0 = ref MemoryMarshal.GetReference(xmm_key);
            for (int rnum = XMM_REG_NUM_KEY_FIRST, offset = 0x00; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_key[as_XMMRegister(rnum)] = AESCrypt_x86.LoadKey(key, (nuint)offset, xmm_key_shuf_mask);
                offset += 0x10;
            }

            var xmm_result = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(rvec));

            if (key.Length != 44)
            {
                goto key_192_256;
            }

            nuint pos = 0;
        loopTop_128:;
            var xmm_temp = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result = Sse2.Xor(xmm_result, xmm_key0);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Encrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (int key_offset = 0x60; key_offset <= 0x90; key_offset += 0x10)
            {
                xmm_result = AesEncKey(xmm_result, key, (nuint)key_offset);
            }

            xmm_temp = AESCrypt_x86.LoadKey(key, 0xa0);
            xmm_result = Aes.EncryptLast(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            pos += AESBlockSize;
            length -= AESBlockSize;
            if (length != 0)
            {
                goto loopTop_128;
            }

        exit:
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(rvec));
            return;

        key_192_256:;
            if (key.Length != 52)
            {
                goto key_256;
            }

            pos = 0;
        loopTop_192:;
            xmm_temp = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result = Sse2.Xor(xmm_result, xmm_key0);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Encrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (nuint key_offset = 0x60; key_offset <= 0xb0; key_offset += 0x10)
            {
                xmm_result = AesEncKey(xmm_result, key, key_offset);
            }

            xmm_temp = AESCrypt_x86.LoadKey(key, 0xc0);
            xmm_result = Aes.EncryptLast(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            pos += AESBlockSize;
            length -= AESBlockSize;
            if (length != 0)
            {
                goto loopTop_192;
            }

            goto exit;

        key_256:;
            pos = 0;
        loopTop_256:
            xmm_temp = Vector128Util.LoadUnsafe(in MemoryMarshal.GetReference(from), pos);
            xmm_result = Sse2.Xor(xmm_result, xmm_temp);
            xmm_result = Sse2.Xor(xmm_result, xmm_key0);
            for (int rnum = XMM_REG_NUM_KEY_FIRST + 1; rnum <= XMM_REG_NUM_KEY_LAST; rnum++)
            {
                xmm_result = Aes.Encrypt(xmm_result, xmm_key[as_XMMRegister(rnum)]);
            }

            for (nuint key_offset = 0x60; key_offset <= 0xd0; key_offset += 0x10)
            {
                xmm_result = AesEncKey(xmm_result, key, key_offset);
            }

            xmm_temp = AESCrypt_x86.LoadKey(key, 0xe0);
            xmm_result = Aes.EncryptLast(xmm_result, xmm_temp);
            xmm_result.StoreUnsafe(ref MemoryMarshal.GetReference(to), pos);
            pos += AESBlockSize;
            length -= AESBlockSize;
            if (length != 0)
            {
                goto loopTop_256;
            }

            goto exit;
        }

        static Vector128<byte> AesDecKey(Vector128<byte> xmmdst, ReadOnlySpan<int> key, nuint offset, Vector128<byte>? xmm_shuf_mask = null)
        {
            return Aes.Decrypt(xmmdst, AESCrypt_x86.LoadKey(key, offset, xmm_shuf_mask));
        }

        static Vector128<byte> AesEncKey(Vector128<byte> xmmdst, ReadOnlySpan<int> key, nuint offset, Vector128<byte>? xmm_shuf_mask = null)
        {
            return Aes.Encrypt(xmmdst, AESCrypt_x86.LoadKey(key, offset, xmm_shuf_mask));
        }

        static int as_XMMRegister(int @in) => @in - 2;

    }

}

#endif
