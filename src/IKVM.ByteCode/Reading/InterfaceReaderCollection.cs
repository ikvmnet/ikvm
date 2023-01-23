using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of method data.
    /// </summary>
    internal sealed class InterfaceReaderCollection : LazyReaderList<InterfaceReader, InterfaceRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal InterfaceReaderCollection(ClassReader declaringClass, InterfaceRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new interface reader.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override InterfaceReader CreateReader(int index, InterfaceRecord record)
        {
            return new InterfaceReader(DeclaringClass, record);
        }

        /// <summary>
        /// Returns <c>true</c> if an interface with the specified name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => Enumerable.Range(0, Count).Any(i => this[i].Class.Name.Value == name);

    }

}
