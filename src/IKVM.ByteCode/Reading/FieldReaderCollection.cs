using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Lazy init collection of fields.
    /// </summary>
    internal sealed class FieldReaderCollection : LazyReaderList<FieldReader, FieldRecord>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="records"></param>
        internal FieldReaderCollection(ClassReader declaringClass, FieldRecord[] records) :
            base(declaringClass, records)
        {

        }

        /// <summary>
        /// Creates a new field reader.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        protected override FieldReader CreateReader(int index, FieldRecord record)
        {
            return new FieldReader(DeclaringClass, record);
        }

        /// <summary>
        /// Gets the field with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldReader this[string name] => Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault() ?? throw new KeyNotFoundException();

        /// <summary>
        /// Gets the names of the fields.
        /// </summary>
        public IEnumerable<string> Names => Enumerable.Range(0, Count).Select(i => this[i]).Select(i => i.Name);

        /// <summary>
        /// Returns <c>true</c> if an field with the specified name exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains(string name) => Enumerable.Range(0, Count).Any(i => this[i].Name == name);

        /// <summary>
        /// Attempts to get the field with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGet(string name, out FieldReader value)
        {
            value = Enumerable.Range(0, Count).Where(i => this[i].Name == name).Select(i => this[i]).FirstOrDefault();
            return value != null;
        }

    }

}
