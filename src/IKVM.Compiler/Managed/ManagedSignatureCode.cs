using System;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an element in a managed signature sequence.
    /// </summary>
    internal readonly struct ManagedSignatureCode
    {

        /// <summary>
        /// Common data elements for each kind.
        /// </summary>
        public readonly ManagedSignatureCodeData Data;

        /// <summary>
        /// Reverse offset within the hosting sequence from this code to the code of the first fixed argument.
        /// </summary>
        public readonly short Arg0;

        /// <summary>
        /// Reverse offset within the hosting sequence from this code to the code of the second fixed argument.
        /// </summary>
        public readonly short Arg1;

        /// <summary>
        /// Sequence of reverse offsets within the hosting sequence from this code to each code element within the variable argument list.
        /// </summary>
        public readonly FixedValueList2<short> Argv;

        /// <summary>
        /// Populates a code instance with the specified data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv"></param>
        public ManagedSignatureCode(in ManagedSignatureCodeData data, short arg0, short arg1, in FixedValueList2<short> argv)
        {
            Data = data;
            Arg0 = arg0;
            Arg1 = arg1;
            Argv = argv;
        }

    }

}
