namespace IKVM.ByteCode.Parsing
{

    internal sealed record MethodTypeConstantRecord(ushort DescriptorIndex) : ConstantRecord
    {

        /// <summary>
        /// Parses a MethodType constant in the constant pool.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="constant"></param>
        /// <param name="skip"></param>
        public static bool TryReadMethodTypeConstant(ref ClassFormatReader reader, out ConstantRecord constant, out int skip)
        {
            constant = null;
            skip = 0;

            if (reader.TryReadU2(out ushort descriptorIndex) == false)
                return false;

            constant = new MethodTypeConstantRecord(descriptorIndex);
            return true;
        }

    }

}
