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

using IKVM.Runtime;

namespace IKVM.Java.Externs.java.lang
{

    static class System
    {

        public static void registerNatives()
        {

        }

        public static void setIn0(object @in)
        {
#if !FIRST_PASS
            global::java.lang.StdIO.@in = (global::java.io.InputStream)@in;
#endif
        }

        public static void setOut0(object @out)
        {
#if !FIRST_PASS
            global::java.lang.StdIO.@out = (global::java.io.PrintStream)@out;
#endif
        }

        public static void setErr0(object err)
        {
#if !FIRST_PASS
            global::java.lang.StdIO.err = (global::java.io.PrintStream)err;
#endif
        }

        public static object initProperties(object props)
        {
#if FIRST_PASS
            return null;
#else
            global::java.lang.VMSystemProperties.initProperties((global::java.util.Properties)props);
            return props;
#endif
        }

        public static string mapLibraryName(string libname)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (libname == null)
                throw new global::java.lang.NullPointerException();

            if (RuntimeUtil.IsWindows)
            {
                return libname + ".dll";
            }
            else if (RuntimeUtil.IsOSX)
            {
                return "lib" + libname + ".jnilib";
            }
            else
            {
                return "lib" + libname + ".so";
            }
#endif
        }

        public static void arraycopy(object src, int srcPos, object dest, int destPos, int length)
        {
            ByteCodeHelper.arraycopy(src, srcPos, dest, destPos, length);
        }

    }

}
