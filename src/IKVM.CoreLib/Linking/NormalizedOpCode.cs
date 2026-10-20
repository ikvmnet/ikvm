/*
  Copyright (C) 2002, 2004, 2005, 2006 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/

namespace IKVM.CoreLib.Linking
{

    internal enum NormalizedOpCode : byte
    {

        /// <summary>
        /// The 'nop' opcode.
        /// </summary>
        Nop = 0,

        /// <summary>
        /// The 'aconst_null' opcode.
        /// </summary>
        AconstNull = 1,

        /// <summary>
        /// The 'iconst_m1' opcode.
        /// </summary>
        IconstM1 = 2,

        /// <summary>
        /// The 'iconst_0' opcode.
        /// </summary>
        Iconst0 = 3,

        /// <summary>
        /// The 'iconst_1' opcode.
        /// </summary>
        Iconst1 = 4,

        /// <summary>
        /// The 'iconst_2' opcode.
        /// </summary>
        Iconst2 = 5,

        /// <summary>
        /// The 'iconst_3' opcode.
        /// </summary>
        Iconst3 = 6,

        /// <summary>
        /// The 'iconst_4' opcode.
        /// </summary>
        Iconst4 = 7,

        /// <summary>
        /// The 'iconst_5' opcode.
        /// </summary>
        Iconst5 = 8,

        /// <summary>
        /// The 'lconst_0' opcode.
        /// </summary>
        Lconst0 = 9,

        /// <summary>
        /// The 'lconst_1' opcode.
        /// </summary>
        Lconst1 = 10,

        /// <summary>
        /// The 'fconst_0' opcode.
        /// </summary>
        Fconst0 = 11,

        /// <summary>
        /// The 'fconst_1' opcode.
        /// </summary>
        Fconst1 = 12,

        /// <summary>
        /// The 'fconst_2' opcode.
        /// </summary>
        Fconst2 = 13,

        /// <summary>
        /// The 'dconst_0' opcode.
        /// </summary>
        Dconst0 = 14,

        /// <summary>
        /// The 'dconst_1' opcode.
        /// </summary>
        Dconst1 = 15,

        /// <summary>
        /// The 'bipush' opcode.
        /// </summary>
        Bipush = 16,

        /// <summary>
        /// The 'sipush' opcode.
        /// </summary>
        Sipush = 17,

        /// <summary>
        /// The 'ldc' opcode.
        /// </summary>
        Ldc = 18,

        /// <summary>
        /// The 'ldc_w' opcode.
        /// </summary>
        LdcW = 19,

        /// <summary>
        /// The 'ldc2_w' opcode.
        /// </summary>
        Ldc2W = 20,

        /// <summary>
        /// The 'iload' opcode.
        /// </summary>
        Iload = 21,

        /// <summary>
        /// The 'lload' opcode.
        /// </summary>
        Lload = 22,

        /// <summary>
        /// The 'fload' opcode.
        /// </summary>
        Fload = 23,

        /// <summary>
        /// The 'dload' opcode.
        /// </summary>
        Dload = 24,

        /// <summary>
        /// The 'aload' opcode.
        /// </summary>
        Aload = 25,

        /// <summary>
        /// The 'iload_0' opcode.
        /// </summary>
        Iload0 = 26,

        /// <summary>
        /// The 'iload_1' opcode.
        /// </summary>
        Iload1 = 27,

        /// <summary>
        /// The 'iload_2' opcode.
        /// </summary>
        Iload2 = 28,

        /// <summary>
        /// The 'iload_3' opcode.
        /// </summary>
        Iload3 = 29,

        /// <summary>
        /// The 'lload_0' opcode.
        /// </summary>
        Lload0 = 30,

        /// <summary>
        /// The 'lload_1' opcode.
        /// </summary>
        Lload1 = 31,

        /// <summary>
        /// The 'lload_2' opcode.
        /// </summary>
        Lload2 = 32,

        /// <summary>
        /// The 'lload_3' opcode.
        /// </summary>
        Lload3 = 33,

        /// <summary>
        /// The 'fload_0' opcode.
        /// </summary>
        Fload0 = 34,

        /// <summary>
        /// The 'fload_1' opcode.
        /// </summary>
        Fload1 = 35,

        /// <summary>
        /// The 'fload_2' opcode.
        /// </summary>
        Fload2 = 36,

        /// <summary>
        /// The 'fload_3' opcode.
        /// </summary>
        Fload3 = 37,

        /// <summary>
        /// The 'dload_0' opcode.
        /// </summary>
        Dload0 = 38,

        /// <summary>
        /// The 'dload_1' opcode.
        /// </summary>
        Dload1 = 39,

        /// <summary>
        /// The 'dload_2' opcode.
        /// </summary>
        Dload2 = 40,

        /// <summary>
        /// The 'dload_3' opcode.
        /// </summary>
        Dload3 = 41,

        /// <summary>
        /// The 'aload_0' opcode.
        /// </summary>
        Aload0 = 42,

        /// <summary>
        /// The 'aload_1' opcode.
        /// </summary>
        Aload1 = 43,

        /// <summary>
        /// The 'aload_2' opcode.
        /// </summary>
        Aload2 = 44,

        /// <summary>
        /// The 'aload_3' opcode.
        /// </summary>
        Aload3 = 45,

        /// <summary>
        /// The 'iaload' opcode.
        /// </summary>
        Iaload = 46,

        /// <summary>
        /// The 'laload' opcode.
        /// </summary>
        Laload = 47,

        /// <summary>
        /// The 'faload' opcode.
        /// </summary>
        Faload = 48,

        /// <summary>
        /// The 'daload' opcode.
        /// </summary>
        Daload = 49,

        /// <summary>
        /// The 'aaload' opcode.
        /// </summary>
        Aaload = 50,

        /// <summary>
        /// The 'baload' opcode.
        /// </summary>
        Baload = 51,

        /// <summary>
        /// The 'caload' opcode.
        /// </summary>
        Caload = 52,

        /// <summary>
        /// The 'saload' opcode.
        /// </summary>
        Saload = 53,

        /// <summary>
        /// The 'istore' opcode.
        /// </summary>
        Istore = 54,

        /// <summary>
        /// The 'lstore' opcode.
        /// </summary>
        Lstore = 55,

        /// <summary>
        /// The 'fstore' opcode.
        /// </summary>
        Fstore = 56,

        /// <summary>
        /// The 'dstore' opcode.
        /// </summary>
        Dstore = 57,

        /// <summary>
        /// The 'astore' opcode.
        /// </summary>
        Astore = 58,

        /// <summary>
        /// The 'istore_0' opcode.
        /// </summary>
        Istore0 = 59,

        /// <summary>
        /// The 'istore_1' opcode.
        /// </summary>
        Istore1 = 60,

        /// <summary>
        /// The 'istore_2' opcode.
        /// </summary>
        Istore2 = 61,

        /// <summary>
        /// The 'istore_3' opcode.
        /// </summary>
        Istore3 = 62,

        /// <summary>
        /// The 'lstore_0' opcode.
        /// </summary>
        Lstore0 = 63,

        /// <summary>
        /// The 'lstore_1' opcode.
        /// </summary>
        Lstore1 = 64,

        /// <summary>
        /// The 'lstore_2' opcode.
        /// </summary>
        Lstore2 = 65,

        /// <summary>
        /// The 'lstore_3' opcode.
        /// </summary>
        Lstore3 = 66,

        /// <summary>
        /// The 'fstore_0' opcode.
        /// </summary>
        Fstore0 = 67,

        /// <summary>
        /// The 'fstore_1' opcode.
        /// </summary>
        Fstore1 = 68,

        /// <summary>
        /// The 'fstore_2' opcode.
        /// </summary>
        Fstore2 = 69,

        /// <summary>
        /// The 'fstore_3' opcode.
        /// </summary>
        Fstore3 = 70,

        /// <summary>
        /// The 'dstore_0' opcode.
        /// </summary>
        Dstore0 = 71,

        /// <summary>
        /// The 'dstore_1' opcode.
        /// </summary>
        Dstore1 = 72,

        /// <summary>
        /// The 'dstore_2' opcode.
        /// </summary>
        Dstore2 = 73,

        /// <summary>
        /// The 'dstore_3' opcode.
        /// </summary>
        Dstore3 = 74,

        /// <summary>
        /// The 'astore_0' opcode.
        /// </summary>
        Astore0 = 75,

        /// <summary>
        /// The 'astore_1' opcode.
        /// </summary>
        Astore1 = 76,

        /// <summary>
        /// The 'astore_2' opcode.
        /// </summary>
        Astore2 = 77,

        /// <summary>
        /// The 'astore_3' opcode.
        /// </summary>
        Astore3 = 78,

        /// <summary>
        /// The 'iastore' opcode.
        /// </summary>
        Iastore = 79,

        /// <summary>
        /// The 'lastore' opcode.
        /// </summary>
        Lastore = 80,

        /// <summary>
        /// The 'fastore' opcode.
        /// </summary>
        Fastore = 81,

        /// <summary>
        /// The 'dastore' opcode.
        /// </summary>
        Dastore = 82,

        /// <summary>
        /// The 'aastore' opcode.
        /// </summary>
        Aastore = 83,

        /// <summary>
        /// The 'bastore' opcode.
        /// </summary>
        Bastore = 84,

        /// <summary>
        /// The 'castore' opcode.
        /// </summary>
        Castore = 85,

        /// <summary>
        /// The 'sastore' opcode.
        /// </summary>
        Sastore = 86,

        /// <summary>
        /// The 'pop' opcode.
        /// </summary>
        Pop = 87,

        /// <summary>
        /// The 'pop2' opcode.
        /// </summary>
        Pop2 = 88,

        /// <summary>
        /// The 'dup' opcode.
        /// </summary>
        Dup = 89,

        /// <summary>
        /// The 'dup_x1' opcode.
        /// </summary>
        DupX1 = 90,

        /// <summary>
        /// The 'dup_x2' opcode.
        /// </summary>
        DupX2 = 91,

        /// <summary>
        /// The 'dup2' opcode.
        /// </summary>
        Dup2 = 92,

        /// <summary>
        /// The 'dup2_x1' opcode.
        /// </summary>
        Dup2X1 = 93,

        /// <summary>
        /// The 'dup2_x2' opcode.
        /// </summary>
        Dup2X2 = 94,

        /// <summary>
        /// The 'swap' opcode.
        /// </summary>
        Swap = 95,

        /// <summary>
        /// The 'iadd' opcode.
        /// </summary>
        Iadd = 96,

        /// <summary>
        /// The 'ladd' opcode.
        /// </summary>
        Ladd = 97,

        /// <summary>
        /// The 'fadd' opcode.
        /// </summary>
        Fadd = 98,

        /// <summary>
        /// The 'dadd' opcode.
        /// </summary>
        Dadd = 99,

        /// <summary>
        /// The 'isub' opcode.
        /// </summary>
        Isub = 100,

        /// <summary>
        /// The 'lsub' opcode.
        /// </summary>
        Lsub = 101,

        /// <summary>
        /// The 'fsub' opcode.
        /// </summary>
        Fsub = 102,

        /// <summary>
        /// The 'dsub' opcode.
        /// </summary>
        Dsub = 103,

        /// <summary>
        /// The 'imul' opcode.
        /// </summary>
        Imul = 104,

        /// <summary>
        /// The 'lmul' opcode.
        /// </summary>
        Lmul = 105,

        /// <summary>
        /// The 'fmul' opcode.
        /// </summary>
        Fmul = 106,

        /// <summary>
        /// The 'dmul' opcode.
        /// </summary>
        Dmul = 107,

        /// <summary>
        /// The 'idiv' opcode.
        /// </summary>
        Idiv = 108,

        /// <summary>
        /// The 'ldiv' opcode.
        /// </summary>
        Ldiv = 109,

        /// <summary>
        /// The 'fdiv' opcode.
        /// </summary>
        Fdiv = 110,

        /// <summary>
        /// The 'ddiv' opcode.
        /// </summary>
        Ddiv = 111,

        /// <summary>
        /// The 'irem' opcode.
        /// </summary>
        Irem = 112,

        /// <summary>
        /// The 'lrem' opcode.
        /// </summary>
        Lrem = 113,

        /// <summary>
        /// The 'frem' opcode.
        /// </summary>
        Frem = 114,

        /// <summary>
        /// The 'drem' opcode.
        /// </summary>
        Drem = 115,

        /// <summary>
        /// The 'ineg' opcode.
        /// </summary>
        Ineg = 116,

        /// <summary>
        /// The 'lneg' opcode.
        /// </summary>
        Lneg = 117,

        /// <summary>
        /// The 'fneg' opcode.
        /// </summary>
        Fneg = 118,

        /// <summary>
        /// The 'dneg' opcode.
        /// </summary>
        Dneg = 119,

        /// <summary>
        /// The 'ishl' opcode.
        /// </summary>
        Ishl = 120,

        /// <summary>
        /// The 'lshl' opcode.
        /// </summary>
        Lshl = 121,

        /// <summary>
        /// The 'ishr' opcode.
        /// </summary>
        Ishr = 122,

        /// <summary>
        /// The 'lshr' opcode.
        /// </summary>
        Lshr = 123,

        /// <summary>
        /// The 'iushr' opcode.
        /// </summary>
        Iushr = 124,

        /// <summary>
        /// The 'lushr' opcode.
        /// </summary>
        Lushr = 125,

        /// <summary>
        /// The 'iand' opcode.
        /// </summary>
        Iand = 126,

        /// <summary>
        /// The 'land' opcode.
        /// </summary>
        Land = 127,

        /// <summary>
        /// The 'ior' opcode.
        /// </summary>
        Ior = 128,

        /// <summary>
        /// The 'lor' opcode.
        /// </summary>
        Lor = 129,

        /// <summary>
        /// The 'ixor' opcode.
        /// </summary>
        Ixor = 130,

        /// <summary>
        /// The 'lxor' opcode.
        /// </summary>
        Lxor = 131,

        /// <summary>
        /// The 'iinc' opcode.
        /// </summary>
        Iinc = 132,

        /// <summary>
        /// The 'i2l' opcode.
        /// </summary>
        I2l = 133,

        /// <summary>
        /// The 'i2f' opcode.
        /// </summary>
        I2f = 134,

        /// <summary>
        /// The 'i2d' opcode.
        /// </summary>
        I2d = 135,

        /// <summary>
        /// The 'l2i' opcode.
        /// </summary>
        L2i = 136,

        /// <summary>
        /// The 'l2f' opcode.
        /// </summary>
        L2f = 137,

        /// <summary>
        /// The 'l2d' opcode.
        /// </summary>
        L2d = 138,

        /// <summary>
        /// The 'f2i' opcode.
        /// </summary>
        F2i = 139,

        /// <summary>
        /// The 'f2l' opcode.
        /// </summary>
        F2l = 140,

        /// <summary>
        /// The 'f2d' opcode.
        /// </summary>
        F2d = 141,

        /// <summary>
        /// The 'd2i' opcode.
        /// </summary>
        D2i = 142,

        /// <summary>
        /// The 'd2l' opcode.
        /// </summary>
        D2l = 143,

        /// <summary>
        /// The 'd2f' opcode.
        /// </summary>
        D2f = 144,

        /// <summary>
        /// The 'i2b' opcode.
        /// </summary>
        I2b = 145,

        /// <summary>
        /// The 'i2c' opcode.
        /// </summary>
        I2c = 146,

        /// <summary>
        /// The 'i2s' opcode.
        /// </summary>
        I2s = 147,

        /// <summary>
        /// The 'lcmp' opcode.
        /// </summary>
        Lcmp = 148,

        /// <summary>
        /// The 'fcmpl' opcode.
        /// </summary>
        Fcmpl = 149,

        /// <summary>
        /// The 'fcmpg' opcode.
        /// </summary>
        Fcmpg = 150,

        /// <summary>
        /// The 'dcmpl' opcode.
        /// </summary>
        Dcmpl = 151,

        /// <summary>
        /// The 'dcmpg' opcode.
        /// </summary>
        Dcmpg = 152,

        /// <summary>
        /// The 'ifeq' opcode.
        /// </summary>
        Ifeq = 153,

        /// <summary>
        /// The 'ifne' opcode.
        /// </summary>
        Ifne = 154,

        /// <summary>
        /// The 'iflt' opcode.
        /// </summary>
        Iflt = 155,

        /// <summary>
        /// The 'ifge' opcode.
        /// </summary>
        Ifge = 156,

        /// <summary>
        /// The 'ifgt' opcode.
        /// </summary>
        Ifgt = 157,

        /// <summary>
        /// The 'ifle' opcode.
        /// </summary>
        Ifle = 158,

        /// <summary>
        /// The 'if_icmpeq' opcode.
        /// </summary>
        IfIcmpeq = 159,

        /// <summary>
        /// The 'if_icmpne' opcode.
        /// </summary>
        IfIcmpne = 160,

        /// <summary>
        /// The 'if_icmplt' opcode.
        /// </summary>
        IfIcmplt = 161,

        /// <summary>
        /// The 'if_icmpge' opcode.
        /// </summary>
        IfIcmpge = 162,

        /// <summary>
        /// The 'if_icmpgt' opcode.
        /// </summary>
        IfIcmpgt = 163,

        /// <summary>
        /// The 'if_icmple' opcode.
        /// </summary>
        IfIcmple = 164,

        /// <summary>
        /// The 'if_acmpeq' opcode.
        /// </summary>
        IfAcmpeq = 165,

        /// <summary>
        /// The 'if_acmpne' opcode.
        /// </summary>
        IfAcmpne = 166,

        /// <summary>
        /// The 'goto' opcode.
        /// </summary>
        Goto = 167,

        /// <summary>
        /// The 'jsr' opcode.
        /// </summary>
        Jsr = 168,

        /// <summary>
        /// The 'ret' opcode.
        /// </summary>
        Ret = 169,

        /// <summary>
        /// The 'tableswitch' opcode.
        /// </summary>
        TableSwitch = 170,

        /// <summary>
        /// The 'lookupswitch' opcode.
        /// </summary>
        LookupSwitch = 171,

        /// <summary>
        /// The 'ireturn' opcode.
        /// </summary>
        Ireturn = 172,

        /// <summary>
        /// The 'lreturn' opcode.
        /// </summary>
        Lreturn = 173,

        /// <summary>
        /// The 'freturn' opcode.
        /// </summary>
        Freturn = 174,

        /// <summary>
        /// The 'dreturn' opcode.
        /// </summary>
        Dreturn = 175,

        /// <summary>
        /// The 'areturn' opcode.
        /// </summary>
        Areturn = 176,

        /// <summary>
        /// The 'return' opcode.
        /// </summary>
        Return = 177,

        /// <summary>
        /// The 'getstatic' opcode.
        /// </summary>
        GetStatic = 178,

        /// <summary>
        /// The 'putstatic' opcode.
        /// </summary>
        PutStatic = 179,

        /// <summary>
        /// The 'getfield' opcode.
        /// </summary>
        GetField = 180,

        /// <summary>
        /// The 'putfield' opcode.
        /// </summary>
        PutField = 181,

        /// <summary>
        /// The 'invokevirtual' opcode.
        /// </summary>
        InvokeVirtual = 182,

        /// <summary>
        /// The 'invokespecial' opcode.
        /// </summary>
        InvokeSpecial = 183,

        /// <summary>
        /// The 'invokestatic' opcode.
        /// </summary>
        InvokeStatic = 184,

        /// <summary>
        /// The 'invokeinterface' opcode.
        /// </summary>
        InvokeInterface = 185,

        /// <summary>
        /// The 'invokedynamic' opcode.
        /// </summary>
        InvokeDynamic = 186,

        /// <summary>
        /// The 'new' opcode.
        /// </summary>
        New = 187,

        /// <summary>
        /// The 'newarray' opcode.
        /// </summary>
        Newarray = 188,

        /// <summary>
        /// The 'anewarray' opcode.
        /// </summary>
        Anewarray = 189,

        /// <summary>
        /// The 'arraylength' opcode.
        /// </summary>
        Arraylength = 190,

        /// <summary>
        /// The 'athrow' opcode.
        /// </summary>
        Athrow = 191,

        /// <summary>
        /// The 'checkcast' opcode.
        /// </summary>
        Checkcast = 192,

        /// <summary>
        /// The 'instanceof' opcode.
        /// </summary>
        InstanceOf = 193,

        /// <summary>
        /// The 'monitorenter' opcode.
        /// </summary>
        MonitorEnter = 194,

        /// <summary>
        /// The 'monitorexit' opcode.
        /// </summary>
        MonitorExit = 195,

        /// <summary>
        /// The 'wide' opcode.
        /// </summary>
        Wide = 196,

        /// <summary>
        /// The 'multianewarray' opcode.
        /// </summary>
        Multianewarray = 197,

        /// <summary>
        /// The 'ifnull' opcode.
        /// </summary>
        IfNull = 198,

        /// <summary>
        /// The 'ifnonnull' opcode.
        /// </summary>
        IfNonNull = 199,

        /// <summary>
        /// The 'goto_w' opcode.
        /// </summary>
        GotoW = 200,

        /// <summary>
        /// The 'jsr_w' opcode.
        /// </summary>
        JsrW = 201,

        // This is where the pseudo-bytecodes start
        PrivilegedInvokeStatic = 235, // the privileged bytecodes are used for accessing host class members
		PrivilegedInvokeVirtual = 237,
		PrivilegedInvokeSpecial = 238,
		LdcNothrow = 239,
		MethodHandleInvoke = 240,
		MethodHandleLink = 241,
		GotoFinally = 242,
		IntrinsicGettype = 243,
		AthrowNoUnmap = 244,
		DynamicGetStatic = 245,
		DynamicPutStatic = 246,
		DynamicGetField = 247,
		DynamicPutField = 248,
		DynamicInvokeInterface = 249,
		DynamicInvokeStatic = 250,
		DynamicInvokeVirtual = 251,
		DynamicInvokeSpecial = 252,
		CloneArray = 253,
		StaticError = 254, // not a real instruction, this signals an instruction that is compiled as an exception
		Iconst = 255,

	}

}