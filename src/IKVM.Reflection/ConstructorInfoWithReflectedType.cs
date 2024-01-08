/*
  Copyright (C) 2009-2012 Jeroen Frijters

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
using System.Diagnostics;

namespace IKVM.Reflection
{

    sealed class ConstructorInfoWithReflectedType : ConstructorInfo
    {

        readonly Type reflectedType;
        readonly ConstructorInfo ctor;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="ctor"></param>
        internal ConstructorInfoWithReflectedType(Type reflectedType, ConstructorInfo ctor)
        {
            Debug.Assert(reflectedType != ctor.DeclaringType);
            this.reflectedType = reflectedType;
            this.ctor = ctor;
        }

        public override bool Equals(object obj)
        {
            var other = obj as ConstructorInfoWithReflectedType;
            return other != null && other.reflectedType == reflectedType && other.ctor == ctor;
        }

        public override int GetHashCode()
        {
            return reflectedType.GetHashCode() ^ ctor.GetHashCode();
        }

        public override Type ReflectedType
        {
            get { return reflectedType; }
        }

        internal override MethodInfo GetMethodInfo()
        {
            return ctor.GetMethodInfo();
        }

        internal override MethodInfo GetMethodOnTypeDefinition()
        {
            return ctor.GetMethodOnTypeDefinition();
        }

    }

}
