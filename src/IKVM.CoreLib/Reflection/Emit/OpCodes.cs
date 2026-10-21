// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Defines the encoded values of the IL instructions supported by the runtime.
    /// </summary>
    /// <remarks>
    /// The value names are used to construct the ilasm-compatible opcode names returned by <see cref="OpCode.Name"/>,
    /// so their exact form is important.
    /// </remarks>
    enum OpCodeValues
    {

        Nop = 0x00,
        Break = 0x01,
        Ldarg_0 = 0x02,
        Ldarg_1 = 0x03,
        Ldarg_2 = 0x04,
        Ldarg_3 = 0x05,
        Ldloc_0 = 0x06,
        Ldloc_1 = 0x07,
        Ldloc_2 = 0x08,
        Ldloc_3 = 0x09,
        Stloc_0 = 0x0a,
        Stloc_1 = 0x0b,
        Stloc_2 = 0x0c,
        Stloc_3 = 0x0d,
        Ldarg_S = 0x0e,
        Ldarga_S = 0x0f,
        Starg_S = 0x10,
        Ldloc_S = 0x11,
        Ldloca_S = 0x12,
        Stloc_S = 0x13,
        Ldnull = 0x14,
        Ldc_I4_M1 = 0x15,
        Ldc_I4_0 = 0x16,
        Ldc_I4_1 = 0x17,
        Ldc_I4_2 = 0x18,
        Ldc_I4_3 = 0x19,
        Ldc_I4_4 = 0x1a,
        Ldc_I4_5 = 0x1b,
        Ldc_I4_6 = 0x1c,
        Ldc_I4_7 = 0x1d,
        Ldc_I4_8 = 0x1e,
        Ldc_I4_S = 0x1f,
        Ldc_I4 = 0x20,
        Ldc_I8 = 0x21,
        Ldc_R4 = 0x22,
        Ldc_R8 = 0x23,
        Dup = 0x25,
        Pop = 0x26,
        Jmp = 0x27,
        Call = 0x28,
        Calli = 0x29,
        Ret = 0x2a,
        Br_S = 0x2b,
        Brfalse_S = 0x2c,
        Brtrue_S = 0x2d,
        Beq_S = 0x2e,
        Bge_S = 0x2f,
        Bgt_S = 0x30,
        Ble_S = 0x31,
        Blt_S = 0x32,
        Bne_Un_S = 0x33,
        Bge_Un_S = 0x34,
        Bgt_Un_S = 0x35,
        Ble_Un_S = 0x36,
        Blt_Un_S = 0x37,
        Br = 0x38,
        Brfalse = 0x39,
        Brtrue = 0x3a,
        Beq = 0x3b,
        Bge = 0x3c,
        Bgt = 0x3d,
        Ble = 0x3e,
        Blt = 0x3f,
        Bne_Un = 0x40,
        Bge_Un = 0x41,
        Bgt_Un = 0x42,
        Ble_Un = 0x43,
        Blt_Un = 0x44,
        Switch = 0x45,
        Ldind_I1 = 0x46,
        Ldind_U1 = 0x47,
        Ldind_I2 = 0x48,
        Ldind_U2 = 0x49,
        Ldind_I4 = 0x4a,
        Ldind_U4 = 0x4b,
        Ldind_I8 = 0x4c,
        Ldind_I = 0x4d,
        Ldind_R4 = 0x4e,
        Ldind_R8 = 0x4f,
        Ldind_Ref = 0x50,
        Stind_Ref = 0x51,
        Stind_I1 = 0x52,
        Stind_I2 = 0x53,
        Stind_I4 = 0x54,
        Stind_I8 = 0x55,
        Stind_R4 = 0x56,
        Stind_R8 = 0x57,
        Add = 0x58,
        Sub = 0x59,
        Mul = 0x5a,
        Div = 0x5b,
        Div_Un = 0x5c,
        Rem = 0x5d,
        Rem_Un = 0x5e,
        And = 0x5f,
        Or = 0x60,
        Xor = 0x61,
        Shl = 0x62,
        Shr = 0x63,
        Shr_Un = 0x64,
        Neg = 0x65,
        Not = 0x66,
        Conv_I1 = 0x67,
        Conv_I2 = 0x68,
        Conv_I4 = 0x69,
        Conv_I8 = 0x6a,
        Conv_R4 = 0x6b,
        Conv_R8 = 0x6c,
        Conv_U4 = 0x6d,
        Conv_U8 = 0x6e,
        Callvirt = 0x6f,
        Cpobj = 0x70,
        Ldobj = 0x71,
        Ldstr = 0x72,
        Newobj = 0x73,
        Castclass = 0x74,
        Isinst = 0x75,
        Conv_R_Un = 0x76,
        Unbox = 0x79,
        Throw = 0x7a,
        Ldfld = 0x7b,
        Ldflda = 0x7c,
        Stfld = 0x7d,
        Ldsfld = 0x7e,
        Ldsflda = 0x7f,
        Stsfld = 0x80,
        Stobj = 0x81,
        Conv_Ovf_I1_Un = 0x82,
        Conv_Ovf_I2_Un = 0x83,
        Conv_Ovf_I4_Un = 0x84,
        Conv_Ovf_I8_Un = 0x85,
        Conv_Ovf_U1_Un = 0x86,
        Conv_Ovf_U2_Un = 0x87,
        Conv_Ovf_U4_Un = 0x88,
        Conv_Ovf_U8_Un = 0x89,
        Conv_Ovf_I_Un = 0x8a,
        Conv_Ovf_U_Un = 0x8b,
        Box = 0x8c,
        Newarr = 0x8d,
        Ldlen = 0x8e,
        Ldelema = 0x8f,
        Ldelem_I1 = 0x90,
        Ldelem_U1 = 0x91,
        Ldelem_I2 = 0x92,
        Ldelem_U2 = 0x93,
        Ldelem_I4 = 0x94,
        Ldelem_U4 = 0x95,
        Ldelem_I8 = 0x96,
        Ldelem_I = 0x97,
        Ldelem_R4 = 0x98,
        Ldelem_R8 = 0x99,
        Ldelem_Ref = 0x9a,
        Stelem_I = 0x9b,
        Stelem_I1 = 0x9c,
        Stelem_I2 = 0x9d,
        Stelem_I4 = 0x9e,
        Stelem_I8 = 0x9f,
        Stelem_R4 = 0xa0,
        Stelem_R8 = 0xa1,
        Stelem_Ref = 0xa2,
        Ldelem = 0xa3,
        Stelem = 0xa4,
        Unbox_Any = 0xa5,
        Conv_Ovf_I1 = 0xb3,
        Conv_Ovf_U1 = 0xb4,
        Conv_Ovf_I2 = 0xb5,
        Conv_Ovf_U2 = 0xb6,
        Conv_Ovf_I4 = 0xb7,
        Conv_Ovf_U4 = 0xb8,
        Conv_Ovf_I8 = 0xb9,
        Conv_Ovf_U8 = 0xba,
        Refanyval = 0xc2,
        Ckfinite = 0xc3,
        Mkrefany = 0xc6,
        Ldtoken = 0xd0,
        Conv_U2 = 0xd1,
        Conv_U1 = 0xd2,
        Conv_I = 0xd3,
        Conv_Ovf_I = 0xd4,
        Conv_Ovf_U = 0xd5,
        Add_Ovf = 0xd6,
        Add_Ovf_Un = 0xd7,
        Mul_Ovf = 0xd8,
        Mul_Ovf_Un = 0xd9,
        Sub_Ovf = 0xda,
        Sub_Ovf_Un = 0xdb,
        Endfinally = 0xdc,
        Leave = 0xdd,
        Leave_S = 0xde,
        Stind_I = 0xdf,
        Conv_U = 0xe0,
        Prefix7 = 0xf8, // reserved
        Prefix6 = 0xf9, // reserved
        Prefix5 = 0xfa, // reserved
        Prefix4 = 0xfb, // reserved
        Prefix3 = 0xfc, // reserved
        Prefix2 = 0xfd, // reserved
        Prefix1 = 0xfe, // first byte of every two-byte opcode
        Prefixref = 0xff, // reserved
        Arglist = 0xfe00,
        Ceq = 0xfe01,
        Cgt = 0xfe02,
        Cgt_Un = 0xfe03,
        Clt = 0xfe04,
        Clt_Un = 0xfe05,
        Ldftn = 0xfe06,
        Ldvirtftn = 0xfe07,
        Ldarg = 0xfe09,
        Ldarga = 0xfe0a,
        Starg = 0xfe0b,
        Ldloc = 0xfe0c,
        Ldloca = 0xfe0d,
        Stloc = 0xfe0e,
        Localloc = 0xfe0f,
        Endfilter = 0xfe11,
        Unaligned_ = 0xfe12,
        Volatile_ = 0xfe13,
        Tail_ = 0xfe14,
        Initobj = 0xfe15,
        Constrained_ = 0xfe16,
        Cpblk = 0xfe17,
        Initblk = 0xfe18,
        Rethrow = 0xfe1a,
        Sizeof = 0xfe1c,
        Refanytype = 0xfe1d,
        Readonly_ = 0xfe1e,

        // if you add more opcodes here, modify OpCode.Name to handle them correctly

    }

    /// <summary>
    /// Provides field representations of the Microsoft intermediate language (MSIL) instructions for emission by the
    /// <see cref="ILGenerator"/> class members.
    /// </summary>
    class OpCodes
    {

        /// <summary>
        /// Fills space if opcodes are patched. No meaningful operation is performed although a processing cycle can be
        /// consumed.
        /// </summary>
        public static readonly OpCode Nop = new(OpCodeValues.Nop,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the argument at index 0 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarg_0 = new(OpCodeValues.Ldarg_0,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the argument at index 1 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarg_1 = new(OpCodeValues.Ldarg_1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the argument at index 2 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarg_2 = new(OpCodeValues.Ldarg_2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the argument at index 3 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarg_3 = new(OpCodeValues.Ldarg_3,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at index 0 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloc_0 = new(OpCodeValues.Ldloc_0,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at index 1 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloc_1 = new(OpCodeValues.Ldloc_1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at index 2 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloc_2 = new(OpCodeValues.Ldloc_2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at index 3 onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloc_3 = new(OpCodeValues.Ldloc_3,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at
        /// index 0.
        /// </summary>
        public static readonly OpCode Stloc_0 = new(OpCodeValues.Stloc_0,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at
        /// index 1.
        /// </summary>
        public static readonly OpCode Stloc_1 = new(OpCodeValues.Stloc_1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at
        /// index 2.
        /// </summary>
        public static readonly OpCode Stloc_2 = new(OpCodeValues.Stloc_2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at
        /// index 3.
        /// </summary>
        public static readonly OpCode Stloc_3 = new(OpCodeValues.Stloc_3,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the argument (referenced by a specified short form index) onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarg_S = new(OpCodeValues.Ldarg_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads an argument address, in short form, onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarga_S = new(OpCodeValues.Ldarga_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores the value on top of the evaluation stack in the argument slot at a specified index, short form.
        /// </summary>
        public static readonly OpCode Starg_S = new(OpCodeValues.Starg_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        public static readonly OpCode Ldloc_S = new(OpCodeValues.Ldloc_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the address of the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        public static readonly OpCode Ldloca_S = new(OpCodeValues.Ldloca_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at
        /// index (short form).
        /// </summary>
        public static readonly OpCode Stloc_S = new(OpCodeValues.Stloc_S,
            ((int)OperandType.ShortInlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a null reference (type O) onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldnull = new(OpCodeValues.Ldnull,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of -1 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_M1 = new(OpCodeValues.Ldc_I4_M1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 0 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_0 = new(OpCodeValues.Ldc_I4_0,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 1 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_1 = new(OpCodeValues.Ldc_I4_1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 2 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_2 = new(OpCodeValues.Ldc_I4_2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 3 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_3 = new(OpCodeValues.Ldc_I4_3,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 4 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_4 = new(OpCodeValues.Ldc_I4_4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 5 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_5 = new(OpCodeValues.Ldc_I4_5,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 6 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_6 = new(OpCodeValues.Ldc_I4_6,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 7 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_7 = new(OpCodeValues.Ldc_I4_7,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the integer value of 8 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4_8 = new(OpCodeValues.Ldc_I4_8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the supplied int8 value onto the evaluation stack as an int32, short form.
        /// </summary>
        public static readonly OpCode Ldc_I4_S = new(OpCodeValues.Ldc_I4_S,
            ((int)OperandType.ShortInlineI) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a supplied value of type int32 onto the evaluation stack as an int32.
        /// </summary>
        public static readonly OpCode Ldc_I4 = new(OpCodeValues.Ldc_I4,
            ((int)OperandType.InlineI) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a supplied value of type int64 onto the evaluation stack as an int64.
        /// </summary>
        public static readonly OpCode Ldc_I8 = new(OpCodeValues.Ldc_I8,
            ((int)OperandType.InlineI8) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a supplied value of type float32 onto the evaluation stack as type F (float).
        /// </summary>
        public static readonly OpCode Ldc_R4 = new(OpCodeValues.Ldc_R4,
            ((int)OperandType.ShortInlineR) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr4 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a supplied value of type float64 onto the evaluation stack as type F (float).
        /// </summary>
        public static readonly OpCode Ldc_R8 = new(OpCodeValues.Ldc_R8,
            ((int)OperandType.InlineR) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation
        /// stack.
        /// </summary>
        public static readonly OpCode Dup = new(OpCodeValues.Dup,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1_push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Removes the value currently on top of the evaluation stack.
        /// </summary>
        public static readonly OpCode Pop = new(OpCodeValues.Pop,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Calls the method indicated by the passed method descriptor.
        /// </summary>
        public static readonly OpCode Call = new(OpCodeValues.Call,
            ((int)OperandType.InlineMethod) |
            ((int)FlowControl.Call << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Varpop << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Varpush << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described
        /// by a calling convention.
        /// </summary>
        public static readonly OpCode Calli = new(OpCodeValues.Calli,
            ((int)OperandType.InlineSig) |
            ((int)FlowControl.Call << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Varpop << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Varpush << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Returns from the current method, pushing a return value (if present) from the callee's evaluation stack onto
        /// the caller's evaluation stack.
        /// </summary>
        public static readonly OpCode Ret = new(OpCodeValues.Ret,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Return << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Varpop << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Unconditionally transfers control to a target instruction (short form).
        /// </summary>
        public static readonly OpCode Br_S = new(OpCodeValues.Br_S,
            ((int)OperandType.ShortInlineBrTarget) |
            ((int)FlowControl.Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Unconditionally transfers control to a target instruction.
        /// </summary>
        public static readonly OpCode Br = new(OpCodeValues.Br,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        public static readonly OpCode Brfalse = new(OpCodeValues.Brfalse,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if value is true, not null, or non-zero.
        /// </summary>
        public static readonly OpCode Brtrue = new(OpCodeValues.Brtrue,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if two values are equal.
        /// </summary>
        public static readonly OpCode Beq = new(OpCodeValues.Beq,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than or equal to the second value.
        /// </summary>
        public static readonly OpCode Bge = new(OpCodeValues.Bge,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than the second value.
        /// </summary>
        public static readonly OpCode Bgt = new(OpCodeValues.Bgt,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than or equal to the second value.
        /// </summary>
        public static readonly OpCode Ble = new(OpCodeValues.Ble,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than the second value.
        /// </summary>
        public static readonly OpCode Blt = new(OpCodeValues.Blt,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction when two unsigned integer values or unordered float values are not
        /// equal.
        /// </summary>
        public static readonly OpCode Bne_Un = new(OpCodeValues.Bne_Un,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than or equal to the second value,
        /// when comparing unsigned integer values or unordered float values.
        /// </summary>
        public static readonly OpCode Bge_Un = new(OpCodeValues.Bge_Un,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than the second value, when
        /// comparing unsigned integer values or unordered float values.
        /// </summary>
        public static readonly OpCode Bgt_Un = new(OpCodeValues.Bgt_Un,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than or equal to the second value, when
        /// comparing unsigned integer values or unordered float values.
        /// </summary>
        public static readonly OpCode Ble_Un = new(OpCodeValues.Ble_Un,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than the second value, when comparing
        /// unsigned integer values or unordered float values.
        /// </summary>
        public static readonly OpCode Blt_Un = new(OpCodeValues.Blt_Un,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Macro << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Implements a jump table.
        /// </summary>
        public static readonly OpCode Switch = new(OpCodeValues.Switch,
            ((int)OperandType.InlineSwitch) |
            ((int)FlowControl.Cond_Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_I1 = new(OpCodeValues.Ldind_I1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_U1 = new(OpCodeValues.Ldind_U1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_I2 = new(OpCodeValues.Ldind_I2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_U2 = new(OpCodeValues.Ldind_U2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type int32 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_I4 = new(OpCodeValues.Ldind_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type int64 as an int64 onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_I8 = new(OpCodeValues.Ldind_I8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type float32 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_R4 = new(OpCodeValues.Ldind_R4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr4 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads a value of type float64 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_R8 = new(OpCodeValues.Ldind_R8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads an object reference as a type O (object reference) onto the evaluation stack indirectly.
        /// </summary>
        public static readonly OpCode Ldind_Ref = new(OpCodeValues.Ldind_Ref,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores an object reference value at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_Ref = new(OpCodeValues.Stind_Ref,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type int8 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_I1 = new(OpCodeValues.Stind_I1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type int16 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_I2 = new(OpCodeValues.Stind_I2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type int32 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_I4 = new(OpCodeValues.Stind_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type int64 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_I8 = new(OpCodeValues.Stind_I8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi8 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type float32 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_R4 = new(OpCodeValues.Stind_R4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popr4 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores a value of type float64 at a supplied address.
        /// </summary>
        public static readonly OpCode Stind_R8 = new(OpCodeValues.Stind_R8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popr8 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Adds two values and pushes the result onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Add = new(OpCodeValues.Add,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Subtracts one value from another and pushes the result onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Sub = new(OpCodeValues.Sub,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Multiplies two values and pushes the result on the evaluation stack.
        /// </summary>
        public static readonly OpCode Mul = new(OpCodeValues.Mul,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Divides two values and pushes the result as a floating-point (type F) or quotient (type int32) onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Div = new(OpCodeValues.Div,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Divides two unsigned integer values and pushes the result (int32) onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Div_Un = new(OpCodeValues.Div_Un,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Divides two values and pushes the remainder onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Rem = new(OpCodeValues.Rem,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Divides two unsigned values and pushes the remainder onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Rem_Un = new(OpCodeValues.Rem_Un,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Computes the bitwise AND of two values and pushes the result onto the evaluation stack.
        /// </summary>
        public static readonly OpCode And = new(OpCodeValues.And,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Computes the bitwise OR of the two integer values on top of the stack and pushes the result onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Or = new(OpCodeValues.Or,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Xor = new(OpCodeValues.Xor,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Shifts an integer value to the left (in zeroes) by a specified number of bits, pushing the result onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Shl = new(OpCodeValues.Shl,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Shr = new(OpCodeValues.Shr,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result
        /// onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Shr_Un = new(OpCodeValues.Shr_Un,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Negates a value and pushes the result onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Neg = new(OpCodeValues.Neg,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Computes the bitwise complement of the integer value on top of the stack and pushes the result onto the
        /// evaluation stack as the same type.
        /// </summary>
        public static readonly OpCode Not = new(OpCodeValues.Not,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to int8, then extends (pads) it to int32.
        /// </summary>
        public static readonly OpCode Conv_I1 = new(OpCodeValues.Conv_I1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to int16, then extends (pads) it to int32.
        /// </summary>
        public static readonly OpCode Conv_I2 = new(OpCodeValues.Conv_I2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to int32.
        /// </summary>
        public static readonly OpCode Conv_I4 = new(OpCodeValues.Conv_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to int64.
        /// </summary>
        public static readonly OpCode Conv_I8 = new(OpCodeValues.Conv_I8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to float32.
        /// </summary>
        public static readonly OpCode Conv_R4 = new(OpCodeValues.Conv_R4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr4 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to float64.
        /// </summary>
        public static readonly OpCode Conv_R8 = new(OpCodeValues.Conv_R8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int32, and extends it to int32.
        /// </summary>
        public static readonly OpCode Conv_U4 = new(OpCodeValues.Conv_U4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int64, and extends it to int64.
        /// </summary>
        public static readonly OpCode Conv_U8 = new(OpCodeValues.Conv_U8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Callvirt = new(OpCodeValues.Callvirt,
            ((int)OperandType.InlineMethod) |
            ((int)FlowControl.Call << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Varpop << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Varpush << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Copies the value type object pointed to by an address to the top of the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldobj = new(OpCodeValues.Ldobj,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes a new object reference to a string literal stored in the metadata.
        /// </summary>
        public static readonly OpCode Ldstr = new(OpCodeValues.Ldstr,
            ((int)OperandType.InlineString) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Newobj = new(OpCodeValues.Newobj,
            ((int)OperandType.InlineMethod) |
            ((int)FlowControl.Call << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Varpop << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Attempts to cast an object passed by reference to the specified class.
        /// </summary>
        public static readonly OpCode Castclass = new(OpCodeValues.Castclass,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Tests whether an object reference (type O) is an instance of a particular class.
        /// </summary>
        public static readonly OpCode Isinst = new(OpCodeValues.Isinst,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the boxed representation of a value type to its unboxed form.
        /// </summary>
        public static readonly OpCode Unbox = new(OpCodeValues.Unbox,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Throws the exception object currently on the evaluation stack.
        /// </summary>
        public static readonly OpCode Throw = new(OpCodeValues.Throw,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Throw << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Finds the value of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldfld = new(OpCodeValues.Ldfld,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Finds the address of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldflda = new(OpCodeValues.Ldflda,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the value stored in the field of an object reference or pointer with a new value.
        /// </summary>
        public static readonly OpCode Stfld = new(OpCodeValues.Stfld,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the value of a static field onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldsfld = new(OpCodeValues.Ldsfld,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the address of a static field onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldsflda = new(OpCodeValues.Ldsflda,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the value of a static field with a value from the evaluation stack.
        /// </summary>
        public static readonly OpCode Stsfld = new(OpCodeValues.Stsfld,
            ((int)OperandType.InlineField) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Copies a value of a specified type from the evaluation stack into a supplied memory address.
        /// </summary>
        public static readonly OpCode Stobj = new(OpCodeValues.Stobj,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-2 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts a value type to an object reference (type O).
        /// </summary>
        public static readonly OpCode Box = new(OpCodeValues.Box,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type
        /// onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Newarr = new(OpCodeValues.Newarr,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldlen = new(OpCodeValues.Ldlen,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the address of the array element at a specified array index onto the top of the evaluation stack as
        /// type &amp; (managed pointer).
        /// </summary>
        public static readonly OpCode Ldelema = new(OpCodeValues.Ldelema,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type int8 at a specified array index onto the top of the evaluation stack as an
        /// int32.
        /// </summary>
        public static readonly OpCode Ldelem_I1 = new(OpCodeValues.Ldelem_I1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as
        /// an int32.
        /// </summary>
        public static readonly OpCode Ldelem_U1 = new(OpCodeValues.Ldelem_U1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type int16 at a specified array index onto the top of the evaluation stack as an
        /// int32.
        /// </summary>
        public static readonly OpCode Ldelem_I2 = new(OpCodeValues.Ldelem_I2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type unsigned int16 at a specified array index onto the top of the evaluation stack
        /// as an int32.
        /// </summary>
        public static readonly OpCode Ldelem_U2 = new(OpCodeValues.Ldelem_U2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type int32 at a specified array index onto the top of the evaluation stack as an
        /// int32.
        /// </summary>
        public static readonly OpCode Ldelem_I4 = new(OpCodeValues.Ldelem_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type int64 at a specified array index onto the top of the evaluation stack as an
        /// int64.
        /// </summary>
        public static readonly OpCode Ldelem_I8 = new(OpCodeValues.Ldelem_I8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type float32 at a specified array index onto the top of the evaluation stack as type
        /// F (float).
        /// </summary>
        public static readonly OpCode Ldelem_R4 = new(OpCodeValues.Ldelem_R4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr4 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element with type float64 at a specified array index onto the top of the evaluation stack as type
        /// F (float).
        /// </summary>
        public static readonly OpCode Ldelem_R8 = new(OpCodeValues.Ldelem_R8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushr8 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element containing an object reference at a specified array index onto the top of the evaluation
        /// stack as type O (object reference).
        /// </summary>
        public static readonly OpCode Ldelem_Ref = new(OpCodeValues.Ldelem_Ref,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushref << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the int8 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_I1 = new(OpCodeValues.Stelem_I1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the int16 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_I2 = new(OpCodeValues.Stelem_I2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the int32 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_I4 = new(OpCodeValues.Stelem_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the int64 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_I8 = new(OpCodeValues.Stelem_I8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popi8 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the float32 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_R4 = new(OpCodeValues.Stelem_R4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popr4 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the float64 value on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_R8 = new(OpCodeValues.Stelem_R8,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popr8 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the object ref value (type O) on the evaluation stack.
        /// </summary>
        public static readonly OpCode Stelem_Ref = new(OpCodeValues.Stelem_Ref,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the element at a specified array index onto the top of the evaluation stack as the type specified in
        /// the instruction.
        /// </summary>
        public static readonly OpCode Ldelem = new(OpCodeValues.Ldelem,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Replaces the array element at a given index with the value on the evaluation stack, whose type is specified
        /// in the instruction.
        /// </summary>
        public static readonly OpCode Stelem = new(OpCodeValues.Stelem,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref_popi_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed int32, throwing <see
        /// cref="System.OverflowException"/> on overflow.
        /// </summary>
        public static readonly OpCode Conv_Ovf_I4 = new(OpCodeValues.Conv_Ovf_I4,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldtoken = new(OpCodeValues.Ldtoken,
            ((int)OperandType.InlineTok) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int16, and extends it to int32.
        /// </summary>
        public static readonly OpCode Conv_U2 = new(OpCodeValues.Conv_U2,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int8, and extends it to int32.
        /// </summary>
        public static readonly OpCode Conv_U1 = new(OpCodeValues.Conv_U1,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Converts the value on top of the evaluation stack to native int.
        /// </summary>
        public static readonly OpCode Conv_I = new(OpCodeValues.Conv_I,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control from the fault or finally clause of an exception block back to the Common Language
        /// Infrastructure (CLI) exception handler.
        /// </summary>
        public static readonly OpCode Endfinally = new(OpCodeValues.Endfinally,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Return << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Exits a protected region of code, unconditionally transferring control to a specific target instruction.
        /// </summary>
        public static readonly OpCode Leave = new(OpCodeValues.Leave,
            ((int)OperandType.InlineBrTarget) |
            ((int)FlowControl.Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Exits a protected region of code, unconditionally transferring control to a target instruction (short form).
        /// </summary>
        public static readonly OpCode Leave_S = new(OpCodeValues.Leave_S,
            ((int)OperandType.ShortInlineBrTarget) |
            ((int)FlowControl.Branch << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (1 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack;
        /// otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ceq = new(OpCodeValues.Ceq,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed
        /// onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Cgt = new(OpCodeValues.Cgt,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Compares two unsigned or unordered values. If the first value is greater than the second, the integer value
        /// 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Cgt_Un = new(OpCodeValues.Cgt_Un,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto
        /// the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Clt = new(OpCodeValues.Clt,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Compares the unsigned or unordered values value1 and value2. If value1 is less than value2, then the integer
        /// value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation
        /// stack.
        /// </summary>
        public static readonly OpCode Clt_Un = new(OpCodeValues.Clt_Un,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1_pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes an unmanaged pointer (type native int) to the native code implementing a specific method onto the
        /// evaluation stack.
        /// </summary>
        public static readonly OpCode Ldftn = new(OpCodeValues.Ldftn,
            ((int)OperandType.InlineMethod) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes an unmanaged pointer (type native int) to the native code implementing a particular virtual method
        /// associated with a specified object onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldvirtftn = new(OpCodeValues.Ldvirtftn,
            ((int)OperandType.InlineMethod) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popref << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads an argument (referenced by a specified index value) onto the stack.
        /// </summary>
        public static readonly OpCode Ldarg = new(OpCodeValues.Ldarg,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads an argument address onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldarga = new(OpCodeValues.Ldarga,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Stores the value on top of the evaluation stack in the argument slot at a specified index.
        /// </summary>
        public static readonly OpCode Starg = new(OpCodeValues.Starg,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloc = new(OpCodeValues.Ldloc,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push1 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Loads the address of the local variable at a specific index onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Ldloca = new(OpCodeValues.Ldloca,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in the local variable list at a
        /// specified index.
        /// </summary>
        public static readonly OpCode Stloc = new(OpCodeValues.Stloc,
            ((int)OperandType.InlineVar) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop1 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Transfers control from the filter clause of an exception back to the Common Language Infrastructure (CLI)
        /// exception handler.
        /// </summary>
        public static readonly OpCode Endfilter = new(OpCodeValues.Endfilter,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Return << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of
        /// the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction.
        /// </summary>
        public static readonly OpCode Unaligned = new(OpCodeValues.Unaligned_,
            ((int)OperandType.ShortInlineI) |
            ((int)FlowControl.Meta << OpCode.FlowControlShift) |
            ((int)OpCodeType.Prefix << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading
        /// that location cannot be cached or that multiple stores to that location cannot be suppressed.
        /// </summary>
        public static readonly OpCode Volatile = new(OpCodeValues.Volatile_,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Meta << OpCode.FlowControlShift) |
            ((int)OpCodeType.Prefix << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Initializes each field of the value type at a specified address to a null reference or a 0 of the
        /// appropriate primitive type.
        /// </summary>
        public static readonly OpCode Initobj = new(OpCodeValues.Initobj,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Copies a specified number of bytes from a source address to a destination address.
        /// </summary>
        public static readonly OpCode Cpblk = new(OpCodeValues.Cpblk,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Popi_popi_popi << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (-3 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Rethrows the current exception.
        /// </summary>
        public static readonly OpCode Rethrow = new(OpCodeValues.Rethrow,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Throw << OpCode.FlowControlShift) |
            ((int)OpCodeType.Objmodel << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            OpCode.EndsUncondJmpBlkFlag |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Pushes the size, in bytes, of a supplied value type onto the evaluation stack.
        /// </summary>
        public static readonly OpCode Sizeof = new(OpCodeValues.Sizeof,
            ((int)OperandType.InlineType) |
            ((int)FlowControl.Next << OpCode.FlowControlShift) |
            ((int)OpCodeType.Primitive << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Pushi << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (1 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Specifies that the subsequent array address operation performs no type check at run time, and that it
        /// returns a managed pointer whose mutability is restricted.
        /// </summary>
        public static readonly OpCode Readonly = new(OpCodeValues.Readonly_,
            ((int)OperandType.InlineNone) |
            ((int)FlowControl.Meta << OpCode.FlowControlShift) |
            ((int)OpCodeType.Prefix << OpCode.OpCodeTypeShift) |
            ((int)StackBehaviour.Pop0 << OpCode.StackBehaviourPopShift) |
            ((int)StackBehaviour.Push0 << OpCode.StackBehaviourPushShift) |
            (2 << OpCode.SizeShift) |
            (0 << OpCode.StackChangeShift)
        );

        /// <summary>
        /// Returns <see langword="true"/> or <see langword="false"/> if the supplied opcode takes a single byte
        /// argument.
        /// </summary>
        /// <param name="inst">An instance of an opcode object.</param>
        /// <returns><see langword="true"/> if the opcode takes a single byte argument; otherwise, <see langword="false"/>.</returns>
        public static bool TakesSingleByteArgument(OpCode inst)
        {
            return inst.OperandType switch
            {
                OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => true,
                _ => false,
            };
        }

    }

}

#nullable restore
