using IKVM.ByteCode.Parsing;

namespace IKVM.ByteCode.Reading
{

    public class ConstantValueAttributeReader : AttributeData<ConstantValueAttributeRecord>
    {

        object value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringClass"></param>
        /// <param name="info"></param>
        /// <param name="data"></param>
        internal ConstantValueAttributeReader(ClassReader declaringClass, AttributeInfoReader info, ConstantValueAttributeRecord data) :
            base(declaringClass, info, data)
        {

        }

        public object Value => value ??= ResolveValue();

        object ResolveValue() => DeclaringClass.ResolveConstant(Data.ValueIndex) switch
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
