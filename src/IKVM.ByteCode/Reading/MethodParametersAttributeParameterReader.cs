using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class MethodParametersAttributeParameterReader
    {

        readonly ClassReader declaringClass;
        readonly MethodParametersAttributeParameterRecord record;

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="record"></param>
        public MethodParametersAttributeParameterReader(ClassReader declaringClass, MethodParametersAttributeParameterRecord record)
        {
            this.declaringClass = declaringClass ?? throw new System.ArgumentNullException(nameof(declaringClass));
            this.record = record;
        }

        /// <summary>
        /// Gets the name of the parameters.
        /// </summary>
        public string Name => ClassReader.LazyGet(ref name, () => declaringClass.ResolveConstant<Utf8ConstantReader>(record.NameIndex).Value);

        /// <summary>
        /// Gets the underlying record being read.
        /// </summary>
        public MethodParametersAttributeParameterRecord Record => record;

        /// <summary>
        /// Gets the access flags of the parameter.
        /// </summary>
        public AccessFlag AccessFlags => record.AccessFlags;

    }

}