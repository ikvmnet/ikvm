package sun.nio.fs;

import java.io.IOException;
import java.nio.file.attribute.*;
import java.util.*;
import java.util.concurrent.TimeUnit;

import static sun.nio.fs.WindowsConstants.*;

final class WindowsFileAttributes implements DosFileAttributes {

    // used to adjust values between Windows and java epoch
    private static final long WINDOWS_EPOCH_IN_MICROSECONDS = -11644473600000000L;

    /**
     * Convert 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601 to a FileTime.
     */
    static FileTime toFileTime(long time) {
        // 100ns -> us
        time /= 10L;
        // adjust to java epoch
        time += WINDOWS_EPOCH_IN_MICROSECONDS;
        return FileTime.from(time, TimeUnit.MICROSECONDS);
    }

    /**
     * Convert FileTime to 64-bit value representing the number of 100-nanosecond intervals since January 1, 1601.
     */
    static long toWindowsTime(FileTime time) {
        long value = time.to(TimeUnit.MICROSECONDS);
        // adjust to Windows epoch+= 11644473600000000L;
        value -= WINDOWS_EPOCH_IN_MICROSECONDS;
        // us -> 100ns
        value *= 10L;
        return value;
    }

    private final int fileAttrs;
    private final long creationTime;
    private final long lastAccessTime;
    private final long lastWriteTime;
    private final long size;
    private final int reparseTag;

    private final int volSerialNumber;
    private final int fileIndexHigh;
    private final int fileIndexLow;

    public WindowsFileAttributes(DotNetPath path) {
        this.path = path;
    }

    boolean isReparsePoint() {
        return isReparsePoint(fileAttrs);
    }

    boolean isDirectoryLink() {
        return isSymbolicLink() && ((fileAttrs & FILE_ATTRIBUTE_DIRECTORY) != 0);
    }
    
    @Override
    public boolean isSymbolicLink() {
        return reparseTag == IO_REPARSE_TAG_SYMLINK;
    }

    @Override
    public boolean isDirectory() {
        // ignore FILE_ATTRIBUTE_DIRECTORY attribute if file is a sym link
        if (isSymbolicLink())
            return false;

        return ((fileAttrs & FILE_ATTRIBUTE_DIRECTORY) != 0);
    }

    @Override
    public boolean isOther() {
        if (isSymbolicLink())
            return false;
        else
            return ((fileAttrs & (FILE_ATTRIBUTE_DEVICE | FILE_ATTRIBUTE_REPARSE_POINT)) != 0);
    }

    @Override
    public boolean isRegularFile() {
        return !isSymbolicLink() && !isDirectory() && !isOther();
    }

    @Override
    public boolean isReadOnly() {
        return (fileAttrs & FILE_ATTRIBUTE_READONLY) != 0;
    }

    @Override
    public boolean isHidden() {
        return (fileAttrs & FILE_ATTRIBUTE_HIDDEN) != 0;
    }

    @Override
    public boolean isArchive() {
        return (fileAttrs & FILE_ATTRIBUTE_ARCHIVE) != 0;
    }

    @Override
    public boolean isSystem() {
        return (fileAttrs & FILE_ATTRIBUTE_SYSTEM) != 0;
    }

}
