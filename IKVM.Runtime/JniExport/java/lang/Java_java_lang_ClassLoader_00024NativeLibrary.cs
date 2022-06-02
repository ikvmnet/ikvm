/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.Runtime.CompilerServices;
using System.Security;
using System.Threading;

using IKVM.Internal;
using IKVM.Runtime.Vfs;

static class Java_java_lang_ClassLoader_00024NativeLibrary
{

    public static void load(object thisNativeLibrary, string name, bool isBuiltin)
    {
#if !FIRST_PASS
		if (VfsTable.Default.IsPath(name))
		{
			// we fake success for native libraries loaded from VFS
			((java.lang.ClassLoader.NativeLibrary)thisNativeLibrary).loaded = true;
		}
		else
		{
			doLoad(thisNativeLibrary, name);
		}
#endif
    }

#if !FIRST_PASS
	// we don't want to inline this method, because that would needlessly cause IKVM.Runtime.JNI.dll to be loaded when loading a fake native library from VFS
	[MethodImpl(MethodImplOptions.NoInlining)]
	[SecuritySafeCritical]
	private static void doLoad(object thisNativeLibrary, string name)
	{
		java.lang.ClassLoader.NativeLibrary lib = (java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
		lib.handle = IKVM.Runtime.JniHelper.LoadLibrary(name, TypeWrapper.FromClass(java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
		lib.loaded = true;
	}
#endif

    public static long find(object thisNativeLibrary, string name)
    {
        // TODO
        throw new NotImplementedException();
    }

    [SecuritySafeCritical]
    public static void unload(object thisNativeLibrary, string name, bool isBuiltin)
    {
#if !FIRST_PASS
		java.lang.ClassLoader.NativeLibrary lib = (java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
		long handle = Interlocked.Exchange(ref lib.handle, 0);
		if (handle != 0)
		{
			IKVM.Runtime.JniHelper.UnloadLibrary(handle, TypeWrapper.FromClass(java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
		}
#endif
    }

    public static string findBuiltinLib(string name)
    {
        return null;
    }
}
