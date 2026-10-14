/*
  Copyright (C) 2011-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{

    /// <summary>
    /// The exception that is thrown when an operation is attempted on a member that could not be resolved, such as a
    /// member of a missing type or of an assembly that could not be found.
    /// </summary>
    [Serializable]
    sealed class MissingMemberException : InvalidOperationException
    {

        [NonSerialized]
        readonly MemberInfo _member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member">The missing member on which the unsupported operation was attempted.</param>
        internal MissingMemberException(MemberInfo member) :
            base("Member '" + member + "' is a missing member and does not support the requested operation.")
        {
            _member = member;
        }

        /// <summary>
        /// Gets the missing member on which the unsupported operation was attempted.
        /// </summary>
        public MemberInfo MemberInfo => _member;

    }

}
