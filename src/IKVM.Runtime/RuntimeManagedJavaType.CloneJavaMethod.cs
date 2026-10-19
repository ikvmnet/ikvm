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
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        /// <summary>
        /// Exposes a virtual 'clone' method for objects which implement ICloneable.
        /// </summary>
        sealed class CloneJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            internal CloneJavaMethod(RuntimeManagedJavaType declaringType) :
                base(declaringType, "clone", "()Ljava.lang.Object;", null, declaringType.Context.JavaBase.TypeOfJavaLangObject, [], Modifiers.Protected | Modifiers.Final, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Isinst, DeclaringType.Context.JavaBase.TypeOfJavaLangCloneable.TypeAsBaseType);
                var label1 = ilgen.DefineLabel();
                ilgen.EmitBrtrue(label1);
                var label2 = ilgen.DefineLabel();
                ilgen.EmitBrfalse(label2);
                ilgen.EmitThrow("java.lang.CloneNotSupportedException");
                ilgen.MarkLabel(label2);
                ilgen.EmitThrow("java.lang.NullPointerException");
                ilgen.MarkLabel(label1);
                ilgen.Emit(OpCodes.Call, DeclaringType.Context.Types.Object.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                EmitCall(ilgen);
            }

#endif

        }

    }

}
