using System;
using System.Text;

namespace IKVM.ByteCode.Text
{

    internal class MUTF8Decoder : Decoder
    {

        readonly MUTF8Encoding encoding;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="encoding"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MUTF8Decoder(MUTF8Encoding encoding)
        {
            this.encoding = encoding ?? throw new ArgumentNullException(nameof(encoding));
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            return encoding.GetCharCount(bytes, index, count);
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            return encoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
        }

    }

}
