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

namespace IKVM.Runtime.JniExport.java.io
{

    static class UnixFileSystem
    {

        public static int getBooleanAttributes0(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.getBooleanAttributes(_this, f);
        }

        public static long getSpace(object _this, global::java.io.File f, int t)
        {
            // TODO
            return 0;
        }

        public static string canonicalize0(object _this, string path)
        {
            return WinNTFileSystem.canonicalize0(_this, path);
        }

        public static bool checkAccess(object _this, global::java.io.File f, int access)
        {
            return WinNTFileSystem.checkAccess(_this, f, access);
        }

        public static long getLastModifiedTime(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.getLastModifiedTime(_this, f);
        }

        public static long getLength(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.getLength(_this, f);
        }

        public static bool setPermission(object _this, global::java.io.File f, int access, bool enable, bool owneronly)
        {
            // TODO consider using Mono.Posix
            return WinNTFileSystem.setPermission(_this, f, access, enable, owneronly);
        }

        public static bool createFileExclusively(object _this, string path)
        {
            return WinNTFileSystem.createFileExclusively(_this, path);
        }

        public static bool delete0(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.delete0(_this, f);
        }

        public static string[] list(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.list(_this, f);
        }

        public static bool createDirectory(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.createDirectory(_this, f);
        }

        public static bool rename0(object _this, global::java.io.File f1, global::java.io.File f2)
        {
            return WinNTFileSystem.rename0(_this, f1, f2);
        }

        public static bool setLastModifiedTime(object _this, global::java.io.File f, long time)
        {
            return WinNTFileSystem.setLastModifiedTime(_this, f, time);
        }

        public static bool setReadOnly(object _this, global::java.io.File f)
        {
            return WinNTFileSystem.setReadOnly(_this, f);
        }

        public static void initIDs()
        {
        }
    }

}