using System;
using System.Buffers;
using System.Diagnostics;

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

        readonly int length;
        readonly ManagedSignatureCode local0;
        readonly ManagedSignatureCode local1;
        readonly ManagedSignatureCode local2;
        readonly ManagedSignatureCode local3;
        readonly ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory;

        /// <summary>
        /// Initializes a new code, with one or two optional arguments, and an optional dynamic argument.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv0"></param>
        internal ManagedSignatureData(ManagedSignatureCodeData data, in ManagedSignatureData? arg0, in ManagedSignatureData? arg1, in ReadOnlyFixedValueList<ManagedSignatureData>? argv0)
        {
            switch (data.Kind)
            {
                case ManagedSignatureCodeKind.Type:
                    WriteType(data, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.PrimitiveType:
                    WritePrimitiveType(data, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.SZArray:
                    WriteSZArray(data, arg0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.Array:
                    WriteArray(data, arg0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.ByRef:
                    WriteByRef(data, arg0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.Generic:
                    WriteGeneric(data, arg0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.GenericConstraint:
                    WriteGenericConstraint(data, argv0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.GenericTypeParameter:
                    WriteGenericTypeParameter(data, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.GenericMethodParameter:
                    WriteGenericMethodParameter(data, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.Modified:
                    WriteModified(data, arg0!.Value, arg1!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.Pointer:
                    WritePointer(data, arg0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                case ManagedSignatureCodeKind.FunctionPointer:
                    WriteFunctionPointer(data, arg0!.Value, argv0!.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
                default:
                    throw new ManagedTypeException("Invalid signature kind.");
            }
        }

        /// <summary>
        /// Writes a new type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteType(in ManagedSignatureCodeData data, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            WriteCode(new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a new primitive type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WritePrimitiveType(in ManagedSignatureCodeData data, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            WriteCode(new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a new szarray type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="elementType"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteSZArray(in ManagedSignatureCodeData data, in ManagedSignatureData elementType, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var elementType_ = WriteSignature(elementType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, elementType_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a new array type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="elementType"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteArray(in ManagedSignatureCodeData data, in ManagedSignatureData elementType, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var elementType_ = WriteSignature(elementType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, elementType_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a new byref type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseType"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteByRef(in ManagedSignatureCodeData data, in ManagedSignatureData baseType, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var baseType_ = WriteSignature(baseType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, baseType_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a generic type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="definition"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteGeneric(in ManagedSignatureCodeData data, in ManagedSignatureData definition, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var definition_ = WriteSignature(definition, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, definition_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a generic constraint to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="constraints"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteGenericConstraint(in ManagedSignatureCodeData data, in ReadOnlyFixedValueList<ManagedSignatureData> constraints, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var constraints_ = WriteSignatureList(constraints, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, 0, constraints_), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a generic type parameter to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteGenericTypeParameter(in ManagedSignatureCodeData data, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            WriteCode(new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a generic method parameter to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteGenericMethodParameter(in ManagedSignatureCodeData data, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            WriteCode(new ManagedSignatureCode(data, 0, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a modified type to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseType"></param>
        /// <param name="modifierType"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteModified(in ManagedSignatureCodeData data, in ManagedSignatureData baseType, in ManagedSignatureData modifierType, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var baseType_ = WriteSignature(baseType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            var modifierType_ = WriteSignature(modifierType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, baseType_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a pointer to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="baseType"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WritePointer(in ManagedSignatureCodeData data, in ManagedSignatureData baseType, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var baseType_ = WriteSignature(baseType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, baseType_, ReadOnlyFixedValueList<int>.Empty), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Writes a function pointer to the sequence.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static void WriteFunctionPointer(in ManagedSignatureCodeData data, in ManagedSignatureData returnType, in ReadOnlyFixedValueList<ManagedSignatureData> parameterTypes, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var returnType_ = WriteSignature(returnType, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            var parameterTypes_ = WriteSignatureList(parameterTypes, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
            WriteCode(new ManagedSignatureCode(data, returnType_, parameterTypes_), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Encodes an existing signature into the passed structure fields.
        /// </summary>
        /// <param name="sig"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        static int WriteSignature(in ManagedSignatureData sig, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var r = 4 - Math.Min(length, 4);
            if (r == 1)
                local0 = sig.local0;



            return length - 1;
        }

        /// <summary>
        /// Encodes an existing signature into the passed structure fields.
        /// </summary>
        /// <param name="sig"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        static ReadOnlyFixedValueList<int> WriteSignatureList(in ReadOnlyFixedValueList<ManagedSignatureData> sigs, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var l = new FixedValueList<int>(sigs.Count);

            for (int i = 0; i < sigs.Count; i++)
            {

            }
        }

        /// <summary>
        /// Appends a block of codes into the passed structure fields.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="block"></param>
        static void WriteCodeBlock(ReadOnlyMemory<ManagedSignatureCode> block, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {

        }

        /// <summary>
        /// Appends a new code into the passed structure fields.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        static int WriteCode(in ManagedSignatureCode code, ref int length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {

        }

    }

}
