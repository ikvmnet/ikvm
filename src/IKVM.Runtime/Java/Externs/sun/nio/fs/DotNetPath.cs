/*
  Copyright (C) 2007-2015 Jeroen Frijters
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
using System.IO;
using System.Security;

using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.sun.nio.fs
{

    static class DotNetPath
    {

        public static string toRealPathImpl(string path)
        {
#if FIRST_PASS
            return null;
#else
            path = global::java.io.DefaultFileSystem.getFileSystem().canonicalize(path);
            if (VfsTable.Default.IsPath(path))
            {
                if (VfsTable.Default.GetEntry(path) is VfsFile file)
                {
                    if (file.CanOpen(FileMode.Open, FileAccess.Read) == false)
                        throw new global::java.nio.file.AccessDeniedException(path);

                    return path;
                }

                throw new global::java.nio.file.NoSuchFileException(path);
            }

            try
            {
                File.GetAttributes(path);
                return path;
            }
            catch (FileNotFoundException)
            {
                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (DirectoryNotFoundException)
            {
                throw new global::java.nio.file.NoSuchFileException(path);
            }
            catch (UnauthorizedAccessException)
            {
                throw new global::java.nio.file.AccessDeniedException(path);
            }
            catch (SecurityException)
            {
                throw new global::java.nio.file.AccessDeniedException(path);
            }
            catch (ArgumentException x)
            {
                throw new global::java.nio.file.FileSystemException(path, null, x.Message);
            }
            catch (NotSupportedException x)
            {
                throw new global::java.nio.file.FileSystemException(path, null, x.Message);
            }
            catch (IOException x)
            {
                throw new global::java.nio.file.FileSystemException(path, null, x.Message);
            }
#endif
        }

    }

}
