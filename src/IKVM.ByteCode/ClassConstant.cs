namespace IKVM.ByteCode
{

    public sealed class ClassConstant : Constant<ClassConstantRecord>
    {

        Utf8Constant name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="clazz"></param>
        /// <param name="record"></param>
        public ClassConstant(Class clazz, ClassConstantRecord record) :
            base(clazz, record)
        {

        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public Utf8Constant Name => name ??= DeclaringClass.ResolveConstant<Utf8Constant>(Record.NameIndex);

    }

}