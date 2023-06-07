namespace IKVM.ByteCode.Reading
{

    public abstract class FieldOrMethodReader<TRecord> : ReaderBase<TRecord>
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

        /// <summary>
        /// Gets the access flags of this field or method.
        /// </summary>
        public abstract AccessFlag AccessFlags { get; }

        /// <summary>
        /// Gets the name of this field or method.
        /// </summary>
        public abstract Utf8ConstantReader Name { get; }

        /// <summary>
        /// Gets the descriptor of this field or method.
        /// </summary>
        public abstract Utf8ConstantReader Descriptor { get; }

        /// <summary>
        /// Gets the attributes of this field or method.
        /// </summary>
        public abstract AttributeReaderCollection Attributes { get; }

    }

}