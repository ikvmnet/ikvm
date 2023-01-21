using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueClassReader : ElementValueReader<ElementValueClassValueRecord>
    {

        string clazz;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueClassReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the class included with this element value.
        /// </summary>
        public string Class => LazyGet(ref clazz, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(ValueRecord.ClassIndex).Value);

    }

}