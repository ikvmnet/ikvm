// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Describes the types of the IL instructions.
    /// </summary>
    enum OpCodeType
    {

        /// <summary>
        /// This enumerator value is reserved and should not be used.
        /// </summary>
        [Obsolete("OpCodeType.Annotation has been deprecated and is not supported.")]
        Annotation = 0,

        /// <summary>
        /// These are IL instructions that are used as a synonym for other IL instructions. For example, <c>ldarg.0</c>
        /// represents the <c>ldarg</c> instruction with an argument of 0.
        /// </summary>
        Macro = 1,

        /// <summary>
        /// Describes a reserved IL instruction.
        /// </summary>
        Nternal = 2,

        /// <summary>
        /// Describes an IL instruction that applies to objects.
        /// </summary>
        Objmodel = 3,

        /// <summary>
        /// Describes a prefix instruction that modifies the behavior of the following instruction.
        /// </summary>
        Prefix = 4,

        /// <summary>
        /// Describes a built-in instruction.
        /// </summary>
        Primitive = 5,

    }

}
