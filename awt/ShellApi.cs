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

        ///<summary>
        ///Sends the specified message to a window or windows. The SendMessage function calls the 
        ///window procedure for the specified window and does not return until the window procedure 
        ///has processed the message.
        ///To send a message and return immediately, use the SendMessageCallback or SendNotifyMessage 
        ///function. To post a message to a thread's message queue and return immediately, use the 
        ///PostMessage or PostThreadMessage function.
        ///</summary>
        ///<param name="pWnd" direction="input">
        ///A handle to the window whose window procedure will receive the message. If this parameter is 
        ///HWND_BROADCAST ((HWND)0xffff), the message is sent to all top-level windows in the system, 
        ///including disabled or invisible unowned windows, overlapped windows, and pop-up windows; 
        ///but the message is not sent to child windows.
        ///Message sending is subject to UIPI. The thread of a process can send messages only to message 
        ///queues of threads in processes of lesser or equal integrity level.
        ///</param>
        ///<param name="uMsg" direction="input">
        ///The message to be sent.
        ///For lists of the system-provided messages, see System-Defined Messages.
        ///</param>
        ///<param name="wParam" direction="input">
        ///Additional message-specific information.
        ///</param>
        ///<param name="lParam" direction="input">
        ///Additional message-specific information.
        ///</param>
        ///<returns>
        ///The return value specifies the result of the message processing; it depends on the message sent.
        ///</returns>
        [DllImport("user32.dll")]
        public static extern Int32 SendMessage(IntPtr pWnd, UInt32 uMsg, UInt32 wParam, IntPtr lParam);

        /// <summary>
        /// Destroys an icon and frees any memory the icon occupied
        /// </summary>
        /// <param name="hIcon">A handle to the icon to be destroyed. The icon must not be in use.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("shell32.dll")]
        public static extern int FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);

        /// <summary>
        /// Retrieves the IShellFolder interface for the desktop folder, which is the root of the Shell's 
        /// namespace.
        /// </summary>
        /// <param name="ppshf" directiion="output">
        /// IShellFolder
        /// When this method returns, receives an IShellFolder interface pointer for the desktop folder. 
        /// The calling application is responsible for eventually freeing the interface by calling its 
        /// IUnknown::Release method.
        /// </param>
        /// <returns>If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetDesktopFolder(ref IShellFolder ppshf);

        /// <summary>
        /// Retrieves information about an object in the file system, such as a file, folder, directory, 
        /// or drive root.
        /// </summary>
        /// <param name="pszPath">
        /// LPCTSTR
        /// A pointer to a null-terminated string of maximum length MAX_PATH that contains the path and 
        /// file name. Both absolute and relative paths are valid.
        /// If the uFlags parameter includes the SHGFI_PIDL flag, this parameter must be the address of 
        /// an ITEMIDLIST (PIDL) structure that contains the list of item identifiers that uniquely 
        /// identifies the file within the Shell's namespace. The PIDL must be a fully qualified PIDL. 
        /// Relative PIDLs are not allowed.
        /// If the uFlags parameter includes the SHGFI_USEFILEATTRIBUTES flag, this parameter does not 
        /// have to be a valid file name. The function will proceed as if the file exists with the 
        /// specified name and with the file attributes passed in the dwFileAttributes parameter. This 
        /// allows you to obtain information about a file type by passing just the extension for pszPath 
        /// and passing FILE_ATTRIBUTE_NORMAL in dwFileAttributes.
        /// This string can use either short (the 8.3 form) or long file names.
        /// </param>
        /// <param name="dwFileAttribs">
        /// DWORD
        /// A combination of one or more file attribute flags (FILE_ATTRIBUTE_ values as defined in Winnt.h). 
        /// If uFlags does not include the SHGFI_USEFILEATTRIBUTES flag, this parameter is ignored.
        /// </param>
        /// <param name="psfi" direction="">
        /// SHFILEINFO
        /// Pointer to a SHFILEINFO structure to receive the file information.
        /// </param>
        /// <param name="cbFileInfo">
        /// UINT
        /// The size, in bytes, of the SHFILEINFO structure pointed to by the psfi parameter.
        /// </param>
        /// <param name="uFlags">
        /// UINT
        /// The flags that specify the file information to retrieve. This parameter can be a combination of the following values.
        /// </param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttribs, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);


        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(IntPtr pIDL, uint dwFileAttributes, out SHFILEINFO psfi, uint cbFileInfo, SHGFI uFlags);

        /// <summary>
        /// Retrieves a pointer to the ITEMIDLIST structure of a special folder.
        /// </summary>
        /// <param name="hwndOwner" direction="input">
        /// HWND
        /// Reserved.
        /// </param>
        /// <param name="nFolder" direction="input">
        /// int
        /// A CSIDL value that identifies the folder of interest.
        /// </param>
        /// <param name="ppidl" direction="output">
        /// PIDLIST_ABSOLUTE
        /// A PIDL specifying the folder's location relative to the root of the namespace (the desktop).
        /// It is the responsibility of the calling application to free the returned IDList by using 
        /// CoTaskMemFree.
        /// </param>
        /// <returns>
        /// HRESULT
        /// If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
        /// </returns>
        [DllImport("shell32.dll")]
        public static extern Int32 SHGetSpecialFolderLocation(IntPtr hwndOwner, CSIDL nFolder, ref IntPtr ppidl);

        /// <summary>
        /// Combines two ITEMIDLIST structures.
        /// </summary>
        /// <param name="pIDLparent" direction="input">
        /// PCIDLIST_ABSOLUTE
        /// A pointer to the first ITEMIDLIST structure.
        /// </param>
        /// <param name="pIDLchild" direction="input">
        /// PCUIDLIST_RELATIVE
        /// A pointer to the second ITEMIDLIST structure. This structure is appended to the structure pointed to by pidl1.
        /// </param>
        /// <returns>
        /// PIDLIST_ABSOLUTE
        /// Returns an ITEMIDLIST containing the combined structures. If you set either pidl1 or pidl2 to 
        /// NULL, the returned ITEMIDLIST structure is a clone of the non-NULL parameter. Returns NULL if 
        /// pidl1 and pidl2 are both set to NULL.
        /// </returns>
        [DllImport("shell32.dll")]
        public static extern IntPtr ILCombine(IntPtr pIDLparent, IntPtr pIDLchild);

        /// <summary>
        /// Converts an item identifier list to a file system path.
        /// </summary>
        /// <param name="pidl" direction="input">
        /// PCIDLIST_ABSOLUTE
        /// The address of an item identifier list that specifies a file or directory location relative to the root of the namespace (the desktop).
        /// </param>
        /// <param name="pszPath" direction="output">
        /// LPTSTR
        /// The address of a buffer to receive the file system path. This buffer must be at least MAX_PATH characters in size.
        /// </param>
        /// <returns>
        /// BOOL
        /// Returns TRUE if successful; otherwise, FALSE.
        /// </returns>
        [DllImport("shell32.dll")]
        public static extern Boolean SHGetPathFromIDList(IntPtr pidl, StringBuilder pszPath);

        #endregion

        #region Flags and Enumerations
        // see http://msdn.microsoft.com/en-us/library/bb762499(v=VS.85).aspx

        /// <summary>
        /// Defines the values used with the IShellFolder::GetDisplayNameOf and IShellFolder::SetNameOf 
        /// methods to specify the type of file or folder names used by those methods.
        /// </summary>
        [Flags]
        public enum SHGDN : uint
        {
            /// <summary>
            /// When not combined with another flag, return the parent-relative name that identifies the item, 
            /// suitable for displaying to the user. This name often does not include extra information such 
            /// as the file extension and does not need to be unique. This name might include information that 
            /// identifies the folder that contains the item. For instance, this flag could cause 
            /// IShellFolder::GetDisplayNameOf to return the string "username (on Machine)" for a particular 
            /// user's folder.
            /// </summary>
            SHGDN_NORMAL = 0x0000,   // Default (display purpose)
            /// <summary>
            /// The name is relative to the folder from which the request was made. This is the name display 
            /// to the user when used in the context of the folder. For example, it is used in the view and 
            /// in the address bar path segment for the folder. This name should not include disambiguation 
            /// information—for instance "username" instead of "username (on Machine)" for a particular user's 
            /// folder.
            /// Use this flag in combinations with SHGDN_FORPARSING and SHGDN_FOREDITING.
            /// </summary>
            SHGDN_INFOLDER = 0x0001,   // Displayed under a folder (relative)
            /// <summary>
            /// The name is used for in-place editing when the user renames the item.
            /// </summary>
            SHGDN_FOREDITING = 0x1000,
            /// <summary>
            /// The name is displayed in an address bar combo box. (remove ugly stuff)
            /// </summary>
            SHGDN_FORADDRESSBAR = 0x4000,
            /// <summary>
            /// The name is used for parsing. That is, it can be passed to IShellFolder::ParseDisplayName to 
            /// recover the object's PIDL. The form this name takes depends on the particular object. When 
            /// SHGDN_FORPARSING is used alone, the name is relative to the desktop. When combined with 
            /// SHGDN_INFOLDER, the name is relative to the folder from which the request was made.
            /// </summary>
            SHGDN_FORPARSING = 0x8000,   // Parsing name for ParseDisplayName()
        }

        /// <summary>
        /// Determines the types of items included in an enumeration. These values are used with the IShellFolder::EnumObjects method.
        /// </summary>
        [Flags]
        public enum SHCONTF : uint // Determines the types of items included in an enumeration
        {
            SHCONTF_CHECKING_FOR_CHILDREN = 0x00010,// >Win7. The calling application is checking for the existence of child items in the folder.
            SHCONTF_FOLDERS = 0x0020,               // Only want folders enumerated (SFGAO_FOLDER)
            SHCONTF_NONFOLDERS = 0x0040,            // Include non folders
            SHCONTF_INCLUDEHIDDEN = 0x0080,         // Show items normally hidden
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,    // Allow EnumObject() to return before validating enum
            SHCONTF_NETPRINTERSRCH = 0x0200,        // Hint that client is looking for printers
            SHCONTF_SHAREABLE = 0x0400,             // Hint that client is looking sharable resources (remote shares)
            SHCONTF_STORAGE = 0x0800,               // Include all items with accessible storage and their ancestors
            SHCONTF_NAVIGATION_ENUM = 0x01000,      // >Win7. Child folders should provide a navigation enumeration.
            SHCONTF_FASTITEMS = 0x02000,            // >Win7. The calling application is looking for resources that can be enumerated quickly.
            SHCONTF_FLATLIST = 0x04000,             // >Win7. Enumerate items as a simple list even if the folder itself is not structured in that way.
            SHCONTF_ENABLE_ASYNC = 0x08000,         // >Win7. The calling application is monitoring for change notifications. 
            SHCONTF_INCLUDESUPERHIDDEN = 0x10000    // >Win7. Include hidden system items in the enumeration. 
        }

        [Flags]
        public enum SFGAOF : uint // IShellFolder attribute flagt
        {
            SFGAO_CANCOPY = 0x1,                   // Objects can be copied  (DROPEFFECT_COPY)
            SFGAO_CANMOVE = 0x2,                   // Objects can be moved   (DROPEFFECT_MOVE)
            SFGAO_CANLINK = 0x4,                   // Objects can be linked  (DROPEFFECT_LINK)
            SFGAO_STORAGE = 0x00000008,            // Supports BindToObject(IID_IStorage)
            SFGAO_CANRENAME = 0x00000010,          // Objects can be renamed
            SFGAO_CANDELETE = 0x00000020,          // Objects can be deleted
            SFGAO_HASPROPSHEET = 0x00000040,       // Objects have property sheets
            SFGAO_DROPTARGET = 0x00000100,         // Objects are drop target
            SFGAO_CAPABILITYMASK = 0x00000177,
            SFGAO_SYSTEM = 0x00001000,             // >Win7, The specified items are system items.
            SFGAO_ENCRYPTED = 0x00002000,          // Object is encrypted (use alt color)
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
            SFGAO_VALIDATE = 0x01000000,           // Invalidate cached information
            SFGAO_REMOVABLE = 0x02000000,          // Is this removeable media?
            SFGAO_COMPRESSED = 0x04000000,         // Object is compressed (use alt color)
            SFGAO_BROWSABLE = 0x08000000,          // Supports IShellFolder, but only implements CreateViewObject() (non-folder view)
            SFGAO_NONENUMERATED = 0x00100000,      // Is a non-enumerated object
            SFGAO_NEWCONTENT = 0x00200000,         // Should show bold in explorer tree
            SFGAO_CANMONIKER = 0x00400000,         // Defunct
            SFGAO_HASSTORAGE = 0x00400000,         // Defunct
            SFGAO_STREAM = 0x00400000,             // Supports BindToObject(IID_IStream)
            SFGAO_STORAGEANCESTOR = 0x00800000,    // May contain children with SFGAO_STORAGE or SFGAO_STREAM
            SFGAO_STORAGECAPMASK = 0x70C50008,     // For determining storage capabilities, ie for open/save semantics
        }

        /// <summary>
        /// A value that specifies the desired format of the string of a STRRET structure
        /// </summary>
        [Flags]
        public enum STRRET_TYPE : uint
        {
            STRRET_WSTR = 0, // The string is returned in the cStr member.
            STRRET_OFFSET = 0x1, // The uOffset member value indicates the number of bytes from the beginning of the item identifier list where the string is located.
            STRRET_CSTR = 0x2, // The string is at the address specified by pOleStr member.
        }

        /// <summary>
        /// Flags for SHGetFileInfo, parameter uFlags : The flags that specify the file information to retrieve.
        /// </summary>
        [Flags]
        public enum SHGFI
        {
            SHGFI_LARGEICON = 0x000000000,
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve the file's small icon. Also used to modify 
            /// SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that 
            /// contains small icon images. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
            /// </summary>
            SHGFI_SMALLICON = 0x000000001,
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve the file's open icon. Also used to modify 
            /// SHGFI_SYSICONINDEX, causing the function to return the handle to the system image list that 
            /// contains the file's small open icon. A container object displays an open icon to indicate that 
            /// the container is open. The SHGFI_ICON and/or SHGFI_SYSICONINDEX flag must also be set.
            /// </summary>
            SHGFI_OPENICON = 0x000000002,
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to retrieve a Shell-sized icon. If this flag is not 
            /// specified the function sizes the icon according to the system metric values. The SHGFI_ICON 
            /// flag must also be set.
            /// </summary>
            SHGFI_SHELLICONSIZE = 0x000000004,
            /// <summary>Indicate that pszPath is the address of an ITEMIDLIST structure rather than a path 
            /// name.</summary>
            SHGFI_PIDL = 0x000000008,
            /// <summary>
            /// Indicates that the function should not attempt to access the file specified by pszPath. 
            /// Rather, it should act as if the file specified by pszPath exists with the file attributes 
            /// passed in dwFileAttributes. This flag cannot be combined with the SHGFI_ATTRIBUTES, 
            /// SHGFI_EXETYPE, or SHGFI_PIDL flags.
            /// </summary>
            SHGFI_USEFILEATTRIBUTES = 0x000000010,
            /// <summary> Apply the appropriate overlays to the file's icon. The SHGFI_ICON flag must also 
            /// be set.</summary>
            SHGFI_ADDOVERLAYS = 0x000000020,
            /// <summary>
            /// Return the index of the overlay icon. The value of the overlay index is returned in the 
            /// upper eight bits of the iIcon member of the structure specified by psfi. This flag requires 
            /// that the SHGFI_ICON be set as well.
            /// </summary>
            SHGFI_OVERLAYINDEX = 0x000000040,
            /// <summary>
            /// Retrieve the handle to the icon that represents the file and the index of the icon within the 
            /// system image list. The handle is copied to the hIcon member of the structure specified by 
            /// psfi, and the index is copied to the iIcon member.
            /// </summary>
            SHGFI_ICON = 0x000000100,
            /// <summary>
            /// Retrieve the display name for the file. The name is copied to the szDisplayName member of the 
            /// structure specified in psfi. The returned display name uses the long file name, if there 
            /// is one, rather than the 8.3 form of the file name.
            /// </summary>
            SHGFI_DISPLAYNAME = 0x000000200,
            /// <summary>
            /// Retrieve the string that describes the file's type. The string is copied to the szTypeName 
            /// member of the structure specified in psfi.
            /// </summary>
            SHGFI_TYPENAME = 0x000000400,
            /// <summary>
            /// Retrieve the item attributes. The attributes are copied to the dwAttributes member of the 
            /// structure specified in the psfi parameter. These are the same attributes that are obtained 
            /// from IShellFolder::GetAttributesOf.
            /// </summary>
            SHGFI_ATTRIBUTES = 0x000000800,
            /// <summary>
            /// Retrieve the name of the file that contains the icon representing the file specified by 
            /// pszPath, as returned by the IExtractIcon::GetIconLocation method of the file's icon handler. 
            /// Also retrieve the icon index within that file. The name of the file containing the icon is 
            /// copied to the szDisplayName member of the structure specified by psfi. The icon's index is 
            /// copied to that structure's iIcon member.
            /// </summary>
            SHGFI_ICONLOCATION = 0x000001000,
            /// <summary>
            /// Retrieve the type of the executable file if pszPath identifies an executable file. The 
            /// information is packed into the return value. This flag cannot be specified with any other flags.
            /// </summary>
            SHGFI_EXETYPE = 0x000002000,
            /// <summary>
            /// Retrieve the index of a system image list icon. If successful, the index is copied to the 
            /// iIcon member of psfi. The return value is a handle to the system image list. Only those images 
            /// whose indices are successfully copied to iIcon are valid. Attempting to access other images 
            /// in the system image list will result in undefined behavior.
            /// </summary>
            SHGFI_SYSICONINDEX = 0x000004000,
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to add the link overlay to the file's icon. The 
            /// SHGFI_ICON flag must also be set.
            /// </summary>
            SHGFI_LINKOVERLAY = 0x000008000,
            /// <summary>
            /// Modify SHGFI_ICON, causing the function to blend the file's icon with the system highlight 
            /// color. The SHGFI_ICON flag must also be set.
            /// </summary>
            SHGFI_SELECTED = 0x000010000,
            /// <summary>
            /// Modify SHGFI_ATTRIBUTES to indicate that the dwAttributes member of the SHFILEINFO structure 
            /// at psfi contains the specific attributes that are desired. These attributes are passed to 
            /// IShellFolder::GetAttributesOf. If this flag is not specified, 0xFFFFFFFF is passed to 
            /// IShellFolder::GetAttributesOf, requesting all attributes. This flag cannot be specified 
            /// with the SHGFI_ICON flag.
            /// </summary>
            SHGFI_ATTR_SPECIFIED = 0x000020000,
        }

        [Flags]
        public enum CSIDL : int
        {
            CSIDL_DESKTOP = 0x0000,	    // The overall root of a windows system
            CSIDL_INTERNET = 0x0001,    // Internet Explorer (icon on desktop)
            CSIDL_PROGRAMS = 0x0002,    // The file system directory that contains the user's program groups, which are also file system directories.
            CSIDL_CONTROLS = 0x0003,    // The virtual folder that contains icons for the Control Panel applications. (My Computer\Control Panel)
            CSIDL_PRINTERS = 0x0004,    // The virtual folder that contains installed printers. (My Computer\Printers)
            CSIDL_PERSONAL = 0x0005,    // The file system directory that serves as a common repository for documents.
            CSIDL_FAVORITES = 0x0006,   // The file system directory that serves as a common repository for the user's favorite items.
            CSIDL_STARTUP = 0x0007,     // The file system directory that corresponds to the user's Startup program group. The system starts these programs when a device is powered on.
            CSIDL_RECENT = 0x0008,      // File system directory that contains the user's most recently used documents.
            CSIDL_SENDTO = 0x0009,      // The file system directory that contains Send To menu items. (<user name>\SendTo)
            CSIDL_BITBUCKET = 0x000a,   // The virtual folder that contains the objects in the user's Recycle Bin. (<desktop>\Recycle Bin)
            CSIDL_STARTMENU = 0x000b,   // The file system directory that contains Start menu items. (<user name>\Start Menu)
            CSIDL_MYDOCUMENTS = 0x000c, // logical "My Documents" desktop icon
            CSIDL_MYMUSIC = 0x000d,     // Folder that contains music files.
            CSIDL_MYVIDEO = 0x000e,     // Folder that contains video files.
            CSIDL_DESKTOPDIRECTORY = 0x0010, //	File system directory used to physically store file objects on the desktop (not to be confused with the desktop folder itself).
            CSIDL_DRIVES = 0x0011,      // My Computer
            CSIDL_NETWORK = 0x0012,     // Network Neighborhood (My Network Places)
            CSIDL_NETHOOD = 0x0013,     // <user name>\nethood
            CSIDL_FONTS = 0x0014,       // The virtual folder that contains fonts.
            CSIDL_TEMPLATES = 0x0015,   // The file system directory that serves as a common repository for document templates. (<user name>\Templates)
            CSIDL_COMMON_STARTMENU = 0x0016,// The file system directory that contains the programs and folders that appear on the Start menu for all users. (All Users\Start Menu)
            CSIDL_COMMON_PROGRAMS = 0X0017, // The file system directory that contains the directories for the common program groups that appear on the Start menu for all users. (All Users\Start Menu\Programs)
            CSIDL_COMMON_STARTUP = 0x0018,  // The file system directory that contains the programs that appear in the Startup folder for all users. (All Users\Startup)
            CSIDL_COMMON_DESKTOPDIRECTORY = 0x0019,    // All Users\Desktop
            CSIDL_APPDATA = 0x001A,     // File system directory that serves as a common repository for application-specific data.
            CSIDL_PRINTHOOD = 0x001b,   // <user name>\PrintHood
            CSIDL_LOCAL_APPDATA = 0x001c,   // <user name>\Local Settings\Applicaiton Data (non roaming)
            CSIDL_ALTSTARTUP = 0x001d,  // non localized startup
            CSIDL_COMMON_ALTSTARTUP = 0x001e,    // non localized common startup
            CSIDL_COMMON_FAVORITES = 0x001f,// The file system directory that serves as a common repository for favorite items common to all users.
            CSIDL_INTERNET_CACHE = 0x0020,  // Version 4.72. The file system directory that serves as a common repository for temporary Internet files.
            CSIDL_COOKIES = 0x0021,     // The file system directory that serves as a common repository for Internet cookies. (<user name>\Cookies)
            CSIDL_HISTORY = 0x0022,     // he file system directory that serves as a common repository for Internet history items.
            CSIDL_COMMON_APPDATA = 0x0023,  // All Users\Application Data
            CSIDL_WINDOWS = 0x0024,     // The windows installation folder.
            CSIDL_SYSTEM = 0x0025,      // Version 5.0. The Windows System folder. (C:\Windows\System32)
            CSIDL_PROGRAM_FILES = 0x0026,   //The program files folder.
            CSIDL_MYPICTURES = 0x0027,  // Folder that contains picture files.
            CSIDL_PROFILE = 0x0028,     // Folder that contains the profile of the user.
            CSIDL_SYSTEMX86         = 0x0029,   // x86 system directory on RISC
            CSIDL_PROGRAM_FILESX86 = 0x002a,    // x86 C:\Program Files on RISC
            CSIDL_PROGRAM_FILES_COMMON = 0x002b,// C:\Program Files\Common
            CSIDL_PROGRAM_FILES_COMMONX86 = 0x002c,    // x86 Program Files\Common on RISC
            CSIDL_COMMON_TEMPLATES = 0x002d,    // The file system directory that contains the templates that are available to all users. (All Users\Templates)
            CSIDL_COMMON_DOCUMENTS = 0x002e,    // The file system directory that contains documents that are common to all users. (All Users\Documents)
            CSIDL_COMMON_ADMINTOOLS = 0x002f,   // Version 5.0. The file system directory that contains administrative tools for all users of the computer. (All Users\Start Menu\Programs\Administrative Tools)
            CSIDL_ADMINTOOLS = 0x0030,  // Version 5.0. The file system directory that is used to store administrative tools for an individual user.  (<user name>\Start Menu\Programs\Administrative Tools)
            CSIDL_CONNECTIONS = 0x0031, // The virtual folder that represents Network Connections, that contains network and dial-up connections.
            CSIDL_COMMON_MUSIC = 0x0035,// Version 6.0. The file system directory that serves as a repository for music files common to all users. (All Users\My Music)
            CSIDL_COMMON_PICTURES = 0x0036, // Version 6.0. The file system directory that serves as a repository for image files common to all users. (All Users\My Pictures)
            CSIDL_COMMON_VIDEO = 0x0037,// Version 6.0. The file system directory that serves as a repository for video files common to all users. (All Users\My Video)
            CSIDL_CDBURN_AREA = 0x003b  // Version 6.0. The file system directory that acts as a staging area for files waiting to be written to a CD. (USERPROFILE\Local Settings\Application Data\Microsoft\CD Burning)
        }

        [Flags]
        public enum SHCOLSTATE : uint // Describes how a property should be treated. These values are defined in Shtypes.h.
        {
            SHCOLSTATE_DEFAULT = 0x00000000, // The value is displayed according to default settings for the column.
            SHCOLSTATE_TYPE_STR = 0x00000001, // The value is displayed as a string.
            SHCOLSTATE_TYPE_INT = 0x00000002, // The value is displayed as an integer.
            SHCOLSTATE_TYPE_DATE = 0x00000003, //The value is displayed as a date/time.
            SHCOLSTATE_TYPEMASK = 0x0000000f, // A mask for display type values SHCOLSTATE_TYPE_STR, SHCOLSTATE_TYPE_STR, and SHCOLSTATE_TYPE_DATE.
            SHCOLSTATE_ONBYDEFAULT = 0x00000010, // The column should be on by default in Details view.
            SHCOLSTATE_SLOW = 0x00000020, // Will be slow to compute. Perform on a background thread.
            SHCOLSTATE_EXTENDED = 0x00000040, // Provided by a handler, not the folder.
            SHCOLSTATE_SECONDARYUI = 0x00000080, // Not displayed in the context menu, but is listed in the More... dialog.
            SHCOLSTATE_HIDDEN = 0x00000100, // Not displayed in the UI.
            SHCOLSTATE_PREFER_VARCMP = 0x00000200, // VarCmp produces same result as IShellFolder::CompareIDs.
            SHCOLSTATE_PREFER_FMTCMP = 0x00000400, // PSFormatForDisplay produces same result as IShellFolder::CompareIDs.
            SHCOLSTATE_NOSORTBYFOLDERNESS = 0x00000800, // Do not sort folders separately.
            SHCOLSTATE_VIEWONLY = 0x00010000, // Only displayed in the UI.
            SHCOLSTATE_BATCHREAD = 0x00020000, // Marks columns with values that should be read in a batch.
            SHCOLSTATE_NO_GROUPBY = 0x00040000, // Grouping is disabled for this column.
            SHCOLSTATE_FIXED_WIDTH = 0x00001000, // Can't resize the column.
            SHCOLSTATE_NODPISCALE = 0x00002000, // The width is the same in all dpi.
            SHCOLSTATE_FIXED_RATIO = 0x00004000, // Fixed width and height ratio.
            SHCOLSTATE_DISPLAYMASK = 0x0000F000, // Filters out new display flags.
        }

        [Flags]
        public enum GIL : uint{ // parameter for IShellIcon.GetIconOf and IExtractIcon.GetIconLocation
            GIL_OPENICON = 0x0001, // The icon is in the open state if both open-state and closed-state images are available. If this flag is not specified, the icon is in the normal or closed state. This flag is typically used for folder objects.
            GIL_FORSHELL = 0x0002, // The icon is displayed in a Shell folder.
            GIL_ASYNC = 0x0020, // Set this flag to determine whether the icon should be extracted asynchronously. If the icon can be extracted rapidly, this flag is usually ignored. If extraction will take more time, GetIconLocation should return E_PENDING. See the Remarks for further discussion.
            GIL_DEFAULTICON = 0x0040, // Retrieve information about the fallback icon. Fallback icons are usually used while the desired icon is extracted and added to the cache.
            GIL_FORSHORTCUT = 0x0080, // The icon indicates a shortcut. However, the icon extractor should not apply the shortcut overlay; that will be done later. Shortcut icons are state-independent.
        }  

        #endregion

        #region Structs

        /// <summary>
        /// Contains information about a file object.
        /// </summary>
        public struct SHFILEINFO
        {
            /// <summary>
            /// A handle to the icon that represents the file. You are responsible for destroying this 
            /// handle with DestroyIcon when you no longer need it.
            /// </summary>
            public IntPtr hIcon;
            /// <summary>
            /// The index of the icon image within the system image list.
            /// </summary>
            public IntPtr iIcon;
            /// <summary>
            /// An array of values that indicates the attributes of the file object. See the SFGAOF flags.
            /// </summary>
            public uint dwAttributes;
            /// <summary>
            /// A string that contains the name of the file as it appears in the Windows Shell, or the path 
            /// and file name of the file that contains the icon representing the file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            /// <summary>
            /// A string that describes the type of file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        /// <summary>
        /// Contains strings returned from the IShellFolder interface methods.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size=264, CharSet = CharSet.Auto)]
        public struct STRRET
        {
            [FieldOffset(0)]
            public UInt32 uType; // A value that specifies the desired format of the string. This can be one of the following values.
            [FieldOffset(4)]
            public IntPtr pOleStr; // A pointer to the string. This memory must be allocated with CoTaskMemAlloc. It is the calling application's responsibility to free this memory with CoTaskMemFree when it is no longer needed.
            [FieldOffset(4)]
            public UInt32 uOffset; // Offset into the IID list
            [FieldOffset(4)]
            public IntPtr cStr; // The buffer to receive the display name
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

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        public interface IShellFolder
        {
            // Translates a file object's or folder's display name into an item identifier list.
            // Return value: error code, if any
            [PreserveSig()]
            uint ParseDisplayName(
                IntPtr hwnd,             // Optional window handle
                IntPtr pbc,              // Optional bind context that controls the parsing operation. This parameter is normally set to NULL. 
                [In(), MarshalAs(UnmanagedType.LPWStr)] 
                string pszDisplayName,   // Null-terminated UNICODE string with the display name.
                out uint pchEaten,       // Pointer to a ULONG value that receives the number of characters of the display name that was parsed.
                out IntPtr ppidl,        // Pointer to an ITEMIDLIST pointer that receives the item identifier list for the object.
                ref uint pdwAttributes); // Optional parameter that can be used to query for file attributes. This can be values from the SFGAO enum

            // Allows a client to determine the contents of a folder by creating an item identifier enumeration object and returning its IEnumIDList interface.
            // Return value: error code, if any
            [PreserveSig()]
            uint EnumObjects(
                IntPtr hwnd,                    // If user input is required to perform the enumeration, this window handle should be used by the enumeration object as the parent window to take user input.
                SHCONTF grfFlags,               // Flags indicating which items to include in the enumeration. For a list of possible values, see the SHCONTF enum. 
                out IEnumIDList ppenumIDList);  // Address that receives a pointer to the IEnumIDList interface of the enumeration object created by this method. 

            // Retrieves an IShellFolder object for a subfolder.
            // Return value: error code, if any
            [PreserveSig()]
            uint BindToObject(
                IntPtr pidl,            // Address of an ITEMIDLIST structure (PIDL) that identifies the subfolder.
                IntPtr pbc,             // Optional address of an IBindCtx interface on a bind context object to be used during this operation.
                [In()]
                ref Guid riid,          // Identifier of the interface to return. 
                out IShellFolder ppv);        // Address that receives the interface pointer.

            // Requests a pointer to an object's storage interface. 
            // Return value: error code, if any
            [PreserveSig()]
            uint BindToStorage(
                IntPtr pidl,            // Address of an ITEMIDLIST structure that identifies the subfolder relative to its parent folder. 
                IntPtr pbc,             // Optional address of an IBindCtx interface on a bind context object to be used during this operation.
                [In()]
                ref Guid riid,          // Interface identifier (IID) of the requested storage interface.
                [MarshalAs(UnmanagedType.Interface)]
                out object ppv);        // Address that receives the interface pointer specified by riid.

            // Determines the relative order of two file objects or folders, given their item identifier lists. 
            // Return value: If this method is successful, the CODE field of the HRESULT contains one of the following values (the code can be retrived using the helper function GetHResultCode)...
            // A negative return value indicates that the first item should precede the second (pidl1 < pidl2). 
            // A positive return value indicates that the first item should follow the second (pidl1 > pidl2).  Zero A return value of zero indicates that the two items are the same (pidl1 = pidl2). 
            [PreserveSig()]
            int CompareIDs(
                int lParam,             // Value that specifies how the comparison should be performed. The lower sixteen bits of lParam define the sorting rule.
                // The upper sixteen bits of lParam are used for flags that modify the sorting rule. values can be from the SHCIDS enum
                IntPtr pidl1,           // Pointer to the first item's ITEMIDLIST structure.
                IntPtr pidl2);          // Pointer to the second item's ITEMIDLIST structure.

            // Requests an object that can be used to obtain information from or interact with a folder object.
            // Return value: error code, if any
            [PreserveSig()]
            uint CreateViewObject(
                IntPtr hwndOwner,       // Handle to the owner window.
                [In()]
                ref Guid riid,          // Identifier of the requested interface.
                [MarshalAs(UnmanagedType.Interface)]
                out object ppv);        // Address of a pointer to the requested interface. 

            // Retrieves the attributes of one or more file objects or subfolders. 
            // Return value: error code, if any
            [PreserveSig()]
            uint GetAttributesOf(
                int cidl,               // Number of file objects from which to retrieve attributes. 
                [In(), MarshalAs(UnmanagedType.LPArray)] IntPtr[] 
                apidl,           // Address of an array of pointers to ITEMIDLIST structures, each of which uniquely identifies a file object relative to the parent folder.
                [MarshalAs(UnmanagedType.LPArray)] 
                SFGAOF[]
                rgfInOut);   // Address of a single ULONG value that, on entry, contains the attributes that the caller is requesting. On exit, this value contains the requested attributes that are common to all of the specified objects. this value can be from the SFGAO enum

            // Retrieves an OLE interface that can be used to carry out actions on the specified file objects or folders. 
            // Return value: error code, if any
            [PreserveSig()]
            uint GetUIObjectOf(
                IntPtr hwndOwner,       // Handle to the owner window that the client should specify if it displays a dialog box or message box.
                int cidl,               // Number of file objects or subfolders specified in the apidl parameter. 
                [In(), MarshalAs(UnmanagedType.LPArray)] IntPtr[]
                apidl,                  // Address of an array of pointers to ITEMIDLIST structures, each of which uniquely identifies a file object or subfolder relative to the parent folder.
                [In()]
                ref Guid riid,          // Identifier of the COM interface object to return.
                IntPtr rgfReserved,     // Reserved. 
                [MarshalAs(UnmanagedType.Interface)]
                out object ppv);        // Pointer to the requested interface.

            // Retrieves the display name for the specified file object or subfolder. 
            // Return value: error code, if any
            [PreserveSig()]
            uint GetDisplayNameOf(
                IntPtr pidl,            // Address of an ITEMIDLIST structure (PIDL) that uniquely identifies the file object or subfolder relative to the parent folder. 
                SHGDN uFlags,           // Flags used to request the type of display name to return. For a list of possible values. 
                out STRRET pName);      // Address of a STRRET structure in which to return the display name.

            // Sets the display name of a file object or subfolder, changing the item identifier in the process.
            // Return value: error code, if any
            [PreserveSig()]
            uint SetNameOf(
                IntPtr hwnd,            // Handle to the owner window of any dialog or message boxes that the client displays.
                IntPtr pidl,            // Pointer to an ITEMIDLIST structure that uniquely identifies the file object or subfolder relative to the parent folder. 
                [In(), MarshalAs(UnmanagedType.LPWStr)] 
                string pszName,         // Pointer to a null-terminated string that specifies the new display name. 
                SHGDN uFlags,           // Flags indicating the type of name specified by the lpszName parameter. For a list of possible values, see the description of the SHGNO enum. 
                out IntPtr ppidlOut);   // Address of a pointer to an ITEMIDLIST structure which receives the new ITEMIDLIST. 
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214e5-0000-0000-c000-000000000046")]
        public interface IShellIcon 
        {
            [PreserveSig()]
            int GetIconOf( // Gets an icon for an object inside a specific folder.
            IntPtr pidl, // The address of the ITEMIDLIST structure that specifies the relative location of the folder.
            uint flags, // Flags specifying how the icon is to display. This parameter can be zero or one of the following values.
            out int pIconIndex); // The address of the index of the icon in the system image list. The following standard image list indexes can be returned.
        }


        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214fa-0000-0000-c000-000000000046")]
        public interface IExtractIcon
        {
            [PreserveSig()]
            int GetIconLocation( // Gets the location and index of an icon.
                uint uFlags,
                [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 2)] 
                StringBuilder szIconFile, // A pointer to a buffer that receives the icon location. The icon location is a null-terminated string that identifies the file that contains the icon.
                int cchMax, // The size of the buffer, in characters, pointed to by pszIconFile.
                out int piIndex, // A pointer to an int that receives the index of the icon in the file pointed to by pszIconFile.
                out uint pwFlags); // A pointer to a UINT value that receives zero or a combination of the following values.

            [PreserveSig()]
            int Extract(
                [MarshalAs( UnmanagedType.LPWStr )] string pstFile, // A pointer to a null-terminated string that specifies the icon location.
                uint nIconIndex, // The index of the icon in the file pointed to by pszFile.
                out IntPtr phiconLarge, // A pointer to an HICON value that receives the handle to the large icon. This parameter may be NULL.
                out IntPtr phiconSmall, // A pointer to an HICON value that receives the handle to the small icon. This parameter may be NULL.
                uint nIconSize ); // The desired size of the icon, in pixels. The low word contains the size of the large icon, and the high word contains the size of the small icon. The size specified can be the width or height. The width of an icon always equals its height.
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("93F2F68C-1D1B-11d3-A30E-00C04F79ABD1")]
        public interface IShellFolder2 : IShellFolder // Extends the capabilities of IShellFolder. Its methods provide a variety of information about the contents of a Shell folder.
        {

            // Requests a pointer to an interface that allows a client to enumerate the available search objects.
            [PreserveSig()]
            uint EnumSearches( // Returns S_OK if successful, or a COM error value otherwise.
                [MarshalAs(UnmanagedType.Interface)]
                out object ppEnum); // The address of a pointer to an enumerator object's IEnumExtraSearch interface.

            // Gets the default sorting and display columns.
            [PreserveSig()]
            uint GetDefaultColumn( // Returns S_OK if successful, or a COM error value otherwise.
                uint dwReserved, // Reserved. Set to zero.
                out ulong pSort, // A pointer to a value that receives the index of the default sorted column.
                out ulong pDisplay); // A pointer to a value that receives the index of the default display column.

            // Gets the default state for a specified column.
            [PreserveSig()]
            uint GetDefaultColumnState( // f the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
                uint iColumn,   // An integer that specifies the column number.
                out uint pcsFlags); // SHCOLSTATEF, A pointer to a value that contains flags that indicate the default column state. This parameter can include a combination of the following flags.

            // Returns the globally unique identifier (GUID) of the default search object for the folder.
            [PreserveSig()]
            uint GetDefaultSearchGUID( // f the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
                ref Guid guid); // The GUID of the default search object.

            // Gets detailed information, identified by a property set identifier (FMTID) and a property identifier (PID), on an item in a Shell folder.
            [PreserveSig()]
            uint GetDetailsEx(// f the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
                IntPtr pidl, // A PIDL of the item, relative to the parent folder. This method accepts only single-level PIDLs. The structure must contain exactly one SHITEMID structure followed by a terminating zero. This value cannot be NULL.
                [In()]
                IntPtr pcsFlags, // A pointer to an SHCOLUMNID structure that identifies the column.
                out IntPtr pv); // A pointer to a VARIANT with the requested information. The value is fully typed. The value returned for properties from the property system must conform to the type specified in that property definition's typeInfo as the legacyType attribute.

            // Gets detailed information, identified by a column index, on an item in a Shell folder.
            [PreserveSig()]
            uint GetDetailsOf( // f the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
                IntPtr pidl, // PIDL of the item for which you are requesting information. This method accepts only single-level PIDLs. The structure must contain exactly one SHITEMID structure followed by a terminating zero. If this parameter is set to NULL, the title of the information field specified by iColumn is returned.
                uint iColumn, // The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.
                out SHELLDETAILS psd); // A pointer to a SHELLDETAILS structure that contains the information.

            // Converts a column to the appropriate property set ID (FMTID) and property ID (PID).
            [PreserveSig()]
            uint MapColumnToSCID( // If the method succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.
                uint iColumn, // The zero-based index of the desired information field. It is identical to the column number of the information as it is displayed in a Windows Explorer Details view.
                out IntPtr pscid); // A pointer to an SHCOLUMNID structure containing the FMTID and PID.
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F2-0000-0000-C000-000000000046")]
        public interface IEnumIDList
        {

            // Retrieves the specified number of item identifiers in the enumeration sequence and advances the current position by the number of items retrieved. 
            [PreserveSig()]
            uint Next(
                uint celt,                // Number of elements in the array pointed to by the rgelt parameter.
                out IntPtr rgelt,         // Address of an array of ITEMIDLIST pointers that receives the item identifiers. The implementation must allocate these item identifiers using the Shell's allocator (retrieved by the SHGetMalloc function). 
                // The calling application is responsible for freeing the item identifiers using the Shell's allocator.
                out Int32 pceltFetched    // Address of a value that receives a count of the item identifiers actually returned in rgelt. The count can be smaller than the value specified in the celt parameter. This parameter can be NULL only if celt is one. 
                );

            // Skips over the specified number of elements in the enumeration sequence. 
            [PreserveSig()]
            uint Skip(
                uint celt                 // Number of item identifiers to skip.
                );

            // Returns to the beginning of the enumeration sequence. 
            [PreserveSig()]
            uint Reset();

            // Creates a new item enumeration object with the same contents and state as the current one. 
            [PreserveSig()]
            uint Clone(
                out IEnumIDList ppenum    // Address of a pointer to the new enumeration object. The calling application must eventually free the new object by calling its Release member function. 
                );
        }

        #endregion
    }

}
