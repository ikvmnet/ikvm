using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class DoubleConstantReader : ConstantReader<DoubleConstantRecord, DoubleConstantOverride>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public DoubleConstantReader(ClassReader declaringClass, DoubleConstantRecord record, DoubleConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public double Value => Override != null ? Override.Value : Record.Value;

        /// <summary>
        /// Returns whether or not this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion == 45 && DeclaringClass.MinorVersion >= 3 || DeclaringClass.MajorVersion > 45;

    }

}