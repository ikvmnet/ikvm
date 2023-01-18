using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class StringConstantReader : ConstantReader<StringConstantRecord, StringConstantOverride>
    {

        object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public StringConstantReader(ClassReader declaringClass, StringConstantRecord record, StringConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the value of the string constant. Result may not actually be a string object as overrides can apply.
        /// </summary>
        public object Value => LazyGet(ref value, () => Override != null ? Override.Value : DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.ValueIndex));

        /// <summary>
        /// Returns <c>true</c> if this class is loadable according to the Java specification.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion == 45 && DeclaringClass.MinorVersion >= 3 || DeclaringClass.MajorVersion > 45;

    }

}