using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class DoubleConstantReader : Constant<DoubleConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public DoubleConstantReader(ClassReader declaringClass, DoubleConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public double Value => Record.Value;

    }

}