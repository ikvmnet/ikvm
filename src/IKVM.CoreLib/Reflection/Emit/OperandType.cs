// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Describes the operand type of a Microsoft intermediate language (MSIL) instruction.
    /// </summary>
    enum OperandType
    {

        /// <summary>
        /// The operand is a 32-bit integer branch target.
        /// </summary>
        InlineBrTarget = 0,

        /// <summary>
        /// The operand is a 32-bit metadata token for a field.
        /// </summary>
        InlineField = 1,

        /// <summary>
        /// The operand is a 32-bit integer.
        /// </summary>
        InlineI = 2,

        /// <summary>
        /// The operand is a 64-bit integer.
        /// </summary>
        InlineI8 = 3,

        /// <summary>
        /// The operand is a 32-bit metadata token for a method.
        /// </summary>
        InlineMethod = 4,

        /// <summary>
        /// No operand.
        /// </summary>
        InlineNone = 5,

        /// <summary>
        /// The operand is reserved and should not be used.
        /// </summary>
        [Obsolete("OperandType.InlinePhi has been deprecated and is not supported.")]
        InlinePhi = 6,

        /// <summary>
        /// The operand is a 64-bit IEEE floating point number.
        /// </summary>
        InlineR = 7,

        /// <summary>
        /// The operand is a 32-bit metadata signature token.
        /// </summary>
        InlineSig = 9,

        /// <summary>
        /// The operand is a 32-bit metadata string token.
        /// </summary>
        InlineString = 10,

        /// <summary>
        /// The operand is the 32-bit integer argument to a switch instruction.
        /// </summary>
        InlineSwitch = 11,

        /// <summary>
        /// The operand is a <c>FieldRef</c>, <c>MethodRef</c>, or <c>TypeRef</c> token.
        /// </summary>
        InlineTok = 12,

        /// <summary>
        /// The operand is a 32-bit metadata token for a type.
        /// </summary>
        InlineType = 13,

        /// <summary>
        /// The operand is a 16-bit integer containing the ordinal of a local variable or an argument.
        /// </summary>
        InlineVar = 14,

        /// <summary>
        /// The operand is an 8-bit integer branch target.
        /// </summary>
        ShortInlineBrTarget = 15,

        /// <summary>
        /// The operand is an 8-bit integer.
        /// </summary>
        ShortInlineI = 16,

        /// <summary>
        /// The operand is a 32-bit IEEE floating point number.
        /// </summary>
        ShortInlineR = 17,

        /// <summary>
        /// The operand is an 8-bit integer containing the ordinal of a local variable or an argument.
        /// </summary>
        ShortInlineVar = 18,

    }

}
