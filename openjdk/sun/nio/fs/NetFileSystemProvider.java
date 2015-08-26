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
import static ikvm.internal.Util.WINDOWS;
import cli.System.IO.Directory;
import cli.System.IO.DirectoryInfo;
import cli.System.IO.DriveInfo;
import cli.System.IO.File;
import cli.System.IO.FileAttributes;
import cli.System.IO.FileInfo;
import cli.System.IO.FileMode;
import cli.System.IO.FileShare;
import cli.System.IO.FileStream;
import cli.System.IO.FileOptions;
import cli.System.Runtime.InteropServices.DllImportAttribute;
import cli.System.Runtime.InteropServices.Marshal;
import cli.System.Security.AccessControl.FileSystemRights;
import com.sun.nio.file.ExtendedOpenOption;
import java.io.FileDescriptor;
import java.io.IOException;
import java.net.URI;
import java.nio.channels.*;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.nio.file.spi.FileSystemProvider;
import java.util.concurrent.ExecutorService;
import java.util.Iterator;
import java.util.HashMap;
import java.util.Map;
import java.util.NoSuchElementException;
import java.util.Set;
import sun.nio.ch.WindowsAsynchronousFileChannelImpl;
import sun.nio.ch.FileChannelImpl;
import sun.nio.ch.ThreadPool;

final class NetFileSystemProvider extends AbstractFileSystemProvider
{
    private final NetFileSystem fs = new NetFileSystem(this);
    private final HashMap<String, FileStore> stores = new HashMap<String, FileStore>();

    final synchronized FileStore getFileStore(DriveInfo drive) throws IOException
    {
        String name = drive.get_Name().toLowerCase();
        FileStore fs = stores.get(name);
        if (fs == null)
        {
            fs = new NetFileStore(drive);
            stores.put(name, fs);
        }
        return fs;
    }

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
        if (WINDOWS)
        {
            return WindowsUriSupport.fromUri(fs, uri);
        }
        else
        {
            return UnixUriUtils.fromUri(fs, uri);
        }
    }

    public AsynchronousFileChannel newAsynchronousFileChannel(Path path, Set<? extends OpenOption> opts, ExecutorService executor, FileAttribute<?>... attrs) throws IOException
    {
        NetPath npath = NetPath.from(path);
        for (FileAttribute<?> attr : attrs)
        {
            // null check
            attr.getClass();
            throw new NotYetImplementedError();
        }
        int mode = FileMode.Open;
        int share = FileShare.ReadWrite | FileShare.Delete;
        int options = FileOptions.Asynchronous;
        boolean read = false;
        boolean write = false;
        boolean truncate = false;
        for (OpenOption opt : opts)
        {
            if (opt instanceof StandardOpenOption)
            {
                switch ((StandardOpenOption)opt)
                {
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
                        break;
                    case SPARSE:
                        break;
                    case SYNC:
                        options |= FileOptions.WriteThrough;
                        break;
                    case TRUNCATE_EXISTING:
                        truncate = true;
                        break;
                    case WRITE:
                        write = true;
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
            else
            {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException();
            }
        }

        if (!read && !write)
        {
            read = true;
        }

        if (truncate)
        {
            if (mode == FileMode.Open)
            {
                mode = FileMode.Truncate;
            }
        }

        int rights = 0;
        if (read)
        {
            rights |= FileSystemRights.Read;
        }
        if (write)
        {
            rights |= FileSystemRights.Write;
        }

        ThreadPool pool;
        if (executor == null)
        {
            pool = null;
        }
        else
        {
            pool = ThreadPool.wrap(executor, 0);
        }

        return WindowsAsynchronousFileChannelImpl.open(open(npath.path, mode, rights, share, options), read, write, pool);
    }

    public SeekableByteChannel newByteChannel(Path path, Set<? extends OpenOption> opts, FileAttribute<?>... attrs) throws IOException
    {
        return newFileChannel(path, opts, attrs);
    }

    public FileChannel newFileChannel(Path path, Set<? extends OpenOption> opts, FileAttribute<?>... attrs) throws IOException
    {
        NetPath npath = NetPath.from(path);
        for (FileAttribute<?> attr : attrs)
        {
            // null check
            attr.getClass();
            throw new NotYetImplementedError();
        }
        int mode = FileMode.Open;
        int share = FileShare.ReadWrite | FileShare.Delete;
        int options = FileOptions.None;
        boolean read = false;
        boolean write = false;
        boolean append = false;
        boolean truncate = false;
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
                        break;
                    case SPARSE:
                        break;
                    case SYNC:
                        options |= FileOptions.WriteThrough;
                        break;
                    case TRUNCATE_EXISTING:
                        truncate = true;
                        break;
                    case WRITE:
                        write = true;
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
            else
            {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException();
            }
        }

        if (!read && !write)
        {
            read = true;
        }

        if (read && append)
        {
            throw new IllegalArgumentException("READ + APPEND not allowed");
        }
        
        if (truncate)
        {
            if (append)
            {
                throw new IllegalArgumentException("APPEND + TRUNCATE_EXISTING not allowed");
            }
            if (mode == FileMode.Open)
            {
                mode = FileMode.Truncate;
            }
        }
        
        int rights = 0;
        if (append)
        {
            // for atomic append to work, we can't set FileSystemRights.Write
            rights |= FileSystemRights.AppendData;
        }
        else
        {
            if (read)
            {
                rights |= FileSystemRights.Read;
            }
            if (write)
            {
                rights |= FileSystemRights.Write;
            }
        }

        return FileChannelImpl.open(open(npath.path, mode, rights, share, options), npath.path, read, write, append, null);
    }

    private static FileDescriptor open(String path, int mode, int rights, int share, int options) throws IOException
    {
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            if ((rights & FileSystemRights.Read) != 0)
            {
                sm.checkRead(path);
            }
            if ((rights & (FileSystemRights.Write | FileSystemRights.AppendData)) != 0)
            {
                sm.checkWrite(path);
            }
            if ((options & FileOptions.DeleteOnClose) != 0)
            {
                sm.checkDelete(path);
            }
        }

        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.PlatformNotSupportedException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            return FileDescriptor.fromStream(new FileStream(path, FileMode.wrap(mode), FileSystemRights.wrap(rights), FileShare.wrap(share), 8, FileOptions.wrap(options)));
        }
        catch (cli.System.ArgumentException x)
        {
            throw new FileSystemException(path, null, x.getMessage());
        }
        catch (cli.System.IO.FileNotFoundException _)
        {
            throw new NoSuchFileException(path);
        }
        catch (cli.System.IO.DirectoryNotFoundException _)
        {
            throw new NoSuchFileException(path);
        }
        catch (cli.System.PlatformNotSupportedException x)
        {
            throw new UnsupportedOperationException(x.getMessage());
        }
        catch (cli.System.IO.IOException x)
        {
            if (mode == FileMode.CreateNew && File.Exists(path))
            {
                throw new FileAlreadyExistsException(path);
            }
            throw new FileSystemException(path, null, x.getMessage());
        }
        catch (cli.System.Security.SecurityException _)
        {
            throw new AccessDeniedException(path);
        }
        catch (cli.System.UnauthorizedAccessException _)
        {
            throw new AccessDeniedException(path);
        }
    }

    public DirectoryStream<Path> newDirectoryStream(Path dir, final DirectoryStream.Filter<? super Path> filter) throws IOException
    {
        final String ndir = NetPath.from(dir).path;
        // null check
        filter.getClass();

        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(ndir);
        }

        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            final String[] files = Directory.GetFileSystemEntries(ndir);
            return new DirectoryStream<Path>() {
                private boolean closed;
                public Iterator<Path> iterator() {
                    if (closed) {
                        throw new IllegalStateException();
                    }
                    closed = true;
                    return new Iterator<Path>() {
                        private int pos;
                        private Path filtered;
                        public boolean hasNext() {
                            if (filtered == null) {
                                while (pos != files.length) {
                                    Path p = new NetPath(fs, cli.System.IO.Path.Combine(ndir, files[pos++]));
                                    try {
                                        if (filter.accept(p)) {
                                            filtered = p;
                                            break;
                                        }
                                    } catch (IOException x) {
                                        throw new DirectoryIteratorException(x);
                                    }
                                }
                            }
                            return filtered != null;
                        }
                        public Path next() {
                            if (!hasNext()) {
                                throw new NoSuchElementException();
                            }
                            Path p = filtered;
                            filtered = null;
                            return p;
                        }
                        public void remove() {
                            throw new UnsupportedOperationException();
                        }
                    };
                }
                public void close() {
                    closed = true;
                }
            };
        }
        catch (cli.System.ArgumentException
             | cli.System.IO.IOException
             | cli.System.Security.SecurityException
             | cli.System.UnauthorizedAccessException x)
        {
            if (File.Exists(ndir))
            {
                throw new NotDirectoryException(ndir);
            }
            throw new IOException(x.getMessage());
        }
    }

    public void createDirectory(Path dir, FileAttribute<?>... attrs) throws IOException
    {
        NetPath ndir = NetPath.from(dir);
        for (FileAttribute<?> attr : attrs)
        {
            // null check
            attr.getClass();
            throw new NotYetImplementedError();
        }
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkWrite(ndir.path);
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            Directory.CreateDirectory(ndir.path);
        }
        catch (cli.System.ArgumentException
             | cli.System.IO.IOException
             | cli.System.Security.SecurityException
             | cli.System.UnauthorizedAccessException x)
        {
            if (File.Exists(ndir.path))
            {
                throw new FileAlreadyExistsException(ndir.path);
            }
            throw new IOException(x.getMessage());
        }
    }

    public void copy(Path source, Path target, CopyOption... options) throws IOException
    {
        NetPath nsource = NetPath.from(source);
        NetPath ntarget = NetPath.from(target);
        boolean overwrite = false;
        boolean copyAttribs = false;
        for (CopyOption opt : options)
        {
            if (opt == StandardCopyOption.REPLACE_EXISTING)
            {
                overwrite = true;
            }
            else if (opt == StandardCopyOption.COPY_ATTRIBUTES)
            {
                copyAttribs = true;
            }
            else
            {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException("Unsupported copy option");
            }
        }
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(nsource.path);
            sm.checkWrite(ntarget.path);
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            if (File.Exists(ntarget.path))
            {
                if (!overwrite)
                {
                    throw new FileAlreadyExistsException(ntarget.path);
                }
                File.Delete(ntarget.path);
            }
            if (Directory.Exists(ntarget.path))
            {
                if (!overwrite)
                {
                    throw new FileAlreadyExistsException(ntarget.path);
                }
                try
                {
                    if (false) throw new cli.System.IO.IOException();
                    Directory.Delete(ntarget.path);
                }
                catch (cli.System.IO.IOException _)
                {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(ntarget.path);
                }
            }
            if (Directory.Exists(nsource.path))
            {
                Directory.CreateDirectory(ntarget.path);
            }
            else
            {
                File.Copy(nsource.path, ntarget.path, overwrite);
            }
            if (copyAttribs)
            {
                if (Directory.Exists(ntarget.path))
                {
                    File.SetAttributes(ntarget.path, File.GetAttributes(nsource.path));
                    Directory.SetCreationTimeUtc(ntarget.path, File.GetCreationTimeUtc(nsource.path));
                    Directory.SetLastAccessTimeUtc(ntarget.path, File.GetLastAccessTimeUtc(nsource.path));
                    Directory.SetLastWriteTimeUtc(ntarget.path, File.GetLastWriteTimeUtc(nsource.path));
                }
                else
                {
                    File.SetAttributes(ntarget.path, File.GetAttributes(nsource.path));
                    File.SetCreationTimeUtc(ntarget.path, File.GetCreationTimeUtc(nsource.path));
                    File.SetLastAccessTimeUtc(ntarget.path, File.GetLastAccessTimeUtc(nsource.path));
                    File.SetLastWriteTimeUtc(ntarget.path, File.GetLastWriteTimeUtc(nsource.path));
                }
            }
        }
        catch (cli.System.IO.FileNotFoundException x)
        {
            throw new NoSuchFileException(x.get_FileName());
        }
        catch (cli.System.IO.DirectoryNotFoundException x)
        {
            throw new NoSuchFileException(nsource.path, ntarget.path, x.getMessage());
        }
        catch (cli.System.IO.IOException | cli.System.ArgumentException x)
        {
            throw new FileSystemException(nsource.path, ntarget.path, x.getMessage());
        }
        catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x)
        {
            throw new AccessDeniedException(nsource.path, ntarget.path, x.getMessage());
        }
    }

    public void move(Path source, Path target, CopyOption... options) throws IOException
    {
        NetPath nsource = NetPath.from(source);
        NetPath ntarget = NetPath.from(target);
        boolean overwrite = false;
        boolean atomicMove = false;
        for (CopyOption opt : options)
        {
            if (opt == StandardCopyOption.REPLACE_EXISTING)
            {
                overwrite = true;
            }
            else if (opt == StandardCopyOption.ATOMIC_MOVE)
            {
                if (WINDOWS)
                {
                    atomicMove = true;
                }
                else
                {
                    throw new AtomicMoveNotSupportedException(nsource.path, ntarget.path, "Unsupported copy option");
                }
            }
            else
            {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException("Unsupported copy option");
            }
        }
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(nsource.path);
            sm.checkWrite(ntarget.path);
        }
        if (atomicMove)
        {
            int MOVEFILE_REPLACE_EXISTING = 1;
            if (MoveFileEx(nsource.path, ntarget.path, MOVEFILE_REPLACE_EXISTING) == 0)
            {
                final int ERROR_FILE_NOT_FOUND = 2;
                final int ERROR_PATH_NOT_FOUND = 3;
                final int ERROR_ACCESS_DENIED = 5;
                final int ERROR_NOT_SAME_DEVICE = 17;
                final int ERROR_FILE_EXISTS = 80;
                final int ERROR_ALREADY_EXISTS = 183;
                int lastError = Marshal.GetLastWin32Error();
                switch (lastError)
                {
                    case ERROR_FILE_NOT_FOUND:
                    case ERROR_PATH_NOT_FOUND:
                        throw new NoSuchFileException(nsource.path, ntarget.path, null);
                    case ERROR_ACCESS_DENIED:
                        throw new AccessDeniedException(nsource.path, ntarget.path, null);
                    case ERROR_NOT_SAME_DEVICE:
                        throw new AtomicMoveNotSupportedException(nsource.path, ntarget.path, "Unsupported copy option");
                    case ERROR_FILE_EXISTS:
                    case ERROR_ALREADY_EXISTS:
                        throw new FileAlreadyExistsException(nsource.path, ntarget.path, null);
                    default:
                        throw new FileSystemException(nsource.path, ntarget.path, "Error " + lastError);
                }
            }
            return;
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            if (File.Exists(ntarget.path))
            {
                if (!overwrite)
                {
                    throw new FileAlreadyExistsException(ntarget.path);
                }
                File.Delete(ntarget.path);
            }
            if (Directory.Exists(ntarget.path))
            {
                if (!overwrite)
                {
                    throw new FileAlreadyExistsException(ntarget.path);
                }
                try
                {
                    if (false) throw new cli.System.IO.IOException();
                    Directory.Delete(ntarget.path);
                }
                catch (cli.System.IO.IOException _)
                {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(ntarget.path);
                }
            }
            if (Directory.Exists(nsource.path))
            {
                Directory.Move(nsource.path, ntarget.path);
            }
            else
            {
                File.Move(nsource.path, ntarget.path);
            }
        }
        catch (cli.System.IO.FileNotFoundException x)
        {
            throw new NoSuchFileException(x.get_FileName());
        }
        catch (cli.System.IO.DirectoryNotFoundException x)
        {
            throw new NoSuchFileException(nsource.path, ntarget.path, x.getMessage());
        }
        catch (cli.System.IO.IOException | cli.System.ArgumentException x)
        {
            throw new FileSystemException(nsource.path, ntarget.path, x.getMessage());
        }
        catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x)
        {
            throw new AccessDeniedException(nsource.path, ntarget.path, x.getMessage());
        }
    }

    @DllImportAttribute.Annotation(value="kernel32", SetLastError=true)
    private static native int MoveFileEx(String lpExistingFileName, String lpNewFileName, int dwFlags);

    public boolean isSameFile(Path path, Path path2) throws IOException
    {
        if (path.equals(path2))
        {
            return true;
        }
        if (!(path instanceof NetPath && path2 instanceof NetPath))
        {
            // null check
            path2.getClass();
            return false;
        }
        return path.toRealPath().equals(path2.toRealPath());
    }

    public boolean isHidden(Path path) throws IOException
    {
        String npath = NetPath.from(path).path;
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(npath);
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            return (File.GetAttributes(npath).Value & (cli.System.IO.FileAttributes.Hidden | cli.System.IO.FileAttributes.Directory)) == cli.System.IO.FileAttributes.Hidden;
        }
        catch (cli.System.IO.FileNotFoundException x)
        {
            throw new NoSuchFileException(npath);
        }
        catch (cli.System.IO.IOException | cli.System.ArgumentException x)
        {
            throw new IOException(x.getMessage());
        }
    }

    private static class NetFileStore extends FileStore
    {
        private final DriveInfo info;
        private final String name;
        private final String type;

        NetFileStore(DriveInfo info) throws IOException
        {
            this.info = info;
            try
            {
                if (false) throw new cli.System.IO.IOException();
                name = info.get_VolumeLabel();
                type = info.get_DriveFormat();
            }
            catch (cli.System.IO.IOException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public Object getAttribute(String attribute) throws IOException
        {
            switch (attribute)
            {
                case "totalSpace":
                    return getTotalSpace();
                case "unallocatedSpace":
                    return getUnallocatedSpace();
                case "usableSpace":
                    return getUsableSpace();
                default:
                    throw new UnsupportedOperationException();
            }
        }

        public <V extends FileStoreAttributeView> V getFileStoreAttributeView(Class<V> type)
        {
            return null;
        }

        public long getTotalSpace() throws IOException
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                return info.get_TotalSize();
            }
            catch (cli.System.IO.IOException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public long getUnallocatedSpace() throws IOException
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                return info.get_TotalFreeSpace();
            }
            catch (cli.System.IO.IOException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public long getUsableSpace() throws IOException
        {
            try
            {
                if (false) throw new cli.System.IO.IOException();
                return info.get_AvailableFreeSpace();
            }
            catch (cli.System.IO.IOException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public boolean isReadOnly()
        {
            return false;
        }

        public String name()
        {
            return name;
        }

        public boolean supportsFileAttributeView(Class<? extends FileAttributeView> type)
        {
            // null check
            type.getClass();
            return type == BasicFileAttributeView.class || type == DosFileAttributeView.class;
        }

        public boolean supportsFileAttributeView(String name)
        {
            return name.equals("basic") || name.equals("dos");
        }

        public String type()
        {
            return type;
        }        

        public String toString()
        {
            return name + " (" + info.get_Name().charAt(0) + ":)";
        }
    }

    public FileStore getFileStore(Path path) throws IOException
    {
        NetPath npath = NetPath.from(path.toAbsolutePath());
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(npath.path);
        }
        return getFileStore(new DriveInfo(npath.path));
    }

    public void checkAccess(Path path, AccessMode... modes) throws IOException
    {
        String npath = NetPath.from(path).path;
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            if (modes.length == 0)
            {
                sm.checkRead(npath);
            }
            for (AccessMode m : modes)
            {
                switch (m)
                {
                    case READ:
                        sm.checkRead(npath);
                        break;
                    case WRITE:
                        sm.checkWrite(npath);
                        break;
                    case EXECUTE:
                        sm.checkExec(npath);
                        break;
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            // note that File.GetAttributes() works for directories as well
            int attr = File.GetAttributes(npath).Value;
            for (AccessMode m : modes)
            {
                switch (m)
                {
                    case READ:
                    case EXECUTE:
                        break;
                    case WRITE:
                        if ((attr & (cli.System.IO.FileAttributes.ReadOnly | cli.System.IO.FileAttributes.Directory)) == cli.System.IO.FileAttributes.ReadOnly)
                        {
                            throw new AccessDeniedException(npath, null, "file has read-only attribute set");
                        }
                        if (getFileStore(path).isReadOnly())
                        {
                            throw new AccessDeniedException(npath, null, "volume is read-only");
                        }
                        break;
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        }
        catch (cli.System.IO.FileNotFoundException | cli.System.IO.DirectoryNotFoundException _)
        {
            throw new NoSuchFileException(npath);
        }
        catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x)
        {
            throw new AccessDeniedException(npath, null, x.getMessage());
        }
        catch (cli.System.ArgumentException | cli.System.IO.IOException | cli.System.NotSupportedException x)
        {
            throw new IOException(x.getMessage());
        }
    }

    private static class BasicFileAttributesViewImpl extends AbstractBasicFileAttributeView
    {
        protected final String path;

        BasicFileAttributesViewImpl(String path)
        {
            this.path = path;
        }

        public BasicFileAttributes readAttributes() throws IOException
        {
            return DosFileAttributesViewImpl.readAttributesImpl(path);
        }

        public void setTimes(FileTime lastModifiedTime, FileTime lastAccessTime, FileTime createTime) throws IOException
        {
            SecurityManager sm = System.getSecurityManager();
            if (sm != null)
            {
                sm.checkWrite(path);
            }
            try
            {
                if (false) throw new cli.System.ArgumentException();
                if (false) throw new cli.System.IO.IOException();
                if (false) throw new cli.System.NotSupportedException();
                if (false) throw new cli.System.Security.SecurityException();
                if (false) throw new cli.System.UnauthorizedAccessException();
                if (File.Exists(path))
                {
                    if (lastModifiedTime != null)
                    {
                        File.SetLastWriteTimeUtc(path, toDateTime(lastModifiedTime));
                    }
                    if (lastAccessTime != null)
                    {
                        File.SetLastAccessTimeUtc(path, toDateTime(lastAccessTime));
                    }
                    if (createTime != null)
                    {
                        File.SetCreationTimeUtc(path, toDateTime(createTime));
                    }
                }
                else if (Directory.Exists(path))
                {
                    if (lastModifiedTime != null)
                    {
                        Directory.SetLastWriteTimeUtc(path, toDateTime(lastModifiedTime));
                    }
                    if (lastAccessTime != null)
                    {
                        Directory.SetLastAccessTimeUtc(path, toDateTime(lastAccessTime));
                    }
                    if (createTime != null)
                    {
                        Directory.SetCreationTimeUtc(path, toDateTime(createTime));
                    }
                }
                else
                {
                    throw new NoSuchFileException(path);
                }
            }
            catch (cli.System.ArgumentException
                 | cli.System.IO.IOException
                 | cli.System.NotSupportedException
                 | cli.System.Security.SecurityException
                 | cli.System.UnauthorizedAccessException x)
            {
                throw new IOException(x.getMessage());
            }
        }
    }

    private static class DosFileAttributesViewImpl extends BasicFileAttributesViewImpl implements DosFileAttributeView
    {
        private static final String READONLY_NAME = "readonly";
        private static final String ARCHIVE_NAME = "archive";
        private static final String SYSTEM_NAME = "system";
        private static final String HIDDEN_NAME = "hidden";
        private static final String ATTRIBUTES_NAME = "attributes";
        private static final Set<String> dosAttributeNames = Util.newSet(basicAttributeNames, READONLY_NAME, ARCHIVE_NAME, SYSTEM_NAME,  HIDDEN_NAME, ATTRIBUTES_NAME);

        DosFileAttributesViewImpl(String path)
        {
            super(path);
        }

        public String name()
        {
            return "dos";
        }

        public DosFileAttributes readAttributes() throws IOException
        {
            return readAttributesImpl(path);
        }

        private static class DosFileAttributesImpl implements DosFileAttributes
        {
            private final FileInfo info;

            DosFileAttributesImpl(FileInfo info)
            {
                this.info = info;
            }

            int attributes()
            {
                return info.get_Attributes().Value;
            }

            public FileTime creationTime()
            {
                return toFileTime(info.get_CreationTimeUtc());
            }

            public Object fileKey()
            {
                return null;
            }

            public boolean isDirectory()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.Directory) != 0;
            }

            public boolean isOther()
            {
                return false;
            }

            public boolean isRegularFile()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.Directory) == 0;
            }

            public boolean isSymbolicLink()
            {
                return false;
            }

            public FileTime lastAccessTime()
            {
                return toFileTime(info.get_LastAccessTimeUtc());
            }

            public FileTime lastModifiedTime()
            {
                return toFileTime(info.get_LastWriteTimeUtc());
            }

            public long size()
            {
                return info.get_Exists() ? info.get_Length() : 0;
            }

            public boolean isArchive()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.Archive) != 0;
            }

            public boolean isHidden()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.Hidden) != 0;
            }

            public boolean isReadOnly()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.ReadOnly) != 0;
            }

            public boolean isSystem()
            {
                return (info.get_Attributes().Value & cli.System.IO.FileAttributes.System) != 0;
            }
        }

        static DosFileAttributesImpl readAttributesImpl(String path) throws IOException
        {
            SecurityManager sm = System.getSecurityManager();
            if (sm != null)
            {
                sm.checkRead(path);
            }
            try
            {
                if (false) throw new cli.System.ArgumentException();
                if (false) throw new cli.System.IO.FileNotFoundException();
                if (false) throw new cli.System.IO.IOException();
                FileInfo info = new FileInfo(path);
                // We have to rely on the (undocumented) fact that FileInfo.Attributes returns -1
                // when the path does not exist. We need this to work for both files and directories
                // and this is the only efficient way to do that.
                if (info.get_Attributes().Value == -1)
                {
                    throw new NoSuchFileException(path);
                }
                return new DosFileAttributesImpl(info);
            }
            catch (cli.System.IO.FileNotFoundException _)
            {
                throw new NoSuchFileException(path);
            }
            catch (cli.System.IO.IOException | cli.System.ArgumentException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public void setArchive(boolean value) throws IOException
        {
            setAttribute(cli.System.IO.FileAttributes.Archive, value);
        }

        public void setHidden(boolean value) throws IOException
        {
            setAttribute(cli.System.IO.FileAttributes.Hidden, value);
        }

        public void setReadOnly(boolean value) throws IOException
        {
            setAttribute(cli.System.IO.FileAttributes.ReadOnly, value);
        }

        public void setSystem(boolean value) throws IOException
        {
            setAttribute(cli.System.IO.FileAttributes.System, value);
        }

        private void setAttribute(int attr, boolean value) throws IOException
        {
            SecurityManager sm = System.getSecurityManager();
            if (sm != null)
            {
                sm.checkWrite(path);
            }
            try
            {
                if (false) throw new cli.System.ArgumentException();
                if (false) throw new cli.System.IO.IOException();
                FileInfo info = new FileInfo(path);
                if (value)
                {
                    info.set_Attributes(cli.System.IO.FileAttributes.wrap(info.get_Attributes().Value | attr));
                }
                else
                {
                    info.set_Attributes(cli.System.IO.FileAttributes.wrap(info.get_Attributes().Value & ~attr));
                }
            }
            catch (cli.System.IO.FileNotFoundException _)
            {
                throw new NoSuchFileException(path);
            }
            catch (cli.System.ArgumentException
                 | cli.System.IO.IOException x)
            {
                throw new IOException(x.getMessage());
            }
        }

        public Map<String,Object> readAttributes(String[] attributes) throws IOException
        {
            AttributesBuilder builder = AttributesBuilder.create(dosAttributeNames, attributes);
            DosFileAttributesImpl attrs = readAttributesImpl(path);
            addRequestedBasicAttributes(attrs, builder);
            if (builder.match(READONLY_NAME))
            {
                builder.add(READONLY_NAME, attrs.isReadOnly());
            }
            if (builder.match(ARCHIVE_NAME))
            {
                builder.add(ARCHIVE_NAME, attrs.isArchive());
            }
            if (builder.match(SYSTEM_NAME))
            {
                builder.add(SYSTEM_NAME, attrs.isSystem());
            }
            if (builder.match(HIDDEN_NAME))
            {
                builder.add(HIDDEN_NAME, attrs.isHidden());
            }
            if (builder.match(ATTRIBUTES_NAME))
            {
                builder.add(ATTRIBUTES_NAME, attrs.attributes());
            }
            return builder.unmodifiableMap();
        }

        public void setAttribute(String attribute, Object value) throws IOException
        {
            switch (attribute)
            {
                case READONLY_NAME:
                    setReadOnly((Boolean)value);
                    break;
                case ARCHIVE_NAME:
                    setArchive((Boolean)value);
                    break;
                case SYSTEM_NAME:
                    setSystem((Boolean)value);
                    break;
                case HIDDEN_NAME:
                    setHidden((Boolean)value);
                    break;
                default:
                    super.setAttribute(attribute, value);
                    break;
            }
        }
    }

    private static void validateLinkOption(LinkOption... options)
    {
        for (LinkOption option : options)
        {
            if (option == LinkOption.NOFOLLOW_LINKS)
            {
                // ignored
            }
            else
            {
                // null check
                option.getClass();
                throw new UnsupportedOperationException();
            }
        }
    }

    public <V extends FileAttributeView> V getFileAttributeView(Path path, Class<V> type, LinkOption... options)
    {
        String npath = NetPath.from(path).path;
        validateLinkOption(options);
        if (type == BasicFileAttributeView.class)
        {
            return (V)new BasicFileAttributesViewImpl(npath);
        }
        else if (type == DosFileAttributeView.class)
        {
            return (V)new DosFileAttributesViewImpl(npath);
        }
        else
        {
            // null check
            type.getClass();
            return null;
        }
    }

    public <A extends BasicFileAttributes> A readAttributes(Path path, Class<A> type, LinkOption... options) throws IOException
    {
        String npath = NetPath.from(path).path;
        // null check
        type.getClass();
        validateLinkOption(options);
        if (type != BasicFileAttributes.class && type != DosFileAttributes.class)
        {
            throw new UnsupportedOperationException();
        }
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkRead(npath);
        }
        return (A)DosFileAttributesViewImpl.readAttributesImpl(npath);
    }

    DynamicFileAttributeView getFileAttributeView(Path file, String name, LinkOption... options)
    {
        validateLinkOption(options);
        if (name.equals("basic"))
        {
            return new BasicFileAttributesViewImpl(NetPath.from(file).path);
        }
        else if (name.equals("dos"))
        {
            return new DosFileAttributesViewImpl(NetPath.from(file).path);
        }
        else
        {
            return null;
        }
    }

    boolean implDelete(Path file, boolean failIfNotExists) throws IOException
    {
        String path = NetPath.from(file).path;
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            sm.checkDelete(path);
        }
        try
        {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            int attr = cli.System.IO.File.GetAttributes(path).Value;
            if ((attr & cli.System.IO.FileAttributes.Directory) != 0)
            {
                try
                {
                    if (false) throw new cli.System.IO.IOException();
                    cli.System.IO.Directory.Delete(path);
                }
                catch (cli.System.IO.IOException _)
                {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(path);
                }
                return true;
            }
            else
            {
                cli.System.IO.File.Delete(path);
                return true;
            }
        }
        catch (cli.System.ArgumentException x)
        {
            throw new FileSystemException(path, null, x.getMessage());
        }
        catch (cli.System.IO.FileNotFoundException _)
        {
            if (failIfNotExists)
            {
                throw new NoSuchFileException(path);
            }
            else
            {
                return false;
            }
        }
        catch (cli.System.IO.DirectoryNotFoundException _)
        {
            if (failIfNotExists)
            {
                throw new NoSuchFileException(path);
            }
            else
            {
                return false;
            }
        }
        catch (cli.System.IO.IOException x)
        {
            throw new FileSystemException(path, null, x.getMessage());
        }
        catch (cli.System.Security.SecurityException _)
        {
            throw new AccessDeniedException(path);
        }
        catch (cli.System.UnauthorizedAccessException _)
        {
            throw new AccessDeniedException(path);
        }
    }

    static FileTime toFileTime(cli.System.DateTime dateTime)
    {
        return FileTime.from((dateTime.get_Ticks() - 621355968000000000L) / 10, java.util.concurrent.TimeUnit.MICROSECONDS);
    }

    static cli.System.DateTime toDateTime(FileTime fileTime)
    {
        return new cli.System.DateTime(fileTime.to(java.util.concurrent.TimeUnit.MICROSECONDS) * 10 + 621355968000000000L);
    }
}
