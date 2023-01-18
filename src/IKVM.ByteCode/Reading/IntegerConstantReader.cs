using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class IntegerConstantReader : ConstantReader<IntegerConstantRecord, IntegerConstantOverride>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public IntegerConstantReader(ClassReader declaringClass, IntegerConstantRecord record, IntegerConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public int Value => Override != null ? Override.Value : Record.Value;

        public override bool IsLoadable => DeclaringClass.MajorVersion == 45 && DeclaringClass.MinorVersion >= 3 || DeclaringClass.MajorVersion > 45;

    }

}