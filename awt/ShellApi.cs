/*
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
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace awt
{
    public class ShellApi
    {

        public static Guid GUID_ISHELLFOLDER = new Guid("{000214E6-0000-0000-C000-000000000046}");
        public static Guid GUID_ISHELLFOLDER2 = new Guid("{93F2F68C-1D1B-11d3-A30E-00C04F79ABD1}");

        #region Windows API access by DLL

        /// <summary>
        /// FreeMem for icon handles
        /// </summary>
        /// <param name="hIcon">The icon handle; must not be in use.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("shell32.dll")]
        public static extern int FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);

        /// <summary>
        /// Returns the IShellFolder for the virtual desktop, which is the root any other folder.
        /// See http://msdn.microsoft.com/en-us/library/bb762175%28VS.85%29.aspx
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(ref IShellFolder ppshf);

        /// <summary>
        /// Returns informations aboud any filesystem object which can be addressed by a normal path.
        /// See http://msdn.microsoft.com/en-us/library/bb762179%28VS.85%29.aspx
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);


        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(IntPtr pIDL, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// Returns the pIDL for a special folder.
        /// See http://msdn.microsoft.com/en-us/library/bb762203%28VS.85%29.aspx
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

        /// <summary>
        /// Returns the path for a desktop-relative pIDL
        /// See http://msdn.microsoft.com/en-us/library/bb762194%28VS.85%29.aspx
        /// </summary>
        [DllImport("shell32.dll")]
        public static extern Boolean SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

        #endregion

        #region Flags and Enumerations
        // see http://msdn.microsoft.com/en-us/library/bb762499(v=VS.85).aspx

        /// <summary>
        /// Constants for IShellFolder.GetDisplayNameOf and IShellFolder.SetNameOf 
        /// </summary>
        [Flags]
        public enum SHGDN : uint
        {
            SHGDN_NORMAL = 0x0000,     // Default
            SHGDN_INFOLDER = 0x0001,   // Displayed relative to a folder
            SHGDN_FOREDITING = 0x1000, // for in-place editing
            SHGDN_FORADDRESSBAR = 0x4000,
            SHGDN_FORPARSING = 0x8000, // parsable by ParseDisplayName()
        }

        /// <summary>
        /// Filter constants for IShellFolder.EnumObjects
        /// </summary>
        [Flags]
        public enum SHCONTF : uint
        {
            SHCONTF_CHECKING_FOR_CHILDREN = 0x00010,// >Win7. The calling application is checking for the existence of child items in the folder.
            SHCONTF_FOLDERS = 0x0020,               // Include folders
            SHCONTF_NONFOLDERS = 0x0040,            // Include non folders
            SHCONTF_INCLUDEHIDDEN = 0x0080,         // Show hidden items
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,    // Allow EnumObject() to return before validating the enumeration
            SHCONTF_NETPRINTERSRCH = 0x0200,        // Hint that client is looking for printers
            SHCONTF_SHAREABLE = 0x0400,             // Hint that client is looking sharable resources
            SHCONTF_STORAGE = 0x0800,               // Include all items with accessible storage
            SHCONTF_NAVIGATION_ENUM = 0x01000,      // >Win7. Child folders should provide a navigation enumeration.
            SHCONTF_FASTITEMS = 0x02000,            // >Win7. The calling application is looking for resources that can be enumerated quickly.
            SHCONTF_FLATLIST = 0x04000,             // >Win7. Enumerate items as a simple list even if the folder itself is not structured in that way.
            SHCONTF_ENABLE_ASYNC = 0x08000,         // >Win7. The calling application is monitoring for change notifications. 
            SHCONTF_INCLUDESUPERHIDDEN = 0x10000    // >Win7. Include hidden system items in the enumeration. 
        }

        /// <summary>
        /// Attributes of an IShellFolder object
        /// </summary>
        [Flags]
        public enum SFGAOF : uint
        {
            SFGAO_CANCOPY = 0x1,                   // can be copied
            SFGAO_CANMOVE = 0x2,                   // can be moved
            SFGAO_CANLINK = 0x4,                   // can be linked
            SFGAO_STORAGE = 0x00000008,            // Supports BindToObject(IID_IStorage)
            SFGAO_CANRENAME = 0x00000010,          // can be renamed
            SFGAO_CANDELETE = 0x00000020,          // can be deleted
            SFGAO_HASPROPSHEET = 0x00000040,       // have property sheets
            SFGAO_DROPTARGET = 0x00000100,         // are drop target
            SFGAO_CAPABILITYMASK = 0x00000177,
            SFGAO_SYSTEM = 0x00001000,             // >Win7, The specified items are system items.
            SFGAO_ENCRYPTED = 0x00002000,          // items are encrypted
            SFGAO_ISSLOW = 0x00004000,             // 'Slow' object
            SFGAO_GHOSTED = 0x00008000,            // Ghosted icon
            SFGAO_LINK = 0x00010000,               // Shortcut (link)
            SFGAO_SHARE = 0x00020000,              // Shared
            SFGAO_READONLY = 0x00040000,           // Read-only
            SFGAO_HIDDEN = 0x00080000,             // Hidden object
            SFGAO_DISPLAYATTRMASK = 0x000FC000,    // Do not use.(Docs of MSDN)
            SFGAO_FILESYSANCESTOR = 0x10000000,    // May contain children with SFGAO_FILESYSTEM
            SFGAO_FOLDER = 0x20000000,             // Support BindToObject(IID_IShellFolder)
            SFGAO_FILESYSTEM = 0x40000000,         // Is a win32 file system object (file/folder/root)
            SFGAO_HASSUBFOLDER = 0x80000000,       // May contain children with SFGAO_FOLDER
            SFGAO_CONTENTSMASK = 0x80000000,       // 
            SFGAO_VALIDATE = 0x01000000,           // revalidate cached information
            SFGAO_REMOVABLE = 0x02000000,          // is a removeable media
            SFGAO_COMPRESSED = 0x04000000,         // is compressed
            SFGAO_BROWSABLE = 0x08000000,          // Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
            SFGAO_NONENUMERATED = 0x00100000,      // is a non-enumerated object
            SFGAO_NEWCONTENT = 0x00200000,         // Should show bold in explorer tree
            SFGAO_CANMONIKER = 0x00400000,         // Not supported.
            SFGAO_HASSTORAGE = 0x00400000,         // Not supported.
            SFGAO_STREAM = 0x00400000,             // Supports BindToObject
            SFGAO_STORAGEANCESTOR = 0x00800000,    // May contain children with SFGAO_STORAGE or SFGAO_STREAM
            SFGAO_STORAGECAPMASK = 0x70C50008,     // For determining storage capabilities, ie for open/save semantics
        }

        /// <summary>
        /// Specifies the desired format of a STRRET structure
        /// </summary>
        [Flags]
        public enum STRRET_TYPE : uint
        {
            STRRET_WSTR = 0, // The string is returned in the cStr
            STRRET_OFFSET = 0x1, // The uOffset value indicates the number of bytes from the beginning of the IDL where the string is located.
            STRRET_CSTR = 0x2, // The string is at the address specified by pOleStr
        }

        /// <summary>
        /// Flags for SHGetFileInfo, parameter uFlags; specifies the file information to retrieve.
        /// See http://www.pinvoke.net/default.aspx/Constants/ShellAPI%20.html
        /// </summary>
        [Flags]
        public enum SHGFI
        {
            SHGFI_LARGEICON = 0x000000000,
            SHGFI_SMALLICON = 0x000000001,
            SHGFI_OPENICON = 0x000000002,
            SHGFI_SHELLICONSIZE = 0x000000004,
            SHGFI_PIDL = 0x000000008,
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            SHGFI_ADDOVERLAYS = 0x000000020,
            SHGFI_OVERLAYINDEX = 0x000000040,
            SHGFI_ICON = 0x000000100,
            SHGFI_DISPLAYNAME = 0x000000200,
            SHGFI_TYPENAME = 0x000000400,
            SHGFI_ATTRIBUTES = 0x000000800,
            SHGFI_ICONLOCATION = 0x000001000,
            SHGFI_EXETYPE = 0x000002000,
            SHGFI_SYSICONINDEX = 0x000004000,
            SHGFI_LINKOVERLAY = 0x000008000,
            SHGFI_SELECTED = 0x000010000,
            SHGFI_ATTR_SPECIFIED = 0x000020000,
        }

        /// <summary>
        /// Special folder constants
        /// See http://msdn.microsoft.com/en-us/library/bb762494%28VS.85%29.aspx
        /// </summary>
        [Flags]
        public enum CSIDL : int
        {
            CSIDL_DESKTOP = 0x0000,
            CSIDL_INTERNET = 0x0001,
            CSIDL_PROGRAMS = 0x0002,
            CSIDL_CONTROLS = 0x0003,
            CSIDL_PRINTERS = 0x0004,
            CSIDL_PERSONAL = 0x0005,
            CSIDL_FAVORITES = 0x0006,
            CSIDL_STARTUP = 0x0007,
            CSIDL_RECENT = 0x0008,
            CSIDL_SENDTO = 0x0009,
            CSIDL_BITBUCKET = 0x000a,
            CSIDL_STARTMENU = 0x000b,
            CSIDL_MYDOCUMENTS = 0x000c,
            CSIDL_MYMUSIC = 0x000d,
            CSIDL_MYVIDEO = 0x000e,
            CSIDL_DESKTOPDIRECTORY = 0x0010,
            CSIDL_DRIVES = 0x0011,
            CSIDL_NETWORK = 0x0012,
            CSIDL_NETHOOD = 0x0013,
            CSIDL_FONTS = 0x0014,  
            CSIDL_TEMPLATES = 0x0015,
            CSIDL_COMMON_STARTMENU = 0x0016,
            CSIDL_COMMON_PROGRAMS = 0X0017,
            CSIDL_COMMON_STARTUP = 0x0018,
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,
            CSIDL_APPDATA = 0x001A,
            CSIDL_PRINTHOOD = 0x001b,
            CSIDL_LOCAL_APPDATA = 0x001c,
            CSIDL_ALTSTARTUP = 0x001d,
            CSIDL_COMMON_ALTSTARTUP = 0x001e,
            CSIDL_COMMON_FAVORITES = 0x001f,
            CSIDL_INTERNET_CACHE = 0x0020,
            CSIDL_COOKIES = 0x0021,
            CSIDL_HISTORY = 0x0022,
            CSIDL_COMMON_APPDATA = 0x0023,
            CSIDL_WINDOWS = 0x0024,
            CSIDL_SYSTEM = 0x0025,
            CSIDL_PROGRAM_FILES = 0x0026,
            CSIDL_MYPICTURES = 0x0027,
            CSIDL_PROFILE = 0x0028,
            CSIDL_SYSTEMX86         = 0x0029,
            CSIDL_PROGRAM_FILESX86 = 0x002a,
            CSIDL_PROGRAM_FILES_COMMON = 0x002b,
            CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,
            CSIDL_COMMON_TEMPLATES = 0x002d,
            CSIDL_COMMON_DOCUMENTS = 0x002e,
            CSIDL_COMMON_ADMINTOOLS = 0x002f,
            CSIDL_ADMINTOOLS = 0x0030,
            CSIDL_CONNECTIONS = 0x0031,
            CSIDL_COMMON_MUSIC = 0x0035,
            CSIDL_COMMON_PICTURES = 0x0036,
            CSIDL_COMMON_VIDEO = 0x0037,
            CSIDL_CDBURN_AREA = 0x003b
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/bb762538%28VS.85%29.aspx
        /// </summary>
        [Flags]
        public enum SHCOLSTATE : uint
        {
            SHCOLSTATE_DEFAULT = 0x00000000,
            SHCOLSTATE_TYPE_STR = 0x00000001,
            SHCOLSTATE_TYPE_INT = 0x00000002,
            SHCOLSTATE_TYPE_DATE = 0x00000003,
            SHCOLSTATE_TYPEMASK = 0x0000000f,
            SHCOLSTATE_ONBYDEFAULT = 0x00000010,
            SHCOLSTATE_SLOW = 0x00000020,
            SHCOLSTATE_EXTENDED = 0x00000040,
            SHCOLSTATE_SECONDARYUI = 0x00000080,
            SHCOLSTATE_HIDDEN = 0x00000100,
            SHCOLSTATE_PREFER_VARCMP = 0x00000200,
            SHCOLSTATE_PREFER_FMTCMP = 0x00000400,
            SHCOLSTATE_NOSORTBYFOLDERNESS = 0x00000800,
            SHCOLSTATE_VIEWONLY = 0x00010000,
            SHCOLSTATE_BATCHREAD = 0x00020000,
            SHCOLSTATE_NO_GROUPBY = 0x00040000,
            SHCOLSTATE_FIXED_WIDTH = 0x00001000,
            SHCOLSTATE_NODPISCALE = 0x00002000,
            SHCOLSTATE_FIXED_RATIO = 0x00004000,
            SHCOLSTATE_DISPLAYMASK = 0x0000F000,
        }

        /// <summary>
        /// Flags for IExtractIcon.GetIconLocation 
        /// See http://msdn.microsoft.com/en-us/library/bb761852%28VS.85%29.aspx
        /// </summary>
        [Flags]
        public enum GIL : uint{ 
            GIL_OPENICON = 0x0001,
            GIL_FORSHELL = 0x0002,
            GIL_ASYNC = 0x0020,
            GIL_DEFAULTICON = 0x0040,
            GIL_FORSHORTCUT = 0x0080,
        }  

        #endregion

        #region Structs

        /// <summary>
        /// File informations
        /// See http://msdn.microsoft.com/en-us/library/bb759792%28VS.85%29.aspx
        /// </summary>
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>
        /// Return value of IShellFolder interface methods
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size=264, CharSet = CharSet.Auto)]
        public struct STRRET
        {
            [FieldOffset(0)]
            public UInt32 uType; // type of storage
            [FieldOffset(4)]
            public IntPtr pOleStr; // pointer to the string
            [FieldOffset(4)]
            public UInt32 uOffset; // Offset into an IID list
            [FieldOffset(4)]
            public IntPtr cStr; // string buffer with the name
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ICONINFO
        {
            internal bool fIcon;
            internal int xHotspot;
            internal int yHotspot;
            internal IntPtr hbmMask;
            internal IntPtr hbmColor;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLDETAILS
        {
            public int fmt;
            public int cxChar;
            public STRRET str;
        }

        #endregion

        #region ComImports

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/bb775075%28VS.85%29.aspx
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        public interface IShellFolder
        {
            [PreserveSig()]
            uint ParseDisplayName(
                IntPtr hwnd,
                IntPtr pbc,
                [In(), MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName,
                out uint pchEaten,
                out IntPtr ppidl,
                ref uint pdwAttributes);

            [PreserveSig()]
            uint EnumObjects(
                IntPtr hwnd,
                SHCONTF grfFlags,
                out IEnumIDList ppenumIDList);

            [PreserveSig()]
            uint BindToObject(
                IntPtr pidl,
                IntPtr pbc,
                [In()] ref Guid riid,
                out IShellFolder ppv);

            [PreserveSig()]
            uint BindToStorage(
                IntPtr pidl,
                IntPtr pbc,
                [In()] ref Guid riid,
                [MarshalAs(UnmanagedType.Interface)] out object ppv);

            [PreserveSig()]
            int CompareIDs(
                int lParam,
                IntPtr pidl1,
                IntPtr pidl2);

            [PreserveSig()]
            uint CreateViewObject(
                IntPtr hwndOwner,
                [In()] ref Guid riid,
                [MarshalAs(UnmanagedType.Interface)] out object ppv);

            [PreserveSig()]
            uint GetAttributesOf(
                int cidl,
                [In(), MarshalAs(UnmanagedType.LPArray)] IntPtr[]  apidl,
                [MarshalAs(UnmanagedType.LPArray)] 
                SFGAOF[] rgfInOut);

            [PreserveSig()]
            uint GetUIObjectOf(
                IntPtr hwndOwner,
                int cidl,
                [In(), MarshalAs(UnmanagedType.LPArray)] IntPtr[] apidl,
                [In()] ref Guid riid,
                IntPtr rgfReserved, 
                [MarshalAs(UnmanagedType.Interface)] out object ppv);

            [PreserveSig()]
            uint GetDisplayNameOf(
                IntPtr pidl,
                SHGDN uFlags,
                out STRRET pName);

            [PreserveSig()]
            uint SetNameOf(
                IntPtr hwnd,
                IntPtr pidl,
                [In(), MarshalAs(UnmanagedType.LPWStr)] string pszName,
                SHGDN uFlags,
                out IntPtr ppidlOut);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/bb761277%28VS.85%29.aspx
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214e5-0000-0000-c000-000000000046")]
        public interface IShellIcon 
        {
            [PreserveSig()]
            int GetIconOf(
            IntPtr pidl,
            uint flags,
            out int pIconIndex);
        }

        /// <summary>
        /// see http://msdn.microsoft.com/en-us/library/bb761854%28VS.85%29.aspx
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214fa-0000-0000-c000-000000000046")]
        public interface IExtractIcon
        {
            [PreserveSig()]
            int GetIconLocation(
                uint uFlags,
                [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 2)] StringBuilder szIconFile,
                int cchMax,
                out int piIndex,
                out uint pwFlags);

            [PreserveSig()]
            int Extract(
                [MarshalAs( UnmanagedType.LPWStr )] string pstFile,
                uint nIconIndex,
                out IntPtr phiconLarge,
                out IntPtr phiconSmall,
                uint nIconSize );
        }
        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/bb775055%28VS.85%29.aspx
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1")]
        public interface IShellFolder2 : IShellFolder // Extends the capabilities of IShellFolder
        {

            [PreserveSig()]
            uint EnumSearches( 
                [MarshalAs(UnmanagedType.Interface)] out object ppEnum); 

            [PreserveSig()]
            uint GetDefaultColumn(
                uint dwReserved,
                out ulong pSort,
                out ulong pDisplay);

            [PreserveSig()]
            uint GetDefaultColumnState(
                uint iColumn,
                out uint pcsFlags);

            [PreserveSig()]
            uint GetDefaultSearchGUID(
                ref Guid guid);

            [PreserveSig()]
            uint GetDetailsEx(
                IntPtr pidl,
                [In()] IntPtr pcsFlags,
                out IntPtr pv);

            [PreserveSig()]
            uint GetDetailsOf(
                IntPtr pidl,
                uint iColumn,
                out SHELLDETAILS psd);

            [PreserveSig()]
            uint MapColumnToSCID(
                uint iColumn,
                out IntPtr pscid);
        }

        /// <summary>
        /// See http://msdn.microsoft.com/en-us/library/bb761982%28VS.85%29.aspx
        /// </summary>
        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        public interface IEnumIDList
        {
            [PreserveSig()]
            uint Next(
                uint celt,
                out IntPtr rgelt,
                out Int32 pceltFetched
                );

            [PreserveSig()]
            uint Skip(
                uint celt
                );

            [PreserveSig()]
            uint Reset();

            [PreserveSig()]
            uint Clone(
                out IEnumIDList ppenum
                );
        }

        #endregion
    }

}
