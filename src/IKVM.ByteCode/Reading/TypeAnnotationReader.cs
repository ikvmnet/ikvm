using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class TypeAnnotationReader : ReaderBase<TypeAnnotationRecord>
    {

        Utf8ConstantReader type;
        ElementValueKeyReaderCollection elements;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        public TypeAnnotationReader(ClassReader declaringClass, TypeAnnotationRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the type of the annotation.
        /// </summary>
        public Utf8ConstantReader Type => LazyGet(ref type, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.TypeIndex));

        /// <summary>
        /// Gets the element values of the annotation.
        /// </summary>
        public ElementValueKeyReaderCollection Elements => LazyGet(ref elements, () => new ElementValueKeyReaderCollection(DeclaringClass, Record.Elements));

    }

}
