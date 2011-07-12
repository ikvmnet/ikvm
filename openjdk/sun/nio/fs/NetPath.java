/*
  Copyright (C) 2011 Jeroen Frijters

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

package sun.nio.fs;

import ikvm.internal.NotYetImplementedError;
import java.io.File;
import java.io.IOException;
import java.net.URI;
import java.nio.file.*;
import java.util.Iterator;
import static ikvm.internal.Util.MACOSX;
import static ikvm.internal.Util.WINDOWS;

final class NetPath extends AbstractPath
{
    private static final char[] invalid = cli.System.IO.Path.GetInvalidFileNameChars();
    private final NetFileSystem fs;
    final String path;

    NetPath(NetFileSystem fs, String path)
    {
        StringBuilder sb = null;
        int separatorCount = 0;
        boolean prevWasSeparator = false;
        for (int i = 0; i < path.length(); i++)
        {
            char c = path.charAt(i);
            if (c == cli.System.IO.Path.AltDirectorySeparatorChar)
            {
                if (sb == null)
                {
                    sb = new StringBuilder();
                    sb.append(path, 0, i);
                }
                c = cli.System.IO.Path.DirectorySeparatorChar;
            }
            if (c == cli.System.IO.Path.DirectorySeparatorChar)
            {
                if (prevWasSeparator && (i != 1 || c != '\\'))
                {
                    if (sb == null)
                    {
                        sb = new StringBuilder();
                        sb.append(path, 0, i);
                    }
                    continue;
                }
                separatorCount++;
                prevWasSeparator = true;
            }
            else
            {
                prevWasSeparator = false;
            }
            if (sb != null)
            {
                sb.append(c);
            }
            if (c == cli.System.IO.Path.DirectorySeparatorChar)
            {
                continue;
            }
            else if (c == ':' && i > 0)
            {
                char d = path.charAt(0);
                if (((d >= 'a' && d <= 'z') || (d >= 'A' && d <= 'Z')) && i == 1 && WINDOWS)
                {
                    continue;
                }
                else if (MACOSX)
                {
                    continue;
                }
            }
            for (char inv : invalid)
            {
                if (inv == c)
                {
                    throw new InvalidPathException(path, "Illegal char <" + c + ">", i);
                }
            }
        }
        if (sb != null)
        {
            path = sb.toString();
        }
        if (path.startsWith("\\\\") && WINDOWS)
        {
            if (separatorCount == 2 || (separatorCount == 3 && path.endsWith("\\")))
            {
                throw new InvalidPathException(path, "UNC path is missing sharename");
            }
            else if (separatorCount == 3)
            {
                path += '\\';
            }
        }
        else if (path.length() == 3 && path.charAt(1) == ':' && WINDOWS)
        {
            // don't remove trailing backslash
        }
        else if (path.length() > 1 && path.charAt(path.length() - 1) == cli.System.IO.Path.DirectorySeparatorChar)
        {
            path = path.substring(0, path.length() - 1);
        }
        this.fs = fs;
        this.path = path;
    }

    public FileSystem getFileSystem()
    {
        return fs;
    }

    public boolean isAbsolute()
    {
        return cli.System.IO.Path.IsPathRooted(path)
            && (!WINDOWS || path.startsWith("\\\\") || (path.length() >= 3 && path.charAt(1) == ':' && path.charAt(2) == '\\'));
    }

    public Path getRoot()
    {
        throw new NotYetImplementedError();
    }

    public Path getFileName()
    {
        throw new NotYetImplementedError();
    }

    public Path getParent()
    {
        String parent = cli.System.IO.Path.GetDirectoryName(path);
        if (parent == null || parent.length() == 0)
        {
            return null;
        }
        return new NetPath(fs, parent);
    }

    public int getNameCount()
    {
        throw new NotYetImplementedError();
    }

    public Path getName(int index)
    {
        throw new NotYetImplementedError();
    }

    public Path subpath(int beginIndex, int endIndex)
    {
        throw new NotYetImplementedError();
    }

    public boolean startsWith(Path other)
    {
        throw new NotYetImplementedError();
    }

    public boolean endsWith(Path other)
    {
        throw new NotYetImplementedError();
    }

    public Path normalize()
    {
        throw new NotYetImplementedError();
    }

    public Path resolve(Path other)
    {
        NetPath nother = NetPath.from(other);
        if (nother.isAbsolute())
        {
            return other;
        }
        if (nother.path.length() == 0)
        {
            return this;
        }
        return new NetPath(fs, cli.System.IO.Path.Combine(path, nother.path));
    }

    public Path relativize(Path other)
    {
        throw new NotYetImplementedError();
    }

    public URI toUri()
    {
        throw new NotYetImplementedError();
    }

    public Path toAbsolutePath()
    {
        if (isAbsolute())
        {
            return this;
        }
        return new NetPath(fs, cli.System.IO.Path.GetFullPath(cli.System.IO.Path.Combine(System.getProperty("user.dir"), path)));
    }

    public Path toRealPath(LinkOption... options) throws IOException
    {
        return new NetPath(fs, toRealPathImpl(path));
    }
    
    private static native String toRealPathImpl(String path);

    public WatchKey register(WatchService watcher, WatchEvent.Kind<?>[] events, WatchEvent.Modifier... modifiers) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public int compareTo(Path other)
    {
        throw new NotYetImplementedError();
    }

    public boolean equals(Object other)
    {
        throw new NotYetImplementedError();
    }

    public int hashCode()
    {
        throw new NotYetImplementedError();
    }

    public String toString()
    {
        return path;
    }

    static NetPath from(Path path)
    {
        if (!(path instanceof NetPath))
        {
            throw new ProviderMismatchException();
        }
        return (NetPath)path;
    }
}
