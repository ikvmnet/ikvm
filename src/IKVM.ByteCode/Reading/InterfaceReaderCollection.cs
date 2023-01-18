using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of method data.
    /// </summary>
    public sealed class InterfaceReaderCollection : LazyReaderList<InterfaceReader, InterfaceRecord>
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
        /// Gets the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public InterfaceReader this[string name] => Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the names of the interfaces.
        /// </summary>
        public IEnumerable<string> Names => Enumerable.Range(0, Count).Select(i => this[i]).Select(i => i.Name);

        /// <summary>
        /// Returns <c>true</c> if an interface with the specified name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => Enumerable.Range(0, Count).Any(i => this[i].Name == name);

        /// <summary>
        /// Attempts to get the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out InterfaceReader value)
        {
            value = Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault();
            return value != null;
        }

    }

}
