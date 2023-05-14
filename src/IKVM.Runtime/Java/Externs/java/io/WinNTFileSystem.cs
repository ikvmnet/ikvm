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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.io
{
    static class WinNTFileSystem
    {

        internal const int ACCESS_READ = 0x04;
        const int ACCESS_WRITE = 0x02;
        const int ACCESS_EXECUTE = 0x01;

        public static string getDriveDirectory(object _this, int drive)
        {
            try
            {
                string path = ((char)('A' + (drive - 1))) + ":";
                return Path.GetFullPath(path).Substring(2);
            }
            catch (ArgumentException)
            {
            }
            catch (SecurityException)
            {
            }
            catch (PathTooLongException)
            {
            }
            return "\\";
        }

        /// <summary>
        /// Attempts to canonicalize the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string CanonicalizePath(string path)
        {
            try
            {
                // begin by processing parent element
                var parent = Path.GetDirectoryName(path);

                // root paths with drive letter should have driver letter upper cased
                if (parent == null)
                    return path.Length > 1 && path[1] == ':' ? $"{char.ToUpper(path[0])}:{Path.DirectorySeparatorChar}" : path;
                else
                    parent = CanonicalizePath(parent);

                // trailing slash would result in a last path element of empty string
                var name = Path.GetFileName(path);
                if (name == "" || name == ".")
                    return parent;
                if (name == "..")
                    return Path.GetDirectoryName(parent);

                try
                {
                    if (VfsTable.Default.IsPath(path) == false)
                    {
                        // consult the file system for an actual node with the appropriate name
                        var all = Directory.EnumerateFileSystemEntries(parent, name);
                        if (all.FirstOrDefault() is string one)
                            name = Path.GetFileName(one);
                    }
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (IOException)
                {

                }

                return Path.Combine(parent, name);
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (SecurityException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return path;
        }

        public static string canonicalize0(object _this, string path)
        {
#if FIRST_PASS
            return null;
#else
            try
            {
                // TODO there is still a known bug here. A dotted path component right after the root component
                // are not removed as they should be. E.g. "c:\..." => "C:\..." or "\\server\..." => IOException
                // Another know issue is that when running under Mono on Windows, the case names aren't converted
                // to the correct (on file system) casing.
                //
                // FXBUG we're appending the directory separator to work around an apparent .NET bug.
                // If we don't do this, "c:\j\." would be canonicalized to "C:\"
                int colon = path.IndexOf(':', 2);
                if (colon != -1)
                    return CanonicalizePath(path.Substring(0, colon) + Path.DirectorySeparatorChar) + path.Substring(colon);

                return CanonicalizePath(path + Path.DirectorySeparatorChar);
            }
            catch (ArgumentException x)
            {
                throw new global::java.io.IOException(x.Message);
            }
#endif
        }

        public static string canonicalizeWithPrefix0(object _this, string canonicalPrefix, string pathWithCanonicalPrefix)
        {
            return canonicalize0(_this, pathWithCanonicalPrefix);
        }

        private static string GetPathFromFile(global::java.io.File file)
        {
#if FIRST_PASS
            return null;
#else
            return file.getPath();
#endif
        }

        public static int getBooleanAttributes(object _this, global::java.io.File f)
        {
            try
            {
                var path = GetPathFromFile(f);
                if (VfsTable.Default.IsPath(path))
                    return VfsTable.Default.GetBooleanAttributes(path);

                FileAttributes attr = File.GetAttributes(path);
                const int BA_EXISTS = 0x01;
                const int BA_REGULAR = 0x02;
                const int BA_DIRECTORY = 0x04;
                const int BA_HIDDEN = 0x08;
                int rv = BA_EXISTS;
                if ((attr & FileAttributes.Directory) != 0)
                {
                    rv |= BA_DIRECTORY;
                }
                else
                {
                    rv |= BA_REGULAR;
                }
                if ((attr & FileAttributes.Hidden) != 0)
                {
                    rv |= BA_HIDDEN;
                }
                return rv;
            }
            catch (ArgumentException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }
            catch (NotSupportedException)
            {
            }
            catch (IOException)
            {
            }
            return 0;
        }

        /// <summary>
        /// Checks if the given access bits are allowed on the given file.
        /// </summary>
        /// <param name="_this"></param>
        /// <param name="f"></param>
        /// <param name="access"></param>
        /// <returns></returns>
        public static bool checkAccess(object _this, global::java.io.File f, int access)
        {
            var path = GetPathFromFile(f);

            // check the VFS for the file
            if (VfsTable.Default.IsPath(path))
            {
                return VfsTable.Default.GetEntry(path) switch
                {
                    VfsFile file => access == ACCESS_READ && file.CanOpen(FileMode.Open, FileAccess.Read),
                    VfsDirectory => true,
                    _ => false,
                };
            }

            var ok = true;
            if ((access & (ACCESS_READ | ACCESS_EXECUTE)) != 0)
            {
                ok = false;

                try
                {
                    // HACK if path refers to a directory, we always return true
                    if (!Directory.Exists(path))
                    {
                        new FileInfo(path).Open(
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.ReadWrite).Close();
                    }

                    ok = true;
                }
                catch (SecurityException)
                {
                }
                catch (ArgumentException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
                catch (NotSupportedException)
                {
                }
            }
            if (ok && ((access & ACCESS_WRITE) != 0))
            {
                ok = false;
                try
                {
                    // HACK if path refers to a directory, we always return true
                    if (Directory.Exists(path))
                    {
                        ok = true;
                    }
                    else
                    {
                        FileInfo fileInfo = new FileInfo(path);
                        // Like the JDK we'll only look at the read-only attribute and not
                        // the security permissions associated with the file or directory.
                        ok = (fileInfo.Attributes & FileAttributes.ReadOnly) == 0;
                    }
                }
                catch (SecurityException)
                {
                }
                catch (ArgumentException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
                catch (NotSupportedException)
                {
                }
            }
            return ok;
        }

        /// <summary>
        /// The .NET ticks representing January 1, 1970 0:00:00, also known as the "epoch".
        /// </summary>
        private const long UnixEpochTicks = 621355968000000000L;

        private const long UnixEpochMilliseconds = UnixEpochTicks / TimeSpan.TicksPerMillisecond; // 62,135,596,800,000

        private static long DateTimeToJavaLongTime(DateTime datetime)
        {
            long milliseconds = datetime.ToUniversalTime().Ticks / TimeSpan.TicksPerMillisecond;
            return milliseconds - UnixEpochMilliseconds;
        }

        private static DateTime JavaLongTimeToDateTime(long datetime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(datetime).LocalDateTime;
        }

        public static long getLastModifiedTime(object _this, global::java.io.File f)
        {
            try
            {
                DateTime dt = File.GetLastWriteTime(GetPathFromFile(f));
                if (dt.ToFileTime() == 0)
                {
                    return 0;
                }
                else
                {
                    return DateTimeToJavaLongTime(dt);
                }
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return 0;
        }

        public static long getLength(object _this, global::java.io.File f)
        {
            try
            {
                var path = GetPathFromFile(f);
                if (VfsTable.Default.IsPath(path))
                    return VfsTable.Default.GetEntry(path) is VfsFile file ? file.Size : 0;

                return new FileInfo(path).Length;
            }
            catch (SecurityException)
            {

            }
            catch (ArgumentException)
            {

            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return 0;
        }

        public static bool setPermission(object _this, global::java.io.File f, int access, bool enable, bool owneronly)
        {
            if ((access & ACCESS_WRITE) != 0)
            {
                try
                {
                    FileInfo file = new FileInfo(GetPathFromFile(f));
                    if (enable)
                    {
                        file.Attributes &= ~FileAttributes.ReadOnly;
                    }
                    else
                    {
                        file.Attributes |= FileAttributes.ReadOnly;
                    }
                    return true;
                }
                catch (SecurityException)
                {
                }
                catch (ArgumentException)
                {
                }
                catch (UnauthorizedAccessException)
                {
                }
                catch (IOException)
                {
                }
                catch (NotSupportedException)
                {
                }
                return false;
            }
            return enable;
        }

        public static bool createFileExclusively(object _this, string path)
        {
#if !FIRST_PASS
            try
            {
                File.Open(path, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None).Close();
                return true;
            }
            catch (ArgumentException x)
            {
                throw new global::java.io.IOException(x.Message);
            }
            catch (IOException x)
            {
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    throw new global::java.io.IOException(x.Message);
                }
            }
            catch (UnauthorizedAccessException x)
            {
                if (!File.Exists(path) && !Directory.Exists(path))
                {
                    throw new global::java.io.IOException(x.Message);
                }
            }
            catch (NotSupportedException x)
            {
                throw new global::java.io.IOException(x.Message);
            }
#endif
            return false;
        }

        public static bool delete0(object _this, global::java.io.File f)
        {
            FileSystemInfo fileInfo = null;
            try
            {
                string path = GetPathFromFile(f);
                if (Directory.Exists(path))
                {
                    fileInfo = new DirectoryInfo(path);
                }
                else if (File.Exists(path))
                {
                    fileInfo = new FileInfo(path);
                }
                else
                {
                    return false;
                }
                // We need to be able to delete read-only files/dirs too, so we clear
                // the read-only attribute, if set.
                if ((fileInfo.Attributes & FileAttributes.ReadOnly) != 0)
                {
                    fileInfo.Attributes &= ~FileAttributes.ReadOnly;
                }
                fileInfo.Delete();
                return true;
            }
            catch (SecurityException)
            {

            }
            catch (ArgumentException)
            {

            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return false;
        }

        public static string[] list(object _this, global::java.io.File f)
        {
            try
            {
                var path = GetPathFromFile(f);
                if (VfsTable.Default.IsPath(path))
                {
                    if (VfsTable.Default.GetEntry(path) is VfsDirectory vfs)
                        return vfs.List();

                    throw new DirectoryNotFoundException();
                }

                string[] l = Directory.GetFileSystemEntries(path);
                for (int i = 0; i < l.Length; i++)
                {
                    int pos = l[i].LastIndexOf(Path.DirectorySeparatorChar);
                    if (pos >= 0)
                        l[i] = l[i].Substring(pos + 1);
                }

                return l;
            }
            catch (ArgumentException)
            {

            }
            catch (IOException)
            {

            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return null;
        }

        public static bool createDirectory(object _this, global::java.io.File f)
        {
            try
            {
                string path = GetPathFromFile(f);
                DirectoryInfo parent = Directory.GetParent(path);
                if (parent == null ||
                    !Directory.Exists(parent.FullName) ||
                    Directory.Exists(path))
                {
                    return false;
                }
                return Directory.CreateDirectory(path) != null;
            }
            catch (SecurityException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }

        public static bool rename0(object _this, global::java.io.File f1, global::java.io.File f2)
        {
            try
            {
                new FileInfo(GetPathFromFile(f1)).MoveTo(GetPathFromFile(f2));
                return true;
            }
            catch (SecurityException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }

        public static bool setLastModifiedTime(object _this, global::java.io.File f, long time)
        {
            try
            {
                new FileInfo(GetPathFromFile(f)).LastWriteTime = JavaLongTimeToDateTime(time);
                return true;
            }
            catch (SecurityException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }

        public static bool setReadOnly(object _this, global::java.io.File f)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(GetPathFromFile(f));
                fileInfo.Attributes |= FileAttributes.ReadOnly;
                return true;
            }
            catch (SecurityException)
            {
            }
            catch (ArgumentException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (IOException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }

        public static int listRoots0()
        {
            try
            {
                int drives = 0;
                foreach (string drive in Environment.GetLogicalDrives())
                {
                    char c = Char.ToUpper(drive[0]);
                    drives |= 1 << (c - 'A');
                }
                return drives;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
            catch (SecurityException)
            {
            }
            return 0;
        }

        [SecuritySafeCritical]
        public static long getSpace0(object _this, global::java.io.File f, int t)
        {
#if !FIRST_PASS
            long freeAvailable;
            long total;
            long totalFree;
            StringBuilder volname = new StringBuilder(256);
            if (GetVolumePathName(GetPathFromFile(f), volname, volname.Capacity) != 0
                && GetDiskFreeSpaceEx(volname.ToString(), out freeAvailable, out total, out totalFree) != 0)
            {
                switch (t)
                {
                    case global::java.io.FileSystem.SPACE_TOTAL:
                        return total;
                    case global::java.io.FileSystem.SPACE_FREE:
                        return totalFree;
                    case global::java.io.FileSystem.SPACE_USABLE:
                        return freeAvailable;
                }
            }
#endif
            return 0;
        }

        [DllImport("kernel32")]
        private static extern int GetDiskFreeSpaceEx(string directory, out long freeAvailable, out long total, out long totalFree);

        [DllImport("kernel32")]
        private static extern int GetVolumePathName(string lpszFileName, [In, Out] StringBuilder lpszVolumePathName, int cchBufferLength);

        public static void initIDs()
        {
        }
    }

}