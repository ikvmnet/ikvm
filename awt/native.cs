/*
  Copyright (C) 2007, 2008, 2010 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace IKVM.NativeCode.sun.awt
{
	static class KeyboardFocusManagerPeerImpl
	{
		public static object getNativeFocusedWindow() { return null; }
		public static object getNativeFocusOwner() { return null; }
		public static void clearNativeGlobalFocusOwner(object activeWindow) { }
	}

	static class SunToolkit
	{
		public static void closeSplashScreen() { }
	}
}

namespace IKVM.NativeCode.sun.awt.shell
{
	/// <summary>
	/// This class should use only on Windows that we can access shell32.dll
	/// </summary>
	static class Win32ShellFolder2
	{
		private const uint SHGFI_LARGEICON = 0x0;
		private const uint SHGFI_SMALLICON = 0x1;
		private const uint SHGFI_ICON = 0x100;
		private const uint SHGFI_TYPENAME = 0x400;
		private const uint SHGFI_ATTRIBUTES = 0x800;

		private const uint IMAGE_BITMAP = 0;
		private const uint IMAGE_ICON = 1;

		private static readonly IntPtr hmodShell32;
		private static readonly IntPtr hmodComctl32;
		private static readonly bool isXP;
		private static readonly bool isVista;

		[System.Security.SecuritySafeCritical]
		static Win32ShellFolder2()
		{
			hmodShell32 = LoadLibrary("shell32.dll");
			hmodComctl32 = LoadLibrary("comctl32.dll");
			isXP = Environment.OSVersion.Version >= new Version(5, 1);
			isVista = Environment.OSVersion.Version.Major >= 6;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct SHFILEINFO
		{
			internal IntPtr hIcon;
			internal IntPtr iIcon;
			internal uint dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
			internal string szTypeName;
		};

		[System.Security.SecurityCritical]
		private sealed class SafeGdiObjectHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
		{
			private SafeGdiObjectHandle()
				: base(true)
			{
			}

			[System.Security.SecurityCritical]
			protected override bool ReleaseHandle()
			{
				return DeleteObject(handle);
			}
		}

		[System.Security.SecurityCritical]
		private sealed class SafeDeviceContextHandle : Microsoft.Win32.SafeHandles.SafeHandleZeroOrMinusOneIsInvalid
		{
			[DllImport("user32.dll")]
			private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

			[DllImport("user32.dll")]
			private static extern SafeDeviceContextHandle GetDC(IntPtr hwnd);

			internal static SafeDeviceContextHandle Get()
			{
				return GetDC(IntPtr.Zero);
			}

			private SafeDeviceContextHandle()
				: base(true)
			{
			}

			[System.Security.SecurityCritical]
			protected override bool ReleaseHandle()
			{
				return ReleaseDC(IntPtr.Zero, handle) == 1;
			}
		}

		[DllImport("gdi32.dll")]
		private static extern int GetDIBits(SafeDeviceContextHandle hdc, IntPtr hbmp, uint uStartScan, uint cScanLines, int[] lpvBits, ref BITMAPINFO lpbmi, uint uUsage);

		[DllImport("gdi32.dll")]
		private static extern int GetDIBits(SafeDeviceContextHandle hdc, SafeGdiObjectHandle hbmp, uint uStartScan, uint cScanLines, int[] lpvBits, ref BITMAPINFO lpbmi, uint uUsage);

		[DllImport("gdi32.dll")]
		private static extern int GetObject(SafeGdiObjectHandle hgdiobj, int cbBuffer, ref BITMAPINFO lpvObject);

		[StructLayout(LayoutKind.Sequential)]
		private struct ICONINFO
		{
			internal bool fIcon;
			internal int xHotspot;
			internal int yHotspot;
			internal IntPtr hbmMask;
			internal IntPtr hbmColor;
		}

		[DllImport("user32.dll")]
		static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

		[StructLayout(LayoutKind.Sequential)]
		private struct BITMAPINFO
		{
			internal uint biSize;
			internal int biWidth;
			internal int biHeight;
			internal ushort biPlanes;
			internal ushort biBitCount;
			internal uint biCompression;
			internal uint biSizeImage;
			internal int biXPelsPerMeter;
			internal int biYPelsPerMeter;
			internal uint biClrUsed;
			internal uint biClrImportant;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
			internal uint[] cols;
		}

		[DllImport("shell32.dll")]
		private static extern int FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);

		[DllImport("shell32.dll")]
		private static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);

		[DllImport("user32.dll")]
		private static extern SafeGdiObjectHandle LoadImage(IntPtr hInstance, IntPtr uID, uint type, int width, int height, int load);

		[DllImport("user32.dll")]
		private static extern SafeGdiObjectHandle LoadImage(IntPtr hInstance, string lpszName, uint type, int width, int height, int load);

		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string Library);

		[DllImport("gdi32.dll")]
		private static extern bool DeleteObject(IntPtr hDc);

		/// <summary>
		/// Get the program to execute or open the file. If it is a exe then it is self
		/// </summary>
		/// <param name="path">path to the file</param>
		/// <returns></returns>
		[System.Security.SecuritySafeCritical]
		public static string getExecutableType(string path)
		{
			StringBuilder objResultBuffer = new StringBuilder(1024);
			int result = FindExecutable(path, path, objResultBuffer);
			if (result >= 32)
			{
				return objResultBuffer.ToString();
			}
			return null;
		}

		/// <summary>
		/// Get the type of a file or folder. On a file it depends on its extension.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		[System.Security.SecuritySafeCritical]
		public static string getFolderType(string path)
		{
			SHFILEINFO shinfo = new SHFILEINFO();
			if (SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_TYPENAME) == IntPtr.Zero)
			{
				return null;
			}
			return shinfo.szTypeName;
		}

		[System.Security.SecuritySafeCritical]
		public static Bitmap getIconBitmap(string path, bool getLargeIcon)
		{
			SHFILEINFO shinfo = new SHFILEINFO();
			if (SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ICON | (getLargeIcon ? 0 : SHGFI_SMALLICON)) == IntPtr.Zero)
			{
				return null;
			}
			int size = getLargeIcon ? 32 : 16;
			int[] iconPixels = getIconBits(shinfo.hIcon, size);
			DeleteObject(shinfo.hIcon);
			Bitmap bitmap = new Bitmap(size, size, PixelFormat.Format32bppArgb);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, size, size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			Marshal.Copy(iconPixels, 0, bitmapData.Scan0, iconPixels.Length);
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}

		[System.Security.SecurityCritical]
		private static int[] getIconBits(IntPtr hIcon, int iconSize)
		{
			ICONINFO iconInfo;
			if (GetIconInfo(hIcon, out iconInfo))
			{
				using (SafeDeviceContextHandle dc = SafeDeviceContextHandle.Get())
				{
					BITMAPINFO bmi = new BITMAPINFO();
					bmi.biSize = 40;
					bmi.biWidth = iconSize;
					bmi.biHeight = -iconSize;
					bmi.biPlanes = 1;
					bmi.biBitCount = 32;
					bmi.biCompression = 0;
					int intArrSize = iconSize * iconSize;
					int[] iconBits = new int[intArrSize];
					GetDIBits(dc, iconInfo.hbmColor, 0, (uint)iconSize, iconBits, ref bmi, 0);
					bool hasAlpha = false;
					if (isXP)
					{
						for (int i = 0; i < iconBits.Length; i++)
						{
							if ((iconBits[i] & 0xFF000000) != 0)
							{
								hasAlpha = true;
								break;
							}
						}
					}
					if (!hasAlpha)
					{
						int[] maskBits = new int[intArrSize];
						GetDIBits(dc, iconInfo.hbmMask, 0, (uint)iconSize, maskBits, ref bmi, 0);
						for (int i = 0; i < iconBits.Length; i++)
						{
							if (maskBits[i] == 0)
							{
								iconBits[i] = (int)((uint)iconBits[i] | 0xFF000000);
							}
						}
					}
					DeleteObject(iconInfo.hbmColor);
					DeleteObject(iconInfo.hbmMask);
					return iconBits;
				}
			}
			return null;
		}

		[System.Security.SecuritySafeCritical]
		public static int getAttribute(string path)
		{
			SHFILEINFO shinfo = new SHFILEINFO();
			if (SHGetFileInfo(path, 0, ref shinfo, (uint)Marshal.SizeOf(shinfo), SHGFI_ATTRIBUTES) == IntPtr.Zero)
			{
				return 0;
			}
			return (int)shinfo.dwAttributes;
		}

		[System.Security.SecuritySafeCritical]
		public static string getLinkLocation(string path)
		{
			using (ShellLink link = new ShellLink())
			{
				link.Load(path);
				return link.GetPath();
			}
		}

		[System.Security.SecuritySafeCritical]
		public static Bitmap getFileChooserBitmap()
		{
			if (hmodShell32 != IntPtr.Zero)
			{
				// Code copied from ShellFolder2.cpp Java_sun_awt_shell_Win32ShellFolder2_getFileChooserBitmapBits
				// Get a handle to an icon.
				SafeGdiObjectHandle hBitmap = null;
				try
				{
					hBitmap = isVista ?
						 LoadImage(hmodShell32, "IDB_TB_SH_DEF_16", IMAGE_BITMAP, 0, 0, 0) :
						 LoadImage(hmodShell32, (IntPtr)216, IMAGE_BITMAP, 0, 0, 0);
					if (hBitmap == null && hmodComctl32 != IntPtr.Zero)
					{
						hBitmap = LoadImage(hmodComctl32, (IntPtr)124, IMAGE_BITMAP, 0, 0, 0);
					}
					if (hBitmap != null)
					{
						BITMAPINFO bmi = new BITMAPINFO();
						GetObject(hBitmap, Marshal.SizeOf(bmi), ref bmi);
						int width = bmi.biWidth;
						int height = bmi.biHeight;
						bmi.biSize = 40;
						bmi.biHeight = -bmi.biHeight;
						bmi.biPlanes = 1;
						bmi.biBitCount = 32;
						bmi.biCompression = 0;
						using (SafeDeviceContextHandle dc = SafeDeviceContextHandle.Get())
						{
							int[] data = new int[width * height];
							GetDIBits(dc, hBitmap, (uint)0, (uint)height, data, ref bmi, 0);
							Bitmap bitmap = new Bitmap(data.Length / 16, 16, PixelFormat.Format32bppArgb);
							BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
							Marshal.Copy(data, 0, bitmapData.Scan0, data.Length);
							bitmap.UnlockBits(bitmapData);
							return bitmap;
						}
					}
				}
				finally
				{
					if (hBitmap != null)
					{
						hBitmap.Close();
					}
				}
			}
			return null;
		}

		[System.Security.SecuritySafeCritical]
		public static Bitmap getShell32IconResourceAsBitmap(int iconID)
		{
			if (hmodShell32 == IntPtr.Zero)
			{
				return null;
			}
			using (SafeGdiObjectHandle hicon = LoadImage(hmodShell32, (IntPtr)iconID, IMAGE_ICON, 16, 16, 0))
			{
				if (hicon != null)
				{
					return Bitmap.FromHicon(hicon.DangerousGetHandle());
				}
			}
			return null;
		}
	}

	[System.Security.SecurityCritical]
	class ShellLink : IDisposable
	{
		[ComImport]
		[Guid("0000010B-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		internal interface IPersistFile
		{
			[PreserveSig]
			void GetClassID(out Guid pClassID);
			[PreserveSig]
			void IsDirty();
			[PreserveSig]
			void Load([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, uint dwMode);
			[PreserveSig]
			void Save([MarshalAs(UnmanagedType.LPWStr)] string pszFileName, [MarshalAs(UnmanagedType.Bool)] bool fRemember);
			[PreserveSig]
			void SaveCompleted([MarshalAs(UnmanagedType.LPWStr)] string pszFileName);
			[PreserveSig]
			void GetCurFile([MarshalAs(UnmanagedType.LPWStr)] out string ppszFileName);
		}

		[ComImport]
		[Guid("000214F9-0000-0000-C000-000000000046")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		private interface IShellLinkW
		{
			void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, IntPtr pfd, uint fFlags);
			void GetIDList(out IntPtr ppidl);
			void SetIDList(IntPtr pidl);
			void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxName);
			void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
			void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
			void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
			void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
			void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
			void GetHotkey(out short pwHotkey);
			void SetHotkey(short pwHotkey);
			void GetShowCmd(out uint piShowCmd);
			void SetShowCmd(uint piShowCmd);
			void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
			void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
			void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, uint dwReserved);
			void Resolve(IntPtr hWnd, uint fFlags);
			void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
		}

		[Guid("00021401-0000-0000-C000-000000000046")]
		[ClassInterfaceAttribute(ClassInterfaceType.None)]
		[ComImport]
		private class CShellLink { }

		[Flags]
		public enum EShowWindowFlags : uint
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_NORMAL = 1,
			SW_SHOWMINIMIZED = 2,
			SW_SHOWMAXIMIZED = 3,
			SW_MAXIMIZE = 3,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWMINNOACTIVE = 7,
			SW_SHOWNA = 8,
			SW_RESTORE = 9,
			SW_SHOWDEFAULT = 10,
			SW_MAX = 10
		}

		private IShellLinkW linkW = (IShellLinkW)new CShellLink();

		[System.Security.SecuritySafeCritical]
		public void Dispose()
		{
			if (linkW != null)
			{
				Marshal.ReleaseComObject(linkW);
				linkW = null;
			}
		}

		public void SetPath(string path)
		{
			linkW.SetPath(path);
		}

		public void SetDescription(string description)
		{
			linkW.SetDescription(description);
		}

		public void SetWorkingDirectory(string dir)
		{
			linkW.SetWorkingDirectory(dir);
		}

		public void SetArguments(string args)
		{
			linkW.SetArguments(args);
		}

		public void SetShowCmd(EShowWindowFlags cmd)
		{
			linkW.SetShowCmd((uint)cmd);
		}

		public void Save(string linkFile)
		{
			((IPersistFile)linkW).Save(linkFile, true);
		}

		public void Load(string linkFile)
		{
			((IPersistFile)linkW).Load(linkFile, 0);
		}

		public string GetArguments()
		{
			StringBuilder sb = new StringBuilder(512);
			linkW.GetArguments(sb, sb.Capacity);
			return sb.ToString();
		}

		public string GetPath()
		{
			StringBuilder sb = new StringBuilder(512);
			linkW.GetPath(sb, sb.Capacity, IntPtr.Zero, 0);
			return sb.ToString();
		}
	}
}

namespace IKVM.NativeCode.sun.java2d
{
	static class DefaultDisposerRecord
	{
		public static void invokeNativeDispose(long disposerMethodPointer, long dataPointer)
		{
			throw new NotImplementedException();
		}
	}

	static class Disposer
	{
		public static void initIDs()
		{
		}
	}
}

namespace IKVM.NativeCode.sun.java2d.pipe
{
	static class Region
	{
		public static void initIDs() { }
	}

	static class RenderBuffer
	{
		public static void copyFromArray(object srcArray, long srcPos, long dstAddr, long length)
		{
			throw new NotImplementedException();
		}
	}
}
