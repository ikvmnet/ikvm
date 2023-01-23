using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of methods.
    /// </summary>
    internal sealed class MethodReaderCollection : LazyReaderList<MethodReader, MethodRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal MethodReaderCollection(ClassReader declaringClass, MethodRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new method reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override MethodReader CreateReader(int index, MethodRecord record)
        {
            return new MethodReader(DeclaringClass, record);
        }

        /// <summary>
        /// Gets the method with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MethodReader this[string name] => this.FirstOrDefault(i => i.Name.Value == name) ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the names of the methods.
        /// </summary>
        public IEnumerable<string> Names => this.Select(i => i.Name.Value);

        /// <summary>
        /// Returns <c>true</c> if an method with the specified name exists.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Contains(string name) => this.Any(i => i.Name.Value == name);

        /// <summary>
        /// Attempts to get the method with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out MethodReader value)
        {
            value = this.FirstOrDefault(i => i.Name.Value == name);
            return value != null;
        }

    }

}
