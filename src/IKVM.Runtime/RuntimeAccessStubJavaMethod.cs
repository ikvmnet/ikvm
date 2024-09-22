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
using System.Reflection.Emit;

using IKVM.Attributes;
using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    sealed class RuntimeAccessStubJavaMethod : RuntimeSmartJavaMethod
    {

        readonly IMethodSymbol stubVirtual;
        readonly IMethodSymbol stubNonVirtual;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="sig"></param>
        /// <param name="core"></param>
        /// <param name="stubVirtual"></param>
        /// <param name="stubNonVirtual"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <param name="modifiers"></param>
        /// <param name="flags"></param>
        internal RuntimeAccessStubJavaMethod(RuntimeJavaType declaringType, string name, string sig, IMethodSymbol core, IMethodSymbol stubVirtual, IMethodSymbol stubNonVirtual, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, MemberFlags flags) :
            base(declaringType, name, sig, core, returnType, parameterTypes, modifiers, flags)
        {
            this.stubVirtual = stubVirtual;
            this.stubNonVirtual = stubNonVirtual;
        }

#if EMITTERS

        protected override void CallImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Call, stubNonVirtual);
        }

        protected override void CallvirtImpl(CodeEmitter ilgen)
        {
            ilgen.Emit(OpCodes.Callvirt, stubVirtual);
        }

#endif 

    }

}
