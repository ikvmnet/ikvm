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

        sealed class ByRefJavaMethod : RuntimeSmartJavaMethod
        {

#if !IMPORTER
            readonly bool[] byrefs;
#endif
            readonly Type[] args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <param name="byrefs"></param>
            /// <param name="declaringType"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="method"></param>
            /// <param name="returnType"></param>
            /// <param name="parameterTypes"></param>
            /// <param name="modifiers"></param>
            /// <param name="hideFromReflection"></param>
            internal ByRefJavaMethod(Type[] args, bool[] byrefs, RuntimeJavaType declaringType, string name, string sig, MethodBase method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, bool hideFromReflection) :
                base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
            {
                this.args = args;
#if !IMPORTER
                this.byrefs = byrefs;
#endif
            }

#if EMITTERS
            protected override void CallImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Call, GetMethod());
            }

            protected override void CallvirtImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Callvirt, GetMethod());
            }

            protected override void NewobjImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Newobj, GetMethod());
            }

            void ConvertByRefArgs(CodeEmitter ilgen)
            {
                var locals = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    var type = args[i];
                    if (type.IsByRef)
                        type = RuntimeArrayJavaType.MakeArrayType(type.GetElementType(), 1);

                    locals[i] = ilgen.DeclareLocal(type);
                    ilgen.Emit(OpCodes.Stloc, locals[i]);
                }

                for (int i = 0; i < args.Length; i++)
                {
                    ilgen.Emit(OpCodes.Ldloc, locals[i]);
                    if (args[i].IsByRef)
                    {
                        ilgen.Emit(OpCodes.Ldc_I4_0);
                        ilgen.Emit(OpCodes.Ldelema, args[i].GetElementType());
                    }
                }
            }

#endif

        }

    }

}
