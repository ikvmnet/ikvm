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
import ikvm.internal.io.FileStreamExtensions;

import cli.System.IO.Directory;
import cli.System.IO.DirectoryInfo;
import cli.System.IO.DriveInfo;
import cli.System.IO.File;
import cli.System.IO.FileAttributes;
import cli.System.IO.FileInfo;
import cli.System.IO.FileMode;
import cli.System.IO.FileShare;
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
import java.security.AccessController;
import java.security.PrivilegedAction;

import sun.nio.ch.DotNetAsynchronousFileChannelImpl;
import sun.nio.ch.FileChannelImpl;
import sun.nio.ch.ThreadPool;

final class DotNetFileSystemProvider extends AbstractFileSystemProvider {

    static {
        AccessController.doPrivileged(new PrivilegedAction<Void>() {
            public Void run() {
                System.loadLibrary("nio");
                return null;
        }});
    }

    private final DotNetFileSystem fs = new DotNetFileSystem(this);
    private final HashMap<String, FileStore> stores = new HashMap<String, FileStore>();

    final synchronized FileStore getFileStore(DriveInfo drive) throws IOException {
        String name = drive.get_Name().toLowerCase();
        FileStore fs = stores.get(name);
        if (fs == null) {
            fs = new DotNetFileStore(drive);
            stores.put(name, fs);
        }

        return fs;
    }

    public String getScheme() {
        return "file";
    }

    public FileSystem newFileSystem(URI uri, Map<String, ?> env) throws IOException {
        throw new FileSystemAlreadyExistsException();
    }

    public FileSystem getFileSystem(URI uri) {
        return fs;
    }

    public Path getPath(URI uri) {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            return DotNetWindowsUriSupport.fromUri(fs, uri);
        } else {
            return DotNetUnixUriUtils.fromUri(fs, uri);
        }
    }

    public AsynchronousFileChannel newAsynchronousFileChannel(Path path, Set<? extends OpenOption> opts, ExecutorService executor, FileAttribute<?>... attrs) throws IOException {
        DotNetPath npath = DotNetPath.from(path);

        for (FileAttribute<?> attr : attrs) {
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

        for (OpenOption opt : opts) {
            if (opt instanceof StandardOpenOption) {
                switch ((StandardOpenOption)opt) {
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
            } else if (opt instanceof ExtendedOpenOption) {
                switch ((ExtendedOpenOption)opt) {
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
            } else {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException();
            }
        }

        if (!read && !write) {
            read = true;
        }

        if (truncate) {
            if (mode == FileMode.Open) {
                mode = FileMode.Truncate;
            }
        }

        int rights = 0;
        if (read) {
            rights |= FileSystemRights.Read;
        }
        if (write) {
            rights |= FileSystemRights.Write;
        }

        ThreadPool pool;
        if (executor == null) {
            pool = null;
        } else {
            pool = ThreadPool.wrap(executor, 0);
        }

        return DotNetAsynchronousFileChannelImpl.open(open(npath.path, mode, rights, share, options), read, write, pool);
    }

    public SeekableByteChannel newByteChannel(Path path, Set<? extends OpenOption> opts, FileAttribute<?>... attrs) throws IOException {
        return newFileChannel(path, opts, attrs);
    }

    public FileChannel newFileChannel(Path path, Set<? extends OpenOption> opts, FileAttribute<?>... attrs) throws IOException {
        DotNetPath npath = DotNetPath.from(path);

        for (FileAttribute<?> attr : attrs) {
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

        for (OpenOption opt : opts) {
            if (opt instanceof StandardOpenOption) {
                switch ((StandardOpenOption)opt) {
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
            } else if (opt instanceof ExtendedOpenOption) {
                switch ((ExtendedOpenOption)opt) {
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
            } else {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException();
            }
        }

        if (!read && !write) {
            read = true;
        }

        if (read && append) {
            throw new IllegalArgumentException("READ + APPEND not allowed");
        }
        
        if (truncate) {
            if (append) {
                throw new IllegalArgumentException("APPEND + TRUNCATE_EXISTING not allowed");
            }
            if (mode == FileMode.Open) {
                mode = FileMode.Truncate;
            }
        }
        
        int rights = 0;
        if (append) {
            // for atomic append to work, we can't set FileSystemRights.Write
            rights |= FileSystemRights.AppendData;
        } else {
            if (read) {
                rights |= FileSystemRights.Read;
            }
            if (write) {
                rights |= FileSystemRights.Write;
            }
        }

        return FileChannelImpl.open(open(npath.path, mode, rights, share, options), npath.path, read, write, append, null);
    }

    private static FileDescriptor open(String path, int mode, int rights, int share, int options) throws IOException {
        return open0(path, FileMode.wrap(mode), FileSystemRights.wrap(rights), FileShare.wrap(share), 1, FileOptions.wrap(options), System.getSecurityManager());
    }

    public native DirectoryStream<Path> newDirectoryStream(Path dir, final DirectoryStream.Filter<? super Path> filter) throws IOException;

    public void createDirectory(Path dir, FileAttribute<?>... attrs) throws IOException {
        DotNetPath ndir = DotNetPath.from(dir);

        for (FileAttribute<?> attr : attrs) {
            // null check
            attr.getClass();
            throw new NotYetImplementedError();
        }

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkWrite(ndir.path);
        }

        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            Directory.CreateDirectory(ndir.path);
        } catch (cli.System.ArgumentException | cli.System.IO.IOException | cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x) {
            if (File.Exists(ndir.path)) {
                throw new FileAlreadyExistsException(ndir.path);
            }

            throw new IOException(x.getMessage());
        }
    }

    public void copy(Path source, Path target, CopyOption... options) throws IOException {
        DotNetPath nsource = DotNetPath.from(source);
        DotNetPath ntarget = DotNetPath.from(target);

        boolean overwrite = false;
        boolean copyAttribs = false;

        for (CopyOption opt : options) {
            if (opt == StandardCopyOption.REPLACE_EXISTING) {
                overwrite = true;
            } else if (opt == StandardCopyOption.COPY_ATTRIBUTES) {
                copyAttribs = true;
            } else {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException("Unsupported copy option");
            }
        }

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(nsource.path);
            sm.checkWrite(ntarget.path);
        }

        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();

            if (File.Exists(ntarget.path)) {
                if (!overwrite) {
                    throw new FileAlreadyExistsException(ntarget.path);
                }

                File.Delete(ntarget.path);
            }

            if (Directory.Exists(ntarget.path)) {
                if (!overwrite) {
                    throw new FileAlreadyExistsException(ntarget.path);
                }

                try {
                    if (false) throw new cli.System.IO.IOException();
                    Directory.Delete(ntarget.path);
                } catch (cli.System.IO.IOException _) {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(ntarget.path);
                }
            }

            if (Directory.Exists(nsource.path)) {
                Directory.CreateDirectory(ntarget.path);
            } else {
                File.Copy(nsource.path, ntarget.path, overwrite);
            }

            if (copyAttribs) {
                if (Directory.Exists(ntarget.path)) {
                    File.SetAttributes(ntarget.path, File.GetAttributes(nsource.path));
                    Directory.SetCreationTimeUtc(ntarget.path, File.GetCreationTimeUtc(nsource.path));
                    Directory.SetLastAccessTimeUtc(ntarget.path, File.GetLastAccessTimeUtc(nsource.path));
                    Directory.SetLastWriteTimeUtc(ntarget.path, File.GetLastWriteTimeUtc(nsource.path));
                } else {
                    File.SetAttributes(ntarget.path, File.GetAttributes(nsource.path));
                    File.SetCreationTimeUtc(ntarget.path, File.GetCreationTimeUtc(nsource.path));
                    File.SetLastAccessTimeUtc(ntarget.path, File.GetLastAccessTimeUtc(nsource.path));
                    File.SetLastWriteTimeUtc(ntarget.path, File.GetLastWriteTimeUtc(nsource.path));
                }
            }
        } catch (cli.System.IO.FileNotFoundException x) {
            throw new NoSuchFileException(x.get_FileName());
        } catch (cli.System.IO.DirectoryNotFoundException x) {
            throw new NoSuchFileException(nsource.path, ntarget.path, x.getMessage());
        } catch (cli.System.IO.IOException | cli.System.ArgumentException x) {
            throw new FileSystemException(nsource.path, ntarget.path, x.getMessage());
        } catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x) {
            throw new AccessDeniedException(nsource.path, ntarget.path, x.getMessage());
        }
    }

    @cli.System.Security.SecuritySafeCriticalAttribute.Annotation
    public void move(Path source, Path target, CopyOption... options) throws IOException {
        DotNetPath nsource = DotNetPath.from(source);
        DotNetPath ntarget = DotNetPath.from(target);
        boolean overwrite = false;
        boolean atomicMove = false;

        for (CopyOption opt : options) {
            if (opt == StandardCopyOption.REPLACE_EXISTING) {
                overwrite = true;
            } else if (opt == StandardCopyOption.ATOMIC_MOVE) {
                atomicMove = true;
            } else {
                // null check
                opt.getClass();
                throw new UnsupportedOperationException("Unsupported copy option");
            }
        }

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(nsource.path);
            sm.checkWrite(ntarget.path);
        }

        if (atomicMove) {
            rename0(nsource.path, ntarget.path);
            return;
        }

        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();

            if (File.Exists(ntarget.path)) {
                if (!overwrite) {
                    throw new FileAlreadyExistsException(ntarget.path);
                }
                File.Delete(ntarget.path);
            }

            if (Directory.Exists(ntarget.path)) {
                if (!overwrite) {
                    throw new FileAlreadyExistsException(ntarget.path);
                }

                try {
                    if (false) throw new cli.System.IO.IOException();
                    Directory.Delete(ntarget.path);
                } catch (cli.System.IO.IOException _) {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(ntarget.path);
                }
            }

            if (Directory.Exists(nsource.path)) {
                Directory.Move(nsource.path, ntarget.path);
            } else {
                File.Move(nsource.path, ntarget.path);
            }
        } catch (cli.System.IO.FileNotFoundException x) {
            throw new NoSuchFileException(x.get_FileName());
        } catch (cli.System.IO.DirectoryNotFoundException x) {
            throw new NoSuchFileException(nsource.path, ntarget.path, x.getMessage());
        } catch (cli.System.IO.IOException | cli.System.ArgumentException x) {
            throw new FileSystemException(nsource.path, ntarget.path, x.getMessage());
        } catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x) {
            throw new AccessDeniedException(nsource.path, ntarget.path, x.getMessage());
        }
    }

    private static native void rename0(String source, String target) throws IOException;

    public boolean isSameFile(Path path, Path path2) throws IOException {
        if (path.equals(path2)) {
            return true;
        }

        if (!(path instanceof DotNetPath && path2 instanceof DotNetPath)) {
            // null check
            path2.getClass();
            return false;
        }

        return path.toRealPath().equals(path2.toRealPath());
    }

    public boolean isHidden(Path path) throws IOException {
        String npath = DotNetPath.from(path).path;

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(npath);
        }

        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            return (File.GetAttributes(npath).Value & (cli.System.IO.FileAttributes.Hidden | cli.System.IO.FileAttributes.Directory)) == cli.System.IO.FileAttributes.Hidden;
        } catch (cli.System.IO.FileNotFoundException x) {
            throw new NoSuchFileException(npath);
        } catch (cli.System.IO.IOException | cli.System.ArgumentException x) {
            throw new IOException(x.getMessage());
        }
    }

    private static class DotNetFileStore extends FileStore {

        private final DriveInfo info;
        private final String name;
        private final String type;

        DotNetFileStore(DriveInfo info) throws IOException {
            this.info = info;
            try {
                if (false) throw new cli.System.IO.IOException();
                name = info.get_VolumeLabel();
                type = info.get_DriveFormat();
            } catch (cli.System.IO.IOException x) {
                throw new IOException(x.getMessage());
            }
        }

        public Object getAttribute(String attribute) throws IOException {
            switch (attribute) {
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

        public <V extends FileStoreAttributeView> V getFileStoreAttributeView(Class<V> type) {
            return null;
        }

        public long getTotalSpace() throws IOException {
            try {
                if (false) throw new cli.System.IO.IOException();
                return info.get_TotalSize();
            } catch (cli.System.IO.IOException x) {
                throw new IOException(x.getMessage());
            }
        }

        public long getUnallocatedSpace() throws IOException {
            try {
                if (false) throw new cli.System.IO.IOException();
                return info.get_TotalFreeSpace();
            } catch (cli.System.IO.IOException x) {
                throw new IOException(x.getMessage());
            }
        }

        public long getUsableSpace() throws IOException {
            try {
                if (false) throw new cli.System.IO.IOException();
                return info.get_AvailableFreeSpace();
            } catch (cli.System.IO.IOException x) {
                throw new IOException(x.getMessage());
            }
        }

        public boolean isReadOnly() {
            return false;
        }

        public String name() {
            return name;
        }

        public boolean supportsFileAttributeView(Class<? extends FileAttributeView> type) {
            // null check
            type.getClass();
            return type == BasicFileAttributeView.class || type == DosFileAttributeView.class;
        }

        public boolean supportsFileAttributeView(String name) {
            return name.equals("basic") || name.equals("dos");
        }

        public String type() {
            return type;
        }        

        public String toString() {
            return name + " (" + info.get_Name().charAt(0) + ":)";
        }

    }

    public FileStore getFileStore(Path path) throws IOException {
        DotNetPath npath = DotNetPath.from(path.toAbsolutePath());
        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(npath.path);
        }

        return getFileStore(new DriveInfo(npath.path));
    }

    public void checkAccess(Path path, AccessMode... modes) throws IOException {
        String npath = DotNetPath.from(path).path;

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            if (modes.length == 0) {
                sm.checkRead(npath);
            }

            for (AccessMode m : modes) {
                switch (m) {
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

        try {
            if (false) throw new cli.System.ArgumentException();
            if (false) throw new cli.System.IO.DirectoryNotFoundException();
            if (false) throw new cli.System.IO.FileNotFoundException();
            if (false) throw new cli.System.IO.IOException();
            if (false) throw new cli.System.NotSupportedException();
            if (false) throw new cli.System.Security.SecurityException();
            if (false) throw new cli.System.UnauthorizedAccessException();
            // note that File.GetAttributes() works for directories as well
            int attr = File.GetAttributes(npath).Value;
            for (AccessMode m : modes) {
                switch (m) {
                    case READ:
                    case EXECUTE:
                        break;
                    case WRITE:
                        if ((attr & (cli.System.IO.FileAttributes.ReadOnly | cli.System.IO.FileAttributes.Directory)) == cli.System.IO.FileAttributes.ReadOnly) {
                            throw new AccessDeniedException(npath, null, "file has read-only attribute set");
                        }
                        if (getFileStore(path).isReadOnly()) {
                            throw new AccessDeniedException(npath, null, "volume is read-only");
                        }
                        break;
                    default:
                        throw new UnsupportedOperationException();
                }
            }
        } catch (cli.System.IO.FileNotFoundException | cli.System.IO.DirectoryNotFoundException _) {
            throw new NoSuchFileException(npath);
        } catch (cli.System.Security.SecurityException | cli.System.UnauthorizedAccessException x) {
            throw new AccessDeniedException(npath, null, x.getMessage());
        } catch (cli.System.ArgumentException | cli.System.IO.IOException | cli.System.NotSupportedException x) {
            throw new IOException(x.getMessage());
        }
    }


    private static void validateLinkOption(LinkOption... options) {
        for (LinkOption option : options) {
            if (option == LinkOption.NOFOLLOW_LINKS) {
                // ignored
            } else {
                // null check
                option.getClass();
                throw new UnsupportedOperationException();
            }
        }
    }

    public <V extends FileAttributeView> V getFileAttributeView(Path path, Class<V> type, LinkOption... options) {
        String npath = DotNetPath.from(path).path;
        validateLinkOption(options);
        if (type == BasicFileAttributeView.class) {
            return (V)new DotNetBasicFileAttributeView(npath);
        } else if (type == DosFileAttributeView.class) {
            return (V)new DotNetDosFileAttributeView(npath);
        } else {
            // null check
            type.getClass();
            return null;
        }
    }

    public <A extends BasicFileAttributes> A readAttributes(Path path, Class<A> type, LinkOption... options) throws IOException {
        String npath = DotNetPath.from(path).path;
        // null check
        type.getClass();
        validateLinkOption(options);
        if (type != BasicFileAttributes.class && type != DosFileAttributes.class) {
            throw new UnsupportedOperationException();
        }

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(npath);
        }

        return (A)DotNetDosFileAttributes.read(npath);
    }

    DynamicFileAttributeView getFileAttributeView(Path file, String name, LinkOption... options) {
        validateLinkOption(options);
        if (name.equals("basic")) {
            return new DotNetBasicFileAttributeView(DotNetPath.from(file).path);
        } else if (name.equals("dos")) {
            return new DotNetDosFileAttributeView(DotNetPath.from(file).path);
        } else {
            return null;
        }
    }

    boolean implDelete(Path file, boolean failIfNotExists) throws IOException {
        String path = DotNetPath.from(file).path;

        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
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
            if ((attr & cli.System.IO.FileAttributes.Directory) != 0) {
                try {
                    if (false) throw new cli.System.IO.IOException();
                    cli.System.IO.Directory.Delete(path);
                } catch (cli.System.IO.IOException _) {
                    // HACK we assume that the IOException is caused by the directory not being empty
                    throw new DirectoryNotEmptyException(path);
                }

                return true;
            } else {
                cli.System.IO.File.Delete(path);
                return true;
            }
        } catch (cli.System.ArgumentException x) {
            throw new FileSystemException(path, null, x.getMessage());
        } catch (cli.System.IO.FileNotFoundException _) {
            if (failIfNotExists) {
                throw new NoSuchFileException(path);
            } else {
                return false;
            }
        } catch (cli.System.IO.DirectoryNotFoundException _) {
            if (failIfNotExists) {
                throw new NoSuchFileException(path);
            } else {
                return false;
            }
        } catch (cli.System.IO.IOException x) {
            throw new FileSystemException(path, null, x.getMessage());
        } catch (cli.System.Security.SecurityException _) {
            throw new AccessDeniedException(path);
        } catch (cli.System.UnauthorizedAccessException _) {
            throw new AccessDeniedException(path);
        }
    }
    
    static native FileDescriptor open0(String path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, SecurityManager sm) throws java.io.IOException;

}
