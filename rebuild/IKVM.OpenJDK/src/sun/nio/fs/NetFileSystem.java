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

import cli.System.IO.DriveInfo;
import cli.System.IO.ErrorEventArgs;
import cli.System.IO.ErrorEventHandler;
import cli.System.IO.FileSystemEventArgs;
import cli.System.IO.FileSystemEventHandler;
import cli.System.IO.FileSystemWatcher;
import cli.System.IO.WatcherChangeTypes;
import java.io.IOException;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.nio.file.spi.FileSystemProvider;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.concurrent.LinkedBlockingQueue;
import java.util.concurrent.TimeUnit;
import java.util.HashSet;
import java.util.List;
import java.util.Set;
import java.util.regex.Pattern;
import static ikvm.internal.Util.WINDOWS;

final class NetFileSystem extends FileSystem
{
    private static final Set<String> attributes = Collections.unmodifiableSet(new HashSet<String>(Arrays.asList("basic")));
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
        SecurityManager sm = System.getSecurityManager();
        ArrayList<Path> list = new ArrayList<>();
        for (DriveInfo info : DriveInfo.GetDrives())
        {
            try
            {
                if (sm != null)
                {
                    sm.checkRead(info.get_Name());
                }
            }
            catch (SecurityException _)
            {
                continue;
            }
            list.add(getPath(info.get_Name()));
        }
        return list;
    }

    public Iterable<FileStore> getFileStores()
    {
        SecurityManager sm = System.getSecurityManager();
        if (sm != null)
        {
            try
            {
                sm.checkPermission(new RuntimePermission("getFileStoreAttributes"));
            }
            catch (SecurityException _)
            {
                return Collections.emptyList();
            }
        }
        ArrayList<FileStore> list = new ArrayList<>();
        for (DriveInfo info : DriveInfo.GetDrives())
        {
            try
            {
                if (sm != null)
                {
                    sm.checkRead(info.get_Name());
                }
            }
            catch (SecurityException _)
            {
                continue;
            }
            try
            {
                list.add(provider.getFileStore(info));
            }
            catch (IOException _)
            {
            }
        }
        return list;
    }

    public Set<String> supportedFileAttributeViews()
    {
        return attributes;
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
        String regex;
        if (syntaxAndPattern.startsWith("glob:"))
        {
            String pattern = syntaxAndPattern.substring(5);
            if (WINDOWS)
            {
                regex = Globs.toWindowsRegexPattern(pattern);
            }
            else
            {
                regex = Globs.toUnixRegexPattern(pattern);
            }
        }
        else if (syntaxAndPattern.startsWith("regex:"))
        {
            regex = syntaxAndPattern.substring(6);
        }
        else if (syntaxAndPattern.indexOf(':') <= 0)
        {
            throw new IllegalArgumentException();
        }
        else
        {
            throw new UnsupportedOperationException();
        }
        final Pattern pattern = Pattern.compile(regex, WINDOWS ? Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CASE : 0);
        return new PathMatcher() {
            @Override
            public boolean matches(Path path) {
                return pattern.matcher(path.toString()).matches();
            }
        };
    }

    public UserPrincipalLookupService getUserPrincipalLookupService()
    {
        throw new UnsupportedOperationException();
    }

    static final class NetWatchService implements WatchService
    {
        static final WatchEvent<?> overflowEvent = new WatchEvent<Object>() {
            public Object context() {
                return null;
            }
            public int count() {
                return 1;
            }
            public WatchEvent.Kind<Object> kind() {
                return StandardWatchEventKinds.OVERFLOW;
            }
        };
        private static final WatchKey CLOSED = new WatchKey() {
            public boolean isValid() { return false; }
            public List<WatchEvent<?>> pollEvents() { return null; }
            public boolean reset() { return false; }
            public void cancel() { }
            public Watchable watchable() { return null; }
        };
        private boolean closed;
        private final ArrayList<NetWatchKey> keys = new ArrayList<>();
        private final LinkedBlockingQueue<WatchKey> queue = new LinkedBlockingQueue<>();

        public synchronized void close()
        {
            if (!closed)
            {
                closed = true;
                for (NetWatchKey key : keys)
                {
                    key.close();
                }
                enqueue(CLOSED);
            }
        }

        private WatchKey checkClosed(WatchKey key)
        {
            if (key == CLOSED)
            {
                enqueue(CLOSED);
                throw new ClosedWatchServiceException();
            }
            return key;
        }

        public WatchKey poll()
        {
            return checkClosed(queue.poll());
        }

        public WatchKey poll(long timeout, TimeUnit unit) throws InterruptedException
        {
            return checkClosed(queue.poll(timeout, unit));
        }

        public WatchKey take() throws InterruptedException
        {
            return checkClosed(queue.take());
        }

        void enqueue(WatchKey key)
        {
            for (;;)
            {
                try
                {
                    queue.put(key);
                    return;
                }
                catch (InterruptedException _)
                {
                }
            }
        }

        private final class NetWatchKey implements WatchKey
        {
            private final NetPath path;
            private FileSystemWatcher fsw;
            private ArrayList<WatchEvent<?>> list = new ArrayList<>();
            private HashSet<String> modified = new HashSet<>();
            private boolean signaled;
            
            NetWatchKey(NetPath path)
            {
                this.path = path;
            }
            
            synchronized void init(final boolean create, final boolean delete, final boolean modify, final boolean overflow, final boolean subtree)
            {
                if (fsw != null)
                {
                    // we could reuse the FileSystemWatcher, but for now we just recreate it
                    // (and we run the risk of missing some events while we're doing that)
                    fsw.Dispose();
                    fsw = null;
                }
                fsw = new FileSystemWatcher(path.path);
                if (create)
                {
                    fsw.add_Created(new FileSystemEventHandler(new FileSystemEventHandler.Method() {
                        public void Invoke(Object sender, FileSystemEventArgs e) {
                            addEvent(createEvent(e), null);
                        }
                    }));
                }
                if (delete)
                {
                    fsw.add_Deleted(new FileSystemEventHandler(new FileSystemEventHandler.Method() {
                        public void Invoke(Object sender, FileSystemEventArgs e) {
                            addEvent(createEvent(e), null);
                        }
                    }));
                }
                if (modify)
                {
                    fsw.add_Changed(new FileSystemEventHandler(new FileSystemEventHandler.Method() {
                        public void Invoke(Object sender, FileSystemEventArgs e) {
                            synchronized (NetWatchKey.this) {
                                if (modified.contains(e.get_Name())) {
                                    // we already have an ENTRY_MODIFY event pending
                                    return;
                                }
                            }
                            addEvent(createEvent(e), e.get_Name());
                        }
                    }));
                }
                fsw.add_Error(new ErrorEventHandler(new ErrorEventHandler.Method() {
                    public void Invoke(Object sender, ErrorEventArgs e) {
                        if (e.GetException() instanceof cli.System.ComponentModel.Win32Exception
                            && ((cli.System.ComponentModel.Win32Exception)e.GetException()).get_ErrorCode() == -2147467259) {
                            // the directory we were watching was deleted
                            cancelledByError();
                        } else if (overflow) {
                            addEvent(overflowEvent, null);
                        }
                    }
                }));
                if (subtree)
                {
                    fsw.set_IncludeSubdirectories(true);
                }
                fsw.set_EnableRaisingEvents(true);
            }

            WatchEvent<?> createEvent(final FileSystemEventArgs e)
            {
                return new WatchEvent<Path>() {
                    public Path context() {
                        return new NetPath((NetFileSystem)path.getFileSystem(), e.get_Name());
                    }
                    public int count() {
                        return 1;
                    }
                    public WatchEvent.Kind<Path> kind() {
                        switch (e.get_ChangeType().Value) {
                            case WatcherChangeTypes.Created:
                                return StandardWatchEventKinds.ENTRY_CREATE;
                            case WatcherChangeTypes.Deleted:
                                return StandardWatchEventKinds.ENTRY_DELETE;
                            default:
                                return StandardWatchEventKinds.ENTRY_MODIFY;
                        }
                    }
                };
            }

            void cancelledByError()
            {
                cancel();
                synchronized (this)
                {
                    if (!signaled)
                    {
                        signaled = true;
                        enqueue(this);
                    }
                }
            }

            synchronized void addEvent(WatchEvent<?> event, String modified)
            {
                list.add(event);
                if (modified != null)
                {
                    this.modified.add(modified);
                }
                if (!signaled)
                {
                    signaled = true;
                    enqueue(this);
                }
            }

            public synchronized boolean isValid()
            {
                return fsw != null;
            }

            public synchronized List<WatchEvent<?>> pollEvents()
            {
                ArrayList<WatchEvent<?>> r = list;
                list = new ArrayList<>();
                modified.clear();
                return r;
            }

            public synchronized boolean reset()
            {
                if (fsw == null)
                {
                    return false;
                }
                if (signaled)
                {
                    if (list.size() == 0)
                    {
                        signaled = false;
                    }
                    else
                    {
                        enqueue(this);
                    }
                }
                return true;
            }

            void close()
            {
                if (fsw != null)
                {
                    fsw.Dispose();
                    fsw = null;
                }
            }

            public void cancel()
            {
                synchronized (NetWatchService.this)
                {
                    keys.remove(this);
                    close();
                }
            }

            public Watchable watchable()
            {
                return path;
            }
        }

        synchronized WatchKey register(NetPath path, boolean create, boolean delete, boolean modify, boolean overflow, boolean subtree)
        {
            if (closed)
            {
                throw new ClosedWatchServiceException();
            }
            NetWatchKey existing = null;
            for (NetWatchKey key : keys)
            {
                if (key.watchable().equals(path))
                {
                    existing = key;
                    break;
                }
            }
            if (existing == null)
            {
                existing = new NetWatchKey(path);
                keys.add(existing);
            }
            existing.init(create, delete, modify, overflow, subtree);
            return existing;
        }
    }

    public WatchService newWatchService() throws IOException
    {
        return new NetWatchService();
    }
}
