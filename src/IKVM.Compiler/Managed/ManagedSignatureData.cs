using System;
using System.Diagnostics;
using System.Net;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Manages a sequence of codes, each of which defines a signature that forms a component of this signature, in reverse polish notation.
    /// </summary>
    /// <remarks>
    /// The first four elements of the sequence can be stored inline. If more codes are required, the memory list can be expanded to map to sequences of those.
    /// </remarks>
    readonly struct ManagedSignatureData
    {

        readonly int localN;
        readonly ManagedSignatureCode local0;
        readonly ManagedSignatureCode local1;
        readonly ManagedSignatureCode local2;
        readonly ManagedSignatureCode local3;
        readonly ReadOnlyFixedValueList<Memory<ManagedSignatureCode>> memory;

        /// <summary>
        /// Initializes a new instance from a single new zero argument code.
        /// </summary>
        /// <param name="code"></param>
        internal ManagedSignatureData(ManagedSignatureCodeData data)
        {
        }

        /// <summary>
        /// Initializes a new code, with one or two optional arguments, and an optional dynamic argument.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv0"></param>
        internal ManagedSignatureData(in ManagedSignatureData? arg0, in ManagedSignatureData? arg1, in ReadOnlyFixedValueList<ManagedSignatureData>? argv0, ManagedSignatureCodeData data)
        {
            switch (data.Kind)
            {
                case ManagedSignatureCodeKind.Type:
                    Debug.Assert(data.Type_Context != null);
                    Debug.Assert(data.Type_AssemblyName != null);
                    Debug.Assert(data.Type_TypeName != null);
                    Debug.Assert(arg0 == null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    localN = 1;
                    local0 = new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty);
                    break;
                case ManagedSignatureCodeKind.PrimitiveType:
                    Debug.Assert(data.Primitive_Type != null);
                    Debug.Assert(arg0 == null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    localN = 1;
                    local0 = new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty);
                    break;
                case ManagedSignatureCodeKind.SZArray:
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    var arg0P = WriteArg(arg0);
                    break;
                case ManagedSignatureCodeKind.Array:
                    Debug.Assert(data.Array_Shape != null);
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.ByRef:
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.Generic:
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 != null);
                    break;
                case ManagedSignatureCodeKind.GenericConstraint:
                    Debug.Assert(arg0 == null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 != null);
                    break;
                case ManagedSignatureCodeKind.GenericTypeParameter:
                    Debug.Assert(data.GenericTypeParameter_Parameter != null);
                    Debug.Assert(arg0 == null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.GenericMethodParameter:
                    Debug.Assert(data.GenericMethodParameter_Parameter != null);
                    Debug.Assert(arg0 == null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.Modified:
                    Debug.Assert(data.Modified_Required != null);
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 != null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.Pointer:
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 == null);
                    break;
                case ManagedSignatureCodeKind.FunctionPointer:
                    Debug.Assert(arg0 != null);
                    Debug.Assert(arg1 == null);
                    Debug.Assert(argv0 != null);
                    break;
            }
        }

    }

}
