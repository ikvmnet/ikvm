using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Interface supported by all of the readers for Ref constants.
    /// </summary>
    internal interface IRefConstantReader : IConstantReader
    {

        /// <summary>
        /// Gets the class name of the constant.
        /// </summary>
        string ClassName { get; }

        /// <summary>
        /// Gets the name of the constant.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the constant.
        /// </summary>
        string Type { get; }

    }

    /// <summary>
    /// Interface supported by all of the readers for Ref constants.
    /// </summary>
    internal interface IRefConstantReader<out TRecord, out TPatch> : IRefConstantReader, IConstantReader<TRecord, TPatch>
        where TRecord : ConstantRecord
        where TPatch : ConstantOverride
    {



    }

}