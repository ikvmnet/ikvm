using System;
using System.Text;

namespace IKVM.Runtime.Text
{

    /// <summary>
    /// Implements a <see cref="Encoding"/> for Sun's modified UTF-8.
    /// </summary>
    internal class MUTF8Encoding : Encoding
    {

        static MUTF8Encoding mutf8;

        /// <summary>
        /// Gets an instance of the Sun modified UTF8 encoding.
        /// </summary>
        public static MUTF8Encoding MUTF8 => mutf8 ??= new MUTF8Encoding();

        /// <summary>
        /// Scans for the position of the first NULL given the specified pointer, up to a maximum offset of <paramref name="max"/>.
        /// </summary>
        /// <param name="ptr"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static unsafe int IndexOfNull(byte* ptr, int max = int.MaxValue)
        {
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
            fixed (char* c = chars)
                return GetByteCount(c, chars.Length);
        }

#endif

        /// <inheritdoc />
        public override unsafe int GetByteCount(char* chars, int count)
        {
            int len = 0;

            for (int i = 0; i < count; i++)
            {
                var ch = chars[i];
                if ((ch != 0) && (ch <= 0x7F))
                    len++;
                else if (ch <= 0x7FF)
                    len += 2;
                else
                    len += 3;
            }

            return len;
        }

        public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
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
            fixed (char* cptr = chars)
            fixed (byte* bptr = bytes)
                return GetBytes(cptr, chars.Length, bptr, bytes.Length);
        }

#endif

        /// <inheritdoc />
        public override unsafe int GetBytes(char* chars, int charCount, byte* bytes, int byteCount)
        {
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));

            int j = 0;

            for (int i = 0; i < charCount; i++)
            {
                var ch = chars[i];
                if ((ch != 0) && (ch <= 0x7F))
                {
                    if (j + 1 > byteCount)
                        throw new ArgumentException();

                    bytes[j++] = (byte)ch;
                }
                else if (ch <= 0x7FF)
                {
                    if (j + 2 > byteCount)
                        throw new ArgumentException();

                    bytes[j++] = (byte)((ch >> 6) | 0xC0);
                    bytes[j++] = (byte)((ch & 0x3F) | 0x80);
                }
                else
                {
                    if (j + 3 > byteCount)
                        throw new ArgumentException();

                    bytes[j++] = (byte)((ch >> 12) | 0xE0);
                    bytes[j++] = (byte)(((ch >> 6) & 0x3F) | 0x80);
                    bytes[j++] = (byte)((ch & 0x3F) | 0x80);
                }
            }

            return j;
        }

        /// <inheritdoc />
        public override unsafe int GetCharCount(byte* bytes, int count)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            var hasNonAscii = false;
            for (int i = 0; i < count; i++)
            {
                if (bytes[i] >= 128)
                {
                    hasNonAscii = true;
                    break;
                }
            }

            // no non-ascii detected, we can just parse as ASCII, where byte count is char count
            if (hasNonAscii == false)
                return count;

            int l = 0;

            for (int i = 0; i < count; i++)
            {
                int c = *bytes++;
                switch (c >> 4)
                {
                    case 12:
                    case 13:
                        (*bytes)++;
                        i++;
                        break;
                    case 14:
                        (*bytes)++;
                        (*bytes)++;
                        i++;
                        i++;
                        break;
                }

                l++;
            }

            return l;
        }

#if NETFRAMEWORK

        public unsafe int GetCharCount(ReadOnlySpan<byte> bytes)
        {
            fixed (byte* ptr = bytes)
                return GetCharCount(ptr, bytes.Length);
        }

#endif

        /// <inheritdoc />
        public unsafe override int GetCharCount(byte[] bytes, int index, int count)
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
        public unsafe override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return GetChars(bytes.AsSpan(byteIndex, byteCount), chars.AsSpan(charIndex));
        }

#if NETFRAMEWORK

        public unsafe int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
        {
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

            var hasNonAscii = false;
            for (int i = 0; i < byteCount; i++)
            {
                if (bytes[i] >= 128)
                {
                    hasNonAscii = true;
                    break;
                }
            }

            // no non-ascii detected, we can just parse as ASCII
            if (hasNonAscii == false)
                return ASCII.GetChars(bytes, byteCount, chars, charCount);

            // current output count
            int o = 0;

            for (int i = 0; i < byteCount && o < charCount; i++)
            {
                int c = *bytes++;
                int char2, char3;
                switch (c >> 4)
                {
                    case 12:
                    case 13:
                        char2 = *bytes++;
                        i++;
                        c = ((c & 0x1F) << 6) | (char2 & 0x3F);
                        break;
                    case 14:
                        char2 = *bytes++;
                        char3 = *bytes++;
                        i++;
                        i++;
                        c = ((c & 0x0F) << 12) | ((char2 & 0x3F) << 6) | (char3 & 0x3F);
                        break;
                }

                chars[o++] = (char)c;
            }

            return o;
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
