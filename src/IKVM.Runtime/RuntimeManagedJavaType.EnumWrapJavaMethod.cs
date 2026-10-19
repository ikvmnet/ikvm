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

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed class EnumWrapJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="fieldType"></param>
            internal EnumWrapJavaMethod(RuntimeManagedJavaType declaringType, RuntimeJavaType fieldType) :
                base(declaringType, "wrap", "(" + fieldType.SignatureName + ")" + declaringType.SignatureName, null, declaringType, [fieldType], Modifiers.Static | Modifiers.Public, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // We don't actually need to do anything here!
                // The compiler will insert a boxing operation after calling us and that will
                // result in our argument being boxed (since that's still sitting on the stack).
            }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object Invoke(object obj, object[] args)
            {
                return Enum.ToObject(DeclaringType.TypeAsTBD, args[0]);
            }

#endif

        }

    }

}
