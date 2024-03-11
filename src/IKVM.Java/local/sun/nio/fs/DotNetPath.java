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

import com.sun.nio.file.ExtendedWatchEventModifier;
import com.sun.nio.file.SensitivityWatchEventModifier;
import java.io.File;
import java.io.IOException;
import java.net.URI;
import java.nio.file.*;
import java.util.ArrayList;
import java.util.Iterator;

final class DotNetPath extends AbstractPath {

    private static final char[] invalid = cli.System.IO.Path.GetInvalidFileNameChars();
    private final DotNetFileSystem fs;
    final String path;

    DotNetPath(DotNetFileSystem fs, String path) {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            path = WindowsPathParser.parse(path).path();
        } else {
            StringBuilder sb = null;
            int separatorCount = 0;
            boolean prevWasSeparator = false;
            for (int i = 0; i < path.length(); i++)
            {
                char c = path.charAt(i);
                if (c == 0)
                {
                    throw new InvalidPathException(path, "Nul character not allowed");
                }
                else if (c == '/')
                {
                    if (prevWasSeparator)
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
            }
            if (sb != null)
            {
                path = sb.toString();
            }
            if (path.length() > 1 && path.charAt(path.length() - 1) == '/')
            {
                path = path.substring(0, path.length() - 1);
            }
        }
        this.fs = fs;
        this.path = path;
    }

    public FileSystem getFileSystem() {
        return fs;
    }

    public boolean isAbsolute() {
        return cli.System.IO.Path.IsPathRooted(path) && (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows() == false || path.startsWith("\\\\") || (path.length() >= 3 && path.charAt(1) == ':' && path.charAt(2) == '\\'));
    }

    public Path getRoot() {
        int len = getRootLength();
        return len == 0 ? null : new DotNetPath(fs, path.substring(0, len));
    }

    private int getRootLength() {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            if (path.length() >= 2 && path.charAt(1) == ':') {
                if (path.length() >= 3 && path.charAt(2) == '\\') {
                    return 3;
                } else {
                    return 2;
                }
            } else if (path.startsWith("\\\\")) {
                return path.indexOf('\\', path.indexOf('\\', 2) + 1) + 1;
            }
        }

        if (path.length() >= 1 && path.charAt(0) == cli.System.IO.Path.DirectorySeparatorChar) {
            return 1;
        } else {
            return 0;
        }
    }

    public Path getFileName() {
        if (path.length() == 0) {
            return this;
        }
        if (path.length() == getRootLength()) {
            return null;
        }
        String name = cli.System.IO.Path.GetFileName(path);
        if (name == null || name.length() == 0) {
            return null;
        }

        return new DotNetPath(fs, name);
    }

    public Path getParent() {
        if (path.length() == getRootLength()) {
            return null;
        }
        String parent = cli.System.IO.Path.GetDirectoryName(path);
        if (parent == null || parent.length() == 0) {
            return null;
        }

        return new DotNetPath(fs, parent);
    }

    public int getNameCount() {
        int len = getRootLength();
        if (path.length() == len) {
            return len == 0 ? 1 : 0;
        }
        int count = 1;
        for (int i = len; i < path.length(); i++) {
            if (path.charAt(i) == cli.System.IO.Path.DirectorySeparatorChar) {
                count++;
            }
        }

        return count;
    }

    public Path getName(int index) {
        return new DotNetPath(fs, getNameImpl(index));
    }

    private String getNameImpl(int index) {
        for (int pos = getRootLength(); pos < path.length(); index--) {
            int next = path.indexOf(cli.System.IO.Path.DirectorySeparatorChar, pos);
            if (index == 0) {
                return next == -1 ? path.substring(pos) : path.substring(pos, next);
            }
            if (next == -1) {
                break;
            }
            pos = next + 1;
        }

        if (path.length() == 0 && index == 0) {
            return "";
        }

        throw new IllegalArgumentException();
    }

    public Path subpath(int beginIndex, int endIndex) {
        StringBuilder sb = new StringBuilder();
        for (int i = beginIndex; i < endIndex; i++) {
            if (i != beginIndex) {
                sb.append(cli.System.IO.Path.DirectorySeparatorChar);
            }
            sb.append(getNameImpl(i));
        }

        return new DotNetPath(fs, sb.toString());
    }

    public boolean startsWith(Path other) {
        String npath = DotNetPath.from(other).path;
        if (npath.length() == 0) {
            return path.length() == 0;
        }

        return path.regionMatches(cli.IKVM.Runtime.RuntimeUtil.get_IsWindows(), 0, npath, 0, npath.length())
            && (npath.length() == getRootLength()
                || (npath.length() > getRootLength()
                    && (path.length() == npath.length()
                        || (path.length() > npath.length() && path.charAt(npath.length()) == cli.System.IO.Path.DirectorySeparatorChar))));
    }

    public boolean endsWith(Path other) {
        DotNetPath nother = DotNetPath.from(other);
        String npath = nother.path;
        if (npath.length() > path.length()) {
            return false;
        }
        if (npath.length() == 0) {
            return path.length() == 0;
        }
        int nameCount = getNameCount();
        int otherNameCount = nother.getNameCount();
        if (otherNameCount > nameCount) {
            return false;
        }
        int otherRootLength = nother.getRootLength();
        if (otherRootLength > 0) {
            if (otherNameCount != nameCount
                || getRootLength() != otherRootLength
                || !path.regionMatches(cli.IKVM.Runtime.RuntimeUtil.get_IsWindows(), 0, npath, 0, otherRootLength))
            {
                return false;
            }
        }

        int skip = nameCount - otherNameCount;
        for (int i = 0; i < otherNameCount; i++)
        {
            String s1 = getNameImpl(i + skip);
            String s2 = nother.getNameImpl(i);
            if (s1.length() != s2.length() || !s1.regionMatches(cli.IKVM.Runtime.RuntimeUtil.get_IsWindows(), 0, s2, 0, s1.length()))
            {
                return false;
            }
        }

        return true;
    }

    public Path normalize()
    {
        int rootLength = getRootLength();
        ArrayList<String> list = new ArrayList<>();
        for (int i = 0, count = getNameCount(); i < count; i++)
        {
            String s = getNameImpl(i);
            if (s.equals(".."))
            {
                if (list.size() == 0)
                {
                    if (rootLength == 0 || (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows() && rootLength == 2))
                    {
                        list.add("..");
                    }
                }
                else if (list.get(list.size() - 1).equals(".."))
                {
                    list.add("..");
                }
                else
                {
                    list.remove(list.size() - 1);
                }
            }
            else if (!s.equals("."))
            {
                list.add(s);
            }
        }
        StringBuilder sb = new StringBuilder();
        sb.append(path.substring(0, getRootLength()));
        for (int i = 0; i < list.size(); i++)
        {
            if (i != 0)
            {
                sb.append(cli.System.IO.Path.DirectorySeparatorChar);
            }
            sb.append(list.get(i));
        }
        return new DotNetPath(fs, sb.toString());
    }

    public Path resolve(Path other)
    {
        DotNetPath nother = DotNetPath.from(other);
        String npath = nother.path;
        if (nother.isAbsolute())
        {
            return other;
        }
        if (npath.length() == 0)
        {
            return this;
        }
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows())
        {
            if (nother.getRootLength() == 2 && getRootLength() == 3 && (path.charAt(0) | 0x20) == (npath.charAt(0) | 0x20))
            {
                // we're in the case where we have a root "x:\" and other "x:", so we have to chop off "x:" from other because
                // otherwise Path.Combine will just return other
                npath = npath.substring(2);
            }
            else if (nother.getRootLength() == 1 && getRootLength() > 3)
            {
                // we're in the case where we have a root "\\host\share\" and other "\",
                // we have to manually handle this because Path.Combine doesn't do the right thing
                return new DotNetPath(fs, path.substring(0, getRootLength()) + npath);
            }
        }
        return new DotNetPath(fs, cli.System.IO.Path.Combine(path, npath));
    }

    public Path relativize(Path other)
    {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            DotNetPath nother = DotNetPath.from(other);
            if (equals(nother))
            {
                return new DotNetPath(fs, "");
            }
            int rootLength = getRootLength();
            if (nother.getRootLength() != rootLength || !path.regionMatches(true, 0, nother.path, 0, rootLength))
            {
                throw new IllegalArgumentException("'other' has different root");
            }
            int nameCount = getNameCount();
            int otherNameCount = nother.getNameCount();
            int count = Math.min(nameCount, otherNameCount);
            int i = 0;
            // skip the common parts
            for (; i < count && getNameImpl(i).equals(nother.getNameImpl(i)); i++)
            {
            }
            // remove the unused parts of our path
            StringBuilder sb = new StringBuilder();
            for (int j = i; j < nameCount; j++)
            {
                sb.append("..\\");
            }
            // append the new parts of other
            for (int j = i; j < otherNameCount; j++)
            {
                if (j != i)
                {
                    sb.append("\\");
                }
                sb.append(nother.getNameImpl(j));
            }
            return new DotNetPath(fs, sb.toString());
        } else {
            DotNetPath nother = DotNetPath.from(other);
            if (other.equals(this))
                return emptyPath();

            // can only relativize paths of the same type
            if (this.isAbsolute() != other.isAbsolute())
                throw new IllegalArgumentException("'other' is different type of Path");

            // this path is the empty path
            if (this.isEmpty())
                return nother;

            int bn = this.getNameCount();
            int cn = other.getNameCount();

            // skip matching names
            int n = (bn > cn) ? cn : bn;
            int i = 0;
            while (i < n) {
                if (!this.getName(i).equals(nother.getName(i)))
                    break;
                i++;
            }

            int dotdots = bn - i;
            if (i < cn) {
                // remaining name components in other
                DotNetPath remainder = DotNetPath.from(nother.subpath(i, cn));
                if (dotdots == 0)
                    return remainder;

                // other is the empty path
                boolean isOtherEmpty = nother.isEmpty();

                // result is a  "../" for each remaining name in base
                // followed by the remaining names in other. If the remainder is
                // the empty path then we don't add the final trailing slash.
                int len = dotdots*3 + remainder.path.length();
                if (isOtherEmpty) {
                    assert remainder.isEmpty();
                    len--;
                }
                char[] result = new char[len];
                int pos = 0;
                while (dotdots > 0) {
                    result[pos++] = '.';
                    result[pos++] = '.';
                    if (isOtherEmpty) {
                        if (dotdots > 1) result[pos++] = '/';
                    } else {
                        result[pos++] = '/';
                    }
                    dotdots--;
                }
                System.arraycopy(remainder.path.toCharArray(), 0, result, pos, remainder.path.length());
                return new DotNetPath(fs, new String(result));
            } else {
                // no remaining names in other so result is simply a sequence of ".."
                char[] result = new char[dotdots*3 - 1];
                int pos = 0;
                while (dotdots > 0) {
                    result[pos++] = '.';
                    result[pos++] = '.';
                    // no tailing slash at the end
                    if (dotdots > 1)
                        result[pos++] = '/';
                    dotdots--;
                }
                return new DotNetPath(fs, new String(result));
            }
        }
    }

    private boolean isEmpty() {
        return path.length() == 0;
    }

    private DotNetPath emptyPath() {
        return new DotNetPath(fs, "");
    }

    public URI toUri() {
        if (cli.IKVM.Runtime.RuntimeUtil.get_IsWindows()) {
            return DotNetWindowsUriSupport.toUri(this);
        } else {
            return DotNetUnixUriUtils.toUri(this);
        }
    }

    public DotNetPath toAbsolutePath() {
        if (isAbsolute()) {
            return this;
        }

        // System.getProperty("user.dir") will trigger the specified security check
        return new DotNetPath(fs, cli.System.IO.Path.GetFullPath(cli.System.IO.Path.Combine(System.getProperty("user.dir"), path)));
    }

    public Path toRealPath(LinkOption... options) throws IOException {
        SecurityManager sm = System.getSecurityManager();
        if (sm != null) {
            sm.checkRead(path);
            if (!isAbsolute()) {
                sm.checkPropertyAccess("user.dir");
            }
        }

        return new DotNetPath(fs, toRealPathImpl(path));
    }
    
    private static native String toRealPathImpl(String path);

    public WatchKey register(WatchService watcher, WatchEvent.Kind<?>[] events, WatchEvent.Modifier... modifiers) throws IOException {
        if (watcher instanceof DotNetFileSystem.NetWatchService) {
            boolean create = false;
            boolean delete = false;
            boolean modify = false;
            boolean overflow = false;
            boolean subtree = false;

            for (WatchEvent.Kind<?> kind : events) {
                if (kind == StandardWatchEventKinds.ENTRY_CREATE) {
                    create = true;
                } else if (kind == StandardWatchEventKinds.ENTRY_DELETE) {
                    delete = true;
                } else if (kind == StandardWatchEventKinds.ENTRY_MODIFY) {
                    modify = true;
                } else if (kind == StandardWatchEventKinds.OVERFLOW) {
                    overflow = true;
                } else {
                    // null check
                    kind.getClass();
                    throw new UnsupportedOperationException();
                }
            }

            if (!create && !delete && !modify) {
                throw new IllegalArgumentException();
            }

            for (WatchEvent.Modifier modifier : modifiers) {
                if (modifier == ExtendedWatchEventModifier.FILE_TREE) {
                    subtree = true;
                } else if (modifier instanceof SensitivityWatchEventModifier) {
                    // ignore
                } else {
                    // null check
                    modifier.getClass();
                    throw new UnsupportedOperationException();
                }
            }

            SecurityManager sm = System.getSecurityManager();
            if (sm != null) {
                sm.checkRead(path);
                if (subtree) {
                    sm.checkRead(path + cli.System.IO.Path.DirectorySeparatorChar + '-');
                }
            }

            return ((DotNetFileSystem.NetWatchService)watcher).register(this, create, delete, modify, overflow, subtree);
        } else if (watcher instanceof PollingWatchService) {
            boolean subtree = false;

            for (WatchEvent.Modifier modifier : modifiers) {
                if (modifier == ExtendedWatchEventModifier.FILE_TREE) {
                    subtree = true;
                } else if (modifier instanceof SensitivityWatchEventModifier) {
                    // ignore
                } else {
                    // null check
                    modifier.getClass();
                    throw new UnsupportedOperationException();
                }
            }

            SecurityManager sm = System.getSecurityManager();
            if (sm != null) {
                sm.checkRead(path);
                if (subtree) {
                    sm.checkRead(path + cli.System.IO.Path.DirectorySeparatorChar + '-');
                }
            }

            return ((PollingWatchService)watcher).register(this, events, modifiers);
        } else {
            watcher.getClass(); // null check
            throw new ProviderMismatchException();
        }
    }

    public int compareTo(Path other) {
        String path2 = ((DotNetPath)other).path;
        int len1 = path.length();
        int len2 = path2.length();
        int min = Math.min(len1, len2);
        for (int i = 0; i < min; i++) {
            char c1 = path.charAt(i);
            char c2 = path2.charAt(i);
            if (c1 != c2 && Character.toUpperCase(c1) != Character.toUpperCase(c2)) {
                return c1 - c2;
            }
        }

        return len1 - len2;
    }

    public boolean equals(Object other) {
        if (!(other instanceof DotNetPath)) {
            return false;
        }

        return compareTo((DotNetPath)other) == 0;
    }

    public int hashCode() {
        int hash = 0;
        for (int i = 0; i < path.length(); i++) {
            hash = 97 * hash + Character.toUpperCase(path.charAt(i));
        }

        return hash;
    }

    public String toString() {
        return path;
    }

    boolean isUnc() {
        return cli.IKVM.Runtime.RuntimeUtil.get_IsWindows() && getRootLength() > 3;
    }

    static DotNetPath from(Path path) {
        if (!(path instanceof DotNetPath)) {
            // null check
            path.getClass();
            throw new ProviderMismatchException();
        }
        
        return (DotNetPath)path;
    }

}