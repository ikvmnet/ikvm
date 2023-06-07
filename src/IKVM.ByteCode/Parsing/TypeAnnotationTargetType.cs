namespace IKVM.ByteCode.Parsing
{

    public enum TypeAnnotationTargetType : byte
    {

        ClassTypeParameter = 0x00,
        MethodTypeParameter = 0x01,
        ClassExtends = 0x10,
        ClassTypeParameterBound = 0x11,
        MethodTypeParameterBound = 0x12,
        Field = 0x13,
        MethodReturn = 0x14,
        MethodReceiver = 0x15,
        MethodFormalParameter = 0x16,
        Throws = 0x17,
        LocalVariable = 0x40,
        ResourceVariable = 0x41,
        ExceptionParameter = 0x42,
        InstanceOf = 0x43,
        New = 0x44,
        ConstructorReference = 0x45,
        MethodReference = 0x46,
        Cast = 0x47,
        ConstructorInvocationTypeArgument = 0x48,
        MethodInvocationTypeArgument = 0x49,
        ConstructorReferenceTypeArgument = 0x4A,
        MethodReferenceTypeArgument = 0x4B,

    }

}