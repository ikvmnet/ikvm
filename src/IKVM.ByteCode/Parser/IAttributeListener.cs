using System;

namespace IKVM.ByteCode.Parser
{

    public interface IAttributeListener
    {

        /// <summary>
        /// Invoked when the 'attribute_name_index' field is parsed.
        /// </summary>
        /// <param name="index"></param>
        void SetName(ConstantPoolIndex index);

        /// <summary>
        /// Invoked when the 'attribute_length' field is parsed.
        /// </summary>
        /// <param name="index"></param>
        void SetLength(int value);

        /// <summary>
        /// Invoked when the 'info' field is parsed.
        /// </summary>
        /// <param name="index"></param>
        void SetInfo(ReadOnlySpan<byte> data);

    }

}