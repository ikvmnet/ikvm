using IKVM.ByteCode.Parsing;
using IKVM.ByteCode.Text;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class Utf8ConstantReader : ConstantReader<Utf8ConstantRecord>
    {

        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        public Utf8ConstantReader(ClassReader declaringClass, ushort index, Utf8ConstantRecord record) :
            base(declaringClass, index, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant. Result is interned.
        /// </summary>
        public string Value => LazyGet(ref value, () => MUTF8Encoding.GetMUTF8(DeclaringClass.Version.Major).GetString(Record.Value));

    }

}
