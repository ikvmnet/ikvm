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
import java.io.IOException;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.nio.file.spi.FileSystemProvider;
import java.util.Set;

final class NetFileSystem extends FileSystem
{
    private final NetFileSystemProvider provider;
    private final String separator = Character.toString(cli.System.IO.Path.DirectorySeparatorChar);

    NetFileSystem(NetFileSystemProvider provider)
    {
        this.provider = provider;
    }

    public FileSystemProvider provider()
    {
        return provider;
    }

    public void close() throws IOException
    {
        throw new UnsupportedOperationException();
    }

    public boolean isOpen()
    {
        return true;
    }

    public boolean isReadOnly()
    {
        return false;
    }

    public String getSeparator()
    {
        return separator;
    }

    public Iterable<Path> getRootDirectories()
    {
        throw new NotYetImplementedError();
    }

    public Iterable<FileStore> getFileStores()
    {
        throw new NotYetImplementedError();
    }

    public Set<String> supportedFileAttributeViews()
    {
        throw new NotYetImplementedError();
    }

    public Path getPath(String first, String... more)
    {
        if (more.length == 0)
        {
            return new NetPath(this, first);
        }
        else
        {
            StringBuilder sb = new StringBuilder(first);
            String sep = sb.length() == 0 ? "" : separator;
            for (String s : more)
            {
                if (s.length() != 0)
                {
                    sb.append(sep);
                    sb.append(s);
                    sep = separator;
                }
            }
            return new NetPath(this, sb.toString());
        }
    }

    public PathMatcher getPathMatcher(String syntaxAndPattern)
    {
        throw new NotYetImplementedError();
    }

    public UserPrincipalLookupService getUserPrincipalLookupService()
    {
        throw new NotYetImplementedError();
    }

    public WatchService newWatchService() throws IOException
    {
        throw new NotYetImplementedError();
    }
}
