using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class InterfaceReader : ReaderBase<InterfaceRecord>
    {

        string name;

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
        public string Name => LazyGet(ref name, () => DeclaringClass.ResolveConstant<ClassConstantReader>(Record.ClassIndex).Name);

    }

}
