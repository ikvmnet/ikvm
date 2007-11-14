/*
  Copyright (C) 2004, 2007 Jeroen Frijters

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
package java.io;

import java.io.File;
import java.net.MalformedURLException;
import java.net.URL;

final class VMFile
{
    // HACK iff we run on unix, we assume a case sensitive file system
    static final boolean IS_CASE_SENSITIVE = cli.System.Environment.get_OSVersion().toString().indexOf("Unix") >= 0;;
    static final boolean IS_DOS_8_3 = false;

    private static long DateTimeToJavaLongTime(cli.System.DateTime datetime)
    {
	return cli.System.DateTime.op_Subtraction(cli.System.TimeZone.get_CurrentTimeZone().ToUniversalTime(datetime), new cli.System.DateTime(1970, 1, 1)).get_Ticks() / 10000L;
    }

    private static cli.System.DateTime JavaLongTimeToDateTime(long datetime)
    {
	return cli.System.TimeZone.get_CurrentTimeZone().ToLocalTime(new cli.System.DateTime(new cli.System.DateTime(1970, 1, 1).get_Ticks() + datetime * 10000L));
    }

    static long lastModified(String path)
    {
	try
	{
            if(false) throw new cli.System.UnauthorizedAccessException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.NotSupportedException();
	    return DateTimeToJavaLongTime(cli.System.IO.File.GetLastWriteTime(path));
	}
	catch(cli.System.UnauthorizedAccessException _)
	{
	    return 0;
	}
        catch(cli.System.ArgumentException _)
        {
            return 0;
        }
        catch(cli.System.IO.IOException _)
        {
            return 0;
        }
        catch(cli.System.NotSupportedException _)
        {
            return 0;
        }
    }

    private static cli.System.IO.FileInfo newFileInfo(String path)
        throws cli.System.ArgumentNullException,
            cli.System.Security.SecurityException,
            cli.System.ArgumentException,
            cli.System.UnauthorizedAccessException,
            cli.System.IO.PathTooLongException,
            cli.System.NotSupportedException
    {
        return new cli.System.IO.FileInfo(path);
    }

    static boolean setReadOnly(String path)
    {
	try
	{
	    cli.System.IO.FileInfo fileInfo = newFileInfo(path);
	    cli.System.IO.FileAttributes attr = fileInfo.get_Attributes();
	    attr = cli.System.IO.FileAttributes.wrap(attr.Value | cli.System.IO.FileAttributes.ReadOnly);
	    fileInfo.set_Attributes(attr);
	    return true;
	}
	catch(cli.System.Security.SecurityException _)
	{
	    return false;
	}
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean create(String path) throws IOException
    {
	try
	{
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.UnauthorizedAccessException();
            if(false) throw new cli.System.NotSupportedException();
	    cli.System.IO.File.Open(path, cli.System.IO.FileMode.wrap(cli.System.IO.FileMode.CreateNew)).Close();
	    return true;
	}
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static String[] list(String dirpath)
    {
	try
	{
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.UnauthorizedAccessException();
            String[] l = cli.System.IO.Directory.GetFileSystemEntries(dirpath);
	    for(int i = 0; i < l.length; i++)
	    {
		int pos = l[i].lastIndexOf(cli.System.IO.Path.DirectorySeparatorChar);
		if(pos >= 0)
		{
		    l[i] = l[i].substring(pos + 1);
		}
	    }
	    return l;
	}
        catch(cli.System.ArgumentException _)
        {
            return null;
        }
        catch(cli.System.IO.IOException _)
        {
            return null;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return null;
        }
    }

    static boolean renameTo(String targetpath, String destpath)
    {
	try
	{
	    newFileInfo(targetpath).MoveTo(destpath);
	    return true;
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static long length(String path)
    {
	try
	{
	    return newFileInfo(path).get_Length();
	}
        catch(cli.System.Security.SecurityException _)
        {
            return 0;
        }
        catch(cli.System.ArgumentException _)
        {
            return 0;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return 0;
        }
        catch(cli.System.IO.IOException _)
        {
            return 0;
        }
        catch(cli.System.NotSupportedException _)
        {
            return 0;
        }
    }

    static boolean exists(String path)
    {
	return cli.System.IO.File.Exists(path) || cli.System.IO.Directory.Exists(path);
    }

    static boolean delete(String path)
    {
	try
	{
            cli.System.IO.FileSystemInfo fileInfo;
            if(cli.System.IO.Directory.Exists(path))
            {
                fileInfo = new cli.System.IO.DirectoryInfo(path);
            }
            else if(cli.System.IO.File.Exists(path))
            {
                fileInfo = newFileInfo(path);
            }
            else
            {
                return false;
            }
            cli.System.IO.FileAttributes attr = fileInfo.get_Attributes();
            // We need to be able to delete read-only files/dirs too, so we clear
            // the read-only attribute, if set.
            if((attr.Value & cli.System.IO.FileAttributes.ReadOnly) != 0)
            {
                attr = cli.System.IO.FileAttributes.wrap(attr.Value & ~cli.System.IO.FileAttributes.ReadOnly);
                fileInfo.set_Attributes(attr);
            }
            fileInfo.Delete();
            return true;
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean setLastModified(String path, long time)
    {
	try
	{
	    newFileInfo(path).set_LastWriteTime(JavaLongTimeToDateTime(time));
	    return true;
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean mkdir(String path)
    {
        try
        {
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.UnauthorizedAccessException();
            if(false) throw new cli.System.ArgumentException();
            if(false) throw new cli.System.NotSupportedException();
            if(false) throw new cli.System.Security.SecurityException();
            cli.System.IO.DirectoryInfo parent = cli.System.IO.Directory.GetParent(path);
            if (parent == null ||
                !cli.System.IO.Directory.Exists(parent.get_FullName()) ||
                cli.System.IO.Directory.Exists(path))
            {
                return false;
            }
            return cli.System.IO.Directory.CreateDirectory(path) != null;
        }
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean isFile(String path)
    {
	return cli.System.IO.File.Exists(path);
    }

    static boolean canWrite(String path)
    {
	try
	{
            cli.System.IO.FileInfo fileInfo = newFileInfo(path);
            cli.System.IO.FileAttributes attr = fileInfo.get_Attributes();
            // Like the JDK we'll only look at the read-only attribute and not
            // the security permissions associated with the file or directory.
            return (attr.Value & cli.System.IO.FileAttributes.ReadOnly) == 0;
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean canWriteDirectory(File dir)
    {
        return canWrite(dir.getPath());
    }

    static boolean canRead(String path)
    {
	try
	{
	    // HACK if file refers to a directory, we always return true
	    if(cli.System.IO.Directory.Exists(path))
	    {
		return true;
	    }
	    newFileInfo(path).Open(
		cli.System.IO.FileMode.wrap(cli.System.IO.FileMode.Open),
		cli.System.IO.FileAccess.wrap(cli.System.IO.FileAccess.Read),
		cli.System.IO.FileShare.wrap(cli.System.IO.FileShare.ReadWrite)).Close();
	    return true;
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static boolean isDirectory(String path)
    {
	return cli.System.IO.Directory.Exists(path);
    }

    static File[] listRoots()
    {
        try
        {
            if(false) throw new cli.System.IO.IOException();
            if(false) throw new cli.System.UnauthorizedAccessException();
	    String[] roots = cli.System.IO.Directory.GetLogicalDrives();
	    File[] fileRoots = new File[roots.length];
	    for(int i = 0; i < roots.length; i++)
	    {
	        fileRoots[i] = new File(roots[i]);
	    }
	    return fileRoots;
        }
        catch(cli.System.IO.IOException _)
        {
            return new File[0];
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return new File[0];
        }
    }

    static boolean isHidden(String path)
    {
	try
	{
	    if(cli.System.IO.Directory.Exists(path))
	    {
		return (new cli.System.IO.DirectoryInfo(path).get_Attributes().Value & cli.System.IO.FileAttributes.Hidden) != 0;
	    }
	    else
	    {
		return (newFileInfo(path).get_Attributes().Value & cli.System.IO.FileAttributes.Hidden) != 0;
	    }
	}
        catch(cli.System.Security.SecurityException _)
        {
            return false;
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
            return false;
        }
        catch(cli.System.IO.IOException _)
        {
            return false;
        }
        catch(cli.System.NotSupportedException _)
        {
            return false;
        }
    }

    static String getName(String path)
    {
        try
        {
            if(false) throw new cli.System.ArgumentException();
	    return cli.System.IO.Path.GetFileName(path);
        }
        catch(cli.System.ArgumentException _)
        {
            // HACK this is not quite compatible with the JDK, but
            // since we only do this for invalid paths anyway, it
            // probably isn't worthwhile to be 100% correct.
            return path.substring(path.lastIndexOf(File.separatorChar) + 1);
        }
    }

    static String toCanonicalForm(String path) throws IOException
    {
	return getFileInfo(path).get_FullName();
    }

    private static cli.System.IO.FileInfo getFileInfo(String path) throws IOException
    {
	try
	{
	    return newFileInfo(path);
	}
	catch(cli.System.Security.SecurityException x1)
	{
	    throw new IOException(x1.getMessage());
	}
	catch(cli.System.ArgumentException x2)
	{
	    throw new IOException(x2.getMessage());
	}
	catch(cli.System.UnauthorizedAccessException x3)
	{
	    throw new IOException(x3.getMessage());
	}
	catch(cli.System.IO.IOException x4)
	{
	    throw new IOException(x4.getMessage());
	}
	catch(cli.System.NotSupportedException x5)
	{
	    throw new IOException(x5.getMessage());
	}
    }

    static String getAbsolutePath(String path)
    {
        // java.io.File.getAbsolutePath() only calls us if path is not absolute,
        // so we can:
        // assert(!isAbsolute(path));
        String userdir = new File(System.getProperty("user.dir")).getPath();
        if (File.separatorChar == '\\')
        {
            if (path.startsWith(File.separator))
            {
                if (userdir.length() > 1
                    && isDriveLetter(userdir.charAt(0))
                    && userdir.charAt(1) == ':')
                {
                    userdir = userdir.substring(0, 2);
                }
                return userdir + path;
            }
            else if (path.length() > 1
                && isDriveLetter(path.charAt(0))
                && path.charAt(1) == ':')
            {
                String dir = path.substring(0, 2);
                path = path.substring(2);
                if (userdir.length() > 2
                    // NOTE this is case sensitive comparison (broken like JDK 1.5)
                    && userdir.charAt(0) == dir.charAt(0)
                    && userdir.charAt(1) == ':'
                    && userdir.charAt(2) == File.separatorChar)
                {
                    return userdir + File.separator + path;
                }
                try
                {
                    // the += is to retain the case of the drive letter
                    dir += cli.System.IO.Path.GetFullPath(dir).substring(2);
                    if (dir.endsWith(File.separator))
                    {
                        dir = dir.substring(0, dir.length() - 1);
                    }
                    // NOTE oddly enough, we only need to do a security check in this case
                    SecurityManager sm = System.getSecurityManager();
                    if (sm != null)
                    {
                        sm.checkRead(dir + File.separator + path);
                    }
                }
                catch (Throwable _)
                {
                }
                return dir + File.separator + path;
            }
        }
        return userdir + File.separator + path;
    }

    private static boolean isDriveLetter(char c)
    {
        return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
    }

    static boolean isAbsolute(String path)
    {
        if (File.separatorChar == '\\')
        {
            if (path.length () > 0 && path.charAt(0) == '\\')
            {
                return path.length() > 1 && path.charAt(1) == '\\';
            }
            return path.length() > 2
                && isDriveLetter(path.charAt(0))
                && path.charAt(1) == ':'
                && path.charAt(2) == '\\';
        }
        else
        {
            return path.startsWith(File.separator);
        }
    }

    static URL toURL(File file) throws MalformedURLException
    {
        // On Win32, Sun's JDK returns URLs of the form "file:/c:/foo/bar.txt",
        // while on UNIX, it returns URLs of the form "file:/foo/bar.txt". 
        if (File.separatorChar == '\\')
            return new URL ("file:/" + file.getAbsolutePath().replace ('\\', '/')
                + (file.isDirectory() ? "/" : ""));
        else
            return new URL ("file:" + file.getAbsolutePath()
                + (file.isDirectory() ? "/" : ""));
    }

    static boolean setReadable(String path, boolean readable, boolean ownerOnly)
    {
        // TODO consider using Mono.Posix on Linux
        return readable;
    }

    static boolean setWritable(String path, boolean writable, boolean ownerOnly)
    {
        try
        {
            // TODO consider using Mono.Posix on Linux
            cli.System.IO.FileInfo file = newFileInfo(path);
            int attr = file.get_Attributes().Value & ~cli.System.IO.FileAttributes.ReadOnly;
            if (!writable)
            {
                attr |= cli.System.IO.FileAttributes.ReadOnly;
            }
            file.set_Attributes(cli.System.IO.FileAttributes.wrap(attr));
            return true;
        }
        catch(cli.System.Security.SecurityException _)
        {
        }
        catch(cli.System.ArgumentException _)
        {
        }
        catch(cli.System.UnauthorizedAccessException _)
        {
        }
        catch(cli.System.IO.IOException _)
        {
        }
        catch(cli.System.NotSupportedException _)
        {
        }
        return false;
    }
    
    static boolean setExecutable(String path, boolean executable, boolean ownerOnly)
    {
        // TODO consider using Mono.Posix on Linux
        return executable;
    }

    static boolean canExecute(String path)
    {
        // TODO consider using Mono.Posix on Linux
        return true;
    }

    static long getTotalSpace(String path)
    {
	// TODO
	return 0;
    }

    static long getFreeSpace(String path)
    {
	// TODO
	return 0;
    }

    static long getUsableSpace(String path)
    {
	// TODO
	return 0;
    }
}
