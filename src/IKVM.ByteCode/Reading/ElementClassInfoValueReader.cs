using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ElementClassInfoValueReader : ElementValueReader<ElementClassInfoValueRecord>
    {

        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public ElementClassInfoValueReader(ClassReader declaringClass, ElementClassInfoValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the annotation included with this element value.
        /// </summary>
        public string Type => LazyGet(ref type, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.ClassInfoIndex).Value);

    }

}