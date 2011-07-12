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
import cli.System.IO.FileMode;
import cli.System.IO.FileShare;
import cli.System.IO.FileStream;
import cli.System.IO.FileOptions;
import cli.System.Security.AccessControl.FileSystemRights;
import com.sun.nio.file.ExtendedOpenOption;
import java.io.FileDescriptor;
import java.io.IOException;
import java.net.URI;
import java.nio.channels.*;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.nio.file.spi.FileSystemProvider;
import java.util.Map;
import java.util.Set;
import sun.nio.ch.FileChannelImpl;

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

    public SeekableByteChannel newByteChannel(Path path, Set<? extends OpenOption> opts, FileAttribute<?>... attrs) throws IOException
    {
        NetPath npath = NetPath.from(path);
        if (attrs.length != 0)
        {
            throw new NotYetImplementedError();
        }
        int mode = FileMode.Open;
        int rights = 0;
        int share = FileShare.ReadWrite | FileShare.Delete;
        int options = FileOptions.None;
        boolean read = false;
        boolean write = false;
        boolean append = false;
        boolean sparse = false;
        for (OpenOption opt : opts)
        {
            if (opt instanceof StandardOpenOption)
            {
                switch ((StandardOpenOption)opt)
                {
                    case APPEND:
                        append = true;
                        write = true;
                        mode = FileMode.Append;
                        rights |= FileSystemRights.AppendData;
                        break;
                    case CREATE:
                        mode = FileMode.Create;
                        break;
                    case CREATE_NEW:
                        mode = FileMode.CreateNew;
                        break;
                    case DELETE_ON_CLOSE:
                        options |= FileOptions.DeleteOnClose;
                        break;
                    case DSYNC:
                        options |= FileOptions.WriteThrough;
                        break;
                    case READ:
                        read = true;
                        rights |= FileSystemRights.Read;
                        break;
                    case SPARSE:
                        sparse = true;
                        break;
                    case SYNC:
                        options |= FileOptions.WriteThrough;
                        break;
                    case TRUNCATE_EXISTING:
                        mode = FileMode.Truncate;
                        break;
                    case WRITE:
                        write = true;
                        rights |= FileSystemRights.Write;
                        break;
                    default:
                        throw new UnsupportedOperationException();
                }
            }
            else if (opt instanceof ExtendedOpenOption)
            {
                switch ((ExtendedOpenOption)opt)
                {
                    case NOSHARE_READ:
                        share &= ~FileShare.Read;
                        break;
                    case NOSHARE_WRITE:
                        share &= ~FileShare.Write;
                        break;
                    case NOSHARE_DELETE:
                        share &= ~FileShare.Delete;
                        break;
                    default:
                        throw new UnsupportedOperationException();
                }
            }
            else if (opt == null)
            {
                throw new NullPointerException();
            }
            else
            {
                throw new UnsupportedOperationException();
            }
        }

        if (!read && !write)
        {
            read = true;
            rights |= FileSystemRights.Read;
        }

        if (read && append)
        {
            throw new IllegalArgumentException("READ + APPEND not allowed");
        }
        
        if (append && mode == FileMode.Truncate)
        {
            throw new IllegalArgumentException("APPEND + TRUNCATE_EXISTING not allowed");
        }
        
        if (mode == FileMode.CreateNew && sparse)
        {
            throw new UnsupportedOperationException();
        }
        
        FileStream fs;
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.PlatformNotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            fs = new FileStream(npath.path, FileMode.wrap(mode), FileSystemRights.wrap(rights), FileShare.wrap(share), 8, FileOptions.wrap(options));
        }
        catch (cli.System.ArgumentException x)
        {
            throw new FileSystemException(npath.path, null, x.getMessage());
        }
        catch (cli.System.IO.FileNotFoundException _)
        {
            throw new NoSuchFileException(npath.path);
        }
        catch (cli.System.IO.DirectoryNotFoundException _)
        {
            throw new NoSuchFileException(npath.path);
        }
        catch (cli.System.PlatformNotSupportedException x)
        {
            throw new UnsupportedOperationException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            throw new FileSystemException(npath.path, null, x.getMessage());
        }
        catch (cli.System.Security.SecurityException _)
        {
            throw new AccessDeniedException(npath.path);
        }
        catch (cli.System.UnauthorizedAccessException _)
        {
            throw new AccessDeniedException(npath.path);
        }
        return FileChannelImpl.open(FileDescriptor.fromStream(fs), read, write, null, append);
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
