/*
  Copyright (C) 2008 Jeroen Frijters

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
using System.Diagnostics;

namespace IKVM.Reflection.Emit
{
	public struct OpCode
	{
		private const int OperandTypeCount = 19;
		private const int FlowControlCount = 9;
		private const int StackDiffCount = 5;
		private readonly short value;
		private readonly short info;

		internal OpCode(short value, OperandType operandType, FlowControl flowControl, short stackDiff)
		{
			Debug.Assert(operandType >= 0 && (int)operandType < OperandTypeCount);
			Debug.Assert(flowControl >= 0 && (int)flowControl < FlowControlCount);
			Debug.Assert(stackDiff >= -3 && stackDiff <= 1);

			this.value = value;
			this.info = (short)(operandType
				+ OperandTypeCount * (short)flowControl
				+ OperandTypeCount * FlowControlCount * (stackDiff + 3));

			Debug.Assert(this.OperandType == operandType);
			Debug.Assert(this.FlowControl == flowControl);
			Debug.Assert(this.StackDiff == stackDiff);
		}

		public short Value
		{
			get { return value; }
		}

		public int Size
		{
			get { return value < 0 ? 2 : 1; }
		}

		public OperandType OperandType
		{
			get { return (OperandType)(info % OperandTypeCount); }
		}

		public FlowControl FlowControl
		{
			get { return (FlowControl)((info / OperandTypeCount) % FlowControlCount); }
		}

		internal short StackDiff
		{
			get { return (short)(((info / (OperandTypeCount * FlowControlCount)) % StackDiffCount) - 3); }
		}
	}

	public class OpCodes
	{
		public static readonly OpCode Nop = new OpCode(0, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Break = new OpCode(1, OperandType.InlineNone, FlowControl.Break, 0);
		public static readonly OpCode Ldarg_0 = new OpCode(2, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldarg_1 = new OpCode(3, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldarg_2 = new OpCode(4, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldarg_3 = new OpCode(5, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldloc_0 = new OpCode(6, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldloc_1 = new OpCode(7, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldloc_2 = new OpCode(8, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldloc_3 = new OpCode(9, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Stloc_0 = new OpCode(10, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Stloc_1 = new OpCode(11, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Stloc_2 = new OpCode(12, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Stloc_3 = new OpCode(13, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldarg_S = new OpCode(14, OperandType.ShortInlineVar, FlowControl.Next, 1);
		public static readonly OpCode Ldarga_S = new OpCode(15, OperandType.ShortInlineVar, FlowControl.Next, 1);
		public static readonly OpCode Starg_S = new OpCode(16, OperandType.ShortInlineVar, FlowControl.Next, -1);
		public static readonly OpCode Ldloc_S = new OpCode(17, OperandType.ShortInlineVar, FlowControl.Next, 1);
		public static readonly OpCode Ldloca_S = new OpCode(18, OperandType.ShortInlineVar, FlowControl.Next, 1);
		public static readonly OpCode Stloc_S = new OpCode(19, OperandType.ShortInlineVar, FlowControl.Next, -1);
		public static readonly OpCode Ldnull = new OpCode(20, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_M1 = new OpCode(21, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_0 = new OpCode(22, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_1 = new OpCode(23, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_2 = new OpCode(24, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_3 = new OpCode(25, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_4 = new OpCode(26, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_5 = new OpCode(27, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_6 = new OpCode(28, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_7 = new OpCode(29, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_8 = new OpCode(30, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4_S = new OpCode(31, OperandType.ShortInlineI, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I4 = new OpCode(32, OperandType.InlineI, FlowControl.Next, 1);
		public static readonly OpCode Ldc_I8 = new OpCode(33, OperandType.InlineI8, FlowControl.Next, 1);
		public static readonly OpCode Ldc_R4 = new OpCode(34, OperandType.ShortInlineR, FlowControl.Next, 1);
		public static readonly OpCode Ldc_R8 = new OpCode(35, OperandType.InlineR, FlowControl.Next, 1);
		public static readonly OpCode Dup = new OpCode(37, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Pop = new OpCode(38, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Jmp = new OpCode(39, OperandType.InlineMethod, FlowControl.Call, 0);
		public static readonly OpCode Call = new OpCode(40, OperandType.InlineMethod, FlowControl.Call, 0);
		public static readonly OpCode Calli = new OpCode(41, OperandType.InlineSig, FlowControl.Call, 0);
		public static readonly OpCode Ret = new OpCode(42, OperandType.InlineNone, FlowControl.Return, 0);
		public static readonly OpCode Br_S = new OpCode(43, OperandType.ShortInlineBrTarget, FlowControl.Branch, 0);
		public static readonly OpCode Brfalse_S = new OpCode(44, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -1);
		public static readonly OpCode Brtrue_S = new OpCode(45, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -1);
		public static readonly OpCode Beq_S = new OpCode(46, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bge_S = new OpCode(47, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bgt_S = new OpCode(48, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Ble_S = new OpCode(49, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Blt_S = new OpCode(50, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bne_Un_S = new OpCode(51, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bge_Un_S = new OpCode(52, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bgt_Un_S = new OpCode(53, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Ble_Un_S = new OpCode(54, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Blt_Un_S = new OpCode(55, OperandType.ShortInlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Br = new OpCode(56, OperandType.InlineBrTarget, FlowControl.Branch, 0);
		public static readonly OpCode Brfalse = new OpCode(57, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -1);
		public static readonly OpCode Brtrue = new OpCode(58, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -1);
		public static readonly OpCode Beq = new OpCode(59, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bge = new OpCode(60, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bgt = new OpCode(61, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Ble = new OpCode(62, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Blt = new OpCode(63, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bne_Un = new OpCode(64, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bge_Un = new OpCode(65, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Bgt_Un = new OpCode(66, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Ble_Un = new OpCode(67, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Blt_Un = new OpCode(68, OperandType.InlineBrTarget, FlowControl.Cond_Branch, -2);
		public static readonly OpCode Switch = new OpCode(69, OperandType.InlineSwitch, FlowControl.Cond_Branch, -1);
		public static readonly OpCode Ldind_I1 = new OpCode(70, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_U1 = new OpCode(71, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_I2 = new OpCode(72, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_U2 = new OpCode(73, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_I4 = new OpCode(74, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_U4 = new OpCode(75, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_I8 = new OpCode(76, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_I = new OpCode(77, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_R4 = new OpCode(78, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_R8 = new OpCode(79, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldind_Ref = new OpCode(80, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Stind_Ref = new OpCode(81, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_I1 = new OpCode(82, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_I2 = new OpCode(83, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_I4 = new OpCode(84, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_I8 = new OpCode(85, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_R4 = new OpCode(86, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Stind_R8 = new OpCode(87, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Add = new OpCode(88, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Sub = new OpCode(89, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Mul = new OpCode(90, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Div = new OpCode(91, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Div_Un = new OpCode(92, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Rem = new OpCode(93, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Rem_Un = new OpCode(94, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode And = new OpCode(95, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Or = new OpCode(96, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Xor = new OpCode(97, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Shl = new OpCode(98, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Shr = new OpCode(99, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Shr_Un = new OpCode(100, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Neg = new OpCode(101, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Not = new OpCode(102, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_I1 = new OpCode(103, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_I2 = new OpCode(104, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_I4 = new OpCode(105, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_I8 = new OpCode(106, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_R4 = new OpCode(107, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_R8 = new OpCode(108, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_U4 = new OpCode(109, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_U8 = new OpCode(110, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Callvirt = new OpCode(111, OperandType.InlineMethod, FlowControl.Call, 0);
		public static readonly OpCode Cpobj = new OpCode(112, OperandType.InlineType, FlowControl.Next, -2);
		public static readonly OpCode Ldobj = new OpCode(113, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Ldstr = new OpCode(114, OperandType.InlineString, FlowControl.Next, 1);
		public static readonly OpCode Newobj = new OpCode(115, OperandType.InlineMethod, FlowControl.Call, 1);
		public static readonly OpCode Castclass = new OpCode(116, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Isinst = new OpCode(117, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Conv_R_Un = new OpCode(118, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Unbox = new OpCode(121, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Throw = new OpCode(122, OperandType.InlineNone, FlowControl.Throw, -1);
		public static readonly OpCode Ldfld = new OpCode(123, OperandType.InlineField, FlowControl.Next, 0);
		public static readonly OpCode Ldflda = new OpCode(124, OperandType.InlineField, FlowControl.Next, 0);
		public static readonly OpCode Stfld = new OpCode(125, OperandType.InlineField, FlowControl.Next, -2);
		public static readonly OpCode Ldsfld = new OpCode(126, OperandType.InlineField, FlowControl.Next, 1);
		public static readonly OpCode Ldsflda = new OpCode(127, OperandType.InlineField, FlowControl.Next, 1);
		public static readonly OpCode Stsfld = new OpCode(128, OperandType.InlineField, FlowControl.Next, -1);
		public static readonly OpCode Stobj = new OpCode(129, OperandType.InlineType, FlowControl.Next, -2);
		public static readonly OpCode Conv_Ovf_I1_Un = new OpCode(130, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I2_Un = new OpCode(131, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I4_Un = new OpCode(132, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I8_Un = new OpCode(133, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U1_Un = new OpCode(134, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U2_Un = new OpCode(135, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U4_Un = new OpCode(136, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U8_Un = new OpCode(137, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I_Un = new OpCode(138, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U_Un = new OpCode(139, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Box = new OpCode(140, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Newarr = new OpCode(141, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Ldlen = new OpCode(142, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Ldelema = new OpCode(143, OperandType.InlineType, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_I1 = new OpCode(144, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_U1 = new OpCode(145, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_I2 = new OpCode(146, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_U2 = new OpCode(147, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_I4 = new OpCode(148, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_U4 = new OpCode(149, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_I8 = new OpCode(150, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_I = new OpCode(151, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_R4 = new OpCode(152, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_R8 = new OpCode(153, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldelem_Ref = new OpCode(154, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Stelem_I = new OpCode(155, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_I1 = new OpCode(156, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_I2 = new OpCode(157, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_I4 = new OpCode(158, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_I8 = new OpCode(159, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_R4 = new OpCode(160, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_R8 = new OpCode(161, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Stelem_Ref = new OpCode(162, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Ldelem = new OpCode(163, OperandType.InlineType, FlowControl.Next, -1);
		public static readonly OpCode Stelem = new OpCode(164, OperandType.InlineType, FlowControl.Next, -3);
		public static readonly OpCode Unbox_Any = new OpCode(165, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I1 = new OpCode(179, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U1 = new OpCode(180, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I2 = new OpCode(181, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U2 = new OpCode(182, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I4 = new OpCode(183, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U4 = new OpCode(184, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I8 = new OpCode(185, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U8 = new OpCode(186, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Refanyval = new OpCode(194, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Ckfinite = new OpCode(195, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Mkrefany = new OpCode(198, OperandType.InlineType, FlowControl.Next, 0);
		public static readonly OpCode Ldtoken = new OpCode(208, OperandType.InlineTok, FlowControl.Next, 1);
		public static readonly OpCode Conv_U2 = new OpCode(209, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_U1 = new OpCode(210, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_I = new OpCode(211, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_I = new OpCode(212, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Conv_Ovf_U = new OpCode(213, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Add_Ovf = new OpCode(214, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Add_Ovf_Un = new OpCode(215, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Mul_Ovf = new OpCode(216, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Mul_Ovf_Un = new OpCode(217, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Sub_Ovf = new OpCode(218, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Sub_Ovf_Un = new OpCode(219, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Endfinally = new OpCode(220, OperandType.InlineNone, FlowControl.Return, 0);
		public static readonly OpCode Leave = new OpCode(221, OperandType.InlineBrTarget, FlowControl.Branch, 0);
		public static readonly OpCode Leave_S = new OpCode(222, OperandType.ShortInlineBrTarget, FlowControl.Branch, 0);
		public static readonly OpCode Stind_I = new OpCode(223, OperandType.InlineNone, FlowControl.Next, -2);
		public static readonly OpCode Conv_U = new OpCode(224, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Prefix7 = new OpCode(248, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix6 = new OpCode(249, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix5 = new OpCode(250, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix4 = new OpCode(251, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix3 = new OpCode(252, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix2 = new OpCode(253, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefix1 = new OpCode(254, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Prefixref = new OpCode(255, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Arglist = new OpCode(-512, OperandType.InlineNone, FlowControl.Next, 1);
		public static readonly OpCode Ceq = new OpCode(-511, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Cgt = new OpCode(-510, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Cgt_Un = new OpCode(-509, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Clt = new OpCode(-508, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Clt_Un = new OpCode(-507, OperandType.InlineNone, FlowControl.Next, -1);
		public static readonly OpCode Ldftn = new OpCode(-506, OperandType.InlineMethod, FlowControl.Next, 1);
		public static readonly OpCode Ldvirtftn = new OpCode(-505, OperandType.InlineMethod, FlowControl.Next, 0);
		public static readonly OpCode Ldarg = new OpCode(-503, OperandType.InlineVar, FlowControl.Next, 1);
		public static readonly OpCode Ldarga = new OpCode(-502, OperandType.InlineVar, FlowControl.Next, 1);
		public static readonly OpCode Starg = new OpCode(-501, OperandType.InlineVar, FlowControl.Next, -1);
		public static readonly OpCode Ldloc = new OpCode(-500, OperandType.InlineVar, FlowControl.Next, 1);
		public static readonly OpCode Ldloca = new OpCode(-499, OperandType.InlineVar, FlowControl.Next, 1);
		public static readonly OpCode Stloc = new OpCode(-498, OperandType.InlineVar, FlowControl.Next, -1);
		public static readonly OpCode Localloc = new OpCode(-497, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Endfilter = new OpCode(-495, OperandType.InlineNone, FlowControl.Return, -1);
		public static readonly OpCode Unaligned = new OpCode(-494, OperandType.ShortInlineI, FlowControl.Meta, 0);
		public static readonly OpCode Volatile = new OpCode(-493, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Tailcall = new OpCode(-492, OperandType.InlineNone, FlowControl.Meta, 0);
		public static readonly OpCode Initobj = new OpCode(-491, OperandType.InlineType, FlowControl.Next, -1);
		public static readonly OpCode Constrained = new OpCode(-490, OperandType.InlineType, FlowControl.Meta, 0);
		public static readonly OpCode Cpblk = new OpCode(-489, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Initblk = new OpCode(-488, OperandType.InlineNone, FlowControl.Next, -3);
		public static readonly OpCode Rethrow = new OpCode(-486, OperandType.InlineNone, FlowControl.Throw, 0);
		public static readonly OpCode Sizeof = new OpCode(-484, OperandType.InlineType, FlowControl.Next, 1);
		public static readonly OpCode Refanytype = new OpCode(-483, OperandType.InlineNone, FlowControl.Next, 0);
		public static readonly OpCode Readonly = new OpCode(-482, OperandType.InlineNone, FlowControl.Meta, 0);
	}
}
