using System;
using System.Text;

namespace IKVM.ByteCode.Text
{

    /// <summary>
    /// Implements an <see cref="Encoding"/> for Sun's modified UTF-8.
    /// </summary>
    internal class MUTF8Encoding : Encoding
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

            int n = count;
            for (int i = 0; i < count; i++)
                if ((bytes[i] & 0xC0) == 0x80)
                    --n;

            return n;
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

        /// <summary>
        /// Copied from UTF8.cpp in OpenJDK.
        /// 
        /// Takes a pointer to a character of a given modified UTF8 string and decodes the character into the pointer
        /// given by value. Returns a pointer to the next character.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static unsafe byte* Next(byte* str, char* value)
        {
            var ptr = str;
            int length = -1; // bad length
            int result = 0;

            byte ch, ch2, ch3;
            switch ((ch = ptr[0]) >> 4)
            {
                default:
                    result = ch;
                    length = 1;
                    break;
                case 0x8:
                case 0x9:
                case 0xA:
                case 0xB:
                case 0xF:
                    // shouldn't happen
                    break;
                case 0xC:
                case 0xD:
                    /* 110xxxxx  10xxxxxx */
                    if (((ch2 = ptr[1]) & 0xC0) == 0x80)
                    {
                        var high_five = ch & 0x1F;
                        var low_six = ch2 & 0x3F;
                        result = (high_five << 6) + low_six;
                        length = 2;
                        break;
                    }
                    break;
                case 0xE:
                    /* 1110xxxx 10xxxxxx 10xxxxxx */
                    if (((ch2 = ptr[1]) & 0xC0) == 0x80)
                    {
                        if (((ch3 = ptr[2]) & 0xC0) == 0x80)
                        {
                            var high_four = ch & 0x0f;
                            var mid_six = ch2 & 0x3f;
                            var low_six = ch3 & 0x3f;
                            result = (((high_four << 6) + mid_six) << 6) + low_six;
                            length = 3;
                        }
                    }
                    break;
            }

            if (length <= 0)
            {
                *value = (char)ptr[0]; // default bad result
                return ptr + 1; // make progress somehow
            }

            *value = (char)result;

            // The assert is correct but the .class file is wrong
            // assert(UNICODE::utf8_size(result) == length, "checking reverse computation");
            return ptr + length;
        }

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

            var ptr = bytes;
            int index = 0;

            // ASCII case loop optimization
            for (; index < charCount; index++)
            {
                byte ch;
                if ((ch = ptr[0]) > 0x7F) break;
                chars[index] = (char)ch;
                ptr++;
            }

            // up until max char count is reached, advance over next character
            for (; index < charCount; index++)
                ptr = Next(ptr, &chars[index]);

            return index;
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
