using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    /// <summary>
    /// Base type for a ref constant reader.
    /// </summary>
    /// <typeparam name="TRecord"></typeparam>
    /// <typeparam name="TOverride"></typeparam>
    public abstract class RefConstantReader<TRecord, TOverride> : ConstantReader<TRecord, TOverride>, IRefConstantReader<TRecord, TOverride>
        where TRecord : RefConstantRecord
        where TOverride : RefConstantOverride
    {

        string className;
        string name;
        string type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public RefConstantReader(ClassReader declaringClass, TRecord record, TOverride @override) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the class name of the field.
        /// </summary>
        public string ClassName => LazyGet(ref className, () => Override != null ? Override.ClassName : DeclaringClass.ResolveConstant<ClassConstantReader>(Record.ClassIndex).Name);

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string Name => LazyGet(ref name, () => Override != null ? Override.Name : DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Name);

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public string Type => LazyGet(ref type, () => Override != null ? Override.Type : DeclaringClass.ResolveConstant<NameAndTypeConstantReader>(Record.NameAndTypeIndex).Type);

    }

}