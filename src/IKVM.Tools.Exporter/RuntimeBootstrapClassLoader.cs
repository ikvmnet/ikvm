/*
  Copyright (C) 2002-2013 Jeroen Frijters

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
using IKVM.Runtime;

namespace IKVM.Runtime
{

    sealed class RuntimeBootstrapClassLoader : RuntimeClassLoader
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal RuntimeBootstrapClassLoader(RuntimeContext context) :
            base(context, CodeGenOptions.None, null)
        {
            var javaLangObject = new RuntimeStubJavaType(context, Modifiers.Public, "java.lang.Object", null, true);
            SetRemappedType(Context.Resolver.ResolveCoreType(typeof(object).FullName), javaLangObject);
            SetRemappedType(Context.Resolver.ResolveCoreType(typeof(string).FullName), new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Final, "java.lang.String", javaLangObject, true));
            SetRemappedType(Context.Resolver.ResolveCoreType(typeof(Exception).FullName), new RuntimeStubJavaType(context, Modifiers.Public, "java.lang.Throwable", javaLangObject, true));
            SetRemappedType(Context.Resolver.ResolveCoreType(typeof(IComparable).FullName), new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.Comparable", null, true));
            
            var autoCloseable = new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.AutoCloseable", null, true);
            autoCloseable.SetMethods(new [] { new RuntimeSimpleCallJavaMethod(autoCloseable, "close", "()V", Context.Resolver.ResolveCoreType(typeof(IDisposable).FullName).GetMethod("Dispose"), context.PrimitiveJavaTypeFactory.VOID, Array.Empty<RuntimeJavaType>(), Modifiers.Public | Modifiers.Abstract, MemberFlags.None, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt) });
            SetRemappedType(Context.Resolver.ResolveCoreType(typeof(IDisposable).FullName), autoCloseable);

            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public, "java.lang.Enum", javaLangObject, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.annotation.Annotation", null, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Final, "java.lang.Class", javaLangObject, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract, "java.lang.invoke.MethodHandle", javaLangObject, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Final, "java.lang.invoke.MethodType", javaLangObject, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract, "ikvm.internal.CallerID", javaLangObject, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.io.Serializable", null, false));
            RegisterInitiatingLoader(new RuntimeStubJavaType(context, Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.Cloneable", null, false));
        }

    }

}
