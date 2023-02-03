using System;
using System.Text;

namespace IKVM.ByteCode.Text
{

    internal class MUTF8Encoder : Encoder
    {

        readonly MUTF8Encoding encoding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="encoding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MUTF8Encoder(MUTF8Encoding encoding)
        {
            this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public override int GetByteCount(char[] chars, int index, int count, bool flush)
        {
            return encoding.GetByteCount(chars, index, count);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
        {
            return encoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
        }

    }

}
