using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class IntegerConstantReader : ConstantReader<IntegerConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public IntegerConstantReader(ClassReader declaringClass, ushort index, IntegerConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public int Value => Record.Value;

        /// <inheritdoc />
        public override bool IsLoadable => DeclaringClass.Version >= new ClassFormatVersion(45, 3);

    }

}