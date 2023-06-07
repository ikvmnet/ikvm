using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public abstract class ElementValueReader : ReaderBase<ElementValueRecord>
    {

        /// <summary>
        /// Resolves the given element value record to a reader type.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        public static ElementValueReader Resolve(ClassReader declaringClass, ElementValueRecord record) => record.Value switch
        {
            ElementValueAnnotationValueRecord => new ElementValueAnnotationReader(declaringClass, record),
            ElementValueArrayValueRecord => new ElementValueArrayReader(declaringClass, record),
            ElementValueClassValueRecord => new ElementValueClassReader(declaringClass, record),
            ElementValueConstantValueRecord => new ElementValueConstantReader(declaringClass, record),
            ElementValueEnumConstantValueRecord => new ElementValueEnumConstantReader(declaringClass, record),
            _ => throw new ByteCodeException("Cannot resolve element value reader."),
        };

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        protected ElementValueReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

    }

    internal abstract class ElementValueReader<TValueRecord> : ElementValueReader
        where TValueRecord : ElementValueValueRecord
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected ElementValueReader(ClassReader declaringClass, ElementValueRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the underlying value record.
        /// </summary>
        public TValueRecord ValueRecord => (TValueRecord)Record.Value;

    }

}
