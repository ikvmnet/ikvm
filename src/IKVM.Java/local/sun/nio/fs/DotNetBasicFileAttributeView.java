package sun.nio.fs;

import java.io.*;
import java.nio.file.*;
import java.nio.file.attribute.*;

class DotNetBasicFileAttributeView extends AbstractBasicFileAttributeView {

    static cli.System.DateTime toDateTime(FileTime fileTime) {
        if (fileTime != null) {
            return new cli.System.DateTime(fileTime.to(java.util.concurrent.TimeUnit.MICROSECONDS) * 10 + 621355968000000000L);
        } else {
            return cli.System.DateTime.MinValue;
        }
    }

    protected final String path;

    DotNetBasicFileAttributeView(String path) {
        this.path = path;
    }

    public BasicFileAttributes readAttributes() throws IOException {
        return DotNetDosFileAttributes.read(path);
    }

    public native void setTimes(FileTime lastModifiedTime, FileTime lastAccessTime, FileTime createTime) throws IOException;

}
