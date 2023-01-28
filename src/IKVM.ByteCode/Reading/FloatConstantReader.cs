using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class FloatConstantReader : ConstantReader<FloatConstantRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public FloatConstantReader(ClassReader declaringClass, ushort index, FloatConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public float Value => Record.Value;

        /// <summary>
        /// Returns <c>true</c> if this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.Version >= new ClassFormatVersion(45, 3);

    }

}