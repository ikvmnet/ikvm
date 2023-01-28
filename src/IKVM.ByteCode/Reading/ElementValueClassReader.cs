using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueClassReader : ElementValueReader<ElementValueClassValueRecord>
    {

        Utf8ConstantReader clazz;

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
        public Utf8ConstantReader Class => LazyGet(ref clazz, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(ValueRecord.ClassIndex));

    }

}