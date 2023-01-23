using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementValueEnumConstantReader : ElementValueReader<ElementValueEnumConstantValueRecord>
    {

        Utf8ConstantReader typeName;
        Utf8ConstantReader constantName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementValueEnumConstantReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        public Utf8ConstantReader TypeName => LazyGet(ref typeName, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(ValueRecord.TypeNameIndex));

        public Utf8ConstantReader ConstantName => LazyGet(ref constantName, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(ValueRecord.ConstantNameIndex));

    }

}