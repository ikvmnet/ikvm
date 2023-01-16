using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class FloatConstantReader : Constant<FloatConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public FloatConstantReader(ClassReader declaringClass, FloatConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public float Value => Record.Value;

    }

}