using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class MethodHandleConstantReader : Constant<MethodHandleConstantRecord>
    {

        ConstantReader reference;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="record"></param>
        public MethodHandleConstantReader(ClassReader owner, MethodHandleConstantRecord record) :
            base(owner, record)
        {

        }

        public ReferenceKind Kind => Record.Kind;

        public ConstantReader Reference => reference ??= DeclaringClass.ResolveConstant<ConstantReader>(Record.ReferenceIndex);

    }

}
