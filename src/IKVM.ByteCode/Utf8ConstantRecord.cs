using System;

using IKVM.ByteCode.Text;

namespace IKVM.ByteCode
{

    public class Utf8Constant : ConstantRecord
    {

        readonly ReadOnlyMemory<byte> buffer;
        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Utf8Constant(ReadOnlyMemory<byte> buffer)
        {
            this.buffer = buffer;
        }

        /// <summary>
        /// Gets the raw buffer underlying the UTF8 value.
        /// </summary>
        public ReadOnlyMemory<byte> Buffer => buffer;

        /// <summary>
        /// Gets the string version of the UTF8 value.
        /// </summary>
        public string Value => value ??= MUTF8Encoding.MUTF8.GetString(buffer.Span);

    }

}
