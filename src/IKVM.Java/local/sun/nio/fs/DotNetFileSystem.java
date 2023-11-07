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

final class DotNetFileSystem extends FileSystem {

    private static final Set<String> attributes = Collections.unmodifiableSet(new HashSet<String>(Arrays.asList("basic")));
    private final DotNetFileSystemProvider provider;
    private final String separator = Character.toString(cli.System.IO.Path.DirectorySeparatorChar);

    DotNetFileSystem(DotNetFileSystemProvider provider) {
        this.provider = provider;
    }

    public FileSystemProvider provider() {
        return provider;
    }

    public void close() throws IOException {
        throw new UnsupportedOperationException();
    }

    public boolean isOpen() {
        return true;
    }

    public boolean isReadOnly() {
        return false;
    }

    public String getSeparator() {
        return separator;
    }

    public Iterable<Path> getRootDirectories() {
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
            return new DotNetPath(this, first);
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

            return new DotNetPath(this, sb.toString());
        }
    }

    public PathMatcher getPathMatcher(String syntaxAndPattern)
    {
        String regex;
        if (syntaxAndPattern.startsWith("glob:"))
        {
            String pattern = syntaxAndPattern.substring(5);
            if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows())
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
        final Pattern pattern = Pattern.compile(regex, cli.IKVM.Runtime.RuntimeUtil.get_IsWindows() ? Pattern.CASE_INSENSITIVE | Pattern.UNICODE_CASE : 0);
        return new PathMatcher() {
            @Override
            public boolean matches(Path path) {
                return pattern.matcher(path.toString()).matches();
            }
        };
    }

    public UserPrincipalLookupService getUserPrincipalLookupService() {
        throw new UnsupportedOperationException();
    }

    public WatchService newWatchService() throws IOException {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            return new DotNetWatchService(this);
        } else {
            // FileSystemWatcher implementation on .NET for Unix consumes way too many inotify queues
            return new PollingWatchService();
        }
    }

}
