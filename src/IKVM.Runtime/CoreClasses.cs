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

        RuntimeJavaType typeOfCliSystemObject;
        RuntimeJavaType typeOfCliSystemException;
        RuntimeJavaType typeOfIkvmInternalCallerID;
        RuntimeJavaType typeOfJavaIoSerializable;
        RuntimeJavaType typeOfJavaLangObject;
        RuntimeJavaType typeOfJavaLangString;
        RuntimeJavaType typeOfJavaLangClass;
        RuntimeJavaType typeOfJavaLangCloneable;
        RuntimeJavaType typeOfjavaLangThrowable;
        RuntimeJavaType typeOfJavaLangInvokeMethodHandle;
        RuntimeJavaType typeOfJavaLangInvokeMethodType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public CoreClasses(RuntimeContext context)
        {
            this.context = context;
        }

        public RuntimeJavaType TypeOfCliSystemObject => typeOfCliSystemObject ??= context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(context.Types.Object);

        public RuntimeJavaType TypeOfCliSystemException => typeOfCliSystemException ??= context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(context.Types.Exception);

        public RuntimeJavaType TypeOfIkvmInternalCallerID => typeOfIkvmInternalCallerID ??= context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.CallerID");

        public RuntimeJavaType TypeOfJavaIoSerializable => typeOfJavaIoSerializable ??= context.ClassLoaderFactory.LoadClassCritical("java.io.Serializable");

        public RuntimeJavaType TypeOfJavaLangObject => typeOfJavaLangObject ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.Object");

        public RuntimeJavaType TypeOfJavaLangString => typeOfJavaLangString ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.String");

        public RuntimeJavaType TypeOfJavaLangClass => typeOfJavaLangClass ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.Class");

        public RuntimeJavaType TypeOfJavaLangCloneable => typeOfJavaLangCloneable ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.Cloneable");

        public RuntimeJavaType TypeOfjavaLangThrowable => typeOfjavaLangThrowable ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.Throwable");

        public RuntimeJavaType TypeOfJavaLangInvokeMethodHandle => typeOfJavaLangInvokeMethodHandle ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodHandle");

        public RuntimeJavaType TypeOfJavaLangInvokeMethodType => typeOfJavaLangInvokeMethodType ??= context.ClassLoaderFactory.LoadClassCritical("java.lang.invoke.MethodType");

    }

}
