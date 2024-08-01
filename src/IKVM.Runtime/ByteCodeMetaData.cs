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

    struct ByteCodeMetaData
    {

        static ByteCodeMetaData[] data = new ByteCodeMetaData[256];
        ByteCodeMode reg;
        ByteCodeModeWide wide;
        NormalizedByteCode normbc;
        ByteCodeFlags flags;
        int arg;

        private ByteCodeMetaData(OpCode bc, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
        {
            this.reg = reg;
            this.wide = wide;
            this.normbc = (NormalizedByteCode)bc;
            this.arg = 0;
            this.flags = ByteCodeFlags.None;
            if (cannotThrow)
            {
                this.flags |= ByteCodeFlags.CannotThrow;
            }
            data[(int)bc] = this;
        }

        private ByteCodeMetaData(OpCode bc, NormalizedByteCode normbc, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
        {
            this.reg = reg;
            this.wide = wide;
            this.normbc = normbc;
            this.arg = 0;
            this.flags = ByteCodeFlags.None;
            if (cannotThrow)
            {
                this.flags |= ByteCodeFlags.CannotThrow;
            }
            data[(int)bc] = this;
        }

        private ByteCodeMetaData(OpCode bc, NormalizedByteCode normbc, int arg, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
        {
            this.reg = reg;
            this.wide = wide;
            this.normbc = normbc;
            this.arg = arg;
            this.flags = ByteCodeFlags.FixedArg;
            if (cannotThrow)
            {
                this.flags |= ByteCodeFlags.CannotThrow;
            }
            data[(int)bc] = this;
        }

        internal static NormalizedByteCode GetNormalizedByteCode(OpCode bc)
        {
            return data[(int)bc].normbc;
        }

        internal static int GetArg(OpCode bc, int arg)
        {
            if ((data[(int)bc].flags & ByteCodeFlags.FixedArg) != 0)
            {
                return data[(int)bc].arg;
            }
            return arg;
        }

        internal static ByteCodeMode GetMode(OpCode bc)
        {
            return data[(int)bc].reg;
        }

        internal static ByteCodeModeWide GetWideMode(OpCode bc)
        {
            return data[(int)bc].wide;
        }

        internal static ByteCodeFlowControl GetFlowControl(NormalizedByteCode bc)
        {
            switch (bc)
            {
                case NormalizedByteCode.__tableswitch:
                case NormalizedByteCode.__lookupswitch:
                    return ByteCodeFlowControl.Switch;

                case NormalizedByteCode.__goto:
                case NormalizedByteCode.__goto_finally:
                    return ByteCodeFlowControl.Branch;

                case NormalizedByteCode.__ifeq:
                case NormalizedByteCode.__ifne:
                case NormalizedByteCode.__iflt:
                case NormalizedByteCode.__ifge:
                case NormalizedByteCode.__ifgt:
                case NormalizedByteCode.__ifle:
                case NormalizedByteCode.__if_icmpeq:
                case NormalizedByteCode.__if_icmpne:
                case NormalizedByteCode.__if_icmplt:
                case NormalizedByteCode.__if_icmpge:
                case NormalizedByteCode.__if_icmpgt:
                case NormalizedByteCode.__if_icmple:
                case NormalizedByteCode.__if_acmpeq:
                case NormalizedByteCode.__if_acmpne:
                case NormalizedByteCode.__ifnull:
                case NormalizedByteCode.__ifnonnull:
                    return ByteCodeFlowControl.CondBranch;

                case NormalizedByteCode.__ireturn:
                case NormalizedByteCode.__lreturn:
                case NormalizedByteCode.__freturn:
                case NormalizedByteCode.__dreturn:
                case NormalizedByteCode.__areturn:
                case NormalizedByteCode.__return:
                    return ByteCodeFlowControl.Return;

                case NormalizedByteCode.__athrow:
                case NormalizedByteCode.__athrow_no_unmap:
                case NormalizedByteCode.__static_error:
                    return ByteCodeFlowControl.Throw;

                default:
                    return ByteCodeFlowControl.Next;
            }
        }

        internal static bool CanThrowException(NormalizedByteCode bc)
        {
            switch (bc)
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
                    return (data[(int)bc].flags & ByteCodeFlags.CannotThrow) == 0;
            }
        }

        internal static bool IsBranch(NormalizedByteCode bc)
        {
            switch (data[(int)bc].reg)
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
            new ByteCodeMetaData(OpCode._ldc_w, NormalizedByteCode.__ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ldc2_w, NormalizedByteCode.__ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._iload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._lload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._fload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._dload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._aload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(OpCode._iload_0, NormalizedByteCode.__iload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_1, NormalizedByteCode.__iload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_2, NormalizedByteCode.__iload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._iload_3, NormalizedByteCode.__iload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_0, NormalizedByteCode.__lload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_1, NormalizedByteCode.__lload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_2, NormalizedByteCode.__lload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lload_3, NormalizedByteCode.__lload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_0, NormalizedByteCode.__fload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_1, NormalizedByteCode.__fload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_2, NormalizedByteCode.__fload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fload_3, NormalizedByteCode.__fload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_0, NormalizedByteCode.__dload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_1, NormalizedByteCode.__dload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_2, NormalizedByteCode.__dload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dload_3, NormalizedByteCode.__dload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_0, NormalizedByteCode.__aload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_1, NormalizedByteCode.__aload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_2, NormalizedByteCode.__aload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._aload_3, NormalizedByteCode.__aload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
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
            new ByteCodeMetaData(OpCode._istore_0, NormalizedByteCode.__istore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_1, NormalizedByteCode.__istore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_2, NormalizedByteCode.__istore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._istore_3, NormalizedByteCode.__istore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_0, NormalizedByteCode.__lstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_1, NormalizedByteCode.__lstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_2, NormalizedByteCode.__lstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._lstore_3, NormalizedByteCode.__lstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_0, NormalizedByteCode.__fstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_1, NormalizedByteCode.__fstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_2, NormalizedByteCode.__fstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._fstore_3, NormalizedByteCode.__fstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_0, NormalizedByteCode.__dstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_1, NormalizedByteCode.__dstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_2, NormalizedByteCode.__dstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._dstore_3, NormalizedByteCode.__dstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_0, NormalizedByteCode.__astore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_1, NormalizedByteCode.__astore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_2, NormalizedByteCode.__astore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._astore_3, NormalizedByteCode.__astore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
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
            new ByteCodeMetaData(OpCode._wide, NormalizedByteCode.__nop, ByteCodeMode.WidePrefix, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._multianewarray, ByteCodeMode.Constant_2_Immediate_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(OpCode._ifnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._ifnonnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._goto_w, NormalizedByteCode.__goto, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(OpCode._jsr_w, NormalizedByteCode.__jsr, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);

        }

    }

}
