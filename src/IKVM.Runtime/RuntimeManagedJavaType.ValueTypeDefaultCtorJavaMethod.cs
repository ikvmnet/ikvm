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

#if IMPORTER
using IKVM.Reflection.Emit;
#else
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        sealed class ValueTypeDefaultCtorJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            internal ValueTypeDefaultCtorJavaMethod(RuntimeManagedJavaType declaringType) :
                base(declaringType, "<init>", "()V", null, declaringType.Context.PrimitiveJavaTypeFactory.VOID, [], Modifiers.Public, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                var local = ilgen.DeclareLocal(DeclaringType.TypeAsTBD);
                ilgen.Emit(OpCodes.Ldloc, local);
                ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Pop);
            }

#endif

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object CreateInstance(object[] args)
            {
                return Activator.CreateInstance(DeclaringType.TypeAsTBD);
            }

#endif

        }

    }

}
