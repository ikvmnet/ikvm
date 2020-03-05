/*
  Copyright (C) 2012 Jeroen Frijters

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
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security;

sealed class ThemingActivationContext : IDisposable
{
	private IntPtr cookie;

	[SecurityCritical]
	public ThemingActivationContext()
	{
		if (Context.hActCtx != IntPtr.Zero)
		{
			IntPtr tmp;
			if (ActivateActCtx(Context.hActCtx, out tmp))
			{
				cookie = tmp;
			}
		}
	}

	[SecuritySafeCritical]
	public void Dispose()
	{
		IntPtr tmp = Interlocked.Exchange(ref cookie, IntPtr.Zero);
		if (tmp != IntPtr.Zero)
		{
			DeactivateActCtx(0, tmp);
		}
	}

	static class Context
	{
		internal static readonly IntPtr hActCtx;

		[SecurityCritical]
		static Context()
		{
			ACTCTX actctx = new ACTCTX();
			actctx.cbSize = Marshal.SizeOf(typeof(ACTCTX));
			actctx.lpSource = Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "XPThemes.manifest");
			IntPtr handle = CreateActCtx(ref actctx);
			if (handle != new IntPtr(-1))
			{
				hActCtx = handle;
			}
		}

		struct ACTCTX
		{
			internal int cbSize;
			internal uint dwFlags;
			internal string lpSource;
			internal ushort wProcessorArchitecture;
			internal ushort wLangId;
			internal string lpAssemblyDirectory;
			internal string lpResourceName;
			internal string lpApplicationName;
		}

		[DllImport("kernel32.dll")]
		extern static IntPtr CreateActCtx(ref ACTCTX actctx);
	}

	[DllImport("kernel32.dll")]
	extern static bool ActivateActCtx(IntPtr hActCtx, out IntPtr lpCookie);
	[DllImport("kernel32.dll")]
	extern static bool DeactivateActCtx(uint dwFlags, IntPtr lpCookie);
}
