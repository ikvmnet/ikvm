namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// 
    /// </summary>
    internal enum ManagedSignatureKind : byte
    {

        Type,
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