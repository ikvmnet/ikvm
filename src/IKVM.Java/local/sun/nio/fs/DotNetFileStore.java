package sun.nio.fs;

import java.io.IOException;
import java.nio.file.*;
import java.nio.file.attribute.*;
import java.util.*;

import cli.System.IO.DriveInfo;

final class DotNetFileStore extends FileStore {

    private final DriveInfo info;

    DotNetFileStore(DriveInfo info) throws IOException {
        this.info = info;
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

    public native long getTotalSpace() throws IOException;

    public native long getUnallocatedSpace() throws IOException;

    public native long getUsableSpace() throws IOException;

    public boolean isReadOnly() {
        return false;
    }

    public String name() {
        return info.get_VolumeLabel();
    }

    public boolean supportsFileAttributeView(Class<? extends FileAttributeView> type) {
        return type == BasicFileAttributeView.class || type == DosFileAttributeView.class || type == PosixFileAttributeView;
    }

    public boolean supportsFileAttributeView(String name) {
        final String name = info.get_VolumeLabel();
        return name.equals("basic") || name.equals("dos") || name.equals("posix") || name.equals("unix");
    }

    public String type() {
        return info.get_DriveFormat();
    }        

    public String toString() {
        final String name = info.get_VolumeLabel();
        return name + " (" + info.get_Name().charAt(0) + ":)";
    }

}
