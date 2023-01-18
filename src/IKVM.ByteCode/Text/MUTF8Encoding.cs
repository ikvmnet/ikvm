using System;
using System.Text;

namespace IKVM.ByteCode.Text
{

    /// <summary>
    /// Implements an <see cref="Encoding"/> for Sun's modified UTF-8.
    /// </summary>
    public class MUTF8Encoding : Encoding
    {

        readonly static MUTF8Encoding JavaSE_1_0 = new MUTF8Encoding(0);
        readonly static MUTF8Encoding JavaSE_1_4 = new MUTF8Encoding(48);

        /// <summary>
        /// Gets an instance of the Sun modified UTF8 encoding targeting the specified JavaSE version.
        /// </summary>
        public static MUTF8Encoding GetMUTF8(int majorVersion) => majorVersion >= 48 ? JavaSE_1_4 : JavaSE_1_0;

        readonly int majorVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="majorVersion"></param>
        public MUTF8Encoding(int majorVersion = 52)
        {
            this.majorVersion = majorVersion;
        }

        /// <summary>
        /// Scans for the position of the first NULL given the specified pointer, up to a maximum offset of <paramref name="max"/>.
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public unsafe int IndexOfNull(byte* ptr, int max = int.MaxValue)
        {
            if (ptr is null)
                throw new ArgumentNullException(nameof(ptr));
            if (max < 0)
                throw new ArgumentOutOfRangeException(nameof(max));

            for (int i = 0; i < max; i++)
                if (ptr[i] == 0)
                    return i;

            return -1;
        }

        /// <inheritdoc />
        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            return GetByteCount(chars.AsSpan(index, count));
        }

#if NETFRAMEWORK

        public unsafe int GetByteCount(ReadOnlySpan<char> chars)
        {
            if (chars.Length == 0)
                return 0;

            fixed (char* c = chars)
                return GetByteCount(c, chars.Length);
        }

#endif

        /// <inheritdoc />
        public override unsafe int GetByteCount(char* chars, int count)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var len = 0;

            for (int i = 0, e = count; i < e; i++)
            {
                var c = chars[i];
                if (c > 0 && c <= 0x007F)
                    len += 1;
                else if (c <= 0x07FF)
                    len += 2;
                else
                    len += 3;
            }

            return len;
        }

        /// <inheritdoc />
        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (s is null)
                throw new ArgumentNullException(nameof(s));
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex));

            return GetBytes(s.AsSpan(charIndex, charCount), bytes.AsSpan(byteIndex));
        }

        /// <inheritdoc />
        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (bytes is null)
                throw new ArgumentNullException(nameof(chars));
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex));

            return GetBytes(chars.AsSpan(charIndex, charCount), bytes.AsSpan(byteIndex));
        }

#if NETFRAMEWORK

        public unsafe int GetBytes(ReadOnlySpan<char> chars, Span<byte> bytes)
        {
            if (chars.Length == 0)
                return 0;

            fixed (char* cptr = chars)
            fixed (byte* bptr = bytes)
                return GetBytes(cptr, chars.Length, bptr, bytes.Length);
        }

#endif

        /// <inheritdoc />
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
        {
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));

            int j = 0;

            for (int i = 0; i < charCount; i++)
            {
                var ch = chars[i];
                if ((ch != 0) && (ch <= 0x7F))
                {
                    if (j + 1 > byteCount)
                        throw new EncoderFallbackException("Out of memory.");

                    bytes[j++] = (byte)ch;
                }
                else if (ch <= 0x7FF)
                {
                    if (j + 2 > byteCount)
                        throw new EncoderFallbackException("Out of memory.");

                    bytes[j++] = (byte)((ch >> 6) | 0xC0);
                    bytes[j++] = (byte)((ch & 0x3F) | 0x80);
                }
                else
                {
                    if (j + 3 > byteCount)
                        throw new EncoderFallbackException("Out of memory.");

                    bytes[j++] = (byte)((ch >> 12) | 0xE0);
                    bytes[j++] = (byte)(((ch >> 6) & 0x3F) | 0x80);
                    bytes[j++] = (byte)((ch & 0x3F) | 0x80);
                }
            }

            return j;
        }

#if NETFRAMEWORK

        public unsafe int GetCharCount(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return 0;

            fixed (byte* ptr = bytes)
                return GetCharCount(ptr, bytes.Length);
        }

#endif

        /// <inheritdoc />
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (index + count > bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(count));

            return GetCharCount(bytes.AsSpan(index, count));
        }

        /// <inheritdoc />
        public override unsafe int GetCharCount(byte* bytes, int count)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int j = 0; j < count; j++)
            {
                if (bytes[j] == 0)
                    throw new DecoderFallbackException("Illegal value in modified UTF8.");

                if (bytes[j] >= 128)
                {
                    int l = 0;
                    for (int i = 0; i < count; i++)
                    {
                        uint c = bytes[i];
                        uint char2, char3;

                        if (c == 0)
                            throw new DecoderFallbackException("Illegal value in modified UTF8.");

                        switch (c >> 4)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                // 0xxxxxxx
                                break;
                            case 12:
                            case 13:
                                // 110x xxxx   10xx xxxx
                                char2 = bytes[++i];
                                if ((char2 & 0xc0) != 0x80 || i >= count)
                                    goto default;

                                c = ((c & 0x1F) << 6) | (char2 & 0x3F);
                                if (c < 0x80 && c != 0 && majorVersion >= 48)
                                    goto default;
                                break;
                            case 14:
                                // 1110 xxxx  10xx xxxx  10xx xxxx
                                char2 = bytes[++i];
                                char3 = bytes[++i];
                                if ((char2 & 0xc0) != 0x80 || (char3 & 0xc0) != 0x80 || i >= count)
                                    goto default;
                                c = ((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0);
                                if (c < 0x800 && majorVersion >= 48)
                                    goto default;
                                break;
                            default:
                                throw new DecoderFallbackException("Illegal value in modified UTF8.");
                        }

                        l++;
                    }

                    return l;
                }
            }

            // fallback to ASCII (char count == byte count)
            return count;
        }

        /// <inheritdoc />
        public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return GetChars(bytes.AsSpan(byteIndex, byteCount), chars.AsSpan(charIndex));
        }

#if NETFRAMEWORK

        /// <inheritdoc />
        public unsafe int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
        {
            // avoid problems with empty buffer
            if (bytes.Length == 0)
                return 0;

            fixed (byte* bptr = bytes)
            fixed (char* cptr = chars)
                return GetChars(bptr, bytes.Length, cptr, chars.Length);
        }

#endif

        /// <inheritdoc />
        public override unsafe int GetChars(byte* bytes, int byteCount, char* chars, int charCount)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));

            for (int j = 0; j < byteCount; j++)
            {
                if (bytes[j] == 0)
                    throw new DecoderFallbackException("Illegal value in modified UTF8.");

                if (bytes[j] >= 128)
                {
                    int l = 0;
                    for (int i = 0; i < byteCount; i++)
                    {
                        uint c = bytes[i];
                        uint char2, char3;

                        if (c == 0)
                            throw new DecoderFallbackException("Illegal value in modified UTF8.");

                        switch (c >> 4)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            case 6:
                            case 7:
                                // 0xxxxxxx
                                break;
                            case 12:
                            case 13:
                                // 110x xxxx   10xx xxxx
                                char2 = bytes[++i];
                                if ((char2 & 0xc0) != 0x80 || i >= byteCount)
                                    goto default;

                                c = ((c & 0x1F) << 6) | (char2 & 0x3F);
                                if (c < 0x80 && c != 0 && majorVersion >= 48)
                                    goto default;
                                break;
                            case 14:
                                // 1110 xxxx  10xx xxxx  10xx xxxx
                                char2 = bytes[++i];
                                char3 = bytes[++i];
                                if ((char2 & 0xc0) != 0x80 || (char3 & 0xc0) != 0x80 || i >= byteCount)
                                    goto default;
                                c = ((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | ((char3 & 0x3F) << 0);
                                if (c < 0x800 && majorVersion >= 48)
                                    goto default;
                                break;
                            default:
                                throw new DecoderFallbackException("Illegal value in modified UTF8.");
                        }

                        chars[l++] = (char)c;
                    }

                    return l;
                }
            }

            // fallback to ASCII
            return ASCII.GetChars(bytes, byteCount, chars, charCount);
        }

        /// <inheritdoc />
        public override int GetMaxByteCount(int charCount)
        {
            return charCount * 3;
        }

        /// <inheritdoc />
        public override int GetMaxCharCount(int byteCount)
        {
            return byteCount;
        }

        /// <inheritdoc />
        public override Decoder GetDecoder()
        {
            return new MUTF8Decoder(this);
        }

        /// <inheritdoc />
        public override Encoder GetEncoder()
        {
            return new MUTF8Encoder(this);
        }

    }

}
