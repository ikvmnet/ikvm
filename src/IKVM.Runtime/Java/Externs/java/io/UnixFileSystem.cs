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

namespace IKVM.Java.Externs.java.io
{

    static class UnixFileSystem
    {

        public static int getBooleanAttributes0(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.getBooleanAttributes(_this, (global::java.io.File)f);
#endif
        }

        public static long getSpace(object _this, object f, int t)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return 0;
#endif
        }

        public static string canonicalize0(object _this, string path)
        {
            return WinNTFileSystem.canonicalize0(_this, path);
        }

        public static bool checkAccess(object _this, object f, int access)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.checkAccess(_this, (global::java.io.File)f, access);
#endif
        }

        public static long getLastModifiedTime(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.getLastModifiedTime(_this, (global::java.io.File)f);
#endif
        }

        public static long getLength(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.getLength(_this, (global::java.io.File)f);
#endif
        }

        public static bool setPermission(object _this, object f, int access, bool enable, bool owneronly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // TODO consider using Mono.Posix
            return WinNTFileSystem.setPermission(_this, (global::java.io.File)f, access, enable, owneronly);
#endif
        }

        public static bool createFileExclusively(object _this, string path)
        {
            return WinNTFileSystem.createFileExclusively(_this, path);
        }

        public static bool delete0(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.delete0(_this, (global::java.io.File)f);
#endif
        }

        public static string[] list(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.list(_this, (global::java.io.File)f);
#endif
        }

        public static bool createDirectory(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.createDirectory(_this, (global::java.io.File)f);
#endif
        }

        public static bool rename0(object _this, object f1, object f2)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.rename0(_this, (global::java.io.File)f1, (global::java.io.File)f2);
#endif
        }

        public static bool setLastModifiedTime(object _this, object f, long time)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.setLastModifiedTime(_this, (global::java.io.File)f, time);
#endif
        }

        public static bool setReadOnly(object _this, object f)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return WinNTFileSystem.setReadOnly(_this, (global::java.io.File)f);
#endif
        }

        public static void initIDs()
        {

        }

    }

}