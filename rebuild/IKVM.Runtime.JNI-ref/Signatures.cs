/*
  Copyright (C) 2010-2014 Jeroen Frijters

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
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    [StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVMOption
	{

	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe struct JavaVMInitArgs
	{

	}

	public class JNI
	{

		public struct Frame
		{

			public IntPtr Enter(ikvm.@internal.CallerID callerID)
			{
				throw null;
			}

			public void Leave()
			{
				throw null;
			}

			public static IntPtr GetFuncPtr(ikvm.@internal.CallerID callerID, string clazz, string name, string sig)
			{
				throw null;
			}

			public IntPtr MakeLocalRef(object obj)
			{
				throw null;
			}

			// NOTE this method has the wrong name, it should unwrap *any* jobject reference type (local and global)
			public object UnwrapLocalRef(IntPtr p)
			{
				throw null;
			}

		}

	}

	internal sealed class JniHelper
	{

		internal static readonly object JniLock;

		internal unsafe static long LoadLibrary(string filename, object loader)
		{
			throw null;
		}

		internal static void UnloadLibrary(long handle, object loader)
		{
			throw null;
		}

	}

}
