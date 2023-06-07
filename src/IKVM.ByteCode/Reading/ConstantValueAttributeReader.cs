using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public sealed class ConstantValueAttributeReader : AttributeReader<ConstantValueAttributeRecord>
    {

        object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="record"></param>
        internal ConstantValueAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ConstantValueAttributeRecord record) :
            base(declaringClass, info, record)
        {

        }

        public object Value => value ??= ResolveValue();

        object ResolveValue() => DeclaringClass.Constants.Get<IConstantReader>(Record.ValueIndex) switch
        {
            LongConstantReader l => l.Value,
            FloatConstantReader f => f.Value,
            DoubleConstantReader d => d.Value,
            IntegerConstantReader i => i.Value,
            StringConstantReader s => s.Value,
            _ => throw new ByteCodeException("Invalid constant type for constant attribute."),
        };

    }

}
