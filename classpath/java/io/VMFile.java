/*
  Copyright (C) 2004 Jeroen Frijters

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
	    // TODO what if "path" is a directory?
	    return DateTimeToJavaLongTime(cli.System.IO.File.GetLastWriteTime(path));
	}
	catch(Throwable x)
	{
	    return 0;
	}
    }

    static boolean setReadOnly(String path)
    {
	try
	{
	    cli.System.IO.FileInfo fileInfo = new cli.System.IO.FileInfo(path);
	    cli.System.IO.FileAttributes attr = fileInfo.get_Attributes();
	    attr = cli.System.IO.FileAttributes.wrap(attr.Value | cli.System.IO.FileAttributes.ReadOnly);
	    fileInfo.set_Attributes(attr);
	    return true;
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean create(String path) throws IOException
    {
	try
	{
	    cli.System.IO.File.Open(path, cli.System.IO.FileMode.wrap(cli.System.IO.FileMode.CreateNew)).Close();
	    return true;
	}
	catch(Throwable x)
	{
	    // TODO handle errors
	    return false;
	}
    }

    static String[] list(String dirpath)
    {
	// TODO error handling
	try
	{
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
	catch(Throwable x)
	{
	    return null;
	}
    }

    static boolean renameTo(String targetpath, String destpath)
    {
	try
	{
	    new cli.System.IO.FileInfo(targetpath).MoveTo(destpath);
	    return true;
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static long length(String path)
    {
	// TODO handle errors
	try
	{
	    return new cli.System.IO.FileInfo(path).get_Length();
	}
	catch(Throwable x)
	{
	    return 0;
	}
    }

    static boolean exists(String path)
    {
	try
	{
	    return cli.System.IO.File.Exists(path) || cli.System.IO.Directory.Exists(path);
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean delete(String path)
    {
	// TODO handle errors
	try
	{
            cli.System.IO.FileSystemInfo fileInfo;
            if(cli.System.IO.Directory.Exists(path))
            {
                fileInfo = new cli.System.IO.DirectoryInfo(path);
            }
            else if(cli.System.IO.File.Exists(path))
            {
                fileInfo = new cli.System.IO.FileInfo(path);
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
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean setLastModified(String path, long time)
    {
	try
	{
	    new cli.System.IO.FileInfo(path).set_LastWriteTime(JavaLongTimeToDateTime(time));
	    return true;
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean mkdir(String path)
    {
	// TODO handle errors
        cli.System.IO.DirectoryInfo parent;
        try
        {
            if(false) throw new cli.System.ArgumentException();
            parent = cli.System.IO.Directory.GetParent(path);
        }
        catch(cli.System.ArgumentException _)
        {
            return false;
        }
	if (parent == null ||
            !cli.System.IO.Directory.Exists(parent.get_FullName()) ||
	    cli.System.IO.Directory.Exists(path))
	{
	    return false;
	}
	return cli.System.IO.Directory.CreateDirectory(path) != null;
    }

    static boolean isFile(String path)
    {
	// TODO handle errors
	// TODO make sure semantics are the same
	try
	{
	    return cli.System.IO.File.Exists(path);
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean canWrite(String path)
    {
	try
	{
            cli.System.IO.FileInfo fileInfo = new cli.System.IO.FileInfo(path);
            cli.System.IO.FileAttributes attr = fileInfo.get_Attributes();
            // Like the JDK we'll only look at the read-only attribute and not
            // the security permissions associated with the file or directory.
            return (attr.Value & cli.System.IO.FileAttributes.ReadOnly) == 0;
	}
	catch(Throwable x)
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
	    new cli.System.IO.FileInfo(path).Open(
		cli.System.IO.FileMode.wrap(cli.System.IO.FileMode.Open),
		cli.System.IO.FileAccess.wrap(cli.System.IO.FileAccess.Read),
		cli.System.IO.FileShare.wrap(cli.System.IO.FileShare.ReadWrite)).Close();
	    return true;
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static boolean isDirectory(String path)
    {
	// TODO handle errors
	// TODO make sure semantics are the same
	try
	{
	    return cli.System.IO.Directory.Exists(path);
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static File[] listRoots()
    {
	String[] roots = cli.System.IO.Directory.GetLogicalDrives();
	File[] fileRoots = new File[roots.length];
	for(int i = 0; i < roots.length; i++)
	{
	    fileRoots[i] = new File(roots[i]);
	}
	return fileRoots;
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
		return (new cli.System.IO.FileInfo(path).get_Attributes().Value & cli.System.IO.FileAttributes.Hidden) != 0;
	    }
	}
	catch(Throwable x)
	{
	    return false;
	}
    }

    static String getName(String path)
    {
	return cli.System.IO.Path.GetFileName(path);
    }

    static String toCanonicalForm(String path) throws IOException
    {
	return getFileInfo(path).get_FullName();
    }

    private static cli.System.IO.FileInfo getFileInfo(String path) throws IOException
    {
	try
	{
	    if(false) throw new cli.System.Security.SecurityException();
	    if(false) throw new cli.System.ArgumentException();
	    if(false) throw new cli.System.UnauthorizedAccessException();
	    if(false) throw new cli.System.IO.IOException();
	    if(false) throw new cli.System.NotSupportedException();
	    return new cli.System.IO.FileInfo(path);
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
}
