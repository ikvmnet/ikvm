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

namespace IKVM.Runtime
{

    sealed partial class RuntimeManagedJavaType
    {

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        /// <summary>
        /// Implements a 'writeReplace' method for Java serialization, which serializes a .NET exceptions as a 'com.sun.xml.@internal.ws.developer.ServerSideException'.
        /// </summary>
        sealed class ExceptionWriteReplaceJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            internal ExceptionWriteReplaceJavaMethod(RuntimeJavaType declaringType) :
                base(declaringType, "writeReplace", "()Ljava.lang.Object;", null, declaringType.Context.JavaBase.TypeOfJavaLangObject, [], Modifiers.Private, MemberFlags.None)
            {

            }

            internal override bool IsDynamicOnly => true;

            internal override object Invoke(object obj, object[] args)
            {
                var x = (Exception)obj;
                var sse = new com.sun.xml.@internal.ws.developer.ServerSideException(ikvm.extensions.ExtensionMethods.getClass(x).getName(), x.Message);
                sse.initCause(x.InnerException);
                sse.setStackTrace(ikvm.extensions.ExtensionMethods.getStackTrace(x));
                return sse;
            }

        }

#endif

    }

}
