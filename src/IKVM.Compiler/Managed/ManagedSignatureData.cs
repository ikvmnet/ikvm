using System;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Manages a sequence of codes, each of which defines a signature that forms a component of this signature, in reverse polish notation.
    /// </summary>
    /// <remarks>
    /// The first four elements of the sequence can be stored inline. If more codes are required, the memory list can be expanded to map to sequences of those.
    /// </remarks>
    internal readonly struct ManagedSignatureData
    {

        internal readonly short length;
        internal readonly ManagedSignatureCode local0;
        internal readonly ManagedSignatureCode local1;
        internal readonly ManagedSignatureCode local2;
        internal readonly ManagedSignatureCode local3;
        internal readonly ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory;

        /// <summary>
        /// Initializes a new code, with one or two optional arguments, and an optional dynamic argument.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="argv"></param>
        internal ManagedSignatureData(in ManagedSignatureCodeData data, in ManagedSignatureData? arg0, in ManagedSignatureData? arg1, in ReadOnlyFixedValueList4<ManagedSignatureData>? argv)
        {
            var arg0Pos = arg0 != null ? WriteSignature(arg0.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory) : short.MinValue;
            var arg1Pos = arg1 != null ? WriteSignature(arg1.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory) : short.MinValue;
            var argvPos = argv != null ? WriteSignatureList(argv.Value, ref length, ref local0, ref local1, ref local2, ref local3, ref memory) : ReadOnlyFixedValueList2<short>.Empty;

            // transform absolute position into relative
            var arg0Off = arg0Pos != short.MinValue ? (short)(arg0Pos - length) : (short)0;
            var arg1Off = arg1Pos != short.MinValue ? (short)(arg1Pos - length) : (short)0;
            var argvOff = new FixedValueList2<short>(argvPos.Count);
            for (int i = 0; i < argvPos.Count; i++)
                argvOff[i] = (short)(argvPos[i] - length);

            WriteCode(new ManagedSignatureCode(data, arg0Off, arg1Off, argvOff.AsReadOnly()), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
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
        static internal short WriteSignature(in ManagedSignatureData sig, ref short length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            switch (length)
            {
                case 0:
                    switch (sig.length)
                    {
                        case 1:
                            local0 = sig.local0;
                            length += 1;
                            break;
                        case 2:
                            local0 = sig.local0;
                            local1 = sig.local1;
                            length += 2;
                            break;
                        case 3:
                            local0 = sig.local0;
                            local1 = sig.local1;
                            local2 = sig.local2;
                            length += 3;
                            break;
                        default:
                            local0 = sig.local0;
                            local1 = sig.local1;
                            local2 = sig.local2;
                            local3 = sig.local3;
                            length += 4;
                            break;
                    }
                    break;
                case 1:
                    switch (sig.length)
                    {
                        case 1:
                            local1 = sig.local0;
                            length += 1;
                            break;
                        case 2:
                            local1 = sig.local0;
                            local2 = sig.local1;
                            length += 2;
                            break;
                        case 3:
                            local1 = sig.local0;
                            local2 = sig.local1;
                            local3 = sig.local2;
                            length += 3;
                            break;
                        default:
                            local1 = sig.local0;
                            local2 = sig.local1;
                            local3 = sig.local2;
                            length += 3;

                            WriteMemory(new ManagedSignatureCode[1] { sig.local3 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 1;

                            break;
                    }
                    break;
                case 2:
                    switch (sig.length)
                    {
                        case 1:
                            local2 = sig.local0;
                            length += 1;
                            break;
                        case 2:
                            local2 = sig.local0;
                            local3 = sig.local1;
                            length += 2;
                            break;
                        case 3:
                            local2 = sig.local0;
                            local3 = sig.local1;
                            length += 2;

                            WriteMemory(new ManagedSignatureCode[1] { sig.local2 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 1;

                            break;
                        default:
                            local2 = sig.local0;
                            local3 = sig.local1;
                            length += 2;

                            WriteMemory(new ManagedSignatureCode[2] { sig.local2, sig.local3 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 1;

                            break;
                    }
                    break;
                case 3:
                    switch (sig.length)
                    {
                        case 1:
                            local3 = sig.local0;
                            length += 1;
                            break;
                        case 2:
                            local3 = sig.local0;
                            length += 1;

                            WriteMemory(new ManagedSignatureCode[1] { sig.local1 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 1;

                            break;
                        case 3:
                            local2 = sig.local0;
                            length += 1;

                            WriteMemory(new ManagedSignatureCode[2] { sig.local1, sig.local2 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 2;

                            break;
                        default:
                            local2 = sig.local0;
                            length += 1;

                            WriteMemory(new ManagedSignatureCode[3] { sig.local1, sig.local2, sig.local3 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 3;

                            break;
                    }
                    break;
                default:
                    switch (sig.length)
                    {
                        case 1:
                            WriteMemory(new ManagedSignatureCode[1] { sig.local0 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 1;
                            break;
                        case 2:
                            WriteMemory(new ManagedSignatureCode[2] { sig.local0, sig.local1 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 2;
                            break;
                        case 3:
                            WriteMemory(new ManagedSignatureCode[3] { sig.local0, sig.local1, sig.local2 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 3;
                            break;
                        default:
                            WriteMemory(new ManagedSignatureCode[4] { sig.local0, sig.local1, sig.local2, sig.local3 }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                            length += 4;
                            break;
                    }
                    break;
            }

            // copy remainder of signature
            WriteMemoryList(sig.memory, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);

            // return position of code of signature (should be last index)
            return (short)(length - 1);
        }

        /// <summary>
        /// Encodes an existing signature into the passed structure fields.
        /// </summary>
        /// <param name="sigs"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        static internal ReadOnlyFixedValueList2<short> WriteSignatureList(in ReadOnlyFixedValueList4<ManagedSignatureData> sigs, ref short length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var l = new FixedValueList2<short>(sigs.Count);

            for (int i = 0; i < sigs.Count; i++)
                l[i] = WriteSignature(sigs[i], ref length, ref local0, ref local1, ref local2, ref local3, ref memory);

            return l.AsReadOnly();
        }

        /// <summary>
        /// Appends a new code into the passed structure fields and returns the position of that code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        /// <returns></returns>
        static internal short WriteCode(in ManagedSignatureCode code, ref short length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            switch (length)
            {
                case 0:
                    local0 = code;
                    length++;
                    break;
                case 1:
                    local1 = code;
                    length++;
                    break;
                case 2:
                    local2 = code;
                    length++;
                    break;
                case 3:
                    local3 = code;
                    length++;
                    break;
                default:
                    WriteMemory(new ManagedSignatureCode[1] { code }, ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
                    break;
            }

            return (short)(length - 1);
        }

        /// <summary>
        /// Appends a block of codes into the passed structure fields.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="block"></param>
        static internal void WriteMemory(ReadOnlyMemory<ManagedSignatureCode> block, ref short length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            var l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(1);
            l[0] = block;
            WriteMemoryList(l.AsReadOnly(), ref length, ref local0, ref local1, ref local2, ref local3, ref memory);
        }

        /// <summary>
        /// Appends a set of blocks of codes into the passed structure fields.
        /// </summary>
        /// <param name="blocks"></param>
        /// <param name="length"></param>
        /// <param name="local0"></param>
        /// <param name="local1"></param>
        /// <param name="local2"></param>
        /// <param name="local3"></param>
        /// <param name="memory"></param>
        static internal void WriteMemoryList(in ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> blocks, ref short length, ref ManagedSignatureCode local0, ref ManagedSignatureCode local1, ref ManagedSignatureCode local2, ref ManagedSignatureCode local3, ref ReadOnlyFixedValueList2<ReadOnlyMemory<ManagedSignatureCode>> memory)
        {
            // skip work if no blocks
            if (blocks.Count == 0)
                return;

            // at 8 blocks added, we pack the blocks into a single block
            if (memory.Count >= 8)
            {
                // copy existing blocks into new block
                var m = new ManagedSignatureCode[length - 4];
                var p = 0;
                for (int i = 0; i < memory.Count; i++)
                {
                    var b = memory[i];
                    b.Span.CopyTo(m.AsSpan(p));
                    p += b.Length;
                }

                // rebuild list with single new block
                var n = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(1);
                n[0] = m;
                memory = n.AsReadOnly();
            }

            // copy existing memory into new list, of size capable of holding existing and all new blocks (max)
            var l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(memory.Count + blocks.Count, memory);
            var c = (short)memory.Count; // track total blocks written (might be less on split)

            // process each memory block until we run out
            for (int i = 0; i < blocks.Count; i++)
            {
                var m = blocks[i];

                // draing memory block until we're left with nothing
                while (m.Length > 0)
                {
                    switch (length)
                    {
                        case 0:
                            local0 = m.Span[0];
                            length++;
                            m = m.Slice(1);
                            break;
                        case 1:
                            local1 = m.Span[0];
                            length++;
                            m = m.Slice(1);
                            break;
                        case 2:
                            local2 = m.Span[0];
                            length++;
                            m = m.Slice(1);
                            break;
                        case 3:
                            local3 = m.Span[0];
                            length++;
                            m = m.Slice(1);
                            break;
                        default:
                            l[c++] = m;
                            length += (short)m.Length;
                            m = m.Slice(m.Length); // should result in empty slice
                            break;
                    }
                }
            }

            // final block count is different than max assumed, rebuild list
            if (l.Count != c)
                l = new FixedValueList2<ReadOnlyMemory<ManagedSignatureCode>>(c, l);

            // save new memory list
            memory = l.AsReadOnly();
        }

    }

}
