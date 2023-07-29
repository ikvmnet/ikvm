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

        class DynamicOnlyJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="returnType"></param>
            /// <param name="parameterTypes"></param>
            /// <param name="flags"></param>
            internal DynamicOnlyJavaMethod(RuntimeJavaType declaringType, string name, string sig, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, MemberFlags flags) :
                base(declaringType, name, sig, null, returnType, parameterTypes, Modifiers.Public | Modifiers.Abstract, flags)
            {

            }

            internal sealed override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            [HideFromJava]
            internal sealed override object Invoke(object obj, object[] args)
            {
                // a DynamicOnlyMethodWrapper is an interface method, but now that we've been called on an actual object instance,
                // we can resolve to a real method and call that instead
                var tw = RuntimeJavaType.FromClass(IKVM.Java.Externs.ikvm.runtime.Util.getClassFromObject(obj));
                var mw = tw.GetMethodWrapper(this.Name, this.Signature, true);
                if (mw == null || mw.IsStatic)
                    throw new java.lang.AbstractMethodError(tw.Name + "." + this.Name + this.Signature);

                if (!mw.IsPublic)
                    throw new java.lang.IllegalAccessError(tw.Name + "." + this.Name + this.Signature);

                mw.Link();
                mw.ResolveMethod();
                return mw.Invoke(obj, args);
            }

#endif

        }

    }

}
