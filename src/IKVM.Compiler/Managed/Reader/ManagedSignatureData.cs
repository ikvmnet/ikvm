using System;
using System.Text;
using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Manages a sequence of codes, each of which defines a signature that forms a component of this signature, in reverse polish notation.
    /// </summary>
    /// <remarks>
    /// The first four elements of the sequence can be stored inline. If more codes are required, the memory list can be expanded to map to sequences of those.
    /// </remarks>
    internal struct ManagedSignatureData
    {

        /// <summary>
        /// Total length of the signature data.
        /// </summary>
        internal short Length;

        /// <summary>
        /// First local code.
        /// </summary>
        internal ManagedSignatureCode Local0;

        /// <summary>
        /// Second local code.
        /// </summary>
        internal ManagedSignatureCode Local1;

        /// <summary>
        /// Third local code.
        /// </summary>
        internal ManagedSignatureCode Local2;

        /// <summary>
        /// Fourth local code.
        /// </summary>
        internal ManagedSignatureCode Local3;

        /// <summary>
        /// Fixed list of additional code memory.
        /// </summary>
        internal FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> Memory;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="codeData"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv"></param>
        public ManagedSignatureData(in ManagedSignatureCodeData codeData, in ManagedSignatureData? arg0, in ManagedSignatureData? arg1, in FixedValueList4<ManagedSignatureData>? argv)
        {
            Write(codeData, arg0, arg1, argv, out this);
        }

        /// <summary>
        /// Writes a new <see cref="ManagedSignatureData"/> instance from the specified input.
        /// </summary>
        /// <param name="codeData"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv"></param>
        public static void Write(in ManagedSignatureCodeData codeData, in ManagedSignatureData? arg0, in ManagedSignatureData? arg1, in FixedValueList4<ManagedSignatureData>? argv, out ManagedSignatureData result)
        {
            result = new ManagedSignatureData();

            // write existing arguments and calculate absolute positions
            var arg0Pos = arg0 != null ? WriteSignature(arg0.Value, ref result) : short.MinValue;
            var arg1Pos = arg1 != null ? WriteSignature(arg1.Value, ref result) : short.MinValue;
            var argvPos = argv != null ? WriteSignatureList(argv.Value, ref result) : FixedValueList2<short>.Empty;

            // transform absolute position into relative
            var arg0Off = arg0Pos != short.MinValue ? (short)(arg0Pos - result.Length) : (short)0;
            var arg1Off = arg1Pos != short.MinValue ? (short)(arg1Pos - result.Length) : (short)0;
            var argvOff = new FixedValueList2<short>(argvPos.Count);
            for (int i = 0; i < argvPos.Count; i++)
                argvOff[i] = (short)(argvPos[i] - result.Length);

            // write new code
            WriteCode(new ManagedSignatureCode(codeData, arg0Off, arg1Off, argvOff), ref result);
        }

        /// <summary>
        /// Encodes an existing signature into the passed structure fields.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static internal short WriteSignature(in ManagedSignatureData source, ref ManagedSignatureData result)
        {
            switch (source.Length)
            {
                case 1:
                    WriteCode(source.Local0, ref result);
                    break;
                case 2:
                    WriteCode(source.Local0, ref result);
                    WriteCode(source.Local1, ref result);
                    break;
                case 3:
                    WriteCode(source.Local0, ref result);
                    WriteCode(source.Local1, ref result);
                    WriteCode(source.Local2, ref result);
                    break;
                default:
                    WriteCode(source.Local0, ref result);
                    WriteCode(source.Local1, ref result);
                    WriteCode(source.Local2, ref result);
                    WriteCode(source.Local3, ref result);
                    break;
            }

            // copy remainder of signature
            WriteMemoryList(source.Memory, int.MaxValue, ref result);

            // return position of code of signature (should be last index)
            return (short)(result.Length - 1);
        }

        /// <summary>
        /// Encodes an existing signature into the passed structure fields.
        /// </summary>
        /// <param name="sigs"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static internal FixedValueList2<short> WriteSignatureList(in FixedValueList4<ManagedSignatureData> sigs, ref ManagedSignatureData result)
        {
            var l = new FixedValueList2<short>(sigs.Count);

            for (int i = 0; i < sigs.Count; i++)
                l[i] = WriteSignature(sigs[i], ref result);

            return l;
        }

        /// <summary>
        /// Packs the blocks into a single block if a threshold is exceeded.
        /// </summary>
        /// <param name="result"></param>
        static internal void Pack(ref FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> blocks)
        {
            // at 8 blocks added, we pack the blocks into a single block
            if (blocks.Count >= 8)
            {
                var s = 0;
                for (int i = 0; i < blocks.Count; i++)
                    s += blocks[i].Length;

                // copy existing blocks into new block
                var m = new ManagedSignatureCode[s];
                var p = 0;
                for (int i = 0; i < blocks.Count; i++)
                {
                    var b = blocks[i];
                    b.Span.CopyTo(m.AsSpan(p));
                    p += b.Length;
                }

                // rebuild list with single new block
                blocks = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(1);
                blocks[0] = m;
            }
        }

        /// <summary>
        /// Appends a new code into the passed structure fields and returns the position of that code.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        static internal void WriteCode(in ManagedSignatureCode code, ref ManagedSignatureData result)
        {
            Pack(ref result.Memory);

            switch (result.Length)
            {
                case 0:
                    result.Local0 = code;
                    result.Length++;
                    break;
                case 1:
                    result.Local1 = code;
                    result.Length++;
                    break;
                case 2:
                    result.Local2 = code;
                    result.Length++;
                    break;
                case 3:
                    result.Local3 = code;
                    result.Length++;
                    break;
                default:
                    // append single code into new block
                    var l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(result.Memory.Count + 1, result.Memory);
                    l[result.Memory.Count] = new[] { code };
                    result.Memory = l;
                    result.Length++;
                    break;
            }
        }

        /// <summary>
        /// Appends a block of codes into the passed structure fields.
        /// </summary>
        /// <param name="block"></param>
        /// <param name="result"></param>
        static internal void WriteMemory(ReadOnlyMemory<ManagedSignatureCode> block, int total, ref ManagedSignatureData result)
        {
            Pack(ref result.Memory);

            // copy existing memory into new list, of size capable of holding existing and all new blocks (max)
            var l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(result.Memory.Count + 1, result.Memory);
            var c = (short)result.Memory.Count; // track total blocks written (might be less on split)

            // draing memory block until we're left with nothing
            while (block.Length > 0 && result.Length < total)
            {
                switch (result.Length)
                {
                    case 0:
                        result.Local0 = block.Span[0];
                        result.Length++;
                        block = block.Slice(1, total);
                        break;
                    case 1:
                        result.Local1 = block.Span[0];
                        result.Length++;
                        block = block.Slice(1, total);
                        break;
                    case 2:
                        result.Local2 = block.Span[0];
                        result.Length++;
                        block = block.Slice(1, total);
                        break;
                    case 3:
                        result.Local3 = block.Span[0];
                        result.Length++;
                        block = block.Slice(1, total);
                        break;
                    default:
                        l[c++] = block;
                        result.Length += (short)block.Length;
                        block = block.Slice(block.Length); // should result in empty slice
                        break;
                }
            }

            // final block count is different than max assumed, rebuild list
            if (l.Count != c)
                l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(c, l);

            // save new memory list
            result.Memory = l;
        }

        /// <summary>
        /// Appends a set of blocks of codes into the passed structure fields.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="result"></param>
        static internal void WriteMemoryList(in FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> blocks, int total, ref ManagedSignatureData result)
        {
            Pack(ref result.Memory);

            // skip work if no blocks
            if (blocks.Count == 0)
                return;

            // copy existing memory into new list, of size capable of holding existing and all new blocks (max)
            var l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(result.Memory.Count + blocks.Count, result.Memory);
            var c = (short)result.Memory.Count; // track total blocks written (might be less on split)

            // process each memory block until we run out
            for (int i = 0; i < blocks.Count && result.Length < total; i++)
            {
                var m = blocks[i];

                // draing memory block until we're left with nothing
                while (m.Length > 0 && result.Length < total)
                {
                    switch (result.Length)
                    {
                        case 0:
                            result.Local0 = m.Span[0];
                            result.Length++;
                            m = m.Slice(1, total);
                            break;
                        case 1:
                            result.Local1 = m.Span[0];
                            result.Length++;
                            m = m.Slice(1, total);
                            break;
                        case 2:
                            result.Local2 = m.Span[0];
                            result.Length++;
                            m = m.Slice(1, total);
                            break;
                        case 3:
                            result.Local3 = m.Span[0];
                            result.Length++;
                            m = m.Slice(1, total);
                            break;
                        default:
                            l[c++] = m;
                            result.Length += (short)m.Length;
                            m = m.Slice(m.Length); // should result in empty slice
                            break;
                    }
                }
            }

            // final block count is different than max assumed, rebuild list
            if (l.Count != c)
                l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(c, l);

            // save new memory list
            result.Memory = l;
        }

        /// <summary>
        /// Extracts a data structure for the code at the specified index.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        internal static void ExtractCode(in ManagedSignatureData source, int index, out ManagedSignatureData result)
        {
            result = new ManagedSignatureData();

            // get starting position by navigating backwards
            var start = index;
            var code = source.GetCodeRef(index);

            // follow code to earliest reference: this will be the starting position of the copy
            while (true)
            {
                // calculate earliest offset of current code
                var offset = Math.Min(code.Arg0, code.Arg1);
                for (var i = 0; i < code.Argv.Count; i++)
                    offset = Math.Min(offset, code.Argv[i]);

                // current code has no arguments, we're done
                if (offset == 0)
                    break;

                // caculate new offset, resolve code at position
                start += offset;
                code = source.GetCodeRef(start);
            }

            // total number of codes to copy beginning at p
            var total = index - start + 1;

            // copy source slots to result slots
            while (start < 4 && result.Length < total)
            {
                switch (start)
                {
                    case 0:
                        WriteCode(source.Local0, ref result);
                        break;
                    case 1:
                        WriteCode(source.Local1, ref result);
                        break;
                    case 2:
                        WriteCode(source.Local2, ref result);
                        break;
                    case 3:
                        WriteCode(source.Local3, ref result);
                        break;
                }

                // increment position, decrement remaining size to copy
                start++;
            }

            // we have more to copy
            // import and slice each block until we reach end
            for (int i = 0; i < source.Memory.Count && result.Length < total; i++)
            {
                var block = source.Memory[i];

                // draing memory block until we're left with nothing
                while (block.Length > 0 && result.Length < total)
                {
                    switch (result.Length)
                    {
                        case 0:
                            result.Local0 = block.Span[0];
                            result.Length++;
                            block = block.Slice(1, total);
                            break;
                        case 1:
                            result.Local1 = block.Span[0];
                            result.Length++;
                            block = block.Slice(1, total);
                            break;
                        case 2:
                            result.Local2 = block.Span[0];
                            result.Length++;
                            block = block.Slice(1, total);
                            break;
                        case 3:
                            result.Local3 = block.Span[0];
                            result.Length++;
                            block = block.Slice(1, total);
                            break;
                        default:
                            WriteMemory(block, int.MaxValue, ref result);
                            block = block.Slice(block.Length); // should result in empty slice
                            break;
                    }
                }
            }
        }

        /// <inheritdoc />
        public override readonly string ToString()
        {
            return CodeToString(Length - 1);
        }

        /// <summary>
        /// Returns a string representation of the signature starting at the specified code position.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        readonly string CodeToString(int i)
        {
            ref readonly var code = ref this.GetCodeRef(i);

            var arg0Pos = code.Arg0 != 0 ? i + code.Arg0 : -1;
            var arg1Pos = code.Arg1 != 0 ? i + code.Arg1 : -1;

            return code.Data.Kind switch
            {
                ManagedSignatureKind.Type => code.Data.Type!.Name,
                ManagedSignatureKind.SZArray => $"{CodeToString(arg0Pos)}[]",
                ManagedSignatureKind.Array => $"{CodeToString(arg0Pos)}{ArrayShapeToString(code.Data.Data.Array_Shape)}",
                ManagedSignatureKind.ByRef => $"{CodeToString(arg0Pos)}&",
                ManagedSignatureKind.Generic => $"{CodeToString(arg0Pos)}{GenericParametersToString(i, code.Argv)}",
                ManagedSignatureKind.GenericConstraint => throw new NotImplementedException(),
                ManagedSignatureKind.GenericTypeParameter => code.Data.Data.GenericTypeParameter_Parameter.Index.ToString(),
                ManagedSignatureKind.GenericMethodParameter => code.Data.Data.GenericMethodParameter_Parameter.Index.ToString(),
                ManagedSignatureKind.Modified => throw new NotImplementedException(),
                ManagedSignatureKind.Pointer => $"{CodeToString(arg0Pos)}*",
                ManagedSignatureKind.FunctionPointer => throw new NotImplementedException(),
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Returns a string representation of an array specifier.
        /// </summary>
        /// <param name="shape"></param>
        /// <returns></returns>
        readonly string ArrayShapeToString(in ManagedArrayShape shape)
        {
            var b = new StringBuilder();
            b.Append("[");
            for (int i = 0; i < shape.Rank - 1; i++)
                b.Append(", ");

            b.Append("]");
            return b.ToString();
        }

        /// <summary>
        /// Returns a string of a generic specifier.
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="argv"></param>
        /// <returns></returns>
        readonly string GenericParametersToString(int pos, in FixedValueList2<short> argv)
        {
            var b = new StringBuilder();
            b.Append("<");
            for (int i = 0; i < argv.Count; i++)
            {
                b.Append(CodeToString(pos + argv[i]));
                if (i < argv.Count - 1)
                    b.Append(", ");
            }

            b.Append(">");
            return b.ToString();
        }

    }

}
