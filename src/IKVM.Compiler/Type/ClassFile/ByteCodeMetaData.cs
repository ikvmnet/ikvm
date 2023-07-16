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


namespace IKVM.Compiler.Type.ClassFile
{

    struct ByteCodeMetaData
    {

        private static ByteCodeMetaData[] data = new ByteCodeMetaData[256];
        private ByteCodeMode reg;
        private ByteCodeModeWide wide;
        private NormalizedByteCode normbc;
        private ByteCodeFlags flags;
        private int arg;

        private ByteCodeMetaData(ByteCode bc, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
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

        private ByteCodeMetaData(ByteCode bc, NormalizedByteCode normbc, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
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

        private ByteCodeMetaData(ByteCode bc, NormalizedByteCode normbc, int arg, ByteCodeMode reg, ByteCodeModeWide wide, bool cannotThrow)
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

        internal static NormalizedByteCode GetNormalizedByteCode(ByteCode bc)
        {
            return data[(int)bc].normbc;
        }

        internal static int GetArg(ByteCode bc, int arg)
        {
            if ((data[(int)bc].flags & ByteCodeFlags.FixedArg) != 0)
            {
                return data[(int)bc].arg;
            }
            return arg;
        }

        internal static ByteCodeMode GetMode(ByteCode bc)
        {
            return data[(int)bc].reg;
        }

        internal static ByteCodeModeWide GetWideMode(ByteCode bc)
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
            new ByteCodeMetaData(ByteCode.__nop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__aconst_null, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_m1, NormalizedByteCode.__iconst, -1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_0, NormalizedByteCode.__iconst, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_1, NormalizedByteCode.__iconst, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_2, NormalizedByteCode.__iconst, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_3, NormalizedByteCode.__iconst, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_4, NormalizedByteCode.__iconst, 4, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iconst_5, NormalizedByteCode.__iconst, 5, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fconst_2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dconst_0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dconst_1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__bipush, NormalizedByteCode.__iconst, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__sipush, NormalizedByteCode.__iconst, ByteCodeMode.Immediate_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ldc, ByteCodeMode.Constant_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__ldc_w, NormalizedByteCode.__ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__ldc2_w, NormalizedByteCode.__ldc, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__iload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__lload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__fload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__dload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__aload, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__iload_0, NormalizedByteCode.__iload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iload_1, NormalizedByteCode.__iload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iload_2, NormalizedByteCode.__iload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iload_3, NormalizedByteCode.__iload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lload_0, NormalizedByteCode.__lload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lload_1, NormalizedByteCode.__lload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lload_2, NormalizedByteCode.__lload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lload_3, NormalizedByteCode.__lload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fload_0, NormalizedByteCode.__fload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fload_1, NormalizedByteCode.__fload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fload_2, NormalizedByteCode.__fload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fload_3, NormalizedByteCode.__fload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dload_0, NormalizedByteCode.__dload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dload_1, NormalizedByteCode.__dload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dload_2, NormalizedByteCode.__dload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dload_3, NormalizedByteCode.__dload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__aload_0, NormalizedByteCode.__aload, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__aload_1, NormalizedByteCode.__aload, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__aload_2, NormalizedByteCode.__aload, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__aload_3, NormalizedByteCode.__aload, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__laload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__faload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__daload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__aaload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__baload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__caload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__saload, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__istore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__lstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__fstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__dstore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__astore, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__istore_0, NormalizedByteCode.__istore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__istore_1, NormalizedByteCode.__istore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__istore_2, NormalizedByteCode.__istore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__istore_3, NormalizedByteCode.__istore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lstore_0, NormalizedByteCode.__lstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lstore_1, NormalizedByteCode.__lstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lstore_2, NormalizedByteCode.__lstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lstore_3, NormalizedByteCode.__lstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fstore_0, NormalizedByteCode.__fstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fstore_1, NormalizedByteCode.__fstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fstore_2, NormalizedByteCode.__fstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fstore_3, NormalizedByteCode.__fstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dstore_0, NormalizedByteCode.__dstore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dstore_1, NormalizedByteCode.__dstore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dstore_2, NormalizedByteCode.__dstore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dstore_3, NormalizedByteCode.__dstore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__astore_0, NormalizedByteCode.__astore, 0, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__astore_1, NormalizedByteCode.__astore, 1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__astore_2, NormalizedByteCode.__astore, 2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__astore_3, NormalizedByteCode.__astore, 3, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__lastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__fastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__dastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__aastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__bastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__castore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__sastore, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__pop, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__pop2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup2_x1, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dup2_x2, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__swap, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ladd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dadd, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__isub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dsub, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__imul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dmul, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__idiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__ldiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__fdiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ddiv, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__irem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__lrem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__frem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__drem, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ineg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dneg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ishl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lshl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ishr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lshr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lushr, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iand, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__land, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ior, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ixor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lxor, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iinc, ByteCodeMode.Local_1_Immediate_1, ByteCodeModeWide.Local_2_Immediate_2, true);
            new ByteCodeMetaData(ByteCode.__i2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__i2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__i2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__l2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__l2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__l2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__f2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__f2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__f2d, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__d2i, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__d2l, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__d2f, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__i2b, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__i2c, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__i2s, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lcmp, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__fcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dcmpl, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dcmpg, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__iflt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifle, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmplt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmpge, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmpgt, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_icmple, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_acmpeq, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__if_acmpne, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__goto, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__jsr, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ret, ByteCodeMode.Local_1, ByteCodeModeWide.Local_2, true);
            new ByteCodeMetaData(ByteCode.__tableswitch, ByteCodeMode.Tableswitch, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lookupswitch, ByteCodeMode.Lookupswitch, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ireturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__lreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__freturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__dreturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__areturn, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__return, ByteCodeMode.Simple, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__getstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__putstatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__getfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__putfield, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__invokevirtual, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__invokespecial, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__invokestatic, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__invokeinterface, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__invokedynamic, ByteCodeMode.Constant_2_1_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__new, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__newarray, ByteCodeMode.Immediate_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__anewarray, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__arraylength, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__athrow, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__checkcast, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__instanceof, ByteCodeMode.Constant_2, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__monitorenter, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__monitorexit, ByteCodeMode.Simple, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__wide, NormalizedByteCode.__nop, ByteCodeMode.WidePrefix, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__multianewarray, ByteCodeMode.Constant_2_Immediate_1, ByteCodeModeWide.Unused, false);
            new ByteCodeMetaData(ByteCode.__ifnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__ifnonnull, ByteCodeMode.Branch_2, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__goto_w, NormalizedByteCode.__goto, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);
            new ByteCodeMetaData(ByteCode.__jsr_w, NormalizedByteCode.__jsr, ByteCodeMode.Branch_4, ByteCodeModeWide.Unused, true);

        }

    }

}