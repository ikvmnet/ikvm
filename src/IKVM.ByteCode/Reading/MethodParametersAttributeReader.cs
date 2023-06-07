using System.Collections.Generic;

using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodParametersAttributeReader : AttributeReader<MethodParametersAttributeRecord>
    {

        IReadOnlyList<MethodParametersAttributeParameterReader> parameters;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal MethodParametersAttributeReader(ClassReader declaringClass, AttributeInfoReader info, MethodParametersAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        /// <summary>
        /// Gets the set of annotations described by this attribute.
        /// </summary>
        public IReadOnlyList<MethodParametersAttributeParameterReader> Parameters => LazyGet(ref parameters, () => new DelegateLazyReaderList<MethodParametersAttributeParameterReader, MethodParametersAttributeParameterRecord>(DeclaringClass, Record.Parameters, (_, record) => new MethodParametersAttributeParameterReader(DeclaringClass, record)));

    }

}