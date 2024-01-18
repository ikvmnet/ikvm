﻿/*
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

    [Serializable]
    public sealed class MissingMemberException : InvalidOperationException
    {

        [NonSerialized]
        readonly MemberInfo member;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="member"></param>
        internal MissingMemberException(MemberInfo member) :
            base("Member '" + member + "' is a missing member and does not support the requested operation.")
        {
            this.member = member;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        MissingMemberException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) :
            base(info, context)
        {

        }

        public MemberInfo MemberInfo
        {
            get { return member; }
        }

    }

}
