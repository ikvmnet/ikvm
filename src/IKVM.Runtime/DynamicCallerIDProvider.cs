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
using System.Diagnostics;
using System.Reflection;

namespace IKVM.Runtime
{

#if !IMPORTER

    sealed class DynamicCallerIDProvider
    {

        // this object acts as a capability that is passed to trusted code to allow the DynamicCallerID()
        // method to be public without giving untrusted code the ability to forge a CallerID token
        internal static readonly DynamicCallerIDProvider Instance = new DynamicCallerIDProvider();

        private DynamicCallerIDProvider() { }

        internal ikvm.@internal.CallerID GetCallerID()
        {
            for (int i = 0; ;)
            {
                MethodBase method = new StackFrame(i++, false).GetMethod();
                if (method == null)
                {
#if !FIRST_PASS
                    return ikvm.@internal.CallerID.create(null, null);
#endif
                }
                if (IKVM.Java.Externs.sun.reflect.Reflection.IsHideFromStackWalk(method))
                {
                    continue;
                }
                RuntimeJavaType caller = RuntimeClassLoader.GetWrapperFromType(method.DeclaringType);
                return CreateCallerID(caller.Host ?? caller);
            }
        }

        internal static ikvm.@internal.CallerID CreateCallerID(RuntimeJavaType tw)
        {
#if FIRST_PASS
            return null;
#else
            return ikvm.@internal.CallerID.create(tw.ClassObject, tw.GetClassLoader().GetJavaClassLoader());
#endif
        }

    }

#endif

}
