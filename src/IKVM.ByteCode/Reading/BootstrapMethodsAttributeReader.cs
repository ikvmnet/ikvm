using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public class BootstrapMethodsAttributeReader : AttributeReader<BootstrapMethodsAttributeRecord>
    {

        IReadOnlyList<BootstrapMethodsAttributeMethodReader> methods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal BootstrapMethodsAttributeReader(ClassReader declaringClass, AttributeInfoReader info, BootstrapMethodsAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the set of methods described by this bootstrap attribute.
        /// </summary>
        public IReadOnlyList<BootstrapMethodsAttributeMethodReader> Methods => LazyGet(ref methods, () => new DelegateLazyReaderList<BootstrapMethodsAttributeMethodReader, BootstrapMethodsAttributeMethodRecord>(DeclaringClass, Record.Methods, (_, record) => new BootstrapMethodsAttributeMethodReader(DeclaringClass, record)));

    }

}
