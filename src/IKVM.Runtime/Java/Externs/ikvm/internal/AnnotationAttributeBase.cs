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

namespace IKVM.Java.Externs.ikvm.@internal
{

    static class AnnotationAttributeBase
    {

        public static object[] unescapeInvalidSurrogates(object[] def)
        {
            return (object[])AnnotationDefaultAttribute.Unescape(def);
        }

        public static object newAnnotationInvocationHandler(object type, object memberValues)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::sun.reflect.annotation.AnnotationInvocationHandler((global::java.lang.Class)type, (global::java.util.Map)memberValues);
#endif
        }

        public static object newAnnotationTypeMismatchExceptionProxy(string msg)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::sun.reflect.annotation.AnnotationTypeMismatchExceptionProxy(msg);
#endif
        }

    }

}
