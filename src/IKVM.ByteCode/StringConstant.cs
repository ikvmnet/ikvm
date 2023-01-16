namespace IKVM.ByteCode
{

    public class StringConstant : Constant<StringConstantRecord>
    {

        Utf8Constant value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public StringConstant(Class clazz, StringConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the value of the constant.
        /// </summary>
        public Utf8Constant Value => value ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.ValueIndex);

    }

}