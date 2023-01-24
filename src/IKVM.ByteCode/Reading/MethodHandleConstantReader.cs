using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    internal sealed class MethodHandleConstantReader : ConstantReader<MethodHandleConstantRecord, MethodHandleConstantOverride>
    {

        IRefConstantReader<RefConstantRecord, RefConstantOverride> reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="record"></param>
        /// <param name="override"></param>
        public MethodHandleConstantReader(ClassReader declaringClass, MethodHandleConstantRecord record, MethodHandleConstantOverride @override) :
            base(declaringClass, record, @override)
        {

        }

        /// <summary>
        /// Gets the kind of this reference.
        /// </summary>
        public ReferenceKind ReferenceKind => Record.Kind;

        /// <summary>
        /// Gets the constant refered to by this MethodHandle.
        /// </summary>
        public IRefConstantReader<RefConstantRecord, RefConstantOverride> Reference => LazyGet(ref reference, () => DeclaringClass.ResolveConstant<IRefConstantReader<RefConstantRecord, RefConstantOverride>>(Record.Index));

        /// <summary>
        /// Gets the class of the method handle.
        /// </summary>
        public string ReferenceClassName => Reference.ClassName;

        /// <summary>
        /// Gets the name of the method handle.
        /// </summary>
        public string ReferenceName => Reference.Name;

        /// <summary>
        /// Gets the type of the method handle.
        /// </summary>
        public string ReferenceType => Reference.Type;

        /// <summary>
        /// Gets an anonymous object supplied by an override.
        /// </summary>
        public object ReferenceOverrideValue => Reference.Override.Value;

        /// <summary>
        /// Returns <c>true</c> if this constant type is loadable.
        /// </summary>
        public override bool IsLoadable => DeclaringClass.MajorVersion >= 51;

    }

}
