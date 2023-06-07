using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class InterfaceReader : ReaderBase<InterfaceRecord>
    {

        ClassConstantReader clazz;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        internal InterfaceReader(ClassReader declaringClass, InterfaceRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the name of the interface.
        /// </summary>
        public ClassConstantReader Class => LazyGet(ref clazz, () => DeclaringClass.Constants.Get<ClassConstantReader>(Record.ClassIndex));

    }

}
