namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// 
    /// </summary>
    enum ManagedSignatureCodeKind : byte
    {

        Type,
        PrimitiveType,
        SZArray,
        Array,
        ByRef,
        Generic,
        GenericConstraint,
        GenericTypeParameter,
        GenericMethodParameter,
        Modified,
        Pointer,
        FunctionPointer,

    }

}