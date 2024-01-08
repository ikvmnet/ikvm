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

namespace IKVM.Reflection.Emit
{

    public sealed class OpCodes
	{

		public static readonly OpCode Nop = new OpCode(4888);
		public static readonly OpCode Break = new OpCode(4199116);
		public static readonly OpCode Ldarg_0 = new OpCode(8492847);
		public static readonly OpCode Ldarg_1 = new OpCode(12687151);
		public static readonly OpCode Ldarg_2 = new OpCode(16881455);
		public static readonly OpCode Ldarg_3 = new OpCode(21075759);
		public static readonly OpCode Ldloc_0 = new OpCode(25270063);
		public static readonly OpCode Ldloc_1 = new OpCode(29464367);
		public static readonly OpCode Ldloc_2 = new OpCode(33658671);
		public static readonly OpCode Ldloc_3 = new OpCode(37852975);
		public static readonly OpCode Stloc_0 = new OpCode(41949467);
		public static readonly OpCode Stloc_1 = new OpCode(46143771);
		public static readonly OpCode Stloc_2 = new OpCode(50338075);
		public static readonly OpCode Stloc_3 = new OpCode(54532379);
		public static readonly OpCode Ldarg_S = new OpCode(58824508);
		public static readonly OpCode Ldarga_S = new OpCode(63224012);
		public static readonly OpCode Starg_S = new OpCode(67115304);
		public static readonly OpCode Ldloc_S = new OpCode(71407420);
		public static readonly OpCode Ldloca_S = new OpCode(75806924);
		public static readonly OpCode Stloc_S = new OpCode(79698216);
		public static readonly OpCode Ldnull = new OpCode(84609339);
		public static readonly OpCode Ldc_I4_M1 = new OpCode(88389823);
		public static readonly OpCode Ldc_I4_0 = new OpCode(92584127);
		public static readonly OpCode Ldc_I4_1 = new OpCode(96778431);
		public static readonly OpCode Ldc_I4_2 = new OpCode(100972735);
		public static readonly OpCode Ldc_I4_3 = new OpCode(105167039);
		public static readonly OpCode Ldc_I4_4 = new OpCode(109361343);
		public static readonly OpCode Ldc_I4_5 = new OpCode(113555647);
		public static readonly OpCode Ldc_I4_6 = new OpCode(117749951);
		public static readonly OpCode Ldc_I4_7 = new OpCode(121944255);
		public static readonly OpCode Ldc_I4_8 = new OpCode(126138559);
		public static readonly OpCode Ldc_I4_S = new OpCode(130332874);
		public static readonly OpCode Ldc_I4 = new OpCode(134530584);
		public static readonly OpCode Ldc_I8 = new OpCode(138827489);
		public static readonly OpCode Ldc_R4 = new OpCode(143124407);
		public static readonly OpCode Ldc_R8 = new OpCode(147421301);
		public static readonly OpCode Dup = new OpCode(155404637);
		public static readonly OpCode Pop = new OpCode(159393399);
		public static readonly OpCode Jmp = new OpCode(163582686);
		public static readonly OpCode Call = new OpCode(168690130);
		public static readonly OpCode Calli = new OpCode(172884439);
		public static readonly OpCode Ret = new OpCode(176258034);
		public static readonly OpCode Br_S = new OpCode(180356455);
		public static readonly OpCode Brfalse_S = new OpCode(184566035);
		public static readonly OpCode Brtrue_S = new OpCode(188760339);
		public static readonly OpCode Beq_S = new OpCode(192949342);
		public static readonly OpCode Bge_S = new OpCode(197143646);
		public static readonly OpCode Bgt_S = new OpCode(201337950);
		public static readonly OpCode Ble_S = new OpCode(205532254);
		public static readonly OpCode Blt_S = new OpCode(209726558);
		public static readonly OpCode Bne_Un_S = new OpCode(213920862);
		public static readonly OpCode Bge_Un_S = new OpCode(218115166);
		public static readonly OpCode Bgt_Un_S = new OpCode(222309470);
		public static readonly OpCode Ble_Un_S = new OpCode(226503774);
		public static readonly OpCode Blt_Un_S = new OpCode(230698078);
		public static readonly OpCode Br = new OpCode(234885812);
		public static readonly OpCode Brfalse = new OpCode(239095392);
		public static readonly OpCode Brtrue = new OpCode(243289696);
		public static readonly OpCode Beq = new OpCode(247475279);
		public static readonly OpCode Bge = new OpCode(251669583);
		public static readonly OpCode Bgt = new OpCode(255863887);
		public static readonly OpCode Ble = new OpCode(260058191);
		public static readonly OpCode Blt = new OpCode(264252495);
		public static readonly OpCode Bne_Un = new OpCode(268446799);
		public static readonly OpCode Bge_Un = new OpCode(272641103);
		public static readonly OpCode Bgt_Un = new OpCode(276835407);
		public static readonly OpCode Ble_Un = new OpCode(281029711);
		public static readonly OpCode Blt_Un = new OpCode(285224015);
		public static readonly OpCode Switch = new OpCode(289427051);
		public static readonly OpCode Ldind_I1 = new OpCode(293929358);
		public static readonly OpCode Ldind_U1 = new OpCode(298123662);
		public static readonly OpCode Ldind_I2 = new OpCode(302317966);
		public static readonly OpCode Ldind_U2 = new OpCode(306512270);
		public static readonly OpCode Ldind_I4 = new OpCode(310706574);
		public static readonly OpCode Ldind_U4 = new OpCode(314900878);
		public static readonly OpCode Ldind_I8 = new OpCode(319197782);
		public static readonly OpCode Ldind_I = new OpCode(323289486);
		public static readonly OpCode Ldind_R4 = new OpCode(327688990);
		public static readonly OpCode Ldind_R8 = new OpCode(331985894);
		public static readonly OpCode Ldind_Ref = new OpCode(336282798);
		public static readonly OpCode Stind_Ref = new OpCode(339768820);
		public static readonly OpCode Stind_I1 = new OpCode(343963124);
		public static readonly OpCode Stind_I2 = new OpCode(348157428);
		public static readonly OpCode Stind_I4 = new OpCode(352351732);
		public static readonly OpCode Stind_I8 = new OpCode(356551166);
		public static readonly OpCode Stind_R4 = new OpCode(360755730);
		public static readonly OpCode Stind_R8 = new OpCode(364955164);
		public static readonly OpCode Add = new OpCode(369216329);
		public static readonly OpCode Sub = new OpCode(373410633);
		public static readonly OpCode Mul = new OpCode(377604937);
		public static readonly OpCode Div = new OpCode(381799241);
		public static readonly OpCode Div_Un = new OpCode(385993545);
		public static readonly OpCode Rem = new OpCode(390187849);
		public static readonly OpCode Rem_Un = new OpCode(394382153);
		public static readonly OpCode And = new OpCode(398576457);
		public static readonly OpCode Or = new OpCode(402770761);
		public static readonly OpCode Xor = new OpCode(406965065);
		public static readonly OpCode Shl = new OpCode(411159369);
		public static readonly OpCode Shr = new OpCode(415353673);
		public static readonly OpCode Shr_Un = new OpCode(419547977);
		public static readonly OpCode Neg = new OpCode(423737322);
		public static readonly OpCode Not = new OpCode(427931626);
		public static readonly OpCode Conv_I1 = new OpCode(432331130);
		public static readonly OpCode Conv_I2 = new OpCode(436525434);
		public static readonly OpCode Conv_I4 = new OpCode(440719738);
		public static readonly OpCode Conv_I8 = new OpCode(445016642);
		public static readonly OpCode Conv_R4 = new OpCode(449313546);
		public static readonly OpCode Conv_R8 = new OpCode(453610450);
		public static readonly OpCode Conv_U4 = new OpCode(457496954);
		public static readonly OpCode Conv_U8 = new OpCode(461793858);
		public static readonly OpCode Callvirt = new OpCode(466484004);
		public static readonly OpCode Cpobj = new OpCode(469790542);
		public static readonly OpCode Ldobj = new OpCode(474077528);
		public static readonly OpCode Ldstr = new OpCode(478872210);
		public static readonly OpCode Newobj = new OpCode(483158791);
		public static readonly OpCode Castclass = new OpCode(487311950);
		public static readonly OpCode Isinst = new OpCode(491095854);
		public static readonly OpCode Conv_R_Un = new OpCode(495553490);
		public static readonly OpCode Unbox = new OpCode(507874780);
		public static readonly OpCode Throw = new OpCode(511759452);
		public static readonly OpCode Ldfld = new OpCode(516056466);
		public static readonly OpCode Ldflda = new OpCode(520455970);
		public static readonly OpCode Stfld = new OpCode(524347262);
		public static readonly OpCode Ldsfld = new OpCode(528588249);
		public static readonly OpCode Ldsflda = new OpCode(532987753);
		public static readonly OpCode Stsfld = new OpCode(536879045);
		public static readonly OpCode Stobj = new OpCode(541090290);
		public static readonly OpCode Conv_Ovf_I1_Un = new OpCode(545577338);
		public static readonly OpCode Conv_Ovf_I2_Un = new OpCode(549771642);
		public static readonly OpCode Conv_Ovf_I4_Un = new OpCode(553965946);
		public static readonly OpCode Conv_Ovf_I8_Un = new OpCode(558262850);
		public static readonly OpCode Conv_Ovf_U1_Un = new OpCode(562354554);
		public static readonly OpCode Conv_Ovf_U2_Un = new OpCode(566548858);
		public static readonly OpCode Conv_Ovf_U4_Un = new OpCode(570743162);
		public static readonly OpCode Conv_Ovf_U8_Un = new OpCode(575040066);
		public static readonly OpCode Conv_Ovf_I_Un = new OpCode(579131770);
		public static readonly OpCode Conv_Ovf_U_Un = new OpCode(583326074);
		public static readonly OpCode Box = new OpCode(587930786);
		public static readonly OpCode Newarr = new OpCode(592133640);
		public static readonly OpCode Ldlen = new OpCode(595953446);
		public static readonly OpCode Ldelema = new OpCode(600157847);
		public static readonly OpCode Ldelem_I1 = new OpCode(604352143);
		public static readonly OpCode Ldelem_U1 = new OpCode(608546447);
		public static readonly OpCode Ldelem_I2 = new OpCode(612740751);
		public static readonly OpCode Ldelem_U2 = new OpCode(616935055);
		public static readonly OpCode Ldelem_I4 = new OpCode(621129359);
		public static readonly OpCode Ldelem_U4 = new OpCode(625323663);
		public static readonly OpCode Ldelem_I8 = new OpCode(629620567);
		public static readonly OpCode Ldelem_I = new OpCode(633712271);
		public static readonly OpCode Ldelem_R4 = new OpCode(638111775);
		public static readonly OpCode Ldelem_R8 = new OpCode(642408679);
		public static readonly OpCode Ldelem_Ref = new OpCode(646705583);
		public static readonly OpCode Stelem_I = new OpCode(650186475);
		public static readonly OpCode Stelem_I1 = new OpCode(654380779);
		public static readonly OpCode Stelem_I2 = new OpCode(658575083);
		public static readonly OpCode Stelem_I4 = new OpCode(662769387);
		public static readonly OpCode Stelem_I8 = new OpCode(666968821);
		public static readonly OpCode Stelem_R4 = new OpCode(671168255);
		public static readonly OpCode Stelem_R8 = new OpCode(675367689);
		public static readonly OpCode Stelem_Ref = new OpCode(679567123);
		public static readonly OpCode Ldelem = new OpCode(683838727);
		public static readonly OpCode Stelem = new OpCode(687965999);
		public static readonly OpCode Unbox_Any = new OpCode(692217246);
		public static readonly OpCode Conv_Ovf_I1 = new OpCode(751098234);
		public static readonly OpCode Conv_Ovf_U1 = new OpCode(755292538);
		public static readonly OpCode Conv_Ovf_I2 = new OpCode(759486842);
		public static readonly OpCode Conv_Ovf_U2 = new OpCode(763681146);
		public static readonly OpCode Conv_Ovf_I4 = new OpCode(767875450);
		public static readonly OpCode Conv_Ovf_U4 = new OpCode(772069754);
		public static readonly OpCode Conv_Ovf_I8 = new OpCode(776366658);
		public static readonly OpCode Conv_Ovf_U8 = new OpCode(780560962);
		public static readonly OpCode Refanyval = new OpCode(814012802);
		public static readonly OpCode Ckfinite = new OpCode(818514898);
		public static readonly OpCode Mkrefany = new OpCode(830595078);
		public static readonly OpCode Ldtoken = new OpCode(872728098);
		public static readonly OpCode Conv_U2 = new OpCode(876927354);
		public static readonly OpCode Conv_U1 = new OpCode(881121658);
		public static readonly OpCode Conv_I = new OpCode(885315962);
		public static readonly OpCode Conv_Ovf_I = new OpCode(889510266);
		public static readonly OpCode Conv_Ovf_U = new OpCode(893704570);
		public static readonly OpCode Add_Ovf = new OpCode(897698633);
		public static readonly OpCode Add_Ovf_Un = new OpCode(901892937);
		public static readonly OpCode Mul_Ovf = new OpCode(906087241);
		public static readonly OpCode Mul_Ovf_Un = new OpCode(910281545);
		public static readonly OpCode Sub_Ovf = new OpCode(914475849);
		public static readonly OpCode Sub_Ovf_Un = new OpCode(918670153);
		public static readonly OpCode Endfinally = new OpCode(922751806);
		public static readonly OpCode Leave = new OpCode(926945972);
		public static readonly OpCode Leave_S = new OpCode(931140291);
		public static readonly OpCode Stind_I = new OpCode(935359988);
		public static readonly OpCode Conv_U = new OpCode(939841914);
		public static readonly OpCode Prefix7 = new OpCode(1040189696);
		public static readonly OpCode Prefix6 = new OpCode(1044384000);
		public static readonly OpCode Prefix5 = new OpCode(1048578304);
		public static readonly OpCode Prefix4 = new OpCode(1052772608);
		public static readonly OpCode Prefix3 = new OpCode(1056966912);
		public static readonly OpCode Prefix2 = new OpCode(1061161216);
		public static readonly OpCode Prefix1 = new OpCode(1065355520);
		public static readonly OpCode Prefixref = new OpCode(1069549824);
		public static readonly OpCode Arglist = new OpCode(-2147170789);
		public static readonly OpCode Ceq = new OpCode(-2142966567);
		public static readonly OpCode Cgt = new OpCode(-2138772263);
		public static readonly OpCode Cgt_Un = new OpCode(-2134577959);
		public static readonly OpCode Clt = new OpCode(-2130383655);
		public static readonly OpCode Clt_Un = new OpCode(-2126189351);
		public static readonly OpCode Ldftn = new OpCode(-2122004966);
		public static readonly OpCode Ldvirtftn = new OpCode(-2117759533);
		public static readonly OpCode Ldarg = new OpCode(-2109627244);
		public static readonly OpCode Ldarga = new OpCode(-2105227740);
		public static readonly OpCode Starg = new OpCode(-2101336448);
		public static readonly OpCode Ldloc = new OpCode(-2097044332);
		public static readonly OpCode Ldloca = new OpCode(-2092644828);
		public static readonly OpCode Stloc = new OpCode(-2088753536);
		public static readonly OpCode Localloc = new OpCode(-2084241010);
		public static readonly OpCode Endfilter = new OpCode(-2076160335);
		public static readonly OpCode Unaligned = new OpCode(-2071982151);
		public static readonly OpCode Volatile = new OpCode(-2067787858);
		public static readonly OpCode Tailcall = new OpCode(-2063593554);
		public static readonly OpCode Initobj = new OpCode(-2059384859);
		public static readonly OpCode Constrained = new OpCode(-2055204938);
		public static readonly OpCode Cpblk = new OpCode(-2050974371);
		public static readonly OpCode Initblk = new OpCode(-2046780067);
		public static readonly OpCode Rethrow = new OpCode(-2038428509);
		public static readonly OpCode Sizeof = new OpCode(-2029730269);
		public static readonly OpCode Refanytype = new OpCode(-2025531014);
		public static readonly OpCode Readonly = new OpCode(-2021650514);

        internal static string GetName(int value) => value switch
        {
            0 => "nop",
            1 => "break",
            2 => "ldarg.0",
            3 => "ldarg.1",
            4 => "ldarg.2",
            5 => "ldarg.3",
            6 => "ldloc.0",
            7 => "ldloc.1",
            8 => "ldloc.2",
            9 => "ldloc.3",
            10 => "stloc.0",
            11 => "stloc.1",
            12 => "stloc.2",
            13 => "stloc.3",
            14 => "ldarg.s",
            15 => "ldarga.s",
            16 => "starg.s",
            17 => "ldloc.s",
            18 => "ldloca.s",
            19 => "stloc.s",
            20 => "ldnull",
            21 => "ldc.i4.m1",
            22 => "ldc.i4.0",
            23 => "ldc.i4.1",
            24 => "ldc.i4.2",
            25 => "ldc.i4.3",
            26 => "ldc.i4.4",
            27 => "ldc.i4.5",
            28 => "ldc.i4.6",
            29 => "ldc.i4.7",
            30 => "ldc.i4.8",
            31 => "ldc.i4.s",
            32 => "ldc.i4",
            33 => "ldc.i8",
            34 => "ldc.r4",
            35 => "ldc.r8",
            37 => "dup",
            38 => "pop",
            39 => "jmp",
            40 => "call",
            41 => "calli",
            42 => "ret",
            43 => "br.s",
            44 => "brfalse.s",
            45 => "brtrue.s",
            46 => "beq.s",
            47 => "bge.s",
            48 => "bgt.s",
            49 => "ble.s",
            50 => "blt.s",
            51 => "bne.un.s",
            52 => "bge.un.s",
            53 => "bgt.un.s",
            54 => "ble.un.s",
            55 => "blt.un.s",
            56 => "br",
            57 => "brfalse",
            58 => "brtrue",
            59 => "beq",
            60 => "bge",
            61 => "bgt",
            62 => "ble",
            63 => "blt",
            64 => "bne.un",
            65 => "bge.un",
            66 => "bgt.un",
            67 => "ble.un",
            68 => "blt.un",
            69 => "switch",
            70 => "ldind.i1",
            71 => "ldind.u1",
            72 => "ldind.i2",
            73 => "ldind.u2",
            74 => "ldind.i4",
            75 => "ldind.u4",
            76 => "ldind.i8",
            77 => "ldind.i",
            78 => "ldind.r4",
            79 => "ldind.r8",
            80 => "ldind.ref",
            81 => "stind.ref",
            82 => "stind.i1",
            83 => "stind.i2",
            84 => "stind.i4",
            85 => "stind.i8",
            86 => "stind.r4",
            87 => "stind.r8",
            88 => "add",
            89 => "sub",
            90 => "mul",
            91 => "div",
            92 => "div.un",
            93 => "rem",
            94 => "rem.un",
            95 => "and",
            96 => "or",
            97 => "xor",
            98 => "shl",
            99 => "shr",
            100 => "shr.un",
            101 => "neg",
            102 => "not",
            103 => "conv.i1",
            104 => "conv.i2",
            105 => "conv.i4",
            106 => "conv.i8",
            107 => "conv.r4",
            108 => "conv.r8",
            109 => "conv.u4",
            110 => "conv.u8",
            111 => "callvirt",
            112 => "cpobj",
            113 => "ldobj",
            114 => "ldstr",
            115 => "newobj",
            116 => "castclass",
            117 => "isinst",
            118 => "conv.r.un",
            121 => "unbox",
            122 => "throw",
            123 => "ldfld",
            124 => "ldflda",
            125 => "stfld",
            126 => "ldsfld",
            127 => "ldsflda",
            128 => "stsfld",
            129 => "stobj",
            130 => "conv.ovf.i1.un",
            131 => "conv.ovf.i2.un",
            132 => "conv.ovf.i4.un",
            133 => "conv.ovf.i8.un",
            134 => "conv.ovf.u1.un",
            135 => "conv.ovf.u2.un",
            136 => "conv.ovf.u4.un",
            137 => "conv.ovf.u8.un",
            138 => "conv.ovf.i.un",
            139 => "conv.ovf.u.un",
            140 => "box",
            141 => "newarr",
            142 => "ldlen",
            143 => "ldelema",
            144 => "ldelem.i1",
            145 => "ldelem.u1",
            146 => "ldelem.i2",
            147 => "ldelem.u2",
            148 => "ldelem.i4",
            149 => "ldelem.u4",
            150 => "ldelem.i8",
            151 => "ldelem.i",
            152 => "ldelem.r4",
            153 => "ldelem.r8",
            154 => "ldelem.ref",
            155 => "stelem.i",
            156 => "stelem.i1",
            157 => "stelem.i2",
            158 => "stelem.i4",
            159 => "stelem.i8",
            160 => "stelem.r4",
            161 => "stelem.r8",
            162 => "stelem.ref",
            163 => "ldelem",
            164 => "stelem",
            165 => "unbox.any",
            179 => "conv.ovf.i1",
            180 => "conv.ovf.u1",
            181 => "conv.ovf.i2",
            182 => "conv.ovf.u2",
            183 => "conv.ovf.i4",
            184 => "conv.ovf.u4",
            185 => "conv.ovf.i8",
            186 => "conv.ovf.u8",
            194 => "refanyval",
            195 => "ckfinite",
            198 => "mkrefany",
            208 => "ldtoken",
            209 => "conv.u2",
            210 => "conv.u1",
            211 => "conv.i",
            212 => "conv.ovf.i",
            213 => "conv.ovf.u",
            214 => "add.ovf",
            215 => "add.ovf.un",
            216 => "mul.ovf",
            217 => "mul.ovf.un",
            218 => "sub.ovf",
            219 => "sub.ovf.un",
            220 => "endfinally",
            221 => "leave",
            222 => "leave.s",
            223 => "stind.i",
            224 => "conv.u",
            248 => "prefix7",
            249 => "prefix6",
            250 => "prefix5",
            251 => "prefix4",
            252 => "prefix3",
            253 => "prefix2",
            254 => "prefix1",
            255 => "prefixref",
            -512 => "arglist",
            -511 => "ceq",
            -510 => "cgt",
            -509 => "cgt.un",
            -508 => "clt",
            -507 => "clt.un",
            -506 => "ldftn",
            -505 => "ldvirtftn",
            -503 => "ldarg",
            -502 => "ldarga",
            -501 => "starg",
            -500 => "ldloc",
            -499 => "ldloca",
            -498 => "stloc",
            -497 => "localloc",
            -495 => "endfilter",
            -494 => "unaligned.",
            -493 => "volatile.",
            -492 => "tail.",
            -491 => "initobj",
            -490 => "constrained.",
            -489 => "cpblk",
            -488 => "initblk",
            -486 => "rethrow",
            -484 => "sizeof",
            -483 => "refanytype",
            -482 => "readonly.",
            _ => throw new ArgumentOutOfRangeException(),
        };

        public static bool TakesSingleByteArgument(OpCode inst)
		{
			switch (inst.Value)
			{
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
				case 31:
				case 43:
				case 44:
				case 45:
				case 46:
				case 47:
				case 48:
				case 49:
				case 50:
				case 51:
				case 52:
				case 53:
				case 54:
				case 55:
				case 222:
				case -494:
					return true;
				default:
					return false;
			}
		}

	}

}
