﻿/*
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
using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    sealed class RuntimeSimpleCallJavaMethod : RuntimeJavaMethod
    {

        readonly SimpleOpCode call;
        readonly SimpleOpCode callvirt;

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
        /// <param name="call"></param>
        /// <param name="callvirt"></param>
        internal RuntimeSimpleCallJavaMethod(RuntimeJavaType declaringType, string name, string sig, IMethodSymbol method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, MemberFlags flags, SimpleOpCode call, SimpleOpCode callvirt) :
            base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
        {
            this.call = call;
            this.callvirt = callvirt;
        }

#if EMITTERS

        internal override void EmitCall(CodeEmitter ilgen)
        {
            ilgen.Emit(SimpleOpCodeToOpCode(call), GetMethod());
        }

        internal override void EmitCallvirt(CodeEmitter ilgen)
        {
            ilgen.Emit(SimpleOpCodeToOpCode(callvirt), GetMethod());
        }

#endif

    }

}
