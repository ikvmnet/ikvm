// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Describes an intermediate language (IL) instruction.
    /// </summary>
    readonly struct OpCode : IEquatable<OpCode>
    {

        // the flags are a packed bitfield, to avoid code bloat

        /// <summary>
        /// The mask of the <see cref="Emit.OperandType"/> bits in the flags.
        /// </summary>
        internal const int OperandTypeMask = 0x1F;              // 000000000000000000000000000XXXXX

        /// <summary>
        /// The position of the <see cref="Emit.FlowControl"/> bits in the flags.
        /// </summary>
        internal const int FlowControlShift = 5;                // 00000000000000000000000XXXX00000

        /// <summary>
        /// The mask of the <see cref="Emit.FlowControl"/> bits, after shifting by <see cref="FlowControlShift"/>.
        /// </summary>
        internal const int FlowControlMask = 0x0F;

        /// <summary>
        /// The position of the opcode type bits in the flags.
        /// </summary>
        internal const int OpCodeTypeShift = 9;                 // 00000000000000000000XXX000000000

        /// <summary>
        /// The position of the <see cref="StackBehaviourPop"/> bits in the flags.
        /// </summary>
        internal const int StackBehaviourPopShift = 12;         // 000000000000000XXXXX000000000000

        /// <summary>
        /// The position of the <see cref="StackBehaviourPush"/> bits in the flags.
        /// </summary>
        internal const int StackBehaviourPushShift = 17;        // 0000000000XXXXX00000000000000000

        /// <summary>
        /// The mask of the stack behaviour bits, after shifting by <see cref="StackBehaviourPopShift"/> or
        /// <see cref="StackBehaviourPushShift"/>.
        /// </summary>
        internal const int StackBehaviourMask = 0x1F;

        /// <summary>
        /// The position of the <see cref="Size"/> bits in the flags.
        /// </summary>
        internal const int SizeShift = 22;                      // 00000000XX0000000000000000000000

        /// <summary>
        /// The mask of the <see cref="Size"/> bits, after shifting by <see cref="SizeShift"/>.
        /// </summary>
        internal const int SizeMask = 0x03;

        /// <summary>
        /// The flag indicating that the instruction unconditionally ends a basic block.
        /// </summary>
        internal const int EndsUncondJmpBlkFlag = 0x01000000;   // 0000000X000000000000000000000000

        // unused                                               // 0000XXX0000000000000000000000000

        /// <summary>
        /// The position of the signed net stack change in the flags.
        /// </summary>
        internal const int StackChangeShift = 28;               // XXXX0000000000000000000000000000

        readonly OpCodeValues _value;
        readonly int _flags;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value">The value of the instruction.</param>
        /// <param name="flags">The packed operand type, flow control, stack behaviour, size and stack change.</param>
        internal OpCode(OpCodeValues value, int flags)
        {
            _value = value;
            _flags = flags;
        }

        /// <summary>
        /// Returns whether the instruction unconditionally ends a basic block, so that the code after it is not
        /// reachable by falling through.
        /// </summary>
        /// <returns><see langword="true"/> if the instruction ends the block; otherwise, <see langword="false"/>.</returns>
        internal bool EndsUncondJmpBlk() =>
            (_flags & EndsUncondJmpBlkFlag) != 0;

        /// <summary>
        /// Returns the net change in the evaluation stack depth caused by the instruction, for instructions whose stack
        /// behaviour is fixed.
        /// </summary>
        /// <returns>The number of items pushed minus the number of items popped.</returns>
        internal int StackChange() =>
            _flags >> StackChangeShift;

        /// <summary>
        /// Gets the operand type of an intermediate language (IL) opcode.
        /// </summary>
        public OperandType OperandType => (OperandType)(_flags & OperandTypeMask);

        /// <summary>
        /// Gets the flow control characteristics of the intermediate language (IL) instruction.
        /// </summary>
        public FlowControl FlowControl => (FlowControl)((_flags >> FlowControlShift) & FlowControlMask);

        /// <summary>
        /// Gets how the intermediate language (IL) instruction pops the stack.
        /// </summary>
        public StackBehaviour StackBehaviourPop => (StackBehaviour)((_flags >> StackBehaviourPopShift) & StackBehaviourMask);

        /// <summary>
        /// Gets how the intermediate language (IL) instruction pushes operand onto the stack.
        /// </summary>
        public StackBehaviour StackBehaviourPush => (StackBehaviour)((_flags >> StackBehaviourPushShift) & StackBehaviourMask);

        /// <summary>
        /// Gets the size of the intermediate language (IL) instruction.
        /// </summary>
        public int Size => (_flags >> SizeShift) & SizeMask;

        /// <summary>
        /// Gets the numeric value of the intermediate language (IL) instruction.
        /// </summary>
        public short Value => (short)_value;

        // the lazily created cache of opcode names, indexed by one-byte value or by 0x100 plus the second byte
        static volatile string[]? s_g_nameCache;

        /// <summary>
        /// Gets the name of the intermediate language (IL) instruction.
        /// </summary>
        public string? Name
        {
            get
            {
                if (Size == 0)
                    return null;

                // create and cache the opcode names lazily, they should be rarely used (only for logging, etc.)
                // note that we do not take any locks here, because we always get the same names; the last one wins
                var nameCache = s_g_nameCache;
                if (nameCache == null)
                {
                    nameCache = new string[0x11f];
                    s_g_nameCache = nameCache;
                }

                OpCodeValues opCodeValue = (OpCodeValues)(ushort)Value;

                var idx = (int)opCodeValue;
                if (idx > 0xFF)
                {
                    if (idx >= 0xfe00 && idx <= 0xfe1e)
                    {
                        // transform the two byte opcode value to a lower range that is suitable for an array index
                        idx = 0x100 + (idx - 0xfe00);
                    }
                    else
                    {
                        // unknown opcode
                        return null;
                    }
                }

                var name = Volatile.Read(ref nameCache[idx]);
                if (name != null)
                    return name;

                // create the ilasm style name from the enum value name
                name = GetEnumName(opCodeValue)!.ToLowerInvariant().Replace('_', '.');
                Volatile.Write(ref nameCache[idx], name);
                return name;
            }
        }

        /// <summary>
        /// Gets the name of an enum value, using the generic overload where the target framework has it.
        /// </summary>
        /// <typeparam name="T">The enum type.</typeparam>
        /// <param name="value">The enum value.</param>
        /// <returns>The name of the value, or <see langword="null"/> if it has none.</returns>
        string? GetEnumName<T>(T value) where T : struct, Enum
        {
#if NETFRAMEWORK
            return Enum.GetName(typeof(T), value);
#else
            return Enum.GetName(value);
#endif
        }

        /// <summary>
        /// Tests whether the given object is equal to this <see cref="OpCode"/>.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="OpCode"/> and is equal to this
        /// object; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object? obj) =>
            obj is OpCode other && Equals(other);

        /// <summary>
        /// Indicates whether the current instance is equal to the specified <see cref="OpCode"/>.
        /// </summary>
        /// <param name="obj">The <see cref="OpCode"/> to compare to the current instance.</param>
        /// <returns>
        /// <see langword="true"/> if the value of <paramref name="obj"/> is equal to the value of the current instance;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public bool Equals(OpCode obj) => obj.Value == Value;

        /// <summary>
        /// Indicates whether two <see cref="OpCode"/> structures are equal.
        /// </summary>
        /// <param name="a">The <see cref="OpCode"/> to compare to <paramref name="b"/>.</param>
        /// <param name="b">The <see cref="OpCode"/> to compare to <paramref name="a"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="a"/> is equal to <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(OpCode a, OpCode b) => a.Equals(b);

        /// <summary>
        /// Indicates whether two <see cref="OpCode"/> structures are not equal.
        /// </summary>
        /// <param name="a">The <see cref="OpCode"/> to compare to <paramref name="b"/>.</param>
        /// <param name="b">The <see cref="OpCode"/> to compare to <paramref name="a"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="a"/> is not equal to <paramref name="b"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(OpCode a, OpCode b) => !(a == b);

        /// <summary>
        /// Returns the generated hash code for this <see cref="OpCode"/>.
        /// </summary>
        /// <returns>Returns the hash code for this instance.</returns>
        public override int GetHashCode() => Value;

        /// <summary>
        /// Returns the name of the Microsoft intermediate language (MSIL) instruction.
        /// </summary>
        /// <returns>The name of the instruction.</returns>
        public override string? ToString() => Name;

    }

}

#nullable restore
