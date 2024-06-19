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
using System;

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    abstract class RuntimeSmartJavaMethod : RuntimeJavaMethod
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
        internal RuntimeSmartJavaMethod(RuntimeJavaType declaringType, string name, string sig, MethodBase method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, MemberFlags flags) :
            base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, flags)
        {

        }

#if EMITTERS

        internal sealed override void EmitCall(CodeEmitter ilgen)
        {
            AssertLinked();
            CallImpl(ilgen);
        }

        protected virtual void CallImpl(CodeEmitter ilgen)
        {
            throw new InvalidOperationException();
        }

        internal sealed override void EmitCallvirt(CodeEmitter ilgen)
        {
            AssertLinked();

            // callvirt isn't allowed on a value type
            // (we don't need to check for a null reference, because we're always dealing with an unboxed value)
            if (DeclaringType.IsNonPrimitiveValueType)
                CallImpl(ilgen);
            else
                CallvirtImpl(ilgen);
        }

        protected virtual void CallvirtImpl(CodeEmitter ilgen)
        {
            throw new InvalidOperationException();
        }

        internal sealed override void EmitNewobj(CodeEmitter ilgen)
        {
            AssertLinked();
            NewobjImpl(ilgen);
            if (DeclaringType.IsNonPrimitiveValueType)
            {
                DeclaringType.EmitBox(ilgen);
            }
        }

        protected virtual void NewobjImpl(CodeEmitter ilgen)
        {
            throw new InvalidOperationException();
        }

#endif // EMITTERS

    }

}
