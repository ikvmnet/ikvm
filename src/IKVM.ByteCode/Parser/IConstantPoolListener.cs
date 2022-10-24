namespace IKVM.ByteCode.Parser
{

    public interface IConstantPoolListener
    {

        /// <summary>
        /// Invoked when the 'tag' field is parsed.
        /// </summary>
        /// <param name="value"></param>
        void SetTag(byte value);

    }

}