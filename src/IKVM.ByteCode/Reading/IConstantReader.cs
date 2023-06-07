using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Interface supported by all of the constant readers.
    /// </summary>
    public interface IConstantReader
    {

        /// <summary>
        /// Gets the index of this constant.
        /// </summary>
        ushort Index { get; }

        /// <summary>
        /// Returns <c>true</c> if this constant is loadable.
        /// </summary>
        bool IsLoadable { get; }

    }

    /// <summary>
    /// Interface supported by all of the constant readers.
    /// </summary>
    public interface IConstantReader<out TRecord> : IConstantReader
        where TRecord : ConstantRecord
    {

        /// <summary>
        /// Gets the record.
        /// </summary>
        TRecord Record { get; }

    }

}
