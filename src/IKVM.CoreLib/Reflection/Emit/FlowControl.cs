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

namespace IKVM.Reflection.Emit
{

    /// <summary>
    /// Describes how an instruction alters the flow of control.
    /// </summary>
    enum FlowControl
    {

        /// <summary>
        /// Branch instruction.
        /// </summary>
        Branch = 0,

        /// <summary>
        /// Break instruction.
        /// </summary>
        Break = 1,

        /// <summary>
        /// Call instruction.
        /// </summary>
        Call = 2,

        /// <summary>
        /// Conditional branch instruction.
        /// </summary>
        Cond_Branch = 3,

        /// <summary>
        /// Provides information about a subsequent instruction, as prefixes such as <c>unaligned.</c> do.
        /// </summary>
        Meta = 4,

        /// <summary>
        /// Normal flow of control.
        /// </summary>
        Next = 5,

        /// <summary>
        /// Return instruction.
        /// </summary>
        Return = 7,

        /// <summary>
        /// Exception throw instruction.
        /// </summary>
        Throw = 8,

    }

}
