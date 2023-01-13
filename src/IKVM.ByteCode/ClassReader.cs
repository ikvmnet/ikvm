using System.Buffers;

namespace IKVM.ByteCode
{

    public partial class ClassReader
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ClassReader()
        {

        }

        /// <summary>
        /// Reads a <see cref="Class"/> from the given buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public Class Read(ReadOnlySequence<byte> buffer)
        {
            var p = new ClassParser(buffer);
            var r = new ClassRecord();
            p.Parse(r);
            return r.Resolve();
        }

    }

}
