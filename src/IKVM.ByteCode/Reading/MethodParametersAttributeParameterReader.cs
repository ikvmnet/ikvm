using IKVM.ByteCode.Parsing;

using static IKVM.ByteCode.Util;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodParametersAttributeParameterReader : ReaderBase<MethodParametersAttributeParameterRecord>
    {

        Utf8ConstantReader name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="record"></param>
        public MethodParametersAttributeParameterReader(ClassReader declaringClass, MethodParametersAttributeParameterRecord record) :
            base(declaringClass, record)
        {

        }

        /// <summary>
        /// Gets the name of the parameters.
        /// </summary>
        public Utf8ConstantReader Name => LazyGet(ref name, () => DeclaringClass.Constants.Get<Utf8ConstantReader>(Record.NameIndex));

        /// <summary>
        /// Gets the access flags of the parameter.
        /// </summary>
        public AccessFlag AccessFlags => Record.AccessFlags;

    }

}