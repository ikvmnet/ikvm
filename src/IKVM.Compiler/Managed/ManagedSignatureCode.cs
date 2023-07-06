using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an element in a managed signature sequence.
    /// </summary>
    readonly struct ManagedSignatureCode
    {

        /// <summary>
        /// Common data elements for each kind.
        /// </summary>
        public readonly ManagedSignatureCodeData Data;

        /// <summary>
        /// Reverse offset within the hosting sequence from this code to the code of the first fixed argument.
        /// </summary>
        public readonly int Arg0;

        /// <summary>
        /// Sequence of reverse offsets within the hosting sequence from this code to each code element within the variable argument list.
        /// </summary>
        public readonly ReadOnlyFixedValueList<int> Argv;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="arg0"></param>
        /// <param name="argv"></param>
        public ManagedSignatureCode(in ManagedSignatureCodeData data, int arg0, in ReadOnlyFixedValueList<int> argv)
        {
            Data = data;
            Arg0 = arg0;
            Argv = argv;
        }

    }

}
