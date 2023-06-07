using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Provides static methods for reading constants.
    /// </summary>
    public static class ConstantReader
    {

        /// <summary>
        /// Initializes a <see cref="IConstantReader"/> from a <see cref="ConstantRecord"/>.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        /// <exception cref="ByteCodeException"></exception>
        internal static IConstantReader Read(ClassReader declaringClass, ushort index, ConstantRecord record) => record switch
        {
            Utf8ConstantRecord c => new Utf8ConstantReader(declaringClass, index, c),
            IntegerConstantRecord c => new IntegerConstantReader(declaringClass, index, c),
            FloatConstantRecord c => new FloatConstantReader(declaringClass, index, c),
            LongConstantRecord c => new LongConstantReader(declaringClass, index, c),
            DoubleConstantRecord c => new DoubleConstantReader(declaringClass, index, c),
            ClassConstantRecord c => new ClassConstantReader(declaringClass, index, c),
            StringConstantRecord c => new StringConstantReader(declaringClass, index, c),
            FieldrefConstantRecord c => new FieldrefConstantReader(declaringClass, index, c),
            MethodrefConstantRecord c => new MethodrefConstantReader(declaringClass, index, c),
            InterfaceMethodrefConstantRecord c => new InterfaceMethodrefConstantReader(declaringClass, index, c),
            NameAndTypeConstantRecord c => new NameAndTypeConstantReader(declaringClass, index, c),
            MethodHandleConstantRecord c => new MethodHandleConstantReader(declaringClass, index, c),
            MethodTypeConstantRecord c => new MethodTypeConstantReader(declaringClass, index, c),
            DynamicConstantRecord c => new DynamicConstantReader(declaringClass, index, c),
            InvokeDynamicConstantRecord c => new InvokeDynamicConstantReader(declaringClass, index, c),
            ModuleConstantRecord c => new ModuleConstantReader(declaringClass, index, c),
            PackageConstantRecord c => new PackageConstantReader(declaringClass, index, c),
            _ => throw new ByteCodeException($"Invalid constant type: {record.GetType().Name}"),
        };

    }

    /// <summary>
    /// Base type for constant readers.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    public abstract class ConstantReader<TRecord> : ReaderBase<TRecord>, IConstantReader<TRecord>
        where TRecord : ConstantRecord
    {

        readonly ushort index;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="index"></param>
        /// <param name="record"></param>
        protected ConstantReader(ClassReader declaringClass, ushort index, TRecord record) :
            base(declaringClass, record)
        {
            this.index = index;
        }

        /// <summary>
        /// Gets the index of the constant.
        /// </summary>
        public ushort Index => index;

        /// <summary>
        /// Returns <c>true</c> if the constant is considered loadable according to the JVM specification.
        /// </summary>
        public virtual bool IsLoadable => false;

        /// <summary>
        /// Gets the underlying constant being read.
        /// </summary>
        TRecord IConstantReader<TRecord>.Record => Record;

    }

}
