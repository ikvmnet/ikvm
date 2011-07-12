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
import java.net.URI;
import java.nio.channels.*;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.nio.file.spi.FileSystemProvider;
import java.util.Map;
import java.util.Set;

final class NetFileSystemProvider extends AbstractFileSystemProvider
{
    private final NetFileSystem fs = new NetFileSystem(this);

    public String getScheme()
    {
        return "file";
    }

    public FileSystem newFileSystem(URI uri, Map<String, ?> env) throws IOException
    {
        throw new FileSystemAlreadyExistsException();
    }

    public FileSystem getFileSystem(URI uri)
    {
        return fs;
    }

    public Path getPath(URI uri)
    {
        throw new NotYetImplementedError();
    }

    public SeekableByteChannel newByteChannel(Path path, Set<? extends OpenOption> options, FileAttribute<?>... attrs) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public DirectoryStream<Path> newDirectoryStream(Path dir, DirectoryStream.Filter<? super Path> filter) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public void createDirectory(Path dir, FileAttribute<?>... attrs) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public void copy(Path source, Path target, CopyOption... options) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public void move(Path source, Path target, CopyOption... options) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public boolean isSameFile(Path path, Path path2) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public boolean isHidden(Path path) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public FileStore getFileStore(Path path) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public void checkAccess(Path path, AccessMode... modes) throws IOException
    {
        throw new NotYetImplementedError();
    }

    public <V extends FileAttributeView> V getFileAttributeView(Path path, Class<V> type, LinkOption... options)
    {
        throw new NotYetImplementedError();
    }

    public <A extends BasicFileAttributes> A readAttributes(Path path, Class<A> type, LinkOption... options) throws IOException
    {
        throw new NotYetImplementedError();
    }

    DynamicFileAttributeView getFileAttributeView(Path file, String name, LinkOption... options)
    {
        return null;
    }

    boolean implDelete(Path file, boolean failIfNotExists) throws IOException
    {
        throw new NotYetImplementedError();
    }
}
