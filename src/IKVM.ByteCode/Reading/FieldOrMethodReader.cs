namespace IKVM.ByteCode.Reading
{

    internal abstract class FieldOrMethodReader<TRecord> : ReaderBase<TRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        protected FieldOrMethodReader(ClassReader declaringClass, TRecord record) : 
            base(declaringClass, record)
        {

        }

    }

}