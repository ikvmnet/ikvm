/*
  Copyright (C) 2007, 2008, 2010 Jeroen Frijters
  Copyright (C) 2009 - 2012 Volker Berlin (i-net software)
  Copyright (C) 2010 Karsten Heinrich (i-net software)

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
using awt;

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
	public static class Win32ShellFolder2
	{
		private const uint IMAGE_BITMAP = 0;
		private const uint IMAGE_ICON = 1;

        private const int HINST_COMMCTRL = -1;
        private const int IDB_VIEW_SMALL_COLOR = 4;

        private const uint WM_USER = 0x0400;
        private const uint TB_GETIMAGELIST = WM_USER + 49;
        private const uint TB_LOADIMAGES = WM_USER + 50;

        private const uint ILD_TRANSPARENT = 0x00000001;

		private static readonly IntPtr hmodShell32;
		private static readonly bool isXP;

		[System.Security.SecuritySafeCritical]
		static Win32ShellFolder2()
		{
			hmodShell32 = LoadLibrary("shell32.dll");
			isXP = Environment.OSVersion.Version >= new Version(5, 1);
		}

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

		[DllImport("shlwapi.dll")]
        private static extern int StrRetToBuf(ref ShellApi.STRRET pstr, IntPtr pIDL, StringBuilder pszBuf, uint cchBuf);

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

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr CreateWindowEx(
           uint dwExStyle,
           string lpClassName,
           string lpWindowName,
           uint dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool DestroyWindow(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr ImageList_GetIcon(IntPtr himl, int i, uint flags);

        [DllImport("comctl32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool ImageList_Destroy(IntPtr himl);

        [DllImport("user32")]
        public static extern int DestroyIcon(IntPtr hIcon);

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
			int result = ShellApi.FindExecutable(path, path, objResultBuffer);
			if (result >= 32)
			{
				return objResultBuffer.ToString();
			}
			return null;
		}

		/// <summary>
		/// Get the type of a file or folder. On a file it depends on its extension.
		/// </summary>
		/// <param name="path">the path of the file or folder</param>
		/// <returns>The type in readable form or null, if the path cannot be resolved</returns>
		[System.Security.SecuritySafeCritical]
		public static string getFolderType(string path)
		{
            ShellApi.SHFILEINFO shinfo = new ShellApi.SHFILEINFO();
			if (ShellApi.SHGetFileInfo(path, 0, out shinfo, (uint)Marshal.SizeOf(shinfo), ShellApi.SHGFI.SHGFI_TYPENAME) == IntPtr.Zero)
			{
				return null;
			}
			return shinfo.szTypeName;
		}

		[System.Security.SecurityCritical]
        public static Bitmap getIconBits(IntPtr hIcon, int iconSize)
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

                    DeleteObject(hIcon);
                    Bitmap bitmap = new Bitmap(iconSize, iconSize, PixelFormat.Format32bppArgb);
                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, iconSize, iconSize), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                    Marshal.Copy(iconBits, 0, bitmapData.Scan0, iconBits.Length);
                    bitmap.UnlockBits(bitmapData);
                    return bitmap;
				}
			}
			return null;
		}

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, or drive root.
        /// </summary>
        /// <param name="path">The path of the file system object</param>
        /// <returns>The SHGFI flags with the attributes</returns>
		[System.Security.SecuritySafeCritical]
		public static int getAttribute(string path)
		{
            ShellApi.SHFILEINFO shinfo = new ShellApi.SHFILEINFO();
            if (ShellApi.SHGetFileInfo(path, 0, out shinfo, (uint)Marshal.SizeOf(shinfo), ShellApi.SHGFI.SHGFI_ATTRIBUTES) == IntPtr.Zero)
			{
				return 0;
			}
			return (int)shinfo.dwAttributes;
		}

        /// <summary>Returns the link target as a pIDL relative to the desktop without resolving the link</summary>
        /// <param name="path">The path of the .lnk file</param>
        /// <returns>the link target as a pIDL relative to the desktop</returns>
		[System.Security.SecuritySafeCritical]
		public static string getLinkLocation(string path )
		{
			using (ShellLink link = new ShellLink())
			{
				link.Load(path);
				return link.GetPath();
			}
		}
        /// <summary>Returns the link target as a pIDL relative to the desktop</summary>
        /// <param name="path">The path of the .lnk file</param>
        /// <param name="resolve">If true, attempts to find the target of a Shell link, 
        /// even if it has been moved or renamed. This may open a file chooser</param>
        /// <returns>the link target as a pIDL relative to the desktop</returns>
        [System.Security.SecuritySafeCritical]
        public static IntPtr getLinkLocation(string path, Boolean resolve)
        {
            using (ShellLink link = new ShellLink())
            {
                link.Load(path);
                if (resolve)
                {
                    link.Resolve();
                }                
                return link.GetIDList();
            }
        }

        // Code copied from Java_sun_awt_shell_Win32ShellFolder2_getStandardViewButton0
        [System.Security.SecuritySafeCritical]
        public static Bitmap getStandardViewButton0(int iconIndex)
        {
            Bitmap result = null;
            using (new ThemingActivationContext())
            {
                // Create a toolbar
                IntPtr hWndToolbar = CreateWindowEx(0, "ToolbarWindow32", null, 0, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                if (hWndToolbar != IntPtr.Zero)
                {
                    SendMessage(hWndToolbar, TB_LOADIMAGES, IDB_VIEW_SMALL_COLOR, HINST_COMMCTRL);

                    IntPtr hImageList = SendMessage(hWndToolbar, TB_GETIMAGELIST, 0, 0);
                    if (hImageList != IntPtr.Zero)
                    {
                        IntPtr hIcon = ImageList_GetIcon(hImageList, iconIndex, ILD_TRANSPARENT);
                        if (hIcon != IntPtr.Zero)
                        {
                            Icon icon = Icon.FromHandle(hIcon);
                            result = icon.ToBitmap();
                            icon.Dispose();
                            DestroyIcon(hIcon);
                        }
                        ImageList_Destroy(hImageList);
                    }
                    DestroyWindow(hWndToolbar);
                }
            }
            return result;
        }

        /// <summary>
        /// Retrieves an icon from the shell32.dell
        /// </summary>
        /// <param name="iconID">the index of the icon</param>
        /// <returns>The icon or null, if there is no icon at the given index</returns>
		[System.Security.SecuritySafeCritical]
        public static Bitmap getShell32IconResourceAsBitmap(int iconID, bool getLargeIcon)
		{
			if (hmodShell32 == IntPtr.Zero)
			{
				return null;
			}
            int size = getLargeIcon ? 32 : 16;
            using (SafeGdiObjectHandle hicon = LoadImage(hmodShell32, (IntPtr)iconID, IMAGE_ICON, size, size, 0))
			{
				if (hicon != null)
				{
                    return getIconBits(hicon.DangerousGetHandle(), 16);
				}
			}
			return null;
		}

        /// <summary>
        /// Returns the pIDL of the desktop itself
        /// </summary>
        [System.Security.SecurityCritical]
        public static IntPtr initDesktopPIDL()
        {
            IntPtr pidl = new IntPtr();

            // get the root shell folder
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, ShellApi.CSIDL.CSIDL_DESKTOP, ref pidl);

            return pidl;
        }

        /// <summary>
        /// Returns an IShellFolder for the desktop
        /// </summary>
        [System.Security.SecurityCritical]
        public static Object initDesktopFolder()
        {
            ShellApi.IShellFolder rootShell = null;

            // get the root shell folder
            ShellApi.SHGetDesktopFolder(ref rootShell);

            return rootShell;
        }

        /// <summary>
        /// Returns the desktop relative pIDL of a special folder
        /// </summary>
        /// <param name="desktopIShellFolder">The IShellFolder instance of the Desktop</param>
        /// <param name="csidl">The CSIDL of the special folder</param>
        /// <returns>the desktop relative pIDL of a special folder</returns>
        [System.Security.SecurityCritical]
        public static IntPtr initSpecialPIDL(Object desktopIShellFolder, int csidl)
        {
            IntPtr result = new IntPtr();        
            ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, (ShellApi.CSIDL)csidl, ref result);
            return result;
        }

        /// <summary>
        /// Creates an IShellFolder for a special folder
        /// </summary>
        /// <param name="desktopIShellFolder">The IShellFolder instance of the Desktop</param>
        /// <param name="pidl">The desktop relative pIDL of the special folder</param>
        /// <returns>The IShellFolder for a special folder</returns>
        [System.Security.SecurityCritical]
        public static Object initSpecialFolder(Object desktopIShellFolder, IntPtr pidl)
        {
            try
            {
                // get desktop instance
                ShellApi.IShellFolder desktop = (ShellApi.IShellFolder)desktopIShellFolder;
                // call BindToObject of the desktop
                ShellApi.IShellFolder specialFolder = null;
                desktop.BindToObject(pidl, IntPtr.Zero, ref ShellApi.GUID_ISHELLFOLDER, out specialFolder);
                return specialFolder;
            }
            catch (System.ArgumentException )
            {
                return 0;
            }
        }

        /// <summary>
        /// Goes down one entry in the given pIDL
        /// </summary>
        /// <param name="pIDL">the pIDL to operate on</param>
        /// <returns>the next entry in the pIDL</returns>
		[System.Security.SecurityCritical]
		public static IntPtr getNextPIDLEntry(IntPtr pIDL)
        {
            if (pIDL == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }

            int length = Marshal.ReadInt16(pIDL);
            if (length == 0) // defined as terminator of a ITEMIDLIST
            {
                return IntPtr.Zero;
            }
            IntPtr newpIDL = new IntPtr(pIDL.ToInt64() + length);
            if (Marshal.ReadInt16(newpIDL) == 0)
            {
                return IntPtr.Zero;
            }
            else
            {
                return newpIDL;
            }
        }
        /// <summary>
        /// Copies the first entry in the given pIDL into a new relative pIDL (with terminator)
        /// </summary>
        /// <param name="pIDL">The pIDL to copy from</param>
        /// <returns>the relative pIDL of the first entry</returns>
		[System.Security.SecurityCritical]
		public static IntPtr copyFirstPIDLEntry(IntPtr pIDL)
        {
            if (pIDL == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            int length = Marshal.ReadInt16(pIDL) + 2; // +2 for the terminator
            byte[] buffer = new byte[length];
            IntPtr newpIDLptr = Marshal.AllocCoTaskMem(length); // create pointer to new pIDL
            Marshal.Copy(pIDL, buffer, 0, length - 2); // copy content
            Marshal.Copy(buffer, 0, newpIDLptr, length); // copy content
            return newpIDLptr;
        }

        /// <summary>
        /// Concatinates two pIDLs
        /// </summary>
        /// <param name="ppIDL">a pIDL, if IntPtr.Zero, IntPtr.Zero will be returned</param>
        /// <param name="pIDL">a pIDL, if IntPtr.Zero, IntPtr.Zero will be returned</param>
        /// <returns>the concatination of ppIDL and pIDL</returns>
		[System.Security.SecurityCritical]
		public static IntPtr combinePIDLs(IntPtr ppIDL, IntPtr pIDL)
        {
            if (ppIDL == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            if (pIDL == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            int lengthP = getPIDLlength(ppIDL);
            int lengthR = getPIDLlength(pIDL);
            byte[] newPIDL = new byte[lengthP + lengthR + 2];
            Marshal.Copy(ppIDL, newPIDL, 0, lengthP);
            Marshal.Copy(pIDL, newPIDL, lengthP, lengthR);
            IntPtr newpIDLptr = Marshal.AllocCoTaskMem(lengthP + lengthR + 2); // create pointer to new pIDL
            Marshal.Copy(newPIDL,0,newpIDLptr,newPIDL.Length); // set pointer to new pIDL and delete local structure
            return newpIDLptr;
        }

        /// <summary>
        /// Calculates the size of a ITEMIDLIST without the two bytes of the terminator
        /// </summary>
        /// <param name="pIDL">a pointer to the IDL to get the length of</param>
        /// <returns>the length in bytes</returns>
		[System.Security.SecurityCritical]
		public static int getPIDLlength(IntPtr pIDL)
        {
            if( pIDL == IntPtr.Zero )
            {
                return 0;
            }
            int length = Marshal.ReadInt16(pIDL);
            int offset = length;
            while (length > 0)
            {
                length = Marshal.ReadInt16(new IntPtr( pIDL.ToInt64() + offset));
                offset += length;
            }
            return offset;
        }
        /// <summary>
        /// Releases the allocted memory of a pIDL
        /// </summary>
        /// <param name="pIDL">The pIDL to be released</param>
		[System.Security.SecurityCritical]
		public static void releasePIDL(IntPtr pIDL)
        {
            if (pIDL == IntPtr.Zero)
            {
                return;
            }
            Marshal.FreeCoTaskMem(pIDL);
        }

		/// <summary>
        /// Releases an IShellFolder COM object
        /// </summary>
        /// <param name="pIShellFolder">The IShellFolder to be released, must not be null</param>
		[System.Security.SecurityCritical]
		public static void releaseIShellFolder(Object pIShellFolder)
        {
            if (pIShellFolder == null)
            {
                return;
            }
            Marshal.ReleaseComObject(pIShellFolder);
        }

		[System.Security.SecurityCritical]
		public static int compareIDs(Object pParentIShellFolder, IntPtr pidl1, IntPtr pidl2)
        {
            if (pParentIShellFolder == null)
            {
                return 0;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)pParentIShellFolder;
            return folder.CompareIDs(0, pidl1, pidl2);
        }

		[System.Security.SecurityCritical]
		public static int getAttributes0(Object pParentIShellFolder, IntPtr pIDL, int attrsMask)
        {
            if (pParentIShellFolder == null || pIDL == IntPtr.Zero )
            {
                return 0;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)pParentIShellFolder;
            ShellApi.SFGAOF[] atts = new ShellApi.SFGAOF[]{ (ShellApi.SFGAOF)attrsMask };
            IntPtr[] pIDLs = new IntPtr[] { pIDL };
            folder.GetAttributesOf(1, pIDLs, atts);
            return (int)atts[0];
        }

        [System.Security.SecuritySafeCritical]
        public static String getFileSystemPath0(int csidl)
        {
            IntPtr pIDL = new IntPtr();
            int hRes = ShellApi.SHGetSpecialFolderLocation(IntPtr.Zero, (ShellApi.CSIDL)csidl, ref pIDL);
            if (hRes != 0)
            {
                //throw Marshal.ThrowExceptionForHR(hRes);
                // TODO exception for hRes
                return null;
            }
            StringBuilder builder = new StringBuilder( 1024 );
            if (ShellApi.SHGetPathFromIDList(pIDL, builder))
            {
                return builder.ToString();
            }
            else
            {
                return null;
            }
        }

        [System.Security.SecurityCritical]
        public static Object getEnumObjects(Object pIShellFolder, Boolean isDesktop, Boolean includeHiddenFiles)
        {
            if (pIShellFolder == null)
            {
                return null;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)pIShellFolder;
            ShellApi.SHCONTF flags = ShellApi.SHCONTF.SHCONTF_FOLDERS | ShellApi.SHCONTF.SHCONTF_NONFOLDERS;
            if( includeHiddenFiles )
            {
                flags |= ShellApi.SHCONTF.SHCONTF_INCLUDEHIDDEN;
            }

            ShellApi.IEnumIDList list = null;
            folder.EnumObjects(IntPtr.Zero, flags, out list);
            return list;
        }

        /// <summary>
        /// Returns the next pIDL in an IEnumIDList
        /// </summary>
        /// <param name="pEnumObjects">The IEnumIDList to get the next element of</param>
        /// <returns>a pIDL or IntPtr.Zero in case the end of the enum is reached</returns>
        [System.Security.SecurityCritical]
        public static IntPtr getNextChild(Object pEnumObjects)
        {
            if (pEnumObjects == null)
            {
                return IntPtr.Zero;
            }
            ShellApi.IEnumIDList list = (ShellApi.IEnumIDList)pEnumObjects;
            IntPtr pIDL = new IntPtr();
            int pceltFetched; // can be ignored, if celt = 1
            uint hRes = list.Next(1, out pIDL, out pceltFetched);
            if ( hRes != 0 || pceltFetched == 0 )
            {
                return IntPtr.Zero;
            }
            else
            {
                return pIDL;
            }
        }

        /// <summary>
        /// Releases an IEnumIDList
        /// </summary>
        /// <param name="pEnumObjects">The IEnumIDList to be released</param>
		[System.Security.SecurityCritical]
		public static void releaseEnumObjects(Object pEnumObjects)
        {
            if (pEnumObjects != null)
            {
                Marshal.ReleaseComObject(pEnumObjects);
            }
        }

        /// <summary>
        /// Binds an IShellFolder to the child of a given shell folder
        /// </summary>
        /// <param name="parentIShellFolder">the parent IShellFolder</param>
        /// <param name="pIDL">the relative pIDL to the child</param>
        /// <returns>The IShellFolder of the child or null, if there is no such child</returns>
		[System.Security.SecurityCritical]
		public static Object bindToObject(Object parentIShellFolder, IntPtr pIDL)
        {
            if (parentIShellFolder == null || pIDL == IntPtr.Zero )
            {
                return null;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)parentIShellFolder;
            ShellApi.IShellFolder newFolder = null;
            folder.BindToObject(pIDL, IntPtr.Zero, ref ShellApi.GUID_ISHELLFOLDER, out newFolder);
            return newFolder;
        }

        /// <summary>
        /// Parses the displayname of a child of a given folder
        /// </summary>
        /// <param name="pIShellFolder">The IShellFolder to get the chilf of</param>
        /// <param name="name">The display name of the child</param>
        /// <returns>the relative pIDL of the child or IntPrt.Zero in case there is no such child</returns>
        [System.Security.SecurityCritical]
        public static IntPtr parseDisplayName0(Object pIShellFolder, String name)
        {
            if (pIShellFolder == null)
            {
                return IntPtr.Zero;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)pIShellFolder;
            IntPtr pIDL = new IntPtr();
            uint pchEaten;
            uint pdwAttribute = 0;
            folder.ParseDisplayName(IntPtr.Zero, IntPtr.Zero, name, out pchEaten, out pIDL, ref pdwAttribute);
            return pIDL;
        }

		[System.Security.SecurityCritical]
		public static String getDisplayNameOf(Object parentIShellFolder, IntPtr relativePIDL, int attrs)
        {
            if (parentIShellFolder == null || relativePIDL == IntPtr.Zero)
            {
                return null;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)parentIShellFolder;
            ShellApi.STRRET result;
            uint hRes = folder.GetDisplayNameOf(relativePIDL, (ShellApi.SHGDN)attrs, out result);
            if ( hRes == 0 )
            {
                StringBuilder name = new StringBuilder( 1024 );
                StrRetToBuf(ref result, relativePIDL, name, 1024);
                string stringName = name.ToString();
                return stringName;
            }
            return null;
        }

		[System.Security.SecurityCritical]
		public static String getFolderType(IntPtr pIDL)
        {
            ShellApi.SHFILEINFO fileInfo = new ShellApi.SHFILEINFO();
            ShellApi.SHGetFileInfo(pIDL, 0, out fileInfo, (uint)Marshal.SizeOf(fileInfo), ShellApi.SHGFI.SHGFI_PIDL | ShellApi.SHGFI.SHGFI_TYPENAME);
            return fileInfo.szTypeName;
        }

        public static Object getIShellIcon(Object pIShellFolder)
        {
            if (pIShellFolder is ShellApi.IShellIcon)
            {
                return pIShellFolder;
            }
            return null;
        }

		[System.Security.SecurityCritical]
		public static int getIconIndex(Object parentIShellFolder, IntPtr relativePIDL)
        {
            if (parentIShellFolder is ShellApi.IShellIcon)
            {
                ShellApi.IShellIcon shellIcon = (ShellApi.IShellIcon)parentIShellFolder;
                int index = 0;
                if( shellIcon.GetIconOf(relativePIDL, (uint)ShellApi.GIL.GIL_FORSHELL, out index) == 0 )
                {
                    return index;
                }
            }
            return 0;
        }

        [System.Security.SecurityCritical]
        public static IntPtr getIcon(String absolutePath, Boolean getLargeIcon) 
        {
            ShellApi.SHFILEINFO shinfo = new ShellApi.SHFILEINFO();
            if (ShellApi.SHGetFileInfo(absolutePath, 0, out shinfo, (uint)Marshal.SizeOf(shinfo), ShellApi.SHGFI.SHGFI_ICON | (getLargeIcon ? ShellApi.SHGFI.SHGFI_LARGEICON : ShellApi.SHGFI.SHGFI_SMALLICON)) == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            return shinfo.hIcon;
        }

		[System.Security.SecurityCritical]
		public static IntPtr extractIcon(Object parentIShellFolder, IntPtr relativePIDL, Boolean getLargeIcon)
        {
            if (parentIShellFolder == null || relativePIDL == IntPtr.Zero)
            {
                return IntPtr.Zero;
            }
            ShellApi.IShellFolder folder = (ShellApi.IShellFolder)parentIShellFolder;
            Guid guid = new Guid("000214fa-0000-0000-c000-000000000046");
            object ppv;
            if (folder.GetUIObjectOf(IntPtr.Zero, 1, new IntPtr[] { relativePIDL }, ref guid, IntPtr.Zero, out ppv) == 0)
            {
                ShellApi.IExtractIcon extractor = (ShellApi.IExtractIcon)ppv;
                int size = 1024;
                StringBuilder path = new StringBuilder( size );
                int piIndex;
                uint pwFlags;
                if (extractor.GetIconLocation((uint)ShellApi.GIL.GIL_FORSHELL, path, size, out piIndex, out pwFlags) == 0)
                {
                    IntPtr hIconL = new IntPtr();
                    IntPtr hIconS = new IntPtr();
                    if (extractor.Extract(path.ToString(), (uint)piIndex, out hIconL, out hIconS, (16 << 16) + 32) == 0)
                    {
                        if (getLargeIcon)
                        {
                            ShellApi.DestroyIcon(hIconS);
                            return hIconL;
                        }
                        else
                        {
                            ShellApi.DestroyIcon(hIconL);
                            return hIconS;
                        }
                    }
                }
            }
            return IntPtr.Zero;
        }

		[System.Security.SecurityCritical]
		public static void disposeIcon(IntPtr hIcon)
        {
            ShellApi.DestroyIcon(hIcon);
        }

		public static Object doGetColumnInfo(Object iShellFolder2)
        {
            // TODO Dummy
            return null;
        }

		public static Object doGetColumnValue(Object parentIShellFolder2, IntPtr childPIDL, int columnIdx)
        {
            // TODO Dummy
            return null;
        }

		public static int compareIDsByColumn(Object pParentIShellFolder, IntPtr pidl1, IntPtr pidl2, int columnIdx)
        {
            // TODO Dummy
            return 0;
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
            /// <summary>Attempts to find the target of a Shell link, even if it has been moved or renamed.</summary>
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

        public void Resolve()
        {
            linkW.Resolve(IntPtr.Zero, 0);
        }

        public IntPtr GetIDList(){
            IntPtr ppidl;
            linkW.GetIDList( out ppidl );
            return ppidl;
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
