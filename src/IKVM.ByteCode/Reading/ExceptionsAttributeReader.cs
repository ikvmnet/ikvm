using System.Collections.Generic;
using System.Linq;

using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ExceptionsAttributeReader : AttributeReader<ExceptionsAttributeRecord>
    {

        string[] exceptions;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal ExceptionsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ExceptionsAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the names of the exceptions.
        /// </summary>
        public IReadOnlyList<string> Exceptions => LazyGet(ref exceptions, () => Record.ExceptionsIndexes.Select(i => DeclaringClass.ResolveConstant<ClassConstantReader>(i).Name).ToArray());

    }

}
