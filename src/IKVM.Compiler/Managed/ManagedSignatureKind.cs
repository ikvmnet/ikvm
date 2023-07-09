namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// 
    /// </summary>
    public enum ManagedSignatureKind : byte
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