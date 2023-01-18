using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Interface supported by all of the constant readers.
    /// </summary>
    internal interface IConstantReader
    {

        /// <summary>
        /// Returns <c>true</c> if this constant is loadable.
        /// </summary>
        bool IsLoadable { get; }

    }

    /// <summary>
    /// Interface supported by all of the constant readers.
    /// </summary>
    internal interface IConstantReader<out TRecord> : IConstantReader
        where TRecord : ConstantRecord
    {

        /// <summary>
        /// Gets the record.
        /// </summary>
        TRecord Record { get; }

    }

    /// <summary>
    /// Interface supported by all of the constant readers.
    /// </summary>
    internal interface IConstantReader<out TRecord, out TOverride> : IConstantReader<TRecord>
        where TRecord : ConstantRecord
        where TOverride : ConstantOverride
    {

        /// <summary>
        /// Gets the override applied to the constant.
        /// </summary>
        TOverride Override { get; }

    }

}