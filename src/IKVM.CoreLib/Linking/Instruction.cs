/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

using System;

using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Linking
{

    internal struct Instruction<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingType : class, ILinkingType<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingMember : class, ILinkingMember<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>
        where TLinkingField : class, ILinkingField<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
        where TLinkingMethod : class, ILinkingMethod<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod>, TLinkingMember
    {

        ushort pc;
        NormalizedOpCode normopcode;
        int arg1;
        short arg2;
        SwitchEntry[] switch_entries;

        struct SwitchEntry
        {
            internal int value;
            internal int target;
        }

        internal void SetHardError(HardError error, int messageId)
        {
            normopcode = NormalizedOpCode.StaticError;
            arg2 = (short)error;
            arg1 = messageId;
        }

        internal readonly HardError HardError => (HardError)arg2;

        internal readonly int HandlerIndex => (ushort)arg2;

        internal readonly int HardErrorMessageId => arg1;

        internal void PatchOpCode(NormalizedOpCode bc)
        {
            this.normopcode = bc;
        }

        internal void PatchOpCode(NormalizedOpCode bc, int arg1)
        {
            this.normopcode = bc;
            this.arg1 = arg1;
        }

        internal void PatchOpCode(NormalizedOpCode bc, int arg1, short arg2)
        {
            this.normopcode = bc;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        internal void SetPC(int pc)
        {
            this.pc = (ushort)pc;
        }

        internal void SetTargetIndex(int targetIndex)
        {
            this.arg1 = targetIndex;
        }

        internal void SetTermNop(ushort pc)
        {
            // TODO what happens if we already have exactly the maximum number of instructions?
            this.pc = pc;
            this.normopcode = NormalizedOpCode.Nop;
        }

        internal void MapSwitchTargets(int[] pcIndexMap)
        {
            arg1 = pcIndexMap[arg1 + pc];
            for (int i = 0; i < switch_entries.Length; i++)
            {
                switch_entries[i].target = pcIndexMap[switch_entries[i].target + pc];
            }
        }

        internal void Read(IKVM.ByteCode.Decoding.Instruction instruction, ClassFile<TLinkingType, TLinkingMember, TLinkingField, TLinkingMethod> classFile)
        {
            this.pc = (ushort)instruction.Offset;

            try
            {
                switch (instruction.OpCode)
                {
                    case IKVM.ByteCode.OpCode.Nop:
                        break;
                    case IKVM.ByteCode.OpCode.AconstNull:
                        break;
                    case IKVM.ByteCode.OpCode.IconstM1:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst0:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst1:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst2:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst3:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst4:
                        break;
                    case IKVM.ByteCode.OpCode.Iconst5:
                        break;
                    case IKVM.ByteCode.OpCode.Lconst0:
                        break;
                    case IKVM.ByteCode.OpCode.Lconst1:
                        break;
                    case IKVM.ByteCode.OpCode.Fconst0:
                        break;
                    case IKVM.ByteCode.OpCode.Fconst1:
                        break;
                    case IKVM.ByteCode.OpCode.Fconst2:
                        break;
                    case IKVM.ByteCode.OpCode.Dconst0:
                        break;
                    case IKVM.ByteCode.OpCode.Dconst1:
                        break;
                    case IKVM.ByteCode.OpCode.Bipush:
                        var _bipush = instruction.AsBipush();
                        arg1 = _bipush.Value;
                        break;
                    case IKVM.ByteCode.OpCode.Sipush:
                        var _sipush = instruction.AsSipush();
                        arg1 = _sipush.Value;
                        break;
                    case IKVM.ByteCode.OpCode.Ldc:
                        var _ldc = instruction.AsLdc();
                        arg1 = _ldc.Constant.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.LdcW:
                        var _ldcw = instruction.AsLdcW();
                        arg1 = _ldcw.Constant.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.Ldc2W:
                        var _ldc2w = instruction.AsLdc2W();
                        arg1 = _ldc2w.Constant.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.Iload:
                        var _iload = instruction.AsIload();
                        arg1 = _iload.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Lload:
                        var _lload = instruction.AsLload();
                        arg1 = _lload.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Fload:
                        var _fload = instruction.AsFload();
                        arg1 = _fload.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Dload:
                        var _dload = instruction.AsDload();
                        arg1 = _dload.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Aload:
                        var _aload = instruction.AsAload();
                        arg1 = _aload.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Iload0:
                        break;
                    case IKVM.ByteCode.OpCode.Iload1:
                        break;
                    case IKVM.ByteCode.OpCode.Iload2:
                        break;
                    case IKVM.ByteCode.OpCode.Iload3:
                        break;
                    case IKVM.ByteCode.OpCode.Lload0:
                        break;
                    case IKVM.ByteCode.OpCode.Lload1:
                        break;
                    case IKVM.ByteCode.OpCode.Lload2:
                        break;
                    case IKVM.ByteCode.OpCode.Lload3:
                        break;
                    case IKVM.ByteCode.OpCode.Fload0:
                        break;
                    case IKVM.ByteCode.OpCode.Fload1:
                        break;
                    case IKVM.ByteCode.OpCode.Fload2:
                        break;
                    case IKVM.ByteCode.OpCode.Fload3:
                        break;
                    case IKVM.ByteCode.OpCode.Dload0:
                        break;
                    case IKVM.ByteCode.OpCode.Dload1:
                        break;
                    case IKVM.ByteCode.OpCode.Dload2:
                        break;
                    case IKVM.ByteCode.OpCode.Dload3:
                        break;
                    case IKVM.ByteCode.OpCode.Aload0:
                        break;
                    case IKVM.ByteCode.OpCode.Aload1:
                        break;
                    case IKVM.ByteCode.OpCode.Aload2:
                        break;
                    case IKVM.ByteCode.OpCode.Aload3:
                        break;
                    case IKVM.ByteCode.OpCode.Iaload:
                        break;
                    case IKVM.ByteCode.OpCode.Laload:
                        break;
                    case IKVM.ByteCode.OpCode.Faload:
                        break;
                    case IKVM.ByteCode.OpCode.Daload:
                        break;
                    case IKVM.ByteCode.OpCode.Aaload:
                        break;
                    case IKVM.ByteCode.OpCode.Baload:
                        break;
                    case IKVM.ByteCode.OpCode.Caload:
                        break;
                    case IKVM.ByteCode.OpCode.Saload:
                        break;
                    case IKVM.ByteCode.OpCode.Istore:
                        var _istore = instruction.AsIstore();
                        arg1 = _istore.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Lstore:
                        var _lstore = instruction.AsLstore();
                        arg1 = _lstore.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Fstore:
                        var _fstore = instruction.AsFstore();
                        arg1 = _fstore.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Dstore:
                        var _dstore = instruction.AsDstore();
                        arg1 = _dstore.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Astore:
                        var _astore = instruction.AsAstore();
                        arg1 = _astore.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Istore0:
                        break;
                    case IKVM.ByteCode.OpCode.Istore1:
                        break;
                    case IKVM.ByteCode.OpCode.Istore2:
                        break;
                    case IKVM.ByteCode.OpCode.Istore3:
                        break;
                    case IKVM.ByteCode.OpCode.Lstore0:
                        break;
                    case IKVM.ByteCode.OpCode.Lstore1:
                        break;
                    case IKVM.ByteCode.OpCode.Lstore2:
                        break;
                    case IKVM.ByteCode.OpCode.Lstore3:
                        break;
                    case IKVM.ByteCode.OpCode.Fstore0:
                        break;
                    case IKVM.ByteCode.OpCode.Fstore1:
                        break;
                    case IKVM.ByteCode.OpCode.Fstore2:
                        break;
                    case IKVM.ByteCode.OpCode.Fstore3:
                        break;
                    case IKVM.ByteCode.OpCode.Dstore0:
                        break;
                    case IKVM.ByteCode.OpCode.Dstore1:
                        break;
                    case IKVM.ByteCode.OpCode.Dstore2:
                        break;
                    case IKVM.ByteCode.OpCode.Dstore3:
                        break;
                    case IKVM.ByteCode.OpCode.Astore0:
                        break;
                    case IKVM.ByteCode.OpCode.Astore1:
                        break;
                    case IKVM.ByteCode.OpCode.Astore2:
                        break;
                    case IKVM.ByteCode.OpCode.Astore3:
                        break;
                    case IKVM.ByteCode.OpCode.Iastore:
                        break;
                    case IKVM.ByteCode.OpCode.Lastore:
                        break;
                    case IKVM.ByteCode.OpCode.Fastore:
                        break;
                    case IKVM.ByteCode.OpCode.Dastore:
                        break;
                    case IKVM.ByteCode.OpCode.Aastore:
                        break;
                    case IKVM.ByteCode.OpCode.Bastore:
                        break;
                    case IKVM.ByteCode.OpCode.Castore:
                        break;
                    case IKVM.ByteCode.OpCode.Sastore:
                        break;
                    case IKVM.ByteCode.OpCode.Pop:
                        break;
                    case IKVM.ByteCode.OpCode.Pop2:
                        break;
                    case IKVM.ByteCode.OpCode.Dup:
                        break;
                    case IKVM.ByteCode.OpCode.DupX1:
                        break;
                    case IKVM.ByteCode.OpCode.DupX2:
                        break;
                    case IKVM.ByteCode.OpCode.Dup2:
                        break;
                    case IKVM.ByteCode.OpCode.Dup2X1:
                        break;
                    case IKVM.ByteCode.OpCode.Dup2X2:
                        break;
                    case IKVM.ByteCode.OpCode.Swap:
                        break;
                    case IKVM.ByteCode.OpCode.Iadd:
                        break;
                    case IKVM.ByteCode.OpCode.Ladd:
                        break;
                    case IKVM.ByteCode.OpCode.Fadd:
                        break;
                    case IKVM.ByteCode.OpCode.Dadd:
                        break;
                    case IKVM.ByteCode.OpCode.Isub:
                        break;
                    case IKVM.ByteCode.OpCode.Lsub:
                        break;
                    case IKVM.ByteCode.OpCode.Fsub:
                        break;
                    case IKVM.ByteCode.OpCode.Dsub:
                        break;
                    case IKVM.ByteCode.OpCode.Imul:
                        break;
                    case IKVM.ByteCode.OpCode.Lmul:
                        break;
                    case IKVM.ByteCode.OpCode.Fmul:
                        break;
                    case IKVM.ByteCode.OpCode.Dmul:
                        break;
                    case IKVM.ByteCode.OpCode.Idiv:
                        break;
                    case IKVM.ByteCode.OpCode.Ldiv:
                        break;
                    case IKVM.ByteCode.OpCode.Fdiv:
                        break;
                    case IKVM.ByteCode.OpCode.Ddiv:
                        break;
                    case IKVM.ByteCode.OpCode.Irem:
                        break;
                    case IKVM.ByteCode.OpCode.Lrem:
                        break;
                    case IKVM.ByteCode.OpCode.Frem:
                        break;
                    case IKVM.ByteCode.OpCode.Drem:
                        break;
                    case IKVM.ByteCode.OpCode.Ineg:
                        break;
                    case IKVM.ByteCode.OpCode.Lneg:
                        break;
                    case IKVM.ByteCode.OpCode.Fneg:
                        break;
                    case IKVM.ByteCode.OpCode.Dneg:
                        break;
                    case IKVM.ByteCode.OpCode.Ishl:
                        break;
                    case IKVM.ByteCode.OpCode.Lshl:
                        break;
                    case IKVM.ByteCode.OpCode.Ishr:
                        break;
                    case IKVM.ByteCode.OpCode.Lshr:
                        break;
                    case IKVM.ByteCode.OpCode.Iushr:
                        break;
                    case IKVM.ByteCode.OpCode.Lushr:
                        break;
                    case IKVM.ByteCode.OpCode.Iand:
                        break;
                    case IKVM.ByteCode.OpCode.Land:
                        break;
                    case IKVM.ByteCode.OpCode.Ior:
                        break;
                    case IKVM.ByteCode.OpCode.Lor:
                        break;
                    case IKVM.ByteCode.OpCode.Ixor:
                        break;
                    case IKVM.ByteCode.OpCode.Lxor:
                        break;
                    case IKVM.ByteCode.OpCode.Iinc:
                        var _iinc = instruction.AsIinc();
                        arg1 = _iinc.Local;
                        arg2 = _iinc.Value;
                        break;
                    case IKVM.ByteCode.OpCode.I2l:
                        break;
                    case IKVM.ByteCode.OpCode.I2f:
                        break;
                    case IKVM.ByteCode.OpCode.I2d:
                        break;
                    case IKVM.ByteCode.OpCode.L2i:
                        break;
                    case IKVM.ByteCode.OpCode.L2f:
                        break;
                    case IKVM.ByteCode.OpCode.L2d:
                        break;
                    case IKVM.ByteCode.OpCode.F2i:
                        break;
                    case IKVM.ByteCode.OpCode.F2l:
                        break;
                    case IKVM.ByteCode.OpCode.F2d:
                        break;
                    case IKVM.ByteCode.OpCode.D2i:
                        break;
                    case IKVM.ByteCode.OpCode.D2l:
                        break;
                    case IKVM.ByteCode.OpCode.D2f:
                        break;
                    case IKVM.ByteCode.OpCode.I2b:
                        break;
                    case IKVM.ByteCode.OpCode.I2c:
                        break;
                    case IKVM.ByteCode.OpCode.I2s:
                        break;
                    case IKVM.ByteCode.OpCode.Lcmp:
                        break;
                    case IKVM.ByteCode.OpCode.Fcmpl:
                        break;
                    case IKVM.ByteCode.OpCode.Fcmpg:
                        break;
                    case IKVM.ByteCode.OpCode.Dcmpl:
                        break;
                    case IKVM.ByteCode.OpCode.Dcmpg:
                        break;
                    case IKVM.ByteCode.OpCode.Ifeq:
                        var _ifeq = instruction.AsIfeq();
                        arg1 = _ifeq.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Ifne:
                        var _ifne = instruction.AsIfne();
                        arg1 = _ifne.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Iflt:
                        var _iflt = instruction.AsIflt();
                        arg1 = _iflt.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Ifge:
                        var _ifge = instruction.AsIfge();
                        arg1 = _ifge.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Ifgt:
                        var _ifgt = instruction.AsIfgt();
                        arg1 = _ifgt.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Ifle:
                        var _ifle = instruction.AsIfle();
                        arg1 = _ifle.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmpeq:
                        var _ificmpeq = instruction.AsIfIcmpeq();
                        arg1 = _ificmpeq.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmpne:
                        var _ificmpne = instruction.AsIfIcmpne();
                        arg1 = _ificmpne.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmplt:
                        var _ificmplt = instruction.AsIfIcmplt();
                        arg1 = _ificmplt.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmpge:
                        var _ificmpge = instruction.AsIfIcmpge();
                        arg1 = _ificmpge.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmpgt:
                        var _ificmpgt = instruction.AsIfIcmpgt();
                        arg1 = _ificmpgt.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfIcmple:
                        var _ificmple = instruction.AsIfIcmple();
                        arg1 = _ificmple.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfAcmpeq:
                        var _ifacmpeq = instruction.AsIfAcmpeq();
                        arg1 = _ifacmpeq.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfAcmpne:
                        var _ifacmpne = instruction.AsIfAcmpne();
                        arg1 = _ifacmpne.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Goto:
                        var _goto = instruction.AsGoto();
                        arg1 = _goto.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Jsr:
                        var _jsr = instruction.AsJsr();
                        arg1 = _jsr.Target;
                        break;
                    case IKVM.ByteCode.OpCode.Ret:
                        var _ret = instruction.AsRet();
                        arg1 = _ret.Local;
                        break;
                    case IKVM.ByteCode.OpCode.Ireturn:
                        break;
                    case IKVM.ByteCode.OpCode.Lreturn:
                        break;
                    case IKVM.ByteCode.OpCode.Freturn:
                        break;
                    case IKVM.ByteCode.OpCode.Dreturn:
                        break;
                    case IKVM.ByteCode.OpCode.Areturn:
                        break;
                    case IKVM.ByteCode.OpCode.Return:
                        break;
                    case IKVM.ByteCode.OpCode.GetStatic:
                        var _getstatic = instruction.AsGetStatic();
                        arg1 = _getstatic.Field.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.PutStatic:
                        var _putstatic = instruction.AsPutStatic();
                        arg1 = _putstatic.Field.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.GetField:
                        var _getfield = instruction.AsGetField();
                        arg1 = _getfield.Field.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.PutField:
                        var _putfield = instruction.AsPutField();
                        arg1 = _putfield.Field.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InvokeVirtual:
                        var _invokevirtual = instruction.AsInvokeVirtual();
                        arg1 = _invokevirtual.Method.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InvokeSpecial:
                        var _invokespecial = instruction.AsInvokeSpecial();
                        arg1 = _invokespecial.Method.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InvokeStatic:
                        var _invokestatic = instruction.AsInvokeStatic();
                        arg1 = _invokestatic.Method.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InvokeInterface:
                        var _invokeinterface = instruction.AsInvokeInterface();
                        if (_invokeinterface.Zero != 0)
                            throw new ClassFormatException("invokeinterface filler must be zero");

                        arg1 = _invokeinterface.Method.Slot;
                        arg2 = _invokeinterface.Count;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InvokeDynamic:
                        var _invokedynamic = instruction.AsInvokeDynamic();
                        if (_invokedynamic.Zero != 0)
                            throw new ClassFormatException("invokedynamic filler must be zero");
                        if (_invokedynamic.Zero2 != 0)
                            throw new ClassFormatException("invokedynamic filler must be zero");

                        arg1 = _invokedynamic.Method.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.New:
                        var _new = instruction.AsNew();
                        arg1 = _new.Constant.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.Newarray:
                        var _newarray = instruction.AsNewarray();
                        arg1 = _newarray.Value;
                        break;
                    case IKVM.ByteCode.OpCode.Anewarray:
                        var _anewarray = instruction.AsAnewarray();
                        arg1 = _anewarray.Constant.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.Arraylength:
                        break;
                    case IKVM.ByteCode.OpCode.Athrow:
                        break;
                    case IKVM.ByteCode.OpCode.Checkcast:
                        var _checkcast = instruction.AsCheckcast();
                        arg1 = _checkcast.Type.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.InstanceOf:
                        var _instanceof = instruction.AsInstanceOf();
                        arg1 = _instanceof.Type.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        break;
                    case IKVM.ByteCode.OpCode.MonitorEnter:
                        break;
                    case IKVM.ByteCode.OpCode.MonitorExit:
                        break;
                    case IKVM.ByteCode.OpCode.Multianewarray:
                        var _multianewarray = instruction.AsMultianewarray();
                        arg1 = _multianewarray.Type.Slot;
                        classFile.MarkLinkRequiredConstantPoolItem(arg1);
                        arg2 = _multianewarray.Dimensions;
                        break;
                    case IKVM.ByteCode.OpCode.IfNull:
                        var _ifnull = instruction.AsIfNull();
                        arg1 = _ifnull.Target;
                        break;
                    case IKVM.ByteCode.OpCode.IfNonNull:
                        var _ifnonnull = instruction.AsIfNonNull();
                        arg1 = _ifnonnull.Target;
                        break;
                    case IKVM.ByteCode.OpCode.GotoW:
                        var _gotow = instruction.AsGotoW();
                        arg1 = _gotow.Target;
                        break;
                    case IKVM.ByteCode.OpCode.JsrW:
                        var _jsrw = instruction.AsJsrW();
                        arg1 = _jsrw.Target;
                        break;
                    case IKVM.ByteCode.OpCode.LookupSwitch:
                        var _lookupswitch = instruction.AsLookupSwitch();
                        if (_lookupswitch.Cases.Count < 0 || _lookupswitch.Cases.Count > 16384)
                            throw new ClassFormatException("Incorrect lookupswitch");

                        arg1 = _lookupswitch.DefaultTarget;

                        var _lookupswitchindex = 0;
                        var lookupswitchentries = new SwitchEntry[_lookupswitch.Cases.Count];
                        foreach (var _case in _lookupswitch.Cases)
                        {
                            lookupswitchentries[_lookupswitchindex].value = _case.Key;
                            lookupswitchentries[_lookupswitchindex].target = _case.Target;
                            _lookupswitchindex++;
                        }

                        switch_entries = lookupswitchentries;
                        break;
                    case IKVM.ByteCode.OpCode.TableSwitch:
                        var _tableswitch = instruction.AsTableSwitch();
                        if (_tableswitch.Low > _tableswitch.High || _tableswitch.High > 16384L + _tableswitch.Low)
                            throw new ClassFormatException("Incorrect tableswitch");

                        arg1 = _tableswitch.DefaultTarget;

                        var _tableswitchindex = 0;
                        var tableswitchentries = new SwitchEntry[_tableswitch.Cases.Count];
                        foreach (var _case in _tableswitch.Cases)
                        {
                            tableswitchentries[_tableswitchindex].value = _tableswitch.Low + _tableswitchindex;
                            tableswitchentries[_tableswitchindex].target = _case;
                            _tableswitchindex++;
                        }

                        switch_entries = tableswitchentries;
                        break;
                    default:
                        throw new ClassFormatException($"Invalid opcode: {instruction.OpCode}");
                }

                normopcode = OpCodeMetaData.GetNormalizedByteCode(instruction.OpCode);
                arg1 = OpCodeMetaData.GetArg0Value(instruction.OpCode, arg1);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ClassFormatException($"Invalid instruction data: {instruction.OpCode}");
            }
        }

        internal readonly int PC => pc;

        internal readonly NormalizedOpCode NormalizedOpCode => normopcode;

        internal readonly int Arg1 => arg1;

        internal int TargetIndex
        {
            get => arg1;
            set => arg1 = value;
        }

        internal readonly int Arg2 => arg2;

        internal readonly int NormalizedArg1 => arg1;

        internal int DefaultTarget
        {
            get => arg1;
            set => arg1 = value;
        }

        internal readonly int SwitchEntryCount => switch_entries.Length;

        internal readonly int GetSwitchValue(int i)
        {
            return switch_entries[i].value;
        }

        internal readonly int GetSwitchTargetIndex(int i)
        {
            return switch_entries[i].target;
        }

        internal void SetSwitchTargets(int[] targets)
        {
            var newEntries = (SwitchEntry[])switch_entries.Clone();
            for (int i = 0; i < newEntries.Length; i++)
                newEntries[i].target = targets[i];

            switch_entries = newEntries;
        }

    }

}
