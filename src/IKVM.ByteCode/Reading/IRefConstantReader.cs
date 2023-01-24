using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Interface supported by all of the readers for Ref constants.
    /// </summary>
    internal interface IRefConstantReader : IConstantReader<RefConstantRecord>
    {

        /// <summary>
        /// Gets the class name of the reference.
        /// </summary>
        ClassConstantReader Class { get; }

        /// <summary>
        /// Gets the name and type reference.
        /// </summary>
        NameAndTypeConstantReader NameAndType { get; }

    }

}