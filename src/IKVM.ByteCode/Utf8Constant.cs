using IKVM.ByteCode.Text;

namespace IKVM.ByteCode
{

    public sealed class Utf8Constant : Constant<Utf8ConstantRecord>
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public Utf8Constant(Class clazz, Utf8ConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public string Value => value ??= string.Intern(MUTF8Encoding.MUTF8.GetString(Record.Value));

    }

}