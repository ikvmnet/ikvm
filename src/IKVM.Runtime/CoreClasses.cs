/*
  Copyright (C) 2004-2015 Jeroen Frijters

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

namespace IKVM.Runtime
{

    /// <summary>
    /// Contains references to common Java base types.
    /// </summary>
    internal class CoreClasses
    {

        readonly RuntimeContext context;

        public readonly RuntimeJavaType cliSystemObject;
        public readonly RuntimeJavaType cliSystemException;
        public readonly RuntimeJavaType ikvmInternalCallerID;
        public readonly RuntimeJavaType javaIoSerializable;
        public readonly RuntimeJavaType javaLangObject;
        public readonly RuntimeJavaType javaLangString;
        public readonly RuntimeJavaType javaLangClass;
        public readonly RuntimeJavaType javaLangCloneable;
        public readonly RuntimeJavaType javaLangThrowable;
        public readonly RuntimeJavaType javaLangInvokeMethodHandle;
        public readonly RuntimeJavaType javaLangInvokeMethodType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public CoreClasses(RuntimeContext context)
        {
            this.context = context;
            cliSystemObject = context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(context.Types.Object);
            cliSystemException = context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(context.Types.Exception);
            ikvmInternalCallerID = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.CallerID");
            javaIoSerializable = context.ClassLoaderFactory.LoadClassCritical("java.io.Serializable");
            javaLangObject = context.ClassLoaderFactory.LoadClassCritical("java.lang.Object");
            javaLangString = context.ClassLoaderFactory.LoadClassCritical("java.lang.String");
            javaLangClass = context.ClassLoaderFactory.LoadClassCritical("java.lang.Class");
            javaLangCloneable = context.ClassLoaderFactory.LoadClassCritical("java.lang.Cloneable");
            javaLangThrowable = context.ClassLoaderFactory.LoadClassCritical("java.lang.Throwable");
            javaLangInvokeMethodHandle = context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodHandle");
            javaLangInvokeMethodType = context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodType");
        }

    }

}
