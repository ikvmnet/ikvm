using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class LongConstantReader : Constant<LongConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public LongConstantReader(ClassReader declaringClass, LongConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public long Value => Record.Value;

    }

}