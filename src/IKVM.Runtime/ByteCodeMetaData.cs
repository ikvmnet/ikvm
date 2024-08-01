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
    /// Describes Java opcodes and their properties.
    /// </summary>
    readonly struct ByteCodeMetaData
    {

        readonly static ByteCodeMetaData[] data = new ByteCodeMetaData[256];

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ByteCodeMetaData()
        {
            new ByteCodeMetaData(OpCode._nop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aconst_null, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_m1, NormalizedByteCode.__iconst, -1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_0, NormalizedByteCode.__iconst, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_1, NormalizedByteCode.__iconst, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_2, NormalizedByteCode.__iconst, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_3, NormalizedByteCode.__iconst, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_4, NormalizedByteCode.__iconst, 4, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iconst_5, NormalizedByteCode.__iconst, 5, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fconst_2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._bipush, NormalizedByteCode.__iconst, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._sipush, NormalizedByteCode.__iconst, ByteCodeMode.Immediate_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ldc, ByteCodeMode.Constant_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ldc_w, NormalizedByteCode._ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ldc2_w, NormalizedByteCode._ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._iload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._lload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._fload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._dload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._aload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._iload_0, NormalizedByteCode._iload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_1, NormalizedByteCode._iload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_2, NormalizedByteCode._iload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_3, NormalizedByteCode._iload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_0, NormalizedByteCode._lload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_1, NormalizedByteCode._lload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_2, NormalizedByteCode._lload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_3, NormalizedByteCode._lload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_0, NormalizedByteCode._fload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_1, NormalizedByteCode._fload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_2, NormalizedByteCode._fload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_3, NormalizedByteCode._fload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_0, NormalizedByteCode._dload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_1, NormalizedByteCode._dload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_2, NormalizedByteCode._dload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_3, NormalizedByteCode._dload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_0, NormalizedByteCode._aload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_1, NormalizedByteCode._aload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_2, NormalizedByteCode._aload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_3, NormalizedByteCode._aload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._laload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._faload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._daload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._aaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._baload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._caload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._saload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._istore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._lstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._fstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._dstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._astore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._istore_0, NormalizedByteCode._istore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_1, NormalizedByteCode._istore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_2, NormalizedByteCode._istore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_3, NormalizedByteCode._istore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_0, NormalizedByteCode._lstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_1, NormalizedByteCode._lstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_2, NormalizedByteCode._lstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_3, NormalizedByteCode._lstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_0, NormalizedByteCode._fstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_1, NormalizedByteCode._fstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_2, NormalizedByteCode._fstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_3, NormalizedByteCode._fstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_0, NormalizedByteCode._dstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_1, NormalizedByteCode._dstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_2, NormalizedByteCode._dstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_3, NormalizedByteCode._dstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_0, NormalizedByteCode._astore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_1, NormalizedByteCode._astore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_2, NormalizedByteCode._astore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_3, NormalizedByteCode._astore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._lastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._fastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._dastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._aastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._bastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._castore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._sastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._pop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._pop2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup2_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dup2_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._swap, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ladd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._isub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._imul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._idiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ldiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._fdiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ddiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._irem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._lrem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._frem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._drem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ineg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ishl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lshl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ishr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lshr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iand, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._land, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ior, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ixor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lxor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iinc, ByteCodeMode.Local_1_Immediate_1, ByteCodeModeWide.Local_2_Immediate_2, true);
            new ByteCodeMetaData(OpCode._i2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._i2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._i2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._l2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._l2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._l2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._f2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._f2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._f2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._d2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._d2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._d2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._i2b, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._i2c, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._i2s, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lcmp, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iflt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifle, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmplt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmpge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmpgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_icmple, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_acmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._if_acmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._goto, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._jsr, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ret, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._tableswitch, ByteCodeMode.Tableswitch, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lookupswitch, ByteCodeMode.Lookupswitch, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ireturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._freturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._areturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._return, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._getstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._putstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._getfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._putfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._invokevirtual, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._invokespecial, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._invokestatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._invokeinterface, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._invokedynamic, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._new, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._newarray, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._anewarray, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._arraylength, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._athrow, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._checkcast, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._instanceof, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._monitorenter, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._monitorexit, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._wide, NormalizedByteCode._nop, ByteCodeMode.WidePrefix, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._multianewarray, ByteCodeMode.Constant_2_Immediate_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ifnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifnonnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._goto_w, NormalizedByteCode._goto, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._jsr_w, NormalizedByteCode._jsr, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
        }

        readonly ByteCodeMode mode;
        readonly ByteCodeModeWide wideMode;
        readonly NormalizedByteCode normalizedOpCode;
        readonly ByteCodeFlags flags;
        readonly int arg;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="mode"></param>
        /// <param name="wideMode"></param>
        /// <param name="cannotThrow"></param>
        ByteCodeMetaData(OpCode opcode, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = (NormalizedByteCode)opcode;
            this.arg = 0;
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
        ByteCodeMetaData(OpCode opcode, NormalizedByteCode normalizedOpCode, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = normalizedOpCode;
            this.arg = 0;
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
        /// <param name="arg"></param>
        /// <param name="mode"></param>
        /// <param name="wideMode"></param>
        /// <param name="cannotThrow"></param>
        ByteCodeMetaData(OpCode opcode, NormalizedByteCode normalizedOpCode, int arg, ByteCodeMode mode, ByteCodeModeWide wideMode, bool cannotThrow)
        {
            this.mode = mode;
            this.wideMode = wideMode;
            this.normalizedOpCode = normalizedOpCode;
            this.arg = arg;
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
        internal static NormalizedByteCode GetNormalizedByteCode(OpCode opcode)
        {
            return data[(int)opcode].normalizedOpCode;
        }

        internal static int GetArg(OpCode bc, int arg)
        {
            if ((data[(int)bc].flags & ByteCodeFlags.FixedArg) != 0)
                return data[(int)bc].arg;

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
        internal static ByteCodeFlowControl GetFlowControl(NormalizedByteCode opcode)
        {
            switch (opcode)
            {
                case NormalizedByteCode._tableswitch:
                case NormalizedByteCode._lookupswitch:
                    return ByteCodeFlowControl.Switch;
                case NormalizedByteCode._goto:
                case NormalizedByteCode.__goto_finally:
                    return ByteCodeFlowControl.Branch;
                case NormalizedByteCode._ifeq:
                case NormalizedByteCode._ifne:
                case NormalizedByteCode._iflt:
                case NormalizedByteCode._ifge:
                case NormalizedByteCode._ifgt:
                case NormalizedByteCode._ifle:
                case NormalizedByteCode._if_icmpeq:
                case NormalizedByteCode._if_icmpne:
                case NormalizedByteCode._if_icmplt:
                case NormalizedByteCode._if_icmpge:
                case NormalizedByteCode._if_icmpgt:
                case NormalizedByteCode._if_icmple:
                case NormalizedByteCode._if_acmpeq:
                case NormalizedByteCode._if_acmpne:
                case NormalizedByteCode._ifnull:
                case NormalizedByteCode._ifnonnull:
                    return ByteCodeFlowControl.CondBranch;
                case NormalizedByteCode._ireturn:
                case NormalizedByteCode._lreturn:
                case NormalizedByteCode._freturn:
                case NormalizedByteCode._dreturn:
                case NormalizedByteCode._areturn:
                case NormalizedByteCode._return:
                    return ByteCodeFlowControl.Return;
                case NormalizedByteCode._athrow:
                case NormalizedByteCode.__athrow_no_unmap:
                case NormalizedByteCode.__static_error:
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
        internal static bool CanThrowException(NormalizedByteCode opcode)
        {
            switch (opcode)
            {
                case NormalizedByteCode.__dynamic_invokeinterface:
                case NormalizedByteCode.__dynamic_invokestatic:
                case NormalizedByteCode.__dynamic_invokevirtual:
                case NormalizedByteCode.__dynamic_getstatic:
                case NormalizedByteCode.__dynamic_putstatic:
                case NormalizedByteCode.__dynamic_getfield:
                case NormalizedByteCode.__dynamic_putfield:
                case NormalizedByteCode.__clone_array:
                case NormalizedByteCode.__static_error:
                case NormalizedByteCode.__methodhandle_invoke:
                case NormalizedByteCode.__methodhandle_link:
                    return true;
                case NormalizedByteCode.__iconst:
                case NormalizedByteCode.__ldc_nothrow:
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
        internal static bool IsBranch(NormalizedByteCode opcode)
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
