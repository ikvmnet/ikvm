using System;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class InnerClassesAttributeItemReader
    {

        readonly ClassReader declaringClass;
        readonly InnerClassesAttributeItemRecord record;

        string innerClassName;
        string outerClassName;
        string innerName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public InnerClassesAttributeItemReader(ClassReader declaringClass, InnerClassesAttributeItemRecord record)
        {
            this.declaringClass = declaringClass ?? throw new ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the class which declared this reader.
        /// </summary>
        public ClassReader DeclaringClass => declaringClass;

        /// <summary>
        /// Gets the underlying record being read.
        /// </summary>
        public InnerClassesAttributeItemRecord Record => record;

        /// <summary>
        /// Gets the name of the inner class.
        /// </summary>
        public string InnerClassName => ClassReader.LazyGet(ref innerClassName, () => DeclaringClass.ResolveConstant<ClassConstantReader>(record.InnerClassInfoIndex).Name);

        /// <summary>
        /// Gets the name of the outer class.
        /// </summary>
        public string OuterClassName => ClassReader.LazyGet(ref outerClassName, () => DeclaringClass.ResolveConstant<ClassConstantReader>(record.OuterClassInfoIndex).Name);

        /// <summary>
        /// Gets the inner name.
        /// </summary>
        public string InnerName => ClassReader.LazyGet(ref innerName, () => DeclaringClass.ResolveConstant<Utf8ConstantReader>(record.InnerNameIndex).Value);

        /// <summary>
        /// Gets the access flags of the inner class.
        /// </summary>
        public AccessFlag InnerClassAccessFlags => record.InnerClassAccessFlags;

    }

}