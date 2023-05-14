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
using IKVM.Internal;
using IKVM.Runtime;

sealed class BootstrapBootstrapClassLoader : ClassLoaderWrapper
{

    internal BootstrapBootstrapClassLoader() :
        base(CodeGenOptions.None, null)
    {
        TypeWrapper javaLangObject = new StubTypeWrapper(Modifiers.Public, "java.lang.Object", null, true);
        SetRemappedType(JVM.Import(typeof(object)), javaLangObject);
        SetRemappedType(JVM.Import(typeof(string)), new StubTypeWrapper(Modifiers.Public | Modifiers.Final, "java.lang.String", javaLangObject, true));
        SetRemappedType(JVM.Import(typeof(Exception)), new StubTypeWrapper(Modifiers.Public, "java.lang.Throwable", javaLangObject, true));
        SetRemappedType(JVM.Import(typeof(IComparable)), new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.Comparable", null, true));
        TypeWrapper tw = new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.AutoCloseable", null, true);
        tw.SetMethods(new MethodWrapper[] { new SimpleCallMethodWrapper(tw, "close", "()V", JVM.Import(typeof(IDisposable)).GetMethod("Dispose"), PrimitiveTypeWrapper.VOID, TypeWrapper.EmptyArray, Modifiers.Public | Modifiers.Abstract, MemberFlags.None, SimpleOpCode.Callvirt, SimpleOpCode.Callvirt) });
        SetRemappedType(JVM.Import(typeof(IDisposable)), tw);

        RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public, "java.lang.Enum", javaLangObject, false));
        RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract | Modifiers.Interface, "java.lang.annotation.Annotation", null, false));
        RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Final, "java.lang.Class", javaLangObject, false));
        RegisterInitiatingLoader(new StubTypeWrapper(Modifiers.Public | Modifiers.Abstract, "java.lang.invoke.MethodHandle", javaLangObject, false));
    }

}
