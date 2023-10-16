using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class DoubleConstantReader : ConstantReader<DoubleConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public DoubleConstantReader(ClassReader declaringClass, ushort index, DoubleConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public double Value => Record.Value;

        /// <summary>
        /// Returns whether or not this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.Version >= new ClassFormatVersion(45, 3);

    }

}