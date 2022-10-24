using System;

namespace IKVM.ByteCode.Parser
{

    /// <summary>
    /// Provides the capability to parse a Java class file constant pool structure.
    /// </summary>
    public static class ConstantPoolInfoParser
    {

        /// <summary>
        /// Parses the 'cp_info' structure and returns the total length.
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static int Parse(IConstantPoolListener visitor, ReadOnlySpan<byte> buffer)
        {
            return 0;
        }

    }

}