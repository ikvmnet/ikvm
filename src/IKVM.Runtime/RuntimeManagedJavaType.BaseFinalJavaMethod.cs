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

using IKVM.Attributes;

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

        /// <summary>
        /// Virtual final methods that represent overrides.
        /// </summary>
        sealed class BaseFinalJavaMethod : RuntimeJavaMethod
        {

            readonly RuntimeJavaMethod method;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="method"></param>
            internal BaseFinalJavaMethod(RuntimeManagedJavaType declaringType, RuntimeJavaMethod method) :
                base(declaringType, method.Name, method.Signature, null, null, null, (method.Modifiers & ~Modifiers.Abstract) | Modifiers.Final, MemberFlags.None)
            {
                this.method = method;
            }

            protected override void DoLinkMethod()
            {
            }

#if EMITTERS

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // we direct EmitCall to EmitCallvirt, because we always want to end up at the instancehelper method
                // (EmitCall would go to our alter ego .NET type and that wouldn't be legal)
                method.EmitCallvirt(ilgen);
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                method.EmitCallvirt(ilgen);
            }

#endif 

        }

    }

}
