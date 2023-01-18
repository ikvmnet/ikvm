using IKVM.ByteCode.Parsing;
using IKVM.ByteCode.Text;

namespace IKVM.ByteCode.Reading
{

    public sealed class Utf8ConstantReader : ConstantReader<Utf8ConstantRecord, Utf8ConstantOverride>
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public Utf8ConstantReader(ClassReader declaringClass, Utf8ConstantRecord record, Utf8ConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public string Value => LazyGet(ref value, () => string.Intern(Override != null && Override.Value is string value ? value : MUTF8Encoding.GetMUTF8(DeclaringClass.MajorVersion).GetString(Record.Value)));

    }

}
