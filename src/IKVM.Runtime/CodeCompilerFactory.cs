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

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif


namespace IKVM.Runtime
{

    /// <summary>
    /// Manages instances of <see cref="CodeCompiler"/>.
    /// </summary>
    class CodeCompilerFactory
    {

        readonly RuntimeContext context;
        readonly bool bootstrap;

        MethodInfo unmapExceptionMethod;
        MethodInfo fixateExceptionMethod;
        MethodInfo suppressFillInStackTraceMethod;
        MethodInfo getTypeFromHandleMethod;
        MethodInfo getTypeMethod;
        MethodInfo keepAliveMethod;
        RuntimeJavaMethod getClassFromTypeHandle;
        RuntimeJavaMethod getClassFromTypeHandle2;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="bootstrap"></param>
        public CodeCompilerFactory(RuntimeContext context, bool bootstrap)
        {
            this.context = context;
            this.bootstrap = bootstrap;

            if (bootstrap && Throwable.TypeAsBaseType is TypeBuilder)
                foreach (var m in Throwable.GetMethods())
                    m.Link();

            GetClassFromTypeHandle.Link();
            GetClassFromTypeHandle2.Link();
        }

        public RuntimeJavaType Throwable => context.JavaBase.TypeOfjavaLangThrowable;

        public MethodInfo UnmapExceptionMethod => unmapExceptionMethod ??= bootstrap ? (MethodInfo)Throwable.GetMethodWrapper("__<unmap>", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).GetMethod() : Throwable.TypeAsBaseType.GetMethod("__<unmap>", new Type[] { context.Types.Exception });

        public MethodInfo FixateExceptionMethod => fixateExceptionMethod ??= bootstrap ? (MethodInfo)Throwable.GetMethodWrapper("__<fixate>", "(Ljava.lang.Throwable;)Ljava.lang.Throwable;", false).GetMethod() : Throwable.TypeAsBaseType.GetMethod("__<fixate>", new Type[] { context.Types.Exception });

        public MethodInfo SuppressFillInStackTraceMethod => suppressFillInStackTraceMethod ??= bootstrap ? (MethodInfo)Throwable.GetMethodWrapper("__<suppressFillInStackTrace>", "()V", false).GetMethod() : Throwable.TypeAsBaseType.GetMethod("__<suppressFillInStackTrace>", Type.EmptyTypes);

        public MethodInfo GetTypeFromHandleMethod => getTypeFromHandleMethod ??= context.Types.Type.GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public, null, new Type[] { context.Types.RuntimeTypeHandle }, null);

        public MethodInfo GetTypeMethod => getTypeMethod ??= context.Types.Object.GetMethod("GetType", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

        public MethodInfo KeepAliveMethod => keepAliveMethod ??= context.Resolver.ResolveCoreType(typeof(GC).FullName).GetMethod("KeepAlive", BindingFlags.Static | BindingFlags.Public, null, new Type[] { context.Types.Object }, null);

        public RuntimeJavaMethod GetClassFromTypeHandle => getClassFromTypeHandle ??= context.ClassLoaderFactory.LoadClassCritical("ikvm.runtime.Util").GetMethodWrapper("getClassFromTypeHandle", "(Lcli.System.RuntimeTypeHandle;)Ljava.lang.Class;", false);

        public RuntimeJavaMethod GetClassFromTypeHandle2 => getClassFromTypeHandle2 ??= context.ClassLoaderFactory.LoadClassCritical("ikvm.runtime.Util").GetMethodWrapper("getClassFromTypeHandle", "(Lcli.System.RuntimeTypeHandle;I)Ljava.lang.Class;", false);

    }

}
