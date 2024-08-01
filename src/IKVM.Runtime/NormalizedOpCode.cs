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

namespace IKVM.Runtime
{

    /// <summary>
    /// Enumeration of select Java instruction code values which we normalize to. For instance, iload_N is removed and replaced with iload(N).
    /// Also contains various pseudo opcodes.
    /// </summary>
    enum NormalizedOpCode : byte
    {

        _nop = 0,
        _aconst_null = 1,
        _lconst_0 = 9,
        _lconst_1 = 10,
        _fconst_0 = 11,
        _fconst_1 = 12,
        _fconst_2 = 13,
        _dconst_0 = 14,
        _dconst_1 = 15,
        _ldc = 18,
        _iload = 21,
        _lload = 22,
        _fload = 23,
        _dload = 24,
        _aload = 25,
        _iaload = 46,
        _laload = 47,
        _faload = 48,
        _daload = 49,
        _aaload = 50,
        _baload = 51,
        _caload = 52,
        _saload = 53,
        _istore = 54,
        _lstore = 55,
        _fstore = 56,
        _dstore = 57,
        _astore = 58,
        _iastore = 79,
        _lastore = 80,
        _fastore = 81,
        _dastore = 82,
        _aastore = 83,
        _bastore = 84,
        _castore = 85,
        _sastore = 86,
        _pop = 87,
        _pop2 = 88,
        _dup = 89,
        _dup_x1 = 90,
        _dup_x2 = 91,
        _dup2 = 92,
        _dup2_x1 = 93,
        _dup2_x2 = 94,
        _swap = 95,
        _iadd = 96,
        _ladd = 97,
        _fadd = 98,
        _dadd = 99,
        _isub = 100,
        _lsub = 101,
        _fsub = 102,
        _dsub = 103,
        _imul = 104,
        _lmul = 105,
        _fmul = 106,
        _dmul = 107,
        _idiv = 108,
        _ldiv = 109,
        _fdiv = 110,
        _ddiv = 111,
        _irem = 112,
        _lrem = 113,
        _frem = 114,
        _drem = 115,
        _ineg = 116,
        _lneg = 117,
        _fneg = 118,
        _dneg = 119,
        _ishl = 120,
        _lshl = 121,
        _ishr = 122,
        _lshr = 123,
        _iushr = 124,
        _lushr = 125,
        _iand = 126,
        _land = 127,
        _ior = 128,
        _lor = 129,
        _ixor = 130,
        _lxor = 131,
        _iinc = 132,
        _i2l = 133,
        _i2f = 134,
        _i2d = 135,
        _l2i = 136,
        _l2f = 137,
        _l2d = 138,
        _f2i = 139,
        _f2l = 140,
        _f2d = 141,
        _d2i = 142,
        _d2l = 143,
        _d2f = 144,
        _i2b = 145,
        _i2c = 146,
        _i2s = 147,
        _lcmp = 148,
        _fcmpl = 149,
        _fcmpg = 150,
        _dcmpl = 151,
        _dcmpg = 152,
        _ifeq = 153,
        _ifne = 154,
        _iflt = 155,
        _ifge = 156,
        _ifgt = 157,
        _ifle = 158,
        _if_icmpeq = 159,
        _if_icmpne = 160,
        _if_icmplt = 161,
        _if_icmpge = 162,
        _if_icmpgt = 163,
        _if_icmple = 164,
        _if_acmpeq = 165,
        _if_acmpne = 166,
        _goto = 167,
        _jsr = 168,
        _ret = 169,
        _tableswitch = 170,
        _lookupswitch = 171,
        _ireturn = 172,
        _lreturn = 173,
        _freturn = 174,
        _dreturn = 175,
        _areturn = 176,
        _return = 177,
        _getstatic = 178,
        _putstatic = 179,
        _getfield = 180,
        _putfield = 181,
        _invokevirtual = 182,
        _invokespecial = 183,
        _invokestatic = 184,
        _invokeinterface = 185,
        _invokedynamic = 186,
        _new = 187,
        _newarray = 188,
        _anewarray = 189,
        _arraylength = 190,
        _athrow = 191,
        _checkcast = 192,
        _instanceof = 193,
        _monitorenter = 194,
        _monitorexit = 195,
        _multianewarray = 197,
        _ifnull = 198,
        _ifnonnull = 199,

        __privileged_invokestatic = 235, // used for accessing host class members
        __privileged_invokevirtual = 237, // used for accessing host class members
        __privileged_invokespecial = 238,// used for accessing host class members
        __ldc_nothrow = 239,
        __methodhandle_invoke = 240,
        __methodhandle_link = 241,
        __goto_finally = 242,
        __intrinsic_gettype = 243,
        __athrow_no_unmap = 244,
        __dynamic_getstatic = 245,
        __dynamic_putstatic = 246,
        __dynamic_getfield = 247,
        __dynamic_putfield = 248,
        __dynamic_invokeinterface = 249,
        __dynamic_invokestatic = 250,
        __dynamic_invokevirtual = 251,
        __dynamic_invokespecial = 252,
        __clone_array = 253,
        __static_error = 254, // signals an instruction that is compiled as an exception
        __iconst = 255,

    }

}
