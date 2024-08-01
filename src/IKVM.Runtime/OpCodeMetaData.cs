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

using IKVM.ByteCode;

namespace IKVM.Runtime
{

    /// <summary>
    /// Describes Java opcodes and their metadata.
    /// </summary>
    readonly struct OpCodeMetaData
    {

        readonly static OpCodeMetaData[] data = new OpCodeMetaData[256];

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static OpCodeMetaData()
        {
            new OpCodeMetaData(OpCode._nop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._aconst_null, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_m1, NormalizedOpCode.__iconst, -1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_0, NormalizedOpCode.__iconst, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_1, NormalizedOpCode.__iconst, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_2, NormalizedOpCode.__iconst, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_3, NormalizedOpCode.__iconst, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_4, NormalizedOpCode.__iconst, 4, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iconst_5, NormalizedOpCode.__iconst, 5, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fconst_2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._bipush, NormalizedOpCode.__iconst, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._sipush, NormalizedOpCode.__iconst, ByteCodeMode.Immediate_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ldc, ByteCodeMode.Constant_1, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._ldc_w, NormalizedOpCode._ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._ldc2_w, NormalizedOpCode._ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._iload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._lload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._fload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._dload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._aload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._iload_0, NormalizedOpCode._iload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iload_1, NormalizedOpCode._iload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iload_2, NormalizedOpCode._iload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iload_3, NormalizedOpCode._iload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lload_0, NormalizedOpCode._lload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lload_1, NormalizedOpCode._lload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lload_2, NormalizedOpCode._lload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lload_3, NormalizedOpCode._lload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fload_0, NormalizedOpCode._fload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fload_1, NormalizedOpCode._fload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fload_2, NormalizedOpCode._fload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fload_3, NormalizedOpCode._fload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dload_0, NormalizedOpCode._dload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dload_1, NormalizedOpCode._dload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dload_2, NormalizedOpCode._dload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dload_3, NormalizedOpCode._dload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._aload_0, NormalizedOpCode._aload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._aload_1, NormalizedOpCode._aload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._aload_2, NormalizedOpCode._aload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._aload_3, NormalizedOpCode._aload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._laload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._faload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._daload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._aaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._baload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._caload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._saload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._istore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._lstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._fstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._dstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._astore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._istore_0, NormalizedOpCode._istore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._istore_1, NormalizedOpCode._istore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._istore_2, NormalizedOpCode._istore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._istore_3, NormalizedOpCode._istore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lstore_0, NormalizedOpCode._lstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lstore_1, NormalizedOpCode._lstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lstore_2, NormalizedOpCode._lstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lstore_3, NormalizedOpCode._lstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fstore_0, NormalizedOpCode._fstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fstore_1, NormalizedOpCode._fstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fstore_2, NormalizedOpCode._fstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fstore_3, NormalizedOpCode._fstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dstore_0, NormalizedOpCode._dstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dstore_1, NormalizedOpCode._dstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dstore_2, NormalizedOpCode._dstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dstore_3, NormalizedOpCode._dstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._astore_0, NormalizedOpCode._astore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._astore_1, NormalizedOpCode._astore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._astore_2, NormalizedOpCode._astore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._astore_3, NormalizedOpCode._astore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._lastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._fastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._dastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._aastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._bastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._castore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._sastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._pop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._pop2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup2_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dup2_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._swap, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ladd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._isub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._imul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._idiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._ldiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._fdiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ddiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._irem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._lrem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._frem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._drem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ineg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ishl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lshl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ishr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lshr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iand, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._land, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ior, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ixor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lxor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iinc, ByteCodeMode.Local_1_Immediate_1, ByteCodeModeWide.Local_2_Immediate_2, true);
            new OpCodeMetaData(OpCode._i2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._i2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._i2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._l2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._l2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._l2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._f2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._f2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._f2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._d2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._d2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._d2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._i2b, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._i2c, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._i2s, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lcmp, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._fcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._iflt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifle, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmplt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmpge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmpgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_icmple, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_acmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._if_acmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._goto, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._jsr, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ret, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new OpCodeMetaData(OpCode._tableswitch, ByteCodeMode.Tableswitch, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lookupswitch, ByteCodeMode.Lookupswitch, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ireturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._lreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._freturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._dreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._areturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._return, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._getstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._putstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._getfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._putfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._invokevirtual, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._invokespecial, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._invokestatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._invokeinterface, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._invokedynamic, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._new, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._newarray, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._anewarray, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._arraylength, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._athrow, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._checkcast, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._instanceof, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._monitorenter, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._monitorexit, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._wide, NormalizedOpCode._nop, ByteCodeMode.WidePrefix, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._multianewarray, ByteCodeMode.Constant_2_Immediate_1, ByteCodeModeWide.Unused, false);
            new OpCodeMetaData(OpCode._ifnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._ifnonnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._goto_w, NormalizedOpCode._goto, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
            new OpCodeMetaData(OpCode._jsr_w, NormalizedOpCode._jsr, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
        }

        readonly ByteCodeMode mode;
        readonly ByteCodeModeWide wideMode;
        readonly NormalizedOpCode normalizedOpCode;
        readonly ByteCodeFlags flags;
        readonly int fixedArg;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="mode"></param>
        /// <param name="wideMode"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = (NormalizedOpCode)opcode;
            this.fixedArg = 0;
            this.flags = ByteCodeFlags.None;
            if (cannotThrow)
                this.flags |= ByteCodeFlags.CannotThrow;

            data[(int)opcode] = this;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="normalizedOpCode"></param>
        /// <param name="mode"></param>
        /// <param name="wideMode"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, NormalizedOpCode normalizedOpCode, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = normalizedOpCode;
            this.fixedArg = 0;
            this.flags = ByteCodeFlags.None;
            if (cannotThrow)
                this.flags |= ByteCodeFlags.CannotThrow;

            data[(int)opcode] = this;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="normalizedOpCode"></param>
        /// <param name="fixedArg"></param>
        /// <param name="mode"></param>
        /// <param name="wideMode"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, NormalizedOpCode normalizedOpCode, int fixedArg, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = normalizedOpCode;
            this.fixedArg = fixedArg;
            this.flags = ByteCodeFlags.FixedArg;
            if (cannotThrow)
                this.flags |= ByteCodeFlags.CannotThrow;

            data[(int)opcode] = this;
        }

        /// <summary>
        /// Gets the associated normalized opcode for the specified opcode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static NormalizedOpCode GetNormalizedByteCode(OpCode opcode)
        {
            return data[(int)opcode].normalizedOpCode;
        }

        internal static int GetArg(OpCode opcode, int arg)
        {
            if ((data[(int)opcode].flags & ByteCodeFlags.FixedArg) != 0)
                return data[(int)opcode].fixedArg;

            return arg;
        }

        /// <summary>
        /// Gets the mode of the specified opcode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static ByteCodeMode GetMode(OpCode opcode)
        {
            return data[(int)opcode].mode;
        }

        /// <summary>
        /// Gets the wide-mode of the specified opcode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static ByteCodeModeWide GetWideMode(OpCode opcode)
        {
            return data[(int)opcode].wideMode;
        }

        /// <summary>
        /// Gets the flow control property of the specified opcode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static ByteCodeFlowControl GetFlowControl(NormalizedOpCode opcode)
        {
            switch (opcode)
            {
                case NormalizedOpCode._tableswitch:
                case NormalizedOpCode._lookupswitch:
                    return ByteCodeFlowControl.Switch;
                case NormalizedOpCode._goto:
                case NormalizedOpCode.__goto_finally:
                    return ByteCodeFlowControl.Branch;
                case NormalizedOpCode._ifeq:
                case NormalizedOpCode._ifne:
                case NormalizedOpCode._iflt:
                case NormalizedOpCode._ifge:
                case NormalizedOpCode._ifgt:
                case NormalizedOpCode._ifle:
                case NormalizedOpCode._if_icmpeq:
                case NormalizedOpCode._if_icmpne:
                case NormalizedOpCode._if_icmplt:
                case NormalizedOpCode._if_icmpge:
                case NormalizedOpCode._if_icmpgt:
                case NormalizedOpCode._if_icmple:
                case NormalizedOpCode._if_acmpeq:
                case NormalizedOpCode._if_acmpne:
                case NormalizedOpCode._ifnull:
                case NormalizedOpCode._ifnonnull:
                    return ByteCodeFlowControl.CondBranch;
                case NormalizedOpCode._ireturn:
                case NormalizedOpCode._lreturn:
                case NormalizedOpCode._freturn:
                case NormalizedOpCode._dreturn:
                case NormalizedOpCode._areturn:
                case NormalizedOpCode._return:
                    return ByteCodeFlowControl.Return;
                case NormalizedOpCode._athrow:
                case NormalizedOpCode.__athrow_no_unmap:
                case NormalizedOpCode.__static_error:
                    return ByteCodeFlowControl.Throw;
                default:
                    return ByteCodeFlowControl.Next;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the specified opcode can throw an exception.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static bool CanThrowException(NormalizedOpCode opcode)
        {
            switch (opcode)
            {
                case NormalizedOpCode.__dynamic_invokeinterface:
                case NormalizedOpCode.__dynamic_invokestatic:
                case NormalizedOpCode.__dynamic_invokevirtual:
                case NormalizedOpCode.__dynamic_getstatic:
                case NormalizedOpCode.__dynamic_putstatic:
                case NormalizedOpCode.__dynamic_getfield:
                case NormalizedOpCode.__dynamic_putfield:
                case NormalizedOpCode.__clone_array:
                case NormalizedOpCode.__static_error:
                case NormalizedOpCode.__methodhandle_invoke:
                case NormalizedOpCode.__methodhandle_link:
                    return true;
                case NormalizedOpCode.__iconst:
                case NormalizedOpCode.__ldc_nothrow:
                    return false;
                default:
                    return (data[(int)opcode].flags & ByteCodeFlags.CannotThrow) == 0;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the specified opcode is a branch.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        internal static bool IsBranch(NormalizedOpCode opcode)
        {
            switch (data[(int)opcode].mode)
            {
                case ByteCodeMode.Branch_2:
                case ByteCodeMode.Branch_4:
                case ByteCodeMode.Lookupswitch:
                case ByteCodeMode.Tableswitch:
                    return true;
                default:
                    return false;
            }
        }

    }

}
