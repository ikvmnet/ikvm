using IKVM.ByteCode.Parsing;
using IKVM.ByteCode.Text;

namespace IKVM.ByteCode.Reading
{

    public sealed class Utf8ConstantReader : Constant<Utf8ConstantRecord>
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public Utf8ConstantReader(ClassReader declaringClass, Utf8ConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public string Value => value ??= string.Intern(MUTF8Encoding.MUTF8.GetString(Record.Value));

    }

}