namespace IKVM.Compiler.Managed.Reader
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