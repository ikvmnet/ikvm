using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class StringConstantReader : Constant<StringConstantRecord>
    {

        Utf8ConstantReader value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public StringConstantReader(ClassReader declaringClass, StringConstantRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public Utf8ConstantReader Value => value ??= DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.ValueIndex);

    }

}