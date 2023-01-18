using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class LongConstantReader : ConstantReader<LongConstantRecord, LongConstantOverride>
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public LongConstantReader(ClassReader declaringClass, LongConstantRecord record, LongConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public long Value => Override != null ? Override.Value : Record.Value;

        /// <summary>
        /// Returns <c>true</c> if this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion == 45 && DeclaringClass.MinorVersion >= 3 || DeclaringClass.MajorVersion > 45;

    }

}