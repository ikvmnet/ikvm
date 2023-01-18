using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class ClassConstantReader : ConstantReader<ClassConstantRecord, ClassConstantOverride>
    {

        string name;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public ClassConstantReader(ClassReader declaringClass, ClassConstantRecord record, ClassConstantOverride @override = null) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        public string Name => LazyGet(ref name, () => Override != null && Override.Value is string value ? value : DeclaringClass.ResolveConstant<Utf8ConstantReader>(Record.NameIndex).Value);

        /// <summary>
        /// Returns whether or not this constant is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion >= 49;

    }

}