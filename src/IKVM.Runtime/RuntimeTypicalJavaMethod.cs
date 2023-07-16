/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

    sealed class RuntimeTypicalJavaMethod : RuntimeSmartJavaMethod
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="method"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        internal RuntimeTypicalJavaMethod(RuntimeJavaType declaringType, string name, string sig, MethodBase method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, MemberFlags flags) :
            base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
        {

        }

#if EMITTERS

        protected override void CallImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, GetMethod());
        }

        protected override void CallvirtImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Callvirt, GetMethod());
        }

        protected override void NewobjImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Newobj, GetMethod());
        }

#endif 

    }

}
