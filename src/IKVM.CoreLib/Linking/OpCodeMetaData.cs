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

namespace IKVM.CoreLib.Linking
{

    internal readonly struct OpCodeMetaData
    {

        readonly static OpCodeMetaData[] _data = new OpCodeMetaData[256];

        readonly NormalizedOpCode _normalizedOpCode;
        readonly OpCodeArgumentLayout _argLayout;
        readonly OpCodeFlags _flags;
        readonly int _arg0Value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="argLayout"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, OpCodeArgumentLayout argLayout, bool cannotThrow)
        {
            _normalizedOpCode = (NormalizedOpCode)opcode;
            _argLayout = argLayout;
            _arg0Value = 0;
            _flags = OpCodeFlags.None;
            if (cannotThrow)
                _flags |= OpCodeFlags.CannotThrow;

            _data[(int)opcode] = this;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="normbc"></param>
        /// <param name="argLayout"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, NormalizedOpCode normbc, OpCodeArgumentLayout argLayout, bool cannotThrow)
        {
            _normalizedOpCode = normbc;
            _argLayout = argLayout;
            _arg0Value = 0;
            _flags = OpCodeFlags.None;
            if (cannotThrow)
                _flags |= OpCodeFlags.CannotThrow;

            _data[(int)opcode] = this;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="opcode"></param>
        /// <param name="normbc"></param>
        /// <param name="arg0Value"></param>
        /// <param name="argLayout"></param>
        /// <param name="cannotThrow"></param>
        OpCodeMetaData(OpCode opcode, NormalizedOpCode normbc, int arg0Value, OpCodeArgumentLayout argLayout, bool cannotThrow)
        {
            _normalizedOpCode = normbc;
            _argLayout = argLayout;
            _arg0Value = arg0Value;
            _flags = OpCodeFlags.FixedArg;
            if (cannotThrow)
                _flags |= OpCodeFlags.CannotThrow;

            _data[(int)opcode] = this;
        }

        internal static NormalizedOpCode GetNormalizedByteCode(OpCode opcode)
        {
            return _data[(int)opcode]._normalizedOpCode;
        }

        internal static int GetArg0Value(OpCode bc, int arg)
        {
            if ((_data[(int)bc]._flags & OpCodeFlags.FixedArg) != 0)
                return _data[(int)bc]._arg0Value;

            return arg;
        }

        internal static OpCodeFlowControl GetFlowKind(NormalizedOpCode opcode)
        {
            switch (opcode)
            {
                case NormalizedOpCode.TableSwitch:
                case NormalizedOpCode.LookupSwitch:
                    return OpCodeFlowControl.Switch;

                case NormalizedOpCode.Goto:
                case NormalizedOpCode.GotoFinally:
                    return OpCodeFlowControl.Branch;

                case NormalizedOpCode.Ifeq:
                case NormalizedOpCode.Ifne:
                case NormalizedOpCode.Iflt:
                case NormalizedOpCode.Ifge:
                case NormalizedOpCode.Ifgt:
                case NormalizedOpCode.Ifle:
                case NormalizedOpCode.IfIcmpeq:
                case NormalizedOpCode.IfIcmpne:
                case NormalizedOpCode.IfIcmplt:
                case NormalizedOpCode.IfIcmpge:
                case NormalizedOpCode.IfIcmpgt:
                case NormalizedOpCode.IfIcmple:
                case NormalizedOpCode.IfAcmpeq:
                case NormalizedOpCode.IfAcmpne:
                case NormalizedOpCode.IfNull:
                case NormalizedOpCode.IfNonNull:
                    return OpCodeFlowControl.ConditionalBranch;

                case NormalizedOpCode.Ireturn:
                case NormalizedOpCode.Lreturn:
                case NormalizedOpCode.Freturn:
                case NormalizedOpCode.Dreturn:
                case NormalizedOpCode.Areturn:
                case NormalizedOpCode.Return:
                    return OpCodeFlowControl.Return;

                case NormalizedOpCode.Athrow:
                case NormalizedOpCode.AthrowNoUnmap:
                case NormalizedOpCode.StaticError:
                    return OpCodeFlowControl.Throw;

                default:
                    return OpCodeFlowControl.Next;
            }
        }

        internal static bool CanThrowException(NormalizedOpCode opcode)
        {
            switch (opcode)
            {
                case NormalizedOpCode.DynamicInvokeInterface:
                case NormalizedOpCode.DynamicInvokeStatic:
                case NormalizedOpCode.DynamicInvokeVirtual:
                case NormalizedOpCode.DynamicGetStatic:
                case NormalizedOpCode.DynamicPutStatic:
                case NormalizedOpCode.DynamicGetField:
                case NormalizedOpCode.DynamicPutField:
                case NormalizedOpCode.CloneArray:
                case NormalizedOpCode.StaticError:
                case NormalizedOpCode.MethodHandleInvoke:
                case NormalizedOpCode.MethodHandleLink:
                    return true;
                case NormalizedOpCode.Iconst:
                case NormalizedOpCode.LdcNothrow:
                    return false;
                default:
                    return (_data[(int)opcode]._flags & OpCodeFlags.CannotThrow) == 0;
            }
        }

        internal static bool IsBranch(NormalizedOpCode opcode)
        {
            switch (_data[(int)opcode]._argLayout)
            {
                case OpCodeArgumentLayout.Branch2:
                case OpCodeArgumentLayout.Branch4:
                case OpCodeArgumentLayout.LookupSwitch:
                case OpCodeArgumentLayout.TableSwitch:
                    return true;
                default:
                    return false;
            }
        }

        static OpCodeMetaData()
        {
            new OpCodeMetaData(OpCode.Nop, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.AconstNull, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.IconstM1, NormalizedOpCode.Iconst, -1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst0, NormalizedOpCode.Iconst, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst1, NormalizedOpCode.Iconst, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst2, NormalizedOpCode.Iconst, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst3, NormalizedOpCode.Iconst, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst4, NormalizedOpCode.Iconst, 4, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iconst5, NormalizedOpCode.Iconst, 5, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lconst0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lconst1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fconst0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fconst1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fconst2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dconst0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dconst1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Bipush, NormalizedOpCode.Iconst, OpCodeArgumentLayout.ImmediateS1, true);
            new OpCodeMetaData(OpCode.Sipush, NormalizedOpCode.Iconst, OpCodeArgumentLayout.ImmediateS2, true);
            new OpCodeMetaData(OpCode.Ldc, OpCodeArgumentLayout.Constant1, false);
            new OpCodeMetaData(OpCode.LdcW, NormalizedOpCode.Ldc, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.Ldc2W, NormalizedOpCode.Ldc, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.Iload, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Lload, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Fload, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Dload, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Aload, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Iload0, NormalizedOpCode.Iload, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iload1, NormalizedOpCode.Iload, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iload2, NormalizedOpCode.Iload, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iload3, NormalizedOpCode.Iload, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lload0, NormalizedOpCode.Lload, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lload1, NormalizedOpCode.Lload, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lload2, NormalizedOpCode.Lload, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lload3, NormalizedOpCode.Lload, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fload0, NormalizedOpCode.Fload, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fload1, NormalizedOpCode.Fload, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fload2, NormalizedOpCode.Fload, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fload3, NormalizedOpCode.Fload, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dload0, NormalizedOpCode.Dload, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dload1, NormalizedOpCode.Dload, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dload2, NormalizedOpCode.Dload, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dload3, NormalizedOpCode.Dload, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Aload0, NormalizedOpCode.Aload, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Aload1, NormalizedOpCode.Aload, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Aload2, NormalizedOpCode.Aload, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Aload3, NormalizedOpCode.Aload, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iaload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Laload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Faload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Daload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Aaload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Baload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Caload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Saload, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Istore, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Lstore, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Fstore, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Dstore, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Astore, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.Istore0, NormalizedOpCode.Istore, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Istore1, NormalizedOpCode.Istore, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Istore2, NormalizedOpCode.Istore, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Istore3, NormalizedOpCode.Istore, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lstore0, NormalizedOpCode.Lstore, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lstore1, NormalizedOpCode.Lstore, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lstore2, NormalizedOpCode.Lstore, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lstore3, NormalizedOpCode.Lstore, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fstore0, NormalizedOpCode.Fstore, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fstore1, NormalizedOpCode.Fstore, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fstore2, NormalizedOpCode.Fstore, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fstore3, NormalizedOpCode.Fstore, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dstore0, NormalizedOpCode.Dstore, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dstore1, NormalizedOpCode.Dstore, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dstore2, NormalizedOpCode.Dstore, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dstore3, NormalizedOpCode.Dstore, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Astore0, NormalizedOpCode.Astore, 0, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Astore1, NormalizedOpCode.Astore, 1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Astore2, NormalizedOpCode.Astore, 2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Astore3, NormalizedOpCode.Astore, 3, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Lastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Fastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Dastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Aastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Bastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Castore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Sastore, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Pop, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Pop2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dup, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.DupX1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.DupX2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dup2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dup2X1, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dup2X2, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Swap, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iadd, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ladd, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fadd, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dadd, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Isub, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lsub, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fsub, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dsub, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Imul, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lmul, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fmul, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dmul, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Idiv, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Ldiv, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Fdiv, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ddiv, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Irem, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Lrem, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Frem, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Drem, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ineg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lneg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fneg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dneg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ishl, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lshl, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ishr, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lshr, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iushr, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lushr, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iand, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Land, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ior, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lor, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ixor, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lxor, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Iinc, OpCodeArgumentLayout.Local1_ImmediateS1, true);
            new OpCodeMetaData(OpCode.I2l, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.I2f, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.I2d, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.L2i, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.L2f, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.L2d, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.F2i, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.F2l, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.F2d, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.D2i, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.D2l, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.D2f, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.I2b, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.I2c, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.I2s, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lcmp, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fcmpl, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Fcmpg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dcmpl, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dcmpg, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Ifeq, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Ifne, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Iflt, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Ifge, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Ifgt, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Ifle, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmpeq, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmpne, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmplt, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmpge, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmpgt, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfIcmple, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfAcmpeq, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfAcmpne, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Goto, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Jsr, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.Ret, OpCodeArgumentLayout.Local1, true);
            new OpCodeMetaData(OpCode.TableSwitch, OpCodeArgumentLayout.TableSwitch, true);
            new OpCodeMetaData(OpCode.LookupSwitch, OpCodeArgumentLayout.LookupSwitch, true);
            new OpCodeMetaData(OpCode.Ireturn, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Lreturn, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Freturn, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Dreturn, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Areturn, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.Return, OpCodeArgumentLayout.None, true);
            new OpCodeMetaData(OpCode.GetStatic, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.PutStatic, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.GetField, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.PutField, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.InvokeVirtual, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.InvokeSpecial, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.InvokeStatic, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.InvokeInterface, OpCodeArgumentLayout.Constant2_Count1_Zero1, false);
            new OpCodeMetaData(OpCode.InvokeDynamic, OpCodeArgumentLayout.Constant2_Count1_Zero1, false);
            new OpCodeMetaData(OpCode.New, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.Newarray, OpCodeArgumentLayout.ImmediateU1, false);
            new OpCodeMetaData(OpCode.Anewarray, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.Arraylength, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Athrow, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Checkcast, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.InstanceOf, OpCodeArgumentLayout.Constant2, false);
            new OpCodeMetaData(OpCode.MonitorEnter, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.MonitorExit, OpCodeArgumentLayout.None, false);
            new OpCodeMetaData(OpCode.Wide, NormalizedOpCode.Nop, OpCodeArgumentLayout.WidePrefix, true);
            new OpCodeMetaData(OpCode.Multianewarray, OpCodeArgumentLayout.Constant2_ImmediateU1, false);
            new OpCodeMetaData(OpCode.IfNull, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.IfNonNull, OpCodeArgumentLayout.Branch2, true);
            new OpCodeMetaData(OpCode.GotoW, NormalizedOpCode.Goto, OpCodeArgumentLayout.Branch4, true);
            new OpCodeMetaData(OpCode.JsrW, NormalizedOpCode.Jsr, OpCodeArgumentLayout.Branch4, true);

        }

    }

}
